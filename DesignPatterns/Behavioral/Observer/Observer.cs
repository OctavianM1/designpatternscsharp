namespace DesignPatterns.Behavioral.Observer;

public static class Observer
{
    public class FallsIllEventArgs
    {
        public string Address { get; set; }
    }

    public class Person
    {
        public async void CatchACold()
        {
            FallsIll?.Invoke(this, new FallsIllEventArgs { Address = "123 London" });

        }
        public event EventHandler<FallsIllEventArgs> FallsIll;
    }

    public static void Render()
    {
        var p = new Person();

        p.FallsIll += CallDoctor;

        p.CatchACold();

        p.FallsIll -= CallDoctor;
    }

    private static void CallDoctor(object? sender, FallsIllEventArgs e)
    {
        Console.WriteLine($"A doctor has been called to {e.Address}");
    }
}
