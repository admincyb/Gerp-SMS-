using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERP.Utilities;
using System.Data;
using BusinessObject.HRMS.Employee;
using System.Web.Security;

namespace HRMS.Employees.UserControls
{
    public partial class EmpBasicInfoControl : System.Web.UI.UserControl
    {
        //private DataTable dtemployeeHeader;
        //private EmployeeBasicInfomtn objEmployeeBasicInfo;
        public EmployeeBasicInfomtn objEmployeeBasicInfo
        {
            get
            {
                return (EmployeeBasicInfomtn)this.ViewState["EmployeeBasicInfo"];
            }
            set
            {
                this.ViewState["EmployeeBasicInfo"] = value;
            }
        }
        #region  Properties
        public int CurrPK
        {
            get
            {
                return Convert.ToInt32(Session[SessionStrings.CurrentPK]);
            }
            set
            {
                Session[SessionStrings.CurrentPK] = value;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            BusinessObject.User currentUser = (BusinessObject.User)Context.User.Identity;
            if (!IsPostBack)
            {
                hdfCurrentDepartment.Value = currentUser.CurrentDeptPK.ToString();
                PageActionHandler();
            }
        }

        protected void Page_PreRender(Object sender, EventArgs e)
        {
            BusinessObject.User currentUser = (BusinessObject.User)Context.User.Identity;
            if (!IsPostBack)
            {
                hdfCurrentDepartment.Value = currentUser.CurrentDeptPK.ToString();
            }
        }
        private void PageActionHandler()
        {
            if (this.CurrPK == 0)
            {
                divEmployeeHeader.Visible = false;
            }
        }

        //private void GetFieldValues(ControlsEnum controlType)
        //{
        //    try
        //    {
        //        switch (controlType)
        //        {
        //            case ControlsEnum.EMPLOYEEDETAILSHEADER:
        //                dtemployeeHeader = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.GetEmployeeDetailListHeader(CurrPK, string.Empty);
        //                break;
        //            case ControlsEnum.EMPLOYEEDETAILSBYID:
        //                objEmployeeBasicInfo = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.GetEmployeebyID(CurrPK);
        //                break;

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //private void SetFieldValues(ControlsEnum controlType)
        //{
        //    try
        //    {
        //        switch (controlType)
        //        {
        //            case ControlsEnum.EMPLOYEEDETAILSHEADER:
        //                GetUIValuesFromObjectHeader(controlType);
        //                break;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public void SetFieldValues()
        {
            try
            {
                GetUIValuesFromObjectHeader(ControlsEnum.EMPLOYEEDETAILSHEADER);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GetUIValuesFromObjectHeader(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.EMPLOYEEDETAILSHEADER:


                        if (objEmployeeBasicInfo != null)
                        {
                            //currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                            CurrPK = objEmployeeBasicInfo.empPK;
                            //lblhdrEmployeeNoTxt.Text = CommonFunctions.GetShortString(HttpUtility.HtmlDecode(objEmployeeBasicInfo.empCode), 20);
                            //lblhdrEmployeeNameTxt.Text = CommonFunctions.GetShortString(HttpUtility.HtmlDecode(objEmployeeBasicInfo.empNameText), 20);
                            //lblhdrEmployeeTxt.Text = CommonFunctions.GetShortString(
                            //    HttpUtility.HtmlDecode(objEmployeeBasicInfo.empCode) + " - " + HttpUtility.HtmlDecode(objEmployeeBasicInfo.empNameText), 43);
                            lblhdrEmployeeTxt.Text = lblEmpName.Text = CommonFunctions.GetShortString(HttpUtility.HtmlDecode(objEmployeeBasicInfo.empName_txt), 43);
                            lblhdrEmployeeTxt.ToolTip = lblEmpName.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empName_txt);

                            if (!string.IsNullOrEmpty(objEmployeeBasicInfo.empDOJText))
                            {
                                string dojText = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empDOJText).Trim(); ;
                                if (!dojText.IsNullOrEmptyOrWhitespace() && dojText[dojText.Length - 2] == ' ')
                                {
                                    dojText = dojText.Remove(dojText.Length - 2, 1);
                                }
                                lblhdrDOJText.Text = dojText;
                            }
                            lblhdrDOJText.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empDOJText);


                            if (!string.IsNullOrEmpty(objEmployeeBasicInfo.empDOBText))
                            {
                                string dobText = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empDOBText).Trim();
                                if (!dobText.IsNullOrEmptyOrWhitespace() && dobText[dobText.Length - 2] == ' ')
                                {
                                    dobText = dobText.Remove(dobText.Length - 2, 1);
                                }
                                lblDOB.Text = lblDOB.ToolTip = dobText;
                            }

                            lblhdrDesignationTxt.Text = CommonFunctions.GetShortString(HttpUtility.HtmlDecode(objEmployeeBasicInfo.empDesignationText), 23);
                            lblhdrDesignationTxt.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empDesignationText);
                            lblhdrDepartmentTxt.Text = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empDepartmentCode);
                            lblhdrDepartmentTxt.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empDepartmentText);
                            //lblhdrEmployeeNoTxt.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empCode);
                            //lblhdrEmployeeNameTxt.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empNameText);
                            //lblhdrEmployeeTxt.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empCode)
                            //    + " - " + HttpUtility.HtmlDecode(objEmployeeBasicInfo.empNameText);


                            //lblhdrDOBTxt.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empDOBText);

                            lblhdrLocationTxt.Text = lblEmpLoc.Text = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empBranchCode);
                            lblhdrLocationTxt.ToolTip = lblEmpLoc.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empBranchText);

                            lblPassportNumber.Text = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empPassportText);
                            lblPassportNumber.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empPassportText);
                            lblEmployementType.Text = HttpUtility.HtmlDecode(objEmployeeBasicInfo.EPD_EMP_TYPE_TEXT);
                            lblEmployementType.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.EPD_EMP_TYPE_TEXT);
                            lblID.Text = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empCode);
                            lblID.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empCode);
                        }

                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
                // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.gComsRes.Information + "');", true);
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

        #region Action Handler
        /// <summary>
        /// For Button Click 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ActionHandler(object sender, EventArgs e)
        {
            if (!ValidatePageDept())
                return;
            try
            {
                #region Getting Command Action
                ActionsEnum commonActions = ActionsEnum.DEFAULT;
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    //if (((ImageButton)sender).ID == "imbSearch") 
                    //    commonActions = ActionsEnum.CHANGE;
                    //else 
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    //if ((((DropDownList)sender).ID == "ddlEmployee"))
                    //{
                    //    commonActions = ActionsEnum.CHANGE;
                    //}                  
                }
                else if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((LinkButton)sender).CommandName));
                }
                #endregion
                switch (commonActions)
                {
                    #region DETAIL
                    case ActionsEnum.DETAIL:
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ClosePopup();ShowContainerDiv('[id$=divInfoPopUp]','"
                               + GetLocalResourceObject("Information").ToString() + "','650','250');", true);
                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion
    }
}