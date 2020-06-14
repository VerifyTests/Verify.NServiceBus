using System.Threading.Tasks;
using NServiceBus;
using VerifyTests.NServiceBus;
using VerifyXunit;
using Xunit;

[UsesVerify]
public class MessageToHandlerMapTests
{
    [Fact]
    public async Task Integration()
    {
        #region MessageToHandlerMap
        var map = new MessageToHandlerMap();
        map.AddMessagesFromAssembly<MyMessage>();
        map.AddHandlersFromAssembly<MyHandler>();
        await Verifier.Verify(map);
        #endregion
    }

    [Fact]
    public Task AddMessage_type()
    {
        var map = new MessageToHandlerMap();
        map.AddMessage<MyMessage>();
        return VerifyMap(map);
    }

    [Fact]
    public Task AddMessage_assembly()
    {
        var map = new MessageToHandlerMap();
        map.AddMessagesFromAssembly<MyMessage>();
        return VerifyMap(map);
    }

    [Fact]
    public Task AddHandler_type()
    {
        var map = new MessageToHandlerMap();
        map.AddHandler<MyHandler>();
        return VerifyMap(map);
    }

    [Fact]
    public Task AddHandler_assembly()
    {
        var map = new MessageToHandlerMap();
        map.AddHandlersFromAssembly<MyHandler>();
        return VerifyMap(map);
    }

    Task VerifyMap(MessageToHandlerMap map)
    {
        return Verifier.Verify(new {map.HandledMessages, map.Messages});
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