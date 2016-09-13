using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tracer.ImplementationClasses;
using Tracer.Interfaces;

namespace Tracer
{
    public class Tracer:ITracer
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


        private Tracer() { }


        public void StartTrace()
        {
            lock (_startLockObject)
            {
                //Stopwatch.StartNew();
                StackTrace stackTrace = new StackTrace();
                var currentMethod = stackTrace.GetFrame(1).GetMethod();
                //TODO initialize TraceResult class to create treelist for info
            }
        }


        public void StopTrace()
        {
            throw new NotImplementedException();
        }


        public TraceResult GetTraceResult()
        {
            throw new NotImplementedException();
 
        }
    }
}
