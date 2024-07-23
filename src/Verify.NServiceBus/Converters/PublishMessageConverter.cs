class PublishedMessageConverter :
    WriteOnlyJsonConverter
{
    static MethodInfo innerWrite;

    static PublishedMessageConverter() =>
        innerWrite = typeof(PublishedMessageConverter)
            .GetMethod(nameof(InnerWrite), BindingFlags.Static | BindingFlags.NonPublic)!;

    public override void Write(VerifyJsonWriter writer, object value)
    {
        writer.WriteStartObject();

        var genericArguments = value
            .GetType()
            .GetGenericArguments();
        innerWrite
            .MakeGenericMethod(genericArguments)
            .Invoke(null, [writer, value]);

        writer.WriteEndObject();
    }

    static void InnerWrite<T>(VerifyJsonWriter writer, PublishedMessage<T> value)
        where T : notnull =>
        OutgoingMessageConverter.WriteBaseMembers(writer, value);

    public override bool CanConvert(Type type) =>
        type.IsGenericType &&
        type.GetGenericTypeDefinition() == typeof(PublishedMessage<>);
}