using System;
using Newtonsoft.Json;
using NServiceBus;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

class SendOptionsConverter :
    JsonConverter
{
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value == null)
        {
            return;
        }
        var options = (SendOptions) value;
        writer.WriteStartObject();

        var deliveryDate = options.GetDeliveryDate();
        if (deliveryDate != null)
        {
            writer.WritePropertyName("DeliveryDate");
            serializer.Serialize(writer, deliveryDate);
        }
        var deliveryDelay = options.GetDeliveryDelay();
        if (deliveryDelay != null)
        {
            writer.WritePropertyName("DeliveryDelay");
            serializer.Serialize(writer, deliveryDelay);
        }
        ExtendableOptionsConverter.WriteBaseMembers(writer, serializer, options);

        writer.WriteEndObject();
    }

    public override bool CanConvert(Type type)
    {
        return typeof(SendOptions).IsAssignableFrom(type);
    }
}