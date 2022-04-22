using System.Text;

namespace DesignPatterns.Structural.Decorator;

public static class Decorator
{
    public class MyStringBuilder
    {
        private readonly StringBuilder _sb = new();

        public static implicit operator MyStringBuilder(string s)
        {
            var msb = new MyStringBuilder();
            msb._sb.Append(s);
            return msb;
        }

        public static MyStringBuilder operator +(MyStringBuilder msb, string s)
        {
            msb._sb.Append(s);
            return msb;
        }

        public override string ToString()
        {
            return _sb.ToString();
        }

    }

    public static void Render()
    {
        MyStringBuilder s = "hello ";
        s += "world";
        Console.WriteLine(s);
    }
}
