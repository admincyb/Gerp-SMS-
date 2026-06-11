using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERP.Utilities;
using System.Reflection;
using System.Configuration;
using ERP.Utilities.Constants.DA;
using BusinessObject.AccountManagement;
using BusinessLogic.AccountManagement;
using BusinessObject.CommonManagement;

namespace ERPSMS_v01.AccountManagement
{
    public partial class ResetPassword : System.Web.UI.Page
    {
        #region Variables and Properties
        // Indicates the state as well as action
        private ActionsEnum commonActions;
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
            string PwdObj;
            ChangePasswordBO objChangePwd;
            CryptoServices objCryptoServices;
            objCryptoServices = new CryptoServices();
            commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
            switch (commonActions)
            {
                //In this case new password send to corresponting email id
                case ActionsEnum.CHANGE:
                    if (!IsValid)
                    {
                        litErrorMsg.Text = Resources.ErpRes.ValidUserID;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Title_Information + "');", true);
                    }
                    else
                    {
                        string oldPassword = string.Empty;
                        if (ChangePasswordBL.CheckUserIdExists(0, txtUserID.Text,out oldPassword) > 0)
                        {
                            // create a dynamic password with 8 digit
                            PwdObj = MakePassword(8);
                            //Save the new password 
                            string pwd = HttpUtility.HtmlEncode(objCryptoServices.EncryptString(PwdObj, System.Configuration.ConfigurationManager.AppSettings["salt"]));
                            objChangePwd = ChangePasswordBL.ResetPassword(HttpUtility.HtmlEncode(txtUserID.Text.Trim()), pwd);
                            //objChangePwd = ChangePasswordBL.ResetPassword(HttpUtility.HtmlEncode(txtUserID.Text.Trim()), HttpUtility.HtmlEncode(PwdObj));
                            if (objChangePwd.ReturnVal > 0) // Success ! re-initialize the page
                            {    // mail sending
                                if (SendPassword(objChangePwd, PwdObj))
                                {
                                    ResetForm();
                                    litErrorMsg.Text = this.GetLocalResourceObject("Reset_Success").ToString();
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), this.GetLocalResourceObject("Title").ToString());
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowChangeLoc('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Title_Information + "');", true);
                                }
                                else
                                {
                                    // If mail Send Failed set old password
                                    objChangePwd = ChangePasswordBL.ResetPassword(HttpUtility.HtmlEncode(txtUserID.Text.Trim()), oldPassword);
                                    ResetForm();
                                    litErrorMsg.Text = this.GetLocalResourceObject("ActionFailedPleaseTryAgain").ToString();
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, this.GetLocalResourceObject("Title").ToString());
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.Captions.Information + "');", true);
                                }
                            }
                            else
                            {
                                switch (objChangePwd.ReturnVal)
                                {
                                    case (int)DbSaveStatus.INCORRECT:   //Incorrect Old password
                                        ResetForm();
                                        litErrorMsg.Text = this.GetLocalResourceObject("Err_UserId_Invalid").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Title_Information + "');", true);
                                        break;
                                    case (int)DbSaveStatus.SQLERROR:   //SQl Error                                 
                                        litErrorMsg.Text = Resources.ErpRes.Msg_Save_Error;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Title_Information + "');", true);
                                        break;
                                }
                            }
                        }
                        else
                        {
                            ResetForm();
                            litErrorMsg.Text = this.GetLocalResourceObject("Err_UserId_Invalid").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Title_Information + "');", true);
                        }
                    }
                    break;
                //Case for Cancel Event
                case ActionsEnum.CANCEL:
                    ResetForm();
                    ScriptManager.RegisterClientScriptBlock(this.Page, typeof(Page), "Logout", "logout()", true);
                    break;
            }
        }
        #endregion



        #endregion

        #region Helper Methods
        /// <summary>
        /// an image displayed when time need to load the page
        /// </summary>
        private void AttachUpdateProgress()
        {
            foreach (var control in Page.Form.FindControl("auplDetailList").Controls)
            {
                if (control is UpdatePanel)
                {
                    this.updateProgress.AssociatedUpdatePanelID = ((UpdatePanel)control).UniqueID;
                }
            }
        }
        /// <summary>
        /// Dynamically create passowrd using ASCII 
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        private string MakePassword(int length)
        {
            Random ran = new Random(DateTime.Now.Second);
            char[] password = new char[length];

            for (int i = 0; i < length; i++)
            {
                int[] n = { ran.Next(48, 57), ran.Next(65, 90), ran.Next(97, 122) };
                int picker = ran.Next(0, 3);

                if (picker == 3)//if i make the maxvalue 2 it "never" appears... 
                    picker = 2;
                password[i] = (char)n[picker];
            }

            return new string(password);
        }

        /// <summary>
        /// Method for passing the mail messages and password
        /// </summary>
        private bool SendPassword(ChangePasswordBO objChangePwd, string Pwd)
        {
            string createdMail;
            string primaryEmail = objChangePwd.EmailId;
            //Get the mail format from the resource
            string emailTemplate = this.GetGlobalResourceObject("EmailTemplates", ERP.Utilities.CommonConstants.RESETPASSWORD).ToString();
            createdMail = string.Format(emailTemplate, txtUserID.Text.Trim(), Pwd);
            if (CommonFunctions.SendMail("User Name and Password", createdMail, primaryEmail, ConfigurationManager.AppSettings["FrmMailResetPwd"]))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Resets the form for a fresh entry
        /// </summary>
        private void ResetForm()
        {
            txtUserID.Text = string.Empty;
            litErrorMsg.Text = string.Empty;
        }
        #endregion

        #region Page Level Events
        /// <summary>
        /// To handle pageload event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                AttachUpdateProgress();
                ScriptManager.GetCurrent(this).SetFocus(txtUserID);
                btnReset.Attributes.Add("onclick", "return ShowDeleteConfirm(this,'" + GetLocalResourceObject("ConfirmAction") + "');");
            }
        }

        #endregion
    }
}