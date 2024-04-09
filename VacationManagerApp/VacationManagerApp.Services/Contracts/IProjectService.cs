using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationManagerApp.ViewModels.Projects;

namespace VacationManagerApp.Services.Contracts
{
    public interface IProjectService
    {
        public Task<IndexProjectsViewModel> GetProjectsAsync(IndexProjectsViewModel model);
        public Task<int> GetProjectCountAsync();
        public Task<string> CreateProjectAsync(CreateProjectViewModel model);
        public Task<ProjectDetailsViewModel> GetProjectDetails(string id);
        public Task<string> AddTeamToProject(AddTeamToProject model);
        public  Task<AddTeamToProject> GetTeamToAddAsync(string id);
        public  Task<string> RemoveTeamFromProject(RemoveTeamViewModel model);
        public  Task<RemoveTeamViewModel> GetTeamToRemoveAsync(string id);
        public Task<EditProjectViewModel?> GetProjectToEditAsync(string id);
        public Task<string> UpdateProjectAsync(EditProjectViewModel project);
        public Task<int> DeleteProjectAsync(string id);
    }

}
