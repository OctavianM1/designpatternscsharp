using System.Text;

namespace DesignPatterns.Structural.Composite;

internal static class GeometricalShapes
{
    public class GraphicObject
    {
        public string Color { get; set; }
        public virtual string Name { get; set; } = "Group";

        private Lazy<List<GraphicObject>> children = new();
        public List<GraphicObject> Children => children.Value;

        private void Print(StringBuilder sb, int depth)
        {
            sb.Append('*', depth)
                .Append(string.IsNullOrWhiteSpace(Color) ? string.Empty : $"{Color} ")
                .AppendLine(Name);

            Children.ForEach(x => x.Print(sb, depth + 1));
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            Print(sb, 0);
            return sb.ToString();
        }
    }

    public class Circle : GraphicObject
    {
        public override string Name { get => "Circle"; set => base.Name = value; }
    }

    public class Square : GraphicObject
    {
        public override string Name { get => "Square"; set => base.Name = value; }
    }

    public static void Render()
    {
        var drawing = new GraphicObject { Name = "My Drawing" };
        drawing.Children.Add(new Square { Color = "Red" });
        drawing.Children.Add(new Circle { Color = "Blue" });

        var group = new GraphicObject();
        group.Children.Add(new Circle { Color = "Yellow" });
        group.Children.Add(new Square { Color = "Yellow" });

        drawing.Children.Add(group);
        Console.WriteLine(drawing);
    }
}
