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

        writer.WriteMember(options, options.GetMessageId(), "MessageId");

        writer.WriteMember(options, options.GetCleanedHeaders(), "Headers");

        ExtendableOptionsConverter.WriteExtensions(writer, options);

        writer.WriteEndObject();
    }
}