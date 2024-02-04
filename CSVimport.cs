using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseSystem
{
    public class CSVimport
    {
        public static string Import(SqlConnection connection, string table, string file)
        {
            string filePath = "data/" + file;

            if (!filePath.EndsWith(".csv"))
            {
                return "Soubor musí mít příponu .csv\n";
            }
            if (!File.Exists(filePath))
            {
                return "Soubor neexistuje\n";
            }

            if (table == "1")
            {
                table = "Bachar";
            }
            else if (table == "2")
            {
                table = "Veznice";
            }

            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    string atributes = sr.ReadLine();
                    atributes.Trim();

                    if (table == "Bachar")
                    {
                        if (atributes != "Jmeno,Prijmeni,Plat")
                        {
                            return "CSV soubor musí obsahovat na začátku Jmeno,Prijmeni,Plat\n" +
                                   "Až poté můžete psát na další řádky data";
                        }
                        connection.Open();
                        while (!sr.EndOfStream)
                        {
                            string[] line = sr.ReadLine().Split(',');

                            string jmeno = line[0];
                            string prijmeni = line[1];
                            string plat = line[2];
                            SqlCommand insertBachar = new SqlCommand($"INSERT INTO Bachar(Jmeno,Prijmeni,Plat) VALUES (@Jmeno,@Prijmeni,@Plat)", connection);
                            insertBachar.Parameters.Clear();

                            insertBachar.Parameters.AddWithValue("@Jmeno", jmeno);
                            insertBachar.Parameters.AddWithValue("@Prijmeni", prijmeni);
                            insertBachar.Parameters.AddWithValue("@Plat", plat);
                            insertBachar.ExecuteNonQuery();
                        }
                        connection.Close();
                    }
                    else if (table == "Veznice")
                    {
                        if (atributes != "Nazev,Adresa,Kapacita_veznu,Volne_mista")
                        {
                            return "CSV soubor musí obsahovat na začátku Nazev,Adresa,Kapacita_veznu,Volne_mista\n" +
                                   "Až poté můžete psát na další řádky data";
                        }
                        connection.Open();
                        while (!sr.EndOfStream)
                        {
                            string[] line = sr.ReadLine().Split(',');

                            string nazev = line[0];
                            string adresa = line[1];
                            string kapacita = line[2];
                            string volno = line[3];
                            SqlCommand insertBachar = new SqlCommand($"INSERT INTO Veznice(Nazev,Adresa,Kapacita_veznu,Volne_mista) VALUES (@Nazev,@Adresa,@Kapacita,@Volno)", connection);
                            insertBachar.Parameters.Clear();

                            insertBachar.Parameters.AddWithValue("@Nazev", nazev);
                            insertBachar.Parameters.AddWithValue("@Adresa", adresa);
                            insertBachar.Parameters.AddWithValue("@Kapacita", kapacita);
                            insertBachar.Parameters.AddWithValue("@Volno", volno);
                            insertBachar.ExecuteNonQuery();
                        }
                        connection.Close();
                    }
                }
                return "Data byla úspěšně importována";
            }
            catch (Exception ex)
            {
                return "Chyba při importu dat: " + ex.Message;
            }
        }
    }
}
