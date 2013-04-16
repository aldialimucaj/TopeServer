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
        public String requestId { get; set; }
        public String responseId { get; set; }
        public DateTime date { get; set; }

        public TopeResponse()
        {
            date = new DateTime();
            requestId = "DUMMY_HASH";
            responseId = "DUMMY_HASH";
        }

        public TopeResponse(bool success) : this()
        {
            this.success = success;
        }
    }
}
