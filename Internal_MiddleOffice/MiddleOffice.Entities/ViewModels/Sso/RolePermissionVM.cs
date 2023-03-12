using System;
using System.Collections.Generic;
using System.Text;

namespace MiddleOffice.Entities.ViewModels.Sso
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
