namespace CinemaApp.Services.Tests
{
    using Microsoft.Extensions.Logging;
    using MockQueryable;
    using Moq;

    using Core;
    using Core.Contracts;
    using Core.Data.Movie.Response;
    using Data.Models;
    using Data.Repository.Contracts;

    [TestFixture]
    public class MovieServiceTests
    {
        private Mock<ILogger<MovieService>> loggerMock = null!;
        private Mock<IMovieRepository> movieRepositoryMock = null!;

        [SetUp]
        public void SetUp()
        {
            this.loggerMock = new Mock<ILogger<MovieService>>();
            this.movieRepositoryMock = new Mock<IMovieRepository>();
        }

        [Test]
        public void Test_Positive()
        {
            Assert.Pass();
        }

        [Test]
        [Property("Requirement", "REQ-MS-GETALL-01")]
        [Property("Requirement", "REQ-MS-GETALL-02")]
        [Property("Requirement", "REQ-MS-GETALL-03")]
        public async Task GetAllMoviesAsync_ReturnsMoviesFromRepositoryWithMappedFields_Positive()
        {
            /* Arrange */
            List<Movie> mockMovies = new List<Movie>()
            {
                new Movie()
                {
                    Id = new Guid("25163666-81d7-4829-9931-b6bd44eb2b23"),
                    Description =
                        "An insomniac office worker and a devil-may-care soapmaker form an underground fight club that evolves into something much more.",
                    Director = "David Fincher",
                    Duration = 139,
                    Genre = "Sci-Fi",
                    ImageUrl =
                        "https://www.movieposters.com/cdn/shop/files/FightClub.mpw.125100_480x.progressive.jpg?v=1707497058",
                    IsDeleted = false,
                    ReleaseDate = new DateTime(1999, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    Title = "Fight Club"
                },
                new Movie()
                {
                    Id = new Guid("25163666-81d7-4829-9931-b6bd44eb2b25"),
                    Description =
                        "An insomniac office worker and a devil-may-care soapmaker form an underground fight club that evolves into something much more.",
                    Director = "David Fincher",
                    Duration = 139,
                    Genre = "Drama",
                    ImageUrl =
                        "https://www.movieposters.com/cdn/shop/files/FightClub.mpw.125100_480x.progressive.jpg?v=1707497058",
                    IsDeleted = false,
                    ReleaseDate = new DateTime(1999, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    Title = "Fight Club"
                },
                new Movie()
                {
                    Id = new Guid("25163666-81d7-4829-9931-b6bd44eb2b27"),
                    Description =
                        "An insomniac office worker and a devil-may-care soapmaker form an underground fight club that evolves into something much more.",
                    Director = "David Fincher",
                    Duration = 139,
                    Genre = "Action",
                    ImageUrl = null,
                    IsDeleted = false,
                    ReleaseDate = new DateTime(1999, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    Title = "Gladiator"
                },
            };
            IQueryable<Movie> moviesQueryableMock = mockMovies.BuildMock();

            movieRepositoryMock.Setup(m => m.AllAsNoTracking())
                .Returns(moviesQueryableMock);

            IMovieService movieService = new MovieService(movieRepositoryMock.Object, loggerMock.Object);
            IEnumerable<GetAllMoviesResponseModel> expectedResult = mockMovies
                .OrderBy(m => m.Title)
                .ThenBy(m => m.Genre)
                .Select(m => new GetAllMoviesResponseModel()
                {
                    Id = m.Id,
                    Title = m.Title,
                    Genre = m.Genre,
                    Director = m.Director,
                    ReleaseDate = m.ReleaseDate,
                    ImageUrl = m.ImageUrl
                })
                .ToArray();

            /* Act */
            IEnumerable<GetAllMoviesResponseModel> actualResult = await movieService.GetAllMoviesAsync();

            /* Assert */
            for (int i = 0; i < expectedResult.Count(); i++)
            {
                Assert.Multiple(() =>
                {
                    Assert.That(actualResult.ElementAt(i).Id, Is.EqualTo(expectedResult.ElementAt(i).Id));
                    Assert.That(actualResult.ElementAt(i).Title, Is.EqualTo(expectedResult.ElementAt(i).Title));
                    Assert.That(actualResult.ElementAt(i).Genre, Is.EqualTo(expectedResult.ElementAt(i).Genre));
                    Assert.That(actualResult.ElementAt(i).Director, Is.EqualTo(expectedResult.ElementAt(i).Director));
                    Assert.That(actualResult.ElementAt(i).ReleaseDate,
                        Is.EqualTo(expectedResult.ElementAt(i).ReleaseDate));
                    Assert.That(actualResult.ElementAt(i).ImageUrl, Is.EqualTo(expectedResult.ElementAt(i).ImageUrl));
                });
            }
        }

        [Test]
        [Property("Requirement", "REQ-MS-GETALL-04")]
        public async Task GetAllMoviesAsync_ReturnsEmptyCollectionWhenNoMoviesInRepository_Positive()
        {
            /* Arrange */
            List<Movie> mockMovies = new List<Movie>();
            IQueryable<Movie> emptyMoviesQueryableMock = mockMovies.BuildMock();

            movieRepositoryMock.Setup(m => m.AllAsNoTracking())
                .Returns(emptyMoviesQueryableMock);

            IMovieService movieService = new MovieService(movieRepositoryMock.Object, loggerMock.Object);
            
            /* Act */
            IEnumerable<GetAllMoviesResponseModel> actualResult = await movieService.GetAllMoviesAsync();

            /* Assert */
            Assert.NotNull(actualResult);
            Assert.Zero(actualResult.Count());
        }
    }
}
