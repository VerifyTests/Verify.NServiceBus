class PublishedMessageConverter :
    WriteOnlyJsonConverter<Published>
{
    public override void Write(VerifyJsonWriter writer, Published value)
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