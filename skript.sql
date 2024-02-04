CREATE TABLE Bachar (
    BacharID INT PRIMARY KEY IDENTITY(1,1),
    Jmeno VARCHAR(50) NOT NULL,
    Prijmeni VARCHAR(50) NOT NULL,
    Plat INT NOT NULL,
);

CREATE TABLE Veznice (
    VezniceID INT PRIMARY KEY IDENTITY(1,1),
    Nazev VARCHAR(50) NOT NULL,
    Adresa VARCHAR(100) NOT NULL,
    Kapacita_veznu INT NOT NULL,
	Volne_mista BIT NOT NULL
);

CREATE TABLE Cela (
    CelaID INT PRIMARY KEY IDENTITY(1,1),
    Pocet_posteli INT NOT NULL,
	VznceID INT  NOT NULL,
    FOREIGN KEY (VznceID) REFERENCES Veznice(VezniceID)
);

CREATE TABLE Strazi (
    StraziID INT PRIMARY KEY IDENTITY(1,1),
    CelID INT  NOT NULL,
    FOREIGN KEY (CelID) REFERENCES Cela(CelaID),
	BachID INT  NOT NULL,
    FOREIGN KEY (BachID) REFERENCES Bachar(BacharID),
);

CREATE TABLE Vezen (
    VezenID INT PRIMARY KEY IDENTITY(1,1),
    Jmeno VARCHAR(50) NOT NULL, 
    Prijmeni VARCHAR(50) NOT NULL, 
    Datum_nar DATE NOT NULL, 
	Vyska DECIMAL(5,2) NOT NULL,
	CelID INT  NOT NULL,
    FOREIGN KEY (CelID) REFERENCES Cela(CelaID)
);

CREATE TABLE Trest (
    TrestID INT PRIMARY KEY IDENTITY(1,1),
	Trest_dan_od DATETIME NOT NULL, 
	Trest_dan_do DATETIME NOT NULL, 
    VezID INT  NOT NULL,
    FOREIGN KEY (VezID) REFERENCES Vezen(VezenID)
);


INSERT INTO Bachar (Jmeno, Prijmeni, Plat) VALUES
    ('Jan', 'Novak', 35000),
    ('Eva', 'Svobodova', 40000),
    ('Petr', 'Kral', 30000);

INSERT INTO Veznice (Nazev, Adresa, Kapacita_veznu, Volne_mista) VALUES
    ('Veznice Praha', 'Prazska 123, Praha', 500, 1),
    ('Veznice Brno', 'Brnenska 456, Brno', 300, 0),
    ('Veznice Ostrava', 'Ostravska 789, Ostrava', 400, 1);

INSERT INTO Cela (Pocet_posteli, VznceID) VALUES
    (2, 1),
    (3, 2),
    (1, 3);

INSERT INTO Strazi (CelID, BachID) VALUES
    (1, 1),
    (2, 2),
    (3, 3);

INSERT INTO Vezen (Jmeno, Prijmeni, Datum_nar, Vyska, CelID) VALUES
    ('David', 'Kovar', '1988-07-18', 185, 1),
    ('Marketa', 'Novotna', '1995-04-25', 168.3, 2),
    ('Jakub', 'Svoboda', '1991-09-08', 176.7, 3),
    ('Alena', 'Dvorakova', '1986-12-12', 172.5, 1),
    ('Pavel', 'Marek', '1993-03-30', 180.2, 2);

INSERT INTO Trest (Trest_dan_od, Trest_dan_do, VezID) VALUES
    ('2023-02-10', '2023-05-10', 1),
    ('2022-11-15', '2023-01-15', 2),
    ('2023-03-20', '2023-06-20', 3),
	('2022-07-26', '2023-04-26', 4),
    ('2023-1-20', '2023-10-20', 5);


select * from Vezen;