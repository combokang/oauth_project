using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Internal.SSO.Server.Bindings.Projects
{
    public class EditProjectBinding
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Domain { get; set; }

        public string CallbackUrls { get; set; }
    }
}
