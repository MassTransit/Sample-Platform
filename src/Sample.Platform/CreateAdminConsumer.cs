using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Sample.Platform.Contracts;

namespace Sample.Platform
{
    public class CreateAdminConsumer : IConsumer<ICreateAdmin>
    {
        private ISchoolContext _schoolContext;
        readonly ILogger<SampleConsumer> _logger;

        public CreateAdminConsumer(ISchoolContext schoolContext, ILogger<SampleConsumer> logger)
        {
            _schoolContext = schoolContext;
            _logger = logger;
        }

        public Task Consume(ConsumeContext<ICreateAdmin> context)
        {
            _logger.LogDebug($"Create Admin :: {context?.Message?.AdminName}");

            _schoolContext.AddAdmin(context?.Message);
            return Task.CompletedTask;
        }
    }
}
