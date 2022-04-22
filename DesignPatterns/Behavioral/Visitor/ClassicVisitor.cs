using System.Text;

namespace DesignPatterns.Behavioral.Visitor;

public static class ClassicVisitor
{
    public interface IExpressionVisitor
    {
        void Visit(DoubleExpression de);
        void Visit(AdditionalExpression ade);
    }

    public abstract class Expression
    {
        public abstract void Accept(IExpressionVisitor visitor);
    }

    public class DoubleExpression : Expression
    {
        public double Value;

        public DoubleExpression(double value)
        {
            Value = value;
        }

        public override void Accept(IExpressionVisitor visitor)
        {
            // double dispatch
            visitor.Visit(this);
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

        public override void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class ExpressionPrinter : IExpressionVisitor
    {
        private StringBuilder sb = new();
        public void Visit(DoubleExpression de)
        {
            sb.Append(de.Value);
        }

        public void Visit(AdditionalExpression ade)
        {
            sb.Append("(");
            ade.Left.Accept(this);
            sb.Append("+");
            ade.Right.Accept(this);
            sb.Append(")");
        }

        public override string ToString()
        {
            return sb.ToString();
        }
    }

    public class ExpressionCalculator : IExpressionVisitor
    {
        public double Result;
        public void Visit(DoubleExpression de)
        {
            Result = de.Value;
        }

        public void Visit(AdditionalExpression ade)
        {
            ade.Left.Accept(this);
            var a = Result;
            ade.Right.Accept(this);
            var b = Result;
            Result = a + b;
        }
    }

    //public static class ExpressionPrinter
    //{
    //    private static DictType actions = new DictType
    //    {
    //        [typeof(DoubleExpression)] = (e, sb) =>
    //        {
    //            var de = (DoubleExpression)e;
    //            sb.Append(de.Value);
    //        },
    //        [typeof(AdditionalExpression)] = (e, sb) =>
    //        {
    //            var ade = (AdditionalExpression)e;
    //            sb.Append("(");
    //            Print(ade.Left, sb);
    //            sb.Append("+");
    //            Print(ade.Right, sb);
    //            sb.Append(")");
    //        }
    //    };

    //    public static void Print(Expression e, StringBuilder sb)
    //    {
    //        actions[e.GetType()](e, sb);
    //    }

    //    //public static void Print(Expression e, StringBuilder sb)
    //    //{
    //    //    if (e is DoubleExpression de)
    //    //    {
    //    //        sb.Append(de.Value);
    //    //    }
    //    //    else if (e is AdditionalExpression ade)
    //    //    {
    //    //        sb.Append("(");
    //    //        Print(ade.Left, sb);
    //    //        sb.Append("+");
    //    //        Print(ade.Right, sb);
    //    //        sb.Append(")");
    //    //    }
    //    //}
    //}

    public static void Render()
    {
        var e = new AdditionalExpression(
            new DoubleExpression(1),
            new AdditionalExpression(
                new DoubleExpression(2),
                new DoubleExpression(3)));

        var ep = new ExpressionPrinter();
        ep.Visit(e);

        Console.WriteLine(ep);

        var calc = new ExpressionCalculator();
        calc.Visit(e);
        Console.WriteLine(calc.Result);
    }
}
