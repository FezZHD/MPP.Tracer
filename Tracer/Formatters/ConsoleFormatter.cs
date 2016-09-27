using System;
using Tracer.ImplementationClasses;
using Tracer.Interfaces;
using Tracer.Types;

namespace Tracer.Formatters
{
    public sealed class ConsoleFormatter: ITraceResultFormatter
    {
        private const uint DefaultTabCount = 0;

        public void Format(TraceResult traceResult)
        {
            foreach (var thread in traceResult.ThreadDictionary)
            {
                Console.WriteLine("Thread {0} Time: {1} ms", thread.Key, thread.Value.NodeInfo.MethodWatch.Elapsed.Milliseconds);
                PrintMethod(thread.Value, DefaultTabCount);
            }   
        }
       

        private void PrintMethod(Node methodeNode, uint tabs)
        {
            string tabsString = new string('\t', (int)tabs);
            tabs++;
            Console.WriteLine("{0} Method Name: {1}, Class Name: {2}, Time: {3} ms, Parameters: {4}", 
                tabsString, 
                methodeNode.NodeInfo.MethodName, 
                methodeNode.NodeInfo.ClassName,
                methodeNode.NodeInfo.MethodWatch.Elapsed.Milliseconds, 
                methodeNode.NodeInfo.ParametersCount);
            foreach (var leafList in methodeNode.ChildernNodes)
            {
                PrintMethod(leafList, tabs);
            }
        }
    }
}
