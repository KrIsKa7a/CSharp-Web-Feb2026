namespace CinemaApp.Services.Common.Utilities
{
    using System.ComponentModel.DataAnnotations;

    public class BasicModelValidator
    {
        private readonly IList<ValidationResult> validationResults;

        public BasicModelValidator()
        {
            this.validationResults = new List<ValidationResult>();
        }

        public IReadOnlyCollection<ValidationResult> ValidationResults
            => this.validationResults.AsReadOnly();

        public bool IsValid(object model)
        {
            var validationContext = new ValidationContext(model);

            return Validator
                .TryValidateObject(model, validationContext, validationResults, true);
        }

        public static bool IsValid(object model, List<ValidationResult> validationResults)
        {
            var validationContext = new ValidationContext(model);

            return Validator
                .TryValidateObject(model, validationContext, validationResults, true);
        }
    }
}
