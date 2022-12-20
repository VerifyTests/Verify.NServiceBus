class MessageSessionConverter :
    WriteOnlyJsonConverter<TestableMessageSession>
{
    public override void Write(VerifyJsonWriter writer, TestableMessageSession instance)
    {
        writer.WriteStartObject();

        WriteMembers(writer, instance);

        writer.WriteEndObject();
    }

    internal static void WriteMembers(VerifyJsonWriter writer, TestableMessageSession instance)
    {
        writer.WriteListOrSingleMember(instance, instance.Subscriptions, "Subscriptions");
        writer.WriteListOrSingleMember(instance, instance.Unsubscription, "Unsubscription");
        writer.WriteListOrSingleMember(instance, instance.PublishedMessages, "PublishedMessages");
        writer.WriteListOrSingleMember(instance, instance.SentMessages, "SentMessages");
        writer.WriteListOrSingleMember(instance, instance.TimeoutMessages, "TimeoutMessages");
    }
}