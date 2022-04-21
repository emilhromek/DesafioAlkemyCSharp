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
    public class CharactersController : ControllerBase
    {
        private readonly ICharacterService _service;

        public CharactersController(ICharacterService characterService)
        {
            _service = characterService;
        }

        // create character

        [HttpPost]
        public IActionResult CreateCharacter(CharacterDtoIn character)
        {
            try
            {
                if (!_service.CheckIfFieldIsNotNull(character.Nombre))
                {
                    throw new BadRequestException("El nombre no puede ser nulo.");
                }
                if (!_service.CheckIfFieldIsNotNull(character.Imagen))
                {
                    throw new BadRequestException("La imagen no puede ser nula.");
                }
                if (!_service.CheckIfFieldIsInt(character.Edad))
                {
                    throw new BadRequestException("La edad debe ser un numero");
                }
                if (!_service.CheckIfFieldIsInt(character.Peso))
                {
                    throw new BadRequestException("El peso debe ser un numero");
                }
                if (!_service.CheckIfFieldIsNotNull(character.Historia))
                {
                    throw new BadRequestException("La imagen no puede ser nula.");
                }
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }

            int id = _service.CreateCharacter(character);

            return Ok("Creado el personaje con id = " + id);
        }

        // get character

        [HttpGet("{id}")]
        public IActionResult GetCharacterById(int id)
        {
            try
            {
                var character = this._service.ReadCharacter(id);

                if (character == null)
                {
                    throw new NotFoundException($"El personaje con id = {id} no existe.");
                }
                    
                return new JsonResult(character) { StatusCode = 200 };
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // get list of characters

        [HttpGet]
        public IActionResult ReadCharacters([FromQuery] string name = "", [FromQuery] string age = "", [FromQuery] string idMovie = "")
        {
            var lista = _service.ListCharacters(name, age, idMovie);
            return new JsonResult(lista) { StatusCode = 200 };
        }

        // delete character

        [HttpDelete("{id}")]
        public IActionResult DeleteCharacterById(int id)
        {
            try
            {
                var character = this._service.ReadCharacter(id);

                if (character == null)
                {
                    throw new NotFoundException("El personaje con id " + id + " no existe");
                }

                _service.DeleteCharacter(id);

                return Ok("El personaje con id " + id + " fue borrado");
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // update character

        [HttpPut]
        public IActionResult UpdateCharacterById(int id, CharacterDtoIn characterDto)
        {
            try
            {
                var character = this._service.ReadCharacter(id);

                if (character == null)
                {
                    throw new NotFoundException("El personaje con id " + id + " no existe");
                }

                if (character == null)
                {
                    throw new NotFoundException("El personaje con id " + id + " no existe");
                }

                if (!_service.CheckIfFieldIsNotNull(characterDto.Imagen))
                {
                    throw new BadRequestException("La imagen no puede ser nula");
                }

                if (!_service.CheckIfFieldIsNotNull(characterDto.Nombre))
                {
                    throw new BadRequestException("El nombre no puede ser nula");
                }

                if (!_service.CheckIfFieldIsNotNull(characterDto.Historia))
                {
                    throw new BadRequestException("La historia no puede ser nula");
                }

                _service.UpdateCharacter(id, characterDto);

                return Ok("Update ok");

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

        // post asociacion a pelicula

        [Route("{id}/movies/{idMovie}")]
        [HttpPost]
        public IActionResult AddMovieToCharacter(int id, int idMovie)
        {
            try
            {
                var character = _service.ReadCharacter(id);

                if (character == null)
                {
                    throw new NotFoundException("No existe ese personaje");
                }

                var movie = _service.CheckIfMovieExists(idMovie);

                if (!movie)
                {
                    throw new NotFoundException("No existe esa pelicula/serie");
                }

                var characterAndMovieAreJoint = _service.CheckIfMovieAndCharacterAreJoint(id, idMovie);

                if (characterAndMovieAreJoint)
                {
                    throw new BadRequestException("El personaje y la pelicula/serie ya estan asociados");
                }

                _service.CreateMovieCharacter(id, idMovie);

                return Ok("Ok");

            }
            catch(NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // borrar asociacion a pelicula

        [Route("{id}/movies/{idMovie}")]
        [HttpDelete]
        public IActionResult RemoveMovieFromCharacter(int id, int idMovie)
        {
            try
            {
                var character = _service.ReadCharacter(id);

                if (character == null)
                {
                    throw new NotFoundException("No existe ese personaje");
                }

                var movie = _service.CheckIfMovieExists(idMovie);

                if (!movie)
                {
                    throw new NotFoundException("No existe esa pelicula/serie");
                }

                var characterAndMovieAreJoint = _service.CheckIfMovieAndCharacterAreJoint(id, idMovie);

                if (!characterAndMovieAreJoint)
                {
                    throw new BadRequestException("El personaje y la pelicula/serie no estan asociados");
                }

                _service.DeleteMovieCharacter(id, idMovie);

                return Ok("Ok");

            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return NotFound(ex.Message);
            }
        }

    }
}
