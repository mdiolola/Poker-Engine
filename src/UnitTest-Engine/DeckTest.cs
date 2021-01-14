using NUnit.Framework;
using Poker_Engine;
using PokerEngine.Model;
using PokerEngine.Service;
using System.Collections.Generic;

namespace UnitTest_Engine
{
    public class DeckTest
    {
        [Test]
        public void HandRanks()
        {
            var players = new List<Player>
            {
                new Player { Name = "RoyalFlush"    , Cards = new string[]{ "KS", "QS", "JS", "AS", "10S"   } },
                new Player { Name = "StraightFlush" , Cards = new string[]{ "KS", "QS", "JS", "9S", "10S"   } },
                new Player { Name = "FourofaKind"   , Cards = new string[]{ "AH", "AD", "AS", "AC", "2H"    } },
                new Player { Name = "FullHouse"     , Cards = new string[]{ "AH", "AD", "AS", "KC", "KH"    } },
                new Player { Name = "Flush"         , Cards = new string[]{ "2H", "7H", "9H", "JH", "KH"    } },
                new Player { Name = "Straight"      , Cards = new string[]{ "AH", "KD", "QS", "JC", "10H"   } },
                new Player { Name = "Threeofakind"  , Cards = new string[]{ "AH", "AD", "AS", "KC", "2H"    } },
                new Player { Name = "TwoPair"       , Cards = new string[]{ "AH", "AD", "KS", "KC", "2H"    } },
                new Player { Name = "OnePair"       , Cards = new string[]{ "AH", "AD", "QS", "KC", "2H"    } },
                new Player { Name = "HighCard"      , Cards = new string[]{ "AH", "3D", "5S", "7C", "9H"    } },
            };

            for (var i = 0; i < players.Count; i++)
            {
                players[i].Hand = DeckService.Analyze(players[i].Cards );

                Assert.AreEqual(i, (int)players[i].Hand);
            }
        }

        [Test]
        public void CompareHands()
        {
            // validate draw/equal cards
            Assert.AreEqual(DeckService.CompareCardInHands(new string[] { "AH", "KH", "QH", "JH", "10H" }, new string[] { "10D", "JD", "QD", "KD", "AD" }, Deck.Hand.RoyalFlush), Deck.CompareHand.Equal);

            // validate higher card
            Assert.AreEqual(DeckService.CompareCardInHands(new string[] { "AH", "KH", "QH", "JH", "10H" }, new string[] { "10D", "JD", "QD", "KD", "9D" }, Deck.Hand.RoyalFlush), Deck.CompareHand.Higher);

            // validate lower card
            Assert.AreEqual(DeckService.CompareCardInHands(new string[] { "9h", "Kh", "Qh", "Jh", "10h" }, new string[] { "10D", "JD", "QD", "KD", "AD" }, Deck.Hand.RoyalFlush), Deck.CompareHand.Lower);
        }

        [Test]
        public void InvalidHands()
        {
            // less than 5 cards
            var invalid1 = DeckService.Analyze(new string[] { "JD" });
            Assert.AreEqual(Deck.Hand.Invalid, invalid1);

            // duplicate cards in hand
            var invalid2 = DeckService.Analyze(new string[] { "JD" , "10S", "9h", "10c", "9H"});
            Assert.AreEqual(Deck.Hand.Invalid, invalid2);


        }

    }
}
