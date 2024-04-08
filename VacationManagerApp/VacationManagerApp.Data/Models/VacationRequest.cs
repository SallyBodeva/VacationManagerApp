using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VacationManagerApp.Data.Models
{
    public class VacationRequest
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime DateOfRequest { get; set; }
        public bool IsHalfDay { get; set; }

        public bool IsApproved { get; set; }

        public string Type { get; set; }

        public string? PatientNote { get; set; }

        public string RequesterId { get; set; }

        public virtual User Requester { get; set; }
    }
}
