[UsesVerify]
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

        var context = new TestableMessageHandlerContext();

        await saga.Handle(new(), context);

        await Verify(new
        {
            context,
            saga.Data
        });
    }

    #endregion
}