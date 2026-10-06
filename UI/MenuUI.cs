
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
                            break;

                        case "2":
                            break;

                        case "3":
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
    }
}