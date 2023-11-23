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
        Recording.TryAdd("log", new LogMessage(LogLevel.Debug, message));

    public void Debug(string message, Exception exception) =>
        Recording.TryAdd("log", new LogMessage(LogLevel.Debug, message, exception));

    public void DebugFormat(string format, params object[] args) =>
        Recording.TryAdd("log", new LogMessage(LogLevel.Debug, format, args));

    public void Info(string message) =>
        Recording.TryAdd("log", new LogMessage(LogLevel.Info, message));

    public void Info(string message, Exception exception) =>
        Recording.TryAdd("log", new LogMessage(LogLevel.Info, message, exception));

    public void InfoFormat(string format, params object[] args) =>
        Recording.TryAdd("log", new LogMessage(LogLevel.Info, format, args));

    public void Warn(string message) =>
        Recording.TryAdd("log", new LogMessage(LogLevel.Warn, message));

    public void Warn(string message, Exception exception) =>
        Recording.TryAdd("log", new LogMessage(LogLevel.Warn, message, exception));

    public void WarnFormat(string format, params object[] args) =>
        Recording.TryAdd("log", new LogMessage(LogLevel.Warn, format, args));

    public void Error(string message) =>
        Recording.TryAdd("log", new LogMessage(LogLevel.Error, message));

    public void Error(string message, Exception exception) =>
        Recording.TryAdd("log", new LogMessage(LogLevel.Error, message, exception));

    public void ErrorFormat(string format, params object[] args) =>
        Recording.TryAdd("log", new LogMessage(LogLevel.Error, format, args));

    public void Fatal(string message) =>
        Recording.TryAdd("log", new LogMessage(LogLevel.Fatal, message));

    public void Fatal(string message, Exception exception) =>
        Recording.TryAdd("log", new LogMessage(LogLevel.Fatal, message, exception));

    public void FatalFormat(string format, params object[] args) =>
        Recording.TryAdd("log", new LogMessage(LogLevel.Fatal, format, args));

    public bool IsDebugEnabled => true;
    public bool IsInfoEnabled => true;
    public bool IsWarnEnabled => true;
    public bool IsErrorEnabled => true;
    public bool IsFatalEnabled => true;
}