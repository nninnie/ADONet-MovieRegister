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
                Console.WriteLine("--- Filmregister ---");
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
                            ShowAllMovies();
                            break;

                        case "2":
                            ShowAllGenres();
                            break;

                        case "3":
                            AddMovie();
                            break;

                        case "4":
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
        public void ShowAllMovies()
        {
            var movies = movieRepo.GetAllMovies(); // Hämtar alla filmer från repository

            Console.WriteLine("=== Alla filmer ===\n");

            foreach (var movie in movies) // Loopar igenom alla filmer och skriver ut deras information
            {
                Console.WriteLine($"ID: {movie.MovieId}, Titel: {movie.Title}, År: {movie.Year}, Genre: {movie.GenreName}");
            }
        }

        public void ShowAllGenres()
        {
            Console.WriteLine("=== Sök filmer efter genre ===\n");

            Console.WriteLine("Angge genre: ");
            string genreName = Console.ReadLine()!; // Tar emot användarens input för genre

            var genres = genreRepo.GetAllGenres(); // Hämtar alla genrer från repository

            Console.Clear(); // Rensar konsolen innan genrerna skrivs ut

            Console.WriteLine("=== Alla genrer ===\n");

            foreach (var genre in genres) // Loopar igenom alla genrer och skriver ut deras information
            {
                Console.WriteLine($"ID: {genre.GenreId}, Namn: {genre.GenreName}");
            }
        }

        private void AddMovie()
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
    }
}