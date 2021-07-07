using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Moq;
using nbaData;
using nbaData.Controllers;
using nbaData.Models;
using NUnit.Framework;

namespace nbaDataTests
{
    public class Tests
    {
        private readonly Mock<IBallDontLieManager> _ballDontLieManagerMock = new();
        private PlayersController _playersController;
        private PlayerController _playerController;

        [SetUp]
        public void Setup()
        {
            _playersController = new PlayersController(_ballDontLieManagerMock.Object);
            _playerController = new PlayerController(_ballDontLieManagerMock.Object);
        }

        #region Players Controller
        
        [Test]
        public void GetPlayers_Success_ReturnPlayers()
        {
            _ballDontLieManagerMock.Reset();
            
            Player player = new("Zach", "Sarkis");

            _ballDontLieManagerMock.Setup(m => m.GetPlayers(true))
                .Returns(new List<Player> {player});

            ObjectResult result = (ObjectResult)_playersController.GetPlayers().Result;
            List<Player> players = result.Value as List<Player>;
            
            _ballDontLieManagerMock.Verify(m => m.GetPlayers(true), Times.Once);
             Assert.That(players.First().first_name == player.first_name);
             Assert.That(players.First().last_name == player.last_name);
             Assert.AreEqual(200, result.StatusCode);
        }
        
        [Test]
        public void GetPlayers_Success_ReturnPlayersFromTeam()
        {
            _ballDontLieManagerMock.Reset();
            
            Team joe = new Team()
            {
                abbreviation = "JOE",
                conference = "East",
                division = "Atlantic"
            };
            
            Player player = new("Zach", "Sarkis", joe);

            _ballDontLieManagerMock.Setup(m => m.GetPlayersByTeam("JOE"))
                .Returns(new List<Player> {player});

            var result = (ObjectResult)_playersController.GetPlayers("JOE").Result;
            List<Player> players = result.Value as List<Player>;
                
            _ballDontLieManagerMock.Verify(m => m.GetPlayersByTeam("JOE"), Times.Once);
            Assert.That(players.First().first_name == player.first_name);
            Assert.That(players.First().last_name == player.last_name);
            Assert.That(players.First().team.abbreviation == player.team.abbreviation);
            Assert.AreEqual(200, result.StatusCode);
        }
        
        [Test]
        public void GetPlayers_Failure_ReturnPlayersFromTeam()
        {
            _ballDontLieManagerMock.Reset();
            
            Player player = new("Zach", "Sarkis");

            _ballDontLieManagerMock.Setup(m => m.GetPlayersByTeam("JOE"))
                .Returns(new List<Player>());

            var result = _playersController.GetPlayers("JOE").Result as NotFoundResult;
                
            _ballDontLieManagerMock.Verify(m => m.GetPlayersByTeam("JOE"), Times.Once);
            
            Assert.AreEqual(404, result.StatusCode);
        }

        #endregion Players Controller

        #region Player Controller

        [Test]
        public void GetPlayer_Success_ReturnPlayer()
        {
            _ballDontLieManagerMock.Reset();
            
            Team boston = new Team()
            {
                abbreviation = "BOS",
                conference = "East",
                division = "Atlantic",
                full_name = "Boston Celtics",
                name = "Celtics",
                city = "Boston",
                id = 0
            };
            
            Player player = new("Zach", "Sarkis", boston, 5, 9, "PG", 180, 0);

            _ballDontLieManagerMock.Setup(p => p.GetPlayer(0))
                .Returns(player);

            ObjectResult result = (ObjectResult)_playerController.GetPlayer(0).Result;

            Player resultingPlayer = result.Value as Player;
            
            _ballDontLieManagerMock.Verify(m => m.GetPlayer(0), Times.Once);
            Assert.That(resultingPlayer.first_name == player.first_name);
            Assert.That(resultingPlayer.last_name == player.last_name);
            Assert.That(resultingPlayer.team == player.team);
            Assert.That(resultingPlayer.height_feet == player.height_feet);
            Assert.That(resultingPlayer.height_inches == player.height_inches);
            Assert.That(resultingPlayer.position == player.position);
            Assert.That(resultingPlayer.weight_pounds == player.weight_pounds);
            Assert.That(resultingPlayer.id == player.id);
            Assert.AreEqual(200, result.StatusCode);
        }

        [Test]
        public void GetPlayer_Failure_ReturnPlayer()
        {
            _ballDontLieManagerMock.Reset();
            
            Team boston = new Team()
            {
                abbreviation = "BOS",
                conference = "East",
                division = "Atlantic",
                full_name = "Boston Celtics",
                name = "Celtics",
                city = "Boston",
                id = 0
            };
            
            Team lal = new Team()
            {
                abbreviation = "LAL",
                conference = "West",
                division = "Pacific",
                full_name = "Los Angles Lakers",
                name = "Lakers",
                city = "Los Angles",
                id = 5
            };
            
            Player player = new("Zach", "Sarkis", lal, 5, 9, "PG", 180, 0);

            _ballDontLieManagerMock.Setup(p => p.GetPlayer(0))
                .Returns(player);

            var result = _playerController.GetPlayer(60).Result as NotFoundResult;

            _ballDontLieManagerMock.Verify(m => m.GetPlayer(60), Times.Once);
            
            Assert.AreEqual(404, result.StatusCode);
        }

        #endregion Player Controller
    }
}