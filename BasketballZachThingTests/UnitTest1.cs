using System;
using System.Collections.Generic;
using System.Linq;
using nbaData.Controllers;
using nbaData.Models;
using NUnit.Framework;

namespace BasketballZachThingTests
{
    public class Tests
    {
        private readonly PlayersController _controllerUnderTest;

        public Tests()
        {
            _controllerUnderTest = new PlayersController();
        }

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var result = _controllerUnderTest.GetPlayers();
        }

      


        [Test]
        public void Test2()
        {
            // TODO: names might be fucked, check that
            // Manually enter 5 players by name from 2 teams
            List<Player> players = new List<Player>()
            {
                new Player("Will", "Barton"),
                new Player("Gary", "Harris"),
                new Player("Nikola", "Jokic"),
                new Player("Paul", "Millsap"),
                new Player("Jamal", "Murray"),
                new Player("Jaylen", "Brown"),
                new Player("Gordon", "Hayward"),
                new Player("Al", "Horford"),
                new Player("Kyrie", "Irving"),
                new Player("Jayson", "Tatum")
            };
            
            // Get players from balldontlie
            var allPlayers = _controllerUnderTest.GetPlayers();

            // Player thatGuy = new Player("George", "King");
            
            // var correctPlayer = allPlayers.Where(player =>
            //     players.Any(guy => guy.first_name.Equals(player.first_name) && guy.last_name.Equals(player.last_name)));

            List<Player> correctPlayers = 
                players.Select(player => 
                    allPlayers.FirstOrDefault(currentPlayer => 
                        currentPlayer.first_name.Equals(player.first_name) && currentPlayer.last_name.Equals(player.last_name))).ToList();

            // Find ID for manually entered players
            // Get season averages for those players from balldontlie
            // Ship object with all 10 players over to FE
        }
    }
}