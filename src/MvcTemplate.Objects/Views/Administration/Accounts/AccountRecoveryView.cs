using System;
using System.ComponentModel.DataAnnotations;

namespace MvcTemplate.Objects
{
    public class AccountRecoveryView : BaseView
    {
        [Required]
        [EmailAddress]
        [StringLength(256)]
        public String Email { get; set; }
    }
}
