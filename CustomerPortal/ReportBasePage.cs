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

namespace ERP.Store.UI
{
    public class ReportBasePage : Page
    {
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

        protected override void OnPreLoad(EventArgs e)
        {
            if (!IsPostBack)
            {
                base.OnPreLoad(e);
                SetUserDept();
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

        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            if (!IsPostBack)
            {
                //assign Page BreadCrumb
                AssignBreadCrumb();
            }
            //Assign click event for delete button
            SetDeleteClickEvent();
            InitializeCulture();
        }
        /// <summary>
        /// 
        /// </summary>
        public void SetUserDept()
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
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
                    if (Session[SessionStrings.CurDept] != null)
                        currentUser.CurrentDeptPK = Convert.ToInt32(Session[SessionStrings.CurDept].ToString());
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
        /// 
        /// </summary>
        private DataTable ConfigurationSettings()
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            DataTable dt = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("Currency Settings", string.Empty, currentUser.SBUID);
            return dt;
        }

        protected override void InitializeCulture()
        {
            base.InitializeCulture();
            var culture = CultureInfo.CreateSpecificCulture("en-US");

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
                Session[ERP.Utilities.SessionStrings.RateDecimalDigit] = Convert.ToInt32(
                    dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "RateDecimalDigit")["ACF_VALUE"].ToString());
                Session[ERP.Utilities.SessionStrings.ExchRateDecimalDigit] = Convert.ToInt32(
                   dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "ExchRateDecimalDigit")["ACF_VALUE"].ToString());
                Session[ERP.Utilities.SessionStrings.RateDecimalDigitP2P] = Convert.ToInt32(
                   dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "RateDecimalDigitP2P")["ACF_VALUE"].ToString());
            }
            //Commented by Dhanya on 26-09-17 as the date time culture values not required to be changed. Only UI culture change required
            Thread.CurrentThread.CurrentCulture = culture;

            Thread.CurrentThread.CurrentUICulture = culture;


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

            // Don' process if UserRights is empty
            if (UserRights == null || UserRights.Rights.Count < 1)
            {
                return;
            }
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
    }
}