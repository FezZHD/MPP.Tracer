using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tracer.Types;

namespace Tracer.ImplementationClasses
{
    public class TraceResult
    {

        private readonly Dictionary<int, List<TraceMethodInfo>> _stackTraceDictionary = new Dictionary<int, List<TraceMethodInfo>>();
        private Dictionary<int, Node> _threadDictionaryInstance;
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
            TraceMethodInfo lastMethod = null;
            if (_stackTraceDictionary.ContainsKey(currentTracerInfo.ThreadId))
            {
                lastMethod =
                    _stackTraceDictionary[currentTracerInfo.ThreadId].LastOrDefault(
                        thread => thread.ThreadId == currentTracerInfo.ThreadId);
            }
            if (lastMethod == null)
            {
                ThreadDictionary.Add(currentTracerInfo.ThreadId, new Node(currentTracerInfo));
                _stackTraceDictionary.Add(currentTracerInfo.ThreadId, new List<TraceMethodInfo>());
                _stackTraceDictionary[currentTracerInfo.ThreadId].Add(currentTracerInfo);
            }
            else
            {
                Node addingNode;
                GetLastMethod(lastMethod, out addingNode, ThreadDictionary[lastMethod.ThreadId]);
                addingNode.ChildernNodes.Add(new Node(currentTracerInfo));
            }
            AddToStack(currentTracerInfo);
        }


        public void RemoveFromStack(TraceMethodInfo removableMethod)
        { 
            removableMethod.MethodWatch.Stop();
            _stackTraceDictionary[removableMethod.ThreadId].Remove(removableMethod);
        }


        private void AddToStack(TraceMethodInfo addingMethod)
        {
            _stackTraceDictionary[addingMethod.ThreadId].Add(addingMethod);
        }


        private void GetLastMethod(TraceMethodInfo methodInfo, out Node lastMethodNode, Node currentNode)//TODO try to make iterator here it future
        {
            lastMethodNode = null;
            foreach (Node node in currentNode)
            {
                if (node.NodeInfo.Equals(methodInfo))
                {
                    lastMethodNode = node;
                    break;
                }
                else
                {
                    if (lastMethodNode != null)
                        break;
                    foreach (Node inNode in node.ChildernNodes)
                    {
                       GetLastMethod(methodInfo, out lastMethodNode,inNode); 
                    }                    
                }
            }
        }
    }
}
