using System;
using System.Threading.Tasks;
using NServiceBus;

#region SimpleSaga

public class MySaga :
    Saga<MySaga.MySagaData>,
    IHandleMessages<MyRequest>
{
    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<MySagaData> mapper)
    {
        mapper.ConfigureMapping<MyRequest>(message => message.OrderId)
            .ToSaga(sagaData => sagaData.OrderId);
    }

    public async Task Handle(MyRequest message, IMessageHandlerContext context)
    {
        await context.Publish(
            new MyPublishMessage
            {
                Property = "Value"
            });

        Data.MessageCount++;
    }

    public class MySagaData :
        ContainSagaData
    {
        public Guid OrderId { get; set; }
        public int MessageCount { get; set; }
    }

}

#endregion