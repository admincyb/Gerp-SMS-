using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Handlers
{
    public class HandlerClass : IHttpHandlerFactory
    {
        public IHttpHandler GetHandler(HttpContext context, string requestType, String url, String pathTranslated)
        {
            IHttpHandler handlerToReturn;
            if ("get" == context.Request.RequestType.ToLower())
            {
                return new Handlers();
            }
            else if ("post" == context.Request.RequestType.ToLower())
            {
                return new Handlers();
            }
            else
            {
                handlerToReturn = null;
            }
            return handlerToReturn;
        }
        public void ReleaseHandler(IHttpHandler handler)
        {
        }

        public bool IsReusable
        {
            get { return false; }
        }


    }
}