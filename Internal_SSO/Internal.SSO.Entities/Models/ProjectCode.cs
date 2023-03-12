using System;
using System.Collections.Generic;
using System.Text;

namespace Internal.SSO.Entities.Models
{
    public class ProjectCode
    {
        public Guid Id { get; set; }

        public Guid ProjectId { get; set; }

        public string UserId { get; set; }

        public string Code { get; set; }

        public DateTime Expires { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
