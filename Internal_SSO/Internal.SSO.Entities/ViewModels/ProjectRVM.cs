using System;
using System.Collections.Generic;
using System.Text;

namespace Internal.SSO.Entities.ViewModels
{
    public class ProjectRVM
    {
        public string Name { get; set; }

        public string UserId { get; set; }

        public string RoleName { get; set; }

        public string UserRoleCreator { get; set; }

        public DateTime? UserRoleCreateTime { get; set; }
    }
}
