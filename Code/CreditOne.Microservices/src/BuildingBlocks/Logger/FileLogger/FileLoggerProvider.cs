using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CreditOne.Microservices.BuildingBlocks.Logger.FileLogger
{
    /// <summary>
    /// An <see cref="ILoggerProvider" /> that writes logs to a file.
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
    ///  </description>
    /// </item>
    /// </list>
    /// </remarks>
    [ProviderAlias("File")]
    public class FileLoggerProvider : BatchingLoggerProvider
    {
        private string _path;
        private string _fileName;
        private string _extension;
        private int? _maxFileSize;
        private int? _maxRetainedFiles;
        private PeriodicityOptions _periodicity;
        private readonly IDisposable _optionsChangeToken;

        /// <summary>
        /// Creates an instance of the <see cref="FileLoggerProvider" /> 
        /// </summary>
        /// <param name="options">The options object controlling the logger</param>
        public FileLoggerProvider(IOptionsMonitor<FileLoggerOptions> options) : base(options)
        {
            _optionsChangeToken = options.OnChange(UpdateOptions);
            UpdateOptions(options.CurrentValue);
        }

        /// <inheritdoc />
        protected override async Task WriteMessagesAsync(IEnumerable<LogMessage> messages, CancellationToken cancellationToken)
        {
            Directory.CreateDirectory(_path);

            foreach (var group in messages.GroupBy(GetGrouping))
            {
                var fullName = GetFullName(group.Key);
                var fileInfo = new FileInfo(fullName);
                if (_maxFileSize > 0 && fileInfo.Exists && fileInfo.Length > _maxFileSize)
                {
                    return;
                }

                using (var streamWriter = File.AppendText(fullName))
                {
                    foreach (var item in group)
                    {
                        await streamWriter.WriteAsync(item.Message);
                    }
                }
            }

            RollFiles();
        }

        private string GetFullName((int Year, int Month, int Day, int Hour, int Minute) group)
        {
            switch (_periodicity)
            {
                case PeriodicityOptions.Minutely:
                    return Path.Combine(_path, $"{_fileName}-{group.Year:0000}{group.Month:00}{group.Day:00}{group.Hour:00}{group.Minute:00}.{_extension}");
                case PeriodicityOptions.Hourly:
                    return Path.Combine(_path, $"{_fileName}-{group.Year:0000}{group.Month:00}{group.Day:00}{group.Hour:00}.{_extension}");
                case PeriodicityOptions.Daily:
                    return Path.Combine(_path, $"{_fileName}-{group.Year:0000}{group.Month:00}{group.Day:00}.{_extension}");
                case PeriodicityOptions.Monthly:
                    return Path.Combine(_path, $"{_fileName}-{group.Year:0000}{group.Month:00}.{_extension}");
            }
            throw new InvalidDataException("Invalid periodicity");
        }

        private (int Year, int Month, int Day, int Hour, int Minute) GetGrouping(LogMessage message)
        {
            return (message.Timestamp.Year, message.Timestamp.Month, message.Timestamp.Day, message.Timestamp.Hour, message.Timestamp.Minute);
        }

        /// <summary>
        /// Deletes old log files, keeping a number of files defined by <see cref="FileLoggerOptions.RetainedFileCountLimit" />
        /// </summary>
        protected void RollFiles()
        {
            if (_maxRetainedFiles > 0)
            {
                var files = new DirectoryInfo(_path)
                    .GetFiles(_fileName + "*")
                    .OrderByDescending(f => f.Name)
                    .Skip(_maxRetainedFiles.Value);

                foreach (var item in files)
                {
                    item.Delete();
                }
            }
        }

        private void UpdateOptions(FileLoggerOptions options)
        {
            if (options.FileName.Contains('\\'))
            {
                var filepath = options.FileName;
                _path = filepath.Substring(0, filepath.LastIndexOf('\\'));
                var fileNameWithExtension = filepath.Substring(filepath.LastIndexOf('\\') + 1);
                _fileName = fileNameWithExtension.Substring(0, fileNameWithExtension.LastIndexOf('.'));
                _extension = filepath.Substring(filepath.LastIndexOf('.') + 1);
            }
            else
            {
                _path = options.LogDirectory;
                _fileName = options.FileName;
                _extension = options.Extension;
            }

            _maxFileSize = options.FileSizeLimit;
            _maxRetainedFiles = options.RetainedFileCountLimit;
            _periodicity = options.Periodicity;
        }
    }
}