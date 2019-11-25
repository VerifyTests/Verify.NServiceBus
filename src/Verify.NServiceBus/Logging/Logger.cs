using System;
using NServiceBus.ApprovalTests;
using NServiceBus.Logging;

class Logger :
    LoggingFactoryDefinition,
    ILoggerFactory,
    ILog
{
    static Logger instance = new Logger();

    protected override ILoggerFactory GetLoggingFactory()
    {
        return instance;
    }

    public ILog GetLogger(Type type)
    {
        return instance;
    }

    public ILog GetLogger(string name)
    {
        return instance;
    }

    public void Debug(string message)
    {
        LogCapture.LogMessages.Add(new LogMessage(LogLevel.Debug, message));
    }

    public void Debug(string message, Exception exception)
    {
        LogCapture.LogMessages.Add(new LogMessage(LogLevel.Debug, message, exception));
    }

    public void DebugFormat(string format, params object[] args)
    {
        LogCapture.LogMessages.Add(new LogMessage(LogLevel.Debug, format, args));
    }

    public void Info(string message)
    {
        LogCapture.LogMessages.Add(new LogMessage(LogLevel.Info, message));
    }

    public void Info(string message, Exception exception)
    {
        LogCapture.LogMessages.Add(new LogMessage(LogLevel.Info, message, exception));
    }

    public void InfoFormat(string format, params object[] args)
    {
        LogCapture.LogMessages.Add(new LogMessage(LogLevel.Info, format, args));
    }

    public void Warn(string message)
    {
        LogCapture.LogMessages.Add(new LogMessage(LogLevel.Warn, message));
    }

    public void Warn(string message, Exception exception)
    {
        LogCapture.LogMessages.Add(new LogMessage(LogLevel.Warn, message, exception));
    }

    public void WarnFormat(string format, params object[] args)
    {
        LogCapture.LogMessages.Add(new LogMessage(LogLevel.Warn, format, args));
    }

    public void Error(string message)
    {
        LogCapture.LogMessages.Add(new LogMessage(LogLevel.Error, message));
    }

    public void Error(string message, Exception exception)
    {
        LogCapture.Context.logMessages.Add(new LogMessage(LogLevel.Error, message, exception));
    }

    public void ErrorFormat(string format, params object[] args)
    {
        LogCapture.LogMessages.Add(new LogMessage(LogLevel.Error, format, args));
    }

    public void Fatal(string message)
    {
        LogCapture.LogMessages.Add(new LogMessage(LogLevel.Fatal, message));
    }

    public void Fatal(string message, Exception exception)
    {
        LogCapture.LogMessages.Add(new LogMessage(LogLevel.Fatal, message, exception));
    }

    public void FatalFormat(string format, params object[] args)
    {
        LogCapture.LogMessages.Add(new LogMessage(LogLevel.Fatal, format, args));
    }

    public bool IsDebugEnabled { get; } = true;
    public bool IsInfoEnabled { get; } = true;
    public bool IsWarnEnabled { get; } = true;
    public bool IsErrorEnabled { get; } = true;
    public bool IsFatalEnabled { get; } = true;
}