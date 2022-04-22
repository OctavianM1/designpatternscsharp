using System.Text;

namespace DesignPatterns.Behavioral.State;

public static class State4
{
    public enum Chest
    {
        Open,
        Closed,
        Locked
    }

    public enum Action
    {
        Open,
        Close
    }

    public static Chest Manipulate(Chest chest, Action action, bool haveKey) =>
        (chest, action, haveKey) switch
        {
            (Chest.Locked, Action.Open, true) => Chest.Open,
            (Chest.Closed, Action.Open, _) => Chest.Open,
            (Chest.Open, Action.Close, true) => Chest.Locked,
            (Chest.Open, Action.Close, false) => Chest.Closed,

            _ => chest
        };

    public static void Render()
    {
        var chest = Chest.Locked;
        Console.WriteLine($"Chest is {chest}");

        chest = Manipulate(chest, Action.Open, true);
        Console.WriteLine($"Chest is {chest}");

        chest = Manipulate(chest, Action.Close, false);
        Console.WriteLine($"Chest is {chest}");

        chest = Manipulate(chest, Action.Close, false);
        Console.WriteLine($"Chest is {chest}");
    }
}
