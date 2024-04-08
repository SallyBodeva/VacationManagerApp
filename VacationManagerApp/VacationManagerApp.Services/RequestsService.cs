using Microsoft.AspNetCore.Identity;
using VacationManagerApp.Data.Models;
using VacationManagerApp.Data;
using Microsoft.EntityFrameworkCore;
using VacationManagerApp.ViewModels.Requests;
using VacationManagerApp.Services.Contracts;
using VacationManagerApp.Common;
using VacationManagerApp.ViewModels.Teams;
using System.Security.Claims;
using Azure.Core;
using Microsoft.AspNetCore.Http;

namespace VacationManagerApp.Services
{
    public class RequestsService :IRequestService
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
                    UserFullName = $"{x.Requester.FirstName} {x.Requester.LastName}",
                    Period = ((x.EndDate - x.StartDate).TotalDays).ToString(),
                })
                .ToListAsync();

            return requests;
        }
        public async Task<int> GetReuestsCountAsync()
        {
            return await context.VacationRequests.CountAsync();
        }
        public async Task<int> CreateRequestAsync(CreateRequestViewModel model)
        {
            VacationRequest request = null;
            if (model.Type == GlobalConstants.PaidTimeOff || model.Type == GlobalConstants.UnpaidTimeOff)
            {
                request = new VacationRequest()
                {
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    IsHalfDay = model.HalfDay,
                    Type = model.Type
                };
            }
            else
            {
                request = new VacationRequest()
                {
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    PatientNote = await ImageToStringAsync(model.File),
                    Type = model.Type
                };
            }
            context.VacationRequests.Add(request);
            return await context.SaveChangesAsync();

        }

        private async Task<string> ImageToStringAsync(IFormFile file)
        {
            List<string> imageExtensions = new List<string>() { ".JPG", ".BMP", ".PNG" };


            if (file != null) // check if the user uploded something
            {
                var extension = Path.GetExtension(file.FileName); //get file extension
                if (imageExtensions.Contains(extension.ToUpperInvariant()))
                {
                    using var dataStream = new MemoryStream();
                    await file.CopyToAsync(dataStream);
                    byte[] imageBytes = dataStream.ToArray();
                    string base64String = Convert.ToBase64String(imageBytes);
                    return base64String;
                }
            }
            return null;
        }
    }
}
