using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatterns.SOLID;

internal static class O
{
    public enum Color
    {
        Red, Green, Blue
    }

    public enum Size
    {
        Small, Medium, Large, Yuge
    }

    public class Product
    {
        public string Name;
        public Color Color;
        public Size Size;

        public Product(string name, Color color, Size size)
        {
            Name = name;
            Color = color;
            Size = size;
        }
    }

    public class ProductFilter
    {
        public static IEnumerable<Product> FilterBySize(IEnumerable<Product> products, Size size)
        {
            foreach (Product product in products)
            {
                if (product.Size == size)
                    yield return product;
            }
        }

        public static IEnumerable<Product> FilterByColor(IEnumerable<Product> products, Color color)
        {
            foreach (Product product in products)
            {
                if (product.Color == color)
                    yield return product;
            }
        }

        public static IEnumerable<Product> FilterByColorAndSize(IEnumerable<Product> products, Size size, Color color)
        {
            foreach (Product product in products)
            {
                if (product.Size == size && product.Color == color)
                    yield return product;
            }
        }
    }

    public interface ISpecification<T>
    {
        bool IsSatisfied(T t);
    }

    public interface IFilter<T>
    {
        public IEnumerable<T> Filter(IEnumerable<T> items, ISpecification<T> spec);
    }

    public class ColorSpecification : ISpecification<Product>
    {
        private Color color;

        public ColorSpecification(Color color)
        {
            this.color = color;
        }

        public bool IsSatisfied(Product t) => t.Color == color;
    }

    public class SizeSpecification : ISpecification<Product>
    {
        private Size size;

        public SizeSpecification(Size size)
        {
            this.size = size;
        }

        public bool IsSatisfied(Product t) => t.Size == size;
    }

    public class AndSpecification<T> : ISpecification<T>
    {
        ISpecification<T> first, second;

        public AndSpecification(ISpecification<T> first, ISpecification<T> second)
        {
            this.first = first;
            this.second = second;
        }

        public bool IsSatisfied(T t)
        {
            return first.IsSatisfied(t) && second.IsSatisfied(t);
        }
    }

    public class ManySpecifications : ISpecification<Product>
    {
        private readonly IEnumerable<ISpecification<Product>> _productSpecifications;

        public ManySpecifications(IEnumerable<ISpecification<Product>> productSpecifications)
        {
            _productSpecifications = productSpecifications;
        }

        public bool IsSatisfied(Product t) => _productSpecifications.All(x => x.IsSatisfied(t));
    }

    public class BetterFilter : IFilter<Product>
    {
        public IEnumerable<Product> Filter(IEnumerable<Product> items, ISpecification<Product> spec) =>
            items.Where(x => spec.IsSatisfied(x));
    }

    public static void Render()
    {
        var apple = new Product("Apple", Color.Green, Size.Small);
        var tree = new Product("Tree", Color.Green, Size.Large);
        var house = new Product("House", Color.Blue, Size.Large);

        Product[] products = { apple, tree, house };

        var bf = new BetterFilter();
        var greenSpec = new ColorSpecification(Color.Green);

        var largeProducts = ProductFilter.FilterBySize(products, Size.Large);
        //var greenLargeProducts = ProductFilter.FilterByColorAndSize(products, Size.Large, Color.Green);
        var betterLarge = bf.Filter(products, greenSpec);
        var betterGreenLargeProducts = bf.Filter(products,
                                                  new ManySpecifications(new List<ISpecification<Product>>
                                                  {
                                                      new ColorSpecification(Color.Green),
                                                      new SizeSpecification(Size.Large)
                                                  }));
        RenderProducts(betterGreenLargeProducts);
    }

    private static void RenderProducts(IEnumerable<Product> products)
    {
        foreach (var product in products)
        {
            Console.WriteLine($"Product name - {product.Name}, size - {product.Size}, color - {product.Color}");
        }
    }
}
