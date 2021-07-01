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
        private PlayersController _controller;

        [SetUp]
        public void Setup()
        {
            _controller = new PlayersController(_ballDontLieManagerMock.Object);
        }

        [Test]
        public void GetPlayers_Success_ReturnPlayers()
        {
            _ballDontLieManagerMock.Reset();
            
            Player player = new("Zach", "Sarkis");

            _ballDontLieManagerMock.Setup(m => m.GetPlayers(true))
                .Returns(new List<Player> {player});

            ObjectResult result = (ObjectResult)_controller.GetPlayers().Result;
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

            var result = (ObjectResult)_controller.GetPlayers("JOE").Result;
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

            var result = _controller.GetPlayers("JOE").Result as NotFoundResult;
                
            _ballDontLieManagerMock.Verify(m => m.GetPlayersByTeam("JOE"), Times.Once);
            
            Assert.AreEqual(404, result.StatusCode);
        }
    }
}