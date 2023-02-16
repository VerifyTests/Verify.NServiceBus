static class ContextBagHelper
{
    static FieldInfo stashField;
    static FieldInfo parentBagField;

    static ContextBagHelper()
    {
        var bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;
        stashField = typeof(ContextBag).GetField("stash", bindingFlags)!;
        parentBagField = typeof(ContextBag).GetField("parentBag", bindingFlags)!;
    }

    public static bool HasContent(ContextBag contextBag)
    {
        while (true)
        {
            if (TryGetStash(contextBag))
            {
                return true;
            }

            if (TryGetParentBag(contextBag, out var parent))
            {
                contextBag = parent;
                continue;
            }

            return false;
        }
    }

    static bool TryGetStash(object value)
    {
        var stash = (Dictionary<string, object>?) stashField.GetValue(value);
        return stash != null && stash.Any();
    }

    public static IEnumerable<KeyValuePair<string, object>> GetValues(this ContextBag value)
    {
        var current = (ContextBag?)value;
        do
        {
            var stash = (Dictionary<string, object>?) stashField.GetValue(current);
            
            if (stash is null)
            {
                break;
            }
            foreach (var item in stash)
            {
                if (item.Value is TransportTransaction)
                {
                    continue;
                }

                yield return new(item.Key, item.Value);
            }

            current = (ContextBag?) parentBagField.GetValue(current);
        } while (current is not null);
    }

    static bool TryGetParentBag(object value, [NotNullWhen(true)] out ContextBag? parentBag)
    {
        parentBag = (ContextBag?) parentBagField.GetValue(value);
        return parentBag != null;
    }
}