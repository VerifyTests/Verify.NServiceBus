using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.DelayedDelivery;
using NServiceBus.DeliveryConstraints;
using NServiceBus.Logging;
using NServiceBus.Pipeline;
using NServiceBus.Routing;
using NServiceBus.Testing;
using NServiceBus.Transport;
using NServiceBus.Unicast.Messages;
using Verify.NServiceBus;
using VerifyXunit;
using Xunit;
using Xunit.Abstractions;

public class Tests :
    VerifyBase
{
    [Fact]
    public async Task Logging()
    {
        var handler = new MyHandlerWithLogging();
        var context = new TestableMessageHandlerContext();

        await handler.Handle(new MyMessage(), context);
        await this.VerifyContext(context,context, includeLogMessages: LogLevel.Debug);
    }

    class MyHandlerWithLogging :
        IHandleMessages<MyMessage>
    {
        static ILog logger = LogManager.GetLogger<MyHandlerWithLogging>();
        public Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            logger.Warn("The log message");
            return Task.CompletedTask;
        }
    }

    [Fact]
    public Task ExtraState()
    {
        var context = new TestableAuditContext();
        context.AddedAuditData.Add("Key", "Value");
        return this.VerifyContext(context,new {Property="Value"});
    }

    [Fact]
    public Task AuditContext()
    {
        var context = new TestableAuditContext();
        context.AddedAuditData.Add("Key", "Value");
        return this.VerifyContext(context);
    }

    [Fact]
    public Task BatchDispatchContext()
    {
        var context = new TestableBatchDispatchContext();
        context.Operations.Add(BuildTransportOperation());
        return this.VerifyContext(context);
    }


    [Fact]
    public Task BehaviorContext()
    {
        var context = new TestableBehaviorContextImp();
        context.Extensions.AddDeliveryConstraint(new DelayDeliveryWith(TimeSpan.FromDays(1)));
        return this.VerifyContext(context);
    }

    public class TestableBehaviorContextImp : TestableBehaviorContext
    {
    }

    [Fact]
    public Task DispatchContext()
    {
        var context = new TestableDispatchContext();
        context.Operations.Add(BuildTransportOperation());
        return this.VerifyContext(context);
    }

    [Fact]
    public async Task EndpointInstance()
    {
        var context = new TestableEndpointInstance();
        await ((IMessageSession)context).Send(new SendMessage {Property = "Value"});
        await this.VerifyContext(context);
    }

    [Fact]
    public Task ForwardingContext()
    {
        var context = new TestableForwardingContext
        {
            Address = "The address",
            Message = BuildOutgoingMessage()
        };
        return this.VerifyContext(context);
    }

    [Fact]
    public Task IncomingLogicalMessageContext()
    {
        var context = new TestableIncomingLogicalMessageContext
        {
            Message = BuildLogicalMessage(),
            Headers = new Dictionary<string, string> {{"Key", "Value"}}
        };
        return this.VerifyContext(context);
    }

    [Fact]
    public Task IncomingPhysicalMessageContext()
    {
        var context = new TestableIncomingPhysicalMessageContext
        {
            Message = BuildIncomingMessage(),
        };
        return this.VerifyContext(context);
    }

    [Fact]
    public async Task InvokeHandlerContext()
    {
        var context = new TestableInvokeHandlerContext
        {
            Headers = new Dictionary<string, string> {{"Key", "Value"}},
        };
        await context.Send(new MyMessage());
        await this.VerifyContext(context);
    }

    [Fact]
    public Task MessageHandlerContext()
    {
        var context = new TestableMessageHandlerContext();
        return this.VerifyContext(context);
    }

    [Fact]
    public async Task MessageProcessingContext()
    {
        var context = new TestableMessageProcessingContext();
        context.MessageHeaders.Add("Key", "Value");
        await context.Publish(new PublishMessage {Property = "Value"});
        await context.Reply(new ReplyMessage {Property = "Value"});
        await context.Send(new SendMessage {Property = "Value"});
        await context.ForwardCurrentMessageTo("newDestination");
        await this.VerifyContext(context);
    }

    [Fact]
    public async Task MessageSession()
    {
        var context = new TestableMessageSession();
        var subscribeOptions = new SubscribeOptions();
        subscribeOptions.RequireImmediateDispatch();
        await context.Subscribe(typeof(MyMessage), subscribeOptions);
        var unsubscribeOptions = new UnsubscribeOptions();
        unsubscribeOptions.RequireImmediateDispatch();
        await context.Unsubscribe(typeof(MyMessage), unsubscribeOptions);
        await this.VerifyContext(context);
    }

    [Fact]
    public Task OutgoingContext()
    {
        var context = new TestableOutgoingContext();
        context.Headers.Add("Key", "Value");
        return this.VerifyContext(context);
    }

    [Fact]
    public async Task Saga()
    {
        var saga = new MySaga
        {
            Data = new MySagaData()
        };
        var context = new TestableMessageHandlerContext();

        await saga.Handle(new MySagaMessage(), context);

        await this.VerifyContext(context,saga.Data);
    }

    public class MySaga:
        NServiceBus.Saga<MySagaData>,
        IHandleMessages<MySagaMessage>,
        IHandleTimeouts<MySagaMessage>
    {
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<MySagaData> mapper)
        {
        }

        public async Task Handle(MySagaMessage message, IMessageHandlerContext context)
        {
            Data.Member = "the data";
            await context.Reply(new MySagaMessage());
            await RequestTimeout<MySagaMessage>(context, TimeSpan.FromHours(1));
        }

        public Task Timeout(MySagaMessage state, IMessageHandlerContext context)
        {
            return Task.CompletedTask;
        }
    }

    public class MySagaMessage
    {
    }

    public class MySagaData :
        ContainSagaData
    {
        public string? Member { get; set; }
    }

    [Fact]
    public Task OutgoingLogicalMessageContext()
    {
        var context = new TestableOutgoingLogicalMessageContext
        {
            Message = BuildOutgoingLogicalMessage()
        };
        return this.VerifyContext(context);
    }

    [Fact]
    public Task OutgoingPhysicalMessageContext()
    {
        var context = new TestableOutgoingPhysicalMessageContext
        {
            Body = new byte[] {1}
        };
        return this.VerifyContext(context);
    }

    [Fact]
    public Task OutgoingPublishContext()
    {
        var context = new TestableOutgoingPublishContext
        {
            Message = BuildOutgoingLogicalMessage()
        };
        return this.VerifyContext(context);
    }

    [Fact]
    public Task OutgoingReplyContext()
    {
        var context = new TestableOutgoingReplyContext
        {
            Message = BuildOutgoingLogicalMessage()
        };
        return this.VerifyContext(context);
    }

    [Fact]
    public Task OutgoingSendContext()
    {
        var context = new TestableOutgoingSendContext
        {
            Message = BuildOutgoingLogicalMessage()
        };
        return this.VerifyContext(context);
    }

    [Fact]
    public async Task PipelineContext()
    {
        var context = new TestablePipelineContext();
        await context.Publish(new PublishMessage { Property = "Value" });
        await context.Send(new SendMessage { Property = "Value" });
        var options = new SendOptions();
        options.DelayDeliveryWith(TimeSpan.FromDays(1));
        await context.Send(new SendMessage {Property = "ValueWithDelay"},options);
        await this.VerifyContext(context);
    }

    [Fact]
    public Task RoutingContext()
    {
        var context = new TestableRoutingContext
        {
            Message = BuildOutgoingMessage()
        };
        return this.VerifyContext(context);
    }

    [Fact]
    public Task SubscribeContext()
    {
        var context = new TestableSubscribeContext
        {
            EventType = typeof(MyMessage)
        };
        return this.VerifyContext(context);
    }

    [Fact]
    public Task TransportReceiveContext()
    {
        var context = new TestableTransportReceiveContext
        {
            Message = BuildIncomingMessage(),
            ReceiveOperationAborted = true
        };
        return this.VerifyContext(context);
    }

    [Fact]
    public Task UnsubscribeContext()
    {
        var context = new TestableUnsubscribeContext
        {
            EventType = typeof(MyMessage)
        };
        return this.VerifyContext(context);
    }

    static TransportOperation BuildTransportOperation()
    {
        var outgoingMessage = BuildOutgoingMessage();
        return new TransportOperation(outgoingMessage,
            new UnicastAddressTag("destination"),
            DispatchConsistency.Isolated,
            new List<DeliveryConstraint> {new DelayDeliveryWith(TimeSpan.FromDays(1))});
    }

    static OutgoingMessage BuildOutgoingMessage()
    {
        return new OutgoingMessage("MessageId", new Dictionary<string, string> {{"key", "value"}}, new byte[] {1});
    }

    static OutgoingLogicalMessage BuildOutgoingLogicalMessage()
    {
        return new OutgoingLogicalMessage(typeof(MyMessage), new MyMessage {Property = "Value"});
    }

    static IncomingMessage BuildIncomingMessage()
    {
        return new IncomingMessage("MessageId", new Dictionary<string, string> {{"key", "value"}}, new byte[] {1});
    }

    static LogicalMessage BuildLogicalMessage()
    {
        return new LogicalMessage(new MessageMetadata(typeof(MyMessage)), new MyMessage {Property = "Value"});
    }

    public Tests(ITestOutputHelper output) :
        base(output)
    {
    }
}

public class MyMessage
{
    public string? Property { get; set; }
}

public class PublishMessage
{
    public string? Property { get; set; }
}

public class ReplyMessage
{
    public string? Property { get; set; }
}

public class SendMessage
{
    public string? Property { get; set; }
}