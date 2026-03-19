namespace CinemaApp.Web.Infrastructure.Utilities
{
    using Contracts;

    public class SlugGenerator : ISlugGenerator
    {
        public string GenerateSlug(string input)
        {
            string[] inputDataSplit = input
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.ToLowerInvariant())
                .ToArray();

            return string.Join("-", inputDataSplit);
        }
    }
}
