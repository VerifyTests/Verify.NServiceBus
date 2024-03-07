namespace VerifyTests.NServiceBus;

public class RecordingMessageSession :
    TestableMessageSession
{
    public override Task Publish(object message, PublishOptions options, Cancel cancel = default)
    {
        RecordingState.Publish(message, options);
        return base.Publish(message, options, cancel);
    }

    public override Task Send(object message, SendOptions options, Cancel cancel = default)
    {
        RecordingState.Send(message, options);
        return base.Send(message, options, cancel);
    }

    public override Task Unsubscribe(Type eventType, UnsubscribeOptions options, Cancel cancel = default)
    {
        RecordingState.Unsubscribe(eventType, options);
        return base.Unsubscribe(eventType, options, cancel);
    }

    public override Task Subscribe(Type eventType, SubscribeOptions options, Cancel cancel = default)
    {
        RecordingState.Subscribe(eventType, options);
        return base.Subscribe(eventType, options, cancel);
    }
}