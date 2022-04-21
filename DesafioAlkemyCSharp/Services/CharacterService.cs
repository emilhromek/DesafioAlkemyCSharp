using System;
using System.Collections.Generic;
using DesafioAlkemyCSharp.Repositories;
using DesafioAlkemyCSharp.Entities;
using DesafioAlkemyCSharp.DTOs;

namespace DesafioAlkemyCSharp.Services
{
    public interface ICharacterService
    {
        public bool CheckIfFieldIsNotNull(string text);
        public bool CheckIfFieldIsInt(object obj);
        public List<CharacterDtoOutShort> ListCharacters(string name, string age, string movies);
        public int CreateCharacter(CharacterDtoIn character);
        public bool CheckIfMovieExists(int movieId);
        public bool CheckIfMovieAndCharacterAreJoint(int characterId, int movieId);
        public void CreateMovieCharacter(int characterId, int movieId);
        public void DeleteMovieCharacter(int idCharacter, int idMovie);
        public void DeleteCharacter(int characterId);
        public CharacterDtoOut ReadCharacter(int CharacterId);
        public void UpdateCharacter(int CharacterId, CharacterDtoIn dto);
    }

    public class CharacterService: ICharacterService
    {
        private readonly Repository _repository;
        public CharacterService(Repository repository)
        {
            _repository = repository;
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

            var listInitial = _repository.Traer<Character>();

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

                foreach (MovieCharacter s in _repository.Traer<MovieCharacter>())
                {
                    if (s.MovieId == Int32.Parse(movies) && listOut2.Contains(_repository.EncontrarPor<Character>(s.CharacterId)))
                    {
                        listOut3.Add(_repository.EncontrarPor<Character>(s.CharacterId));
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
            var newCharacter = new Character()
            {
                Imagen = character.Imagen,
                Nombre = character.Nombre,
                Edad = character.Edad,
                Peso = character.Peso,
                Historia = character.Historia,
            };

            _repository.Agregar(newCharacter);

            return newCharacter.CharacterId;
        }

        public bool CheckIfMovieExists(int movieId)
        {
            var check = _repository.EncontrarPor<Movie>(movieId);

            if (check == null)
            {
                return false;
            }

            return true;
        }

        public bool CheckIfMovieAndCharacterAreJoint(int characterId, int movieId)
        {
            bool join = false;

            foreach (var x in _repository.Traer<MovieCharacter>())
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

            _repository.Agregar(newMovieCharacter);
        }

        public void DeleteMovieCharacter(int idCharacter, int idMovie)
        {
            foreach (var x in _repository.Traer<MovieCharacter>())
            {
                if (x.CharacterId == idCharacter && x.MovieId == idMovie)
                {
                    _repository.Borrar<MovieCharacter>(x);
                    break;
                }
            }
        }

        public void DeleteCharacter(int characterId)
        {
            var character = _repository.EncontrarPor<Character>(characterId);

            if (character == null)
            {
                throw new Exception("El personaje con id = " + characterId + " no existe.");
            }

            _repository.BorrarPor<Character>(characterId);

            // borrar las asociaciones con peliculas

            foreach (MovieCharacter m in _repository.Traer<MovieCharacter>())
            {
                if (m.CharacterId == characterId)
                {
                    _repository.Borrar<MovieCharacter>(m);
                }
            }
        }

        public CharacterDtoOut ReadCharacter (int CharacterId)
        {
            var character = _repository.EncontrarPor<Character>(CharacterId);

            if (character == null)
            {
                return null;
            }

            var movieList = new List<MovieDtoOutShort>();

            foreach (var x in _repository.Traer<MovieCharacter>())
            {
                if (x.CharacterId == CharacterId)
                {
                    var movie = _repository.EncontrarPor<Movie>(x.MovieId);

                    var movieDto = new MovieDtoOutShort()
                    {
                        Imagen = movie.Imagen,
                        Titulo = movie.Titulo,
                        FechaCreacion = movie.FechaCreacion,
                    };

                    movieList.Add(movieDto);
                }
                
            }

            var characterOut = new CharacterDtoOut()
            {
                CharacterId = character.CharacterId,
                Imagen = character.Imagen,
                Nombre = character.Nombre,
                Edad = character.Edad,
                Peso = character.Peso,
                Historia = character.Historia,
                Movies = movieList,
            };

            return characterOut;
        }

        public void UpdateCharacter(int CharacterId, CharacterDtoIn dto)
        {
            var character = _repository.EncontrarPor<Character>(CharacterId);

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

            _repository.Actualizar<Character>(character);
        }

    }

}
