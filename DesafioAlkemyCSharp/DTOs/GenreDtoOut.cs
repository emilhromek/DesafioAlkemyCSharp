using System.Collections.Generic;

namespace DesafioAlkemyCSharp.DTOs
{
    public class GenreDtoOut
    {
        public int GenreId { get; set; }
        public string Nombre { get; set; }
        public string Imagen { get; set; }
        public List<MovieDtoOutShort> Movies { get; set; }
    }
}
