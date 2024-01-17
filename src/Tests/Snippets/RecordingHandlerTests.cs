public class RecordingHandlerTests
{
    #region RecordingHandlerTests

    [Fact]
    public async Task VerifyHandlerResult()
    {
        Recording.Start();
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
        Recording.Start();
        new RecordingHandlerContext();

        await Verify("some other data");
    }
}