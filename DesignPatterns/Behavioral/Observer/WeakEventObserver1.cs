using System.Reactive.Linq;

namespace DesignPatterns.Behavioral.Observer;

public static class WeakEventObserver1
{
    public class Event
    {

    }

    public class FallsIllEvent : Event
    {
        public string Address { get; set; }

    }

    public class Person : IObservable<Event>
    {
        private readonly HashSet<Subscription> _subscriptions = new();
        public IDisposable Subscribe(IObserver<Event> observer)
        {
            var sub = new Subscription(this, observer);
            _subscriptions.Add(sub);
            return sub;
        }

        public void FallIll()
        {
            foreach (var sub in _subscriptions)
            {
                sub.Observer.OnNext(new FallsIllEvent { Address = "123 London" });
            }
        }

        private class Subscription : IDisposable
        {
            private readonly Person _p;
            public IObserver<Event> Observer { get; }

            public Subscription(Person p, IObserver<Event> observer)
            {
                _p = p;
                Observer = observer;
            }

            public void Dispose()
            {
                _p._subscriptions.Remove(this);
            }
        }
    }

    public class Obs : IObserver<Event>
    {
        public void Subscribe<T>(T obj) where T : IObservable<Event>
        {
            obj.Subscribe(this);
        }
        public void OnCompleted() { }

        public void OnError(Exception error) { }

        public void OnNext(Event value)
        {
            if (value is FallsIllEvent args)
            {
                Console.WriteLine($"A doctor is required at {args.Address}");
            }
        }
    }

    public static void Render()
    {
        var obs = new Obs();
        var p1 = new Person();
        var p2 = new Person();

        obs.Subscribe(p1);
        obs.Subscribe(p2);

        p1.FallIll();
    }
}
