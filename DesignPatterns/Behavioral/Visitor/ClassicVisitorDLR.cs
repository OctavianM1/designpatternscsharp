using System.Text;

namespace DesignPatterns.Behavioral.Visitor;

public static class ClassicVisitorDLR
{

    public abstract class Expression
    {
    }

    public class DoubleExpression : Expression
    {
        public double Value;

        public DoubleExpression(double value)
        {
            Value = value;
        }

    }

    public class AdditionalExpression : Expression
    {
        public Expression Left, Right;

        public AdditionalExpression(Expression left, Expression right)
        {
            Left = left;
            Right = right;
        }

    }

    public class ExpressionPrinter
    {
        public void Print(AdditionalExpression ade, StringBuilder sb)
        {
            sb.Append("(");
            Print((dynamic)ade.Left, sb);
            sb.Append("+");
            Print((dynamic)ade.Right, sb);
            sb.Append(")");
        }

        public void Print(DoubleExpression de, StringBuilder sb)
        {
            sb.Append(de.Value);
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
        var ep = new ExpressionPrinter();
        ep.Print(e, sb);
        Console.WriteLine(sb);
    }
}
