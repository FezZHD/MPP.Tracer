using System;
using System.Diagnostics;
using System.Threading;
using Tracer.ImplementationClasses;
using Tracer.Interfaces;
using Tracer.Types;
using System.Collections.Concurrent;

namespace Tracer
{
    public sealed class Tracer : ITracer
    {
        private readonly object _lockObject = new object();
        private readonly ConcurrentStack<TraceMethodInfo> _calledMethodStack; 
        private readonly TraceResult _traceResult;
        private static readonly Lazy<Tracer> LazyInstance = new Lazy<Tracer>(() => new Tracer());

        public static Tracer Instance
        {
            get { return LazyInstance.Value; }                 
        }


        private Tracer()
        {
            _traceResult = new TraceResult();
            _calledMethodStack = new ConcurrentStack<TraceMethodInfo>();
        }


        public void StartTrace()
        {
            StackTrace stackTrace = new StackTrace();
            lock (_lockObject)
            {
                var currentMethod = stackTrace.GetFrame(1).GetMethod();
                var traceMethodInfo = new TraceMethodInfo(
                    currentMethod.Name, 
                    (currentMethod.ReflectedType != null) ? currentMethod.ReflectedType.Name : null, 
                    Stopwatch.StartNew(), 
                    (uint) currentMethod.GetParameters().Length, 
                    Thread.CurrentThread.ManagedThreadId
                    );
                _calledMethodStack.Push(traceMethodInfo);
                _traceResult.AddToNode(traceMethodInfo);
            }
        }


        public void StopTrace()
        {
            lock (_lockObject)
            {
                bool isPoped = false;
                TraceMethodInfo headStackMethodInfo;
                do
                {
                    if (_calledMethodStack.TryPop(out headStackMethodInfo))
                    {
                        isPoped = true;                 
                    }          
                } while (!isPoped);    
                _traceResult.RemoveFromStack(headStackMethodInfo);
            }
        }


        public TraceResult GetTraceResult()
        {
            return _traceResult;
        }
    }
}
