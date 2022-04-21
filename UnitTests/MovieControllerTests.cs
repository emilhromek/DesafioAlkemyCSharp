using Microsoft.AspNetCore.Mvc;
using System;
using DesafioAlkemyCSharp.Controllers;
using DesafioAlkemyCSharp.DTOs;
using DesafioAlkemyCSharp.Services;
using Xunit;

namespace UnitTests
{
    public class MoviesControllerTests
    {       
        private readonly IMovieService _service;
        private readonly MoviesController _controller;        

        public MoviesControllerTests()
        {
            _service = new MovieServiceFake();
            _controller = new MoviesController(_service);
        }

        // test: al leer la lista devuelve un tipo Json

        [Fact]
        public void ReadList_Execute_ReturnsJsonResult()
        {
            var result = _controller.ReadMovies("juan", "3", "ASC");

            Assert.IsType<JsonResult>(result);
        }

        // test: al crear una pelicula/serie valida devuelve ok

        [Fact]
        public void CreateMovie_Execute_ReturnsOk()
        {
            var result = _controller.CreateMovie(new MovieDtoIn
            {
                Imagen = "url_fake",
                Titulo = "Nuevo",
                FechaCreacion = DateTime.Now,
                Calificacion = 5,
                GenreId = 1,
            });

            Assert.IsType<OkObjectResult>(result);
        }

        // test: al crear una serie/pelicula con imagen nula, devuelve bad request

        [Fact]
        public void CreateMovieNullImagen_Execute_ReturnsBadRequest()
        {
            var result = _controller.CreateMovie(new MovieDtoIn
            {
                Imagen = "",
                Titulo = "Nuevo",
                FechaCreacion = DateTime.Now,
                Calificacion = 5,
                GenreId = 1,
            });

            Assert.IsType<BadRequestObjectResult>(result);
        }

        // test: al crear una serie/pelicula con titulo nulo, devuelve bad request

        [Fact]
        public void CreateMovieNullTitulo_Execute_ReturnsBadRequest()
        {
            var result = _controller.CreateMovie(new MovieDtoIn
            {
                Imagen = "imagen",
                Titulo = "",
                FechaCreacion = DateTime.Now,
                Calificacion = 5,
                GenreId = 1,
            });

            Assert.IsType<BadRequestObjectResult>(result);
        }

        // test: al crear una serie/pelicula con un genero inexiste, devuelve not found

        [Fact]
        public void CreateNullGenre_Execute_ReturnsNotFound()
        {
            var result = _controller.CreateMovie(new MovieDtoIn
            {
                Imagen = "imagen",
                Titulo = "Nuevo",
                FechaCreacion = DateTime.Now,
                Calificacion = 5,
                GenreId = 999,
            });

            Assert.IsType<NotFoundObjectResult>(result);
        }

        // test: al borrar un serie/pelicula que existe, devuelve ok

        [Fact]
        public void DeleteMovie_Executes_ReturnsOk()
        {
            var result = _controller.DeleteMovie(1);

            Assert.IsType<OkObjectResult>(result);
        }

        // test: al borrar un serie/pelicula que no existe, devuelve not found

        [Fact]
        public void DeleteNullMovie_Executes_ReturnsNotFound()
        {
            var result = _controller.DeleteMovie(99);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        // test: al asociarle una pelicula/serie a un personaje
        // devuelve bad request si ya existe la asociacion

        [Fact]
        public void AssociateCharacterToMovie_Executes_ReturnsBadRequest()
        {
            var result = _controller.AddCharacterToMovie(1, 1);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        // test: al asociarle una pelicula/serie a un personaje
        // devuelve ok si no existe la asociacion

        [Fact]
        public void AssociateCharacterToMovie_Executes_ReturnsOk()
        {
            var result = _controller.AddCharacterToMovie(2, 2);

            Assert.IsType<OkObjectResult>(result);
        }

        // test: al intentar asociar una pelicula/serie inexistente a un
        // personaje que tampoco existe, devuelve not found

        [Fact]
        public void AssociateNullMovieToNullCharacter_Executes_ReturnsNotFound()
        {
            var result = _controller.AddCharacterToMovie(99, 99);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        // test: al intentar asociar una pelicula/serie inexistente a un
        // personaje que si existe, devuelve not found

        [Fact]
        public void AssociateNullMovieToCharacter_Executes_ReturnsNotFound()
        {
            var result = _controller.AddCharacterToMovie(99, 1);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        // test: al intentar asociar una pelicula/serie que existe a un
        // personaje que no existe, devuelve not found

        [Fact]
        public void AssociateMovieToNullCharacter_Executes_ReturnsNotFound()
        {
            var result = _controller.AddCharacterToMovie(1, 99);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        // test: al intentar desasociar una pelicula/serie de un personaje
        // devuelve ok si esa asociacion existe

        [Fact]
        public void DisassociateMovieFromCharacter_Executes_ReturnsOk()
        {
            var result = _controller.RemoveCharacterFromMovie(1, 1);

            Assert.IsType<OkObjectResult>(result);
        }

        // test: al intentar desasociar una pelicula/serie de un personaje
        // devuelve bad request si esa asociacion es inexistente

        [Fact]
        public void RemoveNullAssociationBetweenMovieAndCharacter_Executes_ReturnsBadRequest()
        {
            var result = _controller.RemoveCharacterFromMovie(3, 3);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        // test: actualizar pelicula devuelve ok

        [Fact]
        public void UpdateMovie_Execute_ReturnsOk()
        {
            var result = _controller.UpdateMovieById(1, new MovieDtoIn
            {
                Imagen = "url_fake_update",
                Titulo = "Nuevo_update",
                Calificacion = 4,
                FechaCreacion = DateTime.Now,
                GenreId = 2,
            });

            Assert.IsType<OkObjectResult>(result);
        }

        // actualizar pelicula inexistente devuelve not found

        [Fact]
        public void UpdateNullMovie_Execute_ReturnsNotFound()
        {
            var result = _controller.UpdateMovieById(99, new MovieDtoIn
            {
                Imagen = "url_fake_update",
                Titulo = "Nuevo_update",
                Calificacion = 4,
                FechaCreacion = DateTime.Now,
                GenreId = 2,
            });

            Assert.IsType<NotFoundObjectResult>(result);
        }

        // actualizar pelicula existente devuelve bad request porque la imagen es nula

        [Fact]
        public void UpdateMovieNullImagen_Execute_ReturnsBadRequest()
        {
            var result = _controller.UpdateMovieById(1, new MovieDtoIn
            {
                Imagen = "",
                Titulo = "Nuevo_update",
                Calificacion = 4,
                FechaCreacion = DateTime.Now,
                GenreId = 2,
            });

            Assert.IsType<BadRequestObjectResult>(result);
        }

        // actualizar pelicula existente devuelve bad request porque el nombre es nulo

        [Fact]
        public void UpdateMovieNullTitulo_Execute_ReturnsBadRequest()
        {
            var result = _controller.UpdateMovieById(1, new MovieDtoIn
            {
                Imagen = "url_fake_update",
                Titulo = "",
                Calificacion = 4,
                FechaCreacion = DateTime.Now,
                GenreId = 2,
            });

            Assert.IsType<BadRequestObjectResult>(result);
        }

        // actualizar pelicula existente devuelve not found porque el genero no existe

        [Fact]
        public void UpdateMovieNullGenre_Execute_ReturnsNotFound()
        {
            var result = _controller.UpdateMovieById(1, new MovieDtoIn
            {
                Imagen = "url_fake_update",
                Titulo = "titulo_update",
                Calificacion = 4,
                FechaCreacion = DateTime.Now,
                GenreId = 999,
            });

            Assert.IsType<NotFoundObjectResult>(result);
        }

    }    

}
