using System;
using System.ComponentModel.DataAnnotations;

namespace MvcTemplate.Objects
{
    public class AuditLog : BaseModel
    {
        [StringLength(128)]
        public String AccountId { get; set; }

        [Required]
        [StringLength(128)]
        public String Action { get; set; }

        [Required]
        [StringLength(128)]
        public String EntityName { get; set; }

        [Required]
        [StringLength(128)]
        public String EntityId { get; set; }

        [Required]
        public String Changes { get; set; }
    }
}
