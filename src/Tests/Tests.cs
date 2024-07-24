public class Tests
{
    class MyHandlerWithLogging :
        IHandleMessages<MyMessage>
    {
        static ILog logger = LogManager.GetLogger<MyHandlerWithLogging>();

        public Task Handle(MyMessage message, HandlerContext context)
        {
            logger.Warn("The log message");
            return Task.CompletedTask;
        }
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