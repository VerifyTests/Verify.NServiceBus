using NServiceBus.Logging;

namespace VerifyTests.NServiceBus;

public static class LogCapture
{
    static AsyncLocal<ConcurrentBag<LogMessage>> loggingContext = new();

    internal static ConcurrentBag<LogMessage> Context
    {
        get
        {
            var context = loggingContext.Value;
            if (context is not null)
            {
                return context;
            }

            context = new();
            loggingContext.Value = context;
            return context;
        }
    }

    public static IEnumerable<LogMessage> MessagesForLevel(LogLevel? includeLogMessages)
    {
        if (includeLogMessages is null)
        {
            return Enumerable.Empty<LogMessage>();
        }
        return LogMessages
            .Where(x => x.Level > includeLogMessages);
    }

    public static IReadOnlyList<LogMessage> LogMessages => Context.ToList();

    internal static void Add(LogMessage logMessage) =>
        Context.Add(logMessage);

    public static void Initialize() =>
        LogManager.Use<Logger>();
}