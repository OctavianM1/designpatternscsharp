using System.Text;

namespace DesignPatterns.Structural.Decorator;

public static class Decorator4
{
    public abstract class Shape
    {
        public abstract string AsString();
    }

    public class Circle : Shape
    {
        private float _radius;

        public Circle() : this(0)
        {

        }

        public Circle(float radius)
        {
            _radius = radius;
        }

        public override string AsString() => $"A circle with radius {_radius}";

        public void Resize(float factor)
        {
            _radius *= factor;
        }
    }

    public class Square : Shape
    {
        private float _side;

        public Square() : this(0)
        {

        }

        public Square(float side)
        {
            _side = side;
        }

        public override string AsString() => $"A square with side {_side}";
    }

    public class ColoredShape : Shape
    {
        private readonly Shape _shape;
        private readonly string _color;

        public ColoredShape(Shape shape, string color)
        {
            _shape = shape;
            _color = color;
        }

        public override string AsString() => $"{_shape.AsString()} has the color {_color}";
    }

    public class ColoredShape<T> : Shape where T : Shape, new()
    {
        private readonly string _color;
        private readonly T _shape = new T();

        public ColoredShape() : this("black")
        {
            
        }

        public ColoredShape(string color)
        {
            _color = color;
        }

        public override string AsString() => $"{_shape.AsString()} has the color {_color}";
    }

    public class TransparentShape<T> : Shape where T : Shape, new()
    {
        private readonly float _transparency;
        private readonly T _shape = new T();

        public TransparentShape() : this(0)
        {

        }

        public TransparentShape(float transparency)
        {
            _transparency = transparency;
        }

        public override string AsString() => $"{_shape.AsString()} has the transparancy {_transparency * 100}%";
    }

    public static void Render()
    {
        var redSquare = new ColoredShape<Square>();
        Console.WriteLine(redSquare.AsString());

        var circle = new TransparentShape<ColoredShape<Circle>>();
        Console.WriteLine(circle.AsString() );
    }
}
