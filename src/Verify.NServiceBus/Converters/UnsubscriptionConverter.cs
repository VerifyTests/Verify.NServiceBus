using NServiceBus.Testing;

class UnsubscriptionConverter :
    WriteOnlyJsonConverter<Unsubscription>
{
    public override void Write(VerifyJsonWriter writer, Unsubscription unsubscription)
    {
        writer.WriteStartObject();

        writer.WriteProperty(unsubscription, unsubscription.Message, "MessageType");
        var options = unsubscription.Options;
        if (options.HasValue())
        {
            writer.WriteProperty(unsubscription, options, "Options");
        }

        writer.WriteEndObject();
    }
}