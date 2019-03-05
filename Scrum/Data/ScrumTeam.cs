using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Scrum.Data
{
    public class ScrumTeam
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public virtual ICollection<ScrumUserTeam> UserTeams { get; set; }

        public virtual ICollection<ProductBacklogItem> SprintBackLog { get; set; }

        public virtual ICollection<ProductTeam> ProductTeams { get; set; }


        public ScrumUser ScrumMaster { get; set; }

        [Required]
        public string TeamName { get; set; }
    }


    public class ScrumUserTeam
    {

        public int UserId { get; set; }
      
        public ScrumUser User { get; set; }
      
        public int TeamId { get; set; }
        public ScrumTeam Team { get; set; }
    }

    public class ProductTeam
    {
        public int ProductId { get; set; }

        public Product Product { get; set; }

        public int TeamId { get; set; }
        public ScrumTeam Team { get; set; }
    }


   
}