using System;
using System.Collections.Generic;
using DesafioAlkemyCSharp.DTOs;
using DesafioAlkemyCSharp.Entities;
using DesafioAlkemyCSharp.Services;

namespace UnitTests
{
    public class MovieServiceFake: IMovieService
    {
        private List<Character> _character;
        private List<Movie> _movie;
        private List<MovieCharacter> _movieCharacters;
        private List<Genre> _genre;

        public MovieServiceFake()
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

            _genre = new List<Genre>();

            _genre.Add(new Genre() { GenreId = 1, Imagen = "imagen", Nombre = "Terror" });
            _genre.Add(new Genre() { GenreId = 2, Imagen = "imagen", Nombre = "Comedia" });
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

        public int CreateMovie(MovieDtoIn movie)
        {
            int id = 0;

            foreach(var x in _movie)
            {
                id = id + x.MovieId;
            }

            var newMovie = new Movie()
            {
                MovieId = id,
                Imagen = movie.Imagen,
                Titulo = movie.Titulo,
                FechaCreacion = movie.FechaCreacion,
                Calificacion = movie.Calificacion,
                GenreId = movie.GenreId,
            };

            _movie.Add(newMovie);

            return id;
        }

        public bool CheckIfCharacterExists(int characterId)
        {
            var character = _character.Find(x => x.CharacterId == characterId);

            if (character == null)
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

        public void DeleteMovie(int movieId)
        {
            _movieCharacters.RemoveAll(m => m.MovieId == movieId);

            foreach (var x in _movie)
            {
                if (x.MovieId == movieId)
                {
                    _movie.Remove(x);
                    break;
                }
            }
        }

        public MovieDtoOut? MovieCharacter(int movieId)
        {
            var movie = _movie.Find(x => x.MovieId == movieId);

            if (movie == null)
                return null;

            return new MovieDtoOut()
            {
                MovieId = movie.MovieId,
                Titulo = movie.Titulo,
            };

        }

        public void UpdateMovie(int movieId, MovieDtoIn dto)
        {
            var movie = _movie.Find(x => x.MovieId == movieId);

            if (movie != null)
            {
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
            }
            
        }

        public List<MovieDtoOutShort> ListMovies(string name, string genre, string order)
        {
            var listInitial = _movie;

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
            var genre = _genre.Find(x => x.GenreId == genreId);

            if (genre == null)
                return false;

            return true;
        }

        public MovieDtoOut? ReadMovie(int movieId)
        {
            var movie = _movie.Find(x => x.MovieId == movieId);

            if (movie == null)
            {
                return null;
            }

            var characterList = new List<CharacterDtoOutShort>();

            foreach (var x in _movieCharacters)
            {
                if (x.MovieId == movieId)
                {
                    var character = _character.Find(y => y.CharacterId == x.CharacterId);

                    if (character != null)
                    {
                        var characterDto = new CharacterDtoOutShort()
                        {
                            CharacterId = character.CharacterId,
                            Imagen = character.Imagen,
                            Nombre = character.Nombre,
                        };

                        characterList.Add(characterDto);
                    }  
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
    }
}
