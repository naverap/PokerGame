using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PokerLib;

public class Player
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int Pot { get; set; }

    public int CurrentBet { get; set; }

    public bool HasFolded { get; set; }

    public List<Card> Cards { get; set; }

    public bool IsDealer { get; set; }

    public bool IsSmallBlind { get; set; }

    public bool IsBigBlind { get; set; }

    public bool HasWon { get; set; }

    public BetType LastBetType { get; set; }

    public int LastBetAmount { get; set; }

    [JsonIgnore]
    public Hand Hand { get; set; }

    public Player() { }

    public Player(string name, int playerPot, bool isPlaying)
    {
        Name = name;
        Pot = playerPot;
        CurrentBet = 0;
        HasFolded = isPlaying;
    }

    public string GetStatus()
    {
        return $"Player {Id}\n{Name}\nPot {Pot}\n{LastBetType} {LastBetAmount}";
    }
}