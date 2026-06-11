using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.AccountManagement;
using BusinessLogic.AccountManagement;
using ERP.Utilities;
using BusinessObject.CommonManagement;
using System.Data;

namespace ERPSMS_v01.AccountManagement
{
    public partial class ChangePassword : ERP.Store.UI.MyBasePage
    {
        #region Variables and Properties
        private ActionsEnum commonActions;
        BusinessObject.User CurrentUser;
        private DataTable dtHistoryPassword;
        #endregion

        #region Page Level Events
        /// <summary>
        /// To handle pageload event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //SetPageVariables();
            if (!IsPostBack)
            {
                ////FindControl the controls in master pages
                //LinkButton lnkBtnMenu = (LinkButton)Master.FindControl("lnkMenu");
                //ImageButton imgBtnHelp = (ImageButton)Master.FindControl("imgBtnHelp");
                //ImageButton imgbtnChangePw = (ImageButton)Master.FindControl("imgbtnChangePwd");

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
                if (Page.Request.QueryString["UserProphile"] != null && Page.Request.QueryString["UserProphile"] == "1")
                {
                    lblBreadCrum.Visible = false;
                    btnCanel.Visible = false;
                    divBtnContainer.Attributes.Add("class", "Button-iframe");
                }
                if (Page.Request.QueryString["Exp"] != null && Page.Request.QueryString["Exp"] == "1")
                    trExpiry.Visible = true;
                ScriptManager.GetCurrent(this).SetFocus(txtoldPassword);
            }
            var limit=GetGlobalResourceObject("ConfigurationsRes","PasswordLength").ToString().Split(',');
            string expression = "^.{" + limit[0].ToString() + "," + limit[1].ToString() + "}$";
            vrePassword.ValidationExpression = expression;
            vrePassword.ErrorMessage=string.Format(GetLocalResourceObject("Msg_MinLen").ToString(),limit[0].ToString(),limit[1].ToString());

        }
        /// <summary>
        /// Set the user theme here.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
           
            // Set the User Theme
            //if (!string.IsNullOrEmpty(CurrentUser.Theme))
            //{
            //    Page.Theme = CurrentUser.Theme;
            //}
            //else
            //{
            //    //Page.Theme = System.Configuration.ConfigurationManager.AppSettings["DefaultTheme"];
            //}
            //Page.Theme = "NewTheme";

        }
        #endregion

        #region ActionHandler
        /// <summary>
        /// Handling control events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, EventArgs e)
        {
            try
            {
               
                CurrentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

                int result;
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }

                switch (commonActions)
                {
                    #region Change
                    // Do Action for , when click save button
                    case ActionsEnum.CHANGE:
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.ErpRes.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else//valid
                        {
                            if (IsPasswordStrength())
                            {
                                litErrorMsg.Text = Resources.ErrorMessages.Err_Password_Format;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                            }
                            else
                            {
                                ERP.Utilities.CryptoServices crypto = new ERP.Utilities.CryptoServices();
                                string oldPwd = HttpUtility.HtmlEncode(crypto.EncryptString(this.txtoldPassword.Text.Trim(), System.Configuration.ConfigurationManager.AppSettings["salt"]));
                                string newPwd = HttpUtility.HtmlEncode(crypto.EncryptString(this.txtPassword.Text.Trim(), System.Configuration.ConfigurationManager.AppSettings["salt"]));
                                dtHistoryPassword = UserAuthBL.GetPasswordHistory(newPwd, CurrentUser.PKUser, Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "PwdHistoryCount")));//Password History
                                if (dtHistoryPassword != null && dtHistoryPassword.Rows.Count > 0)
                                {
                                    litErrorMsg.Text = Resources.ErpRes.Msg_PasswordAlreadyUsed;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                }
                                else
                                {
                                    result = UserAuthBL.ChangePassword(CurrentUser.PKUser, oldPwd, newPwd); //Update existing password
                                    if (result > 0) // Success ! re-initialize the page
                                    {
                                        ResetForm();

                                        litErrorMsg.Text = Resources.Messages.Msg_Save_Success;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), this.GetLocalResourceObject("Title").ToString());

                                        /* Abandon session object to destroy all session variables */
                                        HttpContext.Current.Session.Clear();
                                        HttpContext.Current.Session.Abandon();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "Showchangepwd('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + this.GetLocalResourceObject("Msg_Relogin").ToString() + "');", true);

                                    }
                                    else
                                    {
                                        switch (result)
                                        {
                                            case (int)DbSaveStatus.INCORRECT:   //Incorrect Old password
                                                litErrorMsg.Text = this.GetLocalResourceObject("Err_Old_Pwd_Incorrect").ToString();
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                                break;
                                            case (int)DbSaveStatus.SQLERROR:   //SQl Error
                                                litErrorMsg.Text = Resources.ErpRes.Msg_Save_Error;
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                                break;
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    case ActionsEnum.CANCEL:
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                        ResetForm();
                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        /// <summary>
        /// Method used to Handle all actions in the page with GridViewSort
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewSortEventArgs e)
        {

        }
        #endregion

        #region Pager Methods + Init
        /// <summary>
        /// Method for Page PreInit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Page.Request.QueryString["UserProphile"] != null && Page.Request.QueryString["UserProphile"] == "1")
                this.MasterPageFile = "~/IFrameMaster.Master";
        }
        #endregion

        #region Helper Methods

        private bool IsPasswordStrength()
        {
            bool retVal = false;
            String TestString = txtPassword.Text;
            if (!(System.Text.RegularExpressions.Regex.IsMatch(TestString, "[a-zA-Z]") && System.Text.RegularExpressions.Regex.IsMatch(TestString, "[0-9]")))
            {
                retVal = true;
            }
            return retVal;
        }

        /// <summary>
        /// Resets the form for a fresh entry
        /// </summary>
        private void ResetForm()
        {
            //Codes for Clearing the controls in the page                      
            txtoldPassword.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtConfirmPassword.Text = string.Empty;
            Response.Redirect(GetGlobalResourceObject("PageURL", "InboxURL").ToString());
        }

        ///// <summary>
        ///// Assigns the object with corresponding input control values
        ///// </summary>
        ///// <returns></returns>        
        //private ChangeUsrPassword SetUIValuesToObject()
        //{
        //    CryptoServices objCryptoServices;
        //    objCryptoServices = new CryptoServices();
        //    ChangeUsrPassword objChangePwd;
        //    objChangePwd = new ChangeUsrPassword();
        //    objChangePwd.OldPwd = HttpUtility.HtmlEncode(objCryptoServices.EncryptString(txtoldPassword.Text.Trim(), ConfigurationManager.AppSettings["salt"]));
        //    objChangePwd.NewPwd = HttpUtility.HtmlEncode(objCryptoServices.EncryptString(txtPassword.Text.Trim(), ConfigurationManager.AppSettings["salt"]));
        //    return objChangePwd;
        //}
        #endregion
    }
}