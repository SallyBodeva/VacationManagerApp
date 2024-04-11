using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationManagerApp.Common;
using VacationManagerApp.Data;
using VacationManagerApp.Data.Models;
using VacationManagerApp.Services.Contracts;
using VacationManagerApp.ViewModels.Roles;
using VacationManagerApp.ViewModels.Teams;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VacationManagerApp.Services
{
    public class RoleService : IRoleService
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public RoleService(ApplicationDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task<IndexRolesViewModel> GetRolesAsync(IndexRolesViewModel model)
        {
            if (model == null)
            {
                model = new IndexRolesViewModel();
            }

            model.ElementsCount = await GetRolesCountAsync();

            var roles = await roleManager.Roles
                .Skip((model.Page - 1) * model.ItemsPerPage)
                .Take(model.ItemsPerPage)
                .ToListAsync();

            var roleViewModels = new List<IndexRoleViewModel>();

            foreach (var role in roles)
            {
                var membersCount = (await userManager.GetUsersInRoleAsync(role.Name)).Count();
                var roleViewModel = new IndexRoleViewModel
                {
                    Id = role.Id,
                    Name = role.Name,
                    MembersCount = membersCount
                };
                roleViewModels.Add(roleViewModel);
            }

            model.Roles = roleViewModels;

            return model;
        }

        public async Task<int> GetRolesCountAsync()
        {
            return roleManager.Roles.Count();
        }

        public async Task<int> CreateRole(CreateRoleViewModel model)
        {
            await roleManager.CreateAsync(new IdentityRole(model.NewRoleName));
            return await context.SaveChangesAsync();
        }
        public async Task<RolesMembersViewModel> Members(string id)
        {
            RolesMembersViewModel model = new RolesMembersViewModel();

            if (await roleManager.RoleExistsAsync(id))
            {
                var role = await roleManager.FindByIdAsync(id);
                model.RoleName = role.Name;
                model.Members = new List<User>(await userManager.GetUsersInRoleAsync(role.Name));
            }
            return model;
        }
    }
}
