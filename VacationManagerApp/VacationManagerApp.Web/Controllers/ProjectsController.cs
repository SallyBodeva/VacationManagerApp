using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using VacationManagerApp.Data;
using VacationManagerApp.Services;
using VacationManagerApp.Services.Contracts;
using VacationManagerApp.ViewModels.Projects;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VacationManagerApp.Web.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IProjectService projectService;

        public ProjectsController(ApplicationDbContext context, IProjectService projectService)
        {
            context = context;
            this.projectService = projectService;
        }
        public async Task<IActionResult> Index(IndexProjectsViewModel model)
        {
            model = await projectService.GetProjectsAsync(model);
            return View(model);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateProjectViewModel model)
        {
            if (ModelState.IsValid)
            {
                await projectService.CreateProjectAsync(model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await projectService.GetProjectDetails(id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }
        public async Task<IActionResult> AddTeamToProject(ProjectDetailsViewModel model)
        {
            await projectService.AddTeamToProject(model);
            return RedirectToAction(nameof(Index));
        }
      public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await projectService.DeleteProject(id);


            return View(nameof(Index));
        }
    }
}
