﻿namespace PokerLib;

public class Player
{
    public int Id { get; set; }
    public int Pot { get; set; }
    public int CurrentBet { get; set; }
    public bool HasFolded { get; set; }
    public Card[] Cards { get; set; }

    public Player() { }

    public Player(int playerPot, bool isPlaying)
    {
        Pot = playerPot;
        CurrentBet = 0;
        HasFolded = isPlaying;
    }
}