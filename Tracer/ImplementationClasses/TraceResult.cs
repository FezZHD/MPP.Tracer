﻿using System;
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
                }
                else
                {
                    ThreadDictionary[lastMethod.ThreadId].ChildernNodes.Add(new Node(currentTracerInfo));
                }
            }
        }


        public void RemoveFromStack()
        {
            lock (LockRemoveObject)
            {
                
            }
        }
    }
}
