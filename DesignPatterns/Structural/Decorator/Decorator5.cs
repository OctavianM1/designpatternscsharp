using Autofac;
using System.Text;

namespace DesignPatterns.Structural.Decorator;

public static class Decorator5
{
    public interface IReportingService
    {
        void Report();
    }

    public class ReportingService : IReportingService
    {
        public void Report()
        {
            Console.WriteLine("Here is your report");
        }
    }

    public class ReportingServiceWithLogging : IReportingService
    {
        private readonly IReportingService _decorated;

        public ReportingServiceWithLogging(IReportingService decorated)
        {
            _decorated = decorated;
        }

        public void Report()
        {
            Console.WriteLine("Starting report...");
            _decorated.Report();
            Console.WriteLine("Ending report...");
        }
    }

    public static void Render()
    {
        var cb = new ContainerBuilder();
        cb.RegisterType<ReportingService>().Named<IReportingService>("reporting");
        cb.RegisterDecorator<IReportingService>(
            (context,service) => new ReportingServiceWithLogging(service), "reporting"
        );

        using var c = cb.Build();
        var r = c.Resolve<IReportingService>();
        r.Report();
    }
}
