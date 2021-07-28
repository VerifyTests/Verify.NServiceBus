﻿using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.DelayedDelivery;
using NServiceBus.DeliveryConstraints;
using NServiceBus.Logging;
using NServiceBus.Pipeline;
using NServiceBus.Routing;
using NServiceBus.Testing;
using NServiceBus.Transport;
using Verify.NServiceBus;
using VerifyXunit;
using Xunit;

[UsesVerify]
public class Tests
{
    [Fact]
    public async Task Logging()
    {
        var handler = new MyHandlerWithLogging();
        var context = new TestableMessageHandlerContext();
        context.Extensions.Set("key", "value");

        await handler.Handle(new(), context);
        await Verifier.Verify(
            new
        {
            context,
            LogCapture.LogMessages
        });
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
        context.Extensions.Set("key", "value");
        return Verifier.Verify(
            new
            {
                context,
                state = new
                {
                    Property = "Value"
                }
            });
    }

    [Fact]
    public Task AuditContext()
    {
        var context = new TestableAuditContext();
        context.AddedAuditData.Add("Key", "Value");
        context.Extensions.Set("key", "value");
        return Verifier.Verify(context);
    }

    [Fact]
    public Task BatchDispatchContext()
    {
        var context = new TestableBatchDispatchContext();
        context.Operations.Add(BuildTransportOperation());
        context.Extensions.Set("key", "value");
        return Verifier.Verify(context);
    }

    [Fact]
    public Task BehaviorContext()
    {
        var context = new TestableBehaviorContextImp();
        context.Extensions.AddDeliveryConstraint(new DelayDeliveryWith(TimeSpan.FromDays(1)));
        context.Extensions.Set("key", "value");
        return Verifier.Verify(context);
    }

    public class TestableBehaviorContextImp :
        TestableBehaviorContext
    {
    }

    [Fact]
    public Task DispatchContext()
    {
        var context = new TestableDispatchContext();
        context.Operations.Add(BuildTransportOperation());
        context.Extensions.Set("key", "value");
        return Verifier.Verify(context);
    }

    [Fact]
    public async Task EndpointInstance()
    {
        var context = new TestableEndpointInstance();
        var session = (IMessageSession)context;
        await session.Send(
            new SendMessage
            {
                Property = "Value"
            });
        context.Extensions.Set("key", "value");
        await Verifier.Verify(context);
    }

    [Fact]
    public Task ForwardingContext()
    {
        var context = new TestableForwardingContext
        {
            Address = "The address",
            Message = BuildOutgoingMessage()
        };
        context.Extensions.Set("key", "value");
        return Verifier.Verify(context);
    }

    [Fact]
    public Task IncomingLogicalMessageContext()
    {
        var context = new TestableIncomingLogicalMessageContext
        {
            Message = BuildLogicalMessage(),
            Headers = new() {{"Key", "Value"}}
        };
        context.Extensions.Set("key", "value");
        return Verifier.Verify(context);
    }

    [Fact]
    public Task IncomingPhysicalMessageContext()
    {
        var context = new TestableIncomingPhysicalMessageContext
        {
            Message = BuildIncomingMessage(),
        };
        context.Extensions.Set("key", "value");
        return Verifier.Verify(context);
    }

    [Fact]
    public async Task InvokeHandlerContext()
    {
        var context = new TestableInvokeHandlerContext
        {
            Headers = new() {{"Key", "Value"}},
        };
        await context.Send(new MyMessage());
        context.Extensions.Set("key", "value");
        await Verifier.Verify(context);
    }

    [Fact]
    public Task MessageHandlerContext()
    {
        var context = new TestableMessageHandlerContext();
        context.Extensions.Set("key", "value");
        return Verifier.Verify(context);
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
        context.Extensions.Set("key", "value");
        await Verifier.Verify(context);
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
        context.Extensions.Set("key", "value");
        await Verifier.Verify(context);
    }

    [Fact]
    public Task OutgoingContext()
    {
        var context = new TestableOutgoingContext();
        context.Headers.Add("Key", "Value");
        context.Extensions.Set("key", "value");
        return Verifier.Verify(context);
    }

    [Fact]
    public async Task Saga()
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
        return Verifier.Verify(context);
    }

    [Fact]
    public Task OutgoingPhysicalMessageContext()
    {
        var context = new TestableOutgoingPhysicalMessageContext
        {
            Body = new byte[] {1}
        };
        context.Extensions.Set("key", "value");
        return Verifier.Verify(context);
    }

    [Fact]
    public Task OutgoingPublishContext()
    {
        var context = new TestableOutgoingPublishContext
        {
            Message = BuildOutgoingLogicalMessage()
        };
        context.Extensions.Set("key", "value");
        return Verifier.Verify(context);
    }

    [Fact]
    public Task OutgoingReplyContext()
    {
        var context = new TestableOutgoingReplyContext
        {
            Message = BuildOutgoingLogicalMessage()
        };
        context.Extensions.Set("key", "value");
        return Verifier.Verify(context);
    }

    [Fact]
    public Task OutgoingSendContext()
    {
        var context = new TestableOutgoingSendContext
        {
            Message = BuildOutgoingLogicalMessage()
        };
        context.Extensions.Set("key", "value");
        return Verifier.Verify(context);
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
        await Verifier.Verify(context);
    }

    [Fact]
    public Task RoutingContext()
    {
        var context = new TestableRoutingContext
        {
            Message = BuildOutgoingMessage()
        };
        return Verifier.Verify(context);
    }

    [Fact]
    public Task SubscribeContext()
    {
        var context = new TestableSubscribeContext
        {
            EventType = typeof(MyMessage)
        };
        return Verifier.Verify(context);
    }

    [Fact]
    public Task TransportReceiveContext()
    {
        var context = new TestableTransportReceiveContext
        {
            Message = BuildIncomingMessage(),
            ReceiveOperationAborted = true
        };
        return Verifier.Verify(context);
    }

    [Fact]
    public Task UnsubscribeContext()
    {
        var context = new TestableUnsubscribeContext
        {
            EventType = typeof(MyMessage)
        };
        return Verifier.Verify(context);
    }

    static TransportOperation BuildTransportOperation()
    {
        var outgoingMessage = BuildOutgoingMessage();
        return new(outgoingMessage,
            new UnicastAddressTag("destination"),
            DispatchConsistency.Isolated,
            new() {new DelayDeliveryWith(TimeSpan.FromDays(1))});
    }

    static OutgoingMessage BuildOutgoingMessage()
    {
        return new("MessageId", new() {{"key", "value"}}, new byte[] {1});
    }

    static OutgoingLogicalMessage BuildOutgoingLogicalMessage()
    {
        return new(typeof(MyMessage), new MyMessage {Property = "Value"});
    }

    static IncomingMessage BuildIncomingMessage()
    {
        return new("MessageId", new() {{"key", "value"}}, new byte[] {1});
    }

    static LogicalMessage BuildLogicalMessage()
    {
        return new(new(typeof(MyMessage)), new MyMessage {Property = "Value"});
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