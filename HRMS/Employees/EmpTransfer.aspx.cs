using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.Common;
using BusinessObject.HRMS.Payroll;
using ERP.Utilities;
using System.Data;
//using System.IO;
//using BusinessObject.AccountManagement;
using BusinessObject.CommonManagement;
using ERP.Utilities.HRMS;
using ERPSMS_v01.UserControls;
using BusinessObject.HRMS.Employee;
using ERPSMS_v01;

namespace HRMS.Employees
{
    public partial class EmpTransfer : ERP.Store.UI.WorkFlowBasePage
    {
        #region Properties & Variables
        #region Properties
        /// <summary>
        /// Entry State for managing display status
        /// </summary>
        private EntryStatus EntryStatus
        {
            get
            {
                return this.ViewState[ERP.Utilities.ViewstateStrings.EntryState] == null ? EntryStatus.LISTMODE : (EntryStatus)(this.ViewState[ERP.Utilities.ViewstateStrings.EntryState]);
            }
            set
            {
                this.ViewState[ERP.Utilities.ViewstateStrings.EntryState] = value;
            }
        }

        /// <summary>
        /// Hold data for EmployeeTransferDetailsViewState  to bind Gridview
        /// </summary>
        private List<EmployeeTransferDetails> EmpTransDetlsViewState
        {
            get
            {
                return this.ViewState[ViewstateStrings.EmpTransferDtls] == null ? new List<EmployeeTransferDetails>() : (List<EmployeeTransferDetails>)(this.ViewState[ViewstateStrings.EmpTransferDtls]);
            }
            set
            {
                this.ViewState[ViewstateStrings.EmpTransferDtls] = value;
            }
        }


        /// <summary>
        /// To maintain the LastModifiedTime in viewstate
        /// </summary>
        /// 
        private DateTime LastModifiedTime
        {
            get
            {
                return this.ViewState[ViewstateStrings.LastModifiedTime] == null ? System.DateTime.Now : (DateTime)this.ViewState[ViewstateStrings.LastModifiedTime];
            }
            set
            {
                this.ViewState[ViewstateStrings.LastModifiedTime] = value;
            }
        }

        /// <summary>
        /// To keep Current PK in view state
        /// </summary>
        private int CurrPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.CurrPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.CurrPK] = value;
            }
        }

        /// <summary>
        /// To keep Page Index In View State
        /// </summary>
        private string PageIndexList
        {
            get
            {
                return (string)this.ViewState[ViewstateStrings.PageIndexList];
            }
            set
            {
                this.ViewState[ViewstateStrings.PageIndexList] = value;
            }
        }

        /// <summary>
        /// To keep gerid view row index in view state
        /// </summary>
        private int RowIndex
        {
            get
            {
                return this.ViewState[ViewstateStrings.RowIndex] == null ? -1 : (int)this.ViewState[ViewstateStrings.RowIndex];
            }
            set
            {
                this.ViewState[ViewstateStrings.RowIndex] = value;
            }
        }
        /// <summary>
        /// Aplication referance ID
        /// </summary>
        private int ReferanceID
        {
            get
            {
                return this.ViewState[ViewstateStrings.ReferanceID] == null ? 0 : Convert.ToInt32(this.ViewState[ViewstateStrings.ReferanceID].ToString());
            }
            set
            {
                this.ViewState[ViewstateStrings.ReferanceID] = value;
            }
        }
        /// <summary>
        /// Process ID of the Page
        /// </summary>
        private int PageProcessID
        {
            get
            {
                return this.ViewState[ViewstateStrings.PageProcessID] == null ? 0 : Convert.ToInt32(this.ViewState[ViewstateStrings.PageProcessID]);
            }
            set
            {
                this.ViewState[ViewstateStrings.PageProcessID] = value;
            }
        }
        #endregion
        ActionsEnum commonActions;
        private DataTable dtResult;
        private DataTable dtCompany;
        private DataTable dtList;
        private BusinessObject.User currentUser;
        private EmployeeTransferHeader objEmpTransfer;
        private List<EmployeeTransferDetails> tempList;
        private int CompanyPk = 0;
        private int ConPK = 0;
        private string refID;
        private string inboxFlag;
        private string prefID;
        #endregion
        #region PageEvents
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
        #region Page_Init
        /// <summary>
        /// PageInit Used to set the Pager current Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, System.EventArgs e)
        {
            uclPaging.CurrentPage = 1;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            this.btnSubmit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnSave.PreRender += new EventHandler(btnAction_PreRender);
            this.btnSaveSubmit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnDelete.PreRender += new EventHandler(btnAction_PreRender);
            this.btnNew.PreRender += new EventHandler(btnAction_PreRender);
            this.btnEdit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnCancelSubmit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnEditforCancel.PreRender += new EventHandler(btnAction_PreRender);

            this.btnSubmit.Load += new EventHandler(btnAction_Load);
            this.btnSave.Load += new EventHandler(btnAction_Load);
            this.btnSaveSubmit.Load += new EventHandler(btnAction_Load);
            this.btnDelete.Load += new EventHandler(btnAction_Load);
            this.btnNew.Load += new EventHandler(btnAction_Load);
            this.btnEdit.Load += new EventHandler(btnAction_Load);
            this.btnCancelSubmit.Load += new EventHandler(btnAction_Load);
            this.btnEditforCancel.Load += new EventHandler(btnAction_Load);
        }
        #endregion
        #region btnAction_Load
        protected void btnAction_Load(object sender, EventArgs e)
        {
            (sender as Control).Visible = true;
        } 
        #endregion
        #region btnAction_PreRender
        /// <summary>
        /// To handle the visibility of the action corresponding to the users Privilage        
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAction_PreRender(object sender, EventArgs e)
        {
            base.CheckBtnVisibility(sender);
        } 
        #endregion
        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            PageActionHandler();
        }
        #endregion
        #region Page_PreRender
        /// <summary>
        /// To Handle Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            try
            {
                hdfCurrPk.Value = CurrPK.ToString();

                if (EntryStatus == EntryStatus.VIEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "PageViewMode", "$(document).ready(function(){PageViewMode(1);});", true);
                    lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                }
                else if (EntryStatus == EntryStatus.NEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing(2);});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "PageViewMode", "$(document).ready(function(){PageViewMode(2);});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideDetailSec(1);});", true);
                    lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                }
                else if (EntryStatus == EntryStatus.ENTRYMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideDetailSec('" + hdfShowHideDetailSec.Value + "');});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                    lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                }
                else if (EntryStatus == EntryStatus.EDITMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideDetailSec('" + hdfShowHideDetailSec.Value + "');});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                    lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                }
                else if (EntryStatus == EntryStatus.LISTMODE)
                {
                    ModifiedDatePnl.Visible = false;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowListing", "$(document).ready(function(){ShowListing(1);});", true);
                    lnkList.CssClass = GetLocalResourceObject("TabActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabInActive").ToString();
                }
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideAdvancedSearch();});", true);
                if (grdEmpList.Rows.Count > 0)
                    txtFromBrLoc.Enabled = txtToBrLoc.Enabled = false;
                else
                    txtFromBrLoc.Enabled = txtToBrLoc.Enabled = true;
                btnPrintMultiple.Visible = CurrPK > 0 ? true : false;

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion
        #endregion
        #region PageActionHandler
        private void PageActionHandler()
        {
            try
            {
                ConfigurationSettings();
                ucrWrkf.WrkfSubmit += new EventHandler(ActionHandler);
                ucrWrkf.ViewType = 1;
                if (!IsPostBack)
                {
                    string pid = Request.QueryString[QueryStrings.PID] != null ? Request.QueryString[QueryStrings.PID]
                      : Session[ERP.Utilities.SessionStrings.PID] != null ? Session[ERP.Utilities.SessionStrings.PID].ToString().Split('=')[1] : string.Empty;
                    refID = Request.QueryString[QueryStrings.RefID] != null ? Request.QueryString[QueryStrings.RefID]
                        : Session[ERP.Utilities.SessionStrings.RefID] != null ? Session[ERP.Utilities.SessionStrings.RefID].ToString().Split('=')[1] : string.Empty;
                    prefID = Request.QueryString[QueryStrings.PRefID] != null ? Request.QueryString[QueryStrings.PRefID]
                        : Session[ERP.Utilities.SessionStrings.PRefID] != null ? Session[ERP.Utilities.SessionStrings.PRefID].ToString().Split('=')[1] : string.Empty;
                    inboxFlag = Request.QueryString[QueryStrings.Flag] != null ? Request.QueryString[QueryStrings.Flag]
                    : Session[ERP.Utilities.SessionStrings.InboxFlag] != null ? Session[ERP.Utilities.SessionStrings.InboxFlag].ToString() : string.Empty;

                    ReferanceID = string.IsNullOrEmpty(refID)
                           ? string.IsNullOrEmpty(prefID)
                                 ? 0
                                 : int.Parse(prefID)
                           : int.Parse(refID);
                    FillProcessID(1);

                    GetFieldValues(ControlEnums.COMPANY);
                    SetFieldValues(ControlEnums.COMPANY);
                    GetFieldValues(ControlEnums.TRANSFERREASON);
                    SetFieldValues(ControlEnums.TRANSFERREASON);

                    //If Has RefID (from Inbox)
                    if (!string.IsNullOrEmpty(refID))
                    {
                        if (!string.IsNullOrEmpty(inboxFlag))
                        {
                            ucrWrkf.ViewType = 0;
                            EntryStatus = EntryStatus.VIEWMODE;
                        }
                        else
                        {
                            ucrWrkf.ViewType = 1;
                            EntryStatus = EntryStatus.ENTRYMODE;
                        }
                        ////start
                        if (string.IsNullOrEmpty(pid) || pid.Equals("1") || pid.Equals("11"))
                        {
                            ucrWrkf.RefID = int.Parse(refID);
                            base.WkfRefID = ucrWrkf.RefID;
                            CurrPK = GetApplicationID(ucrWrkf.RefID);
                            if (pid.Equals("11"))
                                hdfIsCancelled.Value = "1";//For Showing Cancelled Stamp in Detail Page
                        }

                    }
                    else if (!string.IsNullOrEmpty(prefID))
                    {
                        base.WkfRefID = ucrWrkf.RefID = int.Parse(prefID);
                        CurrPK = GetApplicationID(ucrWrkf.RefID);
                    }
                    if (CurrPK > 0)
                    {
                        SetCancelRef(CurrPK);
                        ucrWrkf.FillWorkFlowDetails();
                        if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && (EntryStatus == EntryStatus.ENTRYMODE || EntryStatus == EntryStatus.NEWMODE) && ucrWrkf.HasPageTaskPermission)
                            ucrWrkf.ViewType = 1;
                        else
                            ucrWrkf.ViewType = 0;

                        hdfShowHideDetailSec.Value = "0";
                        GetFieldValues(ControlEnums.TRANSFERDETAILS);
                        SetFieldValues(ControlEnums.TRANSFERDETAILS);
                        SetFieldValues(ControlEnums.EMPLIST);
                    }
                    else
                    {
                        uclPaging.CurrentPage = 1;
                        GetFieldValues(ControlEnums.LIST);
                        SetFieldValues(ControlEnums.LIST);
                        EntryStatus = EntryStatus.LISTMODE;
                    }

                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #region ConfigurationSettings
        private void ConfigurationSettings()
        {
            hdfEmpTransferMaxCount.Value = GetGlobalResourceObject("ConfigurationsRes", "HrmsEmpTransferMaxCount").ToString();
        }
        #endregion
        #endregion
        #region ActionHandler

        /// <summary>
        /// Button actions events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, EventArgs e)
        {
            if (!(this.Master as ERPSMS_2).ValidatePageDept())
                return;
            GridViewRow grvRow;
            TextBox WrkfComments;
            bool bIsChecked = false;
            int EmpTranLimit = 0;
            int Status = 0;
            string strError = string.Empty;
            DataTable dtErrorList = new DataTable();
            try
            {
                int? result = null;
                #region Getting Command Action
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((LinkButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
                }

                #endregion
                switch (commonActions)
                {
                    #region SAVE
                    case ActionsEnum.SAVE:
                        if (EmpTransDetlsViewState == null || EmpTransDetlsViewState.Count == 0)
                        {
                            litErrorMsg.Text = (GetLocalResourceObject("Err_NoRecordFoundToSave")).ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "');", true);
                            return;
                        }
                        else if (hdfFromBrLoc.Value == hdfToBrLoc.Value)
                        {
                            litErrorMsg.Text = (GetLocalResourceObject("Err_SameLocations")).ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "');", true);
                            return;
                        }
                        string trxNo = string.Empty;
                        objEmpTransfer = (EmployeeTransferHeader)SetUIValuesToObject(ControlEnums.SAVE);
                        objEmpTransfer.WKF_FLAG = 0;
                        objEmpTransfer.WKF_TRX_FLAG = Convert.ToInt32(WorkflowTransactionFlag.SAVE);
                        string xmlDoc = CommonFunctions.XmlSerialize(objEmpTransfer);
                        result = BusinessLogic.HRMS.Employee.EmployeeTransferBL.SaveEmployeeTransfer(xmlDoc, out trxNo, out dtErrorList);
                        if (result > 0)
                        {
                            lblTrxNo.Text = trxNo;
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Save_Success").ToString();
                            object[] args = new object[2];
                            args[0] = Resources.PageNameRes.EmployeeTransfer;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, args);
                            ResetForm(ControlEnums.CLEAR);
                            ResetForm(ControlEnums.CLEARADD);
                            ResetForm(ControlEnums.CLEARSEARCH);
                            EntryStatus = EntryStatus.LISTMODE;
                            GetFieldValues(ControlEnums.LIST);
                            SetFieldValues(ControlEnums.LIST);
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
                                litErrorMsg.Text = Resources.PageNameRes.EmployeeTransfer + " " + Resources.Messages.EditUsedByAnotherUser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CHECKCURRENCYMASTER)  // Not allow already have pending transfer
                            {
                                strError = string.Empty;
                                if (dtErrorList != null && dtErrorList.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dtErrorList.Rows)
                                    {
                                        strError += "<br />" + HttpUtility.HtmlDecode(Convert.ToString(dr["EFD_EMPLOYEE"]))
                                            + "(" + Convert.ToDateTime(Convert.ToString(dr["EFH_EFFECT_DT"])).ToString(Resources.Constants.HRMSDateFormatShort) + ")";
                                    }
                                }
                                litErrorMsg.Text = GetLocalResourceObject("Msg_TransferPending").ToString() + strError;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowMessageFixed('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.EmployeeTransfer + " " + Resources.Messages.AlreadyDeleted;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                EntryStatus = EntryStatus.LISTMODE;
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeTransfer);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region SAVESUBMIT
                    case ActionsEnum.SAVESUBMIT:
                        //Show WorkFlow Popup
                        if (EmpTransDetlsViewState == null || EmpTransDetlsViewState.Count == 0)
                        {
                            litErrorMsg.Text = (GetLocalResourceObject("Err_NoRecordFoundToSave")).ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "');", true);
                            return;
                        }
                        else if (hdfFromBrLoc.Value == hdfToBrLoc.Value)
                        {
                            litErrorMsg.Text = (GetLocalResourceObject("Err_SameLocations")).ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "');", true);
                            return;
                        }

                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ONE;
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    #endregion
                    #region SUBMIT
                    case ActionsEnum.SUBMIT:
                        //Show WorkFlow Popup   
                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    #endregion
                    #region WRKF SUBMIT
                    case ActionsEnum.WRKFSUBMIT:
                        //validate Page
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        else//valid
                        {
                            objEmpTransfer = (EmployeeTransferHeader)SetUIValuesToObject(ControlEnums.SAVE);
                            ucrWrkf.ApplicationID = 0;
                            if (hdfIsSaveSubmit.Value == CommonConstants.SELECT_VALUE_ONE)
                            {
                                objEmpTransfer.WKF_FLAG = 1;
                                if (objEmpTransfer != null)
                                {
                                    SaveTransaction(objEmpTransfer, Convert.ToInt32(WorkflowTransactionFlag.SAVEANDSUBMIT));
                                }
                            }
                            else if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)//Cancel Salary Payment
                            {
                                if (BusinessLogic.CommonManagement.CommonBL.ValidationForCancellation(CurrPK, ApplicationType.ETF))
                                {
                                    SaveTransaction(objEmpTransfer, Convert.ToInt32(WorkflowTransactionFlag.SUBMIT));
                                }
                                else
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Err_EmpTransfer_Cancel").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                                    if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)
                                        FillProcessID(1);
                                    hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                                    WrkfComments.Text = "";
                                    ResetForm(ControlEnums.CLEAR);
                                    ResetForm(ControlEnums.CLEARADD);
                                    ResetForm(ControlEnums.CLEARSEARCH);
                                    EntryStatus = EntryStatus.LISTMODE;
                                    GetFieldValues(ControlEnums.LIST);
                                    SetFieldValues(ControlEnums.LIST);
                                }
                            }
                            else//Submit
                            {
                                objEmpTransfer.WKF_FLAG = 2;
                                if (objEmpTransfer != null)
                                {
                                    SaveTransaction(objEmpTransfer, Convert.ToInt32(WorkflowTransactionFlag.SUBMIT));
                                }
                            }
                        }
                        break;
                    #endregion
                    #region EDITFORCANCEL
                    case ActionsEnum.EDITFORCANCEL:
                        foreach (GridViewRow grdrow in grdList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                ResetForm(ControlEnums.CLEARHDR);
                                ResetForm(ControlEnums.CLEARADD);
                                ResetForm(ControlEnums.CLEARSEARCH);
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfPk")).Value);
                                HiddenField hdfDelStatus = (HiddenField)grdrow.FindControl("hdfDelStatus");
                                HiddenField hdfStatus = (HiddenField)grdrow.FindControl("hdfStatus");
                                hdfIsCancelled.Value = hdfDelStatus.Value;
                                Status = Convert.ToInt32(hdfStatus.Value);
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            if (hdfIsCancelled.Value == "1")
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_Delete_Record").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                return;
                            }
                            else if (Status == 0)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_NotSubmitted").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                return;
                            }
                            ucrWrkf.Reset();
                            FillProcessID(11);
                            SetUIEditView(commonActions);
                            GetFieldValues(ControlEnums.TRANSFERDETAILS);
                            SetFieldValues(ControlEnums.TRANSFERDETAILS);
                            SetFieldValues(ControlEnums.EMPLIST);
                            WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                            base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID((int)CurrPK, ucrWrkf.ProcessID);
                            SetCancelRef(CurrPK);
                            ucrWrkf.FillWorkFlowDetails();
                            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                            {
                                ucrWrkf.ViewType = 1;
                            }
                            else
                            {
                                ucrWrkf.ViewType = 0;
                            }
                            ucrWrkf.ViewAction();
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Err_SelectRow").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region DELETESUBMIT
                    case ActionsEnum.DELETESUBMIT:
                        //Show WorkFlow Popup
                        hdfIsSaveSubmit.Value = CommonConstants.SELECTVAL;
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    #endregion
                    #region LIST
                    case ActionsEnum.LIST:
                        FillProcessID(1);
                        uclPaging.CurrentPage = 0;
                        this.PageIndexList = "1";
                        this.EntryStatus = EntryStatus.LISTMODE;
                        this.CurrPK = 0;
                        ResetForm(ControlEnums.CLEARSEARCH);
                        GetFieldValues(ControlEnums.LIST);
                        SetFieldValues(ControlEnums.LIST);
                        break;
                    #endregion
                    #region NEW
                    case ActionsEnum.NEW:
                        CurrPK = 0;
                        EntryStatus = EntryStatus.NEWMODE;
                        EmpTransDetlsViewState = null;
                        ResetForm(ControlEnums.CLEAR);
                        ResetForm(ControlEnums.CLEARHDR);
                        ResetForm(ControlEnums.CLEARADD);
                        GetFieldValues(ControlEnums.COMPANY);
                        SetFieldValues(ControlEnums.COMPANY);
                        GetFieldValues(ControlEnums.TRANSFERREASON);
                        SetFieldValues(ControlEnums.TRANSFERREASON);
                        SetFieldValues(ControlEnums.EMPLIST);
                        base.WkfRefID = ucrWrkf.RefID = 0;
                        ucrWrkf.FillWorkFlowDetails();
                        ucrWrkf.ViewType = 1;
                        ucrWrkf.ViewAction();
                        hdfIsCancelled.Value = "0";
                        break;
                    #endregion
                    #region EDIT/DETAIL
                    case ActionsEnum.EDIT:
                    case ActionsEnum.DETAIL:
                        foreach (GridViewRow grdrow in grdList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                ResetForm(ControlEnums.CLEARHDR);
                                ResetForm(ControlEnums.CLEARADD);
                                ResetForm(ControlEnums.CLEARSEARCH);
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfPk")).Value);
                                HiddenField hdfDelStatus = (HiddenField)grdrow.FindControl("hdfDelStatus");
                                hdfIsCancelled.Value = hdfDelStatus.Value;
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            FillProcessID(1);
                            SetUIEditView(commonActions);
                            WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                            base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                            SetCancelRef(CurrPK);
                            ucrWrkf.FillWorkFlowDetails();
                            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission && hdfIsCancelled.Value != "1")
                                ucrWrkf.ViewType = 1;
                            else
                            {
                                ucrWrkf.ViewType = 0;
                            }
                            ucrWrkf.ViewAction();
                            hdfShowHideDetailSec.Value = "0";
                            GetFieldValues(ControlEnums.TRANSFERDETAILS);
                            SetFieldValues(ControlEnums.TRANSFERDETAILS);
                            SetFieldValues(ControlEnums.EMPLIST);
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Err_SelectRow").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region DELETE HDR
                    case ActionsEnum.DELETE:
                        result = BusinessLogic.HRMS.Employee.EmployeeTransferBL.DeleteEmpTransfer(CurrPK, LastModifiedTime);
                        if (result > 0)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeTransfer);
                            ResetForm(ControlEnums.CLEAR);
                            ResetForm(ControlEnums.CLEARSEARCH);
                            EntryStatus = EntryStatus.LISTMODE;
                            GetFieldValues(ControlEnums.LIST);
                            SetFieldValues(ControlEnums.LIST);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else
                        {
                            if (result == (int)DbSaveStatus.REFERRED)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.EmployeeTransfer + " " + Resources.Messages.UsedInAnotherPlace;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.SQLERROR)
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CONCURRENCY)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.EmployeeTransfer + " " + Resources.Messages.EditUsedByAnotherUser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.ALREADYDELETED)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.EmployeeTransfer + " " + Resources.Messages.AlreadyDeleted;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeTransfer);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region ADD ITEM
                    case ActionsEnum.ADDITEM:
                        int.TryParse(hdfEmpTransferMaxCount.Value, out EmpTranLimit);
                        if (hdfEmployee.Value == hdfReportingPerson.Value)
                        {
                            litErrorMsg.Text = (GetLocalResourceObject("Err_EmpAndRptPerson")).ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "');", true);
                            return;
                        }
                        tempList = EmpTransDetlsViewState;

                        if (RowIndex >= 0 && EmpTransDetlsViewState != null && EmpTransDetlsViewState.Count > 0) //update
                        {
                            EmployeeTransferDetails objDetails = EmpTransDetlsViewState[RowIndex];
                            objDetails.EFD_EMPLOYEE = Convert.ToInt32(hdfEmployee.Value);
                            objDetails.EFD_EMPLOYEE_TEXT = HttpUtility.HtmlEncode(txtEmployee.Text);
                            objDetails.EFD_EMPLOYEE_RPT = Convert.ToInt32(hdfReportingPerson.Value) > 0 ? Convert.ToString(hdfReportingPerson.Value) : null;
                            objDetails.EFD_EMPLOYEE_RPT_TEXT = HttpUtility.HtmlEncode(txtReportingPerson.Text);
                            objDetails.EFD_REMARKS = string.IsNullOrEmpty(txtRemarks.Text.Trim()) ? null : HttpUtility.HtmlEncode(txtRemarks.Text);
                        }
                        else //new
                        {

                            if (tempList == null)
                                tempList = new List<EmployeeTransferDetails>();
                            if (EmpTranLimit > 0 && tempList.Count() >= EmpTranLimit)
                            {
                                litErrorMsg.Text = string.Format((GetLocalResourceObject("Msg_Err_TransferLimitExceeds")).ToString(), EmpTranLimit);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                return;
                            }
                            EmployeeTransferDetails existEmp = tempList.Where(x => x.EFD_EMPLOYEE == Convert.ToInt32(hdfEmployee.Value)).FirstOrDefault();
                            if (existEmp != null)
                            {
                                litErrorMsg.Text = string.Format(GetLocalResourceObject("Msg_EmployeeExist").ToString(), existEmp.EFD_EMPLOYEE_TEXT);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" +
                                    CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                return;
                            }

                            EmployeeTransferDetails objDetails = new EmployeeTransferDetails();
                            objDetails.EFD_EMPLOYEE = Convert.ToInt32(hdfEmployee.Value);
                            objDetails.EFD_EMPLOYEE_TEXT = HttpUtility.HtmlEncode(txtEmployee.Text);
                            objDetails.EFD_EMPLOYEE_RPT = Convert.ToInt32(hdfReportingPerson.Value) > 0 ? Convert.ToString(hdfReportingPerson.Value) : null;
                            objDetails.EFD_EMPLOYEE_RPT_TEXT = HttpUtility.HtmlEncode(txtReportingPerson.Text);
                            objDetails.EFD_REMARKS = string.IsNullOrEmpty(txtRemarks.Text.Trim()) ? null : HttpUtility.HtmlEncode(txtRemarks.Text);
                            tempList.Add(objDetails);
                        }
                        EmpTransDetlsViewState = tempList;
                        SetFieldValues(ControlEnums.EMPLIST);
                        ResetForm(ControlEnums.CLEARADD);
                        break;
                    #endregion
                    #region EDIT ITEM
                    case ActionsEnum.EDITITEM:
                        hdfShowHideDetailSec.Value = "1";
                        grvRow = (GridViewRow)((ImageButton)(sender)).Parent.Parent;
                        RowIndex = grvRow.RowIndex;
                        hdfCurrent_EfdPk.Value = (((HiddenField)grvRow.FindControl("hdfEFDPK")).Value);
                        SetFieldValues(ControlEnums.EDITITEM);
                        break;
                    #endregion
                    #region DELETE ITEM
                    case ActionsEnum.DELETEITEM:
                        grvRow = (GridViewRow)((ImageButton)(sender)).Parent.Parent;
                        RowIndex = grvRow.RowIndex;
                        SetFieldValues(ControlEnums.DELETEITEM);
                        SetFieldValues(ControlEnums.EMPLIST);
                        ResetForm(ControlEnums.CLEARADD);
                        break;
                    #endregion
                    #region CLEAR ADD
                    case ActionsEnum.CLEARADD:
                        ResetForm(ControlEnums.CLEARADD);
                        break;
                    #endregion
                    #region CANCEL
                    case ActionsEnum.CANCEL:
                        FillProcessID(1);
                        ResetForm(ControlEnums.CLEARSEARCH);
                        GetFieldValues(ControlEnums.LIST);
                        SetFieldValues(ControlEnums.LIST);
                        break;
                    #endregion
                    #region SEARCH
                    case ActionsEnum.SEARCH:
                        uclPaging.CurrentPage = 0;
                        this.PageIndexList = "1";
                        this.EntryStatus = EntryStatus.LISTMODE;
                        this.CurrPK = 0;
                        GetFieldValues(ControlEnums.LIST);
                        SetFieldValues(ControlEnums.LIST);
                        break;
                    #endregion
                    #region CLEAR SEARCH
                    case ActionsEnum.CLEARSEARCH:
                        ResetForm(ControlEnums.CLEARSEARCH);
                        GetFieldValues(ControlEnums.LIST);
                        SetFieldValues(ControlEnums.LIST);
                        break;
                    #endregion

                    #region PRINT TRANSFER DETAILS
                    case ActionsEnum.PRINTMULTIPLE:
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportPath + "?ID=0" + "&APPTYPE=" + "ETF" + "&APPSUBTYPE= 0" + "&CurPK=" + CurrPK) + "');", true);
                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }

        }

        #region Save Transaction
        private void SaveTransaction(EmployeeTransferHeader objEmpTransfer, int workflowFlag)
        {
            string strError = string.Empty;
            DataTable dtErrorList = new DataTable();
            int? result = 0;
            WorkflowDetails wkfDetails = null;
            string TrxNo = string.Empty;
            string action = string.Empty;
            if (objEmpTransfer == null)
                objEmpTransfer = new EmployeeTransferHeader();
            #region New workflow Submition
            wkfDetails = ucrWrkf.GetWorkflowDetails();
            objEmpTransfer.USER_PK = wkfDetails.UserPK;
            objEmpTransfer.WKF_APPLICATION = CurrPK;
            objEmpTransfer.WKF_COMMENTS = wkfDetails.Comments;
            objEmpTransfer.WKF_TRX_FLAG = workflowFlag;
            objEmpTransfer.WKF_PROCESS = wkfDetails.ProcessID;
            objEmpTransfer.WKF_REFERENCE = wkfDetails.ReferenceID;
            objEmpTransfer.WKF_TASK = wkfDetails.TaskID;
            objEmpTransfer.WKF_TASK_ACTION = wkfDetails.ActionID;
            action = wkfDetails.ActionText;
            #endregion           
            string xmlDoc = CommonFunctions.XmlSerialize(objEmpTransfer);
            result = BusinessLogic.HRMS.Employee.EmployeeTransferBL.SaveEmployeeTransfer(xmlDoc, out TrxNo, out dtErrorList);
            if (result > 0)
            {
                if (!string.IsNullOrEmpty(TrxNo))
                    lblTrxNo.Text = TrxNo;
                if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)
                {
                    FillProcessID(1);
                    litErrorMsg.Text = Resources.Messages.Msg_Cancelled_Success;
                }
                else
                {
                    litErrorMsg.Text = GetLocalResourceObject("Msg_Submit_Success").ToString();
                }
                hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                ((TextBox)ucrWrkf.FindControl("WrkfComments")).Text = string.Empty;//Clear Workflow comments
               
                object[] args = new object[2];
                args[0] = Resources.PageNameRes.EmployeeTransfer;
                args[1] = lblTrxNo.Text.Trim();
                litErrorMsg.Text = string.Format(litErrorMsg.Text, args);
                 // Show Save Message and redired to listing page                                      
                if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && base.WkfRefID > 0)
                {
                    ResetForm(ControlEnums.CLEAR);
                    ResetForm(ControlEnums.CLEARADD);
                    ResetForm(ControlEnums.CLEARSEARCH);
                    EntryStatus = EntryStatus.LISTMODE;
                    GetFieldValues(ControlEnums.LIST);
                    SetFieldValues(ControlEnums.LIST);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                            + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.InboxURL) + "');", true);
                }
                else
                {
                    ResetForm(ControlEnums.CLEAR);
                    ResetForm(ControlEnums.CLEARADD);
                    ResetForm(ControlEnums.CLEARSEARCH);
                    EntryStatus = EntryStatus.LISTMODE;
                    GetFieldValues(ControlEnums.LIST);
                    SetFieldValues(ControlEnums.LIST);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                }
            }
            else
            {
                if (result == (int)DbSaveStatus.SQLERROR)
                {
                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                        + "','" + Resources.ErpRes.Information + "');", true);
                }
                else if (result == (int)DbSaveStatus.CONCURRENCY)
                {
                    litErrorMsg.Text = Resources.PageNameRes.EmployeeTransfer + " " + Resources.Messages.EditUsedByAnotherUser;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                    + "','" + Resources.ErpRes.Information + "');", true);
                }
                else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                {
                    litErrorMsg.Text = Resources.PageNameRes.EmployeeTransfer + " " + Resources.Messages.AlreadyDeleted;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                        + "','" + Resources.ErpRes.Information + "');", true);
                    EntryStatus = EntryStatus.LISTMODE;
                }
                else if (result == (int)DbSaveStatus.CHECKCURRENCYMASTER)  // Not allow already have pending transfer
                {
                    strError = string.Empty;
                    if (dtErrorList != null && dtErrorList.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtErrorList.Rows)
                        {
                            strError += "<br />" + HttpUtility.HtmlDecode(Convert.ToString(dr["EFD_EMPLOYEE"]))
                                + "(" + Convert.ToDateTime(Convert.ToString(dr["EFH_EFFECT_DT"])).ToString(Resources.Constants.HRMSDateFormatShort) + ")";
                        }
                    }
                    litErrorMsg.Text = GetLocalResourceObject("Msg_TransferPending").ToString() + strError;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowMessageFixed('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                        + "','" + Resources.ErpRes.Information + "');", true);
                }
                else
                {
                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeTransfer);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                        + "','" + Resources.ErpRes.Information + "');", true);
                }
            }
        } 
        #endregion

        #region --- For Grid Actions----

        /// <summary>
        /// For Grid View Command
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewCommandEventArgs e)
        {

        }

        /// <summary>
        /// For Grid View Row Events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            #region grdDeductPayDetails
            if ((sender as GridView).ID == "grdList")
            {
                if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                {
                    HiddenField hdfDelStatus = e.Row.FindControl("hdfDelStatus") as HiddenField;
                    if (Convert.ToInt32(hdfDelStatus.Value) == 1)
                    {
                        e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml(GetLocalResourceObject("CancelledTransferColour").ToString());

                    }
                    else
                    {
                        e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml(GetGlobalResourceObject("Controls", "RowColor").ToString()); //  GetLocalResourceObject("RowColor")
                    }
                }
            }
            #endregion
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
        #region Custom Pager Control Navigated Event
        /// <summary>
        /// Action Handlers For Pager Control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    PageIndexList = uclPaging.CurrentPage.ToString();
                    GetFieldValues(ControlEnums.LIST);
                    SetFieldValues(ControlEnums.LIST);
                    EntryStatus = EntryStatus.LISTMODE;
                    EnableDisableButtons(e.TotalPages, "uclPaging");
                }
            }
            catch (Exception ex)
            {
                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.gComsRes.Information + "');", true);
            }
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
                // Should we disable the first link
                uclPaging.FirstButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
                // Should we disable the previous link
                uclPaging.PreviousButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
                // Should we enable the next link
                uclPaging.NextButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
                // Should we enable the last link
                uclPaging.LastButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
            }
        }
        #endregion
        #region ResetForm
        /// <summary>
        /// Method used to Reset Form Controls
        /// </summary>
        private void ResetForm(ControlEnums controlType)
        {
            switch (controlType)
            {
                case ControlEnums.CLEAR:
                    uclPaging.CurrentPage = 0;
                    this.PageIndexList = "1";
                    this.CurrPK = 0;
                    txtDate.Text = DateTime.Now.ToString(Resources.Constants.HRMSDateFormatShort);
                    txtFromBrLoc.Text = string.Empty;
                    hdfFromBrLoc.Value = string.Empty;
                    txtToBrLoc.Text = string.Empty;
                    hdfToBrLoc.Value = string.Empty;
                    ddlReason.ClearSelection();
                    txtEffectiveDate.Text = string.Empty;
                    txtDescription.Text = string.Empty;
                    break;
                case ControlEnums.CLEARSEARCH:
                    txtFilterFromDate.Text = txtFilterToDate.Text = string.Empty;
                    txtFilterFromLocation.Text = txtFilterToLocation.Text = txtEmpName.Text = string.Empty;
                    hdfFilterFromLocation.Value = hdfFilterToLocation.Value = string.Empty;
                    hdfEmpName.Value = "0";
                    ddlStatus.ClearSelection();
                    txtTrxNo.Text = string.Empty;
                    hdfTrxPk.Value = string.Empty;
                    uclPaging.CurrentPage = 0;
                    this.PageIndexList = "1";
                    this.EntryStatus = EntryStatus.LISTMODE;
                    this.CurrPK = 0;
                    break;
                case ControlEnums.CLEARHDR:
                    lblTrxNo.Text = Resources.ErpRes.Draft;
                    this.CurrPK = 0;
                    dtResult = null;
                    hdfCurrent_EfdPk.Value = "0";
                    break;
                #region CLEARADD
                case ControlEnums.CLEARADD:
                    RowIndex = -1;
                    txtEmployee.Text = string.Empty;
                    hdfEmployee.Value = string.Empty;
                    txtReportingPerson.Text = string.Empty;
                    hdfReportingPerson.Value = string.Empty;
                    txtRemarks.Text = string.Empty;
                    break;
                #endregion
            }
        }
        #endregion
        #region Get Field Values
        /// <summary>
        /// Gets the data to bind/assign for the controls(inputs, grids, dropdowns etc.)
        /// pass string.empty to get all field values
        /// Assign it to the page level variables
        /// </summary>
        private void GetFieldValues(ControlEnums type)
        {
            try
            {
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                FilterParameters objFilterParam;
                switch (type)
                {
                    #region COMPANY
                    case ControlEnums.COMPANY:
                        dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(BusinessObject.CommonManagement.DbActiveStatus.ACTIVE), 0, CompanyPk);
                        break;
                    #endregion
                    #region TRANSFER REASON
                    case ControlEnums.TRANSFERREASON:
                        dtResult = BusinessLogic.HRMS.Employee.EmployeeTransferBL.GetTransferReason(ConPK, (int)DbActiveStatus.ACTIVE);
                        break;
                    #endregion
                    #region LIST
                    case ControlEnums.LIST:
                        int empPk = 0;
                        int.TryParse(hdfEmpName.Value, out empPk);
                        objFilterParam = new FilterParameters();
                        objFilterParam.PageNumber = uclPaging.CurrentPage == 0 ? 1 : uclPaging.CurrentPage;
                        objFilterParam.PageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
                        objFilterParam.FromDate = string.IsNullOrEmpty(txtFilterFromDate.Text) ? (DateTime?)null : DateTime.Parse(txtFilterFromDate.Text);
                        objFilterParam.ToDate = string.IsNullOrEmpty(txtFilterToDate.Text) ? (DateTime?)null : DateTime.Parse(txtFilterToDate.Text);
                        objFilterParam.BizUnit = currentUser.SBUID;
                        objFilterParam.BranchLocation = string.IsNullOrEmpty(hdfFilterFromLocation.Value) ? (int?)null : Convert.ToInt32(hdfFilterFromLocation.Value);
                        objFilterParam.ToBranchLocation = string.IsNullOrEmpty(hdfFilterToLocation.Value) ? (int?)null : Convert.ToInt32(hdfFilterToLocation.Value);
                        objFilterParam.PK = string.IsNullOrEmpty(hdfTrxPk.Value) ? (int?)null : Convert.ToInt32(hdfTrxPk.Value);
                        objFilterParam.Status = Convert.ToInt32(ddlStatus.SelectedValue) == 3 ? (int?)null : Convert.ToInt32(ddlStatus.SelectedValue);
                        objFilterParam.UserPK = currentUser.PKUser;
                        objFilterParam.Employee = empPk > 0 ? empPk : (int?)null;
                        dtList = BusinessLogic.HRMS.Employee.EmployeeTransferBL.GetEmpTransferList(objFilterParam);
                        break;
                    #endregion
                    #region TRANSFER DETAILS
                    case ControlEnums.TRANSFERDETAILS:
                        objEmpTransfer = BusinessLogic.HRMS.Employee.EmployeeTransferBL.GetEmpTransferDetails(CurrPK);
                        if (objEmpTransfer != null)
                            EmpTransDetlsViewState = objEmpTransfer.EmployeeTransferDtl.ToList();

                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion
        #region Set Field Values
        /// <summary>
        /// All Field(Input controls, grids, dropdowns) values are assigned here.
        /// To Set all fields, pass "string.Empty()"
        /// </summary>
        private void SetFieldValues(ControlEnums controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlEnums.COMPANY:
                        BindDropDown(ControlEnums.COMPANY);
                        break;
                    case ControlEnums.EMPLIST:
                        BindGrid(ControlEnums.EMPLIST);
                        break;
                    case ControlEnums.EDITITEM:
                        GetUIValuesFromObject(ControlEnums.EDITITEM);
                        break;
                    case ControlEnums.DELETEITEM:
                        GetUIValuesFromObject(ControlEnums.DELETEITEM);
                        break;
                    case ControlEnums.TRANSFERREASON:
                        BindDropDown(ControlEnums.TRANSFERREASON);
                        break;
                    case ControlEnums.LIST:
                        BindGrid(ControlEnums.LIST);
                        break;
                    case ControlEnums.TRANSFERDETAILS:
                        GetUIValuesFromObject(ControlEnums.TRANSFERDETAILS);
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region BindGrid
        /// <summary>
        /// This method is used to Binding Grids
        /// </summary>
        /// <param name="controlType"></param>
        private void BindGrid(ControlEnums controlType)
        {
            switch (controlType)
            {
                #region EMP LIST
                case ControlEnums.EMPLIST:
                    if (EmpTransDetlsViewState != null && EmpTransDetlsViewState.Count > 0)
                        grdEmpList.DataSource = EmpTransDetlsViewState;
                    else
                        grdEmpList.DataSource = null;
                    grdEmpList.DataBind();
                    break;
                #endregion
                #region LIST
                case ControlEnums.LIST:
                    int rowCount = 0;
                    int pageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
                    if (dtList.Rows.Count > 0)
                    {
                        rowCount = Convert.ToInt32(dtList.Rows[0]["TOTAL_ROW_COUNT"].ToString());
                    }
                    uclPaging.TotalPages = rowCount == 0 ? 1 : (rowCount <= pageSize) ? 1 :
                                  (rowCount % pageSize) == 0 ? (rowCount / pageSize) :
                                  (rowCount / pageSize) + 1;
                    PageIndexList = PageIndexList == null ? CommonConstants.SELECT_VALUE_ONE : PageIndexList;
                    uclPaging.CurrentPage = Convert.ToInt32(PageIndexList);
                    grdList.DataSource = dtList;
                    grdList.DataBind();
                    uclPaging.Visible = true;
                    uclPaging.BindPager();
                    break;
                #endregion
            }
        }
        #endregion
        #region SetUIValuesToObject
        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>
        private Object SetUIValuesToObject(ControlEnums controlType)
        {
            Object retObject;
            retObject = null;
            try
            {
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                switch (controlType)
                {
                    #region SAVE
                    case ControlEnums.SAVE:
                        objEmpTransfer = new EmployeeTransferHeader();
                        objEmpTransfer.EFH_COMPANY = Convert.ToInt32(ddlCompany.SelectedValue);
                        objEmpTransfer.EFH_DATE = Convert.ToDateTime(txtDate.Text);
                        objEmpTransfer.EFH_DESC = string.IsNullOrEmpty(txtDescription.Text.Trim()) ? null : HttpUtility.HtmlEncode(txtDescription.Text);
                        objEmpTransfer.EFH_EFFECT_DT = Convert.ToDateTime(txtEffectiveDate.Text);
                        objEmpTransfer.EFH_FROM = Convert.ToInt32(hdfFromBrLoc.Value);
                        objEmpTransfer.EFH_NO = lblTrxNo.Text;
                        objEmpTransfer.EFH_PK = CurrPK;
                        objEmpTransfer.EFH_REASON = Convert.ToInt32(ddlReason.SelectedValue);
                        objEmpTransfer.EFH_STATUS = (int)DbActiveStatus.ACTIVE;
                        objEmpTransfer.EFH_TO = Convert.ToInt32(hdfToBrLoc.Value);
                        objEmpTransfer.EmployeeTransferDtl = EmpTransDetlsViewState;
                        objEmpTransfer.USER_PK = currentUser.PKUser;
                        objEmpTransfer.BIZUNIT = currentUser.SBUID;
                        objEmpTransfer.EFH_DEPT = currentUser.CurrentDeptPK;
                        objEmpTransfer.LAST_MOD_DT = LastModifiedTime;
                        retObject = objEmpTransfer;
                        break;
                    #endregion
                }
                return retObject;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region "GetUIValuesFromObject"
        /// <summary>
        /// Sets the UI input controls from the object values
        /// </summary>
        private void GetUIValuesFromObject(ControlEnums controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region TRANSFER DETAILS
                    case ControlEnums.TRANSFERDETAILS:
                        if (objEmpTransfer != null)
                        {
                            lblTrxNo.Text = string.IsNullOrEmpty(objEmpTransfer.EFH_NO) ? Resources.ErpRes.Draft : objEmpTransfer.EFH_NO;
                            txtDate.Text = objEmpTransfer.EFH_DATE.ToString(Resources.Constants.HRMSDateFormatShort);
                            txtFromBrLoc.Text = HttpUtility.HtmlDecode(objEmpTransfer.EFH_FROM_TEXT);
                            hdfFromBrLoc.Value = objEmpTransfer.EFH_FROM.ToString();
                            txtToBrLoc.Text = HttpUtility.HtmlDecode(objEmpTransfer.EFH_TO_TEXT);
                            hdfToBrLoc.Value = objEmpTransfer.EFH_TO.ToString();
                            ConPK = objEmpTransfer.EFH_REASON;
                            GetFieldValues(ControlEnums.TRANSFERREASON);
                            SetFieldValues(ControlEnums.TRANSFERREASON);
                            ddlReason.SelectedIndex = ddlReason.Items.IndexOf(ddlReason.Items.FindByValue(objEmpTransfer.EFH_REASON.ToString()));
                            txtEffectiveDate.Text = objEmpTransfer.EFH_EFFECT_DT.ToString(Resources.Constants.HRMSDateFormatShort);
                            txtDescription.Text = HttpUtility.HtmlDecode(objEmpTransfer.EFH_DESC);
                            LastModifiedTime = objEmpTransfer.LAST_MOD_DT;
                            CompanyPk = objEmpTransfer.EFH_COMPANY;
                            GetFieldValues(ControlEnums.COMPANY);
                            SetFieldValues(ControlEnums.COMPANY);
                            ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(CompanyPk.ToString()));
                        }
                        break;
                    #endregion
                    #region EDIT ITEM
                    case ControlEnums.EDITITEM:
                        if (EmpTransDetlsViewState != null && EmpTransDetlsViewState.Count > 0 && RowIndex >= 0)
                        {
                            EmployeeTransferDetails objDet = EmpTransDetlsViewState[RowIndex];
                            if (objDet != null)
                            {
                                txtEmployee.Text = HttpUtility.HtmlDecode(objDet.EFD_EMPLOYEE_TEXT);
                                hdfEmployee.Value = objDet.EFD_EMPLOYEE.ToString();
                                txtReportingPerson.Text = HttpUtility.HtmlDecode(objDet.EFD_EMPLOYEE_RPT_TEXT);
                                hdfReportingPerson.Value = objDet.EFD_EMPLOYEE_RPT.ToString();
                                txtRemarks.Text = HttpUtility.HtmlDecode(objDet.EFD_REMARKS);
                            }
                        }
                        break;
                    #endregion
                    #region DELETE ITEM
                    case ControlEnums.DELETEITEM:
                        if (EmpTransDetlsViewState != null && EmpTransDetlsViewState.Count > 0 && RowIndex >= 0)
                        {
                            tempList = EmpTransDetlsViewState;
                            EmployeeTransferDetails objTemp = tempList[RowIndex];
                            tempList.Remove(objTemp);
                            EmpTransDetlsViewState = tempList;
                        }
                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region BindDropDown
        /// <summary>
        /// This method is used to Binding DropDoowns
        /// </summary>
        /// <param name="controlType"></param>
        private void BindDropDown(ControlEnums controlType)
        {
            switch (controlType)
            {
                #region COMPANY
                case ControlEnums.COMPANY:
                    ddlCompany.Items.Clear();
                    ddlCompany.DataTextField = Resources.DataFieldRes.CMP_NAME;
                    ddlCompany.DataValueField = Resources.DataFieldRes.CMP_PK;
                    ddlCompany.DataSource = dtCompany;
                    ddlCompany.DataBind();
                    ddlCompany.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    ddlCompany.Items.HtmlDecode();
                    if (dtCompany != null && dtCompany.Rows.Count > 0 && !string.IsNullOrEmpty(Convert.ToString(dtCompany.Rows[0][Resources.DataFieldRes.CMP_DEFAULT_PK])))
                        ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(dtCompany.Rows[0][Resources.DataFieldRes.CMP_DEFAULT_PK].ToString()));

                    break;
                #endregion
                #region TRANSFER REASON
                case ControlEnums.TRANSFERREASON:
                    ddlReason.Items.Clear();
                    ddlReason.DataSource = dtResult;
                    ddlReason.DataTextField = GTIService.Constants.Common.Fields.CON_NAME;
                    ddlReason.DataValueField = GTIService.Constants.Common.Fields.CON_VALUE;
                    ddlReason.DataBind();
                    ddlReason.Items.HtmlDecode();
                    ddlReason.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
            }
        }
        #endregion
        #endregion
        #region HelperMethods
        private void SetUIEditView(ActionsEnum Mode)
        {
            try
            {
                if (Mode == ActionsEnum.NEW)
                {
                    EntryStatus = EntryStatus.NEWMODE;
                }
                else if (Mode == ActionsEnum.VIEW)
                {
                    EntryStatus = EntryStatus.VIEWMODE;
                }
                else
                {
                    EntryStatus = EntryStatus.ENTRYMODE;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private int? GetNullableInt(string str)
        {
            int result;
            if (int.TryParse(str, out result))
            {
                return (int?)result;
            }
            return null;
        }

        private decimal? GetNullableDecimal(string str)
        {
            decimal result;
            if (decimal.TryParse(str, out result))
            {
                return (decimal?)result;
            }
            return null;
        }

        public int GetInt(string str)
        {
            int result = 0;
            if (int.TryParse(str, out result))
            {
                return result;
            }
            return result;
        }

        private DateTime? GetNullableDate(string str)
        {
            DateTime result;
            if (DateTime.TryParse(str, out result))
            {
                return (DateTime?)result;
            }
            return null;
        }
        #endregion
        #region WorkFlow Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        private int GetApplicationID(int refId)
        {
            int appId = 0;
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            DataTable dtApplication = wrkfService.GetApplicationID(refId);
            if (dtApplication != null)
            {
                if (dtApplication.Rows.Count > 0)
                {
                    appId = Convert.ToInt32((dtApplication.Rows[0][CommonConstants.F_APP_PK] == DBNull.Value) ? 0 : dtApplication.Rows[0][CommonConstants.F_APP_PK]);
                }
            }
            return appId;
        }

        /// <summary>
        /// Method to Fill Process ID
        /// </summary>
        private void FillProcessID(int pid)
        {
            string path = string.Empty;
            if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
                path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "") + "?PID=" + pid.ToString();
            else
                path = Request.Url.AbsolutePath.ToLower() + "?PID=" + pid.ToString();

            if (Session[BusinessObject.Common.SessionStrings.CurDept] != null)
            {
                WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
                DataTable dtProcess = wrkfService.GetProcessID(path, Convert.ToInt32(Session[BusinessObject.Common.SessionStrings.CurDept].ToString()));
                if (dtProcess != null && dtProcess.Rows.Count > 0)
                {
                    ucrWrkf.PageUrl = path;
                    ucrWrkf.ProcessID = int.Parse(dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString());
                    hdfProcessID.Value = dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString();
                    if (pid == 1)
                    {
                        PageProcessID = ucrWrkf.ProcessID;
                    }
                    base.WkfPageUrl = path;
                }
            }
        }
        /// <summary>
        /// For Bind Cancelation comment on workflow user control
        /// </summary>
        /// <param name="curPK"></param>
        private void SetCancelRef(int curPK)
        {
            #region Cancel ref Setting
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            DataTable dtCancelProcess = wrkfService.GetProcessID(GetLocalResourceObject("CancelPageURL").ToString(), currentUser.CurrentDeptPK);
            if (dtCancelProcess != null && dtCancelProcess.Rows.Count > 0)
            {
                WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                ucrWrkf.CancelRefID = workflowCore.GetRefID(curPK, int.Parse(dtCancelProcess.Rows[0][CommonConstants.F_PROCESS].ToString()));
            }
            #endregion
        }
        private string GetUrl()
        {
            string path = string.Empty;
            // if one page containes two process (pageurl?PID=1,pageurl?PID=2)
            if (Request.QueryString[QueryStrings.PID] == null)
            {
                if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
                    path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "") + "?PID=1";
                else
                    path = Request.Url.AbsolutePath.ToLower() + "?PID=1";
            }
            else
            {
                if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
                    path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "");
                else
                    path = Request.Url.AbsolutePath.ToLower();
            }
            return path;
        }




        #endregion
        #region Page Control Enum
        enum ControlEnums
        {
            COMPANY,
            LIST,
            DETAIL,
            CLEAR,
            CLEARSEARCH,
            CLEARHDR,
            ADD,
            CLEARADD,
            EMPLIST,
            EDITITEM,
            DELETEITEM,
            SAVE,
            TRANSFERREASON,
            TRANSFERDETAILS,
            PRINTMULTIPLE
        }
        #endregion
    }
}
