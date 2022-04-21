using Microsoft.AspNetCore.Mvc;
using DesafioAlkemyCSharp.Controllers;
using DesafioAlkemyCSharp.DTOs;
using DesafioAlkemyCSharp.Services;
using Xunit;

namespace UnitTests
{
    public class CharactersControllerTests
    {       
        private readonly ICharacterService _service;
        private readonly CharactersController _controller;        

        public CharactersControllerTests()
        {
            _service = new CharacterServiceFake();
            _controller = new CharactersController(_service);
        }

        // test: al leer la lista devuelve un tipo Json

        [Fact]
        public void ReadList_Execute_ReturnsJsonResult()
        {
            var result = _controller.ReadCharacters("juan", "30", "1");

            Assert.IsType<JsonResult>(result);
        }

        // test: crear personaje retorna ok

        [Fact]
        public void CreateCharacter_Execute_ReturnsOk()
        {
            var result = _controller.CreateCharacter(new CharacterDtoIn
            {
                Imagen = "url_fake",
                Nombre = "Nuevo",
                Edad = 35,
                Peso = 77,
                Historia = "historia"
            });

            Assert.IsType<OkObjectResult>(result);
        }

        // test: crear personaje con imagen nula retorna bad request

        [Fact]
        public void CreateCharacterNullImagen_Execute_ReturnsBadRequest()
        {
            var result = _controller.CreateCharacter(new CharacterDtoIn
            {
                Imagen = "",
                Nombre = "Nuevo",
                Edad = 35,
                Peso = 77,
                Historia = "historia"
            });

            Assert.IsType<BadRequestObjectResult>(result);
        }

        // test: crear personaje con nombre nulo retorna bad request

        [Fact]
        public void CreateCharacterNullNombre_Execute_ReturnsBadRequest()
        {
            var result = _controller.CreateCharacter(new CharacterDtoIn
            {
                Imagen = "Imagen",
                Nombre = "",
                Edad = 35,
                Peso = 77,
                Historia = "historia"
            });

            Assert.IsType<BadRequestObjectResult>(result);
        }

        // test: crear personaje con historia nula retorna bad request

        [Fact]
        public void CreateCharacterNullHistoria_Execute_ReturnsBadRequest()
        {
            var result = _controller.CreateCharacter(new CharacterDtoIn
            {
                Imagen = "Imagen",
                Nombre = "Nombre",
                Edad = 35,
                Peso = 77,
                Historia = ""
            });

            Assert.IsType<BadRequestObjectResult>(result);
        }

        // test: borrar personaje existente retorna ok

        [Fact]
        public void DeleteCharacter_Executes_ReturnsOk()
        {
            var result = _controller.DeleteCharacterById(1);

            Assert.IsType<OkObjectResult>(result);
        }

        // test: borrar personaje inexistente retorna not found

        [Fact]
        public void DeleteNullCharacter_Executes_ReturnsNotFound()
        {
            var result = _controller.DeleteCharacterById(99);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        // test: asociar pelicula con personaje retorna ok

        [Fact]
        public void AssociateMovieToCharacter_Executes_ReturnsOk()
        {
            var result = _controller.AddMovieToCharacter(2, 2);

            Assert.IsType<OkObjectResult>(result);
        }

        // test: asociar pelicula con personaje retorna bad request
        // porque esa asociacion ya existe

        [Fact]
        public void AddMovieToCharacter_Executes_ReturnsBadRequest()
        {
            var result = _controller.AddMovieToCharacter(1, 1);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        // test: asociar pelicula con personaje retorna bad request
        // porque ambos son inexistentes

        [Fact]
        public void AddNotExistentMovieToNotExistentCharacter_Executes_ReturnsNotFound()
        {
            var result = _controller.AddMovieToCharacter(99, 99);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        // test: asociar pelicula con personaje retorna bad request
        // porque el personaje no existe

        [Fact]
        public void AddMovieToNullCharacter_Executes_ReturnsNotFound()
        {
            var result = _controller.AddMovieToCharacter(99, 1);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        // test: asociar pelicula con personaje retorna bad request
        // porque la pelicula no existe

        [Fact]
        public void AddNullMovieToExistentCharacter_Executes_ReturnsNotFound()
        {
            var result = _controller.AddMovieToCharacter(1, 99);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        // test: al intentar desasociar una pelicula/serie de un personaje
        // devuelve ok, porque la asociacion existe

        [Fact]
        public void DisassociateMovieFromCharacter_Executes_ReturnsOk()
        {
            var result = _controller.RemoveMovieFromCharacter(1, 1);

            Assert.IsType<OkObjectResult>(result);
        }

        // test: al intentar desasociar una pelicula/serie de un personaje
        // devuelve bad request si esa asociacion es inexistente

        [Fact]
        public void RemoveNullAssociationBetweenMovieAndCharacter_Executes_ReturnsNotFound()
        {
            var result = _controller.RemoveMovieFromCharacter(3, 3);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        // actualizar personaje existente devuelve ok

        [Fact]
        public void UpdateCharacter_Execute_ReturnsOk()
        {
            var result = _controller.UpdateCharacterById(1, new CharacterDtoIn
            {
                Imagen = "url_fake_update",
                Nombre = "Nuevo_update",
                Edad = 37,
                Peso = 78,
                Historia = "historia_update"
            });

            Assert.IsType<OkObjectResult>(result);
        }

        // actualizar personaje inexistente devuelve not

        [Fact]
        public void UpdateNullCharacter_Execute_ReturnsOk()
        {
            var result = _controller.UpdateCharacterById(99, new CharacterDtoIn
            {
                Imagen = "url_fake_update",
                Nombre = "Nuevo_update",
                Edad = 37,
                Peso = 78,
                Historia = "historia_update"
            });

            Assert.IsType<NotFoundObjectResult>(result);
        }

        // actualizar personaje existente devuelve bad request porque la imagen es nula

        [Fact]
        public void UpdateCharacterNullImagen_Execute_ReturnsBadRequest()
        {
            var result = _controller.UpdateCharacterById(1, new CharacterDtoIn
            {
                Imagen = "",
                Nombre = "Nuevo_update",
                Edad = 37,
                Peso = 78,
                Historia = "historia_update"
            });

            Assert.IsType<BadRequestObjectResult>(result);
        }

        // actualizar personaje existente devuelve bad request porque el nombre es nulo

        [Fact]
        public void UpdateCharacterNullNombre_Execute_ReturnsBadRequest()
        {
            var result = _controller.UpdateCharacterById(1, new CharacterDtoIn
            {
                Imagen = "url_fake_update",
                Nombre = "",
                Edad = 37,
                Peso = 78,
                Historia = "historia_update"
            });

            Assert.IsType<BadRequestObjectResult>(result);
        }

        // actualizar personaje existente devuelve bad request porque la historia es nula

        [Fact]
        public void UpdateCharacterNullHistoria_Execute_ReturnsBadRequest()
        {
            var result = _controller.UpdateCharacterById(1, new CharacterDtoIn
            {
                Imagen = "url_fake_update",
                Nombre = "nombre_update",
                Edad = 37,
                Peso = 78,
                Historia = ""
            });

            Assert.IsType<BadRequestObjectResult>(result);
        }

    }    

}
