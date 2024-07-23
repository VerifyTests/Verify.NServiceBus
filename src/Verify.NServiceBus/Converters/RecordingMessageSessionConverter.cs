class RecordingMessageSessionConverter :
    WriteOnlyJsonConverter<RecordingMessageSession>
{
    public override void Write(VerifyJsonWriter writer, RecordingMessageSession context)
    {
        writer.WriteStartObject();

        writer.WriteMember(context, context.Published, "Published");
        writer.WriteMember(context, context.Sent, "Sent");
        writer.WriteMember(context, context.Subscribed, "Subscribed");
        writer.WriteMember(context, context.Unsubscribed, "Unsubscribed");

        writer.WriteEndObject();
    }
}