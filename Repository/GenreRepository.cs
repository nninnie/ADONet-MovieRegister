using ADONet_MovieRegister.Data;
using ADONet_MovieRegister.Models;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ADONet_MovieRegister.Repository
{
    // Repository som hanterar all databaslogik för genrer.
    // Syftet är att separera dataåtkomst från UI och ge en ren struktur.
    public class GenreRepository
    {
        // Hämtar alla genrer från databasen. Används för att visa valbara genrer i UI.
        public List<Genre> GetAllGenres()
        {
            var genres = new List<Genre>();

            try
            {
                // Skapar en anslutning till databasen med hjälp av anslutningssträngen från DbConfig.
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                {
                    connection.Open();

                    // SQL-fråga för att hämta alla genrer från Genre-tabellen.
                    var sql = @"SELECT g.GenreId, g.GenreName 
                                FROM Genre g";

                    // Skapar ett SqlCommand-objekt för att exekvera SQL-frågan.
                    // Använder ExecuteReader för att läsa data från databasen.
                    using (var command = new SqlCommand(sql, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            genres.Add(new Genre
                            {
                                GenreId = reader.GetInt32(0),
                                GenreName = reader.GetString(1)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ett fel uppstod när genrer hämtades från databasen.", ex);
            }

            return genres;
        }
    }
}
