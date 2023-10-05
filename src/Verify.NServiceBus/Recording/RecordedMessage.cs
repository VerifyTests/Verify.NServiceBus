namespace VerifyTests.NServiceBus;

public class RecordedMessage(object? message, ExtendableOptions? options, Type? eventType)
{
    public object? Message { get; } = message;
    public Type? EventType { get; } = eventType;
    public ExtendableOptions? Options { get; } = options;
}