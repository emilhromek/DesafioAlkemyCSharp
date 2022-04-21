using System.Collections.Generic;
using DesafioAlkemyCSharp.Repositories;
using DesafioAlkemyCSharp.Entities;
using DesafioAlkemyCSharp.DTOs;

namespace DesafioAlkemyCSharp.Services
{
    public interface IGenreService
    {
        public bool CheckIfFieldIsNotNull(string text);
        public List<GenreDtoOutShort> ListGenres();
        public int CreateGenre(GenreDtoIn genre);
        public bool CheckIfGenreIsInUse(int genreId);
        public void DeleteGenre(int genreId);
        public GenreDtoOut ReadGenre(int genreId);
        public void UpdateGenre(int genreId, GenreDtoIn dto);
    }
    public class GenreService: IGenreService
    {
        private readonly Repository _repository;
        public GenreService(Repository repository)
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

        public List<GenreDtoOutShort> ListGenres()
        {
            var listOutDto = new List<GenreDtoOutShort>();

            foreach (Genre genre in _repository.Traer<Genre>())
            {
                var genreOut = new GenreDtoOutShort
                {
                    GenreId = genre.GenreId,
                    Imagen = genre.Imagen,
                    Nombre = genre.Nombre,
                };

                listOutDto.Add(genreOut);
            }
             
            return listOutDto;
        }

        public int CreateGenre(GenreDtoIn genre)
        {
            var newGenre = new Genre()
            {
                Imagen = genre.Imagen,
                Nombre = genre.Nombre,
            };

            _repository.Agregar(newGenre);

            return newGenre.GenreId;
        }

        public bool CheckIfGenreIsInUse(int genreId)
        {
            bool isInUse = false;

            foreach (Movie m in _repository.Traer<Movie>())
            {
                if (m.GenreId == genreId)
                {
                    isInUse = true;
                    break;
                }
            }

            return isInUse;
        }

        public void DeleteGenre(int genreId)
        {    
            _repository.BorrarPor<Genre>(genreId);
        }

        public GenreDtoOut ReadGenre (int genreId)
        {
            var genre = _repository.EncontrarPor<Genre>(genreId);

            if (genre == null)
            {
                return null;
            }

            var movieList = new List<MovieDtoOutShort>();

            foreach (var x in _repository.Traer<Movie>())
            {
                if (x.GenreId == genreId)
                {
                    var movieOut = new MovieDtoOutShort()
                    {
                        Titulo = x.Titulo,
                        Imagen = x.Imagen,
                        FechaCreacion = x.FechaCreacion,
                    };

                    movieList.Add(movieOut);
                }
            }

            var genreOut = new GenreDtoOut()
            {
                GenreId = genre.GenreId,
                Imagen = genre.Imagen,
                Nombre = genre.Nombre,
                Movies = movieList,
            };

            return genreOut;
        }

        public void UpdateGenre(int genreId, GenreDtoIn dto)
        {
            var genre = _repository.EncontrarPor<Genre>(genreId);

            if (genre.Imagen != dto.Imagen)
                genre.Imagen = dto.Imagen;

            if (genre.Nombre != dto.Nombre)
                genre.Nombre = dto.Nombre;

            _repository.Actualizar<Genre>(genre);
        }

    }

}
