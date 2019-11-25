using System.Collections.Generic;

class ContextWrapper
{
    public object? ExtraState;
    public object NsbTestContext;
    public List<LogMessage>? LogMessages;

    public ContextWrapper(object context)
    {
        NsbTestContext = context;
    }
}