using Nancy;
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
        public Int64 topeRequestId { get; set; }    /* unique ID */
        [JsonProperty()]
        public Int64 actionId { get; set; }         /* action ID */
        [JsonProperty()]
        public bool success { get; set; }           /* mark if successfully execute */
        [JsonProperty()]
        public Int64 executed { get; set; }         /* mark > 0 if alredy executed */
        [JsonProperty()]
        public Int64 repeat { get; set; }           /* mark if execution is to be repeted. how many times > 0. arg0 interval */
        [JsonProperty()]
        public bool persistent { get; set; }        /* even after restart or program closed */
        [JsonProperty()]
        public bool authenticated { get; set; }     /* was it an authenticated call */
        /*TODO: REMOVE*/
        [NotMapped]
        [JsonProperty()]
        public String method { get; set; }         /* the command which must be the same as the method name */
        [JsonProperty()]
        public String requestHash { get; set; }
        [JsonProperty()]
        public String responseHash { get; set; }
        [JsonProperty()]
        public String message { get; set; }
        [JsonProperty()]
        public DateTime date { get; set; }
        [NotMapped]
        public Request nancyRequest { get; set; }
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
