using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationManagerApp.Data.Models;
using VacationManagerApp.ViewModels.Teams;

namespace VacationManagerApp.Services.Contracts
{
    public interface ITeamService
    {
        public Task<IndexTeamsViewModel> GetTeamsAsync(IndexTeamsViewModel model);

        public Task<int> GetTeamsCountAsync();

        public Task<DetailsTeamViewModel> GetTeamDetails(string id);

        public Task<AddUserToTeamViewModel> GetUserToAddAsync(string id);
        public Task<string> AddUserToTeamAsync(AddUserToTeamViewModel model);
        public Task<string> CreateTeamAsync(CreateTeamViewModel model);

        public Task<EditTeamViewModel> GetTeamToEditAsync(string id);

        public Task<Team> GetTeamByIdAsync(string id);

        public Task<bool> EditTeamAsync(EditTeamViewModel model);

        public Task<int> DeleteTeamAsync(string id);
        public Task<string> RemoveUserOfTeamAsync(AddUserToTeamViewModel model);
        public Task<DeleteUserViewModel> GetUserToRemoveAsync(string id);
        public Task<string> AssignUserToTeamAsync(AddUserToTeamViewModel model);
        public Task<AssignLeaderViewModel> GetUserToAssignAsync(string id);
    }
}
