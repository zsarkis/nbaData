using System.Collections.Generic;
using System.Linq;
using System.Threading;
using nbaData.Models;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;

namespace nbaData
{
    //TODO: make these methods try
    public class BallDontLieManager : IBallDontLieManager
    {
        public IEnumerable<Player> _players = null;

        public IEnumerable<Player> GetPlayers(bool forceRun = true)
        {
            if (!forceRun && _players != null)
            {
                return _players;
            }

            List<Player> players = new List<Player>();

            RestClient client = new RestClient("https://www.balldontlie.io/api/v1/players?page=1&per_page=100")
                { Timeout = -1 };
            client.UseNewtonsoftJson();
            RestRequest request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);

            BallDontLiePlayerResponse result =
                JsonConvert.DeserializeObject<BallDontLiePlayerResponse>(response.Content);
            players.AddRange(result.data);

            for (int x = 2; x <= result.meta.total_pages; x++)
            {
                RestClient loopClient =
                    new($"https://www.balldontlie.io/api/v1/players?page={x}&per_page=100") { Timeout = -1 };
                RestRequest loopRequest = new RestRequest(Method.GET);
                IRestResponse loopResponse = loopClient.Execute(loopRequest);
                BallDontLiePlayerResponse loopResult =
                    JsonConvert.DeserializeObject<BallDontLiePlayerResponse>(loopResponse.Content);

                players.AddRange(loopResult.data);
            }

            players = FilterPlayers(players);

            _players = players;

            return players;
        }

        public Player GetPlayer(int id)
        {
            RestClient client =
                new RestClient(
                        $"https://www.balldontlie.io/api/v1/players/{id}")
                    { Timeout = -1 };
            client.UseNewtonsoftJson();
            RestRequest request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);

            Player player = JsonConvert.DeserializeObject<Player>(response.Content);

            return player;
        }

        public IEnumerable<Player> GetPlayersByTeam(string teamName)
        {
            List<Player> players;

            players = _players == null ? GetPlayers().ToList() : _players.ToList();

            List<Player> playersForTeam =
                players.Where(player => player.team != null)
                    .Where(player => player.team.full_name.ToLower().Contains(teamName.ToLower())
                                     || player.team.abbreviation.ToLower().Contains(teamName.ToLower())
                                     || player.team.city.ToLower().Contains(teamName.ToLower())
                                     || player.team.name.ToLower().Contains(teamName.ToLower())).ToList();

            return playersForTeam.OrderBy(p => p.last_name);
        }

        public SeasonStats GetShootingStats(int id, int season = 2021)
        {
            List<SeasonStats> stats = new List<SeasonStats>();

            RestClient client =
                new RestClient(
                        $"https://www.balldontlie.io/api/v1/season_averages?season={season}&player_ids[]={id}")
                    { Timeout = -1 };
            client.UseNewtonsoftJson();
            RestRequest request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);

            BallDontLieSeasonStatsResponse result =
                JsonConvert.DeserializeObject<BallDontLieSeasonStatsResponse>(response.Content);
            stats.AddRange(result.data);

            if (stats.Count == 0)
            {
                return null;
            }

            return stats[0];
        }

        public ShootingGameStats GetAverageGameStats(int[] ids, int numberOfRecentGames)
        {
            List<ShootingGameStats> compiledStats = new List<ShootingGameStats>();

            foreach (int id in ids)
            {
                List<GameStats> allStats = new List<GameStats>();

                RestClient client =
                    new RestClient(
                            $"https://www.balldontlie.io/api/v1/stats?seasons[]=2021&player_ids[]={id}&per_page=100")
                        { Timeout = -1 };
                client.UseNewtonsoftJson();
                RestRequest request = new RestRequest(Method.GET);
                IRestResponse response = client.Execute(request);

                BallDontLieGameStatsResponse result =
                    JsonConvert.DeserializeObject<BallDontLieGameStatsResponse>(response.Content);
                allStats.AddRange(result.data);

                List<GameStats> stats = new List<GameStats>();
                List<GameStats> statsEnumerable = allStats.OrderByDescending(stat => stat.id).ToList();
                //TODO: verify the game is not active
                for (int i = 0; i < numberOfRecentGames; i++)
                {
                    stats.Add(statsEnumerable[i]);
                }

                compiledStats.Add(CalculateAverageShootingOverRange(stats));
            }

            return CalculateAverageShootingOverRoster(compiledStats);
        }

        public ShootingGameStats GetAverageGameStats(int id, int numberOfRecentGames)
        {
            List<GameStats> allStats = new List<GameStats>();

            RestClient client =
                new RestClient(
                        $"https://www.balldontlie.io/api/v1/stats?seasons[]=2021&player_ids[]={id}&per_page=100")
                    { Timeout = -1 };
            client.UseNewtonsoftJson();
            RestRequest request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);

            BallDontLieGameStatsResponse result =
                JsonConvert.DeserializeObject<BallDontLieGameStatsResponse>(response.Content);
            allStats.AddRange(result.data);

            List<GameStats> stats = new List<GameStats>();
            List<GameStats> statsEnumerable = allStats.OrderByDescending(stat => stat.id).ToList();
            //TODO: verify the game is not active
            for (int i = 0; i < numberOfRecentGames; i++)
            {
                stats.Add(statsEnumerable[i]);
            }

            ShootingGameStats finalStats = CalculateAverageShootingOverRange(stats);

            return finalStats;
        }

        private List<Player> FilterPlayers(List<Player> players)
        {
            //Find last game played
            //if over 2-3 years ago, boot it
            List<Player> activePlayers = new();
            foreach (Player player in players)
            {
                if (GetShootingStats(player.id) != null) activePlayers.Add(player);

                Thread.Sleep(1000);
            }

            return activePlayers;
        }

        protected ShootingGameStats CalculateAverageShootingOverRange(List<GameStats> gameStatsList)
        {
            double threePointAttemptAverage = gameStatsList.Select(x => x.fg3a).DefaultIfEmpty(0).Average();
            double twoPointAttemptAverage = gameStatsList.Select(x => x.fga - x.fg3a).DefaultIfEmpty(0).Average();
            double threePointMadeAverage = gameStatsList.Select(x => x.fg3m).DefaultIfEmpty(0).Average();
            double twoPointMadeAverage = gameStatsList.Select(x => x.fgm - x.fg3m).DefaultIfEmpty(0).Average();

            double twoPointAttempts = twoPointAttemptAverage * gameStatsList.Count;
            double twoPointMakes = twoPointMadeAverage * gameStatsList.Count;

            double threePointAttempts = threePointAttemptAverage * gameStatsList.Count;
            double threePointMakes = threePointMadeAverage * gameStatsList.Count;

            ShootingGameStats gameStats = new ShootingGameStats();
            gameStats.fg3m = threePointMadeAverage;
            gameStats.fg3a = threePointAttemptAverage;
            gameStats.fg3_pct = threePointMakes / threePointAttempts;
            gameStats.fg2a = twoPointAttemptAverage;
            gameStats.fg2m = twoPointMadeAverage;
            gameStats.fg2_pct = twoPointMakes / twoPointAttempts;

            return gameStats;
        }

        protected ShootingGameStats CalculateAverageShootingOverRoster(List<ShootingGameStats> shootingGameStatsList)
        {
            double threePointAttemptSum = shootingGameStatsList.Select(x => x.fg3a).DefaultIfEmpty(0).Sum();
            double twoPointAttemptSum = shootingGameStatsList.Select(x => x.fg2a).DefaultIfEmpty(0).Sum();
            double threePointMadeSum = shootingGameStatsList.Select(x => x.fg3m).DefaultIfEmpty(0).Sum();
            double twoPointMadeSum = shootingGameStatsList.Select(x => x.fg2m).DefaultIfEmpty(0).Sum();

            double twoPointAttempts = twoPointAttemptSum * shootingGameStatsList.Count;
            double twoPointMakes = twoPointMadeSum * shootingGameStatsList.Count;

            double threePointAttempts = threePointAttemptSum * shootingGameStatsList.Count;
            double threePointMakes = threePointMadeSum * shootingGameStatsList.Count;

            ShootingGameStats gameStats = new ShootingGameStats();
            gameStats.fg3m = threePointMadeSum;
            gameStats.fg3a = threePointAttemptSum;
            gameStats.fg3_pct = threePointMakes / threePointAttempts;
            gameStats.fg2a = twoPointAttemptSum;
            gameStats.fg2m = twoPointMadeSum;
            gameStats.fg2_pct = twoPointMakes / twoPointAttempts;

            return gameStats;
        }
    }

    public interface IBallDontLieManager
    {
        IEnumerable<Player> GetPlayers(bool forceRun = true);

        IEnumerable<Player> GetPlayersByTeam(string teamName);

        Player GetPlayer(int id);

        SeasonStats GetShootingStats(int id, int season = 2021);

        ShootingGameStats GetAverageGameStats(int id, int numberOfRecentGames);

        ShootingGameStats GetAverageGameStats(int[] ids, int numberOfRecentGames);
    }
}