using System.Collections.Generic;

namespace Tracer.Types
{
    public class Node
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
