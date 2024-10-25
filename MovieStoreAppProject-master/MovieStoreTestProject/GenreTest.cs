using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using MovieStoreApp.Controllers;
using MovieStoreApp.Models.Domain;
using MovieStoreApp.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieStoreTestProject
{
    public class GenreTest
    {
        private readonly GenreController genController;
        private readonly Mock<IGenreService> genreServiceMock= new Mock<IGenreService>();
        private readonly Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
        public GenreTest()
        {
            genController = new GenreController(genreServiceMock.Object);
            var mockTempData = new TempDataDictionary(mockHttpContext.Object, Mock.Of<ITempDataProvider>());
            genController.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext.Object
            };
            genController.TempData = mockTempData;
        }
        public static Genre GetGenre()
        {
            return new Genre { Id = 1, GenreName = "dfgtre" };
        }
        [Fact]
        public void AddValid_GenreModel_returnRedirectActionMethod()
        {
            //Arrange
            var genModel = GetGenre();

            genreServiceMock.Setup(s => s.Add(genModel)).Returns(true);
            //Act
            var result = genController.Add(genModel) as RedirectToActionResult;
            //Assert
            Assert.NotNull(result);
            Assert.Equal(nameof(GenreController.Add), result.ActionName);
        }
        [Fact]
        public void EditById_returnView()
        {
            //Arrange
            var genId = 1;
            var expectedModel = GetGenre();
            genreServiceMock.Setup(e => e.GetById(genId)).Returns(expectedModel);

            //Act
            var resp =genController.Edit(genId) as ViewResult;

            //Assert
            Assert.NotNull(resp);
            Assert.Equal(expectedModel, resp.Model);

            var geModel = resp.Model as Genre;
            Assert.NotNull(geModel);
            Assert.Equal(expectedModel.Id, geModel.Id);
        }

        [Fact]
        public void UpdateValidModel_returnRedirectActionMethod()
        {
            //Arrange
            var updateModel = GetGenre();
            genreServiceMock.Setup(u=>u.Update(updateModel)).Returns(true);

            //Act
            var response = genController.Update(updateModel);

            //Assert
            Assert.NotNull(response);
            Assert.IsType<RedirectToActionResult>(response);
            Assert.Equal(nameof(GenreController.GenreList), ((RedirectToActionResult)response).ActionName);

        }
        [Fact]
        public void UpdateInValidModel_returnView()
        {
            //Arrange
            var updateModel = GetGenre();
            genreServiceMock.Setup(u => u.Update(updateModel)).Returns(false);

            //Act
            var response = genController.Update(updateModel);

            //Assert
            Assert.NotNull(response);
            Assert.IsType<ViewResult>(response);           

        }
        [Fact]
        public void DeleteById_returnRedirectActionMethod()
        {
            //Arrange
            int deleteGenreById = 1;
            genreServiceMock.Setup(d=>d.Delete(deleteGenreById)).Returns(true);

            //Act
            var result = genController.Delete(deleteGenreById) as RedirectToActionResult;

            //Assert

            Assert.NotNull(result);
            Assert.Equal(nameof(GenreController.GenreList),result.ActionName);
        }
    }
}
