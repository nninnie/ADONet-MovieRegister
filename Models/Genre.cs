using System;
using System.Collections.Generic;
using System.Text;

namespace ADONet_MovieRegister.Models
{
    // Representerar en genre i systemet och används för att hämta och visa genredata från databasen
    public class Genre
    {
        public int GenreId { get; set; }
        public required string GenreName { get; set; }
    }

}
