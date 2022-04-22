using Autofac;
using MediatR;

namespace DesignPatterns.Behavioral.MediatorCustom;

public static class MediatRMediator
{
    // ping
    
    public class PingCommand : IRequest<PongResponse>
    {

    }

    public class PingCommandHandler : IRequestHandler<PingCommand, PongResponse>
    {
        public async Task<PongResponse> Handle(PingCommand request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new PongResponse { TimeStamp = DateTime.UtcNow }).ConfigureAwait(false);
        }
    }

    public class PongResponse
    {
        public DateTime TimeStamp { get; set; }
    }

    public static async Task Render()
    {
        var cb = new ContainerBuilder();
        cb.RegisterType<Mediator>()
            .As<IMediator>()
            .InstancePerLifetimeScope();

        cb.Register<ServiceFactory>(ctx =>
        {
            var c = ctx.Resolve<IComponentContext>();
            return t => c.Resolve(t);
        });

        cb.RegisterAssemblyTypes(typeof(MediatRMediator).Assembly)
            .AsImplementedInterfaces();

        var c = cb.Build();
        var mediator = c.Resolve<IMediator>();
        var response = await mediator.Send(new PingCommand());
        Console.WriteLine($"We got a response at {response.TimeStamp}");
    }
}
