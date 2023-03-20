class RoutingContextConverter :
    WriteOnlyJsonConverter<TestableRoutingContext>
{
    public override void Write(VerifyJsonWriter writer, TestableRoutingContext context)
    {
        writer.WriteStartObject();

        writer.WriteMember(context, context.Message, "Message");
        writer.WriteMember(context, context.Extensions, "Extensions");

        writer.WriteEndObject();
    }
}