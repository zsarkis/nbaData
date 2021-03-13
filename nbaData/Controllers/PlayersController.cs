using System.Collections.Generic;
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
        public IEnumerable<Player> GetPlayers()
        {
            IEnumerable<Player> players = _manager.GetPlayers();

            return players;
        }
    }
}