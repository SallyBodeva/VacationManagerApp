using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationManagerApp.ViewModels.Shared;
using VacationManagerApp.ViewModels.Users;

namespace VacationManagerApp.ViewModels.Requests
{
    public class IndexRequestsViewModel:PagingViewModel
    {
        public IndexRequestsViewModel() : base(0)
        {

        }
        public IndexRequestsViewModel(int elementsCount, int itemsPerPage = 10, string action = "Index") : base(elementsCount, itemsPerPage, action)
        {
        }

        public ICollection<IndexRequestViewModel> Requests { get; set; } = new List<IndexRequestViewModel>();
    }
}
