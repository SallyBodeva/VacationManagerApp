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
using VacationManagerApp.ViewModels.Projects;

namespace VacationManagerApp.Services
{
    public class RequestsService : IRequestService
    {

        private ApplicationDbContext context;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<User> userManager;
        private readonly IHttpContextAccessor httpContextAccessor;
        public RequestsService(ApplicationDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.httpContextAccessor = httpContextAccessor;
        }
        public async Task<IndexRequestsViewModel> GetMyRequestsAsync(IndexRequestsViewModel model)
        {
            User? user = await userManager.FindByIdAsync(model.UserId);
            IQueryable<VacationRequest> requests = null;

            requests = context.VacationRequests
                 .Where(x => x.RequesterId == model.UserId);
            model.ElementsCount = await GetMyRequestsCountAsync(model.UserId);

            model.Requests = await
                requests
                .Skip((model.Page - 1) * model.ItemsPerPage)
                .Take(model.ItemsPerPage)
                .Select(x => new IndexRequestViewModel()
                {
                    Id = x.Id,
                    UserFullName = $"{x.Requester.FirstName} {x.Requester.LastName}",
                    Days = Math.Ceiling((x.EndDate - x.StartDate).TotalDays).ToString(),
                    Period = $"{x.StartDate.ToShortDateString()} - {x.EndDate.ToShortDateString()}",
                    IsApproved = x.IsApproved
                })
                .ToListAsync();

            return model;
        }
        public async Task<int> DeleteMyRequestsAsync(string id)
        {
            VacationRequest request = await context.VacationRequests.Where(x => x.Id == id).FirstOrDefaultAsync();
            context.Remove(request);
            return await context.SaveChangesAsync();
        }
        public async Task<int> GetMyRequestsCountAsync(string userId)
        {
            return context.VacationRequests.Count(x => x.RequesterId == userId);
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
            request.RequesterId = model.Requester;
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
        public async Task<EditRequestViewModel?> GetRequestToEditAsync(string id)
        {
            EditRequestViewModel? result = null;

            VacationRequest? request = await context.VacationRequests.FirstOrDefaultAsync(x => x.Id == id);

            if (request != null)
            {
                result = new EditRequestViewModel()
                {
                    Id = request.Id,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                };
            }

            return result;
        }
        public async Task<string> UpdateRequestAsync(EditRequestViewModel request)
        {
            VacationRequest? oldRequest = await context.VacationRequests.FirstOrDefaultAsync(x => x.Id == request.Id);

            if (oldRequest != null)
            {
                oldRequest.StartDate = request.StartDate;
                oldRequest.EndDate = request.EndDate;
            }
            context.Update(oldRequest);
            await context.SaveChangesAsync();
            return request.Id;
        }
        public async Task<PendingRequestsViewModel> GetPendingRequestsAsync(PendingRequestsViewModel model)
        {
            model.AdminId = userManager.Users.FirstOrDefaultAsync(x => x.Role == GlobalConstants.AdminRole).Result.Id; 
            model.LeaderIds = userManager.Users.Where(x => x.Role == GlobalConstants.TeamLeader).Select(x=>x.Id).ToList();

            model.LoggedUser = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            model.AdminRequests = new List<PendongRequestViewModel>();
            model.LeaderRequests = new List<PendongRequestViewModel>();

            model.AdminRequests = await context.VacationRequests
                .Where(x => (!x.IsApproved && (x.Requester.Team.Leader == null || x.Requester.Team == null)) || x.AskAdmin==true)
                .Select(x=> new PendongRequestViewModel() 
                {
                    Id = x.Id,
                    StartDate= x.StartDate,
                    EndDate= x.EndDate,
                    DateOfRequest = x.DateOfRequest,
                    IsHalfDay= x.IsHalfDay,
                    IsApproved = x.IsApproved,
                    Type = x.Type,
                    PatientNote = x.PatientNote,
                    RequesterId = x.RequesterId,
                    Requester = x.Requester
                }
                )
                .ToListAsync();

            model.LeaderRequests = await context.VacationRequests
                .Where(x => (!x.IsApproved && x.Requester.Team.LeaderId == model.LoggedUser) && x.AskAdmin==false)
                 .Select(x => new PendongRequestViewModel()
                 {
                     Id = x.Id,
                     StartDate = x.StartDate,
                     EndDate = x.EndDate,
                     DateOfRequest = x.DateOfRequest,
                     IsHalfDay = x.IsHalfDay,
                     IsApproved = x.IsApproved,
                     Type = x.Type,
                     PatientNote = x.PatientNote,
                     RequesterId = x.RequesterId,
                     Requester = x.Requester
                 }
                )
                .ToListAsync();

            return model;

        }
        public async Task<int?> Approve(string id)
        {
            VacationRequest request = await context.VacationRequests.FindAsync(id);
            if (request!=null)
            {
                request.IsApproved = true;
                request.AskAdmin = false;
                context.Update(request);
            }
            return await context.SaveChangesAsync();
        }
        public async Task<int?> DeleteRequest(string id)
        {
            VacationRequest request = await context.VacationRequests.FindAsync(id);
            if (request != null)
            {
                context.Remove(request);
               
            }
            return await context.SaveChangesAsync();
        }
        public async Task<int?> AskAdmin(string id)
        {
            VacationRequest request = await context.VacationRequests.FindAsync(id);
            if (request != null)
            {
                request.AskAdmin = true;
                context.Update(request);
            }
            return await context.SaveChangesAsync();
        }
    }
}
