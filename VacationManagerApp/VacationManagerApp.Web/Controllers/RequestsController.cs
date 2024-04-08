using Microsoft.AspNetCore.Mvc;
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
    }
}
