using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using MassTransit.Platform.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Sample.Platform.Contracts;
using Sample.Platform.db;

namespace Sample.Platform
{
    public class SampleStartup : IPlatformStartup
    {

        public void ConfigureMassTransit(IServiceCollectionBusConfigurator configurator, IServiceCollection services)
        {
            services.AddTransient<ISchoolContext, SchoolContext>();
            configurator.AddConsumer<CreateSchoolConsumer>();
            configurator.AddConsumersFromNamespaceContaining<SampleConsumer>();
        }

        public void ConfigureBus<TEndpointConfigurator>(IBusFactoryConfigurator<TEndpointConfigurator> configurator, IBusRegistrationContext context) where TEndpointConfigurator : IReceiveEndpointConfigurator
        {
        }
    }
}
