namespace DesignPatterns.SOLID;

public static class L
{
    public class Rectangle
    {
        public virtual int Height { get; set; }
        public virtual int Width { get; set; }

        public Rectangle()
        {

        }
        public Rectangle(int height, int width)
        {
            Height = height;
            Width = width;
        }

        public override string ToString()
        {
            return $"{nameof(Width)}: {Width}, {nameof(Height)}: {Height}";
        } 
    }

    public class Square : Rectangle
    {
        public new int Width
        {
            set
            {
                base.Width = base.Height = value;
            }
        }

        public new int Height
        {
            set
            {
                base.Height = base.Width = value;
            }
        }
    }

    public static int Area(Rectangle r) => r.Height * r.Width;
    public static void Render()
    {
        var rc = new Rectangle(2, 3);
        Rectangle s = new Square();
        s.Width = 5;
        Console.WriteLine(Area(rc));
        Console.WriteLine(Area(s));

    }
}
