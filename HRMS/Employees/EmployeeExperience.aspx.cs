using System.Web;
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
    public partial class EmployeeExperience : ERP.Store.UI.MyBasePage
    {
        #region Variables and Properties

        #region  Properties

        //private int CurrPK
        //{
        //    get
        //    {
        //        return Convert.ToInt32(this.ViewState[ViewstateStrings.CurrPK]);
        //    }
        //    set
        //    {
        //        this.ViewState[ViewstateStrings.CurrPK] = value;
        //    }
        //}
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
        private int CountryPk;
        private int exitPk;
        private DataTable dtemployeeHeader;
        private DataTable dtCurrency;
        private DataTable dtBaseCurrency;

        private DataTable dtExperienceList;
        private BusinessObject.AccountManagement.ActionsEnum commonActions;
        private EmployeeExprnc objEmployeeExprnc;
        private EmployeeBasicInfomtn objEmployeeBasicInfo;
        User currentUser;
        EmpDocUploadBinder blUploadObj;

        #endregion
        #endregion

        #region Page level Events
        protected void Page_Load(object sender, EventArgs e)
        {
            txtCompany.Focus();
            string[] itemkeyarray;
            itemkeyarray = new string[1];
            itemkeyarray[0] = "DOC_SEQ_NO";
            grdUploads.DataKeyNames = itemkeyarray;
            ButtonClicked();
            if (!IsPostBack)
            {

                GetFieldValues(ControlsEnum.CURRENCY);
                SetFieldValues(ControlsEnum.CURRENCY);
                GetFieldValues(ControlsEnum.BASECURRENCY);
                SetFieldValues(ControlsEnum.BASECURRENCY);
                //ddlCurrency.SelectedIndex = 0;

                //if (Request.QueryString["EmpPK"] != null)
                if (this.CurrPK != 0)
                {
                    //CurrPK = Convert.ToInt32(Request.QueryString["EmpPK"]);
                    GetFieldValues(ControlsEnum.EMPLOYEEDETAILSBYID);
                    SetFieldValues(ControlsEnum.EMPLOYEEDETAILSBYID);
                    GetFieldValues(ControlsEnum.EMPLOYEEDETAILSHEADER);
                    SetFieldValues(ControlsEnum.EMPLOYEEDETAILSHEADER);
                    GetFieldValues(ControlsEnum.EMPLOYEEEXPERIENCELIST);
                    SetFieldValues(ControlsEnum.EMPLOYEEEXPERIENCELIST);
                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('1')", true);
                    ShowListingTabRegScript(1);
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
                //if (Session["HRdiv"] != null)
                //{

                //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide", " ShowHideHRbDetails('1')", true);
                //    Session["HRdiv"] = null;
                //}

                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
                //ShowListingTabRegScript(0);
                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitDate1", "$(document).ready(function () {InitDate();});", true);
                if (EntryStatus == EntryStatus.VIEWMODE)
                {
                    hdfEntryStatus.Value = "1";
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(1);});", true);
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('1')", true);
                ShowListingTabRegScript(1);
            }
        }
        protected void btnAction_PreRender(object sender, EventArgs e)
        {
            int result;
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
                    Session["ExpDetail"] = 1;
                    commonActions = (BusinessObject.AccountManagement.ActionsEnum)(Enum.Parse(typeof(BusinessObject.AccountManagement.ActionsEnum), ((LinkButton)sender).CommandName));
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
                        int cmpDOBperiodFrom = 0;
                        if (hdfEmployeeEDOB.Value != "")
                        {
                            DateTime dateofBirth = Convert.ToDateTime(hdfEmployeeEDOB.Value);
                            DateTime datePeriodFrom = Convert.ToDateTime(string.IsNullOrEmpty(txtPeriodfrom.Text.Trim()) ? DateTime.Now.ToString(Resources.Constants.HRMSDateFormatShort) : txtPeriodfrom.Text.Trim()); ;
                            cmpDOBperiodFrom = datePeriodFrom.CompareTo(dateofBirth);
                        }
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('0')", true);
                            ShowListingTabRegScript(0);
                        }
                        else if (cmpDOBperiodFrom < 0)
                        {
                            litErrorMsg.Text = " Period from must greater than DOB";
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                       + "','" + Resources.ErpRes.Information + "');", true);
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('0')", true);
                            ShowListingTabRegScript(0);
                        }
                        else
                        {
                            objEmployeeExprnc = SetUIValuesToObject();
                            string xmlDoc = CommonFunctions.XmlSerialize<EmployeeExprnc>(objEmployeeExprnc);
                            result = BusinessLogic.HRMS.Employee.EmployeeExperienceBL.SaveEmployeeExperience(xmlDoc);
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
                                    //savePath = string.Format(@"{0}\HR\Misc\", System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower());
                                    savePath = string.Format(@"{0}HR/Misc/", System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower());
                                }
                                if (BLUploadList != null)
                                {
                                    foreach (EmpDocUploadBinder obj in BLUploadList)
                                    {
                                        string filePath = savePath + obj.AttachmentFileName;
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
                                    //GetFieldValues(ControlsEnum.EMPLOYEEEXPERIENCELIST);
                                    //SetFieldValues(ControlsEnum.EMPLOYEEEXPERIENCELIST);
                                    //// Show Save Message and redired to listing page                                        
                                    //litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                    //litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeExperience);
                                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                    ////ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('1')", true);
                                    //ShowListingTabRegScript(1);
                                }

                                GetFieldValues(ControlsEnum.EMPLOYEEEXPERIENCELIST);
                                SetFieldValues(ControlsEnum.EMPLOYEEEXPERIENCELIST);
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeExperience);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('1')", true);
                                ShowListingTabRegScript(1);
                            }
                            else
                            {
                                if (result == (int)DbSaveStatus.SQLERROR)
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('1')", true);
                                    ShowListingTabRegScript(1);
                                }
                                else if (result == (int)DbSaveStatus.CONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.EmployeeExperience + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('1')", true);
                                    ShowListingTabRegScript(1);
                                }
                                else if (result == (int)DbSaveStatus.CODEEXIST)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.EmployeeExperience + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('1')", true);
                                    ShowListingTabRegScript(1);
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeExperience);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('1')", true);
                                    ShowListingTabRegScript(1);
                                }
                            }

                        }
                        break;
                    #endregion
                    #region SAVEANDCONTINUE
                    case BusinessObject.AccountManagement.ActionsEnum.SAVEANDCONTINUE:
                        int cmpDOBperiodFrom2 = 0;
                        if (hdfEmployeeEDOB.Value != "")
                        {
                            DateTime dateofBirth2 = Convert.ToDateTime(hdfEmployeeEDOB.Value);
                            DateTime datePeriodFrom2 = Convert.ToDateTime(string.IsNullOrEmpty(txtPeriodfrom.Text.Trim()) ? DateTime.Now.ToString(Resources.Constants.HRMSDateFormatShort) : txtPeriodfrom.Text.Trim()); ;
                            cmpDOBperiodFrom2 = datePeriodFrom2.CompareTo(dateofBirth2);
                        }
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('0')", true);
                            ShowListingTabRegScript(0);
                        }
                        else if (cmpDOBperiodFrom2 < 0)
                        {
                            litErrorMsg.Text = " Period from must greater than DOB";
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                       + "','" + Resources.ErpRes.Information + "');", true);
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('0')", true);
                            ShowListingTabRegScript(0);
                        }
                        else
                        {
                            objEmployeeExprnc = SetUIValuesToObject();
                            string xmlDoc = CommonFunctions.XmlSerialize<EmployeeExprnc>(objEmployeeExprnc);
                            result = BusinessLogic.HRMS.Employee.EmployeeExperienceBL.SaveEmployeeExperience(xmlDoc);
                            if (result > 0) // Success !  redirect to listing page
                            {
                                if (!string.IsNullOrEmpty(Request.Form[btnSaveContinue.UniqueID]))
                                {
                                    Application["ExpSaveandContinue"] = 1;
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
                                    // savePath = string.Format(@"{0}\HR\Misc\", System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower());
                                    savePath = string.Format(@"{0}HR/Misc/", System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower());
                                }
                                if (BLUploadList != null)
                                {
                                    foreach (EmpDocUploadBinder obj in BLUploadList)
                                    {
                                        string filePath = savePath + obj.AttachmentFileName;
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
                                    //GetFieldValues(ControlsEnum.EMPLOYEEEXPERIENCELIST);
                                    //SetFieldValues(ControlsEnum.EMPLOYEEEXPERIENCELIST);
                                    //// Show Save Message and redired to listing page                                        
                                    //litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                    //litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeQualification);
                                    //Response.Redirect(Resources.PageURL.HrmsSkillDetails);
                                }
                                GetFieldValues(ControlsEnum.EMPLOYEEEXPERIENCELIST);
                                SetFieldValues(ControlsEnum.EMPLOYEEEXPERIENCELIST);
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeExperience);
                                 CommonBL userAuth = new CommonBL();
                                 string nextPageUrl = BusinessLogic.HRMS.Common.HRMSCommonBL.GetNextTabUrl(Convert.ToInt16(EmpTabEnum.Experiance), GetGlobalResourceObject("ConfigurationsRes", "HrmsEmpTabNextURL").ToString().Split(','),
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
                                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('1')", true);
                                    ShowListingTabRegScript(1);
                                }
                                else if (result == (int)DbSaveStatus.CONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.EmployeeExperience + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('1')", true);
                                    ShowListingTabRegScript(1);
                                }
                                else if (result == (int)DbSaveStatus.CODEEXIST)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.EmployeeExperience + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('1')", true);
                                    ShowListingTabRegScript(1);
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeExperience);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('1')", true);
                                    ShowListingTabRegScript(1);
                                }
                            }
                            //Response.Redirect(Resources.PageURL.HrmsSkillDetails);

                        }
                        break;
                    #endregion
                    #region CANCEL

                    case BusinessObject.AccountManagement.ActionsEnum.CANCEL:
                        ResetForm();
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('1')", true);
                        ShowListingTabRegScript(1);
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
                        ResetForm();
                        btnDelete.Visible = false;
                        ModifiedDatePnl.Visible = false;
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('0')", true);
                        ShowListingTabRegScript(0);
                        break;

                    #endregion
                    #region EDIT

                    case BusinessObject.AccountManagement.ActionsEnum.EDIT:
                        foreach (GridViewRow grdrow in grdEmployeeExperiencelist.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {

                                bIsChecked = true;
                                SelectedPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfExperncePk")).Value);
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            if (!string.IsNullOrEmpty(Request.Form[btnEdit.UniqueID]))
                            {
                                Session["EditExperienceClicked"] = 1;
                            }
                            btnDelete.Visible = true;
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('0')", true);
                            ShowListingTabRegScript(0);
                            GetFieldValues(ControlsEnum.EMPLOYEEEXPERIENCEBYID);
                            SetFieldValues(ControlsEnum.EMPLOYEEEXPERIENCEBYID);
                            if (this.objEmployeeExprnc != null)
                            {
                                this.BLUploadList = this.objEmployeeExprnc.DocDetails;
                                BindGridImage(controlType);
                            }
                        }
                        else
                        {

                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('1')", true);
                            ShowListingTabRegScript(1);
                        }
                        break;
                    #endregion
                    #region EMPDOCDETAIL

                    case BusinessObject.AccountManagement.ActionsEnum.EMPDOCDETAIL:
                        foreach (GridViewRow grdrow in grdEmployeeExperiencelist.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {

                                bIsChecked = true;
                                SelectedPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfExperncePk")).Value);
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('0')", true);
                            ShowListingTabRegScript(0);
                            GetFieldValues(ControlsEnum.EMPLOYEEEXPERIENCEBYID);
                            SetFieldValues(ControlsEnum.EMPLOYEEEXPERIENCEBYID);
                            if (this.objEmployeeExprnc != null)
                            {
                                this.BLUploadList = this.objEmployeeExprnc.DocDetails;
                                BindGridImage(controlType);
                            }
                        }
                        else
                        {

                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('1')", true);
                            ShowListingTabRegScript(1);
                        }
                        break;
                    #endregion
                    #region EMPEXPLIST

                    case BusinessObject.AccountManagement.ActionsEnum.EMPDOCLIST:
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('1')", true);
                        ShowListingTabRegScript(1);
                        FileDetailsList = null;
                        break;
                    #endregion
                    #region DELETE
                    case BusinessObject.AccountManagement.ActionsEnum.DELETE:
                        Session["ExpDelete"] = 1;
                        foreach (GridViewRow grdrow in grdEmployeeExperiencelist.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                SelectedPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfExperncePk")).Value);
                                break;
                            }
                        }
                        if (bIsChecked)
                        {

                            result = BusinessLogic.HRMS.Employee.EmployeeExperienceBL.DeleteexperiencebyexpId(SelectedPK);

                            DbDeleteStatus deleteStatus = (DbDeleteStatus)result;
                            switch (deleteStatus)
                            {
                                case DbDeleteStatus.DELETED://If deletion is success
                                    ResetForm();
                                    GetFieldValues(ControlsEnum.EMPLOYEEEXPERIENCELIST);
                                    SetFieldValues(ControlsEnum.EMPLOYEEEXPERIENCELIST);
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeExperience);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('1')", true);
                                    ShowListingTabRegScript(1);
                                    break;
                                case DbDeleteStatus.REFERRED://If referred to another page
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Ref;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeExperience);
                                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('1')", true);
                                    ShowListingTabRegScript(1);

                                    break;
                                case DbDeleteStatus.SQLERROR://Sql error
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Sql_Error;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeExperience);
                                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('1')", true);
                                    ShowListingTabRegScript(1);
                                    break;
                            }
                        }
                        else
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('1')", true);
                            ShowListingTabRegScript(1);
                        }

                        break;
                    #endregion
                    #region ADDITEM
                    case BusinessObject.AccountManagement.ActionsEnum.ADDITEM:
                        if (!string.IsNullOrEmpty(Request.Form[btnAddItem.UniqueID]))
                        {
                            Session["AddgriditemClicked"] = 1;
                        }
                        //validate Page
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_SavError;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('0')", true);
                            ShowListingTabRegScript(0);
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
                                                blUploadObj.DOC_PATH = System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower() + "HR/Misc/" + attachmentFileName;
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
                                        blUploadObj.DOC_PATH = System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower() + "HR/Misc/" + attachmentFileName;
                                    }

                                    // blUploadObj.SCD_FILE_PATH = "~/Upload/" + attachmentFileName;
                                    blUploadObj.DOC_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                                    FileDetailsList.Add(new FileDetails() { SlNo = slno, EmpDocFile = HttpContext.Current.Request.Files[0] });
                                    BLUploadList.Add(blUploadObj);
                                }
                            }
                            SetFieldValues(ControlsEnum.UPLOADEDFILES);
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('0')", true);
                            ShowListingTabRegScript(0);
                            fupUpload.Focus();
                            //EntryStatus = EntryStatus.NEWMODE;
                            // ResetForm(ActionsEnum.ADDITEM);
                        }
                        break;
                    #endregion
                    #region REMOVEITEM
                    case BusinessObject.AccountManagement.ActionsEnum.REMOVEITEM:
                        Session["ExpRemove"] = 1;
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


                }


            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('0')", true);
                ShowListingTabRegScript(0);
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
                    //if (Session["AddgriditemClicked"] != null)
                    //{

                    //    if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
                    //    {
                    //        //e.Row.Cells[4].Visible = false;
                    //        e.Row.Cells[2].Visible = false;
                    //        e.Row.Cells[3].Visible = false;
                    //        //e.Row.Cells[4].Visible = false;
                    //        //Session["AddgriditemClicked"] = null;
                    //    //}
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
                GetFieldValues(ControlsEnum.EMPLOYEEEXPERIENCELIST);
                SetFieldValues(ControlsEnum.EMPLOYEEEXPERIENCELIST);
                DataTable dt = new DataTable();
                grdEmployeeExperiencelist.DataSource = dtExperienceList;
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
                    DataView sortedView = new DataView(dtExperienceList);
                    sortedView.Sort = e.SortExpression + " " + SortDir;
                    grdEmployeeExperiencelist.DataSource = sortedView;
                    grdEmployeeExperiencelist.DataBind();
                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('1')", true);
                    ShowListingTabRegScript(1);
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

                    case ControlsEnum.CURRENCY:
                        currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                        dtCurrency = BusinessLogic.HRMS.Employee.EmployeeExperienceBL.GetCurrency(currentUser.SBUID);
                        break;


                    case ControlsEnum.BASECURRENCY:
                        currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                        dtBaseCurrency = BusinessLogic.HRMS.Employee.EmployeeExperienceBL.GetBaseCurrency();
                        break;

                    case ControlsEnum.EMPLOYEEDETAILSHEADER:
                        dtemployeeHeader = DataAccess.HRMS.Employee.EmployeeBasicInfoDL.GetEmployeeDetailListHeader(CurrPK, string.Empty);
                        break;

                    case ControlsEnum.EMPLOYEEEXPERIENCELIST:
                        dtExperienceList = new DataTable();
                        dtExperienceList = BusinessLogic.HRMS.Employee.EmployeeExperienceBL.GetEmployeeExperienceList(CurrPK);
                        break;

                    case ControlsEnum.EMPLOYEEEXPERIENCEBYID:
                        objEmployeeExprnc = BusinessLogic.HRMS.Employee.EmployeeExperienceBL.GetEmployeeexperiencebyID(SelectedPK);
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

                    case ControlsEnum.CURRENCY:
                        BindDropDown(controlType);
                        break;
                    case ControlsEnum.BASECURRENCY:
                        ddlCurrency.SelectedValue = Convert.ToInt32(dtBaseCurrency.Rows[0]["ACF_DATA"]).ToString();
                        break;
                    case ControlsEnum.EMPLOYEEDETAILSHEADER:
                        GetUIValuesFromObjectHeader(controlType);
                        break;
                    case ControlsEnum.EMPLOYEEDETAILSBYID:
                        GetUIValuesFromObjectEmployee(controlType);
                        break;
                    case ControlsEnum.EMPLOYEEEXPERIENCELIST:
                        if (dtExperienceList.Rows.Count > 0)
                        {
                            BindGrid(controlType);
                        }
                        else
                        {
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('0')", true);
                            ShowListingTabRegScript(0);
                            btnDelete.Visible = false;
                        }
                        break;
                    case ControlsEnum.EMPLOYEEEXPERIENCEBYID:
                        GetUIValuesFromObject(controlType);
                        break;
                    case ControlsEnum.UPLOADEDFILES:
                        if (objEmployeeExprnc != null)
                            GetUIValuesFromObject(ControlsEnum.UPLOADEDFILES);
                        BindGridImage(controlType);
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('0')", true);
                        ShowListingTabRegScript(0);
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

        private void BindDropDown(ControlsEnum controlType)
        {
            switch (controlType)
            {

                case ControlsEnum.CURRENCY:
                    if (dtCurrency != null)
                    {
                        ddlCurrency.DataSource = CommonFunctions.HtmlDecodeDataTable(dtCurrency, "CUR_NAME");
                        ddlCurrency.DataTextField = "CUR_CODE";
                        ddlCurrency.DataValueField = "CUR_PK";
                        ddlCurrency.DataBind();
                        //ddlCurrency.Items.Insert(0, new ListItem("Select", "0"));
                    }


                    break;

                default:
                    break;
            }
        }
        private EmployeeExprnc SetUIValuesToObject()
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            objEmployeeExprnc = new EmployeeExprnc();
            objEmployeeExprnc.EED_EMPLOYEE = CurrPK;
            objEmployeeExprnc.EED_PK = SelectedPK;
            objEmployeeExprnc.EED_EMPLOYER = HttpUtility.HtmlEncode(txtCompany.Text.Trim());
            objEmployeeExprnc.EED_DESIGNATION = HttpUtility.HtmlEncode(txtDesignation.Text.Trim());
            objEmployeeExprnc.EED_EXIT_REASON = Convert.ToInt16(hdfExitReason.Value == string.Empty ? 0 : Convert.ToInt16(hdfExitReason.Value));
            objEmployeeExprnc.EED_PERIOD_FROM = string.IsNullOrEmpty(txtPeriodfrom.Text.Trim()) ? "" : txtPeriodfrom.Text.Trim();
            objEmployeeExprnc.EED_PERIOD_TO = string.IsNullOrEmpty(txtTo.Text.Trim()) ? " " : txtTo.Text.Trim();
            //objEmployeeExprnc.EED_PERIOD_FROM = HttpUtility.HtmlEncode(txtPeriodfrom.Text.Trim());
            //objEmployeeExprnc.EED_PERIOD_TO = HttpUtility.HtmlEncode(txtTo.Text.Trim());
            objEmployeeExprnc.EED_COUNTRY = Convert.ToInt16(hdfCountry.Value == string.Empty ? 0 : Convert.ToInt16(hdfCountry.Value));
            objEmployeeExprnc.EED_CURRENCY = Convert.ToInt16(ddlCurrency.SelectedValue == (0).ToString() ? 0 : Convert.ToInt16(ddlCurrency.SelectedValue));
            objEmployeeExprnc.EED_SALARY = Convert.ToDecimal(txtSalary.Text == string.Empty ? 0 : Convert.ToDouble(txtSalary.Text.Trim()));
            objEmployeeExprnc.EED_JOB_NATURE = HttpUtility.HtmlEncode(txtNatureOfJob.Text.Trim());
            objEmployeeExprnc.EED_CITY = HttpUtility.HtmlEncode(txtCity.Text.Trim());
            objEmployeeExprnc.EED_STATE = HttpUtility.HtmlEncode(txtState.Text.Trim());
            objEmployeeExprnc.EED_ZIP_CODE = HttpUtility.HtmlEncode(txtZipCode.Text.Trim());
            objEmployeeExprnc.EED_PHONE = HttpUtility.HtmlEncode(txtPhone.Text.Trim());
            objEmployeeExprnc.EED_MOBILE = HttpUtility.HtmlEncode(txtMobile.Text.Trim());
            objEmployeeExprnc.EED_JOB_DESCRIPTION = HttpUtility.HtmlEncode(txtJobDescription.Text.Trim());
            objEmployeeExprnc.EED_REMARKS = HttpUtility.HtmlEncode(txtRemarks.Text.Trim());

            objEmployeeExprnc.EED_HR_NAME = HttpUtility.HtmlEncode(txtHRName.Text.Trim());
            objEmployeeExprnc.EED_HR_DESIGNATION = HttpUtility.HtmlEncode(txtHRDesignation.Text.Trim());
            objEmployeeExprnc.EED_HR_EMAIL = HttpUtility.HtmlEncode(txtHREmail.Text.Trim());
            objEmployeeExprnc.EED_HR_PHONE = HttpUtility.HtmlEncode(txtHRPhone.Text.Trim());
            objEmployeeExprnc.EED_HR_PHONE_EXT = HttpUtility.HtmlEncode(txtHRExtension.Text.Trim());
            objEmployeeExprnc.EED_HR_MOBILE1 = HttpUtility.HtmlEncode(txtHRmobile1.Text.Trim());
            objEmployeeExprnc.EED_HR_MOBILE2 = HttpUtility.HtmlEncode(txtHRMobile2.Text.Trim());

            objEmployeeExprnc.EED_REF_NAME = HttpUtility.HtmlEncode(txtVerName.Text.Trim());
            objEmployeeExprnc.EED_REF_DESIGNATION = HttpUtility.HtmlEncode(txtVerDesignation.Text.Trim());
            objEmployeeExprnc.EED_REF_EMAIL = HttpUtility.HtmlEncode(txtVerEmail.Text.Trim());
            objEmployeeExprnc.EED_REF_PHONE = HttpUtility.HtmlEncode(txtVerPhone.Text.Trim());
            objEmployeeExprnc.EED_REF_PHONE_EXT = HttpUtility.HtmlEncode(txtVerExt.Text.Trim());
            objEmployeeExprnc.EED_REF_MOBILE1 = HttpUtility.HtmlEncode(txtVermobile1.Text.Trim());
            objEmployeeExprnc.EED_REF_MOBILE2 = HttpUtility.HtmlEncode(txtVerMobile2.Text.Trim());




            objEmployeeExprnc.BIZUNIT = currentUser.SBUID;
            objEmployeeExprnc.ACTIVE = Convert.ToInt16(DbActiveStatus.ACTIVE);
            objEmployeeExprnc.USER_PK = Convert.ToInt16(currentUser.PKUser);
            objEmployeeExprnc.LAST_MOD_DT = Convert.ToString(DateTime.Now);
            objEmployeeExprnc.DocDetails = this.BLUploadList;
            return objEmployeeExprnc;


        }
        private void GetUIValuesFromObject(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.EMPLOYEEEXPERIENCEBYID:


                        if (objEmployeeExprnc != null)
                        {
                            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                            SelectedPK = objEmployeeExprnc.EED_PK;
                            txtCompany.Text = HttpUtility.HtmlDecode(objEmployeeExprnc.EED_EMPLOYER);
                            txtDesignation.Text = HttpUtility.HtmlDecode(objEmployeeExprnc.EED_DESIGNATION);

                            if (objEmployeeExprnc.EED_PERIOD_FROM == "1900-01-01")
                            {
                                txtPeriodfrom.Text = "";
                            }
                            else
                            {
                                txtPeriodfrom.Text = Convert.ToDateTime(HttpUtility.HtmlEncode(objEmployeeExprnc.EED_PERIOD_FROM.ToString())).ToString(Resources.Constants.HRMSDateFormatShort);
                            }
                            if (objEmployeeExprnc.EED_PERIOD_TO == "1900-01-01")
                            {
                                txtTo.Text = "";
                            }
                            else
                            {
                                txtTo.Text = Convert.ToDateTime(HttpUtility.HtmlEncode(objEmployeeExprnc.EED_PERIOD_TO.ToString())).ToString(Resources.Constants.HRMSDateFormatShort);
                            }
                            txtNatureOfJob.Text = HttpUtility.HtmlDecode(objEmployeeExprnc.EED_JOB_NATURE);
                            ddlCurrency.SelectedValue = objEmployeeExprnc.EED_CURRENCY.ToString() == string.Empty ? "0" : objEmployeeExprnc.EED_CURRENCY.ToString();
                            txtSalary.Text = Convert.ToDecimal(objEmployeeExprnc.EED_SALARY).ToString("#0.00");
                            txtCity.Text = HttpUtility.HtmlDecode(objEmployeeExprnc.EED_CITY);
                            txtState.Text = HttpUtility.HtmlDecode(objEmployeeExprnc.EED_STATE);
                            txtZipCode.Text = HttpUtility.HtmlDecode(objEmployeeExprnc.EED_ZIP_CODE);
                            txtPhone.Text = HttpUtility.HtmlDecode(objEmployeeExprnc.EED_PHONE);
                            txtMobile.Text = HttpUtility.HtmlDecode(objEmployeeExprnc.EED_MOBILE);
                            txtJobDescription.Text = HttpUtility.HtmlDecode(objEmployeeExprnc.EED_JOB_DESCRIPTION);
                            txtRemarks.Text = HttpUtility.HtmlDecode(objEmployeeExprnc.EED_REMARKS);

                            hdfCountry.Value = objEmployeeExprnc.EED_COUNTRY.ToString() == string.Empty ? "-1" : objEmployeeExprnc.EED_COUNTRY.ToString();
                            if (Convert.ToInt16(hdfCountry.Value) != 0)
                            {
                                CountryPk = -1;
                                Int32.TryParse(objEmployeeExprnc.EED_COUNTRY.ToString(), out CountryPk);
                                txtCountry.Text = HttpUtility.HtmlEncode(objEmployeeExprnc.EED_COUNTRY_TEXT.ToString());
                                hdfCountry.Value = objEmployeeExprnc.EED_COUNTRY.ToString() == string.Empty ? "-1" : objEmployeeExprnc.EED_COUNTRY.ToString();
                            }

                            hdfExitReason.Value = objEmployeeExprnc.EED_EXIT_REASON.ToString() == string.Empty ? "-1" : objEmployeeExprnc.EED_EXIT_REASON.ToString();
                            if (Convert.ToInt16(hdfExitReason.Value) != 0)
                            {
                                exitPk = -1;
                                Int32.TryParse(objEmployeeExprnc.EED_EXIT_REASON.ToString(), out exitPk);
                                txtExitReason.Text = HttpUtility.HtmlEncode(objEmployeeExprnc.EED_EXIT_REASON_TEXT.ToString());
                                hdfExitReason.Value = objEmployeeExprnc.EED_EXIT_REASON.ToString() == string.Empty ? "-1" : objEmployeeExprnc.EED_EXIT_REASON.ToString();
                            }

                            txtHRName.Text = HttpUtility.HtmlDecode(objEmployeeExprnc.EED_HR_NAME);
                            txtHRDesignation.Text = HttpUtility.HtmlDecode(objEmployeeExprnc.EED_HR_DESIGNATION);
                            txtHREmail.Text = HttpUtility.HtmlDecode(objEmployeeExprnc.EED_HR_EMAIL);
                            txtHRPhone.Text = HttpUtility.HtmlDecode(objEmployeeExprnc.EED_HR_PHONE);
                            txtHRExtension.Text = HttpUtility.HtmlDecode(objEmployeeExprnc.EED_HR_PHONE_EXT);
                            txtHRmobile1.Text = HttpUtility.HtmlDecode(objEmployeeExprnc.EED_HR_MOBILE1);
                            txtHRMobile2.Text = HttpUtility.HtmlDecode(objEmployeeExprnc.EED_HR_MOBILE2);

                            txtVerName.Text = HttpUtility.HtmlDecode(objEmployeeExprnc.EED_REF_NAME);
                            txtVerDesignation.Text = HttpUtility.HtmlDecode(objEmployeeExprnc.EED_REF_DESIGNATION);
                            txtVerEmail.Text = HttpUtility.HtmlDecode(objEmployeeExprnc.EED_REF_EMAIL);
                            txtVerPhone.Text = HttpUtility.HtmlDecode(objEmployeeExprnc.EED_REF_PHONE);
                            txtVerExt.Text = HttpUtility.HtmlDecode(objEmployeeExprnc.EED_REF_PHONE_EXT);
                            txtVermobile1.Text = HttpUtility.HtmlDecode(objEmployeeExprnc.EED_REF_MOBILE1);
                            txtVerMobile2.Text = HttpUtility.HtmlDecode(objEmployeeExprnc.EED_REF_MOBILE2);


                            txtCompany.ToolTip = HttpUtility.HtmlDecode(objEmployeeExprnc.EED_EMPLOYER);
                            txtDesignation.ToolTip = HttpUtility.HtmlDecode(objEmployeeExprnc.EED_DESIGNATION);
                            txtSalary.ToolTip = Convert.ToDecimal(objEmployeeExprnc.EED_SALARY).ToString("#0.00");
                            txtCity.ToolTip = HttpUtility.HtmlDecode(objEmployeeExprnc.EED_CITY);
                            txtState.ToolTip = HttpUtility.HtmlDecode(objEmployeeExprnc.EED_STATE);
                            txtZipCode.ToolTip = HttpUtility.HtmlDecode(objEmployeeExprnc.EED_ZIP_CODE);
                            txtPhone.ToolTip = HttpUtility.HtmlDecode(objEmployeeExprnc.EED_PHONE);
                            txtMobile.ToolTip = HttpUtility.HtmlDecode(objEmployeeExprnc.EED_MOBILE);
                            txtJobDescription.ToolTip = HttpUtility.HtmlDecode(objEmployeeExprnc.EED_JOB_DESCRIPTION);
                            txtRemarks.ToolTip = HttpUtility.HtmlDecode(objEmployeeExprnc.EED_REMARKS);
                            txtHRName.ToolTip = HttpUtility.HtmlDecode(objEmployeeExprnc.EED_HR_NAME);
                            txtHRDesignation.ToolTip = HttpUtility.HtmlDecode(objEmployeeExprnc.EED_HR_DESIGNATION);
                            txtHREmail.ToolTip = HttpUtility.HtmlDecode(objEmployeeExprnc.EED_HR_EMAIL);
                            txtHRPhone.ToolTip = HttpUtility.HtmlDecode(objEmployeeExprnc.EED_HR_PHONE);
                            txtHRExtension.ToolTip = HttpUtility.HtmlDecode(objEmployeeExprnc.EED_HR_PHONE_EXT);
                            txtHRmobile1.ToolTip = HttpUtility.HtmlDecode(objEmployeeExprnc.EED_HR_MOBILE1);
                            txtHRMobile2.ToolTip = HttpUtility.HtmlDecode(objEmployeeExprnc.EED_HR_MOBILE2);
                            txtVerName.ToolTip = HttpUtility.HtmlDecode(objEmployeeExprnc.EED_REF_NAME);
                            txtVerDesignation.ToolTip = HttpUtility.HtmlDecode(objEmployeeExprnc.EED_REF_DESIGNATION);
                            txtVerEmail.ToolTip = HttpUtility.HtmlDecode(objEmployeeExprnc.EED_REF_EMAIL);
                            txtVerPhone.ToolTip = HttpUtility.HtmlDecode(objEmployeeExprnc.EED_REF_PHONE);
                            txtVerExt.ToolTip = HttpUtility.HtmlDecode(objEmployeeExprnc.EED_REF_PHONE_EXT);
                            txtVermobile1.ToolTip = HttpUtility.HtmlDecode(objEmployeeExprnc.EED_REF_MOBILE1);
                            txtVerMobile2.ToolTip = HttpUtility.HtmlDecode(objEmployeeExprnc.EED_REF_MOBILE2);


                            lblLastModifiedDate.InnerHtml = Resources.ErpRes.LastModifiedOn + Convert.ToDateTime((objEmployeeExprnc.LAST_MOD_DT.ToString())).ToString(Resources.ErpRes.LastModifiedDatetimeFormat);
                            ModifiedDatePnl.Visible = true;

                            if (txtHRName.Text == "")
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide", "ShowHideHRbDetails('0')", true);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide", " ShowHideHRbDetails('1')", true);
                            }
                            if (txtVerName.Text == "")
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHideV", "ShowHideContactDetails('0')", true);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHideV", "ShowHideContactDetails('1')", true);
                            }

                        }
                        break;
                    case ControlsEnum.UPLOADEDFILES:
                        SelectedPK = objEmployeeExprnc.DOC_PK;
                        LastModifiedTime = Convert.ToDateTime(objEmployeeExprnc.LAST_MOD_DT);
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
                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowListingTab('1')", true);
                ShowListingTabRegScript(1);
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

                            hdfEmployeeEDOB.Value = objEmployeeBasicInfo.empDOB;
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
        public void BindGrid(ControlsEnum controlType)
        {
            try
            {


                if (dtExperienceList != null && dtExperienceList.Rows.Count > 0)
                    grdEmployeeExperiencelist.DataSource = dtExperienceList;

                else
                    grdEmployeeExperiencelist.DataSource = null;
                grdEmployeeExperiencelist.DataBind();
                if (dtExperienceList.Rows.Count == 0)
                {
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                }
                else
                {
                    btnEdit.Visible = true;
                    btnDelete.Visible = true;
                }
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
        private void ResetForm()
        {

            SelectedPK = 0;
            GetFieldValues(ControlsEnum.CURRENCY);
            SetFieldValues(ControlsEnum.CURRENCY);
            GetFieldValues(ControlsEnum.BASECURRENCY);
            SetFieldValues(ControlsEnum.BASECURRENCY);
            //ddlCurrency.SelectedIndex = 0;
            txtCity.Text = "";
            txtCompany.Text = "";
            txtCountry.Text = "";
            txtDesignation.Text = "";
            txtExitReason.Text = "";
            txtJobDescription.Text = "";
            txtMobile.Text = "";
            txtNatureOfJob.Text = "";
            txtPeriodfrom.Text = "";
            txtPhone.Text = "";
            txtRemarks.Text = "";
            txtSalary.Text = "";
            txtState.Text = "";
            txtTo.Text = "";
            txtZipCode.Text = "";
            hdfCountry.Value = "";
            hdfExitReason.Value = "";

            txtHRName.Text = "";
            txtHRDesignation.Text = "";
            txtHREmail.Text = "";
            txtHRPhone.Text = "";
            txtHRPhone.Text = "";
            txtHRmobile1.Text = "";
            txtHRMobile2.Text = "";
            txtVerName.Text = "";
            txtVerDesignation.Text = "";
            txtVerEmail.Text = "";
            txtVerPhone.Text = "";
            txtVermobile1.Text = "";
            txtVerMobile2.Text = "";

            txtHRExtension.Text = "";
            txtVerExt.Text = "";
            grdUploads.DataSource = null;
            grdUploads.DataBind();
            BLUploadList = null;
            FileDetailsList = null;
            anchorFile.Visible = false;
            vrfFileUpload.Enabled = true;
            CurrSlNo = 0;

        }
        private void ButtonClicked()
        {
            if (!string.IsNullOrEmpty(Request.Form[btnNew.UniqueID]))
            {
                Session["ExpNewClicked"] = 1;
            }
            if (!string.IsNullOrEmpty(Request.Form[btnEdit.UniqueID]))
            {
                Session["ExpEditClicked"] = 1;
            }
            if (!string.IsNullOrEmpty(Request.Form[btnCancel.UniqueID]))
            {
                Session["ExpCancelClicked"] = 1;
            }
            if (!string.IsNullOrEmpty(Request.Form[btnAddItem.UniqueID]))
            {
                Session["ExpAddItem"] = 1;
            }
            if (!string.IsNullOrEmpty(Request.Form[btnSave.UniqueID]))
            {
                Session["ExpSave"] = 1;
            }
            if (!string.IsNullOrEmpty(Request.Form[btnCancelList.UniqueID]))
            {
                Session["CancelExpClicked"] = 1;
            }

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
        #endregion

        #region ControlEnum
        public enum ControlsEnum
        {


            EMPLOYEEDETAILSHEADER,
            EMPLOYEEDETAILSBYID,
            EMPLOYEEEXPERIENCELIST,
            EMPLOYEEEXPERIENCEBYID,
            UPLOADEDFILES,
            SELECTEDDOC,
            CURRENCY,
            BASECURRENCY
        }
        public ControlsEnum controlType { get; set; }

        #endregion
    }
}