﻿using NServiceBus.Testing;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

class TimeoutMessageConverter :
    WriteOnlyJsonConverter
{
    public override void Write(VerifyJsonWriter writer, object value, JsonSerializer serializer)
    {
        writer.WriteStartObject();

        var at = OutgoingMessageHelper.GetAt(value);
        writer.WriteProperty(value, at, "At");

        var within = OutgoingMessageHelper.GetWithin(value);
        writer.WriteProperty(value, within, "Within");

        OutgoingMessageConverter.WriteBaseMembers(writer, value, serializer);

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