using System.Linq;
using Newtonsoft.Json;
using Verify;
using Verify.NServiceBus;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

class MessageToHandlerMapConverter :
    WriteOnlyJsonConverter<MessageToHandlerMap>
{
    public override void WriteJson(JsonWriter writer, MessageToHandlerMap? map, JsonSerializer serializer)
    {
        if (map == null)
        {
            return;
        }

        var messagesWithNoHandler = map.Messages.Except(map.Handlers.SelectMany(x => x.Value)).ToList();
        if (!messagesWithNoHandler.Any())
        {
            return;
        }

        writer.WriteStartObject();
        writer.WritePropertyName("MessagesWithNoHandler");
        serializer.Serialize(writer, messagesWithNoHandler);
        writer.WriteEndObject();
    }
}