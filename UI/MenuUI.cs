using ADONet_MovieRegister.Models;
using ADONet_MovieRegister.Repository;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Text;

namespace ADONet_MovieRegister.UI
{
    // UI-klass som hanterar hela menyflödet och kopplar användaren till repository-lagret
    public class MenuUI
    {
        private readonly MovieRepository movieRepo = new MovieRepository();
        private readonly GenreRepository genreRepo = new GenreRepository();

        // Visar huvudmenyn och styr programflödet baserat på användarens val
        public void ShowMenu()
        {
            bool running = true;

            while (running)
            {
                AnsiConsole.Clear();

                AnsiConsole.Write(
                    new Panel("[bold hotpink]=== FILMREGISTER ===[/]")
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

        // Hämtar alla filmer och visar dem i en tabell för tydlig översikt
        public void ShowAllMovies()
        {
            AnsiConsole.Clear();

            var movies = movieRepo.GetAllMovies();

            AnsiConsole.Write(
                new Panel("[bold hotpink]=== Alla filmer ===[/]")
                    .BorderColor(Color.Orange1)
            );

            var table = new Table()
                .Border(TableBorder.Rounded)
                .BorderColor(Color.Orange3)
                .AddColumn("ID")
                .AddColumn("Titel")
                .AddColumn("År")
                .AddColumn("Genre");

            foreach (var movie in movies)
            {
                table.AddRow(movie.MovieId.ToString(), movie.Title, movie.Year.ToString(), movie.GenreName);
            }

            AnsiConsole.Write(table);
            WaitForKey();
        }

        // Låter användaren välja en genre och visar alla filmer som matchar
        public void ShowAllGenres()
        {
            AnsiConsole.Clear();

            AnsiConsole.Write(
                new Panel("[bold hotpink]=== Sök filmer efter genre ===[/]")
                    .BorderColor(Color.Orange1)
            );

            var genres = genreRepo.GetAllGenres();

            var genreChoice = AnsiConsole.Prompt(
                new SelectionPrompt<Genre>()
                    .Title("[cyan]Välj en genre:[/]")
                    .UseConverter(g => $"{g.GenreId}. {g.GenreName}")
                    .AddChoices(genres)
            );

            var movies = movieRepo.GetMoviesByGenre(genreChoice.GenreId);

            AnsiConsole.Clear();

            AnsiConsole.Write(
                new Panel($"[bold hotpink]=== Filmer i genren: {genreChoice.GenreName} ===[/]")
                    .BorderColor(Color.Orange1)
            );

            if (movies.Count == 0)
            {
                AnsiConsole.MarkupLine("\n[grey]Inga filmer hittades i denna genre.[/]");
            }
            else
            {
                var table = new Table()
                    .Border(TableBorder.Rounded)
                    .BorderColor(Color.Orange3)
                    .AddColumn("ID")
                    .AddColumn("Titel")
                    .AddColumn("År")
                    .AddColumn("Genre");

                foreach (var movie in movies)
                {
                    table.AddRow(movie.MovieId.ToString(), movie.Title, movie.Year.ToString(), movie.GenreName);
                }

                AnsiConsole.Write(table);
            }

            WaitForKey();
        }

        // Lägger till en ny film och validerar titel och år för att undvika felaktiga värden
        public void AddMovie()
        {
            AnsiConsole.Clear();

            AnsiConsole.Write(
                new Panel("[bold hotpink]=== Lägg till film ===[/]")
                    .BorderColor(Color.Orange1)
            );

            string title = AnsiConsole.Ask<string>("[cyan]Ange titel:[/]");
            if (string.IsNullOrWhiteSpace(title))
            {
                AnsiConsole.MarkupLine("[red]Titel får inte vara tom.[/]");
                WaitForKey();
                return;
            }

            int year = AnsiConsole.Ask<int>("[cyan]Ange år:[/]");
            if (year <= 0 || year > DateTime.Now.Year)
            {
                AnsiConsole.MarkupLine("[red]År får inte vara negativt eller i framtiden.[/]");
                WaitForKey();
                return;
            }

            var genres = genreRepo.GetAllGenres();

            var genreChoice = AnsiConsole.Prompt(
                new SelectionPrompt<Genre>()
                    .Title("[cyan]Välj en genre:[/]")
                    .UseConverter(g => $"{g.GenreId}. {g.GenreName}")
                    .AddChoices(genres)
            );

            movieRepo.AddMovie(title, year, genreChoice.GenreId);

            AnsiConsole.MarkupLine($"\n[green]Filmen '{title}' har lagts till![/]");
            WaitForKey();
        }

        // Uppdaterar en befintlig film och låter användaren behålla nuvarande värden om de vill
        public void UpdateMovie()
        {
            AnsiConsole.Clear();

            AnsiConsole.Write(
                new Panel("[bold hotpink]=== Uppdatera film ===[/]")
                    .BorderColor(Color.Orange1)
            );

            var movies = movieRepo.GetAllMovies();
            if (movies.Count == 0)
            {
                AnsiConsole.MarkupLine("\n[grey]Inga filmer hittades att uppdatera.[/]");
                WaitForKey();
                return;
            }

            var movieChoice = AnsiConsole.Prompt(
                new SelectionPrompt<Movie>()
                    .Title("[cyan]Välj en film att uppdatera:[/]")
                    .UseConverter(m => $"{m.MovieId}. {m.Title} ({m.Year}) - {m.GenreName}")
                    .AddChoices(movies)
            );

            var newTitle = AnsiConsole.Ask<string>("[cyan]Ange ny titel (Enter för att behålla):[/]", movieChoice.Title);
            if (string.IsNullOrWhiteSpace(newTitle))
            {
                AnsiConsole.MarkupLine("[red]Titel får inte vara tom.[/]");
                WaitForKey();
                return;
            }

            var newYear = AnsiConsole.Ask<int>("[cyan]Ange nytt år (Enter för att behålla):[/]", movieChoice.Year);
            if (newYear <= 0 || newYear > DateTime.Now.Year)
            {
                AnsiConsole.MarkupLine("[red]År får inte vara negativt eller i framtiden.[/]");
                WaitForKey();
                return;
            }

            var genres = genreRepo.GetAllGenres();

            var genreChoice = AnsiConsole.Prompt(
                new SelectionPrompt<Genre>()
                    .Title("[cyan]Välj en ny genre:[/]")
                    .UseConverter(g => $"{g.GenreId}. {g.GenreName}")
                    .AddChoices(genres)
            );

            movieRepo.UpdateMovie(movieChoice.MovieId, newTitle, newYear, genreChoice.GenreId);

            AnsiConsole.MarkupLine("\n[green]Filmen har uppdaterats![/]");
            WaitForKey();
        }

        // Tar bort en film och kräver bekräftelse för att undvika oavsiktliga borttagningar
        public void DeleteMovie()
        {
            AnsiConsole.Clear();

            AnsiConsole.Write(
                new Panel("[bold hotpink]=== Ta bort film ===[/]")
                    .BorderColor(Color.Orange1)
            );

            var movies = movieRepo.GetAllMovies();
            if (movies.Count == 0)
            {
                AnsiConsole.MarkupLine("\n[grey]Inga filmer hittades att ta bort.[/]");
                WaitForKey();
                return;
            }

            var movieChoice = AnsiConsole.Prompt(
                new SelectionPrompt<Movie>()
                    .Title("[cyan]Välj en film att ta bort:[/]")
                    .UseConverter(m => $"{m.MovieId}. {m.Title} ({m.Year}) - {m.GenreName}")
                    .AddChoices(movies)
            );

            AnsiConsole.MarkupLine($"\n[red]Är du säker på att du vill ta bort filmen '{movieChoice.Title}'?[/]");
            var confirm = AnsiConsole.Confirm("Bekräfta borttagning:");

            if (!confirm)
            {
                AnsiConsole.MarkupLine("\n[yellow]Borttagning avbröts.[/]");
                WaitForKey();
                return;
            }

            movieRepo.DeleteMovie(movieChoice.MovieId);

            AnsiConsole.MarkupLine($"\n[green]Filmen '{movieChoice.Title}' har tagits bort![/]");
            WaitForKey();
        }

        // Pausfunktion för att ge användaren tid att läsa innan menyn fortsätter
        private void WaitForKey()
        {
            AnsiConsole.MarkupLine("\n[grey]Tryck på valfri tangent för att gå tillbaka...[/]");
            Console.ReadKey(true);
        }
    }
}
