namespace DesignPatterns.Behavioral.ChainOfResponsability;

public static class COR1
{
    public class Creature
    {
        public string Name { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }

        public Creature(string name, int attack, int defense)
        {
            Name = name;
            Attack = attack;
            Defense = defense;
        }

        public override string ToString()
        {
            return $"{{{nameof(Name)}={Name}, {nameof(Attack)}={Attack.ToString()}, {nameof(Defense)}={Defense.ToString()}}}";
        }
    }

    public class CreatureModifier
    {
        protected readonly Creature _creature;
        protected CreatureModifier _next;

        public CreatureModifier(Creature creature)
        {
            _creature = creature;
        }

        public void Add(CreatureModifier cm)
        {
            if (_next != null) _next.Add(cm);
            else _next = cm;
        }

        public virtual void Handle() => _next?.Handle();
    }

    public class DoubleAttackModifier : CreatureModifier
    {
        public DoubleAttackModifier(Creature creature) : base(creature)
        {
        }

        public override void Handle()
        {
            Console.WriteLine($"Doubling {_creature.Name}'s attack");
            _creature.Attack *= 2;
            base.Handle();
        }
    }

    public class NoBonusesModifier : CreatureModifier
    {
        public NoBonusesModifier(Creature creature) : base(creature)
        {
        }

        public override void Handle()
        {

        }
    }

    public class IncreaseDefenseModifier : CreatureModifier
    {
        public IncreaseDefenseModifier(Creature creature) : base(creature)
        {
        }

        public override void Handle()
        {
            Console.WriteLine($"Increasing {_creature.Name}'s defense");
            _creature.Defense += 3;
            base.Handle();
        }
    }

    public static void Render()
    {
        var goblin = new Creature("Gobling", 2, 2);
        Console.WriteLine(goblin);

        var root = new CreatureModifier(goblin);

        root.Add(new NoBonusesModifier(goblin));
        root.Add(new DoubleAttackModifier(goblin));
        root.Add(new IncreaseDefenseModifier(goblin));
        root.Handle();
        Console.WriteLine(goblin);
    }
}
