using Microsoft.AspNetCore.Mvc;
using DesafioAlkemyCSharp.DTOs;
using DesafioAlkemyCSharp.Services;
using Microsoft.AspNetCore.Authorization;
using DesafioAlkemyCSharp.Exceptions;

namespace DesafioAlkemyCSharp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {

        private readonly IMovieService _service;

        public MoviesController(IMovieService movieService)
        {
            _service = movieService;
        }

        [HttpPost]
        public IActionResult CreateMovie(MovieDtoIn movie)
        {
            try
            {
                var checkGenre = _service.CheckIfGenreExists(movie.GenreId);
                {
                    if (!checkGenre)
                    {
                        throw new NotFoundException("El genero con id = " + movie.GenreId + " no existe.");
                    }
                }

                if (!_service.CheckIfFieldIsNotNull(movie.Titulo))
                {
                    throw new BadRequestException("El titulo no puede ser nulo.");
                }
                if (!_service.CheckIfFieldIsNotNull(movie.Imagen))
                {
                    throw new BadRequestException("La imagen no puede ser nula.");
                }

                int id = _service.CreateMovie(movie);

                return Ok("Creada la pelicula o serie con id = " + id);

            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult ReadMovieById(int id)
        {
            try
            {
                var movie = this._service.ReadMovie(id);

                if (movie == null)
                {
                    throw new NotFoundException($"La pelicula o serie con id = {id} no existe");
                }
                    
                return new JsonResult(movie) { StatusCode = 200 };
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult ReadMovies([FromQuery] string name = "", [FromQuery] string genre = "", [FromQuery] string order = "")
        {
            var lista = _service.ListMovies(name, genre, order);

            return new JsonResult(lista) { StatusCode = 200 };
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteMovie(int id)
        {
            try
            {
                var movie = this._service.ReadMovie(id);

                if (movie == null)
                {
                    throw new NotFoundException("La pelicula o serie con id " + id + " no existe");
                }

                _service.DeleteMovie(id);

                return Ok("La pelicula o serie con id " + id + " fue borrada");
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut]
        public IActionResult UpdateMovieById(int id, MovieDtoIn movieDto)
        {
            try
            {
                var movie = this._service.ReadMovie(id);

                if (movie == null)
                {
                    throw new NotFoundException("La pelicula o serie con id " + id + " no existe.");
                }

                if (!_service.CheckIfGenreExists(movieDto.GenreId))
                {
                    throw new NotFoundException("No existe ese genero");
                }

                if (!_service.CheckIfFieldIsNotNull(movieDto.Titulo))
                {
                    throw new BadRequestException("El titulo no puede ser nulo.");
                }
                if (!_service.CheckIfFieldIsNotNull(movieDto.Imagen))
                {
                    throw new BadRequestException("La imagen no puede ser nula.");
                }

                _service.UpdateMovie(id, movieDto);

                return Ok("La pelicula o serie con id " + id + " fue actualizada.");

            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }

            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // post asociacion a personaje

        [Route("{id}/characters/{idCharacter}")]
        [HttpPost]
        public IActionResult AddCharacterToMovie(int id, int idCharacter)
        {
            try
            {
                var movie = _service.ReadMovie(id);

                if (movie == null)
                {
                    throw new NotFoundException("No existe esa pelicula/serie");
                }

                var character = _service.CheckIfCharacterExists(idCharacter);

                if (!character)
                {
                    throw new NotFoundException("No existe ese personaje");
                }

                var characterAndMovieAreJoint = _service.CheckIfMovieAndCharacterAreJoint(idCharacter, id);

                if (characterAndMovieAreJoint)
                {
                    throw new BadRequestException("El personaje y la pelicula/serie ya estan asociados");
                }

                _service.CreateMovieCharacter(idCharacter, id);

                return Ok("Ok");

            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // borrar asociacion a personaje

        [Route("{id}/characters/{idCharacter}")]
        [HttpDelete]
        public IActionResult RemoveCharacterFromMovie(int id, int idCharacter)
        {
            try
            {
                var movie = _service.ReadMovie(id);

                if (movie == null)
                {
                    throw new NotFoundException("No existe esa pelicula/serie");
                }

                var character = _service.CheckIfCharacterExists(idCharacter);

                if (!character)
                {
                    throw new NotFoundException("No existe ese personaje");
                }

                var characterAndMovieAreJoint = _service.CheckIfMovieAndCharacterAreJoint(idCharacter, id);

                if (!characterAndMovieAreJoint)
                {
                    throw new BadRequestException("El personaje y la pelicula/serie no estan asociados");
                }

                _service.DeleteMovieCharacter(idCharacter, id);

                return Ok("Ok");

            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
