using System.Collections.Generic;

namespace PokerLib;

public class Hand
{
    public static IHand CreateHand(List<Card> cards)
    {
        IHand hand;

        hand = new Hand9StraightFlush(cards);
        if (hand.IsValid) return hand;

        hand = new Hand8FourOfAKind(cards);
        if (hand.IsValid) return hand;

        hand = new Hand7FullHouse(cards);
        if (hand.IsValid) return hand;

        hand = new Hand6Flush(cards);
        if (hand.IsValid) return hand;

        hand = new Hand5Straight(cards);
        if (hand.IsValid) return hand;

        hand = new Hand4ThreeOfAKind(cards);
        if (hand.IsValid) return hand;

        hand = new Hand3TwoPair(cards);
        if (hand.IsValid) return hand;

        hand = new Hand2OnePair(cards);
        if (hand.IsValid) return hand;

        return new Hand1HighCard(cards);
    }

    public static long GetMultiplier(int rank) => rank switch
    {
        1 => 0x1,
        2 => 0x100000,
        3 => 0x1000000,
        4 => 0x10000000,
        5 => 0x100000000,
        6 => 0x1000000000,
        7 => 0x10000000000,
        8 => 0x100000000000,
        9 => 0x1000000000000,
        _ => 0x1
    };
}
