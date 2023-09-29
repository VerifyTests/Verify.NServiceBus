#region SimpleSaga

public class MySaga :
    NServiceBus.Saga<MySaga.MySagaData>,
    IHandleMessages<MyRequest>
{
    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<MySagaData> mapper) =>
        mapper.ConfigureMapping<MyRequest>(message => message.OrderId)
            .ToSaga(sagaData => sagaData.OrderId);

    public async Task Handle(MyRequest message, HandlerContext context)
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