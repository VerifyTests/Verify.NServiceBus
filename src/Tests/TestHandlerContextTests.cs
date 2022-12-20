[UsesVerify]
public class TestHandlerContextTests
{
    [Fact]
    public Task Simple()
    {
        var context = new TestHandlerContext();
        context.Publish(new MyEvent());
        context.Send(new MyCommand());
        context.Reply(new MyCommand());
        context.ForwardCurrentMessageTo("forward-destination");
        context.Headers.Add("HeaderKey","Value");
        context.MessageHeaders.Add("MessageHeaderKey","Value");
        context.Extensions.Set("ExtensionKey","Value");
        return Verify(context);
    }

    public record MyEvent;
    public record MyCommand;
}
