using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using NServiceBus.Extensibility;

static class ContextBagHelper
{
    static FieldInfo stashField;
    static FieldInfo parentBagField;

    static ContextBagHelper()
    {
        var bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;
        stashField = typeof(ContextBag).GetField("stash", bindingFlags);
        parentBagField = typeof(ContextBag).GetField("parentBag", bindingFlags);
    }

    public static bool HasContent(ContextBag contextBag)
    {
        if (TryGetStash(contextBag, out _))
        {
            return true;
        }

        if (TryGetParentBag(contextBag, out var parent))
        {
            return HasContent(parent);
        }

        return false;
    }

    static bool TryGetStash(object value, out Dictionary<string, object> stash)
    {
        stash = (Dictionary<string, object>) stashField.GetValue(value);
        if (stash.Any())
        {
            return true;
        }

        return false;
    }

    public static IEnumerable<KeyValuePair<string, object>> GetValues(this ContextBag value)
    {
        do
        {
            var stash = (Dictionary<string, object>) stashField.GetValue(value);
            foreach (var item in stash)
            {
                 yield return new KeyValuePair<string, object>(item.Key, item.Value);
            }
            value = (ContextBag) parentBagField.GetValue(value);
        } while (value != null);
    }

    static bool TryGetParentBag(object value, [NotNullWhen(true)] out ContextBag? parentBag)
    {
        parentBag = (ContextBag?) parentBagField.GetValue(value);
        return parentBag != null;
    }
}