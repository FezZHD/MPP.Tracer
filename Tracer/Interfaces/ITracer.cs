using Tracer.ImplementationClasses;

namespace Tracer.Interfaces
{
    interface ITracer
    {
        void StartTrace();


        void StopTrace();


        TraceResult GetTraceResult();
    }
}
