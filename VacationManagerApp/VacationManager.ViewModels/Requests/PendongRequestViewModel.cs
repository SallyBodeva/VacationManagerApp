using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationManagerApp.Data.Models;

namespace VacationManagerApp.ViewModels.Requests
{
    public class PendongRequestViewModel
    {
        public string Id { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime DateOfRequest { get; set; }
        public bool IsHalfDay { get; set; }

        public bool IsApproved { get; set; }

        public string Type { get; set; }

        public string? PatientNote { get; set; }

        public string RequesterId { get; set; }

        public virtual User Requester { get; set; }

        public bool AskAdmin { get; set; }
    }
}
