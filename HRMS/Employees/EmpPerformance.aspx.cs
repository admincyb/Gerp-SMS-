using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERP.Utilities;
using BusinessObject;
using BusinessObject.HRMS.Employee;
using System.Data;
using BusinessLogic.HRMS.Employee;
using BusinessObject.CommonManagement;
using System.Threading;
using BusinessLogic.CommonManagement;
using BusinessObject.Common;
using System.IO;
using System.Configuration;
using ERPSMS_v01.UserControls;
using ERPManager;
using ERPData;
using ERPSMS_v01;

namespace HRMS.Employees
{
    public partial class Behavior : ERP.Store.UI.MyBasePage
    {
        #region Variables
        private EmployeeBasicInfomtn objEmployeeBasicInfo;
        EmpDocUploadBinder blUploadObj;
        private EmployeePerformanceBO objEmployeePerformance;
        private DataTable dtList;
        private DataTable dtAction;
        private DataTable dtCategory;
        private DataTable dtCompany;
        private BusinessObject.AccountManagement.ActionsEnum commonActions;
        User currentUser;
        #endregion
        #region Properties
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
        private int SelectedPK
        {
            get
            {
                return (int)(this.ViewState[ViewstateStrings.SelectedPK] ?? 0);
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedPK] = value;
            }
        }
        private int CurrSlNo
        {
            get
            {
                return (int)(this.ViewState[ViewstateStrings.CurrSlNo] ?? 0);
            }
            set
            {
                ViewState[ERP.Utilities.ViewstateStrings.CurrSlNo] = value;
            }
        }
        private DateTime LastModifiedTime
        {
            get
            {
                return (DateTime)(this.ViewState[ViewstateStrings.LastModifiedTime] ?? System.DateTime.Now);
            }
            set
            {
                this.ViewState[ViewstateStrings.LastModifiedTime] = value;
            }
        }
        private List<EmpDocUploadBinder> BLUploadList
        {
            get
            {
                return ViewState[ViewstateStrings.BLUploadList] == null ? null : (List<EmpDocUploadBinder>)ViewState[ViewstateStrings.BLUploadList];
            }
            set
            {
                ViewState[ViewstateStrings.BLUploadList] = value;
            }
        }
        private List<FileDetails> FileDetailsList
        {
            get
            {
                return Session[ERP.Utilities.SessionStrings.FileBLDetailsList] == null ? null : (List<FileDetails>)Session[ERP.Utilities.SessionStrings.FileBLDetailsList];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.FileBLDetailsList] = value;
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
        /// <summary>
        /// To maintain the PageIndex in viewstate
        /// </summary>
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

        /// <summary>
        /// Entry State for managing display status
        /// </summary>
        private EntryStatus EntryStatusHdr
        {
            get
            {
                return this.Session[ViewstateStrings.EntryState] == null ? EntryStatus.LISTMODE : (EntryStatus)(this.Session[ViewstateStrings.EntryState]);
            }
            set
            {
                this.Session[ViewstateStrings.EntryState] = value;
            }
        }
        #endregion       
        #region Helper Methods
        #region PageActionHandler
        private void PageActionHandler()
        {
            
            if (this.CurrPK != 0)
            {
                PageIndex = Convert.ToInt16(CommonConstants.SELECT_VALUE_ONE);
                uclPaging.TotalPages = TotalPages;
                uclPaging.CurrentPage = 1;
                dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(DbActiveStatus.ACTIVE), currentUser.SBUID, 0);
                if (dtCompany != null && dtCompany.Rows.Count > 0)
                {
                    hdfCompany.Value = dtCompany.Rows[0][Resources.DataFieldRes.CompanyMstPK].ToString();
                }
                ddlAction.Enabled = false;
                GetFieldValues(ControlsEnum.EMPLOYEEDETAILSBYID);
                SetFieldValues(ControlsEnum.EMPLOYEEDETAILSBYID);
                GetFieldValues(ControlsEnum.LIST);
                SetFieldValues(ControlsEnum.LIST);
                GetFieldValues(ControlsEnum.Category);
                SetFieldValues(ControlsEnum.Category);
                GetFieldValues(ControlsEnum.Action);
                SetFieldValues(ControlsEnum.Action);
                if (dtList.Rows.Count <= 0)
                {
                    ResetForm(ControlsEnum.CLEAR);
                    btnDelete.Visible = false;
                    ModifiedDatePnl.Visible = false;
                    EntryStatus = EntryStatus.NEWMODE;
                }
            }
            if (this.CurrPK == 0)
            {
                litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                Session["SelectMessage"] = litErrorMsg.Text;
                Response.Redirect(Resources.PageURL.EmployeeList);
            }
        }
        #endregion
        #region GetFieldValues
        private void GetFieldValues(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region Get Employee Details
                    case ControlsEnum.EMPLOYEEDETAILSBYID:
                        objEmployeeBasicInfo = EmployeeBasicInfoBL.GetEmployeebyID(CurrPK);
                        break;
                    #endregion
                    case ControlsEnum.LIST:
                        dtList = EmployeePerformanceBL.GetEmployeePerformanceList(CurrPK);
                        break;
                    #region Category
                    case ControlsEnum.Category:
                        dtCategory = EmployeePerformanceBL.GetCategory(Convert.ToInt32(ConstGroupType.HRMSSetup), Convert.ToInt16(currentUser.SBUID), Convert.ToInt16(DbActiveStatus.ACTIVE), GetLocalResourceObject("CategoryConfig").ToString());
                        break;
                    #endregion
                    #region Action
                    case ControlsEnum.Action:
                        dtAction = EmployeePerformanceBL.GetAction(Convert.ToInt32(ConstGroupType.HRMSSetup), Convert.ToInt16(currentUser.SBUID), Convert.ToInt16(DbActiveStatus.ACTIVE),Convert.ToInt32( ddlCategory.SelectedValue));
                        break;
                    #endregion
                    case ControlsEnum.EMPLOYEEPERFORMANCEBYPK:
                        objEmployeePerformance = EmployeePerformanceBL.GetEmployeePerformanceByPk(SelectedPK);
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
        #region SetFieldValues
        private void SetFieldValues(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region Set Employee Details By Id
                    case ControlsEnum.EMPLOYEEDETAILSBYID:
                        GetUIValuesFromObject(controlType);
                        break;
                    #endregion
                    #region LIST
                    case ControlsEnum.LIST:
                            BindGrid(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region UPLOADEDFILES
                    case ControlsEnum.UPLOADEDFILES:
                        BindGrid(ControlsEnum.UPLOADEDFILES);
                        break;
                    #endregion
                    #region Category
                    case ControlsEnum.Category:
                        BindDropDown(ControlsEnum.Category);
                        break;
                    #endregion
                    #region Action
                    case ControlsEnum.Action:
                        BindDropDown(ControlsEnum.Action);
                        break;
                    #endregion
                    #region EMPLOYEEPERFORMANCE BY PK
                    case ControlsEnum.EMPLOYEEPERFORMANCEBYPK:
                        GetUIValuesFromObject(ControlsEnum.EMPLOYEEPERFORMANCEBYPK);
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
                    #region Get - Employee Details
                    case ControlsEnum.EMPLOYEEDETAILSBYID:
                        if (objEmployeeBasicInfo != null)
                        {
                            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                            CurrPK = objEmployeeBasicInfo.empPK;
                            UCempBasicHdr.objEmployeeBasicInfo = objEmployeeBasicInfo;
                            UCempBasicHdr.SetFieldValues();
                        }
                        break;
                    #endregion
                    #region Get - Employee Performance Details
                    case ControlsEnum.EMPLOYEEPERFORMANCEBYPK:
                        if (objEmployeePerformance != null)
                        {
                            SelectedPK = objEmployeePerformance.EPD_PK;
                            CurrPK = objEmployeePerformance.EPD_EMPLOYEE;
                            txtDate.Text = string.IsNullOrEmpty(objEmployeePerformance.EPD_DATE) ? string.Empty : Convert.ToDateTime(objEmployeePerformance.EPD_DATE).ToString(Resources.Constants.HRMSDateFormatShort);
                            hdfDoneByEmployee.Value = objEmployeePerformance.EPD_DONE_BY.ToString();
                            txtDoneBy.Text = objEmployeePerformance.EPD_DONE_BY_TEXT.HtmlDecode();
                            ddlCategory.SelectedValue = objEmployeePerformance.EPD_CATEGORY;
                            GetFieldValues(ControlsEnum.Action);
                            SetFieldValues(ControlsEnum.Action);
                            ddlAction.SelectedValue = objEmployeePerformance.EPD_ACTION;
                            ddlAction.Enabled = true;
                            txtIncident.Text = objEmployeePerformance.EPD_INCIDENT.HtmlDecode();
                            LastModifiedTime = objEmployeePerformance.EPD_MOD_DT;
                            this.BLUploadList = objEmployeePerformance.DocDetails;
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
        #region SetUIValuesToObject
        private Object SetUIValuesToObject(ControlsEnum controlType)
        {
            object returnObject = new object();
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                switch (controlType)
                {
                    #region ADD EMP PERFORMANCE DETAILS
                    case ControlsEnum.ADDEMPPERFORMANCEDETAILS:
                       EmployeePerformanceBO tempEmployeePerformance = new EmployeePerformanceBO();
                        tempEmployeePerformance.EPD_PK = SelectedPK;
                        tempEmployeePerformance.EPD_EMPLOYEE = CurrPK;
                        tempEmployeePerformance.EPD_DATE = string.IsNullOrEmpty(txtDate.Text.Trim()) ? null : txtDate.Text.Trim(); 
                        tempEmployeePerformance.EPD_DONE_BY =Convert.ToInt32(hdfDoneByEmployee.Value);
                        tempEmployeePerformance.EPD_CATEGORY = (ddlCategory.SelectedValue == "-1" ? null : (ddlCategory.SelectedValue));
                        tempEmployeePerformance.EPD_ACTION = (ddlAction.SelectedValue == "-1" ? null : ddlAction.SelectedValue);
                        tempEmployeePerformance.EPD_INCIDENT = txtIncident.Text.HtmlDecode();
                        tempEmployeePerformance.EPD_FOLLOW_UP = string.Empty;
                        tempEmployeePerformance.EPD_COMMENT = string.Empty;
                        tempEmployeePerformance.EPD_ACTIVE = Convert.ToInt16(DbActiveStatus.ACTIVE);
                        tempEmployeePerformance.EPD_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                        tempEmployeePerformance.EPD_MOD_DT = LastModifiedTime;
                        tempEmployeePerformance.USER_PK = currentUser.PKUser;
                        tempEmployeePerformance.DocDetails = this.BLUploadList;
                        returnObject = tempEmployeePerformance;
                        break;
                    #endregion

                    default:
                        break;
                }
                return returnObject;
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
        #region BindDropDown
        private void BindDropDown(ControlsEnum controlsEnum)
        {
            switch (controlsEnum)
            {
                #region Category
                case ControlsEnum.Category:
                    ddlCategory.Items.Clear();
                    if (dtCategory != null && dtCategory.Rows.Count > 0)
                    {
                        ddlCategory.DataSource = dtCategory;
                        ddlCategory.DataTextField = GTIService.Constants.Common.Common.CNG_NAME;
                        ddlCategory.DataValueField = GTIService.Constants.Common.Common.CNG_PK;
                        ddlCategory.DataBind();
                    }
                    ddlCategory.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region Action
                case ControlsEnum.Action:
                    ddlAction.Items.Clear();
                    if (dtAction != null && dtAction.Rows.Count > 0)
                    {
                        ddlAction.DataSource = dtAction;
                        ddlAction.DataTextField = GTIService.Constants.Common.Common.CON_NAME;
                        ddlAction.DataValueField = GTIService.Constants.Common.Common.CON_PK;
                        ddlAction.DataBind();
                    }
                    ddlAction.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                default:
                    break;
            }
        }
        #endregion
        #region BindGrid
        /// <summary>
        /// Method for Grid binding
        /// </summary>
        public void BindGrid(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region LIST
                    case ControlsEnum.LIST:
                        uclPaging.Visible = false;
                        if (dtList != null && dtList.Rows.Count > 0)
                        {
                            int rowCount = 0;
                            rowCount = Convert.ToInt32(dtList.Rows[0]["TOTAL_ROW_COUNT"].ToString());
                            uclPaging.TotalPages = rowCount == 0 ? 1 : (rowCount <= this.PageSize) ? 1 :
                                              (rowCount % this.PageSize) == 0 ? (rowCount / this.PageSize) :
                                              (rowCount / this.PageSize) + 1;
                            PageIndex = PageIndex == null ? Convert.ToInt32(CommonConstants.SELECT_VALUE_ONE) : PageIndex;
                            uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                            grdEmployeePerformancelist.DataSource = dtList;
                            grdEmployeePerformancelist.DataBind();
                            uclPaging.Visible = true;
                            uclPaging.BindPager();
                        }
                        else
                        {
                            grdEmployeePerformancelist.DataSource = null;
                            grdEmployeePerformancelist.DataBind();
                        }
                        break;
                    #endregion
                    #region UPLOADEDFILES
                    case ControlsEnum.UPLOADEDFILES:
                        if (BLUploadList != null && BLUploadList.Count > 0)
                        {
                            grdUploads.DataSource = BLUploadList;
                            grdUploads.DataBind();
                        }
                        else
                        {
                            grdUploads.DataSource = null;
                            grdUploads.DataBind();
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
        #region GetNullableInt
        private int? GetNullableInt(string str)
        {
            int result;
            return (int.TryParse(str, out result) ? (int?)result : null);
        }
         #endregion
        #region ResetForm
        private void ResetForm(ControlsEnum controlType)
        {
            switch (controlType)
            {
                # region CLEAR
                case ControlsEnum.CLEAR:
                    txtDate.Text = string.Empty;
                    txtIncident.Text = string.Empty;
                    txtDoneBy.Text = string.Empty;
                    hdfDoneByEmployee.Value = "0";
                    uclPaging.CurrentPage = 0;
                    PageIndex = 1;
                    SelectedPK = 0;
                    ddlAction.SelectedValue = CommonConstants.SELECTVAL;
                    ddlCategory.SelectedValue = CommonConstants.SELECTVAL;
                    ddlAction.Enabled = false;
                    BLUploadList = null;
                    break;
                #endregion
                # region CLEAR DETAILS
                case ControlsEnum.CLEARETAILS:

                    break;
                #endregion
            }
        }
         #endregion
        #endregion
        #region Action Handler
        protected void ActionHandler(object sender, EventArgs e)
        {
            if (!(this.Master as ERPSMS_2).ValidatePageDept())
                return;
            try
            {
                bool bIsChecked = false;
                int? result=0;

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
                else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    if (((DropDownList)sender).ID == "ddlCategory")
                    {
                        commonActions = BusinessObject.AccountManagement.ActionsEnum.CHANGETYPE;
                    }
                }
                switch (commonActions)
                {
                    #region SAVE
                    case BusinessObject.AccountManagement.ActionsEnum.SAVE:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else
                        {
                            objEmployeePerformance = new EmployeePerformanceBO();
                            objEmployeePerformance = (EmployeePerformanceBO)SetUIValuesToObject(ControlsEnum.ADDEMPPERFORMANCEDETAILS);
                            string xmlDoc = CommonFunctions.XmlSerialize<EmployeePerformanceBO>(objEmployeePerformance);

                            result = BusinessLogic.HRMS.Employee.EmployeePerformanceBL.SaveEmployeePerformance(xmlDoc);
                            if (result > 0) // Success !  redirect to listing page
                            {
                                ModifiedDatePnl.Visible = false;
                                string savePath = string.Empty;
                                if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                                {
                                    savePath = HttpContext.Current.Request.PhysicalApplicationPath + @"Upload\HR\Misc";
                                    if (!Directory.Exists(savePath))
                                        Directory.CreateDirectory(savePath);
                                    savePath = HttpContext.Current.Request.PhysicalApplicationPath + @"Upload\HR\Misc\";
                                }
                                else
                                {
                                    savePath = string.Format(@"{0}HR/Misc/", System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower());
                                }
                                if (BLUploadList != null)
                                {
                                    foreach (EmpDocUploadBinder obj in BLUploadList)
                                    {
                                        string filePath = savePath + "/" + obj.AttachmentFileName;
                                        FileInfo attachedFileInfo = new FileInfo(filePath);
                                        if (FileDetailsList != null)
                                        {
                                            FileDetails fileDetailsObj = FileDetailsList.SingleOrDefault(aa => aa.SlNo == obj.DOC_SEQ_NO);
                                            if (fileDetailsObj != null)
                                            {
                                                fileDetailsObj.EmpDocFile.SaveAs(attachedFileInfo.FullName);
                                            }
                                        }
                                    }
                                    ResetForm(ControlsEnum.CLEAR);
                                    BLUploadList = null;
                                    FileDetailsList = null;
                                    anchorFile.Visible = false;
                                    vrfFileUpload.Enabled = true;
                                    CurrSlNo = 0;
                                    anchorFile.Attributes.Remove("onclick");
                                }
                                EntryStatus = EntryStatus.LISTMODE;
                                GetFieldValues(ControlsEnum.LIST);
                                SetFieldValues(ControlsEnum.LIST);
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeePerformance);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                            }
                            else
                            {
                                if (result == (int)DbSaveStatus.SQLERROR)
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbSaveStatus.CONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.EmployeePerformance + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbSaveStatus.CODEEXIST)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.EmployeePerformance + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeePerformance);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                        }
                        break;
                    #endregion
                    #region Save And Continue
                    case BusinessObject.AccountManagement.ActionsEnum.SAVEANDCONTINUE:

                            objEmployeePerformance = new EmployeePerformanceBO();
                            objEmployeePerformance = (EmployeePerformanceBO)SetUIValuesToObject(ControlsEnum.ADDEMPPERFORMANCEDETAILS);
                            string xmlDoc2 = CommonFunctions.XmlSerialize<EmployeePerformanceBO>(objEmployeePerformance);

                            result = BusinessLogic.HRMS.Employee.EmployeePerformanceBL.SaveEmployeePerformance(xmlDoc2);
                            if (result > 0) // Success !  redirect to listing page
                            {
                                string savePath = string.Empty;
                                if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                                {
                                    savePath = HttpContext.Current.Request.PhysicalApplicationPath + @"Upload\HR\Misc";
                                    if (!Directory.Exists(savePath))
                                        Directory.CreateDirectory(savePath);
                                    savePath = HttpContext.Current.Request.PhysicalApplicationPath + @"Upload\HR\Misc\";
                                }
                                else
                                {
                                    savePath = string.Format(@"{0}HR/Misc/", System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower());
                                }
                                if (BLUploadList != null)
                                {
                                    foreach (EmpDocUploadBinder obj in BLUploadList)
                                    {
                                        string filePath = savePath + "/" + obj.AttachmentFileName;
                                        FileInfo attachedFileInfo = new FileInfo(filePath);
                                        if (FileDetailsList != null)
                                        {
                                            FileDetails fileDetailsObj = FileDetailsList.SingleOrDefault(aa => aa.SlNo == obj.DOC_SEQ_NO);
                                            if (fileDetailsObj != null)
                                            {
                                                fileDetailsObj.EmpDocFile.SaveAs(attachedFileInfo.FullName);
                                            }
                                        }
                                    }
                                    ResetForm(ControlsEnum.CLEAR);

                                    BLUploadList = null;
                                    FileDetailsList = null;
                                    anchorFile.Visible = false;
                                    vrfFileUpload.Enabled = true;
                                    CurrSlNo = 0;
                                    anchorFile.Attributes.Remove("onclick");
                                }
                                GetFieldValues(ControlsEnum.LIST);
                                SetFieldValues(ControlsEnum.LIST);
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeePerformance);
                                CommonBL userAuth = new CommonBL();
                                string nextPageUrl = BusinessLogic.HRMS.Common.HRMSCommonBL.GetNextTabUrl(Convert.ToInt16(EmpTabEnum.BEHAVIOUR), GetGlobalResourceObject("ConfigurationsRes", "HrmsEmpTabNextURL").ToString().Split(','),
                                                       GetGlobalResourceObject("ConfigurationsRes", "HrmsEmpInactiveTabs").ToString().Split(','), Resources.PageURL.EmployeeList.ToString());
                                if (userAuth.IsUserHasRights(currentUser.PKUser, nextPageUrl.Replace("~", ""), currentUser.SBUID, currentUser.CurrentDeptPK))
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(nextPageUrl) + "');", true);
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.EmployeeList) + "');", true);
                                }
                            }
                            else
                            {
                                if (result == (int)DbSaveStatus.SQLERROR)
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbSaveStatus.CONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.EmployeePerformance + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbSaveStatus.CODEEXIST)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.EmployeePerformance + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeePerformance);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }

                       
                        break;
                    #endregion
                    #region CANCEL
                    case BusinessObject.AccountManagement.ActionsEnum.CANCEL:
                        EntryStatus = EntryStatus.LISTMODE;
                        ResetForm(ControlsEnum.CLEAR);
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region CANCELTOLIST
                    case BusinessObject.AccountManagement.ActionsEnum.CANCELTOLIST:
                        CurrPK = 0;
                        Response.Redirect(Resources.PageURL.EmployeeList, true);
                        break;
                    #endregion
                    #region NEW
                    case BusinessObject.AccountManagement.ActionsEnum.NEW:
                        ResetForm(ControlsEnum.CLEAR);
                        btnDelete.Visible = false;
                        ModifiedDatePnl.Visible = false;
                        EntryStatus = EntryStatus.NEWMODE;
                        SetFieldValues(ControlsEnum.UPLOADEDFILES);
                        break;
                    #endregion
                    #region CHANGE CATEGORY
                    case BusinessObject.AccountManagement.ActionsEnum.CHANGETYPE:
                        if (ddlCategory.SelectedValue == CommonConstants.SELECTVAL)
                        {
                            ddlAction.Enabled = false;
                            ddlAction.SelectedValue = CommonConstants.SELECTVAL;
                        }
                        else
                        {
                            ddlAction.Enabled = true;
                            GetFieldValues(ControlsEnum.Action);
                            SetFieldValues(ControlsEnum.Action);
                        }
                        break;
                    #endregion
                    #region EDIT
                    case BusinessObject.AccountManagement.ActionsEnum.EDIT:
                        foreach (GridViewRow grdrow in grdEmployeePerformancelist.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                SelectedPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfPerPk")).Value);
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            EntryStatus = EntryStatus.EDITMODE;
                            GetFieldValues(ControlsEnum.EMPLOYEEPERFORMANCEBYPK);
                            SetFieldValues(ControlsEnum.EMPLOYEEPERFORMANCEBYPK);
                            if (this.objEmployeePerformance != null)
                            {
                                this.BLUploadList = this.objEmployeePerformance.DocDetails;
                                BindGrid(ControlsEnum.UPLOADEDFILES);
                            }
                        }
                        else
                        {

                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region ADDITEM
                    case BusinessObject.AccountManagement.ActionsEnum.ADDITEM:
                        if (CurrSlNo != 0)
                        {
                            if (fupUpload.HasFile || !string.IsNullOrEmpty(anchorFile.HRef))
                            {
                                blUploadObj = BLUploadList.SingleOrDefault(itm => itm.DOC_SEQ_NO == CurrSlNo);
                                if (blUploadObj != null)
                                {
                                    if (FileDetailsList == null)
                                    {
                                        FileDetailsList = new List<FileDetails>();
                                    }
                                    if (fupUpload.HasFile)
                                    {
                                        FileInfo tempFileInfoObj;
                                        tempFileInfoObj = new FileInfo(fupUpload.PostedFile.FileName);
                                        string attachmentFileFormat = tempFileInfoObj.Extension;
                                        string attachmentFileName = Guid.NewGuid().ToString() + attachmentFileFormat;

                                        blUploadObj.DOC_TYPE = tempFileInfoObj.Extension;
                                        blUploadObj.DOC_NAME = fupUpload.FileName;
                                        blUploadObj.DOC_TITLE = attachmentFileName;
                                        blUploadObj.AttachmentFileName = attachmentFileName;

                                        if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower()))
                                        {
                                            blUploadObj.DOC_PATH = "~/Upload/HR/Misc/" + attachmentFileName;
                                        }
                                        else
                                        {
                                            string savePath = string.Format(@"{0}HR/Misc/", System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower());
                                            blUploadObj.DOC_PATH = savePath + attachmentFileName;
                                        }
                                        FileDetails fileDetailsObj = FileDetailsList.SingleOrDefault(aa => aa.SlNo == CurrSlNo);
                                        if (fileDetailsObj == null)
                                        {
                                            FileDetailsList.Add(new FileDetails() { SlNo = CurrSlNo, EmpDocFile = HttpContext.Current.Request.Files[0] });
                                        }
                                        else
                                        {
                                            fileDetailsObj.EmpDocFile = HttpContext.Current.Request.Files[0];
                                        }
                                    }
                                }
                            }

                        }
                        else
                        {
                            if (fupUpload.HasFile)
                            {

                                int slno = 1;
                                if (BLUploadList == null || BLUploadList.Count == 0)
                                {
                                    BLUploadList = new List<EmpDocUploadBinder>();
                                    slno = 1;
                                }
                                else
                                {
                                    slno = BLUploadList.Max(itm => itm.DOC_SEQ_NO);
                                    slno++;
                                }
                                if (FileDetailsList == null)
                                {
                                    FileDetailsList = new List<FileDetails>();
                                }

                                blUploadObj = new EmpDocUploadBinder();
                                blUploadObj.DOC_PK = 0;
                                blUploadObj.DOC_SEQ_NO = slno;
                                FileInfo tempFileInfoObj;
                                tempFileInfoObj = new FileInfo(fupUpload.PostedFile.FileName);
                                string attachmentFileFormat = tempFileInfoObj.Extension;
                                string attachmentFileName = Guid.NewGuid().ToString() + attachmentFileFormat;

                                blUploadObj.DOC_TYPE = tempFileInfoObj.Extension;
                                blUploadObj.DOC_NAME = fupUpload.FileName;
                                blUploadObj.DOC_TITLE = attachmentFileName;
                                blUploadObj.AttachmentFileName = attachmentFileName;

                                if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower()))
                                {
                                    blUploadObj.DOC_PATH = "~/Upload/HR/Misc/" + attachmentFileName;
                                }
                                else
                                {
                                    string savePath = string.Format(@"{0}HR/Misc/", System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower());
                                    blUploadObj.DOC_PATH = savePath + attachmentFileName;
                                }
                                blUploadObj.DOC_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                                FileDetailsList.Add(new FileDetails() { SlNo = slno, EmpDocFile = HttpContext.Current.Request.Files[0] });
                                BLUploadList.Add(blUploadObj);
                            }
                        }
                        SetFieldValues(ControlsEnum.UPLOADEDFILES);
                        fupUpload.Focus();

                        break;
                    #endregion
                    #region DELETE
                    case BusinessObject.AccountManagement.ActionsEnum.DELETE:
                        foreach (GridViewRow grdrow in grdEmployeePerformancelist.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                SelectedPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfPerPk")).Value);
                                break;
                            }

                        }
                        if (bIsChecked)
                        {

                            result = BusinessLogic.HRMS.Employee.EmployeePerformanceBL.DeleteEmployeePerformanceById(SelectedPK, Convert.ToString(this.LastModifiedTime));
                            DbDeleteStatus deleteStatus = (DbDeleteStatus)result;
                            switch (deleteStatus)
                            {
                                case DbDeleteStatus.DELETED://If deletion is success
                                    if (grdEmployeePerformancelist.Rows.Count == 1 && Convert.ToInt32(PageIndex) > 1)
                                    {
                                        PageIndex = Convert.ToInt32(PageIndex) - 1;
                                    }
                                    EntryStatus = EntryStatus.LISTMODE;
                                    ResetForm(ControlsEnum.CLEARETAILS);
                                    GetFieldValues(ControlsEnum.LIST);
                                    SetFieldValues(ControlsEnum.LIST);
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeePerformance);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                    break;
                                case DbDeleteStatus.REFERRED://If referred to another page
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Ref;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeePerformance);
                                    break;
                                case DbDeleteStatus.SQLERROR://Sql error
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Sql_Error;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeePerformance);
                                    break;
                                case DbDeleteStatus.DELETECONCURRENCY:
                                    litErrorMsg.Text = Resources.PageNameRes.EmployeePerformance + " " +
                                    GetGlobalResourceObject("Messages", "EditUsedByAnotherUser").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                    break;
                            }
                        }
                        else
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }

                        break;
                    #endregion
                    #region REMOVEITEM
                    case BusinessObject.AccountManagement.ActionsEnum.REMOVEITEM:
                        if (BLUploadList != null && BLUploadList.Count > 0)
                        {
                            int SlNo = Convert.ToInt32(grdUploads.DataKeys[(((Button)sender).Parent.Parent as GridViewRow).RowIndex][0]);
                            if (SlNo > 0)
                            {
                                BLUploadList = BLUploadList.Where(row => SlNo != row.DOC_SEQ_NO).ToList();
                                FileDetailsList = FileDetailsList.Where(row => SlNo != row.SlNo).ToList();
                                SetFieldValues(ControlsEnum.UPLOADEDFILES);
                                anchorFile.Visible = false;
                                vrfFileUpload.Enabled = true;
                                CurrSlNo = 0;
                                anchorFile.Attributes.Remove("onclick");
                            }
                        }
                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            int slno;
            try
            {
                #region grdUploads
                if (((GridView)sender).ID == "grdUploads")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        slno = Convert.ToInt32(grdUploads.DataKeys[e.Row.RowIndex][0]);
                        if (slno > 0)
                        {
                            e.Row.FindControl("fileView").Visible = FileDetailsList == null || FileDetailsList.Where(fle => fle.SlNo == slno).Count() == 0;
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.ErpRes.Information + "');", true);
            }
            finally
            {
            }
        }
        protected void ActionHandler(object sender, DataNavigatorEventArgs e)
        {
            try
            {
                PgerControlNew pagerControl = (PgerControlNew)sender;
                string senderId = pagerControl.ID;
                switch (e.Action)
                {
                    case NavigationEnum.PAGECHANGE:
                        pagerControl.CurrentPage = e.CurrentPage;
                        break;
                    case NavigationEnum.FIRST:
                        if (e.CurrentPage > 1)
                            pagerControl.CurrentPage = 1;
                        break;
                    case NavigationEnum.LAST:
                        if (e.CurrentPage <= e.TotalPages)
                            pagerControl.CurrentPage = e.TotalPages;
                        break;
                    case NavigationEnum.NEXT:
                        if (e.CurrentPage <= e.TotalPages)
                            pagerControl.CurrentPage++;
                        break;
                    case NavigationEnum.PREVIOUS:
                        if (e.CurrentPage > 1)
                            pagerControl.CurrentPage--;
                        break;
                }
                if (senderId == "uclPaging")
                {
                    PageIndex = uclPaging.CurrentPage;
                    GetFieldValues(ControlsEnum.LIST);
                    SetFieldValues(ControlsEnum.LIST);
                    EntryStatus = EntryStatus.LISTMODE;
                    EnableDisableButtons(e.TotalPages, "uclPaging");
                }
            }
            catch (Exception ex)
            {
                // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.gComsRes.Information + "');", true);
            }
        }
        protected void ActionHandler(object sender, GridViewPageEventArgs e)
        {
            PageIndex = e.NewPageIndex;
        }
        #endregion
        #region Page Events
        /// <summary>
        /// To Handle Page Load Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            string[] itemkeyarray;
            itemkeyarray = new string[1];
            itemkeyarray[0] = "DOC_SEQ_NO";
            grdUploads.DataKeyNames = itemkeyarray;

            if (!IsPostBack)
            {
                PageActionHandler();
            }
        }

        /// <summary>
        /// To Handle Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
                if (EntryStatus == EntryStatus.EDITMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing(0);", true);
                    lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                }
                else if (EntryStatus == EntryStatus.NEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing(0);", true);
                    lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                }
                else if (EntryStatus == EntryStatus.ENTRYMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing(0);", true);
                    lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                }
                else if (EntryStatus == EntryStatus.LISTMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing(1);", true);
                    lnkList.CssClass = GetLocalResourceObject("TabActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabInActive").ToString();
                }
                if (EntryStatusHdr == EntryStatus.VIEWMODE)
                {
                    hdfEntryStatus.Value = "1";
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(1);});", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        /// <summary>
        /// Methord  For PageInite Used to set the Pager current Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, System.EventArgs e)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));    
            uclPaging.CurrentPage = 1;

   
        }

        protected void btnAction_Load(object sender, EventArgs e)
        {
            (sender as Control).Visible = true;
        }
        /// <summary>
        /// To handle the visibility of the action corresponding to the users Privilage        
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAction_PreRender(object sender, EventArgs e)
        {
            //base.CheckBtnVisibility(sender);
        }

        /// <summary>
        /// To handle OnInit event Used to assign the Event for all the actions used in this page
        /// Leave this section if using Master Screens
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeComponent();
        }


        #endregion
        #region InitializeComponent
        /// <summary>
        /// Method Used to initialize the Pager Control
        /// </summary>
        private void InitializeComponent()
        {
            this.uclPaging.FirstPage += new ActionHandler(this.ActionHandler);
            this.uclPaging.PreviousPage += new ActionHandler(this.ActionHandler);
            this.uclPaging.NextPage += new ActionHandler(this.ActionHandler);
            this.uclPaging.LastPage += new ActionHandler(this.ActionHandler);
            this.uclPaging.PageChanged += new ActionHandler(this.ActionHandler);
            this.Init += new EventHandler(this.Page_Init);
        }
        #endregion
        #region EnableDisableButtons
        /// <summary>
        /// Methord used to enable and Disable Page Navigation Controls
        /// </summary>
        /// <param name="iTotalPages"></param>
        private void EnableDisableButtons(int iTotalPages, string pagerId)
        {
            if (pagerId == "uclPaging")
            {
                uclPaging.FirstButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;// Should we disable the first link
                uclPaging.PreviousButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;// Should we disable the previous link
                uclPaging.NextButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false; // Should we enable the next link
                uclPaging.LastButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;// Should we enable the last link
            }
        }
        #endregion
        #region ControlEnum
        public enum ControlsEnum
        {
            EMPLOYEEDETAILSBYID,
            LIST,
            EMPLOYEEPERFORMANCEBYPK,
            ADDEMPPERFORMANCEDETAILS,
            CLEAR,
            CLEARETAILS,
            UPLOADEDFILES,
            Category,
            Action


        }

        #endregion
    }
}