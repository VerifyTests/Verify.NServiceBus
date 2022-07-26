using NServiceBus.Testing;

class SubscriptionConverter :
    WriteOnlyJsonConverter<Subscription>
{
    public override void Write(VerifyJsonWriter writer, Subscription subscription)
    {
        writer.WriteStartObject();

        writer.WriteMember(subscription, subscription.Message, "MessageType");

        var options = subscription.Options;
        if (options.HasValue())
        {
            writer.WriteMember(subscription, options, "Options");
        }

        writer.WriteEndObject();
    }
}