using NServiceBus.Extensibility;
using NServiceBus.Transport;

class ContextBagConverter :
    WriteOnlyJsonConverter<ContextBag>
{
    public override void Write(VerifyJsonWriter writer, ContextBag bag)
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