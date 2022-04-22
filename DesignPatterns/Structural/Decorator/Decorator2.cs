namespace DesignPatterns.Structural.Decorator;

public static class Decorator2
{
    public interface ICreature
    {
        int Age { get; set; }
    }

    public interface IBird : ICreature
    {
        void Fly()
        {
            if (Age >= 10)
            {
                Console.WriteLine("I'm flying");
            }
        }
    }

    public interface ILizard : ICreature
    {
        void Crawl()
        {
            if (Age < 10)
            {
                Console.WriteLine("I'm crawling");
            }
        }
    }

    public class Organism { }

    public class Dragon : Organism, IBird, ILizard
    {
        public int Age { get; set; }

    }

    public static void Render()
    {
        var d = new Dragon { Age = 5 };

        if (d is IBird bird)
            bird.Fly();

        if (d is ILizard lizard)
            lizard.Crawl();

    }
}
