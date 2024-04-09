using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using VacationManagerApp.Data;
using VacationManagerApp.Services;
using VacationManagerApp.Services.Contracts;
using VacationManagerApp.ViewModels.Projects;
using VacationManagerApp.ViewModels.Users;
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
        [HttpGet]
        public async Task<IActionResult> RemoveTeamFromProject(string id)
        {
            var model = await projectService.GetTeamToRemoveAsync(id);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> RemoveTeamFromProject(RemoveTeamViewModel model)
        {
            if (ModelState.IsValid)
            {
                await projectService.RemoveTeamFromProject(model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> AddTeamToProject(string id)
        {
            var model = await projectService.GetTeamToAddAsync(id);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddTeamToProject(AddTeamToProject model)
        {
            if (ModelState.IsValid)
            {
                await projectService.AddTeamToProject(model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var model = await projectService.GetProjectToEditAsync(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditProjectViewModel model)
        {
            if (ModelState.IsValid)
            {
                await projectService.UpdateProjectAsync(model);
                return this.RedirectToAction(nameof(Index));
            }
            return View(model);
        }
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await projectService.DeleteProjectAsync(id);


            return RedirectToAction(nameof(Index));
        }
    }
}
