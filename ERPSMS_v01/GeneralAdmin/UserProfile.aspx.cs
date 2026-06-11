using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERP.Utilities;
using System.Web.Security;

namespace ERPSMS_v01.GeneralAdmin
{
    public partial class UserProfile : System.Web.UI.Page//ERP.Store.UI.MyBasePage
    {
        #region Variable & Properties
        private ControlsEnum controlActions;
        BusinessObject.User objUser;
        #endregion
        #region PageLevel Events
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            objUser = GetUserIdentity();
            // Set the User Theme
            if (!string.IsNullOrEmpty(objUser.Theme))
            {
                Page.Theme = objUser.Theme;
            }
            else
            {
                Page.Theme = "ClassicExt";
            }
            //Page.Theme = "NewTheme";

        }
        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);

            if (!IsPostBack)
            {
                //assign Page BreadCrumb
                AssignBreadCrumb();
            }
            InitializeCulture();
        }
        /// <summary>
        /// assign Page BreadCrumb
        /// </summary>
        public virtual void AssignBreadCrumb()
        {
            try
            {
                if (this.GetLocalResourceObject("Breadcrumb") != null && (WebControl)(Page.Form.FindControl("MainContent").FindControl("lblBreadCrum")) != null)
                {
                    string breadCrumb;
                    breadCrumb = this.GetLocalResourceObject("Breadcrumb").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
                    ((Label)(Page.Form.FindControl("MainContent").FindControl("lblBreadCrum"))).Text = breadCrumb;
                }
            }
            catch (Exception ex)
            {

            }
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
            try
            {
                if (!IsPostBack)
                {
                    divFrame.Visible = true;
                    objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                    string strTabUrlChangePwd = GetGlobalResourceObject("PageURL", "ChangePwdUser").ToString() + objUser.PKUser.ToString();

                  //  string strTabUrl = GetGlobalResourceObject("PageURL", "EditUser").ToString() + objUser.PKUser.ToString() + "&UserProphile=1";
                    Response.Redirect(strTabUrlChangePwd);
                   // frmDetails.Attributes.Add("src", strTabUrl);
                  //  ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SetTab", "SetTabs(1);", true);
                }
            }
            catch (Exception ex)
             {
             }
            finally
             {

             }
        }
       
        #endregion
        #region PageActionHandler

        /// <summary>
        /// Handle All page Related Events
        /// </summary>
        private void PageActionHandler()
        {
            try
            {
          
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
            finally
            {
            }
        }
        #endregion

        #region Action Handlers
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, EventArgs e)
        {
            try
            {
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    controlActions = (ControlsEnum)(Enum.Parse(typeof(ControlsEnum), ((Button)sender).CommandName));
                }
                if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                {
                    controlActions = (ControlsEnum)(Enum.Parse(typeof(ControlsEnum), ((LinkButton)sender).CommandName));
                }
                if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    controlActions = (ControlsEnum)(Enum.Parse(typeof(ControlsEnum), ((ImageButton)sender).CommandName));
                }
                switch (controlActions)
                { 
                    case ControlsEnum.EDIT:
                        divFrame.Visible = true;
                        objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                        string strTabUrl = GetGlobalResourceObject("PageURL", "EditUser").ToString() + objUser.PKUser.ToString()+"&UserProphile=1";
                        //frmDetails.Attributes.Add("src", strTabUrl);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SetTab", "SetTabs(1);", true);
                        break;
                    case ControlsEnum.INBOXMAPPING:
                        divFrame.Visible = true;
                        objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                        string strTabUrlInboxMap = GetGlobalResourceObject("PageURL", "InboxMappingUser").ToString() + objUser.PKUser.ToString() + "&UserProphile=1";
                    //    frmDetails.Attributes.Add("src", strTabUrlInboxMap);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SetTab", "SetTabs(2);", true);
                        break;
                    case ControlsEnum.CHANGEPASSWORD:
                        divFrame.Visible = true;
                        objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                        string strTabUrlChangePwd = GetGlobalResourceObject("PageURL", "ChangePwdUser").ToString() + objUser.PKUser.ToString() + "&UserProphile=1";
                     //   frmDetails.Attributes.Add("src", strTabUrlChangePwd);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SetTab", "SetTabs(3);", true);
                        break;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "AlertMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Captions.Information + "');", true);
            }
            finally
            {

            }
        }
        #endregion

        #region Enum
        /// <summary>
        ///Page Controls Enum 
        /// </summary>
        public enum ControlsEnum
        {
            EDIT,
            CANCEL,
            INBOXMAPPING,
            CHANGEPASSWORD,
            ACTIVITIES
        }
        #endregion

    }
}