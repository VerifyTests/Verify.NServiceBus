using NServiceBus.Testing;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

class UnsubscriptionConverter :
    WriteOnlyJsonConverter<Unsubscription>
{
    public override void Write(VerifyJsonWriter writer, Unsubscription unsubscription, JsonSerializer serializer)
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