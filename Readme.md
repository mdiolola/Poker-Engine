Poker Code Reference: https://rosettacode.org/wiki/Poker_hand_analyser

Assumptions:
- Player Name can be duplicate
- Using Traditional High Poker Hand Ranks
- Only evaluated cards/hand based on Rank (1 is lowest, Ace is highest). Suit doesn’t matter.
- If both players have identical hand with different suit, both players win.
- Validation of cards after adding all players
- Validations for duplicate cards vs other players upon adding new player
- Add players has no limits but will validate duplicates
- Support below hands :
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
