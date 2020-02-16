using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace OCCBPackage.Extensions
{
    public static class MediatRExtensions
    {
        public static IServiceCollection AddMediatR(this IServiceCollection services) => services
            .AddScoped<ServiceFactory>(p => t => p.GetService(t))
            .AddScoped<IMediator, Mediator>();
    }
}
