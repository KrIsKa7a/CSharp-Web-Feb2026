namespace CinemaApp.Services.Tests
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Moq;

    using Data.Models;
    using Data.Repository.Contracts;
    using Core;
    using CinemaApp.Services.Core.Data.Movie.Enums;
    using CinemaApp.Services.Core.Data.Movie.Request;

    [TestFixture]
    public class MovieServiceCreateMovieTests
    {
        private Mock<IMovieRepository> movieRepositoryMock = null!;
        private Mock<ILogger<MovieService>> loggerMock = null!;
        private MovieService movieService = null!;

        [SetUp]
        public void SetUp()
        {
            this.movieRepositoryMock = new Mock<IMovieRepository>(MockBehavior.Strict);
            this.loggerMock = new Mock<ILogger<MovieService>>();
            this.movieService = new MovieService(this.movieRepositoryMock.Object, this.loggerMock.Object);
        }

        private static CreateMovieRequestData ValidRequest() => new CreateMovieRequestData
        {
            Title = "Inception",
            Genre = "Sci-Fi",
            ReleaseDate = new DateTime(2010, 7, 16),
            Duration = 120,
            Director = "Christopher Nolan",
            Description = "A thief who steals secrets through dreams.",
            ImageUrl = "https://example.com/inception.jpg"
        };

        [Test]
        [Property("Requirement", "REQ-MS-CREATE-01")]
        public async Task CreateMovieAsync_InvalidRequest_ReturnsInvalidRequestAndDoesNotPersist()
        {
            CreateMovieRequestData request = ValidRequest();
            request.Title = null!;

            var result = await this.movieService.CreateMovieAsync(request);

            Assert.That(result.Status, Is.EqualTo(MovieServiceResponseStatus.InvalidRequest));
            Assert.That(result.ErrorMessages, Is.Not.Empty);
            this.movieRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Movie>()), Times.Never);
            this.movieRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Test]
        [Property("Requirement", "REQ-MS-CREATE-02")]
        [Property("Requirement", "REQ-MS-CREATE-03")]
        [Property("Requirement", "REQ-MS-CREATE-04")]
        public async Task CreateMovieAsync_ValidRequest_CreatesMovieWithRequestDataAndReturnsSuccess()
        {
            CreateMovieRequestData request = ValidRequest();
            Movie? captured = null;
            this.movieRepositoryMock
                .Setup(r => r.AddAsync(It.IsAny<Movie>()))
                .Callback<Movie>(m => captured = m)
                .Returns(Task.CompletedTask);
            this.movieRepositoryMock
                .Setup(r => r.SaveChangesAsync())
                .ReturnsAsync(1);

            var result = await this.movieService.CreateMovieAsync(request);

            Assert.That(result.Status, Is.EqualTo(MovieServiceResponseStatus.Success));
            Assert.That(captured, Is.Not.Null);
            Assert.That(captured!.Title, Is.EqualTo(request.Title));
            Assert.That(captured.Genre, Is.EqualTo(request.Genre));
            Assert.That(captured.ReleaseDate, Is.EqualTo(request.ReleaseDate));
            Assert.That(captured.Duration, Is.EqualTo(request.Duration));
            Assert.That(captured.Director, Is.EqualTo(request.Director));
            Assert.That(captured.Description, Is.EqualTo(request.Description));
            Assert.That(captured.ImageUrl, Is.EqualTo(request.ImageUrl));
            this.movieRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Movie>()), Times.Once);
            this.movieRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        [Property("Requirement", "REQ-MS-CREATE-05")]
        public async Task CreateMovieAsync_AddAsyncThrowsDbUpdateException_ReturnsGeneralError()
        {
            this.movieRepositoryMock
                .Setup(r => r.AddAsync(It.IsAny<Movie>()))
                .ThrowsAsync(new DbUpdateException("add failed"));

            var result = await this.movieService.CreateMovieAsync(ValidRequest());

            Assert.That(result.Status, Is.EqualTo(MovieServiceResponseStatus.GeneralError));
            Assert.That(result.ErrorMessages, Is.EqualTo(new[] { "add failed" }));
        }

        [Test]
        [Property("Requirement", "REQ-MS-CREATE-05")]
        public async Task CreateMovieAsync_SaveChangesThrowsDbUpdateException_ReturnsGeneralError()
        {
            this.movieRepositoryMock
                .Setup(r => r.AddAsync(It.IsAny<Movie>()))
                .Returns(Task.CompletedTask);
            this.movieRepositoryMock
                .Setup(r => r.SaveChangesAsync())
                .ThrowsAsync(new DbUpdateException("save failed"));

            var result = await this.movieService.CreateMovieAsync(ValidRequest());

            Assert.That(result.Status, Is.EqualTo(MovieServiceResponseStatus.GeneralError));
            Assert.That(result.ErrorMessages, Is.EqualTo(new[] { "save failed" }));
        }

        [Test]
        [Property("Requirement", "REQ-MS-CREATE-06")]
        public void CreateMovieAsync_NonDbException_ThrowsException()
        {
            this.movieRepositoryMock
                .Setup(r => r.AddAsync(It.IsAny<Movie>()))
                .ThrowsAsync(new InvalidOperationException("boom"));

            Assert.ThrowsAsync<InvalidOperationException>(
                () => this.movieService.CreateMovieAsync(ValidRequest()));
        }
    }
}
