using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MiddleOffice.Web.Bindings.Sso.Roles
{
    public class EditRoleBinding
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public List<EditRolePermissionBinding> EditRolePermissions { get; set; } = new List<EditRolePermissionBinding>();
    }

    public class EditRolePermissionBinding
    {
        [Required]
        public bool Checked { get; set; }

        [Required]
        public Guid PermissionId { get; set; }

        public string PermissionCandidateKey { get; set; }

        public string PermissionName { get; set; }

        public string PermissionDescription { get; set; }
    }
}
