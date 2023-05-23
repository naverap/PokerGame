using System.Collections.Generic;

namespace PokerLib;

public class Hand4ThreeOfAKind : HandNOfAKind
{
    public Hand4ThreeOfAKind(IList<Card> cards) : base(cards, 3, 4, "three of a kind") { }
}