﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DesignPatterns.MicrosoftDI;

internal static class Test
{
    public interface IOperation
    {
        string OperationId { get; }
    }

    public interface ITransientOperation : IOperation
    {
    }

    public interface IScopedOperation : IOperation
    {
    }

    public interface ISingletonOperation : IOperation
    {
    }

    public class DefaultOperation :
    ITransientOperation,
    IScopedOperation,
    ISingletonOperation
    {
        public string OperationId { get; } = Guid.NewGuid().ToString()[^4..];
    }

    public class DefaultOperation2 :
            ITransientOperation,
            IScopedOperation,
            ISingletonOperation
    {
        public string OperationId { get; } = Guid.NewGuid().ToString()[^5..];
    }

    public class OperationLogger
    {
        private readonly ITransientOperation _transientOperation;
        private readonly IScopedOperation _scopedOperation;
        private readonly ISingletonOperation _singletonOperation;

        public OperationLogger(ITransientOperation transientOperation, IScopedOperation scopedOperation, ISingletonOperation singletonOperation)
        {
            _transientOperation = transientOperation;
            _scopedOperation = scopedOperation;
            _singletonOperation = singletonOperation;
        }

        public void LogOperations(string scope)
        {
            LogOperation(_transientOperation, scope, "Always different");
            LogOperation(_scopedOperation, scope, "Changes only with scope");
            LogOperation(_singletonOperation, scope, "Always the same");
        }


        private static void LogOperation<T>(T operation, string scope, string message)
            where T : IOperation =>
            Console.WriteLine(
                $"{scope}: {typeof(T).Name,-19} [ {operation.OperationId}...{message,-23} ]");
    }

    public static async Task Render()
    {
        using IHost host = Host.CreateDefaultBuilder()
    .ConfigureServices((_, services) =>
        services.AddTransient<ITransientOperation, DefaultOperation>()
            .AddTransient<ITransientOperation, DefaultOperation2>()
            .AddScoped<IScopedOperation, DefaultOperation>()
            .AddSingleton<ISingletonOperation, DefaultOperation>()
            .AddTransient<OperationLogger>())
    .Build();

        ExemplifyScoping(host.Services, "Scope 1");
        ExemplifyScoping(host.Services, "Scope 2");

        await host.RunAsync();
    }

    static void ExemplifyScoping(IServiceProvider services, string scope)
    {
        using IServiceScope serviceScope = services.CreateScope();
        IServiceProvider provider = serviceScope.ServiceProvider;

        OperationLogger logger = provider.GetRequiredService<OperationLogger>();
        logger.LogOperations($"{scope}-Call 1 .GetRequiredService<OperationLogger>()");

        Console.WriteLine("...");

        logger = provider.GetRequiredService<OperationLogger>();
        logger.LogOperations($"{scope}-Call 2 .GetRequiredService<OperationLogger>()");

        Console.WriteLine();
    }
}
