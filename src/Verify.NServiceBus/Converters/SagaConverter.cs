class SagaConverter :
    WriteOnlyJsonConverter
{
    public override void Write(VerifyJsonWriter writer, object value)
    {
        var data = (Saga)value;
        writer.Serialize(data.Entity);
    }

    public override bool CanConvert(Type type) =>
        type.IsAssignableTo(typeof(Saga));
}