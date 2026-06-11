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

namespace HRMS.Employees.UserControls
{
    public partial class GtiTabControl : System.Web.UI.UserControl
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
        ActionsEnum commonActions = ActionsEnum.DEFAULT;
        string redirectUrl = Resources.PageURL.HrmsEmpListing;// "~/Employees/EmployeeList.aspx";
        private int EmpPayrollType = 0;
        protected void ActionHandler(object sender, EventArgs e)
        {
            try
            {
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((LinkButton)sender).CommandName));
                }

                switch (commonActions)
                {
                    case ActionsEnum.DEFAULT:
                        this.ActiveTab = ActionsEnum.DEFAULT;
                        redirectUrl = Resources.PageURL.HrmsEmpListing;
                        CurrPK = 0;
                        CheckUserRightsAndRedirect(redirectUrl, commonActions);
                        break;
                    case ActionsEnum.BASICDETAILS:
                        this.ActiveTab = ActionsEnum.BASICDETAILS;
                        redirectUrl = Resources.PageURL.EmployeeBasicDetails;
                        CheckUserRightsAndRedirect(redirectUrl, commonActions);
                        break;
                    case ActionsEnum.QUALIFICATIONS:
                        this.ActiveTab = ActionsEnum.QUALIFICATIONS;
                        redirectUrl = Resources.PageURL.HrmsQualifications;
                        CheckUserRightsAndRedirect(redirectUrl, commonActions);
                        break;
                    case ActionsEnum.EXPERIENCE:
                        this.ActiveTab = ActionsEnum.EXPERIENCE;
                        redirectUrl = Resources.PageURL.HrmsExperience;
                        CheckUserRightsAndRedirect(redirectUrl, commonActions);
                        break;
                    case ActionsEnum.DOCUMENTS:
                        this.ActiveTab = ActionsEnum.DOCUMENTS;
                        redirectUrl = Resources.PageURL.HrmsDocuments;
                        CheckUserRightsAndRedirect(redirectUrl, commonActions);
                        break;
                    case ActionsEnum.PAYDETAILS:
                        this.ActiveTab = ActionsEnum.PAYDETAILS;
                        redirectUrl = Resources.PageURL.HrmsEmpPayDetails;
                        CheckUserRightsAndRedirect(redirectUrl, commonActions);
                        break;
                    case ActionsEnum.SKILLDETAILS:
                        this.ActiveTab = ActionsEnum.SKILLDETAILS;
                        redirectUrl = Resources.PageURL.HrmsSkillDetails;
                        CheckUserRightsAndRedirect(redirectUrl, commonActions);
                        break;
                    case ActionsEnum.LEAVETYPE:
                        this.ActiveTab = ActionsEnum.LEAVETYPE;
                        redirectUrl = Resources.PageURL.HrmsEmpLeaveType;
                        CheckUserRightsAndRedirect(redirectUrl, commonActions);
                        break;
                    case ActionsEnum.SALARYDETAILS:
                        this.ActiveTab = ActionsEnum.SALARYDETAILS;
                        redirectUrl = Resources.PageURL.HrmsSalary;
                        CheckUserRightsAndRedirect(redirectUrl, commonActions);
                        break;
                    case ActionsEnum.SALARYREVISION:
                        this.ActiveTab = ActionsEnum.SALARYREVISION;
                        redirectUrl = Resources.PageURL.HrmsSalaryRevision;
                        CheckUserRightsAndRedirect(redirectUrl, commonActions);
                        break;
                    case ActionsEnum.ITDECLARATION:
                        this.ActiveTab = ActionsEnum.ITDECLARATION;
                        redirectUrl = Resources.PageURL.HrmsIncomeTax;
                        CurrEmployeePayrollPK = 0;  // For IncomeTax Page Navigation
                        CheckUserRightsAndRedirect(redirectUrl, commonActions);
                        break;
                    case ActionsEnum.BEHAVIOUR:
                        this.ActiveTab = ActionsEnum.BEHAVIOUR;
                        redirectUrl = Resources.PageURL.HrmsEmpPerformance;
                        CheckUserRightsAndRedirect(redirectUrl, commonActions);
                        break;
                    case ActionsEnum.TRAINING:
                        this.ActiveTab = ActionsEnum.TRAINING;
                        redirectUrl = Resources.PageURL.HrmsEmpTraining;
                        //Response.Redirect("~/Employees/EmpTraining.aspx");
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
            if ((commonActions != ActionsEnum.BASICDETAILS && commonActions != ActionsEnum.DEFAULT) && CurrPK == 0)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(Resources.Report.Msg_Not_Selected) + "','" + Resources.Messages.Information + "');", true);
                return;
            }
            CommonBL userAuth = new CommonBL();
            if (currentUser == null)
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            UserRightsBO UsrRights = userAuth.GetUserPageRights(currentUser.PKUser, RedirectUrl.Replace("~", ""), currentUser.SBUID, currentUser.CurrentDeptPK);
            if (UsrRights != null && UsrRights.Rights.Count() > 0 && UsrRights.Rights[0].UserDeptRight)
            {
                if (commonActions == ActionsEnum.SALARYDETAILS)
                {
                    EmpPayrollType = 0;
                    DataTable dtPayrollTypeMpg = EmployeePayDetailsBL.GetPayrollTypeUserMapping(currentUser.PKUser, CurrPK, out EmpPayrollType);
                    if (EmpPayrollType == 0)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(Resources.ErrorMessages.Msg_PayrollTypeNotMapped) + "','" + Resources.ErpRes.Information + "');", true);
                        return;
                    }
                    if (dtPayrollTypeMpg != null && dtPayrollTypeMpg.Rows.Count > 0)
                    {
                        Response.Redirect(RedirectUrl, false);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(Resources.ErpRes.NoPressionMsg) + "','" + Resources.ErpRes.Information + "');", true);
                    }
                }
                else
                    Response.Redirect(RedirectUrl, false);
            }
            else
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(Resources.ErpRes.NoPressionMsg) + "','" + Resources.ErpRes.Information + "');", true);
        }

        /// <summary>
        /// To Handle Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            InactiveAllTabs();
            SetTabVisibility();
            switch (this.CurrentTab)
            {
                #region Employee List
                case (int)EmpTabEnum.EmpList:
                    SetTabActive(spnEmpListing);
                    break;
                #endregion
                #region Basic Details
                case (int)EmpTabEnum.EmpDetails:
                    SetTabActive(spnBasicDetails);
                    break;
                #endregion
                #region Qualification
                case (int)EmpTabEnum.Qualifications:
                    SetTabActive(spnQualifications);
                    break;
                #endregion
                #region Experience
                case (int)EmpTabEnum.Experiance:
                    SetTabActive(spnExperience);
                    break;
                #endregion
                #region Skill Details
                case (int)EmpTabEnum.SkillsAndExpertise:
                    SetTabActive(spnSkillDetails);
                    break;
                #endregion
                #region Documents
                case (int)EmpTabEnum.Documents:
                    SetTabActive(spnDocuments);
                    break;
                #endregion
                #region Pay Details
                case (int)EmpTabEnum.PayDetails:
                    //SetTabActive(spnPayDetails);                     
                    break;
                #endregion
                #region Salary Details
                case (int)EmpTabEnum.Salary:
                    SetTabActive(spnSalaryDetails);
                    break;
                #endregion
                #region SALARYREVISION
                case (int)EmpTabEnum.SalaryRevision:
                    SetTabActive(spnSalaryRevision);
                    break;
                #endregion
                #region LEAVE TYPE
                case (int)EmpTabEnum.LeaveType:
                    SetTabActive(spnLeaveType);
                    break;
                #endregion
                #region IT DECLARATION
                case (int)EmpTabEnum.ITDeclaration:
                    SetTabActive(spnItDeclaration);
                    break;
                #endregion
                #region BEHAVIOUR/PERFORMANCE
                case (int)EmpTabEnum.BEHAVIOUR:
                    SetTabActive(spnHrmsBehaviour);
                    break;
                #endregion
                #region TRAINING
                case (int)EmpTabEnum.TRAINING:
                    SetTabActive(spnHrmsTraining);
                    break;
                #endregion
                default:
                    break;
            }
        }

        private void SetTabVisibility()
        {
            string[] strInactivetabs = GetGlobalResourceObject("ConfigurationsRes", "HrmsEmpInactiveTabs").ToString().Split(',');
            if (strInactivetabs != null && strInactivetabs.Count() > 0)
            {
                foreach (string strTab in strInactivetabs)
                {
                    int emptab = 0;
                    int.TryParse(strTab, out emptab);
                    switch (emptab)
                    {
                        #region Employee List
                        case (int)EmpTabEnum.EmpList:
                            liEmpListing.Visible = false;
                            break;
                        #endregion
                        #region Basic Details
                        case (int)EmpTabEnum.EmpDetails:
                            liBasicDetails.Visible = false;
                            break;
                        #endregion
                        #region Qualification
                        case (int)EmpTabEnum.Qualifications:
                            liQualifications.Visible = false;
                            break;
                        #endregion
                        #region Experience
                        case (int)EmpTabEnum.Experiance:
                            liExperience.Visible = false;
                            break;
                        #endregion
                        #region Skill Details
                        case (int)EmpTabEnum.SkillsAndExpertise:
                            liSkillDetails.Visible = false;
                            break;
                        #endregion
                        #region Documents
                        case (int)EmpTabEnum.Documents:
                            liDocuments.Visible = false;
                            break;
                        #endregion
                        #region Pay Details
                        case (int)EmpTabEnum.PayDetails:
                            //SetTabActive(spnPayDetails);                     
                            break;
                        #endregion
                        #region Salary Details
                        case (int)EmpTabEnum.Salary:
                            liSalaryDetails.Visible = false;
                            break;
                        #endregion
                        #region SALARYREVISION
                        case (int)EmpTabEnum.SalaryRevision:
                            liSalaryRevision.Visible = false;
                            break;
                        #endregion
                        #region LEAVE TYPE
                        case (int)EmpTabEnum.LeaveType:
                            liLeaveType.Visible = false;
                            break;
                        #endregion
                        #region IT DECLARATION
                        case (int)EmpTabEnum.ITDeclaration:
                            liItDeclaration.Visible = false;
                            break;
                        #endregion
                        #region BEHAVIOUR
                        case (int)EmpTabEnum.BEHAVIOUR:
                            liBehaviour.Visible = false;
                            break;
                        #endregion
                        #region TRAINING
                        case (int)EmpTabEnum.TRAINING:
                            liTraining.Visible = false;
                            break;
                        #endregion

                        default:
                            break;
                    }
                }
            }
        }


        /// <summary>
        /// Set the Tab as active
        /// </summary>
        /// <param name="spnTab"></param>
        private void SetTabActive(HtmlGenericControl spnTab)
        {
            spnTab.Attributes.Add("class", GetLocalResourceObject("TabActive").ToString());
        }

        /// <summary>
        /// All tabs style set as Inactive
        /// </summary>
        private void InactiveAllTabs()
        {
            spnEmpListing.Attributes.Add("class", GetLocalResourceObject("TabInactive").ToString());
            spnBasicDetails.Attributes.Add("class", GetLocalResourceObject("TabInactive").ToString());
            spnQualifications.Attributes.Add("class", GetLocalResourceObject("TabInactive").ToString());
            spnExperience.Attributes.Add("class", GetLocalResourceObject("TabInactive").ToString());
            spnSkillDetails.Attributes.Add("class", GetLocalResourceObject("TabInactive").ToString());
            spnDocuments.Attributes.Add("class", GetLocalResourceObject("TabInactive").ToString());
            //spnPayDetails.Attributes.Add("class", GetLocalResourceObject("TabInactive").ToString());
            spnLeaveType.Attributes.Add("class", GetLocalResourceObject("TabInactive").ToString());
            spnSalaryDetails.Attributes.Add("class", GetLocalResourceObject("TabInactive").ToString());
            spnSalaryRevision.Attributes.Add("class", GetLocalResourceObject("TabInactive").ToString());
            spnItDeclaration.Attributes.Add("class", GetLocalResourceObject("TabInactive").ToString());
            spnHrmsBehaviour.Attributes.Add("class", GetLocalResourceObject("TabInactive").ToString());
            spnHrmsTraining.Attributes.Add("class", GetLocalResourceObject("TabInactive").ToString());
        }



    }
}