namespace Sample.Platform.Tests.DSL;

using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using MassTransit.Testing;
using Microsoft.Extensions.Logging;
using Contracts;

public interface ISampleService
{
    public void GivenWeHaveATimeCommand();
    public void WhenWeSendMessage();
    public void ThenMessageShouldBeSent();
}

public class SampleService : ISampleService
{
    private ServiceProvider _provider;
    private InMemoryTestHarness _harness;
    private ConsumerTestHarness<IConsumer<SampleCommand>> _consumerHarness;

    public void GivenWeHaveATimeCommand()
    {
        _provider = new ServiceCollection()
            .AddTransient<IConsumer<SampleCommand>, SampleConsumer>()
            .AddMassTransitInMemoryTestHarness(cfg =>
            {
                //cfg.AddConsumer<SampleConsumer>();
                cfg.AddConsumersFromNamespaceContaining<SampleConsumer>();
            })
            .AddLogging(cfg =>
            {
                cfg.AddConsole();
            })
            .BuildServiceProvider(true);

        _harness = _provider.GetRequiredService<InMemoryTestHarness>();
        var consumer = _provider.GetRequiredService<IConsumer<SampleCommand>>();
        _consumerHarness = _harness.Consumer(() => consumer);
        _harness.Start();
    }

    public async void WhenWeSendMessage()
    {
        await _harness.InputQueueSendEndpoint.Send<SampleCommand>(new { Value = "Hello" });
    }

    public async void ThenMessageShouldBeSent()
    {
        Assert.True(await _consumerHarness.Consumed.Any<SampleCommand>());
    }
}
