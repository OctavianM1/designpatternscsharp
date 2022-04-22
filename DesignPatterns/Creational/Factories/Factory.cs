namespace DesignPatterns.Creational.Factories;

public static class Factory
{
    enum CoordinateSystem
    {
        Cartisian,
        Polar
    }

    public class Point
    {
        private readonly double _x;
        private readonly double _y;

        private Point(double x, double y)
        {
            _x = x;
            _y = y;
        }

        // Factory method
        public static Point NewCartesianPoint(double x, double y)
        {
            return new Point(x, y);
        }

        public static Point NewPolarPoint(double rho, double theta)
        {
            return new Point(rho * Math.Cos(theta), rho * Math.Sin(theta));
        }

        public override string ToString()
        {
            return $"x: {_x}, y: {_y}";
        }
    }

    public static void Render()
    {
        var point = Point.NewPolarPoint(1.0, Math.PI / 2);
        Console.WriteLine(point);
    }
}
