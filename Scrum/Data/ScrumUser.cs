using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Scrum.Data
{
    public class ScrumUser : IdentityUser<int>
    {
      public virtual ICollection<ScrumUserTeam> UserTeams { get; set; }
        

        public virtual ICollection<Product> ManagedProducts { get; set; } 

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string Nicknames { get; set; }

        [NotMapped]
        public string FullName { get => FirstName + " " + LastName; }
    }
}
