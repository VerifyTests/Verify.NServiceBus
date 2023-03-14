class IncomingMessageConverter :
    WriteOnlyJsonConverter<IncomingMessage>
{
    public override void Write(VerifyJsonWriter writer, IncomingMessage message)
    {
        writer.WriteStartObject();
        writer.WriteMember(message, message.MessageId, "MessageId");
        writer.WriteMember(message, message.NativeMessageId, "NativeMessageId");
        var headers = ExtendableOptionsHelper.CleanedHeaders(message.Headers);
        headers.Remove("MessageId");
        writer.WriteMember(message, headers, "Headers");
        writer.WriteMember(message, message.Body, "Body");
        writer.WriteEndObject();
    }
}