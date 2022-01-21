using NServiceBus.Extensibility;
using NServiceBus.Transport;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

class ContextBagConverter :
    WriteOnlyJsonConverter<ContextBag>
{
    public override void Write(VerifyJsonWriter writer, ContextBag bag, JsonSerializer serializer)
    {
        writer.WriteStartObject();
        foreach (var pair in bag.GetValues())
        {
            if (pair.Value is TransportTransaction)
            {
                continue;
            }
            writer.WriteProperty(bag, pair.Value, pair.Key);
        }
        writer.WriteEndObject();
    }
}