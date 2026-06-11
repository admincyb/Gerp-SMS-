using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace ERPSMS_v01
{
    public partial class Default : System.Web.UI.Page
    {
        BusinessObject.User currentUser;
        /// <summary>
        /// To Handle Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(Object sender, EventArgs e)
        {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideProgress", "$(document).ready(function () {$('#updateProgress').hide();});", true);
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            currentUser = GetUserIdentity();
            // Set the User Theme
            if (!string.IsNullOrEmpty(currentUser.Theme))
            {
                Page.Theme = currentUser.Theme;
            }
            else
            {
                Page.Theme = "ClassicExt";
            }
            //Page.Theme = "NewTheme";

        }
        /// <summary>
        /// Get User Identity
        /// </summary>
        /// <returns></returns>
        public BusinessObject.User GetUserIdentity()
        {
            try
            {
                return (((BusinessObject.User)(HttpContext.Current.User.Identity)));
            }
            catch (Exception ex)
            {
                Session.Abandon();
                FormsAuthentication.SignOut();
                Response.Redirect("~/Login.aspx");
            }
            return null;
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}