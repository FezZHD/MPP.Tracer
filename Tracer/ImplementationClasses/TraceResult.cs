using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tracer.Types;

namespace Tracer.ImplementationClasses
{
    public class TraceResult
    {
        private List<TraceMethodInfo> _stackTracerList = new List<TraceMethodInfo>();

        private static object _lockAddObject = new object();
        private static object _lockRemoveObject = new object();



        public void AddToNode(TraceMethodInfo currentTracerInfo)
        {
            
        }
    }
}
