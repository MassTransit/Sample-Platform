using System.Diagnostics.Eventing.Reader;
using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework.Internal;
using Sample.Platform.Contracts;
using Sample.Platform.db;

namespace Sample.Platform.Tests.DSL;

public enum ProductType
{
    Car,
    Van,
    Minibus
}

/**
 * https://computersciencewiki.org/index.php/Example_Problem_Set
 *
 * At a prestigious international school, we have only 5 administrators, Michael, Carol, Jen, Constance and TJ.
 * Your program should ask the user to type in their name.
 * If their name matches one of our administrators,
 * your program must output a special greeting.
 *
 * If the user input is any other name (does not match the list of administrators)
 * your program should simply output a simple greeting.
 *
*/

public interface ISchoolWelcome
{
    Task<ISchool> GivenWeHaveASchool(string name);
    Task AndWeHaveAdministrator(string schoolId, string admin);
    void WhenSearchedNameIs(string schoolId, string input);
    void ThenOutputSpecialGreeting();
    void ThenOutputStandardGreeting();
}

public class SchoolService : ISchoolWelcome
{
    private ServiceProvider _provider;
    private InMemoryTestHarness _harness;
    private ConsumerTestHarness<IConsumer<CreateAdminConsumer>> _CreateAdminConsumerHarness;

    public SchoolService()
    {
        _provider = new ServiceCollection()
            .AddTransient<IConsumer<ICreateSchool>, CreateSchoolConsumer>()
            .AddTransient<IConsumer<ICreateAdmin>, CreateAdminConsumer>()
            .AddSingleton<ISchoolContext, SchoolContext>()
            .AddMassTransitInMemoryTestHarness(cfg =>
            {
                cfg.AddConsumer<CreateSchoolConsumer>();
                cfg.AddConsumer<CreateAdminConsumer>();
                cfg.AddConsumerTestHarness<CreateAdminConsumer>();

            })
            .AddLogging(cfg =>
            {
                cfg.AddConsole();
            })
            .BuildServiceProvider(true);

        _harness = _provider.GetRequiredService<InMemoryTestHarness>();
        _CreateAdminConsumerHarness = _harness.Consumer(() => _provider.GetRequiredService<IConsumer<CreateAdminConsumer>>() );
        _harness.Start();
    }

    public async Task<ISchool> GivenWeHaveASchool(string name)
    {
        var client = _harness.Bus.CreateRequestClient<ICreateSchool>();
        var result = await client.GetResponse<ISchool>( new { SchoolName = name });
        return result.Message;
    }

    public async Task AndWeHaveAdministrator(string schoolId, string admin)
    {
        await _harness.Bus.Publish<ICreateAdmin>(new
        {
            SchoolId = schoolId.ToString(),
            AdminName = admin
        });
    }

    public async void WhenSearchedNameIs(string schoolId, string input)
    {
        await _harness.InputQueueSendEndpoint.Send(new { SchoolId = schoolId, AdminSearch = input });
    }

    public async void ThenOutputSpecialGreeting()
    {
        Assert.True(await _CreateAdminConsumerHarness.Consumed.Any<ISpecialGreeting>());
    }

    public async void ThenOutputStandardGreeting()
    {
        Assert.True(await _CreateAdminConsumerHarness.Consumed.Any<IStandardGreeting>());
    }
}

public interface School {
    public string SchoolName { get; set; }
    public List<Admin> Admins { get; set; }
}

public interface Admin
{
    public string AdminName { get; set; }
}
