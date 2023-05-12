
using PokerLib;
using System.Linq;

namespace PokerLib
{
    public class HandThreeOfAKInd : Hand
    {
        public HandThreeOfAKInd(Card[] cards)
        {
            Cards = cards;
            Stregth = 3;
            Name = "ThreeOfAKind";
            IsValid = Verify;
        }
        public override bool Verify => Cards.GroupBy(h => h.Value)
       .Where(g => g.Count() == 3)
       .Count() > 0;

    }
}