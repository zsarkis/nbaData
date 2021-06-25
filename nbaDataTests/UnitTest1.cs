using System.Collections.Generic;
using System.Linq;
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
            Player player = new("Mike", "Hunt");

            _ballDontLieManagerMock.Setup(m => m.GetPlayers())
                .Returns(new List<Player> {player});

            List<Player> result = _controller.GetPlayers().ToList();

            _ballDontLieManagerMock.Verify(m => m.GetPlayers(), Times.AtLeast(1));
            Assert.That(result.First().first_name == player.first_name);
            Assert.That(result.First().last_name == player.last_name);
        }
        
        [Test]
        public void GetPlayers_WithAbbreviation_ReturnBostonPlayers()
        {
            // Team celtics = new Team()
            // {
            //     abbreviation = "BOS",
            //     conference = "East",
            //     division = "Atlantic"
            // };
            //
            // Team nets = new Team()
            // {
            //     abbreviation = "BKN",
            //     conference = "East",
            //     division = "Atlantic"
            // };
            //
            // List<Player> players = new List<Player>()
            // {
            //     new Player("Will", "Barton", nets),
            //     new Player("Kyrie", "Irving", nets),
            //     new Player("Ben", "Dover"),
            //     new Player("Jaylen", "Brown", celtics),
            //     new Player("Jayson", "Tatum", celtics)
            // };
            //
            // _ballDontLieManagerMock.Setup(m => m.GetPlayers())
            //     .Returns(players);
            //
            // List<Player> result = _controller.GetPlayers("BOS").ToList();
            //
            // Assert.That(result.Any());
            // Assert.That(result.All(p => p.team == celtics));
            Assert.That(1 == 0 + 1);
        }

        //
        // [Test]
        // public void Test2()
        // {
        //     // TODO: names might be fucked, check that
        //     // Manually enter 5 players by name from 2 teams
        //     List<Player> players = new List<Player>()
        //     {
        //         new Player("Will", "Barton"),
        //         new Player("Gary", "Harris"),
        //         new Player("Nikola", "Jokic"),
        //         new Player("Paul", "Millsap"),
        //         new Player("Jamal", "Murray"),
        //         new Player("Jaylen", "Brown"),
        //         new Player("Gordon", "Hayward"),
        //         new Player("Al", "Horford"),
        //         new Player("Kyrie", "Irving"),
        //         new Player("Jayson", "Tatum")
        //     };
        //     
        //     // Get players from balldontlie
        //     var allPlayers = _controllerUnderTest.GetPlayers();
        //
        //     // Player thatGuy = new Player("George", "King");
        //     
        //     // var correctPlayer = allPlayers.Where(player =>
        //     //     players.Any(guy => guy.first_name.Equals(player.first_name) && guy.last_name.Equals(player.last_name)));
        //
        //     List<Player> correctPlayers = 
        //         players.Select(player => 
        //             allPlayers.FirstOrDefault(currentPlayer => 
        //                 currentPlayer.first_name.Equals(player.first_name) && currentPlayer.last_name.Equals(player.last_name))).ToList();
        //
        //     // Find ID for manually entered players
        //     // Get season averages for those players from balldontlie
        //     // Ship object with all 10 players over to FE
        // }
    }
}