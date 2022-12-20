class AuditContextConverter :
    WriteOnlyJsonConverter<TestableAuditContext>
{
    public override void Write(VerifyJsonWriter writer, TestableAuditContext context)
    {
        writer.WriteStartObject();
        writer.WriteMember(context, context.Message, "Message");
        writer.WriteMember(context, context.AuditAddress, "AuditAddress");
        writer.WriteListOrSingleMember(context, context.AuditMetadata, "AuditMetadata");
        writer.WriteMember(context, context.AuditAction, "AuditAction");
        BehaviorContextConverter.WriteMembers(writer, context);
        writer.WriteEndObject();
    }
}