using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Tracer.ImplementationClasses;
using Tracer.Interfaces;
using Tracer.Types;

namespace Tracer
{
    public class Tracer : ITracer
    {

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


        private readonly object _startLockObject = new object();
        private readonly object _stopLockObject = new object();
        private static readonly object InstanceLockObject = new object();

        private readonly Stack<TraceMethodInfo> _calledMethodStack; 

        private TraceResult _traceResult;

        private Tracer()
        {
            _traceResult = new TraceResult();
            _calledMethodStack = new Stack<TraceMethodInfo>();
        }


        public void StartTrace()
        {
            StackTrace stackTrace = new StackTrace();
            lock (_startLockObject)
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
            lock (_stopLockObject)
            {
                  _traceResult.RemoveFromStack(_calledMethodStack.Pop());
            }
        }


        public TraceResult GetTraceResult()
        {
            return _traceResult;
        }
    }
}
