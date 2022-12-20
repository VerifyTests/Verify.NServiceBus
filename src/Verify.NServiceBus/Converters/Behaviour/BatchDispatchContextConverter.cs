class BatchDispatchContextConverter :
    WriteOnlyJsonConverter<TestableBatchDispatchContext>
{
    public override void Write(VerifyJsonWriter writer, TestableBatchDispatchContext context)
    {
        writer.WriteStartObject();
        writer.WriteMember(context, context.Operations, "Operations");
        BehaviorContextConverter.WriteMembers(writer, context);
        writer.WriteEndObject();
    }
}