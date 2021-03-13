using System.Collections.Generic;
using nbaData.Models;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;

namespace nbaData
{
    public class BallDontLieManager : IBallDontLieManager
    {
        public IEnumerable<Player> GetPlayers()
        {
            var players = new List<Player>();

            var client = new RestClient("https://www.balldontlie.io/api/v1/players?page=0&per_page=100") {Timeout = -1};
            client.UseNewtonsoftJson();
            var request = new RestRequest(Method.GET);
            var response = client.Execute(request);

            var result = JsonConvert.DeserializeObject<BallDontLieResponse>(response.Content);
            players.AddRange(result.data);

            for (var x = 2; x <= result.meta.total_pages; x++)
            {
                var loopRequest = new RestRequest($"https://www.balldontlie.io/api/v1/players?page={x}&per_page=100");
                var loopResponse = client.Execute(loopRequest);
                var loopResult = JsonConvert.DeserializeObject<BallDontLieResponse>(loopResponse.Content);

                players.AddRange(loopResult.data);
            }

            return players;
        }
    }

    public interface IBallDontLieManager
    {
        IEnumerable<Player> GetPlayers();
    }
}