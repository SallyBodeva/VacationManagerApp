using Microsoft.AspNetCore.Mvc;
using VacationManagerApp.Data;
using VacationManagerApp.Services;
using VacationManagerApp.Services.Contracts;
using VacationManagerApp.ViewModels.Projects;
using VacationManagerApp.ViewModels.Roles;

namespace VacationManagerApp.Web.Controllers
{
    public class RolesController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IRoleService roleService;

        public RolesController(ApplicationDbContext context, IRoleService roleService)
        {
            this.context = context;
            this.roleService = roleService;
        }
        public async Task<IActionResult> Index(IndexRolesViewModel model)
        {
            model = await roleService.GetRolesAsync(model);
            return View(model);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                await roleService.CreateRole(model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Members(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await roleService.Members(id);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

    }
}
