using NServiceBus;

class SendOptionsConverter :
    WriteOnlyJsonConverter<SendOptions>
{
    public override void Write(VerifyJsonWriter writer, SendOptions options)
    {
        writer.WriteStartObject();

        var deliveryDate = options.GetDeliveryDate();
        writer.WriteMember(options, deliveryDate, "DeliveryDate");
        var deliveryDelay = options.GetDeliveryDelay();
        writer.WriteMember(options, deliveryDelay, "DeliveryDelay");

        ExtendableOptionsConverter.WriteBaseMembers(writer, options);

        writer.WriteEndObject();
    }
}