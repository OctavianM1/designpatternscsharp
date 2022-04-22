//using MoreLinq;

//namespace DesignPatterns.Creational.Singleton;

//public static class LazySingleton
//{
//    public interface IDatabase
//    {
//        int GetPopulation(string name);
//    }

//    public class SingletonDatabase : IDatabase
//    {
//        private readonly Dictionary<string, int> _capitals;

//        private static int instanceCount;
//        public static int Count => instanceCount;

//        private SingletonDatabase()
//        {
//            instanceCount++;
//            Console.WriteLine("Initializing database");
//            _capitals = File.ReadAllLines(@"Creational\Singleton\capitals.txt")
//                .Batch(2)
//                .ToDictionary(
//                list => list.ElementAt(0).Trim(),
//                list => int.Parse(list.ElementAt(1))
//            );
//        }

//        public int GetPopulation(string name)
//        {
//            return _capitals[name];
//        }

//        private static Lazy<SingletonDatabase> instance = new Lazy<SingletonDatabase>(() => new SingletonDatabase());

//        public static SingletonDatabase Instance => instance.Value;
//    }

//    public class OrdinaryDatabase : IDatabase
//    {
//        private readonly Dictionary<string, int> _capitals;

//        public OrdinaryDatabase()
//        {
//            Console.WriteLine("Initializing database");
//            _capitals = File.ReadAllLines(@"Creational\Singleton\capitals.txt")
//                .Batch(2)
//                .ToDictionary(
//                list => list.ElementAt(0).Trim(),
//                list => int.Parse(list.ElementAt(1))
//            );
//        }

//        public int GetPopulation(string name)
//        {
//            return _capitals[name];
//        }
//    }

//    public class ConfigurableRecordFinder
//    {
//        private IDatabase _database;

//        public ConfigurableRecordFinder(IDatabase database)
//        {
//            _database = database;
//        }

//        public int GetTotalPopulation(IEnumerable<string> names)
//        {
//            int result = 0;
//            foreach (string name in names)
//            {
//                result += _database.GetPopulation(name);
//            }
//            return result;
//        }
//    }

//    public class DummyDatabase : IDatabase
//    {
//        public int GetPopulation(string name)
//        {
//            return new Dictionary<string, int>
//            {
//                ["A"] = 1,
//                ["B"] = 2,
//                ["C"] = 3
//            }[name];
//        }
//    }

//    public class SingletonRecordFinder
//    {
//        public int GetTotalPopulation(IEnumerable<string> names)
//        {
//            int result = 0;
//            foreach (string name in names)
//            {
//                result += SingletonDatabase.Instance.GetPopulation(name);
//            }
//            return result;
//        }
//    }

//    public static void Render()
//    {
//        var db = SingletonDatabase.Instance;
//        var city = "A";
//        var pA = db.GetPopulation(city);
//        Console.WriteLine(pA);
//    }
//}
