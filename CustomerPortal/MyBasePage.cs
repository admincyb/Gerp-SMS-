using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.Common;
using System.Data;
using System.Globalization;
using System.Threading;
using BusinessObject;
using BusinessLogic.CommonManagement;
using System.Web.Security;
using ERP.Utilities;
using ERPSMS_v01.UserControls;

namespace ERP.Store.UI
{
    public class MyBasePage : Page
    {
        #region Variables and Properties
        /// <summary>
        /// Holds User rights for the page
        /// </summary>
        public UserRightsBO UserRights
        {
            get
            {
                return (UserRightsBO)this.ViewState["UserRigts"];
            }
            set
            {
                this.ViewState["UserRigts"] = value;
            }
        }
        public ConfigData SessionConfigData
        {
            get
            {
                return (Session[ERP.Utilities.SessionStrings.SessionConfigData] == null ? null : (ConfigData)Session[ERP.Utilities.SessionStrings.SessionConfigData]);
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.SessionConfigData] = value;
            }
        }
        BusinessObject.User currentUser;
        #endregion

        #region Page Events
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
      
        protected override void InitializeCulture()
        {
            var culture = CultureInfo.CreateSpecificCulture(GetGlobalResourceObject("ConfigurationsRes", "DefaultCulture").ToString());
            base.InitializeCulture();            
            DataTable dt = ConfigurationSettings();
            if (dt != null && dt.Rows.Count > 0)
            {
                culture.NumberFormat.CurrencyDecimalDigits = Convert.ToInt32(
                    dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "CurrencyDecimalDigit")["ACF_VALUE"].ToString());
                culture.NumberFormat.CurrencyDecimalSeparator = ".";
                culture.NumberFormat.CurrencyGroupSizes = new int[] { 
                    Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "CurrencyNumberGroup2")["ACF_VALUE"].ToString())
                    , Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "CurrencyNumberGroup1")["ACF_VALUE"].ToString()) };
                culture.NumberFormat.CurrencyGroupSeparator = ",";
                culture.NumberFormat.NumberDecimalDigits =
                    Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "NumberDecimalDigit")["ACF_VALUE"].ToString());
                culture.NumberFormat.NumberDecimalSeparator = ".";
                culture.NumberFormat.NumberGroupSizes = new int[] { 
                    Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "NumberGroup2")["ACF_VALUE"].ToString())
                    , Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "NumberGroup1")["ACF_VALUE"].ToString()) };
                culture.NumberFormat.NumberGroupSeparator = ",";
                culture.NumberFormat.CurrencySymbol = "";
                culture.NumberFormat.CurrencyNegativePattern = 1;
                Session[ERP.Utilities.SessionStrings.RateDecimalDigit] = Convert.ToInt32(
                    dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "RateDecimalDigit")["ACF_VALUE"].ToString());
                Session[ERP.Utilities.SessionStrings.ExchRateDecimalDigit] = Convert.ToInt32(
                    dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "ExchRateDecimalDigit")["ACF_VALUE"].ToString());
                Session[ERP.Utilities.SessionStrings.RateDecimalDigitP2P] = Convert.ToInt32(
                   dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "RateDecimalDigitP2P")["ACF_VALUE"].ToString());
            }
            Thread.CurrentThread.CurrentCulture = culture;

            HttpCookie userCulture = Request.Cookies["Culture"];
            culture = userCulture != null ?
                CultureInfo.CreateSpecificCulture(userCulture.Value) :
                CultureInfo.CreateSpecificCulture(GetGlobalResourceObject("ConfigurationsRes", "DefaultCulture").ToString());
            Page.UICulture = culture.ToString();
            Thread.CurrentThread.CurrentUICulture = culture;
        }
        protected override void OnPreLoad(EventArgs e)
        {
            if (!IsPostBack)
            {
                base.OnPreLoad(e);
                SetUserDept();
            }
        }
        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            if (!IsPostBack)
            {
                //assign Page BreadCrumb
                AssignBreadCrumb();
                #region Variables
                string pageURL;
                CommonBL userAuth;
                #endregion
                pageURL = GetPageUrlWithoutParams();
                userAuth = new CommonBL();
                currentUser = GetUserIdentity(); 
                //// get the user rights
                UserRights = userAuth.GetUserRights(currentUser.PKUser, pageURL, currentUser.SBUID);
                if (UserRights.Rights.Count < 1) //If user has no rights defined, prevent page access
                {
                    Response.Redirect("~/Login.aspx");
                }
                //// Hide the non user right sections
                Hide(UserRights);
            }
            //Assign click event for delete button
            SetDeleteClickEvent();
            //InitializeCulture();
        }
        #endregion
        #region Helper Methods
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
        /// <summary>
        /// Get Configuration for Currency & Number Formats
        /// </summary>
        private DataTable ConfigurationSettings()
        {
            currentUser = GetUserIdentity();
            DataTable dt = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("Currency Settings", string.Empty, currentUser.SBUID);
            return dt;
        }
        /// <summary>
        /// Set User Department from Department Session
        /// </summary>
        public void SetUserDept()
        {
            int curDep;
            if (Request.QueryString[QueryStrings.ProcessDept] != null)
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    currentUser = GetUserIdentity();
                    curDep = Request.QueryString[QueryStrings.ProcessDept] != null ? Convert.ToInt32(Request.QueryString[QueryStrings.ProcessDept].ToString()) : currentUser.CurrentDeptPK;
                    MenuControl ctrlMenu = (MenuControl)this.Master.FindControl("MenuControl1");
                    DropDownList ddlCostCenter = (DropDownList)ctrlMenu.FindControl("ddlCostCenter");
                    if (ctrlMenu != null)
                    {
                        Session[BusinessObject.Common.SessionStrings.CurDept] = curDep;
                        ctrlMenu.FillLocation(true);
                        if (Session[BusinessObject.Common.SessionStrings.CurDept] != null)
                        {
                            ddlCostCenter.SelectedIndex = ddlCostCenter.Items.IndexOf(ddlCostCenter.Items.FindByValue(Session[BusinessObject.Common.SessionStrings.CurDept].ToString()));
                        }
                    }
                }
            }
            currentUser = GetUserIdentity();
            string pageURL = "";
            if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
            {
                pageURL = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "");
            }
            else
            {
                pageURL = Request.Url.AbsolutePath.ToLower();
            }
            int pageId = Convert.ToInt32(BusinessLogic.CommonManagement.CommonBL.GetPageId(pageURL));
            if (pageId > 0)
            {
                System.Data.DataTable dtDept = BusinessLogic.CommonManagement.CommonBL.GetPageDept(pageId, currentUser.PKUser);
                if (dtDept != null && dtDept.Rows.Count > 0)
                {
                    BusinessObject.Department dept = null;
                    if (Session[BusinessObject.Common.SessionStrings.CurDept] != null)
                        currentUser.CurrentDeptPK = Convert.ToInt32(Session[BusinessObject.Common.SessionStrings.CurDept].ToString());
                    DataRow currDept = dtDept.AsEnumerable().SingleOrDefault(dr => dr.Field<int>("DPT_PK") == currentUser.CurrentDeptPK);
                    if (currDept == null)
                        currDept = dtDept.Rows[0];
                    dept = new BusinessObject.Department()
                    {
                        CurrentSBU = currentUser.CurrentSBU,
                        CurrentSBUPK = currentUser.SBUID,
                        CurrentDept = currDept["DPT_NAME"].ToString(),
                        CurrentDeptPK = Convert.ToInt32(currDept["DPT_PK"]),
                        BaseCurrency = Convert.ToInt32(currDept["DPT_CURR"])
                    };
                    BusinessLogic.AccountManagement.UserAuthBL userAuth = new BusinessLogic.AccountManagement.UserAuthBL();
                    userAuth.SetUserProperty(BusinessObject.AccountManagement.UserPropertyEnum.CurrentDept, dept);
                }
            }
        }
        /// <summary>
        /// Assign click event call to delete button to show confirmation message.
        /// </summary>
        private void SetDeleteClickEvent()
        {
            System.Web.UI.Control control = Page.Form.FindControl("MainContent").FindControl("btnDelete");
            if (control != null)
            {
                ((Button)control).OnClientClick = "return ShowDeleteConfirm(this);";
            }
        }
        /// <summary>
        /// assign Page BreadCrumb
        /// </summary>
        public void AssignBreadCrumb()
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
        /// Get Page Url with Virtual Directory removing Parameters
        /// </summary>
        /// <returns></returns>
        public string GetPageUrlWithoutParams()
        {
            string pageURL;
            if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
                pageURL = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "") + Request.Url.Query.ToLower();
            else
                pageURL = Request.Url.PathAndQuery.ToLower();
            return pageURL.IndexOf("&refid") > 0 ? pageURL.Remove(pageURL.IndexOf("&refid"))
                : pageURL.IndexOf("?refid") > 0 ? pageURL.Remove(pageURL.IndexOf("?refid"))
                : pageURL.IndexOf("&prefid") > 0 ? pageURL.Remove(pageURL.IndexOf("&prefid"))
                : pageURL.IndexOf("?dep") > 0 ? pageURL.Remove(pageURL.IndexOf("?dep"))
                : pageURL.IndexOf("&dep") > 0 ? pageURL.Remove(pageURL.IndexOf("&dep"))
                : pageURL.IndexOf("?prefid") > 0 ? pageURL.Remove(pageURL.IndexOf("?prefid")) : pageURL;
        }
        /// <summary>
        /// Checks the visibility of the button based on the UserRights for the page
        /// </summary>
        /// <param name="sender"></param>
        public void CheckBtnVisibility(object sender)
        {
            UserRightBO usrActions;
            string action;
            string section;
            IButtonControl ctrl;
            WebControl wctrl;
            // Don' process if UserRights is empty
            if (UserRights == null || UserRights.Rights.Count == 0)
                return;

            action = String.Empty;
            section = String.Empty;
            ctrl = sender as IButtonControl;
            wctrl = sender as WebControl;

            if (ctrl != null && wctrl != null)
            {
                // Get Action and Section from the button
                action = ctrl.CommandName.ToLower();
                section = ctrl.CommandArgument.ToLower();
            }
            else return;

            usrActions = (from bp in UserRights.Rights
                          where (bp.SectionName.ToLower() == section && bp.ActionName.ToLower() == action)
                          select bp).FirstOrDefault();

            // If actions don't have any result, then hide the button
            if (usrActions == null)
            {
                wctrl.Visible = false;
            }
            else if (usrActions.HasActionRight == true)
            {
                if (usrActions.IsTab)
                    wctrl.Enabled = true;
                else
                    wctrl.Visible = true;
            }
            else
            {
                if (usrActions.IsTab)
                    wctrl.Enabled = false;
                else
                    wctrl.Visible = false;
            }
        }
        /// <summary>
        /// Hides sections which don't have user access rights
        /// </summary>
        /// <param name="currentPageSections"></param>
        /// <param name="usrSections"></param>
        private void Hide(UserRightsBO usrRights)
        {
            WebControl secControl;
            List<string> sections, usrSections;
            sections = (from p in usrRights.Rights
                        select p.SectionName).Distinct<string>().ToList<string>();
            foreach (string usrSection in sections)
            {
                //getting the user
                usrSections = (from p in usrRights.Rights
                               where (p.HasActionRight && p.SectionName == usrSection)
                               select p.SectionName).Distinct<string>().ToList<string>();
                secControl = Page.Form.FindControl("MainContent") != null ? Page.Form.FindControl("MainContent").FindControl(usrSection) as WebControl : null;
                if (secControl != null)
                {
                    if (usrSections.Count > 0)
                        secControl.Visible = true;
                    else
                        secControl.Visible = false;
                }
            }
        }

        /// <summary>
        /// Method to get application configuration data
        /// </summary>
        /// <returns></returns>
        public ConfigData GetConfigData()
        {
            ConfigData objConfig = null;
            if (SessionConfigData == null)
            {
                DataTable dtAppConfigs = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("MULTIPLE PLANT", "ENABLED");
                if (dtAppConfigs != null && dtAppConfigs.Rows.Count > 0)
                {
                    objConfig = new ConfigData();
                    objConfig.IsMultiplePlant = Convert.ToInt32(dtAppConfigs.Rows[0]["ACF_VALUE"]) == 1 ? true : false;
                    SessionConfigData = objConfig;
                }
            }
            else
                objConfig = SessionConfigData;
            return objConfig;
        }
        #endregion
    }
}