using System.Collections.Generic;
using System.Linq;
using nbaData.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
            if(!forceRun && _players != null)
            {
                return _players;
            }
            
            List<Player> players = new List<Player>();

            RestClient client = new RestClient("https://www.balldontlie.io/api/v1/players?page=1&per_page=100")
                {Timeout = -1};
            client.UseNewtonsoftJson();
            RestRequest request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);

            BallDontLiePlayerResponse result =
                JsonConvert.DeserializeObject<BallDontLiePlayerResponse>(response.Content);
            players.AddRange(result.data);

            for (int x = 2; x <= result.meta.total_pages; x++)
            {
                RestClient loopClient =
                    new RestClient($"https://www.balldontlie.io/api/v1/players?page={x}&per_page=100") {Timeout = -1};
                RestRequest loopRequest = new RestRequest(Method.GET);
                IRestResponse loopResponse = loopClient.Execute(loopRequest);
                BallDontLiePlayerResponse loopResult =
                    JsonConvert.DeserializeObject<BallDontLiePlayerResponse>(loopResponse.Content);

                players.AddRange(loopResult.data);
            }

            _players = players;

            return players;
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

        public SeasonStats GetShootingStats(int id)
        {
            List<SeasonStats> stats = new List<SeasonStats>();

            RestClient client =
                new RestClient(
                        $"https://www.balldontlie.io/api/v1/season_averages?season=2020&player_ids[]={id}")
                    {Timeout = -1};
            client.UseNewtonsoftJson();
            RestRequest request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);

            BallDontLieSeasonStatsResponse result =
                JsonConvert.DeserializeObject<BallDontLieSeasonStatsResponse>(response.Content);
            stats.AddRange(result.data);

            return stats[0];
        }
        
        public GameStats GetAverageGameStats(int id, int numberOfRecentGames)
        {
            List<GameStats> stats = new List<GameStats>();

            RestClient client =
                new RestClient(
                        $"https://www.balldontlie.io/api/v1/stats?seasons[]=2020&player_ids[]={id}&per_page=100")
                    {Timeout = -1};
            client.UseNewtonsoftJson();
            RestRequest request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);

            BallDontLieGameStatsResponse result =
                JsonConvert.DeserializeObject<BallDontLieGameStatsResponse>(response.Content);
            //TODO: Sort through most recent numberOfRecentGames by game ID or date
            stats.AddRange(result.data);

            return stats[0];
        }
    }

    public interface IBallDontLieManager
    {
        IEnumerable<Player> GetPlayers(bool forceRun = true);

        IEnumerable<Player> GetPlayersByTeam(string teamName);

        SeasonStats GetShootingStats(int id);

        GameStats GetAverageGameStats(int id, int numberOfRecentGames);
    }
}