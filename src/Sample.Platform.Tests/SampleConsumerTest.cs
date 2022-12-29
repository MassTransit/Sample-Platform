using Sample.Platform.Contracts;
using Sample.Platform.Tests.DSL;

namespace Sample.Platform.Tests;

public class UnitTest1
{
    private readonly SampleService _SampleService;

    public UnitTest1()
    {
        _SampleService = new SampleService();
    }

    [Fact]
    public void SendingMessagesHitConsumer()
    {
        _SampleService.GivenWeHaveATimeCommand();
        _SampleService.WhenWeSendMessage();
        _SampleService.ThenMessageShouldBeSent();
    }
}
