using NServiceBus.Pipeline;

class LogicalMessageConverter :
    WriteOnlyJsonConverter<LogicalMessage>
{
    public override void Write(VerifyJsonWriter writer, LogicalMessage message)
    {
        writer.WriteStartObject();
        writer.WriteMember(message, message.Instance, "Instance");
        writer.WriteMember(message, message.MessageType, "MessageType");
        writer.WriteEndObject();
    }
}