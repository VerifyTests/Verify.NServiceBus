class SentMessageConverter :
    WriteOnlyJsonConverter<Sent>
{
    public override void Write(VerifyJsonWriter writer, Sent value)
    {
        var message = value.Message;

        writer.WriteStartObject();
        var name = message
            .GetType()
            .SimpleName();

        writer.WriteMember(value, message, name);
        var options = value.Options;
        if (options.HasValue())
        {
            writer.WriteMember(value, options, "Options");
        }

        writer.WriteEndObject();
    }
}