namespace DesignPatterns.Creational.Factories;

public static class FactorySimple
{
    public class Point
    {
        private readonly double _x;
        private readonly double _y;

        // internal for assembly (libraries)
        private Point(double x, double y)
        {
            _x = x;
            _y = y;
        }

        public static Point Origin => new Point(0, 0);
        public static Point Origin2 = new Point(0, 0);

        public override string ToString()
        {
            return $"x: {_x}, y: {_y}";
        }
        public static class Factory
        {
            public static Point NewCartesianPoint(double x, double y)
            {
                return new Point(x, y);
            }

            public static Point NewPolarPoint(double rho, double theta)
            {
                return new Point(rho * Math.Cos(theta), rho * Math.Sin(theta));
            }
        }
    }
    public static void Render()
    {
        var point = Point.Factory.NewPolarPoint(1.0, Math.PI / 2);
        Console.WriteLine(point);

        //Task.Factory.StartNew
    }
}
