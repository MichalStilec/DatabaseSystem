using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseSystem
{
    public class Vezen
    {
        /// <summary>
        /// Vloží do databáze nového vězně společně s automaticky vytvořeným 
        /// trestem vězně, který má nastavenou délku na 1 rok
        /// </summary>
        public static void VlozVezne(SqlConnection connection, string jmeno, string prijmeni, DateTime datum, double vyska)
        {
            Random r = new Random();
            connection.Open();

            // Začne transakci pro přidání dvou záznamů do dvou různých tabulek
            SqlTransaction transaction = connection.BeginTransaction("VlozVezneTransaction");

            try
            {
                SqlCommand insertVezen = new SqlCommand($"INSERT INTO Vezen (Jmeno, Prijmeni, Datum_nar, Vyska, CelID) VALUES (@Jmeno, @Prijmeni, @Datum_nar, @Vyska, @CelID); SELECT SCOPE_IDENTITY()", connection, transaction);

                insertVezen.Parameters.AddWithValue("@Jmeno", jmeno);
                insertVezen.Parameters.AddWithValue("@Prijmeni", prijmeni);
                insertVezen.Parameters.AddWithValue("@Datum_nar", datum);
                insertVezen.Parameters.AddWithValue("@Vyska", vyska);
                insertVezen.Parameters.AddWithValue("@CelID", (r.Next(3) + 1));

                // Získání ID nově přidaného vězně
                int vezenID = Convert.ToInt32(insertVezen.ExecuteScalar());

                SqlCommand insertTrest = new SqlCommand($"INSERT INTO Trest (Trest_dan_od, Trest_dan_do, VezID) VALUES (@Trest_dan_od, @Trest_dan_do, @VezenID)", connection, transaction);

                // Přidání trestu s odkazem na nově přidaného vězně
                insertTrest.Parameters.AddWithValue("@Trest_dan_od", DateTime.Now);
                // Trest je na rok
                insertTrest.Parameters.AddWithValue("@Trest_dan_do", DateTime.Now.AddYears(1)); 
                insertTrest.Parameters.AddWithValue("@VezenID", vezenID);

                insertTrest.ExecuteNonQuery();

                // Potvrzení transakce
                transaction.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Během přidávání vězně a trestu se stala chyba: " + ex);
                // V případě chyby zrušíme transakci
                transaction.Rollback();
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Vypíše všechny vězně, který jsou momentálně v databázi
        /// </summary>
        public static string VypisVeznu(SqlConnection connection)
        {
            connection.Open();

            SqlCommand selectVeznu = new SqlCommand("Select * from Vezen", connection);
            StringBuilder result = new StringBuilder();

            try
            {
                SqlDataReader reader = selectVeznu.ExecuteReader();

                while (reader.Read())
                {
                    int vezenID = Convert.ToInt32(reader["VezenID"]);
                    string jmeno = reader["Jmeno"].ToString();
                    string prijmeni = reader["Prijmeni"].ToString();
                    DateTime datum = Convert.ToDateTime(reader["Datum_nar"]);
                    double vyska = Convert.ToDouble(reader["Vyska"]);
                    string formattedDatum = datum.ToString("dd.MM.yyyy");

                    result.AppendLine($"ID: {vezenID}, Jméno: {jmeno}, Příjmení: {prijmeni}, Datum narození: {formattedDatum}, Výška: {vyska}");
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Během výpisu vězňů se stala chyba: " + ex);
            }
            finally
            {
                connection.Close();
            }

            return result.ToString();
        }

        /// <summary>
        /// Smaže vězně společně i se souvisejícím trestem
        /// </summary>
        public static void SmazVezne(SqlConnection connection, int id)
        {
            connection.Open();
            SqlCommand smazTrest = new SqlCommand($"Delete from Trest where VezID = {id};", connection);
            SqlCommand smazVezne = new SqlCommand($"Delete from Vezen where VezenID = {id};", connection);

            try 
            {
                smazTrest.ExecuteNonQuery();
                smazVezne.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Během smazaní vězně se stala chyba: " + ex);
            }
        }

        /// <summary>
        /// Upraví jméno vězně podle výběru uživatele
        /// </summary>
        public static void UpravVezne(SqlConnection connection, string noveJmeno, int id)
        {
            connection.Open();
            SqlCommand updateJmeno = new SqlCommand("UPDATE Vezen SET Jmeno = @NoveJmeno WHERE VezenID = @VezenID", connection);

            try
            {
                updateJmeno.Parameters.AddWithValue("@NoveJmeno", noveJmeno);
                updateJmeno.Parameters.AddWithValue("@VezenID", id);

                updateJmeno.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Během aktualizace jména se stala chyba: " + ex);
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
