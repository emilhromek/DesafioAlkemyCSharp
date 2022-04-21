using System.Collections.Generic;
using System;

namespace DesafioAlkemyCSharp.DTOs
{
    public class MovieDtoOut
    {
        public int MovieId { get; set; }
        public string Imagen { get; set; }
        public string Titulo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int Calificacion { get; set; }
        public int GenreId { get; set; }
        public List<CharacterDtoOutShort> Characters { get; set; }
    }
}
