using System.Text;

namespace DesignPatterns.Behavioral.Visitor;

public static class AcyclicVisitor
{
    public interface IVisitor<TVisitable>
    {
        void Visit(TVisitable obj);
    }

    public interface IVisitor { }

    public abstract class Expression
    {
        public virtual void Accept(IVisitor visitor)
        {
            if (visitor is IVisitor<Expression> typed)
                typed.Visit(this);
        }
    }

    public class DoubleExpression : Expression
    {
        public double Value { get; set; }

        public DoubleExpression(double value)
        {
            Value = value;
        }

        public override void Accept(IVisitor visitor)
        {
            if (visitor is IVisitor<DoubleExpression> typed)
                typed.Visit(this);
        }
    }

    public class AdditionalExpression : Expression
    {
        public Expression Left { get; set; }
        public Expression Right { get; set; }

        public AdditionalExpression(Expression left, Expression right)
        {
            Left = left;
            Right = right;
        }

        public override void Accept(IVisitor visitor)
        {
            if (visitor is IVisitor<AdditionalExpression> typed)
                typed.Visit(this);
        }
    }

    public class ExpressionPrinter : IVisitor,
        IVisitor<Expression>,
        IVisitor<DoubleExpression>,
        IVisitor<AdditionalExpression>
    {
        private StringBuilder sb = new();

        public void Visit(AdditionalExpression obj)
        {
            sb.Append("(");
            obj.Left.Accept(this);
            sb.Append("+");
            obj.Right.Accept(this);
            sb.Append(")");
        }

        public void Visit(Expression obj)
        {
        }

        public void Visit(DoubleExpression obj)
        {
            sb.Append(obj.Value);
        }

        public override string ToString()
        {
            return sb.ToString();
        }
    }

    public static void Render()
    {
        var e = new AdditionalExpression(
                    new DoubleExpression(1),
                    new AdditionalExpression(
                        new DoubleExpression(2),
                        new DoubleExpression(3)));

        var ev = new ExpressionPrinter();
        ev.Visit(e);
        Console.WriteLine(ev);
    }
}
