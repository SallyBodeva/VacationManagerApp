using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationManagerApp.ViewModels.Shared;

namespace VacationManagerApp.ViewModels.Roles
{
    public class IndexRolesViewModel : PagingViewModel
    {
        public IndexRolesViewModel(int elementsCount, int itemsPerPage = 5, string action = "Index") : base(elementsCount, itemsPerPage, action)
        {
        }
        public IndexRolesViewModel() : base(0)
        {
        }
        public ICollection<IndexRoleViewModel> Roles { get; set; } = new List<IndexRoleViewModel>();
    }
}
