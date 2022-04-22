namespace DesignPatterns.Creational.Prototype;

public static class PrototypeCopyConstructor
{
    public class Person
    {
        public string[] Names { get; set; }
        public Address Address { get; set; }

        public Person(string[] names, Address address)
        {
            Names = names;
            Address = address;
        }

        // C++ way
        public Person(Person other)
        {
            Names = other.Names.Select(x => x).ToArray();
            Address = new Address(other.Address);
        }

        public override string ToString()
        {
            return $"{{{nameof(Names)}={string.Join(' ', Names)}, {nameof(Address)}={Address}}}";
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

        public Address(Address other)
        {
            StreetName = other.StreetName;
            HouseNumber = other.HouseNumber;
        }

        public override string ToString()
        {
            return $"{{{nameof(StreetName)}={StreetName}, {nameof(HouseNumber)}={HouseNumber}}}";
        }
    }

    public static void Render()
    {
        var john = new Person(new[] { "John", "Smith" }, new Address("London Road", 123));

        var jane = new Person(john);
        jane.Address.HouseNumber = 555;
        jane.Names[0] = "Jane";

        Console.WriteLine(john);
        Console.WriteLine(jane);
    }
}
