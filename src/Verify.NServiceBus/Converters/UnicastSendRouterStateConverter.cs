class UnicastSendRouterStateConverter :
    WriteOnlyJsonConverter
{
    public override bool CanConvert(Type type) =>
        UnicastRouterHelper.IsUnicastSendRouter(type);

    public override void Write(VerifyJsonWriter writer, object value)
    {
        writer.WriteStartObject();

        var explicitDestination = UnicastRouterHelper.GetExplicitDestination(value);
        if (explicitDestination != null)
        {
            writer.WriteMember(value, explicitDestination, "ExplicitDestination");
        }

        var specificInstance = UnicastRouterHelper.GetSpecificInstance(value);
        if (specificInstance != null)
        {
            writer.WriteMember(value, specificInstance, "SpecificInstance");
        }

        if (explicitDestination == null && specificInstance == null)
        {
            var option = UnicastRouterHelper.GetOption(value);
            writer.WriteMember(value, option, "Option");
        }

        writer.WriteEndObject();
    }
}