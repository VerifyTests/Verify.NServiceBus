class PipelineContextConverter :
    WriteOnlyJsonConverter<TestablePipelineContext>
{
    public override void Write(VerifyJsonWriter writer, TestablePipelineContext context)
    {
        writer.WriteStartObject();

        writer.WriteMember(context, context.PublishedMessages, "PublishedMessages");
        writer.WriteMember(context, context.SentMessages, "SentMessages");
        writer.WriteMember(context, context.TimeoutMessages, "TimeoutMessages");
        writer.WriteMember(context, context.Extensions, "Extensions");

        writer.WriteEndObject();
    }
}