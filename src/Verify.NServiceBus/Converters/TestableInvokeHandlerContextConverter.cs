class TestableInvokeHandlerContextConverter :
    WriteOnlyJsonConverter<TestableInvokeHandlerContext>
{
    public override void Write(VerifyJsonWriter writer, TestableInvokeHandlerContext context)
    {
        writer.WriteStartObject();

        var headers = ExtendableOptionsHelper.CleanedHeaders(context.Headers);
        writer.WriteListOrSingleMember(context, headers, "Headers");
        var messageHeaders = ExtendableOptionsHelper.CleanedHeaders(context.MessageHeaders);
        writer.WriteListOrSingleMember(context, messageHeaders, "MessageHeaders");
        writer.WriteListOrSingleMember(context, context.PublishedMessages, "Published");
        writer.WriteListOrSingleMember(context, context.RepliedMessages, "Replied");
        writer.WriteListOrSingleMember(context, context.SentMessages, "Sent");
        writer.WriteListOrSingleMember(context, context.ForwardedMessages, "Forwarded");
        writer.WriteListOrSingleMember(context, context.TimeoutMessages, "Timeouts");

        writer.WriteMember(context, context.Extensions, "Extensions");
        if (context.DoNotContinueDispatchingCurrentMessageToHandlersWasCalled)
        {
            writer.WriteMember(context, true, "DoNotContinueDispatchingCurrentMessageToHandlersWasCalled");
        }

        if (context.HandlerInvocationAborted)
        {
            writer.WriteMember(context, true, "HandlerInvocationAborted");
        }

        writer.WriteEndObject();
    }
}