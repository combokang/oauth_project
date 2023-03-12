using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MiddleOffice.Web.Bindings.Sso.Permissions
{
    public class PagedPermissionBinding : PagedResultRequestBinding
    {
        [Required]
        public Guid ProjectId { get; set; }
    }
}
