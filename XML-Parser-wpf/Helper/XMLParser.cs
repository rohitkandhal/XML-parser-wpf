using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;

namespace Test1.Helper
{
    class XMLParser
    {
        public static XDocument GetXMLDataFromFileName(string fileName)
        {
            return XDocument.Load(new FileStream(fileName, FileMode.Open));
        }
    }
}
