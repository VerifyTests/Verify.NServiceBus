static class Extensions
{
    public static void WriteListOrSingleMember<T>(this VerifyJsonWriter writer, object target, IEnumerable<T> value, string name)
    {
        if (value.Count() == 1)
        {
            writer.WriteMember(target, value.Single(), name);
        }
        else
        {
            writer.WriteMember(target, value, name);
        }
    }
    public static bool IsHandler(this Type type)
    {
        if (!type.IsClass)
        {
            return false;
        }

        if (type.IsAbstract)
        {
            return false;
        }

        if (type.IsGenericTypeDefinition)
        {
            return false;
        }

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

    public static bool IsMessage(this Type x) =>
        typeof(IMessage).IsAssignableFrom(x);
}