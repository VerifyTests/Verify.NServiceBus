static class ContextBagHelper
{
    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "stash")]
    static extern ref Dictionary<string, object>? Stash(this ContextBag bag);

    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "parentBag")]
    static extern ref ContextBag? ParentBag(this ContextBag bag);

    /// <summary>
    /// Whether anything would actually be written for the bag. Defined in terms of
    /// <see cref="GetValues" /> so the emptiness check and the writer cannot disagree. A bag
    /// holding only entries GetValues filters out, e.g. the TransportTransaction stashed when
    /// sending inside an ambient transaction, counts as empty. Treating it as content wrote an
    /// "Options" member that then serialized to {}.
    /// </summary>
    public static bool HasContent(ContextBag contextBag) =>
        contextBag.GetValues().Any();

    public static IEnumerable<KeyValuePair<string, object>> GetValues(this ContextBag value)
    {
        var current = (ContextBag?)value;
        while (current is not null)
        {
            // a bag with no stash of its own can still have a parent that has one
            var stash = current.Stash();

            if (stash is not null)
            {
                foreach (var item in stash)
                {
                    if (WritesNothing(item))
                    {
                        continue;
                    }

                    yield return new(item.Key, item.Value);
                }
            }

            current = current.ParentBag();
        }
    }

    /// <summary>
    /// Entries that the writer would emit nothing for. They have to be excluded here rather than
    /// at the point of writing, so that <see cref="HasContent" /> agrees with the writer and no
    /// empty "Options" object is left behind.
    /// </summary>
    static bool WritesNothing(KeyValuePair<string, object> item)
    {
        var value = item.Value;

        // the ambient transaction when sending inside one
        if (value is TransportTransaction)
        {
            return true;
        }

        // written as a single ImmediateDispatch member, which is dropped when false since
        // Verify ignores default values
        return item.Key == RoutingToDispatchConnectorHelper.TypeName &&
               !RoutingToDispatchConnectorHelper.GetImmediateDispatch(value);
    }
}