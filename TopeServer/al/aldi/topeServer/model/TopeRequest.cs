﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace TopeServer.al.aldi.topeServer.model
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class TopeRequest
    {
        [JsonProperty()]
        public bool success { get; set; }
        [JsonProperty()]
        public String command { get; set; }
        [JsonProperty()]
        public String requestId { get; set; }
        [JsonProperty()]
        public String responseId { get; set; }
        [JsonProperty()]
        public DateTime date { get; set; }
        [JsonProperty()]
        public int timeToExecute { get; set; }


        public TopeRequest()
        {
        }

        public TopeRequest(bool success)
            : this()
        {
            this.success = success;
        }
    }
}
