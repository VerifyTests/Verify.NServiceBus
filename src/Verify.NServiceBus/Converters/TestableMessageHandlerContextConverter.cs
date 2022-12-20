class TestableMessageHandlerContextConverter :
    WriteOnlyJsonConverter<TestableMessageHandlerContext>
{
    public override void Write(VerifyJsonWriter writer, TestableMessageHandlerContext context)
    {
        writer.WriteStartObject();

        var headers = ExtendableOptionsHelper.CleanedHeaders(context.Headers);
        if (headers.Count == 1)
        {
            writer.WriteMember(context, headers.Single(), "Headers");
        }
        else
        {
            writer.WriteMember(context, headers, "Headers");
        }

        var messageHeaders = ExtendableOptionsHelper.CleanedHeaders(context.MessageHeaders);
        if (messageHeaders.Count == 1)
        {
            writer.WriteMember(context, messageHeaders.Single(), "MessageHeaders");
        }
        else
        {
            writer.WriteMember(context, messageHeaders, "MessageHeaders");
        }

        var published = context.PublishedMessages;
        if (published.Length == 1)
        {
            writer.WriteMember(context, published.Single(), "Published");
        }
        else
        {
            writer.WriteMember(context, published, "Published");
        }

        var replied = context.RepliedMessages;
        if (replied.Length == 1)
        {
            writer.WriteMember(context, replied.Single(), "Replied");
        }
        else
        {
            writer.WriteMember(context, replied, "Replied");
        }

        var sent = context.SentMessages;
        if (sent.Length == 1)
        {
            writer.WriteMember(context, sent.Single(), "Sent");
        }
        else
        {
            writer.WriteMember(context, sent, "Sent");
        }

        var forwarded = context.ForwardedMessages;
        if (forwarded.Length == 1)
        {
            writer.WriteMember(context, forwarded.Single(), "Forwarded");
        }
        else
        {
            writer.WriteMember(context, forwarded, "Forwarded");
        }

        writer.WriteMember(context, context.Extensions, "Extensions");

        writer.WriteEndObject();
    }
}