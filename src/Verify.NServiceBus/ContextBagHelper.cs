using System.Diagnostics.CodeAnalysis;
using NServiceBus.Extensibility;

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
        var stash = (Dictionary<string, object>) stashField.GetValue(value)!;
        return stash.Any();
    }

    public static IEnumerable<KeyValuePair<string, object>> GetValues(this ContextBag value)
    {
        var current = (ContextBag?)value;
        do
        {
            var stash = (Dictionary<string, object>) stashField.GetValue(current)!;
            foreach (var item in stash)
            {
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