using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VacationManagerApp.Data;
using VacationManagerApp.Data.Models;
using VacationManagerApp.Services;
using VacationManagerApp.Services.Contracts;
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
        public async Task<IActionResult> Index(IndexRequestsViewModel model)
        {
            model = await requestService.GetRequestsAsync(model);
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
            model.Requester = userManager.Users.FirstOrDefault(x => x.Id == userManager.GetUserId(User));
            if (ModelState.IsValid)
            {
                await requestService.CreateRequestAsync(model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
    }
}
