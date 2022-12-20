public static class ModuleInitializer
{
    #region enable

    [ModuleInitializer]
    public static void Initialize()
    {
        VerifyNServiceBus.Enable();

        #endregion

        VerifyDiffPlex.Initialize();
    }
}