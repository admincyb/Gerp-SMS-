using System;
using System.Web;
using System.Threading;
using System.Collections;
using System.Configuration;

namespace GTIService.Security
{
    public class AuthenticationModule:IHttpModule
    {
        HttpApplication ERPApp = null;
        public void Dispose()
        {
            
        }

        public void Init(HttpApplication context)
        {
            ERPApp = context;
            ERPApp.AuthenticateRequest += new EventHandler(ERPApp_AuthenticateRequest);
        }

        void ERPApp_AuthenticateRequest(object sender, EventArgs e)
        {
            HttpRequest Request = ERPApp.Request;
                HttpResponse Response = ERPApp.Response;
                HttpContext CurrentContext = ERPApp.Context;
            try
            {
                
                if (Request.Url.AbsolutePath.ToLower().EndsWith(".do"))
                {
                    if (CurrentContext.User.Identity.IsAuthenticated)
                    {
                        if (Request.Headers["Referer"].ToLower().Contains(".aspx"))
                        {
                            if (Request.PhysicalApplicationPath == null || Request.PhysicalApplicationPath == string.Empty)
                            {
                                SendErrorResponse(Response);
                            }
                        }
                        else
                        {
                            SendErrorResponse(Response);
                        }
                    }
                    else
                    {
                        SendErrorResponse(Response);
                    }
                }
            }
            catch (Exception ex)
            {
                SendErrorResponse(Response);
                
            }
         
            
            
        }
        private void SendErrorResponse(HttpResponse Response)
        {
            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("~/login.aspx");
            Response.End();
        }
    }
}
