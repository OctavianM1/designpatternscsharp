using System.Text;

namespace DesignPatterns.Creational.Builder;

public static class Builder
{
    public class HtmlElement
    {
        public string Name, Text;
        public List<HtmlElement> Elements { get; set; } = new();
        private const int indentSize = 2;

        public HtmlElement()
        {

        }

        public HtmlElement(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public HtmlElement(string name, string text)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Text = text ?? throw new ArgumentNullException(nameof(text));
        }

        private string ToStringImple(int indent)
        {
            var sb = new StringBuilder();
            var i = new string(' ', indentSize * indent);
            sb.AppendLine($"{i}<{Name}>");
            if (!string.IsNullOrWhiteSpace(Text))
            {
                sb.Append(new string(' ', indentSize * (indent + 1)));
                sb.AppendLine(Text);
            }

            foreach (var e in Elements)
            {
                sb.Append(e.ToStringImple(indent + 1));
            }

            sb.AppendLine($"{i}</{Name}>");

            return sb.ToString();
        }

        public override string ToString()
        {
            return ToStringImple(0);
        }
    }

    public class HtmlBuilder
    {
        private readonly string _rootName;
        HtmlElement root = new();

        public HtmlBuilder(string rootName)
        {
            root.Name = rootName;
            _rootName = rootName;
        }

        private HtmlBuilder Add(HtmlElement e)
        {
            root.Elements.Add(e);
            return this;
        }

        public HtmlBuilder AddChild(string childName, string childText)
        {
            var e = new HtmlElement(childName, childText);
            return Add(e);
        }

        public HtmlBuilder AddChild(HtmlElement child) => Add(child);

        public HtmlBuilder AddChild(HtmlBuilder child) => Add(child.root);

        public override string ToString()
        {
            return root.ToString();
        }

        public void Clear()
        {
            root = new HtmlElement();
            root.Name = _rootName;
        }
    }

    public static void Render()
    {
        var hello = "Hello";
        var sb = new StringBuilder();
        sb.Append("<p>");
        sb.Append(hello);
        sb.Append("</p>");
        Console.WriteLine(sb);

        sb.Clear();
        var words = new string[] { "hello", "world" };
        sb.Append("<ul>");
        foreach (var word in words)
        {
            sb.AppendFormat("<li>{0}</li>", word);
        }
        sb.Append("</ul>");
        Console.WriteLine(sb);

        var div = new HtmlElement("div");
        var span = new HtmlElement("span");
        var p = new HtmlElement("p", "HELLO");
        //p.Elements.Add(div);
        //p.Elements.Add(span);
        //Console.WriteLine(p);

        var builder = new HtmlBuilder("ul");
        builder.AddChild("li", "hello").AddChild("li", "world");

        var builder1 = new HtmlBuilder("div");
        builder1.AddChild(builder);

        var builder2 = new HtmlBuilder("div");
        builder2.AddChild(builder1);

        var builder3 = new HtmlBuilder("div");
        builder3.AddChild(builder2);
        Console.WriteLine(builder3);
    }
}
