class TestableEndpointInstanceConverter :
    WriteOnlyJsonConverter<TestableEndpointInstance>
{
    public override void Write(VerifyJsonWriter writer, TestableEndpointInstance instance)
    {
        writer.WriteStartObject();

        writer.WriteListOrSingleMember(instance, instance.Subscriptions, "Subscriptions");
        writer.WriteListOrSingleMember(instance, instance.Unsubscription, "Unsubscription");
        writer.WriteListOrSingleMember(instance, instance.PublishedMessages, "PublishedMessages");
        writer.WriteListOrSingleMember(instance, instance.SentMessages, "SentMessages");
        writer.WriteListOrSingleMember(instance, instance.TimeoutMessages, "TimeoutMessages");

        writer.WriteEndObject();
    }
}