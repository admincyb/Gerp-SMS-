using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject;
using BusinessObject.AccountManagement;
using ERP.Utilities;
using System.Data;
using BusinessObject.Common;
using System.Threading;
using BusinessObject.CommonManagement;
using BusinessLogic.CommonManagement;
using BusinessObject.HRMS.Payroll;
using BusinessLogic.HRMS.Payroll;
using ERP.Utilities.HRMS;
using ERPSMS_v01.UserControls;
using System.IO;
using System.Configuration;
using System.Data.OleDb;
using System.Text;
using ERPSMS_v01;

namespace HRMS.Payroll
{
    public partial class OtherAdditionDeduction : ERP.Store.UI.WorkFlowBasePage
    {
        #region Variables
        private ActionsEnum commonActions;
        Int32 CurEMP_PK = 0;
        User currentUser;
        DataTable dtTable;
        DataTable dtCompany;
        private DataTable dtResult;
        private DataSet dsExchangeRate;
        private AdditionDeductionHeader objHeader;
        private AdditionDeductionDetails objEmpAddDedDtls;
        private List<AdditionDeductionDetails> objEmpAddDedDtlsList;
        private string refID;
        private string inboxFlag;
        private string prefID;
        private int JournalPK;
        private int CompanyPk = 0;
        private string uploadPath;
        private string[] excelColumns;
        private FileInfo attchInfo;
        private OleDbConnection connExcel;
        private OleDbCommand cmdExcel;
        private OleDbDataAdapter oleDbDataAdapter;
        private DataTable dtExcelSchema;
        private DataSet dsImportdata;
        private string landingSheet;
        private StringBuilder sb;
        private string xmlLanding;
        private string[] airColums_General = { "Date", "EmpCode", "Amount", "Remarks" };
        #endregion
        #region Properties
        private int CurrPK
        {
            get
            {
                return this.ViewState[ViewstateStrings.CurrPK] == null ? 0 : Convert.ToInt32(this.ViewState[ViewstateStrings.CurrPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.CurrPK] = value;
            }
        }

        private int CurrEmpAddDedPK
        {
            get
            {
                return this.ViewState[ViewstateStrings.CurrEmpAddDedPK] == null ? 0 : Convert.ToInt32(this.ViewState[ViewstateStrings.CurrEmpAddDedPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.CurrEmpAddDedPK] = value;
            }
        }

        private List<AdditionDeductionDetails> EmpAddDedDetailList
        {
            get
            {
                return (List<AdditionDeductionDetails>)ViewState[ViewstateStrings.OtherDetailList];
            }
            set
            {
                ViewState[ViewstateStrings.OtherDetailList] = value;
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

        private int CurrSlNo
        {
            get
            {
                return ViewState[ERP.Utilities.ViewstateStrings.CurrSlNo] == null ? 0 : (int)ViewState[ERP.Utilities.ViewstateStrings.CurrSlNo];
            }
            set
            {
                ViewState[ERP.Utilities.ViewstateStrings.CurrSlNo] = value;
            }
        }
        /// <summary>
        /// Aplication referance ID
        /// </summary>
        private int ReferanceID
        {
            get
            {
                return this.ViewState["ReferanceID"] == null ? 0 : Convert.ToInt32(this.ViewState["ReferanceID"].ToString());
            }
            set
            {
                this.ViewState["ReferanceID"] = value;
            }
        }
        /// <summary>
        /// Process ID of the Page
        /// </summary>
        private int PageProcessID
        {
            get
            {
                return this.ViewState["PageProcessID"] == null ? 0 : Convert.ToInt32(this.ViewState["PageProcessID"]);
            }
            set
            {
                this.ViewState["PageProcessID"] = value;
            }
        }

        /// <summary>
        /// To keep Page Index In View State
        /// </summary>
        private string PageIndexList
        {
            get
            {
                return (string)this.ViewState["PageIndexList"];
            }
            set
            {
                this.ViewState["PageIndexList"] = value;
            }
        }

        private bool MultiCurrencyEnabled
        {
            get
            {
                return this.ViewState[ViewstateStrings.MultiCurrencyEnabled] == null ? false : Convert.ToBoolean((this.ViewState[ViewstateStrings.MultiCurrencyEnabled]));
            }
            set
            {
                this.ViewState[ViewstateStrings.MultiCurrencyEnabled] = value;
            }
        }

        #endregion
        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            PageActionHandler();
        }
        #endregion
        #region Page Action Handler
        private void PageActionHandler()
        {
            try
            {
                //if (Session[ERP.Utilities.SessionStrings.TransactionType] == null)
                //{
                ucrWrkf.WrkfSubmit += new EventHandler(ActionHandler);
                //    hdfJournalizeWorkFlow.Value = "0";
                //}
                ucrWrkf.ViewType = 1;
                //set of hidden fields used to format Quantity, Amount, Rate
                if (!IsPostBack)
                {
                    hdfDecimalDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits.ToString();
                    string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
                    hdfCurrencyFormatWithComma.Value = "#" + currencysep + "#0.";
                    hdfCurrencyFormat.Value = "#0.";
                    for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits; i++)
                    {
                        hdfCurrencyFormat.Value += "0";
                        hdfCurrencyFormatWithComma.Value += "0";
                    }

                    MultiCurrencyEnabled = CommonFunctions.IsMultyCurrencyEnabled();
                    if (MultiCurrencyEnabled == true)
                        hdfCurrencyMode.Value = "1";
                    else
                        hdfCurrencyMode.Value = "0";

                    hdfAdvSearch.Value = "0";
                    string pid = Request.QueryString[QueryStrings.PID] != null ? Request.QueryString[QueryStrings.PID]
                        : Session[ERP.Utilities.SessionStrings.PID] != null ? Session[ERP.Utilities.SessionStrings.PID].ToString().Split('=')[1] : string.Empty;
                    refID = Request.QueryString[QueryStrings.RefID] != null ? Request.QueryString[QueryStrings.RefID]
                        : Session[ERP.Utilities.SessionStrings.RefID] != null ? Session[ERP.Utilities.SessionStrings.RefID].ToString().Split('=')[1] : string.Empty;
                    prefID = Request.QueryString[QueryStrings.PRefID] != null ? Request.QueryString[QueryStrings.PRefID]
                        : Session[ERP.Utilities.SessionStrings.PRefID] != null ? Session[ERP.Utilities.SessionStrings.PRefID].ToString().Split('=')[1] : string.Empty;
                    inboxFlag = Request.QueryString[QueryStrings.Flag] != null ? Request.QueryString[QueryStrings.Flag]
                    : Session[ERP.Utilities.SessionStrings.InboxFlag] != null ? Session[ERP.Utilities.SessionStrings.InboxFlag].ToString() : string.Empty;
                    hdfExchangeRateFormat.Value = "#0.";
                    int exchrateDecimalDigits = (Session[ERP.Utilities.SessionStrings.ExchRateDecimalDigit] == null
                        ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits
                        : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.ExchRateDecimalDigit]));
                    for (int i = 0; i < exchrateDecimalDigits; i++)
                    {
                        hdfExchangeRateFormat.Value += "0";
                    }
                    ReferanceID = string.IsNullOrEmpty(refID)
                           ? string.IsNullOrEmpty(prefID)
                                 ? 0
                                 : int.Parse(prefID)
                           : int.Parse(refID);
                    FillProcessID(1);

                    //EntryStatus = EntryStatus.LISTMODE;
                    GetFieldValues(ControlsEnum.SEARCHTYPE);
                    SetFieldValues(ControlsEnum.SEARCHTYPE);
                    GetFieldValues(ControlsEnum.COMPANY);
                    SetFieldValues(ControlsEnum.COMPANY);
                    GetFieldValues(ControlsEnum.SEARCHPAYELEMENT);
                    SetFieldValues(ControlsEnum.SEARCHPAYELEMENT);
                    GetFieldValues(ControlsEnum.CURRENCY);
                    SetFieldValues(ControlsEnum.CURRENCY);
                    GetFieldValues(ControlsEnum.EXCHANGERATE);
                    SetFieldValues(ControlsEnum.EXCHANGERATE);

                    // From Payroll Preview Report
                    if (Request.QueryString[QueryStrings.FromExt] != null && Request.QueryString[QueryStrings.FromExt] == "T")
                    {
                        CurrPK = Convert.ToInt32(Request.QueryString[QueryStrings.PK]);
                        if (CurrPK > 0)
                        {
                            CurEMP_PK = Request.QueryString[QueryStrings.EMP_PK] != null ? Convert.ToInt32(Request.QueryString[QueryStrings.EMP_PK]) : 0;
                            SetUIEditView(commonActions);
                            GetFieldValues(ControlsEnum.ADDITIONDEDUCTIONBYPK);
                            SetFieldValues(ControlsEnum.ADDITIONDEDUCTIONBYPK);
                        }
                    }
                    else
                    {
                        #region else region
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
                                //if (pid.Equals("11"))
                                //    hdfIsInvCancelled.Value = "1";//For Showing Cancelled Stamp in Detail Page
                            }
                            else if (pid.Equals("2") || pid.Equals("12"))
                            {
                                //ucrWrkf.RefID = int.Parse(refID);
                                //JournalPK = GetApplicationID(ucrWrkf.RefID);
                                //GetFieldValues(ControlsEnum.GETEXPENSEPKBYJOURNALPK);
                                //if (finTrxHdrList != null && finTrxHdrList.Count == 1)
                                //{
                                //    CurrPK = (Int32)finTrxHdrList[0].FTH_REF_PK;
                                //    WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                                //    base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                                //}
                            }
                        }
                        else if (!string.IsNullOrEmpty(prefID))
                        {
                            base.WkfRefID = ucrWrkf.RefID = int.Parse(prefID);
                            CurrPK = GetApplicationID(ucrWrkf.RefID);
                        }
                        if (CurrPK > 0)
                        {
                            //SetCancelRef(CurrPK);
                            ucrWrkf.FillWorkFlowDetails();
                            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && (EntryStatus == EntryStatus.ENTRYMODE || EntryStatus == EntryStatus.NEWMODE) && ucrWrkf.HasPageTaskPermission)
                                ucrWrkf.ViewType = 1;
                            else
                            {
                                ucrWrkf.ViewType = 0;
                            }

                            GetFieldValues(ControlsEnum.ADDITIONDEDUCTIONBYPK);
                            SetFieldValues(ControlsEnum.ADDITIONDEDUCTIONBYPK);
                        }
                        else
                        {
                            uclPaging.CurrentPage = 1;
                            GetFieldValues(ControlsEnum.LISTPAGE);
                            SetFieldValues(ControlsEnum.LISTPAGE);
                            EntryStatus = EntryStatus.LISTMODE;
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion
        #region Functions
        protected void ActionHandler(object sender, EventArgs e)
        {
            //To check if department is different by opening in new tab
            if (!(this.Master as ERPSMS_2).ValidatePageDept())
                return;

            try
            {
                int? result;
                result = 0;
                bool bIsChecked = false;
                GridViewRow gdRow;
                HiddenField hdfitemPK;
                HiddenField hdfitemSlNo;
                DropDownList ddlWkfAction;
                TextBox WrkfComments;
                string action;
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
                else if (sender.GetType().IsEquivalentTo(typeof(RadioButton)))
                {
                    //if (((RadioButton)sender).ID == "rbtSelect")
                    //{
                    //    commonActions = ActionsEnum.ITEMSELECTED;
                    //}
                }
                else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    if (((DropDownList)sender).ID == "ddlType")
                    {
                        commonActions = ActionsEnum.TYPECHANGED;
                    }
                    else if (((DropDownList)sender).ID == "ddlSrchType")
                    {
                        commonActions = ActionsEnum.SEARCHTYPECHANGED;
                    }
                }
                else if (sender.GetType().IsEquivalentTo(typeof(TextBox)))
                {
                    //if (((TextBox)sender).ID == "txtProcessFromDate")
                    //{
                    //    commonActions = ActionsEnum.RESETPROCESSTODATE;
                    //}
                }

                switch (commonActions)
                {
                    #region SAVE
                    case ActionsEnum.SAVE:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else
                        {
                            objHeader = new AdditionDeductionHeader();
                            objHeader = (AdditionDeductionHeader)SetUIValuesToObject(ControlsEnum.ADDITIONDEDUCTIONHDR);
                            if (objHeader != null)
                            {
                                if (objHeader.EmpAddDedDtl != null && objHeader.EmpAddDedDtl.Count > 0)
                                {
                                    string TrxNo = string.Empty;
                                    objHeader.WKF_FLAG = 0;
                                    string xmlDoc = CommonFunctions.XmlSerialize<AdditionDeductionHeader>(objHeader);
                                    result = OtherAdditionDeductionBL.SaveAdditionDeductionDetails(xmlDoc, out TrxNo);
                                    if (result > 0)
                                    {
                                        lblTrxNo.Text = TrxNo;
                                        //litErrorMsg.Text = GetLocalResourceObject("Msg_Save_Success").ToString();
                                        //object[] args = new object[2];
                                        //args[0] = Resources.PageNameRes.EmpAddDeduction;
                                        //args[1] = TrxNo;
                                        //litErrorMsg.Text = string.Format(litErrorMsg.Text, args);
                                        litErrorMsg.Text = Resources.Messages.Msg_AddDedSave_Successfully;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.ErpRes.Information + "');", true);
                                        ResetForm(ControlsEnum.CLEARALL);
                                        EntryStatus = EntryStatus.LISTMODE;
                                        CurrPK = (int)result;
                                        GetFieldValues(ControlsEnum.LISTPAGE);
                                        SetFieldValues(ControlsEnum.LISTPAGE);
                                    }
                                    else
                                    {
                                        if (result == (int)DbSaveStatus.SQLERROR)
                                        {
                                            litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                        }
                                        else if (result == (int)DbSaveStatus.CONCURRENCY)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.EmpAddDeduction + " " + Resources.Messages.EditUsedByAnotherUser;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                        }
                                        else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.EmpAddDeduction + " " + Resources.Messages.AlreadyDeleted;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                            EntryStatus = EntryStatus.LISTMODE;
                                        }
                                        else if (result == (int)DbSaveStatus.CODEEXIST)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.EmpAddDeduction + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "','" + "');", true);
                                        }
                                        else if (result == (int)DbSaveStatus.INCORRECT)
                                        {
                                            litErrorMsg.Text = GetLocalResourceObject("Err_RecordExist").ToString();
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                            + "','" + Resources.Captions.Information + "','" + "');", true);
                                        }
                                        else
                                        {
                                            litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmpAddDeduction);
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                        }
                                    }
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.Err_AddEmpAddtnDedtn;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                }
                            }
                        }
                        break;
                    #endregion

                    #region SAVESUBMIT
                    case ActionsEnum.SAVESUBMIT:
                        //Show WorkFlow Popup
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
                            ucrWrkf.ApplicationID = 0;
                            if (hdfIsSaveSubmit.Value == CommonConstants.SELECT_VALUE_ONE)
                            {
                                objHeader = new AdditionDeductionHeader();
                                objHeader = (AdditionDeductionHeader)SetUIValuesToObject(ControlsEnum.ADDITIONDEDUCTIONHDR);
                                if (objHeader != null)
                                {
                                    if (objHeader.EmpAddDedDtl != null && objHeader.EmpAddDedDtl.Count > 0)
                                    {
                                        string TrxNo = string.Empty;
                                        objHeader.WKF_FLAG = 1;
                                        string xmlDoc = CommonFunctions.XmlSerialize<AdditionDeductionHeader>(objHeader);
                                        result = OtherAdditionDeductionBL.SaveAdditionDeductionDetails(xmlDoc, out TrxNo);
                                        if (result > 0)
                                        {
                                            lblTrxNo.Text = TrxNo;
                                            ucrWrkf.ApplicationID = result.Value;
                                        }
                                        else
                                        {
                                            if (result == (int)DbSaveStatus.SQLERROR)
                                            {
                                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                            }
                                            else if (result == (int)DbSaveStatus.CONCURRENCY)
                                            {
                                                litErrorMsg.Text = Resources.PageNameRes.EmpAddDeduction + " " + Resources.Messages.EditUsedByAnotherUser;
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                            }
                                            else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                            {
                                                litErrorMsg.Text = Resources.PageNameRes.EmpAddDeduction + " " + Resources.Messages.AlreadyDeleted;
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                                EntryStatus = EntryStatus.LISTMODE;
                                            }
                                            else if (result == (int)DbSaveStatus.CODEEXIST)
                                            {
                                                litErrorMsg.Text = Resources.PageNameRes.EmpAddDeduction + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "','" + "');", true);
                                            }
                                            else if (result == (int)DbSaveStatus.INCORRECT)
                                            {
                                                litErrorMsg.Text = GetLocalResourceObject("Err_RecordExist").ToString();
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                + "','" + Resources.Captions.Information + "','" + "');", true);
                                            }
                                            else
                                            {
                                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmpAddDeduction);
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        litErrorMsg.Text = Resources.Messages.Err_AddEmpAddtnDedtn;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                    }
                                }


                            }
                            else if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)
                            {
                                if (BusinessLogic.CommonManagement.CommonBL.ValidationForCancellation(CurrPK, ApplicationType.EMPADDDED))
                                {
                                    ucrWrkf.ApplicationID = CurrPK;
                                }
                                else
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Err_AddDed_Cancel").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                                    if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)
                                        FillProcessID(1);
                                    hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                                    WrkfComments.Text = "";
                                    ResetForm(ControlsEnum.CLEARALL);
                                    EntryStatus = EntryStatus.LISTMODE;
                                    GetFieldValues(ControlsEnum.LISTPAGE);
                                    SetFieldValues(ControlsEnum.LISTPAGE);

                                }
                            }
                            else
                                ucrWrkf.ApplicationID = CurrPK;
                            if (ucrWrkf.ApplicationID > 0)
                            {
                                ddlWkfAction = (DropDownList)ucrWrkf.FindControl("WRKFACT_ID");
                                WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                                //Do WorkFlow if WorkFlow has Actions
                                if (ddlWkfAction.Items.Count > 0)
                                {
                                    action = ddlWkfAction.SelectedItem.ToString();
                                    result = ucrWrkf.DoWorkFlow();
                                    if (result.HasValue && result.Value > 0)
                                    {

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
                                        WrkfComments.Text = "";

                                        object[] args = new object[2];
                                        args[0] = Resources.PageNameRes.EmpAddDeduction;
                                        args[1] = lblTrxNo.Text.Trim();
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, args);
                                        // Show Save Message and redired to listing page                                      
                                        if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && base.WkfRefID > 0)
                                        {
                                            ResetForm(ControlsEnum.CLEARALL);
                                            EntryStatus = EntryStatus.LISTMODE;
                                            CurrPK = (int)result;
                                            GetFieldValues(ControlsEnum.LISTPAGE);
                                            SetFieldValues(ControlsEnum.LISTPAGE);
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.InboxURL) + "');", true);
                                        }
                                        else
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                            ResetForm(ControlsEnum.CLEARALL);
                                            EntryStatus = EntryStatus.LISTMODE;
                                            CurrPK = (int)result;
                                            GetFieldValues(ControlsEnum.LISTPAGE);
                                            SetFieldValues(ControlsEnum.LISTPAGE);
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    #endregion

                    #region EDIT/DETAIL/VIEW
                    case ActionsEnum.EDIT:
                    case ActionsEnum.DETAIL:
                    case ActionsEnum.VIEW:
                        foreach (GridViewRow grdrow in grdAddDedList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                HiddenField hdfDept;
                                int dept;
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfAddDedPK")).Value);

                                hdfDept = grdrow.FindControl("hdfDept") as HiddenField;
                                if (hdfDept != null && int.TryParse(hdfDept.Value, out dept))
                                {
                                    Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                                    base.SetUserDept();
                                }
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
                            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                                ucrWrkf.ViewType = 1;
                            else
                            {
                                ucrWrkf.ViewType = 0;
                            }
                            ucrWrkf.ViewAction();
                            GetFieldValues(ControlsEnum.ADDITIONDEDUCTIONBYPK);
                            SetFieldValues(ControlsEnum.ADDITIONDEDUCTIONBYPK);
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Record").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion

                    #region EDITFORCANCEL
                    case ActionsEnum.EDITFORCANCEL:
                        foreach (GridViewRow grdrow in grdAddDedList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            HiddenField hdfDept;
                            int dept;
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfAddDedPK")).Value);
                                hdfDept = grdrow.FindControl("hdfDept") as HiddenField;
                                if (hdfDept != null && int.TryParse(hdfDept.Value, out dept))
                                {
                                    Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                                    base.SetUserDept();
                                }
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            ucrWrkf.Reset();
                            FillProcessID(11);
                            SetUIEditView(commonActions);
                            GetFieldValues(ControlsEnum.ADDITIONDEDUCTIONBYPK);
                            SetFieldValues(ControlsEnum.ADDITIONDEDUCTIONBYPK);
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
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Record").ToString();
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

                    #region DELETE
                    case ActionsEnum.DELETE:
                        result = OtherAdditionDeductionBL.DeleteAdditionDeduction(this.CurrPK, Convert.ToString(LastModifiedTime));
                        if (result > 0)
                        {
                            EntryStatus = EntryStatus.LISTMODE;
                            ResetForm(ControlsEnum.CLEARALL);
                            GetFieldValues(ControlsEnum.LISTPAGE);
                            SetFieldValues(ControlsEnum.LISTPAGE);
                            btnNew.Focus();
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmpAddDeduction);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else
                        {
                            if (result == (int)DbSaveStatus.REFERRED)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.EmpAddDeduction;
                                litErrorMsg.Text += " " + GetGlobalResourceObject("Messages", "UsedInAnotherPlace").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.SQLERROR)
                            {
                                litErrorMsg.Text = GetGlobalResourceObject("Messages", "ActionFailedPleaseTryAgain").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CONCURRENCY)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.EmpAddDeduction + " " +
                                    GetGlobalResourceObject("Messages", "EditUsedByAnotherUser").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CODEEXIST)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.EmpAddDeduction + " " +
                                    GetGlobalResourceObject("Messages", "Itemsalreadyaddedbyanotheruser").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.ALREADYDELETED)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.EmpAddDeduction + " " +
                                    GetGlobalResourceObject("Messages", "AlreadyDeleted").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = GetGlobalResourceObject("Messages", "ActionFailedPleaseTryAgain").ToString();
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmpAddDeduction);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        break;
                    #endregion

                    #region PRINTLISTING

                    case ActionsEnum.PRINTLISTING:
                        foreach (GridViewRow grdrow in grdAddDedList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfAddDedPK")).Value);
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportPath + "?ID=0" + "&CurPK=" + CurrPK + "&APPTYPE=" + "EMPADDDED" + "&APPSUBTYPE= 0") + "&ISEXCELPRINT= 1" + "');", true);
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Record").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;

                    #endregion

                    #region PRINT

                    case ActionsEnum.PRINT:
                        if (CurrPK > 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportPath + "?ID=0" + "&CurPK=" + CurrPK + "&APPTYPE=" + "EMPADDDED" + "&APPSUBTYPE= 0") + "&ISEXCELPRINT= 1" + "');", true);
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Record").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;

                    #endregion

                    #region LIST
                    case ActionsEnum.LIST:
                        FillProcessID(1);
                        CurrPK = 0;
                        uclPaging.CurrentPage = 0;
                        this.PageIndexList = "1";
                        EntryStatus = EntryStatus.LISTMODE;
                        ResetForm(ControlsEnum.CLEARALL);
                        hdfAdvSearch.Value = "1";
                        ddlSrchType.ClearSelection();
                        ddlStatus.ClearSelection();
                        ddlSrchPayElement.ClearSelection();
                        txtSrchFromDate.Text = string.Empty;
                        txtSrchToDate.Text = string.Empty;
                        txtSearchName.Text = string.Empty;
                        txtTrxNo.Text = string.Empty;
                        hdfTrxPk.Value = string.Empty;
                        GetFieldValues(ControlsEnum.LISTPAGE);
                        SetFieldValues(ControlsEnum.LISTPAGE);
                        break;
                    #endregion

                    #region NEW
                    case ActionsEnum.NEW:
                        EntryStatus = EntryStatus.NEWMODE;
                        ddlType.Enabled = true;
                        ddlPayElement.Enabled = true;
                        ResetForm(ControlsEnum.CLEARALL);
                        GetFieldValues(ControlsEnum.TYPE);
                        SetFieldValues(ControlsEnum.TYPE);
                        GetFieldValues(ControlsEnum.PAYELEMENT);
                        SetFieldValues(ControlsEnum.PAYELEMENT);
                        GetFieldValues(ControlsEnum.CURRENCY);
                        SetFieldValues(ControlsEnum.CURRENCY);
                        GetFieldValues(ControlsEnum.EXCHANGERATE);
                        SetFieldValues(ControlsEnum.EXCHANGERATE);
                        GetFieldValues(ControlsEnum.COMPANY);
                        SetFieldValues(ControlsEnum.COMPANY);

                        GetFieldValues(ControlsEnum.BRANCH);
                        SetFieldValues(ControlsEnum.BRANCH);
                        GetFieldValues(ControlsEnum.EMPLOYEETYPE);
                        SetFieldValues(ControlsEnum.EMPLOYEETYPE);
                        GetFieldValues(ControlsEnum.EMPLOYMENTTYPE);
                        SetFieldValues(ControlsEnum.EMPLOYMENTTYPE);
                        GetFieldValues(ControlsEnum.PAYMENTMODE);
                        SetFieldValues(ControlsEnum.PAYMENTMODE);
                        SetFieldValues(ControlsEnum.EMPDETAILS);
                        base.WkfRefID = ucrWrkf.RefID = 0;
                        //SetCancelRef(CurrPK);
                        ucrWrkf.FillWorkFlowDetails();
                        ucrWrkf.ViewType = 1;
                        ucrWrkf.ViewAction();
                        break;
                    #endregion

                    #region ADD TO LIST
                    case ActionsEnum.ADDTOLIST:
                        int employee = 0;
                        int.TryParse(hdfEmployee.Value, out employee);
                        if (grdEmpAddDedList.Rows.Count > 0 && employee <= 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Employee").ToString()) + "','" + Resources.Captions.Information + "');", true);
                            return;
                        }
                        else if (grdEmpAddDedList.Rows.Count <= 0 && Convert.ToInt32(ddlBranchLocation.SelectedValue) <= 0 && employee <= 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_EmployeeOrBranch").ToString()) + "','" + Resources.Captions.Information + "');", true);
                            return;
                        }
                        GetFieldValues(ControlsEnum.GETADDITIONDEDUCTION);
                        if (objEmpAddDedDtlsList != null && objEmpAddDedDtlsList.Count > 0)
                        {
                            List<AdditionDeductionDetails> objDtlsList = EmpAddDedDetailList;
                            if (objDtlsList == null)
                                objDtlsList = new List<AdditionDeductionDetails>();
                            if (employee > 0 && objDtlsList.Where(r => r.OAD_DATE == Convert.ToDateTime(txtAddDedDate.Text) && r.OAD_EMPLOYEE == employee).Count() > 0)
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_RecordExist").ToString()) + "','" + Resources.Captions.Information + "');", true);
                                return;
                            }
                            objDtlsList.AddRange(objEmpAddDedDtlsList);
                            EmpAddDedDetailList = objDtlsList;
                            SetFieldValues(ControlsEnum.EMPDETAILS);
                            ResetForm(ControlsEnum.CLEARADDTOLIST);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_NoEmployee").ToString()) + "','" + Resources.Captions.Information + "');", true);
                        }
                        break;
                    #endregion

                    #region EDIT ITEM
                    case ActionsEnum.EDITITEM:
                        gdRow = ((ImageButton)sender).Parent.Parent as GridViewRow;
                        hdfitemPK = (HiddenField)grdEmpAddDedList.Rows[gdRow.RowIndex].FindControl("hdfitemPK");
                        hdfitemSlNo = (HiddenField)grdEmpAddDedList.Rows[gdRow.RowIndex].FindControl("hdfitemSlNo");
                        var editItem = EmpAddDedDetailList.Where(itm => itm.OAD_PK == Convert.ToInt32(hdfitemPK.Value) && itm.SlNo == Convert.ToInt32(hdfitemSlNo.Value)).SingleOrDefault();
                        if (editItem != null)
                        {
                            CurrEmpAddDedPK = editItem.OAD_PK;
                            hdfAddDedSlNo.Value = editItem.SlNo.ToString();
                            txtAddDedDate.Text = Convert.ToDateTime(editItem.OAD_DATE).ToString(Resources.Constants.HRMSDateFormatShort);
                            txtAmount.Text = editItem.OAD_AMOUNT.ToString();
                            if (editItem.EmpBranch > 0)
                            {
                                ddlBranchLocation.SelectedValue = editItem.EmpBranch.ToString();
                            }
                            hdfEmployee.Value = editItem.OAD_EMPLOYEE.ToString();
                            txtEmployee.Text = editItem.OAD_EMPLOYEE_NAME;
                            txtRemarks.Text = editItem.OAD_REMARKS;
                        }
                        break;
                    #endregion

                    #region REMOVE ITEM
                    case ActionsEnum.REMOVEITEM:

                        if (EmpAddDedDetailList != null && EmpAddDedDetailList.Count > 0)
                        {
                            gdRow = ((ImageButton)sender).Parent.Parent as GridViewRow;
                            //hdfitemPK = (HiddenField)grdEmpAddDedList.Rows[gdRow.RowIndex].FindControl("hdfitemPK");
                            //hdfitemSlNo = (HiddenField)grdEmpAddDedList.Rows[gdRow.RowIndex].FindControl("hdfitemSlNo");                           
                            HiddenField hdfgrdEmpPk = (HiddenField)gdRow.FindControl("hdfgrdEmpPk");
                            TextBox txtEmpDate = (TextBox)gdRow.FindControl("txtEmpDate");
                            EmpAddDedDetailList = (List<AdditionDeductionDetails>)SetUIValuesToObject(ControlsEnum.ADDITIONDEDUCTIONDTL);
                            //var removeItem = EmpAddDedDetailList.Where(itm => itm.OAD_PK == Convert.ToInt32(hdfitemPK.Value) && itm.SlNo == Convert.ToInt32(hdfitemSlNo.Value)).SingleOrDefault();
                            var removeItem = EmpAddDedDetailList.Where(itm => itm.OAD_EMPLOYEE == Convert.ToInt32(hdfgrdEmpPk.Value) && itm.OAD_DATE == DateTime.Parse(txtEmpDate.Text)).SingleOrDefault();
                            if (removeItem != null)
                            {
                                EmpAddDedDetailList.Remove(removeItem);
                                SetFieldValues(ControlsEnum.EMPDETAILS);
                                ResetForm(ControlsEnum.CLEARADDTOLIST);
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailed;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        break;
                    #endregion

                    #region DELETE ALL
                    case ActionsEnum.DELETEALL:
                        int count = 0;
                        if (EmpAddDedDetailList != null && EmpAddDedDetailList.Count > 0)
                        {
                            foreach (GridViewRow grow in grdEmpAddDedList.Rows)
                            {
                                CheckBox chkEmpselect = (CheckBox)grow.FindControl("chkEmpselect");

                                if (chkEmpselect.Checked && chkEmpselect.Enabled)
                                {
                                    count++;
                                    HiddenField hdfgrdEmpPk = (HiddenField)grow.FindControl("hdfgrdEmpPk");
                                    TextBox txtEmpDate = (TextBox)grow.FindControl("txtEmpDate");
                                    EmpAddDedDetailList = (List<AdditionDeductionDetails>)SetUIValuesToObject(ControlsEnum.ADDITIONDEDUCTIONDTL);
                                    var removeItem = EmpAddDedDetailList.Where(itm => itm.OAD_EMPLOYEE == Convert.ToInt32(hdfgrdEmpPk.Value) && itm.OAD_DATE == DateTime.Parse(txtEmpDate.Text)).SingleOrDefault();
                                    if (removeItem != null)
                                    {
                                        EmpAddDedDetailList.Remove(removeItem);
                                    }
                                    else
                                    {
                                        litErrorMsg.Text = Resources.Messages.ActionFailed;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                }

                            }
                            SetFieldValues(ControlsEnum.EMPDETAILS);
                            ResetForm(ControlsEnum.CLEARADDTOLIST);
                        }
                        if (count == 0)
                        {
                            litErrorMsg.Text = Resources.Messages.Err_SlctEmpAddtnDedtnDel;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        break;
                    #endregion

                    #region CLEAR
                    case ActionsEnum.CLEAR:
                        ResetForm(ControlsEnum.EMPDETAILS);
                        break;
                    #endregion

                    #region TYPE CHANGED
                    case ActionsEnum.TYPECHANGED:
                        GetFieldValues(ControlsEnum.PAYELEMENT);
                        SetFieldValues(ControlsEnum.PAYELEMENT);
                        break;
                    #endregion

                    #region SEARCH TYPE CHANGED
                    case ActionsEnum.SEARCHTYPECHANGED:
                        GetFieldValues(ControlsEnum.SEARCHPAYELEMENT);
                        SetFieldValues(ControlsEnum.SEARCHPAYELEMENT);
                        hdfAdvSearch.Value = "1";
                        break;
                    #endregion

                    #region CANCEL
                    case ActionsEnum.CANCEL:
                        FillProcessID(1);
                        EntryStatus = EntryStatus.LISTMODE;
                        ResetForm(ControlsEnum.CLEARALL);
                        GetFieldValues(ControlsEnum.LISTPAGE);
                        SetFieldValues(ControlsEnum.LISTPAGE);
                        break;
                    #endregion

                    #region CLEARSEARCH
                    case ActionsEnum.CLEARSEARCH:
                        uclPaging.CurrentPage = 0;
                        this.PageIndexList = "1";
                        hdfAdvSearch.Value = "1";
                        ddlSrchType.ClearSelection();
                        ddlStatus.ClearSelection();
                        ddlSrchPayElement.ClearSelection();
                        txtSrchFromDate.Text = string.Empty;
                        txtSrchToDate.Text = string.Empty;
                        txtSearchName.Text = string.Empty;
                        txtEmpName.Text = string.Empty;
                        hdfEmpName.Value = string.Empty;
                        txtTrxNo.Text = string.Empty;
                        hdfTrxPk.Value = string.Empty;
                        GetFieldValues(ControlsEnum.LISTPAGE);
                        SetFieldValues(ControlsEnum.LISTPAGE);
                        break;
                    #endregion

                    #region SEARCH
                    case ActionsEnum.SEARCH:
                        uclPaging.CurrentPage = 0;
                        this.PageIndexList = "1";
                        hdfAdvSearch.Value = "1";
                        GetFieldValues(ControlsEnum.LISTPAGE);
                        SetFieldValues(ControlsEnum.LISTPAGE);
                        break;
                    #endregion

                    #region CURRENCYSELECTED
                    case ActionsEnum.CURRENCYSELECTED:
                        GetFieldValues(ControlsEnum.EXCHANGERATE);
                        SetFieldValues(ControlsEnum.EXCHANGERATE);
                        break;
                    #endregion

                    #region IMPORT
                    case ActionsEnum.IMPORT:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else
                        {
                            if (fupImport.HasFile)
                            {
                                string conStr;
                                string filePath = SaveDetails(out conStr, fupImport);
                                if (!string.IsNullOrEmpty(filePath))
                                {
                                    ImportToGrid(filePath, conStr);
                                }
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_InvalidFile").ToString())
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }

                        }
                        break;
                    #endregion                    

                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        /// <summary>
        /// Methode used to read the excel data and save into grid
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="conStr"></param>
        private void ImportToGrid(string filePath, string conStr)
        {
            try
            {
                StringBuilder sbImport = new StringBuilder();
                string SheetName = CommonConstants.EXELSHEETNAME.ToLower();
                excelColumns = airColums_General;
                conStr = String.Format(conStr, filePath);
                connExcel = new OleDbConnection(conStr);
                cmdExcel = new OleDbCommand();
                oleDbDataAdapter = new OleDbDataAdapter();
                dtExcelSchema = new DataTable();
                dsImportdata = new DataSet();
                cmdExcel.Connection = connExcel;
                connExcel.Open();
                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                if (dtExcelSchema != null && dtExcelSchema.Rows.Count > 0)
                {
                    if (dtExcelSchema != null)
                    {
                        landingSheet = string.Empty;
                        foreach (DataRow dr in dtExcelSchema.Rows)
                        {
                            if (dr["TABLE_NAME"].ToString().ToLower() == SheetName.ToLower())
                            {
                                landingSheet = dr["TABLE_NAME"].ToString();
                            }
                        }
                        if (landingSheet == string.Empty)
                        {
                            litErrorMsg.Text = this.GetLocalResourceObject("Err_ExelsheetName").ToString();
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, SheetName.Replace("$", ""));
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                            return;
                        }
                    }
                    cmdExcel.CommandText = "SELECT * From [" + landingSheet + "]";
                    oleDbDataAdapter.SelectCommand = cmdExcel;
                    oleDbDataAdapter.Fill(dsImportdata, "Landing");
                    connExcel.Close();
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }
                else
                {
                    throw new Exception(GetLocalResourceObject("Err_IncorrectFormat").ToString());
                }
                if (dsImportdata != null && dsImportdata.Tables.Count > 0)
                {

                    foreach (DataColumn item in dsImportdata.Tables[0].Columns)
                    {
                        item.ColumnName = item.ColumnName.Replace(" ", "");
                    }
                    int cnt = (from p in excelColumns
                               where this.dsImportdata.Tables[0].Columns.Contains(p)
                               select p).Count();
                    if (cnt != excelColumns.Length)
                    {
                        sb = new StringBuilder();
                        sb.Append(this.GetLocalResourceObject("Err_ExcelSheet").ToString());
                        if (excelColumns != null && excelColumns.Count() > 0)
                        {
                            foreach (string item in excelColumns)
                            {
                                sb.Append("<ul><li>" + item + "</li></ul>");
                            }
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(sb.ToString())
                                           + "','" + Resources.ErpRes.Information + "');", true);
                        return;
                    }

                    DataTable dtImportData = dsImportdata.Tables[0];
                    AddDedImportHeader objImport = new AddDedImportHeader();
                    objImport = (AddDedImportHeader)SetUIValuesToObject(ControlsEnum.IMPORT);
                    List<AddDedImportDetails> objDetList = new List<AddDedImportDetails>();
                    DataColumnCollection importColumns = dtImportData.Columns;
                    for (int i = 0; i < dtImportData.Rows.Count; i++)
                    {
                        string date = Convert.ToString(dtImportData.Rows[i]["Date"]);
                        if (!string.IsNullOrEmpty(date.Trim()))
                        {
                            double Amount = 0;
                            AddDedImportDetails objDetails = new AddDedImportDetails();
                            objDetails.OAD_DATE = Convert.ToDateTime(date);
                            if (importColumns.Contains("EmpCode"))
                            {
                                objDetails.OAD_EMPLOYEE_CODE = HttpUtility.HtmlEncode(Convert.ToString(dtImportData.Rows[i]["EmpCode"]).Trim());
                            }
                            if (importColumns.Contains("Amount") && double.TryParse(Convert.ToString(dtImportData.Rows[i]["Amount"]).Trim(), out Amount))
                            {
                                objDetails.OAD_AMOUNT = Amount;
                            }
                            if (importColumns.Contains("Remarks"))
                            {
                                objDetails.OAD_REMARKS = HttpUtility.HtmlEncode(Convert.ToString(dtImportData.Rows[i]["Remarks"]).Trim());
                            }
                            if (!string.IsNullOrEmpty(objDetails.OAD_EMPLOYEE_CODE))
                                objDetList.Add(objDetails);
                        }
                    }

                    objImport.EmpAddDedDetails = objDetList;
                    xmlLanding = CommonFunctions.XmlSerialize(objImport);
                    DataTable dtOut = null;
                    int result = OtherAdditionDeductionBL.AdditionDeductionImport(xmlLanding, ref dtOut);
                    if (result > 0 && dsImportdata.Tables.Count > 0)
                    {
                        CurrPK = result;
                        FillProcessID(1);
                        SetUIEditView(commonActions);
                        WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                        base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                        SetCancelRef(CurrPK);
                        ucrWrkf.FillWorkFlowDetails();
                        if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                            ucrWrkf.ViewType = 1;
                        else
                            ucrWrkf.ViewType = 0;
                        ucrWrkf.ViewAction();
                        GetFieldValues(ControlsEnum.ADDITIONDEDUCTIONBYPK);
                        SetFieldValues(ControlsEnum.ADDITIONDEDUCTIONBYPK);
                        litErrorMsg.Text = this.GetLocalResourceObject("ImportSuccess").ToString();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                            + "','" + Resources.ErpRes.Information + "');", true);
                    }
                    else if (result == (int)DbImportStatus.SQLERROR)
                    {
                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                            + "','" + Resources.ErpRes.Information + "');", true);
                    }
                    else if (result == (int)DbImportStatus.CONCURRENCY)
                    {
                        litErrorMsg.Text = Resources.PageNameRes.Attendance + " " + Resources.Messages.EditUsedByAnotherUser;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                        + "','" + Resources.ErpRes.Information + "');", true);
                    }
                    else if (result == (int)DbImportStatus.NOEXELROWS)
                    {
                        litErrorMsg.Text = this.GetLocalResourceObject("Err_NoExelRows").ToString();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                    }
                    else if (result == (int)DbImportStatus.ALREADYEXISTS)
                    {
                        litErrorMsg.Text = this.GetLocalResourceObject("Err_AlreadyExists").ToString();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                            + "','" + Resources.ErpRes.Information + "');", true);
                    }
                    else if (result == (int)DbImportStatus.EMPLOYEENOTFOUND)
                    {
                        litErrorMsg.Text = this.GetLocalResourceObject("Err_EmployeeNotFound").ToString();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                        + "','" + Resources.ErpRes.Information + "');", true);
                    }
                    else
                    {
                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.Attendance);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                            + "','" + Resources.ErpRes.Information + "');", true);
                    }
                }
            }
            catch (OleDbException ex)
            {
                litErrorMsg.Text = CommonFunctions.ProcessException(ex);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
            }
            catch (Exception ex)
            {
                litErrorMsg.Text = CommonFunctions.ProcessException(ex);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
            }
        }

        /// <summary>
        /// Methode used to save the excel file
        /// </summary>
        private string SaveDetails(out string conStr, FileUpload fupUpload)
        {
            uploadPath = string.Empty;
            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
            {
                uploadPath = HttpContext.Current.Request.PhysicalApplicationPath + "Upload\\AdditionDeductionImports";
                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);
                uploadPath = HttpContext.Current.Request.PhysicalApplicationPath + "Upload\\AdditionDeductionImports\\";
            }
            else
            {
                uploadPath = System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + "AdditionDeductionImports";
                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);
                uploadPath = System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + "AdditionDeductionImports\\";
            }

            FileInfo tempFileInfoObj;
            tempFileInfoObj = new FileInfo(fupUpload.PostedFile.FileName);
            string attachmentFileFormat = tempFileInfoObj.Extension;
            string FileName = Guid.NewGuid().ToString() + attachmentFileFormat;
            conStr = CheckValidFileType(attachmentFileFormat);
            if (attachmentFileFormat.ToLower() != ".xls" && attachmentFileFormat.ToLower() != ".xlsx")
            {
                return string.Empty;
            }

            if (!string.IsNullOrEmpty(conStr))
            {
                fupUpload.SaveAs(uploadPath + FileName);
            }
            return uploadPath + FileName;
        }

        /// <summary>
        /// Check File is valid or not , using File Extension , if valid then get the connection string
        /// </summary>
        /// <param name="extn"></param>
        /// <returns>bool : True - valid File, false - Invalid File</returns>
        private string CheckValidFileType(string extn)
        {
            string conStr;
            switch (extn.ToLower())
            {
                case ".xls": //Excel 97-03
                    conStr = ConfigurationManager.AppSettings["Excel03ConString"];
                    break;
                case ".xlsx": //Excel 07
                    conStr = ConfigurationManager.AppSettings["Excel07ConString"];
                    break;
                default: conStr = string.Empty; break;
            }
            return conStr;
        }

        private Object SetUIValuesToObject(ControlsEnum controlType)
        {
            Object retObject;
            retObject = null;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            AddDedImportHeader objImportHeader;
            try
            {
                switch (controlType)
                {
                    #region ADDITION / DEDUCTION HDR
                    case ControlsEnum.ADDITIONDEDUCTIONHDR:
                        objHeader.OAH_PK = CurrPK;
                        objHeader.OAH_NO = lblTrxNo.Text;
                        objHeader.OAH_DATE_FROM = txtFromDate.Text;
                        objHeader.OAH_DATE_TO = txtToDate.Text;
                        objHeader.OAH_CLASS = Convert.ToInt32(ddlType.SelectedValue);
                        objHeader.OAH_PAY_ELEMENT = Convert.ToInt32(ddlPayElement.SelectedValue);
                        objHeader.OAH_DESC = string.IsNullOrEmpty(txtDescription.Text.Trim()) ? null : HttpUtility.HtmlEncode(txtDescription.Text.Trim());
                        objHeader.OAH_DISP_NAME = string.IsNullOrEmpty(txtName.Text.Trim()) ? null : HttpUtility.HtmlEncode(txtName.Text.Trim());
                        objHeader.OAH_DEPT = Convert.ToInt16(currentUser.CurrentDeptPK);
                        objHeader.OAH_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                        objHeader.OAH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        objHeader.USER_PK = Convert.ToInt16(currentUser.PKUser);
                        objHeader.OAH_CURRENCY = Convert.ToInt32(hdfCurrency.Value);
                        objHeader.OAH_BASE_CURR = currentUser.BaseCurrency <= 0 ? 1 : currentUser.BaseCurrency;
                        objHeader.OAH_EXCHG_RATE = string.IsNullOrEmpty(txtExchangeRate.Text.Trim()) ? 1 : Convert.ToDouble(txtExchangeRate.Text.Trim());
                        objHeader.LAST_MOD_DT = LastModifiedTime;
                        objHeader.OAH_COMPANY = Convert.ToInt32(ddlCompany.SelectedValue);
                        objHeader.EmpAddDedDtl = (List<AdditionDeductionDetails>)SetUIValuesToObject(ControlsEnum.ADDITIONDEDUCTIONDTL); //EmpAddDedDetailList;
                        retObject = objHeader;
                        break;
                    #endregion
                    #region ADDITION / DEDUCTION DETAILS
                    case ControlsEnum.ADDITIONDEDUCTIONDTL:
                        List<AdditionDeductionDetails> AttendanceList = EmpAddDedDetailList;
                        if (AttendanceList != null && AttendanceList.Count > 0)
                        {
                            AdditionDeductionDetails AttendanceEntry;
                            foreach (GridViewRow grdrow in grdEmpAddDedList.Rows)
                            {
                                TextBox txtgrdAmount = (TextBox)grdrow.FindControl("txtgrdAmount");
                                TextBox txtgrdRemarks = (TextBox)grdrow.FindControl("txtgrdRemarks");
                                HiddenField hdfgrdEmpPk = (HiddenField)grdrow.FindControl("hdfgrdEmpPk");
                                TextBox txtEmpDate = (TextBox)grdrow.FindControl("txtEmpDate");
                                HiddenField hdfitemPK = (HiddenField)grdrow.FindControl("hdfitemPK");
                                if (EntryStatus == EntryStatus.NEWMODE)
                                    AttendanceEntry = AttendanceList.SingleOrDefault(r => r.OAD_EMPLOYEE == Convert.ToInt32(hdfgrdEmpPk.Value) && r.OAD_DATE == DateTime.Parse(txtEmpDate.Text));
                                else
                                    AttendanceEntry = AttendanceList.SingleOrDefault(r => r.OAD_EMPLOYEE == Convert.ToInt32(hdfgrdEmpPk.Value) && r.OAD_PK == Convert.ToInt32(hdfitemPK.Value));

                                if (AttendanceEntry != null)
                                {
                                    AttendanceEntry.OAD_ACTIVE = (int)DbActiveStatus.ACTIVE;
                                    AttendanceEntry.OAD_DATE = Convert.ToDateTime(txtEmpDate.Text);
                                    AttendanceEntry.OAD_AMOUNT = double.Parse(txtgrdAmount.Text);
                                    AttendanceEntry.OAD_REMARKS = string.IsNullOrEmpty(txtgrdRemarks.Text.Trim()) ? null : HttpUtility.HtmlEncode(txtgrdRemarks.Text);
                                }
                            }
                            EmpAddDedDetailList = AttendanceList;
                            retObject = EmpAddDedDetailList;
                        }
                        break;
                    #endregion
                    #region IMPORT
                    case ControlsEnum.IMPORT:
                        objImportHeader = new AddDedImportHeader();
                        objImportHeader.OAH_PK = CurrPK;
                        objImportHeader.OAH_NO = lblTrxNo.Text;
                        objImportHeader.OAH_DATE_FROM = txtFromDate.Text;
                        objImportHeader.OAH_DATE_TO = txtToDate.Text;
                        objImportHeader.OAH_CLASS = Convert.ToInt32(ddlType.SelectedValue);
                        objImportHeader.OAH_PAY_ELEMENT = Convert.ToInt32(ddlPayElement.SelectedValue);
                        objImportHeader.OAH_DESC = string.IsNullOrEmpty(txtDescription.Text.Trim()) ? null : HttpUtility.HtmlEncode(txtDescription.Text.Trim());
                        objImportHeader.OAH_DISP_NAME = string.IsNullOrEmpty(txtName.Text.Trim()) ? null : HttpUtility.HtmlEncode(txtName.Text.Trim());
                        objImportHeader.OAH_DEPT = Convert.ToInt16(currentUser.CurrentDeptPK);
                        objImportHeader.OAH_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                        objImportHeader.OAH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        objImportHeader.USER_PK = Convert.ToInt16(currentUser.PKUser);
                        objImportHeader.OAH_CURRENCY = Convert.ToInt32(hdfCurrency.Value);
                        objImportHeader.OAH_BASE_CURR = currentUser.BaseCurrency <= 0 ? 1 : currentUser.BaseCurrency;
                        objImportHeader.OAH_EXCHG_RATE = string.IsNullOrEmpty(txtExchangeRate.Text.Trim()) ? 1 : Convert.ToDouble(txtExchangeRate.Text.Trim());
                        objImportHeader.LAST_MOD_DT = LastModifiedTime;
                        objImportHeader.OAH_COMPANY = Convert.ToInt32(ddlCompany.SelectedValue);
                        retObject = objImportHeader;
                        break;
                    #endregion
                }
                return retObject;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        private void GetUIValuesFromObject(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region ADDITION DEDUCTION BY PK
                    case ControlsEnum.ADDITIONDEDUCTIONBYPK:
                        if (objHeader != null)
                        {
                            lblTrxNo.Text = string.IsNullOrEmpty(objHeader.OAH_NO) ? Resources.ErpRes.Draft : objHeader.OAH_NO;
                            txtFromDate.Text = Convert.ToDateTime(objHeader.OAH_DATE_FROM).ToString(Resources.Constants.HRMSDateFormatShort);
                            txtToDate.Text = Convert.ToDateTime(objHeader.OAH_DATE_TO).ToString(Resources.Constants.HRMSDateFormatShort);
                            txtCurrency.Text = string.Format(GetLocalResourceObject("CurrencyDisplayFormat").ToString(), Convert.ToString(objHeader.OAH_CURRENCY_CODE_TEXT), HttpUtility.HtmlDecode(objHeader.OAH_CURRENCY_NAME_TEXT));
                            hdfCurrency.Value = objHeader.OAH_CURRENCY.ToString();
                            txtExchangeRate.Text = GetFormattedExchangerate(objHeader.OAH_EXCHG_RATE);
                            CompanyPk = objHeader.OAH_COMPANY;
                            GetFieldValues(ControlsEnum.COMPANY);
                            SetFieldValues(ControlsEnum.COMPANY);
                            ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(CompanyPk.ToString()));
                            txtAddDedDate.Focus();
                            GetFieldValues(ControlsEnum.TYPE);
                            SetFieldValues(ControlsEnum.TYPE);
                            ddlType.SelectedValue = objHeader.OAH_CLASS.ToString();
                            GetFieldValues(ControlsEnum.PAYELEMENT);
                            SetFieldValues(ControlsEnum.PAYELEMENT);
                            ddlPayElement.SelectedValue = objHeader.OAH_PAY_ELEMENT.ToString();
                            txtDescription.Text = HttpUtility.HtmlDecode(objHeader.OAH_DESC);
                            txtName.Text = HttpUtility.HtmlDecode(objHeader.OAH_DISP_NAME);
                            LastModifiedTime = objHeader.LAST_MOD_DT;
                            GetFieldValues(ControlsEnum.BRANCH);
                            SetFieldValues(ControlsEnum.BRANCH);
                            GetFieldValues(ControlsEnum.EMPLOYEETYPE);
                            SetFieldValues(ControlsEnum.EMPLOYEETYPE);
                            GetFieldValues(ControlsEnum.EMPLOYMENTTYPE);
                            SetFieldValues(ControlsEnum.EMPLOYMENTTYPE);
                            GetFieldValues(ControlsEnum.PAYMENTMODE);
                            SetFieldValues(ControlsEnum.PAYMENTMODE);
                            EmpAddDedDetailList = objHeader.EmpAddDedDtl;
                            SetFieldValues(ControlsEnum.EMPDETAILS);
                            // Enable/Disable dropdown base on OAH_PAYROLL_DTL value
                            if (objHeader.OAH_PAYROLL_DTL > 0)
                            {
                                ddlType.Enabled = false;
                                ddlPayElement.Enabled = false;
                            }
                            else
                            {
                                ddlType.Enabled = true;
                                ddlPayElement.Enabled = true;
                            }

                        }
                        break;
                    #endregion
                    #region EXCHANGE RATE
                    case ControlsEnum.EXCHANGERATE:
                        if (dsExchangeRate != null && dsExchangeRate.Tables[0].Rows.Count > 0)
                        {
                            if (Convert.ToDouble(dsExchangeRate.Tables[0].Rows[0][0]) > 0)
                                txtExchangeRate.Text = GetFormattedExchangerate(dsExchangeRate.Tables[0].Rows[0][0].ToString());
                            else
                                txtExchangeRate.Text = string.Empty;
                        }
                        else
                        {
                            txtExchangeRate.Text = string.Empty;
                        }
                        //  Exchange rate field is not editable(Domestic).ie,If selected currency is same as SBU base currency
                        if (currentUser.BaseCurrency == Convert.ToInt32(hdfCurrency.Value))
                            txtExchangeRate.Enabled = false;
                        else
                            txtExchangeRate.Enabled = true;
                        break;
                    #endregion
                    #region CURRENCY
                    case ControlsEnum.CURRENCY:
                        if (dtResult != null && dtResult.Rows.Count > 0)
                        {
                            txtCurrency.Text = string.Format(GetLocalResourceObject("CurrencyDisplayFormat").ToString(), Convert.ToString(dtResult.Rows[0]["CUR_CODE"]), Convert.ToString(dtResult.Rows[0]["CUR_NAME"]));
                            hdfCurrency.Value = Convert.ToString(dtResult.Rows[0]["CUR_PK"]);
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

        private void GetFieldValues(ControlsEnum type)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            FilterParameters objFilterParams;
            try
            {
                switch (type)
                {
                    #region TYPE
                    case ControlsEnum.TYPE:
                    case ControlsEnum.SEARCHTYPE:
                        dtResult = CommonBL.GetAppConfig(currentUser.SBUID, GetLocalResourceObject("ClassificationType").ToString(), GetLocalResourceObject("AddDed").ToString());
                        break;
                    #endregion
                    #region PAY ELEMENT
                    case ControlsEnum.PAYELEMENT:
                        int elmntType = string.IsNullOrEmpty(ddlType.SelectedValue) ? 0 : Convert.ToInt32(ddlType.SelectedValue);
                        dtResult = BusinessLogic.HRMS.Admin.Masters.PayElementsMasterBL.GetParentElement(0, Convert.ToInt32(CommonConstants.ACTIVE), currentUser.SBUID, 0, 0, elmntType > 0 ? elmntType : 0);
                        break;
                    #endregion
                    #region SEARCH PAY ELEMENT
                    case ControlsEnum.SEARCHPAYELEMENT:
                        int srchType = string.IsNullOrEmpty(ddlSrchType.SelectedValue) ? 0 : Convert.ToInt32(ddlSrchType.SelectedValue);
                        dtResult = BusinessLogic.HRMS.Admin.Masters.PayElementsMasterBL.GetParentElement(0, Convert.ToInt32(CommonConstants.ACTIVE), currentUser.SBUID, 0, 0, srchType > 0 ? srchType : 0);
                        break;
                    #endregion
                    #region BRANCH / LOCATION
                    case ControlsEnum.BRANCH:
                        dtResult = BusinessLogic.HRMS.Payroll.LoansAndAdvancesBL.GetBranchLocation();
                        break;
                    #endregion
                    #region LIST ALL RECORDS
                    case ControlsEnum.LISTPAGE:
                        objFilterParams = new FilterParameters();
                        int addDedPk = 0;
                        int empPk = 0;
                        int.TryParse(hdfTrxPk.Value, out addDedPk);
                        int.TryParse(hdfEmpName.Value, out empPk);
                        objFilterParams.PageNumber = uclPaging.CurrentPage == 0 ? 1 : uclPaging.CurrentPage;
                        objFilterParams.PageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
                        int srhType = string.IsNullOrEmpty(ddlSrchType.SelectedValue) ? 0 : Convert.ToInt32(ddlSrchType.SelectedValue);
                        int srhPayemt = string.IsNullOrEmpty(ddlSrchPayElement.SelectedValue) ? 0 : Convert.ToInt32(ddlSrchPayElement.SelectedValue);
                        objFilterParams.Employee = empPk > 0 ? empPk : (int?)null;
                        objFilterParams.FromDate = string.IsNullOrEmpty(txtSrchFromDate.Text) ? (DateTime?)null : Convert.ToDateTime(txtSrchFromDate.Text);
                        objFilterParams.ToDate = string.IsNullOrEmpty(txtSrchToDate.Text) ? (DateTime?)null : Convert.ToDateTime(txtSrchToDate.Text);
                        objFilterParams.Name = HttpUtility.HtmlEncode(txtSearchName.Text.Trim());
                        objFilterParams.UserPK = currentUser.PKUser;
                        objFilterParams.Status = Convert.ToInt32(ddlStatus.SelectedValue);
                        objFilterParams.PK = addDedPk > 0 ? addDedPk : (int?)null;
                        dtTable = BusinessLogic.HRMS.Payroll.OtherAdditionDeductionBL.GetAdditionDeductionList(objFilterParams, currentUser.SBUID, srhType, srhPayemt);
                        break;
                    #endregion
                    #region GET ADDITION DEDUCTION BY PK
                    case ControlsEnum.ADDITIONDEDUCTIONBYPK:
                        objHeader = OtherAdditionDeductionBL.GetAdditionDeductionByPK(currentUser.SBUID, Convert.ToInt32(CommonConstants.ACTIVE), CurrPK);
                        if (objHeader == null && CurrPK != 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "AlertMessage('" + Resources.ErrorMessages.Msg_Edit_Delete + "','" + Resources.Captions.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region GET ADDITION DEDUCTION
                    case ControlsEnum.GETADDITIONDEDUCTION:
                        int employee = 0, dept = 0;
                        decimal amount = 0;
                        int.TryParse(hdfEmployee.Value, out employee);
                        int.TryParse(hdfDepartment.Value, out dept);
                        decimal.TryParse(txtAmount.Text, out amount);
                        objFilterParams = new FilterParameters();
                        objFilterParams.Date = DateTime.Parse(txtAddDedDate.Text);
                        objFilterParams.BranchLocation = Convert.ToInt32(ddlBranchLocation.SelectedValue) > 0 ? Convert.ToInt32(ddlBranchLocation.SelectedValue) : (int?)null;
                        objFilterParams.Employee = employee > 0 ? employee : (int?)null;
                        objFilterParams.EmployeeType = Convert.ToInt32(ddlEmployeeType.SelectedValue) > 0 ? Convert.ToInt32(ddlEmployeeType.SelectedValue) : (int?)null;
                        objFilterParams.EmploymentType = Convert.ToInt32(ddlEmploymentType.SelectedValue) > 0 ? Convert.ToInt32(ddlEmploymentType.SelectedValue) : (int?)null;
                        objFilterParams.Department = dept > 0 ? dept : (int?)null;
                        objFilterParams.PaymentMode = Convert.ToInt32(ddlPaymentMode.SelectedValue) > 0 ? Convert.ToInt32(ddlPaymentMode.SelectedValue) : (int?)null;
                        objFilterParams.EmpCurrency = !string.IsNullOrEmpty(hdfCurrency.Value) ? Convert.ToInt32(hdfCurrency.Value) : (int?)null;
                        objHeader = OtherAdditionDeductionBL.GetAdditionDeduction(objFilterParams, amount, txtRemarks.Text);
                        if (objHeader != null)
                            objEmpAddDedDtlsList = objHeader.EmpAddDedDtl;
                        break;
                    #endregion
                    #region EMPLOYMENTTYPE
                    case ControlsEnum.EMPLOYMENTTYPE:
                        dtResult = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.GetEmploymentType(0);
                        break;
                    #endregion
                    #region EMPLOYEE TYPE
                    case ControlsEnum.EMPLOYEETYPE:
                        dtResult = BusinessLogic.HRMS.Employee.EmployeePayDetailsBL.GetEmployeeTypeGetKV(0, currentUser.SBUID, (int)DbActiveStatus.ACTIVE);
                        break;
                    #endregion
                    #region PAYMENT MODE
                    case ControlsEnum.PAYMENTMODE:
                        dtResult = BusinessLogic.HRMS.Employee.EmployeePayDetailsBL.GetPaymentMode(currentUser.SBUID);
                        break;
                    #endregion
                    #region EXCHANGE RATE
                    case ControlsEnum.EXCHANGERATE:
                        if (!string.IsNullOrEmpty(hdfCurrency.Value) && Convert.ToInt32(hdfCurrency.Value) > 0)
                        {
                            DateTime Date = DateTime.Now;
                            DateTime.TryParse((txtFromDate.Text.Trim() == string.Empty ? DateTime.Now.ToString(Resources.Constants.HRMSDateFormatShort) : txtFromDate.Text.Trim()), out Date);
                            dsExchangeRate = BusinessLogic.PurchaseOrderManagement.RequestForQuote.GetExchangeRate(Convert.ToInt32(hdfCurrency.Value), currentUser.BaseCurrency, Date);
                        }
                        break;
                    #endregion
                    #region CURRENCY
                    case ControlsEnum.CURRENCY:
                        dtResult = CommonBL.GetCurrency(currentUser.BaseCurrency, currentUser.SBUID, (int)DbActiveStatus.HASPK);
                        break;
                    #endregion
                    #region COMPANY
                    case ControlsEnum.COMPANY:
                        dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(BusinessObject.CommonManagement.DbActiveStatus.ACTIVE), 0, CompanyPk);
                        break;
                    #endregion
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

        private void SetFieldValues(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region TYPE
                    case ControlsEnum.TYPE:
                        BindDropDown(ControlsEnum.TYPE);
                        break;
                    #endregion
                    #region SEARCH TYPE
                    case ControlsEnum.SEARCHTYPE:
                        BindDropDown(ControlsEnum.SEARCHTYPE);
                        break;
                    #endregion
                    #region PAY ELEMENT
                    case ControlsEnum.PAYELEMENT:
                        BindDropDown(ControlsEnum.PAYELEMENT);
                        break;
                    #endregion
                    #region SEARCH PAY ELEMENT
                    case ControlsEnum.SEARCHPAYELEMENT:
                        BindDropDown(ControlsEnum.SEARCHPAYELEMENT);
                        break;
                    #endregion
                    #region BRANCH
                    case ControlsEnum.BRANCH:
                        BindDropDown(ControlsEnum.BRANCH);
                        break;
                    #endregion
                    #region EMP DETAILS
                    case ControlsEnum.EMPDETAILS:
                        BindGrid(ControlsEnum.EMPDETAILS);
                        break;
                    #endregion
                    #region LIST PAGE
                    case ControlsEnum.LISTPAGE:
                        BindGrid(ControlsEnum.LISTPAGE);
                        break;
                    #endregion
                    #region ADDITION DEDUCTION BY PK
                    case ControlsEnum.ADDITIONDEDUCTIONBYPK:
                        GetUIValuesFromObject(ControlsEnum.ADDITIONDEDUCTIONBYPK);
                        break;
                    #endregion
                    #region EMPLOYMENT TYPE
                    case ControlsEnum.EMPLOYMENTTYPE:
                        BindDropDown(ControlsEnum.EMPLOYMENTTYPE);
                        break;
                    #endregion
                    #region EMPLOYEE TYPE
                    case ControlsEnum.EMPLOYEETYPE:
                        BindDropDown(ControlsEnum.EMPLOYEETYPE);
                        break;
                    #endregion
                    #region PAYMENT MODE
                    case ControlsEnum.PAYMENTMODE:
                        BindDropDown(ControlsEnum.PAYMENTMODE);
                        break;
                    #endregion
                    #region EXCHANGERATE
                    case ControlsEnum.EXCHANGERATE:
                        GetUIValuesFromObject(ControlsEnum.EXCHANGERATE);
                        break;
                    #endregion
                    #region CURRENCY
                    case ControlsEnum.CURRENCY:
                        GetUIValuesFromObject(ControlsEnum.CURRENCY);
                        break;
                    #endregion
                    #region COMPANY
                    case ControlsEnum.COMPANY:
                        BindDropDown(ControlsEnum.COMPANY);
                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void BindDropDown(ControlsEnum controlType)
        {
            switch (controlType)
            {
                #region TYPE
                case ControlsEnum.TYPE:
                    ddlType.Items.Clear();
                    if (dtResult != null && dtResult.Rows.Count > 0)
                    {
                        ddlType.DataSource = dtResult;
                        ddlType.DataTextField = GTIService.Constants.Common.Common.CFG_DATA_TEXT_FIELD;
                        ddlType.DataValueField = GTIService.Constants.Common.Common.CFG_VALUE_VALUE_FIELD;
                        ddlType.DataBind();
                    }
                    ddlType.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    break;

                case ControlsEnum.SEARCHTYPE:
                    ddlSrchType.Items.Clear();
                    if (dtResult != null && dtResult.Rows.Count > 0)
                    {
                        ddlSrchType.DataSource = dtResult;
                        ddlSrchType.DataTextField = GTIService.Constants.Common.Common.CFG_DATA_TEXT_FIELD;
                        ddlSrchType.DataValueField = GTIService.Constants.Common.Common.CFG_VALUE_VALUE_FIELD;
                        ddlSrchType.DataBind();
                    }
                    ddlSrchType.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region PAY ELEMENT
                case ControlsEnum.PAYELEMENT:
                    ddlPayElement.Items.Clear();
                    if (dtResult != null && dtResult.Rows.Count > 0)
                    {
                        ddlPayElement.DataSource = dtResult;
                        ddlPayElement.DataTextField = GTIService.Constants.HRMS.Admin.Masters.Fields.F_PEL_NAME;
                        ddlPayElement.DataValueField = GTIService.Constants.HRMS.Admin.Masters.Fields.F_PEL_PK;
                        ddlPayElement.DataBind();
                    }
                    ddlPayElement.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    break;

                case ControlsEnum.SEARCHPAYELEMENT:
                    ddlSrchPayElement.Items.Clear();
                    if (dtResult != null && dtResult.Rows.Count > 0)
                    {
                        ddlSrchPayElement.DataSource = dtResult;
                        ddlSrchPayElement.DataTextField = GTIService.Constants.HRMS.Admin.Masters.Fields.F_PEL_NAME;
                        ddlSrchPayElement.DataValueField = GTIService.Constants.HRMS.Admin.Masters.Fields.F_PEL_PK;
                        ddlSrchPayElement.DataBind();
                    }
                    ddlSrchPayElement.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region BRANCH / LOCATION
                case ControlsEnum.BRANCH:
                    ddlBranchLocation.Items.Clear();
                    if (dtResult != null && dtResult.Rows.Count == 1)
                    {
                        ddlBranchLocation.DataSource = dtResult;
                        ddlBranchLocation.DataTextField = GTIService.Constants.HRMS.Employee.Fields.HRM_CON_TEXT;
                        ddlBranchLocation.DataValueField = GTIService.Constants.HRMS.Employee.Fields.HRM_CON_PK;
                        ddlBranchLocation.DataBind();
                    }
                    else
                    {
                        ddlBranchLocation.DataSource = dtResult;
                        ddlBranchLocation.DataTextField = GTIService.Constants.HRMS.Employee.Fields.HRM_CON_TEXT;
                        ddlBranchLocation.DataValueField = GTIService.Constants.HRMS.Employee.Fields.HRM_CON_PK;
                        ddlBranchLocation.DataBind();
                        ddlBranchLocation.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    }
                    break;
                #endregion
                #region EMPLOYMENTTYPE
                case ControlsEnum.EMPLOYMENTTYPE:
                    ddlEmploymentType.Items.Clear();
                    if (dtResult != null && dtResult.Rows.Count > 0)
                    {
                        ddlEmploymentType.DataSource = dtResult;
                        ddlEmploymentType.DataTextField = GTIService.Constants.HRMS.Employee.Fields.HRM_CON_TEXT;
                        ddlEmploymentType.DataValueField = GTIService.Constants.HRMS.Employee.Fields.HRM_CON_PK;
                        ddlEmploymentType.DataBind();
                    }
                    ddlEmploymentType.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region EMPLOYEE TYPE
                case ControlsEnum.EMPLOYEETYPE:
                    ddlEmployeeType.Items.Clear();
                    ddlEmployeeType.DataSource = dtResult;
                    ddlEmployeeType.DataTextField = GTIService.Constants.HRMS.Admin.Masters.Fields.EMT_NAME;
                    ddlEmployeeType.DataValueField = GTIService.Constants.HRMS.Admin.Masters.Fields.EMT_PK;
                    ddlEmployeeType.DataBind();
                    ddlEmployeeType.Items.HtmlDecode();
                    ddlEmployeeType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region COMPANY
                case ControlsEnum.COMPANY:
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
                #region PAYMENT MODE
                case ControlsEnum.PAYMENTMODE:
                    ddlPaymentMode.DataSource = dtResult;
                    ddlPaymentMode.DataTextField = GTIService.Constants.HRMS.Employee.Fields.ADM_CFG_TEXT;
                    ddlPaymentMode.DataValueField = GTIService.Constants.HRMS.Employee.Fields.ADM_CFG_VALUE;
                    ddlPaymentMode.DataBind();
                    ddlPaymentMode.Items.HtmlDecode();
                    ddlPaymentMode.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                default:
                    break;
            }
        }

        public void BindGrid(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region EMPLOYEE ADDITION / DEDUCTION
                    case ControlsEnum.EMPDETAILS:
                        if (EmpAddDedDetailList != null)
                        {
                            grdEmpAddDedList.DataSource = EmpAddDedDetailList;
                            grdEmpAddDedList.DataBind();
                        }
                        else
                        {
                            grdEmpAddDedList.DataSource = null;
                            grdEmpAddDedList.DataBind();
                        }
                        break;
                    #endregion
                    #region ADDITION / DEDUCTION LIST PAGE
                    case ControlsEnum.LISTPAGE:
                        int rowCount = 0;
                        int pageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
                        if (dtTable.Rows.Count > 0)
                        {
                            rowCount = Convert.ToInt32(dtTable.Rows[0]["TOTAL_ROW_COUNT"].ToString());
                        }
                        uclPaging.TotalPages = rowCount == 0 ? 1 : (rowCount <= pageSize) ? 1 :
                                      (rowCount % pageSize) == 0 ? (rowCount / pageSize) :
                                      (rowCount / pageSize) + 1;
                        PageIndexList = PageIndexList == null ? CommonConstants.SELECT_VALUE_ONE : PageIndexList;
                        uclPaging.CurrentPage = Convert.ToInt32(PageIndexList);
                        grdAddDedList.DataSource = dtTable;
                        grdAddDedList.DataBind();
                        uclPaging.Visible = true;
                        uclPaging.BindPager();

                        //if (dtTable != null && dtTable.Rows.Count > 0)
                        //{
                        //    grdAddDedList.DataSource = dtTable;
                        //    grdAddDedList.DataBind();
                        //}
                        //else
                        //{
                        //    grdAddDedList.DataSource = null;
                        //    grdAddDedList.DataBind();
                        //}
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

        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                {
                    //ImageButton imbEditPayroll = e.Row.FindControl("imbEditPayroll") as ImageButton;
                    ImageButton imbDeletePayroll = e.Row.FindControl("imbDeletePayroll") as ImageButton;
                    CheckBox chkEmpselect = e.Row.FindControl("chkEmpselect") as CheckBox;
                    Label lblBalanceAmt = e.Row.FindControl("lblPayrollDtl") as Label;
                    TextBox txtEmpDate = e.Row.FindControl("txtEmpDate") as TextBox;
                    HiddenField hdfgrdEmpPk = e.Row.FindControl("hdfgrdEmpPk") as HiddenField;
                    TextBox txtgrdAmount = e.Row.FindControl("txtgrdAmount") as TextBox;
                    if (Convert.ToInt16(lblBalanceAmt.Text) > 0)
                    {
                        //imbEditPayroll.Visible = false;
                        chkEmpselect.Visible = false;
                        imbDeletePayroll.Visible = false;
                    }
                    // For Payroll Report 
                    if ((CurEMP_PK > 0) && (Convert.ToInt32(hdfgrdEmpPk.Value) == CurEMP_PK))
                    {
                        txtgrdAmount.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ResetForm(ControlsEnum controlType)
        {
            switch (controlType)
            {
                #region EM PDETAILS
                case ControlsEnum.EMPDETAILS:
                    txtAddDedDate.Text = string.Empty;
                    ddlBranchLocation.ClearSelection();
                    ddlEmployeeType.ClearSelection();
                    ddlEmploymentType.ClearSelection();
                    ddlPaymentMode.ClearSelection();
                    txtAmount.Text = string.Empty;
                    txtRemarks.Text = string.Empty;
                    txtEmployee.Text = Resources.ErpRes.AutoDefaultValue;
                    txtDepartment.Text = Resources.ErpRes.AutoDefaultValue;
                    hdfEmployee.Value = "0";
                    hdfDepartment.Value = CommonConstants.SELECTVAL;
                    CurrEmpAddDedPK = 0;
                    txtEmpName.Text = string.Empty;
                    break;
                #endregion
                #region CLEAR ALL
                case ControlsEnum.CLEARALL:
                    lblTrxNo.Text = Resources.ErpRes.Draft;
                    txtFromDate.Text = string.Empty;
                    txtToDate.Text = string.Empty;
                    ddlType.ClearSelection();
                    ddlPayElement.ClearSelection();
                    ddlPaymentMode.ClearSelection();
                    txtDescription.Text = string.Empty;
                    txtName.Text = string.Empty;
                    txtAddDedDate.Text = string.Empty;
                    ddlBranchLocation.ClearSelection();
                    txtEmployee.Text = Resources.ErpRes.AutoDefaultValue;
                    txtDepartment.Text = Resources.ErpRes.AutoDefaultValue;
                    hdfEmployee.Value = "0";
                    hdfDepartment.Value = CommonConstants.SELECTVAL;
                    txtAmount.Text = string.Empty;
                    txtRemarks.Text = string.Empty;
                    CurrPK = 0;
                    CurrEmpAddDedPK = 0;
                    EmpAddDedDetailList = null;
                    CurrSlNo = 0;
                    base.WkfRefID = 0;
                    txtTrxNo.Text = string.Empty;
                    hdfTrxPk.Value = string.Empty;
                    break;
                #endregion
                #region CLEAR ADDTOLIST
                case ControlsEnum.CLEARADDTOLIST:
                    txtEmployee.Text = Resources.ErpRes.AutoDefaultValue;
                    txtDepartment.Text = Resources.ErpRes.AutoDefaultValue;
                    hdfEmployee.Value = "0";
                    hdfDepartment.Value = CommonConstants.SELECTVAL;
                    CurrEmpAddDedPK = 0;
                    hdfAddDedSlNo.Value = "0";
                    break;
                #endregion
            }
        }

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

        public string GetFormattedCurrency(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfCurrencyFormat.Value);
        }

        public string GetFormattedCurrencyWithComma(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfCurrencyFormatWithComma.Value);
        }

        private int GetCurrSequenceNo()
        {
            if (EmpAddDedDetailList != null && EmpAddDedDetailList.Count > 0)
            {
                if (EmpAddDedDetailList.Where(itm => itm.OAD_PK == 0).Count() >= 1)
                {
                    CurrSlNo = CurrSlNo + 1;
                }
                else
                {
                    CurrSlNo = EmpAddDedDetailList.Max(itm => itm.OAD_PK) + 1;
                }
            }
            else
            {
                CurrSlNo = CurrSlNo + 1;
            }
            return CurrSlNo;
        }

        public string GetFormattedExchangerate(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfExchangeRateFormat.Value);
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
                    GetFieldValues(ControlsEnum.LISTPAGE);
                    SetFieldValues(ControlsEnum.LISTPAGE);
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

        #region Page Events
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
            //this.btnCancel.PreRender += new EventHandler(btnAction_PreRender);
            this.btnPrint.PreRender += new EventHandler(btnAction_PreRender);
            this.btnDelete.PreRender += new EventHandler(btnAction_PreRender);
            this.btnNew.PreRender += new EventHandler(btnAction_PreRender);
            this.btnEdit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnView.PreRender += new EventHandler(btnAction_PreRender);
            this.btnPrintList.PreRender += new EventHandler(btnAction_PreRender);
            this.btnCancelSubmit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnEditforCancel.PreRender += new EventHandler(btnAction_PreRender);

            this.btnSubmit.Load += new EventHandler(btnAction_Load);
            this.btnSave.Load += new EventHandler(btnAction_Load);
            this.btnSaveSubmit.Load += new EventHandler(btnAction_Load);
            //  this.btnCancel.Load += new EventHandler(btnAction_Load);
            this.btnPrint.Load += new EventHandler(btnAction_Load);
            this.btnDelete.Load += new EventHandler(btnAction_Load);
            this.btnNew.Load += new EventHandler(btnAction_Load);
            this.btnEdit.Load += new EventHandler(btnAction_Load);
            this.btnView.Load += new EventHandler(btnAction_Load);
            this.btnPrintList.Load += new EventHandler(btnAction_Load);
            this.btnCancelSubmit.Load += new EventHandler(btnAction_Load);
            this.btnEditforCancel.Load += new EventHandler(btnAction_Load);
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
            base.CheckBtnVisibility(sender);
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            lblBreadCrum.Text = GetLocalResourceObject("Breadcrumb").ToString().Replace(">>",
                "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeComponent();
        }
        protected void Page_PreRender(Object sender, EventArgs e)
        {


            if (EntryStatus == EntryStatus.EDITMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
            }
            else if (EntryStatus == EntryStatus.NEWMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing(0);", true);
                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowHideEmployeeAddDed", "ShowHideEmployeeAddDed(1);", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "ViewMode(2);", true);
                lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                grdEmpAddDedList.Columns[5].Visible = true;
            }
            else if (EntryStatus == EntryStatus.ENTRYMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                grdEmpAddDedList.Columns[5].Visible = true;
            }
            else if (EntryStatus == EntryStatus.LISTMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowListing", "ShowListing(1);", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideAdvance", "ShowHideAdvancedSearch();", true);
                lnkList.CssClass = GetLocalResourceObject("TabActive").ToString();
                lnkDetail.CssClass = GetLocalResourceObject("TabInActive").ToString();
            }
            if (EntryStatus == EntryStatus.VIEWMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "ViewMode(1);", true);
                lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                btnSubmit.Visible = false;
                btnSave.Visible = false;
                btnSaveSubmit.Visible = false;
            }
            if (!MultiCurrencyEnabled)
                txtCurrency.Enabled = false;
            if (grdEmpAddDedList.Rows.Count > 0)
                divImportSec.Visible = false;
            else
                divImportSec.Visible = true;
            btnPrint.Visible = CurrPK > 0 ? true : false;

            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
        }
        #endregion

        #region Enum
        /// <summary>
        ///Page Controls Enum 
        /// </summary>
        public enum ControlsEnum
        {
            LISTPAGE,
            EMPDETAILS,
            CLEARALL,
            CLEARADDTOLIST,
            TYPE,
            SEARCHTYPE,
            PAYELEMENT,
            SEARCHPAYELEMENT,
            BRANCH,
            ADDITIONDEDUCTIONHDR,
            ADDITIONDEDUCTIONBYPK,
            LIST,
            GETADDITIONDEDUCTION,
            ADDITIONDEDUCTIONDTL,
            EMPLOYMENTTYPE,
            EMPLOYEETYPE,
            PAYMENTMODE,
            EXCHANGERATE,
            CURRENCY,
            COMPANY,
            DELETEALL,
            IMPORT
        }
        #endregion
    }
}