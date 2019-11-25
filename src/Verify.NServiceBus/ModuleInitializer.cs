using NServiceBus.ApprovalTests;

static class ModuleInitializer
{
    public static void Initialize()
    {
        LogCapture.Initialize();
    }
}