using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERP.Utilities;
using BusinessObject.AccountManagement;
using BusinessObject.Common;
using ERPService;
using ERPData;
using System.Data;
using ERPManager;
using BusinessObject.CommonManagement;
using BusinessObject;
using BusinessObject.Finance;
using System.Threading;

namespace ERPSMS_v01.Finance
{
    public partial class BadDebts : ERP.Store.UI.WorkFlowBasePage
    {
        #region Variables and Properties

          #region Properties
            /// <summary>
            /// Entry State for managing display status
            /// </summary>
            private EntryStatus EntryStatus
            {
                get
                {
                    return this.ViewState[ViewstateStrings.EntryState] == null ? EntryStatus.ENTRYMODE : (EntryStatus)(this.ViewState[ViewstateStrings.EntryState]);
                }
                set
                {
                    this.ViewState[ViewstateStrings.EntryState] = value;
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
            /// Current PK
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
            /// To maintain the PageIndex in viewstate
            /// </summary>
            private string PageIndex
            {
                get
                {
                    return (string)this.ViewState[ViewstateStrings.PageIndex];
                }
                set
                {
                    this.ViewState[ViewstateStrings.PageIndex] = value;
                }
            }
            /// <summary>
            /// To maintain the total pages in viewstate
            /// </summary>
            private int TotalPages
            {
                get
                {
                    return (int)this.ViewState[ViewstateStrings.TotalPages];
                }
                set
                {
                    this.ViewState[ViewstateStrings.TotalPages] = value;
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
            /// To maintain the SortExpression or then By in viewstate
            /// </summary>
            private string ThenBy
            {
                get
                {
                    return (string)this.ViewState[ViewstateStrings.ThenBy];
                }
                set
                {
                    this.ViewState[ViewstateStrings.ThenBy] = value;
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
            /// <summary>
            /// To maintain the Then Direction in viewstate
            /// </summary>
            private string ThenDirection
            {
                get
                {
                    return (string)this.ViewState[ViewstateStrings.ThenDirection];
                }
                set
                {
                    this.ViewState[ViewstateStrings.ThenDirection] = value;
                }
            }
            /// <summary>
            /// To maintain the SortExpression or sort By in viewstate
            /// </summary>

            /// <summary>
            /// To maintain the LastModifiedTime in viewstate
            /// </summary>
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
            /// <summary>
            /// Approved
            /// </summary>
            private bool Posted
            {
                get
                {
                    return Convert.ToBoolean(this.ViewState[ViewstateStrings.Posted]);
                }
                set
                {
                    this.ViewState[ViewstateStrings.Posted] = value;
                }
            }         
            /// <summary>
            /// BDApplicationType
            /// </summary>
            private string BDApplicationType
            {
                get
                {
                    return (string)this.ViewState[ViewstateStrings.BDApplicationType];
                }
                set
                {
                    this.ViewState[ViewstateStrings.BDApplicationType] = value;
                }
            }
            #endregion      
        
            private ActionsEnum commonActions;
            private ADM_COMPANY_MST admCompanyMstObj;
            private List<ADM_COMPANY_MST> admCompanyMstList;
            private DataTable dtCompany;
            private FIN_TRX_HDR finTrxHdrObj;
            private List<FIN_TRX_HDR> finTrxHdrList;
            User currentUser;
            private DataTable dtBadDebits;
            private DataTable dtReliefDetails;
            DataSet dsPageData;
            private List<BDDetails> BadDebitDetailsList;
            private BadDebitBO BadDebitBOObj;
            int JournalPK;
        
        //Workflow
            private string refID;
            private string inboxFlag;
            private int processPK;

        #endregion

        #region PageLevel Events
        /// <summary>
        /// page Load event
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
            string prefID;
            try
            {
                if (Session[ERP.Utilities.SessionStrings.TransactionType] == null)
                {
                    hdfJournalizeWorkFlow.Value = "0";
                    ucrWrkf.WrkfSubmit += new EventHandler(ActionHandler);
                }
                ucrJournalize.JournalizeSave += new EventHandler(ActionHandler);
                ucrJournalize.JournalizeSubmit += new EventHandler(ActionHandler);
                ucrJournalize.JournalizeDelete += new EventHandler(ActionHandler);
                ucrJournalize.JournalizeCancel += new EventHandler(ActionHandler);

                ucrWrkf.ViewType = 1;
                if (!IsPostBack)
                {
                    currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));               
                    #region Date Setting
                    txtFromDate.Text = DateTime.Now.AddMonths(-1).ToString(Resources.Constants.DateFormatShort);
                    hdfFromDate.Value = DateTime.Now.AddMonths(-1).ToString();
                    txtToDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    hdfToDate.Value = DateTime.Now.ToString(); 
                    //Detail Tab Date Settings
                    txtFromDateDetail.Text = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString(Resources.Constants.DateFormatShort);
                    hdfFromDateDetail.Value = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString();
                    txtToDateDetail.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    hdfToDateDetail.Value = DateTime.Now.ToString(); 
                    #endregion
                    #region Company Settings
                    dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(DbActiveStatus.ACTIVE), currentUser.SBUID, 0);
                    if (dtCompany != null && dtCompany.Rows.Count > 0)
                    {
                        hdfCompany.Value = hdfSBUcompany.Value = dtCompany.Rows[0][Resources.DataFieldRes.CompanyMstPK].ToString();
                    } 
                    #endregion
                    ConfigurationSettings();

                  
                    string pid = Request.QueryString[QueryStrings.Type] != null ? Request.QueryString[QueryStrings.Type]
                         : Session[ERP.Utilities.SessionStrings.Type] != null ? Session[ERP.Utilities.SessionStrings.Type].ToString().Split('=')[1] : string.Empty;
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
                      //If Request From External(Report or Other page) other than Menu or Inbox
                    if (Request.QueryString[QueryStrings.FromExt] != null && Request.QueryString[QueryStrings.FromExt] == "T")
                    {                        
                        GetFieldValues(ControlsEnum.RELIEFDETAILGET);
                        SetFieldValues(ControlsEnum.RELIEFDETAILSPECIFIC); 
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
                            if (string.IsNullOrEmpty(pid) || pid.Equals("1"))
                            {
                                ucrWrkf.RefID = int.Parse(refID);
                                base.WkfRefID = ucrWrkf.RefID;
                                CurrPK = GetApplicationID(ucrWrkf.RefID);
                            }
                            else if (pid.Equals("2"))
                            {
                                ucrWrkf.RefID = int.Parse(refID);
                                JournalPK = GetApplicationID(ucrWrkf.RefID);
                                GetFieldValues(ControlsEnum.GETBDPKBYJOURNALPK);
                                if (finTrxHdrList != null && finTrxHdrList.Count == 1)
                                {
                                    CurrPK = (Int32)finTrxHdrList[0].FTH_REF_PK;
                                    WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                                    base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                                }
                            }
                        }
                        else if (!string.IsNullOrEmpty(prefID))
                        {
                            base.WkfRefID = ucrWrkf.RefID = int.Parse(prefID);
                            CurrPK = GetApplicationID(ucrWrkf.RefID);
                        }
                        if (CurrPK > 0)
                        {
                            ucrWrkf.FillWorkFlowDetails();
                            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && (EntryStatus == EntryStatus.ENTRYMODE || EntryStatus == EntryStatus.NEWMODE) && ucrWrkf.HasPageTaskPermission)
                                ucrWrkf.ViewType = 1;
                            else
                            {
                                ucrWrkf.ViewType = 0;
                            }
                            GetFieldValues(ControlsEnum.RELIEFDETAILSPECIFIC);
                            SetFieldValues(ControlsEnum.RELIEFDETAILSPECIFIC);
                        }
                        else
                        {
                            GetFieldValues(ControlsEnum.RELIEFCLAIMLIST);
                            SetFieldValues(ControlsEnum.RELIEFCLAIMLIST);
                            EntryStatus = EntryStatus.LISTMODE;
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                ucrWrkf.ViewType = 0;
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion

        #region Get Field Values
        /// <summary>
        /// Gets the data to bind/assign for the controls(inputs, grids, dropdowns etc.)
        /// pass string.empty to get all field values
        /// Assign it to the page level variables
        /// </summary>
        private void GetFieldValues(ControlsEnum type)
        {
            FinTrxService finTrxServiceClient;
            ServiceUtility serviceUtilityObj;
          
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));         
            try
            {
                switch (type)
                {
                    #region Company
                    //case ControlsEnum.COMPANY:
                    //    ////gets Company List
                    //    //admCompanyMstServiceClient = new AdmCompanyMstService();
                    //    //admCompanyMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_COMPANY_MST>();
                    //    //admCompanyMstObj.CMP_ACTIVE = 1;
                    //    //serviceUtilityObj = new ServiceUtility();
                    //    //admCompanyMstList = admCompanyMstServiceClient.GetCompanyList(admCompanyMstObj, serviceUtilityObj);
                    //    ////To get the company related to current SBU
                    //    //dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(DbActiveStatus.ACTIVE), currentUser.SBUID, 0);
                    //    break;
                     #endregion
                    #region RELIEFCLAIMLIST
                    case ControlsEnum.RELIEFCLAIMLIST:                       
                        dsPageData = BusinessLogic.Finance.BadDebitsBL.GetReliefList(
                           new BusinessObject.GridPrams()
                           {
                               SortBy = string.IsNullOrEmpty(SortBy) ? "ICH_DATE" : SortBy,
                               SortDirection = string.IsNullOrEmpty(SortDirection) ? Resources.Report.SortDescending : SortDirection,
                               ThenBy = SortBy == ThenBy || SortBy == "ICH_NO" ? string.Empty : string.IsNullOrEmpty(ThenBy) ? "ICH_NO" : ThenBy,
                               ThenDirection = SortBy == ThenBy || SortBy == "ICH_NO" ? string.Empty : string.IsNullOrEmpty(ThenDirection) ? Resources.Report.SortDescending : ThenDirection,
                               FromDate = string.Empty,
                               ToDate =  string.Empty,
                               SearchBy = "ICH_NO"
                              // SearchValue = string.Empty;
                           }, currentUser, 0,0);
                        
                        if (dsPageData != null)
                        {                           
                            DataView dvExpense = dsPageData.Tables[1].DefaultView;
                            dtReliefDetails = dvExpense.ToTable();
                        }
                        break;
                    #endregion
                    #region BADDEBITLIST
                    case ControlsEnum.BADDEBITLIST:                       
                        dsPageData = BusinessLogic.Finance.BadDebitsBL.GetBadDebits(
                            new BusinessObject.GridPrams()
                            {                               
                                FromDate = string.IsNullOrEmpty(txtFromDateDetail.Text.Trim()) ? string.Empty : txtFromDateDetail.Text.Trim(),
                                ToDate = string.IsNullOrEmpty(txtToDateDetail.Text.Trim()) ? string.Empty : txtToDateDetail.Text.Trim(),
                             
                            }, currentUser,Convert.ToInt32(hdfMonthLimit.Value) );
                        
                        if (dsPageData != null)
                        {                           
                            DataView dvBadDebits = dsPageData.Tables[0].DefaultView;
                            dtBadDebits = dvBadDebits.ToTable();
                        }
                        break;
                    #endregion
                    #region RELIEFDETAILSPECIFIC
                    case ControlsEnum.RELIEFDETAILSPECIFIC:

                        dsPageData = BusinessLogic.Finance.BadDebitsBL.GetReliefSpecificDetails(
                            new BusinessObject.GridPrams()
                            {
                                FromDate = string.IsNullOrEmpty(txtFromDate.Text.Trim()) ? string.Empty : txtFromDate.Text.Trim(),
                                ToDate = string.IsNullOrEmpty(txtToDate.Text.Trim()) ? string.Empty : txtToDate.Text.Trim()
                            }, currentUser, CurrPK, 2);

                        if (dsPageData != null)
                        {
                            DataView dvExpense = dsPageData.Tables[0].DefaultView;
                            dtReliefDetails = dvExpense.ToTable();
                        }
                        break;
                    #endregion
                    #region RELIEF DETAIL GET
                    case ControlsEnum.RELIEFDETAILGET:
                            CurrPK = Convert.ToInt32(Request.QueryString[QueryStrings.PK]);
                            SetUIEditView(ActionsEnum.VIEW);
                            WorkflowCore.CoreService workflowCore1 = new WorkflowCore.CoreService();
                            base.WkfRefID = ucrWrkf.RefID = workflowCore1.GetRefID(CurrPK, ucrWrkf.ProcessID);
                            ucrWrkf.FillWorkFlowDetails();
                            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                                ucrWrkf.ViewType = 1;
                            else
                            {
                                ucrWrkf.ViewType = 0;
                            }
                            ucrWrkf.ViewAction();
                            GetFieldValues(ControlsEnum.RELIEFDETAILSPECIFIC);                            
                      break;
                    #endregion
                    #region FIN HEADER
                    case ControlsEnum.FINHEADER:
                        finTrxServiceClient = new FinTrxService();
                        finTrxServiceClient = CommonFunctions.InitiateClient(finTrxServiceClient);
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.PageSize = 10;
                        finTrxHdrObj = ERP.Utilities.CommonFunctions.Initilize<FIN_TRX_HDR>();
                        finTrxHdrObj.FTH_REF_TYPE = Session[ERP.Utilities.SessionStrings.TransactionType].ToString();
                        finTrxHdrObj.FTH_REF_PK = Convert.ToInt32(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString());
                        finTrxHdrObj.FTH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        finTrxHdrList = finTrxServiceClient.GetfinTxtHdrList(finTrxHdrObj, serviceUtilityObj);
                        break;
                    #endregion
                    #region FIN HEADER
                    case ControlsEnum.GETBDPKBYJOURNALPK:
                        finTrxServiceClient = new FinTrxService();
                        finTrxServiceClient = CommonFunctions.InitiateClient(finTrxServiceClient);
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.PageSize = 10;
                        finTrxHdrObj = ERP.Utilities.CommonFunctions.Initilize<FIN_TRX_HDR>();
                        finTrxHdrObj.FTH_PK = JournalPK;
                        finTrxHdrObj.FTH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        finTrxHdrList = finTrxServiceClient.GetfinTxtHdrList(finTrxHdrObj, serviceUtilityObj);
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
                finTrxServiceClient = null;
            }
        }
        #endregion

        #region Set Field Values
        /// <summary>
        /// All Field(Input controls, grids, dropdowns) values are assigned here.
        /// To Set all fields, pass "string.Empty()"
        /// </summary>
        private void SetFieldValues(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.BADDEBITLIST:
                        BindGrid(controlType);
                        break;
                    case ControlsEnum.RELIEFCLAIMLIST:
                        BindGrid(controlType);
                        break;
                    case ControlsEnum.RELIEFDETAILSPECIFIC:
                        BindGrid(controlType);
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Helper Methods

        /// <ConfigurationSettings>
        /// 
        /// </summary>
        private void ConfigurationSettings()
        {
            hdfMonthLimit.Value = GetGlobalResourceObject("ConfigurationsRes", "BadDebitLimit").ToString();           
        }

        /// <summary>
        /// Method for Grid binding
        /// </summary>
        public void BindGrid(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.BADDEBITLIST:
                        if (dtBadDebits != null)
                        {
                            grdBadDebitDetails.DataSource = dtBadDebits.DefaultView;
                            grdBadDebitDetails.DataBind();                         
                        }
                        break;
                    case ControlsEnum.RELIEFCLAIMLIST:
                        if (dtReliefDetails!= null)
                        {
                            PageIndex = PageIndex == null ? "0" : PageIndex;
                            grdSavedBadDebit.PageIndex = Convert.ToInt32(PageIndex);
                            grdSavedBadDebit.DataSource = dtReliefDetails.DefaultView;
                            grdSavedBadDebit.DataBind();
                        }
                        break;
                    case ControlsEnum.RELIEFDETAILSPECIFIC:
                        if (dtReliefDetails != null)
                        {
                            Approved = Convert.ToInt32(dtReliefDetails.Rows[0]["IBD_STATUS"]);
                            grdBadDebitDetails.DataSource = dtReliefDetails.DefaultView;
                            grdBadDebitDetails.DataBind();
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SetUIEditView(ActionsEnum Mode)
        {
            try
            {
                if (Mode == ActionsEnum.VIEW)
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

        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>
        private Object SetUIValuesToObject(ControlsEnum controlType)
        {
            Object retObject;
            retObject = null;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            bool bIsChecked = false;
            try
            {
                switch (controlType)
                {
                    #region BADDEBITDETAILS
                    case ControlsEnum.BADDEBITDETAILS:
                        foreach (GridViewRow grdrow in grdBadDebitDetails.Rows)
                        {
                            RadioButton rbtSelect;
                            Label lblInvoiceNo;
                            Label lblOutStandingAmt;                          
                            Label lblOutStandingAmtClaimable;
                            Label lblBDReliefClaimable;
                            HiddenField hdfInvoicePk;
                            HiddenField hdfLastModDate;
                            rbtSelect = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtSelect.Checked)
                            {
                                hdfInvoicePk = (HiddenField)grdrow.FindControl("hdfInvoicePk");
                                lblInvoiceNo = (Label)grdrow.FindControl("lblInvoiceNo");
                                lblOutStandingAmt = (Label)grdrow.FindControl("lblOutStandingAmt");
                                lblOutStandingAmtClaimable = (Label)grdrow.FindControl("lblOutStandingAmtClaimable");
                                lblBDReliefClaimable = (Label)grdrow.FindControl("lblBDReliefClaimable");
                                hdfLastModDate = (HiddenField)grdrow.FindControl("hdfLastModDate");
                                if (BadDebitDetailsList == null || BadDebitDetailsList.Count == 0)
                                {
                                    BadDebitDetailsList = new List<BDDetails>();
                                }
                                BadDebitBOObj = new BadDebitBO();
                                BDDetails BDDetailsObj = new BDDetails();
                                BDDetailsObj.BadDebitPk = CurrPK;
                                BDDetailsObj.InvoicePk = Convert.ToInt32(hdfInvoicePk.Value);
                                BDDetailsObj.OutstandingAmount = !string.IsNullOrEmpty(lblOutStandingAmt.Text.Trim()) ? Math.Round(Convert.ToDouble(lblOutStandingAmt.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits) : 0;
                                BDDetailsObj.OutAmountClaim = !string.IsNullOrEmpty(lblOutStandingAmtClaimable.Text.Trim()) ? Math.Round(Convert.ToDouble(lblOutStandingAmtClaimable.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits) : 0;
                                BDDetailsObj.BadDebitReliefClaim = !string.IsNullOrEmpty(lblBDReliefClaimable.Text.Trim()) ? Math.Round(Convert.ToDouble(lblBDReliefClaimable.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits) : 0;
                                BDDetailsObj.Bizunit = Convert.ToInt16(currentUser.SBUID);
                                BDDetailsObj.Active = Convert.ToByte(DbActiveStatus.ACTIVE);
                                BDDetailsObj.STATUS = 0;
                                BDDetailsObj.USER_PK= Convert.ToInt16(currentUser.PKUser);
                                BDDetailsObj.LAST_MOD_DT = Convert.ToDateTime(hdfLastModDate.Value);
                                BadDebitDetailsList.Add(BDDetailsObj);
                                BadDebitBOObj.BadDebitDetails = BadDebitDetailsList;
                                retObject = BadDebitBOObj;
                            }
                        }                              
                        break;
                    #endregion
                    #region Journalize
                    case ControlsEnum.JOURNALIZE:                       
                        if (EntryStatus == EntryStatus.LISTMODE)
                        {                            
                            foreach (GridViewRow grdrow in grdSavedBadDebit.Rows)
                            {
                                CheckBox chkInvselect;
                                chkInvselect = (CheckBox)grdrow.FindControl("chkInvselect");                             
                                if (chkInvselect.Checked)
                                {
                                    bIsChecked = true;
                                   // CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfExpenseID")).Value);  
                                    break;
                                }
                            }                            
                        }
                        else if (EntryStatus == EntryStatus.ENTRYMODE || EntryStatus == EntryStatus.VIEWMODE)
                        {
                            bIsChecked = true;
                        }                     
                        if (bIsChecked)
                        {
                            if (Approved == 2)
                            {
                                GetFieldValues(ControlsEnum.RELIEFDETAILSPECIFIC);

                                Session[ERP.Utilities.SessionStrings.CrDrType] = null;
                                Session[ERP.Utilities.SessionStrings.SubTypeDDr] = null;
                                Session[ERP.Utilities.SessionStrings.SubTypeCCr] = null;
                                Session[ERP.Utilities.SessionStrings.SubTypeCr] = null;
                                Session[ERP.Utilities.SessionStrings.SubTypeDr] = null;
                                Session[ERP.Utilities.SessionStrings.JournalMode] = null;

                                //Journalize New sessions start
                                Session[ERP.Utilities.SessionStrings.DrControls] = null;
                                Session[ERP.Utilities.SessionStrings.CrControls] = null;
                                Session[ERP.Utilities.SessionStrings.RemovedControls] = null;
                                Session[ERP.Utilities.SessionStrings.AccountType] = null;
                                Session[ERP.Utilities.SessionStrings.ControlInfo] = null;
                                Session[ERP.Utilities.SessionStrings.FinTrxPk] = null;
                                //Journalize New sessions End

                                BDApplicationType = ApplicationType.BDJ;
                                ucrJournalize.TransactionType = BDApplicationType;
                                Session[ERP.Utilities.SessionStrings.TransactionType] = BDApplicationType;                               
                                ucrJournalize.TransactionPK = CurrPK;
                                Session[ERP.Utilities.SessionStrings.TransactionPK] = CurrPK;
                                ucrJournalize.JournalizePK = 0;
                                Session[ERP.Utilities.SessionStrings.JournalizePK] = null;
                                Session[ERP.Utilities.SessionStrings.TransactionNo] = dtReliefDetails.Rows[0]["ICH_NO"].ToString();
                                Session[ERP.Utilities.SessionStrings.TransactionDate] = dtReliefDetails.Rows[0]["ICH_DATE"].ToString();
                                Session[ERP.Utilities.SessionStrings.TransactionCurrency] = dtReliefDetails.Rows[0]["ICH_CURRENCY"].ToString();
                                Session[ERP.Utilities.SessionStrings.AccountPayable] = ApplicationType.AP;                             
                                Session[ERP.Utilities.SessionStrings.JournalType] = ApplicationType.BDJ;
                                ucrWrkf.WrkfSubmit -= ActionHandler;
                                ucrWrkf.Reset();
                                ucrWrkf.ViewType = 1;
                                FillProcessID(2);
                                GetFieldValues(ControlsEnum.FINHEADER);                               
                                WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                                if (finTrxHdrList != null && finTrxHdrList.Count > 0)
                                {
                                    ucrWrkf.RefID = workflowCore.GetRefID((int)finTrxHdrList[0].FTH_PK, ucrWrkf.ProcessID);
                                    base.WkfRefID = ucrWrkf.RefID;
                                }
                                ucrWrkf.FillWorkFlowDetails();
                                if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                                {
                                    ucrJournalize.JournalizeRefPK = ucrWrkf.RefID;
                                    ucrWrkf.ViewType = 1;                                    
                                }
                                else
                                {
                                    ucrWrkf.ViewType = 0;                                 
                                }
                                ucrJournalize.HasWkfPermission = ucrWrkf.HasPageTaskPermission;
                                Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus == EntryStatus.ENTRYMODE ? EntryStatus.ENTRYMODE : EntryStatus.VIEWMODE;

                                ucrWrkf.ViewAction();
                                TextBox WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                                WrkfComments.Text = "";
                                Session[ERP.Utilities.SessionStrings.RemoveRowIndex] = null;
                                hdfJournalizeWorkFlow.Value = "1";
                                ucrJournalize.CallUserControl();
                                Session[ERP.Utilities.SessionStrings.JournalHead] = GetLocalResourceObject("BadDebit_Journal").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_PickApproved_Msg").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            }
                        }
                        else
                        {
                            if (EntryStatus == EntryStatus.NEWMODE)
                                litErrorMsg.Text = GetLocalResourceObject("Msg_PickApproved_Msg").ToString();
                            else
                                litErrorMsg.Text = Resources.Report.Msg_Not_Selected;

                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                  
                }
                return retObject;
            }
            catch
            {
                throw;
            }
            finally
            {
            }
        }

        /// <summary>
        /// Method used to Reset Dorm Controls
        /// </summary>
        private void ResetForm(ControlsEnum controlType)
        {
            switch (controlType)
            {
                case ControlsEnum.RELIEFCLAIMLIST:
                    CurrPK = 0;
                    txtFromDate.Text = DateTime.Now.AddMonths(-1).ToString(Resources.Constants.DateFormatShort);
                    hdfFromDate.Value = DateTime.Now.AddMonths(-1).ToString();
                    txtToDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    hdfToDate.Value = DateTime.Now.ToString();
                    //Detail Tab Date Settings
                    txtFromDateDetail.Text = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString(Resources.Constants.DateFormatShort);
                    hdfFromDateDetail.Value = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString();
                    txtToDateDetail.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    hdfToDateDetail.Value = DateTime.Now.ToString();
                    pnlDetailSearch.Visible = true;
                    base.WkfRefID = 0;
                    break;
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
                path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "");
            else
                path = Request.Url.AbsolutePath.ToLower();

            path = path + "?TYPE="+pid;
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

        #region ActionHandler
        /// <summary>
        /// Handling control events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, EventArgs e)
        {
            //Session Logout on Department change
            if (!(this.Master as ERPSMS_2).ValidatePageDept())
                return;

            TextBox WrkfComments;
            DropDownList ddlWkfAction;
            string action;
            CommonService CommonServiceClient;
            CommonServiceClient = null;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            bool bIsChecked = false;
            try
            {
               
                int? result;
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((LinkButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    if (((DropDownList)sender).ID == "ddlGroup")
                    {
                        commonActions = ActionsEnum.SELECTEDINDEXCHANGED;
                    }
                    if (((DropDownList)sender).ID == "ddlGroupParent")
                    {
                        commonActions = ActionsEnum.PARENTSELECTEDINDEXCHANGED;
                    }
                }
                else if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(RadioButton)))
                {
                    if (((RadioButton)sender).ID == "rbtSelect")
                    {
                        commonActions = ActionsEnum.ITEMSELECTED;
                    }
                }
                switch (commonActions)
                {
                    #region ItemSelected
                    case ActionsEnum.ITEMSELECTED:
                        GridViewRow gvr;
                        HiddenField hdfIBD_PK;  
                        int pk;
                        gvr = ((RadioButton)sender).Parent.Parent as GridViewRow;
                        hdfIBD_PK = gvr.FindControl("hdfIBD_PK") as HiddenField;
                        if (hdfIBD_PK != null && int.TryParse(hdfIBD_PK.Value, out pk))
                        {
                            CurrPK = pk;
                        }

                        break;
                    #endregion

                    #region RELIEFCLAIMLIST(Saved List)
                    case ActionsEnum.RELIEFCLAIMLIST:
                        ResetForm(ControlsEnum.RELIEFCLAIMLIST);
                        GetFieldValues(ControlsEnum.RELIEFCLAIMLIST);//SPFIN_INVOICE_CUS_BAD_DEBIT_GET_LIST
                        SetFieldValues(ControlsEnum.RELIEFCLAIMLIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion
                    #region ListLinkButton Click
                    case ActionsEnum.BADDEBITLIST:
                        ResetForm(ControlsEnum.RELIEFCLAIMLIST);
                        GetFieldValues(ControlsEnum.RELIEFCLAIMLIST);//Bad Debit Relief Claim List
                        SetFieldValues(ControlsEnum.RELIEFCLAIMLIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion
                    #region NEW
                    case ActionsEnum.NEW:
                        CurrPK = 0; ;
                        EntryStatus = EntryStatus.NEWMODE;
                        base.WkfRefID = ucrWrkf.RefID = 0;
                        ucrWrkf.FillWorkFlowDetails();
                        ucrWrkf.ViewType = 1;
                        ucrWrkf.ViewAction();
                        GetFieldValues(ControlsEnum.BADDEBITLIST);//SPFIN_INVOICE_BAD_DEBIT_GET
                        SetFieldValues(ControlsEnum.BADDEBITLIST);
                        break;
                    #endregion
                    #region BADDEBITDETAIL
                    case ActionsEnum.BADDEBITDETAIL:
                        SetUIEditView(commonActions);
                        WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                        base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                        ucrWrkf.FillWorkFlowDetails();
                        if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                            ucrWrkf.ViewType = 1;
                        else
                        {
                            ucrWrkf.ViewType = 0;                           
                            ucrWrkf.ViewAction();
                        }
                        pnlDetailSearch.Visible = false;
                        GetFieldValues(ControlsEnum.RELIEFDETAILSPECIFIC);//SPFIN_INVOICE_CUS_BAD_DEBIT_DTL_GET_KV
                        SetFieldValues(ControlsEnum.RELIEFDETAILSPECIFIC);
                        break;
                    #endregion
                    #region SEARCH
                    case ActionsEnum.SEARCH:
                        EntryStatus = EntryStatus.LISTMODE;
                        GetFieldValues(ControlsEnum.BADDEBITLIST);//SPFIN_INVOICE_BAD_DEBIT_GET
                        SetFieldValues(ControlsEnum.BADDEBITLIST);
                        break;
                    #endregion
                    #region RELIEFCLAIMSEARCH
                    case ActionsEnum.RELIEFCLAIMSEARCH:                       
                        GetFieldValues(ControlsEnum.RELIEFCLAIMLIST);//SPFIN_INVOICE_CUS_BAD_DEBIT_GET_LIST
                        SetFieldValues(ControlsEnum.RELIEFCLAIMLIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion
                    #region Clear
                    case ActionsEnum.CLEAR:
                        ResetForm(ControlsEnum.RELIEFCLAIMLIST);
                        GetFieldValues(ControlsEnum.RELIEFCLAIMLIST);
                        SetFieldValues(ControlsEnum.RELIEFCLAIMLIST);
                        break;
                    #endregion
                    #region SAVE
                    case ActionsEnum.SAVE:
                        BadDebitBOObj = (BadDebitBO)SetUIValuesToObject(ControlsEnum.BADDEBITDETAILS);
                        if (BadDebitBOObj != null)
                        {
                            string xmlDoc = CommonFunctions.XmlSerialize<BadDebitBO>(BadDebitBOObj);
                            result = BusinessLogic.Finance.BadDebitsBL.SaveBadDebitDetails(xmlDoc);
                            if (result > 0) // Success !  redirect to listing page
                            {
                                // Show Save Message and redired to listing page                                        
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, GetLocalResourceObject("BadDebit").ToString());
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                EntryStatus = EntryStatus.LISTMODE;
                                ResetForm(ControlsEnum.RELIEFCLAIMLIST);
                                GetFieldValues(ControlsEnum.RELIEFCLAIMLIST);
                                SetFieldValues(ControlsEnum.RELIEFCLAIMLIST);
                                EntryStatus = EntryStatus.LISTMODE;
                            }
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_SelectItem").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }

                        break;
                    #endregion
                    #region CANCEL
                    case ActionsEnum.CANCEL:
                        ResetForm(ControlsEnum.RELIEFCLAIMLIST);
                        CurrPK = 0;
                        FillProcessID(1);
                        GetFieldValues(ControlsEnum.RELIEFCLAIMLIST);
                        SetFieldValues(ControlsEnum.RELIEFCLAIMLIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion
                    #region Edit
                    case ActionsEnum.EDIT:
                        foreach (GridViewRow grdrow in grdSavedBadDebit.Rows)
                        {
                            RadioButton rbtSelect;                            
                            rbtSelect = (RadioButton)grdrow.FindControl("rbtSelect");                          
                            if (rbtSelect.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfIBD_PK")).Value);
                                Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                                Posted = Convert.ToBoolean(((HiddenField)grdrow.FindControl("hdfPosted")).Value);                               
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            SetUIEditView(commonActions);
                            WorkflowCore.CoreService workflowCore1 = new WorkflowCore.CoreService();
                            base.WkfRefID = ucrWrkf.RefID = workflowCore1.GetRefID(CurrPK, ucrWrkf.ProcessID);
                            ucrWrkf.FillWorkFlowDetails();
                            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                                ucrWrkf.ViewType = 1;
                            else
                            {
                                ucrWrkf.ViewType = 0;   
                            }
                            ucrWrkf.ViewAction();
                            GetFieldValues(ControlsEnum.RELIEFDETAILSPECIFIC);
                            SetFieldValues(ControlsEnum.RELIEFDETAILSPECIFIC); 
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Select").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }                          
                        break;
                    #endregion
                    #region SAVESUBMIT
                    case ActionsEnum.SAVESUBMIT:
                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ONE;
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    #endregion
                    #region Submit
                    case ActionsEnum.SUBMIT:
                        //Show WorkFlow Popup
                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    #endregion
                    #region WRKSubmit
                    case ActionsEnum.WRKFSUBMIT:                      
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        else if (BadDebitBOObj != null)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Min_Items").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else//valid
                        {
                            ucrWrkf.ApplicationID = 0;
                            if (hdfIsSaveSubmit.Value == CommonConstants.SELECT_VALUE_ONE)
                            {
                                BadDebitBOObj = (BadDebitBO)SetUIValuesToObject(ControlsEnum.BADDEBITDETAILS);
                                if (BadDebitBOObj != null)
                                {
                                    string xmlDoc = CommonFunctions.XmlSerialize<BadDebitBO>(BadDebitBOObj);
                                    result = BusinessLogic.Finance.BadDebitsBL.SaveBadDebitDetails(xmlDoc);
                                    if (result.HasValue && result.Value > 0) // Success !  redirect to listing page
                                    {
                                        ucrWrkf.ApplicationID = result.Value;
                                    }
                                    else
                                    {
                                        if (result == (int)DbSaveStatus.SQLERROR)
                                        {
                                            litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                        else if (result == (int)DbSaveStatus.CONCURRENCY)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.Expenses + " " + Resources.Messages.EditUsedByAnotherUser;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                        else if (result == (int)DbSaveStatus.CODEEXIST)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.Expenses + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                            + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                        }
                                        else if (result == (int)DbSaveStatus.REFNOEXIST)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.Expenses + " " + GetLocalResourceObject("RefNoExist").ToString();
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                            + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                        }
                                        else if (result == (int)DbSaveStatus.INVNOEXISTS)
                                        {
                                            litErrorMsg.Text = GetLocalResourceObject("InvNoExist").ToString();
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                            + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                        }
                                        else
                                        {
                                            litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.Expenses);
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                        return;
                                    }

                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1", "ClosePopup();", true);
                                    litErrorMsg.Text = Resources.Messages.Msg_ErrConversionFactor;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    return;
                                }
                            }
                            else if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)
                            {
                                if (BusinessLogic.CommonManagement.CommonBL.ValidationForCancellation((int)CurrPK, ApplicationType.EI))
                                {
                                    ucrWrkf.ApplicationID = (int)CurrPK;
                                }
                                else
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Err_EI_Cancel").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                    WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                                    FillProcessID(1);
                                    hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                                    WrkfComments.Text = "";
                                    EntryStatus = EntryStatus.LISTMODE;
                                    ResetForm(ControlsEnum.RELIEFCLAIMLIST);
                                    GetFieldValues(ControlsEnum.RELIEFCLAIMLIST);
                                    SetFieldValues(ControlsEnum.RELIEFCLAIMLIST);
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
                                        ADM_APP_TRX_LOG AdmTrxLogDet = new ADM_APP_TRX_LOG();
                                        //Show Save success message and reset Contract Entry
                                        if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)
                                        {
                                            FillProcessID(1);
                                            litErrorMsg.Text = Resources.Messages.Msg_Cancelled_Success;
                                            AdmTrxLogDet.ATL_ACTION = (byte)LogAction.CANCEL;
                                        }
                                        else
                                        {
                                            litErrorMsg.Text = GetLocalResourceObject("Msg_Submit_Success").ToString();
                                            AdmTrxLogDet.ATL_ACTION = (byte)LogAction.SUBMIT;
                                        }                                      
                                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                                        WrkfComments.Text = "";
                                        CurrPK = ucrWrkf.ApplicationID;                                    
                                        object[] args = new object[2];
                                        args[0] = GetLocalResourceObject("BadDebit").ToString(); 
                                        args[1] = "";

                                        #region LOG SAVE
                                        CommonServiceClient = new CommonService();
                                        CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);

                                        List<ADM_APP_TRX_LOG> AdmTrxLogList = new List<ADM_APP_TRX_LOG>();
                                        AdmTrxLogDet.ATL_APP_TRX_CODE = string.Empty;
                                        AdmTrxLogDet.ATL_APP_TYPE = ApplicationType.BD;
                                        AdmTrxLogDet.ATL_MOD_BY = Convert.ToInt16(currentUser.PKUser);
                                        AdmTrxLogDet.ATL_MOD_DT = DateTime.Now;
                                        AdmTrxLogDet.ATL_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                                        AdmTrxLogDet.ATL_APP_TRX_PK = Convert.ToInt16(currentUser.SBUID);
                                        AdmTrxLogDet.ATL_PK = 0;
                                        AdmTrxLogList.Add(AdmTrxLogDet);
                                        CommonServiceClient.SaveLog(AdmTrxLogList);
                                        CommonServiceClient = null;

                                        #endregion

                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, args);
                                        // Show Save Message and redired to listing page  
                                        if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && base.WkfRefID > 0)
                                        {
                                            ResetForm(ControlsEnum.RELIEFCLAIMLIST);                                       
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                                        + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.InboxURL) + "');", true);
                                        }
                                        else
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                            EntryStatus = EntryStatus.LISTMODE;
                                            ResetForm(ControlsEnum.RELIEFCLAIMLIST);
                                            GetFieldValues(ControlsEnum.RELIEFCLAIMLIST);
                                            SetFieldValues(ControlsEnum.RELIEFCLAIMLIST);
                                        }
                                    }
                                }
                            }
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1", "ClosePopup();", true);
                        break;
                    #endregion
                    #region Delete
                    case ActionsEnum.DELETE:
                        if (CurrPK > 0)
                        {
                            result = 0;
                            result = BusinessLogic.Finance.BadDebitsBL.DeleteBadDebitReliefDetails(CurrPK, LastModifiedTime, ApplicationType.BD, currentUser.PKUser.ToString());
                            if (result > 0) // Success ! re-initialize the page
                            {
                                //Show Save success message and reset Contract Entry
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.BadDebt);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                ResetForm(ControlsEnum.RELIEFCLAIMLIST);
                                GetFieldValues(ControlsEnum.RELIEFCLAIMLIST);
                                SetFieldValues(ControlsEnum.RELIEFCLAIMLIST);
                                EntryStatus = EntryStatus.LISTMODE;
                            }
                            else
                            {
                                if (result == (int)DbDeleteStatus.SQLERROR)
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.CONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.Expenses + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    ResetForm(ControlsEnum.RELIEFCLAIMLIST);
                                    GetFieldValues(ControlsEnum.RELIEFCLAIMLIST);
                                    SetFieldValues(ControlsEnum.RELIEFCLAIMLIST);
                                    EntryStatus = EntryStatus.LISTMODE;
                                }
                                else if (result == (int)DbDeleteStatus.REFERRED)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.Expenses + " " + Resources.Messages.UsedInAnotherPlace;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.Expenses + " " + Resources.Messages.AlreadyDeleted;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    ResetForm(ControlsEnum.RELIEFCLAIMLIST);
                                    GetFieldValues(ControlsEnum.RELIEFCLAIMLIST);
                                    SetFieldValues(ControlsEnum.RELIEFCLAIMLIST);
                                    EntryStatus = EntryStatus.LISTMODE;
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.Expenses);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                        }
                        break;
                    #endregion
                    #region Journalize
                    case ActionsEnum.JOURNALIZE:
                        SetUIValuesToObject(ControlsEnum.JOURNALIZE);
                        break;
                    #endregion
                    #region Journalize Update
                    case ActionsEnum.JOURNALIZEUPDATE:
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrJournalize.ResetForm();
                        EntryStatus = EntryStatus.LISTMODE;
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        ucrWrkf.Reset();
                        FillProcessID(1);
                        ResetForm(ControlsEnum.RELIEFCLAIMLIST);
                        GetFieldValues(ControlsEnum.RELIEFCLAIMLIST);
                        SetFieldValues(ControlsEnum.RELIEFCLAIMLIST);
                        break;
                    #endregion
                    #region Journalize Save
                    case ActionsEnum.JOURNALIZESAVE:
                        hdfJournalizeWorkFlow.Value = "0";
                        ((Button)sender).CommandName = ActionsEnum.SAVE.ToString();
                        ucrWrkf.Reset();
                        FillProcessID(1);
                        ResetForm(ControlsEnum.RELIEFCLAIMLIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        GetFieldValues(ControlsEnum.RELIEFCLAIMLIST);
                        SetFieldValues(ControlsEnum.RELIEFCLAIMLIST);
                        break;
                    #endregion
                    #region Journalize Submit
                    case ActionsEnum.JOURNALIZESUBMIT:
                        if (Session[ERP.Utilities.SessionStrings.Transaction] != null)
                        {
                            string Transaction = Session[ERP.Utilities.SessionStrings.Transaction].ToString();                           
                        }
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        FillProcessID(1);
                        ResetForm(ControlsEnum.RELIEFCLAIMLIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        GetFieldValues(ControlsEnum.RELIEFCLAIMLIST);
                        SetFieldValues(ControlsEnum.RELIEFCLAIMLIST);

                        break;
                    #endregion
                    #region Journalize Delete
                    case ActionsEnum.JOURNALIZEDELETE:
                        if (Session[ERP.Utilities.SessionStrings.Transaction] != null)
                        {
                            string Transaction = Session[ERP.Utilities.SessionStrings.Transaction].ToString();                           
                        }
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        FillProcessID(1);
                        ResetForm(ControlsEnum.RELIEFCLAIMLIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        GetFieldValues(ControlsEnum.RELIEFCLAIMLIST);
                        SetFieldValues(ControlsEnum.RELIEFCLAIMLIST);
                        break;
                    #endregion
                    #region Journalize Cancel
                    case ActionsEnum.JOURNALIZECANCEL:
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        FillProcessID(1);
                        ResetForm(ControlsEnum.RELIEFCLAIMLIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        GetFieldValues(ControlsEnum.RELIEFCLAIMLIST);
                        SetFieldValues(ControlsEnum.RELIEFCLAIMLIST);
                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
            finally
            {

            }
        }

        /// <summary>
        /// Method used to Handle all actions in the page with GridViewSort
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewSortEventArgs e)
        {
            string GridID = ((GridView)sender).ID;
            try
            {
               
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        /// <summary>
        /// Method used to Handle all actions in the page with GridView Row Binding
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            RadioButton rbtSelect;
            try
            {

                if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                {
                    if (((GridView)sender).ID == "grdBadDebitDetails")
                    {
                        rbtSelect = e.Row.FindControl("rbtSelect") as RadioButton;
                        if (CurrPK > 0)
                        {
                            rbtSelect.Checked = true;
                            //rdoSelection.Enabled = false;
                        }
                    }
                }
               
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        /// <summary>
        /// Page Index Handler for grdBadDebitDetails
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewPageEventArgs e)
        {
            PageIndex = e.NewPageIndex.ToString();
            GetFieldValues(ControlsEnum.RELIEFCLAIMLIST);
            SetFieldValues(ControlsEnum.RELIEFCLAIMLIST);
            EntryStatus = EntryStatus.LISTMODE;
        }

        #endregion             

        #region Pager Methods + Init
        /// <summary>
        /// Methord  For PageInite Used to set the Pager current Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, System.EventArgs e)
        {
            btnSaveSubmit.PreRender += new EventHandler(btnAction_PreRender);
            btnSubmit.PreRender += new EventHandler(btnAction_PreRender);
            btnSave.PreRender += new EventHandler(btnAction_PreRender);
            btnDeleteBD.PreRender += new EventHandler(btnAction_PreRender);        
            btnCancel.PreRender += new EventHandler(btnAction_PreRender);
            btnJournalize.PreRender += new EventHandler(btnAction_PreRender);
            
            btnCancelSubmit.PreRender += new EventHandler(btnAction_PreRender);
            btnEditforCancel.PreRender += new EventHandler(btnAction_PreRender);

            btnNew.PreRender += new EventHandler(btnAction_PreRender);
            btnEdit.PreRender += new EventHandler(btnAction_PreRender);
            btnView.PreRender += new EventHandler(btnAction_PreRender);         
            lnkList.PreRender += new EventHandler(btnAction_PreRender);
            lnkDetail.PreRender += new EventHandler(btnAction_PreRender);
        

            btnSaveSubmit.Load += new EventHandler(btnAction_Load);
            btnSubmit.Load += new EventHandler(btnAction_Load);
            btnSave.Load += new EventHandler(btnAction_Load);
            btnDeleteBD.Load += new EventHandler(btnAction_Load);   
            btnCancel.Load += new EventHandler(btnAction_Load);
            btnJournalize.Load += new EventHandler(btnAction_Load);          
            btnCancelSubmit.Load += new EventHandler(btnAction_Load);
            btnEditforCancel.Load += new EventHandler(btnAction_Load);

            btnNew.Load += new EventHandler(btnAction_Load);
            btnEdit.Load += new EventHandler(btnAction_Load);
            btnView.Load += new EventHandler(btnAction_Load);
        

            lnkList.Load += new EventHandler(btnAction_Load);
            lnkDetail.Load += new EventHandler(btnAction_Load);
              
        }

        /// <summary>
        /// Button Load event
        /// Resets visibility
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAction_Load(object sender, EventArgs e)
        {
            (sender as Control).Visible = true;
        }
        /// <summary>
        /// To handle the visibility of the action corresponding to the users Privilage
        /// Leave this section if using Master Screens
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAction_PreRender(object sender, EventArgs e)
        {
            base.CheckBtnVisibility(sender);
        }

        /// <summary>
        /// Method Used to initialize the Pager Control
        /// </summary>
        private void InitializeComponent()
        {
            this.Init += new EventHandler(this.Page_Init);
        }
               /// <summary>
        /// Methord used to enable and Disable Page Navigation Controls
        /// </summary>
        /// <param name="iTotalPages"></param>
        private void EnableDisableButtons(int iTotalPages)
        {
            //    uclPaging.FirstButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;

            //    uclPaging.PreviousButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;

            //    uclPaging.NextButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;

            //    uclPaging.LastButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
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

        /// <summary>
        /// To Handle Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            try
            {
                
                if (EntryStatus == EntryStatus.VIEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "PageViewMode", "$(document).ready(function(){PageViewMode(1);});", true);
                    lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                }
                else if (EntryStatus == EntryStatus.NEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "PageViewMode", "$(document).ready(function(){PageViewMode(2);});", true);
                    lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                }
                else if (EntryStatus == EntryStatus.ENTRYMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                    lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                }
                else if (EntryStatus == EntryStatus.LISTMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowListing", "$(document).ready(function(){ShowListing(1);});", true);
                    lnkList.CssClass = GetLocalResourceObject("TabActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabInActive").ToString();
                }
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);              
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion

        #region ControlEnum
        public enum ControlsEnum
        {
            DEFAULT,
            RELIEFCLAIMLIST,
            BADDEBITLIST ,
            BADDEBITDETAILS,
            RELIEFDETAILSPECIFIC,
            RELIEFDETAILGET,
            JOURNALIZE,
            FINHEADER,
            GETBDPKBYJOURNALPK
        }
        #endregion
    }
}