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
        return Verify(message.Message);
    }
}