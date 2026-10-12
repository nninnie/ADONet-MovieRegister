using ADONet_MovieRegister.Data;
using ADONet_MovieRegister.Models;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Text;

namespace ADONet_MovieRegister.Repository
{
    // Hanterar all databaslogik för filmer och separerar dataåtkomst från UI
    public class MovieRepository
    {
        // Hämtar alla filmer med tillhörande genre för att visas i UI
        public List<Movie> GetAllMovies()
        {
            var movies = new List<Movie>();

            try
            {
                // Använder SqlConnection för att ansluta till databasen med connection string från DbConfig
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                {
                    connection.Open();

                    // SQL-fråga som hämtar alla filmer och deras genrer genom en INNER JOIN mellan Movie och Genre-tabellerna
                    var sql = @"SELECT m.MovieId, m.Title, m.Year, g.GenreId, g.GenreName
                                FROM Movie m
                                INNER JOIN Genre g ON m.GenreId = g.GenreId";

                    using (var command = new SqlCommand(sql, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            movies.Add(new Movie
                            {
                                MovieId = reader.GetInt32(0),
                                Title = reader.GetString(1),
                                Year = reader.GetInt32(2),
                                GenreId = reader.GetInt32(3),
                                GenreName = reader.GetString(4)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ett fel uppstod när filmer hämtades från databasen.", ex);
            }

            return movies;
        }

        // Hämtar filmer som matchar en viss genre för filtrering i UI
        public List<Movie> GetMoviesByGenre(int genreId)
        {
            var movies = new List<Movie>();

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                {
                    connection.Open();

                    var sql = @"SELECT m.MovieId, m.Title, m.Year, g.GenreId, g.GenreName
                                FROM Movie m
                                INNER JOIN Genre g ON m.GenreId = g.GenreId
                                WHERE g.GenreId = @GenreId";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@GenreId", genreId);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                movies.Add(new Movie
                                {
                                    MovieId = reader.GetInt32(0),
                                    Title = reader.GetString(1),
                                    Year = reader.GetInt32(2),
                                    GenreId = reader.GetInt32(3),
                                    GenreName = reader.GetString(4)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ett fel uppstod när filmer hämtades baserat på genre.", ex);
            }

            return movies;
        }

        // Lägger till en ny film i databasen baserat på användarens input
        public void AddMovie(string title, int year, int genreId)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                {
                    connection.Open();

                    var sql = @"INSERT INTO Movie (Title, Year, GenreId) VALUES (@Title, @Year, @GenreId)";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Title", title);
                        command.Parameters.AddWithValue("@Year", year);
                        command.Parameters.AddWithValue("@GenreId", genreId);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ett fel uppstod när en film skulle läggas till.", ex);
            }
        }

        // Uppdaterar en befintlig film med nya värden från UI
        public void UpdateMovie(int movieId, string title, int year, int genreId)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                {
                    connection.Open();

                    var sql = @"UPDATE Movie SET Title = @Title, Year = @Year, GenreId = @GenreId WHERE MovieId = @MovieId";

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
            catch (Exception ex)
            {
                throw new Exception("Ett fel uppstod när en film skulle uppdateras.", ex);
            }
        }

        // Tar bort en film baserat på ID när användaren bekräftar borttagning
        public void DeleteMovie(int movieId)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                {
                    connection.Open();

                    var sql = @"DELETE FROM Movie WHERE MovieId = @MovieId";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@MovieId", movieId);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ett fel uppstod när en film skulle tas bort.", ex);
            }
        }
    }
}