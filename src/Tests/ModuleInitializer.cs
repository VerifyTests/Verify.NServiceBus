using NServiceBus.Testing;

public static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Initialize()
    {
        VerifyNServiceBus.Enable();
        VerifierSettings.IgnoreMembers<TestableInvokeHandlerContext>(
            _ => _.DoNotContinueDispatchingCurrentMessageToHandlersWasCalled,
            _ => _.HandlerInvocationAborted);
    }
}