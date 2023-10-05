namespace VerifyTests.NServiceBus;

public class RecordingHandlerContext :
    TestableMessageHandlerContext
{
    RecordingState state = new();

    public override Task Publish(object message, PublishOptions options)
    {
        state.Publish(message, options);
        return base.Publish(message, options);
    }

    public override Task Reply(object message, ReplyOptions options)
    {
        state.Reply(message, options);
        return base.Reply(message, options);
    }

    public override Task Send(object message, SendOptions options)
    {
        state.Send(message, options);
        return base.Send(message, options);
    }
}