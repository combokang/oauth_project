using System;
using System.Collections.Generic;
using System.Text;

namespace Internal.SSO.Entities.Models
{
    public class UserAccessLog
    {
        public int Id { get; set; }
        public int ActionId { get; set; }
        public string Memo { get; set; }
        public string UserId { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
