using System;
using System.Collections.Generic;
using System.Text;

namespace PokerEngine.Model
{
    public class Card : IEquatable<Card>, IComparable<Card>
    {
        private static readonly string[] ranks = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "j", "q", "k", "a" };
        private static readonly string[] suits = { "C", "D", "H", "S" };

        public static readonly Card Invalid = new Card(-1, -1);

        public Card(int rank, int suit)
        {
            (Rank, Suit, Code) = (rank, suit) switch
            {
                (_, -1) => (-1, -1, -1),
                (-1, _) => (-1, -1, -1),
                (0, _) => (0, 0, 0),
                (1, _) => (rank, suit, (1 << (13 + suit)) | ((1 << 13) | 1)),
                (_, _) => (rank, suit, (1 << (13 + suit)) | (1 << (rank - 1)))
            };
        }

        public static implicit operator Card((int rank, int suit) tuple) => new Card(tuple.rank, tuple.suit);
        public int Rank { get; }
        public int Suit { get; }
        public int Code { get; }

        public override string ToString() => Rank switch
        {
            -1 => "invalid",
            _ => $"{ranks[Rank - 1]}{suits[Suit - 1]}"
        };

        //public override int GetHashCode() => Rank << 16 | Suit;
        public bool Equals(Card other) => Rank == other.Rank && Suit == other.Suit;

        public int CompareTo(Card other)
        {
            int c = Rank.CompareTo(other.Rank);
            if (c != 0) return c;
            return Suit.CompareTo(other.Suit);
        }
    }
}
