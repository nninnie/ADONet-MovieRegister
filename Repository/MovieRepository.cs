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



        // Hämtar alla filmer som tillhör en viss genre
        public List<Movie> GetMoviesByGenre(int genreId)
        {
            var movies = new List<Movie>(); // Lista som fylls med filmer
            using (var connection = new SqlConnection(DbConfig.ConnectionString)) // Skapar en databasanslutning
            {
                connection.Open(); // Öppnar databasanslutningen

                // SQL-fråga: hämta film + genre via JOIN där GenreId matchar
                var sql = @"SELECT m.MovieId, m.Title, m.Year, g.GenreId, g.GenreName
                            FROM Movies m
                            INNER JOIN Genres g ON m.GenreId = g.GenreId
                            WHERE g.GenreId = @GenreId";

                // Skapar SQL-kommandot som ska köras
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@GenreId", genreId); // Lägger till parameter för GenreId
                    using (var reader = command.ExecuteReader()) // Läser resultatet rad för rad
                    {
                        // Loopar igenom alla rader
                        while (reader.Read())
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
            }
            return movies; // Returnerar alla filmer som matchar genren
        }



        // Lägger till en ny film i databasen
        public void AddMovie(string title, int year, int genreId)
        {
            // Skapar en databasanslutning med connection string från DbConfig
            using (var connection = new SqlConnection(DbConfig.ConnectionString))
            {
                connection.Open(); // Öppnar databasanslutningen

                // SQL - fråga: lägger till en ny film i Movies - tabellen
                var sql = @"INSERT INTO Movies (Title, Year, GenreId) VALUES (@Title, @Year, @GenreId)";

                // Skapar SQL-kommandot som ska köras
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Title", title);
                    command.Parameters.AddWithValue("@Year", year);
                    command.Parameters.AddWithValue("@GenreId", genreId);
                    command.ExecuteNonQuery();
                }
            }
        }



        // Uppdaterar en befintlig film i databasen
        public void UpdateMovie(int movieId, string title, int year, int genreId)
        {
            // Skapar en databasanslutning med connection string från DbConfig
            using (var connection = new SqlConnection(DbConfig.ConnectionString))
            {
                connection.Open(); // Öppnar databasanslutningen

                // SQL - fråga: uppdaterar en film i Movies - tabellen
                var sql = @"UPDATE Movies SET Title = @Title, Year = @Year, GenreId = @GenreId WHERE MovieId = @MovieId";

                // Skapar SQL-kommandot som ska köras
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Title", title);
                    command.Parameters.AddWithValue("@Year", year);
                    command.Parameters.AddWithValue("@GenreId", genreId);
                    command.Parameters.AddWithValue("@MovieId", movieId);
                    command.ExecuteNonQuery();
                }
            }
        }



        // Tar bort en film från databasen baserat på MovieId
        public void DeleteMovie(int movieId)
        {
            // Skapar en databasanslutning med connection string från DbConfig
            using (var connection = new SqlConnection(DbConfig.ConnectionString))
            {
                connection.Open(); // Öppnar databasanslutningen

                // SQL - fråga: tar bort en film från Movies - tabellen baserat på MovieId
                var sql = @"DELETE FROM Movies WHERE MovieId = @MovieId";

                // Skapar SQL-kommandot som ska köras
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@MovieId", movieId);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
 