using System;
using System.Collections.Generic;
using System.Text;

namespace ADONet_MovieRegister.Data
{
    // Håller databasens connection string samlad på ett ställe för enkel åtkomst och ändring
    internal class DbConfig
    {
        public static string ConnectionString =
          "Server=localhost;Database=MovieRegisterDB;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;";
    }

}
