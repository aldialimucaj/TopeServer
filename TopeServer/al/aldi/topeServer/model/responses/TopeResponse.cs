using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TopeServer.al.aldi.topeServer.model
{
    /// <summary>
    /// Base class for JSON responses. It implements the basic attributes for the response. 
    /// The deriving subclasses have to implement the payload wether by adding a simple object
    /// or by implementing a new class and passing it as payload.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class TopeResponse : ITopeResponse
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
        /* every object has a payload. the implementation is up to the subclasses */
        [JsonProperty()]
        public Object payload { get; set; }

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

        public TopeResponse(Object payload)
            : this()
        {
            this.payload = payload;
        }

        public Object getPayload() 
        {
            return payload;
        }
        public void setPayload(Object payload)
        {
            this.payload = payload;
        }

        public bool isSuccessful()
        {
            return success;
        }

        public void setSuccess(bool success)
        {
            this.success = success;
        }
    }
}
