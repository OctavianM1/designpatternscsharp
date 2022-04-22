using static DesignPatterns.Creational.Prototype.PrototypeInheritance;
namespace DesignPatterns.Creational.Prototype;

public static class ExtensionMethods
{
    public static T DeepCopy<T>(this IDeepCopyable<T> item) where T : new()
    {
        return item.DeepCopy();
    }

    public static T DeepCopy<T>(this T person) where T : Person, new()
    {
        return ((IDeepCopyable<T>)person).DeepCopy();
    }
}

public static class PrototypeInheritance
{
    public interface IDeepCopyable<T> where T : new()
    {
        void CopyTo(T target);
        public T DeepCopy()
        {
            T t = new T();
            CopyTo(t);
            return t;
        }
    }

    public class Address : IDeepCopyable<Address>
    {
        public string StreetName { get; set; }
        public int HouseNumber { get; set; }

        public Address(string streetName, int houseNumber)
        {
            StreetName = streetName;
            HouseNumber = houseNumber;
        }

        public Address()
        {

        }

        public override string ToString()
        {
            return $"{{{nameof(StreetName)}={StreetName}, {nameof(HouseNumber)}={HouseNumber}}}";
        }

        public void CopyTo(Address target)
        {
            target.StreetName = StreetName;
            target.HouseNumber = HouseNumber;
        }
    }

    public class Person : IDeepCopyable<Person>
    {
        public string[] Names { get; set; }
        public Address Address { get; set; }

        public Person(string[] names, Address address)
        {
            Names = names;
            Address = address;
        }

        public Person()
        {

        }

        public override string ToString()
        {
            return $"{{{nameof(Names)}={string.Join(',', Names)}, {nameof(Address)}={Address}}}";
        }

        public void CopyTo(Person target)
        {
            target.Names = (string[])Names.Clone();
            target.Address = Address.DeepCopy();
        }
    }

    public class Employee : Person, IDeepCopyable<Employee>
    {
        public int Salary { get; set; }

        public Employee(string[] names, Address address, int salary) : base(names, address)
        {
            Salary = salary;
        }

        public Employee()
        {

        }

        public override string ToString()
        {
            return $"{{{nameof(Salary)}={Salary}, {base.ToString()}}}";
        }

        public void CopyTo(Employee target)
        {
            base.CopyTo(target);
            target.Salary = Salary;
        }
    }

    public static void Render()
    {
        var john = new Employee();
        john.Names = new[] { "John", "Smith" };
        john.Address = new Address
        {
            HouseNumber = 123,
            StreetName = "London Road"
        };
        john.Salary = 10000;

        var e = john.DeepCopy();
        var p = john.DeepCopy<Person>();

        e.Names[1] = "Doe";
        e.Salary = 999;
        e.Address.HouseNumber = 555;

        Console.WriteLine(john);
        Console.WriteLine(e);
        Console.WriteLine(p);
    }
}
