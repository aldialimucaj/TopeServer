using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace TopeServer.al.aldi.topeServer.model
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class TopeAction
    {
        [Key]
        [JsonProperty()]
        public Int64 actionId { get; set; }
        [JsonProperty()]
        public String module { get; set; }
        [JsonProperty()]
        public String method { get; set; }
        [JsonProperty()]
        public String commandFullPath { get; set; }
        [JsonProperty()]
        Int64 itemId { get; set; }
        [JsonProperty()]
        public String title { get; set; }
        [JsonProperty()]
        public String active { get; set; }
        [JsonProperty()]
        public Int64 oppositeActionId { get; set; }
    }
}
