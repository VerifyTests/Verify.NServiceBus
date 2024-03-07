namespace VerifyTests.NServiceBus;

public class RecordingHandlerContext :
    TestableMessageHandlerContext
{
    public override Task Publish(object message, PublishOptions options)
    {
        RecordingState.Publish(message, options);
        return base.Publish(message, options);
    }

    public override Task Reply(object message, ReplyOptions options)
    {
        RecordingState.Reply(message, options);
        return base.Reply(message, options);
    }

    public override Task Send(object message, SendOptions options)
    {
        RecordingState.Send(message, options);
        return base.Send(message, options);
    }
}