using System;
using System.Collections.Generic;
using static System.Linq.Enumerable;
using PokerEngine.Model;
using static PokerEngine.Model.Deck;

namespace PokerEngine.Service
{

    public static class DeckService
    {
        private const bool Y = true;

        private const int rankMask = 0b11_1111_1111_1111;
        private const int suitMask = 0b1111 << 14;

        private static readonly Card[] deck = (from suit in Range(1, 4) from rank in Range(1, 13) select new Card(rank, suit)).ToArray();

        public static Hand Analyze(string[] cards)
        {
            if (cards.Count() != 5) return Hand.Invalid; //hand must consist of 5 cards
            var cardsOnHand = ParseCards(cards);

            if (cardsOnHand.GroupBy(x => x.Code).Where(g => g.Count() > 1).Any()) return Hand.Invalid; // Duplicate card in hands

            cardsOnHand.Sort();
            if (cardsOnHand[0].Equals(Card.Invalid)) return Hand.Invalid;

            var hand = Evaluate(cardsOnHand);

            return hand;
        }

        static List<Card> ParseCards(string[] hands) => hands.Select(c => ParseCard(c.ToLower())).ToList();

        static Card ParseCard(string card) => (card.Length, card) switch
        {
            (3, _) when card[..2] == "10" => (10, ParseSuit(card[2])),
            (2, _) => (ParseRank(card[0]), ParseSuit(card[1])),
            (_, _) => Card.Invalid
        };

        static int ParseRank(char rank) => rank switch
        {
            'a' => 1,
            'j' => 11,
            'q' => 12,
            'k' => 13,
            _ when rank >= '2' && rank <= '9' => rank - '0',
            _ => -1
        };

        static int ParseSuit(char suit) => suit switch
        {
            'C' => 1,
            'c' => 1,
            'D' => 2,
            'd' => 2,
            'H' => 3,
            'h' => 3,
            'S' => 4,
            's' => 4,
            _ => -1
        };


        public static CompareHand CompareCardInHands(string[] player1, string[] player2, Hand hand)
        {
            return CompareHands(ParseCards(player1), ParseCards(player2), hand);
        }

        static CompareHand CompareHands(List<Card> player1, List<Card> player2, Hand hand)
        {
            var p1 = GetHighCard(player1, hand);
            var p2 = GetHighCard(player2, hand);

            if (!p1.Rank.Equals(p2.Rank))
                return p1.Rank > p2.Rank ? CompareHand.Lower : CompareHand.Higher;

            if (p1.Rank == p2.Rank && player1.Count != 1)
            {
                player1.RemoveAll(r => r.Rank == p1.Rank);
                player2.RemoveAll(r => r.Rank == p2.Rank);

                return CompareHands(player1, player2, hand);
            }

            return CompareHand.Equal;
        }

        static Card GetHighCard(List<Card> cardOnHand, Hand hand)
        {
            return hand switch
            {
                var c when
                c == Hand.FourofaKind ||
                c == Hand.FullHouse ||
                c == Hand.Flush ||
                c == Hand.Straight ||
                c == Hand.Threeofakind ||
                c == Hand.TwoPair ||
                c == Hand.OnePair => cardOnHand.GroupBy(x => x.Rank).OrderByDescending(i => i.Key).SelectMany(s => s).FirstOrDefault(),
                _ => cardOnHand.OrderByDescending(i => i.Rank).FirstOrDefault()
            };
        }

        static Hand Evaluate(List<Card> hand)
        {
            var frequencies = hand.GroupBy(c => c.Rank).Select(g => g.Count()).OrderByDescending(c => c).ToArray();
            (int f0, int f1) = (frequencies[0], frequencies.Length > 1 ? frequencies[1] : 0);

            return (IsRoyalFlush(), IsFlush(), IsStraight(), f0, f1) switch
            {
                (Y, Y, Y, _, _) => Hand.RoyalFlush,
                (_, Y, Y, _, _) => Hand.StraightFlush,
                (_, _, _, 4, _) => Hand.FourofaKind,
                (_, _, _, 3, 2) => Hand.FullHouse,
                (_, Y, _, _, _) => Hand.Flush,
                (_, _, Y, _, _) => Hand.Straight,
                (_, _, _, 3, _) => Hand.Threeofakind,
                (_, _, _, 2, 2) => Hand.TwoPair,
                (_, _, _, 2, _) => Hand.OnePair,
                _ => Hand.HighCard
            };

            bool IsFlush() => hand.Aggregate(suitMask, (r, c) => r & c.Code) > 0;

            bool IsStraight()
            {
                int r = hand.Aggregate(0, (r, c) => r | c.Code) & rankMask;
                for (int i = 0; i < 4; i++) r &= r << 1;
                return r > 0;
            }

            bool IsRoyalFlush() { return (IsFlush() && IsStraight() && hand.OrderBy(i => i.Rank).FirstOrDefault().Rank == 1); }
        }
    }
}
