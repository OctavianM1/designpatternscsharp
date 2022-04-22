namespace DesignPatterns.Behavioral.Observer;

public static class WeakEventObserver
{
    public class Button
    {
        public event EventHandler Clicked;

        public void Fire()
        {
            Clicked?.Invoke(this, EventArgs.Empty);
        }
    }

    public class Window
    {
        private readonly Button _button;

        public Window(Button button)
        {
            _button = button;
            _button.Clicked += OnClickButton;
        }

        private void OnClickButton(object? sender, EventArgs e)
        {
            Console.WriteLine("Button clicked (window handler)");
        }

        ~Window()
        {
            Console.WriteLine("Window finalized");
        }
    }

    public class Test
    {
        public int MyProperty { get; set; }
        ~Test()
        {
            Console.WriteLine("Finalizing Test");
        }

        public override string ToString()
        {
            return "TEST _ TO _ STRING";
        }
    }

    public static void Render()
    {
        MakeGarbage();
        FireGC();
    }

    private static void FireGC()
    {
        Console.WriteLine("Starting GC");
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        Console.WriteLine("GC is done!");
    }

    private static void MakeGarbage()
    {
        var btn = new Button();
        var window = new Window(btn);
        btn.Fire();
        Console.WriteLine("Setting window to null");
        window = null;
    }
}
