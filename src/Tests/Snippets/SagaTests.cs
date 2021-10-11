using NServiceBus.Testing;
using VerifyXunit;
using Xunit;

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

        await Verifier.Verify(new
        {
            context,
            saga.Data
        });
    }

    #endregion
}