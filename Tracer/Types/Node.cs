using System.Collections;
using System.Collections.Generic;

namespace Tracer.Types
{
    internal class Node : IEnumerable
    {
        internal TraceMethodInfo NodeInfo { get; private set; }

        internal List<Node> ChildernNodes { get; private set; }

        internal Node(TraceMethodInfo currentTraceMethod)
        {
            NodeInfo = currentTraceMethod;
            ChildernNodes = new List<Node>();
        }

        public IEnumerator GetEnumerator()
        {
            yield return this;
        }
    }
}
