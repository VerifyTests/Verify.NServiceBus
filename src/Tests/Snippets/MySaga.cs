#region SimpleSaga

public class MySaga :
    Saga<MySaga.MySagaData>,
    IHandleMessages<MyRequest>
{
    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<MySagaData> mapper) =>
        mapper.MapSaga(sagaData => sagaData.OrderId)
            .ToMessage<MyRequest>(message => message.OrderId);

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