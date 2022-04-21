using Microsoft.AspNetCore.Mvc;
using DesafioAlkemyCSharp.Controllers;
using DesafioAlkemyCSharp.DTOs;
using DesafioAlkemyCSharp.Services;
using Xunit;

namespace UnitTests
{
    public class GenresControllerTests
    {       
        private readonly IGenreService _service;
        private readonly GenresController _controller;        

        public GenresControllerTests()
        {
            _service = new GenreServiceFake();
            _controller = new GenresController(_service);
        }

        // test: al leer la lista devuelve un tipo Json

        [Fact]
        public void ReadList_Execute_ReturnsJsonResult()
        {
            var result = _controller.GetGeneros();

            Assert.IsType<JsonResult>(result);
        }

        // test: crear genero retorna ok

        [Fact]
        public void CreateCharacter_Execute_ReturnsOk()
        {
            var result = _controller.PostGenero(new GenreDtoIn
            {
                Imagen = "url_fake",
                Nombre = "Nuevo",
            });

            Assert.IsType<OkObjectResult>(result);
        }

        // test: crear genero con imagen nula retorna bad request

        [Fact]
        public void CreateGenreNullImagen_Execute_ReturnsBadRequest()
        {
            var result = _controller.PostGenero(new GenreDtoIn
            {
                Imagen = "",
                Nombre = "Nuevo",
            });

            Assert.IsType<BadRequestObjectResult>(result);
        }

        // test: crear genero con nombre nulo retorna bad request

        [Fact]
        public void CreateGenreNullNombre_Execute_ReturnsBadRequest()
        {
            var result = _controller.PostGenero(new GenreDtoIn
            {
                Imagen = "imagen",
                Nombre = "",
            });

            Assert.IsType<BadRequestObjectResult>(result);
        }

        // test: borrar genero existente retorna ok

        [Fact]
        public void DeleteGenre_Executes_ReturnsOk()
        {
            var result = _controller.DeleteGeneroById(4);

            Assert.IsType<OkObjectResult>(result);
        }

        // test: borrar genero inexistente retorna not found

        [Fact]
        public void DeleteNullGenre_Executes_ReturnsNotFound()
        {
            var result = _controller.DeleteGeneroById(99);

            Assert.IsType<NotFoundObjectResult>(result);
        }
        // test: borrar personaje existente retorna bad request, porque esta en uso por alguna pelicula

        [Fact]
        public void DeleteGenre_Executes_ReturnsBadRequest()
        {
            var result = _controller.DeleteGeneroById(1);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void UpdateGenre_Execute_ReturnsOk()
        {
            var result = _controller.UpdateGeneroById(1, new GenreDtoIn
            {
                Imagen = "imagen",
                Nombre = "nombre",
            });

            Assert.IsType<OkObjectResult>(result);
        }

        // actualizar genero inexistente devuelve not found

        [Fact]
        public void UpdateNullGenre_Execute_ReturnsNotFound()
        {
            var result = _controller.UpdateGeneroById(99, new GenreDtoIn
            {
                Imagen = "imagen",
                Nombre = "nombre",
            });

            Assert.IsType<NotFoundObjectResult>(result);
        }

        // actualizar personaje existente devuelve bad request porque la imagen es nula

        [Fact]
        public void UpdateCharacterNullImagen_Execute_ReturnsBadRequest()
        {
            var result = _controller.UpdateGeneroById(1, new GenreDtoIn
            {
                Imagen = "",
                Nombre = "nombre",
            });

            Assert.IsType<BadRequestObjectResult>(result);
        }

        // actualizar genero existente devuelve bad request porque el nombre es nulo

        [Fact]
        public void UpdateCharacterNullNombre_Execute_ReturnsBadRequest()
        {
            var result = _controller.UpdateGeneroById(1, new GenreDtoIn
            {
                Imagen = "imagen",
                Nombre = "",
            });

            Assert.IsType<BadRequestObjectResult>(result);
        }
    }    

}
