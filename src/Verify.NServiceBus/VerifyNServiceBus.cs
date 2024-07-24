using Argon;

namespace VerifyTests;

public static class VerifyNServiceBus
{
    internal static List<JsonConverter> converters =
    [
        new RecordingHandlerContextConverter(),
        new RecordingMessageSessionConverter(),
        new SendOptionsConverter(),
        new UnsubscriptionConverter(),
        new SubscriptionConverter(),
        new LogicalMessageConverter(),
        new IncomingMessageConverter(),
        new ContextBagConverter(),
        new ExtendableOptionsConverter(),
        new MessageToHandlerMapConverter(),
        new UnicastSendRouterStateConverter(),
        new RoutingToDispatchConnectorStateConverter(),
        new SentMessageConverter(),
        new RepliedMessageConverter(),
        new PublishedMessageConverter(),
        new SagaConverter()
    ];

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
            LogManager.Use<Logger>();
        }

        VerifierSettings.AddNamedGuid(RecordingHandlerContext.DefaultMessageId, "MessageId");
        VerifierSettings.AddNamedGuid(RecordingHandlerContext.DefaultConversationId, "ConversationId");
        VerifierSettings.AddNamedGuid(RecordingHandlerContext.DefaultCorrelationId, "CorrelationId");
        VerifierSettings.IgnoreInstance<ContextBag>(_ => !ContextBagHelper.HasContent(_));
        VerifierSettings.AddExtraSettings(_ => _.Converters.AddRange(converters));
    }
}