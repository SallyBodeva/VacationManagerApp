using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VacationManagerApp.ViewModels.Users
{
    public class DetailsUserViewModel
    {
        public string Id { get; set; }


        [Display(Name="Full name")]
        public string Name { get; set; }

        [Display(Name = "Team")]
        public string Team { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Role")]
        public string Role { get; set; }
    }
}
