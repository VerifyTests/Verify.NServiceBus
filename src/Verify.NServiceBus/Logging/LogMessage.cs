﻿using NServiceBus.Logging;

namespace VerifyTests.NServiceBus;

public class LogMessage
{
    public LogMessage(LogLevel level, string message, Exception? exception = null)
    {
        Level = level;
        Message = message;
        Exception = exception;
        Args = Enumerable.Empty<object>();
    }

    public LogMessage(LogLevel level, string format, IReadOnlyList<object> args, Exception? exception = null)
    {
        Level = level;
        Format = format;
        Exception = exception;
        try
        {
            Message = string.Format(format, args.ToArray());
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
    public IReadOnlyList<object> Args { get; }
}