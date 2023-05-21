using System.Collections.Generic;

namespace PokerLib;

public class Deck : List<Card>
{
    public Deck() : base()
    {
        for (int i = 0; i < 52; i++)
        {
            Add(new Card(i));
        }
    }
}
