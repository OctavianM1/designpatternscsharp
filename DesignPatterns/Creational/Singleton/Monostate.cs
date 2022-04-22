namespace DesignPatterns.Creational.Singleton;

public static class Monostate
{
    public class CEO
    {
        private static string name;
        private static int age;

        public string Name { get => name; set => name = value; }
        public int Age { get => age; set => age = value; }

        public override string ToString()
        {
            return $"{{{nameof(Name)}={Name}, {nameof(Age)}={Age}}}";
        }
    }

    public static void Render()
    {
        var ceo = new CEO();
        ceo.Name = "Adam Smith";
        var ceo1 = new CEO();
        ceo1.Name = "Other";
        Console.WriteLine(ceo);
        Console.WriteLine(ceo1);
    }
}
