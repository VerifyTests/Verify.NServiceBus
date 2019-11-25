using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Extensibility;
using NServiceBus.Logging;
using NServiceBus.ObjectBuilder;
using NServiceBus.Testing;
using VerifyXunit;

namespace Verify.NServiceBus
{
    public static class TestContextVerifier
    {
        static TestContextVerifier()
        {
            Global.ModifySerialization(settings =>
            {
                settings.IgnoreMember<TestableInvokeHandlerContext>(x => x.MessageHeaders);
                settings.IgnoreMember<TestableInvokeHandlerContext>(x => x.Headers);
                settings.IgnoreMember<TestableInvokeHandlerContext>(x => x.Extensions);
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
                settings.IgnoreMembersWithType<IBuilder>();
                settings.AddExtraSettings(serializerSettings =>
                {

                    var converters = serializerSettings.Converters;
                    converters.Add(new ContextBagConverter());
                    converters.Add(new SendOptionsConverter());
                    converters.Add(new ExtendableOptionsConverter());
                    converters.Add(new UnsubscriptionConverter());
                    converters.Add(new TimeoutMessageConverter());
                    converters.Add(new SubscriptionConverter());
                    converters.Add(new OutgoingMessageConverter());
                });
            });
        }

        static Task InnerVerify(VerifyBase verifyBase, object context, object? state, LogLevel? includeLogMessages)
        {
            Guard.AgainstNull(context, nameof(context));

            List<LogMessage>? logMessages = null;
            if (includeLogMessages != null)
            {
                logMessages = LogCapture.LogMessages
                    .Where(x => x.Level > includeLogMessages.Value)
                    .ToList();
            }

            if (state == null && logMessages == null)
            {
                return verifyBase.Verify(context);
            }

            var wrapper = new ContextWrapper(context)
            {
                ExtraState = state,
                LogMessages = logMessages
            };
            return verifyBase.Verify(wrapper);
        }

        public static Task VerifyContext(this VerifyBase verifyBase, TestableAuditContext context, object? state = null, LogLevel? includeLogMessages = null)
        {
            return InnerVerify(verifyBase, context, state, includeLogMessages);
        }

        public static Task VerifyContext(this VerifyBase verifyBase, TestableBatchDispatchContext context, object? state = null, LogLevel? includeLogMessages = null)
        {
            return InnerVerify(verifyBase, context, state, includeLogMessages);
        }

        public static Task VerifyContext(this VerifyBase verifyBase, TestableBehaviorContext context, object? state = null, LogLevel? includeLogMessages = null)
        {
            return InnerVerify(verifyBase, context, state, includeLogMessages);
        }

        public static Task VerifyContext(this VerifyBase verifyBase, TestableDispatchContext context, object? state = null, LogLevel? includeLogMessages = null)
        {
            return InnerVerify(verifyBase, context, state, includeLogMessages);
        }

        public static Task VerifyContext(this VerifyBase verifyBase, TestableEndpointInstance context, object? state = null, LogLevel? includeLogMessages = null)
        {
            return InnerVerify(verifyBase, context, state, includeLogMessages);
        }

        public static Task VerifyContext(this VerifyBase verifyBase, TestableForwardingContext context, object? state = null, LogLevel? includeLogMessages = null)
        {
            return InnerVerify(verifyBase, context, state, includeLogMessages);
        }

        public static Task VerifyContext(this VerifyBase verifyBase, TestableIncomingLogicalMessageContext context, object? state = null, LogLevel? includeLogMessages = null)
        {
            return InnerVerify(verifyBase, context, state, includeLogMessages);
        }

        public static Task VerifyContext(this VerifyBase verifyBase, TestableIncomingPhysicalMessageContext context, object? state = null, LogLevel? includeLogMessages = null)
        {
            return InnerVerify(verifyBase, context, state, includeLogMessages);
        }

        public static Task VerifyContext(this VerifyBase verifyBase, TestableInvokeHandlerContext context, object? state = null, LogLevel? includeLogMessages = null)
        {
            return InnerVerify(verifyBase, context, state, includeLogMessages);
        }

        public static Task VerifyContext(this VerifyBase verifyBase, TestableMessageHandlerContext context, object? state = null, LogLevel? includeLogMessages = null)
        {
            return InnerVerify(verifyBase, context, state, includeLogMessages);
        }

        public static Task VerifyContext(this VerifyBase verifyBase, TestableMessageProcessingContext context, object? state = null, LogLevel? includeLogMessages = null)
        {
            return InnerVerify(verifyBase, context, state, includeLogMessages);
        }

        public static Task VerifyContext(this VerifyBase verifyBase, TestableMessageSession context, object? state = null, LogLevel? includeLogMessages = null)
        {
            return InnerVerify(verifyBase, context, state, includeLogMessages);
        }

        public static Task VerifyContext(this VerifyBase verifyBase, TestableOutgoingContext context, object? state = null, LogLevel? includeLogMessages = null)
        {
            return InnerVerify(verifyBase, context, state, includeLogMessages);
        }

        public static Task VerifyContext(this VerifyBase verifyBase, TestableOutgoingLogicalMessageContext context, object? state = null, LogLevel? includeLogMessages = null)
        {
            return InnerVerify(verifyBase, context, state, includeLogMessages);
        }

        public static Task VerifyContext(this VerifyBase verifyBase, TestableOutgoingPhysicalMessageContext context, object? state = null, LogLevel? includeLogMessages = null)
        {
            return InnerVerify(verifyBase, context, state, includeLogMessages);
        }

        public static Task VerifyContext(this VerifyBase verifyBase, TestableOutgoingPublishContext context, object? state = null, LogLevel? includeLogMessages = null)
        {
            return InnerVerify(verifyBase, context, state, includeLogMessages);
        }

        public static Task VerifyContext(this VerifyBase verifyBase, TestableOutgoingReplyContext context, object? state = null, LogLevel? includeLogMessages = null)
        {
            return InnerVerify(verifyBase, context, state, includeLogMessages);
        }

        public static Task VerifyContext(this VerifyBase verifyBase, TestableOutgoingSendContext context, object? state = null, LogLevel? includeLogMessages = null)
        {
            return InnerVerify(verifyBase, context, state, includeLogMessages);
        }

        public static Task VerifyContext(this VerifyBase verifyBase, TestablePipelineContext context, object? state = null, LogLevel? includeLogMessages = null)
        {
            return InnerVerify(verifyBase, context, state, includeLogMessages);
        }

        public static Task VerifyContext(this VerifyBase verifyBase, TestableRoutingContext context, object? state = null, LogLevel? includeLogMessages = null)
        {
            return InnerVerify(verifyBase, context, state, includeLogMessages);
        }

        public static Task VerifyContext(this VerifyBase verifyBase, TestableSubscribeContext context, object? state = null, LogLevel? includeLogMessages = null)
        {
            return InnerVerify(verifyBase, context, state, includeLogMessages);
        }

        public static Task VerifyContext(this VerifyBase verifyBase, TestableTransportReceiveContext context, object? state = null, LogLevel? includeLogMessages = null)
        {
            return InnerVerify(verifyBase, context, state, includeLogMessages);
        }

        public static Task VerifyContext(this VerifyBase verifyBase, TestableUnsubscribeContext context, object? state = null, LogLevel? includeLogMessages = null)
        {
            return InnerVerify(verifyBase, context, state, includeLogMessages);
        }
    }
}