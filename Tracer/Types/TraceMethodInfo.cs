using System.Diagnostics;

namespace Tracer.Types
{
    public class TraceMethodInfo
    {

        public string MethodName { get; private set; }

        public string ClassName { get; private set; }

        public Stopwatch MethodWatch { get; private set; }

        public uint ParametersCount { get; private set; }

        public int ThreadId { get; private set; }

        public TraceMethodInfo(string methodName, string className, Stopwatch methodWatch, uint parametersCount, int treadId)
        {
            MethodName = methodName;
            ClassName = className;
            MethodWatch = methodWatch;
            ParametersCount = parametersCount;
            ThreadId = treadId;
        }
    }
}
