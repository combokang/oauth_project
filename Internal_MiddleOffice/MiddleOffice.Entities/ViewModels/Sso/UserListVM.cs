using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MiddleOffice.Entities.ViewModels.Sso
{
    public class UserListVM : PagedVM
    {
        [Display(Name = "帳號")]
        public string Id { get; set; }

        [Display(Name = "名稱")]
        public string Name { get; set; }

        public int ErrorCount { get; set; }
    }
}
