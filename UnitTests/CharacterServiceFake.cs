using System;
using System.Collections.Generic;
using DesafioAlkemyCSharp.DTOs;
using DesafioAlkemyCSharp.Entities;
using DesafioAlkemyCSharp.Services;

namespace UnitTests
{
    public class CharacterServiceFake: ICharacterService
    {
        private List<Character> _character;
        private List<Movie> _movie;
        private List<MovieCharacter> _movieCharacters;

        public CharacterServiceFake()
        {
            _character = new List<Character>();

            _character.Add(new Character() { CharacterId = 1, Imagen = "url_fake", Nombre = "Jose", Edad = 25, Peso = 70, Historia = "historia" });
            _character.Add(new Character() { CharacterId = 2, Imagen = "url_fake", Nombre = "Martin", Edad = 26, Peso = 71, Historia = "historia" });
            _character.Add(new Character() { CharacterId = 3, Imagen = "url_fake", Nombre = "Pablo", Edad = 27, Peso = 72, Historia = "historia" });

            _movie = new List<Movie>();

            _movie.Add(new Movie() { MovieId = 1, Imagen = "imagen", Titulo = "Pelicula 1", FechaCreacion = DateTime.UtcNow, Calificacion = 5, GenreId = 1 });
            _movie.Add(new Movie() { MovieId = 2, Imagen = "imagen", Titulo = "Pelicula 2", FechaCreacion = DateTime.UtcNow, Calificacion = 5, GenreId = 2 });
            _movie.Add(new Movie() { MovieId = 3, Imagen = "imagen", Titulo = "Pelicula 3", FechaCreacion = DateTime.UtcNow, Calificacion = 5, GenreId = 3 });

            _movieCharacters = new List<MovieCharacter>();

            _movieCharacters.Add(new MovieCharacter { MovieCharacterId = 1, MovieId = 1, CharacterId = 1 });
        }

        public bool CheckIfFieldIsNotNull(string text)
        {
            bool check = true;

            if (string.IsNullOrEmpty(text))
                check = false;

            return check;
        }

        public bool CheckIfFieldIsInt(object obj)
        {
            bool check = false;

            if (obj is int)
                check = true;

            return check;
        }

        public List<CharacterDtoOutShort> ListCharacters(string name, string age, string movies)
        {
            var listOutDto = new List<CharacterDtoOutShort>();

            var listInitial = _character;

            var listOut1 = new List<Character>();

            if (name == "")
            {
                listOut1.AddRange(listInitial);
            }
            else
            {
                foreach (Character p in listInitial)
                {
                    if (p.Nombre.Contains(name, StringComparison.OrdinalIgnoreCase))
                    {
                        listOut1.Add(p);
                    }
                }
            }

            var listOut2 = new List<Character>();

            if (age == "")
            {
                listOut2.AddRange(listOut1);
            }
            else
            {
                foreach (Character p in listInitial)
                {
                    if (p.Edad.ToString() == age)
                    {
                        listOut2.Add(p);
                    }
                }
            }

            var listOut3 = new List<Character>();

            if (movies == "")
            {
                listOut3.AddRange(listOut2);
            }
            else
            {
                // armamos la lista de personajes de esas peliculas

                foreach (MovieCharacter s in _movieCharacters)
                {
                    if (s.MovieId == Int32.Parse(movies) && listOut2.Contains(_character.Find(c => c.CharacterId == s.CharacterId)))
                    {
                        listOut3.Add(_character.Find(c => c.CharacterId == s.CharacterId));
                    }
                }
            }

            foreach (Character character in listOut3)
            {
                var characterOut = new CharacterDtoOutShort
                {
                    CharacterId = character.CharacterId,
                    Imagen = character.Imagen,
                    Nombre = character.Nombre,
                };

                listOutDto.Add(characterOut);
            }

            return listOutDto;
        }

        public int CreateCharacter(CharacterDtoIn character)
        {
            int id = 0;

            foreach(var x in _character)
            {
                id = id + x.CharacterId;
            }

            var newCharacter = new Character()
            {
                CharacterId = id,
                Imagen = character.Imagen,
                Nombre = character.Nombre,
                Edad = character.Edad,
                Peso = character.Peso,
                Historia = character.Historia,
            };

            _character.Add(newCharacter);

            return id;
        }

        public bool CheckIfMovieExists(int movieId)
        {
            var movie = _movie.Find(x => x.MovieId == movieId);

            if (movie == null)
                return false;

            return true;
        }

        public bool CheckIfMovieAndCharacterAreJoint(int characterId, int movieId)
        {
            bool join = false;

            foreach (var x in _movieCharacters)
            {
                if (x.CharacterId == characterId && x.MovieId == movieId)
                {
                    join = true;
                    break;
                }
            }

            return join;
        }

        public void CreateMovieCharacter(int characterId, int movieId)
        {
            var newMovieCharacter = new MovieCharacter()
            {
                MovieId = movieId,
                CharacterId = characterId,
            };

            _movieCharacters.Add(newMovieCharacter);
        }

        public void DeleteMovieCharacter(int idCharacter, int idMovie)
        {
            foreach (var x in _movieCharacters)
            {
                if (x.CharacterId == idCharacter && x.MovieId == idMovie)
                {
                    _movieCharacters.Remove(x);
                    break;
                }
            }
        }

        public void DeleteCharacter(int characterId)
        {
            foreach (var x in _character)
            {
                if (x.CharacterId == characterId)
                {
                    _character.Remove(x);
                    break;
                }               
            }

            _movieCharacters.RemoveAll(m => m.CharacterId == characterId);
        }

        public CharacterDtoOut? ReadCharacter(int characterId)
        {
            var character = _character.Find(x => x.CharacterId == characterId);

            if (character == null)
                return null;

            var movieList = new List<MovieDtoOutShort>();

            foreach (var x in _movieCharacters)
            {
                if (x.CharacterId == characterId)
                {
                    var movie = _movie.Find(m => m.MovieId == x.MovieId);

                    if (movie != null)
                    {
                        var movieDto = new MovieDtoOutShort()
                        {
                            Imagen = movie.Imagen,
                            Titulo = movie.Titulo,
                            FechaCreacion = movie.FechaCreacion,
                        };

                        movieList.Add(movieDto);
                    } 
                }

            }

            return new CharacterDtoOut()
            {
                CharacterId = character.CharacterId,
                Imagen = character.Imagen,
                Nombre = character.Nombre,
                Edad = character.Edad,
                Peso = character.Peso,
                Historia = character.Historia,
                Movies = movieList,
            };

        }

        public void UpdateCharacter(int characterId, CharacterDtoIn dto)
        {
            var character = _character.Find(x => x.CharacterId == characterId);
            
            if (character != null)
            {
                if (character.Imagen != dto.Imagen)
                    character.Imagen = dto.Imagen;

                if (character.Nombre != dto.Nombre)
                    character.Nombre = dto.Nombre;

                if (character.Edad != dto.Edad)
                    character.Edad = dto.Edad;

                if (character.Peso != dto.Peso)
                    character.Peso = dto.Peso;

                if (character.Historia != dto.Historia)
                    character.Historia = dto.Historia;
            }
            
        }
    }
}
