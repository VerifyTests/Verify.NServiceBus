class RecordingHandlerContextConverter :
    WriteOnlyJsonConverter<RecordingHandlerContext>
{
    public override void Write(VerifyJsonWriter writer, RecordingHandlerContext context)
    {
        writer.WriteStartObject();
        writer.WriteMember(context, context.Extensions, "Extensions");
        writer.WriteMember(context, context.Forwarded, "Forward");
        writer.WriteMember(context, context.Published, "Publish");
        writer.WriteMember(context, context.Replied, "Reply");
        writer.WriteMember(context, context.Sent, "Send");
        writer.WriteEndObject();
    }
}