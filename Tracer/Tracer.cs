using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Tracer.ImplementationClasses;
using Tracer.Interfaces;
using Tracer.Types;
using System.Collections.Concurrent;

namespace Tracer
{
    public class Tracer : ITracer
    {
        private readonly object _lockObject = new object();
        private static readonly object InstanceLockObject = new object();

        private readonly ConcurrentStack<TraceMethodInfo> _calledMethodStack; 

        private readonly TraceResult _traceResult;
        private static Tracer _instance;

        public static Tracer Instance
        {
            get
            {
                if(_instance == null)
                    lock (InstanceLockObject)
                    {
                        if (_instance == null)
                            _instance = new Tracer();
                    }
            return _instance;
            }
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
                var traceMethodInfo = new TraceMethodInfo(currentMethod.Name, 
                    (currentMethod.DeclaringType != null)?currentMethod.DeclaringType.ToString():null, 
                    Stopwatch.StartNew(), 
                    (uint) currentMethod.GetParameters().Length, 
                    Thread.CurrentThread.ManagedThreadId);
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
                        isPoped = true;
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
