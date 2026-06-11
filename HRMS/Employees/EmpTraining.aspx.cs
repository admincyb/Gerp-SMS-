using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERP.Utilities;
using BusinessObject.HRMS.Employee;
using BusinessObject.AccountManagement;
using BusinessLogic.HRMS.Employee;
using DataAccess.CommonManagement;
using System.Data;
using ERPData;
using ERPService;
using ERPManager;
using BusinessObject.CommonManagement;
using BusinessObject;
using System.IO;
using CustomControls;
using BusinessLogic.CommonManagement;
using BusinessObject.Common;
using ERPSMS_v01;

namespace HRMS.Employees
{
    public partial class EmpTraining : ERP.Store.UI.MyBasePage     //System.Web.UI.Page 
    {

        #region VARIABLES AND PROPERTIES
        public int CurrPK
        {
            get
            {
                return Convert.ToInt32(Session[ERP.Utilities.SessionStrings.CurrentPK]);
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.CurrentPK] = value;
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

        public DateTime LastModifiedTime
        {
            get
            {
                return ViewState[ViewstateStrings.LastModifiedTime] == null ? System.DateTime.Now : (DateTime)this.ViewState[ViewstateStrings.LastModifiedTime];
            }
            set
            {
                ViewState[ViewstateStrings.LastModifiedTime] = value;
            }
        }

        private EntryStatus EntryStatus
        {
            get
            {
                return this.ViewState[ViewstateStrings.EntryState] == null ? EntryStatus.LISTMODE : (EntryStatus)(this.ViewState[ViewstateStrings.EntryState]);
            }
            set
            {
                this.ViewState[ViewstateStrings.EntryState] = value;
            }
        }

        private int PageIndex
        {
            get
            {
                return (int)this.ViewState[ViewstateStrings.PageIndex];
            }
            set
            {
                this.ViewState[ViewstateStrings.PageIndex] = value;
            }
        }
        private int PageSize
        {
            get
            {
                return Convert.ToInt32(GetLocalResourceObject("PageSize").ToString());
            }
        }

        /// <summary>
        /// To maintain the total pages in viewstate
        /// </summary>
        private int TotalPages
        {
            get
            {
                return (int)(this.ViewState[ViewstateStrings.TotalPages] ?? 1);
            }
            set
            {
                this.ViewState[ViewstateStrings.TotalPages] = value;
            }
        }

        User currentUser;
        private BusinessObject.AccountManagement.ActionsEnum commonActions;
        private EmployeeBasicInfomtn objEmployeeBasicInfo;
        private DataTable dtempTrainingDet;
        #endregion

        #region PAGE LEVEL EVENTS
        protected void Page_Load(object sender, EventArgs e)
		{
            if (!IsPostBack)
                PageActionHandler();
		}

        private void PageActionHandler()
        {
            try
            {
                if (this.CurrPK > 0)
                {
                    PageIndex = Convert.ToInt16(CommonConstants.SELECT_VALUE_ONE);
                    uclPaging.TotalPages = TotalPages;
                    uclPaging.CurrentPage = 1;
                    hdfPageFlag.Value = PageEnum.Training.ToString();
                    GetFieldValues(ControlsEnum.EMPLOYEEDETAILSBYID);
                    SetFieldValues(ControlsEnum.EMPLOYEEDETAILSBYID);
                    GetFieldValues(ControlsEnum.EMPLOYEETRAININGBYID);
                    SetFieldValues(ControlsEnum.EMPLOYEETRAININGBYID);
                }
                if (this.CurrPK == 0)
                {
                    litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                    Session["SelectMessage"] = litErrorMsg.Text;
                    Response.Redirect(Resources.PageURL.EmployeeList);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion

        #region GET FIELD VALUES

        private void GetFieldValues(ControlsEnum type)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                switch (type)
                {

                    case ControlsEnum.EMPLOYEETRAININGBYID:
                        dtempTrainingDet = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.GetEmployeeTrainingByID(CurrPK);
                        break;
                    case ControlsEnum.EMPLOYEEDETAILSBYID:
                        objEmployeeBasicInfo = EmployeeBasicInfoBL.GetEmployeebyID(CurrPK);
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        #endregion

        #region SET FIELD VALUES

        private void SetFieldValues(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.EMPLOYEEDETAILSBYID:
                        GetUIValuesFromObject(controlType);
                        break;
                    case ControlsEnum.EMPLOYEETRAININGBYID:
                        BindGrid(ControlsEnum.EMPLOYEETRAININGBYID);
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void BindGrid(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region LIST
                    case ControlsEnum.EMPLOYEETRAININGBYID:

                        int rowCount = 0;
                        int pageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));

                        if (dtempTrainingDet != null && dtempTrainingDet.Rows.Count > 0)
                        {
                            rowCount = Convert.ToInt32(dtempTrainingDet.Rows[0]["TOTAL_ROW_COUNT"].ToString());

                            uclPaging.TotalPages = rowCount == 0 ? 1 : (rowCount <= pageSize) ? 1 :
                                  (rowCount % pageSize) == 0 ? (rowCount / pageSize) :
                                  (rowCount / pageSize) + 1;

                            PageIndex = PageIndex == null ? Convert.ToInt32(CommonConstants.SELECT_VALUE_ONE) : PageIndex;
                            uclPaging.CurrentPage = Convert.ToInt32(PageIndex);

                            grdEmpTrainingList.DataSource = dtempTrainingDet;
                            grdEmpTrainingList.DataBind();

                            uclPaging.Visible = true;
                            uclPaging.BindPager();
                        }
                        else
                        {
                            grdEmpTrainingList.DataSource = null;
                            grdEmpTrainingList.DataBind();
                            uclPaging.Visible = false;
                        }
                        break;
                    #endregion
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region GetUIValuesFromObject

        private void GetUIValuesFromObject(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.EMPLOYEEDETAILSBYID:
                        if (objEmployeeBasicInfo != null)
                        {
                            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                            CurrPK = objEmployeeBasicInfo.empPK;
                            UCempBasicHdr.objEmployeeBasicInfo = objEmployeeBasicInfo;
                            UCempBasicHdr.SetFieldValues();
                        }
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region ACTIONHANDLER

        protected void ActionHandler(object sender, EventArgs e)
        {
            if (!(this.Master as ERPSMS_2).ValidatePageDept())
                return;
            try
            {
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (BusinessObject.AccountManagement.ActionsEnum)(Enum.Parse(typeof(BusinessObject.AccountManagement.ActionsEnum), ((Button)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                {
                    commonActions = (BusinessObject.AccountManagement.ActionsEnum)(Enum.Parse(typeof(BusinessObject.AccountManagement.ActionsEnum), ((LinkButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    commonActions = (BusinessObject.AccountManagement.ActionsEnum)(Enum.Parse(typeof(BusinessObject.AccountManagement.ActionsEnum), ((ImageButton)sender).CommandName));
                }

                switch (commonActions)
                {
                    case BusinessObject.AccountManagement.ActionsEnum.CANCEL:
                       CurrPK = 0;
                        if (hdfPageFlag.Value == PageEnum.Training.ToString())
                        {
                            CurrEmployeePayrollPK = 0;
                            Response.Redirect(Resources.PageURL.EmployeeList, true);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        #endregion

        #region ControlEnum
        public enum ControlsEnum
        {
            LIST,
            CLEAR,
            EMPLOYEEDETAILSBYID,
            EMPLOYEETRAININGBYID
        }

        public enum PageEnum
        {
            EmployeeMaster = 0,
            Training = 1
        }

        #endregion

        #region HELPER METHODS

        public string GetFormattedNumber(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            string format = "##0.00";
            string s = num.ToString(format);
            return s;
        }

        public string GetTimeSpan(object pdt)
        {
            if (string.IsNullOrEmpty(Convert.ToString(pdt)))
                return string.Empty;
            DateTime dt;
            try
            {
                dt = Convert.ToDateTime(pdt);
                return dt.ToString(CommonConstants.TIMEFORMAT);
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
        #endregion

    }
}