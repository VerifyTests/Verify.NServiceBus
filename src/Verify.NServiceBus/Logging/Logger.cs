class Logger :
    LoggingFactoryDefinition,
    ILoggerFactory,
    ILog
{
    static Logger instance = new();

    protected override ILoggerFactory GetLoggingFactory() =>
        instance;

    public ILog GetLogger(Type type) =>
        instance;

    public ILog GetLogger(string name) =>
        instance;

    public void Debug(string message) =>
        LogCapture.Add(new(LogLevel.Debug, message));

    public void Debug(string message, Exception exception) =>
        LogCapture.Add(new(LogLevel.Debug, message, exception));

    public void DebugFormat(string format, params object[] args) =>
        LogCapture.Add(new(LogLevel.Debug, format, args));

    public void Info(string message) =>
        LogCapture.Add(new(LogLevel.Info, message));

    public void Info(string message, Exception exception) =>
        LogCapture.Add(new(LogLevel.Info, message, exception));

    public void InfoFormat(string format, params object[] args) =>
        LogCapture.Add(new(LogLevel.Info, format, args));

    public void Warn(string message) =>
        LogCapture.Add(new(LogLevel.Warn, message));

    public void Warn(string message, Exception exception) =>
        LogCapture.Add(new(LogLevel.Warn, message, exception));

    public void WarnFormat(string format, params object[] args) =>
        LogCapture.Add(new(LogLevel.Warn, format, args));

    public void Error(string message) =>
        LogCapture.Add(new(LogLevel.Error, message));

    public void Error(string message, Exception exception) =>
        LogCapture.Add(new(LogLevel.Error, message, exception));

    public void ErrorFormat(string format, params object[] args) =>
        LogCapture.Add(new(LogLevel.Error, format, args));

    public void Fatal(string message) =>
        LogCapture.Add(new(LogLevel.Fatal, message));

    public void Fatal(string message, Exception exception) =>
        LogCapture.Add(new(LogLevel.Fatal, message, exception));

    public void FatalFormat(string format, params object[] args) =>
        LogCapture.Add(new(LogLevel.Fatal, format, args));

    public bool IsDebugEnabled { get; } = true;
    public bool IsInfoEnabled { get; } = true;
    public bool IsWarnEnabled { get; } = true;
    public bool IsErrorEnabled { get; } = true;
    public bool IsFatalEnabled { get; } = true;
}