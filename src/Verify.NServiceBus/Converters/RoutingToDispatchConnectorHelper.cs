static class RoutingToDispatchConnectorHelper
{
    static PropertyInfo immediateDispatchProperty;

    static RoutingToDispatchConnectorHelper()
    {
        var type = typeof(IMessage).Assembly.GetType(TypeName, true)!;
        immediateDispatchProperty = type.GetProperty("ImmediateDispatch")!;
    }

    public const string TypeName = "NServiceBus.RoutingToDispatchConnector+State";

    public static bool IsRoutingToDispatchConnector(Type type) =>
        type.FullName == TypeName;

    public static bool GetImmediateDispatch(object value) =>
        (bool) immediateDispatchProperty.GetValue(value)!;

    public static bool TryWriteRoute(VerifyJsonWriter writer, string key, object value)
    {
        if (key != TypeName)
        {
            return false;
        }

        writer.WriteMember(value, GetImmediateDispatch(value), "ImmediateDispatch");

        return true;
    }
}