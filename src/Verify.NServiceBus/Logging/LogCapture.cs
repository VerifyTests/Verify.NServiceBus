using System.Collections.Generic;
using System.Threading;
using NServiceBus.Logging;

namespace NServiceBus.ApprovalTests
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

        internal static List<LogMessage> LogMessages
        {
            get => Context.logMessages;
        }

        public static void Initialize()
        {
            LogManager.Use<Logger>();
        }
    }
}