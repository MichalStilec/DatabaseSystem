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
        /// <summary>
        /// Načte databázi podle IP adresy, kvůli vzdálenému připojení
        /// </summary>
        /// <returns></returns>
        public string LoadDataSource()
        {
            return ConfigStructure("Data Source", "193.85.203.188");
        }
        /// <summary>
        /// Načte danou databázi podle jména 
        /// </summary>
        /// <returns></returns>
        public string LoadCatalog()
        {
            return ConfigStructure("Initial Catalog", "stilec2");
        }
        /// <summary>
        /// Přihlásí uživatele podle přihlašovacího jména
        /// </summary>
        /// <returns></returns>
        public string LoadUser()
        {
            return ConfigStructure("User ID", "stilec2");
        }
        /// <summary>
        /// K přihlášení je povinné i heslo,
        /// heslo samozřejmě nikde jinde nepoužívám
        /// </summary>
        /// <returns></returns>
        public string LoadPassword()
        {
            return ConfigStructure("Password", "cojetoheslo");
        }

        /// <summary>
        /// Načítá vybraný řádek ze souboru config.cfg
        /// </summary>
        /// <param name="data">Název řádku</param>
        /// <param name="defaultData">Základní hodnota, která se nastaví pokud program nenajde jinou</param>
        /// <returns></returns>
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
