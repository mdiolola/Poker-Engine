using System;
using System.Linq;


namespace PokerEngine.Model
{
    public class Player
    {
        public string Name { get; set; }

        public string[] Cards { get; set; }

        public Deck.Hand Hand { get; set; }

    }
}
