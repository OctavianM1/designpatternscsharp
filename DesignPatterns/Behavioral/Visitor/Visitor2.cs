namespace DesignPatterns.Behavioral.Visitor;

public static class Visitor2
{
    public abstract class Expression
    {
        public abstract T Reduce<T>(ITransformer<T> transformer);
    }

    public class DoubleExpression : Expression
    {
        public readonly double Value;

        public DoubleExpression(double value)
        {
            Value = value;
        }

        public override T Reduce<T>(ITransformer<T> transformer)
        {
            return transformer.Transform(this);
        }
    }

    public class AdditionalExpression : Expression
    {
        public readonly Expression Left, Right;

        public AdditionalExpression(Expression left, Expression right)
        {
            Left = left;
            Right = right;
        }

        public override T Reduce<T>(ITransformer<T> transformer) => transformer.Transform(this);
    }

    public interface ITransformer<out T>
    {
        T Transform(DoubleExpression de);
        T Transform(AdditionalExpression ade);
    }

    public class EvaluationTransformer : ITransformer<double>
    {
        public double Transform(DoubleExpression de) => de.Value;

        public double Transform(AdditionalExpression ade) => ade.Left.Reduce(this) + ade.Right.Reduce(this);
    }

    public class PrintTransformer : ITransformer<string>
    {
        public string Transform(DoubleExpression de) => de.Value.ToString();

        public string Transform(AdditionalExpression ade) => $"({ade.Left.Reduce(this)} + {ade.Right.Reduce(this)})";
    }

    public class SquaredTransformer : ITransformer<Expression>
    {
        public Expression Transform(DoubleExpression de)
        {
            return new DoubleExpression(de.Value * de.Value);
        }

        public Expression Transform(AdditionalExpression ade)
        {
            return new AdditionalExpression(ade.Left.Reduce(this), ade.Right.Reduce(this));
        }
    }

    public static void Render()
    {
        var expr = new AdditionalExpression(new DoubleExpression(1), new DoubleExpression(2));
        var ev = new EvaluationTransformer();
        var res = ev.Transform(expr);
        var pt = new PrintTransformer();
        var text = pt.Transform(expr);

        Console.WriteLine($"{text} = {res}");

        var st = new SquaredTransformer();
        var newExpr = expr.Reduce(st);
        var text1 = newExpr.Reduce(pt);
        var res1 = newExpr.Reduce(ev);
        Console.WriteLine($"{text1} = {res1}");
    }
}
