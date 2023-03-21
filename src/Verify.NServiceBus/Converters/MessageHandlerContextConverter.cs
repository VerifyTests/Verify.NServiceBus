class MessageHandlerContextConverter :
    WriteOnlyJsonConverter<TestableMessageHandlerContext>
{
    public override void Write(VerifyJsonWriter writer, TestableMessageHandlerContext context)
    {
        writer.WriteStartObject();
        if (context.HandlerInvocationAborted)
        {
            writer.WriteMember(context, true, "HandlerInvocationAborted");
        }

        writer.WriteMember(context, context.RepliedMessages, "RepliedMessages");
        writer.WriteMember(context, context.TimeoutMessages, "TimeoutMessages");
        writer.WriteMember(context, context.ForwardedMessages, "ForwardedMessages");
        writer.WriteMember(context, context.SentMessages, "SentMessages");
        writer.WriteMember(context, context.PublishedMessages, "PublishedMessages");
        writer.WriteMember(context, context.Extensions, "Extensions");
        if (context.ReplyToAddress != "reply address")
        {
            writer.WriteMember(context, context.ReplyToAddress, "ReplyToAddress");
        }

        writer.WriteEndObject();
    }
}