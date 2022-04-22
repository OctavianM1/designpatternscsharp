using MoreLinq;
using System.Collections;
using System.Collections.ObjectModel;
using static DesignPatterns.Structural.Composite.NeuralNetwork;

namespace DesignPatterns.Structural.Composite;

public static class ExtensionsMethod
{
    public static void ConnectTo(this IEnumerable<Neuron> self, IEnumerable<Neuron> other)
    {
        if (ReferenceEquals(self, other)) return;

        foreach (var from in self)
        {
            foreach (var to in other)
            {
                from.ConnectTo(to); 
                to.ConnectTo(from);
            }
        }
    }
}

public static class NeuralNetwork
{
    public class Neuron : IEnumerable<Neuron>
    {
        public float Value { get; set; }
        public List<Neuron> In { get; set; } = new();
        public List<Neuron> Out { get; set; } = new();

        public void ConnectTo(Neuron neuron)
        {
            Out.Add(neuron);
            neuron.In.Add(this);
        }

        public IEnumerator<Neuron> GetEnumerator()
        {
            yield return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class NeuronLayer : Collection<Neuron>
    {

    }

    public class NeuronRing : List<Neuron>
    {

    }

    public static void Render()
    {
        var neuron1 = new Neuron();
        var neuron2 = new Neuron();

        neuron1.ConnectTo(neuron2);

        var layer1 = new NeuronLayer();
        var layer2 = new NeuronLayer();

        neuron1.ConnectTo(layer1);
        layer1.ConnectTo(layer2);
    }
}
