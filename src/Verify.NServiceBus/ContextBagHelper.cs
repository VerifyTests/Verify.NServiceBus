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
                    if (item.Value is TransportTransaction)
                    {
                        continue;
                    }

                    yield return new(item.Key, item.Value);
                }
            }

            current = current.ParentBag();
        }
    }
}