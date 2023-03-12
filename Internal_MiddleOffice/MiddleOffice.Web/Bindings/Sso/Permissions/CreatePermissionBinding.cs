using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MiddleOffice.Web.Bindings.Sso.Permissions
{
    public class CreatePermissionBinding
    {
        [Required]
        public string CandidateKey { get; set; }

        [Required]
        public string Name { get; set; }


        public string Description { get; set; }

        [Required]
        public Guid ProjectId { get; set; }
    }
}
