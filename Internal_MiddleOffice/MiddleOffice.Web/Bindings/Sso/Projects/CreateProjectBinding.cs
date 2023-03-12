using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MiddleOffice.Web.Bindings.Sso.Projects
{
    public class CreateProjectBinding
    {
        [Required]
        [Display(Name = "名稱")]
        public string Name { get; set; }

        public string Domain { get; set; }

        public string CallbackUrls { get; set; }
    }
}
