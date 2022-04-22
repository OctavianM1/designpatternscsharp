using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;

namespace DesignPatterns.Structural.Proxy;

public static class Proxy4
{
    public enum Op : byte
    {
        [Description("*")]
        Mul = 0,
        [Description("/")]
        Div = 1,
        [Description("+")]
        Add = 2,
        [Description("-")]
        Sub = 3
    }



    public class TwoBitSet
    {
        private readonly ulong _data;

        public TwoBitSet(ulong data)
        {
            _data = data;
        }

        public byte this[int index]
        {
            get
            {
                var shift = index << 1;
                ulong mask = (0b11U << shift);
                return (byte)((_data & mask) >> shift);
            }
        }
    }
    public static void Render()
    {
        
    }
}
