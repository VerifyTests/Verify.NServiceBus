using NServiceBus.Testing;

class UnsubscriptionConverter :
    WriteOnlyJsonConverter<Unsubscription>
{
    public override void Write(VerifyJsonWriter writer, Unsubscription unsubscription)
    {
        writer.WriteStartObject();

        writer.WriteMember(unsubscription, unsubscription.Message, "MessageType");
        var options = unsubscription.Options;
        if (options.HasValue())
        {
            writer.WriteMember(unsubscription, options, "Options");
        }

        writer.WriteEndObject();
    }
}