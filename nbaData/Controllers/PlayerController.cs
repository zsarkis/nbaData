using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using nbaData.Models;

namespace nbaData.Controllers
{
    [ApiController]
    [Route("api/v1/player")]
    [EnableCors("CorsPolicy")]
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
            
            if (player == null || player.id == 0)
            {
                return NotFound();
            }

            return Ok(player);
        }
        
        [HttpGet("{playerId}/stats/seasonAverage")]
        public ActionResult<SeasonStats> GetSeasonStats(int playerId)
        {
            Player player = _manager.GetPlayer(playerId);

            //TODO: Figure out how to deal with players that were not active for this season
            if (player == null || player.id == 0)
            {
                return NotFound();
            }

            SeasonStats stats = _manager.GetShootingStats(playerId);
            
            return stats != null ? Ok(stats) : NotFound();
        }
        
        [HttpGet("{playerId}/stats/gameAverages")]
        public ActionResult<ShootingGameStats> GetAverageGameStats(int playerId, int numberOfRecentGames)
        {
            ShootingGameStats stats = _manager.GetAverageGameStats(playerId, numberOfRecentGames);

            if (stats.fg3a == 0 && stats.fg2a == 0)
            {
                return NotFound();
            }
            
            return stats != null ? Ok(stats) : NotFound();
        }
    }
}