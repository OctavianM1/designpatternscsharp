using System.Collections;

namespace DesignPatterns.Behavioral.Iterator;

public static class Iterator2
{
    public class Creature : IEnumerable<int>
    {
        private int[] stats = new int[3];
        private const int strength = 0;
        public int Strength { get => stats[strength]; set => stats[strength] = value; }
        public int Agility { get; set; }
        public int Intelligence { get; set; }

        public double AverageStat => stats.Average();

        public IEnumerator<int> GetEnumerator() => stats.AsEnumerable().GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int this[int index]
        {
            get => stats[index];
            set => stats[index] = value;
        }
    }

    public static void Render()
    {

    }
}
