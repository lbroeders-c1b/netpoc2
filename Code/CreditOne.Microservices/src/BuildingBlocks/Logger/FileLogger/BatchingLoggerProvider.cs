using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CreditOne.Microservices.BuildingBlocks.Logger.FileLogger
{
    /// <summary>
    /// An <see cref="ILoggerProvider" /> that writes logs to a queue.
    /// </summary>
    /// <remarks>
    /// <list type="table">
    /// <listheader>
    ///  <term>Date</term>
    ///  <term>Who</term>
    ///  <term>BR/WO</term>
    ///  <description>Description</description>
    /// </listheader>
    /// <item>
    ///  <term>05-12-2020</term>
    ///  <term>Daniel Lobaton</term>
    ///  <term>WO376694</term>
    ///  <description>
    ///  Migrated from CreditOne.Operations.Common
    ///  New method GetMinLogLevel, returns the minimun log level for the given category
    ///  </description>
    /// </item>
    /// </list>
    /// </remarks>
    public abstract class BatchingLoggerProvider : ILoggerProvider, ISupportExternalScope
    {
        #region Private Members

        private const string Application = "Application";
        private const char DotSplitter = '.';

        private readonly List<LogMessage> _currentBatch = new List<LogMessage>();
        private TimeSpan _interval;
        private int? _queueSize;
        private int? _batchSize;
        private readonly IDisposable _optionsChangeToken;
        private BlockingCollection<LogMessage> _messageQueue;
        private Task _outputTask;
        private CancellationTokenSource _cancellationTokenSource;

        private bool _includeScopes;
        private Dictionary<string, LogLevel> _logLevel;
        private readonly string _appName;

        private IExternalScopeProvider _scopeProvider;

        #endregion

        internal IExternalScopeProvider ScopeProvider => _includeScopes ? _scopeProvider : null;

        #region Constructors

        /// <summary>
        /// Creates an instance of the <see cref="BatchingLoggerProvider" /> 
        /// </summary>
        /// <param name="options">The options object controlling the logger</param>
        protected BatchingLoggerProvider(IOptionsMonitor<BatchingLoggerOptions> options)
        {
            var loggerOptions = options.CurrentValue;
            if (loggerOptions.BatchSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(loggerOptions.BatchSize), $"{nameof(loggerOptions.BatchSize)} must be a positive number.");
            }
            if (loggerOptions.FlushPeriod <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(loggerOptions.FlushPeriod), $"{nameof(loggerOptions.FlushPeriod)} must be longer than zero.");
            }

            _appName = loggerOptions.ApplicationName;
            _logLevel = loggerOptions.LogLevel;
            _interval = loggerOptions.FlushPeriod;
            _batchSize = loggerOptions.BatchSize;
            _queueSize = loggerOptions.BackgroundQueueSize;

            _optionsChangeToken = options.OnChange(UpdateOptions);
            UpdateOptions(options.CurrentValue);
        }

        #endregion

        #region Public Properties

        public bool IsEnabled { get; private set; }

        protected abstract Task WriteMessagesAsync(IEnumerable<LogMessage> messages, CancellationToken token);

        #endregion

        #region Public Methods

        protected virtual Task IntervalAsync(TimeSpan interval, CancellationToken cancellationToken)
        {
            return Task.Delay(interval, cancellationToken);
        }

        internal void AddMessage(DateTimeOffset timestamp, string message)
        {
            if (!_messageQueue.IsAddingCompleted)
            {
                _messageQueue.Add(new LogMessage { Message = message, Timestamp = timestamp }, _cancellationTokenSource.Token);
            }
        }

        public void Dispose()
        {
            _optionsChangeToken?.Dispose();
            if (IsEnabled)
            {
                Stop();
            }
        }

        /// <summary>
        /// Creates a new Microsoft.Extensions.Logging.ILogger instance.
        /// </summary>
        public ILogger CreateLogger(string categoryName)
        {
            return new BatchingLogger(this, categoryName);
        }

        /// <summary>
        /// Sets external scope information source for logger provider.
        /// </summary>
        void ISupportExternalScope.SetScopeProvider(IExternalScopeProvider scopeProvider)
        {
            _scopeProvider = scopeProvider;
        }

        #endregion

        #region Private Methods

        private void UpdateOptions(BatchingLoggerOptions options)
        {
            var oldIsEnabled = IsEnabled;
            IsEnabled = options.IsEnabled;

            if (options.BatchSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(options.BatchSize), $"{nameof(options.BatchSize)} must be a positive number.");
            }

            if (options.FlushPeriod <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(options.FlushPeriod), $"{nameof(options.FlushPeriod)} must be longer than zero.");
            }
            _includeScopes = options.IncludeScopes;
            _logLevel = options.LogLevel;
            _interval = options.FlushPeriod;
            _batchSize = options.BatchSize;
            _queueSize = options.BackgroundQueueSize;

            if (oldIsEnabled != IsEnabled)
            {
                if (IsEnabled)
                {
                    Start();
                }
                else
                {
                    Stop();
                }
            }

        }

        private async Task ProcessLogQueue()
        {
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                var limit = _batchSize ?? int.MaxValue;

                while (limit > 0 && _messageQueue.TryTake(out var message))
                {
                    _currentBatch.Add(message);
                    limit--;
                }

                if (_currentBatch.Count > 0)
                {
                    try
                    {
                        await WriteMessagesAsync(_currentBatch, _cancellationTokenSource.Token);
                    }
                    catch
                    {
                        // ignored
                    }

                    _currentBatch.Clear();
                }

                await IntervalAsync(_interval, _cancellationTokenSource.Token);
            }
        }

        private void Start()
        {
            _messageQueue = _queueSize == null ?
                new BlockingCollection<LogMessage>(new ConcurrentQueue<LogMessage>()) :
                new BlockingCollection<LogMessage>(new ConcurrentQueue<LogMessage>(), _queueSize.Value);

            _cancellationTokenSource = new CancellationTokenSource();
            _outputTask = Task.Run(ProcessLogQueue);
        }

        private void Stop()
        {
            _cancellationTokenSource.Cancel();
            _messageQueue.CompleteAdding();

            try
            {
                _outputTask.Wait(_interval);
            }
            catch (TaskCanceledException)
            {
            }
            catch (AggregateException ex) when (ex.InnerExceptions.Count == 1 && ex.InnerExceptions[0] is TaskCanceledException)
            {
            }
        }

        /// <summary>
        /// Returns the minimun log level for the given category.
        /// </summary>
        /// <param name="categoryName"></param>
        public LogLevel GetMinLogLevel(string categoryName)
        {
            var appKey = categoryName.Split(DotSplitter).ToList().First() == _appName ? Application : categoryName.Split(DotSplitter).ToList().First();
            var minLogLevel = _logLevel.Keys.Contains(appKey) ? _logLevel[appKey] : LogLevel.Error;
            return minLogLevel;
        }


        #endregion
    }
}