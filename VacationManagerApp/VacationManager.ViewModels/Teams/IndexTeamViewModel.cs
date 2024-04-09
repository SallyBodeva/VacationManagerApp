using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationManagerApp.Data.Models;

namespace VacationManagerApp.ViewModels.Teams
{
    public class IndexTeamViewModel
    {
        public string Id { get; set; }
        public string TeamName { get; set; }
        public string TeamLeader{ get; set; }
        public string? TeamLeaderId { get; set; }
    }
}
