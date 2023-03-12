using System;
using System.Collections.Generic;
using System.Text;

namespace Internal.SSO.Entities.Models
{
    public class RolePermission
    {
        public Guid Id { get; set; }
        public Guid RoleId { get; set; }
        public Guid PermissionId { get; set; }
    }
}
