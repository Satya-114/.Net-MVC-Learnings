using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MovieStoreUnitTestProject.TestingModels
{
    [TestClass]
    public class MovieStoreTest
    {
        [TestMethod]
        public void Add_ValidMovie_ReturnsRedirectToAction()
        {
            // Arrange        
            
            var movieServiceMock = new Mock<IMovieService>();

            var controller = new MovieController(movieServiceMock.Object);

            var validMovie = new Movie
            {
                // Set properties of a valid movie for testing
            };

            // Setup mock behavior for dependencies
            genServiceMock.Setup(g => g.List()).Returns(new List<Genre>());
            fileServiceMock.Setup(f => f.SaveImage(It.IsAny<IFormFile>())).Returns((1, "dummyImageName"));
            movieServiceMock.Setup(m => m.Add(validMovie)).Returns(true);

            // Act
            var result = controller.Add(validMovie) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(nameof(MovieController.Add), result.ActionName);
        }


    }
}
