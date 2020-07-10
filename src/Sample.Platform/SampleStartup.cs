using System;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using MassTransit.Platform.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Sample.Platform
{
    public class SampleStartup :
        IPlatformStartup
    {
        public void ConfigureMassTransit(IServiceCollectionBusConfigurator configurator, IServiceCollection services)
        {
            configurator.AddConsumer<SampleConsumer>();
        }

        public void ConfigureBus<TEndpointConfigurator>(IBusFactoryConfigurator<TEndpointConfigurator> configurator, IBusRegistrationContext context) where TEndpointConfigurator : IReceiveEndpointConfigurator
        {

        }
    }
}
