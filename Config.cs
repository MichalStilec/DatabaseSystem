using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseSystem
{
    public class Config
    {
        // Metody pro načtení dat z konfiguračního souboru
        public string LoadDataSource()
        {
            return ConfigStructure("Data Source", "193.85.203.188");
        }
        public string LoadCatalog()
        {
            return ConfigStructure("Initial Catalog", "stilec2");
        }
        public string LoadUser()
        {
            return ConfigStructure("User ID", "stilec2");
        }
        public string LoadPassword()
        {
            return ConfigStructure("Password", "cojetoheslo");
        }

        private string ConfigStructure(string data, string defaultData)
        {
            string configFile = "config/config.cfg";

            if (File.Exists(configFile))
            {
                // Načtení všech řádků konfiguračního souboru
                string[] lines = File.ReadAllLines(configFile);

                foreach (var line in lines)
                {
                    var parts = line.Split('=');

                    // Kontrola, zda řádek obsahuje daný nastavení
                    if (parts.Length == 2 && parts[0].Trim() == data)
                    {
                        return parts[1].Trim();
                    }
                }
            }

            // Návrat výchozí hodnoty v případě nenalezení správné hodnoty
            return defaultData;
        }
    }
}
