class LogEntryConverter :
    WriteOnlyJsonConverter<LogEntry>
{
    public override void Write(VerifyJsonWriter writer, LogEntry entry)
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