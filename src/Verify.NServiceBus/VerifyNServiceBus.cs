﻿using NServiceBus.Extensibility;
using NServiceBus.Testing;
using VerifyTests.NServiceBus;

namespace VerifyTests;

public static class VerifyNServiceBus
{
    public static void Enable(bool captureLogs = true)
    {
        if (captureLogs)
        {
            LogCapture.Initialize();
        }
        VerifierSettings.ModifySerialization(settings =>
        {
            settings.IgnoreMember<TestableInvokeHandlerContext>(x => x.MessageHeaders);
            settings.IgnoreMember<TestableInvokeHandlerContext>(x => x.Headers);
            settings.IgnoreMember<TestableInvokeHandlerContext>(x => x.MessageId);
            settings.IgnoreMember<TestableInvokeHandlerContext>(x => x.MessageHandler);
            settings.IgnoreMember<TestableInvokeHandlerContext>(x => x.MessageBeingHandled);
            settings.IgnoreMember<TestableInvokeHandlerContext>(x => x.MessageMetadata);
            settings.IgnoreMember<IMessageProcessingContext>(x => x.ReplyToAddress);
            settings.IgnoreMember<TestableEndpointInstance>(x => x.EndpointStopped);
            settings.IgnoreMember<TestableOutgoingLogicalMessageContext>(x => x.RoutingStrategies);
            settings.IgnoreMember<TestableOutgoingPhysicalMessageContext>(x => x.RoutingStrategies);
            settings.IgnoreMember<TestableRoutingContext>(x => x.RoutingStrategies);
            settings.IgnoreInstance<ContextBag>(x => !ContextBagHelper.HasContent(x));
            settings.AddExtraSettings(serializerSettings =>
            {
                var converters = serializerSettings.Converters;
                converters.Add(new ContextBagConverter());
                converters.Add(new SendOptionsConverter());
                converters.Add(new ExtendableOptionsConverter());
                converters.Add(new UnsubscriptionConverter());
                converters.Add(new TimeoutMessageConverter());
                converters.Add(new MessageToHandlerMapConverter());
                converters.Add(new SubscriptionConverter());
                converters.Add(new OutgoingMessageConverter());
            });
        });
    }
}