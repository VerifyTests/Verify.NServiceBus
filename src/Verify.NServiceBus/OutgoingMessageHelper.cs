using System;
using System.Reflection;
using NServiceBus.Extensibility;

static class OutgoingMessageHelper
{
    static BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public;

    public static ExtendableOptions GetOptions(object target)
    {
        return (ExtendableOptions) GetPropertyValue(target, "Options");
    }

    public static TimeSpan? GetWithin(object target)
    {
        return (TimeSpan?) GetPropertyValue(target, "Within");
    }

    public static DateTimeOffset? GetAt(object target)
    {
        return (DateTimeOffset?) GetPropertyValue(target, "At");
    }

    public static object GetMessage(object target)
    {
        return GetPropertyValue(target, "Message");
    }

    static object GetPropertyValue(object target, string name)
    {
        var type = target.GetType();
        var propertyInfo = type.GetProperty(name, bindingFlags);
        if (propertyInfo == null)
        {
            throw new Exception($"Could not read {name} from {type.FullName}");
        }

        var method = propertyInfo.GetMethod;
        return method.Invoke(target, null);
    }
}