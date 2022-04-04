class SendOptionsConverter :
    WriteOnlyJsonConverter<SendOptions>
{
    public override void Write(VerifyJsonWriter writer, SendOptions options)
    {
        writer.WriteStartObject();

        var deliveryDate = options.GetDeliveryDate();
        writer.WriteProperty(options, deliveryDate, "DeliveryDate");
        var deliveryDelay = options.GetDeliveryDelay();
        writer.WriteProperty(options, deliveryDelay, "DeliveryDelay");

        ExtendableOptionsConverter.WriteBaseMembers(writer, options);

        writer.WriteEndObject();
    }
}