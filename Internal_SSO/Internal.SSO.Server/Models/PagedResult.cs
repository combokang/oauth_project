using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Internal.SSO.Server.Models
{
    public class PagedResult<T>
    {
        public PagedResult(IEnumerable<T> items, int pageNo, long totalRecordCount)
        {
            Data = new List<T>(items);
            Page = pageNo;
            FilteredTotal = totalRecordCount;
            Total = totalRecordCount;
        }

        [JsonPropertyName("data")]
        public List<T> Data { get; private set; }

        [JsonPropertyName("draw")]
        public int Page { get; private set; }

        [JsonPropertyName("recordsFiltered")]
        public long FilteredTotal { get; private set; }

        [JsonPropertyName("recordsTotal")]
        public long Total { get; private set; }

        [JsonPropertyName("error")]
        public string Error { get; set; }
    }
}
