using System.Text;

namespace DesignPatterns.Behavioral.Visitor;

public static class Visitor
{
    
    public abstract class Expression
    {
        public abstract void Print(StringBuilder sb);
    }

    public class DoubleExpression : Expression
    {
        private double _value;

        public DoubleExpression(double value)
        {
            _value = value;
        }

        public override void Print(StringBuilder sb)
        {
            sb.Append(_value);
        }
    }

    public class AdditionalExpression : Expression
    {
        private Expression left, right;

        public AdditionalExpression(Expression left, Expression right)
        {
            this.left = left;
            this.right = right;
        }

        public override void Print(StringBuilder sb)
        {
            sb.Append("(");
            left.Print(sb);
            sb.Append("+");
            right.Print(sb);
            sb.Append(")");
        }
    }

    public static void Render()
    {
        var e = new AdditionalExpression(
            new DoubleExpression(1),
            new AdditionalExpression(
                new DoubleExpression(2),
                new DoubleExpression(3)));

        var sb = new StringBuilder();
        e.Print(sb);
        Console.WriteLine(sb);
    }
}
