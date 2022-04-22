using Autofac;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace DesignPatterns.Behavioral.MediatorCustom;

public static class EventBrokerMediator
{
    public class Actor
    {
        protected EventBroker _broker;

        public Actor(EventBroker broker)
        {
            _broker = broker;
        }
    }

    public class FootballPlayer : Actor
    {
        public string Name { get; set; }
        public int GoalsScore { get; set; } = 0;

        public FootballPlayer(EventBroker broker, string name) : base(broker)
        {
            Name = name;

            broker.OfType<PlayerScoreEvent>()
                .Where(ps => !ps.Name.Equals(Name))
                .Subscribe(pe => Console.WriteLine($"{name}: Nicely done, {pe.Name}! It's your {pe.GoalsScored}!"));

            broker.OfType<PlayerSentOffEvent>()
                .Where(ps => !ps.Name.Equals(Name))
                .Subscribe(pe => Console.WriteLine($"{name}: see you in the lockers, {pe.Name}"));
        }

        public void Score()
        {
            GoalsScore++;
            _broker.Publish(new PlayerScoreEvent { Name = Name, GoalsScored = GoalsScore });
        }

        public void Assault()
        {
            _broker.Publish(new PlayerSentOffEvent { Name = Name, Reason = "violence" });
        }
    }

    public class FootballCoach : Actor
    {
        public FootballCoach(EventBroker broker) : base(broker)
        {
            broker.OfType<PlayerScoreEvent>()
                .Subscribe(pe =>
                {
                    if (pe.GoalsScored < 3)
                        Console.WriteLine($"Coach: well, done {pe.Name}");
                });

            broker.OfType<PlayerSentOffEvent>()
                .Subscribe(pe =>
                {
                    if (pe.Reason == "violence")
                        Console.WriteLine($"Coach: how could you, {pe.Name}");
                });
        }
    }

    public class PlayerEvent
    {
        public string Name { get; set; }
    }

    public class PlayerScoreEvent : PlayerEvent
    {
        public int GoalsScored { get; set; }
    }

    public class PlayerSentOffEvent : PlayerEvent
    {
        public string Reason { get; set; }
    }

    public class EventBroker : IObservable<PlayerEvent>
    {
        private readonly Subject<PlayerEvent> _subscriptions = new();
        public IDisposable Subscribe(IObserver<PlayerEvent> observer)
        {
            return _subscriptions.Subscribe(observer);
        }

        public void Publish(PlayerEvent pe)
        {
            _subscriptions.OnNext(pe);
        }
    }

    public static void Render()
    {
        var cb = new ContainerBuilder();
        cb.RegisterType<EventBroker>().SingleInstance();
        cb.RegisterType<FootballCoach>();
        cb.Register((c, p) => new FootballPlayer(c.Resolve<EventBroker>(), p.Named<string>("name")));

        using var c = cb.Build();
        var coach = c.Resolve<FootballCoach>();
        var player1 = c.Resolve<FootballPlayer>(new NamedParameter("name", "John"));
        var player2 = c.Resolve<FootballPlayer>(new NamedParameter("name", "Chris"));

        player1.Score();
        player1.Score();
        player1.Score();
        player1.Assault();
        player2.Score();
    }
}
