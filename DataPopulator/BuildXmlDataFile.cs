using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;

namespace DataPopulator
{
    // Here we shall populate the Xml Data File
    public class BuildXmlDataFile
    {
        public void LoadTextFileAndBuildXmlDB()
        {
            using(var reader = new StreamReader(@"AllWords.txt"))
            {
                while (reader.Peek() > 0)
                {
                    string word = reader.ReadLine();

                    if (!String.IsNullOrEmpty(word))
                    {
                        XDocument doc = XDocument.Load(@"DataFile.xml");

                        doc.Element("AllWords").Add(new XElement("Word", new XAttribute("id", word), new XAttribute("displayed",0)));

                        
                      

                        doc.Save(@"DataFile.xml");
                        //doc.Descendants("AllWords").Where(r=> r.)
                    }
                }
            }
        }
    }
}
