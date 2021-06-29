using System.Collections.Generic;
using System.Linq;
using nbaData.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;

namespace nbaData
{
    public class BallDontLieManager : IBallDontLieManager
    {
        public IEnumerable<Player> GetPlayers()
        {
            List<Player> players = new List<Player>();
            
            RestClient client = new RestClient("https://www.balldontlie.io/api/v1/players?page=1&per_page=100") {Timeout = -1};
            client.UseNewtonsoftJson();
            RestRequest request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);

            BallDontLiePlayerResponse result = JsonConvert.DeserializeObject<BallDontLiePlayerResponse>(response.Content);
            players.AddRange(result.data);

            for (int x = 2; x <= result.meta.total_pages; x++)
            {
                RestClient loopClient = new RestClient($"https://www.balldontlie.io/api/v1/players?page={x}&per_page=100") {Timeout = -1};
                RestRequest loopRequest = new RestRequest(Method.GET);
                IRestResponse loopResponse = loopClient.Execute(loopRequest);
                BallDontLiePlayerResponse loopResult = JsonConvert.DeserializeObject<BallDontLiePlayerResponse>(loopResponse.Content);
                
                players.AddRange(loopResult.data);
            }
            
            return players;
        }
        
        public IEnumerable<Player> GetPlayersByTeam(string teamName)
        {
            List<Player> players = GetPlayers().ToList();

            List<Player> playersForTeam = new List<Player>();
            
            foreach (Player player in players)
            {
                if (player.team != null)
                {
                    if (player.team.full_name.ToLower().Contains(teamName.ToLower()) ||
                        player.team.abbreviation.ToLower().Contains(teamName.ToLower()) ||
                        player.team.city.ToLower().Contains(teamName.ToLower()) ||
                        player.team.name.ToLower().Contains(teamName.ToLower()))
                    {
                        playersForTeam.Add(player);
                    }
                }
            }
            
            return playersForTeam.OrderBy( p => p.last_name);
        }
        
        //TODO: Add short, mid, long term???
        public ShootingStats GetShootingStats(Player player)
        {
            int id = player.id;
            List<ShootingStats> stats = new List<ShootingStats>();
            
            RestClient client = new RestClient($"https://www.balldontlie.io/api/v1/season_averages?season=2020&player_ids[]={player.id}") {Timeout = -1};
            client.UseNewtonsoftJson();
            RestRequest request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);

            BallDontLieSeasonStatsResponse result = JsonConvert.DeserializeObject<BallDontLieSeasonStatsResponse>(response.Content);
            stats.AddRange(result.data);

            return stats[0];
        }
    }

    public interface IBallDontLieManager
    {
        IEnumerable<Player> GetPlayers();
        
        IEnumerable<Player> GetPlayersByTeam(string teamName);

        ShootingStats GetShootingStats(Player player);
    }
}