namespace DesignPatterns.Creational.Singleton;

internal static class PerThreadSingleton
{
    public sealed class PerThreadSingl
    {
        private static ThreadLocal<PerThreadSingl> _threadInstance = new ThreadLocal<PerThreadSingl>(() => new PerThreadSingl());

        public int Id { get; set; }

        private PerThreadSingl()
        {
            Id = Thread.CurrentThread.ManagedThreadId;
        }

        public static PerThreadSingl Instance => _threadInstance.Value;
    }

    public static void Render()
    {
        var t1 = Task.Factory.StartNew(() =>
        {
            Console.WriteLine($"t1: {PerThreadSingl.Instance.Id}");
            var t1 = Task.Factory.StartNew(() =>
            {
                Console.WriteLine($"tt1: {PerThreadSingl.Instance.Id}");
                var t1 = Task.Factory.StartNew(() =>
                {
                    Console.WriteLine($"t11: {PerThreadSingl.Instance.Id}");
                });
                var t2 = Task.Factory.StartNew(() =>
                {
                    Console.WriteLine($"t22: {PerThreadSingl.Instance.Id}");
                });
                var t3 = Task.Factory.StartNew(() =>
                {
                    Console.WriteLine($"t33: {PerThreadSingl.Instance.Id}");
                });
                var t4 = Task.Factory.StartNew(() =>
                {
                    Console.WriteLine($"t44: {PerThreadSingl.Instance.Id}");
                });
                Task.WaitAll(t1, t2, t3, t4);
            });
            Task.WaitAll(t1);
        });

        var t2 = Task.Factory.StartNew(() =>
        {
            Console.WriteLine($"t2: {PerThreadSingl.Instance.Id}");
            Console.WriteLine($"t2: {PerThreadSingl.Instance.Id}");
        });

        Task.WaitAll(t1, t2);
    }
}
