using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationManagerApp.Data.Models;
using VacationManagerApp.Data;
using VacationManagerApp.Services.Contracts;
using VacationManagerApp.ViewModels.Teams;
using Microsoft.EntityFrameworkCore;
using VacationManagerApp.Common;
using VacationManagerApp.ViewModels.Users;

namespace VacationManagerApp.Services
{
    public class TeamsService : ITeamService
    {
        private ApplicationDbContext context;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<User> userManager;
        public TeamsService(ApplicationDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task<IndexTeamsViewModel> GetTeamsAsync(IndexTeamsViewModel model)
        {
            if (model == null)
            {
                model = new IndexTeamsViewModel();
            }
            model.ElementsCount = await GetTeamsCountAsync();
            model.Teams = await context
            .Teams
                .Skip((model.Page - 1) * model.ItemsPerPage)
                .Take(model.ItemsPerPage)
                .Select(x => new IndexTeamViewModel()
                {
                    Id = x.Id,
                    TeamName = x.Name,
                    TeamLeader = $"{x.Leader.FirstName} {x.Leader.FirstName}"
                })
                .ToListAsync();

            return model;
        }

        public async Task<int> GetTeamsCountAsync()
        {
            return await context.Teams.CountAsync();
        }

        public async Task<DetailsTeamViewModel> GetTeamDetails(string id)
        {
            Team team = await context.Teams.FindAsync(id);

            if (team == null) { return null; }
            if (team.Leader==null)
            {
                return new DetailsTeamViewModel()
                {
                    Id = team.Id,
                    TeamName = team.Name,
                    Project = team.Project,
                    Developers = team.Developers
                };
            }
            else
            {
                return new DetailsTeamViewModel()
                {
                    Id = team.Id,
                    TeamName = team.Name,
                    Project = team.Project,
                    Developers = team.Developers,
                    LeaderName = $"{team.Leader.FirstName} {team.Leader.LastName}"
                };
            }
        }

        public async Task<AddUserToTeamViewModel> GetUserToAddAsync(string id)
        {
            AddUserToTeamViewModel? result = null;

            Team? t = await GetTeamByIdAsync(id);
            List<string> userName = await userManager.Users
                .Where(x=>x.TeamId==null && x.Role!=GlobalConstants.AdminRole)
                .Select(x => x.Email).ToListAsync();

            if (t != null)
            {
                result = new AddUserToTeamViewModel()
                {
                    UserNames = userName,
                    TeamId = t.Id
                };
            }

            return result;
        }

        public async Task<AssignLeaderViewModel> GetUserToAssignAsync(string id)
        {
            AssignLeaderViewModel? result = null;

            Team? t = await GetTeamByIdAsync(id);
            List<string> userName = await userManager.Users
                .Where(x => x.TeamId == t.Id && x.Role != GlobalConstants.AdminRole && x.Role != GlobalConstants.TeamLeader)
                .Select(x => x.Email).ToListAsync();

            if (t != null)
            {
                result = new AssignLeaderViewModel()
                {
                    UserNames = userName,
                    TeamId = t.Id
                };
            }

            return result;
        }
        public async Task<string> AssignUserToTeamAsync(AddUserToTeamViewModel model)
        {
            Team team = await GetTeamByIdAsync(model.TeamId);
            User? user = await userManager.Users.FirstOrDefaultAsync(x => x.Email == model.Email);

            if (team != null && user != null)
            {
                team.Leader = user;
                user.TeamLed = team;
                await  userManager.AddToRoleAsync(user, GlobalConstants.TeamLeader);
                await context.SaveChangesAsync();
            }

            return model.TeamId;
        }
        public async Task<string> AddUserToTeamAsync(AddUserToTeamViewModel model)
        {
            Team team = await GetTeamByIdAsync(model.TeamId);
            User? user = await userManager.Users.FirstOrDefaultAsync(x => x.Email == model.Email);

            if (team != null && user != null)
            {
                team.Developers.Add(user);
                await context.SaveChangesAsync();
            }

            return model.TeamId;
        }

        public async Task<string> CreateTeamAsync(CreateTeamViewModel model)
        {

            Team team = new Team() { Name = model.TeamName };


            context.Add(team);
            await context.SaveChangesAsync();

            return team.Id;
        }

        public async Task<EditTeamViewModel> GetTeamToEditAsync(string id)
        {
            Team team = await context.Teams.FindAsync(id);

            if (team == null)
            {
                return null;
            }
            var teamLeader = team.Leader;
            if (team.Leader==null)
            {
                return new EditTeamViewModel()
                {
                    Id = team.Id,
                    Name = team.Name,
                    Developers = team.Developers.Select(x => x).ToList()
                };
            }
            return new EditTeamViewModel()
            {
                Id = team.Id,
                Name = team.Name,
                Developers = team.Developers.Select(x => x).ToList(),
                TeamLeader = teamLeader,
                LeaderName = $"{teamLeader.FirstName} {teamLeader.LastName}"
            };
        }

        public async Task<Team> GetTeamByIdAsync(string id)
        {
            return await this.context.Teams.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<bool> EditTeamAsync(EditTeamViewModel model)
        {
            Team team = await context.Teams.FindAsync(model.Id);


            if (team == null)
            {
                return false;
            }

            team.Name = model.Name;
            team.Developers = model.Developers;
            team.Leader = model.TeamLeader;
            context.Update(team);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<int> DeleteTeamAsync(string id)
        {
            var team = await context.Teams.FindAsync(id);
            if (team != null && team.Leader!=null)
            {
                team.Developers.Clear();
                team.Leader.TeamId =null;
                team.Leader.TeamId = null;
                team.Leader.TeamLed = null;
                team.Leader.Role = GlobalConstants.Unassigned;
                context.Teams.Remove(team);
            }
            else
            {
                team.Developers.Select(x => x.TeamId == null);
				team.Developers.Select(x => x.Role == GlobalConstants.Unassigned);
				team.Developers.Clear();
                context.Teams.Remove(team);

            }

            return await context.SaveChangesAsync();
        }

        public async Task<string> RemoveUserOfTeamAsync(AddUserToTeamViewModel model)
        {
            Team team = await GetTeamByIdAsync(model.TeamId);
            User? user = await userManager.Users.FirstOrDefaultAsync(x => x.Email == model.Email);

            if (team != null && user != null)
            {
                team.Developers.Remove(user);
                await context.SaveChangesAsync();

            }
			if (user.TeamId == team.Id && user.Role == GlobalConstants.TeamLeader)
			{
                team.LeaderId = null;
                team.Leader = null;
			}
			user.TeamId = null;
            return model.TeamId;
        }

        public async Task<DeleteUserViewModel> GetUserToRemoveAsync(string id)
        {
            DeleteUserViewModel? result = null;

            Team? t = await GetTeamByIdAsync(id);
            List<string> userName = await userManager.Users.Where(x => x.TeamId == t.Id).Select(x => x.Email).ToListAsync();

            if (t != null)
            {
                result = new DeleteUserViewModel()
                {
                    UserNames = userName,
                    TeamId = t.Id
                };
            }

            return result;
        }
    }
}
