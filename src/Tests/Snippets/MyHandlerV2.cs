public class MyHandlerV2 :
    IHandleMessages<MyRequest>
{
    public async Task Handle(MyRequest message, IMessageHandlerContext context)
    {
        #region SimpleHandlerV2

        await context.Publish(
            new MyPublishMessage
            {
                Property = "Value"
            });

        await context.Reply(
            new MyReplyMessage
            {
                Property = "Value"
            });

        var sendOptions = new SendOptions();
        sendOptions.DelayDeliveryWith(TimeSpan.FromDays(1));
        await context.Send(
            new MySendMessage
            {
                Property = "Value"
            },
            sendOptions);

        await context.ForwardCurrentMessageTo("newDestination");

        #endregion
    }
}