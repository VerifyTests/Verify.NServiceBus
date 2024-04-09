static class ContextBagHelper
{
    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "stash")]
    static extern ref Dictionary<string, object>? Stash(this ContextBag bag);

    static bool TryGetStash(ContextBag value)
    {
        var stash = value.Stash();
        return stash != null && stash.Count != 0;
    }

    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "parentBag")]
    static extern ref ContextBag? ParentBag(this ContextBag bag);

    static bool TryGetParentBag(ContextBag value, [NotNullWhen(true)] out ContextBag? parentBag)
    {
        parentBag = value.ParentBag();
        return parentBag != null;
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

    public static IEnumerable<KeyValuePair<string, object>> GetValues(this ContextBag value)
    {
        var current = (ContextBag?)value;
        do
        {
            var stash = current?.Stash();

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

            current = current?.ParentBag();
        } while (current is not null);
    }
}