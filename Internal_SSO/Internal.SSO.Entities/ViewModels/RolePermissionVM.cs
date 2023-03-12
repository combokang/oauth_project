using System;
using System.Collections.Generic;
using System.Text;

namespace Internal.SSO.Entities.ViewModels
{
    public class RolePermissionVM
    {
        public bool Checked { get; set; }

        public Guid PermissionId { get; set; }

        public string PermissionCandidateKey { get; set; }

        public string PermissionName { get; set; }

        public string PermissionDescription { get; set; }
    }
}
