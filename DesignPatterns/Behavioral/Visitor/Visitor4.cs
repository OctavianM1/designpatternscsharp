using System.Text;
using static DesignPatterns.Behavioral.Visitor.Visitor4;

namespace DesignPatterns.Behavioral.Visitor;

using DictType = Dictionary<Type, Action<Expression, StringBuilder>>;

public static class Visitor4
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

    public static class ExpressionPrinter
    {
        private static DictType actions = new DictType
        {
            [typeof(DoubleExpression)] = (e, sb) =>
            {
                var de = (DoubleExpression)e;
                sb.Append(de.Value);
            },
            [typeof(AdditionalExpression)] = (e, sb) =>
            {
                var ade = (AdditionalExpression)e;
                sb.Append("(");
                Print(ade.Left, sb);
                sb.Append("+");
                Print(ade.Right, sb);
                sb.Append(")");
            }
        };

        public static void Print(Expression e, StringBuilder sb)
        {
            actions[e.GetType()](e, sb);
        }

        //public static void Print(Expression e, StringBuilder sb)
        //{
        //    if (e is DoubleExpression de)
        //    {
        //        sb.Append(de.Value);
        //    }
        //    else if (e is AdditionalExpression ade)
        //    {
        //        sb.Append("(");
        //        Print(ade.Left, sb);
        //        sb.Append("+");
        //        Print(ade.Right, sb);
        //        sb.Append(")");
        //    }
        //}
    }

    public static void Render()
    {
        var e = new AdditionalExpression(
            new DoubleExpression(1),
            new AdditionalExpression(
                new DoubleExpression(2),
                new DoubleExpression(3)));

        var sb = new StringBuilder();
        ExpressionPrinter.Print(e, sb);
        Console.WriteLine(sb);
    }
}
