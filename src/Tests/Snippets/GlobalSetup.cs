using VerifyTests;
using Xunit;

public static class ModuleInitializer
{
    public static void Initialize()
    {
        VerifyNServiceBus.Enable();
    }
}