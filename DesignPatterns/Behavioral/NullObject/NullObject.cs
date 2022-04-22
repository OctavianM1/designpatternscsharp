using Autofac;
using ImpromptuInterface;
using System.Dynamic;

namespace DesignPatterns.Behavioral.NullObject;

public static class NullObject
{
    public interface ILog
    {
        void Info(string msg);
        void Warn(string msg);
    }

    public class ConsoleLog : ILog
    {
        public void Info(string msg)
        {
            Console.WriteLine(msg);
        }

        public void Warn(string msg)
        {
            Console.WriteLine($"WARNING - {msg}");
        }
    }

    public class BankAccount
    {
        private readonly ILog _log;
        private int _balance;

        public BankAccount(ILog log)
        {
            _log = log;
        }

        public void Deposit(int amount)
        {
            _balance += amount;
            _log.Info($"Deposited {amount}, balance is now {_balance}");
        }
    }

    public class NullLog : ILog
    {
        public void Info(string msg)
        {
        }

        public void Warn(string msg)
        {
        }
    }

    public class Null<TInterface> : DynamicObject where TInterface : class
    {
        public static TInterface Instance
        {
            get
            {
                return new Null<TInterface>().ActLike<TInterface>();
            }
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object?[]? args, out object? result)
        {
            result = Activator.CreateInstance(binder.ReturnType);
            return true;
        }
    }

    public static void Render()
    {
        //var cb = new ContainerBuilder();
        //cb.RegisterType<BankAccount>();
        //cb.RegisterType<NullLog>().As<ILog>();
        //using var c = cb.Build();
        //var ba = c.Resolve<BankAccount>();
        //ba.Deposit(10);

        var log = Null<ILog>.Instance;
        var ba = new BankAccount(log);
        ba.Deposit(100);
    }
}
