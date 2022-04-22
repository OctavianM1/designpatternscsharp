using System.Numerics;

namespace DesignPatterns.Additional;

public static class ContinuationPassyingStyle
{
    public enum WorkflowResult
    {
        Success, Failure
    }
    // i^0 == 1, i^1 == i, i^2 == -1, i^3 == -i, i^4 == 1
    public class QuadraticEquationSolver
    {
        public WorkflowResult Start(double a, double b, double c, out Tuple<Complex, Complex> result)
        {
            var disc = b * b - 4 * a * c;
            if (disc < 0)
            {
                result = null;
                return WorkflowResult.Failure;
                //return SolveComplex(a, b, c, disc);
            }
            return SolveSimple(a, b, disc, out result);
        }

        private WorkflowResult SolveSimple(double a, double b, double disc, out Tuple<Complex, Complex> result)
        {
            var root = Math.Sqrt(disc);
            result = Tuple.Create(
                new Complex(((-b + root) / 2 * a), 0),
                new Complex(((-b - root) / 2 * a), 0)
                );
            return WorkflowResult.Success;
        }

        private Tuple<Complex, Complex> SolveComplex(double a, double b, double c, double disc)
        {
            var root = Complex.Sqrt(new Complex(disc, 0));
            return Tuple.Create(
                ((-b + root) / 2 * a),
                ((-b - root) / 2 * a)
                );
        }
    }

    public class ContinuationPassingStyleDemo
    {
        static void Main(string[] args)
        {
            var solver = new QuadraticEquationSolver();
            var flag = solver.Start(1, 10, 16, out var solution);
            if (flag == WorkflowResult.Success)
            {
                Console.WriteLine($"[{solution.Item1}, {solution.Item2}]");
            }
        }
    }
}
