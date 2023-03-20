class InvokeHandlerContextConverter :
    WriteOnlyJsonConverter<TestableInvokeHandlerContext>
{
    public override void Write(VerifyJsonWriter writer, TestableInvokeHandlerContext context)
    {
        writer.WriteStartObject();
        writer.WriteMember(context, context.PublishedMessages, "PublishedMessages");
        writer.WriteMember(context, context.SentMessages, "SentMessages");
        writer.WriteMember(context, context.TimeoutMessages, "TimeoutMessages");
        writer.WriteMember(context, context.Extensions, "Extensions");
        writer.WriteMember(context, context.ForwardedMessages, "ForwardedMessages");
        writer.WriteMember(context, context.RepliedMessages, "RepliedMessages");
        if (context.ReplyToAddress != "reply address")
        {
            writer.WriteMember(context, context.ReplyToAddress, "ReplyToAddress");
        }
        writer.WriteEndObject();
    }
}