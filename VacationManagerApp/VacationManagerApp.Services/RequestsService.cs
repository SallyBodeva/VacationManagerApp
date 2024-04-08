using Microsoft.AspNetCore.Identity;
using VacationManagerApp.Data.Models;
using VacationManagerApp.Data;
using Microsoft.EntityFrameworkCore;
using VacationManagerApp.ViewModels.Requests;
using VacationManagerApp.Services.Contracts;

namespace VacationManagerApp.Services
{
    public class RequestsService:IRequestService
    {

        private ApplicationDbContext context;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<User> userManager;
        public RequestsService(ApplicationDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        public async Task<IndexRequestsViewModel> GetRequestsAsync(IndexRequestsViewModel requests)
        {
            if (requests == null)
            {
                requests = new IndexRequestsViewModel(0);
            }
            requests.ElementsCount = await GetReuestsCountAsync();

            requests.Requests = await context
                .VacationRequests
                .Skip((requests.Page - 1) * requests.ItemsPerPage)
                .Take(requests.ItemsPerPage)
                .Select(x => new IndexRequestViewModel()
                {
                    Id = x.Id,
                    UserFullName = $"{x.Applicant.FirstName} {x.Applicant.LastName}",
                    Period = ((x.EndDate-x.StartDate).TotalDays).ToString(),
                })
                .ToListAsync();

            return requests;
        }
        public async Task<int> GetReuestsCountAsync()
        {
            return await context.VacationRequests.CountAsync();
        }
    }
}
