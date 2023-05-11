using System.Linq;

namespace PokerLib;

public interface IHand
{
    public Card[] Cards { get; set; }
    public int Strength { get; set; }
}
