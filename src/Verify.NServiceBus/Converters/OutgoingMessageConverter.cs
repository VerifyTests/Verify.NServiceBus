class OutgoingMessageConverter :
    WriteOnlyJsonConverter
{
    static MethodInfo writeBaseMembers;

    static OutgoingMessageConverter()
    {
        var writeBaseMembersName = nameof(WriteBaseMembers);
        writeBaseMembers = typeof(OutgoingMessageConverter)
            .GetMethod(writeBaseMembersName, BindingFlags.Static | BindingFlags.Public)!;
    }

    public override void Write(VerifyJsonWriter writer, object value)
    {
        writer.WriteStartObject();

        var genericArguments =
            value
                .GetType()
                .GetGenericArguments();
        writeBaseMembers
            .MakeGenericMethod(genericArguments)
            .Invoke(null, [writer, value]);

        writer.WriteEndObject();
    }

    public static void WriteBaseMembers<TMessage, TOptions>(VerifyJsonWriter writer, OutgoingMessage<TMessage, TOptions> value)
        where TMessage : notnull
        where TOptions : ExtendableOptions
    {
        var message = value.Message;

        //TODO: cant use T here since https://github.com/Particular/NServiceBus.Testing/pull/660/files
        //var name = typeof(T).SimpleName();
        var name = message
            .GetType()
            .SimpleName();

        writer.WriteMember(value, message, name);

        var options = value.Options;
        if (options.HasValue())
        {
            writer.WriteMember(value, options, "Options");
        }
    }

    public override bool CanConvert(Type type) =>
        type.IsGenericType &&
        type.GetGenericTypeDefinition() == typeof(OutgoingMessage<,>);
}