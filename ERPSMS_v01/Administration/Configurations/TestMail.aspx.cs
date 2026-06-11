using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.AccountManagement;
using System.Web.Security;
using ERP.Utilities;
using System.Configuration;

namespace ERPSMS_v01.Administration.Configurations
{
    public partial class TestMail : ERP.Store.UI.MyBasePage
    {
        #region Variables and Properties
        // Holds the current logged in user
        private BusinessObject.User currentUser;

        // Indicates the state as well as action
        private ActionsEnum commonActions;
        #endregion

        #region Set Page Variables
        /// <summary>
        /// Set the Page Level variables and properties
        /// </summary>
        private void SetPageVariables()
        {
            //Initialze the current logged in user to the currentUser variable
            currentUser = (BusinessObject.User)HttpContext.Current.User.Identity;

        }
        #endregion

        #region Action Handlers

        #region -- For Buttons ---
        /// <summary>
        /// For Button Click (Save/Cancel)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, EventArgs e)
        {

            commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
            switch (commonActions)
            {   //change the old password to new password
                case ActionsEnum.CHANGE:

                    SendTestMail(txtMailID.Text.Trim());
                    ResetForm();
                    break;
                case ActionsEnum.CANCEL:
                    Response.RedirectPermanent(FormsAuthentication.DefaultUrl, true);
                    break;
            }
        }

        private void SendTestMail(string toMailID)
        {
            bool result = CommonFunctions.SendMail(GetLocalResourceObject("MailSubject").ToString(), GetLocalResourceObject("MailMatter").ToString(), toMailID, ConfigurationManager.AppSettings["FrmMailResetPwd"]);
            if (result)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("sendSuccess").ToString()) + "','" + Resources.ErpRes.Title_Information.ToString() + "');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("sendFailed").ToString()) + "','" + Resources.ErpRes.Title_Information.ToString() + "');", true);
            }

        }
        #endregion

        #endregion

        #region Page Level Events
        /// <summary>
        /// To handle pageload event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageVariables();
            if (!IsPostBack)
            {
                //FindControl the controls in master pages
                LinkButton lnkBtnMenu = (LinkButton)Master.FindControl("lnkMenu");
                ImageButton imgBtnHelp = (ImageButton)Master.FindControl("imgBtnHelp");
                ImageButton imgbtnChangePw = (ImageButton)Master.FindControl("imgbtnChangePwd");
                AssignBreadCrumb();
                //if (currentUser.IsCCOwner && !currentUser.IsAllCCInitiated)
                //{    //Invisible master page button controls 
                //    lnkBtnMenu.Visible = false;
                //    imgbtnChangePw.Visible = false;
                //}
                //else
                //{
                //    lnkBtnMenu.Visible = true;
                //    imgbtnChangePw.Visible = true;
                //}
                ScriptManager.GetCurrent(this).SetFocus(txtMailID);
            }
        }
        #endregion

        #region Helper Methods

        /// <summary>
        /// Resets the form for a fresh entry
        /// </summary>
        private void ResetForm()
        {
            //Codes for Clearing the controls in the page                      
            txtMailID.Text = string.Empty;
        }

        public void AssignBreadCrumb()
        {
            try
            {
                if (this.GetLocalResourceObject("Breadcrumb") != null && ((Label)this.Master.FindControl("lblBreadCrum")) != null)
                {
                    string breadCrumb;
                    breadCrumb = this.GetLocalResourceObject("Breadcrumb").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;&#9658;</label>");
                    ((Label)this.Master.FindControl("lblBreadCrum")).Text = breadCrumb;
                }
            }
            catch
            {

            }


        }

        #endregion
    }
}