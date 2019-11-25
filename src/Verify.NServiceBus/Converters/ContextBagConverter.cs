using System;
using Newtonsoft.Json;
using NServiceBus.Extensibility;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

class ContextBagConverter :
    JsonConverter
{
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value == null)
        {
            return;
        }

        var contextBag = (ContextBag) value;
        writer.WriteStartObject();
        foreach (var pair in contextBag.GetValues())
        {
            writer.WritePropertyName(pair.Key);
            serializer.Serialize(writer, pair.Value);
        }
        writer.WriteEndObject();
    }

    public override bool CanConvert(Type type)
    {
        return typeof(ContextBag).IsAssignableFrom(type);
    }
}