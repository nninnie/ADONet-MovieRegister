using System;
using System.Collections.Generic;
using System.Text;

namespace ADONet_MovieRegister.Data
{
    internal class DbConfig
    {
        // Connectionstring för databasen (pekar på MovieRegisterDB)
        public static string ConnectionString =
            "Server=localhost;Database=MovieRegisterDB;Trusted_Connection=True;";
    }
}
