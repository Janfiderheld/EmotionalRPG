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
        private string nextNode = "Einleitung";

        static void Main(string[] args)
        {
            string str;
            Program prg = new Program();
            prg.FillTheDictionary();
            do
            {
                prg.strings.TryGetValue(prg.nextNode, out str);
                Console.WriteLine(str);
                Console.ReadLine();
                prg.Decide(prg.nextNode);

            } while (!prg.nextNode.Contains("Ende"));
            prg.strings.TryGetValue(prg.nextNode, out str);
            Console.WriteLine(str);
            Console.ReadLine();
        }

        private void Decide(string currentNode)
        {
            if (currentNode.Contains("Antwort") && !currentNode.Equals("E3_AntwortRueckfrage"))
            {
                nextNode = currentNode.Replace("Antwort", "Reaktion");
                return;
            }
            else if (currentNode.Equals("Einleitung"))
            {
                nextNode = "E1_Frage";
            }
            else if (currentNode.Equals("E3_Frage"))
            {
                nextNode = "E3_Rueckfrage";
            } else if (currentNode.Equals("E3_Rueckfrage"))
            {
                nextNode = "E3_AntwortRueckfrage";
            } else if (currentNode.Contains("Frage") || currentNode.Equals("E3_AntwortRueckfrage"))
            {
                currentNode = currentNode.Split('_')[0] + "_";
                EmotionEnum emo = EmotionAPI.EmotionRequest().Result;
                switch (emo)
                {  
                    case EmotionEnum.Anger:
                        currentNode = currentNode.Replace("_", "_AntwortWut");
                        break;
                    case EmotionEnum.Contempt:
                        currentNode = currentNode.Replace("_", "_AntwortVerachtung");
                        break;
                    case EmotionEnum.Disgust:
                        currentNode = currentNode.Replace("_", "_AntwortEkel");
                        break;
                    case EmotionEnum.Fear:
                        currentNode = currentNode.Replace("_", "_AntwortAngst");
                        break;
                    case EmotionEnum.Happy:
                        currentNode = currentNode.Replace("_", "_AntwortFreude");
                        break;
                    case EmotionEnum.Neutral:
                        currentNode = currentNode.Replace("_", "_AntwortNeutral");
                        break;
                    case EmotionEnum.Sadness:
                        currentNode = currentNode.Replace("_", "_AntwortTrauer");
                        break;
                    case EmotionEnum.Surprise:
                        currentNode = currentNode.Replace("_", "_AntwortUeberraschung");
                        break;
                }
                if (!strings.ContainsKey(currentNode)) {
                    nextNode = currentNode.Split('_')[0] + "_AntwortNeutral";
                } else {
                    nextNode = currentNode;
                }
            } else if (currentNode.Contains("Reaktion"))
            {
                if (currentNode.Contains("E1")){
                    nextNode = "E2_Frage";
                }
                else if (currentNode.Contains("E2"))
                {
                    if (currentNode.Contains("Neutral") || (currentNode.Contains("Angst")))
                    {
                        nextNode = "E3_Frage";
                    }
                    else
                    {
                        nextNode = "E5_Frage";
                    }
                }
                else if (currentNode.Contains("E3"))
                {
                    if (currentNode.Contains("Ekel"))
                    {
                        nextNode = "Ende6_Wookiee";
                    } else
                    {
                        nextNode = "E4_Frage";
                    }

                }
                else if (currentNode.Contains("E4"))
                {
                    if (currentNode.Contains("Freude") || currentNode.Contains("Neutral"))
                    {
                        nextNode = "Ende4_CommanderAlone";
                    }
                    else
                    {
                        nextNode = "Ende5_CommanderTogether";
                    }
                }
                else if (currentNode.Contains("E5"))
                {
                    if (currentNode.Contains("Neutral"))
                    {
                        nextNode = "E4_Frage";
                    } else
                    {
                        nextNode = "E6_Frage";
                    }
                }
                else if (currentNode.Contains("E6"))
                {
                    if (currentNode.Contains("Freude"))
                    {
                        nextNode = "Ende1_BothFlee";
                    } else if (currentNode.Contains("Neutral"))
                    {
                        nextNode = "Ende3_ST2Leaves";
                    } else
                    {
                        nextNode = "Ende2_Fight";
                    }
                }
            }
            return;
        }

        private void FillTheDictionary()
        {
            try
            {
                int i = 0;
                // neuen XML-Reader für gegebene Datei erstellen
                XmlReader xmlReader = XmlReader.Create("C:\\Users\\Alissa\\source\\repos\\Dialoge.xml");
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
             /*            Console.WriteLine("Node: {0}\tText: {1}", xmlReader.Name, xmlReader.GetAttribute("Text"));
                        Console.ReadLine();
               */     }
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
