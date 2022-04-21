using System;
using System.Collections.Generic;
using DesafioAlkemyCSharp.Repositories;
using DesafioAlkemyCSharp.Entities;
using DesafioAlkemyCSharp.DTOs;

namespace DesafioAlkemyCSharp.Services
{
    public interface IMovieService
    {
        public bool CheckIfFieldIsNotNull(string text);
        public bool CheckIfFieldIsInt(object obj);
        public List<MovieDtoOutShort> ListMovies(string name, string genre, string order);
        public bool CheckIfGenreExists(int genreId);
        public int CreateMovie(MovieDtoIn movie);
        public void DeleteMovie(int movieId);
        public MovieDtoOut ReadMovie(int movieId);
        public void UpdateMovie(int movieId, MovieDtoIn dto);
        public bool CheckIfCharacterExists(int characterId);
        public void CreateMovieCharacter(int characterId, int movieId);
        public bool CheckIfMovieAndCharacterAreJoint(int characterId, int movieId);
        public void DeleteMovieCharacter(int idCharacter, int idMovie);        

    }
    public class MovieService: IMovieService
    {
        private readonly Repository _repository;
        public MovieService(Repository repository)
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

        public List<MovieDtoOutShort> ListMovies(string name, string genre, string order)
        {
            var listInitial = _repository.Traer<Movie>();

            var listOutDto = new List<MovieDtoOutShort>();

            var listOut1 = new List<Movie>();

            if (name == "")
            {
                listOut1.AddRange(listInitial);
            }
            else
            {
                foreach (Movie p in listInitial)
                {
                    if (p.Titulo.Contains(name, StringComparison.OrdinalIgnoreCase))
                    {
                        listOut1.Add(p);
                    }
                }
            }

            var listOut2 = new List<Movie>();

            if (genre == "")
            {
                listOut2.AddRange(listOut1);
            }
            else
            {
                foreach (Movie p in listOut1)
                {
                    if (p.GenreId.ToString() == genre)
                    {
                        listOut2.Add(p);
                    }
                }
            }

            if (order == "")
            {

            }
            else
            {
                string orden = order.ToUpper();

                if (orden.Equals("ASC"))
                {
                    listOut2.Sort((x, y) =>
                    x.FechaCreacion.CompareTo(y.FechaCreacion));
                }

                if (orden.Equals("DESC"))
                {
                    listOut2.Sort((x, y) =>
                    y.FechaCreacion.CompareTo(x.FechaCreacion));
                }
                else
                {

                }

            }
            
            foreach (Movie s in listOut2)
            {
                var movieDto = new MovieDtoOutShort
                {
                    Imagen = s.Imagen,
                    Titulo = s.Titulo,
                    FechaCreacion = s.FechaCreacion,
                };

                listOutDto.Add(movieDto);
            }

            return listOutDto;
        }

        public bool CheckIfGenreExists(int genreId)
        {
            var check = _repository.EncontrarPor<Genre>(genreId);

            if (check == null)
            {
                return false;
            }

            return true;
        }

        public int CreateMovie(MovieDtoIn movie)
        {

            var newMovie = new Movie()
            {
                Imagen = movie.Imagen,
                Titulo = movie.Titulo,
                FechaCreacion = movie.FechaCreacion,
                Calificacion = movie.Calificacion,
                GenreId = movie.GenreId,
            };

            _repository.Agregar(newMovie);

            return newMovie.MovieId;
        }

        public void DeleteMovie(int movieId)
        {
            _repository.BorrarPor<Movie>(movieId);

            // borrar las asociaciones con personajes

            foreach (MovieCharacter m in _repository.Traer<MovieCharacter>())
            {
                if (m.MovieId == movieId)
                {
                    _repository.Borrar<MovieCharacter>(m);
                }
            }
        }

        public MovieDtoOut ReadMovie(int movieId)
        {
            var movie = _repository.EncontrarPor<Movie>(movieId);

            if (movie == null)
            {
                return null;
            }

            var characterList = new List<CharacterDtoOutShort>();

            foreach (var x in _repository.Traer<MovieCharacter>())
            {
                if (x.MovieId == movieId)
                {
                    var character = _repository.EncontrarPor<Character>(x.CharacterId);

                    var characterDto = new CharacterDtoOutShort()
                    {
                        CharacterId = character.CharacterId,
                        Imagen = character.Imagen,
                        Nombre = character.Nombre,
                    };

                    characterList.Add(characterDto);
                }

            }

            var movieOut = new MovieDtoOut()
            {
                MovieId = movie.MovieId,
                Imagen = movie.Imagen,
                Titulo = movie.Titulo,
                FechaCreacion = movie.FechaCreacion,
                Calificacion = movie.Calificacion,
                GenreId = movie.GenreId,
                Characters = characterList,
            };

            return movieOut;
        }

        public void UpdateMovie(int movieId, MovieDtoIn dto)
        {
            var movie = _repository.EncontrarPor<Movie>(movieId);

            if (movie.Imagen != dto.Imagen)
                movie.Imagen = dto.Imagen;

            if (movie.Titulo != dto.Titulo)
                movie.Titulo = dto.Titulo;

            if (movie.FechaCreacion != dto.FechaCreacion)
                movie.FechaCreacion = dto.FechaCreacion;

            if (movie.Calificacion != dto.Calificacion)
                movie.Calificacion = dto.Calificacion;

            if (movie.GenreId != dto.GenreId)
                movie.GenreId = dto.GenreId;

            _repository.Actualizar<Movie>(movie);
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

        public void CreateMovieCharacter(int characterId, int movieId)
        {
            var newMovieCharacter = new MovieCharacter()
            {
                MovieId = movieId,
                CharacterId = characterId,
            };

            _repository.Agregar(newMovieCharacter);
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

    }

}
