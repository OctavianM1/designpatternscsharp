namespace DesignPatterns.Behavioral.Momento;

public static class Memento
{
    public class MomentoToken
    {
        public int Balance { get; }

        public MomentoToken(int balance)
        {
            Balance = balance;
        }
    }

    public class BankAccount
    {
        private int balance;
        private readonly List<MomentoToken> _changes = new();
        private int _current;

        public BankAccount(int balance)
        {
            this.balance = balance;
            _changes.Add(new MomentoToken(balance));
        }

        public MomentoToken Deposit(int amount)
        {
            balance += amount;
            var m = new MomentoToken(balance);
            _changes.Add(m);
            _current++;
            return m;
        }

        public MomentoToken Restore(MomentoToken m)
        {
            if (m != null)
            {
                balance = m.Balance;
                _changes.Add(m);
            }
            return null;
        }

        public MomentoToken Undo()
        {
            if (_current > 0)
            {
                var m = _changes[--_current];
                balance = m.Balance;
                return m;
            }
            return null;
        }

        public MomentoToken Redo()
        {
            if (_current + 1 < _changes.Count)
            {
                var m = _changes[++_current];
                balance = m.Balance;
                return m;
            }
            return null;
        }

        public override string ToString()
        {
            return $"Balance :{balance}";
        }
    }

    public static void Render()
    {
        var ba = new BankAccount(100);
        var m1 = ba.Deposit(50);
        var m2 = ba.Deposit(25);
        Console.WriteLine(ba);
        ba.Undo();
        Console.WriteLine($"Undo 1: {ba}");
        ba.Undo();
        Console.WriteLine($"Undo 2: {ba}");
        ba.Redo();
        Console.WriteLine($"Redo: {ba}");
    }
}
