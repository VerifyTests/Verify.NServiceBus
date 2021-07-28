using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NServiceBus.Testing;
using VerifyTests;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

class TimeoutMessageConverter :
    WriteOnlyJsonConverter
{
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer, IReadOnlyDictionary<string, object> context)
    {
        writer.WriteStartObject();

        var at = OutgoingMessageHelper.GetAt(value);
        if (at != null)
        {
            writer.WritePropertyName("At");
            serializer.Serialize(writer, at);
        }

        var within = OutgoingMessageHelper.GetWithin(value);
        if (within != null)
        {
            writer.WritePropertyName("Within");
            serializer.Serialize(writer, within);
        }

        OutgoingMessageConverter.WriteBaseMembers(writer, value, serializer);

        writer.WriteEndObject();
    }

    public override bool CanConvert(Type type)
    {
        if (type.IsGenericType)
        {
            var typeDefinition = type.GetGenericTypeDefinition();
            return typeDefinition == typeof(TimeoutMessage<>);
        }
        return false;
    }
}