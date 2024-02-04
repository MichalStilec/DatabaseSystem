using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Povolí programu používat háčky a čárky
            Console.OutputEncoding = Encoding.UTF8;

            // Zde se načte veškerý config
            Config config = new Config();
            string source = config.LoadDataSource(); string catalog = config.LoadCatalog(); 
            string user = config.LoadUser(); string passwrd = config.LoadPassword();

            string connectionString = $"Data Source={source};Initial Catalog={catalog};User ID={user};Password={passwrd};";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    // Zde se program zkusí poprvé připojit do databáze
                    connection.Open();
                    bool end = false;
                    Console.WriteLine("Program se úspěšně připojil do databáze " + catalog + "\n");
                    connection.Close();

                    while (!end)
                    {
                        // Hlavní menu programu
                        Console.WriteLine("---      Alpha.3      ---\n" +
                                          "Vyberte jednu z možností: \n" +
                                          "1. Možnosti tabulky Vězeň \n" +
                                          "2. Vypsat report \n" +
                                          "3. Import souboru CSV\n" +
                                          "0. Ukončit program \n");

                        string answer = Console.ReadLine();

                        switch (answer) 
                        {

                            case "0":
                                connection.Close();
                                end = true;
                                break;

                            case "1":
                                Console.Clear();
                                Console.WriteLine("Vyberte co chcete udělat s tabulkou Vězeň: \n" +
                                                  "1. Vypsat vězně\n" +
                                                  "2. Přidat vězně\n" +
                                                  "3. Smazat vězně\n" +
                                                  "4. Upravit vězně\n" +
                                                  "0. Ukončit výběr\n");
                                string vyber = Console.ReadLine();
                                if (vyber != "1" && vyber != "2" && vyber != "3" && vyber != "4") { Console.Clear(); break; }
                                string jmno = "";

                                if (vyber == "1")
                                {
                                    Console.Clear(); Console.WriteLine(Vezen.VypisVeznu(connection));
                                    Console.WriteLine("Stiskněte libovolné tlačítko pro pokračování"); Console.ReadKey(true);
                                }
                                else if (vyber == "2")
                                {
                                    try
                                    {
                                        Console.WriteLine("Napiš následující informace: Jméno"); jmno = Console.ReadLine();
                                        Console.WriteLine("Napiš následující informace: Příjmení"); string prjmni = Console.ReadLine();
                                        Console.WriteLine("Napiš následující informace: Datum narození"); string dtm = Console.ReadLine(); DateTime datm = DateTime.Parse(dtm);
                                        Console.Write("Napiš následující informace: Výška \n(desetinné číslo pište s čárkou): "); string vsk = Console.ReadLine(); double vska = double.Parse(vsk);
                                        Vezen.VlozVezne(connection, jmno, prjmni, datm, vska);
                                        Console.WriteLine("Vězeň " + jmno + " byl úspěšně přidán\n");
                                        Console.WriteLine("Stiskněte libovolné tlačítko pro pokračování"); Console.ReadKey(true);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine("Během zadávání hodnot se vyskytla chyba\n" + ex.Message + "\n\nStiskněte libovolné tlačítko pro pokračování"); Console.ReadKey(true);
                                    }
                                    
                                }
                                else if (vyber == "3")
                                {
                                    try
                                    {
                                        Console.Clear(); Console.WriteLine(Vezen.VypisVeznu(connection) + "\nNapiš ID vězně, kterého chceš smazat: ");
                                        string smz = Console.ReadLine(); int smaz = int.Parse(smz);
                                        Vezen.SmazVezne(connection, smaz);
                                        Console.WriteLine("Vězeň " + smaz + " byl úspěšně smazán");
                                        Console.WriteLine("Stiskněte libovolné tlačítko pro pokračování"); Console.ReadKey(true);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine("Během mazání vězně se vyskytla chyba\n" + ex.Message + "\n\nStiskněte libovolné tlačítko pro pokračování"); Console.ReadKey(true);
                                    }
                                }
                                else if (vyber == "4")
                                {
                                    try
                                    {
                                        Console.Clear(); Console.WriteLine(Vezen.VypisVeznu(connection) + "\nNapiš ID vězně, kterého chceš upravit: ");
                                        string id = Console.ReadLine(); int idd = int.Parse(id);
                                        Console.WriteLine("Napiš nové jméno vězně"); string noveJmeno = Console.ReadLine();
                                        Vezen.UpravVezne(connection, noveJmeno, idd);
                                        Console.WriteLine("Vězeň " + idd + " byl úspěšně upraven");
                                        Console.WriteLine("Stiskněte libovolné tlačítko pro pokračování"); Console.ReadKey(true);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine("Během úpravy vězně se vyskytla chyba\n" + ex.Message + "\n\nStiskněte libovolné tlačítko pro pokračování"); Console.ReadKey(true);
                                    }
                                }
                                Console.Clear();
                                break;

                            case "2":
                                Console.Clear();
                                Report.VygenerujReport(connection);
                                Console.WriteLine("Stiskněte libovolné tlačítko pro pokračování");
                                Console.ReadKey(true);
                                Console.Clear();
                                break;

                            case "3":
                                Console.Clear();
                                Console.WriteLine("Vyberte do které tabulky chcete importovat data: \n" +
                                                  "1. Bachar\n" +
                                                  "2. Veznice\n" +
                                                  "0. Ukončit výběr\n");
                                string table = Console.ReadLine();
                                if (table != "1" && table != "2") { Console.Clear(); break; }
                                Console.WriteLine("Vyberte CSV soubor pro import");
                                string file = Console.ReadLine();
                                Console.WriteLine(CSVimport.Import(connection, table, file));
                                Console.WriteLine("Stiskněte libovolné tlačítko pro pokračování");
                                Console.ReadKey(true);
                                Console.Clear();

                                break;

                            default:
                                Console.Clear();
                                Console.WriteLine("Neplatná volba, zkuste to znovu\n");
                                break;
                        }
                    }  
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Během připojování do databáze se vyskytla chyba\n" + ex.Message);
                }
            }
        }
    }
}
