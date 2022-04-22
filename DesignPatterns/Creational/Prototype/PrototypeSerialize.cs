using System.Text.Json;
using System.Xml.Serialization;

namespace DesignPatterns.Creational.Prototype.Serialize;

public static class ExtensionMethods
{
    public static T DeepCopy1<T>(this T self)
    {
        using var ms = new MemoryStream();
        JsonSerializer.Serialize(ms, self);
        ms.Position = 0;
        var copy = JsonSerializer.Deserialize<T>(ms);
        return copy;
        // todo
    }
    public static T DeepCopy<T>(this T self)
    {
        var stream = new MemoryStream();
        JsonSerializer.Serialize(stream, self);
        stream.Seek(0, SeekOrigin.Begin);
        var copy = JsonSerializer.Deserialize<T>(stream);
        stream.Close();
        return copy;
    }

    public static T DeepCopyXml<T>(this T self)
    {
        using var ms = new MemoryStream();
        var s = new XmlSerializer(typeof(T));
        s.Serialize(ms, self);
        ms.Position = 0;
        var copy = s.Deserialize(ms);
        return (T)copy;
    }
}

public static class PrototypeSerialize
{
    public class Person
    {
        public string[] Names { get; set; }
        public Address Address { get; set; }

        public Person()
        {

        }

        public Person(string[] names, Address address)
        {
            Names = names;
            Address = address;
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

        public Address()
        {

        }

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

        var mary = john.DeepCopyXml();
        mary.Names[0] = "Mary";
        mary.Address.HouseNumber = 444;


        Console.WriteLine(john);
        Console.WriteLine(jane);
        Console.WriteLine(mary);
    }
}
