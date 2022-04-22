using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DesignPatterns.Structural.Proxy;

public static class Proxy3
{
    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class PersonViewModel : INotifyPropertyChanged
    {
        private readonly Person _person;

        public PersonViewModel(Person person)
        {
            _person = person;
        }

        public string FirstName
        {
            get => _person.FirstName;
            set
            {
                if (_person.FirstName == value) return;
                _person.FirstName = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FullName));
            }
        }

        public string LastName
        {
            get => _person.LastName;
            set
            {
                if (_person.LastName == value) return;
                _person.LastName = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FullName));
            }
        }

        public string FullName
        {
            get => $"{FirstName} {LastName}".Trim();
            set
            {
                if (value == null)
                {
                    FirstName = LastName = null;
                    return;
                }
                var items = value.Split();
                if (items.Length > 0)
                    FirstName = items[0];
                if (items.Length > 1)
                    LastName = items[1];
            }
        }

        public event PropertyChangedEventHandler PropertyChanged = TestEvent;

        private static void TestEvent(object s, PropertyChangedEventArgs a)
        {
            Console.WriteLine($"TEST - {s} - {a.PropertyName}");
        }

        protected virtual void OnPropertyChanged(
            [CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public static void Render()
    {
        var p = new Person { FirstName = "firstname", LastName = "lastname" };
        var pProxy = new PersonViewModel(p);
        pProxy.FullName = "Hello guy";
        Console.WriteLine(pProxy.FirstName);
        Console.WriteLine(pProxy.LastName);
        Console.WriteLine(pProxy.FullName);
    }
}
