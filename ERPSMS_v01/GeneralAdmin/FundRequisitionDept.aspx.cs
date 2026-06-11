using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using ERPSMS_v01.UserControls;
using BusinessObject.AccountManagement;
using System.Data;
using BusinessObject.CommonManagement;
using ERP.Utilities;
using System.Threading;
using System.Text;
using System.IO;
using BusinessLogic.CommonManagement;
using ERPData;
using ERPManager;
using ERPService;
using BusinessObject;
using BusinessObject.Common;
using BusinessObject.Administration.Masters;
using BusinessLogic.Administration.Masters;

namespace ERPSMS_v01.GeneralAdmin
{
    public partial class FundRequisitionDept : ERP.Store.UI.WorkFlowBasePage
    {
        #region Variables and Properties
        #region  Properties
        private int CurrPK
        {
            get
            {
                return ViewState[ERP.Utilities.ViewstateStrings.CurrPK] == null ? 0 : (int)ViewState[ERP.Utilities.ViewstateStrings.CurrPK];
            }
            set
            {
                ViewState[ERP.Utilities.ViewstateStrings.CurrPK] = value;
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
        /// To keep RowIndex in view state
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
        private int ItemRowIndex
        {
            get
            {
                return this.ViewState[ViewstateStrings.ItemRowIndex] == null ? -1 : (int)this.ViewState[ViewstateStrings.ItemRowIndex];
            }
            set
            {
                this.ViewState[ViewstateStrings.ItemRowIndex] = value;
            }
        }

        /// <summary>
        /// To keep popup RowIndex in view state
        /// </summary>
        private int RowIndexPopup
        {
            get
            {
                return this.ViewState[ViewstateStrings.RowIndexPopup] == null ? -1 : (int)this.ViewState[ViewstateStrings.RowIndexPopup];
            }
            set
            {
                this.ViewState[ViewstateStrings.RowIndexPopup] = value;
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
        /// Approved
        /// </summary>
        private int Approved
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.Approved]);
            }
            set
            {
                this.ViewState[ViewstateStrings.Approved] = value;
            }
        }

        private int Status
        {
            get
            {
                return this.ViewState[ViewstateStrings.Status] == null ? 0 : Convert.ToInt32((this.ViewState[ViewstateStrings.Status]));
            }
            set
            {
                this.ViewState[ViewstateStrings.Status] = value;
            }
        }
        /// <summary>
        /// Duty slip Detail PK
        /// </summary>
        private int SRDPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.PoDtlPK]);//[SRD_PK]
            }
            set
            {
                this.ViewState[ViewstateStrings.PoDtlPK] = value;
            }
        }
        /// <summary>
        /// To keep details in view state
        /// </summary>
        private List<FundRequisitionDeptDetails> FundRequisitionDeptDetailList
        {
            get
            {
                return (List<FundRequisitionDeptDetails>)ViewState[ViewstateStrings.FundRequisitionDeptDetailList];
            }
            set
            {
                ViewState[ViewstateStrings.FundRequisitionDeptDetailList] = value;
            }
        }
        private List<FundRequisitionDeptDetailsUploads> FundRequisitionDeptUploadList
        {
            get
            {
                return ViewState[ViewstateStrings.FundRequisitionDeptUploadList] == null ? null : (List<FundRequisitionDeptDetailsUploads>)ViewState[ViewstateStrings.FundRequisitionDeptUploadList];
            }
            set
            {
                ViewState[ViewstateStrings.FundRequisitionDeptUploadList] = value;
            }
        }
        private List<BusinessObject.Administration.Masters.FileDetails> FileDetailsList
        {
            get
            {
                return Session[ERP.Utilities.SessionStrings.FileDSADetailsList] == null ? null : (List<BusinessObject.Administration.Masters.FileDetails>)Session[ERP.Utilities.SessionStrings.FileDSADetailsList];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.FileDSADetailsList] = value;
            }
        }
        private int CurrSlNo
        {
            get
            {
                return ViewState[ViewstateStrings.CurrSlNo] == null ? 0 : (int)ViewState[ViewstateStrings.CurrSlNo];
            }
            set
            {
                ViewState[ViewstateStrings.CurrSlNo] = value;
            }
        }
        private int SID_SL_NO
        {
            get
            {
                return ViewState[ViewstateStrings.CurrDocSlNo] == null ? 0 : (int)ViewState[ViewstateStrings.CurrDocSlNo];
            }
            set
            {
                ViewState[ViewstateStrings.CurrDocSlNo] = value;
            }
        }
        private int EditActionFlag
        {
            get
            {
                return this.ViewState["EditActionFlag"] == null ? 0 : (int)(this.ViewState["EditActionFlag"]);
            }
            set
            {
                this.ViewState["EditActionFlag"] = value;
            }
        }
        /// <summary>
        /// Is invoice cancelled or not
        /// </summary>
        private bool IsDeleted
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsDeleted] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsDeleted]);
            }
            set
            {
                this.ViewState[ViewstateStrings.IsDeleted] = value;
            }
        }

        #endregion

        #region  Variables
        private BusinessObject.AccountManagement.ActionsEnum commonActions;
        User currentUser;
        private DataTable dtResult;
        private DataTable dtCompany;
        private DataTable dtDept;

        private string strResult = string.Empty;
        private string refID;
        private string inboxFlag;
        private string prefID;
        private int CompanyPk = 0;
        private int dptType;
        DataSet dsPageData;
        DataTable dtPageData;
        private FundRequisitionDeptHeader objFundRequisitionDeptHeader;
        private FundRequisitionDeptDetails fundRequisitionDeptDetailsObj;
        GridViewRow grvRow;

        private ADM_COMPANY_MST admCompanyMstObj;
        private List<ADM_COMPANY_MST> admCompanyMstList;
        FundRequisitionDeptDetailsUploads FRDUploadObj;
        int selectedItemPK;
        private CommonService cm;
        private List<SPADM_APP_SUB_TYPE_DATA_GET_Result> AppTypeDetailsList;
        #endregion
        #endregion

        #region PageLevel Events
        /// <summary>
        /// Page load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            PageActionHandler();
        }

        #endregion

        #region PageActionHandler
        /// <summary>
        /// Handle All page Related Events
        /// </summary>
        private void PageActionHandler()
        {
            try
            {
                if (Session[ERP.Utilities.SessionStrings.TransactionType] == null)
                {
                    ucrWrkf.WrkfSubmit += new EventHandler(ActionHandler);
                    hdfJournalizeWorkFlow.Value = "0";
                }

                ucrWrkf.ViewType = 1;
                // InitializeComponent();
                if (!IsPostBack)
                {
                    GetFieldValues(ControlsEnum.COMPANY);
                    SetFieldValues(ControlsEnum.COMPANY);
                    GetFieldValues(ControlsEnum.TRANSACTIONSTATUS);
                    SetFieldValues(ControlsEnum.TRANSACTIONSTATUS);

                    PageIndex = Convert.ToInt16(CommonConstants.SELECT_VALUE_ONE);
                    hdfCurrencyDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits.ToString();
                    string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
                    txtDate.Text = DateTime.Now.ToString(Resources.Constants.HRMSDateFormatShort);
                    uclPaging.TotalPages = TotalPages;
                    uclPaging.CurrentPage = 1;

                    #region Format Settings
                    int NoDecimalDigitsP2P = Session[ERP.Utilities.SessionStrings.NumberDecimalDigitsP2P] != null ? Convert.ToInt32(Session[ERP.Utilities.SessionStrings.NumberDecimalDigitsP2P].ToString()) : 2;
                    hdfDecimalFormat.Value = "#0.";
                    for (int i = 0; i < NoDecimalDigitsP2P; i++)
                    {
                        hdfDecimalFormat.Value += "0";
                        hdfDecimalFormatWithSeperation.Value += "0";
                    }
                    hdfNumberDigits.Value = NoDecimalDigitsP2P.ToString();

                    hdfRateFormat.Value = "#0.";
                    int rateDecimalDigits = (Session[ERP.Utilities.SessionStrings.RateDecimalDigitP2P] == null
                        ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits
                        : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.RateDecimalDigitP2P]));
                    for (int i = 0; i < rateDecimalDigits; i++)
                    {
                        hdfRateFormat.Value += "0";
                    }

                    hdfCurrencyFormatWithComma.Value = "#" + currencysep + "#0.";
                    hdfCurrencyFormat.Value = "#0.";
                    for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits; i++)
                    {
                        hdfCurrencyFormat.Value += "0";
                        hdfCurrencyFormatWithComma.Value += "0";
                    }
                    hdfExchangeRateFormat.Value = "#0.";
                    int exchrateDecimalDigits = (Session[ERP.Utilities.SessionStrings.ExchRateDecimalDigit] == null
                        ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits
                        : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.ExchRateDecimalDigit]));
                    for (int i = 0; i < exchrateDecimalDigits; i++)
                    {
                        hdfExchangeRateFormat.Value += "0";
                    }
                    #endregion

                    #region pid,refID,prefID,inboxFlag
                    string pid = Request.QueryString[QueryStrings.PID] != null ? Request.QueryString[QueryStrings.PID]
                                  : Session[ERP.Utilities.SessionStrings.PID] != null ? Session[ERP.Utilities.SessionStrings.PID].ToString().Split('=')[1] : string.Empty;
                    refID = Request.QueryString[QueryStrings.RefID] != null ? Request.QueryString[QueryStrings.RefID]
                        : Session[ERP.Utilities.SessionStrings.RefID] != null ? Session[ERP.Utilities.SessionStrings.RefID].ToString().Split('=')[1] : string.Empty;
                    prefID = Request.QueryString[QueryStrings.PRefID] != null ? Request.QueryString[QueryStrings.PRefID]
                        : Session[ERP.Utilities.SessionStrings.PRefID] != null ? Session[ERP.Utilities.SessionStrings.PRefID].ToString().Split('=')[1] : string.Empty;
                    inboxFlag = Request.QueryString[QueryStrings.Flag] != null ? Request.QueryString[QueryStrings.Flag]
                    : Session[ERP.Utilities.SessionStrings.InboxFlag] != null ? Session[ERP.Utilities.SessionStrings.InboxFlag].ToString() : string.Empty;
                    #endregion

                    ReferanceID = string.IsNullOrEmpty(refID)
                           ? string.IsNullOrEmpty(prefID)
                                 ? 0
                                 : int.Parse(prefID)
                           : int.Parse(refID);
                    FillProcessID(1);

                    string[] datakeyarray;
                    datakeyarray = new string[2];
                    datakeyarray[0] = "DFD_PK";
                    datakeyarray[1] = "DFD_SEQUENCE";
                    grdFrqDtls.DataKeyNames = datakeyarray;

                    FileDetailsList = null;
                    FundRequisitionDeptUploadList = null;
                    string[] itemkeyarray;
                    itemkeyarray = new string[1];
                    itemkeyarray[0] = "DOC_SEQ_NO";
                    grdUploads.DataKeyNames = itemkeyarray;

                    FundRequisitionDeptDetailList = new List<FundRequisitionDeptDetails>();

                    EntryStatus = EntryStatus.LISTMODE;
                    GetFieldValues(ControlsEnum.CURRENCY);
                    SetFieldValues(ControlsEnum.CURRENCY);
                    GetFieldValues(ControlsEnum.REQUESTEDDEPT);
                    SetFieldValues(ControlsEnum.REQUESTEDDEPT);

                    #region Work Flow
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
                        ResetForm(ControlsEnum.CLEAR);
                        base.WkfRefID = ucrWrkf.RefID = int.Parse(prefID);
                        CurrPK = GetApplicationID(ucrWrkf.RefID);
                        GetFieldValues(ControlsEnum.DEPARTMENTBYTYPE);
                        if (dtDept != null && dtDept.Rows.Count > 0)
                        {
                            Session[BusinessObject.Common.SessionStrings.CurDept] = Convert.ToInt32(dtDept.Rows[0]["DPT_PK"]);
                            base.SetUserDept();
                        }
                    }
                    if (CurrPK > 0)
                    {
                        EntryStatus = EntryStatus.ENTRYMODE;
                        FillProcessID(1);
                        WorkflowCore.CoreService objWorkflowCore = new WorkflowCore.CoreService();
                        base.WkfRefID = ucrWrkf.RefID = objWorkflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                        SetCancelRef(CurrPK);
                        ucrWrkf.FillWorkFlowDetails();
                        if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && (EntryStatus == EntryStatus.ENTRYMODE || EntryStatus == EntryStatus.NEWMODE) && ucrWrkf.HasPageTaskPermission)
                            ucrWrkf.ViewType = 1;
                        else
                        {
                            ucrWrkf.ViewType = 0;
                        }
                        SetUIEditView(commonActions);
                        GetFieldValues(ControlsEnum.EDIT);
                        SetFieldValues(ControlsEnum.EDIT);
                        ResetForm(ControlsEnum.CLEARADD);
                    }
                    else
                    {
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        EntryStatus = EntryStatus.LISTMODE;
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
            finally { }
        }
        #endregion

        #region Helper Methods
        #region Get Field Values
        private void GetFieldValues(ControlsEnum type)
        {
            AdmCompanyMstService admCompanyMstServiceClient;
            ServiceUtility serviceUtilityObj;
            BusinessObject.GridPrams gridParam;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            string pageUrl = string.Empty;
            try
            {
                switch (type)
                {
                    #region LIST
                    case ControlsEnum.LIST:
                        gridParam = new BusinessObject.GridPrams();
                        gridParam.SearchBy = string.Empty;
                        gridParam.SearchValue = string.Empty;
                        gridParam.PageNumber = uclPaging.CurrentPage == 0 ? 1 : uclPaging.CurrentPage;
                        gridParam.PageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
                        gridParam.FromDate = string.IsNullOrEmpty(txtListFromDate.Text) ? string.Empty : txtListFromDate.Text.Trim();
                        gridParam.ToDate = string.IsNullOrEmpty(txtListToDate.Text) ? string.Empty : txtListToDate.Text.Trim();
                        gridParam.FilterStatus = ddlTrnStatus.SelectedValue == "-1" ? string.Empty : ddlTrnStatus.SelectedValue;

                        string trxNo = txtListTrxNo.Text.Trim() == "Select/Type" ? string.Empty : txtListTrxNo.Text.Trim();
                        int reqDeptPK = string.IsNullOrEmpty(ddlRequestedDeptAdvSearch.SelectedValue) ? 0 : Convert.ToInt32(ddlRequestedDeptAdvSearch.SelectedValue);
                        dtResult = FundRequisitionDeptBL.GetFundRequisitionDeptList(gridParam, currentUser, trxNo, reqDeptPK);
                        break;
                    #endregion
                    #region CURRENCY
                    case ControlsEnum.CURRENCY:
                        dtResult = CommonBL.GetCurrency(0, currentUser.SBUID, (int)DbActiveStatus.ACTIVE);
                        break;
                    #endregion
                    #region EXCHANGERATE
                    case ControlsEnum.EXCHANGERATE:
                        //DataSet dsExchangeRate = BusinessLogic.PurchaseOrderManagement.RequestForQuote.GetExchangeRate(Convert.ToInt32(hdfCurrency.Value), currentUser.BaseCurrency, Convert.ToDateTime(txtInvoiceDate.Text.Trim()));
                        //if (dsExchangeRate != null && dsExchangeRate.Tables[0].Rows.Count > 0)
                        //{
                        //    hdfExchangeRate.Value = dsExchangeRate.Tables[0].Rows[0][0].ToString();
                        //  //  txtExchangeRate.Text = Convert.ToDouble(dsExchangeRate.Tables[0].Rows[0][0]).ToString();
                        //}
                        //else
                        //{
                        //    hdfExchangeRate.Value = "-1";
                        //   // txtExchangeRate.Text = "";
                        //}

                        break;
                    #endregion
                    #region Company
                    case ControlsEnum.COMPANY:
                        ////gets Company List
                        admCompanyMstServiceClient = new AdmCompanyMstService();
                        admCompanyMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_COMPANY_MST>();
                        admCompanyMstObj.CMP_ACTIVE = 1;
                        serviceUtilityObj = new ServiceUtility();
                        admCompanyMstList = admCompanyMstServiceClient.GetCompanyList(admCompanyMstObj, serviceUtilityObj);

                        //To get the company related to current SBU
                        dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(DbActiveStatus.ACTIVE), currentUser.SBUID, 0);
                        break;
                    #endregion
                    #region REQUESTEDDEPT
                    case ControlsEnum.REQUESTEDDEPT:
                        dtPageData = DataAccess.Administration.Masters.CompanyMasterDA.GetDepartments(0, (int)DbActiveStatus.ACTIVE, currentUser.PKUser, currentUser.SBUID, 0, 0);
                        break;
                    #endregion
                    #region EDIT
                    case ControlsEnum.EDIT:
                        objFundRequisitionDeptHeader = FundRequisitionDeptBL.GetFundRequisitionDeptByPK(CurrPK);
                        break;
                    #endregion
                    #region Transaction Status
                    case ControlsEnum.TRANSACTIONSTATUS:
                        dtPageData = CommonBL.GetAppStatus("FRD", string.Empty);
                        break;
                    #endregion
                    default:
                        break;
                }
            }
            catch (Exception ex) { throw ex; }
            finally { }
        }
        #endregion
        #region Set Field Values
        private void SetFieldValues(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region LIST
                    case ControlsEnum.LIST:
                        BindGrid(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region CURRENCY
                    case ControlsEnum.CURRENCY:
                        BindDropDown(ControlsEnum.CURRENCY);
                        break;
                    #endregion
                    #region COMPANY
                    case ControlsEnum.COMPANY:
                        BindDropDown(ControlsEnum.COMPANY);
                        break;
                    #endregion
                    #region REQUESTEDDEPT
                    case ControlsEnum.REQUESTEDDEPT:
                        BindDropDown(ControlsEnum.REQUESTEDDEPT);
                        break;
                    #endregion
                    #region FUNDREQUISITIONDEPTDTL
                    case ControlsEnum.FUNDREQUISITIONDEPTDTL:
                        BindGrid(ControlsEnum.FUNDREQUISITIONDEPTDTL);
                        break;
                    #endregion
                    #region EDIT
                    case ControlsEnum.EDIT:
                        GetUIValuesFromObject(ControlsEnum.EDIT);
                        break;
                    #endregion
                    #region UPLOADEDFILES
                    case ControlsEnum.UPLOADEDFILES:
                        if (objFundRequisitionDeptHeader != null)
                            GetUIValuesFromObject(ControlsEnum.UPLOADEDFILES);
                        BindGrid(ControlsEnum.UPLOADEDFILES);
                        break;
                    #endregion
                    case ControlsEnum.TRANSACTIONSTATUS:
                        BindDropDown(ControlsEnum.TRANSACTIONSTATUS);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex) { throw ex; }
        }
        #endregion
        #region Set UIValues To Object
        private Object SetUIValuesToObject(ControlsEnum controlType)
        {
            Object retObject;
            retObject = null;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                switch (controlType)
                {
                    #region FUNDREQUISITIONDEPTHDR
                    case ControlsEnum.FUNDREQUISITIONDEPTHDR:
                        objFundRequisitionDeptHeader.DFH_PK = CurrPK;
                        objFundRequisitionDeptHeader.DFH_NO = lblTrxNo.Text;
                        objFundRequisitionDeptHeader.DFH_DATE = Convert.ToDateTime(txtDate.Text);
                        objFundRequisitionDeptHeader.DFH_REQ_DEPT = String.IsNullOrEmpty(ddlRequestedDept.SelectedValue) ? 0 : Convert.ToInt32(ddlRequestedDept.SelectedValue);
                        objFundRequisitionDeptHeader.DFH_DESC = HttpUtility.HtmlEncode(txtDescription.Text);
                        objFundRequisitionDeptHeader.DFH_REQ_DEPT = Convert.ToInt16(ddlRequestedDept.SelectedValue);
                        objFundRequisitionDeptHeader.DFH_TOTAL = FundRequisitionDeptDetailList.Sum(f => f.DFD_APP_AMT) > 0 ? FundRequisitionDeptDetailList.Sum(f => f.DFD_APP_AMT) : FundRequisitionDeptDetailList.Sum(f => f.DFD_REQ_AMT);
                        objFundRequisitionDeptHeader.DFH_COMPANY = Convert.ToInt16(ddlCompany.SelectedValue);
                        objFundRequisitionDeptHeader.DFH_CURR = currentUser.BaseCurrency <= 0 ? 1 : currentUser.BaseCurrency;
                        objFundRequisitionDeptHeader.DFH_BASE_CURR = currentUser.BaseCurrency <= 0 ? 1 : currentUser.BaseCurrency;
                        objFundRequisitionDeptHeader.DFH_EXCH_RATE = 1;// string.IsNullOrEmpty(hdfExchangeRate.Value) ? 1 : Convert.ToDouble(hdfExchangeRate.Value);
                        objFundRequisitionDeptHeader.DFH_DEPT = Convert.ToInt16(currentUser.CurrentDeptPK);
                        objFundRequisitionDeptHeader.DFH_BIZUNIT = Convert.ToInt32(currentUser.SBUID);

                        objFundRequisitionDeptHeader.APT_CODE = ApplicationType.FRD;
                        objFundRequisitionDeptHeader.AST_DOC_MODE = Convert.ToInt16(GetDOCMODE());
                        objFundRequisitionDeptHeader.BIZUNIT_PK = Convert.ToInt32(currentUser.SBUID);
                        objFundRequisitionDeptHeader.USER_PK = Convert.ToInt32(currentUser.PKUser);
                        objFundRequisitionDeptHeader.DFH_CRTD_BY = Convert.ToInt32(currentUser.PKUser);
                        objFundRequisitionDeptHeader.DFH_CRTD_DT = DateTime.Now;
                        objFundRequisitionDeptHeader.LAST_MOD_DT = LastModifiedTime;

                        objFundRequisitionDeptHeader.Details = FundRequisitionDeptDetailList;
                        objFundRequisitionDeptHeader.FileList = FundRequisitionDeptUploadList; //File Uploads
                        retObject = objFundRequisitionDeptHeader;
                        break;
                    #endregion
                    #region FUNDREQUISITIONDEPTDTL
                    case ControlsEnum.FUNDREQUISITIONDEPTDTL:
                        if (CurrSlNo != 0 && FundRequisitionDeptDetailList != null)
                        {
                            fundRequisitionDeptDetailsObj = FundRequisitionDeptDetailList.SingleOrDefault(itm => itm.DFD_SEQUENCE == CurrSlNo);
                            if (fundRequisitionDeptDetailsObj != null)
                            {
                                //fundRequisitionDeptDetailsObj.DFD_PK = SRDPK;
                                fundRequisitionDeptDetailsObj.DFD_HDR_PK = CurrPK;
                                fundRequisitionDeptDetailsObj.DFD_HEAD = Convert.ToInt32(hdfHead.Value);
                                fundRequisitionDeptDetailsObj.DFD_HEAD_TEXT = HttpUtility.HtmlEncode(txtHead.Text);
                                fundRequisitionDeptDetailsObj.DFD_DESC = HttpUtility.HtmlEncode(txtDtlDesc.Text);
                                fundRequisitionDeptDetailsObj.DFD_REQ_AMT = string.IsNullOrEmpty(txtDtlAmount.Text) ? 0 : Convert.ToDecimal(txtDtlAmount.Text);
                                retObject = fundRequisitionDeptDetailsObj;
                            }
                        }
                        else
                        {
                            int slno = 1;
                            if (FundRequisitionDeptDetailList == null || FundRequisitionDeptDetailList.Count == 0)
                            {
                                FundRequisitionDeptDetailList = new List<FundRequisitionDeptDetails>();
                                slno = 1;
                            }
                            else
                            {
                                slno = FundRequisitionDeptDetailList.Max(itm => itm.DFD_SEQUENCE);
                                slno++;
                            }
                            fundRequisitionDeptDetailsObj = new FundRequisitionDeptDetails();
                            CurrSlNo = fundRequisitionDeptDetailsObj.DFD_SEQUENCE = slno;
                            fundRequisitionDeptDetailsObj.DFD_HDR_PK = CurrPK;
                            fundRequisitionDeptDetailsObj.DFD_PK = 0;
                            fundRequisitionDeptDetailsObj.DFD_HEAD = Convert.ToInt32(hdfHead.Value);
                            fundRequisitionDeptDetailsObj.DFD_HEAD_TEXT = HttpUtility.HtmlEncode(txtHead.Text);
                            fundRequisitionDeptDetailsObj.DFD_DESC = HttpUtility.HtmlEncode(txtDtlDesc.Text);
                            fundRequisitionDeptDetailsObj.DFD_REQ_AMT = string.IsNullOrEmpty(txtDtlAmount.Text) ? 0 : Convert.ToDecimal(txtDtlAmount.Text);
                            FundRequisitionDeptDetailList.Add(fundRequisitionDeptDetailsObj);
                        }
                        retObject = FundRequisitionDeptDetailList;
                        break;
                    #endregion
                    #region FUNDAPPROVALDETAIL
                    case ControlsEnum.FUNDAPPROVALDETAIL:
                        if (FundRequisitionDeptDetailList != null)
                        {
                            foreach (GridViewRow gvr in grdFrqDtls.Rows)
                            {
                                long currDetailPk = Convert.ToInt64(grdFrqDtls.DataKeys[gvr.RowIndex].Values[0]);
                                TextBox txtAppAmount = gvr.FindControl("txtAppAmount") as TextBox;
                                TextBox txtRemarks = gvr.FindControl("txtRemarks") as TextBox;
                                decimal approvedAmount;

                                if (txtAppAmount != null && !string.IsNullOrEmpty(txtAppAmount.Text))
                                {
                                    if (decimal.TryParse(txtAppAmount.Text, out approvedAmount))
                                    {
                                        FundRequisitionDeptDetailList.ForEach(dtl =>
                                        {
                                            if (dtl.DFD_PK == currDetailPk)
                                            {
                                                dtl.DFD_APP_AMT = approvedAmount;
                                                dtl.DFD_REMARKS = txtRemarks == null ? string.Empty : HttpUtility.HtmlEncode(txtRemarks.Text);
                                            }
                                        });
                                    }
                                    else
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("Msg_DtlAmount_Valid").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                        throw new Exception(litErrorMsg.Text);
                                    }
                                }

                            }
                        }
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
        #endregion
        #region Get UIValues From Object
        private void GetUIValuesFromObject(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region EDIT
                    case ControlsEnum.EDIT:
                        if (objFundRequisitionDeptHeader != null)
                        {
                            txtDate.Text = Convert.ToDateTime(objFundRequisitionDeptHeader.DFH_DATE).ToString(Resources.Constants.HRMSDateFormatShort);
                            lblTrxNo.Text = string.IsNullOrEmpty(objFundRequisitionDeptHeader.DFH_NO) ? Resources.ErpRes.Draft : objFundRequisitionDeptHeader.DFH_NO;
                            ddlRequestedDept.SelectedValue = objFundRequisitionDeptHeader.DFH_REQ_DEPT.ToString();
                            txtDescription.Text = HttpUtility.HtmlDecode(objFundRequisitionDeptHeader.DFH_DESC);
                            LastModifiedTime = objFundRequisitionDeptHeader.DFH_MOD_DT;
                            Status = objFundRequisitionDeptHeader.DFH_STATUS;
                            GetFieldValues(ControlsEnum.COMPANY);
                            SetFieldValues(ControlsEnum.COMPANY);
                            hdfIsCancelled.Value = Convert.ToString(objFundRequisitionDeptHeader.DFH_IS_DELETED);
                            FundRequisitionDeptDetailList = objFundRequisitionDeptHeader.Details;
                            #region Setting Approved amount by default
                            if (Status == 1 && ucrWrkf.ViewType == 1)
                            {
                                FundRequisitionDeptDetailList.ForEach(dtl =>
                                {
                                    if (dtl.DFD_APP_AMT == 0)
                                    {
                                        dtl.DFD_APP_AMT = dtl.DFD_REQ_AMT;
                                    }
                                });
                            }
                            #endregion
                            SetFieldValues(ControlsEnum.FUNDREQUISITIONDEPTDTL);
                            SetFieldValues(ControlsEnum.UPLOADEDFILES);
                        }
                        break;
                    #endregion
                    #region CURRENCY
                    case ControlsEnum.CURRENCY:
                        if (dtResult != null && dtResult.Rows.Count > 0)
                        {
                            //txtCurrency.Text = string.Format(GetLocalResourceObject("CurrencyDisplayFormat").ToString(), Convert.ToString(dtResult.Rows[0]["CUR_CODE"]), Convert.ToString(dtResult.Rows[0]["CUR_NAME"]));
                            //hdfCurrency.Value = Convert.ToString(dtResult.Rows[0]["CUR_PK"]);
                        }
                        break;
                    #endregion
                    #region FILLITEMDETAILS
                    case ControlsEnum.FILLITEMDETAILS:
                        if (fundRequisitionDeptDetailsObj != null)
                        {
                            CurrSlNo = fundRequisitionDeptDetailsObj.DFD_SEQUENCE;
                            txtHead.Text = HttpUtility.HtmlDecode(fundRequisitionDeptDetailsObj.DFD_HEAD_TEXT);
                            hdfHead.Value = fundRequisitionDeptDetailsObj.DFD_HEAD.ToString();
                            txtDtlDesc.Text = HttpUtility.HtmlDecode(fundRequisitionDeptDetailsObj.DFD_DESC);
                            txtDtlAmount.Text = GetFormattedCurrency(fundRequisitionDeptDetailsObj.DFD_REQ_AMT);
                        }
                        break;
                    #endregion
                    #region SELECTED DOC
                    case ControlsEnum.SELECTEDDOC:
                        if (FRDUploadObj != null)
                        {
                            CurrSlNo = FRDUploadObj.DOC_SEQ_NO;
                            anchorFile.Visible = true;
                            vrfFileUpload.Enabled = false;
                            anchorFile.InnerHtml = FRDUploadObj.DOC_NAME;
                            anchorFile.HRef = FRDUploadObj.DOC_PATH;
                            if (FileDetailsList != null && FileDetailsList.Where(fle => fle.SlNo == CurrSlNo).Count() > 0)
                            {
                                anchorFile.Attributes.Add("onclick", "return false;");
                            }
                        }
                        break;
                    #endregion
                    # region  FILE_UPLOAD
                    case ControlsEnum.UPLOADEDFILES:
                        CurrPK = objFundRequisitionDeptHeader.DFH_PK;
                        FundRequisitionDeptUploadList = objFundRequisitionDeptHeader.FileList;
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
        #region Bind DropDown
        private void BindDropDown(ControlsEnum controlType)
        {
            switch (controlType)
            {

                #region CURRENCY
                case ControlsEnum.CURRENCY:
                    //ddlCurrency.Items.Clear();
                    //if (dtResult != null && dtResult.Rows.Count > 0)
                    //{
                    //    ddlCurrency.DataValueField = GTIService.Constants.Common.Fields.CURRENCYID;
                    //    ddlCurrency.DataTextField = GTIService.Constants.Common.Fields.CURRENCYCODE;
                    //    ddlCurrency.DataSource = dtResult;
                    //    ddlCurrency.DataBind();
                    //}
                    //ddlCurrency.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECT_VALUE_ZERO));
                    //foreach (ListItem item in ddlCurrency.Items)
                    //{
                    //    item.Text = HttpUtility.HtmlDecode(item.Text);
                    //}
                    break;
                #endregion
                case ControlsEnum.COMPANY:
                    ddlCompany.Items.Clear();
                    if (admCompanyMstList != null && admCompanyMstList.Count > 0)
                    {
                        ddlCompany.DataSource = CommonFunctions.HtmlDecode(admCompanyMstList, Resources.DataFieldRes.CompanySpecs);
                        ddlCompany.DataTextField = Resources.DataFieldRes.CompanySpecs;
                        ddlCompany.DataValueField = Resources.DataFieldRes.CompanyMstPK;
                        ddlCompany.DataBind();
                    }
                    //To set company related to current SBU 
                    if (dtCompany != null && dtCompany.Rows.Count > 0)
                    {
                        ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(dtCompany.Rows[0][Resources.DataFieldRes.CompanyMstPK].ToString()));
                    }
                    break;

                #region REQUESTEDDEPT
                case ControlsEnum.REQUESTEDDEPT:
                    ddlRequestedDept.Items.Clear();
                    if (dtPageData != null && dtPageData.Rows.Count > 0)
                    {
                        ddlRequestedDept.DataValueField = GTIService.Constants.Common.Fields.CON_VALUE;
                        ddlRequestedDept.DataTextField = GTIService.Constants.Common.Fields.CON_NAME;
                        ddlRequestedDept.DataSource = dtPageData;
                        ddlRequestedDept.DataBind();
                    }
                    ddlRequestedDept.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECT_VALUE_ZERO));
                    foreach (ListItem item in ddlRequestedDept.Items)
                    {
                        item.Text = HttpUtility.HtmlDecode(item.Text);
                    }
                    #region Listing RequestDept
                    ddlRequestedDeptAdvSearch.Items.Clear();
                    if (dtPageData != null && dtPageData.Rows.Count > 0)
                    {
                        ddlRequestedDeptAdvSearch.DataValueField = GTIService.Constants.Common.Fields.CON_VALUE;
                        ddlRequestedDeptAdvSearch.DataTextField = GTIService.Constants.Common.Fields.CON_NAME;
                        ddlRequestedDeptAdvSearch.DataSource = dtPageData;
                        ddlRequestedDeptAdvSearch.DataBind();
                    }
                    ddlRequestedDeptAdvSearch.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECT_VALUE_ZERO));
                    foreach (ListItem item in ddlRequestedDeptAdvSearch.Items)
                    {
                        item.Text = HttpUtility.HtmlDecode(item.Text);
                    }
                    #endregion
                    break;
                #endregion
                #region TRANSACTIONSTATUS
                case ControlsEnum.TRANSACTIONSTATUS:
                    ddlTrnStatus.Items.Clear();
                    if (dtPageData != null && dtPageData.Rows.Count > 0)
                    {
                        ddlTrnStatus.DataValueField = GTIService.Constants.Common.Fields.ASC_VALUE;
                        ddlTrnStatus.DataTextField = GTIService.Constants.Common.Fields.ASC_NAME;
                        ddlTrnStatus.DataSource = dtPageData;
                        ddlTrnStatus.DataBind();
                    }
                    ddlTrnStatus.Items.Insert(0, new ListItem(CommonConstants.SELECTTEXT, CommonConstants.SELECTVAL));
                    foreach (ListItem item in ddlTrnStatus.Items)
                    {
                        item.Text = HttpUtility.HtmlDecode(item.Text);
                    }
                    break;
                #endregion
                default:
                    break;
            }
        }
        #endregion
        #region BindGrid
        public void BindGrid(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region LIST
                    case ControlsEnum.LIST:
                        uclPaging.Visible = false;
                        if (dtResult != null && dtResult.Rows.Count > 0)
                        {
                            int rowCount = 0;
                            rowCount = Convert.ToInt32(dtResult.Rows[0]["TOTAL_ROW_COUNT"].ToString());
                            uclPaging.TotalPages = rowCount == 0 ? 1 : (rowCount <= this.PageSize) ? 1 :
                                              (rowCount % this.PageSize) == 0 ? (rowCount / this.PageSize) :
                                              (rowCount / this.PageSize) + 1;
                            uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                            grdList.DataSource = dtResult;
                            grdList.DataBind();
                            uclPaging.Visible = true;
                            uclPaging.BindPager();
                        }
                        else
                        {
                            grdList.DataSource = null;
                            grdList.DataBind();
                        }
                        break;
                    #endregion
                    #region FUNDREQUISITIONDEPTDTL
                    case ControlsEnum.FUNDREQUISITIONDEPTDTL:
                        grdFrqDtls.DataSource = FundRequisitionDeptDetailList;
                        grdFrqDtls.DataBind();
                        break;
                    #endregion
                    #region UPLOADED FILES
                    case ControlsEnum.UPLOADEDFILES:
                        grdUploads.DataSource = FundRequisitionDeptUploadList;
                        grdUploads.DataBind();
                        break;
                    #endregion
                    default:
                        break;
                }
            }
            catch (Exception ex) { throw ex; }
        }
        #endregion
        #region Reset Form
        private void ResetForm(ControlsEnum controlType)
        {
            switch (controlType)
            {
                # region CLEARSERACH
                case ControlsEnum.CLEARSEARCH:
                    CurrPK = 0;
                    txtListFromDate.Text = string.Empty;
                    txtListToDate.Text = string.Empty;
                    txtListTrxNo.Text = string.Empty;

                    ddlTrnStatus.SelectedValue = "-1";
                    ddlRequestedDeptAdvSearch.SelectedIndex = 0;
                    uclPaging.CurrentPage = 0;
                    PageIndex = 1;
                    Status = 0;
                    break;
                #endregion
                # region CLEAR
                case ControlsEnum.CLEAR:
                    CurrPK = 0;
                    txtDate.Text = DateTime.Now.ToString(Resources.Constants.HRMSDateFormatShort);
                    lblTrxNo.Text = Resources.ErpRes.Draft;
                    uclPaging.CurrentPage = 0;
                    PageIndex = 1;
                    txtListTrxNo.Text = string.Empty;
                    hdfTrxPk.Value = string.Empty;
                    ddlRequestedDept.SelectedValue = "0";

                    txtDescription.Text = string.Empty;
                    Status = 0;
                    base.WkfRefID = 0;

                    FundRequisitionDeptDetailList = null;

                    ResetForm(ControlsEnum.CLEARADD);
                    ResetForm(ControlsEnum.CLEARPOPUPDETAILS);
                    ResetForm(ControlsEnum.CLEARSEARCH);
                    FileDetailsList = null;
                    FundRequisitionDeptUploadList = null;
                    anchorFile.Visible = false;
                    BindGrid(ControlsEnum.UPLOADEDFILES);
                    EditActionFlag = 0;

                    break;
                #endregion
                # region CLEAR ADD
                case ControlsEnum.CLEARADD:
                    txtHead.Text = string.Empty;
                    hdfHead.Value = string.Empty;
                    txtDtlDesc.Text = string.Empty;
                    txtDtlAmount.Text = string.Empty;
                    CurrSlNo = 0;
                    SID_SL_NO = 0;
                    RowIndex = -1;
                    EditActionFlag = 0;
                    break;
                #endregion
                case ControlsEnum.UPLOADEDFILES:
                    anchorFile.Visible = false;
                    vrfFileUpload.Enabled = true;
                    CurrSlNo = 0;
                    anchorFile.Attributes.Remove("onclick");
                    break;
            }
        }
        #endregion
        public string GetFormattedNumber(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfDecimalFormat.Value);
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
        public string GetFormattedRate(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfRateFormat.Value);
        }
        private int? GetNullableInt(string str)
        {
            int result;
            return (int.TryParse(str, out result) ? (int?)result : null);
        }
        /// <summary>
        /// Funtion used get doc mode
        /// </summary>
        private string GetDOCMODE()
        {
            cm = new CommonService();
            AppTypeDetailsList = cm.GetReportParameters(ApplicationType.FRD, 0, DateTime.Now);
            if (AppTypeDetailsList.Count > 0)
            {
                return AppTypeDetailsList[0].AST_DOC_MODE.ToString();
            }
            else
            {
                return "0";
            }
        }
        /// <summary>
        /// 
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

        #region ActionHandler
        /// <summary>
        /// Button actions events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, EventArgs e)
        {
            try
            {

                int? result;
                bool bIsChecked = false;
                string TrxNo = string.Empty;
                int grdListRowDeptId = 0;
                string savePath = string.Empty;
                FileInfo tempFileInfoObj;

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
                        commonActions = ActionsEnum.LISTITEMSELECTED;
                    }
                }
                else if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
                }

                switch (commonActions)
                {
                    #region NEW
                    case ActionsEnum.NEW:
                        EntryStatus = EntryStatus.NEWMODE;
                        GetFieldValues(ControlsEnum.CURRENCY);
                        SetFieldValues(ControlsEnum.CURRENCY);
                        ResetForm(ControlsEnum.CLEAR);
                        ResetForm(ControlsEnum.CLEARADD);
                        ResetForm(ControlsEnum.CLEARPOPUPDETAILS);
                        SetFieldValues(ControlsEnum.FUNDREQUISITIONDEPTDTL);
                        base.WkfRefID = ucrWrkf.RefID = 0;
                        ucrWrkf.FillWorkFlowDetails();
                        ucrWrkf.ViewType = 1;
                        ucrWrkf.ViewAction();
                        hdfIsCancelled.Value = "0";
                        txtDate.Focus();
                        btnPrint.Visible = false;
                        dvRequest.Visible = true;
                        break;
                    #endregion
                    #region Grid Item Selected
                    case ActionsEnum.LISTITEMSELECTED:
                        foreach (GridViewRow grdrow in grdList.Rows)
                        {
                            RadioButton rbtn;
                            HiddenField hdfDept;
                            HiddenField hdfDelStatus;
                            HiddenField hdfStatus;
                            int selectedPK;
                            int dept;

                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                selectedPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfDFH_PK")).Value);
                                hdfDelStatus = grdrow.FindControl("hdfDelStatus") as HiddenField;
                                hdfStatus = grdrow.FindControl("hdfStatus") as HiddenField;
                                //   hdfSelRecordStatus.Value = hdfStatus.Value.ToString();//For btnEditforCancel show/Hide
                                hdfDept = grdrow.FindControl("hdfDept") as HiddenField;
                                if (hdfDept != null && int.TryParse(hdfDept.Value, out dept))
                                {
                                    Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                                    base.SetUserDept();
                                }
                                WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                                base.WkfRefID = workflowCore.GetRefID(selectedPK, PageProcessID);
                                if (Convert.ToInt32(hdfDelStatus.Value) == 1)
                                {
                                    btnSave.Visible = false;
                                    btnEditforCancel.Visible = false;
                                    btnEdit.Visible = false;
                                }
                                else
                                {
                                    btnEditforCancel.Visible = true;
                                }
                                if (Convert.ToInt32(hdfStatus.Value) == 0 || Convert.ToInt32(hdfStatus.Value) == 3)//0-->Draft,3=>Rejected
                                {
                                    btnEditforCancel.Visible = false;
                                }
                                break;
                            }
                        }
                        break;
                    #endregion

                    #region Requisition Details(ADD/EDITITEM/REMOVEITEM)
                    #region ADD
                    case ActionsEnum.ADD:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_SavError;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else//valid
                        {
                            // insert duty slip details 
                            if (FundRequisitionDeptDetailList == null)
                                FundRequisitionDeptDetailList = new List<FundRequisitionDeptDetails>();
                            if (!IsExpenseHeadExists(Convert.ToInt32(hdfHead.Value)))
                            {
                                FundRequisitionDeptDetailList = (List<FundRequisitionDeptDetails>)SetUIValuesToObject(ControlsEnum.FUNDREQUISITIONDEPTDTL);
                                if (FundRequisitionDeptDetailList != null && FundRequisitionDeptDetailList.Count > 0)
                                {
                                    SetFieldValues(ControlsEnum.FUNDREQUISITIONDEPTDTL);
                                    ResetForm(ControlsEnum.CLEARADD);
                                }
                            }
                            else
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Err_AlreadyAdded").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region EDITITEM
                    case ActionsEnum.EDITITEM:
                        ResetForm(ControlsEnum.CLEARADD);
                        if (FundRequisitionDeptDetailList != null && FundRequisitionDeptDetailList.Count > 0)
                        {
                            CurrSlNo = Convert.ToInt32(grdFrqDtls.DataKeys[(((ImageButton)sender).Parent.Parent as GridViewRow).RowIndex][1]);
                            if (CurrSlNo > 0)
                            {
                                fundRequisitionDeptDetailsObj = FundRequisitionDeptDetailList.SingleOrDefault(row => CurrSlNo == row.DFD_SEQUENCE);
                                GetUIValuesFromObject(ControlsEnum.FILLITEMDETAILS);
                            }
                        }
                        break;
                    #endregion
                    #region REMOVEITEM
                    case ActionsEnum.REMOVEITEM:
                        grvRow = (GridViewRow)((ImageButton)(sender)).Parent.Parent;
                        RowIndex = grvRow.RowIndex;
                        if (FundRequisitionDeptDetailList != null && FundRequisitionDeptDetailList.Count > 0)
                        {
                            CurrSlNo = Convert.ToInt32(((ImageButton)sender).CommandArgument.ToString());
                            if (CurrSlNo > 0)
                            {
                                FundRequisitionDeptDetailList = FundRequisitionDeptDetailList.Where(row => CurrSlNo != row.DFD_SEQUENCE).ToList();
                                SetFieldValues(ControlsEnum.FUNDREQUISITIONDEPTDTL);
                            }
                        }
                        ResetForm(ControlsEnum.CLEARADD);
                        break;
                    #endregion
                    #endregion

                    #region SAVE,/DETAIL/EDIT/VIEW,DELETE,SAVESUBMIT,SUBMIT,WRKFSUBMIT
                    #region SAVE
                    case ActionsEnum.SAVE:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else if (FundRequisitionDeptDetailList == null || FundRequisitionDeptDetailList.Count == 0)
                        {
                            litErrorMsg.Text = Resources.Messages.AddAtleastOneItem;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                        }
                        else
                        {
                            objFundRequisitionDeptHeader = new FundRequisitionDeptHeader();

                            if (Status > 1)
                            {
                                //Update Approved Amount & Remarks in current List
                                SetUIValuesToObject(ControlsEnum.FUNDAPPROVALDETAIL);
                            }

                            objFundRequisitionDeptHeader = (FundRequisitionDeptHeader)SetUIValuesToObject(ControlsEnum.FUNDREQUISITIONDEPTHDR);
                            if (objFundRequisitionDeptHeader != null)
                            {
                                if (objFundRequisitionDeptHeader.Details != null && objFundRequisitionDeptHeader.Details.Count > 0)
                                {
                                    objFundRequisitionDeptHeader.WKF_FLAG = 0;
                                    objFundRequisitionDeptHeader.WKF_TRX_FLAG = Convert.ToInt32(WorkflowTransactionFlag.SAVE);
                                    string trxNo = string.Empty;
                                    string xmlDoc = CommonFunctions.XmlSerialize<FundRequisitionDeptHeader>(objFundRequisitionDeptHeader);
                                    result = FundRequisitionDeptBL.SaveFundRequisitionDeptDetails(xmlDoc, out trxNo);
                                    if (result > 0)
                                    {
                                        #region Success
                                        #region ATTACHMENT SAVE
                                        if (FundRequisitionDeptUploadList != null && FundRequisitionDeptUploadList.Count > 0)
                                        {
                                            savePath = string.Empty;
                                            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                                            {
                                                savePath = HttpContext.Current.Request.PhysicalApplicationPath + "Upload";
                                                if (!Directory.Exists(savePath))
                                                    Directory.CreateDirectory(savePath);
                                                savePath = HttpContext.Current.Request.PhysicalApplicationPath + "Upload\\";
                                            }
                                            else
                                            {
                                                savePath = System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower();//Server.MapPath("../Upload");
                                            }

                                            foreach (FundRequisitionDeptDetailsUploads obj in FundRequisitionDeptUploadList)
                                            {
                                                string filePath = savePath + obj.AttachmentFileName;
                                                FileInfo attachedFileInfo = new FileInfo(filePath);
                                                if (FileDetailsList != null)
                                                {
                                                    FileDetails fileDetailsObj = FileDetailsList.SingleOrDefault(aa => aa.SlNo == obj.DOC_SEQ_NO);
                                                    if (fileDetailsObj != null)
                                                    {
                                                        fileDetailsObj.PoFile.SaveAs(attachedFileInfo.FullName);

                                                    }
                                                }
                                            }
                                        }
                                        #endregion
                                        lblTrxNo.Text = trxNo;
                                        litErrorMsg.Text = GetLocalResourceObject("Msg_Save_Success").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.ErpRes.Information + "');", true);
                                        ResetForm(ControlsEnum.CLEAR);
                                        EntryStatus = EntryStatus.LISTMODE;
                                        CurrPK = (int)result;
                                        GetFieldValues(ControlsEnum.LIST);
                                        SetFieldValues(ControlsEnum.LIST);
                                        #endregion
                                    }
                                    else
                                    {
                                        #region Error/Validation
                                        if (result == (int)DbSaveStatus.SQLERROR)
                                        {
                                            litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                        }
                                        else if (result == (int)DbSaveStatus.CONCURRENCY)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.FundRequisitionDept + " " + Resources.Messages.EditUsedByAnotherUser;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                        }
                                        else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.FundRequisitionDept + " " + Resources.Messages.AlreadyDeleted;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                            EntryStatus = EntryStatus.LISTMODE;
                                        }
                                        else if (result == (int)DbSaveStatus.CODEEXIST)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.FundRequisitionDept + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "','" + "');", true);
                                        }
                                        else if (result == (int)DbSaveStatus.REFNOEXIST)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.FundRequisitionDept + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                        else
                                        {
                                            litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.FundRequisitionDept);
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                        }
                                        #endregion
                                    }
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.AddAtleastOneItem;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                }
                            }
                        }
                        break;
                    #endregion
                    #region DETAIL/EDIT/VIEW
                    case ActionsEnum.DETAIL:
                    case ActionsEnum.EDIT:
                    case ActionsEnum.VIEW:
                        btnPrint.Visible = true;
                        Status = 0;
                        //EditActionFlag = ucrWrkf.ViewType;
                        foreach (GridViewRow grdrow in grdList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                ResetForm(ControlsEnum.CLEAR);
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfDFH_PK")).Value);
                                HiddenField hdfDelStatus = (HiddenField)grdrow.FindControl("hdfDelStatus");
                                HiddenField hdfStatus = (HiddenField)grdrow.FindControl("hdfStatus");
                                Status = Convert.ToInt32(hdfStatus.Value);
                                hdfIsCancelled.Value = hdfDelStatus.Value;
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
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
                                ucrWrkf.ViewAction();
                            }
                            GetFieldValues(ControlsEnum.EDIT);
                            SetFieldValues(ControlsEnum.EDIT);
                            ResetForm(ControlsEnum.CLEARADD);                            
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Record").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region DELETE
                    case ActionsEnum.DELETE:
                        result = FundRequisitionDeptBL.DeleteFundRequisitionDept(CurrPK, Convert.ToString(this.LastModifiedTime));
                        if (result > 0)
                        {
                            #region Success
                            if (grdList.Rows.Count == 1 && Convert.ToInt32(PageIndex) > 1)
                            {
                                PageIndex = Convert.ToInt32(PageIndex) - 1;
                            }
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.FundRequisitionDept);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.ErpRes.Information + "');", true);
                            EntryStatus = EntryStatus.LISTMODE;
                            ResetForm(ControlsEnum.CLEAR);
                            GetFieldValues(ControlsEnum.LIST);
                            SetFieldValues(ControlsEnum.LIST);
                            #endregion
                        }
                        else
                        {
                            #region ErrorMsg/Validation
                            if (result == (int)DbSaveStatus.REFERRED)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.FundRequisitionDept;
                                litErrorMsg.Text += " " + GetGlobalResourceObject("Messages", "UsedInAnotherPlace").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.SQLERROR)
                            {
                                litErrorMsg.Text = GetGlobalResourceObject("Messages", "ActionFailedPleaseTryAgain").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CONCURRENCY)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.FundRequisitionDept + " " +
                                    GetGlobalResourceObject("Messages", "EditUsedByAnotherUser").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CODEEXIST)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.FundRequisitionDept + " " +
                                    GetGlobalResourceObject("Messages", "Itemsalreadyaddedbyanotheruser").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.ALREADYDELETED)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.FundRequisitionDept + " " +
                                    GetGlobalResourceObject("Messages", "AlreadyDeleted").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = GetGlobalResourceObject("Messages", "ActionFailedPleaseTryAgain").ToString();
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.FundRequisitionDept);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            #endregion
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
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        else if (FundRequisitionDeptDetailList == null || FundRequisitionDeptDetailList.Count == 0)
                        {
                            litErrorMsg.Text = Resources.Messages.AddAtleastOneItem;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                        }
                        else//valid
                        {
                            ucrWrkf.ApplicationID = 0;
                            if (hdfIsSaveSubmit.Value == CommonConstants.SELECT_VALUE_ONE)
                            {
                                objFundRequisitionDeptHeader = new FundRequisitionDeptHeader();

                                //Update Approved Amount & Remarks in current List
                                SetUIValuesToObject(ControlsEnum.FUNDAPPROVALDETAIL);

                                objFundRequisitionDeptHeader = (FundRequisitionDeptHeader)SetUIValuesToObject(ControlsEnum.FUNDREQUISITIONDEPTHDR);
                                if (objFundRequisitionDeptHeader != null)
                                {
                                    if (objFundRequisitionDeptHeader.Details != null && objFundRequisitionDeptHeader.Details.Count > 0)
                                    {
                                        TrxNo = string.Empty;
                                        objFundRequisitionDeptHeader.WKF_FLAG = 1;
                                        SaveTransaction(objFundRequisitionDeptHeader, Convert.ToInt32(WorkflowTransactionFlag.SAVEANDSUBMIT));
                                    }
                                    else
                                    {
                                        litErrorMsg.Text = Resources.Messages.AddAtleastOneItem;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                    }
                                }
                            }
                            else if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)//Cancel 
                            {
                                SaveTransaction(null, Convert.ToInt32(WorkflowTransactionFlag.SUBMIT));
                            }
                            else//Submit
                            {
                                SaveTransaction(null, Convert.ToInt32(WorkflowTransactionFlag.SUBMIT));
                            }
                        }
                        break;
                    #endregion
                    #endregion
                    #region EDITFORCANCEL,DELETESUBMIT
                    #region EDITFORCANCEL
                    case ActionsEnum.EDITFORCANCEL:
                        foreach (GridViewRow grdrow in grdList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfDFH_PK")).Value);
                                grdListRowDeptId = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfDept")).Value);
                                if (Convert.ToInt16(((HiddenField)grdrow.FindControl("hdfDelStatus")).Value) == 1)
                                {
                                    btnSave.Visible = false;
                                    IsDeleted = true;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_alreadycancelled").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                    return;

                                }
                                else
                                {
                                    btnSave.Visible = true;
                                    btnEdit.Visible = true;
                                    IsDeleted = false;
                                }
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            ucrWrkf.Reset();
                            FillProcessID(11);//11-->For Cancel
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

                            GetFieldValues(ControlsEnum.EDIT);
                            SetFieldValues(ControlsEnum.EDIT);
                            ResetForm(ControlsEnum.CLEARADD);
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Record").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region DELETESUBMIT (CANCEL SUBMIT Button Click)
                    case ActionsEnum.DELETESUBMIT:
                        //Show WorkFlow Popup
                        hdfIsSaveSubmit.Value = CommonConstants.SELECTVAL;
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    #endregion
                    #endregion

                    #region FILTER
                    case ActionsEnum.FILTER:
                        uclPaging.CurrentPage = 0;
                        this.PageIndex = 1;
                        this.EntryStatus = EntryStatus.LISTMODE;
                        this.CurrPK = 0;
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region CLEARSEARCH
                    case ActionsEnum.CLEARSEARCH:
                        FillProcessID(1);
                        EntryStatus = EntryStatus.LISTMODE;
                        ResetForm(ControlsEnum.CLEARSEARCH);
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region CANCEL /LIST
                    case ActionsEnum.CANCEL:
                    case ActionsEnum.LIST:
                        base.WkfRefID = 0;
                        FillProcessID(1);
                        CurrPK = 0;
                        EntryStatus = EntryStatus.LISTMODE;
                        ResetForm(ControlsEnum.CLEARSEARCH);
                        ResetForm(ControlsEnum.CLEAR);
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region CLEARADD
                    case ActionsEnum.CLEARADD:
                        ResetForm(ControlsEnum.CLEARADD);
                        break;
                    #endregion

                    #region ADDITEM
                    case ActionsEnum.ADDITEMUPLOAD:
                        //validate Page
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_SavError;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else//valid
                        {
                            if (fupUpload.HasFile)
                            {
                                tempFileInfoObj = new FileInfo(fupUpload.PostedFile.FileName);

                                if (!IsValidExtension(tempFileInfoObj.Extension))
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(Resources.ErrorMessages.Msg_Valid_File) + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else
                                {
                                    if (CurrSlNo != 0)
                                    {
                                        if (fupUpload.HasFile || !string.IsNullOrEmpty(anchorFile.HRef))
                                        {
                                            FRDUploadObj = FundRequisitionDeptUploadList.SingleOrDefault(itm => itm.DOC_SEQ_NO == CurrSlNo);
                                            if (FRDUploadObj != null)
                                            {
                                                if (FileDetailsList == null)
                                                {
                                                    FileDetailsList = new List<FileDetails>();
                                                }
                                                if (fupUpload.HasFile)
                                                {

                                                    tempFileInfoObj = new FileInfo(fupUpload.PostedFile.FileName);
                                                    string attachmentFileFormat = tempFileInfoObj.Extension;
                                                    string attachmentFileName = Guid.NewGuid().ToString() + attachmentFileFormat;
                                                    FRDUploadObj.AttachmentFileName = attachmentFileName;
                                                    FRDUploadObj.FileExtension = tempFileInfoObj.Extension;
                                                    FRDUploadObj.DOC_NAME = fupUpload.FileName;
                                                    FRDUploadObj.DOC_TYPE = tempFileInfoObj.Extension;
                                                    if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower()))
                                                    {
                                                        FRDUploadObj.DOC_PATH = "~/Upload/" + attachmentFileName;
                                                    }
                                                    else
                                                    {
                                                        FRDUploadObj.DOC_PATH = System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower() + attachmentFileName;
                                                    }
                                                    FileDetails fileDetailsObj = FileDetailsList.SingleOrDefault(aa => aa.SlNo == CurrSlNo);
                                                    if (fileDetailsObj == null)
                                                    {
                                                        FileDetailsList.Add(new FileDetails() { SlNo = CurrSlNo, PoFile = HttpContext.Current.Request.Files[0] });
                                                    }
                                                    else
                                                    {
                                                        fileDetailsObj.PoFile = HttpContext.Current.Request.Files[0];
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
                                            if (FundRequisitionDeptUploadList == null || FundRequisitionDeptUploadList.Count == 0)
                                            {
                                                FundRequisitionDeptUploadList = new List<FundRequisitionDeptDetailsUploads>();
                                                slno = 1;
                                            }
                                            else
                                            {
                                                slno = FundRequisitionDeptUploadList.Max(itm => itm.DOC_SEQ_NO);
                                                slno++;
                                            }
                                            if (FileDetailsList == null)
                                            {
                                                FileDetailsList = new List<FileDetails>();
                                            }

                                            FRDUploadObj = new FundRequisitionDeptDetailsUploads();
                                            FRDUploadObj.DOC_PK = 0;
                                            FRDUploadObj.DOC_SEQ_NO = slno;
                                            tempFileInfoObj = new FileInfo(fupUpload.PostedFile.FileName);
                                            string attachmentFileFormat = tempFileInfoObj.Extension;
                                            string attachmentFileName = Guid.NewGuid().ToString() + attachmentFileFormat;

                                            FRDUploadObj.AttachmentFileName = attachmentFileName;
                                            FRDUploadObj.FileExtension = tempFileInfoObj.Extension;
                                            FRDUploadObj.DOC_NAME = fupUpload.FileName;
                                            FRDUploadObj.DOC_TYPE = tempFileInfoObj.Extension;
                                            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower()))
                                            {
                                                FRDUploadObj.DOC_PATH = "~/Upload/" + attachmentFileName;
                                            }
                                            else
                                            {
                                                FRDUploadObj.DOC_PATH = System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower() + attachmentFileName;
                                            }
                                            FRDUploadObj.DOC_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                                            FileDetailsList.Add(new FileDetails() { SlNo = slno, PoFile = HttpContext.Current.Request.Files[0] });
                                            FundRequisitionDeptUploadList.Add(FRDUploadObj);

                                        }
                                    }
                                }
                            }
                            BindGrid(ControlsEnum.UPLOADEDFILES);
                            ResetForm(ControlsEnum.UPLOADEDFILES);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowUploadDocDetails", "$(document).ready(function(){ShowHideUploadDocDetails(1);});", true);
                            grdUploads.Focus();
                        }
                        break;
                    #endregion
                    #region REMOVEITEMUPLOAD
                    case ActionsEnum.REMOVEITEMUPLOAD:
                        if (FundRequisitionDeptUploadList != null && FundRequisitionDeptUploadList.Count > 0)
                        {
                            selectedItemPK = Convert.ToInt32(grdUploads.DataKeys[(((Button)sender).Parent.Parent as GridViewRow).RowIndex][0]);
                            if (selectedItemPK > 0)
                            {
                                FundRequisitionDeptUploadList = FundRequisitionDeptUploadList.Where(row => selectedItemPK != row.DOC_SEQ_NO).ToList();
                                BindGrid(ControlsEnum.UPLOADEDFILES);
                                ResetForm(ControlsEnum.UPLOADEDFILES);
                            }
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowUploadDocDetails", "$(document).ready(function(){ShowHideUploadDocDetails(1);});", true);

                        break;
                    #endregion
                    #region EDITITEMUPLOAD
                    case ActionsEnum.EDITITEMUPLOAD:
                        if (FundRequisitionDeptUploadList != null && FundRequisitionDeptUploadList.Count > 0)
                        {
                            selectedItemPK = Convert.ToInt32(grdUploads.DataKeys[(((Button)sender).Parent.Parent as GridViewRow).RowIndex][0]);
                            if (selectedItemPK > 0)
                            {
                                FRDUploadObj = FundRequisitionDeptUploadList.SingleOrDefault(row => selectedItemPK == row.DOC_SEQ_NO);
                                GetUIValuesFromObject(ControlsEnum.SELECTEDDOC);
                            }
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowUploadDocDetails", "$(document).ready(function(){ShowHideUploadDocDetails(1);});", true);

                        break;
                    #endregion

                    #region PRINT
                    case ActionsEnum.PRINT:
                        foreach (GridViewRow grdrow in grdList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfDFH_PK")).Value);
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + CurrPK + "&APPTYPE=" + "FRD" + "&APPSUBTYPE=0") + "');", true);
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Record").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
            finally { }
        }
        private void ResetPageAndWorkflow()
        {
            ResetForm(ControlsEnum.CLEAR);
            EntryStatus = EntryStatus.LISTMODE;
            GetFieldValues(ControlsEnum.LIST);
            SetFieldValues(ControlsEnum.LIST);
            #region Reset Workflow
            ucrWrkf.Reset();
            FillProcessID(1);
            WorkflowCore.CoreService objworkflowCore = new WorkflowCore.CoreService();
            base.WkfRefID = ucrWrkf.RefID = objworkflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
            SetCancelRef(CurrPK);
            ucrWrkf.FillWorkFlowDetails();
            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.LISTMODE && ucrWrkf.HasPageTaskPermission)
                ucrWrkf.ViewType = 1;
            else
            {
                ucrWrkf.ViewType = 0;
                ucrWrkf.ViewAction();
            }
            #endregion
        }

        /// <summary>
        /// Save With workflow submition
        /// </summary>
        /// <param name="objFundRequestHeader"></param>
        private void SaveTransaction(FundRequisitionDeptHeader objFundRequestHeader, int workflowFlag)
        {
            int? result = 0;
            WorkflowDetails wkfDetails = null;
            string TrxNo = string.Empty;
            string savePath = string.Empty;
            string action = string.Empty;
            if (objFundRequestHeader == null)
                objFundRequestHeader = new FundRequisitionDeptHeader();
            #region New workflow Submition
            wkfDetails = ucrWrkf.GetWorkflowDetails();
            objFundRequestHeader.USER_PK = wkfDetails.UserPK;
            objFundRequestHeader.WKF_APPLICATION = CurrPK;
            objFundRequestHeader.WKF_COMMENTS = wkfDetails.Comments;
            objFundRequestHeader.WKF_TRX_FLAG = workflowFlag;
            objFundRequestHeader.WKF_PROCESS = wkfDetails.ProcessID;
            objFundRequestHeader.WKF_REFERENCE = wkfDetails.ReferenceID;
            objFundRequestHeader.WKF_TASK = wkfDetails.TaskID;
            objFundRequestHeader.WKF_TASK_ACTION = wkfDetails.ActionID;
            action = wkfDetails.ActionText;
            #endregion
            string xmlDoc = CommonFunctions.XmlSerialize<FundRequisitionDeptHeader>(objFundRequestHeader);
            result = FundRequisitionDeptBL.SaveFundRequisitionDeptDetails(xmlDoc, out TrxNo);
            if (result > 0)
            {
                #region Success
                #region ATTACHMENT SAVE
                if (FundRequisitionDeptUploadList != null && FundRequisitionDeptUploadList.Count > 0)
                {
                    savePath = string.Empty;
                    if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                    {
                        savePath = HttpContext.Current.Request.PhysicalApplicationPath + "Upload";
                        if (!Directory.Exists(savePath))
                            Directory.CreateDirectory(savePath);
                        savePath = HttpContext.Current.Request.PhysicalApplicationPath + "Upload\\";
                    }
                    else
                    {
                        savePath = System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower();//Server.MapPath("../Upload");
                    }

                    foreach (FundRequisitionDeptDetailsUploads obj in FundRequisitionDeptUploadList)
                    {
                        string filePath = savePath + obj.AttachmentFileName;
                        FileInfo attachedFileInfo = new FileInfo(filePath);
                        if (FileDetailsList != null)
                        {
                            FileDetails fileDetailsObj = FileDetailsList.SingleOrDefault(aa => aa.SlNo == obj.DOC_SEQ_NO);
                            if (fileDetailsObj != null)
                            {
                                fileDetailsObj.PoFile.SaveAs(attachedFileInfo.FullName);

                            }
                        }
                    }
                }
                #endregion
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
                args[0] = Resources.PageNameRes.FundRequisitionDept;
                args[1] = lblTrxNo.Text.Trim();
                litErrorMsg.Text = string.Format(litErrorMsg.Text, args);
                //Show Save Message and redired to listing page                                      
                if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && base.WkfRefID > 0)
                {
                    ResetForm(ControlsEnum.CLEAR);
                    EntryStatus = EntryStatus.LISTMODE;
                    CurrPK = (int)result;
                    GetFieldValues(ControlsEnum.LIST);
                    SetFieldValues(ControlsEnum.LIST);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                            + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.InboxURL) + "');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                    ResetForm(ControlsEnum.CLEAR);
                    EntryStatus = EntryStatus.LISTMODE;
                    CurrPK = (int)result;
                    GetFieldValues(ControlsEnum.LIST);
                    SetFieldValues(ControlsEnum.LIST);
                }
                #endregion
            }
            else
            {
                #region Error/Validadion
                if (result == (int)DbSaveStatus.SQLERROR)
                {
                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                }
                else if (result == (int)DbSaveStatus.CONCURRENCY)
                {
                    litErrorMsg.Text = Resources.PageNameRes.FundRequisitionDept + " " + Resources.Messages.EditUsedByAnotherUser;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                }
                else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                {
                    litErrorMsg.Text = Resources.PageNameRes.FundRequisitionDept + " " + Resources.Messages.AlreadyDeleted;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                    EntryStatus = EntryStatus.LISTMODE;
                }
                else if (result == (int)DbSaveStatus.CODEEXIST)
                {
                    litErrorMsg.Text = Resources.PageNameRes.FundRequisitionDept + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "','" + "');", true);
                }
                else if (result == (int)DbSaveStatus.REFNOEXIST)
                {
                    litErrorMsg.Text = Resources.PageNameRes.FundRequisitionDept + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                        + "','" + Resources.ErpRes.Information + "');", true);
                }
                else
                {
                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.FundRequisitionDept);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                }
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
        private bool IsExpenseHeadExists(int headPk)
        {
            bool IsExists = false;
            if (FundRequisitionDeptDetailList != null)
            {
                foreach (FundRequisitionDeptDetails item in FundRequisitionDeptDetailList)
                {
                    if (item.DFD_HEAD == headPk && CurrSlNo != item.DFD_SEQUENCE)
                    {
                        IsExists = true;
                        break;
                    }
                }
            }
            return IsExists;
        }
        /// <summary>
        /// Check the uploaded file is valid
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        private bool IsValidExtension(string extension)
        {
            string BlockedExtensions = "dll";
            if (System.Configuration.ConfigurationManager.AppSettings["BlockedExtensions"].ToLower() != string.Empty)
            {
                BlockedExtensions = System.Configuration.ConfigurationManager.AppSettings["BlockedExtensions"].ToLower();
            }
            bool flag = true;
            string[] extensionList = BlockedExtensions.Split(',');
            for (int i = 0; i < extensionList.Length; i++)
                if (("." + extensionList[i]) == extension)
                {
                    flag = false;
                    break;
                }
            return flag;
        }
        #endregion

        #region ActionHandler (GridViewCommandEventArgs,GridViewRowEventArgs)
        protected void ActionHandler(object sender, GridViewCommandEventArgs e)
        {
        }

        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            try
            {
                #region grdFrqDtls
                if (((GridView)sender).ID == "grdFrqDtls")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                    {
                        #region DataRow
                        List<int> intList = new List<int>();
                        string[] arrrayStatus = GetLocalResourceObject("MoreInfoStatus").ToString().Split(',');//6,16,26,28
                        foreach (var item in arrrayStatus)
                        {
                            intList.Add(Convert.ToInt16(item));
                        }
                        bool IsMoreInfoStatus = intList.IndexOf(Status) != -1;

                        if (Status == 0)//Draft
                        {
                            #region Record is in Draft Stage
                            grdFrqDtls.Columns[3].Visible = false;
                            grdFrqDtls.Columns[4].Visible = false;
                            grdFrqDtls.Columns[5].Visible = true;//Edit,Delete
                            dvRequest.Visible = true;
                            #endregion
                        }
                        else if (Status == 1)//Submitted
                        {
                            #region Record is in Submitted stage   
                            if (ucrWrkf.ViewType == 0)
                            {
                                grdFrqDtls.Columns[3].Visible = false;
                                grdFrqDtls.Columns[4].Visible = false;                             
                            }
                            else
                            {
                                grdFrqDtls.Columns[3].Visible = true;
                                grdFrqDtls.Columns[4].Visible = true;                               
                            }
                            grdFrqDtls.Columns[5].Visible = false;//Edit,Delete
                            dvRequest.Visible = false;
                            #endregion
                        }
                        else if (IsMoreInfoStatus)//6,16,26,28
                        {
                            #region Record is in any of the More Info Status
                            grdFrqDtls.Columns[3].Visible = true;                            
                            grdFrqDtls.Columns[4].Visible = true;
                            if (ucrWrkf.ViewType == 0)
                            {
                                grdFrqDtls.Columns[5].Visible = false;//Edit,Delete
                                dvRequest.Visible = false;
                            }
                            else
                            {
                                grdFrqDtls.Columns[5].Visible = true;//Edit,Delete
                                dvRequest.Visible = true;
                            }
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "DisableApprovedAmount", "DisableApprovedAmount();", true);//Should not allow to edit Approved amount during the time of resubmission
                            #endregion
                        }
                        else
                        {
                            grdFrqDtls.Columns[3].Visible = true;
                            grdFrqDtls.Columns[4].Visible = true;
                            grdFrqDtls.Columns[5].Visible = false;//Edit,Delete
                            dvRequest.Visible = false;
                        }
                        #endregion
                    }
                    else if (e.Row.RowType == DataControlRowType.Footer && e.Row != null)
                    {
                        #region Footer
                        Label lblTotalRequAmount = e.Row.FindControl("lblTotalRequAmount") as Label;
                        decimal Amount = 0;
                        if (FundRequisitionDeptDetailList != null && FundRequisitionDeptDetailList.Count > 0)
                        {
                            Amount = decimal.Parse(FundRequisitionDeptDetailList.Sum(fun => fun.DFD_REQ_AMT).ToString());
                        }
                        hdfTotalReqAmount.Value = Amount.ToString();
                        lblTotalRequAmount.Text = lblTotalRequAmount.ToolTip = GetFormattedCurrencyWithComma(Amount);
                        #endregion
                    }
                }
                #endregion
                #region FileAttachment
                int slno;
                if (((GridView)sender).ID == "grdUploads")
                {
                    if (EntryStatus == EntryStatus.VIEWMODE)
                    {
                        if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
                        {
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
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Pager Methods + Init
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

        protected void Page_PreRender(Object sender, EventArgs e)
        {
            try
            {
                if (EntryStatus == EntryStatus.EDITMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                    lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                }
                else if (EntryStatus == EntryStatus.NEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "ViewMode(2);", true);
                    lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                }
                else if (EntryStatus == EntryStatus.ENTRYMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                    lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                }
                else if (EntryStatus == EntryStatus.LISTMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowListing", "ShowListing(1);", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideAdvance", "ShowHideAdvancedSearch();", true);
                    lnkList.CssClass = GetLocalResourceObject("TabActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabInActive").ToString();
                }
                else if (EntryStatus == EntryStatus.VIEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "ViewMode(1);", true);
                    lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                }
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideAdvancedSearch(1);});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowUploadDocDetails", "$(document).ready(function(){ShowHideUploadDocDetails();});", true);

                if (CurrPK == 0)
                    btnSubmit.Visible = false;
                if (CurrPK == 0 || Status == 0)
                    btnCancelSubmit.Visible = false;

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
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
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            uclPaging.CurrentPage = 1;
            this.btnSubmit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnSave.PreRender += new EventHandler(btnAction_PreRender);
            this.btnSaveSubmit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnDeleteNew.PreRender += new EventHandler(btnAction_PreRender);
            this.btnNew.PreRender += new EventHandler(btnAction_PreRender);
            this.btnEdit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnView.PreRender += new EventHandler(btnAction_PreRender);
            btnEditforCancel.PreRender += new EventHandler(btnAction_PreRender);
            btnCancelSubmit.PreRender += new EventHandler(btnAction_PreRender);


            this.btnSubmit.Load += new EventHandler(btnAction_Load);
            this.btnSave.Load += new EventHandler(btnAction_Load);
            this.btnSaveSubmit.Load += new EventHandler(btnAction_Load);
            this.btnDeleteNew.Load += new EventHandler(btnAction_Load);
            this.btnNew.Load += new EventHandler(btnAction_Load);
            this.btnEdit.Load += new EventHandler(btnAction_Load);
            this.btnView.Load += new EventHandler(btnAction_Load);
            btnEditforCancel.Load += new EventHandler(btnAction_Load);
            btnCancelSubmit.Load += new EventHandler(btnAction_Load);

        }
        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            lblBreadCrum.Text = GetLocalResourceObject("Breadcrumb").ToString().Replace(">>",
                "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
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
        #endregion
        #endregion

        #region Enum
        /// <summary>
        ///Page Controls Enum 
        /// </summary>
        public enum ControlsEnum
        {
            CURRENCY,
            LIST,
            CLEAR,
            PAYMENTMODE,
            EDIT,
            CLEARPOPUPDETAILS,
            COMPANY,
            BRANCH,
            PROCESSMODE,
            CLEARADD,
            GRIDEDIT,
            GRIDDELETE,
            DATDETAILS,
            DEPARTMENTBYTYPE,
            REQUESTEDDEPT,
            FUNDREQUISITIONDEPTHDR,
            FUNDREQUISITIONDEPTDTL,
            ASSETSERVICETYPE,
            FILLASSETDETAILS,
            ITEMDETAILS,
            FILLITEMDETAILS,
            CLEARSEARCH,
            PRINT,
            EXCHANGERATE,
            UPLOADEDFILES,
            ADDITEM,
            SELECTEDDOC,
            FUNDAPPROVALDETAIL,
            TRANSACTIONSTATUS
        }
        #endregion
    }
}