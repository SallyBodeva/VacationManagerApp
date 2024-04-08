using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VacationManagerApp.Data;
using VacationManagerApp.Services;
using VacationManagerApp.Services.Contracts;
using VacationManagerApp.ViewModels.Requests;

namespace VacationManagerApp.Web.Controllers
{
    public class RequestsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IRequestService requestService;

        public RequestsController(ApplicationDbContext context, IRequestService requestService)
        {
            _context = context;
            this.requestService = requestService;
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
            model.ApplicantId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (ModelState.IsValid)
            {
                await requestService.CreateRequestAsync(model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
    }
}
