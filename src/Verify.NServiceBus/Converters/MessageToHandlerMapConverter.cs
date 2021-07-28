using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using VerifyTests;
using VerifyTests.NServiceBus;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

class MessageToHandlerMapConverter :
    WriteOnlyJsonConverter<MessageToHandlerMap>
{
    public override void WriteJson(JsonWriter writer, MessageToHandlerMap map, JsonSerializer serializer, IReadOnlyDictionary<string, object> context)
    {
        var messagesWithNoHandler = map.Messages
            .Except(map.HandledMessages)
            .ToList();
        writer.WriteStartObject();
        writer.WritePropertyName("MessagesWithNoHandler");
        serializer.Serialize(writer, messagesWithNoHandler);
        writer.WriteEndObject();
    }
}