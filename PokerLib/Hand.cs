using System.Linq;

namespace PokerLib;

public class Hand
{
    public Card[] Cards { get; set; }
    public int Stregth { get; set; }
    public string Name { get; set; }
    public bool IsValid { get; set; }
    public virtual bool Verify => true;

    public Hand(Card[] cards)
    {
        Cards = cards;
    }
    public Hand() { }

    public static Hand CreateHand(Card[] cards)
    {
        Hand straightFlush = new HandStraightFlush(cards);
        if (straightFlush.IsValid) return straightFlush;
        Hand fourOfAKind = new HandFourOfAKind(cards);
        if (fourOfAKind.IsValid) return fourOfAKind;
        Hand fullHouse = new HandFullHouse(cards);
        if (fullHouse.IsValid) return fullHouse;
        Hand flush = new HandFlush(cards);
        if (flush.IsValid) return flush;
        Hand straight = new HandStraight(cards);
        if (straight.IsValid) return straight;
        Hand threeOfAKind = new HandThreeOfAKInd(cards);
        if (threeOfAKind.IsValid) return threeOfAKind; 
        Hand twoPair = new HandTwoPair(cards);
        if (twoPair.IsValid) return twoPair;
        Hand pair = new HandPair(cards);
        if (pair.IsValid) return pair;
        return new HandHighCard(cards);
    }

    public bool IsFlush => Cards.GroupBy(h => h.Suit)
        .Where(g => g.Count() >= 5)
        .Any();

    protected static bool ContainsStraight(Hand hand)
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

    protected bool Contains(CardValue v)
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
}
