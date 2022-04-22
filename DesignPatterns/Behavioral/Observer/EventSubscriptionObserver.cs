using Autofac;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace DesignPatterns.Behavioral.Observer;

public static class EventSubscriptionObserver
{
    public interface IEvent { }

    public interface ISend<TEvent> where TEvent : IEvent
    {
        event EventHandler<TEvent> Sender;
    }

    public interface IHandle<TEvent> where TEvent : IEvent
    {
        void Handle(object sender, TEvent args);
    }

    public class ButtonProcessedEvent : IEvent
    {
        public int NumberOfClicks { get; set; }
    }

    public class Button : ISend<ButtonProcessedEvent>
    {
        public event EventHandler<ButtonProcessedEvent> Sender;

        public void Fire(int clicks)
        {
            Sender?.Invoke(this, new ButtonProcessedEvent { NumberOfClicks = clicks });
        }
    }

    public class Logging : IHandle<ButtonProcessedEvent>
    {
        public void Handle(object sender, ButtonProcessedEvent args)
        {
            Console.WriteLine($"Button clicked {args.NumberOfClicks} times");
        }
    }

    public static void Render()
    {
        var cb = new ContainerBuilder();
        var ass = Assembly.GetExecutingAssembly();

        cb.RegisterAssemblyTypes(ass)
            .AsClosedTypesOf(typeof(ISend<>))
            .SingleInstance();

        cb.RegisterAssemblyTypes(ass)
            .Where(t => t.GetInterfaces()
                .Any(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IHandle<>)
                    ))
            .OnActivated(act =>
            {
                var instanceType = act.Instance.GetType();
                var interfaces = instanceType.GetInterfaces();
                foreach (var i in interfaces)
                {
                    if (i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IHandle<>))
                    {
                        var arg0 = i.GetGenericArguments()[0];
                        var senderType = typeof(ISend<>).MakeGenericType(arg0);
                        var allSenderTypes = typeof(IEnumerable<>).MakeGenericType(senderType);
                        var allServices = act.Context.Resolve(allSenderTypes);
                        foreach (var service in (IEnumerable)allServices)
                        {
                            var eventInfo = service.GetType().GetEvent("Sender");
                            var handleMethod = instanceType.GetMethod("Handle");
                            var handler = Delegate.CreateDelegate(eventInfo.EventHandlerType, null, handleMethod);
                            eventInfo.AddEventHandler(service, handler);
                        }
                    }
                }
            })
            .SingleInstance()
            .AsSelf();

        var container = cb.Build();

        var btn = container.Resolve<Button>();
        var logging = container.Resolve<Logging>();

        btn.Fire(1);
        btn.Fire(2);
    }
}
