using System;
using System.Collections.Generic;
using System.Text;

namespace PokerEngine.Model
{
    public static class Deck
    {
        public enum Hand
        {
            RoyalFlush = 0,
            StraightFlush = 1,
            FourofaKind = 2,
            FullHouse = 3,
            Flush = 4,
            Straight = 5,
            Threeofakind = 6,
            TwoPair = 7,
            OnePair = 8,
            HighCard = 9,
            Invalid = 99,
        }

        public enum CompareHand
        {
            Higher,
            Lower,
            Equal
        }
    }
}
