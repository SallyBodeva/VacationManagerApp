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
using VacationManagerApp.ViewModels.Projects;
using Microsoft.EntityFrameworkCore;

namespace VacationManagerApp.Services
{
    public class ProjectService :IProjectService
    {

        private ApplicationDbContext context;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<User> userManager;
        public ProjectService(ApplicationDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task<IndexProjectsViewModel> GetProjectsAsync(IndexProjectsViewModel model)
        {
            if (model == null)
            {
                model = new IndexProjectsViewModel();
            }
            model.ElementsCount = await GetProjectCountAsync();
            model.Projects = await context
            .Projects
                .Skip((model.Page - 1) * model.ItemsPerPage)
                .Take(model.ItemsPerPage)
                .Select(x => new IndexProjectViewModel()
                {
                    Id = x.Id,
                   Name = x.Name,
                   Description =x.Description
                })
                .ToListAsync();

            return model;
        }
        public async Task<int> GetProjectCountAsync()
        {
            return await context.Projects.CountAsync();
        }
        public async Task<string> CreateProjectAsync(CreateProjectViewModel model)
        {
            Project project = new Project()
            {
                Name = model.Name,
                Description = model.Description
            };
            context.Add(project);
            await context.SaveChangesAsync();
            return project.Id;
        }
        public async Task<ProjectDetailsViewModel> GetProjectDetails(string id)
        {
            Project p = context.Projects.Where(x => x.Id == id).Include(x => x.Teams).FirstOrDefault();
            if (p == null)
            {
                return null;
            }
            ProjectDetailsViewModel model = new ProjectDetailsViewModel()
            {
                Project = p,
                TeamsWithoutProject = await context.Teams.Where(x => x.ProjectId == null).ToListAsync(),
                ProjectId=p.Id
            };
            return model;
        }
        public async Task<string> AddTeamToProject(ProjectDetailsViewModel model)
        {
            Project p = context.Projects.Where(x => x.Id == model.ProjectId).Include(x => x.Teams).FirstOrDefault();
            if (p == null)
            {
                return null;
            }
            Team team= context.Teams.Where(x=>x.Id==model.TeamToAddId).FirstOrDefault();
            if (team==null)
            {
                return null;
            }
            team.ProjectId = model.ProjectId;
            await context.SaveChangesAsync();
            return team.Id;
        }
        public async Task<int> DeleteProject(string id)
        {
            Project p = await context.Projects.Where(x => x.Id == id).FirstOrDefaultAsync();
           
            context.Remove(p);
           return await context.SaveChangesAsync();
        }
    }
}

