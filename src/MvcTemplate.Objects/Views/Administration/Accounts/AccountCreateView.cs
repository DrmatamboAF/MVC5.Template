using MvcTemplate.Components.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace MvcTemplate.Objects
{
    public class AccountCreateView : BaseView
    {
        [Required]
        [StringLength(128)]
        public String Username { get; set; }

        [Required]
        [NotTrimmed]
        [StringLength(128)]
        public String Password { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(256)]
        public String Email { get; set; }

        public String RoleId { get; set; }
    }
}
