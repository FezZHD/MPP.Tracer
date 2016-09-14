using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tracer.ImplementationClasses;

namespace TracerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Tracer.Tracer.Instance.StartTrace();
            TestMethod();
            Tracer.Tracer.Instance.StopTrace();
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
    }
}
