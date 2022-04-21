using System;
using System.Collections.Generic;
using DesafioAlkemyCSharp.DTOs;
using DesafioAlkemyCSharp.Entities;
using DesafioAlkemyCSharp.Services;

namespace UnitTests
{
    public class GenreServiceFake: IGenreService
    {
        private List<Movie> _movie;
        private List<Genre> _genre;

        public GenreServiceFake()
        {
            _movie = new List<Movie>();

            _movie.Add(new Movie() { MovieId = 1, Imagen = "imagen", Titulo = "Pelicula 1", FechaCreacion = DateTime.UtcNow, Calificacion = 5, GenreId = 1 });
            _movie.Add(new Movie() { MovieId = 2, Imagen = "imagen", Titulo = "Pelicula 2", FechaCreacion = DateTime.UtcNow, Calificacion = 5, GenreId = 2 });
            _movie.Add(new Movie() { MovieId = 3, Imagen = "imagen", Titulo = "Pelicula 3", FechaCreacion = DateTime.UtcNow, Calificacion = 5, GenreId = 3 });

            _genre = new List<Genre>();

            _genre.Add(new Genre() { GenreId = 1, Imagen = "imagen", Nombre = "Terror" });
            _genre.Add(new Genre() { GenreId = 2, Imagen = "imagen", Nombre = "Comedia" });
            _genre.Add(new Genre() { GenreId = 3, Imagen = "imagen", Nombre = "Ciencia ficcion" });
            _genre.Add(new Genre() { GenreId = 4, Imagen = "imagen", Nombre = "Suspenso" });
        }

        public bool CheckIfFieldIsNotNull(string text)
        {
            bool check = true;

            if (string.IsNullOrEmpty(text))
                check = false;

            return check;
        }

        public int CreateGenre(GenreDtoIn genre)
        {
            int id = 0;

            foreach(var x in _genre)
            {
                id = id + x.GenreId;
            }

            var newGenre = new Genre()
            {
                GenreId = id,
                Imagen = genre.Imagen,
                Nombre = genre.Nombre,
            };

            _genre.Add(newGenre);

            return id;
        }

        public bool CheckIfGenreExists(int genreId)
        {
            var genre = _genre.Find(x => x.GenreId == genreId);

            if (genre == null)
                return false;

            return true;
        }

        public void UpdateGenre(int genreId, GenreDtoIn dto)
        {
            var genre = _genre.Find(x => x.GenreId == genreId);

            if (genre != null)
            {
                if (genre.Imagen != dto.Imagen)
                    genre.Imagen = dto.Imagen;

                if (genre.Nombre != dto.Nombre)
                    genre.Nombre = dto.Nombre;
            }
            
        }

        public GenreDtoOut? ReadGenre(int genreId)
        {
            var genre = _genre.Find(x => x.GenreId == genreId);

            if (genre == null)
            {
                return null;
            }

            var movieList = new List<MovieDtoOutShort>();

            foreach (var x in _movie)
            {
                if (x.GenreId == genreId)
                {
                    var movieOut = new MovieDtoOutShort()
                    {
                        Titulo = x.Titulo,
                        Imagen = x.Imagen,
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

        public List<GenreDtoOutShort> ListGenres()
        {
            var listOutDto = new List<GenreDtoOutShort>();

            foreach (Genre genre in _genre)
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

        public bool CheckIfGenreIsInUse(int genreId)
        {
            bool isInUse = false;

            foreach (Movie m in _movie)
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
            var genre = _genre.Find(x => x.GenreId == genreId);

            if (genre != null)

            _genre.Remove(genre);
        }

    }
}
