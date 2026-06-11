using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace ERPSMS_v01.Administration
{
    public partial class AdminMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int HostIndex = 0;
            if (HttpContext.Current.Request.ApplicationPath == "/")
            {
                HostIndex = 9;
            }
            else
            {
                HostIndex = HttpContext.Current.Request.Url.ToString().ToUpper().IndexOf(HttpContext.Current.Request.ApplicationPath.ToUpper()) + 1;
            }
            string ScriptRoot = HttpContext.Current.Request.Url.ToString().Substring(0, HttpContext.Current.Request.Url.ToString().IndexOf("/", HostIndex)) + "/Scripts/";
            System.Text.StringBuilder sbScript = new System.Text.StringBuilder();
            sbScript.AppendLine("<script src=\"" + ScriptRoot + "jquery/jquery-1.5.min.js\" type='text/javascript'></script>");
            sbScript.AppendLine("<script src=\"" + ScriptRoot + "jquery/UI/jquery-ui.min.js\" type='text/javascript'></script>");
            sbScript.AppendLine("<script src=\"" + ScriptRoot + "jquery/Validate/jquery.validate.js.axd\" type='text/javascript'></script>");
            sbScript.AppendLine("<script src=\"" + ScriptRoot + "GrandGridMulti.js\" type='text/javascript'></script>");
            sbScript.AppendLine("<script src=\"" + ScriptRoot + "roundedbox.js\" type='text/javascript'></script>");
            sbScript.AppendLine("<script src=\"" + ScriptRoot + "jquery/json2.js\" type='text/javascript'></script>");
            sbScript.AppendLine("<script src=\"" + ScriptRoot + "GrandScriptUtils.js.axd\" type='text/javascript'></script>");
            sbScript.AppendLine("<script src=\"" + ScriptRoot + "GrandTreeMulti.js\" type='text/javascript'></script>");
            sbScript.AppendLine("<script src=\"" + ScriptRoot + "PageScript/MasterPage.js.axd\" type='text/javascript'></script>");
            //Adding script to the page assigning to the hidden field
            this.scriptSrc.Text = sbScript.ToString();
            if (!Page.IsPostBack)
            {
                BusinessObject.User myUser = (BusinessObject.User)Context.User.Identity;
                UserPk.Value = myUser.PKUser.ToString();
                lblUserName.Text = "Welcome " + myUser.EmpName;
            }
        }

        protected void Home_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/Administration/Default.aspx");
        }

        protected void Inbox_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/AccountManagement/WorkflowInbox.aspx");
        }

        /// <summary>
        /// Event for logout button
        /// </summary>
        /// <returns></returns>

        /// <summary>
        /// Event for logout button
        /// </summary>
        /// <returns></returns>
        protected void Logout_Click(object sender, ImageClickEventArgs e)
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            string nextpage = "~/login.aspx";
            Response.Write("<SCRIPT LANGUAGE=javascript>");
            Response.Write("{");
            Response.Write(" var Backlen=history.length;");
            Response.Write(" history.go(-Backlen);");
            Response.Write(" window.location.href='" + nextpage + "'; ");
            Response.Write("}");
            Response.Write("</SCRIPT>");
            if (System.Configuration.ConfigurationManager.AppSettings["ASSETURL"] != null)
            {
                Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["ASSETURL"] + "?Logout=1");
            }
            else if (System.Configuration.ConfigurationManager.AppSettings["CUSTPORTALURL"] != null)
            {
                Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["CUSTPORTALURL"] + "?Logout=1");
            }
            else if (System.Configuration.ConfigurationManager.AppSettings["ERPURL"] != null)
            {
                Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["ERPURL"] + "?Logout=1");
            }
            else
            {
                FormsAuthentication.RedirectToLoginPage();
            }
        }

        protected void lnbLogout_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            string nextpage = "~/login.aspx";

            Response.Write("<SCRIPT LANGUAGE=javascript>");
            Response.Write("{");
            Response.Write(" var Backlen=history.length;");
            Response.Write(" history.go(-Backlen);");
            Response.Write(" window.location.href='" + nextpage + "'; ");
            Response.Write("}");
            Response.Write("</SCRIPT>");
            if (System.Configuration.ConfigurationManager.AppSettings["ASSETURL"] != null)
            {
                Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["ASSETURL"] + "?Logout=1");
            }
            else if (System.Configuration.ConfigurationManager.AppSettings["CUSTPORTALURL"] != null)
            {
                Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["CUSTPORTALURL"] + "?Logout=1");
            }
            else if (System.Configuration.ConfigurationManager.AppSettings["ERPURL"] != null)
            {
                Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["ERPURL"] + "?Logout=1");
            }
            else
            {
                FormsAuthentication.RedirectToLoginPage();
            }
        }
    }
}