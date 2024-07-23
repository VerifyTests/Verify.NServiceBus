class MessageHandlerContextConverter :
    WriteOnlyJsonConverter<RecordingHandlerContext>
{
    public override void Write(VerifyJsonWriter writer, RecordingHandlerContext context)
    {
        writer.WriteStartObject();
        writer.WriteMember(context, context.Extensions, "Extensions");
        writer.WriteMember(context, context.Forwarded, "Forwarded");
        writer.WriteMember(context, context.Published, "Published");
        writer.WriteMember(context, context.Replied, "Replied");
        writer.WriteMember(context, context.Sent, "Sent");
        writer.WriteEndObject();
    }
}