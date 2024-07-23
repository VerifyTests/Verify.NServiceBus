class RecordingMessageSessionConverter :
    WriteOnlyJsonConverter<RecordingMessageSession>
{
    public override void Write(VerifyJsonWriter writer, RecordingMessageSession context)
    {
        writer.WriteStartObject();

        writer.WriteMember(context, context.Published, "Publish");
        writer.WriteMember(context, context.Sent, "Send");
        writer.WriteMember(context, context.Subscribed, "Subscribe");
        writer.WriteMember(context, context.Unsubscribed, "Unsubscribe");

        writer.WriteEndObject();
    }
}