using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Fwbs.Framework
{
    internal static class EventLogger
    {
        private const string SEP = "-----------------------------------------------------------------------------------------------";

        public static void Write(string title, string text, string resolution, Exception exception)
        {

            try
            {
                if (!EventLog.SourceExists("OMSDOTNET"))
                    EventLog.CreateEventSource("OMSDOTNET", "Application");
            }
            catch (Exception)
            {
                //Do not have permission to create the OMSDOTNET event source.
                return;
            }

            exception = ResolveException(exception);


            StringBuilder header = new StringBuilder();
            StringBuilder msg = new StringBuilder();

            var ex = exception;

            header.Append(title);
            header.AppendLine();
            header.AppendLine(text);


            if (!String.IsNullOrEmpty(resolution))
            {
                msg.AppendLine();
                msg.AppendLine();
                msg.AppendLine("Resolution: ");
                msg.AppendLine();
                msg.AppendLine(resolution);
            }

            while (ex != null)
            {
                Write(exception, msg);
                ex = ex.InnerException;
            }

            // Create an EventLog instance and assign its source.
            EventLog myLog = new EventLog();
            myLog.Source = "OMSDOTNET";
            myLog.WriteEntry(header.ToString() + SEP + msg.ToString(), EventLogEntryType.Error);
            myLog.Dispose();

        }

        public static void Write(Exception exception)
        {
            // Create the source, if it does not already exist.
            if (!EventLog.SourceExists("OMSDOTNET"))
                EventLog.CreateEventSource("OMSDOTNET", "Application");

            exception = ResolveException(exception);


            StringBuilder msg = new StringBuilder();

            var ex = exception;

            while (ex != null)
            {
                Write(exception, msg);
                ex = ex.InnerException;
            }

            // Create an EventLog instance and assign its source.
            EventLog myLog = new EventLog();
            myLog.Source = "OMSDOTNET";
            myLog.WriteEntry(exception.Message + Environment.NewLine + SEP + msg.ToString(), EventLogEntryType.Error);
            myLog.Dispose();
        }


        private static Exception ResolveException(Exception exception)
        {
            var tex = exception as TargetException;

            if (tex != null)
            {
                return ResolveException(tex.InnerException);
            }

            return exception;
        }

        private static void Write(Exception exception, StringBuilder msg)
        {
            var comex = exception as COMException;

            if (msg.Length > 0)
            {
                msg.AppendLine("-----------------------------------------------------------------------------------------------");
                msg.AppendLine();
            }

            msg.AppendLine(exception.GetType().Name);
            msg.AppendLine(exception.Message);
            if (comex != null)
            {
                msg.AppendLine("HResult: " + comex.ErrorCode.ToString());
            }

            msg.AppendLine();

            msg.Append(Environment.NewLine);
            msg.Append(exception.StackTrace);
            msg.Append(Environment.NewLine);

        }



    }
}
