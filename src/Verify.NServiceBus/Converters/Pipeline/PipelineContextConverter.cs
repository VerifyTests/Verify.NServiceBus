class PipelineContextConverter :
    WriteOnlyJsonConverter<TestablePipelineContext>
{
    public override void Write(VerifyJsonWriter writer, TestablePipelineContext context)
    {
        writer.WriteStartObject();

        WriteMembers(writer, context);

        writer.WriteEndObject();
    }

    public static void WriteMembers(VerifyJsonWriter writer, TestablePipelineContext context)
    {
        writer.WriteListOrSingleMember(context, context.PublishedMessages, "Published");
        writer.WriteListOrSingleMember(context, context.SentMessages, "Sent");
        writer.WriteListOrSingleMember(context, context.TimeoutMessages, "Timeouts");

        writer.WriteMember(context, context.Extensions, "Extensions");
    }
}