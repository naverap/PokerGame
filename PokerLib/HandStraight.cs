namespace PokerLib;

public class HandStraight : Hand
{
    public HandStraight(Card[] cards)
    {
        Cards = cards;
        Stregth = 4;
        Name = "Straight";
        IsValid = Verify;

    }

    public override bool Verify => Hand.ContainsStraight(this);
}
