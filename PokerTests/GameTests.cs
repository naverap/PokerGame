using System.Text.Json;

namespace PokerTests;

[TestClass]
public class GameTests
{
    [TestMethod]
    public void CreateGame()
    {
        var game = Game.CreateGame();
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
        var game = Game.CreateGame();
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
}