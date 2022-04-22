using System.Text;

namespace DesignPatterns.Structural.Flyweight;

public static class FlyWeight1
{
    public class FormattedText
    {
        private readonly string _plainText;
        private readonly bool[] _capitalize;

        public FormattedText(string plainText)
        {
            _plainText = plainText;
            _capitalize = new bool[plainText.Length];
        }

        public void Capitalize(int start, int end)
        {
            for (int i = start; i < end; i++)
            {
                _capitalize[i] = true;
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < _plainText.Length; i++)
            {
                sb.Append(_capitalize[i] ? char.ToUpper(_plainText[i]) : _plainText[i]);
            }
            return sb.ToString();
        }
    }

    public class BetterFormattedText
    {
        private readonly string _plainText;
        private readonly List<TextRange> _formatting = new();

        public BetterFormattedText(string plainText)
        {
            _plainText = plainText;
        }

        public TextRange GetRange(int start, int end)
        {
            var range = new TextRange { Start = start, End = end };
            _formatting.Add(range);
            return range;
        }

        public class TextRange
        {
            public int Start { get; set; }
            public int End { get; set; }
            public bool Capitalize { get; set; }
            public bool Bold { get; set; }
            public bool Italic { get; set; }

            public bool Covers(int position)
            {
                return position >= Start && position <= End;
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            for (int i = 0; i < _plainText.Length; i++)
            {
                char c = _plainText[i];
                foreach (var range in _formatting)
                {
                    if (range.Covers(i) && range.Capitalize)
                    {
                        c = char.ToUpper(c);
                    }
                }
                sb.Append(c);
            }
            return sb.ToString();
        }
    }

    public static void Render()
    {
        var ft = new FormattedText("This is a brave new world");
        ft.Capitalize(10, 15);
        Console.WriteLine(ft);

        var bft = new BetterFormattedText("This is a brave new world");
        bft.GetRange(10, 15).Capitalize = true;
        Console.WriteLine(bft);
    }
}
