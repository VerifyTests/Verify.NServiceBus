using Newtonsoft.Json;
using NServiceBus;
using VerifyTests;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

class SendOptionsConverter :
    WriteOnlyJsonConverter<SendOptions>
{
    public override void WriteJson(JsonWriter writer, SendOptions? options, JsonSerializer serializer)
    {
        if (options == null)
        {
            return;
        }
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
}