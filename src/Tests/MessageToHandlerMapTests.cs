using System.Threading.Tasks;
using NServiceBus;
using Verify.NServiceBus;
using VerifyXunit;
using Xunit;
using Xunit.Abstractions;

public class MessageToHandlerMapTests :
    VerifyBase
{
    [Fact]
    public Task Integration()
    {
        var map = new MessageToHandlerMap();
        map.AddMessagesFromAssembly<MyMessage>();
        map.AddHandlersFromAssembly<MyMessage>();
        return base.Verify(map);
    }

    [Fact]
    public Task AddMessage_type()
    {
        var map = new MessageToHandlerMap();
        map.AddMessage<MyMessage>();
        return Verify(map);
    }

    [Fact]
    public Task AddMessage_assembly()
    {
        var map = new MessageToHandlerMap();
        map.AddMessagesFromAssembly<MyMessage>();
        return Verify(map);
    }

    [Fact]
    public Task AddHandler_type()
    {
        var map = new MessageToHandlerMap();
        map.AddHandler<MyHandler>();
        return Verify(map);
    }

    [Fact]
    public Task AddHandler_assembly()
    {
        var map = new MessageToHandlerMap();
        map.AddHandlersFromAssembly<MyHandler>();
        return Verify(map);
    }

    Task Verify(MessageToHandlerMap map)
    {
        return Verify(new {map.Handlers, map.Messages});
    }

    public MessageToHandlerMapTests(ITestOutputHelper output) :
        base(output)
    {
    }

    class MyMessage : IMessage
    {

    }

    class MessageWithNoHandler : IMessage
    {

    }

    class MyHandler : IHandleMessages<MyMessage>
    {
        public Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            return null!;
        }
    }
}