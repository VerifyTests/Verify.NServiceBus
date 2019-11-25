using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NServiceBus.Logging;

namespace Verify.NServiceBus
{
    public static class LogCapture
    {
        static AsyncLocal<Context> loggingContext = new AsyncLocal<Context>();

        internal static Context Context
        {
            get
            {
                var context = loggingContext.Value;
                if (context != null)
                {
                    return context;
                }

                context = new Context();
                loggingContext.Value = context;
                return context;
            }
        }

        public static IEnumerable<LogMessage> MessagesForLevel(LogLevel includeLogMessages)
        {
            return LogMessages
                .Where(x => x.Level > includeLogMessages);
        }

        public static IReadOnlyList<LogMessage> LogMessages
        {
            get => Context.logMessages;
        }

        internal static List<LogMessage> WritableLogMessages
        {
            get => Context.logMessages;
        }

        public static void Initialize()
        {
            LogManager.Use<Logger>();
        }
    }
}