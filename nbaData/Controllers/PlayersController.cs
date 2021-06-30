using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using nbaData.Models;

namespace nbaData.Controllers
{
    [ApiController]
    [Route("api/v1/players")]
    public class PlayersController : ControllerBase
    {
        private readonly IBallDontLieManager _manager;

        public PlayersController(IBallDontLieManager manager)
        {
            _manager = manager;
        }

        [HttpGet]
        public IEnumerable<Player> GetPlayers(string teamName = null)
        {
            IEnumerable<Player> players;

            if (string.IsNullOrEmpty(teamName))
            {
                players = _manager.GetPlayers();
            }
            else
            {
                players = _manager.GetPlayersByTeam(teamName);
                //
                // players = allPlayers.Where(p =>
                // {
                //     return p.team.full_name.ToLower().Contains(teamName.ToLower()) ||
                //            p.team.abbreviation.ToLower().Contains(teamName.ToLower()) ||
                //            p.team.city.ToLower().Contains(teamName.ToLower()) ||
                //            p.team.name.ToLower().Contains(teamName.ToLower());
                // });
            }

            return players;
        }

        [HttpGet]
        [Route("stats")]
        public SeasonStats GetSeasonStats(string player)
        {
            IEnumerable<Player> players = _manager.GetPlayers();

            Player athlete = players.First(p => p.first_name == player.Split()[0] && p.last_name == player.Split()[1]);
            
            SeasonStats stats = _manager.GetShootingStats(athlete);

            return stats;
        }
    }
}