
using ADONet_MovieRegister.Models;
using ADONet_MovieRegister.Repository;
using Spectre.Console;
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
                AnsiConsole.Clear();

                AnsiConsole.Write(
                    new Panel("[bold hotpink]FILMREGISTER[/]")
                        .Border(BoxBorder.Rounded)
                        .BorderColor(Color.Orange3)
                );

                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[cyan]Välj ett menyval[/]")
                        .AddChoices(
                            "Visa alla filmer",
                            "Sök filmer efter genre",
                            "Lägg till film",
                            "Uppdatera film",
                            "Ta bort film",
                            "Avsluta"
                        )
                );

                // Switch-sats för att hantera användarens val
                switch (choice)
                {
                    case "Visa alla filmer":
                        ShowAllMovies();
                        break;

                    case "Sök filmer efter genre":
                        ShowAllGenres();
                        break;

                    case "Lägg till film":
                        AddMovie();
                        break;

                    case "Uppdatera film":
                        UpdateMovie();
                        break;

                    case "Ta bort film":
                        DeleteMovie();
                        break;

                    case "Avsluta":
                        running = false;
                        break;
                }
            }
        }


        // ==== VISA ALLA FILMER ====
        public void ShowAllMovies()
        {
            AnsiConsole.Clear(); // Rensar skärmen

            // Hämtar alla filmer från repository
            var movies = movieRepo.GetAllMovies();

            // Rensar konsolen innan filmerna skrivs ut
            AnsiConsole.Write(
                new Panel("[bold hotpink]=== Alla filmer ===[/]")
                    .BorderColor(Color.Orange1)
            );

            // Skapar en ny tabell för att visa filmerna
            var table = new Table()
                .Border(TableBorder.Rounded)
                .BorderColor(Color.Orange3)
                .AddColumn("ID")
                .AddColumn("Titel")
                .AddColumn("År")
                .AddColumn("Genre");

            // Loopar igenom alla filmer och lägger till dem i tabellen
            foreach (var movie in movies)
            {
                table.AddRow(movie.MovieId.ToString(), movie.Title, movie.Year.ToString(), movie.GenreName);
            }

            // Skriver ut tabellen med filmerna till konsolen
            AnsiConsole.Write(table);

            // Skriver ut ett meddelande om att användaren kan trycka ESC för att gå tillbaka
            AnsiConsole.MarkupLine("\n[grey]Tryck ESC för att gå tillbaka...[/]");

            var key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Escape)
                return;

        }


        // ==== VISA FILM EFTER GENRE ====
        public void ShowAllGenres()
        {
            AnsiConsole.Clear(); // Rensar skärmen

            AnsiConsole.Write(
                new Panel("[bold hotpink]=== Sök filmer efter genre ===[/]")
                    .BorderColor(Color.Orange1)
            );

            var genres = genreRepo.GetAllGenres(); // Hämtar alla genrer från repository

            var genreChoice = AnsiConsole.Prompt(
                new SelectionPrompt<Genre>()
                    .Title("[cyan]Välj en genre:[/]")
                    .UseConverter(g => $"{g.GenreId}. {g.GenreName}")
                    .AddChoices(genres) // Lägger till alla genrer som val
            );

            var movies = movieRepo.GetMoviesByGenre(genreChoice.GenreId); // Hämtar alla filmer som tillhör den valda genren

            AnsiConsole.Clear();

            AnsiConsole.Write(
                new Panel($"[bold hotpink]=== Filmer i genren: {genreChoice.GenreName} ===[/]")
                    .BorderColor(Color.Orange1)
            );

            // Om inga filmer hittades i den valda genren, skriv ut ett meddelande
            if (movies.Count == 0)
            {
                AnsiConsole.MarkupLine("\n[grey]Inga filmer hittades i denna genre.[/]");
            }
            // Annars, skapa en tabell och skriv ut alla filmer i den valda genren
            else
            {
                var table = new Table()
                    .Border(TableBorder.Rounded)
                    .BorderColor(Color.Orange3)
                    .AddColumn("ID")
                    .AddColumn("Titel")
                    .AddColumn("År")
                    .AddColumn("Genre");

                // Loopar igenom alla filmer i den valda genren och lägger till dem i tabellen
                foreach (var movie in movies)
                {
                    table.AddRow(movie.MovieId.ToString(), movie.Title, movie.Year.ToString(), movie.GenreName);
                }

                AnsiConsole.Write(table);
            }

            // Skriver ut ett meddelande om att användaren kan trycka ESC för att gå tillbaka
            AnsiConsole.MarkupLine("\n[grey]Tryck ESC för att gå tillbaka...[/]");

            var key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Escape)
                return;

        }


        // ==== LÄGG TILL FILM ====
        public void AddMovie()
        {
            // Rensar konsolen innan användaren lägger till en ny film
            AnsiConsole.Clear();

            // Skriver ut en panel med rubriken "Lägg till film"
            AnsiConsole.Write(
                new Panel("[bold hotpink]=== Lägg till film ===[/]")
                    .BorderColor(Color.Orange1)
                    );

            // Frågar användaren om titel och år för den nya filmen
            string title = AnsiConsole.Ask<string>("[cyan]Ange titel:[/]");
            if (string.IsNullOrWhiteSpace(title)) // Kollar om titeln är tom eller bara innehåller whitespace
            {
                AnsiConsole.MarkupLine("[red]Titel får inte vara tom.[/]");
                return;
            }

            int year = AnsiConsole.Ask<int>("[cyan]Ange år:[/]"); // Frågar användaren om år för den nya filmen

            if (year <= 0 || year > DateTime.Now.Year) // Kollar om året är negativt eller i framtiden
            {
                AnsiConsole.MarkupLine("[red]År får inte vara negativt eller i framtiden.[/]");
                return;
            }

            var genres = genreRepo.GetAllGenres(); // Hämtar alla genrer från repository

            // Visar en lista med genrer och låta användaren välja en
            var genreChoice = AnsiConsole.Prompt(
                new SelectionPrompt<Genre>()
                    .Title("[cyan]Välj en genre:[/]")
                    .UseConverter(g => $"{g.GenreId}. {g.GenreName}")
                    .AddChoices(genres)
            );

            movieRepo.AddMovie(title, year, genreChoice.GenreId); // Lägger till filmen i repository

            AnsiConsole.MarkupLine($"\n[green]Filmen '{title}' har lagts till![/]"); // Skriver ut ett meddelande om att filmen har lagts till

            // Skriver ut ett meddelande om att användaren kan trycka på valfri tangent för att återgå
            AnsiConsole.MarkupLine("\n[grey]Tryck ESC för att gå tillbaka...[/]");

            var key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Escape)
                return;

        }


        // ==== UPPDATERA FILM ====
        public void UpdateMovie()
        {
            AnsiConsole.Clear(); // Rensar skärmen

            // Rensar konsolen innan användaren uppdaterar en film
            AnsiConsole.Write(
                new Panel("[bold hotpink]=== Uppdatera film ===[/]")
                    .BorderColor(Color.Orange1)
            );

            var movies = movieRepo.GetAllMovies(); // Hämtar alla filmer

            // Loopar igenom alla filmer och skriver ut deras information
            var movieChoice = AnsiConsole.Prompt(
                new SelectionPrompt<Movie>()
                    .Title("[cyan]Välj en film att uppdatera:[/]")
                    .UseConverter(m => $"{m.MovieId}. {m.Title} ({m.Year}) - {m.GenreName}")
                    .AddChoices(movies)
            );


            // Frågar användaren om ny titel och år för den valda filmen, med nuvarande värden som standard
            var newTitle = AnsiConsole.Ask<string>("[cyan]Ange ny titel (Enter för att behålla):[/]",
                movieChoice.Title);

            if (string.IsNullOrWhiteSpace(newTitle)) // Kollar om titeln är tom eller bara innehåller whitespace
            {
                AnsiConsole.MarkupLine("[red]Titel får inte vara tom.[/]");
                return;
            }

            // Frågar användaren om nytt år för den valda filmen, med nuvarande värde som standard
            var newYear = AnsiConsole.Ask<int>("[cyan]Ange nytt år (Enter för att behålla):[/]",
                movieChoice.Year
                );

            if (newYear <= 0 || newYear > DateTime.Now.Year) // Kollar om året är negativt eller i framtiden    
            {
                AnsiConsole.MarkupLine("[red]År får inte vara negativt eller i framtiden.[/]");
                return;
            }

            var genres = genreRepo.GetAllGenres(); // Hämtar alla genrer

            // Visar en lista med genrer och låta användaren välja en ny genre för filmen
            var genreChoice = AnsiConsole.Prompt(
                new SelectionPrompt<Genre>()
                    .Title("[cyan]Välj en ny genre:[/]")
                    .UseConverter(g => $"{g.GenreId}. {g.GenreName}")
                    .AddChoices(genres)
            );

            movieRepo.UpdateMovie(movieChoice.MovieId, newTitle, newYear, genreChoice.GenreId); // Uppdaterar filmen i repository

            // Skriver ut ett meddelande om att filmen har uppdaterats
            AnsiConsole.MarkupLine("\n[green]Filmen har uppdaterats![/]");

            // Skriver ut ett meddelande om att användaren kan trycka ESC för att gå tillbaka
            AnsiConsole.MarkupLine("\n[grey]Tryck ESC för att gå tillbaka...[/]");

            var key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Escape)
                return;


        }


        // ==== TA BORT FILM ====
        public void DeleteMovie()
        {
            AnsiConsole.Clear(); // Rensar skärmen

            // Rensar konsolen innan användaren tar bort en film
            AnsiConsole.Write(
                new Panel("[bold hotpink]=== Ta bort film ===[/]")
                    .BorderColor(Color.Orange1)
            );

            var movies = movieRepo.GetAllMovies(); // Hämtar alla filmer

            // Visar en lista med filmer och låter användaren välja en film att ta bort
            var movieChoice = AnsiConsole.Prompt(
                new SelectionPrompt<Movie>()
                    .Title("[cyan]Välj en film att ta bort:[/]")
                    .UseConverter(m => $"{m.MovieId}. {m.Title} ({m.Year}) - {m.GenreName}")
                    .AddChoices(movies)
            );

            movieRepo.DeleteMovie(movieChoice.MovieId); // Tar bort filmen från repository

            // Skriver ut ett meddelande om att filmen har tagits bort
            AnsiConsole.MarkupLine($"\n[green]Filmen '{movieChoice.Title}' har tagits bort![/]");

            // Skriver ut ett meddelande om att användaren kan trycka ESC för att gå tillbaka
            AnsiConsole.MarkupLine("\n[grey]Tryck ESC för att gå tillbaka...[/]");
            var key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Escape)
                return;

        }
    }
}
