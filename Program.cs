using ADONet_MovieRegister.UI;

namespace ADONet_MovieRegister
{
    internal class Program
    {
        // Programstart för applikationen. Ansvarar för att initiera UI och starta huvudmenyn.
        static void Main(string[] args)
        {
            var menu = new MenuUI();
            menu.ShowMenu();
        }
    }
}