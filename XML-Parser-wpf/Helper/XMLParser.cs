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
        /// <summary>
        /// Loads XML Data File from given file name
        /// </summary>
        /// <param name="fileName">File Source Path</param>
        /// <returns>Queryable XML doc</returns>
        public static XDocument GetXMLDataFromFileName(string fileName)
        {
            // Validate File Path
            if (fileName == null || !System.IO.File.Exists((string)fileName))
            {
                throw new FileNotFoundException("XML Not found at specified path.");
            }

            FileStream xmlStream = new FileStream(fileName, FileMode.Open);

            return XDocument.Load(xmlStream);
        }
    }
}
