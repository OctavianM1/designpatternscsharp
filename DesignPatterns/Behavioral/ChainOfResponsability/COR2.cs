namespace DesignPatterns.Behavioral.ChainOfResponsability;

public static class COR2
{
    // mediator
    public class Game
    {
        public event EventHandler<Query> Queries;

        public void PerformQuery(object sender, Query q)
        {
            Queries?.Invoke(sender, q);
        }
    }

    public class Query
    {
        public string Name { get; set; }
        public enum Argument
        {
            Attack,
            Defense
        }

        public Argument WhatToQuery { get; set; }
        public int Value { get; set; }

        public Query(string name, Argument whatToQuery, int value)
        {
            Name = name;
            WhatToQuery = whatToQuery;
            Value = value;
        }
    }

    public class Creature
    {
        private readonly Game _game;
        public string Name { get; set; }
        private readonly int _attack, _defense;

        public Creature(Game game, string name, int attack, int defense)
        {
            _game = game;
            Name = name;
            _attack = attack;
            _defense = defense;
        }

        public int Attack
        {
            get
            {
                var q = new Query(Name, Query.Argument.Attack, _attack);
                _game.PerformQuery(this, q);
                return q.Value;
            }
        }
        public int Defense
        {
            get
            {
                var q = new Query(Name, Query.Argument.Defense, _defense);
                _game.PerformQuery(this, q);
                return q.Value;
            }
        }

        public override string ToString()
        {
            return $"{{{nameof(Name)}={Name}, {nameof(Attack)}={Attack.ToString()}, {nameof(Defense)}={Defense.ToString()}}}";
        }
    }

    public abstract class CreatureModifier : IDisposable
    {
        protected Game _game;
        protected Creature _creature;

        protected CreatureModifier(Game game, Creature creature)
        {
            _game = game;
            _creature = creature;
            _game.Queries += Handle;
        }

        protected abstract void Handle(object sender, Query q);

        public void Dispose()
        {
            _game.Queries -= Handle;
        }
    }

    public class DoubleAttackModifier : CreatureModifier
    {
        public DoubleAttackModifier(Game game, Creature creature) : base(game, creature)
        {
        }

        protected override void Handle(object sender, Query q)
        {
            if (q.Name == _creature.Name && q.WhatToQuery == Query.Argument.Attack)
            {
                q.Value *= 2;
            }
        }
    }

    public class IncreaseDefenseModifier : CreatureModifier
    {
        public IncreaseDefenseModifier(Game game, Creature creature) : base(game, creature)
        {
        }

        protected override void Handle(object sender, Query q)
        {
            if (q.Name == _creature.Name && q.WhatToQuery == Query.Argument.Defense)
            {
                q.Value += 2;
            }
        }
    }

    public static void Render()
    {
        var game = new Game();
        var goblin = new Creature(game, "Strong goblin", 3, 3);
        Console.WriteLine(goblin);
        using (new DoubleAttackModifier(game, goblin))
        {
            Console.WriteLine(goblin);
            Console.WriteLine(goblin);
            Console.WriteLine(goblin);
            Console.WriteLine(goblin);
            Console.WriteLine(goblin);
            Console.WriteLine(goblin);
            Console.WriteLine(goblin);
            Console.WriteLine(goblin);

        }
        Console.WriteLine(goblin);
    }
}
