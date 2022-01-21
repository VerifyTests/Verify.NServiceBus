using NServiceBus.Testing;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

class SubscriptionConverter :
    WriteOnlyJsonConverter<Subscription>
{
    public override void Write(VerifyJsonWriter writer, Subscription subscription, JsonSerializer serializer)
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