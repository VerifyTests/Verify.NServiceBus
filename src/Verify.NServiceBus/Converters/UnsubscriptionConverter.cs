using System.Collections.Generic;
using Newtonsoft.Json;
using NServiceBus.Testing;
using VerifyTests;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

class UnsubscriptionConverter :
    WriteOnlyJsonConverter<Unsubscription>
{
    public override void WriteJson(JsonWriter writer, Unsubscription unsubscription, JsonSerializer serializer, IReadOnlyDictionary<string, object> context)
    {
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
}