using System.Linq;

namespace PokerLib;

public class HandPair : Hand
{
    public HandPair(Card[] cards)
    {
        Cards = cards;
        Stregth = 1;
        Name = "Pair";
        IsValid = Verify;
        //TieBreaker = FindTieBreaker();
    }

    public override bool Verify => Cards.GroupBy(h => h.Value)
        .Where(g => g.Count() == 2)
        .Count() == 1;
    //private Card FindTieBreaker()
    //{
    //    IGrouping<CardValue, Card> t = Cards.GroupBy(h => h.Value).Max();
    //    return  t.First();
    //}
}
