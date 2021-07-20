using System;
using System.Collections.Generic;
using System.Linq;
using NServiceBus.Logging;

namespace Verify.NServiceBus
{
    public class LogMessage
    {
        public LogMessage(LogLevel level, string message, Exception? exception = null)
        {
            Level = level;
            Message = message;
            Exception = exception;
            Args = Array.Empty<object>();
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
}