using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Tracer.Types
{
    class Node
    {
        public TraceMethodInfo NodeInfo { get; private set; }

        public List<Node> ChildernNodes { get; private set; }

        public Node(TraceMethodInfo currentTraceMethod)
        {
            NodeInfo = currentTraceMethod;
            ChildernNodes = new List<Node>();
        }
    }
}
