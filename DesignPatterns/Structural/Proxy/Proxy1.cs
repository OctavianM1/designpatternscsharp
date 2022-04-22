namespace DesignPatterns.Structural.Proxy;

public static class Proxy1
{
    public class Property<T> where T : new()
    {
        private T _value;
        public T Value
        {
            get => _value;
            set
            {
                if (Equals(_value, value))
                {
                    return;
                }
                Console.WriteLine($"Assigning value to {value}");
                _value = value;
            }

        }

        public Property() : this(Activator.CreateInstance<T>())
        {

        }

        public Property(T value)
        {
            _value = value;
        }

        public static implicit operator T(Property<T> property)
        {
            return property.Value;
        }

        public static implicit operator Property<T>(T value)
        {
            return new Property<T>(value);
        }
    }

    public class Creature
    {
        private Property<int> agility = new();

        public int Agility
        {
            get => agility.Value;
            set => agility.Value = value;
        }

    }

    public static void Render()
    {
        var c = new Creature();
        c.Agility = 10;
    }
}
