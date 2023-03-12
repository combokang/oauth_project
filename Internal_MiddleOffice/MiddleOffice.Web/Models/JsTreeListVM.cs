using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MiddleOffice.Web.Models
{
    public class JsTreeListVM
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("parent")]
        public string ParentId { get; set; }

        [JsonPropertyName("icon")]
        public string Icon { get; set; }

        [JsonPropertyName("text")]
        public string Display { get; set; }

        [JsonPropertyName("state")]
        public JsTreeListState State { get; set; } = new JsTreeListState();

        [JsonPropertyName("a_attr")]
        public object AAttr { get; set; }
    }

    public class JsTreeListState
    {
        [JsonPropertyName("opened")]
        public bool Opened { get; set; } = true;

        [JsonPropertyName("selected")]
        public bool Selected { get; set; } = false;

        [JsonPropertyName("disabled")]
        public bool Disabled { get; set; } = false;
    }
}
