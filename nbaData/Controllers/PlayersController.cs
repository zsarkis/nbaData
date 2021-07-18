using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using nbaData.Models;

namespace nbaData.Controllers
{
    [ApiController]
    [Route("api/v1/players")]
    [EnableCors("CorsPolicy")]
    public class PlayersController : ControllerBase
    {
        private readonly IBallDontLieManager _manager;

        public PlayersController(IBallDontLieManager manager)
        {
            _manager = manager;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Player>> GetPlayers(string teamName = null)
        {
            IEnumerable<Player> players;

            if (string.IsNullOrEmpty(teamName))
            {
                players = _manager.GetPlayers();
            }
            else
            {
                players = _manager.GetPlayersByTeam(teamName);
            }

            return players.Count() > 0 ? Ok(players.ToList()) : NotFound();
        }
    }
}