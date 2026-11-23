namespace CinemaApp.Web.Areas.Admin.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    using static GCommon.InputValidationConstants.UserRole;

    public class UserRoleInputModel
    {
        [Required]
        [MaxLength(IdentityUserIdMaxLength)]
        public string UserId { get; set; } = null!;

        [Required]
        [MaxLength(IdentityRoleNameMaxLength)]
        public string RoleName { get; set; } = null!;
    }
}
