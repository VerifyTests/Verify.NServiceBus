﻿class TimeoutMessageConverter :
    WriteOnlyJsonConverter
{
    static MethodInfo innerWrite;

    static TimeoutMessageConverter() =>
        innerWrite = typeof(TimeoutMessageConverter)
            .GetMethod(nameof(InnerWrite), BindingFlags.Static | BindingFlags.NonPublic)!;

    public override void Write(VerifyJsonWriter writer, object value)
    {
        writer.WriteStartObject();

        var genericArguments =
            value
                .GetType()
                .GetGenericArguments();
        innerWrite
            .MakeGenericMethod(genericArguments)
            .Invoke(null, [writer, value]);

        writer.WriteEndObject();
    }

    static void InnerWrite<T>(VerifyJsonWriter writer, TimeoutMessage<T> value)
        where T : notnull
    {
        writer.WriteMember(value, value.At, "At");

        writer.WriteMember(value, value.Within, "Within");

        OutgoingMessageConverter.WriteBaseMembers(writer, value);
    }

    public override bool CanConvert(Type type) =>
        type.IsGenericType &&
        type.GetGenericTypeDefinition() == typeof(TimeoutMessage<>);
}