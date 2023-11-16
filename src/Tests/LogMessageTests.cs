[UsesVerify]
public class LogMessageTests
{
    [Fact]
    public Task Logging()
    {
        var message = new LogMessage(LogLevel.Error,"{0} {1}", ["foo", "bar"]);
        return Verify(message.Message);
    }
}