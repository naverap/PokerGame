using System.Linq;

namespace PokerLib;

public class Hand
{
    public Card[] Cards { get; set; }

    public Hand(Card[] cards)
    {
        Cards = cards;
    }
    public Hand() { }

    public int Strength()
    {

        if (IsStraightFlush)
        {
            return 8;
        }
        if (IsFourOfAKind)
        {
            return 7;
        }
        if (IsFullHouse)
        {
            return 6;
        }
        if (IsFlush)
        {
            return 5;
        }
        if (IsStraight)
        {
            return 4;
        }
        if (IsThreeOfAKind)
        {
            return 3;
        }
        if (IsTwoPair)
        {
            return 2;
        }
        if (IsPair)
        {
            return 1;
        }
        return 0;
    }

    public bool IsPair => Cards.GroupBy(h => h.Value)
        .Where(g => g.Count() == 2)
        .Count() == 1;

    public bool IsTwoPair => Cards.GroupBy(h => h.Value)
        .Where(g => g.Count() == 2)
        .Count() > 1;

    public bool IsThreeOfAKind => Cards.GroupBy(h => h.Value)
        .Where(g => g.Count() == 3)
        .Count() == 1;

    public bool IsDoubleThreeOfAKind => Cards.GroupBy(h => h.Value)
        .Where(g => g.Count() == 3)
        .Count() == 2;

    public bool IsFourOfAKind => Cards.GroupBy(h => h.Value)
        .Where(g => g.Count() == 4)
        .Any();
    public bool IsFullHouse => IsPair && IsThreeOfAKind || IsDoubleThreeOfAKind;

    public bool IsFlush => Cards.GroupBy(h => h.Suit)
        .Where(g => g.Count() >= 5)
        .Any();

    public bool IsStraight => ContainsStraight(this);

    private static bool ContainsStraight(Hand hand)
    {
        if (hand.Contains(CardValue.Ace) &&
             hand.Contains(CardValue.Two) &&
             hand.Contains(CardValue.Three) &&
             hand.Contains(CardValue.Four) &&
            hand.Contains(CardValue.Five))
        {
            return true;
        }

        var ordered = hand.Cards
            .GroupBy(h => h.Value)
            .OrderBy(g => g.Key)
            .ToArray();

        if (ordered.Length < 5) return false;

        for (var i = 0; i < ordered.Length - 5; i++)
        {
            var first = ordered[i].Key;
            var last = ordered[i + 4].Key;
            if (last - first == 4) return true;
        }
        return false;
    }

    private bool Contains(CardValue v)
    {
        for (int i = 0; i < Cards.Length; i++)
        {
            if (Cards[i].Value == v)
            {
                return true;
            }
        }
        return false;
    }

    public bool IsStraightFlush
    {
        get
        {
            if (!IsFlush) return false;

            var flashSuit = Cards.GroupBy(h => h.Suit)
                .Where(g => g.Count() >= 5)
                .Single()
                .Key;

            var handFromFlush = new Hand(Cards.Where(card => card.Suit == flashSuit).ToArray());

            return ContainsStraight(handFromFlush);
        }
    }
}
