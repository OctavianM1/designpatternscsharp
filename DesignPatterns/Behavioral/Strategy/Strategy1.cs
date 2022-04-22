namespace DesignPatterns.Behavioral.Strategy;

public static class Strategy1
{
    public class Person : IComparable<Person>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }

        public Person(int id, string name, int age)
        {
            Id = id;
            Name = name;
            Age = age;
        }

        public int CompareTo(Person? other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return Id.CompareTo(other.Id);
        }
    }

    private sealed class NameRelationalComparer : IComparer<Person>
    {
        public int Compare(Person? x, Person? y)
        {
            if (ReferenceEquals(x, y)) return 0;
            if (ReferenceEquals(x, null)) return -1;
            if (ReferenceEquals(y, null)) return 1;
            return string.Compare(x.Name, y.Name, StringComparison.Ordinal);
        }
    }

    public static IComparer<Person> NameComparer { get; } = new NameRelationalComparer();

    public static void Render()
    {
        var people = new List<Person>
        {
            new Person(5,"da",45),
            new Person(15,"z",45),
            new Person(2,"b",45),
            new Person(1,"c",45),
        };

        people.Sort(NameComparer);
        foreach (var p in people)
        {
            Console.WriteLine($"id - {p.Id}, name - {p.Name}, age - {p.Age}");
        }
        Console.WriteLine();
        people.Sort();
        foreach (var p in people)
        {
            Console.WriteLine($"id - {p.Id}, name - {p.Name}, age - {p.Age}");
        }
        Console.WriteLine();
        people.Sort((x, y) => x.Name.CompareTo(y.Name));
        foreach (var p in people)
        {
            Console.WriteLine($"id - {p.Id}, name - {p.Name}, age - {p.Age}");
        }
    }
}
