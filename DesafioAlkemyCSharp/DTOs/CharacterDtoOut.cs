using System.Collections.Generic;

namespace DesafioAlkemyCSharp.DTOs
{
    public class CharacterDtoOut
    {
        public int CharacterId { get; set; }
        public string Imagen { get; set; }
        public string Nombre { get; set; }
        public int Edad { get; set; }
        public int Peso { get; set; }
        public string Historia { get; set; }
        public List<MovieDtoOutShort> Movies { get; set; }
    }
}
