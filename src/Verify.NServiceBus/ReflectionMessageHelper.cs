static class ReflectionMessageHelper
{
    static BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public;

    public static ExtendableOptions GetOptions(object target) =>
        (ExtendableOptions) GetPropertyValue(target, "Options");

    public static TimeSpan? GetWithin(object target) =>
        (TimeSpan?) GetPropertyValue(target, "Within");

    public static DateTimeOffset? GetAt(object target) =>
        (DateTimeOffset?) GetPropertyValue(target, "At");

    public static object GetMessage(object target) =>
        GetPropertyValue(target, "Message");

    static object GetPropertyValue(object target, string name)
    {
        var type = target.GetType();
        var property = type.GetProperty(name, bindingFlags);
        if (property is null)
        {
            throw new($"Could not read {name} from {type.FullName}");
        }

        var method = property.GetMethod!;
        return method.Invoke(target, null)!;
    }
}