class RecordedMessageConverter :
    WriteOnlyJsonConverter<RecordedMessage>
{
    public override void Write(VerifyJsonWriter writer, RecordedMessage entry)
    {
        writer.WriteStartObject();

        if (entry.Message != null)
        {
            writer.WriteMember(entry, entry.Message, entry.Message.GetType().Name);
        }

        writer.WriteMember(entry, entry.EventType, "EventType");
        writer.WriteMember(entry, entry.Options, "Options");
        writer.WriteEndObject();
    }
}