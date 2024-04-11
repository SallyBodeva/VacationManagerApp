using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationManagerApp.ViewModels.Requests;

namespace VacationManagerApp.Services.Contracts
{
    public interface IRequestService
    {
        public Task<int> GetMyRequestsCountAsync(string userId);
        public Task<IndexRequestsViewModel> GetMyRequestsAsync(IndexRequestsViewModel requests);

        public Task<int> CreateRequestAsync(CreateRequestViewModel model);
        public Task<int> DeleteMyRequestsAsync(string id);
        public  Task<EditRequestViewModel?> GetRequestToEditAsync(string id);
        public Task<string> UpdateRequestAsync(EditRequestViewModel request);
        public Task<PendingRequestsViewModel> GetPendingRequestsAsync(PendingRequestsViewModel model);
        public Task<int?> Approve(string id);
        public Task<int?> DeleteRequest(string id);
        public Task<int?> AskAdmin(string id);
    }
}
