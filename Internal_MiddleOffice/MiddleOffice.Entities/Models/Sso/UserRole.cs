using System;
using System.Collections.Generic;
using System.Text;

namespace MiddleOffice.Entities.Models.Sso
{
    public class UserRole
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public Guid RoleId { get; set; }
        public string Creator { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
