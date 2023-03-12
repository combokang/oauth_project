using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Internal.SSO.Entities.ViewModels
{
    public class ProjectListVM : PagedVM
    {
        [Display(Name = "Id")]
        public Guid Id { get; set; }

        [Display(Name = "名稱")]
        public string Name { get; set; }

        public string Domain { get; set; }
    }
}
