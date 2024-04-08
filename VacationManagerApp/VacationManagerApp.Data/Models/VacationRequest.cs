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
        public string Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime IssueDate { get; set; }
        public bool? IsHalfDay { get; set; }
        public bool IsApproved { get; set; }
        public string Type { get; set; }
        public string? PatientNote { get; set; }

        public string ApplicantId { get; set; }

        public virtual User Applicant { get; set; }
    }
}
