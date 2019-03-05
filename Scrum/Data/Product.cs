using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Scrum.Data
{

    public enum ProductStatus
    {
        CONCEPTUALIZED, IN_DEVELOPMENT, IN_PRODUCTION
    }
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        
        public ScrumUser ProductManager { get; set; }
        public virtual ICollection<ProductBacklogItem> ProductBacklog { get; set; }

        public virtual ICollection<ProductTeam> ProductTeams { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public Priority ProductPriority { get; set; }

        [Required]
        public ProductStatus ProductStatus { get; set; }
    }
}