﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace PokerLib;

public class Game
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public int Blind { get; set; } = 100;
    public int Pot { get; set; } = 0;
    public int LastBet { get; set; }
    public Round Round { get; set; }

    // Cards
    public List<Card> Deck { get; set; }
    public List<Card> CommunityCards { get; set; }

    // Players
    public int MaxPlayers { get; set; } = 4;
    public List<Player> Players { get; set; }
    public bool HasPlayer(string playerName) => Players.Any(p => p.Name == playerName);
    public int CurrentPlayerIndex { get; set; }
    public Player CurrentPlayer => Players[CurrentPlayerIndex];

    // Dealer
    public int DealerIndex { get; set; }
    public Player Dealer => Players[DealerIndex];

    public bool IsOngoing { get; set; }
    public bool CanJoin => Players.Count < MaxPlayers && !IsOngoing;

    public Game()
    {
        Players = new();
        Deck = new Deck();
        Deck.Shuffle();
        CommunityCards = Deck.Pop(5);
    }

    int GetLowestAvailableId()
    {
        for (int id = 0; id < MaxPlayers; id++)
            if (!Players.Exists(p => p.Id == id))
                return id;
        throw new Exception("no available space in game");
    }

    public void AddPlayer(Player player)
    {
        player.Id = GetLowestAvailableId();
        player.Cards = Deck.Pop(2);
        Players.Add(player);
    }

    public Player GetPlayerByName(string playerName)
    {
        return Players.SingleOrDefault(p => p.Name == playerName);
    }

    public void SetAndPayWinners()
    {
        foreach (Player player in Players)
        {
            player.Hand = Hand.CreateHand(CommunityCards.Concat(player.Cards).ToArray());
        }

        var winners = Players.Where(p => !p.HasFolded)
            .GroupBy(p => p.Hand.Stregth)
            .OrderByDescending(g => g.Key)
            .First();

        var reward = Pot / winners.Count();

        foreach (var player in winners)
        {
            player.HasWon = true;
            player.Pot += reward;
        }

        Pot = 0;
    }

    public void SetNextPlayerTurn()
    {
        CurrentPlayerIndex++;
        if (CurrentPlayerIndex >= Players.Count)
        {
            CurrentPlayerIndex = 0;
        }
    }

    public bool RoundHasEnded()
    {
        foreach (var player in Players)
        {
            if (player.LastBetType == BetType.None)
                return false;
            if (player.LastBetAmount != LastBet && !player.HasFolded)
                return false;
        }
        return true;
    }

    public void NextRound()
    {
        Round++;
        LastBet = 0;
        foreach (var player in Players)
        {
            player.LastBetAmount = 0;
            player.LastBetType = BetType.None;
        }
        if (Round == Round.Ended)
        {
            SetAndPayWinners();
        }
    }

    public bool Bet(Player player, BetType betType, int amount = 0)
    {
        if (player != CurrentPlayer)
            return false;

        if (player.HasFolded)
            return false;

        var isValidBet = MakeBet(player, betType, amount);
        if (!isValidBet)
            return false;

        player.LastBetType = betType;
        player.LastBetAmount = LastBet;
        if (betType == BetType.Fold)
            player.LastBetAmount = 0;

        do { SetNextPlayerTurn(); }
        while (CurrentPlayer.HasFolded);

        if (RoundHasEnded())
            NextRound();

        return true;
    }

    bool MakeBet(Player player, BetType betType, int amount)
    {
        switch (betType)
        {
            case BetType.Fold:
                player.HasFolded = true;
                return true;
            case BetType.Check:
                if (LastBet > 0)
                    return false;
                return true;
            case BetType.Bet:
                if (player.Pot < amount)
                    return false;
                Pot += amount;
                player.Pot -= amount;
                LastBet = amount;
                return true;
            case BetType.Call:
                if (LastBet == 0)
                    return false;
                amount = LastBet - player.LastBetAmount;
                if (player.Pot < amount)
                    return false; // TODO: allow to call and return money to previous betters. Also applicable to all in
                Pot += amount;
                player.Pot -= amount;
                return true;
            case BetType.Raise:
                if (LastBet == 0)
                    return false;
                return MakeBet(player, BetType.Bet, LastBet + amount);
            case BetType.AllIn:
                return MakeBet(player, BetType.Bet, player.Pot);
            default:
                throw new ArgumentOutOfRangeException(nameof(betType));
        }
    }
}
