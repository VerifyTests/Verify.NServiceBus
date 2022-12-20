class TestableEndpointInstanceConverter :
    WriteOnlyJsonConverter<TestableEndpointInstance>
{
    public override void Write(VerifyJsonWriter writer, TestableEndpointInstance instance)
    {
        writer.WriteStartObject();

        writer.WriteMember(instance, instance.Subscriptions, "Subscriptions");
        writer.WriteMember(instance, instance.Unsubscription, "Unsubscription");
        writer.WriteMember(instance, instance.PublishedMessages, "PublishedMessages");
        writer.WriteMember(instance, instance.SentMessages, "SentMessages");
        writer.WriteMember(instance, instance.TimeoutMessages, "TimeoutMessages");

        writer.WriteEndObject();
    }
}