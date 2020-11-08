using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NServiceBus.Testing;
using VerifyTests;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

class UnsubscriptionConverter :
    WriteOnlyJsonConverter<Unsubscription>
{
    public override void WriteJson(JsonWriter writer, Unsubscription? unsubscription, JsonSerializer serializer, IReadOnlyDictionary<string, object> context)
    {
        if (unsubscription == null)
        {
            return;
        }
        writer.WriteStartObject();

        writer.WritePropertyName("MessageType");
        serializer.Serialize(writer, unsubscription.Message);
        var options = unsubscription.Options;
        if (options.HasValue())
        {
            writer.WritePropertyName("Options");
            serializer.Serialize(writer, options);
        }

        writer.WriteEndObject();
    }

    public override bool CanConvert(Type type)
    {
        return typeof(Unsubscription).IsAssignableFrom(type);
    }
}