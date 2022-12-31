using Bogus;
using MassTransit;
using Sample.Platform.Contracts;

namespace sample_producer;

public class SchoolWorker : BackgroundService
{
    private IBusControl _busControl;

    private readonly ILogger<Worker> _logger;

    public SchoolWorker(ILogger<Worker> logger)
    {
        _logger = logger;

        EndpointConvention.Map<ICreateSchool>(new Uri("queue:create-school"));
        EndpointConvention.Map<ICreateAdmin>(new Uri("queue:create-admin"));
        EndpointConvention.Map<ISchoolSearch>(new Uri("queue:school-search"));

        _busControl = Bus.Factory.CreateUsingRabbitMq(cfg => {
            cfg.Host("rabbitmq", x => {
                x.Username("guest");
            });
        });
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try {
            _logger.LogInformation("Starting: School Producer");
            await _busControl.StartAsync();

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Creating School");
                var adminName = "";

                var schoolFake = new Faker<CreateSchool>()
                    .RuleFor(u => u.SchoolName, f => $"{f.Name.JobArea()} Academy");

                var client = _busControl.CreateRequestClient<ICreateSchool>();
                var result = await client.GetResponse<ISchool>(schoolFake.Generate(1).First());
                var school = result.Message;

                var numAdmins = new Random().Next(10);
                for (var x = 0; x <= numAdmins; x++)
                {

                    var adminFake = new Faker<CreateAdmin>()
                        .RuleFor(u => u.AdminName, f => f.Name.FirstName());

                    var admin = adminFake.Generate();
                    admin.SchoolId = school.SchoolId;
                    await _busControl.Send<ICreateAdmin>(admin);

                    // Choose to find an Admin randomly
                    adminName = new Random().Next(10) > 5 ? admin.AdminName : "Bob";
                }

                // Search for Admin
                await _busControl.Send<ISchoolSearch>(new
                {
                    school.SchoolId,
                    SearchName = adminName
                });

                await Task.Delay(2000, stoppingToken);
            }

        } catch(Exception ex) {
            _logger.LogError(ex.Message, ex);
        } finally {
            await _busControl.StopAsync();
        }
    }
}
