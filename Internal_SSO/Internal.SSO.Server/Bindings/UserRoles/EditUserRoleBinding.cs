using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Internal.SSO.Server.Bindings.UserRoles
{
    public class EditUserRoleBinding
    {
        [Required]
        public string UserId { get; set; }

        public List<UserRoleBinding> UserRoles { get; set; } = new List<UserRoleBinding>();
    }

    public class UserRoleBinding
    {
        public string UserId { get; set; }

        public Guid RoleId { get; set; }
    }
}
