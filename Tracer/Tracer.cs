using System;
using System.Diagnostics;
using System.Threading;
using Tracer.ImplementationClasses;
using Tracer.Interfaces;
using Tracer.Types;
using System.Collections.Generic;

namespace Tracer
{
    public sealed class Tracer : ITracer
    {
        private readonly object _lockObject = new object();
        private readonly Dictionary<int, Stack<TraceMethodInfo>> _calledMethodStack; 
        private readonly TraceResult _traceResult;
        private static readonly Lazy<Tracer> LazyInstance = new Lazy<Tracer>(() => new Tracer());

        public static Tracer Instance
        {
            get { return LazyInstance.Value; }                 
        }


        private Tracer()
        {
            _traceResult = new TraceResult();
            _calledMethodStack = new Dictionary<int, Stack<TraceMethodInfo>>();
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
                AddToStack(traceMethodInfo);
                _traceResult.AddToNode(traceMethodInfo);
            }
        }


        public void StopTrace()
        {
            lock (_lockObject)
            {
                int currentThread = Thread.CurrentThread.ManagedThreadId;
                _traceResult.RemoveFromStack(_calledMethodStack[currentThread].Pop());
            }
        }


        public TraceResult GetTraceResult()
        {
            return _traceResult;
        }


        private void AddToStack(TraceMethodInfo currentMethodInfo)
        {
            if (!_calledMethodStack.ContainsKey(currentMethodInfo.ThreadId))
            {
                _calledMethodStack.Add(currentMethodInfo.ThreadId,new Stack<TraceMethodInfo>());
            }
            _calledMethodStack[currentMethodInfo.ThreadId].Push(currentMethodInfo);   
        }
    }
}
