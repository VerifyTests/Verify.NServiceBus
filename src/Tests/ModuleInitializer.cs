public static class ModuleInitializer
{
    #region enable

    [ModuleInitializer]
    public static void Initialize() =>
        VerifyNServiceBus.Initialize();

    #endregion

    [ModuleInitializer]
    public static void InitializeOther()
    {
        VerifierSettings.InitializePlugins();
        VerifierSettings.IgnoreMembers<RecordingHandlerContext>(
            _ => _.DoNotContinueDispatchingCurrentMessageToHandlersWasCalled);
    }
}