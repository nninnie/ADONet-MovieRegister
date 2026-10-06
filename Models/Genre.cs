using System;
using System.Collections.Generic;
using System.Text;

namespace ADONet_MovieRegister.Models
{
    // Modellklass för en genre, med egenskaper för ID och namn
    public class Genre
    {
        public int GenreId { get; set; }
        public required string GenreName { get; set; }
    }
}
