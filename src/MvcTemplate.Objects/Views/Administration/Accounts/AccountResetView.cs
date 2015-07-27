using MvcTemplate.Components.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace MvcTemplate.Objects
{
    public class AccountResetView : BaseView
    {
        [Required]
        [StringLength(128)]
        public String Token { get; set; }

        [Required]
        [NotTrimmed]
        [StringLength(128)]
        public String NewPassword { get; set; }
    }
}
