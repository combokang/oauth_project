using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MiddleOffice.Entities.ViewModels.Sso
{
    public class ProjectInfoListVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Domain { get; set; }
    }
}
