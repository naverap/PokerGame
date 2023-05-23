using System.Collections.Generic;

namespace PokerLib;

public interface IHand
{
    IList<Card> Cards { get; }
    string Name { get; }
    long Score { get; }
    bool IsValid { get; }
}
