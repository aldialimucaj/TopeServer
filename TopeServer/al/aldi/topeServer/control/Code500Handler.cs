using Nancy;
using Nancy.ErrorHandling;
using Nancy.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TopeServer.al.aldi.topeServer.control
{
    public class Code500Hander : IStatusCodeHandler
    {
        public bool HandlesStatusCode(HttpStatusCode statusCode,
                                      NancyContext context)
        {
            return statusCode == HttpStatusCode.InternalServerError;
        }

        public void Handle(HttpStatusCode statusCode, NancyContext context)
        {
            
#if DEBUG
            //var response = new GenericFileResponse("500.html", "text/html");
            var response = new JsonResponse(new { success = false, status = 500, messsage = "internal server error" }, new DefaultJsonSerializer());
#else
            var response = new JsonResponse(new { success = false, status = 500, messsage = "internal server error" }, new DefaultJsonSerializer());
#endif

            response.StatusCode = statusCode;
            context.Response = response;
        }
    }
}
