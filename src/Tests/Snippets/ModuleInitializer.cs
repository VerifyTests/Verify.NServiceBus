using Verify.NServiceBus;

public static class ModuleInitializer
{
    public static void Initialize()
    {
        VerifyNServiceBus.Enable();
    }
}