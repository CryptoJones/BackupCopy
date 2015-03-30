using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;

namespace BackupCopy
{
    class Program
    {
        public class GlobalVariables{
            public static string targetPath;
            public static string sourcePath;
        }

        public static void ParseXML(XmlDocument xmlFile, XmlNamespaceManager xmlnm)
        {
            XmlNodeList nodes = xmlFile.SelectNodes("//ns:SourcePath | //ns:TargetPath", xmlnm);
            foreach (XmlNode node in nodes)
            {
                //  MessageBox.Show(node.Name + " = " + node.InnerXml);
                if (node.Name == "SourcePath")
                {
                    GlobalVariables.sourcePath = node.InnerXml;
                }
                else if (node.Name == "TargetPath")
                {
                    GlobalVariables.targetPath = node.InnerXml;
                }

            }
        }

        static void Main(string[] args)
        {

            // Open XML Document
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load("config.xml");
            XmlNamespaceManager xmlnm = new XmlNamespaceManager(xmlDocument.NameTable);
            xmlnm.AddNamespace("ns", "http://www.w3.org/2005/Atom");
            ParseXML(xmlDocument, xmlnm);

           
            // Verify Source Path Exists
            if (!Directory.Exists(GlobalVariables.sourcePath))
            {
                Console.WriteLine("Source path does not exist.");
                Console.ReadLine();
                return; 
            }


             //Backup each file in Array
            foreach (string file in Directory.EnumerateFiles(GlobalVariables.sourcePath))
            {
                Backup(file);
            }
        
        }

        public static void Backup(string filename)
        {
            // Pull filename from method input parameter
            string fileName = filename;          
          
            // Verify Target Path Exists
            if (!Directory.Exists(GlobalVariables.targetPath))
            {
                Console.WriteLine("Target path does not exist.");
                Console.ReadLine();
                return; 
            }

            // Use Path class to manipulate file and directory paths. 
            string sourceFile = System.IO.Path.Combine(GlobalVariables.sourcePath, fileName);
            string destFile = System.IO.Path.Combine(GlobalVariables.targetPath, fileName);

            // To copy a file to another location and  
            // overwrite the destination file if it already exists.
            try
            {
                File.Copy(sourceFile, Path.Combine(GlobalVariables.targetPath, Path.GetFileName(fileName)), true);
            }
            catch (IOException exc)
            {
                Console.WriteLine(exc);
                Console.ReadLine();
                return;
            }
        }
    }

}
