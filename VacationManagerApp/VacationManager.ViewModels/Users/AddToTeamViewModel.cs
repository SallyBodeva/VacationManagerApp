using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VacationManagerApp.ViewModels.Users
{
    public class AddToTeamViewModel
    {
        public string UserId { get; set; }
        public string TeamName { get; set; }

        public List<string>? TeamNames { get; set; } = new List<string>();
    }
}
