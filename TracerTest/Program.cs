﻿using System;
using System.Threading;
using Tracer.Formatters;

namespace TracerTest
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Tracer.Tracer.Instance.StartTrace();
            TestMethod();
            object checkValue;           
            Thread testThread = new Thread(() => ThreadTestMethod(out checkValue)); 
            testThread.Start();  
            AnotherTestMethod(); 
            Thread.Sleep(3000);
            Tracer.Tracer.Instance.StopTrace();             
            testThread.Join();
            PrintResult();
            Save();
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
            InputInputMethod();
            Tracer.Tracer.Instance.StopTrace();
        }


        private static void ThreadTestMethod(out object checkValue)
        {
            Tracer.Tracer.Instance.StartTrace();
            checkValue = 228; 
            InThreadTest(1, 2);
            InThreadSecondMethod();
            Thread.Sleep(1500);
            Tracer.Tracer.Instance.StopTrace();
        }


        private static void AnotherTestMethod()
        {
            Tracer.Tracer.Instance.StartTrace();
            Thread.Sleep(200);
            Tracer.Tracer.Instance.StopTrace();
        }


        private static void InThreadTest(int firstValue, int secondValue)
        {
            Tracer.Tracer.Instance.StartTrace();
            int result = firstValue + secondValue;
            Thread.Sleep(1500);
            Tracer.Tracer.Instance.StopTrace();
        }


        private static void InThreadSecondMethod()
        {
            Tracer.Tracer.Instance.StartTrace();
            InputTestMethod();
            Thread.Sleep(1000);
            Tracer.Tracer.Instance.StopTrace();
        }


        private static void PrintResult()
        {
            var consoleOutput = new ConsoleFormatter();
            consoleOutput.Format(Tracer.Tracer.Instance.GetTraceResult());
        }


        private static void InputInputMethod()
        {
            Tracer.Tracer.Instance.StartTrace();
            Thread.Sleep(200);
            Tracer.Tracer.Instance.StopTrace();
        }


        private static void Save()
        {
            var xmlOutput = new XmlFormatter();
            xmlOutput.Format(Tracer.Tracer.Instance.GetTraceResult());
        }
    }
}
