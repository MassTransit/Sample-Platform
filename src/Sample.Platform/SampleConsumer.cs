using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Sample.Platform
{
    public class SampleConsumer :
        IConsumer<SampleCommand>
    {
        readonly ILogger<SampleConsumer> _logger;

        public SampleConsumer(ILogger<SampleConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<SampleCommand> context)
        {
            _logger.LogDebug("Consuming something");

            return Task.CompletedTask;
        }
    }
}