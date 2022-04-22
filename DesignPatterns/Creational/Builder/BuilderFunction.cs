using static DesignPatterns.Creational.Builder.BuilderFunction;

namespace DesignPatterns.Creational.Builder;

public static class PersonBuildExtensions
{
    public static PersonBuilder WorksAs(this PersonBuilder builder, string position) => builder.Do(p => p.Position = position);
}


public static class BuilderFunction
{
    public class Person
    {
        public string Name, Position;
        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}, {nameof(Position)}: {Position}";
        }
    }

    public abstract class FunctionBuilder<TSubject, TSelf>
        where TSelf : FunctionBuilder<TSubject, TSelf>
        where TSubject : new()
    {
        private readonly List<Func<TSubject, TSubject>> actions = new();

        public TSelf Do(Action<TSubject> action) => AddAction(action);

        public TSubject Build() => actions.Aggregate(new TSubject(), (p, f) => f(p));

        private TSelf AddAction(Action<TSubject> action)
        {
            actions.Add(p =>
            {
                action(p);
                return p;
            });
            return (TSelf)this;
        }
    }

    public sealed class PersonBuilder : FunctionBuilder<Person, PersonBuilder>
    {
        public PersonBuilder Called(string name) => Do(p => p.Name = name);
    }


    public static void Render()
    {
        var person = new PersonBuilder().Called("John").WorksAs("quant").Build();
        Console.WriteLine(person);
    }
}