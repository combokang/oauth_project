using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Internal.SSO.Server.Bindings.Roles
{
    public class CreateRoleBinding
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public Guid ProjectId { get; set; }

        public List<Guid> PermissionIds { get; set; } = new List<Guid>();
    }
}
