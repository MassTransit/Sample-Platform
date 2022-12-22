namespace sample_producer;
using MassTransit;

public class Worker : BackgroundService
{
    public IBusControl BusControl { get; }

    private readonly ILogger<Worker> _logger;


    public Worker(ILogger<Worker> logger)
    {
        BusControl = Bus.Factory.CreateUsingRabbitMq(cfg => {
            cfg.Host("rabbitmq", x => {
                x.Username("guest");
            });
        });
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try {
            _logger.LogInformation("Starting: Sample Producer");
            await BusControl.StartAsync();

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Sending Message to queue:: {time}", DateTimeOffset.Now);
                await BusControl.Publish<Sample.Platform.Contracts.SampleCommand>(new { Value = DateTimeOffset.Now.ToString() });
                await Task.Delay(2000, stoppingToken);
            }

        } catch(Exception ex) {
            _logger.LogError(ex.Message, ex);
        } finally {
            await BusControl.StopAsync();
        }
    }
}

