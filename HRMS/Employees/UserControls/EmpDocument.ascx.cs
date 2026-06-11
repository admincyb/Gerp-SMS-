using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.HRMS.Employee;
using BusinessLogic.HRMS.Employee;
using System.Data;
using BusinessObject.Common;
using ERP.Utilities;
using BusinessObject.CommonManagement;
using System.IO;
using ERP.Utilities.HRMS;
using System.Web.UI.HtmlControls;
using BusinessLogic.CommonManagement;
using System.Web.Security;

namespace HRMS.Employees.UserControls
{
    public partial class EmpDocument : System.Web.UI.UserControl
    {
        /// <summary>
        /// Current PK
        /// </summary>
        private int CurrPK
        {
            get
            {
                return (int)(Session[ERP.Utilities.SessionStrings.CurrentPK] ?? 0);
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.CurrentPK] = value;
            }
        }

        /// <summary>
        /// Select PK
        /// </summary>
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

        /// <summary>
        /// Entry State for managing display status
        /// </summary>
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
        /// <summary>
        /// To maintain the SortExpression or sort By in viewstate
        /// </summary>
        private string SortBy
        {
            get
            {
                return (string)this.ViewState[ViewstateStrings.SortBy];
            }
            set
            {
                this.ViewState[ViewstateStrings.SortBy] = value;
            }
        }

        /// <summary>
        /// To maintain the Sort Direction in viewstate
        /// </summary>
        private string SortDirection
        {
            get
            {
                return (string)this.ViewState[ViewstateStrings.SortDirection];
            }
            set
            {
                this.ViewState[ViewstateStrings.SortDirection] = value;
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

        public List<EmpDocUploadBinder> BLUploadList
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
        public List<FileDetails> FileDetailsList
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
        public EmployeeDoc EmpDocDetails
        {
            get
            {
                return ViewState[ViewstateStrings.EmpDocDetails] == null ? null : (EmployeeDoc)ViewState[ViewstateStrings.EmpDocDetails];
            }
            set
            {
                ViewState[ViewstateStrings.EmpDocDetails] = value;
            }
        }
        public int DefaultDocType
        {
            get
            {
                return ViewState[ViewstateStrings.DefaultDocType] == null ? 0 : Convert.ToInt32(ViewState[ViewstateStrings.DefaultDocType]);
            }
            set
            {
                ViewState[ViewstateStrings.DefaultDocType] = value;
                if (DefaultDocType > 0 && pageData != null && pageData.Rows.Count > 0)
                    ddlDocumentTypeList.SelectedIndex = ddlDocumentTypeList.Items.IndexOf(ddlDocumentTypeList.Items.FindByValue(Convert.ToString(ViewState[ViewstateStrings.DefaultDocType])));

            }
        }
        public string EmployeeDOB
        {
            get
            {
                return hdfHdrDOB.Value;
            }
            set
            {
                hdfHdrDOB.Value = value;
            }
        }
        
       
        public event EventHandler AfterPostback;
        public event EventHandler AfterApply;  
        /// <summary>
        /// Entry State for managing display status
        /// </summary>
        public ControlParentPage ParentPage 
        {
            get
            {
                return this.ViewState[ViewstateStrings.ParentPage] == null ? ControlParentPage.EmpDoc : (ControlParentPage)(this.ViewState[ViewstateStrings.ParentPage]);
            }
            set
            {
                this.ViewState[ViewstateStrings.ParentPage] = value;
                if ((ControlParentPage)(this.ViewState[ViewstateStrings.ParentPage]) == ControlParentPage.EmployeeMaster)
                {
                    divNormalButtons.Visible = false;
                    divPopupButtons.Visible = true;
                    divEmpBasicInfo.Visible = false;
                    divBtnListDtl.Visible = false;
                    this.EntryStatus = EntryStatus.NEWMODE;
                    this.currentUser = (BusinessObject.User)Context.User.Identity;
                    SetUIEditView(ActionsEnum.NEW);
                }
                else
                {
                    divNormalButtons.Visible = true;
                    divPopupButtons.Visible = false;
                    divEmpBasicInfo.Visible = true;
                    divBtnListDtl.Visible = true;
                }
            }
        }

        //private int SelectedDocPk;
        private ActionsEnum commonActions = ActionsEnum.DEFAULT;
        private DataTable pageData;
        private BusinessObject.User currentUser;
        private EmployeeDoc selectedEmployeeDoc;
        //private List<EmployeeDocumentDetails> EmployeeDocumentDetailsList;
        private DataTable EmployeeDocumentDetailsList;

        //Header Info
        private EmployeeBasicInfomtn objEmployeeBasicInfo;
        private DataTable dtemployeeHeader;

        EmpDocUploadBinder blUploadObj;

        protected void Page_Load(object sender, EventArgs e)
        {
            PageActionHandler();
        }
      
        #region PageActionHandler
        /// <summary>
        /// Handle All page Related Events
        /// </summary>
        private void PageActionHandler()
        {           
            Page.Form.Attributes.Add("enctype", "multipart/form-data");
            this.currentUser = (BusinessObject.User)Context.User.Identity;
            if (this.CurrPK == 0 && ParentPage == ControlParentPage.EmpDoc)
            {
                litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                Session["SelectMessage"] = litErrorMsg.Text;
                Response.Redirect(Resources.PageURL.EmployeeList);
            }
            else
            {
                string[] itemkeyarray;
                itemkeyarray = new string[1];
                itemkeyarray[0] = "DOC_SEQ_NO";
                grdUploads.DataKeyNames = itemkeyarray;

                if (!IsPostBack)
                {
                    hdfCurrentDepartment.Value = currentUser.CurrentDeptPK.ToString();
                    InitializePage();
                    GetFieldValues(ControlsEnum.EMPLOYEEDETAILSHEADER);
                    SetFieldValues(ControlsEnum.EMPLOYEEDETAILSHEADER);

                    GetFieldValues(ControlsEnum.EMPDOCLIST);
                    SetFieldValues(ControlsEnum.EMPDOCLIST);
                }
                this.lblExpiryDaysLeft.Text = hdfExpiryDaysLeft.Value.ToString();
            }
        }
        #endregion

        private void InitializePage()
        {
            txtCheckedInOn.Text = DateTime.Now.ToString(Resources.Constants.HRMSDateFormatShort);
        }

        protected void Page_PreRender(Object sender, EventArgs e)
        {

            lblBreadCrum.Text = GetLocalResourceObject("Breadcrumb").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
            try
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitializeComponents();});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SetIssuedDate", "$(document).ready(function(){setDocumentIssuedDate();});", true);
                if (EntryStatus == EntryStatus.VIEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "EmpDocViewMode", "$(document).ready(function(){EmpDocViewMode(1);});", true);
                    lnkList.CssClass = GetLocalResourceObject("TabInactive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                    ModifiedDatePnl.Visible = true;
                }
                else if (EntryStatus == EntryStatus.NEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "EmpDocViewMode", "$(document).ready(function(){EmpDocViewMode(2);});", true);
                    lnkList.CssClass = GetLocalResourceObject("TabInactive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                    ModifiedDatePnl.Visible = false;
                }
                else if (EntryStatus == EntryStatus.ENTRYMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                    lnkList.CssClass = GetLocalResourceObject("TabInactive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                    ModifiedDatePnl.Visible = false;
                }
                else if (EntryStatus == EntryStatus.EDITMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                    lnkList.CssClass = GetLocalResourceObject("TabInactive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                    ModifiedDatePnl.Visible = true;
                }
                else if (EntryStatus == EntryStatus.LISTMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowListing", "$(document).ready(function(){ShowListing(1);});", true);
                    lnkList.CssClass = GetLocalResourceObject("TabActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabInactive").ToString();
                    ModifiedDatePnl.Visible = false;
                }

                if (EntryStatusHdr == EntryStatus.VIEWMODE)
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewModeHdr", "$(document).ready(function(){ViewModeHdr(1);});", true);
                if (ParentPage == ControlParentPage.EmpDoc)
                {
                    pnlSaveContinue.Visible = true;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
            currentUser = (BusinessObject.User)Context.User.Identity;
            if (!IsPostBack)
            {
                hdfCurrentDepartment.Value = currentUser.CurrentDeptPK.ToString();
            }
        }

        /// <summary>
        /// Method used to Handle all actions in the page with GridViewSort
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewSortEventArgs e)
        {
            try
            {
                if (SortBy == e.SortExpression)
                {
                    //Toggle the sort expression
                    if (SortDirection == Resources.ErpRes.SortAscending)
                        SortDirection = Resources.ErpRes.SortDescending;
                    else
                        SortDirection = Resources.ErpRes.SortAscending;
                }
                else
                {
                    SortBy = e.SortExpression;
                    SortDirection = Resources.ErpRes.SortAscending;
                }

                //this.PageIndex = CommonConstants.SELECT_VALUE_ONE;
                GetFieldValues(ControlsEnum.EMPDOCLIST);
                SetFieldValues(ControlsEnum.EMPDOCLIST);
                EntryStatus = EntryStatus.LISTMODE;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        protected void ActionHandler(object sender, EventArgs e)
        {
            if (!ValidatePageDept())
                return;
            try
            {
                int result;
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((LinkButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(RadioButton)))
                {
                    if (((RadioButton)sender).ID == "rbtSelect")
                    {
                        commonActions = ActionsEnum.ITEMSELECTED;
                    }
                }
                else if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
                }
                switch (commonActions)
                {
                    case ActionsEnum.DEFAULT:
                        GetFieldValues(ControlsEnum.DOCTYPELIST);
                        break;
                    #region ITEMSELECTED
                    case ActionsEnum.ITEMSELECTED:
                        bool bIsChecked = false;
                        foreach (GridViewRow grdrow in grdEmpDocList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;// by Biju
                                this.SelectedPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfDocPk")).Value);
                                break;
                            }
                        }
                        if (!bIsChecked)
                        {
                            throw new ApplicationException(GetLocalResourceObject("ItemNotSelected").ToString());
                        }
                        //litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(Resources.Report.Msg_Not_Selected) + "','" + Resources.Messages.Information + "');", true);
                        //Biju//ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowSelectedRow", "$(document).ready(function(){SetSelectedRowColor();});", true);
                        break;
                    #endregion
                    case ActionsEnum.EMPDOCLIST:
                        BLUploadList = null;
                        blUploadObj = null;
                        FileDetailsList = null;
                        GetFieldValues(ControlsEnum.EMPDOCLIST);
                        SetFieldValues(ControlsEnum.EMPDOCLIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    case ActionsEnum.NEW:
                        this.EntryStatus = EntryStatus.NEWMODE;
                        SetUIEditView(ActionsEnum.NEW);
                        ddlDocumentTypeList.Focus();
                        break;
                    case ActionsEnum.VIEW:
                        if (this.SelectedPK > 0)
                        {
                            hdfSelectedItemPk.Value = this.SelectedPK.ToString();
                            EntryStatus = EntryStatus.VIEWMODE;
                            SetUIEditView(ActionsEnum.VIEW);
                            break;
                        }
                        else
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(Resources.Report.Msg_Not_Selected) + "','" + Resources.Messages.Information + "');", true);
                            break;
                        }
                    case ActionsEnum.GOHOME:
                        Response.Redirect(Resources.PageURL.EmployeeList);
                        break;
                    case ActionsEnum.EDIT:
                    case ActionsEnum.EMPDOCDETAIL:
                        if (this.SelectedPK > 0)
                        {
                            hdfSelectedItemPk.Value = this.SelectedPK.ToString();
                            EntryStatus = EntryStatus.EDITMODE;
                            SetUIEditView(ActionsEnum.EDIT);
                            ddlDocumentTypeList.Focus();
                            break;
                        }
                        else
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(Resources.Report.Msg_Not_Selected) + "','" + Resources.Messages.Information + "');", true);
                            break;
                        }
                    #region Cancel
                    case ActionsEnum.CANCEL:
                        BLUploadList = null;
                        blUploadObj = null;
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion
                    #region Save
                    case ActionsEnum.SAVE:
                        string empName = string.Empty;
                        selectedEmployeeDoc = (EmployeeDoc)SetUiValuesToObject(commonActions);
                        result = EmployeeDocBL.Save(selectedEmployeeDoc, out empName);
                        if (result >= 0) // Success ! re-initialize the page
                        {
                            string savePath = string.Empty;
                            hdfIscontYes.Value = CommonConstants.SELECT_VALUE_ZERO;
                            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                            {
                                savePath = HttpContext.Current.Request.PhysicalApplicationPath + @"\Upload\HR\EmpDocs";
                                if (!Directory.Exists(savePath))
                                    Directory.CreateDirectory(savePath);
                                savePath = HttpContext.Current.Request.PhysicalApplicationPath + @"\Upload\HR\EmpDocs\";
                            }
                            else
                            {
                                savePath = string.Format(@"{0}HR\EmpDocs\", System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower());
                                //savePath = string.Format(@"{0}HR/EmpDocs/", System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower());
                            }
                            if (BLUploadList != null)
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
                            ResetForm(ActionsEnum.SAVE);

                            GetFieldValues(ControlsEnum.UPLOADEDFILES);
                            SetFieldValues(ControlsEnum.UPLOADEDFILES);

                            EntryStatus = EntryStatus.LISTMODE;

                            litErrorMsg.Text = string.Format(Resources.ErrorMessages.Msg_Save_Success, Resources.PageNameRes.Documents);
                            //litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.); // By Biju
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                      + "','" + Resources.ErrorMessages.Msg_Heading_Information + "');", true);
                        }
                        else
                        {
                            #region Db Error Checking
                            if (result == (int)DbSaveStatus.SQLERROR)
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CONCURRENCY)
                            {
                                litErrorMsg.Text = Resources.Messages.EditUsedByAnotherUser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CODEEXIST)
                            {
                                litErrorMsg.Text = Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.ALREADYEXIST)
                            {
                                litErrorMsg.Text = string.Format(GetLocalResourceObject("MsgDocExist").ToString(), empName);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowConfirmMsg", "$(document).ready(function(){ShowConfirmDocDetails('" + litErrorMsg.Text + "');});", true);
                                return;
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.Documents);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            #endregion
                        }
                        break;
                    #endregion
                    #region Save and Continue
                    case ActionsEnum.SAVEANDCONTINUE:
                        string emploName = string.Empty;
                        selectedEmployeeDoc = (EmployeeDoc)SetUiValuesToObject(commonActions);
                        result = EmployeeDocBL.Save(selectedEmployeeDoc, out emploName);
                        if (result >= 0) // Success ! re-initialize the page
                        {
                            string savePath = string.Empty;
                            hdfIscontYes.Value = CommonConstants.SELECT_VALUE_ZERO;
                            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                            {
                                savePath = HttpContext.Current.Request.PhysicalApplicationPath + @"\Upload\HR\EmpDocs";
                                if (!Directory.Exists(savePath))
                                    Directory.CreateDirectory(savePath);
                                savePath = HttpContext.Current.Request.PhysicalApplicationPath + @"\Upload\HR\EmpDocs\";
                            }
                            else
                            {
                                savePath = string.Format(@"{0}HR\EmpDocs\", System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower());
                                //savePath = string.Format(@"{0}HR/EmpDocs/", System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower());
                            }
                            if (BLUploadList != null)
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
                           // ResetForm(ActionsEnum.SAVE);

                          //  GetFieldValues(ControlsEnum.UPLOADEDFILES);
                           // SetFieldValues(ControlsEnum.UPLOADEDFILES);

                            //EntryStatus = EntryStatus.LISTMODE;

                            litErrorMsg.Text = string.Format(Resources.ErrorMessages.Msg_Save_Success, Resources.PageNameRes.Documents);
                            CommonBL userAuth = new CommonBL();
                            string nextPageUrl = BusinessLogic.HRMS.Common.HRMSCommonBL.GetNextTabUrl(Convert.ToInt16(EmpTabEnum.Documents), GetGlobalResourceObject("ConfigurationsRes", "HrmsEmpTabNextURL").ToString().Split(','),
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
                            //litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.); // By Biju
                            ///ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                     // + "','" + Resources.ErrorMessages.Msg_Heading_Information + "');", true);
                        }
                        else
                        {
                            #region Db Error Checking
                            if (result == (int)DbSaveStatus.SQLERROR)
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CONCURRENCY)
                            {
                                litErrorMsg.Text = Resources.Messages.EditUsedByAnotherUser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CODEEXIST)
                            {
                                litErrorMsg.Text = Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.ALREADYEXIST)
                            {
                                litErrorMsg.Text = string.Format(GetLocalResourceObject("MsgDocExist").ToString(), emploName);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowConfirmMsg", "$(document).ready(function(){ShowConfirmDocDetails('" + litErrorMsg.Text + "');});", true);
                                return;
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.Documents);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            #endregion
                        }
                        break;
                    #endregion
                    #region Delete
                    case ActionsEnum.DELETE:
                        if (this.SelectedPK != 0)
                        {
                            result = EmployeeDocBL.DeleteEmployeeDoc(SelectedPK, this.LastModifiedTime);
                            if (result > 0)
                            {
                                BLUploadList = null;
                                blUploadObj = null;
                                FileDetailsList = null;
                                GetFieldValues(ControlsEnum.EMPDOCLIST);
                                SetFieldValues(ControlsEnum.EMPDOCLIST);
                                EntryStatus = EntryStatus.LISTMODE;
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), this.GetLocalResourceObject("EmpDoc").ToString());
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "');", true);
                            }
                            else
                            {
                                #region Error Message
                                if (result == (int)DbSaveStatus.SQLERROR)
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                }
                                else if (result == (int)DbSaveStatus.CONCURRENCY)
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("EmpDoc").ToString() + " " + Resources.Messages.EditUsedByAnotherUser;
                                }
                                else if (result == (int)DbSaveStatus.REFNOEXIST)
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("EmpDoc").ToString() + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                }
                                else if (result == (int)DbDeleteStatus.REFERRED)
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("EmpDoc").ToString() + " " + Resources.Messages.UsedInAnotherPlace;
                                }
                                else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("EmpDoc").ToString() + " " + Resources.Messages.AlreadyDeleted;
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                }
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                #endregion
                            }
                        }
                        break;
                    #endregion
                    #region ADDITEM
                    case ActionsEnum.ADDITEM:
                        //validate Page
                       // if (!IsValid)
                       // {
                       //     litErrorMsg.Text = Resources.ErrorMessages.Msg_SavError;
                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                       // }
                       // else//valid
                       // {

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

                                            //if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower()))
                                            //{
                                            //    blUploadObj.DOC_PATH = @"\Upload\HR\EmpDocs\" + attachmentFileName;
                                            //}
                                            //else
                                            //{
                                            //    blUploadObj.DOC_PATH = System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower() + "HR/EmpDocs/" + attachmentFileName;
                                            //}
                                            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower()))
                                            {
                                                blUploadObj.DOC_PATH = "~/Upload/HR/EmpDocs/" + attachmentFileName;
                                            }
                                            else
                                            {
                                                blUploadObj.DOC_PATH = System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower() + "HR/EmpDocs/" + attachmentFileName;
                                            }
                                            FileDetails fileDetailsObj = FileDetailsList.SingleOrDefault(aa => aa.SlNo == CurrSlNo);
                                            if (fileDetailsObj == null)
                                            {
                                                FileDetailsList.Add(new FileDetails() { SlNo = CurrSlNo, EmpDocFile = (HttpContext.Current.Request.Files.Count > 1 ? HttpContext.Current.Request.Files[1] : HttpContext.Current.Request.Files[0]) });
                                            }
                                            else
                                            {
                                                fileDetailsObj.EmpDocFile = (HttpContext.Current.Request.Files.Count > 1 ? HttpContext.Current.Request.Files[1] : HttpContext.Current.Request.Files[0]);
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
                                    //string SavePath = string.Empty;
                                    //if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower()))
                                    //{
                                    //    SavePath = HttpContext.Current.Request.PhysicalApplicationPath + "Upload";
                                    //}
                                    //else
                                    //{
                                    //    //SavePath = Server.MapPath("../Upload");
                                    //    SavePath = System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower();
                                    //}
                                    tempFileInfoObj = new FileInfo(fupUpload.PostedFile.FileName);
                                    string attachmentFileFormat = tempFileInfoObj.Extension;
                                    string attachmentFileName = Guid.NewGuid().ToString() + attachmentFileFormat;

                                    blUploadObj.DOC_TYPE = tempFileInfoObj.Extension;
                                    blUploadObj.DOC_NAME = fupUpload.FileName;
                                    blUploadObj.DOC_TITLE = attachmentFileName;
                                    blUploadObj.AttachmentFileName = attachmentFileName;

                                    //if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower()))
                                    //{
                                    //    blUploadObj.DOC_PATH = @"\Upload\HR\EmpDocs\" + attachmentFileName;
                                    //}
                                    //else
                                    //{
                                    //    //SavePath = Server.MapPath("../Upload");
                                    //    //blUploadObj.SCD_FILE_PATH = "~/Upload/" + attachmentFileName;
                                    //    blUploadObj.DOC_PATH = System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower() + "HR/EmpDocs/" + attachmentFileName;
                                    //}
                                    if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower()))
                                    {
                                        blUploadObj.DOC_PATH = "~/Upload/HR/EmpDocs/" + attachmentFileName;
                                    }
                                    else
                                    {
                                        //SavePath = Server.MapPath("../Upload");
                                        //blUploadObj.SCD_FILE_PATH = "~/Upload/" + attachmentFileName;
                                        blUploadObj.DOC_PATH = System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower() + "HR/EmpDocs/" + attachmentFileName;
                                    }

                                    // blUploadObj.SCD_FILE_PATH = "~/Upload/" + attachmentFileName;
                                    blUploadObj.DOC_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                                    FileDetailsList.Add(new FileDetails() { SlNo = slno, EmpDocFile = (HttpContext.Current.Request.Files.Count > 1 ? HttpContext.Current.Request.Files[1] : HttpContext.Current.Request.Files[0]) });
                                    BLUploadList.Add(blUploadObj);
                                }
                            }
                            SetFieldValues(ControlsEnum.UPLOADEDFILES);
                            //EntryStatus = EntryStatus.NEWMODE;
                            ResetForm(ActionsEnum.ADDITEM);
                        //}
                         if (AfterPostback != null)
                             AfterPostback(this, EventArgs.Empty);
                        break;
                    #endregion
                    #region REMOVEITEM
                    case ActionsEnum.REMOVEITEM:
                        if (BLUploadList != null && BLUploadList.Count > 0)
                        {
                            int SlNo = Convert.ToInt32(grdUploads.DataKeys[(((Button)sender).Parent.Parent as GridViewRow).RowIndex][0]);
                            if (SlNo > 0)
                            {
                                BLUploadList = BLUploadList.Where(row => SlNo != row.DOC_SEQ_NO).ToList();
                                FileDetailsList = null;// Sequence Contains more than Error
                                SetFieldValues(ControlsEnum.UPLOADEDFILES);
                                ResetForm(ActionsEnum.ADDITEM);
                            }
                        }
                        if (AfterPostback != null)
                            AfterPostback(this, EventArgs.Empty);
                        break;
                    #endregion
                    #region EDITITEM
                    case ActionsEnum.EDITITEM:
                        if (BLUploadList != null && BLUploadList.Count > 0)
                        {
                            int SlNo = Convert.ToInt32(grdUploads.DataKeys[(((Button)sender).Parent.Parent as GridViewRow).RowIndex][0]);
                            if (SlNo > 0)
                            {
                                blUploadObj = BLUploadList.SingleOrDefault(row => SlNo == row.DOC_SEQ_NO);
                                GetUIValuesFromObject(ControlsEnum.SELECTEDDOC);
                            }
                        }
                        break;
                    #endregion
                    #region Show Log
                    case ActionsEnum.SHOWLOG:
                        //this.SelectedDocPk = Convert.ToInt32(((HiddenField)((ImageButton)sender).Parent.Parent.FindControl("hdfDocPk")).Value);
                        this.SelectedPK = Convert.ToInt32(((ImageButton)sender).CommandArgument);
                        GetFieldValues(ControlsEnum.LOGLIST);
                        SetFieldValues(ControlsEnum.LOGLIST);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divCheckInCheckOutLog]','" + Resources.PageNameRes.Log + "','800','500');", true);
                        break;
                    #endregion
                    #region Check In
                    case ActionsEnum.CHECKIN:
                        if (this.EntryStatus == BusinessObject.Common.EntryStatus.LISTMODE)
                        {
                            this.SelectedPK = Convert.ToInt32(((ImageButton)sender).CommandArgument);
                        }

                        GetFieldValues(ControlsEnum.CHECKIN);
                        SetFieldValues(ControlsEnum.CHECKIN);

                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divCheckInControlContainer]','" + Resources.PageNameRes.CheckIn + "','900','520');", true);
                        break;
                    #endregion
                    #region Check Out
                    case ActionsEnum.CHECKOUT:
                        if (this.EntryStatus == BusinessObject.Common.EntryStatus.LISTMODE)
                        {
                            this.SelectedPK = Convert.ToInt32(((ImageButton)sender).CommandArgument);
                        }
                        GetFieldValues(ControlsEnum.CHECKOUT);
                        SetFieldValues(ControlsEnum.CHECKOUT);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divCheckOutControlContainer]','" + Resources.PageNameRes.CheckOut + "','900','520');", true);
                        break;
                    #endregion
                    #region CHECKINLOGCLOSE
                    case ActionsEnum.CHECKINLOGCLOSE:
                        GetFieldValues(ControlsEnum.CHECKIN);
                        SetFieldValues(ControlsEnum.CHECKIN);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divCheckInControlContainer]','" + Resources.PageNameRes.CheckIn + "','900','520');", true);
                        break; 
                    #endregion
                    #region CHECKOUTLOGCLOSE
                    case ActionsEnum.CHECKOUTLOGCLOSE:
                        GetFieldValues(ControlsEnum.CHECKOUT);
                        SetFieldValues(ControlsEnum.CHECKOUT);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divCheckOutControlContainer]','" + Resources.PageNameRes.CheckOut + "','900','520');", true);
                        break; 
                    #endregion
                    #region CHECKOUTERROR
                    case ActionsEnum.CHECKOUTERROR:
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divCheckOutControlContainer]','" + Resources.PageNameRes.CheckOut + "','900','520');", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + ((Button)sender).CommandArgument + "','" + Resources.ErpRes.Information + "');", true);
                        break; 
                    #endregion
                    #region CHECKINERROR
                    case ActionsEnum.CHECKINERROR:
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divCheckOutControlContainer]','" + Resources.PageNameRes.CheckOut + "','900','520');", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + ((Button)sender).CommandArgument + "','" + Resources.ErpRes.Information + "');", true);
                        break; 
                    #endregion
                    #region APPLY
                    case ActionsEnum.APPLY:
                        EmpDocDetails = (EmployeeDoc)SetUiValuesToObject(ActionsEnum.SAVE);
                        if (AfterApply != null)
                            AfterApply(this, EventArgs.Empty);
                        break;
                    #endregion
                    #region POPUPCANCEL
                    case ActionsEnum.POPUPCANCEL:
                        clearControls();
                        if (AfterApply != null)
                            AfterApply(this, EventArgs.Empty);
                        break;
                    #endregion
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex)
                                          + "','" + Resources.ErpRes.Information + "');", true);
            }
        }

        public void SaveAttachments()
        {
            string savePath = string.Empty;
            hdfIscontYes.Value = CommonConstants.SELECT_VALUE_ZERO;
            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
            {
                savePath = HttpContext.Current.Request.PhysicalApplicationPath + @"\Upload\HR\EmpDocs";
                if (!Directory.Exists(savePath))
                    Directory.CreateDirectory(savePath);
                savePath = HttpContext.Current.Request.PhysicalApplicationPath + @"\Upload\HR\EmpDocs\";
            }
            else
            {
                savePath = string.Format(@"{0}HR\EmpDocs\", System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower());
                //savePath = string.Format(@"{0}HR/EmpDocs/", System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower());
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
            }
            clearControls();
            BLUploadList = null;
            FileDetailsList = null;
        }
        #region GridView Action Handler
        /// <summary>
        /// Method used to Handle all actions in the page with GridView Row Bindinw
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            int slno;
            try
            {
                #region grdUploads
                if (((GridView)sender).ID == "grdUploads")
                {
                    if (EntryStatus == EntryStatus.VIEWMODE)
                    {
                        if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
                        {
                            //e.Row.Cells[4].Visible = false;
                            e.Row.Cells[3].Visible = false;
                            e.Row.Cells[4].Visible = false;
                        }
                    }
                    else if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        slno = Convert.ToInt32(grdUploads.DataKeys[e.Row.RowIndex][0]);
                        if (slno > 0)
                        {
                            e.Row.FindControl("fileView").Visible = FileDetailsList == null || FileDetailsList.Where(fle => fle.SlNo == slno).Count() == 0;
                        }
                    }
                }
                #endregion
                else if (((GridView)sender).ID == "grdEmpDocList")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        HiddenField hdf = (HiddenField)e.Row.FindControl("hdfIsCheckedIn");
                        ImageButton lnkChkdOut = (ImageButton)e.Row.FindControl("lnkCheckOut");
                        ImageButton lnkChkdIn = (ImageButton)e.Row.FindControl("lnkCheckIn");
                        if (lnkChkdOut != null && lnkChkdIn != null && hdf != null)
                        {
                            sbyte isCheckedIn = Convert.ToSByte(hdf.Value.Trim());

                            if (isCheckedIn == (short)CheckStateStatus.CheckedIn)
                            {
                                lnkChkdOut.Visible = true;
                                lnkChkdIn.Visible = false;
                            }
                            else if (isCheckedIn == (short)CheckStateStatus.CheckedOut)
                            {
                                lnkChkdIn.Visible = true;
                                lnkChkdOut.Visible = false;
                            }
                            else if (isCheckedIn == (short)CheckStateStatus.OriginalNotSubmitted)
                            {
                                lnkChkdIn.Visible = false;
                                lnkChkdOut.Visible = false;
                            }
                        }

                        HtmlGenericControl imbStatusIndicator = (HtmlGenericControl)e.Row.FindControl("imbStatusIndicator");
                        HiddenField hdf2 = (HiddenField)e.Row.FindControl("hdfStatus");
                        if (hdf2 != null)
                        {
                            string _statusText = hdf2.Value;
                            _statusText = _statusText.Trim();
                            string _toolTipText = string.Empty;
                            imbStatusIndicator.Attributes.Add("class", GetListImage(_statusText, out _toolTipText));
                            if (!_toolTipText.IsNullOrEmptyOrWhitespace()) imbStatusIndicator.Attributes.Add("tooltip", _toolTipText);
                        }
                    }
                }
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
        #endregion

        protected void ResetForm(ActionsEnum action)
        {
            switch (action)
            {
                case ActionsEnum.ADDITEM:
                    anchorFile.Visible = false;
                    vrfFileUpload.Enabled = true;
                    CurrSlNo = 0;
                    anchorFile.Attributes.Remove("onclick");
                    break;
                case ActionsEnum.SAVE:
                    BLUploadList = null;
                    FileDetailsList = null;
                    ResetForm(ActionsEnum.EMPDOCLIST);
                    ResetForm(ActionsEnum.ADDITEM);
                    break;
                case ActionsEnum.EMPDOCLIST:
                    BLUploadList = null;
                    FileDetailsList = null;
                    GetFieldValues(ControlsEnum.EMPDOCLIST);
                    SetFieldValues(ControlsEnum.EMPDOCLIST);
                    break;
                case ActionsEnum.CHECKIN:
                    if (EntryStatus == BusinessObject.Common.EntryStatus.EDITMODE)
                    {
                        GetFieldValues(ControlsEnum.SELECTEDEMPDOC);
                        SetFieldValues(ControlsEnum.SELECTEDEMPDOC);
                    }
                    else
                    {
                        ResetForm(ActionsEnum.EMPDOCLIST);
                    }
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_DocCheckedIn").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                    break;
                case ActionsEnum.CHECKOUT:
                    if (EntryStatus == BusinessObject.Common.EntryStatus.EDITMODE)
                    {
                        GetFieldValues(ControlsEnum.SELECTEDEMPDOC);
                        SetFieldValues(ControlsEnum.SELECTEDEMPDOC);
                    }
                    else
                    {
                        ResetForm(ActionsEnum.EMPDOCLIST);
                    }
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_DocCheckedOut").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                    break;
                case ActionsEnum.NEW:
                    clearControls();
                    break;
            }
        }

        protected void btnAction_PreRender(object sender, EventArgs e)
        {
            try
            {
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((LinkButton)sender).CommandName));
                }

                switch (commonActions)
                {
                    case ActionsEnum.CHECKIN:
                        break;
                    case ActionsEnum.CHECKOUT:
                        break;
                }
            }
            catch (Exception)
            {
            }
        }

        //protected void txtDocumentExpiresOn_Changed(object sender, EventArgs e)
        //{
        //    DateTime dummyDate;
        //    if (DateTime.TryParse(txtDocumentExpiresOn.Text.Trim(), out dummyDate))
        //    {
        //        int daysLeft = GetDaysLeft(dummyDate);
        //        lblExpiryDaysLeft.Text = daysLeft >= 0
        //                    ? string.Format(GetLocalResourceObject("OverdueDays").ToString(), daysLeft)
        //                    : string.Format(GetLocalResourceObject("DaysLeftFomat").ToString(), daysLeft);
        //        return;
        //    }
        //    // Handle Exception
        //}

        protected string GetListImage(string str, out string toolTip)
        {
            var result = "";
            toolTip = string.Empty;

            if (str == "0")
            {
                //Red
                result = "red-icon";
                toolTip = "Expired";
            }
            if (str == "1")
            {
                //Orange
                result = "orange-icon";
                toolTip = "Near To Expire";
            }
            if (str == "2")
            {
                //Green
                result = "green-icon";
            }
            return result;
        }

        public void clearControls()
        {
            if (ddlDocumentTypeList.Items.Count > 0) ddlDocumentTypeList.SelectedIndex = 0;

            // Clear Session
            this.SelectedPK = 0;
            hdfSelectedItemPk.Value = this.SelectedPK.ToString();

            txtDocumentNo.Text = string.Empty;
            txtPlaceOfIssue.Text = string.Empty;
            txtDocRefNo.Text = string.Empty;
            txtEmpDocIssuedBy.Text = string.Empty;
            txtDocumentIssuedOn.Text = string.Empty;
            txtDocumentExpiresOn.Text = string.Empty;
            lblExpiryDaysLeft.Text = string.Empty;
            txtDocumentRefDate.Text = string.Empty;
            txtDocumentTitle.Text = string.Empty;
            txtDocumetAddlInfo.Text = string.Empty;
            txtDocumentRemarks.Text = string.Empty;
            txtIntimateBefore.Text = string.Empty;
            txtCheckedInOn.Text = DateTime.Now.ToString(Resources.Constants.HRMSDateFormatShort);
            chkOriginalSubmitted.Checked = false;

            txtAutoCheckedInBy.Text = currentUser.EmpName;
            hdfAutoCheckedInBy.Value = currentUser.PKEmployee.ToString();

            chkOriginalSubmitted.Enabled = true;
            btnCheckIn.Visible = false;
            btnCheckOut.Visible = false;

            this.BLUploadList = null;
            this.blUploadObj = null;

            grdUploads.DataSource = null;
            grdUploads.DataBind();

            grdCheckIncheckOutLog.DataSource = null;
            grdCheckIncheckOutLog.DataBind();

            EmpDocDetails = null;
            lblExpiryDaysLeft.Text = string.Empty;
            hdfExpiryDaysLeft.Value = string.Empty;
        }

        private object BindSelectedEmployeeDoc()
        {
            try
            {
                EmployeeDoc retObj = new EmployeeDoc();

                //=========================
                //this.CurrPK = 1071; //Error
                //============================

                DateTime dummyDateField;
                int dummyIntField;

                retObj.EmpPK = this.CurrPK;
                retObj.DocPk = this.SelectedPK;
                retObj.DocNo = txtDocumentNo.Text.Trim();

                retObj.DocTypePK = Convert.ToInt32(ddlDocumentTypeList.SelectedValue); //*
                retObj.DocTypeText = ddlDocumentTypeList.Text; //*

                retObj.Active = (short)DbActiveStatus.ACTIVE;
                retObj.AdditionalInfo = txtDocumetAddlInfo.Text.Trim();

                //Unique Docment Check Pending  ( DocType & Issued By & Doc No )           

                if (Int32.TryParse(hdfAutoCheckedInBy.Value.ToString().Trim(), out dummyIntField) && dummyIntField > 0)
                    retObj.CheckedInBy = dummyIntField.ToString();
                else if (!hdfAutoCheckedInBy.Value.ToString().Trim().IsNullOrEmptyOrWhitespace()) throw new ApplicationException(GetLocalResourceObject("InvalidChkdInby").ToString());

                if (DateTime.TryParse(txtCheckedInOn.Text.Trim(), out dummyDateField))
                    retObj.CheckedInOn = dummyDateField.ToString();
                else if (txtCheckedInOn.Text.Trim().Length > 0) throw new ApplicationException(GetLocalResourceObject("InvalidChkdInDate").ToString());

                DateTime dob = DateTime.Parse(hdfHdrDOB.Value.Trim());
                if (!string.IsNullOrEmpty(txtDocumentIssuedOn.Text.Trim()))
                    if ( DateTime.Parse(txtDocumentIssuedOn.Text.Trim()) > dob)
                        retObj.IssuedOn = txtDocumentIssuedOn.Text.Trim(); //*
                    else throw new ApplicationException(GetLocalResourceObject("IssueDateCompare").ToString());
                //else throw new ApplicationException(GetLocalResourceObject("InvalidIssueDate").ToString());

                if (!string.IsNullOrEmpty(txtDocumentIssuedOn.Text.Trim()))
                {
                    if (DateTime.TryParse(txtDocumentExpiresOn.Text.Trim(), out dummyDateField))
                        if (!string.IsNullOrEmpty(txtDocumentIssuedOn.Text.Trim()))
                        {
                            if (dummyDateField > DateTime.Parse(retObj.IssuedOn))
                                retObj.ExpiresOn = txtDocumentExpiresOn.Text.Trim();
                        }
                        else
                            retObj.ExpiresOn = txtDocumentExpiresOn.Text.Trim();
                    else throw new ApplicationException(GetLocalResourceObject("ExpiryDateCompare").ToString());
                }
                //else throw new ApplicationException("Expires on date is not valid");

                if (DateTime.TryParse(txtDocumentRefDate.Text.Trim(), out dummyDateField))
                    retObj.ReferenceDate = dummyDateField.ToString();
                else if (txtDocumentRefDate.Text.Trim().Length > 0) throw new ApplicationException(GetLocalResourceObject("InvaldRefDate").ToString());

                if (Int32.TryParse(txtIntimateBefore.Text.Trim(), out dummyIntField) && dummyIntField > 0)
                    retObj.InitimateBefore = dummyIntField;
                else if (txtIntimateBefore.Text.Trim().Length > 0) throw new ApplicationException(GetLocalResourceObject("InvaldIntimateBfDate").ToString());

                if (!string.IsNullOrEmpty(retObj.ExpiresOn))
                    retObj.DaysLeft = GetDaysLeft(Convert.ToDateTime(retObj.ExpiresOn));

                retObj.IsCheckedIn = (short)CheckStateStatus.CheckedIn;
                retObj.IssuedBy = txtEmpDocIssuedBy.Text; //*
                retObj.OriginalSubmitted = (short)(chkOriginalSubmitted.Checked ? CommonStatus.Active : CommonStatus.Inactive);
                retObj.PalceOfIssue = txtPlaceOfIssue.Text.Trim();
                retObj.ReferenceNo = txtDocRefNo.Text.Trim();
                retObj.Remarks = txtDocumentRemarks.Text.TrimEnd();
                retObj.Title = txtDocumentTitle.Text.TrimEnd();
                retObj.LastModifiedDate = DateTime.Now;
                retObj.DocActive = (short)CommonStatus.Active;
                retObj.User = currentUser.PKUser;
                retObj.DocDetails = this.BLUploadList;
                retObj.BizUnit = currentUser.SBUID;
                retObj.DOC_EXIST = Convert.ToInt16(hdfIscontYes.Value);

                return retObj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private int GetDaysLeft(DateTime expiryDate)
        {
            if (expiryDate == null) throw new ArgumentNullException("expiryDate", GetLocalResourceObject("Err_Expdate_Null").ToString());
            if (expiryDate < DateTime.Now) return ((TimeSpan)(expiryDate - DateTime.Now)).Days;
            return ((TimeSpan)(expiryDate - DateTime.Now)).Days + 1;
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

        #region Common UI Methods
        /// <summary>
        /// Getting Data From Db To Fields
        /// </summary>
        /// <param name="controlsEnum">Field</param>
        private void GetFieldValues(ControlsEnum controlsEnum)
        {
            switch (controlsEnum)
            {
                case ControlsEnum.EMPDOCLIST:
                    pageData = EmployeeDocBL.GetEmployeeDocs(this.CurrPK, currentUser.SBUID, SortBy, SortDirection);
                    break;
                case ControlsEnum.SELECTEDEMPDOC:
                    GetFieldValues(ControlsEnum.DOCTYPELIST);
                    //GetFieldValues(ControlsEnum.EMPLOYEEAUTOCOMPLETE);
                    //Files are handled in GetUIValuesFromObject
                    selectedEmployeeDoc = EmployeeDocBL.GetEmployeeDocByDocID(this.SelectedPK, (int)DbActiveStatus.ACTIVE);
                    break;
                case ControlsEnum.DOCTYPELIST:
                    pageData = EmployeeDocBL.GetEmployeeDocTypeList(GTIService.Constants.HRMS.Employee.Constatnts.EMP_DOC_TYPE);
                    BindDropDown(controlsEnum);
                    break;
                #region UPLOADEDFILES
                case ControlsEnum.UPLOADEDFILES:
                    if (SelectedPK > 0)
                    {
                        selectedEmployeeDoc = EmployeeDocBL.GetEmployeeDocByDocID(SelectedPK, (int)DbActiveStatus.ACTIVE);
                    }
                    break;
                #endregion
                case ControlsEnum.LOGLIST:
                    pageData = EmployeeDocBL.GetEmployeeDocsLog(this.SelectedPK);
                    break;
                case ControlsEnum.CHECKIN:
                    this.EmployeeDocumentDetailsList = EmployeeDocBL.GetEmpDocByFilterOptions(new EmpDocumentFilterParameterBinder
                    {
                        DocPk = this.SelectedPK
                    });
                    break;
                case ControlsEnum.CHECKOUT:
                    this.EmployeeDocumentDetailsList = EmployeeDocBL.GetEmpDocByFilterOptions(new EmpDocumentFilterParameterBinder
                    {
                        DocPk = this.SelectedPK
                    });
                    break;
                case ControlsEnum.EMPLOYEEDETAILSHEADER:
                    int hasPK = 2;
                    dtemployeeHeader = EmployeeBasicInfoBL.GetEmployeeDetailListHeader(CurrPK, string.Empty, hasPK);
                    objEmployeeBasicInfo = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.GetEmployeebyID(CurrPK);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Used To Setting UI
        /// Calling BindGrid,BindDropDown,GetUIValuesToObject Methods From Here
        /// </summary>
        /// <param name="controlType"></param>
        private void SetFieldValues(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.EMPDOCLIST:
                        BindGrid(controlType);
                        break;
                    case ControlsEnum.DOCTYPELIST:
                        BindDropDown(ControlsEnum.DOCTYPELIST);
                        break;
                    case ControlsEnum.SELECTEDEMPDOC:
                        SetFieldValues(ControlsEnum.DOCTYPELIST);
                        //SetFieldValues(ControlsEnum.EMPLOYEEAUTOCOMPLETE);
                        GetUIValuesFromObject(ControlsEnum.SELECTEDEMPDOC);
                        if (this.selectedEmployeeDoc != null)
                        {
                            this.BLUploadList = this.selectedEmployeeDoc.DocDetails;
                            BindGrid(ControlsEnum.UPLOADEDFILES);
                        }
                        break;
                    case ControlsEnum.UPLOADEDFILES:
                        if (selectedEmployeeDoc != null)
                            GetUIValuesFromObject(ControlsEnum.UPLOADEDFILES);
                        BindGrid(ControlsEnum.UPLOADEDFILES);
                        break;
                    case ControlsEnum.LOGLIST:
                        BindGrid(ControlsEnum.LOGLIST);
                        break;
                    case ControlsEnum.CHECKIN:
                        BindUserControl(controlType);
                        break;
                    case ControlsEnum.CHECKOUT:
                        BindUserControl(controlType);
                        break;
                    case ControlsEnum.EMPLOYEEDETAILSHEADER:
                        GetUIValuesFromObject(controlType);
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void BindUserControl(ControlsEnum controlType)
        {
            switch (controlType)
            {
                case ControlsEnum.CHECKOUT:
                    CheckOutControl1.SelectedDocItems = this.EmployeeDocumentDetailsList;
                    CheckOutControl1.SetProperties();
                    break;
                case ControlsEnum.CHECKIN:
                    CheckInControl1.SelectedDocItems = this.EmployeeDocumentDetailsList;
                    CheckInControl1.SetProperties();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Used To Setting UI Components like TextBox
        /// </summary>
        /// <param name="controlsEnum"></param>
        private void GetUIValuesFromObject(ControlsEnum controlsEnum)
        {
            switch (controlsEnum)
            {
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
                case ControlsEnum.SELECTEDEMPDOC:
                    BindDetailsView();
                    break;
                case ControlsEnum.UPLOADEDFILES:
                    SelectedPK = selectedEmployeeDoc.DocPk;
                    LastModifiedTime = selectedEmployeeDoc.LastModifiedDate;
                    lblLastModifiedHDR.Text = Resources.Report.LastModifiedOn + LastModifiedTime.ToString(Resources.Report.LastModifiedDatetimeFormat);
                    //BLUploadList = selectedEmployeeDoc.UPloaded Doc List
                    break;
                case ControlsEnum.EMPLOYEEDETAILSHEADER:
                    if (dtemployeeHeader != null && dtemployeeHeader.Rows.Count > 0)
                    {
                        //lblhdrEmployeeNoTxt.Text = CommonFunctions.GetShortString(HttpUtility.HtmlDecode(dtemployeeHeader.Rows[0]["empCode"].ToString()), 20);
                        //lblhdrEmployeeNameTxt.Text = CommonFunctions.GetShortString(HttpUtility.HtmlDecode(dtemployeeHeader.Rows[0]["empNameText"].ToString()), 20);                        
                        //lblhdrDOJText.Text = dtemployeeHeader.Rows[0]["empDOJText"].ToString();
                        //lblhdrDOBTxt.Text = dtemployeeHeader.Rows[0]["empDOBText"].ToString();
                        //lblhdrDesignationTxt.Text =  HttpUtility.HtmlDecode(dtemployeeHeader.Rows[0]["empDesignationText"].ToString());
                        //lblhdrDepartmentTxt.Text = CommonFunctions.GetShortString(HttpUtility.HtmlDecode(dtemployeeHeader.Rows[0]["empDepartmentText"].ToString()), 20); 

                        hdfHdrDOB.Value = dtemployeeHeader.Rows[0]["empDOB"] == DBNull.Value
                                          ? string.Empty
                                          : Convert.ToDateTime(dtemployeeHeader.Rows[0]["empDOB"]).ToString(Resources.Constants.HRMSDateFormatShort);
                        //lblhdrEmployeeNameTxt.ToolTip = HttpUtility.HtmlDecode(dtemployeeHeader.Rows[0]["empNameText"].ToString());
                        //lblhdrEmployeeNoTxt.ToolTip = HttpUtility.HtmlDecode(dtemployeeHeader.Rows[0]["empCode"].ToString());
                        //lblhdrDepartmentTxt.ToolTip = HttpUtility.HtmlDecode(dtemployeeHeader.Rows[0]["empDepartmentText"].ToString());
                        //lblhdrDOJText.ToolTip = dtemployeeHeader.Rows[0]["empDOJText"].ToString();
                        //lblhdrDOBTxt.ToolTip = dtemployeeHeader.Rows[0]["empDOBText"].ToString();
                        //lblhdrDesignationTxt.ToolTip = HttpUtility.HtmlDecode(dtemployeeHeader.Rows[0]["empDesignationText"].ToString());
                    }
                    if (objEmployeeBasicInfo != null)
                    {
                        CurrPK = objEmployeeBasicInfo.empPK;
                        UCempBasicHdr.objEmployeeBasicInfo = objEmployeeBasicInfo;
                        UCempBasicHdr.SetFieldValues();
                    }
                    break;
                default:
                    break;
            }
        }

        private void BindDetailsView()
        {
            if (selectedEmployeeDoc != null)
            {
                try
                {
                    ddlDocumentTypeList.SelectedValue = this.selectedEmployeeDoc.DocTypePK.ToString();// ddlDocumentTypeList.Items.FindByValue(this.selectedEmployeeDoc.DocTypePK.ToString());
                    txtDocumentNo.Text = this.selectedEmployeeDoc.DocNo;
                    txtPlaceOfIssue.Text = this.selectedEmployeeDoc.PalceOfIssue;
                    txtDocRefNo.Text = this.selectedEmployeeDoc.ReferenceNo;
                    txtEmpDocIssuedBy.Text = this.selectedEmployeeDoc.IssuedBy;

                    if (!string.IsNullOrEmpty(selectedEmployeeDoc.IssuedOn))
                    {
                        hdfDocumentEntryPageIssuedOn.Value = this.selectedEmployeeDoc.IssuedOn.ToString();
                        txtDocumentIssuedOn.Text = Convert.ToDateTime(this.selectedEmployeeDoc.IssuedOn).ToString(Resources.Constants.HRMSDateFormatShort);
                    }
                    if (!string.IsNullOrEmpty(selectedEmployeeDoc.ExpiresOn))
                    {
                        hdfDocumentEntryPageExpiresOn.Value = this.selectedEmployeeDoc.ExpiresOn.ToString();
                        txtDocumentExpiresOn.Text = Convert.ToDateTime(this.selectedEmployeeDoc.ExpiresOn).ToString(Resources.Constants.HRMSDateFormatShort);
                        int daysLeft = GetDaysLeft(Convert.ToDateTime(this.selectedEmployeeDoc.ExpiresOn));
                        if (daysLeft > 1)
                        {
                            lblExpiryDaysLeft.Text = string.Format(GetLocalResourceObject("DaysLeftFomat").ToString(), daysLeft);
                        }
                        else if (daysLeft > -1)
                        {
                            lblExpiryDaysLeft.Text = string.Format(GetLocalResourceObject("DayLeftFomat").ToString(), daysLeft);
                        }
                        else if (daysLeft > -2)
                        {
                            lblExpiryDaysLeft.Text = string.Format(GetLocalResourceObject("OverdueDay").ToString(), Math.Abs(daysLeft));
                        }
                        else
                        {
                            lblExpiryDaysLeft.Text = string.Format(GetLocalResourceObject("OverdueDays").ToString(), Math.Abs(daysLeft));
                        }

                        hdfExpiryDaysLeft.Value = lblExpiryDaysLeft.Text.Trim();
                    }
                    if (!string.IsNullOrEmpty(selectedEmployeeDoc.ReferenceDate))
                        txtDocumentRefDate.Text = Convert.ToDateTime(selectedEmployeeDoc.ReferenceDate).ToString(Resources.Constants.HRMSDateFormatShort);
                    else
                        txtDocumentRefDate.Text = string.Empty;
                    txtDocumentTitle.Text = this.selectedEmployeeDoc.Title;
                    txtDocumetAddlInfo.Text = this.selectedEmployeeDoc.AdditionalInfo;
                    txtDocumentRemarks.Text = this.selectedEmployeeDoc.Remarks;
                    txtIntimateBefore.Text = this.selectedEmployeeDoc.InitimateBefore != 0
                                            ? this.selectedEmployeeDoc.InitimateBefore.ToString()
                                            : string.Empty;
                    if (!string.IsNullOrEmpty(selectedEmployeeDoc.CheckedInOn))
                        txtCheckedInOn.Text = Convert.ToDateTime(selectedEmployeeDoc.CheckedInOn).ToString(Resources.Constants.HRMSDateFormatShort);
                    else
                        txtCheckedInOn.Text = string.Empty;
                    chkOriginalSubmitted.Checked = this.selectedEmployeeDoc.OriginalSubmitted == 1 ? true : false;
                    txtAutoCheckedInBy.Text = this.selectedEmployeeDoc.CheckedInByText;
                    hdfAutoCheckedInBy.Value = this.selectedEmployeeDoc.CheckedInBy != "0"
                                            ? this.selectedEmployeeDoc.CheckedInBy
                                            : string.Empty;
                    LastModifiedTime = selectedEmployeeDoc.LastModifiedDate;
                    lblLastModifiedHDR.Text = Resources.Report.LastModifiedOn + LastModifiedTime.ToString(Resources.Report.LastModifiedDatetimeFormat);

                    if (selectedEmployeeDoc.IsCheckedIn == (short)CheckStateStatus.CheckedOut)
                    {
                        chkOriginalSubmitted.Enabled = false;
                    }
                    else
                    {
                        chkOriginalSubmitted.Enabled = true;
                    }

                    if (this.EntryStatus == BusinessObject.Common.EntryStatus.EDITMODE
                        || this.EntryStatus == BusinessObject.Common.EntryStatus.ENTRYMODE)
                    {
                        if (this.selectedEmployeeDoc.IsCheckedIn == (short)CheckStateStatus.CheckedOut)
                        {
                            btnCheckIn.Visible = true;
                            btnCheckOut.Visible = false;
                        }
                        else if (this.selectedEmployeeDoc.IsCheckedIn == (short)CheckStateStatus.CheckedIn)
                        {
                            btnCheckOut.Visible = true;
                            btnCheckIn.Visible = false;
                        }
                        else if (this.selectedEmployeeDoc.IsCheckedIn == (short)CheckStateStatus.CheckedIn)
                        {
                            btnCheckIn.Visible = false;
                            btnCheckOut.Visible = false;
                        }
                        else // anything else
                        {
                            btnCheckIn.Visible = false;
                            btnCheckOut.Visible = false;
                        }
                    }
                    else
                    {
                        btnCheckIn.Visible = btnCheckOut.Visible = false;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Collect All Data From UI (including Complex Object) to Local Field Variables
        /// </summary>
        /// <param name="controlsEnum"></param>
        private object SetUiValuesToObject(ActionsEnum mode)
        {
            object returnObj;
            returnObj = null;
            try
            {
                switch (mode)
                {
                    case ActionsEnum.SAVE:
                    case ActionsEnum.SAVEANDCONTINUE:
                        returnObj = BindSelectedEmployeeDoc();
                        break;

                }
                return returnObj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                returnObj = null;
            }
        }

        /// <summary>
        /// Set values to the control when edit details
        /// </summary>
        private void SetUIEditView(ActionsEnum Mode)
        {
            //EntryStatus = EntryStatus.ENTRYMODE;//Test Code
            try
            {
                switch (Mode)
                {
                    case ActionsEnum.DEFAULT:
                        GetFieldValues(ControlsEnum.DOCTYPELIST);
                        SetFieldValues(ControlsEnum.DOCTYPELIST);
                        break;
                    case ActionsEnum.EMPDOCDETAIL:
                    case ActionsEnum.EDIT:
                        GetFieldValues(ControlsEnum.SELECTEDEMPDOC);
                        SetFieldValues(ControlsEnum.SELECTEDEMPDOC);
                        break;
                    case ActionsEnum.NEW:
                        ResetForm(ActionsEnum.NEW);
                        EntryStatus = EntryStatus.NEWMODE;
                        SetUIEditView(ActionsEnum.DEFAULT);
                        break;
                    case ActionsEnum.VIEW:
                        SetUIEditView(ActionsEnum.EDIT);
                        break;
                    case ActionsEnum.SAVE:
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

        private void BindGrid(ControlsEnum controlsEnum)
        {
            switch (controlsEnum)
            {
                case ControlsEnum.EMPDOCLIST:
                    grdEmpDocList.DataSource = pageData;
                    grdEmpDocList.DataBind();
                    break;
                case ControlsEnum.UPLOADEDFILES:
                    grdUploads.DataSource = BLUploadList;
                    grdUploads.DataBind();
                    break;
                case ControlsEnum.LOGLIST:
                    grdCheckIncheckOutLog.DataSource = pageData; // Change Grid
                    grdCheckIncheckOutLog.DataBind();
                    break;
                default:
                    break;
            }
        }

        private void BindDropDown(ControlsEnum controlsEnum)
        {
            switch (controlsEnum)
            {
                case ControlsEnum.DOCTYPELIST:
                    ddlDocumentTypeList.Items.Clear();
                    ddlDocumentTypeList.DataSource = pageData;
                    ddlDocumentTypeList.DataTextField = GTIService.Constants.HRMS.Employee.Fields.ADM_CFG_TEXT;
                    ddlDocumentTypeList.DataValueField = GTIService.Constants.HRMS.Employee.Fields.ADM_CFG_VALUE;
                    ddlDocumentTypeList.DataBind();
                    ddlDocumentTypeList.Items.HtmlDecode();
                    ddlDocumentTypeList.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                default:
                    break;
            }
        }
        #endregion
    }

    /// <summary>
    ///Page Controls Enum 
    /// </summary>
    public enum ControlsEnum
    {
        DEFAULT,
        EMPDOCLIST,
        SELECTEDEMPDOC,
        DOCTYPELIST,
        EMPLOYEEAUTOCOMPLETE,
        UPLOADEDFILES,
        SELECTEDDOC,
        LOGLIST,
        EMPLIST,
        CHECKOUT,
        CHECKIN,
        INTERNALEXTENALTYPE,
        ISSUEDFOR,
        //FILTER,
        COMPANY,
        FILTERFOR,
        EMPLOYEEDETAILSHEADER,
        EMPLOYEEDETAILSBYID,
        EMPLOYEETRAININGBYID,
        DETAIL
    }
}
