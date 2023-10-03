namespace VerifyTests.NServiceBus;

public class RecordingMessageSession :
    TestableMessageSession
{
    RecordingState state = new();

    public override Task Publish(object message, PublishOptions options, Cancel cancel = default)
    {
        state.Publish(message, options);
        return base.Publish(message, options, cancel);
    }


    public override Task Send(object message, SendOptions options, Cancel cancel = default)
    {
        state.Send(message, options);
        return base.Send(message, options, cancel);
    }

    public override Task Unsubscribe(Type eventType, UnsubscribeOptions options, Cancel cancel = default)
    {
        state.Unsubscribe(eventType, options);
        return base.Unsubscribe(eventType, options, cancel);
    }

    public override Task Subscribe(Type eventType, SubscribeOptions options, Cancel cancel = default)
    {
        state.Subscribe(eventType, options);
        return base.Subscribe(eventType, options, cancel);
    }
}