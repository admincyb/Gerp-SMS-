using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERP.Utilities;
using BusinessObject.HRMS.Employee;
using System.Web.UI.HtmlControls;
using BusinessLogic.CommonManagement;
using BusinessObject;
using System.Data;
using BusinessLogic.HRMS.Employee;
using BusinessObject.CommonManagement;
using System.Web.Security;

namespace HRMS.Employees.UserControls
{
    public partial class PayrollTabControl : System.Web.UI.UserControl
    {
        private ActionsEnum ActiveTab
        {
            get
            {
                return (ActionsEnum)(Session[ERP.Utilities.SessionStrings.ActiveEmployeeTab] ?? ActionsEnum.DEFAULT);
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.ActiveEmployeeTab] = value;
            }
        }
        public int CurrPK
        {
            get
            {
                return Convert.ToInt32(Session[ERP.Utilities.SessionStrings.CurrentPK]);
            }
            set
            {
                this.Session[ERP.Utilities.SessionStrings.CurrentPK] = value;
            }
        }
        private int CurrEmployeePayrollPK
        {
            get
            {
                return Convert.ToInt32(this.Session[ERP.Utilities.SessionStrings.CurrEmployeePayrollPK]);
            }
            set
            {
                this.Session[ERP.Utilities.SessionStrings.CurrEmployeePayrollPK] = value;
            }
        }
        /// <summary>
        /// Used to set the active Tab
        /// </summary>
        public int CurrentTab
        {
            get;
            set;
        }

        User currentUser;
        ActionsEnum commonActions = ActionsEnum.PREPROCESSDATA;
        string redirectUrl = Resources.PageURL.HrmsPayrollPreprocess;
        protected void ActionHandler(object sender, EventArgs e)
        {
            if (!ValidatePageDept())
                return;
            try
            {
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((LinkButton)sender).CommandName));
                }

                switch (commonActions)
                {
                    case ActionsEnum.PREPROCESSDATA:
                        this.ActiveTab = ActionsEnum.PREPROCESSDATA;
                        redirectUrl = Resources.PageURL.HrmsPayrollPreprocess;                       
                        CheckUserRightsAndRedirect(redirectUrl, commonActions);
                        break;
                    case ActionsEnum.SALARYPROCESS:
                        this.ActiveTab = ActionsEnum.SALARYPROCESS;
                        redirectUrl = Resources.PageURL.HrmsPayroll;
                        CheckUserRightsAndRedirect(redirectUrl, commonActions);
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
            finally
            {
            }
        }


        /// <summary>
        /// Redirect to a page if user has permission on that page.
        /// </summary>
        /// <param name="RedirectUrl">Page Url</param>
        private void CheckUserRightsAndRedirect(string RedirectUrl, ActionsEnum commonActions)
        {
            CommonBL userAuth = new CommonBL();
            if (currentUser == null)
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            UserRightsBO UsrRights = userAuth.GetUserPageRights(currentUser.PKUser, RedirectUrl.Replace("~", ""), currentUser.SBUID, currentUser.CurrentDeptPK);
            if (UsrRights != null && UsrRights.Rights.Count() > 0 && UsrRights.Rights[0].UserDeptRight)
            {
                Response.Redirect(RedirectUrl, false);
            }
            else
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(Resources.ErpRes.NoPressionMsg) + "','" + Resources.ErpRes.Information + "');", true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// To Handle Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.currentUser = (BusinessObject.User)Context.User.Identity;
                hdfCurrentDepartment.Value = currentUser.CurrentDeptPK.ToString();
            }
            InactiveAllTabs();
            SetTabVisibility();
            switch (this.CurrentTab)
            {
                #region PRE PROCESS DATA
                case (int)PayrollTabEnum.PREPROCESSDATA:
                    SetTabActive(spnPreprocessData);
                    break;
                #endregion
                #region SALARY PROCESS
                case (int)PayrollTabEnum.SALARYPROCESS:
                    SetTabActive(spnSalaryProcess);
                    break;
                #endregion
                default:
                    break;
            }
        }

        private void SetTabVisibility()
        {
            string[] strInactivetabs = GetGlobalResourceObject("ConfigurationsRes", "HrmsPayrollInactiveTabs").ToString().Split(',');
            if (strInactivetabs != null && strInactivetabs.Count() > 0)
            {
                foreach (string strTab in strInactivetabs)
                {
                    int emptab = 0;
                    int.TryParse(strTab, out emptab);
                    switch (emptab)
                    {
                        #region PRE PROCESS DATA
                        case (int)PayrollTabEnum.PREPROCESSDATA:
                            liPreprocessData.Visible = false;
                            break;
                        #endregion
                        #region SALARY PROCESS
                        case (int)PayrollTabEnum.SALARYPROCESS:
                            liSalaryProcess.Visible = false;
                            break;
                        #endregion
                        default:
                            break;
                    }
                }
            }
        }

        public bool ValidatePageDept()
        {
            bool result = true;
            string redirectURL = "../../login.aspx";
            if (hdfCurrentDepartment.Value != "-1" && hdfCurrentDepartment.Value != ((BusinessObject.User)(HttpContext.Current.User.Identity)).CurrentDeptPK.ToString())
            {
                Session.Abandon();
                FormsAuthentication.SignOut();
                if (System.Configuration.ConfigurationManager.AppSettings["ASSETURL"] != null)
                {
                    redirectURL = System.Configuration.ConfigurationManager.AppSettings["ASSETURL"] + "?Logout=1";
                }
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetGlobalResourceObject("ErrorMessages", "Msg_Dept_Session_Expired").ToString()) + "','" + GetGlobalResourceObject("Messages", "Information").ToString() + "','" + redirectURL + "');", true);
                result = false;
            }
            return result;
        }

        /// <summary>
        /// Set the Tab as active
        /// </summary>
        /// <param name="spnTab"></param>
        private void SetTabActive(HtmlGenericControl spnTab)
        {
            spnTab.Attributes.Add("class", Resources.Controls.TabActive);//GetLocalResourceObject("TabActive").ToString()
        }

        /// <summary>
        /// All tabs style set as Inactive
        /// </summary>
        private void InactiveAllTabs()
        {
            spnPreprocessData.Attributes.Add("class", Resources.Controls.TabInActive);//GetLocalResourceObject("TabInActive").ToString()
            spnSalaryProcess.Attributes.Add("class", Resources.Controls.TabInActive);
        }

    }
}