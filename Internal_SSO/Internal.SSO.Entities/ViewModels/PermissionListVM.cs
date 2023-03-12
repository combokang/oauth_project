using System;
using System.Collections.Generic;
using System.Text;

namespace Internal.SSO.Entities.ViewModels
{
    public class PermissionListVM : PagedVM
    {
        public Guid Id { get; set; }
        public string CandidateKey { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
