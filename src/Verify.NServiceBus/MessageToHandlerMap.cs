namespace VerifyTests.NServiceBus;

public class MessageToHandlerMap
{
    internal HashSet<Type> Messages = new();
    internal HashSet<Type> HandledMessages = new();

    public void AddMessagesFromAssembly(Assembly assembly)
    {
        foreach (var type in assembly.GetTypes().Where(_ => _.IsMessage()))
        {
            AddMessage(type);
        }
    }

    public void AddMessagesFromAssembly<T>() =>
        AddMessagesFromAssembly(typeof(T).Assembly);

    public void AddMessage(Type type) =>
        Messages.Add(type);

    public void AddMessage<T>() =>
        Messages.Add(typeof(T));

    public void AddHandler(Type handlerType)
    {
        foreach (var interfaceType in handlerType
            .GetInterfaces()
            .Where(_ =>
            {
                if (!_.IsGenericType)
                {
                    return false;
                }

                var typeDefinition = _.GetGenericTypeDefinition();
                return typeDefinition == typeof(IHandleMessages<>);
            }))
        {
            HandledMessages.Add(interfaceType.GenericTypeArguments.Single());
        }
    }

    public void AddHandler<T>() =>
        AddHandler(typeof(T));

    public void AddHandlersFromAssembly(Assembly assembly)
    {
        foreach (var type in assembly.GetTypes().Where(_ => _.IsHandler()))
        {
            AddHandler(type);
        }
    }

    public void AddHandlersFromAssembly<T>() =>
        AddHandlersFromAssembly(typeof(T).Assembly);
}