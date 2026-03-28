namespace CinemaApp.IntegrationTests.ControllerTests
{
    using System.Net;
    using System.Net.Http.Json;

    using Data.Models;
    using Web;
    using static GCommon.ApplicationConstants;

    [TestFixture]
    public class CinemaControllerIntegrationTests
    {
        // TODO: Use myTestedAspNet instead of plain Integration Testing
        // TODO: Fix bugs related to found defect
        // Hint: UseStatusCodePagesWithReExecute()
        private readonly CinemaAppWebApplicationFactory<Program> cinemaAppFactory;
        private readonly HttpClient cinemaAppClient;

        public CinemaControllerIntegrationTests()
        {
            cinemaAppFactory = new CinemaAppWebApplicationFactory<Program>();
            cinemaAppClient = cinemaAppFactory.CreateClient();
        }

        [Test]
        public async Task Index_AnonymousUser_FullRoute_ReturnsCorrectHttpCodeAndResultType()
        {
            HttpResponseMessage httpResponse = await cinemaAppClient
                .GetAsync("Cinema/Index");
            string responseContent = await httpResponse.Content.ReadAsStringAsync();

            httpResponse.EnsureSuccessStatusCode();
            Assert.That(httpResponse.Content.Headers.ContentType!.MediaType, Is.EqualTo("text/html"));

            // Check if View contains Cinema data correctly
            // Use libraries for UI validation
            Assert.That(responseContent.Contains("86e9d655-4bec-4685-b42f-40f93efedda2"));
        }

        [Test]
        public async Task Index_AnonymousUser_DefaultAction_ReturnsCorrectHttpCodeAndResultType()
        {
            HttpResponseMessage httpResponse = await cinemaAppClient
                .GetAsync("Cinema");
            string responseContent = await httpResponse.Content.ReadAsStringAsync();

            httpResponse.EnsureSuccessStatusCode();
            Assert.That(httpResponse.Content.Headers.ContentType!.MediaType, Is.EqualTo("text/html"));

            // Check if View contains Cinema data correctly
            // Use libraries for UI validation
            Assert.That(responseContent.Contains("86e9d655-4bec-4685-b42f-40f93efedda2"));
        }

        [Test]
        public async Task Index_AnonymousUser_IndexPost_ReturnsNotSupported()
        {
            // If requirement specifies that return code shall be 405, then we have a bug
            // If requirement specified redirection with Found, then we are up to spec
            HttpResponseMessage httpResponse = await cinemaAppClient
                .PostAsync("/Cinema/Index", JsonContent.Create("{}"));

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.MethodNotAllowed));
        }

        [Test]
        public async Task Details_AnonymousUser_ExistingCinema_ReturnsCorrectHttpCodeAndResultType()
        {
            // TODO: Implement seeding in factory and use data from there
            Cinema existingCinema = new Cinema
            {
                Id = Guid.Parse("86e9d655-4bec-4685-b42f-40f93efedda2"),
                Name = "Grand Cinema",
                Location = "Downtown",
                IsDeleted = false
            };
            List<Movie> moviesInCinemaProgram = new List<Movie>()
            {
                new Movie()
                {
                    Id = Guid.Parse("ae50a5ab-9642-466f-b528-3cc61071bb4c"),
                    Title = "Harry Potter and the Goblet of Fire",
                    Genre = "Fantasy",
                    ReleaseDate = DateOnly.FromDateTime(new DateTime(2005, 11, 01)),
                    Director = "Mike Newel",
                    Duration = 157,
                    Description =
                        "Harry Potter and the Goblet of Fire is a 2005 fantasy film directed by Mike Newell from a screenplay by Steve Kloves. It is based on the 2000 novel Harry Potter and the Goblet of Fire by J. K. Rowling.",
                    ImageUrl =
                        "https://m.media-amazon.com/images/M/MV5BMTI1NDMyMjExOF5BMl5BanBnXkFtZTcwOTc4MjQzMQ@@._V1_.jpg"
                },
                new Movie()
                {
                    Id = Guid.Parse("777634e2-3bb6-4748-8e91-7a10b70c78ac"),
                    Title = "Lord of the Rings",
                    Genre = "Fantasy",
                    ReleaseDate = DateOnly.FromDateTime(new DateTime(2001, 05, 01)),
                    Director = "Peter Jackson",
                    Duration = 178,
                    Description =
                        "The Lord of the Rings: The Fellowship of the Ring is a 2001 epic high fantasy adventure film directed by Peter Jackson from a screenplay by Fran Walsh, Philippa Boyens, and Jackson, based on 1954's The Fellowship of the Ring, the first volume of the novel The Lord of the Rings by J. R. R. Tolkien.",
                    ImageUrl =
                        "https://m.media-amazon.com/images/M/MV5BNzIxMDQ2YTctNDY4MC00ZTRhLTk4ODQtMTVlOWY4NTdiYmMwXkEyXkFqcGc@._V1_FMjpg_UX1000_.jpg"
                }
            };

            HttpResponseMessage httpResponse = await cinemaAppClient
                .GetAsync($"Cinema/Program/{existingCinema.Id}");
            string responseContent = await httpResponse.Content.ReadAsStringAsync();

            httpResponse.EnsureSuccessStatusCode();
            Assert.That(httpResponse.Content.Headers.ContentType!.MediaType, Is.EqualTo("text/html"));

            // Check if View contains Cinema data correctly
            foreach (Movie movie in moviesInCinemaProgram)
            {
                Assert.That(responseContent.Contains(movie.Title));
                Assert.That(responseContent.Contains(movie.Director));

                if (movie.ImageUrl != null)
                {
                    Assert.That(responseContent.Contains(movie.ImageUrl));
                }
                else
                {
                    Assert.That(responseContent.Contains(DefaultImageUrl));
                }
            }
        }

        [Test]
        public async Task Details_AnonymousUser_NonExistingCinema_ReturnsNotFound()
        {
            HttpResponseMessage httpResponse = await cinemaAppClient
                .GetAsync($"Cinema/Program/{Guid.NewGuid()}");

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task Details_AnonymousUser_InvalidId_ReturnsBadRequest()
        {
            HttpResponseMessage httpResponse = await cinemaAppClient
                .GetAsync($"Cinema/Program/InvalidGuid");

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
    }
}
