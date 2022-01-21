using NServiceBus.Testing;

class SubscriptionConverter :
    WriteOnlyJsonConverter<Subscription>
{
    public override void Write(VerifyJsonWriter writer, Subscription subscription)
    {
        writer.WriteStartObject();

        writer.WriteProperty(subscription, subscription.Message, "MessageType");

        var options = subscription.Options;
        if (options.HasValue())
        {
            writer.WriteProperty(subscription, options, "Options");
        }

        writer.WriteEndObject();
    }
}