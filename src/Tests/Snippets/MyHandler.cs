using System;
using System.Threading.Tasks;
using NServiceBus;

#region SimpleHandler

public class MyHandler :
    IHandleMessages<MyRequest>
{
    public async Task Handle(MyRequest message, IMessageHandlerContext context)
    {
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
        sendOptions.DelayDeliveryWith(TimeSpan.FromHours(12));
        await context.Send(
            new MySendMessage
            {
                Property = "Value"
            },
            sendOptions);

        await context.ForwardCurrentMessageTo("newDestination");
    }
}

#endregion