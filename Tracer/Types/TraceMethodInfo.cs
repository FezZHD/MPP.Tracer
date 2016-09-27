using System.Diagnostics;

namespace Tracer.Types
{
    internal class TraceMethodInfo
    {

        internal string MethodName { get; private set; }

        internal string ClassName { get; private set; }

        internal Stopwatch MethodWatch { get; private set; }

        internal uint ParametersCount { get; private set; }

        internal int ThreadId { get; private set; }

        internal TraceMethodInfo(string methodName, string className, Stopwatch methodWatch, uint parametersCount, int treadId)
        {
            MethodName = methodName;
            ClassName = className;
            MethodWatch = methodWatch;
            ParametersCount = parametersCount;
            ThreadId = treadId;
        }
    }
}
