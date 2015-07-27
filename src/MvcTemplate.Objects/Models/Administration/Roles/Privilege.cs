using System;
using System.ComponentModel.DataAnnotations;

namespace MvcTemplate.Objects
{
    public class Privilege : BaseModel
    {
        [StringLength(128)]
        public String Area { get; set; }

        [Required]
        [StringLength(128)]
        public String Controller { get; set; }

        [Required]
        [StringLength(128)]
        public String Action { get; set; }
    }
}
