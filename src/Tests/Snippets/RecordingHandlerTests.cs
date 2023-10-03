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
}