namespace VerifyTests.NServiceBus;

public class LogMessage
{
    public LogMessage(LogLevel level, string message, Exception? exception = null)
    {
        Level = level;
        Message = message;
        Exception = exception;
        Args = [];
    }

    public LogMessage(LogLevel level, string format, object[] args, Exception? exception = null)
    {
        Level = level;
        Format = format;
        Exception = exception;
        try
        {
            Message = string.Format(format, args);
        }
        catch (Exception formatException)
        {
            throw new($"Could not format message. Format: {format}.", formatException);
        }

        Args = args;
    }

    public LogLevel Level { get; }
    public string Message { get; }
    public Exception? Exception { get; }
    public string? Format { get; }
    public object[] Args { get; }
}