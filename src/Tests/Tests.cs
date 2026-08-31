public class Tests
{
    class MyHandlerWithLogging(ILogger logger) :
        IHandleMessages<MyMessage>
    {
        public Task Handle(MyMessage message, HandlerContext context)
        {
            logger.LogWarning("The log message");
            return Task.CompletedTask;
        }
    }

    [Fact]
    public async Task HandlerWithLogging()
    {
        Recording.Start();
        var logger = RecordingProvider.CreateLogger<MyHandlerWithLogging>();
        var handler = new MyHandlerWithLogging(logger);
        var context = new RecordingHandlerContext();

        await handler.Handle(new(), context);

        await Verify(context);
    }

    [Fact]
    public async Task MessageHandlerContext()
    {
        var context = new RecordingHandlerContext(
            [
                new("key", "value"),
                new("NServiceBus.MessageId", "TheId"),
            ]
        );
        context.Extensions.Set("key", "value");
        await context.Publish("publish message");
        await context.Send("send message");
        await context.SendLocal("send local message");
        await Verify(context);
    }

    [Fact]
    public async Task EnsureDefaultHeaders()
    {
        var context = new RecordingHandlerContext(
            [
                //one new
                new("key", "value"),
                //one overwrite
                new("NServiceBus.MessageId", "TheId"),
            ]
        );
        await Verify(context.MessageHeaders);
    }

    [Fact]
    public async Task MessageSession()
    {
        var context = new RecordingMessageSession();
        await context.Publish("message");
        var subscribeOptions = new SubscribeOptions();
        subscribeOptions.RequireImmediateDispatch();
        await context.Subscribe(typeof(MyMessage), subscribeOptions);
        var unsubscribeOptions = new UnsubscribeOptions();
        unsubscribeOptions.RequireImmediateDispatch();
        await context.Unsubscribe(typeof(MyMessage), unsubscribeOptions);
        await Verify(context);
    }

    [Fact]
    public async Task OptionsWithOnlyImmediateDispatchFalse()
    {
        var context = new RecordingMessageSession();
        var options = new SendOptions();
        // RequireImmediateDispatch stashes the state with true. Sending inside an ambient
        // transaction leaves it false, which writes nothing, so on its own it must not produce
        // an empty Options member.
        options.RequireImmediateDispatch();
        var extensions = NServiceBus.Extensibility.ExtendableOptionsExtensions.GetExtensions(options);
        var state = extensions.GetValues().Single().Value;
        state.GetType()
            .GetProperty("ImmediateDispatch")!
            .SetValue(state, false);
        await context.Send("message", options);
        await Verify(context);
    }

    [Fact]
    public async Task OptionsWithOnlyTransportTransaction()
    {
        var context = new RecordingMessageSession();
        var options = new SendOptions();
        // what NServiceBus stashes when sending inside an ambient transaction. It is filtered
        // out of the output, so on its own it must not produce an empty Options member.
        NServiceBus.Extensibility.ExtendableOptionsExtensions
            .GetExtensions(options)
            .Set(new TransportTransaction());
        await context.Send("message", options);
        await Verify(context);
    }

    [Fact]
    public async Task Saga()
    {
        var saga = new MySaga
        {
            Data = new()
        };
        var context = new RecordingHandlerContext();

        await saga.Handle(new(), context);

        await Verify(new
        {
            context,
            saga
        });
    }

    [Fact]
    public async Task CompletedSaga()
    {
        var saga = new MySaga
        {
            Data = new()
        };
        saga.MarkCompleted();
        var context = new RecordingHandlerContext();

        await saga.Handle(new(), context);

        await Verify(new
        {
            context,
            saga
        });
    }

    public class MySaga :
        Saga<MySagaData>,
        IHandleMessages<MySagaMessage>,
        IHandleTimeouts<MySagaMessage>
    {
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<MySagaData> mapper)
        {
        }

        public async Task Handle(MySagaMessage message, HandlerContext context)
        {
            Data.Member = "the data";
            await context.Reply(new MySagaMessage());
            await RequestTimeout<MySagaMessage>(context, TimeSpan.FromHours(1));
        }

        public Task Timeout(MySagaMessage state, HandlerContext context) =>
            Task.CompletedTask;

        public void MarkCompleted() =>
            MarkAsComplete();
    }

    public class MySagaMessage;

    public class MySagaData :
        ContainSagaData
    {
        public string? Member { get; set; }
    }
}

public class MyMessage
{
    public string? Property { get; set; }
}

public class PublishMessage
{
    public string? Property { get; set; }
}

public class ReplyMessage
{
    public string? Property { get; set; }
}

public class SendMessage
{
    public string? Property { get; set; }
}