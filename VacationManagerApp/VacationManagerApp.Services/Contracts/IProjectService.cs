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
        public Task<string> AddTeamToProject(ProjectDetailsViewModel model);
        public Task<int> DeleteProject(string id);
    }

}
