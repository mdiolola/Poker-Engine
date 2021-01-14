using PokerEngine.Model;
using PokerEngine.Service;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Poker_Engine.Service
{
    public class GameService
    {
        public List<Player> Players { get => this._Players; }

        private List<Player> _Players { get; set; } = new List<Player>();

        public bool AddPlayer(Player newPlayer, out string message)
        {
            if (this.isDuplicateWithOtherPlayers(newPlayer.Cards))
            {
                message = $"Player {newPlayer.Name} has duplicate cards with other player";
                return false;
            }

            this._Players.Add(newPlayer);
            message = null;
            return true;
        }

        public List<Player> Winners { get => this.GetWinners(); }
       
        private List<Player> GetWinners()
        {

            if (_Players.Count == 0)
                throw new ApplicationException("No player/s added");

            var winners = new List<Player> { };

            foreach (var p in _Players)
            {
                p.Hand = DeckService.Analyze(p.Cards);

                if (p.Hand == Deck.Hand.Invalid)
                    continue;

                if (winners.Count == 0)
                    winners.Add(p);
                else
                {
                    var compare = DeckService.CompareCardInHands(winners[0].Cards, p.Cards, p.Hand);

                    if (p.Hand == winners[0].Hand && compare == Deck.CompareHand.Equal)
                    {
                        winners.Add(p);
                    }
                    else if ((p.Hand == winners[0].Hand && compare == Deck.CompareHand.Higher) || p.Hand < winners[0].Hand)
                    {
                        winners.Clear();
                        winners.Add(p);
                    }
                }
            }

            return winners;
        }

        private bool isDuplicateWithOtherPlayers(string[] cards)
        {
            if (_Players.Count > 0)
                foreach (var playerCards in _Players.Select(x => x.Cards))
                    if (playerCards.Intersect(cards).Any())
                        return true;

            return false;
        }

    }
}
