namespace CinemaApp.Web.Infrastructure.Utilities.Contracts
{
    public interface ISlugGenerator
    {
        string GenerateEntitySlug(string name);
    }
}
