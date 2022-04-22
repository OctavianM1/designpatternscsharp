using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace DesignPatterns.Behavioral.Observer;

public static class PropertyDependenciesObserver
{
    public class PropertyNotificationSupport : INotifyPropertyChanged
    {
        private readonly Dictionary<string, HashSet<string>> _affectedBy = new();

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            foreach (var affected in _affectedBy.Keys)
            {
                // Circular dependencies
                if (_affectedBy[affected].Contains(propertyName))
                    OnPropertyChanged(affected);
            }
        }

        protected Func<T> Property<T>(string name, Expression<Func<T>> expr)
        {
            Console.WriteLine($"Creating computed property for expression {expr}");

            var visitor = new MemberAccessVisitor(GetType());
            visitor.Visit(expr);

            if (visitor.PropertyNames.Any())
            {
                if (!_affectedBy.ContainsKey(name))
                    _affectedBy.Add(name, new HashSet<string>());

                foreach (var propName in visitor.PropertyNames)
                {
                    if (propName != name)
                    {
                        _affectedBy[name].Add(propName);
                    }
                }
            }

            return expr.Compile();
        }

        private class MemberAccessVisitor : ExpressionVisitor
        {
            private readonly Type _declaringType;
            public readonly IList<string> PropertyNames = new List<string>();

            public MemberAccessVisitor(Type declaringType)
            {
                _declaringType = declaringType;
            }

            public override Expression? Visit(Expression expr)
            {
                if (expr != null && expr.NodeType == ExpressionType.MemberAccess)
                {
                    var memberExpr = (MemberExpression)expr;
                    if (memberExpr.Member.DeclaringType == _declaringType)
                    {
                        PropertyNames.Add(memberExpr.Member.Name);
                    }
                }

                return base.Visit(expr);
            }
        }
    }

    public class Person : PropertyNotificationSupport
    {   
        private bool _citizen;

        public bool Citizen
        {
            get => _citizen;
            set
            {
                if (value == _citizen) return;
                _citizen = value;
                OnPropertyChanged();
            }
        }

        private int _age;

        public int Age
        { 
            get => _age;
            set
            {
                // 4->5
                // false->false
                if (value == _age) return;
                _age = value;
                OnPropertyChanged();
                //OnPropertyChanged(nameof(CanVote));
            }
        }

        public Person()
        {
            _canVote = Property(nameof(CanVote), () => Age >= 16 && Citizen);
        }

        //public bool CanVote => Age >= 16;

        private readonly Func<bool> _canVote;
        public bool CanVote => _canVote();
    }

    public static void Render()
    {
        var p = new Person();
        p.PropertyChanged += (sender, args) =>
        {
            Console.WriteLine($"{args.PropertyName} changed");
        };

        p.Age = 15;
        p.Citizen = true;
    }
}
