namespace DesafioAlkemyCSharp.Entities
{
    // Entidad que actua como tabla intermedia entre peliculas/series y personajes
    public class MovieCharacter
    {
        public int MovieCharacterId { get; set; }
        public int MovieId { get; set; }
        public int CharacterId { get; set; }
    }
}
