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
using ERP.Utilities;
using ERPSMS_v01.UserControls;
using BusinessObject;
using BusinessLogic.AccountManagement;
using System.Web.Security;

namespace ERP.Store.UI
{
    public class MyBasePage : Page
    {
        int CurDep;
        BusinessObject.User currentUser;
        CryptoServices crypto = new CryptoServices();
        UserAuthBL userAuth;
        User CurrentUser;
        UserRightsBO usrRights;
        public string currentPageURL = string.Empty;
        #region Properties
        /// <summary>
        /// WorkFlow Page Url
        /// </summary>
        public string WkfPageUrl
        {
            get
            {
                return this.ViewState["BaseWkfPageUrl"] == null ? string.Empty : this.ViewState["BaseWkfPageUrl"].ToString();
            }
            set
            {
                this.ViewState["BaseWkfPageUrl"] = value;
            }
        }
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
        /// <summary>
        /// Configuration Data
        /// </summary>
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
        #endregion
        protected override void OnPreLoad(EventArgs e)
        {
            CultureInfo culture;
            HiddenField hdfAmtDecimalB;
            HiddenField hdfQtyDecimalB;
            HiddenField hdfRateDecimalB;
            HiddenField hdfExcRateDecimalB;
            HiddenField hdfRateDecimalDigitP2P;
            HiddenField hdfQtyDecimalBP2P; //Qty Decimal Digit for Purchase
            HiddenField hdfCompoundingDecimal;//Dispersion/Compounding for Production
            HiddenField hdfNumberDecimalDigitBin;//For Bincard/Production

            if (!IsPostBack)
            {
                culture = Thread.CurrentThread.CurrentCulture;
                hdfAmtDecimalB = this.Page.Master.FindControl("hdfAmtDecimal") as HiddenField;
                if (hdfAmtDecimalB != null)
                    hdfAmtDecimalB.Value = culture.NumberFormat.CurrencyDecimalDigits.ToString();
                hdfQtyDecimalB = this.Page.Master.FindControl("hdfQtyDecimal") as HiddenField;
                if (hdfQtyDecimalB != null)
                    hdfQtyDecimalB.Value = culture.NumberFormat.NumberDecimalDigits.ToString();
                hdfRateDecimalB = this.Page.Master.FindControl("hdfRateDecimal") as HiddenField;
                if (hdfRateDecimalB != null)
                    hdfRateDecimalB.Value = Session[ERP.Utilities.SessionStrings.RateDecimalDigit].ToString();
                hdfExcRateDecimalB = this.Page.Master.FindControl("hdfExcRateDecimal") as HiddenField;
                if (hdfExcRateDecimalB != null)
                    hdfExcRateDecimalB.Value = Session[ERP.Utilities.SessionStrings.ExchRateDecimalDigit].ToString();

                hdfRateDecimalDigitP2P = this.Page.Master.FindControl("hdfRateDecimalDigitP2P") as HiddenField;
                if (hdfRateDecimalDigitP2P != null)
                    hdfRateDecimalDigitP2P.Value = Session[ERP.Utilities.SessionStrings.RateDecimalDigitP2P].ToString();
                //Qty Decimal Digit for Purchase
                hdfQtyDecimalBP2P = this.Page.Master.FindControl("hdfQtyDecimalP2P") as HiddenField;
                if (hdfQtyDecimalBP2P != null)
                    hdfQtyDecimalBP2P.Value = Session[ERP.Utilities.SessionStrings.NumberDecimalDigitsP2P].ToString();
                //Decimal for Dispersion/Compounding- Production
                hdfCompoundingDecimal = this.Page.Master.FindControl("hdfCompoundingDecimal") as HiddenField;
                if (Session[ERP.Utilities.SessionStrings.NumberDecimalDigitsCompounding] != null && hdfCompoundingDecimal != null)
                    hdfCompoundingDecimal.Value = Session[ERP.Utilities.SessionStrings.NumberDecimalDigitsCompounding].ToString();
                else
                    hdfCompoundingDecimal.Value = "0";
                //Decimal for Bincard- Production
                hdfNumberDecimalDigitBin = this.Page.Master.FindControl("hdfNumberDecimalDigitBin") as HiddenField;
                if (Session[ERP.Utilities.SessionStrings.NumberDecimalDigitBin] != null && hdfNumberDecimalDigitBin != null)
                    hdfNumberDecimalDigitBin.Value = Session[ERP.Utilities.SessionStrings.NumberDecimalDigitBin].ToString();
                else
                    hdfNumberDecimalDigitBin.Value = "0";

                if (Request.QueryString[QueryStrings.ProcessDept] != null)
                {
                    if (HttpContext.Current.User.Identity.IsAuthenticated)
                    {
                        currentUser = GetUserIdentity();
                        CurDep = Request.QueryString[QueryStrings.ProcessDept] != null ? Convert.ToInt32(Request.QueryString[QueryStrings.ProcessDept].ToString()) : currentUser.CurrentDeptPK;
                        MenuControl ctrlMenu = (MenuControl)this.Master.FindControl("MenuControl1");
                        DropDownList ddlCostCenter = (DropDownList)ctrlMenu.FindControl("ddlCostCenter");
                        if (ctrlMenu != null)
                        {
                            Session[BusinessObject.Common.SessionStrings.CurDept] = CurDep;
                            ctrlMenu.FillLocation(true);
                            if (Session[BusinessObject.Common.SessionStrings.CurDept] != null)
                            {
                                ddlCostCenter.SelectedIndex = ddlCostCenter.Items.IndexOf(ddlCostCenter.Items.FindByValue(Session[BusinessObject.Common.SessionStrings.CurDept].ToString()));
                            }
                        }
                    }
                }
                base.OnPreLoad(e);
                if (Request.QueryString["KeepCurDep"] == null)//The page get session out on clicking the PR hyper link before creating PO(Bug ID:36614)
                    SetUserDept();
            }
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

        public virtual void AssignBreadCrumb(string breadCrumbResourceName)
        {
            try
            {
                if (this.GetLocalResourceObject(breadCrumbResourceName) != null && (WebControl)(Page.Form.FindControl("MainContent").FindControl("lblBreadCrum")) != null)
                {
                    string breadCrumb;
                    breadCrumb = this.GetLocalResourceObject(breadCrumbResourceName).ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
                    ((Label)(Page.Form.FindControl("MainContent").FindControl("lblBreadCrum"))).Text = breadCrumb;
                }
            }
            catch (Exception ex)
            {

            }
        }
        /// <summary>
        /// Check user has right to access the page
        /// </summary>
        /// <returns></returns>
        protected bool HasPageRight()
        {
            bool Result = false;
            string pageURL;
            if (string.IsNullOrEmpty(this.WkfPageUrl))
                pageURL = GetPageUrlWithoutParams();
            else
                pageURL = this.WkfPageUrl;
            DataTable dtResult = BusinessLogic.CommonManagement.CommonBL.GetDepartmentPageRight(currentUser.PKUser, pageURL, currentUser.CurrentDeptPK, 1);
            if (dtResult != null && dtResult.Rows.Count > 0)
                Result = true;
            else
                Result = false;

            return Result;
        }
        /// <summary>

        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);

            if (!IsPostBack)
            {
                //assign Page BreadCrumb
                AssignBreadCrumb();
                CurrentUser = GetUserIdentity();

                //if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
                //{
                //    // Use this if has QueryString
                //    //pageURL = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "");
                //    //pageURL = pageURL + Request.Url.Query;

                //    pageURL = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "");
                //}
                //else
                //{
                //    // Use this if has QueryString
                //    //pageURL = Request.Url.PathAndQuery.ToLower();
                //    pageURL = Request.Url.AbsolutePath.ToLower();
                //}

            }
            //Assign click event for delete button
            SetDeleteClickEvent();
            InitializeCulture();
        }

        protected override void OnPreRender(EventArgs e)
        {
            string pageURL;
            base.OnPreRender(e);
            try
            {
                if (string.IsNullOrEmpty(this.WkfPageUrl))
                    pageURL = GetPageUrlWithoutParams();
                else
                    pageURL = this.WkfPageUrl;

                //// get the user rights
                userAuth = new UserAuthBL();
                usrRights = userAuth.GetUserRights(currentUser.PKUser, pageURL, currentUser.SBUID);
                UserRights = usrRights;
                //// Hide the non user right sections

            }
            catch
            {

            }
        }
        #region ------------ Helper Methods -------------------
        public void CheckUserRight(string path)
        {
            currentUser = GetUserIdentity();
            this.WkfPageUrl = path;
            UserAuthBL userAuth = new UserAuthBL();
            UserRightsBO usrRights = userAuth.GetUserRights(currentUser.PKUser, path, currentUser.SBUID);
            if (usrRights.Rights.Count < 1) //If user has no rights defined, prevent page access
            {
                Session.Abandon();
                FormsAuthentication.SignOut();
                Response.Redirect("~/Login.aspx");
            }
        }

        private string GetPageUrlWithoutParams()
        {
            string pageURL;
            if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
                pageURL = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "");
            else
                pageURL = Request.Url.AbsolutePath.ToLower();
            return pageURL;
        }
        /// <summary>
        /// Checks the visibility of the button based on the UserRights for the page
        /// </summary>
        /// <param name="sender"></param>
        public void CheckBtnVisibility(object sender)
        {
            List<string> usrActions;
            string action;
            string section;

            //// Don' process if UserRights is empty
            //if (UserRights == null || UserRights.Rights.Count < 1)
            //{
            //    return;
            //}
            usrActions = new List<string>();

            action = String.Empty;
            section = String.Empty;

            if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
            {
                // Get Action and Section from the button
                action = ((ImageButton)sender).CommandName.ToLower();
                section = ((ImageButton)sender).CommandArgument.ToLower();
            }
            else if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
            {
                // Get Action and Section from the button
                action = ((LinkButton)sender).CommandName.ToLower();
                section = ((LinkButton)sender).CommandArgument.ToLower();
            }
            else if (sender.GetType().IsEquivalentTo(typeof(Button)))
            {
                // Get Action and Section from the button
                action = ((Button)sender).CommandName.ToLower();
                section = ((Button)sender).CommandArgument.ToLower();
            }
            usrActions = (from bp in UserRights.Rights
                          where (bp.SectionName.ToLower() == section && bp.ActionName.ToLower() == action && bp.HasActionRight == true)
                          select bp.ActionName).ToList<string>();

            // If actions don't have any result, then hide the button
            if (usrActions.Count == 0)
            {
                if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    ((ImageButton)sender).Visible = false;
                }
                else if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                {
                    ((LinkButton)sender).Visible = false;
                }
                else if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    ((Button)sender).Visible = false;
                }
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
                               where ((p.HasActionRight == true) && (p.SectionName == usrSection))
                               select p.SectionName).Distinct<string>().ToList<string>();

                if (usrSections.Count > 0)
                {
                    try
                    {
                        secControl = (WebControl)Page.Form.FindControl("MainContent").FindControl(usrSection);
                    }
                    catch
                    {
                        secControl = null;
                    }
                    if (secControl != null)
                    {
                        secControl.Visible = true;
                    }

                }
                else
                {
                    try
                    {
                        secControl = (WebControl)Page.Form.FindControl("MainContent").FindControl(usrSection);
                    }
                    catch
                    {
                        secControl = null;
                    }
                    if (secControl != null)
                    {
                        secControl.Visible = false;
                    }
                }

            }

        }


        /// <summary>
        /// 
        /// </summary>
        public void SetUserDept()
        {
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
            if (!string.IsNullOrEmpty(currentPageURL))
                pageURL = currentPageURL;
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
            System.Web.UI.Control control = null;
            if (Page.Form.FindControl("MainContent") != null)
                control = Page.Form.FindControl("MainContent").FindControl("btnDelete");
            if (control != null)
            {
                ((Button)control).OnClientClick = "return ShowDeleteConfirm(this);";
            }



        }

        /// <summary>
        /// 
        /// </summary>
        private DataTable ConfigurationSettings()
        {
            currentUser = GetUserIdentity();
            DataTable dt = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("Currency Settings", string.Empty, currentUser.SBUID);
            return dt;
        }
        #endregion

        protected override void InitializeCulture()
        {
            base.InitializeCulture();
            var culture = CultureInfo.CreateSpecificCulture(GetGlobalResourceObject("ConfigurationsRes", "DefaultCulture").ToString());
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
                //Qty Decimal Digit for Purchase
                Session[ERP.Utilities.SessionStrings.NumberDecimalDigitsP2P] =
                  Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "NumberDecimalDigitP2P")["ACF_VALUE"].ToString());
                //Decimal Digit for Production(Disp/Compounding)
                if (dt.AsEnumerable().Any(x => x.Field<string>("ACF_DATA") == "NumberDecimalDigitCompounding"))
                {
                    Session[ERP.Utilities.SessionStrings.NumberDecimalDigitsCompounding] =
                    Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "NumberDecimalDigitCompounding")["ACF_VALUE"].ToString());
                }
                //Bincard weight decimal digit-Production
                if (dt.AsEnumerable().Any(x => x.Field<string>("ACF_DATA") == "NumberDecimalDigitBin"))
                {
                    Session[ERP.Utilities.SessionStrings.NumberDecimalDigitBin] = Convert.ToInt32(
                       dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "NumberDecimalDigitBin")["ACF_VALUE"].ToString());
                }
            }

            Thread.CurrentThread.CurrentCulture = culture;

            HttpCookie userCulture = Request.Cookies["Culture"];
            culture = userCulture != null ?
                CultureInfo.CreateSpecificCulture(userCulture.Value) :
                CultureInfo.CreateSpecificCulture(GetGlobalResourceObject("ConfigurationsRes", "DefaultCulture").ToString());
            Page.UICulture = culture.ToString();
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Gets the postback control
        /// </summary>
        /// <returns>null - Not found; control</returns>
        public System.Web.UI.Control GetPostBackControl()
        {
            System.Web.UI.Control control = null;

            string ctrlname = Page.Request.Params.Get("__EVENTTARGET");
            if (ctrlname != null && ctrlname != string.Empty)
            {
                control = Page.FindControl(ctrlname);
            }
            return control;
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

    }
}