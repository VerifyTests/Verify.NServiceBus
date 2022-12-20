namespace VerifyTests.NServiceBus;

public class TestHandlerContext : TestableMessageHandlerContext
{
    public TestHandlerContext()
    {
        MessageHeaders["NServiceBus.ConversationId"] = Guid.NewGuid().ToString();
        MessageHeaders["NServiceBus.TimeSent"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffffff Z");
    }
}