using System.Text;

namespace DesignPatterns.Behavioral.Strategy;

public static class Strategy
{
    public enum OutputFormat
    {
        Markdown,
        Html
    }

    public interface IListStrategy
    {
        void Start(StringBuilder sb);
        void End(StringBuilder sb);
        void AddListItem(StringBuilder sb, string item);
    }

    public class HtmlListStrategy : IListStrategy
    {
        public void AddListItem(StringBuilder sb, string item)
        {
            sb.AppendLine($"\t<li>{item}</li>");
        }

        public void End(StringBuilder sb)
        {
            sb.AppendLine("</ul>");
        }

        public void Start(StringBuilder sb)
        {
            sb.AppendLine("<ul>");
        }
    }

    public class MarkdownListStrategy : IListStrategy
    {
        public void AddListItem(StringBuilder sb, string item)
        {
            sb.AppendLine($" * {item}");
        }

        public void End(StringBuilder sb)
        {
        }

        public void Start(StringBuilder sb)
        {
        }
    }

    public class TextProcessor
    {
        private StringBuilder sb = new();
        private IListStrategy listStrategy;

        public void SetOutputFormat(OutputFormat format)
        {
            switch (format)
            {
                case OutputFormat.Markdown:
                    listStrategy = new MarkdownListStrategy();
                    break;
                case OutputFormat.Html:
                    listStrategy = new HtmlListStrategy();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(format));
            }
        }

        public void AppendList(IEnumerable<string> items)
        {
            listStrategy.Start(sb);
            foreach (var item in items)
            {
                listStrategy.AddListItem(sb, item);
            }
            listStrategy.End(sb);
        }

        public override string ToString()
        {
            return sb.ToString();
        }

        public StringBuilder Clear() => sb.Clear();
    }


    public static void Render()
    {
        var tp = new TextProcessor();

        tp.SetOutputFormat(OutputFormat.Markdown);
        tp.AppendList(new[] { "foo", "bar", "baz" });
        Console.WriteLine(tp);

        tp.Clear();

        tp.SetOutputFormat(OutputFormat.Html);
        tp.AppendList(new[] { "foo", "bar", "baz" });
        Console.WriteLine(tp);
    }
}
