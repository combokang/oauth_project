using System;
using System.Collections.Generic;
using System.Text;

namespace MiddleOffice.Entities.Models.Sso
{
    public class Project
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Domain { get; set; }
        public string CallbackUrls { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public bool IsDeleted { get; set; }
        public string Creator { get; set; }
        public DateTime CreateTime { get; set; }
        public string Updater { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
