using System;

namespace DesafioAlkemyCSharp.DTOs
{
    public class MovieDtoIn
    {
        public string Imagen { get; set; }
        public string Titulo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int Calificacion { get; set; }
        public int GenreId { get; set; }
    }
}
