using System;
using System.Collections;
using System.Linq;
using System.Text;
using Nancy;
using Nancy.Responses.Negotiation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TopeServer.al.aldi.topeServer.model;


namespace TopeServer.al.aldi.topeServer.control
{
    class TopeResponseNegotiator 
    {
        private Negotiator response = null;
        public Negotiator Response { get { return getResponse(); } set {response = value;}}
        private TopeResponse model = null;
        

        public TopeResponseNegotiator(Negotiator negotiator, TopeResponse topeResponse)
        {

            this.Response = negotiator;
            this.model = topeResponse;

            this.response.WithStatusCode(HttpStatusCode.OK); /* setting the status code */
        }

        public Negotiator getResponse()
        {
            /* generation the json object out of the response*/
            /* putting the json model */
            String json = JsonConvert.SerializeObject(model);
            response.WithModel(json); 
            return response;
        }



    }
}
