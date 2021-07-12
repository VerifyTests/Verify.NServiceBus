using System.Threading.Tasks;
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
            Data = new MySaga.MySagaData()
        };

        var context = new TestableMessageHandlerContext();

        await saga.Handle(new MyRequest(), context);

        await Verifier.Verify(new
        {
            context,
            saga.Data
        });
    }

    #endregion
}