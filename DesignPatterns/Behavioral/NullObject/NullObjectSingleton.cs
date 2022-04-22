namespace DesignPatterns.Behavioral.NullObject;

public static class NullObjectSingleton
{
    public interface ILog
    {
        void Warn();

        public static ILog Null => NullLog.Instance;

        private sealed class NullLog : ILog
        {
            private NullLog() { }
            public void Warn()
            {
            }

            private static Lazy<NullLog> _instance = new(() => new NullLog());

            public static ILog Instance => _instance.Value;
        }
    }

    public static void Render()
    {
        
    }
}
