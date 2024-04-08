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
        public Task<int> GetReuestsCountAsync();
        public Task<IndexRequestsViewModel> GetRequestsAsync(IndexRequestsViewModel requests);
    }
}
