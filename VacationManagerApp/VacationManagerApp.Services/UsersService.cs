using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationManagerApp.Data.Models;
using VacationManagerApp.Data;
using VacationManagerApp.Services.Contracts;
using VacationManagerApp.ViewModels.Users;
using VacationManagerApp.Common;
using Microsoft.EntityFrameworkCore;

namespace VacationManagerApp.Services
{
    public class UsersService : IUserService
    {
        private readonly UserManager<User> userManager;
        private readonly ApplicationDbContext context;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<User> signInManager;
        private const int ItemsCount = 0;

        public UsersService(UserManager<User> userManager, ApplicationDbContext context, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager)
        {
            this.userManager = userManager;
            this.context = context;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
        }


        public async Task<string> AddUserToTeamAsync(AddToTeamViewModel model)
        {
            User? u = await GetUserByIdAsync(model.UserId);
            Team? t = await context.Teams.FirstOrDefaultAsync(x => x.Name == model.TeamName);
            if (t != null && u != null)
            {
                t.Developers.Add(u);
                u.Role = GlobalConstants.Developer;
                await userManager.AddToRoleAsync(u, GlobalConstants.Developer);
                await context.SaveChangesAsync();
            }
            await userManager.UpdateAsync(u);
            return u.Id;
        }

        public async Task<string> CreateUserAsync(CreateUserViewModel model)
        {
            User user = new User()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.Email
            };

            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                if (userManager.Users.Count() <= 1)
                {
                    IdentityRole roleUnassign = new IdentityRole() { Name = GlobalConstants.Unassigned };
                    IdentityRole roleAdmin = new IdentityRole() { Name = GlobalConstants.AdminRole };
                    IdentityRole roleDeveloper = new IdentityRole() { Name = GlobalConstants.Developer };
                    IdentityRole roleTeamLeader = new IdentityRole() { Name = GlobalConstants.TeamLeader };
                    await roleManager.CreateAsync(roleUnassign);
                    await roleManager.CreateAsync(roleAdmin);
                    await roleManager.CreateAsync(roleDeveloper);
                    await roleManager.CreateAsync(roleTeamLeader);
                    await userManager.AddToRoleAsync(user, GlobalConstants.AdminRole);
                    user.Role = GlobalConstants.AdminRole;
                }
                else
                {
                    await userManager.AddToRoleAsync(user, GlobalConstants.Unassigned);
                    user.Role = GlobalConstants.Unassigned;
                }
                await userManager.UpdateAsync(user);
            }
            return user.Id;
        }
     

        public async Task<bool> DeleteUserAsync(string id)
        {
            User? user = await GetUserByIdAsync(id);
            if (user != null)
            {
                var result = await userManager.DeleteAsync(user);
                return result.Succeeded;
            }
            return false;
        }

        public async Task<DetailsUserViewModel?> GetUserDetailsAsync(string id)
        {
            DetailsUserViewModel? result = null;

            User user = await GetUserByIdAsync(id);

            if (user != null)
            {
                string roles = string.Join(", ", await userManager.GetRolesAsync(user));

                result = new DetailsUserViewModel()
                {
                    Id = user.Id,
                    Name = $"{user.FirstName} {user.LastName}",
                    Email = user.Email != null ? user.Email : "n/a",
                    Role = user.Role
                };
                if (user.Team != null)
                {
                    result.Team = user.Team.Name;
                }
                else
                {
                    result.Team = "This user does not participate in any team...";
                }
            }
            return result;
        }

        public async Task<IndexUsersViewModel> GetUsersAsync(IndexUsersViewModel users)
        {
            if (users == null)
            {
                users = new IndexUsersViewModel(0);
            }
            users.ElementsCount = await GetUsersCountAsync();

            users.Users = await userManager
                .Users
                .Skip((users.Page - 1) * users.ItemsPerPage)
                .Take(users.ItemsPerPage)
                .Select(x => new IndexUserViewModel()
                {
                    Id = x.Id,
                    Name = $"{x.FirstName} {x.LastName}",
                    Role = x.Role
                })
                .ToListAsync();

            return users;
        }

        public async Task<int> GetUsersCountAsync()
        {
            return await userManager.Users.CountAsync();
        }

        public async Task<AddToTeamViewModel> GetUserToAddAsync(string id)
        {
            AddToTeamViewModel? result = null;

            User? user = await GetUserByIdAsync(id);
            List<string> teamName = context.Teams.Select(x => x.Name).ToList();

            if (user != null)
            {
                result = new AddToTeamViewModel()
                {
                    UserId = user.Id,
                    TeamNames = teamName
                };
            }

            return result;

        }

       

        public async Task<EditUserViewModel?> GetUserToEditAsync(string id)
        {
            EditUserViewModel? result = null;

            User? user = await GetUserByIdAsync(id);

            if (user != null)
            {
                result = new EditUserViewModel()
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                };
            }

            return result;
        }

        public async Task<SignInResult> Login(LoginViewModel model)
        {
            return await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
        }

        public async Task Logout()
        {
            await signInManager.SignOutAsync();
        }

        public async Task<string> UpdateUserAsync(EditUserViewModel user)
        {
            User? oldUser = await GetUserByIdAsync(user.Id);

            if (oldUser != null)
            {
                oldUser.FirstName = user.FirstName;
                oldUser.LastName = user.LastName;
                await userManager.UpdateAsync(oldUser);
            }

            return user.Id;
        }
        private async Task<User> GetUserByIdAsync(string id)
        {
            return await userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}