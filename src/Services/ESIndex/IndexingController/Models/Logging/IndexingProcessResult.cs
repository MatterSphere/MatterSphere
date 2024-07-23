using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models.Common;
using Models.DbModels;
using Models.ElasticsearchModels;
using Models.Interfaces;

namespace IndexingController.Models.Logging
{
    public class IndexingProcessResult
    {
        public static readonly string NewLine = Environment.NewLine;
        public Guid LogID { get; private set; }
        public ElasticsearchResponse Response { get; set; }
        public IEnumerable<object> ProcessingItems { get; set; }
        public TimeSpan Elapsed { get; set; }
        public ParametersData Params { get; set; }
        public Exception Exception { get; set; }
        public bool ExceptionOccurred => Exception != null;
        public string ProcessedObjectType { get; set; }
        public bool CompletedSuccessfully => !ExceptionOccurred && !Response.HasErrors;
        public bool HasErrors => Exception != null || (Response?.HasErrors ?? false);
        public bool UsingExtendedLogs => Params?.UseExtendedLogs ?? false;

        public int NumberOfItemsSucceeded
        {
            get
            {
                if (Response == null) { return -1; }
                return Response.Result ? Response.SuccessNumber : Response.FailedNumber;
            }
        }

        public int NumberOfItemsFailed
        {
            get
            {
                if (Response == null) { return -1; }
                var totalItems = (ProcessingItems?.Count() ?? Response.Logs?.Count ?? -1);
                if (Exception != null) { return totalItems; }
                return Response.Result ? 0 : totalItems;
            }
        }

        public IndexingProcessResult()
        {
            LogID = Guid.NewGuid();
        }

        public override string ToString()
        {
            var logBuilder = new StringBuilder("Indexing process result:");
            if (ExceptionOccurred)
            {
                logBuilder.Append($" Exception occurred");
                logBuilder.Append($"{NewLine}Error: {Exception.Message}");
            }
            else if (Response.HasErrors)
            {
                logBuilder.Append($" Completed with errors");
                if (Response.Logs != null)
                {
                    foreach (KeyValuePair<Guid, ProcessingItemLog> logItem in Response.Logs)
                    {
                        logBuilder.Append($"{NewLine}[{logItem.Key}]{logItem.Value.ErrorType}");
                        logBuilder.Append($"{NewLine}{logItem.Value.ErrorReason}");
                    }
                }
            }
            else
            {
                logBuilder.Append($" Completed successfully");
            }

            if (!string.IsNullOrWhiteSpace(ProcessedObjectType))
            {
                logBuilder.Append($"{NewLine}Message type: {ProcessedObjectType}");
            }
            logBuilder.Append($"{NewLine}Log ID: {LogID}");

            if (Elapsed != TimeSpan.Zero)
            {
                var daysStr = Elapsed.Days > 0 ? $"{Elapsed.Days}d " : string.Empty;
                logBuilder.Append($"{NewLine}Duration: {daysStr}{Elapsed.ToString(@"%hh\:%mm\:%ss\.fff")}");
            }

            logBuilder.Append($"{NewLine}Items: Succeeded - {NumberOfItemsSucceeded}; Failed - {NumberOfItemsFailed}");

            if (UsingExtendedLogs && Response?.Logs != null)
            {
                logBuilder.Append($"{NewLine}{ToReadableLog(Response.Logs, NewLine)}");
            }

            if (ExceptionOccurred && !string.IsNullOrEmpty(Exception.StackTrace))
            {
                logBuilder.Append($"{NewLine}{NewLine}{Exception.StackTrace}");
            }

            return logBuilder.ToString();
        }

        public MessageLog ToMessageLog()
        {
            return new MessageLog(
                Elapsed,
                ProcessedObjectType,
                NumberOfItemsSucceeded,
                NumberOfItemsFailed
            );
        }
        
        private string ToReadableLog(IDictionary<Guid, ProcessingItemLog> logs, string separator = "\r\n")
        {
            return string.Join(separator, logs.Select(ToReadableLogItem));
        }

        private string ToReadableLogItem(KeyValuePair<Guid, ProcessingItemLog> logPair)
        {
            var item = logPair.Value;
            return $"Entity '{Convert.ToString(logPair.Key)}' - {(item.IsSucceeded ? "succeeded" : "failed")}"
                + $", {item.Result}";
        }

    }
}
