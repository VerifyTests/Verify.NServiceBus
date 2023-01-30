class SagaConverter :
    WriteOnlyJsonConverter
{
    public override void Write(VerifyJsonWriter writer, object value)
    {
        var saga = (Saga) value;

        if (saga.Completed)
        {
            writer.WriteStartObject();

            writer.WriteMember(value, saga.Entity, "Data");
            writer.WriteMember(saga, true, "Completed");

            writer.WriteEndObject();
        }
        else
        {
            writer.Serialize(saga.Entity);
        }
    }

    public override bool CanConvert(Type type) =>
        type.IsAssignableTo(typeof(Saga));
}