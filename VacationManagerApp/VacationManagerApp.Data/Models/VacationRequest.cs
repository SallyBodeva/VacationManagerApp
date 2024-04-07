using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VacationManagerApp.Data.Models
{
    public class VacationRequest
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateOnly FromDate { get; set; }
        public DateOnly ToDate { get; set; }
        public DateTime IssueDate { get; set; }
        public bool IsHalfDay { get; set; }
        public bool IsApproved { get; set; }

        public string AplicantId { get; set; }
        public virtual User Aplicant { get; set; }
    }
}
