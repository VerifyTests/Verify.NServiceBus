class SubscriptionConverter :
    WriteOnlyJsonConverter<Subscribed>
{
    public override void Write(VerifyJsonWriter writer, Subscribed subscribed)
    {
        writer.WriteStartObject();

        writer.WriteMember(subscribed, subscribed.EventType, "EventType");

        var options = subscribed.Options;
        if (options.HasValue())
        {
            writer.WriteMember(subscribed, options, "Options");
        }

        writer.WriteEndObject();
    }
}