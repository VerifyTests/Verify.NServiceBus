static class BehaviorContextConverter
{
    public static void WriteMembers(VerifyJsonWriter writer, TestableBehaviorContext context) =>
        writer.WriteMember(context, context.Extensions, "Extensions");
}