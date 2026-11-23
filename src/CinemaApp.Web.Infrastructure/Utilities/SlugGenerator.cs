namespace CinemaApp.Web.Infrastructure.Utilities
{
    using Contracts;

    public class SlugGenerator : ISlugGenerator
    {
        public string GenerateEntitySlug(string name)
        {
            string[] nameParts = name
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(np => np.ToLowerInvariant())
                .ToArray();

            return string.Join("-", nameParts);
        }
    }
}
