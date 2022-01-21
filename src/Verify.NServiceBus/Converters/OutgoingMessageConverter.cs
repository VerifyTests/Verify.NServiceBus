using NServiceBus.Testing;
using SimpleInfoName;

class OutgoingMessageConverter :
    WriteOnlyJsonConverter
{
    public override void Write(VerifyJsonWriter writer, object value)
    {
        writer.WriteStartObject();
        WriteBaseMembers(writer, value);
        writer.WriteEndObject();
    }

    public static void WriteBaseMembers(VerifyJsonWriter writer, object value)
    {
        var message = OutgoingMessageHelper.GetMessage(value);

        var type = message.GetType();

        var name = type.SimpleName();
        writer.WriteProperty(value, message, name);

        var options = OutgoingMessageHelper.GetOptions(value);
        if (options.HasValue())
        {
            writer.WriteProperty(value, options, "Options");
        }
    }

    public override bool CanConvert(Type type)
    {
        var baseType = type.BaseType;
        if (baseType is not {IsGenericType: true})
        {
            return false;
        }

        var typeDefinition = baseType.GetGenericTypeDefinition();
        return typeDefinition == typeof(OutgoingMessage<,>);
    }
}