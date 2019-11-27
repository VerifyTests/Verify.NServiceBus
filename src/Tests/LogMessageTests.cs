using System.Collections.Generic;
using System.Threading.Tasks;
using NServiceBus.Logging;
using Verify.NServiceBus;
using VerifyXunit;
using Xunit;
using Xunit.Abstractions;

public class LogMessageTests :
    VerifyBase
{
    [Fact]
    public Task Logging()
    {
        var message = new LogMessage(LogLevel.Error,"{0} {1}", new List<string>{"foo", "bar"});
        return Verify(message.Message);
    }

    public LogMessageTests(ITestOutputHelper output) :
        base(output)
    {
    }
}