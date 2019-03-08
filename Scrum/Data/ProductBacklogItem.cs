using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Scrum.Data
{

    public enum BacklogStatus
    {
        CREATED, REJECTED, ON_HOLD, IN_PROGRESS, COMPLETED
    } 

    public enum Priority
    {
        NONE, VERY_LOW, LOW, MEDIUM, HIGH, VERY_HIGH
    }
    public class ProductBacklogItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public ScrumTeam Team { get; set; }

        [Required]
        public Product Product { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime LastUpdated { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Created { get; set; }

        [Required]
        public Priority Priority { get; set; }

        [Required]
        public BacklogStatus Status { get; set; }

        public virtual ICollection<BacklogUpdate> Updates { get; set; }

        public virtual ICollection<BacklogTask> Tasks { get; set; }
    }

    public class BacklogTask
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        [Required]
        public ProductBacklogItem Item { get; set; }

        [Required]
        public string Description { get; set; }

        public virtual ICollection<BacklogTaskSchedule> Schedule { get; set; }
    }

    public class BacklogTaskSchedule
    {

        
        public int BackLogTaskId { get; set; }

        
        public BacklogTask Task { get; set; }

        public DateTime Day { get; set; }

        [Required]
        public int Hours { get; set; }
    }

    public class BacklogUpdate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public ProductBacklogItem ProductBacklog { get; set; }


        [Required]
        public string Description { get; set; }

        [Required]
        public ScrumUser UpdatePerson { get; set; }

        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime UpdateTime { get; set; }
    }


}
