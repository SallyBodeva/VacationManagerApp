using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationManagerApp.Data.Models;

namespace VacationManagerApp.ViewModels.Teams
{
	public class DetailsTeamViewModel
	{
		public string Id { get; set; }

		public string TeamName { get; set; }
        public string LeaderName { get; set; }
        public  Project? Project { get; set; }

		public  ICollection<User> Developers { get; set; } = new List<User>();
	}
}
