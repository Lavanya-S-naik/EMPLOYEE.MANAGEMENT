
using Microsoft.Extensions.Logging;





    /// <summary>
    /// Logger provider that creates instances of <see cref="SimpleFileLogger"/>.
    /// </summary>
    public class SimpleFileLoggerProvider : ILoggerProvider

    {

        private readonly string _filePath;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleFileLoggerProvider"/> class.
        /// </summary>
        /// <param name="filePath">The file path where log entries will be written.</param>
        public SimpleFileLoggerProvider(string filePath)

        {

            _filePath = filePath;

        }

        /// <inheritdoc />
        public ILogger CreateLogger(string categoryName)

        {

            return new SimpleFileLogger(categoryName, _filePath);

        }

        /// <inheritdoc />
        public void Dispose()

        {

        }

    }


