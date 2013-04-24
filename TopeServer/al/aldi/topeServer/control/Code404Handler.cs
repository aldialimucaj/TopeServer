using Nancy;
using Nancy.ErrorHandling;
using Nancy.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TopeServer.al.aldi.topeServer.control
{
    public class Code404Hander : IStatusCodeHandler
    {
        public bool HandlesStatusCode(HttpStatusCode statusCode,
                                      NancyContext context)
        {
            return statusCode == HttpStatusCode.NotFound;
        }

        public void Handle(HttpStatusCode statusCode, NancyContext context)
        {
            var response = new JsonResponse(new { success = false, status = 404, messsage = "not found" }, new DefaultJsonSerializer());
            response.StatusCode = statusCode;
            context.Response = response;
        }
    }
}
