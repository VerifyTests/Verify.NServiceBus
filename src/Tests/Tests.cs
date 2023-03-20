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
        await Verify(
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
        public Task Handle(MyMessage message, HandlerContext context)
        {
            logger.Warn("The log message");
            return Task.CompletedTask;
        }
    }

    [Fact]
    public Task ExtraState()
    {
        var context = new TestableAuditContext();
        context.AuditMetadata.Add("key", "value");
        context.Extensions.Set("key", "value");
        return Verify(
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
        var context = new TestableAuditContext
        {
            Message = BuildOutgoingMessage()
        };
        context.AuditMetadata.Add("key", "value");
        context.Extensions.Set("key", "value");
        return Verify(context);
    }

    [Fact]
    public Task BatchDispatchContext()
    {
        var context = new TestableBatchDispatchContext();
        context.Operations.Add(BuildTransportOperation());
        context.Extensions.Set("key", "value");
        return Verify(context);
    }

    [Fact]
    public Task BehaviorContext()
    {
        var context = new TestableBehaviorContextImp();
        context.Extensions.Set("key", "value");
        return Verify(context);
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
        return Verify(context);
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
        await Verify(context);
    }

    [Fact]
    public Task IncomingLogicalMessageContext()
    {
        var context = new TestableIncomingLogicalMessageContext
        {
            Message = BuildLogicalMessage(),
            Headers = new()
            {
                {
                    "key", "value"
                },
                {
                    "NServiceBus.MessageId", "TheId"
                }
            }
        };
        context.Extensions.Set("key", "value");
        context.MessageHeaders.Add("key","value");
        context.Publish("publish message");
        context.Send("send message");
        context.SendLocal("send local message");
        return Verify(context);
    }

    [Fact]
    public Task IncomingPhysicalMessageContext()
    {
        var context = new TestableIncomingPhysicalMessageContext
        {
            Message = BuildIncomingMessage(),
        };
        context.ForwardCurrentMessageTo("forward destination");
        context.MessageHeaders.Add("key","value");
        context.Reply("reply destination");
        context.Publish("publish message");
        context.Send("Send message");
        context.SendLocal("Send local message");
        context.Extensions.Set("key", "value");
        return Verify(context);
    }

    [Fact]
    public async Task InvokeHandlerContext()
    {
        var context = new TestableInvokeHandlerContext
        {     Headers = new()
            {
                {
                    "key", "value"
                },
                {
                    "NServiceBus.MessageId", "TheId"
                }
            }
        };
        await context.Send(new MyMessage());
        context.Extensions.Set("key", "value");
        await Verify(context);
    }

    [Fact]
    public Task RecoverabilityContext()
    {
        var context = new TestableRecoverabilityContext
        {
            FailedMessage = BuildIncomingMessage(),
            Exception = new("error"),
            ReceiveAddress = "the ReceiveAddress",
            ImmediateProcessingFailures = 10,
            DelayedDeliveriesPerformed = 5,
            RecoverabilityAction = RecoverabilityAction.DelayedRetry(TimeSpan.FromDays(1)),
            Metadata = new()
            {
                {
                    "key", "value"
                }
            }
        };
        context.Extensions.Set("key", "value");
        return Verify(context);
    }

    [Fact]
    public Task TimeoutMessageWithin()
    {
        var context = new TimeoutMessage<string>("message",new(), TimeSpan.FromDays(10));
        return Verify(context);
    }

    [Fact]
    public Task TimeoutMessageAt()
    {
        var context = new TimeoutMessage<string>("message",new(), new DateTimeOffset(new(2020,10,1)));
        return Verify(context);
    }

    [Fact]
    public Task MessageHandlerContext()
    {
        var context = new TestableMessageHandlerContext
        {
            Headers = new()
            {
                {
                    "key", "value"
                },
                {
                    "NServiceBus.MessageId", "TheId"
                }
            }
        };
        context.Extensions.Set("key", "value");
        context.MessageHeaders.Add("key","value");
        context.Publish("publish message");
        context.Send("send message");
        context.SendLocal("send local message");
        return Verify(context);
    }

    [Fact]
    public Task Unsubscription()
    {
        var context = new Unsubscription(typeof(string), new());
        return Verify(context);
    }

    [Fact]
    public async Task MessageProcessingContext()
    {
        var context = new TestableMessageProcessingContext();
        context.MessageHeaders.Add("key", "value");
        await context.Publish(new PublishMessage {Property = "Value"});
        await context.Reply(new ReplyMessage {Property = "Value"});
        await context.Send(new SendMessage {Property = "Value"});
        await context.ForwardCurrentMessageTo("newDestination");
        context.Extensions.Set("key", "value");
        await Verify(context);
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
        await Verify(context);
    }

    [Fact]
    public Task OutgoingContext()
    {
        var context = new TestableOutgoingContext
        {
            Headers = new()
            {
                {
                    "key", "value"
                },
                {
                    "NServiceBus.MessageId", "TheId"
                }
            }
        };
        context.Extensions.Set("key", "value");
        context.Publish("publish message");
        context.Send("send message");
        context.SendLocal("send local message");
        return Verify(context);
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

        await Verify(new
        {
            context,
            saga
        });
    }

    [Fact]
    public async Task CompletedSaga()
    {
        var saga = new MySaga
        {
            Data = new()
        };
        saga.MarkCompleted();
        var context = new TestableMessageHandlerContext();

        await saga.Handle(new(), context);

        await Verify(new
        {
            context,
            saga
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

        public async Task Handle(MySagaMessage message, HandlerContext context)
        {
            Data.Member = "the data";
            await context.Reply(new MySagaMessage());
            await RequestTimeout<MySagaMessage>(context, TimeSpan.FromHours(1));
        }

        public Task Timeout(MySagaMessage state, HandlerContext context) =>
            Task.CompletedTask;

        public void MarkCompleted() =>
            MarkAsComplete();
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
            Message = BuildOutgoingLogicalMessage(),
            Headers = new()
            {
                {
                    "key", "value"
                },
                {
                    "NServiceBus.MessageId", "TheId"
                }
            }
        };
        context.Publish("publish message");
        context.Send("send message");
        context.SendLocal("send local message");
        context.Extensions.Set("key", "value");
        return Verify(context);
    }

    [Fact]
    public Task OutgoingPhysicalMessageContext()
    {
        var context = new TestableOutgoingPhysicalMessageContext
        {
            Body = new byte[] {1},
            Headers = new()
            {
                {
                    "key", "value"
                },
                {
                    "NServiceBus.MessageId", "TheId"
                }
            }
        };
        context.Extensions.Set("key", "value");
        context.Publish("publish message");
        context.Send("send message");
        context.SendLocal("send local message");
        return Verify(context);
    }

    [Fact]
    public Task OutgoingPublishContext()
    {
        var context = new TestableOutgoingPublishContext
        {
            Message = BuildOutgoingLogicalMessage(),
            Headers = new()
            {
                {
                    "key", "value"
                },
                {
                    "NServiceBus.MessageId", "TheId"
                }
            }
        };
        context.Extensions.Set("key", "value");
        context.Publish("publish message");
        context.Send("send message");
        context.SendLocal("send local message");
        return Verify(context);
    }

    [Fact]
    public Task OutgoingReplyContext()
    {
        var context = new TestableOutgoingReplyContext
        {
            Message = BuildOutgoingLogicalMessage(),
            Headers = new()
            {
                {
                    "key", "value"
                },
                {
                    "NServiceBus.MessageId", "TheId"
                }
            }
        };
        context.Extensions.Set("key", "value");
        context.Publish("publish message");
        context.Send("send message");
        context.SendLocal("send local message");
        return Verify(context);
    }

    [Fact]
    public Task OutgoingSendContext()
    {
        var context = new TestableOutgoingSendContext
        {
            Message = BuildOutgoingLogicalMessage(),
            Headers = new()
            {
                {
                    "key", "value"
                },
                {
                    "NServiceBus.MessageId", "TheId"
                }
            }
        };
        context.Extensions.Set("key", "value");
        context.Publish("publish message");
        context.Send("send message");
        context.SendLocal("send local message");
        return Verify(context);
    }

    [Fact]
    public async Task PipelineContext()
    {
        var context = new TestablePipelineContext();
        await context.Publish(new PublishMessage { Property = "Value" });
        await context.Send(new SendMessage { Property = "Value" });
        var options = new SendOptions();
        options.DelayDeliveryWith(TimeSpan.FromDays(1));
        context.Extensions.Set("key", "value");
        await context.Send(new SendMessage {Property = "ValueWithDelay"},options);
        await Verify(context);
    }

    [Fact]
    public Task RoutingContext()
    {
        var context = new TestableRoutingContext
        {
            Message = BuildOutgoingMessage()
        };
        return Verify(context);
    }

    [Fact]
    public Task SubscribeContext()
    {
        var context = new TestableSubscribeContext
        {
            EventType = typeof(MyMessage)
        };
        return Verify(context);
    }

    [Fact]
    public Task TransportReceiveContext()
    {
        var context = new TestableTransportReceiveContext
        {
            Message = BuildIncomingMessage(),
            ReceiveOperationAborted = true
        };
        return Verify(context);
    }

    [Fact]
    public Task UnsubscribeContext()
    {
        var context = new TestableUnsubscribeContext
        {
            EventType = typeof(MyMessage)
        };
        return Verify(context);
    }

    [Fact]
    public void ConverterOrder()
    {
        var converters = VerifyNServiceBus.converters
            .Select(_ => _.GetType().BaseType!)
            .Where(_ => _.IsGenericType)
            .Select(_ => _.GenericTypeArguments[0])
            .ToList();
        for (var index = 0; index < converters.Count - 1; index++)
        {
            var converter = converters[index];
            var next = converters[index + 1];
            Assert.True(TypeDepth(converter) >= TypeDepth(next), $"{next.Name} should be before {converter.Name}");
        }
    }

    static int TypeDepth(Type? type)
    {
        var depth = 0;
        while (type != null)
        {
            depth++;
            type = type.BaseType;
        }

        return depth;
    }

    static TransportOperation BuildTransportOperation() =>
        new(
            BuildOutgoingMessage(),
            new UnicastAddressTag("destination"),
            new()
            {
                DelayDeliveryWith = new(TimeSpan.FromDays(1))
            },
            DispatchConsistency.Isolated);

    static OutgoingMessage BuildOutgoingMessage() =>
        new(
            "MessageId",
            new()
            {
                {"key", "value"},
                {"NServiceBus.MessageId", "TheId"},
            },
            new byte[] {1});

    static OutgoingLogicalMessage BuildOutgoingLogicalMessage() =>
        new(
            typeof(MyMessage),
            new MyMessage
            {
                Property = "Value"
            });

    static IncomingMessage BuildIncomingMessage() =>
        new(
            "NativeMessageId",
            new()
            {
                {"key", "value"},
                {"NServiceBus.MessageId", "TheId"},
            },
            new byte[] {1});

    static LogicalMessage BuildLogicalMessage() =>
        new(
            new(typeof(MyMessage)),
            new MyMessage
            {
                Property = "Value"
            });
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