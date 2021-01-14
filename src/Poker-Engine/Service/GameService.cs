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
            if (this.IsDuplicateWithOtherPlayers(newPlayer.Cards))
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
            // check # of players
            if (_Players.Count == 0)
                throw new ApplicationException("No player/s added");

            var winners = new List<Player> { };


            // compare each players hands
            foreach (var p in _Players)
            {
                p.Hand = DeckService.Analyze(p.Cards);

                // skip invalid hand
                if (p.Hand == Deck.Hand.Invalid)
                    continue;

                // initialize winner, 1st player
                if (winners.Count == 0)
                    winners.Add(p);
                else
                {
                    var compare = DeckService.CompareCard(winners[0].Cards, p.Cards, p.Hand);

                    // if players has equal hands and equal cards, it will be draw
                    if (p.Hand == winners[0].Hand && compare == Deck.CompareHand.Equal)
                    {
                        winners.Add(p);
                    }
                    else if (p.Hand < winners[0].Hand || (p.Hand == winners[0].Hand && compare == Deck.CompareHand.Higher)) // current player has higher hands, replace the winners
                    {
                        winners.Clear();
                        winners.Add(p);
                    }
                }
            }

            return winners;
        }

        private bool IsDuplicateWithOtherPlayers(string[] cards)
        {
            if (_Players.Count > 0)
                foreach (var playerCards in _Players.Select(x => x.Cards))
                    if (playerCards.Intersect(cards).Any())
                        return true;

            return false;
        }

    }
}
