using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VacationManagerApp.ViewModels.Projects
{
    public class AddTeamToProject
    {
        public string ProjectId { get; set; }
        public string TeamName { get; set; }

        public List<string>? TeamNames { get; set; } = new List<string>();
    }
}
