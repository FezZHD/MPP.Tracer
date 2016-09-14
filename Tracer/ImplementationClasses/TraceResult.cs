using System.Collections.Generic;
using System.Linq;
using Tracer.Types;

namespace Tracer.ImplementationClasses
{
    public class TraceResult
    {

        private readonly List<TraceMethodInfo> _stackTracerList = new List<TraceMethodInfo>();

        private Dictionary<int, Node> _threadDictionaryInstance;
        private readonly object _lockObject = new object();
        private readonly object _instanceLock = new object();

        public Dictionary<int, Node> ThreadDictionary
        {
            get
            {
                if (_threadDictionaryInstance == null)
                    lock (_instanceLock)
                    {
                        if (_threadDictionaryInstance == null)
                            _threadDictionaryInstance = new Dictionary<int, Node>();
                    }
                return _threadDictionaryInstance;
            }
        }



        public void AddToNode(TraceMethodInfo currentTracerInfo)
        {
            lock (_lockObject)
            {
                TraceMethodInfo lastMethod = _stackTracerList.LastOrDefault(thread => thread.ThreadId == currentTracerInfo.ThreadId);
                if (lastMethod == null)
                {
                    ThreadDictionary.Add(currentTracerInfo.ThreadId, new Node(currentTracerInfo)); 
                    AddToStack(currentTracerInfo);
                }               
                else
                {
                    if ((ThreadDictionary[lastMethod.ThreadId].ChildernNodes.Count == 0) || (lastMethod.Equals(ThreadDictionary[lastMethod.ThreadId].NodeInfo)))
                    {
                        ThreadDictionary[lastMethod.ThreadId].ChildernNodes.Add(new Node(currentTracerInfo));
                    }
                    else
                    {
                        ThreadDictionary[lastMethod.ThreadId].ChildernNodes.Last(method => method.NodeInfo.Equals(lastMethod)).ChildernNodes.Add(new Node(currentTracerInfo));
                    }
                    AddToStack(currentTracerInfo);
                }
            }
        }


        public void RemoveFromStack(TraceMethodInfo removableMethod)
        {
            lock (_lockObject)
            {   
                removableMethod.MethodWatch.Stop();
                _stackTracerList.Remove(_stackTracerList[_stackTracerList.Count - 1]);
            }
        }


        private void AddToStack(TraceMethodInfo addingMethod)
        {
            _stackTracerList.Add(addingMethod);
        }
    }
}
