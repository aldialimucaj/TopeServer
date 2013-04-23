using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TopeServer.al.aldi.topeServer.model
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class TopeResponse
    {
        [JsonProperty()]
        public bool success { get; set; }
        [JsonProperty()]
        public String command { get; set; }
        [JsonProperty()]
        public String message { get; set; }
        [JsonProperty()]
        public String requestId { get; set; }
        [JsonProperty()]
        public String responseId { get; set; }
        [JsonProperty()]
        public String date { get; set; }

        public TopeResponse()
        {
            date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            requestId = "DUMMY_HASH";
            responseId = "DUMMY_HASH";
        }

        public TopeResponse(bool success) : this()
        {
            this.success = success;
        }
    }
}
