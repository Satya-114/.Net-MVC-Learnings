using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using MovieStoreApp.Controllers;
using MovieStoreApp.Models.Domain;
using MovieStoreApp.Repositories.Abstract;

namespace MovieStoreTestProject
{
    
    public class MovieStoreTest
    {
        private readonly MovieController controller;
        private readonly Mock<IFileService> _fileServiceMock = new Mock<IFileService>();
        private readonly Mock<IMovieService> _movieServiceMock = new Mock<IMovieService>();
        private readonly Mock<IGenreService> _genServiceMock = new Mock<IGenreService>();
        private readonly Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();


        public MovieStoreTest() 
        {
            controller = new MovieController(_movieServiceMock.Object,_fileServiceMock.Object,_genServiceMock.Object);

            var mockTempData = new TempDataDictionary(mockHttpContext.Object, Mock.Of<ITempDataProvider>());
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext.Object
            };
            controller.TempData = mockTempData;
        }

        
        [Fact]
        public void Add_ValidMovie_ReturnsRedirectToAction()
        {
            // Arrange          


            var validMovie = GetMovie();
           
            _genServiceMock.Setup(g => g.List()).Returns(new List<Genre>());
            _fileServiceMock.Setup(f => f.SaveImage(It.IsAny<IFormFile>())).Returns(new Tuple<int,string>(1, "dummyImageName"));
            _movieServiceMock.Setup(m => m.Add(validMovie)).Returns(true);

            // Act
            var result = controller.Add(validMovie) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(nameof(MovieController.Add), result.ActionName);
        }
        public Movie GetMovie()
        {
            return new Movie
            {
                Id = 1,
                Title = "Title",
                ReleaseYear = new DateTime().ToString(),
                MovieImage = "fgjvb.jpg",
                Cast = "edd",
                Director = "feewd",
                ImageFile = null,
                Genres = null,
                GenreList = null,
                GenreNames = "dfejs",
            };
        }

        [Fact]
        public void Edit_ReturnsCorrectViewAndModel()
        {
            // Arrange          

            int movieId = 1;
            var expectedModel = GetMovie();
            var expectedGenres = new List<int> { 1,2,3,4,5,6 };

            // Mocking _movieService
            _movieServiceMock.Setup(m => m.GetById(movieId)).Returns(expectedModel);
            _movieServiceMock.Setup(m => m.GetGenreByMovieId(movieId)).Returns(expectedGenres);

            // Mocking _genService
            _genServiceMock.Setup(g => g.List()).Returns(new List<Genre> { new Genre { Id = 1, GenreName = "dsdgsd" },new Genre { Id =2,GenreName ="sfrw"} }) ;


            // Act
            var result = controller.Edit(movieId) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedModel, result.Model); 

            var model = result.Model as Movie;
            Assert.NotNull(model);
            Assert.Equal(expectedModel.Id, model.Id);

            // Verify that GetGenreByMovieId was called with the correct movieId
            _movieServiceMock.Verify(m => m.GetGenreByMovieId(movieId), Times.Once);

            // Verify that List method of _genService was called
            _genServiceMock.Verify(g => g.List(), Times.Once);
        }

        [Fact]
        public void Edit_InvalidModelState_ReturnsViewResult()
        {
            // Arrange
            var model = GetMovie();
            controller.ModelState.AddModelError("PropertyName", "Error Message");
            _movieServiceMock.Setup(m => m.GetGenreByMovieId(It.IsAny<int>())).Returns(new List<int>());
            _genServiceMock.Setup(g => g.List()).Returns(new List<Genre>());
            _fileServiceMock.Setup(f => f.SaveImage(It.IsAny<IFormFile>())).Returns(Tuple.Create(1, "FileName"));
            _movieServiceMock.Setup(m => m.Update(It.IsAny<Movie>())).Returns(false);

            // Act
            var result = controller.Edit(model);

            // Assert
            Assert.IsType<ViewResult>(result);
            _movieServiceMock.Verify(m => m.Update(It.IsAny<Movie>()), Times.Never); // Ensure Update is not called
        }

        [Fact]
        public void Edit_ValidModelState_ReturnsRedirectToAction()
        {
            // Arrange           

            var model = GetMovie();
            _movieServiceMock.Setup(m => m.GetGenreByMovieId(It.IsAny<int>())).Returns(new List<int>());
            _genServiceMock.Setup(g => g.List()).Returns(new List<Genre>());
            _fileServiceMock.Setup(f => f.SaveImage(It.IsAny<IFormFile>())).Returns(Tuple.Create(1, "FileName"));
            _movieServiceMock.Setup(m => m.Update(It.IsAny<Movie>())).Returns(true);

            // Act
            var result = controller.Edit(model);

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(MovieController.MovieList), ((RedirectToActionResult)result).ActionName);
            _movieServiceMock.Verify(m => m.Update(It.IsAny<Movie>()), Times.Once); 
        }
        [Fact]
        public void Delete_ReturnsRedirectToAction_WhenDeleteIsSuccessful()
        {
            // Arrange
            int movieIdToDelete = 1;
            _movieServiceMock.Setup(x => x.Delete(movieIdToDelete)).Returns(true); 

            // Act
            IActionResult result = controller.Delete(movieIdToDelete);

            // Assert
            Assert.Equal(typeof(RedirectToActionResult), result.GetType());
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.Equal(nameof(MovieController.MovieList), redirectToActionResult.ActionName);
        }
        
    }
}