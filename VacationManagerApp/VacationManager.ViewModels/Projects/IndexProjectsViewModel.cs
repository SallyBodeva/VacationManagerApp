using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationManagerApp.ViewModels.Shared;
using VacationManagerApp.ViewModels.Teams;

namespace VacationManagerApp.ViewModels.Projects
{
    public class IndexProjectsViewModel : PagingViewModel
    {
        public IndexProjectsViewModel(int elementsCount, int itemsPerPage = 5, string action = "Index") : base(elementsCount, itemsPerPage, action)
        {
        }
        public IndexProjectsViewModel() : base(0)
        {
        }
        public ICollection<IndexProjectViewModel> Projects { get; set; } = new List<IndexProjectViewModel>();
    }
}
