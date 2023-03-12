using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MiddleOffice.Web.Bindings.Sso.Roles
{
    public class CreateRoleBinding
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public Guid ProjectId { get; set; }

        public List<CreateRolePermissionBinding> CreateRolePermissions { get; set; } = new List<CreateRolePermissionBinding>();
    }

    public class CreateRolePermissionBinding
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
