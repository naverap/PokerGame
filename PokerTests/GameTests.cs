using System.Text.Json;

namespace PokerTests;

[TestClass]
public class GameTests
{
    [TestMethod]
    public void CreateGame()
    {
        var game = new Game();
        game.AddPlayer(new Player("test", 1000, false));
        game.AddPlayer(new Player("test", 1000, false));
        Assert.AreEqual(2, game.Players.Count);
        Assert.AreEqual(2, game.Players.First().Cards.Count());
        Assert.AreEqual(0, game.Players.First().Id);
        Assert.AreEqual(1, game.Players.Last().Id);
        Console.WriteLine(JsonSerializer.Serialize(game, new JsonSerializerOptions() { WriteIndented = true }));
    }

    [TestMethod]
    public void SerializeGame()
    {
        var game = new Game();
        game.AddPlayer(new Player("test", 1000, false));
        game.AddPlayer(new Player("test", 1000, false));
        //Console.WriteLine(JsonSerializer.Serialize(game, new JsonSerializerOptions() { WriteIndented = true }));
        var serialized = JsonSerializer.Serialize(game);
        var deserialized = JsonSerializer.Deserialize<Game>(serialized);
        Assert.AreEqual(game.Players.Count, deserialized?.Players.Count);
        Assert.AreEqual(game.Players.First().Cards.First().Value, deserialized?.Players.First().Cards.First().Value);
        Console.WriteLine(JsonSerializer.Serialize(deserialized, new JsonSerializerOptions() { WriteIndented = true }));
    }

    [TestMethod]
    public void SerializePlayer()
    {
        var player = new Player("test", 1000, true);
        var serialized = JsonSerializer.Serialize(player);
        var deserialized = JsonSerializer.Deserialize<Player>(serialized);
        Assert.AreEqual(deserialized?.Pot, 1000);
    }

    [TestMethod]
    public void BetRaiseCall()
    {
        var p1 = new Player("p1", 1000, false);
        var p2 = new Player("p2", 1000, false);
        var game = new Game();
        game.AddPlayer(p1);
        game.AddPlayer(p2);
        game.Round++; // start game

        game.Bet(p1, BetType.Bet, 200);
        Assert.AreEqual(Round.PreFlop, game.Round);
        Assert.AreEqual(200, game.Pot);
        Assert.AreEqual(200, game.LastBet);
        Assert.AreEqual(200, p1.LastBetAmount);
        Assert.AreEqual(800, p1.Pot);

        game.Bet(p2, BetType.Raise, 100);
        Assert.AreEqual(Round.PreFlop, game.Round);
        Assert.AreEqual(500, game.Pot);
        Assert.AreEqual(300, game.LastBet);
        Assert.AreEqual(300, p2.LastBetAmount);
        Assert.AreEqual(700, p2.Pot);

        game.Bet(p1, BetType.Call, 0);
        Assert.AreEqual(Round.Flop, game.Round);
        Assert.AreEqual(600, game.Pot);
        Assert.AreEqual(0, game.LastBet);
        Assert.AreEqual(0, p1.LastBetAmount);
        Assert.AreEqual(700, p1.Pot);

        Console.WriteLine(JsonSerializer.Serialize(game, new JsonSerializerOptions() { WriteIndented = true }));
    }

    [TestMethod]
    public void BetRaiseRaise()
    {
        var p1 = new Player("p1", 1000, false);
        var p2 = new Player("p2", 1000, false);
        var game = new Game();
        game.AddPlayer(p1);
        game.AddPlayer(p2);
        game.Round++; // start game

        game.Bet(p1, BetType.Bet, 200);
        Assert.AreEqual(Round.PreFlop, game.Round);
        Assert.AreEqual(200, game.Pot);
        Assert.AreEqual(200, game.LastBet);
        Assert.AreEqual(200, p1.LastBetAmount);
        Assert.AreEqual(800, p1.Pot);

        game.Bet(p2, BetType.Raise, 100);
        Assert.AreEqual(Round.PreFlop, game.Round);
        Assert.AreEqual(500, game.Pot);
        Assert.AreEqual(300, game.LastBet);
        Assert.AreEqual(300, p2.LastBetAmount);
        Assert.AreEqual(700, p2.Pot);

        var result = game.Bet(p1, BetType.Raise, 0);
        Assert.IsFalse(result);
        Assert.AreEqual(Round.PreFlop, game.Round);
        Assert.AreEqual(500, game.Pot);

        result = game.Bet(p1, BetType.Bet, 0);
        Assert.IsFalse(result);
        Assert.AreEqual(Round.PreFlop, game.Round);
        Assert.AreEqual(500, game.Pot);

        Console.WriteLine(JsonSerializer.Serialize(game, new JsonSerializerOptions() { WriteIndented = true }));
    }

    [TestMethod]
    public void BetRaiseRaiseCall()
    {
        var p1 = new Player("p1", 1000, false);
        var p2 = new Player("p2", 1000, false);
        var game = new Game();
        game.AddPlayer(p1);
        game.AddPlayer(p2);
        game.Round++; // start game

        game.Bet(p1, BetType.Bet, 200);
        Assert.AreEqual(Round.PreFlop, game.Round);
        Assert.AreEqual(200, game.Pot);
        Assert.AreEqual(200, game.LastBet);
        Assert.AreEqual(200, p1.LastBetAmount);
        Assert.AreEqual(800, p1.Pot);

        game.Bet(p2, BetType.Raise, 100);
        Assert.AreEqual(Round.PreFlop, game.Round);
        Assert.AreEqual(500, game.Pot);
        Assert.AreEqual(300, game.LastBet);
        Assert.AreEqual(300, p2.LastBetAmount);
        Assert.AreEqual(700, p2.Pot);

        game.Bet(p1, BetType.Raise, 500);
        Assert.AreEqual(Round.PreFlop, game.Round);
        Assert.AreEqual(1100, game.Pot);
        Assert.AreEqual(800, game.LastBet);
        Assert.AreEqual(800, p1.LastBetAmount);
        Assert.AreEqual(200, p1.Pot);

        game.Bet(p2, BetType.Call, 0);
        Assert.AreEqual(Round.Flop, game.Round);
        Assert.AreEqual(1600, game.Pot);
        Assert.AreEqual(0, game.LastBet);
        Assert.AreEqual(0, p1.LastBetAmount);
        Assert.AreEqual(0, p2.LastBetAmount);
        Assert.AreEqual(200, p1.Pot);
        Assert.AreEqual(200, p2.Pot);

        Console.WriteLine(JsonSerializer.Serialize(game, new JsonSerializerOptions() { WriteIndented = true }));
    }

    [TestMethod]
    public void BetRaiseRaiseCallMissingAmount()
    {
        var p1 = new Player("p1", 1000, false);
        var p2 = new Player("p2", 500, false);
        var game = new Game();
        game.AddPlayer(p1);
        game.AddPlayer(p2);
        game.Round++; // start game

        game.Bet(p1, BetType.Bet, 200);
        Assert.AreEqual(Round.PreFlop, game.Round);
        Assert.AreEqual(200, game.Pot);
        Assert.AreEqual(200, game.LastBet);
        Assert.AreEqual(200, p1.LastBetAmount);
        Assert.AreEqual(800, p1.Pot);

        game.Bet(p2, BetType.Raise, 100);
        Assert.AreEqual(Round.PreFlop, game.Round);
        Assert.AreEqual(500, game.Pot);
        Assert.AreEqual(300, game.LastBet);
        Assert.AreEqual(300, p2.LastBetAmount);
        Assert.AreEqual(200, p2.Pot);

        game.Bet(p1, BetType.Raise, 500);
        Assert.AreEqual(Round.PreFlop, game.Round);
        Assert.AreEqual(1100, game.Pot);
        Assert.AreEqual(800, game.LastBet);
        Assert.AreEqual(800, p1.LastBetAmount);
        Assert.AreEqual(200, p1.Pot);

        game.Bet(p2, BetType.Call, 0);
        Assert.AreEqual(Round.Ended, game.Round);
        Assert.AreEqual(0, game.Pot);
        Assert.AreEqual(0, game.LastBet);
        Assert.AreEqual(0, p1.LastBetAmount);
        Assert.AreEqual(0, p2.LastBetAmount);
        Assert.AreEqual(1800, p1.Pot);
        Assert.AreEqual(-300, p2.Pot);

        Console.WriteLine(JsonSerializer.Serialize(game, new JsonSerializerOptions() { WriteIndented = true }));
    }
}