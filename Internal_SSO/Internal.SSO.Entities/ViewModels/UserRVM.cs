using System;
using System.Collections.Generic;
using System.Text;

namespace Internal.SSO.Entities.ViewModels
{
    public class UserRVM
    {
        public string Id { get; set; }

        public string ProjectName { get; set; }

        public string RoleName { get; set; }

        public string UserRoleCreator { get; set; }

        public DateTime? UserRoleCreateTime { get; set; }
    }
}
