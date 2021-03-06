﻿using System;
using System.Collections.Generic;
using System.Linq;
using Tracer.Types;

namespace Tracer.ImplementationClasses
{
    public sealed class TraceResult
    {

        private readonly Dictionary<int, List<TraceMethodInfo>> _stackTraceDictionary = new Dictionary<int, List<TraceMethodInfo>>();
        private readonly Lazy<Dictionary<int, Node>> _threadDictionaryInstance = new Lazy<Dictionary<int, Node>>(() => new Dictionary<int, Node>());

        internal Dictionary<int, Node> ThreadDictionary
        {
            get { return _threadDictionaryInstance.Value; }
        }


        internal void AddToNode(TraceMethodInfo currentTracerInfo)
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
                GetLastMethod(lastMethod, ThreadDictionary[lastMethod.ThreadId], out addingNode);
                addingNode.ChildernNodes.Add(new Node(currentTracerInfo));
            }
            AddToStack(currentTracerInfo);
        }


        internal void RemoveFromStack(TraceMethodInfo removableMethod)
        { 
            removableMethod.MethodWatch.Stop();
            _stackTraceDictionary[removableMethod.ThreadId].Remove(removableMethod);
        }


        private void AddToStack(TraceMethodInfo addingMethod)
        {
            _stackTraceDictionary[addingMethod.ThreadId].Add(addingMethod);
        }


        private void GetLastMethod(TraceMethodInfo methodInfo, Node currentNode, out Node lastMethodNode)//TODO try to make iterator here in future
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
                    {
                        break;
                    }     
                    foreach (Node inNodeNodes in node.ChildernNodes)
                    {
                       GetLastMethod(methodInfo, inNodeNodes, out lastMethodNode); 
                    }                    
                }
            }
        }
    }
}
