using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace MiddleOffice.Web.Models
{
    public class ErrorResponseVM
    {
        public string ErrorCode { get; set; } = string.Empty;
        public string Message { get; set; }
        public List<FormError> FormErrors { get; set; } = new List<FormError>();

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }

    public class FormError
    {
        public string Key { get; set; }
        public List<string> Errors { get; set; }
    }
}
