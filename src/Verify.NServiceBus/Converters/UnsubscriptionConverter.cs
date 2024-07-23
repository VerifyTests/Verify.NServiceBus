class UnsubscriptionConverter :
    WriteOnlyJsonConverter<Unsubscribed>
{
    public override void Write(VerifyJsonWriter writer, Unsubscribed unsubscribed)
    {
        writer.WriteStartObject();

        writer.WriteMember(unsubscribed, unsubscribed.EventType, "EventType");
        var options = unsubscribed.Options;
        if (options.HasValue())
        {
            writer.WriteMember(unsubscribed, options, "Options");
        }

        writer.WriteEndObject();
    }
}