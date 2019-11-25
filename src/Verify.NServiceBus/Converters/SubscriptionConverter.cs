using System;
using Newtonsoft.Json;
using NServiceBus.Testing;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

class SubscriptionConverter :
    JsonConverter
{
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value == null)
        {
            return;
        }
        var subscription = (Subscription) value;
        writer.WriteStartObject();

        writer.WritePropertyName("MessageType");
        serializer.Serialize(writer, subscription.Message);
        var options = subscription.Options;
        if (options.HasValue())
        {
            writer.WritePropertyName("Options");
            serializer.Serialize(writer, options);
        }

        writer.WriteEndObject();
    }

    public override bool CanConvert(Type type)
    {
        return typeof(Subscription).IsAssignableFrom(type);
    }
}