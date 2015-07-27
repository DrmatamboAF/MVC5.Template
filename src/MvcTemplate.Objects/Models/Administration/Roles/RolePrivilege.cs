using System;
using System.ComponentModel.DataAnnotations;

namespace MvcTemplate.Objects
{
    public class RolePrivilege : BaseModel
    {
        [Required]
        public String RoleId { get; set; }
        public virtual Role Role { get; set; }

        [Required]
        public String PrivilegeId { get; set; }
        public virtual Privilege Privilege { get; set; }
    }
}
