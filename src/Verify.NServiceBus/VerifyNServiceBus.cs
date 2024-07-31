namespace VerifyTests;

public static partial class VerifyNServiceBus
{
    internal static List<JsonConverter> converters =
    [
        new RecordingHandlerContextConverter(),
        new RecordingInvokeHandlerContextConverter(),
        new RecordingIncomingPhysicalMessageContextConverter(),
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

        VerifierSettings.AddNamedGuid(DefaultMessageId, "MessageId");
        VerifierSettings.AddNamedGuid(DefaultConversationId, "ConversationId");
        VerifierSettings.AddNamedGuid(DefaultCorrelationId, "CorrelationId");
        VerifierSettings.IgnoreInstance<ContextBag>(_ => !ContextBagHelper.HasContent(_));
        VerifierSettings.AddExtraSettings(_ => _.Converters.AddRange(converters));
    }
}