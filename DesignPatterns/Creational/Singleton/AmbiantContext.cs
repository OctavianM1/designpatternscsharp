using System.Text;

namespace DesignPatterns.Creational.Singleton;

internal static class AmbiantContext
{
    public class BuildingContext : IDisposable
    {
        public int WallHeight { get; set; }
        private static Stack<BuildingContext> stack = new();

        static BuildingContext()
        {
            stack.Push(new BuildingContext(0));
        }

        public BuildingContext(int wallHeight)
        {
            WallHeight = wallHeight;
            stack.Push(this);
        }

        public static BuildingContext Current => stack.Peek();

        public void Dispose()
        {
            if (stack.Count > 1)
            {
                stack.Pop();
            }
        }
    }
    public class Building
    {
        public List<Wall> Walls { get; set; } = new();

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var wall in Walls)
            {
                sb.AppendLine(wall.ToString());
            }
            return sb.ToString();
        }
    }

    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"{{{nameof(X)}={X.ToString()}, {nameof(Y)}={Y.ToString()}}}";
        }
    }

    public class Wall
    {
        public Point Start { get; set; }
        public Point End { get; set; }
        public int Height { get; set; }

        public Wall(Point start, Point end)
        {
            Start = start;
            End = end;
            Height = BuildingContext.Current.WallHeight;
        }

        public override string ToString()
        {
            return $"{{{nameof(Start)}={Start}, {nameof(End)}={End}, {nameof(Height)}={Height.ToString()}}}";
        }
    }

    public static void Render()
    {
        var t1 = Task.Run(() =>
        {
            var house = new Building();
            using (new BuildingContext(3000))
            {
                // gnd 3000
                house.Walls.Add(new Wall(new Point(0, 0), new Point(5000, 0)));
                house.Walls.Add(new Wall(new Point(0, 0), new Point(0, 4000)));

                // 1st 3500
                using (new BuildingContext(3500))
                {
                    house.Walls.Add(new Wall(new Point(0, 0), new Point(6000, 0)));
                    house.Walls.Add(new Wall(new Point(0, 0), new Point(0, 4000)));
                }

                // gnd 3000
                house.Walls.Add(new Wall(new Point(5000, 0), new Point(5000, 4000)));
            }

            Console.WriteLine($"h1: {house}");
        });

        var t2 = Task.Run(() =>
        {
            var house = new Building();
            using (new BuildingContext(3))
            {
                // gnd 3
                house.Walls.Add(new Wall(new Point(0, 0), new Point(5000, 0)));
                house.Walls.Add(new Wall(new Point(0, 0), new Point(0, 4000)));

                // 1st 4
                using (new BuildingContext(4))
                {
                    house.Walls.Add(new Wall(new Point(0, 0), new Point(6000, 0)));
                    house.Walls.Add(new Wall(new Point(0, 0), new Point(0, 4000)));
                }

                // gnd 3
                house.Walls.Add(new Wall(new Point(5000, 0), new Point(5000, 4000)));
            }
            Console.WriteLine($"h2: {house}");
        });

        var t3 = Task.Run(() =>
        {
            var house = new Building();
            using (new BuildingContext(1))
            {
                // gnd 1
                house.Walls.Add(new Wall(new Point(0, 0), new Point(5000, 0)));
                house.Walls.Add(new Wall(new Point(0, 0), new Point(0, 4000)));

                // 1st 2
                using (new BuildingContext(2))
                {
                    house.Walls.Add(new Wall(new Point(0, 0), new Point(6000, 0)));
                    house.Walls.Add(new Wall(new Point(0, 0), new Point(0, 4000)));
                }

                // gnd 1
                house.Walls.Add(new Wall(new Point(5000, 0), new Point(5000, 4000)));
            }
            Console.WriteLine($"h3: {house}");
        });

        Task.WaitAll(t1, t2, t3);
    }
}
