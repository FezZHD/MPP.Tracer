using System;
using System.Xml.Linq;
using Tracer.ImplementationClasses;
using Tracer.Interfaces;
using Tracer.Types;
using System.Windows.Forms;

namespace Tracer.Formatters
{
    
    public class XmlFormatter: ITraceResultFormatter
    {
        private XDocument _xmlOutputDocument = new XDocument();

        public void Format(TraceResult traceResult)
        {
            var root = new XElement("Trace");
            foreach (var thread in traceResult.ThreadDictionary)
            {
                var head = new XElement("Thread", 
                        new XAttribute("ID", thread.Key), 
                        new XAttribute("Time", thread.Value.NodeInfo.MethodWatch.Elapsed.Milliseconds));
                PrintMethod(thread.Value, head);
                root.Add(head);
            }
            _xmlOutputDocument.Add(root);
            SaveInFile();
        }


        private void PrintMethod(Node methodNode, XElement method)
        {
            var currentMethod = new XElement("Method",
                    new XAttribute("Name", methodNode.NodeInfo.MethodName),
                    new XAttribute("Class", methodNode.NodeInfo.ClassName),
                    new XAttribute("Time", methodNode.NodeInfo.MethodWatch.Elapsed.Milliseconds),
                    new XAttribute("Parameters", methodNode.NodeInfo.ParametersCount));
            method.Add(currentMethod);
            foreach (var leafNode in methodNode.ChildernNodes)
            {
                PrintMethod(leafNode, currentMethod);
            }
        }

        
        private void SaveInFile()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "XML|*.xml";
            saveFileDialog.RestoreDirectory = true;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                _xmlOutputDocument.Save(saveFileDialog.FileName);
                MessageBox.Show("Saved");
            }
        }
    }
}