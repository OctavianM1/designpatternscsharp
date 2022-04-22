namespace DesignPatterns.Additional;

public static class CQRS
{
    public class Person
    {
        private int age;
        public EventBroker Broker { get; set; }
        public bool CanVode => age >= 16;

        public Person(EventBroker broker)
        {
            Broker = broker;
            Broker.Commands += BrokerOnCommands;
            Broker.Queries += BrokerOnQueries;
        }

        private void BrokerOnQueries(object? sender, Query e)
        {
            var ac = e as AgeQuery;
            if (ac != null && ac.Target == this)
            {
                ac.Result = age;
            }
        }

        private void BrokerOnCommands(object? sender, Command c)
        {
            var cac = c as ChangeAgeCommand;
            if (cac != null && cac.Target == this)
            {
                if (cac.Registred) Broker.AllEvents.Add(new AgeChangedEvent(this, age, cac.Age));
                age = cac.Age;
            }
        }
    }

    public class EventBroker
    {
        // 1. All events that happened.
        public IList<Event> AllEvents = new List<Event>();
        // 2. Commands
        public event EventHandler<Command> Commands;
        // 3. Query
        public event EventHandler<Query> Queries;

        public void Command(Command c)
        {
            Commands?.Invoke(this, c);
        }

        public T Query<T>(Query q)
        {
            Queries?.Invoke(this, q);
            return (T)q.Result;
        }

        public void UndoLast()
        {
            var e = AllEvents.LastOrDefault();
            if (e != null && e is AgeChangedEvent ac)
            {
                Command(new ChangeAgeCommand(ac.Target, ac.OldValue) { Registred = false });
                AllEvents.Remove(e);
            }
        }

    }

    public class Command
    {
        public bool Registred { get; set; } = true;
    }

    public class ChangeAgeCommand : Command
    {
        public Person Target { get; set; }
        public int Age { get; set; }

        public ChangeAgeCommand(Person target, int age)
        {
            Target = target;
            Age = age;
        }
    }

    public class Query
    {
        public object Result { get; set; }
    }

    public class AgeQuery : Query
    {
        public Person Target { get; set; }

        public AgeQuery(Person target)
        {
            Target = target;
        }
    }

    public class Event
    {
        // backtrack
    }

    public class AgeChangedEvent : Event
    {
        public Person Target { get; set; }
        public int OldValue { get; set; }
        public int NewValue { get; set; }

        public AgeChangedEvent(Person target, int oldValue, int newValue)
        {
            Target = target;
            OldValue = oldValue;
            NewValue = newValue;
        }

        public override string ToString()
        {
            return $"Age changed from {OldValue} to {NewValue}";
        }
    }

    public static void Render()
    {
        var eb = new EventBroker();
        var p = new Person(eb);

        eb.Command(new ChangeAgeCommand(p, 123));

        foreach (var ev in eb.AllEvents)
        {
            Console.WriteLine(ev);
        }

        int age = eb.Query<int>(new AgeQuery(p));

        Console.WriteLine(age);

        eb.UndoLast();

        foreach (var ev in eb.AllEvents)
        {
            Console.WriteLine(ev);
        }

        age = eb.Query<int>(new AgeQuery(p));
        Console.WriteLine(age);
    }
}

