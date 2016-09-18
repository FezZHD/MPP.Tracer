using Tracer.ImplementationClasses;

namespace Tracer.Interfaces
{
    internal interface ITracer
    {
        void StartTrace();


        void StopTrace();


        TraceResult GetTraceResult();
    }
}
