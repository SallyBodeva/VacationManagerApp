using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VacationManagerApp.ViewModels.Teams
{
    public class DeleteUserViewModel
    {
        public string TeamId { get; set; }
        public string? Email { get; set; }
        public List<string>? UserNames { get; set; } = new List<string>();
    }
}
