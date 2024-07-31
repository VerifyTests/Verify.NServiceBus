class RecordingIncomingPhysicalMessageContextConverter :
    WriteOnlyJsonConverter<RecordingIncomingPhysicalMessageContext>
{
    public override void Write(VerifyJsonWriter writer, RecordingIncomingPhysicalMessageContext context)
    {
        writer.WriteStartObject();
        writer.WriteMember(context, context.Extensions, "Extensions");
        writer.WriteMember(context, context.Forwarded, "Forward");
        writer.WriteMember(context, context.Published, "Publish");
        writer.WriteMember(context, context.Replied, "Reply");
        writer.WriteMember(context, context.Sent, "Send");
        writer.WriteMember(context, context.UpdatedMessage, "UpdatedMessage");
        writer.WriteMember(context, context.Message, "Message");
        writer.WriteEndObject();
    }
}