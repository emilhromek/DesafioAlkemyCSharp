using System;

namespace DesafioAlkemyCSharp.Entities
{
    // Entidad que representa a las peliculas y series
    public class Movie
    {
        public int MovieId { get; set; }
        public string Imagen { get; set; }
        public string Titulo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int Calificacion { get; set; }
        public int GenreId { get; set; }
    }
}
