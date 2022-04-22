namespace DesignPatterns.Structural.Decorator;

public static class Decorator1
{
    public interface IFly
    {
        void Fly();
        int Weight { get; set; }
    }
    public interface ICrawl
    {
        void Crawl();
        int Weight { get; set; }
    }
    public class Bird : IFly
    {
        public int Weight { get; set; }
        public void Fly()
        {
            Console.WriteLine($"Soaring in the sky with weight {Weight}");
        }
    }

    public class Lizard : ICrawl
    {
        public int Weight { get; set; }
        public void Crawl()
        {
            Console.WriteLine($"Crawling in the dirt with weight {Weight}");
        }
    }

    public class Dragon : IFly, ICrawl
    {
        private readonly IFly _fly = new Bird();
        private readonly ICrawl _crawl = new Lizard();
        private int _weight;

        public Dragon()
        {
        }

        public void Crawl() => _crawl.Crawl();

        public void Fly() => _fly.Fly();

        public int Weight
        {
            get => _weight;
            set
            {
                _weight = value;
                _fly.Weight = _weight;
                _crawl.Weight = _weight;
            }
        }
    }

    public static void Render()
    {
        var d = new Dragon();
        d.Weight = 12;
        d.Fly();
        d.Crawl();
    }
}
