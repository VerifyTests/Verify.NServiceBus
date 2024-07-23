﻿class TimeoutMessageConverter :
    WriteOnlyJsonConverter
{
    public override void Write(VerifyJsonWriter writer, object value)
    {
        writer.WriteStartObject();

        var at = ReflectionMessageHelper.GetAt(value);
        writer.WriteMember(value, at, "At");

        var within = ReflectionMessageHelper.GetWithin(value);
        writer.WriteMember(value, within, "Within");

        OutgoingMessageConverter.WriteBaseMembers(writer, value);

        writer.WriteEndObject();
    }

    public override bool CanConvert(Type type)
    {
        if (!type.IsGenericType)
        {
            return false;
        }

        var typeDefinition = type.GetGenericTypeDefinition();
        return typeDefinition == typeof(TimeoutMessage<>);
    }
}