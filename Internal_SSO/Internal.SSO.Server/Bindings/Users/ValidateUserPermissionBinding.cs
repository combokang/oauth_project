using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Internal.SSO.Server.Bindings.Users
{
    public class ValidateUserPermissionBinding
    {
        [Required]
        public string ClientId { get; set; }

        [Required]
        public string PermissionKey { get; set; }
    }
}
