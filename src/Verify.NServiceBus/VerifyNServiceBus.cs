using Argon;

namespace VerifyTests;

public static class VerifyNServiceBus
{
    internal static List<JsonConverter> converters = new()
    {
        new MessageHandlerContextConverter(),
        new IncomingPhysicalMessageContextConverter(),
        new InvokeHandlerContextConverter(),
        new IncomingLogicalMessageContextConverter(),
        new IncomingContextConverter(),
        new OutgoingLogicalMessageContextConverter(),
        new OutgoingPhysicalMessageContextConverter(),
        new OutgoingPublishContextConverter(),
        new OutgoingReplyContextConverter(),
        new OutgoingSendContextConverter(),
        new OutgoingContextConverter(),
        new SendOptionsConverter(),
        new MessageProcessingContextConverter(),
        new UnsubscriptionConverter(),
        new SubscriptionConverter(),
        new EndpointInstanceConverter(),
        new RoutingContextConverter(),
        new LogicalMessageConverter(),
        new PipelineContextConverter(),
        new MessageSessionConverter(),
        new IncomingMessageConverter(),
        new ContextBagConverter(),
        new ExtendableOptionsConverter(),
        new MessageToHandlerMapConverter(),
        new UnicastSendRouterStateConverter(),
        new RoutingToDispatchConnectorStateConverter(),
        new TimeoutMessageConverter(),
        new SagaConverter(),
        new OutgoingMessageConverter()
    };

    public static bool Initialized { get; private set; }

    public static void Initialize(bool captureLogs = true)
    {
        if (Initialized)
        {
            throw new("Already Initialized");
        }

        Initialized = true;

        InnerVerifier.ThrowIfVerifyHasBeenRun();
        if (captureLogs)
        {
            LogCapture.Initialize();
        }

        VerifierSettings.IgnoreInstance<ContextBag>(_ => !ContextBagHelper.HasContent(_));
        VerifierSettings.AddExtraSettings(_ => _.Converters.AddRange(converters));

        VerifierSettings.RegisterJsonAppender(_ =>
        {
            var entries = RecordingState.Stop();
            if (entries is null)
            {
                return null;
            }

            return new ToAppend("messages", entries);
        });
    }
}