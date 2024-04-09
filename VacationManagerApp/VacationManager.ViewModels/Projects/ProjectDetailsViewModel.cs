using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationManagerApp.Data.Models;

namespace VacationManagerApp.ViewModels.Projects
{
    public class ProjectDetailsViewModel
    {
        public Project Project { get; set; }
        public List<Team> TeamsWithoutProject { get; set; }
        public string TeamToAddId { get; set; }
        public string ProjectId { get; set; }
    }
}
