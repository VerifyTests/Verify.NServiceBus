public static class ModuleInitializer
{
    #region enable

    [ModuleInitializer]
    public static void Initialize() =>
        VerifyNServiceBus.Enable();

    #endregion

    [ModuleInitializer]
    public static void InitializeOther()
    {
        VerifyDiffPlex.Initialize();
        VerifierSettings.IgnoreMembers<TestableInvokeHandlerContext>(
            _ => _.DoNotContinueDispatchingCurrentMessageToHandlersWasCalled,
            _ => _.HandlerInvocationAborted);
    }
}