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

        #region GetPlayers

        [Test]
        public void GetPlayers_Success_ReturnPlayers()
        {
            _ballDontLieManagerMock.Reset();

            Player player = new("Zach", "Sarkis");

            _ballDontLieManagerMock.Setup(m => m.GetPlayers(true))
                .Returns(new List<Player> {player});

            ObjectResult result = (ObjectResult) _playersController.GetPlayers().Result;
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

            var result = (ObjectResult) _playersController.GetPlayers("JOE").Result;
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

        #endregion GetPlayers

        #endregion Players Controller

        #region Player Controller

        #region GetPlayer

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

            Player player = new("Zach", "Sarkis", boston, 5, 9, "PG", 180, 999);

            _ballDontLieManagerMock.Setup(p => p.GetPlayer(999))
                .Returns(player);

            ObjectResult result = (ObjectResult) _playerController.GetPlayer(999).Result;

            Player resultingPlayer = result.Value as Player;

            _ballDontLieManagerMock.Verify(m => m.GetPlayer(999), Times.Once);
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

        #endregion GetPlayer

        #region GetSeasonStats

        [Test]
        public void GetSeasonStats_Success_ReturnPlayer()
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

            Player player = new("Zach", "Sarkis", boston, 5, 9, "PG", 180, 999);

            _ballDontLieManagerMock.Setup(p => p.GetPlayer(999))
                .Returns(player);
            
            SeasonStats stats = new SeasonStats();

            stats.games_played = 65;
            stats.player_id = 434;
            stats.season = 2020;
            stats.min = "35:50";
            stats.fgm = 9.52;
            stats.fga = 20.77;
            stats.fg3m = 2.95;
            stats.fg3a = 7.65;
            stats.ftm = 4.8;
            stats.fta = 5.49;
            stats.oreb = 0.8;
            stats.dreb = 6.58;
            stats.reb = 7.38;
            stats.ast = 4.31;
            stats.stl = 1.17;
            stats.blk = 0.51;
            stats.turnover = 2.66;
            stats.pf = 1.89;
            stats.pts = 26.8;
            stats.fg_pct = 0.459;
            stats.fg3_pct = 0.386;
            stats.ft_pct = 0.874;

            _ballDontLieManagerMock.Setup(p => p.GetShootingStats(999, 2020))
                .Returns(stats);

            ObjectResult result = (ObjectResult)_playerController.GetSeasonStats(999).Result;

            _ballDontLieManagerMock.Verify(m => m.GetShootingStats(999, 2020), Times.Once);

            SeasonStats resultingStats = result.Value as SeasonStats;
            
            Assert.That(stats.ast == resultingStats.ast);
            Assert.That(stats.blk == resultingStats.blk);
            Assert.That(stats.dreb == resultingStats.dreb);
            Assert.That(stats.fg3_pct == resultingStats.fg3_pct);
            Assert.That(stats.fg3a == resultingStats.fg3a);
            Assert.That(stats.fg3m == resultingStats.fg3m);
            Assert.That(stats.fg_pct == resultingStats.fg_pct);
            Assert.That(stats.fga == resultingStats.fga);
            Assert.That(stats.fgm == resultingStats.fgm);
            Assert.That(stats.ft_pct == resultingStats.ft_pct);
            Assert.That(stats.fta == resultingStats.fta);
            Assert.That(stats.ftm == resultingStats.ftm);
            Assert.That(stats.games_played == resultingStats.games_played);
            Assert.That(stats.oreb == resultingStats.oreb);
            Assert.That(stats.pf == resultingStats.pf);
            Assert.That(stats.player_id == resultingStats.player_id);
            Assert.That(stats.pts == resultingStats.pts);
            Assert.That(stats.reb == resultingStats.reb);
            Assert.That(stats.season == resultingStats.season);
            Assert.That(stats.stl == resultingStats.stl);
            Assert.That(stats.turnover == resultingStats.turnover);
            Assert.That(stats.min == resultingStats.min);
            Assert.AreEqual(200, result.StatusCode);
        }

        [Test]
        public void GetSeasonStats_Failure_ReturnPlayer()
        {
            _ballDontLieManagerMock.Reset();

            SeasonStats stats = new SeasonStats();

            _ballDontLieManagerMock.Setup(p => p.GetShootingStats(999999, 2020))
                .Returns(stats);

            var result = _playerController.GetSeasonStats(999999).Result as NotFoundResult;

            _ballDontLieManagerMock.Verify(m => m.GetShootingStats(999999, 2020), Times.Never);

            Assert.AreEqual(404, result.StatusCode);
        }

        #endregion GetSeasonStats

        #region GetGameAverageStats

        [Test]
        public void GetGameAverageStats_Success_ReturnPlayer()
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

            Player player = new("Zach", "Sarkis", boston, 5, 9, "PG", 180, 999);

            _ballDontLieManagerMock.Setup(p => p.GetPlayer(999))
                .Returns(player);
            
            ShootingGameStats shootingGameStats = new ShootingGameStats();
            
            shootingGameStats.fg3_pct = 0.0;
            shootingGameStats.fg3a = 9.666666666666666;
            shootingGameStats.fg3m = 4.0;
            shootingGameStats.fg2_pct = 0.0;
            shootingGameStats.fg2a = 16.666666666666668;
            shootingGameStats.fg2m = 8.666666666666666;
            
            _ballDontLieManagerMock.Setup(p => p.GetAverageGameStats(999, 3))
                .Returns(shootingGameStats);

            ObjectResult result = (ObjectResult)_playerController.GetAverageGameStats(999, 3).Result;

            _ballDontLieManagerMock.Verify(m => m.GetAverageGameStats(999, 3), Times.Once);

            ShootingGameStats resultingStats = result.Value as ShootingGameStats;
            
            Assert.That(shootingGameStats.fg3_pct == resultingStats.fg3_pct);
            Assert.That(shootingGameStats.fg3a == resultingStats.fg3a);
            Assert.That(shootingGameStats.fg3m == resultingStats.fg3m);
            Assert.That(shootingGameStats.fg2_pct == resultingStats.fg2_pct);
            Assert.That(shootingGameStats.fg2a == resultingStats.fg2a);
            Assert.That(shootingGameStats.fg2m == resultingStats.fg2m);
            Assert.AreEqual(200, result.StatusCode);
        }

        [Test]
        public void GetGameAverageStats_Failure_ReturnPlayer()
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

            Player player = new("Zach", "Sarkis", boston, 5, 9, "PG", 180, 999);

            _ballDontLieManagerMock.Setup(p => p.GetPlayer(999))
                .Returns(player);
            
            ShootingGameStats shootingGameStats = new ShootingGameStats();
            
            _ballDontLieManagerMock.Setup(p => p.GetAverageGameStats(999, 3))
                .Returns(shootingGameStats);

            NotFoundResult result = _playerController.GetAverageGameStats(999, 3).Result as NotFoundResult;

            _ballDontLieManagerMock.Verify(m => m.GetAverageGameStats(999, 3), Times.Once);

            Assert.AreEqual(404, result.StatusCode);
        }

        #endregion GetGameAverageStats

        #endregion Player Controller
    }
}