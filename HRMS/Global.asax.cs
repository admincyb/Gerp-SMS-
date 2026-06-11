using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using BusinessObject;
using System.IO;

namespace HRMS
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current.User != null && HttpContext.Current.User.Identity.Name != string.Empty)
            {
                if (HttpContext.Current.User.Identity.AuthenticationType != "Forms")
                    throw new Exception("Only forms authentication is supported, not " + HttpContext.Current.User.Identity.AuthenticationType);
                FormsIdentity UserIdentity = (FormsIdentity)HttpContext.Current.User.Identity;

                if (UserIdentity.Name != null)
                {
                    BusinessObject.User currUser = Newtonsoft.Json.JsonConvert.DeserializeObject<BusinessObject.User>(UserIdentity.Ticket.UserData);
                    List<UserGroup> groupList = BusinessLogic.Login.GetUserGroup(currUser.PKUser);
                    currUser.GroupList = groupList;
                    string strRoles = string.Empty;
                    foreach (var r in groupList)
                        strRoles += r.GroupPK + ",";
                    strRoles = strRoles.Substring(0, strRoles.Length - 1);
                    currUser.Roles = strRoles;
                    HttpContext.Current.User = new BusinessObject.ERPPrincipal(currUser);
                }
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            if (System.Configuration.ConfigurationManager.AppSettings["EnableErrorLog"] != null && Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["EnableErrorLog"]))
            {
                try
                {
                    Exception exc = Server.GetLastError();
                    string url = Server.MapPath("~/ErrorLogs/") + "ApplicationErr.txt";
                    FileStream fs1 = new FileStream(url, FileMode.Append, FileAccess.Write);
                    StreamWriter writer = new StreamWriter(fs1);
                    //writer.Write(exc.StackTrace);
                    //writer.Write(Environment.NewLine);
                    //writer.Write("..........................");
                    //writer.Write(Environment.NewLine);
                    //writer.Write(exc.InnerException);
                    //writer.Close();

                    writer.WriteLine("********** {0} **********", DateTime.Now);
                    if (exc.InnerException != null)
                    {
                        writer.Write("Inner Exception Type: ");
                        writer.WriteLine(exc.InnerException.GetType().ToString());
                        writer.Write("Inner Exception: ");
                        writer.WriteLine(exc.InnerException.Message);
                        writer.Write("Inner Source: ");
                        writer.WriteLine(exc.InnerException.Source);
                        if (exc.InnerException.StackTrace != null)
                        {
                            writer.WriteLine("Inner Stack Trace: ");
                            writer.WriteLine(exc.InnerException.StackTrace);
                        }
                    }
                    writer.Write("Exception Type: ");
                    writer.WriteLine(exc.GetType().ToString());
                    writer.WriteLine("Exception: " + exc.Message); ;
                    // writer.WriteLine("Hash Code: " + exc.GetHashCode());
                    writer.WriteLine("Stack Trace: ");
                    if (exc.StackTrace != null)
                    {
                        writer.WriteLine(exc.StackTrace);
                        writer.WriteLine();
                    }
                    writer.Close();
                }
                catch (Exception) { }
            }
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}