using System.Collections.Generic;
using DesafioAlkemyCSharp.Repositories;
using DesafioAlkemyCSharp.Entities;
using DesafioAlkemyCSharp.DTOs;

namespace DesafioAlkemyCSharp.Services
{
    public class MovieCharacterService
    {
        private readonly Repository _repository;
        public MovieCharacterService(Repository repository)
        {
            _repository = repository;
        }

        public List<MovieCharacterDtoOut> ListMovieCharacter()
        {
            var listOutDto = new List<MovieCharacterDtoOut>();

            foreach (MovieCharacter movieCharacter in _repository.Traer<MovieCharacter>())
            {
                var movieCharacterOut = new MovieCharacterDtoOut
                {
                    MovieCharacterId = movieCharacter.MovieCharacterId,
                    MovieId = movieCharacter.MovieId,
                    CharacterId = movieCharacter.CharacterId,
                };

                listOutDto.Add(movieCharacterOut);
            }
             
            return listOutDto;
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
        public bool CheckIfCharacterExists(int characterId)
        {
            var check = _repository.EncontrarPor<Character>(characterId);

            if (check == null)
            {
                return false;
            }

            return true;
        }

        public void CreateMovieCharacter(MovieCharacterDtoIn movieCharacter)
        {       
            var newMovieCharacter = new MovieCharacter()
            {
                MovieId = movieCharacter.MovieId,
                CharacterId = movieCharacter.CharacterId,
            };

            _repository.Agregar(newMovieCharacter);
        }

        public void DeleteMovieCharacter(int movieCharacterId)
        {
            _repository.BorrarPor<MovieCharacter>(movieCharacterId);
        }

        public MovieCharacterDtoOut ReadMovieCharacter(int movieCharacterId)
        {
            var movieCharacter = _repository.EncontrarPor<MovieCharacter>(movieCharacterId);

            if (movieCharacter == null)
            {
                return null;
            }

            var movieCharacterOut = new MovieCharacterDtoOut()
            {
                MovieId = movieCharacter.MovieId,
                CharacterId = movieCharacter.CharacterId,
            };

            return movieCharacterOut;
        }

    }

}
