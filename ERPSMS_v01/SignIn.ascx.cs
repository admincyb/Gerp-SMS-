using System;
using System.Web;
using System.Web.Security;

using BusinessLogic;
using BusinessObject;
using System.Web.UI.WebControls;
using BusinessObject.AccountManagement;
using System.Data;
using System.Configuration;
using GTIService;
using System.Collections.Generic;
using System.Web.UI;
using GTIService.Constants.Common;

namespace LatexERPV2
{
    public partial class SignIn : System.Web.UI.UserControl
    {
        #region Variables and Properties

        User currentUser = new User();
        string uID;
        string passWord;
        //User authUser;
        BusinessLogic.AccountManagement.UserAuthBL authentication;
        string userData;
        string encTicket;
        string ErrMsg;
        FormsAuthenticationTicket authTicket;
        HttpCookie authCookie;
        DataTable dtSBU;
        #endregion
        private static NLog.Logger logger = NLog.LogManager.GetLogger("User Login");

        protected void Page_PreRender(Object sender, EventArgs e)
        {
            
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                #region LogOut Logic
                if (Request.QueryString["Logout"] != null)
                {
                    if (Context.User.Identity.IsAuthenticated)
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
                        Response.Redirect("~/Login.aspx");

                        if (System.Configuration.ConfigurationManager.AppSettings["PORTALURL"] != null)
                        {
                            Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["PORTALURL"] + "?Logout=1");
                        }
                        else if (System.Configuration.ConfigurationManager.AppSettings["ASSETURL"] != null)
                        {
                            Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["ASSETURL"] + "?Logout=1");
                        }
                        else if (System.Configuration.ConfigurationManager.AppSettings["PRODUCTIONURL"] != null)
                        {
                            Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["PRODUCTIONURL"] + "?Logout=1");
                        }
                        else if (System.Configuration.ConfigurationManager.AppSettings["HRSURL"] != null)
                        {
                            Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["HRSURL"] + "?Logout=1");
                        }
                        else if (System.Configuration.ConfigurationManager.AppSettings["CONSTRUCTIONURL"] != null)
                        {
                            Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["CONSTRUCTIONURL"] + "?Logout=1");
                        }
                        else
                        {
                            FormsAuthentication.RedirectToLoginPage();
                        }
                    }
                    else
                    {
                        FormsAuthentication.RedirectToLoginPage();
                    }
                }
                #endregion
                #region Multiple Login Logic
                if (Request.QueryString["PostUser"] != null && Request.QueryString["PostPassword"] != null)
                {
                    uID = CommonFunctions.Decrypt(HttpUtility.UrlDecode(Request.QueryString["PostUser"]));
                    passWord = Request.QueryString["PostPassword"];
                    passWord = passWord.Replace(" ", "+");
                    BusinessLogic.Login login = new BusinessLogic.Login();
                    User authUser = login.Authenticate(uID, passWord);
                    if (authUser != null)
                    {
                        this.Authenticate(authUser);
                    }
                    else
                    {
                        ErrorText.Text = "Invalid Authentication";
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowInvalidMessage('" + ErrorText.Text + "');", true);
                    }
                }
                #endregion
                #region SBU
                if (ConfigurationManager.AppSettings["HasMultipleSBULlicense"] != null &&
                    ConfigurationManager.AppSettings["HasMultipleSBULlicense"] == "1")
                {
                    loginwrap.Attributes["class"] = "loginwrapSbu";
                    dtSBU = BusinessLogic.CommonManagement.CommonManagement.GetAllBizUnit(0, 1);
                    ddlSBU.Items.Clear();
                    if (dtSBU != null && dtSBU.Rows.Count > 0)
                    {
                        ddlSBU.DataTextField = "BZU_NAME";
                        ddlSBU.DataValueField = "BZU_PK";
                        ddlSBU.DataSource = dtSBU;
                        ddlSBU.DataBind();
                    }
                    ddlSBU.Items.Insert(0, new ListItem(CommonConstants.SELECTSBUTEXT, CommonConstants.SELECTVAL));
                }
                else
                {
                    ddlSBU.Visible = false;
                    loginwrap.Attributes["class"] = "loginwrap";
                }
                #endregion
            }
            gERPUserID.Focus();

        }

        /// <summary>
        /// Performs the Forms Authentication.
        /// Stores the Login object in Session with the SessionKey = UserID
        /// Redirects the User based on Home page preference.
        /// </summary>
        /// <param name="uLogin"></param>
        private void Authenticate(User uLogin)
        {
            Session[ERP.Utilities.SessionStrings.IsDashBoardUser] = null;
            Session[uLogin.PKUser.ToString()] = uLogin;
            DataTable sbuDetails = BusinessLogic.CommonManagement.CommonBL.GetBizUnit(uLogin.PKUser, 0);
            if (sbuDetails.Rows.Count > 0)
            {
                uLogin.SBUID = Convert.ToInt32(sbuDetails.Rows[0]["DPT_BIZUNIT"]);
                uLogin.CurrentSBUPK = Convert.ToInt32(sbuDetails.Rows[0]["DPT_BIZUNIT"]);
                uLogin.CurrentSBU = sbuDetails.Rows[0]["BZU_NAME"].ToString();
                if (uLogin.CurrentDeptPK == 0)
                {
                    DataTable dtDept = BusinessLogic.CommonManagement.CommonBL.GetDepartment(uLogin.PKUser, uLogin.SBUID);
                    if (dtDept.Rows.Count > 0)
                    {
                        uLogin.CurrentDeptPK = Convert.ToInt32(dtDept.Rows[0]["DPT_PK"]);
                        uLogin.ActiveDepID = uLogin.CurrentDeptPK;
                        uLogin.CurrentDept = dtDept.Rows[0]["DPT_NAME"].ToString();
                    }
                }
            }
            Context.User = new ERPPrincipal(uLogin);
            //Set Current department to session
            Session[BusinessObject.Common.SessionStrings.CurDept] = uLogin.CurrentDeptPK;
            //bool remeberSet = (this.LgnUser.FindControl("chbRememberMeSet") as CheckBox).Checked;
            bool remeberSet = true;
            FormsAuthenticationTicket AuthTicket = new FormsAuthenticationTicket(1, uLogin.UserID, DateTime.Now, DateTime.Now.AddMinutes(15), remeberSet, Newtonsoft.Json.JsonConvert.SerializeObject(uLogin));
            string EncTicket = FormsAuthentication.Encrypt(AuthTicket);
            Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, EncTicket));
            string redirectURL = FormsAuthentication.GetRedirectUrl(uLogin.UserID, remeberSet);
            #region set Default Culture To Cookies
            //Create Cookie for kkep default culture.
            HttpCookie cookie = new HttpCookie("Culture");
            cookie.Value = uLogin.UserCulture;
            Response.Cookies.Add(cookie);

            //HttpCookie CroseTabAttrib = new HttpCookie("CroseTabAttrib");
            //CroseTabAttrib.Value = uLogin.PKUser.ToString();
            //Response.Cookies.Add(CroseTabAttrib);

            //HttpCookie authValue = new HttpCookie("authValue");
            //Random random = new Random();
            //CroseTabAttrib.Value = random.Next(100, 200).ToString();
            //Response.Cookies.Add(authValue);

            #endregion

            //#region Set multiple plant config values in cookie
            //DataTable dtAppConfigs = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("MULTIPLE PLANT", "ENABLED");
            //if (dtAppConfigs != null && dtAppConfigs.Rows.Count > 0)
            //{
            //    ConfigData appConfigs = new ConfigData();
            //    appConfigs.IsMultiplePlant = Convert.ToInt32(dtAppConfigs.Rows[0]["ACF_VALUE"]) == 1 ? true : false;
            //    Session[ERP.Utilities.SessionStrings.SessionConfigData] = appConfigs;
            //} 
            //#endregion

            redirectURL = "~/AccountManagement/WorkflowInbox.aspx";
            //Code start for multi application login
            bool isLoginSMS, isLoginCustomerPortal, isLoginAsset, isLoginProduction, isLoginHrms, isLoginConstruction;
            isLoginSMS = isLoginCustomerPortal = isLoginAsset = isLoginProduction = isLoginHrms = isLoginConstruction = false;
            isLoginSMS = true;

            ERP.Utilities.CryptoServices crypto = new ERP.Utilities.CryptoServices();


            #region Getting User Name And Pswd
            string userName = HttpUtility.UrlEncode(CommonFunctions.Encrypt(uLogin.UserName));
            string password = Request.QueryString["PostPassword"] ??
                                HttpUtility.UrlEncode(
                                    HttpUtility.HtmlEncode(
                                        crypto.EncryptString(
                                            this.gERPPwd.Text.Trim(),
                                            ConfigurationManager.AppSettings["salt"]
                                        )
                                    )
                                );
            #endregion

            #region Get Module Login Flgs

            if (Request.QueryString["isLoginCustomerPortal"] != null && !string.IsNullOrEmpty(Request.QueryString["isLoginCustomerPortal"]))
            {
                Boolean.TryParse(Request.QueryString["isLoginCustomerPortal"], out isLoginCustomerPortal);
            }
            if (Request.QueryString["isLoginAsset"] != null && !string.IsNullOrEmpty(Request.QueryString["isLoginAsset"]))
            {
                Boolean.TryParse(Request.QueryString["isLoginAsset"], out isLoginAsset);
            }
            if (Request.QueryString["isLoginProduction"] != null && !string.IsNullOrEmpty(Request.QueryString["isLoginProduction"]))
            {
                Boolean.TryParse(Request.QueryString["isLoginProduction"], out isLoginProduction);
            }
            if (Request.QueryString["isLoginHrms"] != null && !string.IsNullOrEmpty(Request.QueryString["isLoginHrms"]))
            {
                Boolean.TryParse(Request.QueryString["isLoginHrms"], out isLoginHrms);
            }
            if (Request.QueryString["isLoginConstruction"] != null && !string.IsNullOrEmpty(Request.QueryString["isLoginConstruction"]))
            {
                Boolean.TryParse(Request.QueryString["isLoginConstruction"], out isLoginConstruction);
            }
            #endregion

            #region Is First Login User
            if (uLogin.IsFirstLogin && GetGlobalResourceObject("ConfigurationsRes", "FirstLoginCheckRequired").ToString() == "1")
            {
                redirectURL = "~" + GetGlobalResourceObject("PageURL", "ChangePwdURL").ToString() + uLogin.PKUser.ToString();
            }
            #endregion
            #region password Epiry checking
            else if (GetGlobalResourceObject("ConfigurationsRes", "PwdExpiryChecking").ToString() == "1" && uLogin.IsSysUser == 0 &&
                    ERP.Utilities.CommonFunctions.IsPasswordExpired(uLogin.usrPwdModOn, Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "PwdExpiryDays").ToString())))
            {
                redirectURL = "~" + GetGlobalResourceObject("PageURL", "ChangePwdURL").ToString() + uLogin.PKUser.ToString() + "&Exp=1";
            }
            #endregion
            #region Customer Login
            else if (IsCustomer(uLogin.PKUser, uLogin.SBUID))
            {
                redirectURL = "{0}?PostUser={1}&PostPassword={2}&isLoginSMS={3}&isLoginCustomerPortal={4}&isLoginAsset={5}&isLoginProduction={6}&isLoginHrms={7}&isLoginConstruction={8}";
                string url = ConfigurationManager.AppSettings["CUSTOMERDOMINE"];
                redirectURL = string.Format(redirectURL,
                                                url,
                                                userName,
                                                password,
                                                isLoginSMS,
                                                isLoginCustomerPortal,
                                                isLoginAsset,
                                                isLoginProduction,
                                                isLoginHrms,
                                                isLoginConstruction
                                                );
            }
            else
            #endregion
                #region Portal Url
                if (System.Configuration.ConfigurationManager.AppSettings["PORTALURL"] != null && !isLoginCustomerPortal)
                {
                    redirectURL = "{0}?PostUser={1}&PostPassword={2}&isLoginSMS={3}&isLoginCustomerPortal={4}&isLoginAsset={5}&isLoginProduction={6}&isLoginHrms={7}&isLoginConstruction={8}";
                    string url = ConfigurationManager.AppSettings["PORTALURL"];

                    redirectURL = string.Format(redirectURL,
                                                    url,
                                                    userName,
                                                    password,
                                                    isLoginSMS,
                                                    isLoginCustomerPortal,
                                                    isLoginAsset,
                                                    isLoginProduction,
                                                    isLoginHrms,
                                                    isLoginConstruction
                                                    );
                }
                #endregion
                #region Asset Url
                else if (System.Configuration.ConfigurationManager.AppSettings["ASSETURL"] != null && !isLoginAsset)
                {
                    redirectURL = "{0}?PostUser={1}&PostPassword={2}&isLoginSMS={3}&isLoginCustomerPortal={4}&isLoginAsset={5}&isLoginProduction={6}&isLoginHrms={7}&isLoginConstruction={8}";
                    string url = ConfigurationManager.AppSettings["ASSETURL"];

                    redirectURL = string.Format(redirectURL,
                                                  url,
                                                  userName,
                                                  password,
                                                  isLoginSMS,
                                                  isLoginCustomerPortal,
                                                  isLoginAsset,
                                                  isLoginProduction,
                                                  isLoginHrms,
                                                  isLoginConstruction
                                                  );
                }
                #endregion
                #region Production Url
                else if (System.Configuration.ConfigurationManager.AppSettings["PRODUCTIONURL"] != null && !isLoginProduction)
                {
                    redirectURL = "{0}?PostUser={1}&PostPassword={2}&isLoginSMS={3}&isLoginCustomerPortal={4}&isLoginAsset={5}&isLoginProduction={6}&isLoginHrms={7}&isLoginConstruction={8}";
                    string url = ConfigurationManager.AppSettings["PRODUCTIONURL"];

                    redirectURL = string.Format(redirectURL,
                                                  url,
                                                  userName,
                                                  password,
                                                  isLoginSMS,
                                                  isLoginCustomerPortal,
                                                  isLoginAsset,
                                                  isLoginProduction,
                                                  isLoginHrms,
                                                  isLoginConstruction
                                                  );
                }
                #endregion
                #region Hrms Url
                else if (System.Configuration.ConfigurationManager.AppSettings["HRMSURL"] != null && !isLoginHrms)
                {
                    redirectURL = "{0}?PostUser={1}&PostPassword={2}&isLoginSMS={3}&isLoginCustomerPortal={4}&isLoginAsset={5}&isLoginProduction={6}&isLoginHrms={7}&isLoginConstruction={8}";
                    string url = ConfigurationManager.AppSettings["HRMSURL"];

                    redirectURL = string.Format(redirectURL,
                                                    url,
                                                    userName,
                                                    password,
                                                    isLoginSMS,
                                                    isLoginCustomerPortal,
                                                    isLoginAsset,
                                                    isLoginProduction,
                                                    isLoginHrms,
                                                    isLoginConstruction
                                                    );
                }
                #endregion
                #region Construction Url
                else if (System.Configuration.ConfigurationManager.AppSettings["CONSTRUCTIONURL"] != null && !isLoginConstruction)
                {
                    redirectURL = "{0}?PostUser={1}&PostPassword={2}&isLoginSMS={3}&isLoginCustomerPortal={4}&isLoginAsset={5}&isLoginProduction={6}&isLoginHrms={7}&isLoginConstruction={8}";
                    string url = ConfigurationManager.AppSettings["CONSTRUCTIONURL"];

                    redirectURL = string.Format(redirectURL,
                                                    url,
                                                    userName,
                                                    password,
                                                    isLoginSMS,
                                                    isLoginCustomerPortal,
                                                    isLoginAsset,
                                                    isLoginProduction,
                                                    isLoginHrms,
                                                    isLoginConstruction
                                                    );
                }
                #endregion
                #region Default
                else
                {
                    redirectURL = "~/AccountManagement/WorkflowInbox.aspx";
                    //if (GetGlobalResourceObject("ConfigurationsRes", "ShowDashboard").ToString() == "1")
                    //{
                    if (BusinessLogic.CommonManagement.CommonBL.IsDashBoardUser(uLogin.PKUser, uLogin.SBUID))
                    {
                        //Dashboard Url 
                        Session[ERP.Utilities.SessionStrings.IsDashBoardUser] = true;
                        redirectURL = string.IsNullOrEmpty(ConfigurationManager.AppSettings["DASHBOARDURL"]) ? "~/AccountManagement/WorkflowInbox.aspx" : ConfigurationManager.AppSettings["DASHBOARDURL"];
                    }
                    //}
                }
                #endregion
            
            Response.Redirect(redirectURL, false);
        }

        private bool IsCustomer(int UserPK, int BuzUnit)
        {
            ERPService.CommonService commonService = new ERPService.CommonService();
            bool iscustomer = false;
            List<ERPData.SPCRM_CUSTOMER_USER_GET_Result> lst = new List<ERPData.SPCRM_CUSTOMER_USER_GET_Result>();
            try
            {
                Session[ERP.Utilities.SessionStrings.CUSTOMERLOGINPK] = null;
                lst = commonService.GetCustomerDetails(UserPK, BuzUnit);
                if (lst != null && lst.Count > 0)
                {
                    if (lst[0].CUS_PK > 0)
                    {
                        Session[ERP.Utilities.SessionStrings.CUSTOMERLOGINPK] = lst[0].CUS_PK;
                        iscustomer = true;
                    }
                }
            }
            catch (Exception ex)
            {
                SignIn.logger.Log(NLog.LogLevel.Fatal, "Error Occured " + ex.Message);
            }
            finally
            {
                lst = null;
                commonService = null;
            }
            return iscustomer;
        }


        /// <summary>
        /// Validate version
        /// </summary>
        private bool IsValidVersion()
        {
            bool flag = false;
            DataTable dt = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("VERSION", string.Empty, 1);
            if (dt != null && dt.Rows.Count > 0)
            {
                if (System.Configuration.ConfigurationManager.AppSettings["Version"] != null)
                {
                    if (dt.Rows[0]["ACF_DATA"].ToString() == System.Configuration.ConfigurationManager.AppSettings["Version"].ToString())
                    {
                        flag = true;
                    }
                }
            }
            return flag;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnLogin_Click(object sender, EventArgs e)

        {
            try
            {
                DateTime expDate;
                GTIService.Utilities.CryptoServices crypto = new GTIService.Utilities.CryptoServices();
                //string test= crypto.DecryptString(ConfigurationManager.AppSettings["SYSVAL"], ConfigurationManager.AppSettings["SYSKEY"]);
                if (DateTime.TryParse(crypto.DecryptString(ConfigurationManager.AppSettings["SYSVAL"],
                                        ConfigurationManager.AppSettings["SYSKEY"]), out expDate) &&
                                        expDate >= Convert.ToDateTime(DateTime.Now.ToShortDateString()))
                {

                    #region Authentication
                    string userName = gERPUserID.Text;
                    string password = gERPPwd.Text;
                    int backgroundSPcall = Convert.ToInt16(GetGlobalResourceObject("ConfigurationsRes", "BackGroundSPCall"));
                    password = HttpUtility.HtmlEncode(crypto.EncryptString(this.gERPPwd.Text.Trim(), System.Configuration.ConfigurationManager.AppSettings["salt"]));
                    BusinessLogic.Login login = new BusinessLogic.Login();
                    int BizUnit = 0;
                    if (ddlSBU.SelectedIndex > 0)
                        BizUnit = Convert.ToInt32(ddlSBU.SelectedValue);
                    if (IsValidVersion())
                    {
                        var uLogin = login.Authenticate(userName, password, backgroundSPcall, BizUnit);
                        if (uLogin != null)
                        {
                            if (uLogin.IsPublicUser)
                                this.Authenticate(uLogin);
                            else
                                if (CommonFunctions.IsLocalHost(HttpContext.Current.Request.Url.Host.ToString().Trim(), GetGlobalResourceObject("Constants", "PrivateIPRange").ToString()))
                                    this.Authenticate(uLogin);
                                else
                                    FailureText.Text = "Authentication Failed.";
                        }
                        else
                        {
                            FailureText.Text = "Authentication Failed.";
                        }
                    }
                    else
                    {
                        FailureText.Text = CommonFunctions.FormatErrorMessage(Resources.ErrorMessages.Err_Version);

                    }
                    #endregion
                }
                else
                {
                    FailureText.Text = CommonFunctions.FormatErrorMessage(Resources.ErrorMessages.Err_Expiry);
                }

            }
            catch (Exception ex)
            {
                SignIn.logger.Log(NLog.LogLevel.Fatal, "Error Occured " + ex.Message);
            }
        }
        /// <summary>
        /// Event for Authentication
        /// </summary>
        /// <param name="uLogin"></param>

        protected void LgnUser_Authenticate(object sender, System.Web.UI.WebControls.AuthenticateEventArgs e)
        {
            try
            {
                DateTime expDate;

                GTIService.Utilities.CryptoServices crypto = new GTIService.Utilities.CryptoServices();
                //string test= crypto.DecryptString(ConfigurationManager.AppSettings["SYSVAL"], ConfigurationManager.AppSettings["SYSKEY"]);
                if (DateTime.TryParse(crypto.DecryptString(ConfigurationManager.AppSettings["SYSVAL"],
                                        ConfigurationManager.AppSettings["SYSKEY"]), out expDate) &&
                                        expDate >= Convert.ToDateTime(DateTime.Now.ToShortDateString()))
                {
                    #region Authentication
                    string userName = this.LgnUser.UserName;
                    string password = this.LgnUser.Password;
                    BusinessLogic.Login login = new BusinessLogic.Login();
                    var uLogin = login.Authenticate(userName, password);
                    if (uLogin != null)
                    {
                        this.Authenticate(uLogin);
                    }
                    else
                    {
                        this.LgnUser.FailureText = "Authentication Failed.";
                        this.LgnUser.FailureTextStyle.ForeColor = System.Drawing.Color.Red;

                    }
                    #endregion
                }
                else
                {
                    ErrMsg = CommonFunctions.FormatErrorMessage(Resources.ErrorMessages.Err_Expiry);
                    this.LgnUser.FailureText = ErrMsg;
                }
            }

            catch (Exception ex)
            {
                SignIn.logger.Log(NLog.LogLevel.Fatal, "Error Occured " + ex.Message);
            }

        }

        protected string cryptoKey
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["CryptoKey"];
            }
        }
    }
}