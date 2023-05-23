using System.Collections.Generic;

namespace PokerLib;

public class Hand8FourOfAKind : HandNOfAKind
{
    public Hand8FourOfAKind(IList<Card> cards) : base(cards, 4, 8, "four of a kind") { }
}
