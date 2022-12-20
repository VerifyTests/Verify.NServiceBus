namespace VerifyTests;

public static class VerifyNServiceBus
{
    public static void Enable(bool captureLogs = true)
    {
        InnerVerifier.ThrowIfVerifyHasBeenRun();
        if (captureLogs)
        {
            LogCapture.Initialize();
        }

        VerifierSettings.IgnoreMember<IMessageProcessingContext>(x => x.ReplyToAddress);
        VerifierSettings.IgnoreInstance<ContextBag>(x => !ContextBagHelper.HasContent(x));
        VerifierSettings.AddExtraSettings(serializerSettings =>
        {
            var converters = serializerSettings.Converters;
            converters.Add(new AuditContextConverter());
            converters.Add(new BatchDispatchContextConverter());
            converters.Add(new ContextBagConverter());
            converters.Add(new DispatchContextConverter());
            converters.Add(new EndpointInstanceConverter());
            converters.Add(new ExtendableOptionsConverter());
            converters.Add(new IncomingContextConverter());
            converters.Add(new IncomingLogicalMessageContextConverter());
            converters.Add(new IncomingPhysicalMessageContextConverter());
            converters.Add(new LogicalMessageConverter());
            converters.Add(new MessageHandlerContextConverter());
            converters.Add(new MessageToHandlerMapConverter());
            converters.Add(new MessageSessionConverter());
            converters.Add(new MessageProcessingContextConverter());
            converters.Add(new OutgoingMessageConverter());
            converters.Add(new OutgoingPhysicalMessageContextConverter());
            converters.Add(new PipelineContextConverter());
            converters.Add(new RecoverabilityContextConverter());
            converters.Add(new RoutingContextConverter());
            converters.Add(new SendOptionsConverter());
            converters.Add(new SubscriptionConverter());
            converters.Add(new SubscribeContextConverter());
            converters.Add(new TransportReceiveContextConverter());
            converters.Add(new TimeoutMessageConverter());
            converters.Add(new UnsubscriptionConverter());
            converters.Add(new UnsubscribeContextConverter());
        });
    }
}