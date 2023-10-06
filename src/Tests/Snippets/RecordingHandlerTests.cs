[UsesVerify]
public class RecordingHandlerTests
{
    #region RecordingHandlerTests

    [Fact]
    public async Task VerifyHandlerResult()
    {
        var handler = new MyHandler();
        var context = new RecordingHandlerContext();

        var message = new MyRequest();
        await handler.Handle(message, context);

        await Verify("some other data");
    }

    #endregion

    [Fact]
    public async Task NoMessages()
    {
        new RecordingHandlerContext();

        await Verify("some other data");
    }

    [Fact]
    public async Task Clear()
    {
        var handler = new MyHandler();
        var context = new RecordingHandlerContext();

        var message = new MyRequest();
        await handler.Handle(message, context);
        VerifyNServiceBus.ClearRecordedMessages();
        await Verify("some other data");
    }
}