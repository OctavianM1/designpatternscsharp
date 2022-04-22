using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace DesignPatterns.Behavioral.Observer;

public static class BidirectionalObserver
{
    public class Product : INotifyPropertyChanged
    {
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (value == _name) return;
                _name = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            return $"Product - {_name}";
        }

    }

    public class Window : INotifyPropertyChanged
    {
        private string _productname;
        public string ProductName
        {
            get => _productname;
            set
            {
                if (value == _productname) return;
                _productname = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            return $"Window - {_productname}";
        }
    }

    public sealed class BidirectionalBinding : IDisposable
    {
        private bool _disposed;

        public BidirectionalBinding(
            INotifyPropertyChanged first,
            Expression<Func<object>> firstProperty,
            INotifyPropertyChanged second,
            Expression<Func<object>> secondProperty
            )
        {
            // xProperty is MemberExpression
            if (firstProperty.Body is MemberExpression firstExpr &&
                secondProperty.Body is MemberExpression secondExpr)
            {
                if (firstExpr.Member is PropertyInfo firstProp &&
                    secondExpr.Member is PropertyInfo secondProp)
                {
                    first.PropertyChanged += (sender, args) =>
                    {
                        if (!_disposed)
                        {
                            secondProp.SetValue(second, firstProp.GetValue(first));
                        }
                    };

                    second.PropertyChanged += (sender, args) =>
                    {
                        if (!_disposed)
                        {
                            firstProp.SetValue(first, secondProp.GetValue(second));
                        }
                    };
                }
            }
        }

        public void Dispose()
        {
            _disposed = true;
        }
    }

    public static void Render()
    {
        var product = new Product { Name = "Book" };
        var window = new Window { ProductName = "Book" };

        //product.PropertyChanged += (sender, eventArgs) =>
        //{
        //    if (eventArgs.PropertyName == "Name")
        //    {
        //        Console.WriteLine("Name changed in Product");
        //        window.ProductName = product.Name;
        //    }
        //};

        //window.PropertyChanged += (sender, eventArgs) =>
        //{
        //    if (eventArgs.PropertyName == "Name")
        //    {
        //        Console.WriteLine("Product name changed in Window");
        //        product.Name = window.ProductName;
        //    }
        //};

        using var binding = new BidirectionalBinding(
            product,
            () => product.Name,
            window,
            () => window.ProductName);

        product.Name = "Smart book";
        window.ProductName = "Changed book";

        Console.WriteLine(product);
        Console.WriteLine(window);
    }
}
