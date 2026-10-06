using ADONet_MovieRegister.Data;
using ADONet_MovieRegister.Models;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Text;

namespace ADONet_MovieRegister.Repository
{
    public class MovieRepository
    {
        // Hämtar alla filmer från databasen och returnerar dem som Movie-objekt
        public List<Movie> GetAllMovies()
        {
            // Lista som fylls med filmer
            var movies = new List<Movie>();

            // Skapar en databasanslutning med connection string från DbConfig
            using (var connection = new SqlConnection(DbConfig.ConnectionString))
            {
                connection.Open(); // Öppnar databasanslutningen

                // SQL-fråga: hämta film + genre via JOIN
                var sql = @"SELECT m.MovieId, m.Title, m.Year, g.GenreId, g.GenreName
                    FROM Movies m
                    INNER JOIN Genres g ON m.GenreId = g.GenreId";

                // Skapar SQL-kommandot som ska köras
                using (var command = new SqlCommand(sql, connection))

                // Läser resultatet rad för rad
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read()) // Loopar igenom alla rader
                    {
                        // Skapar ett Movie-objekt från aktuell rad
                        var movie = new Movie
                        {
                            MovieId = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            Year = reader.GetInt32(2),
                            GenreId = reader.GetInt32(3),
                            GenreName = reader.GetString(4)
                        };

                        movies.Add(movie); // Lägger till filmen i listan
                    }
                }
            }
            return movies; // Returnerar alla filmer
        }
    }
}
