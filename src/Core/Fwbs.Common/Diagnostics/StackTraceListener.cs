using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace FWBS.Common.Diagnostics
{
    public sealed class StackTraceListener : TextWriterTraceListener
    {

        // for our constructors, explicitly call the base class constructor.
        public StackTraceListener(System.IO.Stream stream, string name)
            :
            base(stream, name) { }
        public StackTraceListener(System.IO.Stream stream)
            :
            base(stream) { }
        public StackTraceListener(string fileName, string name)
            :
            base(fileName, name) { }
        public StackTraceListener(string fileName)
            :
            base(fileName) { }
        public StackTraceListener(System.IO.TextWriter writer, string name)
            :
            base(writer, name) { }
        public StackTraceListener(System.IO.TextWriter writer)
            :
            base(writer) { }


        public override void Write(string message)
        {

            base.Write(getPreambleMessage(message));
        }

        public override void WriteLine(string message)
        {

            base.WriteLine(getPreambleMessage(message));
        }


        [System.Runtime.CompilerServices.MethodImpl(
             System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        private string getPreambleMessage(string message)
        {

            StringBuilder preamble = new StringBuilder();

            StackTrace stackTrace = new StackTrace();
            StackFrame stackFrame;
            MethodBase stackFrameMethod;

            int frameCount = 0;
            string typeName;
            do
            {
                frameCount++;
                stackFrame = stackTrace.GetFrame(frameCount);
                stackFrameMethod = stackFrame.GetMethod();
                typeName = stackFrameMethod.ReflectedType.FullName;
            } while (typeName.StartsWith("System") || typeName.EndsWith("StackTraceListener"));

            //log DateTime, Namespace, Class and Method Name
            preamble.Append(DateTime.Now.ToString());
            preamble.Append(": ");
            preamble.Append(message);
            preamble.AppendLine();
            preamble.Append("\t");
            preamble.Append(typeName);
            preamble.Append(".");
            preamble.Append(stackFrameMethod.Name);
            preamble.Append("( ");

            // log parameter types and names
            ParameterInfo[] parameters = stackFrameMethod.GetParameters();
            int parameterIndex = 0;
            while (parameterIndex < parameters.Length)
            {
                preamble.Append(parameters[parameterIndex].ParameterType.Name);
                preamble.Append(" ");
                preamble.Append(parameters[parameterIndex].Name);
                parameterIndex++;
                if (parameterIndex != parameters.Length) preamble.Append(", ");
            }

            preamble.Append(" ): ");

            for (int ctr = frameCount + 1; ctr < stackTrace.FrameCount; ctr++)
            {
                StackFrame frame = stackTrace.GetFrame(ctr);
                MethodBase meth = frame.GetMethod();
                preamble.AppendLine();
                preamble.Append("\t");
                preamble.Append(meth.ReflectedType.FullName);
                preamble.Append(".");
                preamble.Append(meth.Name);
            }

            return preamble.ToString();

        }
    }
}
