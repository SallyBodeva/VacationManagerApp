using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationManagerApp.Data.Models;

namespace VacationManagerApp.ViewModels.Roles
{
    public class RolesMembersViewModel
    {
        public string RoleName { get; set; }
        public List<User> Members { get; set; }
    }
}
