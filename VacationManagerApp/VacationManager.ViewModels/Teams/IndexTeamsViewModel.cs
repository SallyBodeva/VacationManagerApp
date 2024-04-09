using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationManagerApp.ViewModels.Shared;

namespace VacationManagerApp.ViewModels.Teams
{
    public class IndexTeamsViewModel : PagingViewModel
    {
        public IndexTeamsViewModel() : base(0)
        {

        }

        public IndexTeamsViewModel(int elementsCount, int itemsPerPage = 5, string action = "Index") : base(elementsCount, itemsPerPage, action)
        {
        }
        public ICollection<IndexTeamViewModel> Teams { get; set; } = new List<IndexTeamViewModel>();
        public string? LogedUserid { get; set; }
    }
}
