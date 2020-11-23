using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NServiceBus.Logging;

namespace Verify.NServiceBus
{
    public static class LogCapture
    {
        static AsyncLocal<ConcurrentBag<LogMessage>> loggingContext = new();

        internal static ConcurrentBag<LogMessage> Context
        {
            get
            {
                var context = loggingContext.Value;
                if (context != null)
                {
                    return context;
                }

                context = new ConcurrentBag<LogMessage>();
                loggingContext.Value = context;
                return context;
            }
        }

        public static IEnumerable<LogMessage> MessagesForLevel(LogLevel? includeLogMessages)
        {
            if (includeLogMessages == null)
            {
                return Array.Empty<LogMessage>();
            }
            return LogMessages
                .Where(x => x.Level > includeLogMessages);
        }

        public static IReadOnlyList<LogMessage> LogMessages
        {
            get => Context.ToList();
        }

        internal static void Add(LogMessage logMessage)
        {
            Context.Add(logMessage);
        }

        public static void Initialize()
        {
            LogManager.Use<Logger>();
        }
    }
}