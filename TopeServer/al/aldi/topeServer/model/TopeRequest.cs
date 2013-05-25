using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;


namespace TopeServer.al.aldi.topeServer.model
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class TopeRequest
    {
        [JsonProperty()]
        public Int64 topeRequestId { get; set; }
        [JsonProperty()]
        public Int64 actionId { get; set; }
        [JsonProperty()]
        public bool success { get; set; }
        [NotMapped]
        [JsonProperty()]
        public bool authenticated { get; set; }
        /*TODO: REMOVE*/
        [NotMapped]
        [JsonProperty()]
        public String command { get; set; }
        [JsonProperty()]
        public String requestHash { get; set; }
        [JsonProperty()]
        public String responseHash { get; set; }
        [JsonProperty()]
        public String message { get; set; }
        [JsonProperty()]
        public DateTime date { get; set; }
        [JsonProperty()]
        public DateTime timeToExecute { get; set; }
        [JsonProperty()]
        public long timeToWait { get; set; }
        [JsonProperty()]
        public String user { get; set; }
        /* WARNING: It must not be mapped */
        [NotMapped]
        [JsonProperty()]
        public String password { get; set; }
        [JsonProperty()]
        public String domain { get; set; }
        [JsonProperty()]
        public String arg0 { get; set; }
        [JsonProperty()]
        public String arg1 { get; set; }
        [JsonProperty()]
        public String arg2 { get; set; }
        [JsonProperty()]
        public String arg3 { get; set; }
        [JsonProperty()]
        public String arg4 { get; set; }




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
