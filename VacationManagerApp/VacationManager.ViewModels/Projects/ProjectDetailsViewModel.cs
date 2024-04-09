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
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Team> Teams { get; set; } = new List<Team>();

    }
}
