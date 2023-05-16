using System.Linq;

namespace PokerLib;

public class HandStraightFlush : Hand
{
    public HandStraightFlush(Card[] cards)
    {
        Cards = cards;
        Stregth = 8;
        Name = "StraightFlush";
        IsValid = Verify;

    }

    public override bool Verify => IsStraightFlush;

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

            return Hand.ContainsStraight(handFromFlush);
        }
    }
}
