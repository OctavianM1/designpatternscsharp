using System.Collections;

namespace DesignPatterns.Structural.Proxy;

public static class CompositeProxy
{
    public class Creature
    {
        public byte Age { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class Creatures
    {
        private readonly int _size;
        private byte[] age;
        private int[] X, Y;

        public Creatures(int size)
        {
            _size = size;
            age = new byte[size];
            X = new int[size];
            Y = new int[size];
        }

        public struct CreatureProxy
        {
            private readonly Creatures _creatures;
            private readonly int _index;

            public CreatureProxy(Creatures creatures, int index)
            {
                _creatures = creatures;
                _index = index;
            }

            public ref byte Age => ref _creatures.age[_index];
            public ref int X => ref _creatures.X[_index];
            public ref int Y => ref _creatures.Y[_index];
        }

        public IEnumerator<CreatureProxy> GetEnumerator()
        {
            for (int i = 0; i < _size; i++)
            {
                yield return new CreatureProxy(this, i);
            }
        }
    }

    public static void Render()
    {
        var creatures = new Creature[100];


        foreach (var c in creatures)
        {
            c.X++;
        }

        var creatures2 = new Creatures(100);
        foreach (var c in creatures2)
        {
            c.X++;
        }
    }
}
