using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MiddleOffice.Web.Bindings.Sso.UserRoles
{
    public class EditUserRoleBinding
    {
        [Required]
        public string UserId { get; set; }

        public string UserName { get; set; }

        public List<UserRoleBinding> UserRoles { get; set; } = new List<UserRoleBinding>();
    }

    public class UserRoleBinding
    {
        public string ProjectName { get; set; }

        public Guid RoleId { get; set; }

        public string RoleName { get; set; }

        public bool Checked { get; set; }
    }
}
