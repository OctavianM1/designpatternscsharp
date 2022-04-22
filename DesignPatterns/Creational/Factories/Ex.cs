namespace DesignPatterns.Creational.Factories;

public static class Ex
{
    enum CoordinateSystem
    {
        Cartisian,
        Polar
    }
    class Point
    {
        private double x, y;

        /// <summary>
        /// Initializes a point from either cartisian or polar
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="system"></param>
        public Point(double a, double b, CoordinateSystem system = CoordinateSystem.Cartisian)
        {
            switch (system)
            {
                case CoordinateSystem.Cartisian:
                    x = a;
                    y = b;
                    break;
                case CoordinateSystem.Polar:
                    x = a * Math.Cos(b);
                    y = a * Math.Sin(b);
                    break;
                default:
                    break;
            }
        }
    }

    public static void Render()
    {

    }
}
