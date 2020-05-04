using System;
using System.Linq;
using NServiceBus;

static class Extensions
{
    public static bool IsHandler(this Type type)
    {
        return type.GetInterfaces()
            .Any(x =>
            {
                if (!x.IsGenericType)
                {
                    return false;
                }

                var typeDefinition = x.GetGenericTypeDefinition();
                return typeDefinition == typeof(IHandleMessages<>);
            });
    }
    public static bool IsMessage(this Type x)
    {
        return typeof(IMessage).IsAssignableFrom(x);
    }
}