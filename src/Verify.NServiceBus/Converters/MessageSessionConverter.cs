class MessageSessionConverter :
    WriteOnlyJsonConverter<TestableMessageSession>
{
    public override void Write(VerifyJsonWriter writer, TestableMessageSession context)
    {
        writer.WriteStartObject();

        writer.WriteMember(context, context.TimeoutMessages, "TimeoutMessages");
        writer.WriteMember(context, context.SentMessages, "SentMessages");
        writer.WriteMember(context, context.Subscriptions, "Subscriptions");
        writer.WriteMember(context, context.Unsubscription, "Unsubscriptions");

        writer.WriteEndObject();
    }
}