using NUnit.Framework;
using Poker_Engine.Service;
using PokerEngine.Model;
using System.Collections.Generic;

namespace UnitTest_Engine
{
    public class GameTest
    {

        [Test]
        public void Flush()
        {
            var players = new List<Player>
            {
                new Player { Name = "Joe" , Cards = new string[]{ "3H","6H","8H","JH","KH" } },
                new Player { Name = "Jen" , Cards = new string[]{ "3C","3D","3S","8C","10H" } },
                new Player { Name = "Bob" , Cards = new string[]{ "2H","5C","7S","10C","AC" } },
            };

            var game = new GameService();

            // add the players
            foreach (var p in players)
            {
                // validate player cards
                Assert.IsTrue(game.AddPlayer(p, out var message), message);
            }

            var winners = game.Winners;

            Assert.True(winners[0].Name == "Joe");
        }

        [Test]
        public void HighCard()
        {
            var players = new List<Player>
            {
                new Player { Name = "Joe" , Cards = new string[]{ "3H","4D","9C","9D","QH" } },
                new Player { Name = "Jen" , Cards = new string[]{ "5C","7D","9H","9S","QS" } },
                new Player { Name = "Bob" , Cards = new string[]{ "2H","2C","5S","10C","AH" } },
            };

            var game = new GameService();

            // add the players
            foreach (var p in players)
            {
                // validate player cards
                Assert.IsTrue(game.AddPlayer(p, out var message), message);
            }

            var winners = game.Winners;

            Assert.True(winners[0].Name == "Jen");
        }

        [Test]
        public void Draw()
        {
            var players = new List<Player>
            {
                new Player { Name = "Joe" , Cards = new string[]{ "AH","KH","QH","JH","10H" } },
                new Player { Name = "Jen" , Cards = new string[]{ "5C","4S","9H","9S","QS" } },
                new Player { Name = "Bob" , Cards = new string[]{ "10D","JD","QD","KD","AD" } },
            };

            var game = new GameService();

            // add the players
            foreach (var p in players)
            {
                // validate player cards
                Assert.IsTrue(game.AddPlayer(p, out var message), message);
            }

            var winners = game.Winners;

            Assert.True(winners.Count == 2);

        }

        [Test]
        public void DuplicateCardWithOtherPlayer()
        {
            // add the players
            var players = new List<Player>
            {
                new Player { Name = "Joe" , Cards = new string[]{ "3H","6H","8H","JH","KH" } },
                new Player { Name = "Jen" , Cards = new string[]{ "3C","3D","3S","8C","10H" } },
                new Player { Name = "Bob" , Cards = new string[]{ "2H","5C","7S","10C","3H" } },
            };

            var game = new GameService();

            var message = "";

            foreach (var p in players)
            {
                game.AddPlayer(p, out message);
            }

            // validate if player cards has duplicate with other player
            Assert.AreEqual("Player Bob has duplicate cards with other player", message);

        }
    }
}