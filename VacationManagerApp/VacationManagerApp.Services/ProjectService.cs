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
using VacationManagerApp.Common;
using VacationManagerApp.ViewModels.Users;

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
            Project p = context.Projects.Where(x => x.Id == id).FirstOrDefault();
            if (p == null)
            {
                return null;
            }
            ProjectDetailsViewModel model = new ProjectDetailsViewModel()
            {
                Id = p.Id,
                Name = p.Name,
                Description=p.Description,
                Teams = context.Teams.Where(x=>x.ProjectId==p.Id).ToList(),
            };
            return model;
        }
        public async Task<string> AddTeamToProject(AddTeamToProject model)
        {
            Project? project = await context.Projects.FirstOrDefaultAsync(x => x.Id == model.ProjectId);
            Team? team = await context.Teams.FirstOrDefaultAsync(x => x.Name == model.TeamName);
            if (team != null && project != null)
            {
                team.Project = project;
                await context.SaveChangesAsync();
            }
            context.Teams.Update(team);
            return project.Id;
        }

        public async Task<AddTeamToProject> GetTeamToAddAsync(string id)
        {
            AddTeamToProject? result = null;

            Project? project = await context.Projects.FindAsync(id);
            List<string> teamName = context.Teams.Select(x => x.Name).ToList();

            if (project != null)
            {
                result = new AddTeamToProject()
                {
                    ProjectId= project.Id,
                    TeamNames = teamName
                };
            }

            return result;

        }

        public async Task<RemoveTeamViewModel> GetTeamToRemoveAsync(string id)
        {
            RemoveTeamViewModel? result = null;

            Project? project = await context.Projects.FindAsync(id);
            List<string> teamName = context.Teams.Where(x=>x.ProjectId==project.Id).Select(x => x.Name).ToList();

            if (project != null)
            {
                result = new RemoveTeamViewModel()
                {
                    ProjectId = project.Id,
                    TeamNames = teamName
                };
            }

            return result;

        }

        public async Task<int> DeleteProject(string id)
        {
            Project p = await context.Projects.Where(x => x.Id == id).FirstOrDefaultAsync();
           
            context.Remove(p);
           return await context.SaveChangesAsync();
        }

        public async Task<string> RemoveTeamFromProject(RemoveTeamViewModel model)
        {
            Project? project = await context.Projects.FirstOrDefaultAsync(x => x.Id == model.ProjectId);
            Team? team = await context.Teams.FirstOrDefaultAsync(x => x.Name == model.TeamName);
            if (team != null && project != null)
            {
                project.Teams.Remove(team);
                await context.SaveChangesAsync();
            }
            team.ProjectId = null;
            context.Update(team);
            return project.Id;
        }
        public async Task<EditProjectViewModel?> GetProjectToEditAsync(string id)
        {
            EditProjectViewModel? result = null;

            Project? project = await context.Projects.FirstOrDefaultAsync(x=>x.Id==id);

            if (project != null)
            {
                result = new EditProjectViewModel()
                {
                    Id = project.Id,
                    ProjectName = project.Name,
                    Description = project.Description,
                };
            }

            return result;
        }
        public async Task<string> UpdateProjectAsync(EditProjectViewModel project)
        {
            Project? oldProject = await context.Projects.FirstOrDefaultAsync(x => x.Id == project.Id);

            if (oldProject != null)
            {
                oldProject.Name = project.ProjectName;
                oldProject.Description = project.Description;
            }
            context.Update(oldProject);
            await context.SaveChangesAsync();
            return project.Id;
        }
        public async Task<int> DeleteProjectAsync(string id)
        {
            Project? project = await context.Projects.FirstOrDefaultAsync(x=>x.Id==id);
            if (project != null)
            {
                 context.Projects.Remove(project);
            }
            return await context.SaveChangesAsync();
        }
    }
}

