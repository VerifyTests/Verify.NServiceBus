class InvokeHandlerContextConverter :
    WriteOnlyJsonConverter<TestableInvokeHandlerContext>
{
    public override void Write(VerifyJsonWriter writer, TestableInvokeHandlerContext context)
    {
        writer.WriteStartObject();
        WriteMembers(writer, context);
        writer.WriteEndObject();
    }

    internal static void WriteMembers(VerifyJsonWriter writer, TestableInvokeHandlerContext context)
    {
        IncomingContextConverter.WriteMembers(writer, context);
        var headers = ExtendableOptionsHelper.CleanedHeaders(context.Headers);
        writer.WriteListOrSingleMember(context, headers, "Headers");
        if (context.HandlerInvocationAborted)
        {
            writer.WriteMember(context, true, "HandlerInvocationAborted");
        }
    }
}