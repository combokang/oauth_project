using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MiddleOffice.Web.Bindings.Sso.Roles
{
    public class PagedRoleBinding : PagedResultRequestBinding
    {
        [Required]
        public Guid ProjectId { get; set; }
    }
}
