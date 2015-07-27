using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcTemplate.Objects
{
    public class Account : BaseModel
    {
        [Required]
        [StringLength(128)]
        [Index(IsUnique = true)]
        public String Username { get; set; }

        [Required]
        [StringLength(512)]
        public String Passhash { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(256)]
        [Index(IsUnique = true)]
        public String Email { get; set; }

        public Boolean IsLocked { get; set; }

        [StringLength(128)]
        public String RecoveryToken { get; set; }
        public DateTime? RecoveryTokenExpirationDate { get; set; }

        public String RoleId { get; set; }
        public virtual Role Role { get; set; }
    }
}
