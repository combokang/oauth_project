using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MiddleOffice.Web.Bindings.Sso.Permissions
{
    public class EditPermissionBinding
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string CandidateKey { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
