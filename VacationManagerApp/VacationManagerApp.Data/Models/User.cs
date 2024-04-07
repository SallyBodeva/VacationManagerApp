using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VacationManagerApp.Data.Models
{
    public class User:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string? TeamId { get; set; }
        public virtual Team Team { get; set; }

        public string? TeamLedId { get; set; }
        public virtual Team TeamLed { get; set; }

        public virtual ICollection<VacationRequest> VacationRequests { get; set; } = new List<VacationRequest>();
        public virtual ICollection<SickLeave> SickLeaves { get; set; } = new List<SickLeave>();

        public string Role { get; set; }
    }
}
