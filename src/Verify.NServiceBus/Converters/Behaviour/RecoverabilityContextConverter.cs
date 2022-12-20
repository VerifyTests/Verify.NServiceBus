class RecoverabilityContextConverter :
    WriteOnlyJsonConverter<TestableRecoverabilityContext>
{
    public override void Write(VerifyJsonWriter writer, TestableRecoverabilityContext context)
    {
        writer.WriteStartObject();
        writer.WriteMember(context, context.FailedMessage, "FailedMessage");
        writer.WriteMember(context, context.Exception, "Exception");
        writer.WriteMember(context, context.ReceiveAddress, "ReceiveAddress");
        writer.WriteMember(context, context.ImmediateProcessingFailures, "ImmediateProcessingFailures");
        writer.WriteMember(context, context.DelayedDeliveriesPerformed, "DelayedDeliveriesPerformed");
        writer.WriteMember(context, context.RecoverabilityAction, "RecoverabilityAction");
        BehaviorContextConverter.WriteMembers(writer, context);
        writer.WriteEndObject();
    }
}