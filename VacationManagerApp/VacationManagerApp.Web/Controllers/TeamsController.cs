using Microsoft.AspNetCore.Mvc;
using VacationManagerApp.Data;
using VacationManagerApp.Services.Contracts;
using VacationManagerApp.ViewModels.Teams;

namespace VacationManagerApp.Web.Controllers
{
    public class TeamsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ITeamService teamService;

        public TeamsController(ApplicationDbContext context, ITeamService teamService)
        {
            _context = context;
            this.teamService = teamService;
        }
        public async Task<IActionResult> Index(IndexTeamsViewModel model)
        {
            model = await teamService.GetTeamsAsync(model);
            return View(model);
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await teamService.GetTeamDetails(id);
            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTeamViewModel model)
        {
            if (ModelState.IsValid)
            {
                await teamService.CreateTeamAsync(model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await teamService.GetTeamToEditAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditTeamViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await teamService.EditTeamAsync(model);

                if (result == false)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurant = await teamService.GetTeamDetails(id);
            if (restaurant == null)
            {
                return NotFound();
            }

            return View(restaurant);
        }

        // POST: Restaurants/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await teamService.DeleteTeamAsync(id);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> AddUserToTeam(string id)
        {
            var model = await teamService.GetUserToAddAsync(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddUserToTeam(AddUserToTeamViewModel model)
        {
            if (ModelState.IsValid)
            {
                await teamService.AddUserToTeamAsync(model);
                return this.RedirectToAction(nameof(Index));
            }
            return View(model);
        }
        public async Task<IActionResult> RemoveUserOfTeam(string id)
        {
            var model = await teamService.GetUserToRemoveAsync(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveUserOfTeam(AddUserToTeamViewModel model)
        {
            if (ModelState.IsValid)
            {
                await teamService.RemoveUserOfTeamAsync(model);
                return this.RedirectToAction(nameof(Index));
            }
            return View(model);
        }
    }
}
