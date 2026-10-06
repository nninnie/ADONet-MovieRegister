
using ADONet_MovieRegister.Models;
using ADONet_MovieRegister.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace ADONet_MovieRegister.UI
{
    public class MenuUI
    {
        // Instanser av repository-klasserna för att hantera filmer och genrer
        private readonly MovieRepository movieRepo = new MovieRepository();
        private readonly GenreRepository genreRepo = new GenreRepository();

        // Metod för att visa huvudmenyn och hantera användarens val
        public void ShowMenu()
        {
            // Variabel för att hålla reda på om programmet ska fortsätta köra
            bool running = true;

            // Huvudloop för menyn
            while (running)
            {
                Console.Clear();
                Console.WriteLine("=== Filmregister ===");
                Console.WriteLine("1. Visa alla filmer");
                Console.WriteLine("2. Sök filmer efter genre");
                Console.WriteLine("3. Lägg till film");
                Console.WriteLine("4. Ta bort film");
                Console.WriteLine("0. Avsluta");
                Console.WriteLine("Välj ett alternativ: ");

                string choice = Console.ReadLine()!; // Användarens val
                Console.Clear(); // Rensar konsolen för att visa nästa meny eller resultat

                // Try-catch block för att hantera eventuella fel som kan uppstå vid användarens val
                try
                {
                    // Switch-sats för att hantera användarens val
                    switch (choice)
                    {
                        case "1":
                            ShowAllMovies(); // Anropar metoden för att visa alla filmer
                            break;

                        case "2":
                            ShowAllGenres(); // Anropar metoden för att visa alla genrer
                            break;

                        case "3":
                            AddMovie(); // Anropar metoden för att lägga till en ny film
                            break;

                        case "4":
                            UpdateMovie(); // Anropar metoden för att uppdatera en film
                            break;

                        case "5":
                            DeleteMovie(); // Anropar metoden för att ta bort en film
                            break;

                        case "0":
                            running = false;
                            break;
                    }
                }
                // Catch block för att fånga och visa eventuella undantag som uppstår
                catch (Exception ex)
                {
                    Console.WriteLine($"Ett fel uppstod: {ex.Message}");
                }
            }
        }


        // Metod för att visa alla filmer i databasen
        public void ShowAllMovies()
        {
            var movies = movieRepo.GetAllMovies(); // Hämtar alla filmer från repository

            Console.WriteLine("=== Alla filmer ===\n");

            foreach (var movie in movies) // Loopar igenom alla filmer och skriver ut deras information
            {
                Console.WriteLine($"ID: {movie.MovieId}, Titel: {movie.Title}, År: {movie.Year}, Genre: {movie.GenreName}");
            }
        }

        // Metod för att visa alla genrer i databasen
        public void ShowAllGenres()
        {
            Console.WriteLine("Sök film efter genre: ");
            string genreName = Console.ReadLine()!; // Tar emot användarens input för genre

            var genres = genreRepo.GetAllGenres(); // Hämtar alla genrer från repository

            Console.Clear(); // Rensar konsolen innan genrerna skrivs ut

            Console.WriteLine("=== Alla genrer ===\n");

            foreach (var genre in genres) // Loopar igenom alla genrer och skriver ut deras information
            {
                Console.WriteLine($"ID: {genre.GenreId}, Namn: {genre.GenreName}");
            }
        }

        // Metod för att lägga till en ny film i databasen
        public void AddMovie()
        {
            Console.WriteLine("=== Lägg till film ===\n");

            // Titel för filmen, kontrollerar om den är tom eller bara whitespace
            Console.WriteLine("Ange titel: ");
            string title = Console.ReadLine()!;

            if (string.IsNullOrWhiteSpace(title))
            {
                Console.WriteLine("Titeln får inte vara tom. Försök igen.");
                return; // Avslutar metoden om titeln är tom
            }

            // Årtal för filmen, kontrollerar om det är ett giltigt heltal
            Console.WriteLine("Ange årtal: ");
            if (!int.TryParse(Console.ReadLine(), out int year))
            {
                Console.WriteLine("Ogiltigt årtal. Försök igen.");
                return;
            }

            // Genrelista för att visa användaren vilka genrer som finns tillgängliga
            var genres = genreRepo.GetAllGenres(); // Hämtar alla genrer från repository
            Console.WriteLine("\nAnge genre: ");

            foreach (var genre in genres) // Loopar igenom alla genrer och skriver ut deras information
            {
                Console.WriteLine($"ID: {genre.GenreId}, Namn: {genre.GenreName}");
            }

            Console.WriteLine("Ange genre-ID: ");
            if (!int.TryParse(Console.ReadLine(), out int genreId) || !genres.Exists(g => g.GenreId == genreId)) // Kontrollera om genre-ID är giltigt
            {
                Console.WriteLine("Ogiltigt genre-ID. Försök igen.");
                return; // Avslutar metoden om genre-ID är ogiltigt
            }

            movieRepo.AddMovie(title, year, genreId); // Lägger till filmen i repository

            Console.WriteLine($"\nFilmen '{title}' ({year}) har lagts till!");


        }


        // Metod för att uppdatera en film i databasen
        public void UpdateMovie()
        {
            Console.WriteLine("=== Uppdatera film ===\n");

            var movies = movieRepo.GetAllMovies(); // Hämtar alla filmer

            Console.Clear();
            Console.WriteLine("=== Tillgängliga filmer ===");
            foreach (var movie in movies)
            {
                Console.WriteLine($"ID: {movie.MovieId}, Titel: {movie.Title}, År: {movie.Year}, Genre: {movie.GenreName}");
            }

            Console.WriteLine("\nAnge ID för filmen du vill uppdatera:");
            if (!int.TryParse(Console.ReadLine(), out int movieId))
            {
                Console.WriteLine("Ogiltigt ID.");
                return;
            }

            var movieToUpdate = movies.FirstOrDefault(m => m.MovieId == movieId);

            if (movieToUpdate == null)
            {
                Console.WriteLine("Filmen hittades inte.");
                return;
            }

            Console.WriteLine($"Ny titel ({movieToUpdate.Title}):");
            string newTitle = Console.ReadLine()!;
            if (string.IsNullOrWhiteSpace(newTitle))
                newTitle = movieToUpdate.Title;

            Console.WriteLine($"Nytt årtal ({movieToUpdate.Year}):");
            string yearInput = Console.ReadLine()!;
            int newYear = movieToUpdate.Year;
            if (!string.IsNullOrWhiteSpace(yearInput) && int.TryParse(yearInput, out int parsedYear))
                newYear = parsedYear;

            var genres = genreRepo.GetAllGenres();
            Console.WriteLine("\nTillgängliga genrer:");
            foreach (var g in genres)
            {
                Console.WriteLine($"{g.GenreId}. {g.GenreName}");
            }

            Console.WriteLine($"Nytt genre-ID ({movieToUpdate.GenreId}):");
            string genreInput = Console.ReadLine()!;
            int newGenreId = movieToUpdate.GenreId;
            if (!string.IsNullOrWhiteSpace(genreInput) && int.TryParse(genreInput, out int parsedGenreId))
                newGenreId = parsedGenreId;

            movieRepo.UpdateMovie(movieId, newTitle, newYear, newGenreId);

            Console.WriteLine("\nFilmen har uppdaterats!");
        }


        // Metod för att ta bort en film från databasen
        public void DeleteMovie()
        {
            Console.WriteLine("=== Ta bort film ===\n");
            var movies = movieRepo.GetAllMovies(); // Hämtar alla filmer
            Console.Clear();
            Console.WriteLine("=== Tillgängliga filmer ===");
            foreach (var movie in movies)
            {
                Console.WriteLine($"ID: {movie.MovieId}, Titel: {movie.Title}, År: {movie.Year}, Genre: {movie.GenreName}");
            }
            Console.WriteLine("\nAnge ID för filmen du vill ta bort:");
            if (!int.TryParse(Console.ReadLine(), out int movieId))
            {
                Console.WriteLine("Ogiltigt ID.");
                return;
            }
            var movieToDelete = movies.FirstOrDefault(m => m.MovieId == movieId);
            if (movieToDelete == null)
            {
                Console.WriteLine("Filmen hittades inte.");
                return;
            }
            movieRepo.DeleteMovie(movieId);
            Console.WriteLine($"\nFilmen '{movieToDelete.Title}' har tagits bort!");
        }



    }
}
