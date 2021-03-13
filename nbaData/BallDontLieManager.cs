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
            List<Player> players = new();

            RestClient client = new("https://www.balldontlie.io/api/v1/players?page=0&per_page=100") {Timeout = -1};
            client.UseNewtonsoftJson();
            RestRequest request = new(Method.GET);
            IRestResponse response = client.Execute(request);

            BallDontLieResponse result = JsonConvert.DeserializeObject<BallDontLieResponse>(response.Content);
            players.AddRange(result.data);

            for (var x = 2; x <= result.meta.total_pages; x++)
            {
                RestRequest loopRequest = new($"https://www.balldontlie.io/api/v1/players?page={x}&per_page=100");
                IRestResponse loopResponse = client.Execute(loopRequest);
                BallDontLieResponse loopResult =
                    JsonConvert.DeserializeObject<BallDontLieResponse>(loopResponse.Content);

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