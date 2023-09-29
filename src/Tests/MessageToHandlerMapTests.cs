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
        await Verify(map);
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

    static Task VerifyMap(MessageToHandlerMap map) =>
        Verify(new {map.HandledMessages, map.Messages});

    class MyMessage : IMessage;

    class MessageWithNoHandler : IMessage;

    class MyHandler : IHandleMessages<MyMessage>
    {
        public Task Handle(MyMessage message, HandlerContext context) =>
            null!;
    }
}