using System.Threading.Tasks;
using NServiceBus.Testing;
using VerifyXunit;
using Xunit;

[UsesVerify]
public class HandlerTests
{
    #region HandlerTest

    [Fact]
    public async Task VerifyHandlerResult()
    {
        var handler = new MyHandler();
        var context = new TestableMessageHandlerContext();

        await handler.Handle(new MyRequest(), context);

        await Verifier.Verify(context);
    }

    #endregion
}