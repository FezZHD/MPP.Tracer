using System.Collections.Generic;
using System.Linq;
using Tracer.Types;

namespace Tracer.ImplementationClasses
{
    public class TraceResult
    {

        private readonly List<TraceMethodInfo> _stackTracerList = new List<TraceMethodInfo>();

        private static Dictionary<int, Node> _threadDictionaryInstance;
        private static readonly object LockAddObject = new object();
        private static readonly object LockRemoveObject = new object();
        private static readonly object InstanceLock = new object();

        public Dictionary<int, Node> ThreadDictionary
        {
            get
            {
                if (_threadDictionaryInstance == null)
                    lock (InstanceLock)
                    {
                        if (_threadDictionaryInstance == null)
                            _threadDictionaryInstance = new Dictionary<int, Node>();
                    }
                return _threadDictionaryInstance;
            }
        }



        public void AddToNode(TraceMethodInfo currentTracerInfo)
        {
            lock (LockAddObject)
            {
                TraceMethodInfo lastMethod = _stackTracerList.LastOrDefault(thread => thread.ThreadId == currentTracerInfo.ThreadId);
                if (lastMethod == null)
                {
                    ThreadDictionary.Add(currentTracerInfo.ThreadId, new Node(currentTracerInfo)); 
                    AddToStack(currentTracerInfo);
                }               
                else
                {
                    if (ThreadDictionary[lastMethod.ThreadId].ChildernNodes.Count == 0)
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
            lock (LockRemoveObject)
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
