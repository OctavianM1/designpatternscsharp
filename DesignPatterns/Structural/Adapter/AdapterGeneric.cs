namespace DesignPatterns.Structural.Adapter;

internal static class AdapterGeneric
{
    // Vector2f, Vector3i

    public interface IInteger
    {
        int Value { get; }
    }

    public static class Dimensions
    {
        public class Two : IInteger
        {
            public int Value => 2;
        }

        public class Three : IInteger
        {
            public int Value => 3;
        }
    }


    public class Vector<TSelf, T, D> 
        where D : IInteger, new()
        where TSelf : Vector<TSelf, T, D>, new()
    {
        protected T[] data;

        public Vector()
        {
            data = new T[new D().Value];
        }

        public Vector(params T[] values)
        {
            var requiredSize = new D().Value;
            data = new T[requiredSize];

            var providedSize = values.Length;

            for (int i = 0; i < Math.Min(requiredSize, providedSize); i++)
            {
                data[i] = values[i];
            }
        }

        public static TSelf Create(params T[] values)
        {
            var result = new TSelf();
            var requiredSize = new D().Value;
            result.data = new T[requiredSize];

            var providedSize = values.Length;

            for (int i = 0; i < Math.Min(requiredSize, providedSize); i++)
            {
                result.data[i] = values[i];
            }

            return result;
        }

        public T this[int index]
        {
            get => data[index];
            set => data[index] = value;
        }

        public T X
        {
            get => data[0];
            set => data[0] = value;
        }
    }

    public class VectorOfInt<TSelf, D> : Vector<VectorOfInt<TSelf, D>, int, D> where D : IInteger, new()
    {
        public VectorOfInt()
        {
        }
        public VectorOfInt(params int[] values) : base(values)
        {
        }

        public static VectorOfInt<TSelf, D> operator +(VectorOfInt<TSelf, D> lhs, VectorOfInt<TSelf, D> rhs)
        {
            var result = new VectorOfInt<TSelf, D>();
            var dim = new D().Value;
            for (int i = 0; i < dim; i++)
            {
                result[i] = lhs[i] + rhs[i];
            }
            return result;
        }
    }

    public class Vector2i : VectorOfInt<Vector2i, Dimensions.Two>
    {
        public Vector2i(params int[] values) : base(values)
        {
        }
    }

    public class Vector3f : VectorOfFloat<Vector3f, Dimensions.Three>
    {
        public override string ToString()
        {
            return $"{string.Join(',', data)}";
        }
    }

    public class VectorOfFloat<TSelf, D> : Vector<TSelf, float, D> 
        where D : IInteger, new()
        where TSelf : Vector<TSelf, float, D>, new()
    {
        
    }

    public static void Render()
    {
        var v = new Vector2i(1, 2);
        v[0] = 0;

        var vv = new Vector2i(3, 2);

        var result = v + vv;

        var u = Vector3f.Create(2.3f, 6.5f, 1);
    }
}
