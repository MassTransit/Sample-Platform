using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Sample.Platform.Contracts;

namespace Sample.Platform
{
    public class SchoolSearchConsumer :
        IConsumer<ISchoolSearch>
    {
        private ISchoolContext _schoolContext;
        readonly ILogger<SampleConsumer> _logger;

        public SchoolSearchConsumer(ISchoolContext schoolContext, ILogger<SampleConsumer> logger)
        {
            _schoolContext = schoolContext;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ISchoolSearch> context)
        {
            _logger.LogInformation("Searching school for admin");

            var school = _schoolContext.Get(context.Message.SchoolId);
            var found = school.Admins.Any(x => x.AdminName.Equals(context.Message.SearchName));

            if (found)
            {
                await context.Publish<ISpecialGreeting>(new
                {
                    Message = $"Special Hello, {context.Message.SearchName}"
                });
            }
            else
            {
                await context.Publish<IStandardGreeting>(new
                {
                    Message = $"Standard Hello, {context.Message.SearchName}"
                });
            }


        }
    }
}
