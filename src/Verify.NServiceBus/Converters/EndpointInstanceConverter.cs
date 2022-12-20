class EndpointInstanceConverter :
    WriteOnlyJsonConverter<TestableEndpointInstance>
{
    public override void Write(VerifyJsonWriter writer, TestableEndpointInstance instance)
    {
        writer.WriteStartObject();

        MessageSessionConverter.WriteMembers(writer, instance);

        writer.WriteEndObject();
    }
}