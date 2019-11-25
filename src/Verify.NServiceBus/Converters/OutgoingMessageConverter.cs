using System;
using Newtonsoft.Json;
using NServiceBus.Testing;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

class OutgoingMessageConverter :
    JsonConverter
{
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value == null)
        {
            return;
        }
        writer.WriteStartObject();
        WriteBaseMembers(writer, value, serializer);
        writer.WriteEndObject();
    }

    public static void WriteBaseMembers(JsonWriter writer, object value, JsonSerializer serializer)
    {
        var message = OutgoingMessageHelper.GetMessage(value);

        var type = message.GetType();

        writer.WritePropertyName(type.FullName);
        serializer.Serialize(writer, message);

        var options = OutgoingMessageHelper.GetOptions(value);
        if (options.HasValue())
        {
            writer.WritePropertyName("Options");
            serializer.Serialize(writer, options);
        }
    }

    public override bool CanConvert(Type type)
    {
        var baseType = type.BaseType;
        if (baseType != null && baseType.IsGenericType)
        {
            var typeDefinition = baseType.GetGenericTypeDefinition();
            return typeDefinition == typeof(OutgoingMessage<,>);
        }

        return false;
    }
}