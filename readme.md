# Database System
## Hlavní informace
- Informace o projektu: Projekt je vytvářen pro školu. Aplikace je dělaná pro práci s databází, umožňující různé operace jako čtení, přidání, mazání a úpravu záznamů v tabulce Vězeň. Také obsahuje možnosti generování reportu a importu dat ze souboru CSV do tabulek Bachar a Veznice.
- Autor: Michal Štilec
- Třída: C4b
- Kontakt: stilec2@spsejecna.cz
- Datum vypracování: 22.1.2024 - 4.2.2024
- Název školy: Střední průmyslová elektrotechnická škola (SPŠE) Ječná 
- Hardwareové požadavky: Windows 10/11, internetové připojení
- Architektonický návrhový styl: Single Responsibility Principle (SRP)
- Jazyk: C#

# Popis instalace
1. Stáhněte soubor zip a v nově vytvořené složce si zip soubor extrahujte
2. Otevřete pdf soubor **TestCase_1**
3. Pokud chcete změnit konfigurační soubor nebo nahrát svá data pro import, pokračujte v **TestCase_1**
4. Pokud si nepřejete nic měnit, přeskočte na krok **5. Spuštění exe souboru**
5. Program by měl být spustěný, pokračujte podle návodu v **TestCase_2**, **TestCase_3** a **TestCase_4**

# Použité knihovny
- System.Collections.Generic;
- System.ComponentModel.Design;
- System.Data.SqlClient;
- System.Linq;
- System.Text;
- System.IO;
- System.Threading.Tasks;

# Struktura programu

# Metoda Main

## Nastavení kódování výstupu na UTF-8.
```
Console.OutputEncoding = Encoding.UTF8;
```
## Načtení konfigurace ze třídy Config.
```
string source = config.LoadDataSource(); string catalog = config.LoadCatalog();
string user = config.LoadUser(); string passwrd = config.LoadPassword();

string connectionString = $"Data Source={source};Initial Catalog={catalog};User ID={user};Password={passwrd};";
```
# Třída Config
- Třída pro načítání konfigurace z konfiguračního souboru.

### ConfigStructure(): 
- Načítá vybraný řádek ze souboru config.cfg
![image](https://github.com/MichalStilec/DatabaseSystem/assets/113086016/75902cb2-a79b-4dca-a6a8-f35f183d7d99)

### LoadDataSource(): 
- Načte zdroj dat
### LoadCatalog():
- Načte název katalogu
### LoadUser(): 
- Načte uživatelské jméno
### LoadPassword(): 
- Načte heslo
# Třída Vezen
Popis: Třída pro manipulaci s tabulkou Vězeň v databázi.
Metody:
### VypisVeznu(SqlConnection connection): 
- Vypíše všechny vězně, který jsou momentálně v databázi
![image](https://github.com/MichalStilec/DatabaseSystem/assets/113086016/f2078cce-f837-49e8-9f1b-4b592448e484)

### VlozVezne(SqlConnection connection, string jmno, string prijmeni, DateTime datumNarozeni, double vyska): 
- Vloží do databáze nového vězně společně s automaticky vytvořeným
![image](https://github.com/MichalStilec/DatabaseSystem/assets/113086016/7f6702b1-9a94-4662-adaa-189d1d1be953)
![image](https://github.com/MichalStilec/DatabaseSystem/assets/113086016/2286d915-3724-4d75-b826-1ef102a597d5)

### SmazVezne(SqlConnection connection, int id): 
- Smaže vězně společně i se souvisejícím trestem
![image](https://github.com/MichalStilec/DatabaseSystem/assets/113086016/715da384-ceeb-4619-a8e4-21425f7603c3)

### UpravVezne(SqlConnection connection, string noveJmeno, int id): 
- Upraví jméno vězně podle výběru uživatele
![image](https://github.com/MichalStilec/DatabaseSystem/assets/113086016/03b81a6f-5433-4459-b8bc-4c861f29f4ee)

# Třída Report
- Třída pro generování reportu.
### VygenerujReport(SqlConnection connection): 
- Vypíše informace z tabulek Vezen, Bachar a Veznice
![image](https://github.com/MichalStilec/DatabaseSystem/assets/113086016/f328e383-61dc-4ee1-b622-248033384e68)
![image](https://github.com/MichalStilec/DatabaseSystem/assets/113086016/29895b7c-dd9d-4fa7-8f5a-8a2fa32d112e)

# Třída CSVimport
- Třída pro import dat ze souboru CSV do databáze.

### Schéma pro import bachaře
```
Jmeno,Prijmeni,Plat
Karel,Novak,37000
Tomas,Novak,32000
```
### Schéma pro import věznice
```
Nazev,Adresa,Kapacita_veznu,Volne_mista
Veznice Liberec,Liberecka 888,350,True
```

### Import(SqlConnection connection, string table, string file): 
- Importuje data z CSV souboru do vybrané tabulky
```
public static string Import(SqlConnection connection, string table, string file)
{
    string filePath = "data/" + file;

    // Kontroluje, zda soubor končí .csv
    if (!filePath.EndsWith(".csv"))
    {
        return "Soubor musí mít příponu .csv\n";
    }
    // Kontroluje, zda soubor existuje
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
            // Zde se načtou atributy pro výpis
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
```

## Závěr
- Tento projekt umožňuje efektivní práci s databází a poskytuje uživateli různé možnosti manipulace s daty. Je navržen tak, aby byl přehledný a snadno rozšiřitelný o další funkcionalitu.
