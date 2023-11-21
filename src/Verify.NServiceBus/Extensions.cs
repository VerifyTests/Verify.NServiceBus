static class Extensions
{
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
            .Any(_ =>
            {
                if (!_.IsGenericType)
                {
                    return false;
                }

                var typeDefinition = _.GetGenericTypeDefinition();
                return typeDefinition == typeof(IHandleMessages<>);
            });
    }

    public static bool IsMessage(this Type type) =>
        typeof(IMessage).IsAssignableFrom(type);
}