using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using nbaData.Models;

namespace nbaData.Controllers
{
    [ApiController]
    [Route("api/v1/player")]
    public class PlayerController : ControllerBase
    {
        private readonly IBallDontLieManager _manager;

        public PlayerController(IBallDontLieManager manager)
        {
            _manager = manager;
        }
        
        [HttpGet("{playerId}")]
        public ActionResult<Player> GetPlayer(int playerId)
        {
            Player player = _manager.GetPlayer(playerId);
            
            return player != null ? Ok(player) : NotFound();
        }
        
        [HttpGet("{playerId}/stats/seasonAverage")]
        public ActionResult<SeasonStats> GetSeasonStats(int playerId)
        {
            IEnumerable<Player> players = _manager.GetPlayers(false);
            
            SeasonStats stats = _manager.GetShootingStats(playerId);
            
            return stats != null ? Ok(stats) : NotFound();
        }
        
        [HttpGet("{playerId}/stats/gameAverages")]
        public ActionResult<ShootingGameStats> GetAverageGameStats(int playerId, int numberOfRecentGames)
        {
            ShootingGameStats stats = _manager.GetAverageGameStats(playerId, numberOfRecentGames);
            
            return stats != null ? Ok(stats) : NotFound();
        }
    }
}