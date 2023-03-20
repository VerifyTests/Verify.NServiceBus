class MessageProcessingContextConverter :
    WriteOnlyJsonConverter<TestableMessageProcessingContext>
{
    public override void Write(VerifyJsonWriter writer, TestableMessageProcessingContext context)
    {
        writer.WriteStartObject();

        writer.WriteMember(context, context.RepliedMessages, "RepliedMessages");
        writer.WriteMember(context, context.ForwardedMessages, "ForwardedMessages");
        writer.WriteMember(context, context.SentMessages, "SentMessages");
        if (context.ReplyToAddress != "reply address")
        {
            writer.WriteMember(context, context.ReplyToAddress, "ReplyToAddress");
        }
        writer.WriteMember(context, context.PublishedMessages, "PublishedMessages");
        writer.WriteMember(context, context.TimeoutMessages, "TimeoutMessages");
        writer.WriteMember(context, context.Extensions, "Extensions");

        writer.WriteEndObject();
    }
}