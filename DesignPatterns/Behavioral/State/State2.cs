using System.Text;

namespace DesignPatterns.Behavioral.State;

public static class State2
{
    public enum State
    {
        Locked,
        Failed,
        Unlocked
    }
    public static void Render()
    {
        string code = "1234";
        var state = State.Locked;
        var entry = new StringBuilder();

        while(true)
        {
            switch (state)
            {
                case State.Locked:
                    entry.Append(Console.ReadKey().KeyChar);

                    if (entry.ToString() == code)
                    {
                        state = State.Unlocked;
                        break;
                    }

                    if (!code.StartsWith(entry.ToString()))
                    {
                        state = State.Failed;
                    }

                    break;
                case State.Failed:
                    Console.CursorLeft = 0;
                    Console.WriteLine("FAILED");
                    entry.Clear();
                    state = State.Locked;
                    break;
                case State.Unlocked:
                    Console.CursorLeft = 0;
                    Console.WriteLine("UNLOCKED");
                    return;
            }
        }
    }
}
