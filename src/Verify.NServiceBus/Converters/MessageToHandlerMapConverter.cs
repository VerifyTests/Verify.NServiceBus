using VerifyTests.NServiceBus;

class MessageToHandlerMapConverter :
    WriteOnlyJsonConverter<MessageToHandlerMap>
{
    public override void Write(VerifyJsonWriter writer, MessageToHandlerMap map)
    {
        var withNoHandler = map.Messages
            .Except(map.HandledMessages)
            .ToList();
        writer.WriteStartObject();
        writer.WriteProperty(map, withNoHandler, "MessagesWithNoHandler");
        writer.WriteEndObject();
    }
}