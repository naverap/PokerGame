using System.Collections.Generic;

namespace PokerLib;

public class Player
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Pot { get; set; }
    public int CurrentBet { get; set; }
    public bool HasFolded { get; set; }
    public List<Card> Cards { get; set; }
    public bool HasWon { get; set; }

    public Player() { }

    public Player(string name, int playerPot, bool isPlaying)
    {
        Name = name;
        Pot = playerPot;
        CurrentBet = 0;
        HasFolded = isPlaying;
    }
}