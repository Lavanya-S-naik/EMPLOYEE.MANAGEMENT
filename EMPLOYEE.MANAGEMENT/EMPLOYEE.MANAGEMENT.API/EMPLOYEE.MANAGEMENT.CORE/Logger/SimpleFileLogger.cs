using Microsoft.Extensions.Logging;





    /// <summary>
    /// Simple file-based logger implementation that writes log messages to a file.
    /// </summary>
    public class SimpleFileLogger : ILogger
    {
        private readonly string _filePath;
        private readonly string _categoryName;
        private static readonly object _lock = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleFileLogger"/> class.
        /// </summary>
        /// <param name="categoryName">The logging category name.</param>
        /// <param name="filePath">The file path where logs will be written.</param>
        public SimpleFileLogger(string categoryName, string filePath)
        {
            _categoryName = categoryName;
            _filePath = filePath;
        }

        /// <inheritdoc />
        public IDisposable BeginScope<TState>(TState state) => null!;

        /// <inheritdoc />
        public bool IsEnabled(LogLevel logLevel) =>
            logLevel >= LogLevel.Information;

        /// <inheritdoc />
        public void Log<TState>(LogLevel logLevel, EventId eventId,
            TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel)) return;

            var message = $"{DateTime.UtcNow:u} [{logLevel}] {_categoryName}: {formatter(state, exception)}";
            if (exception != null)
                message += Environment.NewLine + exception;

            lock (_lock)
            {
                File.AppendAllText(_filePath, message + Environment.NewLine);
            }
        }
    }
