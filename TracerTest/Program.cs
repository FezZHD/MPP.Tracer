using System;
using System.Threading;
using Tracer.ImplementationClasses;

namespace TracerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Tracer.Tracer.Instance.StartTrace();
            TestMethod();
            object checkValue = 0;
            Tracer.Tracer.Instance.StopTrace();
            Thread testThread = new Thread(() => ThreadTestMethod(out checkValue));     
            testThread.Start();
            testThread.Join();
            Console.WriteLine("End");
            TraceResult testResult = Tracer.Tracer.Instance.GetTraceResult();
            Console.ReadLine();
        }

        private static void TestMethod()
        {
            Thread.Sleep(500);
            Tracer.Tracer.Instance.StartTrace();
            InputTestMethod();
            Tracer.Tracer.Instance.StopTrace();

        }

        private static void InputTestMethod()
        {   
            Tracer.Tracer.Instance.StartTrace();
            Thread.Sleep(1000);
            Tracer.Tracer.Instance.StopTrace();
        }


        private static void ThreadTestMethod(out object checkValue)
        {
            Tracer.Tracer.Instance.StartTrace();
            checkValue = 228;
            Thread.Sleep(1500);
            Tracer.Tracer.Instance.StopTrace();
        }
    }
}
