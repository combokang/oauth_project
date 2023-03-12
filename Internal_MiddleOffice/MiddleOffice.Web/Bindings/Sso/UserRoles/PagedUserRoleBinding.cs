using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MiddleOffice.Web.Bindings.Sso.UserRoles
{
    public class PagedUserRoleBinding : PagedResultRequestBinding
    {
        public string UserId { get; set; }

        public Guid ProjectId { get; set; }
    }
}
