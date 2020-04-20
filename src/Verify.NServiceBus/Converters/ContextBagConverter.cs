using Newtonsoft.Json;
using NServiceBus.Extensibility;
using NServiceBus.Transport;
using Verify;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

class ContextBagConverter :
    WriteOnlyJsonConverter<ContextBag>
{
    public override void WriteJson(JsonWriter writer, ContextBag? bag, JsonSerializer serializer)
    {
        if (bag == null)
        {
            return;
        }

        writer.WriteStartObject();
        foreach (var pair in bag.GetValues())
        {
            if (pair.Value is TransportTransaction)
            {
                continue;
            }
            writer.WritePropertyName(pair.Key);
            serializer.Serialize(writer, pair.Value);
        }
        writer.WriteEndObject();
    }
}