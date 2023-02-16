static class UnicastRouterHelper
{
    static PropertyInfo explicitDestinationProperty;
    static PropertyInfo specificInstanceProperty;
    static PropertyInfo optionProperty;

    static UnicastRouterHelper()
    {
        var type = typeof(IMessage).Assembly.GetType(TypeName, true)!;
        explicitDestinationProperty = type.GetProperty("ExplicitDestination")!;
        specificInstanceProperty = type.GetProperty("SpecificInstance")!;
        optionProperty = type.GetProperty("Option")!;
    }

    public const string TypeName = "NServiceBus.UnicastSendRouter+State";

    public static bool IsUnicastSendRouter(Type type) =>
        type.FullName == TypeName;

    public static object GetOption(object value) =>
        optionProperty.GetValue(value)!;

    public static string? GetSpecificInstance(object value) =>
        (string?) specificInstanceProperty.GetValue(value);

    public static string? GetExplicitDestination(object value) =>
        (string?) explicitDestinationProperty.GetValue(value);

    public static bool TryWriteRoute(VerifyJsonWriter writer, string key, object value)
    {
        if (key != TypeName)
        {
            return false;
        }

        var explicitDestination = GetExplicitDestination(value);
        if (explicitDestination != null)
        {
            writer.WriteMember(value, explicitDestination, "RouteToExplicitDestination");
        }

        var specificInstance = GetSpecificInstance(value);
        if (specificInstance != null)
        {
            writer.WriteMember(value, specificInstance, "RouteToSpecificInstance");
        }

        if (explicitDestination == null && specificInstance == null)
        {
            var option = GetOption(value).ToString()!;
            writer.WriteMember(value, option.Replace("Route", ""), "Route");
        }

        return true;
    }
}