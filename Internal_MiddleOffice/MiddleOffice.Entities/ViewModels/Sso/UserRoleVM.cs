using System;
using System.Collections.Generic;
using System.Text;

namespace MiddleOffice.Entities.ViewModels.Sso
{
    public class UserRoleVM
    {
        public string ProjectName { get; set; }

        public Guid RoleId { get; set; }

        public string RoleName { get; set; }

        public bool Checked { get; set; }

        public string Creator { get; set; }

        public DateTime? CreateTime { get; set; }
    }
}
