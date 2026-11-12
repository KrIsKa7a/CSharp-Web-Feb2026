namespace CinemaApp.Services.Core.Contracts
{
    using Data.Cinema.Response;

    public interface ICinemaService
    {
        Task<IEnumerable<GetAllCinemasResponseModel>> GetAllCinemasAsync();
    }
}
