using Argon;
using NServiceBus.Pipeline;

namespace VerifyTests;

public static class VerifyNServiceBus
{
    internal static List<JsonConverter> converters = new()
    {
        new SendOptionsConverter(),
        new UnsubscriptionConverter(),
        new SubscriptionConverter(),
        new IncomingMessageConverter(),
        new ContextBagConverter(),
        new UnicastSendRouterStateConverter(),
        new RoutingToDispatchConnectorStateConverter(),
        new ExtendableOptionsConverter(),
        new TimeoutMessageConverter(),
        new SagaConverter(),
        new MessageToHandlerMapConverter(),
        new OutgoingMessageConverter()
    };
    [Obsolete("Use Initialize")]
    public static void Enable(bool captureLogs = true) =>
        Initialize(captureLogs);

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

        VerifierSettings.IgnoreMember<TestableMessageProcessingContext>(_ => _.MessageHeaders);
        VerifierSettings.IgnoreMember<TestableInvokeHandlerContext>(_ => _.Headers);
        VerifierSettings.IgnoreMember<TestableMessageProcessingContext>(_ => _.MessageId);
        VerifierSettings.IgnoreMember<TestableInvokeHandlerContext>(_ => _.MessageHandler);
        VerifierSettings.IgnoreMember<TestableInvokeHandlerContext>(_ => _.MessageBeingHandled);
        VerifierSettings.IgnoreMember<TestableInvokeHandlerContext>(_ => _.MessageMetadata);
        VerifierSettings.IgnoreMember<LogicalMessage>(_ => _.Metadata);
        VerifierSettings.IgnoreMember<IMessageProcessingContext>(_ => _.ReplyToAddress);
        VerifierSettings.IgnoreMember<TestableEndpointInstance>(_ => _.EndpointStopped);
        VerifierSettings.IgnoreMember<TestableOutgoingLogicalMessageContext>(_ => _.RoutingStrategies);
        VerifierSettings.IgnoreMember<TestableOutgoingPhysicalMessageContext>(_ => _.RoutingStrategies);
        VerifierSettings.IgnoreMember<TestableRoutingContext>(_ => _.RoutingStrategies);
        VerifierSettings.IgnoreInstance<ContextBag>(_ => !ContextBagHelper.HasContent(_));
        VerifierSettings.AddExtraSettings(_ => _.Converters.AddRange(converters));
    }
}