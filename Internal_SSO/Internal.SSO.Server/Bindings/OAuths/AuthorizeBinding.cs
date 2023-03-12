using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Internal.SSO.Server.Bindings.OAuths
{
    public class AuthorizeBinding
    {
        [Required]
        public int TypeId { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public Guid ProjectId { get; set; }

        [Required]
        public string RedirectUri { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string State { get; set; }
    }
}
