using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationManagerApp.Data.Models;

namespace VacationManagerApp.ViewModels.Teams
{
    public class EditTeamViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public new List<User>? Developers { get; set; } = new List<User>();
        public User? TeamLeader { get; set; }
        public string? LeaderName { get; set; }
    }
}
