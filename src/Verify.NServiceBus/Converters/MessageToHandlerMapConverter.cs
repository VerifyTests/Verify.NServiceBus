using VerifyTests.NServiceBus;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

class MessageToHandlerMapConverter :
    WriteOnlyJsonConverter<MessageToHandlerMap>
{
    public override void Write(VerifyJsonWriter writer, MessageToHandlerMap map, JsonSerializer serializer)
    {
        var withNoHandler = map.Messages
            .Except(map.HandledMessages)
            .ToList();
        writer.WriteStartObject();
        writer.WriteProperty(map, withNoHandler, "MessagesWithNoHandler");
        writer.WriteEndObject();
    }
}