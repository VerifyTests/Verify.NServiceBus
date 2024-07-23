public class SagaTests
{
    #region SagaTest

    [Fact]
    public async Task VerifySagaResult()
    {
        var saga = new MySaga
        {
            Data = new()
        };

        var context = new RecordingHandlerContext();

        var message = new MyRequest();
        await saga.Handle(message, context);

        await Verify(new
        {
            context,
            saga
        });
    }

    #endregion
}