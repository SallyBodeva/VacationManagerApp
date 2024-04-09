using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationManagerApp.Data.Models;

namespace VacationManagerApp.ViewModels.Requests
{
    public class CreateRequestViewModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool HalfDay { get; set; }
        public string? Type { get; set; }
        public DateTime DateOfRequest { get; set; }
        [BindProperty]
        public IFormFile? File { get; set; }

        public string? Requester { get; set; }
    }
}
