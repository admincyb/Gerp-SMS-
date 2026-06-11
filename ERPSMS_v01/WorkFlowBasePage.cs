using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic.CommonManagement;
using BusinessObject;
using BusinessObject.Common;
using ERPSMS_v01.UserControls;
using ERP.Utilities;

namespace ERP.Store.UI
{
    public class WorkFlowBasePage : Page
    {
        #region Properties and Variables
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
        /// WorkFlow Page Type
        /// </summary>
        public int WkfPageType
        {
            get
            {
                return (this.ViewState["BaseWkfPageType"] == null ? 0 : (int)this.ViewState["BaseWkfPageType"]);
            }
            set
            {
                this.ViewState["BaseWkfPageType"] = value;
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
        /// <summary>
        /// WorkFlow RefID
        /// </summary>
        public int WkfRefID
        {
            get
            {
                return (this.ViewState["BaseWkfRefID"] == null ? 0 : (int)this.ViewState["BaseWkfRefID"]);
            }
            set
            {
                this.ViewState["BaseWkfRefID"] = value;
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

        BusinessObject.User currentUser;
        #endregion

        #region Page Events
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            currentUser = (BusinessObject.User)HttpContext.Current.User.Identity;
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
            base.InitializeCulture();
            var culture = CultureInfo.CreateSpecificCulture(GetGlobalResourceObject("ConfigurationsRes", "DefaultCulture").ToString());
            //Get Number & Currency Culture Format 
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
                Session[ERP.Utilities.SessionStrings.WeightDecimalDigit] = Convert.ToInt32(
                    dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "WeightDecimalDigit")["ACF_VALUE"].ToString());
                //Qty Decimal Digit for Purchase
                Session[ERP.Utilities.SessionStrings.NumberDecimalDigitsP2P] =
                  Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "NumberDecimalDigitP2P")["ACF_VALUE"].ToString());
                //Decimal Digit for Production(Disp/Compounding)
                if (dt.AsEnumerable().Any(x => x.Field<string>("ACF_DATA") == "NumberDecimalDigitCompounding"))
                {
                    Session[ERP.Utilities.SessionStrings.NumberDecimalDigitsCompounding] =
                      Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "NumberDecimalDigitCompounding")["ACF_VALUE"].ToString());
                }
                // Miscellaneous rate decimal digit
                Session[ERP.Utilities.SessionStrings.MiscRateDecimalDigit] = Convert.ToInt32(
                   dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "MiscRateDecimalDigit")["ACF_VALUE"].ToString());
                //Bincard weight decimal digit-Production
                Session[ERP.Utilities.SessionStrings.NumberDecimalDigitBin] = Convert.ToInt32(
                   dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "NumberDecimalDigitBin")["ACF_VALUE"].ToString());
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
            CultureInfo culture;
            HiddenField hdfAmtDecimalB;
            HiddenField hdfQtyDecimalB;
            HiddenField hdfRateDecimalB;
            HiddenField hdfWeightDecimalDigitB;
            HiddenField hdfExcRateDecimalB;
            HiddenField hdfQtyDecimalBP2P; //Qty Decimal Digit for Purchase
            HiddenField hdfCompoundingDecimal;//Dispersion/Compounding for Production
            HiddenField hdfMiscRateDecimalDigit;
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


                hdfWeightDecimalDigitB = this.Page.Master.FindControl("hdfWeightDecimalDigit") as HiddenField;
                if (hdfWeightDecimalDigitB != null)
                    hdfWeightDecimalDigitB.Value = Session[ERP.Utilities.SessionStrings.WeightDecimalDigit].ToString();

                hdfExcRateDecimalB = this.Page.Master.FindControl("hdfExcRateDecimal") as HiddenField;
                if (hdfExcRateDecimalB != null)
                    hdfExcRateDecimalB.Value = Session[ERP.Utilities.SessionStrings.ExchRateDecimalDigit].ToString();

                hdfMiscRateDecimalDigit = this.Page.Master.FindControl("hdfMiscRateDecimalDigit") as HiddenField;
                if (hdfMiscRateDecimalDigit != null)
                    hdfMiscRateDecimalDigit.Value = Session[ERP.Utilities.SessionStrings.MiscRateDecimalDigit].ToString();

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

                //Decimal for Dispersion/Compounding- Production
                hdfNumberDecimalDigitBin = this.Page.Master.FindControl("hdfNumberDecimalDigitBin") as HiddenField;
                if (Session[ERP.Utilities.SessionStrings.NumberDecimalDigitBin] != null && hdfNumberDecimalDigitBin != null)
                    hdfNumberDecimalDigitBin.Value = Session[ERP.Utilities.SessionStrings.NumberDecimalDigitBin].ToString();
                else
                    hdfNumberDecimalDigitBin.Value = "0";

                base.OnPreLoad(e);
                SetUserDept();
            }
        }
        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            try
            {
                if (!IsPostBack)
                {
                    //assign Page BreadCrumb
                    AssignBreadCrumb();
                    currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                    SetPageDepartmentText();
                }
                //Assign click event for delete button
                SetDeleteClickEvent();
            }
            catch
            {

            }
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            try
            {
                GetWorkflowUserRights();
                if (UserRights.Rights.Count < 1) //If user has no rights defined, prevent page access
                {
                    Session.Abandon();
                    FormsAuthentication.SignOut();
                    Response.Redirect("~/Login.aspx");
                }
                // Hide the non user right sections
                Hide(UserRights);
            }
            catch
            {

            }
        }
        #endregion
        #region Helper Methods
        /// <summary>
        /// Get Number & Currency Culture Format 
        /// </summary>
        private DataTable ConfigurationSettings()
        {
            DataTable dt = new DataTable();
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                dt = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("Currency Settings", string.Empty, currentUser.SBUID);
            }
            else
            {
                System.Web.Security.FormsAuthentication.SignOut();
                System.Web.HttpContext.Current.User = null;
                Response.Redirect("~/Login.aspx", true);
            }
            return dt;
        }
        /// <summary>
        /// Set Department Text
        /// </summary>
        public void SetPageDepartmentText()
        {
            if (this.Page != null && this.Page.Master != null)
            {
                Label lblPageDeptText = this.Page.Form.FindControl("MainContent").FindControl("lblPageDeptText") as Label;
                Literal litDept = this.Page.Master.FindControl("litDept") as Literal;

                if (lblPageDeptText != null && litDept != null)
                {
                    lblPageDeptText.Text = ERP.Utilities.CommonFunctions.GetShortString(litDept.Text, 20); //strText[1];
                    lblPageDeptText.ToolTip = litDept.Text;
                }
            }
        }
        /// <summary>
        /// Set Department from Department Session
        /// </summary>
        public void SetUserDept()
        {
            int curDep;
            if (Request.QueryString[QueryStrings.ProcessDept] != null)
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    currentUser = (BusinessObject.User)HttpContext.Current.User.Identity;
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
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            string pageURL = "";
            pageURL = GetPageUrlWithoutParams();
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
                    if (this.Master != null)
                    {
                        MenuControl ctrlMenu = (MenuControl)this.Master.FindControl("MenuControl1");
                        if (ctrlMenu != null)
                        {
                            DropDownList ddlCostCenter = (DropDownList)ctrlMenu.FindControl("ddlCostCenter");
                            if (ddlCostCenter != null && Session[BusinessObject.Common.SessionStrings.CurDept] != null)
                            {
                                ddlCostCenter.SelectedIndex = ddlCostCenter.Items.IndexOf(ddlCostCenter.Items.FindByValue(Session[BusinessObject.Common.SessionStrings.CurDept].ToString()));
                                Literal litDeptCaption = (Literal)this.Page.Master.FindControl("litDeptCaption");
                                litDeptCaption.Text = Resources.Messages.DepartmentDisplayCaption.ToString();
                                Literal litDept = (Literal)this.Page.Master.FindControl("litDept");
                                if (litDept != null && ddlCostCenter.SelectedItem != null)
                                {
                                    ctrlMenu.FillMenu(false);
                                    litDept.Text = ddlCostCenter.SelectedItem.Text;
                                }
                            }
                        }
                    }
                }
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
        /// Get User WorkFlow Rights
        /// </summary>
        public void GetWorkflowUserRights()
        {
            #region Variables
            string pageURL;
            CommonBL userAuth;
            #endregion

            if (string.IsNullOrEmpty(this.WkfPageUrl))
                pageURL = GetPageUrlWithoutParams();
            else
                pageURL = this.WkfPageUrl;
            userAuth = new CommonBL();
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            //// get the user WorkFlow Rights
            UserRights = userAuth.GetWorkFlowUserRightsInfo(currentUser.PKUser, pageURL, currentUser.CurrentDeptPK, WkfRefID, WkfPageType);
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
