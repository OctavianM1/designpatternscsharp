namespace DesignPatterns.Creational.Prototype;

public static class PrototypeEx
{
    public class Person : ICloneable
    {
        public string[] Names { get; set; }
        public Address Address { get; set; }

        public Person(string[] names, Address address)
        {
            Names = names;
            Address = address;
        }

        public override string ToString()
        {
            return $"{{{nameof(Names)}={string.Join(' ', Names)}, {nameof(Address)}={Address}}}";
        }

        public object Clone()
        {
            return new Person(Names, Address);
        }
    }

    public class Address
    {
        public string StreetName { get; set; }
        public int HouseNumber { get; set; }

        public Address(string streetName, int houseNumber)
        {
            StreetName = streetName;
            HouseNumber = houseNumber;
        }

        public override string ToString()
        {
            return $"{{{nameof(StreetName)}={StreetName}, {nameof(HouseNumber)}={HouseNumber}}}";
        }
    }

    public static void Render()
    {
        var john = new Person(new[] { "John", "Smith" }, new Address("London Road", 123));

        var jane = john;
        jane.Names[0] = "Jane";

        var mary = (Person)john.Clone();
        mary.Names[0] = "Mary";
        mary.Address.HouseNumber = 444;


        Console.WriteLine(john);
        Console.WriteLine(jane);
        Console.WriteLine(mary);
    }
}
