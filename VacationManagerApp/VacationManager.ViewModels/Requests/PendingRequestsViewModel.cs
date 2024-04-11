using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationManagerApp.Data.Models;

namespace VacationManagerApp.ViewModels.Requests
{
    public class PendingRequestsViewModel
    {
        public ICollection<VacationRequest>? AdminRequests { get; set; } = new List<VacationRequest>();
        public ICollection<VacationRequest>? LeaderRequests { get; set; } = new List<VacationRequest>();

        public string LoggedUser { get; set; }

        public string AdminId { get; set; }
        public List<string> LeaderIds { get; set; }
    }
}
