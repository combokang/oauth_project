using System;
using System.Collections.Generic;
using System.Text;

namespace MiddleOffice.Entities.ViewModels.Sso
{
    public class PermissionListVM : PagedVM
    {
        public Guid Id { get; set; }
        public string CandidateKey { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
