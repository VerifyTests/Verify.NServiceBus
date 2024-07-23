class RepliedMessageConverter :
    WriteOnlyJsonConverter<Replied>
{
    public override void Write(VerifyJsonWriter writer, Replied value)
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