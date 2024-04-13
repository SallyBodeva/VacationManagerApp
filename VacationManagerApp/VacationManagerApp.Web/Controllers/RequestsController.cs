using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VacationManagerApp.Common;
using VacationManagerApp.Data;
using VacationManagerApp.Data.Models;
using VacationManagerApp.Services;
using VacationManagerApp.Services.Contracts;
using VacationManagerApp.ViewModels.Projects;
using VacationManagerApp.ViewModels.Requests;

namespace VacationManagerApp.Web.Controllers
{
    public class RequestsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IRequestService requestService;
        private readonly Microsoft.AspNetCore.Identity.UserManager<User> userManager;

        public RequestsController(ApplicationDbContext context, IRequestService requestService,UserManager<User> userManager)
        {
            _context = context;
            this.requestService = requestService;
            this.userManager = userManager;
        }
        [Authorize]
        public async Task<IActionResult> Index(IndexRequestsViewModel model)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            model.UserId = userId;
            model = await requestService.GetMyRequestsAsync(model);
            return View(model);
           
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRequestViewModel model)
        {
            model.DateOfRequest = DateTime.Today;
            model.Requester = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (ModelState.IsValid)
            {
                await requestService.CreateRequestAsync(model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var model = await requestService.GetRequestToEditAsync(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditRequestViewModel model)
        {
            if (ModelState.IsValid)
            {
                await requestService.UpdateRequestAsync(model);
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

            var request = await requestService.DeleteMyRequestsAsync(id);


            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        [Authorize(Roles = GlobalConstants.AdminRole + "," + GlobalConstants.TeamLeader)]
        public async Task<IActionResult> Pending(PendingRequestsViewModel model)
        {
            model = await requestService.GetPendingRequestsAsync(model);
            return View(model);
        }


        public async Task<IActionResult> Approve(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var request = await requestService.Approve(id);


            return RedirectToAction(nameof(Pending));
        }
        public async Task<IActionResult> DeleteRequest(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var request = await requestService.DeleteRequest(id);

            return RedirectToAction(nameof(Pending));
        }
        public async Task<IActionResult> AskAdmin(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var request = await requestService.AskAdmin(id);

            return RedirectToAction(nameof(Pending));
        }
    }
}
