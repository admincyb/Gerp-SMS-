using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERP.Utilities;
using BusinessObject.Common;
using BusinessObject.AccountManagement;
using BusinessObject;
using ERPManager;
using ERPSMS_v01.UserControls;
using System.Data;
using BusinessObject.CommonManagement;
using BusinessObject.SaleOrder;
using System.Xml;
using System.Text;
using System.Threading;
using ERPService;
using ERPData;
using System.Web.UI.HtmlControls;
using BusinessObject.AlertManagement;
using BusinessObject.Finance;
using ERP.Store.UI;



namespace ERPSMS_v01.Finance
{
    public partial class VoucherLocking : MyBasePage
    {
        #region Variables and Properties
        #region Properties
        /// <summary>
        /// Is continue
        /// </summary>
        private bool Iscont
        {
            get
            {
                return this.ViewState[ViewstateStrings.Iscont] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.Iscont]);
            }
            set
            {
                this.ViewState[ViewstateStrings.Iscont] = value;
            }
        }
        /// <summary>
        /// Is Otehr charge deducted from inv
        /// </summary>
        private bool IsOCded
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsOCded] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsOCded]);
            }
            set
            {
                this.ViewState[ViewstateStrings.IsOCded] = value;
            }
        }

        /// <summary>
        /// Is Other charge Edited
        /// </summary>
        private bool IsOCEdit
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsOCEdit] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsOCEdit]);
            }
            set
            {
                this.ViewState[ViewstateStrings.IsOCEdit] = value;
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
        /// Invoice
        /// </summary>
        private string Invoice
        {
            get
            {
                return (string)this.ViewState[ViewstateStrings.Invoice];
            }
            set
            {
                this.ViewState[ViewstateStrings.Invoice] = value;
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
        /// GON PK
        /// </summary>
        private int DespatchID
        {
            get
            {
                return this.ViewState[ViewstateStrings.DespatchID] != null ? Convert.ToInt32(this.ViewState[ViewstateStrings.DespatchID]) : 0;
            }
            set
            {
                this.ViewState[ViewstateStrings.DespatchID] = value;
            }
        }
        /// <summary>
        /// Current Quotation PK
        /// </summary>
        private int CurrSOPK
        {
            get
            {
                return this.ViewState[ViewstateStrings.CurrSOPK] != null ? Convert.ToInt32(this.ViewState[ViewstateStrings.CurrSOPK]) : 0;
            }
            set
            {
                this.ViewState[ViewstateStrings.CurrSOPK] = value;
            }
        }
        /// <summary>
        /// Receipt detail PK
        /// </summary>
        private int ReceiptMpgPK
        {
            get
            {
                return this.ViewState[ViewstateStrings.ReceiptMpgPK] != null ? Convert.ToInt32(this.ViewState[ViewstateStrings.ReceiptMpgPK]) : 0;
            }
            set
            {
                this.ViewState[ViewstateStrings.ReceiptMpgPK] = value;
            }
        }
        /// <summary>
        /// Tax PK
        /// </summary>
        private int TaxPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.TaxPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.TaxPK] = value;
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
        /// Sale Order TYPE
        /// </summary>
        private int SaleOrderType
        {
            get
            {
                return (int)this.ViewState[ViewstateStrings.SaleOrderType];
            }
            set
            {
                this.ViewState[ViewstateStrings.SaleOrderType] = value;
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
        /// To maintain keep SO Invoice Header Tax Splitting
        /// </summary>
        private SOInvoiceHeader SOInvoiceHeaderSession
        {
            get
            {
                return (SOInvoiceHeader)Session[ERP.Utilities.SessionStrings.SOInvoiceHeaderSession];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.SOInvoiceHeaderSession] = value;
            }
        }
        /// <summary>
        /// To maintain keep PO Invoice Header Tax Splitting
        /// </summary>
        private SOInvoiceHeader TempInvoiceHeaderTemp
        {
            get
            {
                return (SOInvoiceHeader)Session[ERP.Utilities.SessionStrings.TempSOInvoiceDtlSession];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.TempSOInvoiceDtlSession] = value;
            }
        }

        /// <summary>
        /// To maintain keep SO Invoice Header Tax Splitting
        /// </summary>
        private SOInvoiceHeader TempSOInvoiceHeaderSession
        {
            get
            {
                return (SOInvoiceHeader)this.ViewState[ERP.Utilities.SessionStrings.TempSOInvoiceHeaderSession];
            }
            set
            {
                this.ViewState[ERP.Utilities.SessionStrings.TempSOInvoiceHeaderSession] = value;
            }
        }

        /// <summary>
        /// To maintain keep CustomerAll Pending Allocation
        /// </summary>
        private SOInvoiceHeader TempSOInvoiceHeaderSessionCustAll
        {
            get
            {
                return (SOInvoiceHeader)this.ViewState[ERP.Utilities.SessionStrings.TempSOInvoiceHeaderSessionCustAll];
            }
            set
            {
                this.ViewState[ERP.Utilities.SessionStrings.TempSOInvoiceHeaderSessionCustAll] = value;
            }
        }


        /// <summary>
        /// To maintain keep SO Invoice Dtl Tax deduction from line item
        /// </summary>       
        private List<SOInvoiceDetails> TempORGsoInvoiceDetailsList
        {
            get
            {
                return (List<SOInvoiceDetails>)this.ViewState[ERP.Utilities.SessionStrings.TempSOInvoiceDtlSession];
            }
            set
            {
                this.ViewState[ERP.Utilities.SessionStrings.TempSOInvoiceDtlSession] = value;
            }
        }
        /// <summary>
        /// SO Invoice PK
        /// </summary>
        private int SOInvoicePK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.SOInvoicePK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.SOInvoicePK] = value;
            }
        }

        private int SelectedItemPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.SelectedItemPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedItemPK] = value;
            }
        }
        private bool IsHeaderTax
        {
            get
            {
                return Convert.ToBoolean(this.ViewState[ViewstateStrings.IsHeaderTax]);
            }
            set
            {
                this.ViewState[ViewstateStrings.IsHeaderTax] = value;
            }
        }

        private bool IsEditMode
        {
            get
            {
                return Convert.ToBoolean(this.ViewState[ViewstateStrings.IsEditMode]);
            }
            set
            {
                this.ViewState[ViewstateStrings.IsEditMode] = value;
            }
        }

        private string SelectedTaxText
        {
            get
            {
                return (string)this.ViewState[ViewstateStrings.SelectedTaxText];
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedTaxText] = value;
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
        /// To maintain keep selected pos
        /// </summary>
        private List<long> SelectedSalesInvoices
        {
            get
            {
                return (List<long>)Session[ERP.Utilities.SessionStrings.SelectedSalesInvoices];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.SelectedSalesInvoices] = value;
            }

        }
        /// <summary>
        /// To maintain keep selected Currency
        /// </summary>
        private long SelectedCurrency
        {
            get
            {
                return Convert.ToInt64(this.ViewState[ViewstateStrings.SelectedInvoiceType]);
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedInvoiceType] = value;
            }

        }
        /// <summary>
        /// To maintain keep selected Currency
        /// </summary>
        private long SelectedInvoiceType
        {
            get
            {
                return Convert.ToInt64(this.ViewState[ViewstateStrings.SelectedCurrency]);
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedCurrency] = value;
            }

        }

        private long SelectedCustomers
        {
            get
            {
                return Convert.ToInt64(this.ViewState[ViewstateStrings.SelectedCustomers]);
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedCustomers] = value;
            }
        }
        /// <summary>
        /// To maintain keep selected invoices
        /// </summary>
        private List<long> SelectedInvoicesCrDr
        {
            get
            {
                return (List<long>)Session[ERP.Utilities.SessionStrings.SelectedInvoicesCrDr];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.SelectedInvoicesCrDr] = value;
            }
        }
        private List<decimal> SelectedINVTax
        {
            get
            {
                return (List<decimal>)this.ViewState["SelectedINVTax"];
            }
            set
            {
                this.ViewState["SelectedINVTax"] = value;
            }

        }
        private decimal INVTax
        {
            get
            {
                return (decimal)this.ViewState["INVTax"];
            }
            set
            {
                this.ViewState["INVTax"] = value;
            }

        }


        /// <summary>
        /// To set custom tax config value
        /// </summary>
        private bool IsCustomTaxEnabled
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsCustomTaxEnabled] == null ? true : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsCustomTaxEnabled]);
            }
            set
            {
                this.ViewState[ViewstateStrings.IsCustomTaxEnabled] = value;
            }
        }

        private int SelectedRowIndex
        {
            get
            {
                return this.ViewState[ViewstateStrings.CurrRow] == null ? -1 : Convert.ToInt32(this.ViewState[ViewstateStrings.CurrRow].ToString());
            }
            set
            {
                this.ViewState[ViewstateStrings.CurrRow] = value;
            }
        }
        #endregion
        // Indicates the state as well as action
        private ActionsEnum commonActions;
        private ControlsEnum controlEnum;
        User currentUser;
        private ServiceUtility serviceUtilityObj;
        private List<ADM_COMPANY_MST> admCompanyMstList;

        SOInvoiceDetails soDtlObj;
        string selectedVendor;
        DataSet dsInvHeader;
        DataTable dtTaxDetails;
        private DataSet dsPageData;
        private DataTable dtTransaction;
        private DataTable dtVoucherTypes;
        private DataTable dtLockList;
        private VoucherLockingBO objVoucherLoc;

        DataTable dtCompany = new DataTable();

        private int CustomerTypeSelectedPk;
        private string CustomerSavedBranchId;
        private string CustomerSavedTaxId;

        DataSet dsDueDate;
        private int paymentTermPK;
        private int custPK;
        private string scPK;
        bool hasValidRate;
        private FIN_INVOICE_CUS_HDR finInvoiceCusHdrObj;

        private FIN_TRX_HDR finTrxHdrObj;
        private List<FIN_TRX_HDR> finTrxHdrList;
        List<SAL_DESPATCH_DTL> salDespatchDtlList;


        private ADM_CONFIG_MST admConfigMstObj;
        private List<ADM_CONFIG_MST> workflowStatusList;

        private List<ADM_CONFIG_MST> admConfigMstList;
        private ADM_CURRENCY_MST admCurrencyMstObj;
        private List<ADM_CURRENCY_MST> admCurrencyMstList;
        private List<ADM_CONST_MST> admConstMstList;
        private ADM_APP_CONFIG_MST admAppConfigMstObj;
        private List<ADM_APP_CONFIG_MST> admAppConstMstList;
        private ADM_APP_TYPE_MST admAppTypeMstObj;
        private List<ADM_APP_TYPE_MST> admAppTypeMstList;
        DataSet dsAlertList;
        private int invPK;
        private string appType;
        private string TypeRef;
        string JournalType = string.Empty;

        private int JournalPK;
        private int shippingPlanPK;

        private string refID;
        private decimal totalAllocatedTax;
        private decimal totalAllocatedDiscount;
        private decimal amtAdjAdvDeduction = 0;
        private string inboxFlag;
        private decimal TotalHDRDiscount = 0;
        private int isHaveDiscount = 0;
        private long InvoicePk = 0;
        private decimal totalTaxSplitFooter = 0;

        private List<SPADM_APP_SUB_TYPE_DATA_GET_Result> AppTypeDetailsList;
        private CommonService cm;

        private ADM_COMPANY_MST admCompanyMstObj;

        private string[] _operators = { "-", "+", "/", "*", "^" };
        private Func<double, double, double>[] _operations = {
        (a1, a2) => a1 - a2,
        (a1, a2) => a1 + a2,
        (a1, a2) => a1 / a2,
        (a1, a2) => a1 * a2,
        (a1, a2) => Math.Pow(a1, a2)
    };

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
               
                if (!IsPostBack)
                {
                    if (Request.QueryString["MOD"] != null)
                    {
                        hdfLockModule.Value = Request.QueryString["MOD"].ToString();
                    }
                    ResetForm();
                    GetFieldValues(ControlsEnum.VOUCHERLOCKLIST);
                    SetFieldValues(ControlsEnum.VOUCHERLOCKLIST);                  
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
            finally
            {
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
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));           
            try
            {
                switch (type)
                {
                    #region VOUCHERLOCKLIST
                    case ControlsEnum.VOUCHERLOCKLIST:
                        //serviceUtilityObj = new ServiceUtility();
                        //serviceUtilityObj.CurrentPage = uclPaging.CurrentPage == 0 ? 1 : uclPaging.CurrentPage;
                        //serviceUtilityObj.PageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
                        //serviceUtilityObj.TotalRecords = 0;

                        dtLockList = new DataTable();
                        dtLockList = BusinessLogic.Finance.VoucherLockingBL.GetVoucherLockList(null, null, currentUser.SBUID,Convert.ToInt32(hdfLockModule.Value));

                        //serviceUtilityObj.TotalRecords = dtVoucherList.Rows.Count > 0 ? Convert.ToInt32(dtVoucherList.Rows[0]["REC_COUNT"].ToString()) : 0;
                        //TotalPages = serviceUtilityObj.TotalRecords == 0 ? 1 : (serviceUtilityObj.TotalRecords <= serviceUtilityObj.PageSize) ? 1 :
                        //            (serviceUtilityObj.TotalRecords % serviceUtilityObj.PageSize) == 0 ? (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) :
                        //            (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) + 1;
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

                    #region VOUCHERLOCKLIST
                    case ControlsEnum.VOUCHERLOCKLIST:
                        BindGrid(controlType);
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
        #region Set UI Values To Object

        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>
        private Object SetUIValuesToObject(ControlsEnum controlType)
        {
            Object retObject;
            retObject = null;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            VoucherLockingBO VoucherLockHdr = new VoucherLockingBO();
            List<YearEndVoucherDetails> yearEndDetailsList = new List<YearEndVoucherDetails>();
            YearEndVoucherDetails yearEndDetails = new YearEndVoucherDetails();

            try
            {
                switch (controlType)
                {
                    #region YEARENDVOUCHERHDR
                    case ControlsEnum.VOUCHERLOCKHDR:
                        VoucherLockHdr.FLL_ACTIVE = (int)DbActiveStatus.ACTIVE;
                        VoucherLockHdr.FLL_BIZUNIT = Convert.ToInt32(currentUser.SBUID);
                        VoucherLockHdr.FLL_CRTD_BY = currentUser.PKUser;
                        VoucherLockHdr.FLL_CRTD_DT = DateTime.Now;
                        VoucherLockHdr.FLL_DATE = DateTime.Parse(txtLockUpTo.Text);
                        VoucherLockHdr.FLL_PK = 0;
                        VoucherLockHdr.FLL_REMARKS = HttpUtility.HtmlEncode(txtRemarks.Text);
                        VoucherLockHdr.FLL_VERSION = 0;
                        VoucherLockHdr.FLL_MODULE = Convert.ToInt32(hdfLockModule.Value);
                        retObject = VoucherLockHdr;
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
        /// <summary>
        /// Sets the UI input controls from the object values
        /// </summary>
        private void GetUIValuesFromObject(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    default: ;
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// function used to bind Drop Downs Corresponding to the Drop Down passed
        /// </summary>
        private void BindDropDown(ControlsEnum controlType)
        {
            switch (controlType)
            {
                default: ;
                    break;
            }
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

                    case ControlsEnum.VOUCHERLOCKLIST:
                            //uclPaging.TotalPages = TotalPages;
                            //PageIndex = PageIndex == null ? CommonConstants.SELECT_VALUE_ONE : PageIndex;                            
                            //uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                            //grdLockList.PageIndex = Convert.ToInt32(PageIndex);
                            grdLockList.DataSource = dtLockList;
                            grdLockList.DataBind();
                            //uclPaging.Visible = true;
                            //uclPaging.BindPager();                     
                            break;


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Set values to the control when edit details
        /// </summary>
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
        /// Button actions events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, EventArgs e)
        {
            //Session Logout on Department change
            if (!(this.Master as ERPSMS_2).ValidatePageDept())
                return;
            try
            {              
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                int? result;              

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
                            objVoucherLoc = new VoucherLockingBO();
                            objVoucherLoc = (VoucherLockingBO)SetUIValuesToObject(ControlsEnum.VOUCHERLOCKHDR);
                            if (objVoucherLoc != null)
                            {

                                result = BusinessLogic.Finance.VoucherLockingBL.SaveVoucherLockingDetails(objVoucherLoc);
                                if (result > 0) // Success !  redirect to listing page
                                {
                                    ResetForm();
                                    GetFieldValues(ControlsEnum.VOUCHERLOCKLIST);
                                    SetFieldValues(ControlsEnum.VOUCHERLOCKLIST);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + GetLocalResourceObject("Msg_Save_Success") + "','" + Resources.Messages.Information + "');", true);
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                        }
                        break;
                    #endregion                  
                    #region CANCEL
                    case ActionsEnum.CANCEL:
                        ResetForm();
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
        ///  Method used to Reset Dorm Controls
        /// </summary>
        private void ResetForm()
        {
            txtLockUpTo.Text = string.Empty;
            txtRemarks.Text = string.Empty;     
        }

        #region --- For Grid Actions----
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        /// <summary>
        /// Page Index Handler for grdCurrency
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewPageEventArgs e)
        {
            PageIndex = e.NewPageIndex.ToString();
            GetFieldValues(ControlsEnum.VOUCHERLIST);
            SetFieldValues(ControlsEnum.VOUCHERLIST);
            EntryStatus = EntryStatus.LISTMODE;
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

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion
        #endregion
        #region Pager Methods + Init
        /// <summary>
        /// Methord  For PageInite Used to set the Pager current Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, System.EventArgs e)
        {
            //uclPaging.CurrentPage = 1;          
        }

        /// <summary>
        /// Button Load event
        /// Resets visibility
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAction_Load(object sender, EventArgs e)
        {
            //(sender as Control).Visible = true;
        }
        /// <summary>
        /// To handle the visibility of the action corresponding to the users Privilage
        /// Leave this section if using Master Screens
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAction_PreRender(object sender, EventArgs e)
        {
            //base.CheckBtnVisibility(sender);
        }

        /// <summary>
        /// Method Used to initialize the Pager Control
        /// </summary>
        private void InitializeComponent()
        {
            //this.uclPaging.FirstPage += new ActionHandler(this.ActionHandler);
            //this.uclPaging.PreviousPage += new ActionHandler(this.ActionHandler);
            //this.uclPaging.NextPage += new ActionHandler(this.ActionHandler);
            //this.uclPaging.LastPage += new ActionHandler(this.ActionHandler);
            //this.uclPaging.PageChanged += new ActionHandler(this.ActionHandler);
            this.Init += new EventHandler(this.Page_Init);
        }

        /// <summary>
        /// Action Handlers For Pager Control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, DataNavigatorEventArgs e)
        {
            try
            {
                //switch (e.Action)
                //{
                //    case NavigationEnum.PAGECHANGE:
                //        uclPaging.CurrentPage = e.CurrentPage;
                //        break;
                //    case NavigationEnum.FIRST:
                //        if (e.CurrentPage > 1)
                //            uclPaging.CurrentPage = 1;
                //        break;
                //    case NavigationEnum.LAST:
                //        if (e.CurrentPage <= e.TotalPages)
                //            uclPaging.CurrentPage = e.TotalPages;
                //        break;
                //    case NavigationEnum.NEXT:
                //        // increment the current page index.
                //        if (e.CurrentPage <= e.TotalPages)
                //            uclPaging.CurrentPage++;
                //        break;
                //    case NavigationEnum.PREVIOUS:
                //        // Decrement the current page index.
                //        if (e.CurrentPage > 1)
                //            uclPaging.CurrentPage--;
                //        break;


                //}

                //PageIndex = uclPaging.CurrentPage.ToString();
                //// Change Code As per the page
                //GetFieldValues(ControlsEnum.VOUCHERLIST);
                //SetFieldValues(ControlsEnum.VOUCHERLIST);
                //EntryStatus = EntryStatus.LISTMODE;
                ////============================
                //EnableDisableButtons(e.TotalPages);
            }
            catch (Exception ex)
            {
                // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.gComsRes.Information + "');", true);
            }

        }

        /// <summary>
        /// Methord used to enable and Disable Page Navigation Controls
        /// </summary>
        /// <param name="iTotalPages"></param>
        private void EnableDisableButtons(int iTotalPages)
        {
            //uclPaging.FirstButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
            //uclPaging.PreviousButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
            //uclPaging.NextButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
            //uclPaging.LastButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
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
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion
        
        #region Enum
        /// <summary>
        ///Page Controls Enum 
        /// </summary>
        public enum ControlsEnum
        {

            EXCHANGERATE,
            JOURNALIZE,
            FINHEADER,
            FILLWORKFLOWSTATUS,
            COMPANY,
            TRANSACTIONS,
            VOUCHERTYPES,
            VOUCHERLIST,
            GETINVOICEPKBYJOURNALPK,
            YEARENDVOUCHER,
            JOURNALIZATIONTYPE,
            PARTYDDL,
            VOUCHERDDL,
            VOUCHERLOCKHDR,
            VOUCHERLOCKLIST

        }
        /// <summary>
        /// Define Activity Status Enum
        /// </summary>
        public enum WkfStatusEnum
        {
            DRAFTED = 0,
            APPROVED = 2
        }


        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoadComplete(EventArgs e)
        {
            if (!IsPostBack)
            {
                //assign Page BreadCrumb
                base.OnLoadComplete(e);
                AssignLocalBreadCrumb();
            }
        }
        /// <summary>
        /// Assign breadCrumb
        /// </summary>
        public void AssignLocalBreadCrumb()
        {
            try
            {
                //Breadcrumb Material Return
                if (Request.QueryString["MOD"] != null && Request.QueryString["MOD"].ToString() == Convert.ToByte(BusinessObject.CommonManagement.LockingModule.SMS).ToString())//2-->SMS,10-->Finance (ADM_MODULE_MST)
                {
                    if (this.GetLocalResourceObject("BreadcrumbInventory") != null)
                    {
                        string breadCrumb;
                        breadCrumb = this.GetLocalResourceObject("BreadcrumbInventory").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
                        lblBreadCrum.Text = breadCrumb;
                        Page.Title = GetLocalResourceObject("InventoryLockTitle").ToString();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

    }
}