using System.Collections;
using System.Collections.ObjectModel;

namespace DesignPatterns.Structural.Adapter;

internal static class Adapter
{
    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object? obj)
        {
            return obj is Point point &&
                   X == point.X &&
                   Y == point.Y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }

    public class Line
    {
        public Point Start { get; set; }
        public Point End { get; set; }

        public Line(Point start, Point end)
        {
            Start = start;
            End = end;
        }

        public override bool Equals(object? obj)
        {
            return obj is Line line &&
                   EqualityComparer<Point>.Default.Equals(Start, line.Start) &&
                   EqualityComparer<Point>.Default.Equals(End, line.End);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Start, End);
        }
    }

    public class VectorObject : Collection<Line>
    {

    }

    public class VectorRectangle : VectorObject
    {
        public VectorRectangle(int x, int y, int width, int height)
        {
            Add(new Line(new Point(x, y), new Point(x + width, y)));
            Add(new Line(new Point(x + width, y), new Point(x + width, y + height)));
            Add(new Line(new Point(x, y), new Point(x, y + height)));
            Add(new Line(new Point(x, y + height), new Point(x + width, y + height)));
        }
    }

    public class LineToPointAdapter : IEnumerable<Point>
    {
        private static int count;

        private static Dictionary<int, List<Point>> _cache = new();

        public LineToPointAdapter(Line line)
        {
            var hashCode = line.GetHashCode();
            if (_cache.ContainsKey(hashCode))
            {
                Console.WriteLine("Line Cached");
                return;
            }
            var points = new List<Point>();
            Console.WriteLine($"{++count}: Generating points for line [{line.Start.X},{line.Start.Y}]-[{line.End.X},{line.End.Y}]");

            int left = Math.Min(line.Start.X, line.End.X);
            int right = Math.Max(line.Start.X, line.End.X);
            int top = Math.Max(line.Start.Y, line.End.Y);
            int bottom = Math.Min(line.Start.Y, line.End.Y);

            int dx = right - left;
            int dy = top - bottom;

            if (dx == 0)
            {
                for (int i = bottom; i <= top; i++)
                {
                    var point = new Point(left, i);
                    points.Add(point);
                }
            }
            else if (dy == 0)
            {
                for (int i = left; i <= right; i++)
                {
                    var point = new Point(i, top);
                    points.Add(point);
                }
            }
            Console.WriteLine($"Hashcode: {hashCode}");
            _cache.Add(hashCode, points);
        }

        public IEnumerator<Point> GetEnumerator()
        {
            return _cache.Values.SelectMany(x => x).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerable<Point> GetPointsForLine(Line line)
        {
            var hashCode = line.GetHashCode();
            return _cache[hashCode];
        }
    }

    public static void DrawPoint(Point p)
    {
        Console.Write(".");
    }

    private static readonly List<VectorObject> _vectorObjects = new List<VectorObject>
        {
            new VectorRectangle(1, 1, 10, 10),
            new VectorRectangle(3, 3, 6, 6)
        };

    public static void Draw()
    {
        foreach (var vo in _vectorObjects)
        {
            foreach (var line in vo)
            {
                var adapter = new LineToPointAdapter(line);
                foreach (var point in adapter.GetPointsForLine(line))
                {
                    Console.WriteLine($"Point: X:{point.X} - Y: {point.Y}");
                    DrawPoint(point);
                }
            }
        }
    }

    public static void Render()
    {
        Draw();
        Draw();
    }
}
