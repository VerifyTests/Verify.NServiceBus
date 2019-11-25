using System;
using System.Collections.Generic;
using NServiceBus.Logging;

namespace Verify.NServiceBus
{
    public class LogMessage
    {
        public LogMessage(LogLevel level, string message, Exception? exception = null)
        {
            Guard.AgainstNull(message, nameof(message));
            Level = level;
            Message = message;
            Exception = exception;
            Args = Array.Empty<object>();
        }

        public LogMessage(LogLevel level, string format, IReadOnlyList<object> args, Exception? exception = null)
        {
            Guard.AgainstNull(format, nameof(format));
            Guard.AgainstNull(args, nameof(args));
            Level = level;
            Format = format;
            Exception = exception;
            try
            {
                Message = string.Format(format, args);
            }
            catch (Exception formatException)
            {
                throw new Exception($"Could not format message. Format: {format}.", formatException);
            }

            Args = args;
        }

        public LogLevel Level { get; }
        public string Message { get; }
        public Exception? Exception { get; }
        public string? Format { get; }
        public IReadOnlyList<object> Args { get; }
    }
}