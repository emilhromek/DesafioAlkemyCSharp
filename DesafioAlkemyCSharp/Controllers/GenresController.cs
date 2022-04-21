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
    public class GenresController : ControllerBase
    {
        private readonly IGenreService _service;

        public GenresController(IGenreService generoService)
        {
            _service = generoService;
        }

        [HttpPost]
        public IActionResult PostGenero(GenreDtoIn genero)
        {
            try
            {
                if (!_service.CheckIfFieldIsNotNull(genero.Nombre))
                {
                    throw new BadRequestException("El nombre no puede ser nulo.");
                }
                if (!_service.CheckIfFieldIsNotNull(genero.Imagen))
                {
                    throw new BadRequestException("La imagen no puede ser nula.");
                }

                int id = _service.CreateGenre(genero);

                return Ok("Creado el genero con id = " + id);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }          


        }

        [HttpGet("{id}")]
        public IActionResult GetGeneroById(int id)
        {
            try
            {
                var genero = this._service.ReadGenre(id);

                if (genero == null)
                {
                    throw new NotFoundException("El genero con id " + id + " no existe");
                }

                return new JsonResult(genero) { StatusCode = 200 };
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult GetGeneros()
        {
            var lista = _service.ListGenres();

            return new JsonResult(lista) { StatusCode = 200 };
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteGeneroById(int id)
        {
            try
            {
                if (_service.ReadGenre(id) == null)
                {
                    throw new NotFoundException("El genero con id = " + id + " no existe");
                }

                if (_service.CheckIfGenreIsInUse(id))
                {
                    throw new BadRequestException("El genero con id = " + id + " esta en uso por al menos una pelicula/serie.");
                }

                _service.DeleteGenre(id);

                return Ok("El genero con id = " + id + " fue borrado");
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

        [HttpPut]
        public IActionResult UpdateGeneroById(int id, GenreDtoIn generoDTO)
        {
            try
            {
                if (_service.ReadGenre(id) == null)
                {
                    throw new NotFoundException("El genero con id = " + id + " no existe");
                }
                if (!_service.CheckIfFieldIsNotNull(generoDTO.Nombre))
                {
                    throw new BadRequestException("El nombre no puede ser nulo.");
                }
                if (!_service.CheckIfFieldIsNotNull(generoDTO.Imagen))
                {
                    throw new BadRequestException("La imagen no puede ser nula.");
                }

                _service.UpdateGenre(id, generoDTO);

                return Ok("El genero con id = " + id + " fue atualizado");

            }
            catch (NotFoundException ex)
            {
                return NotFound(ex);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
