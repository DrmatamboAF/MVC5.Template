using System;
using System.ComponentModel.DataAnnotations;

namespace MvcTemplate.Objects
{
    public class AccountEditView : BaseView
    {
        [Required]
        [Editable(false)]
        public String Username { get; set; }

        [Required]
        [Editable(false)]
        public String Email { get; set; }

        public Boolean IsLocked { get; set; }

        public String RoleId { get; set; }
    }
}
