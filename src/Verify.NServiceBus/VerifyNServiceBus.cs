﻿namespace VerifyTests;

public static class VerifyNServiceBus
{
    public static void Enable(bool captureLogs = true)
    {
        InnerVerifier.ThrowIfVerifyHasBeenRun();
        if (captureLogs)
        {
            LogCapture.Initialize();
        }

        VerifierSettings.IgnoreMember<TestableMessageProcessingContext>(x => x.MessageHeaders);
        VerifierSettings.IgnoreMember<TestableInvokeHandlerContext>(x => x.Headers);
        VerifierSettings.IgnoreMember<TestableMessageProcessingContext>(x => x.MessageId);
        VerifierSettings.IgnoreMember<TestableInvokeHandlerContext>(x => x.MessageHandler);
        VerifierSettings.IgnoreMember<TestableInvokeHandlerContext>(x => x.MessageBeingHandled);
        VerifierSettings.IgnoreMember<TestableInvokeHandlerContext>(x => x.MessageMetadata);
        VerifierSettings.IgnoreMember<IMessageProcessingContext>(x => x.ReplyToAddress);
        VerifierSettings.IgnoreMember<TestableOutgoingLogicalMessageContext>(x => x.RoutingStrategies);
        VerifierSettings.IgnoreMember<TestableOutgoingPhysicalMessageContext>(x => x.RoutingStrategies);
        VerifierSettings.IgnoreMember<TestableRoutingContext>(x => x.RoutingStrategies);
        VerifierSettings.IgnoreInstance<ContextBag>(x => !ContextBagHelper.HasContent(x));
        VerifierSettings.AddExtraSettings(serializerSettings =>
        {
            var converters = serializerSettings.Converters;
            converters.Add(new ContextBagConverter());
            converters.Add(new TestableEndpointInstanceConverter());
            converters.Add(new TestableMessageHandlerContextConverter());
            converters.Add(new LogicalMessageConverter());
            converters.Add(new SendOptionsConverter());
            converters.Add(new ExtendableOptionsConverter());
            converters.Add(new UnsubscriptionConverter());
            converters.Add(new TimeoutMessageConverter());
            converters.Add(new MessageToHandlerMapConverter());
            converters.Add(new SubscriptionConverter());
            converters.Add(new OutgoingMessageConverter());
        });
    }
}