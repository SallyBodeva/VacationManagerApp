using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationManagerApp.ViewModels.Roles;

namespace VacationManagerApp.Services.Contracts
{
    public interface IRoleService
    {
        public Task<IndexRolesViewModel> GetRolesAsync(IndexRolesViewModel model);
        public Task<int> GetRolesCountAsync();
        public Task<int> CreateRole(CreateRoleViewModel model);
    }
}
