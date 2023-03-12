using System;
using System.Collections.Generic;
using System.Text;

namespace Internal.SSO.Entities.Models
{
    public class User
    {
        public string Id { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string AdGroupNames { get; set; }
        public int ErrorCount { get; set; }
        public string Creator { get; set; }
        public DateTime CreateTime { get; set; }
        public string Updater { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
