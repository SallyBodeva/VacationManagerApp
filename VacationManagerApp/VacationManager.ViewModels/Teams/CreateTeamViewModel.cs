using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationManagerApp.Data.Models;

namespace VacationManagerApp.ViewModels.Teams
{
    public class CreateTeamViewModel
    {
        [Required]
        [Display(Name = "Team name")]
        public string TeamName { get; set; }
        public User? TeamLeader  { get; set; }
    }
}
