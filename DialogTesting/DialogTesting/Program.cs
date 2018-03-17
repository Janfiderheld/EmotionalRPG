using System;
using System.Collections.Generic;
using System.Xml;

namespace DialogTesting
{
    class Program
    {
        // speichert die Strings
        // Keys: Knotennamen aus XML
        // Values: Text 
        private Dictionary<string, string> strings = new Dictionary<string, string>();

        static void Main(string[] args)
        {
            Program prg = new Program();
            prg.FillTheDictionary();


        }

        private void FillTheDictionary()
        {
            try
            {
                int i = 0;
                // neuen XML-Reader für gegebene Datei erstellen
                XmlReader xmlReader = XmlReader.Create("C://Users//IXE5KB//Documents//Hackathons//HEX//Dialoge.xml");
                // Solange in der XML-Datei noch knoten da sind...
                while (xmlReader.Read())
                {
                    // Da der Reader merkwürdigerweise nach jedem gültigen Element ein leeres Element einliest, 
                    // wird nur jeder zweite Knoten verwendet. Und das auch nur wenn es das Startelement eines Knoten ist
                    // (kein </...>). Zusätzlich werden alle Elemente mit "Entscheidung" im Namen ausgeschlossen, da diese nicht benötigt werden
                    if ((i % 2) == 0 && xmlReader.IsStartElement() && !xmlReader.Name.Contains("Entscheidung"))
                    {
                        if (xmlReader.GetAttribute("Text") != null)
                        {
                            string tempAttribute = xmlReader.GetAttribute("Text");
                            if (tempAttribute.Contains("ENTER"))
                            {
                                tempAttribute = tempAttribute.Replace("ENTER", System.Environment.NewLine.ToString());
                            }
                            strings.Add(xmlReader.Name, tempAttribute);
                        }
                        // Console.WriteLine("Node: {0}\tText: {1}", xmlReader.Name, xmlReader.GetAttribute("Text"));
                    }
                    i++;
                }
                // Entfernt den "Dialoge" und den "Endings"-Knoten, da diese ebenfalls nicht benötigt werden
                strings.Remove("Dialoge");
                strings.Remove("Endings");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
