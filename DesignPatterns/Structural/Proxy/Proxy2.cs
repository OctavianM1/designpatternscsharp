using System.Diagnostics;
using static DesignPatterns.Structural.Proxy.Proxy2;

namespace DesignPatterns.Structural.Proxy;

public static class PercentageExtensions
{
    public static Percentage Percent(this int value)
    {
        return new Percentage(value / 100.0f);
    }

    public static Percentage Percent(this float value)
    {
        return new Percentage(value / 100.0f);
    }
}

public static class Proxy2
{
    [DebuggerDisplay("{value*100.0f}%")]
    public struct Percentage : IEquatable<Percentage>
    {
        private readonly float _value;

        public Percentage(float value)
        {
            _value = value;
        }

        public static float operator*(float f, Percentage p)
        {
            return f * p._value;
        }

        public static Percentage operator*(Percentage a, Percentage b)
        {
            return new Percentage(a._value * b._value);
        }

        public static Percentage operator+(Percentage a, Percentage b)
        {
            return new Percentage(a._value + b._value);
        }

        public static bool operator ==(Percentage left, Percentage right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Percentage left, Percentage right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            return $"{_value * 100}%";
        }

        public override bool Equals(object? obj)
        {
            return obj is Percentage percentage && Equals(percentage);
        }

        public bool Equals(Percentage other)
        {
            return _value == other._value;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_value);
        }
    }

    public static void Render()
    {
        Console.WriteLine(
            10f * 5.Percent()
            );

        Console.WriteLine(
            2.Percent() + 3.Percent() // 5%
            );

        Console.WriteLine(
           2.Percent() * 3.Percent()
           );
    }
}
