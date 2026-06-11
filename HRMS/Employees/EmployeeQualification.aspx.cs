using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.HRMS.Employee;
using ERP.Utilities;

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
using BusinessObject.Common;
using BusinessLogic.CommonManagement;
using ERPSMS_v01;

namespace HRMS.Employees
{
    public partial class EmployeeQualification : ERP.Store.UI.MyBasePage
    {

        #region Variables and Properties

        #region  Properties

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
                return Session[ERP.Utilities.SessionStrings.FileBLDetailsList] == null
                            ? null
                            : (List<FileDetails>)Session[ERP.Utilities.SessionStrings.FileBLDetailsList];
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
                return this.Session[ViewstateStrings.EntryState] == null ? EntryStatus.LISTMODE : (EntryStatus)(this.Session[ViewstateStrings.EntryState]);
            }
            set
            {
                this.Session[ViewstateStrings.EntryState] = value;
            }
        }
        #endregion
        #region  Variables
        private int commonPK = 0;
        private int CountryPk;
        private DataTable dtemployeeHeader;
        private DataTable dtQualificationType;
        private DataTable dtQualificationList;
        private DataTable dtStatus;
        private BusinessObject.AccountManagement.ActionsEnum commonActions;
        private EmployeeQualifcn objEmployeeQualifcn;
        private EmployeeBasicInfomtn objEmployeeBasicInfo;
        EmpDocUploadBinder blUploadObj;
        private List<ADM_COMPANY_MST> admCompanyMstList;
        private ADM_COMPANY_MST admCompanyMstObj;
        User currentUser;

        #endregion
        #endregion

        #region Page level Events
        protected void Page_Load(object sender, EventArgs e)
        {


            string[] itemkeyarray;
            itemkeyarray = new string[1];
            itemkeyarray[0] = "DOC_SEQ_NO";
            grdUploads.DataKeyNames = itemkeyarray;
            ButtonClicked();

            ddlQualificationType.Focus();
            if (!IsPostBack)
            {
                chkActive.Checked = true;
                GetFieldValues(ControlsEnum.QUALIFICATIONTYPE);
                SetFieldValues(ControlsEnum.QUALIFICATIONTYPE);
                GetFieldValues(ControlsEnum.STATUSLIST);
                SetFieldValues(ControlsEnum.STATUSLIST);
                //if (Request.QueryString["EmpPK"] != null)
                if (this.CurrPK != 0)
                {
                    //CurrPK = Convert.ToInt32(Request.QueryString["EmpPK"]);
                    GetFieldValues(ControlsEnum.EMPLOYEEDETAILSBYID);
                    SetFieldValues(ControlsEnum.EMPLOYEEDETAILSBYID);
                    GetFieldValues(ControlsEnum.EMPLOYEEDETAILSHEADER);
                    SetFieldValues(ControlsEnum.EMPLOYEEDETAILSHEADER);
                    GetFieldValues(ControlsEnum.EMPLOYEEQUALIFICATIONLIST);
                    SetFieldValues(ControlsEnum.EMPLOYEEQUALIFICATIONLIST);
                    ShowListingTabRegScript(1);
                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('1');", true);
                }
                if (this.CurrPK == 0)
                {
                    litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                    Session["SelectMessage"] = litErrorMsg.Text;
                    //Response.Redirect("~/Employees/EmployeeList.aspx");
                    Response.Redirect(Resources.PageURL.EmployeeList);


                }


            }

        }

        protected void Page_PreRender(Object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "InitComponents();", true);
                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitDate1", "InitDate();", true);
                if (EntryStatus == EntryStatus.VIEWMODE)
                {
                    hdfEntryStatus.Value = "1";
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(1);});", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
                ShowListingTabRegScript(1);
                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('1');", true);
            }
        }

        protected void btnAction_PreRender(object sender, EventArgs e)
        {           
            if (sender.GetType().IsEquivalentTo(typeof(Button)))
            {
                commonActions = (BusinessObject.AccountManagement.ActionsEnum)(Enum.Parse(typeof(BusinessObject.AccountManagement.ActionsEnum), ((Button)sender).CommandName));
            }
            else if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
            {
                commonActions = (BusinessObject.AccountManagement.ActionsEnum)(Enum.Parse(typeof(BusinessObject.AccountManagement.ActionsEnum), ((LinkButton)sender).CommandName));
            }
        }
        #endregion

        #region Action Handler
        protected void ActionHandler(object sender, EventArgs e)
        {
            if (!(this.Master as ERPSMS_2).ValidatePageDept())
                return;

            bool bIsChecked = false;

            int? result;

            try
            {

                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (BusinessObject.AccountManagement.ActionsEnum)(Enum.Parse(typeof(BusinessObject.AccountManagement.ActionsEnum), ((Button)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                {
                    commonActions = (BusinessObject.AccountManagement.ActionsEnum)(Enum.Parse(typeof(BusinessObject.AccountManagement.ActionsEnum), ((LinkButton)sender).CommandName));
                    Session["QualDetail"] = 1;
                }
                else if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    commonActions = (BusinessObject.AccountManagement.ActionsEnum)(Enum.Parse(typeof(BusinessObject.AccountManagement.ActionsEnum), ((ImageButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {


                }
                switch (commonActions)
                {

                    #region Save
                    case BusinessObject.AccountManagement.ActionsEnum.SAVE:
                        int cmpPeriodFromQualifiedOn = 0;
                        int cmpDOBperiodFrom = 0;
                        DateTime dateQualifiedon = Convert.ToDateTime(string.IsNullOrEmpty(txtQualifiedOn.Text.Trim()) ? DateTime.Now.ToString(Resources.Constants.HRMSDateFormatShort) : txtQualifiedOn.Text.Trim()); ;
                        if (hdfEmployeeQDOB.Value != "")
                        {
                            DateTime dateofBirth = Convert.ToDateTime(hdfEmployeeQDOB.Value);
                            DateTime datePeriodFrom = Convert.ToDateTime(string.IsNullOrEmpty(txtPeriodfrom.Text.Trim()) ? DateTime.Now.ToString(Resources.Constants.HRMSDateFormatShort) : txtPeriodfrom.Text.Trim()); ;
                            DateTime datePeriodto = Convert.ToDateTime(string.IsNullOrEmpty(txtTo.Text.Trim()) ? DateTime.Now.ToString(Resources.Constants.HRMSDateFormatShort) : txtTo.Text.Trim()); ;
                            cmpPeriodFromQualifiedOn = dateQualifiedon.CompareTo(datePeriodto);
                            cmpDOBperiodFrom = datePeriodFrom.CompareTo(dateofBirth);
                        }
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                            ShowListingTabRegScript(0);
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('0');", true);
                        }
                        else if (cmpPeriodFromQualifiedOn < 0)
                        {
                            litErrorMsg.Text = " Qualified on must greater than Period to";
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                       + "','" + Resources.ErpRes.Information + "');", true);
                            ShowListingTabRegScript(0);
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('0');", true);
                        }
                        else if (cmpDOBperiodFrom < 0)
                        {
                            litErrorMsg.Text = " Period from must greater than DOB";
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                       + "','" + Resources.ErpRes.Information + "');", true);
                            ShowListingTabRegScript(0);
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('0');", true);
                        }
                        else
                        {
                            objEmployeeQualifcn = SetUIValuesToObject();
                            string xmlDoc = CommonFunctions.XmlSerialize<EmployeeQualifcn>(objEmployeeQualifcn);
                            result = BusinessLogic.HRMS.Employee.EmployeeQualificationBL.SaveEmployeeQualification(xmlDoc);
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
                                    //savePath = string.Format(@"{0}\HR\Misc\", System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower());
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
                                    ResetForm();
                                    BLUploadList = null;
                                    FileDetailsList = null;
                                    anchorFile.Visible = false;
                                    vrfFileUpload.Enabled = true;
                                    CurrSlNo = 0;
                                    anchorFile.Attributes.Remove("onclick");
                                    //GetFieldValues(ControlsEnum.EMPLOYEEQUALIFICATIONLIST);
                                    //SetFieldValues(ControlsEnum.EMPLOYEEQUALIFICATIONLIST);
                                    //// Show Save Message and redired to listing page                                        
                                    //litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                    //litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeQualification);
                                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                    //ShowListingTabRegScript(1);
                                    ////ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('1');", true);
                                }
                                GetFieldValues(ControlsEnum.EMPLOYEEQUALIFICATIONLIST);
                                SetFieldValues(ControlsEnum.EMPLOYEEQUALIFICATIONLIST);
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeQualification);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                ShowListingTabRegScript(1);
                                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('1');", true);
                            }
                            else
                            {
                                if (result == (int)DbSaveStatus.SQLERROR)
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    ShowListingTabRegScript(1);
                                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('1');", true);
                                }
                                else if (result == (int)DbSaveStatus.CONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.EmployeeQualification + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                    ShowListingTabRegScript(1);
                                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('1');", true);
                                }
                                else if (result == (int)DbSaveStatus.CODEEXIST)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.EmployeeQualification + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                    ShowListingTabRegScript(1);
                                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('1');", true);
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeQualification);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    ShowListingTabRegScript(1);
                                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('1');", true);
                                }
                            }
                        }
                        break;
                    #endregion
                    #region Save And Continue
                    case BusinessObject.AccountManagement.ActionsEnum.SAVEANDCONTINUE:
                        int cmpPeriodFromQualifiedOn2 = 0;
                        int cmpDOBperiodFrom2 = 0;
                        if (hdfEmployeeQDOB.Value != "")
                        {
                            DateTime dateQualifiedon2 = Convert.ToDateTime(string.IsNullOrEmpty(txtQualifiedOn.Text.Trim()) ? DateTime.Now.ToString(Resources.Constants.HRMSDateFormatShort) : txtQualifiedOn.Text.Trim()); ;
                            DateTime dateofBirth2 = Convert.ToDateTime(hdfEmployeeQDOB.Value);
                            DateTime datePeriodFrom2 = Convert.ToDateTime(string.IsNullOrEmpty(txtPeriodfrom.Text.Trim()) ? DateTime.Now.ToString(Resources.Constants.HRMSDateFormatShort) : txtPeriodfrom.Text.Trim()); ;
                            DateTime datePeriodto2 = Convert.ToDateTime(string.IsNullOrEmpty(txtTo.Text.Trim()) ? DateTime.Now.ToString(Resources.Constants.HRMSDateFormatShort) : txtTo.Text.Trim()); ;
                            cmpPeriodFromQualifiedOn2 = dateQualifiedon2.CompareTo(datePeriodto2);
                            cmpDOBperiodFrom2 = datePeriodFrom2.CompareTo(dateofBirth2);
                        }
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                            ShowListingTabRegScript(0);
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('0');", true);
                        }
                        else if (cmpPeriodFromQualifiedOn2 < 0)
                        {
                            litErrorMsg.Text = " Qualified On must greater than Period To";
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                       + "','" + Resources.ErpRes.Information + "');", true);
                            ShowListingTabRegScript(0);
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('0');", true);
                        }
                        else if (cmpDOBperiodFrom2 < 0)
                        {
                            litErrorMsg.Text = " Period from must greater than DOB";
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                       + "','" + Resources.ErpRes.Information + "');", true);
                            ShowListingTabRegScript(0);
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('0');", true);
                        }
                        else
                        {
                            objEmployeeQualifcn = SetUIValuesToObject();
                            string xmlDoc = CommonFunctions.XmlSerialize<EmployeeQualifcn>(objEmployeeQualifcn);
                            result = BusinessLogic.HRMS.Employee.EmployeeQualificationBL.SaveEmployeeQualification(xmlDoc);
                            if (result > 0) // Success !  redirect to listing page
                            {
                                if (!string.IsNullOrEmpty(Request.Form[btnSaveContinue.UniqueID]))
                                {
                                    Application["QualSaveandContinue"] = 1;
                                }

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
                                    //savePath = string.Format(@"{0}\HR\Misc\", System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower());
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
                                    ResetForm();

                                    BLUploadList = null;
                                    FileDetailsList = null;
                                    anchorFile.Visible = false;
                                    vrfFileUpload.Enabled = true;
                                    CurrSlNo = 0;
                                    anchorFile.Attributes.Remove("onclick");
                                    //GetFieldValues(ControlsEnum.EMPLOYEEQUALIFICATIONLIST);
                                    //SetFieldValues(ControlsEnum.EMPLOYEEQUALIFICATIONLIST);

                                    //// Show Save Message and redired to listing page                                        
                                    //litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                    //litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeQualification);
                                    ////      ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    ////+ "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.HrmsExperience) + "');", true);
                                    //Response.Redirect(Resources.PageURL.HrmsExperience);
                                }
                                GetFieldValues(ControlsEnum.EMPLOYEEQUALIFICATIONLIST);
                                SetFieldValues(ControlsEnum.EMPLOYEEQUALIFICATIONLIST);
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeQualification);
                                CommonBL userAuth = new CommonBL();


                                string nextPageUrl = BusinessLogic.HRMS.Common.HRMSCommonBL.GetNextTabUrl(Convert.ToInt16(EmpTabEnum.Qualifications), GetGlobalResourceObject("ConfigurationsRes", "HrmsEmpTabNextURL").ToString().Split(','),
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
                                    ShowListingTabRegScript(1);
                                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('1');", true);
                                }
                                else if (result == (int)DbSaveStatus.CONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.EmployeeQualification + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                    ShowListingTabRegScript(1);
                                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('1');", true);
                                }
                                else if (result == (int)DbSaveStatus.CODEEXIST)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.EmployeeQualification + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                    ShowListingTabRegScript(1);
                                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('1');", true);
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeQualification);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    ShowListingTabRegScript(1);
                                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('1');", true);
                                }
                            }
                            
                        }
                        break;
                    #endregion
                    #region CANCEL

                    case BusinessObject.AccountManagement.ActionsEnum.CANCEL:
                        ResetForm();
                        ShowListingTabRegScript(1);
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('1');", true);
                        break;

                    #endregion
                    #region CANCELTOLIST
                    case BusinessObject.AccountManagement.ActionsEnum.CANCELTOLIST:
                        CurrPK = 0;
                        if (!string.IsNullOrEmpty(Request.Form[btnCancelList.UniqueID]))
                        {
                            Session["CancelQualificationClicked"] = 1;
                        }
                        Response.Redirect(Resources.PageURL.EmployeeList, true);
                        break;

                    #endregion

                    #region NEW

                    case BusinessObject.AccountManagement.ActionsEnum.NEW:
                        ResetForm();
                        btnDelete.Visible = false;
                        ModifiedDatePnl.Visible = false;
                        ShowListingTabRegScript(0);
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('0');", true);
                        break;

                    #endregion
                    #region EDIT

                    case BusinessObject.AccountManagement.ActionsEnum.EDIT:
                        foreach (GridViewRow grdrow in grdEmployeeQualificationlist.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {

                                bIsChecked = true;
                                SelectedPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfQualfPk")).Value);
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            if (!string.IsNullOrEmpty(Request.Form[btnEdit.UniqueID]))
                            {
                                Session["EditQualificationClicked"] = 1;
                            }
                            btnDelete.Visible = true;
                            ShowListingTabRegScript(0);
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('0');", true);
                            GetFieldValues(ControlsEnum.EMPLOYEEQUALIFICATIONBYID);
                            SetFieldValues(ControlsEnum.EMPLOYEEQUALIFICATIONBYID);
                            if (this.objEmployeeQualifcn != null)
                            {
                                this.BLUploadList = this.objEmployeeQualifcn.DocDetails;
                                BindGridImage(controlType);
                            }


                        }
                        else
                        {

                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            ShowListingTabRegScript(1);
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('1');", true);
                        }
                        break;
                    #endregion
                    #region DELETE
                    case BusinessObject.AccountManagement.ActionsEnum.DELETE:
                        Session["QualDelete"] = 1;
                        foreach (GridViewRow grdrow in grdEmployeeQualificationlist.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                SelectedPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfQualfPk")).Value);
                                break;
                            }

                        }
                        if (bIsChecked)
                        {

                            result = BusinessLogic.HRMS.Employee.EmployeeQualificationBL.DeleteQualificationbyQualId(SelectedPK);
                            DbDeleteStatus deleteStatus = (DbDeleteStatus)result;
                            switch (deleteStatus)
                            {
                                case DbDeleteStatus.DELETED://If deletion is success
                                    ResetForm();
                                    GetFieldValues(ControlsEnum.EMPLOYEEQUALIFICATIONLIST);
                                    SetFieldValues(ControlsEnum.EMPLOYEEQUALIFICATIONLIST);
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeQualification);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                    ShowListingTabRegScript(1);
                                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('1');", true);
                                    break;
                                case DbDeleteStatus.REFERRED://If referred to another page
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Ref;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeQualification);
                                    ShowListingTabRegScript(1);
                                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('1');", true);

                                    break;
                                case DbDeleteStatus.SQLERROR://Sql error
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Sql_Error;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeQualification);
                                    ShowListingTabRegScript(1);
                                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('1');", true);
                                    break;
                            }
                        }
                        else
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            ShowListingTabRegScript(1);
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('1');", true);
                        }

                        break;
                    #endregion
                    #region ADDITEM
                    case BusinessObject.AccountManagement.ActionsEnum.ADDITEM:
                        if (!string.IsNullOrEmpty(Request.Form[btnAddItem.UniqueID]))
                        {
                            Session["AddgriditemClickedQualification"] = 1;
                        }
                        //validate Page
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_SavError;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                            ShowListingTabRegScript(0);
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('0');", true);
                        }
                        else//valid
                        {

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
                                        //SavePath = Server.MapPath("../Upload");
                                        //blUploadObj.SCD_FILE_PATH = "~/Upload/" + attachmentFileName;
                                        string savePath = string.Format(@"{0}HR/Misc/", System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower());
                                        blUploadObj.DOC_PATH = savePath + attachmentFileName;
                                    }

                                    // blUploadObj.SCD_FILE_PATH = "~/Upload/" + attachmentFileName;
                                    blUploadObj.DOC_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                                    FileDetailsList.Add(new FileDetails() { SlNo = slno, EmpDocFile = HttpContext.Current.Request.Files[0] });
                                    BLUploadList.Add(blUploadObj);
                                }
                            }
                            SetFieldValues(ControlsEnum.UPLOADEDFILES);
                            ShowListingTabRegScript(0);
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('0');", true);
                            fupUpload.Focus();
                            //EntryStatus = EntryStatus.NEWMODE;
                            // ResetForm(ActionsEnum.ADDITEM);
                        }
                        break;
                    #endregion
                    #region REMOVEITEM

                    case BusinessObject.AccountManagement.ActionsEnum.REMOVEITEM:
                        Session["QualRemove"] = 1;
                        if (BLUploadList != null && BLUploadList.Count > 0)
                        {
                            int SlNo = Convert.ToInt32(grdUploads.DataKeys[(((Button)sender).Parent.Parent as GridViewRow).RowIndex][0]);
                            if (SlNo > 0)
                            {
                                BLUploadList = BLUploadList.Where(row => SlNo != row.DOC_SEQ_NO).ToList();
                                FileDetailsList = null;// Sequence Contains more than Error
                                SetFieldValues(ControlsEnum.UPLOADEDFILES);
                                anchorFile.Visible = false;
                                vrfFileUpload.Enabled = true;
                                CurrSlNo = 0;
                                anchorFile.Attributes.Remove("onclick");
                            }
                        }
                        break;
                    #endregion
                    #region EMPDOCDETAIL

                    case BusinessObject.AccountManagement.ActionsEnum.EMPDOCDETAIL:
                        foreach (GridViewRow grdrow in grdEmployeeQualificationlist.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                SelectedPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfQualfPk")).Value);
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            ShowListingTabRegScript(0);
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('0');", true);
                            GetFieldValues(ControlsEnum.EMPLOYEEQUALIFICATIONBYID);
                            SetFieldValues(ControlsEnum.EMPLOYEEQUALIFICATIONBYID);


                            if (this.objEmployeeQualifcn != null)
                            {
                                this.BLUploadList = this.objEmployeeQualifcn.DocDetails;
                                BindGridImage(controlType);
                            }
                        }
                        else
                        {

                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            ShowListingTabRegScript(1);
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('1');", true);
                        }
                        break;
                    #endregion

                    #region EMPQUALLIST

                    case BusinessObject.AccountManagement.ActionsEnum.EMPDOCLIST:
                        ShowListingTabRegScript(1);
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('1');", true);
                        FileDetailsList = null;
                        break;
                    #endregion

                }


            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
                ShowListingTabRegScript(0);
                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('0');", true);
            }
        }

        /// <summary>
        /// Register script for Show Listing Function
        /// </summary>
        /// <param name="p"></param>
        private void ShowListingTabRegScript(int intShow)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('" + intShow + "');", true);
        }
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            int slno;
            try
            {
                #region grdUploads
                if (((GridView)sender).ID == "grdUploads")
                {


                    //if (Session["AddgriditemClickedQualification"] != null)
                    //{

                    //if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
                    //{
                    //    //e.Row.Cells[4].Visible = false;
                    //    e.Row.Cells[2].Visible = false;
                    //    e.Row.Cells[3].Visible = false;
                    //    //e.Row.Cells[4].Visible = false;
                    //    //Session["AddgriditemClickedQualification"] = null;
                    //}
                    //}

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
                //Process Exception and show error message
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.ErpRes.Information + "');", true);
            }
            finally
            {
                //reset all objects
            }
        }

        protected void ActionHandler(object sender, GridViewSortEventArgs e)
        {
            try
            {
                GetFieldValues(ControlsEnum.EMPLOYEEQUALIFICATIONLIST);
                SetFieldValues(ControlsEnum.EMPLOYEEQUALIFICATIONLIST);
                DataTable dt = new DataTable();
                grdEmployeeQualificationlist.DataSource = dtQualificationList;
                {
                    string SortDir = string.Empty;
                    if (dir == SortDirection.Ascending)
                    {
                        dir = SortDirection.Descending;
                        SortDir = "Desc";
                    }
                    else
                    {
                        dir = SortDirection.Ascending;
                        SortDir = "Asc";
                    }
                    DataView sortedView = new DataView(dtQualificationList);
                    sortedView.Sort = e.SortExpression + " " + SortDir;
                    grdEmployeeQualificationlist.DataSource = sortedView;
                    grdEmployeeQualificationlist.DataBind();
                    ShowListingTabRegScript(1);
                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('1');", true);
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        #endregion

        #region Get Field Values
        private void GetFieldValues(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.EMPLOYEEDETAILSBYID:
                        objEmployeeBasicInfo = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.GetEmployeebyID(CurrPK);
                        break;

                    case ControlsEnum.EMPLOYEEDETAILSHEADER:
                        dtemployeeHeader = DataAccess.HRMS.Employee.EmployeeBasicInfoDL.GetEmployeeDetailListHeader(CurrPK, string.Empty);
                        break;

                    case ControlsEnum.QUALIFICATIONTYPE:
                        dtQualificationType = DataAccess.HRMS.Employee.EmployeeQualificationDL.GetQualificationType(commonPK);
                        break;

                    case ControlsEnum.STATUSLIST:
                        dtStatus = DataAccess.HRMS.Employee.EmployeeQualificationDL.GetQualificationStatus(GTIService.Constants.HRMS.Employee.Constatnts.EMP_STATUS_TYPE);
                        break;

                    case ControlsEnum.EMPLOYEEQUALIFICATIONLIST:
                        dtQualificationList = new DataTable();
                        dtQualificationList = BusinessLogic.HRMS.Employee.EmployeeQualificationBL.GetEmployeeQualificationList(CurrPK);
                        break;

                    case ControlsEnum.EMPLOYEEQUALIFICATIONBYID:
                        objEmployeeQualifcn = BusinessLogic.HRMS.Employee.EmployeeQualificationBL.GetEmployeequalificationbyID(SelectedPK);
                        break;
                }

            }
            catch
            {
            }
        }

        #endregion

        #region Set Field Values
        private void SetFieldValues(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {

                    case ControlsEnum.EMPLOYEEDETAILSHEADER:
                        GetUIValuesFromObjectHeader(controlType);
                        break;
                    case ControlsEnum.EMPLOYEEDETAILSBYID:
                        GetUIValuesFromObjectEmployee(controlType);
                        break;
                    case ControlsEnum.QUALIFICATIONTYPE:
                        BindDropDown(ControlsEnum.QUALIFICATIONTYPE);
                        break;
                    case ControlsEnum.STATUSLIST:
                        BindDropDown(ControlsEnum.STATUSLIST);
                        break;
                    case ControlsEnum.EMPLOYEEQUALIFICATIONLIST:
                        if (dtQualificationList.Rows.Count > 0)
                        {
                            BindGrid(controlType);
                        }
                        else
                        {
                            ShowListingTabRegScript(0);
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('0');", true);
                            btnDelete.Visible = false;
                           // chkHighestQualf.Checked = true;
                        }
                        break;
                    case ControlsEnum.EMPLOYEEQUALIFICATIONBYID:
                        GetUIValuesFromObject(controlType);
                        break;
                    case ControlsEnum.UPLOADEDFILES:
                        if (objEmployeeQualifcn != null)
                            GetUIValuesFromObject(ControlsEnum.UPLOADEDFILES);
                        BindGridImage(controlType);
                        ShowListingTabRegScript(0);
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('0');", true);
                        break;

                    default:
                        break;
                }
            }
            catch
            {
            }
        }
        #endregion

        #region Helper Methods
        private EmployeeQualifcn SetUIValuesToObject()
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

            objEmployeeQualifcn = new EmployeeQualifcn();
            objEmployeeQualifcn.EQD_EMPLOYEE = CurrPK;
            objEmployeeQualifcn.EQD_PK = SelectedPK;
            objEmployeeQualifcn.EQD_QUALIFICATION_TYPE = Convert.ToInt16(ddlQualificationType.SelectedValue);
            objEmployeeQualifcn.EQD_QUAL_STATUS = Convert.ToInt16(ddlStatus.SelectedValue);
            if (chkHighestQualf.Checked == true)
            {
                objEmployeeQualifcn.EQD_HIGHEST_QUAL = Convert.ToInt16(1);
            }
            else
            {
                objEmployeeQualifcn.EQD_HIGHEST_QUAL = Convert.ToInt16(0);
            }
            objEmployeeQualifcn.EQD_TITLE = HttpUtility.HtmlEncode(txtTitle.Text.Trim());
            objEmployeeQualifcn.EQD_FROM_DATE = string.IsNullOrEmpty(txtPeriodfrom.Text.Trim()) ? null : txtPeriodfrom.Text.Trim();
            objEmployeeQualifcn.EQD_TO_DATE = string.IsNullOrEmpty(txtTo.Text.Trim()) ? null : txtTo.Text.Trim();
            objEmployeeQualifcn.EQD_QUALIFIED_ON = string.IsNullOrEmpty(txtQualifiedOn.Text.Trim()) ? null : txtQualifiedOn.Text.Trim();
            objEmployeeQualifcn.EQD_EXPIRY_DATE = string.IsNullOrEmpty(txtExpiresOn.Text.Trim()) ? null : txtExpiresOn.Text.Trim();
            objEmployeeQualifcn.EQD_ISSUED_BY = HttpUtility.HtmlEncode(txtIssuedBy.Text.Trim());
            objEmployeeQualifcn.EQD_INSTITUTE = HttpUtility.HtmlEncode(txtInstitute.Text.Trim());
            objEmployeeQualifcn.EQD_REMARKS = HttpUtility.HtmlEncode(txtRemarks.Text.Trim());
            objEmployeeQualifcn.EQD_CERT_NO = HttpUtility.HtmlEncode(txtCertificateNo.Text.Trim());
            objEmployeeQualifcn.EQD_COUNTRY = Convert.ToInt16(hdfCountry.Value == string.Empty ? 0 : Convert.ToInt16(hdfCountry.Value));
            objEmployeeQualifcn.EQD_INTIMATE_BEFORE = Convert.ToInt16(txtIntimateBefore.Text == string.Empty ? 0 : Convert.ToInt16(txtIntimateBefore.Text.Trim()));
            if (chkActive.Checked == true)
            {
                objEmployeeQualifcn.EQD_ACTIVE = Convert.ToInt16(1);
            }
            else
            {
                objEmployeeQualifcn.EQD_ACTIVE = Convert.ToInt16(0);
            }
            objEmployeeQualifcn.BIZUNIT = currentUser.SBUID;
            objEmployeeQualifcn.ACTIVE = Convert.ToInt16(DbActiveStatus.ACTIVE);
            objEmployeeQualifcn.USER_PK = Convert.ToInt16(currentUser.PKUser);
            objEmployeeQualifcn.LAST_MOD_DT = Convert.ToString(DateTime.Now);
            objEmployeeQualifcn.DocDetails = this.BLUploadList;

            return objEmployeeQualifcn;

        }



        private void GetUIValuesFromObject(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.EMPLOYEEQUALIFICATIONBYID:


                        if (objEmployeeQualifcn != null)
                        {
                            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                            SelectedPK = objEmployeeQualifcn.EQD_PK;
                            txtTitle.Text = HttpUtility.HtmlDecode(objEmployeeQualifcn.EQD_TITLE);
                            ddlQualificationType.SelectedValue = objEmployeeQualifcn.EQD_QUALIFICATION_TYPE.ToString();
                            ddlStatus.SelectedValue = objEmployeeQualifcn.EQD_QUAL_STATUS.ToString();
                            if (objEmployeeQualifcn.EQD_HIGHEST_QUAL == Convert.ToInt16(1))
                            {
                                chkHighestQualf.Checked = true;
                            }
                            else
                            {
                                chkHighestQualf.Checked = false;
                            }
                            if (objEmployeeQualifcn.EQD_FROM_DATE == "1900-01-01" || objEmployeeQualifcn.EQD_FROM_DATE == null)
                            {
                                txtPeriodfrom.Text = "";
                            }
                            else
                            {
                                txtPeriodfrom.Text = Convert.ToDateTime(HttpUtility.HtmlEncode(objEmployeeQualifcn.EQD_FROM_DATE.ToString())).ToString(Resources.Constants.HRMSDateFormatShort);
                            }
                            if (objEmployeeQualifcn.EQD_TO_DATE == "1900-01-01" || objEmployeeQualifcn.EQD_TO_DATE == null)
                            {
                                txtTo.Text = "";
                            }
                            else
                            {
                                txtTo.Text = Convert.ToDateTime(HttpUtility.HtmlEncode(objEmployeeQualifcn.EQD_TO_DATE.ToString())).ToString(Resources.Constants.HRMSDateFormatShort);
                            }

                            if (objEmployeeQualifcn.EQD_QUALIFIED_ON == "1900-01-01" || objEmployeeQualifcn.EQD_QUALIFIED_ON == null)
                            {
                                txtQualifiedOn.Text = "";
                            }
                            else
                            {
                                txtQualifiedOn.Text = Convert.ToDateTime(HttpUtility.HtmlEncode(objEmployeeQualifcn.EQD_QUALIFIED_ON.ToString())).ToString(Resources.Constants.HRMSDateFormatShort);
                            }
                            if (objEmployeeQualifcn.EQD_EXPIRY_DATE == "1900-01-01" || objEmployeeQualifcn.EQD_EXPIRY_DATE == null)
                            {
                                txtExpiresOn.Text = "";
                            }
                            else
                            {
                                txtExpiresOn.Text = Convert.ToDateTime(HttpUtility.HtmlEncode(objEmployeeQualifcn.EQD_EXPIRY_DATE.ToString())).ToString(Resources.Constants.HRMSDateFormatShort);
                            }
                            txtCertificateNo.Text = HttpUtility.HtmlDecode(objEmployeeQualifcn.EQD_CERT_NO);
                            txtInstitute.Text = HttpUtility.HtmlDecode(objEmployeeQualifcn.EQD_INSTITUTE);
                            txtIssuedBy.Text = HttpUtility.HtmlDecode(objEmployeeQualifcn.EQD_ISSUED_BY);
                            txtRemarks.Text = HttpUtility.HtmlDecode(objEmployeeQualifcn.EQD_REMARKS);
                            txtIntimateBefore.Text = (Convert.ToInt16(objEmployeeQualifcn.EQD_INTIMATE_BEFORE).ToString());
                            if (objEmployeeQualifcn.EQD_ACTIVE == Convert.ToInt16(1))
                            {
                                chkActive.Checked = true;
                            }
                            else
                            {
                                chkActive.Checked = false;
                            }
                            hdfCountry.Value = objEmployeeQualifcn.EQD_COUNTRY.ToString() == string.Empty ? "-1" : objEmployeeQualifcn.EQD_COUNTRY.ToString();
                            if (Convert.ToInt16(hdfCountry.Value) != 0)
                            {
                                CountryPk = -1;
                                Int32.TryParse(objEmployeeQualifcn.EQD_COUNTRY.ToString(), out CountryPk);
                                txtCountry.Text = HttpUtility.HtmlEncode(objEmployeeQualifcn.EQD_COUNTRY_TEXT.ToString());
                                hdfCountry.Value = objEmployeeQualifcn.EQD_COUNTRY.ToString() == string.Empty ? "-1" : objEmployeeQualifcn.EQD_COUNTRY.ToString();
                            }
                            lblLastModifiedDate.InnerHtml = Resources.ErpRes.LastModifiedOn + Convert.ToDateTime((objEmployeeQualifcn.LAST_MOD_DT.ToString())).ToString(Resources.ErpRes.LastModifiedDatetimeFormat);
                            ModifiedDatePnl.Visible = true;
                        }
                        break;
                    case ControlsEnum.UPLOADEDFILES:
                        SelectedPK = objEmployeeQualifcn.DOC_PK;
                        LastModifiedTime = Convert.ToDateTime(objEmployeeQualifcn.LAST_MOD_DT);
                        //lblLastModifiedHDR.Text = Resources.Report.LastModifiedOn + LastModifiedTime.ToString(Resources.Report.LastModifiedDatetimeFormat);
                        //BLUploadList = selectedEmployeeDoc.UPloaded Doc List
                        break;
                    case ControlsEnum.SELECTEDDOC:
                        if (blUploadObj != null)
                        {
                            CurrSlNo = blUploadObj.DOC_SEQ_NO;
                            anchorFile.Visible = true;
                            vrfFileUpload.Enabled = false;
                            anchorFile.InnerHtml = blUploadObj.DOC_NAME;
                            anchorFile.HRef = blUploadObj.DOC_PATH;
                            if (FileDetailsList != null && FileDetailsList.Where(fle => fle.SlNo == CurrSlNo).Count() > 0)
                            {
                                anchorFile.Attributes.Add("onclick", "return false;");
                                anchorFile.Attributes.Add("class", "removedownloadClass");
                            }
                            else
                            {
                                anchorFile.Attributes.Add("onclick", "return true;");
                                anchorFile.Attributes.Add("class", "downloadClass");
                            }
                        }
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                ShowListingTabRegScript(0);
                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('0');", true);
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
                            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                            CurrPK = objEmployeeBasicInfo.empPK;
                            UCempBasicHdr.objEmployeeBasicInfo = objEmployeeBasicInfo;
                            UCempBasicHdr.SetFieldValues();
                            //lblhdrEmployeeNoTxt.Text = CommonFunctions.GetShortString(HttpUtility.HtmlDecode(objEmployeeBasicInfo.empCode), 20);
                            //lblhdrEmployeeNameTxt.Text = CommonFunctions.GetShortString(HttpUtility.HtmlDecode(objEmployeeBasicInfo.empNameText), 20);
                            //if (objEmployeeBasicInfo.empDOJText == null)
                            //{

                            //}
                            //else
                            //{
                            //    string dojText = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empDOJText).Trim(); ;
                            //    if (!dojText.IsNullOrEmptyOrWhitespace() && dojText[dojText.Length - 2] == ' ')
                            //    {
                            //        dojText = dojText.Remove(dojText.Length - 2, 1);
                            //    }
                            //    lblhdrDOJText.Text = dojText;
                            //}


                            //if (objEmployeeBasicInfo.empDOBText == null)
                            //{

                            //}
                            //else
                            //{
                            //    string dobText = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empDOBText).Trim();
                            //    if (!dobText.IsNullOrEmptyOrWhitespace() && dobText[dobText.Length - 2] == ' ')
                            //    {
                            //        dobText = dobText.Remove(dobText.Length - 2, 1);
                            //    }
                            //    lblhdrDOBTxt.Text = dobText;
                            //}

                            //lblhdrDesignationTxt.Text = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empDesignationText);
                            //lblhdrDepartmentTxt.Text = CommonFunctions.GetShortString(HttpUtility.HtmlDecode(objEmployeeBasicInfo.empDepartmentText), 20);

                            //lblhdrEmployeeNoTxt.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empCode);
                            //lblhdrEmployeeNameTxt.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empNameText);
                            //lblhdrDOJText.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empDOJText);
                            //lblhdrDOBTxt.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empDOBText);
                            //lblhdrDesignationTxt.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empDesignationText);
                            //lblhdrDepartmentTxt.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empDepartmentText);

                            hdfEmployeeQDOB.Value = objEmployeeBasicInfo.empDOB;
                        }

                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.gComsRes.Information + "');", true);
            }
        }
        private void GetUIValuesFromObjectEmployee(ControlsEnum controlType)
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
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.gComsRes.Information + "');", true);
            }
        }
        private void BindDropDown(ControlsEnum controlType)
        {
            switch (controlType)
            {
                // Fill Shift Details to DropDown


                case ControlsEnum.QUALIFICATIONTYPE:
                    if (dtQualificationType != null)
                    {
                        ddlQualificationType.DataSource = CommonFunctions.HtmlDecodeDataTable(dtQualificationType, ("CON_NAME").ToString());
                        ddlQualificationType.DataTextField = "CON_NAME";
                        ddlQualificationType.DataValueField = "CON_PK";
                        ddlQualificationType.DataBind();
                    }
                    ddlQualificationType.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    break;

                case ControlsEnum.STATUSLIST:
                    if (dtStatus != null)
                    {
                        ddlStatus.DataSource = dtStatus;
                        ddlStatus.DataTextField = GTIService.Constants.HRMS.Employee.Fields.ADM_CFG_TEXT;
                        ddlStatus.DataValueField = GTIService.Constants.HRMS.Employee.Fields.ADM_CFG_VALUE;
                        ddlStatus.DataBind();
                    }
                    ddlStatus.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    break;


                default:
                    break;
            }
        }
        public void BindGrid(ControlsEnum controlType)
        {
            try
            {
                if (dtQualificationList != null && dtQualificationList.Rows.Count > 0)
                    grdEmployeeQualificationlist.DataSource = dtQualificationList;
                else
                    grdEmployeeQualificationlist.DataSource = null;
                grdEmployeeQualificationlist.DataBind();
                if (dtQualificationList.Rows.Count == 0)
                {
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                }
                else
                {
                    btnEdit.Visible = true;
                    btnDelete.Visible = true;
                }
                grdUploads.DataSource = BLUploadList;
                grdUploads.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }
        public void BindGridImage(ControlsEnum controlType)
        {
            grdUploads.DataSource = BLUploadList;
            grdUploads.DataBind();
        }
        public SortDirection dir
        {
            get
            {
                if (ViewState["dirState"] == null)
                {
                    ViewState["dirState"] = SortDirection.Ascending;
                }
                return (SortDirection)ViewState["dirState"];
            }
            set
            {
                ViewState["dirState"] = value;
            }
        }

        private void ButtonClicked()
        {
            if (!string.IsNullOrEmpty(Request.Form[btnNew.UniqueID]))
            {
                Session["QualNewClicked"] = 1;
            }
            if (!string.IsNullOrEmpty(Request.Form[btnEdit.UniqueID]))
            {
                Session["QualEditClicked"] = 1;
            }
            if (!string.IsNullOrEmpty(Request.Form[btnCancel.UniqueID]))
            {
                Session["QualCancelClicked"] = 1;
            }
            if (!string.IsNullOrEmpty(Request.Form[btnAddItem.UniqueID]))
            {
                Session["QualAddItem"] = 1;
            }
            if (!string.IsNullOrEmpty(Request.Form[btnSave.UniqueID]))
            {
                Session["QualSave"] = 1;
            }

        }

        private void ResetForm()
        {
            //CurrPK = 0;
            SelectedPK = 0;

            ddlQualificationType.Items.Clear();
            ddlQualificationType.ClearSelection();
            GetFieldValues(ControlsEnum.QUALIFICATIONTYPE);
            SetFieldValues(ControlsEnum.QUALIFICATIONTYPE);
            txtTitle.Text = "";
            ddlStatus.Items.Clear();
            ddlStatus.ClearSelection();
            GetFieldValues(ControlsEnum.STATUSLIST);
            SetFieldValues(ControlsEnum.STATUSLIST);
            chkHighestQualf.Checked = false;
            txtExpiresOn.Text = "";
            txtTo.Text = "";
            txtQualifiedOn.Text = "";
            txtPeriodfrom.Text = "";
            txtCountry.Text = "";
            hdfCountry.Value = "";
            txtCertificateNo.Text = "";
            txtInstitute.Text = "";
            txtRemarks.Text = "";
            txtIntimateBefore.Text = "";
            txtIssuedBy.Text = string.Empty;
            //chkActive.Checked = true;
            grdUploads.DataSource = null;
            grdUploads.DataBind();
            BLUploadList = null;
            FileDetailsList = null;
            anchorFile.Visible = false;
            vrfFileUpload.Enabled = true;
            CurrSlNo = 0;

        }
        #endregion

        #region ControlEnum
        public enum ControlsEnum
        {

            EMPLOYEEDETAILSHEADER,
            QUALIFICATIONTYPE,
            STATUSLIST,
            EMPLOYEEDETAILSBYID,
            EMPLOYEEQUALIFICATIONLIST,
            EMPLOYEEQUALIFICATIONBYID,
            UPLOADEDFILES,
            SELECTEDDOC
        }

        #endregion

        public ControlsEnum controlType { get; set; }
    }
}