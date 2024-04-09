using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VacationManagerApp.ViewModels.Projects
{
    public class EditProjectViewModel
    {
        public string Id { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(100)]

        [Display(Name = "First name")]
        public string ProjectName { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(100)]

        [Display(Name = "Last name")]
        public string Description { get; set; }
    }
}
