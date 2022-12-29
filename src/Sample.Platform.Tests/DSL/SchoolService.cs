using System.Diagnostics.Eventing.Reader;
using MassTransit;
using MassTransit.Definition;
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
    Task AndWeHaveAdministrator(string schoolId, string adminName);
    void WhenSearchedNameIs(string schoolId, string input);
    Task<bool> ThenOutputSpecialGreeting();
    Task<bool> ThenOutputStandardGreeting();
}

public class SchoolService : ISchoolWelcome
{
    private ServiceProvider _provider;
    private InMemoryTestHarness _harness;

    public SchoolService()
    {
        EndpointConvention.Map<ICreateAdmin>(new Uri("queue:CreateAdmin"));
        EndpointConvention.Map<ISchoolSearch>(new Uri("queue:SchoolSearch"));

        _provider = new ServiceCollection()
            .AddTransient<IConsumer<ICreateSchool>, CreateSchoolConsumer>()
            .AddTransient<IConsumer<ICreateAdmin>, CreateAdminConsumer>()
            .AddTransient<IConsumer<ISchoolSearch>, SchoolSearchConsumer>()
            .AddSingleton<ISchoolContext, SchoolContext>()
            .AddMassTransitInMemoryTestHarness(cfg =>
            {
                cfg.AddConsumer<CreateSchoolConsumer>();
                cfg.AddConsumer<CreateAdminConsumer>();
                cfg.AddConsumer<SchoolSearchConsumer>();
            })
            .AddLogging(cfg =>
            {
                cfg.AddConsole();
            })
            .BuildServiceProvider(true);

        _harness = _provider.GetRequiredService<InMemoryTestHarness>();
        _harness.Start();
    }

    public async Task<ISchool> GivenWeHaveASchool(string name)
    {
        var client = _harness.Bus.CreateRequestClient<ICreateSchool>();
        var result = await client.GetResponse<ISchool>( new { SchoolName = name });
        return result.Message;
    }

    public async Task AndWeHaveAdministrator(string schoolId, string adminName)
    {
        await _harness.Bus.Send<ICreateAdmin>(new
        {
            SchoolId = schoolId.ToString(),
            AdminName = adminName
        });
    }

    public async void WhenSearchedNameIs(string schoolId, string input)
    {
        await _harness.Bus.Send<ISchoolSearch>(new
        {
            SchoolId = schoolId,
            SearchName = input
        });
    }

    public async Task<bool> ThenOutputSpecialGreeting()
    {
        return await _harness.Published.Any<ISpecialGreeting>();
    }

    public async Task<bool> ThenOutputStandardGreeting()
    {
        return await _harness.Published.Any<IStandardGreeting>();
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
