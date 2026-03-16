namespace CinemaApp.Services.Core
{
    using Contracts;
    using Data.Models;
    using Data.Repository.Contracts;
    using Models.ApplicationUser;

    using AutoMapper;

    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;

        private readonly IMapper mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<UserManageAllDto>> GetAllManageableUsersAsync(string adminUserId)
        {
            IEnumerable<ApplicationUser> appUsersNoCurrentUser = await userRepository
                .GetAllUsersAsync(filterQuery: u => u.Id.ToString() != adminUserId);
            
            IEnumerable<UserManageAllDto> userAllManageDtos = mapper
                .Map<IEnumerable<UserManageAllDto>>(appUsersNoCurrentUser);
            foreach (UserManageAllDto userDto in userAllManageDtos)
            {
                ApplicationUser appUser = appUsersNoCurrentUser
                    .First(u => u.Id.ToString() == userDto.Id.ToString());
                userDto.Roles = await userRepository
                    .GetUserRolesAsync(appUser);
            }

            return userAllManageDtos;
        }
    }
}
