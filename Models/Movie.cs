using System;
using System.Collections.Generic;
using System.Text;

namespace ADONet_MovieRegister.Models
{
    // Representerar en film i systemet och används för att överföra data mellan databas och UI
    public class Movie
    {
        public int MovieId { get; set; }
        public required string Title { get; set; }
        public int Year { get; set; }
        public int GenreId { get; set; }
        public required string GenreName { get; set; }
    }

}
