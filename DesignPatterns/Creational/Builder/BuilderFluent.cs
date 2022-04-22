namespace DesignPatterns.Creational.Builder;

public static class BuilderFluent
{
    public class Person
    {
        public string Name { get; set; }
        public string Position { get; set; }

        public class Builder : PersonJobBuilder<Builder>
        {

        }

        public static Builder New => new Builder();

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}, {nameof(Position)} position";
        }
    }

    public class PersonInfoBuilder<SELF> : PersonBuilder where SELF : PersonInfoBuilder<SELF>
    {
        public SELF Called(string name)
        {
            person.Name = name;
            return (SELF)this;
        }
    }

    public class PersonJobBuilder<SELF> : PersonInfoBuilder<PersonJobBuilder<SELF>> where SELF : PersonJobBuilder<SELF>
    {
        public SELF WorksAsA(string postion)
        {
            person.Position = postion;
            return (SELF)this;
        }
    }

    public abstract class PersonBuilder
    {
        protected Person person = new();
        public Person Build()
        {
            return person;
        }
    }

    public static void Render()
    {
        var p = Person.New.Called("John").WorksAsA("quant").Build();
        Console.WriteLine(p);
    }
}
