using System;
using System.Threading;
using System.Windows.Forms;
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
            Cycles();
            Thread.Sleep(3000);
            Tracer.Tracer.Instance.StopTrace();             
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
            Thread.Sleep(500);
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


        private static void CycleMethod()
        {
            Tracer.Tracer.Instance.StartTrace();
            Thread.Sleep(100);
            Tracer.Tracer.Instance.StopTrace();
        }


        private static void Cycles()
        {
            Tracer.Tracer.Instance.StartTrace();
            for (int index = 0; index <= 5; index++)
            {
                CycleMethod();
            }
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
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "XML|*.xml";
            saveFileDialog.RestoreDirectory = true;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                var xmlOutput = new XmlFormatter(saveFileDialog.FileName);
                xmlOutput.Format(Tracer.Tracer.Instance.GetTraceResult());
                MessageBox.Show("Saved");
            }
        }
    }
}
