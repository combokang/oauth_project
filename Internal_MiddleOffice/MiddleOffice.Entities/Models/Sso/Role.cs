using System;
using System.Collections.Generic;
using System.Text;

namespace MiddleOffice.Entities.Models.Sso
{
    public class Role
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid ProjectId { get; set; }
        public string Creator { get; set; }
        public DateTime CreateTime { get; set; }
        public string Updater { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
