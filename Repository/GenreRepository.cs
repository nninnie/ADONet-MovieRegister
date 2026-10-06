using ADONet_MovieRegister.Data;
using ADONet_MovieRegister.Models;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;    
using System.Linq;
using System.Text;

namespace ADONet_MovieRegister.Repository
{
    public class GenreRepository
    {
        // Hämtar alla genrer från databasen och returnerar dem som Genre-objekt
        public List <Genre> GetAllGenres()
        {
            // Skapar en lista för att lagra genrerna
            var genres = new List<Genre>();

            // Använder SqlConnection för att ansluta till databasen
            using (var connection = new SqlConnection(DbConfig.ConnectionString))
            {
                connection.Open(); // Öppnar anslutningen till databasen

                // SQL-fråga för att hämta alla genrer
                var sql = @"SELECT g.GenreId, g.GenreName 
                            FROM Genres g";

                // Använder SqlCommand för att exekvera SQL-frågan
                using (var command = new SqlCommand(sql, connection))
                {
                    // Använder SqlDataReader för att läsa resultaten från SQL-frågan
                    using (var reader = command.ExecuteReader())
                    {
                        // Loopar igenom resultaten och skapar Genre-objekt för varje rad
                        while (reader.Read())
                        {

                            // Skapar ett nytt Genre-objekt och fyller det med data från databasen
                            var genre = new Genre
                            {
                                GenreId = reader.GetInt32(0),
                                GenreName = reader.GetString(1)
                            };

                            // Lägger till Genre-objektet i listan
                            genres.Add(genre);
                        }
                    }
                }
            }

            return genres; // Returnerar listan med alla genrer
        }
    }
}
