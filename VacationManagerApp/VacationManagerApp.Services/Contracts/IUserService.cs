using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationManagerApp.ViewModels.Users;

namespace VacationManagerApp.Services.Contracts
{
    public interface IUserService
    {
        public Task<string> CreateUserAsync(CreateUserViewModel model);

        public Task<bool> DeleteUserAsync(string id);

        public Task<IndexUsersViewModel> GetUsersAsync(IndexUsersViewModel users);

        public Task<int> GetUsersCountAsync();

        public Task<DetailsUserViewModel?> GetUserDetailsAsync(string id);

        public Task<EditUserViewModel?> GetUserToEditAsync(string id);

        public Task<string> UpdateUserAsync(EditUserViewModel user);
        public Task<AddToTeamViewModel> GetUserToAddAsync(string id);

        public Task<string> AddUserToTeamAsync(AddToTeamViewModel model);


        public Task Logout();

        public Task<SignInResult> Login(LoginViewModel model);
    }
}
