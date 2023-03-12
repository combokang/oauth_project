using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MiddleOffice.Entities.ViewModels
{
    public class PagedVM
    {
        [IgnoreDataMember]
        public int TotalRecords { get; set; }
    }
}
