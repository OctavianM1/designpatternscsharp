using System.Text;

namespace DesignPatterns.Structural.Decorator;

public static class Decorator3
{
    public interface IShape
    {
        string AsString();
    }

    public class Circle : IShape
    {
        private float _radius;

        public Circle(float radius)
        {
            _radius = radius;
        }

        public string AsString() => $"A circle with radius {_radius}";

        public void Resize(float factor)
        {
            _radius *= factor;
        }
    }

    public class Square : IShape
    {
        private float _side;

        public Square(float side)
        {
            _side = side;
        }

        public string AsString() => $"A square with side {_side}";
    }

    //public class ColoredShape : IShape
    //{
    //    private readonly IShape _shape;
    //    private readonly string _color;

    //    public ColoredShape(IShape shape, string color)
    //    {
    //        _shape = shape;
    //        _color = color;
    //    }

    //    public string AsString() => $"{_shape.AsString()} has the color {_color}";
    //}

    public class TransparentShape : IShape
    {
        private readonly IShape _shape;
        private readonly float _transparency;

        public TransparentShape(IShape shape, float transparency)
        {
            _shape = shape;
            _transparency = transparency;
        }

        public string AsString() => $"{_shape.AsString()} has the transparency {_transparency * 100}%";
    }

    public abstract class ShapeDecoratorCyclePolicy
    {
        public abstract bool TypeAdditionAllowed(Type type, IList<Type> allTypes);
        public abstract bool ApplicationAllowed(Type type, IList<Type> allTypes);
    }

    public class ThrowOnCyclePolicy : ShapeDecoratorCyclePolicy
    {
        private bool Handler(Type type, IList<Type> allTypes)
        {
            if (allTypes.Contains(type))
            {
                throw new InvalidOperationException($"Cycle detected! Type is already a {type.FullName}");
            }
            return true;
        }

        public override bool ApplicationAllowed(Type type, IList<Type> allTypes)
        {
            return Handler(type, allTypes);
        }

        public override bool TypeAdditionAllowed(Type type, IList<Type> allTypes)
        {
            return Handler(type, allTypes);
        }
    }

    public abstract class ShapeDecorator : IShape
    {
        protected internal readonly List<Type> _types = new();
        protected internal readonly IShape _shape;

        public ShapeDecorator(IShape shape)
        {
            _shape = shape;
            if (shape is ShapeDecorator sd)
                _types.AddRange(sd._types);
        }

        public virtual string AsString() => _shape.AsString();
    }

    public abstract class ShapeDecorator<TSelf, TCyclePolicy> : ShapeDecorator
        where TCyclePolicy : ShapeDecoratorCyclePolicy, new()
    {
        protected readonly TCyclePolicy _policy = new();

        protected ShapeDecorator(IShape shape) : base(shape)
        {
            if (_policy.TypeAdditionAllowed(typeof(TSelf), _types))
            {
                _types.Add(typeof(TSelf));
            }
        }
    }

    public class ShapeDecoratorWithPolicy<T> : ShapeDecorator<T, AbsorbCyclePolicy>
    {
        public ShapeDecoratorWithPolicy(IShape shape) : base(shape)
        {
        }
    }

    public class ColoredShape
        : ShapeDecorator<ColoredShape, AbsorbCyclePolicy>
    {

        private readonly string color;

        public ColoredShape(IShape shape, string color) : base(shape)
        {
            this.color = color;
        }

        public override string AsString()
        {
            var sb = new StringBuilder($"{_shape.AsString()}");

            if (_policy.ApplicationAllowed(_types[0], _types.Skip(1).ToList()))
                sb.Append($" has the color {color}");

            return sb.ToString();
        }
    }

    public class CyclesAllowedPolicy : ShapeDecoratorCyclePolicy
    {
        public override bool ApplicationAllowed(Type type, IList<Type> allTypes) => true;

        public override bool TypeAdditionAllowed(Type type, IList<Type> allTypes) => true;
    }

    public class AbsorbCyclePolicy : ShapeDecoratorCyclePolicy
    {
        public override bool ApplicationAllowed(Type type, IList<Type> allTypes) => !allTypes.Contains(type);

        public override bool TypeAdditionAllowed(Type type, IList<Type> allTypes) => true;
    }

    public static void Render()
    {
        var square = new Square(3f);

        var colored1 = new ColoredShape(square, "red");

        var colored2 = new ColoredShape(colored1, "blue");
        Console.WriteLine(colored2.AsString());
    }
}
