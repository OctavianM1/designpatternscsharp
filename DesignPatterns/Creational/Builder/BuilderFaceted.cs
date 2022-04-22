namespace DesignPatterns.Creational.Builder;

public static class BuilderFaceted
{
    public class Person
    {
        // address
        public string StreetAddress, Postcode, City;

        // employment
        public string CompanyName, Position;
        public int AnnualIncome;

        public override string ToString()
        {
            return StreetAddress + Postcode + City + CompanyName + Position + AnnualIncome;
        }

    }

    public class PersonBuilder // Facade
    {
        protected Person person = new();

        public PersonJobBuilder Works => new PersonJobBuilder(person);
        public PersonAddressBuilder Lives => new PersonAddressBuilder(person);

        public static implicit operator Person(PersonBuilder pb)
        {
            return pb.person;
        }

    }

    public class PersonAddressBuilder : PersonBuilder
    {
        public PersonAddressBuilder(Person person)
        {
            this.person = person;
        }

        public PersonAddressBuilder At(string address)
        {
            person.StreetAddress = address;
            return this;
        }

        public PersonAddressBuilder In(string city)
        {
            person.City = city;
            return this;
        }

        public PersonAddressBuilder WithPostCode(string postcode)
        {
            person.Postcode = postcode;
            return this;
        }

    }

    public class PersonJobBuilder : PersonBuilder
    {
        public PersonJobBuilder(Person person)
        {
            this.person = person;
        }

        public PersonJobBuilder At(string companyName)
        {
            person.CompanyName = companyName;
            return this;
        }

        public PersonJobBuilder AsA(string position)
        {
            person.Position = position;
            return this;
        }

        public PersonJobBuilder Earning(int amount)
        {
            person.AnnualIncome = amount;
            return this;
        }
    }

    public static void Render()
    {
        var pb = new PersonBuilder();
        Person person = pb
            .Lives
                .At("123 Londod Road")
                .In("London")
                .WithPostCode("123 432")
            .Works
                .At("Google")
                .AsA("Engineer")
                .Earning(10000);

        Console.WriteLine(person);
    }
}
