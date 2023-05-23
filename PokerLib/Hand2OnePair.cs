using System.Collections.Generic;

namespace PokerLib;

public class Hand2OnePair : HandNOfAKind
{
    public Hand2OnePair(IList<Card> cards) : base(cards, 2, 2, "one pair") { }
}
