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
    public class Team
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }

        public string? LeaderId { get; set; }

        public virtual User Leader { get; set; }
        public string? ProjectId { get; set; }

        public virtual ICollection<User> Developers { get; set; } = new List<User>();

        public virtual Project Project { get; set; }
    }
}
