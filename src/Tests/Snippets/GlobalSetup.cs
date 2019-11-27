using Verify.NServiceBus;
using Xunit;

[GlobalSetUp]
public static class GlobalSetup
{
    public static void Setup()
    {
        VerifyNServiceBus.Enable();
    }
}