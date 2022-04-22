namespace DesignPatterns.Behavioral.State;

public static class State1
{
    public enum State
    {
        OffHook,
        Connecting,
        Connected,
        OnHold
    }

    public enum Trigger
    {
        CallDialed,
        HungUp,
        CallConnected,
        PlacedOnHold,
        TakenOffHold,
        LeftMessage
    }

    private static Dictionary<State, List<(Trigger, State)>> _rules = new()
    {
        [State.OffHook] = new()
        {
            (Trigger.CallDialed, State.Connecting)
        },
        [State.Connecting] = new()
        {
            (Trigger.HungUp, State.OffHook),
            (Trigger.CallConnected, State.Connected)
        },
        [State.Connected] = new()
        {
            (Trigger.LeftMessage, State.OffHook),
            (Trigger.HungUp, State.OffHook),
            (Trigger.PlacedOnHold, State.OnHold)
        },
        [State.OnHold] = new()
        {
            (Trigger.TakenOffHold, State.Connected),
            (Trigger.HungUp, State.OffHook)
        }
    };

    public static void Render()
    {
        var state = State.OffHook;
        while (true)
        {
            Console.WriteLine($"The phone is currently {state}");
            Console.WriteLine("Select a trigger");

            for (int i = 0; i < _rules[state].Count; i++)
            {
                var (t, _) = _rules[state][i];
                Console.WriteLine($"{i}. {t}");

            }
            int input = int.Parse(Console.ReadLine());

            var (_, s) = _rules[state][input];
            state = s;
        }
    }
}
