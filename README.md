# 🎬 Filmregister – ADO.NET Console Application

En strukturerad C#‑applikation för att hantera ett filmregister med koppling till SQL Server.  
Projektet demonstrerar klassisk dataåtkomst med **ADO.NET**, tydlig lagerindelning och ett modernt konsol‑UI byggt med Spectre.Console.

<img src="movieRegister.png" width="700">

---

## 📌 Översikt

Applikationen är en konsollösning där användaren kan:

- Visa alla filmer med tillhörande genre  
- Söka filmer baserat på genre  
- Lägga till nya filmer  
- Uppdatera befintliga filmer  
- Ta bort filmer  

Data lagras i en SQL Server‑databas med två relaterade tabeller: **Movie** och **Genre**.

---

## 🛠️ Tekniker & Arkitektur

### **Språk & Ramverk**
- C# .NET  
- ADO.NET (SqlConnection, SqlCommand, SqlDataReader, SqlParameter)

### **Databas**
- SQL Server  
- Relation mellan tabellerna:  
  - `Genre` (1) → `Movie` (N)

### **Arkitektur**
- **Repository‑mönster** för all dataåtkomst  
- **Separering av UI och logik**  
- **Modellklasser (POCOs)** för datatransport  
- **Konsolgränssnitt med Spectre.Console** för tabeller, paneler och tydlig presentation  
- **Parametriserade SQL‑frågor** för säker datahantering  
- **Validering och felhantering** i UI‑lagret  
- **DRY & SOLID‑principer** på grundnivå

---

## 📁 Projektstruktur

```plaintext
/Models
    Movie.cs        → Representerar en film
    Genre.cs        → Representerar en genre

/Repository
    MovieRepository.cs  → SQL-frågor och dataåtkomst för filmer
    GenreRepository.cs  → SQL-frågor och dataåtkomst för genrer
/Data
    DbConfig        → Connection string för databasen

/UI
    MenuUI.cs       → Menyer, input, validering, presentation

Program.cs          → Applikationens startpunkt

```
---

Strukturen följer branschstandard för små .NET‑applikationer med tydlig separation mellan lager.

---

## 🧱 Arkitektur & Kodprinciper

### **Repository‑mönster**
All databaslogik ligger i repository‑klasserna.  
UI‑lagret anropar repository och får tillbaka modeller.

### **Separering av UI och logik**
MenuUI hanterar:
- menyer  
- input  
- validering  
- felmeddelanden  
- presentation av data  

UI‑lagret innehåller ingen SQL‑kod.

### **Modellklasser**
Movie och Genre är enkla POCO‑modeller som representerar databasens tabeller.

### **ADO.NET‑flöde**
Projektet använder klassiskt ADO.NET:
- `SqlConnection`  
- `SqlCommand`  
- `SqlDataReader`  
- `SqlParameter`  

Allt implementerat enligt etablerad standard.

---

## ✔️ Validering

Applikationen innehåller grundläggande validering:

- Titel får inte vara tom  
- Årtal måste vara giltigt  
- Genre måste väljas  
- Felaktig input hanteras med tydliga meddelanden  
- Spectre.Console används för att presentera fel och val på ett tydligt sätt  

---

## ⚠️ Felhantering

- Try/catch runt databasoperationer  
- Felmeddelanden visas i UI  
- Programmet fortsätter köra vid fel  
- Databasanslutningar stängs korrekt även vid undantag  

---

## 🔁 Kodprinciper (DRY & SOLID)

### **DRY – Don’t Repeat Yourself**
- SQL‑logik ligger i repository  
- UI‑logik ligger i MenuUI  
- Modeller återanvänds  
- Inga duplicerade SQL‑kommandon  

### **SOLID (grundnivå)**

**S – Single Responsibility**  
Varje klass har ett tydligt ansvar.

**O – Open/Closed**  
Nya funktioner kan läggas till utan att ändra befintlig kod i UI.

**L – Liskov Substitution**  
Modellerna är enkla och följer LSP naturligt.

**I – Interface Segregation**  
Inga onödiga interfaces — korrekt för projektets storlek.

**D – Dependency Inversion**  
UI beror på repository‑klasser, inte på SQL‑kod direkt.

---

## 🔌 Connection String

Justera connection string i repository‑klasserna så den matchar din lokala SQL Server‑instans:

```csharp
Server=localhost;Database=MovieRegisterDB;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;
```

---

## ▶️ Kör applikationen

1. Skapa databasen MovieRegisterDB i SQL Server
2. Skapa tabellerna Movie och Genre
3. Lägg in några testgenrer och filmer
4. Klona projektet
5. Kör applikationen i Visual Studio

---

## 👤 Utvecklare
**Ninnie Dahlström  
Systemutvecklare .NET
Stockholm, Sverige
