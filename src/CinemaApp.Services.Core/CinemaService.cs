namespace CinemaApp.Services.Core
{
    using Microsoft.EntityFrameworkCore;

    using CinemaApp.Data.Repository.Contracts;
    using Contracts;
    using Data.Cinema.Response;

    public class CinemaService : ICinemaService
    {
        private readonly ICinemaRepository cinemaRepository;

        public CinemaService(ICinemaRepository  cinemaRepository)
        {
            this.cinemaRepository = cinemaRepository;
        }

        public async Task<IEnumerable<GetAllCinemasResponseModel>> GetAllCinemasAsync()
        {
            IEnumerable<GetAllCinemasResponseModel> responseData = await cinemaRepository
                .AllAsNoTracking()
                .OrderBy(c => c.Name)
                .ThenBy(c => c.Location)
                .Select(c => new GetAllCinemasResponseModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                    Location = c.Location
                })
                .ToArrayAsync();

            return responseData;
        }
    }
}
