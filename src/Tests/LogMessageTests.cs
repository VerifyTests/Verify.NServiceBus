using System.Collections.Generic;
using System.Threading.Tasks;
using NServiceBus.Logging;
using Verify.NServiceBus;
using VerifyXunit;
using Xunit;

[UsesVerify]
public class LogMessageTests
{
    [Fact]
    public Task Logging()
    {
        var message = new LogMessage(LogLevel.Error,"{0} {1}", new List<string>{"foo", "bar"});
        return Verifier.Verify(message.Message);
    }
}