using System;
using System.Collections.Generic;
using System.Text;

namespace ADONet_MovieRegister.Models
{
    // Modellklass för en film, med egenskaper för ID, titel, år, genre-ID och genre-namn
    public class Movie
        {
            public int MovieId { get; set; }
            public required string Title { get; set; }
            public int Year { get; set; }
            public int GenreId { get; set; }
            public required string GenreName { get; set; } 
        }
    }
