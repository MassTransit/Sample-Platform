using sample_producer;
using MassTransit;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        // services.AddHostedService<SchoolWorker>();
    })
    .Build();

await host.RunAsync();
