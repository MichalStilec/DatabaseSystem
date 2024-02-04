using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseSystem
{
    public class Report
    {
        /// <summary>
        /// Vypíše informace z tabulek Vezen, Bachar a Veznice
        /// </summary>
        public static void VygenerujReport(SqlConnection connection)
        {
            string select = "Vezen";
            connection.Open();
            while (true)
            {
                SqlCommand command = new SqlCommand($"SELECT * FROM {select}", connection);
                SqlDataReader reader = command.ExecuteReader();

                Console.WriteLine(new string('-', 76));
                Console.WriteLine("{0,35}", $"{select}");

                Console.WriteLine(new string('-', 76));

                // Každý select má jiný druh výpisu kvůli různosti atributů
                if (select == "Vezen")
                {
                    Console.WriteLine("{0,-15} | {1,-20} | {2,-15} | {3,-10}", "Jmeno", "Prijmeni", "Datum narozeni", "Vyska");
                }
                else if (select == "Bachar")
                {
                    Console.WriteLine("{0,-15} | {1,-20} | {2,-15}", "Jmeno", "Prijmeni", "Plat");
                }
                else if (select == "Veznice")
                {
                    Console.WriteLine("{0,-15} | {1,-25} | {2,-15} | {3,-10}", "Nazev", "Adresa", "Kapacita veznu", "Volne mista");
                }

                while (reader.Read())
                {
                    if (select == "Vezen")
                    {
                        Console.WriteLine("{0,-15} | {1,-20} | {2,-15} | {3,-10}",
                                      reader["Jmeno"], reader["Prijmeni"],
                                      ((DateTime)reader["Datum_nar"]).ToString("dd.MM.yyyy"),
                                      ((decimal)reader["Vyska"]).ToString("F1"));
                    }
                    else if (select == "Bachar")
                    {
                        Console.WriteLine("{0,-15} | {1,-20} | {2,-15}",
                                      reader["Jmeno"], reader["Prijmeni"], reader["Plat"]);
                    }
                    else if (select == "Veznice")
                    {
                        Console.WriteLine("{0,-15} | {1,-25} | {2,-15} | {3,-10}",
                                      reader["Nazev"], reader["Adresa"], reader["Kapacita_veznu"], reader["Volne_mista"]);
                    }
                }

                Console.WriteLine(new string('-', 76) + "\n");
                reader.Close();

                // Nebylo nutné vytvářet novou metodu pro každou tabulku,
                // takže se mění po každým vypsaným selectu
                if (select == "Vezen")
                {
                    select = "Bachar";
                }
                else if (select == "Bachar")
                {
                    select = "Veznice";
                }
                else break;
            }
            connection.Close();
        }
    }
}
