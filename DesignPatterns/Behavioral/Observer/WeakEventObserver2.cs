using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DesignPatterns.Behavioral.Observer;

public static class WeakEventObserver2
{
    public class Market
    {
        public BindingList<float> Prices = new();
    }

    public static void Render()
    {
        var market = new Market();
        market.Prices.ListChanged += (sender, args) =>
        {
            if (args.ListChangedType == ListChangedType.ItemAdded)
            {
                float price = ((BindingList<float>)sender)[args.NewIndex];
                Console.WriteLine($"Binding list got a price of {price}");
            }
        };

        market.Prices.Add(45f);
    }

}
