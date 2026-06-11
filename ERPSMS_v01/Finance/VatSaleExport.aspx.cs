using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject;
using BusinessObject.AccountManagement;
using BusinessObject.Common;
using BusinessObject.CommonManagement;
using BusinessObject.POInvoicing;
using ERP.Utilities;
using ERPData;
using ERPManager;
using ERPService;
using ERPSMS_v01.UserControls;
using BusinessObject.AlertManagement;
using BusinessObject.PurchaseOrderManagement;
using BusinessObject.Finance;
using System.IO;
namespace ERPSMS_v01.Finance
{
    public partial class VatSaleExport : ERP.Store.UI.MyBasePage // ERP.Store.UI.WorkFlowBasePage
	{
        #region Variables and Properties
        #region Properties
        /// <summary>
        /// Vat Sale Item PK
        /// </summary>
        private int vatSaleItemPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.vatSaleItemPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.vatSaleItemPK] = value;
            }
        }
        /// <summary>
        /// Vat sale List
        /// </summary>
        private VatSale VatSaleSession
        {
            get
            {
                return (VatSale)Session[ERP.Utilities.SessionStrings.VatSaleSession];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.VatSaleSession] = value;
            }
        }
        private VatSaleDetails VatSaleDetailsSession
        {
            get
            {
                return (VatSaleDetails)Session[ERP.Utilities.SessionStrings.VatSaleDetailsSession];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.VatSaleDetailsSession] = value;
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
        /// WorkFlow RefID
        /// </summary>
        public int WkfRefID
        {
            get
            {
                return (this.ViewState["BaseWkfRefID"] == null ? 0 : (int)this.ViewState["BaseWkfRefID"]);
            }
            set
            {
                this.ViewState["BaseWkfRefID"] = value;
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
        /// Current PK
        /// </summary>
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
        /// Current Quotation PK
        /// </summary>
        private int CurrPOPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.CurrSOPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.CurrSOPK] = value;
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
        /// To maintain keep selected Currency
        /// </summary>
        private long SelectedCurrency
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
        private long SelectedVendors
        {
            get
            {
                return Convert.ToInt64(this.ViewState[ViewstateStrings.SelectedVendors]);
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedVendors] = value;
            }
        }
        /// <summary>
        /// Process ID of the Page
        /// </summary>
        //private int WkfStatus
        //{
        //    get
        //    {
        //        return this.ViewState["WkfStatus"] == null ? Convert.ToByte(0) : Convert.ToByte(this.ViewState["WkfStatus"]);
        //    }
        //    set
        //    {
        //        this.ViewState["WkfStatus"] = value;
        //    }
        //}
        #endregion
        // Indicates the state as well as action
        private ActionsEnum commonActions;
        User currentUser;
        TextBox WrkfComments;
        DropDownList ddlWkfAction;
        bool hasValidRate;
        private string refID;
        private List<SPADM_APP_SUB_TYPE_DATA_GET_Result> AppTypeDetailsList;
        private CommonService cm;
        private List<ADM_CURRENCY_MST> admCurrencyMstList;
        private string[] _operators = { "-", "+", "/", "*", "^" };
        private Func<double, double, double>[] _operations = {
        (a1, a2) => a1 - a2,
        (a1, a2) => a1 + a2,
        (a1, a2) => a1 / a2,
        (a1, a2) => a1 * a2,
        (a1, a2) => Math.Pow(a1, a2)
        };

        //page related Entity Object
        private DataTable dtVatSale = new DataTable();
        private DateTime selectedDate;
        private DateTime startOfMonth;
        private DateTime endOfMonth;
        private VatSale vatSaleObj;
        List<VatSaleDetails> vatSalesDetailsList;
        private VatSaleDetails vatSaleDetailsObj;
        private DataSet dsVatSaleList;
        private StringBuilder sb;
        private DataTable dtVatSaleSearch;

        private int saveResult;
        #endregion
        #region PageLevel Events
        /// <summary>
        /// Page load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
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
                    string[] itemkeyarray;
                    itemkeyarray = new string[1];
                    itemkeyarray[0] = "TSD_SL_NO";
                    grdVatSaleList.DataKeyNames = itemkeyarray;
                    hdfNumberDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits.ToString();
                    hdfCurrencyDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits.ToString();
                    hdfDecimalDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits.ToString();

                    //GetFieldValues(ControlsEnum.VATSALELIST);
                    //SetFieldValues(ControlsEnum.VATSALELIST);
                    hdfDecimalFormat.Value = "#0.";
                    for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits; i++)
                    {
                        hdfDecimalFormat.Value += "0";
                    }
                    hdfCurrencyFormat.Value = "#0.";
                    for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits; i++)
                    {
                        hdfCurrencyFormat.Value += "0";
                    }
                    hdfRateFormat.Value = "#0.";
                    int rateDecimalDigits = (Session[ERP.Utilities.SessionStrings.RateDecimalDigitP2P] == null
                        ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits
                        : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.RateDecimalDigitP2P]));
                    //hdfExchangeDigits.Value = rateDecimalDigits.ToString();
                    for (int i = 0; i < rateDecimalDigits; i++)
                    {
                        hdfRateFormat.Value += "0";
                    }

                    hdfExngRateFormat.Value = "#0.";
                    int rateExngDecimalDigits = (Session[ERP.Utilities.SessionStrings.ExchRateDecimalDigit] == null
                        ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits
                        : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.ExchRateDecimalDigit]));
                    hdfExchangeDigits.Value = rateExngDecimalDigits.ToString();
                    for (int i = 0; i < rateExngDecimalDigits; i++)
                    {
                        hdfExngRateFormat.Value += "0";
                    }
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
                    #region  VATSALELIST
                    case ControlsEnum.VATSALELIST:
                        selectedDate = Convert.ToDateTime(txtCalender.Text);
                        startOfMonth = new DateTime(selectedDate.Year, selectedDate.Month, 1);
                        endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);
                        vatSaleObj = BusinessLogic.Finance.VatSaleExportBL.GetVatSale(
                            new BusinessObject.GridPrams()
                           {
                               SortBy = string.IsNullOrEmpty(SortBy) ? "TSD_INVOICE_DATE" : SortBy,
                               SortDirection = string.IsNullOrEmpty(SortDirection) ? Resources.Report.SortDescending : SortDirection,
                               ThenBy = SortBy == ThenBy || SortBy == "TSD_INVOICE_NO" ? string.Empty : string.IsNullOrEmpty(ThenBy) ? "TSD_INVOICE_NO" : ThenBy,
                               ThenDirection = SortBy == ThenBy || SortBy == "TSD_INVOICE_NO" ? string.Empty : string.IsNullOrEmpty(ThenDirection) ? Resources.Report.SortDescending : ThenDirection,
                               FromDate = string.IsNullOrEmpty(startOfMonth.ToString()) ? DateTime.Now.AddYears(-100).ToString() : startOfMonth.ToString(),
                               ToDate = string.IsNullOrEmpty(endOfMonth.ToString()) ? DateTime.Now.AddYears(10).ToString() : endOfMonth.ToString(),
                           }, currentUser);
                        if (vatSaleObj != null)
                        {
                            VatSaleSession = vatSaleObj;
                            CurrPK = vatSaleObj.TSH_PK;
                            LastModifiedTime = string.IsNullOrEmpty(vatSaleObj.LAST_MOD_DT) ? DateTime.Now : Convert.ToDateTime(vatSaleObj.LAST_MOD_DT);
                        }
                        else
                        {
                            vatSaleObj = null;
                            VatSaleSession = vatSaleObj;
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Edit_Delete").ToString();
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.VatSaleExport);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region  VATSALESEARCHLIST
                    case ControlsEnum.VATSALESEARCHLIST:
                        dtVatSaleSearch = BusinessLogic.Finance.VatSaleExportBL.GetVatSaleSearch(
                            new BusinessObject.GridPrams()
                            {
                                FromDate = string.IsNullOrEmpty(txtSearchFromDate.Text.Trim()) ? DateTime.Now.AddYears(-100).ToString() : txtSearchFromDate.Text.Trim(),
                                ToDate = string.IsNullOrEmpty(txtSearchToDate.Text.Trim()) ? DateTime.Now.AddYears(10).ToString() : txtSearchToDate.Text.Trim(),
                            },HttpUtility.HtmlEncode(txtSearchCustomer.Text.Trim()), txtSearchInvoiceNo.Text.Trim(), currentUser);
                        if (dtVatSaleSearch == null)
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Edit_Delete").ToString();
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.VatSaleExport);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                        }
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
                    case ControlsEnum.VATSALELIST:
                        BindGrid(controlType);
                        break;
                    case ControlsEnum.VATSALESEARCHLIST:
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
        /// <summary>
        /// Check the uploaded file is valid
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        private bool IsValidExtension(string extension)
        {
            bool flag = true;
            return flag;
        }
        /// <summary>
        /// Method to Fill Process ID
        /// </summary>
        //
        //private int FillProcessId()
        //{
        //    int procId = 0;
        //    string path;
        //    path = "/Finance/FCReverse.aspx";
        //    WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
        //    DataTable dtProcess = wrkfService.GetProcessID(path, currentUser.CurrentDeptPK);
        //    if (dtProcess != null && dtProcess.Rows.Count > 0)
        //    {
        //        procId = int.Parse(dtProcess.Rows[0]["PROCESS_PK"].ToString());
        //    }
        //    return procId;
        //}
        /// <summary>
        /// Funtion used get doc mode
        /// </summary>
        private string GetDOCMODE()
        {
            cm = new CommonService();
            AppTypeDetailsList = cm.GetReportParameters(ApplicationType.FCHR, 0, DateTime.Now);
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
        /// Funtion used get FC NO
        /// </summary>
        private void getFCReverseNo()
        {
            //cm = new CommonService();
            //currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            //string WhtNo = cm.GetTrxDocNo(ApplicationType.FCHR, 0, currentUser.CurrentDeptPK,
            //    txtFCDate.Text == string.Empty ? DateTime.Now : Convert.ToDateTime(txtFCDate.Text), currentUser.PKUser, true, 0);
            //hdfFCReverseNo.Value = WhtNo;
        }
        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>
        private Object SetUIValuesToObject(ControlsEnum controlType)
        {
            Object retObject;
            retObject = null;
            bool bIsChecked = false;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                switch (controlType)
                {
                    #region Items
                    case ControlsEnum.VATSALELIST:
                        if (CurrSlNo != 0 && vatSalesDetailsList != null)
                        {
                            vatSaleDetailsObj = vatSalesDetailsList.FirstOrDefault(itm => itm.TSD_SL_NO == CurrSlNo);
                            //vatSaleDetailsObj = vatSalesDetailsList.SingleOrDefault(itm => itm.TSD_SL_NO == CurrSlNo);
                            if (vatSaleDetailsObj != null)
                            {
                                vatSaleDetailsObj.TSD_INVOICE_DATE = Convert.ToDateTime(txtItemInvoiceDate.Text).ToString();
                                vatSaleDetailsObj.TSD_INVOICE_NO = HttpUtility.HtmlEncode(txtItemInvoiceNo.Text);
                                vatSaleDetailsObj.TSD_DECLARATION_NO= HttpUtility.HtmlEncode(txtItemDeclarationNo.Text);
                                vatSaleDetailsObj.TSD_CUSTOMER_NAME= HttpUtility.HtmlEncode(txtItemCustomerName.Text);
                                vatSaleDetailsObj.TSD_ITEM_TEXT= HttpUtility.HtmlEncode(txtItemProduct.Text);
                                vatSaleDetailsObj.TSD_NET_VALUE_TC = string.IsNullOrEmpty(txtItemValueFOB.Text.Trim()) ? 0 : Convert.ToDouble(txtItemValueFOB.Text.Trim());
                                vatSaleDetailsObj.TSD_EXCHG_RATE = string.IsNullOrEmpty(txtItemExchangeRate.Text.Trim()) ? 0 : Convert.ToDouble(txtItemExchangeRate.Text.Trim());
                                vatSaleDetailsObj.TSD_NET_VALUE_BC = string.IsNullOrEmpty(txtItemAmountTHB.Text.Trim()) ? 0 : Convert.ToDouble(txtItemAmountTHB.Text.Trim());
                            }
                        }
                        retObject = vatSalesDetailsList;
                        break;
                    #endregion
                    #region Save
                    case ControlsEnum.ALERTSAVE:
                        vatSaleObj = VatSaleSession;
                        vatSaleObj.TSH_PK = CurrPK;
                        vatSaleObj.TSH_STATUS = 0;
                        vatSaleObj.ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        selectedDate = Convert.ToDateTime(txtCalender.Text);
                        startOfMonth = new DateTime(selectedDate.Year, selectedDate.Month, 1);
                        endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);
                        vatSaleObj.TSH_FROM_DATE = startOfMonth.ToString();
                        vatSaleObj.TSH_TO_DATE = endOfMonth.ToString();
                        vatSaleObj.USER_PK = currentUser.PKUser;
                        vatSaleObj.BIZUNIT_PK = currentUser.SBUID;
                        vatSaleObj.TSH_DEPT = currentUser.CurrentDeptPK;
                        vatSaleObj.LAST_MOD_DT = LastModifiedTime.ToString();
                        retObject = vatSaleObj;
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
        /// Sets the UI input controls from the object values
        /// </summary>
        private void GetUIValuesFromObject(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region Line Item Details
                    case ControlsEnum.SELECTEDITEM:
                        if (vatSaleDetailsObj != null)
                        {
                            vatSaleItemPK = vatSaleDetailsObj.TSD_PK;
                            hdfDetailPK.Value = CommonConstants.SELECT_VALUE_ONE;
                            CurrSlNo = vatSaleDetailsObj.TSD_SL_NO;
                            txtItemInvoiceDate.Text = Convert.ToDateTime(vatSaleDetailsObj.TSD_INVOICE_DATE.ToString()).ToString("dd-MMM-yyyy");
                            txtItemInvoiceNo.Text = vatSaleDetailsObj.TSD_INVOICE_NO.ToString();
                            txtItemDeclarationNo.Text =HttpUtility.HtmlDecode(vatSaleDetailsObj.TSD_DECLARATION_NO.ToString());
                            txtItemCustomerName.Text = HttpUtility.HtmlDecode(vatSaleDetailsObj.TSD_CUSTOMER_NAME.ToString());
                            //if( vatSaleDetailsObj.TSD_ITEM_TEXT != null ? txtItemProduct.Text = vatSaleDetailsObj.TSD_ITEM_TEXT.ToString(): txtItemProduct.Text=string.Empty)
                            if (vatSaleDetailsObj.TSD_ITEM_TEXT != null)
                            {
                                txtItemProduct.Text = HttpUtility.HtmlDecode(vatSaleDetailsObj.TSD_ITEM_TEXT.ToString());
                            }
                            txtItemValueFOB.Text = GetFormattedCurrency(vatSaleDetailsObj.TSD_NET_VALUE_TC).Replace(",", "");
                            //txtItemExchangeRate.Text = GetFormattedRate(vatSaleDetailsObj.TSD_EXCHG_RATE).Replace(",", "");
                            txtItemExchangeRate.Text = GetFormattedRateExngRate(vatSaleDetailsObj.TSD_EXCHG_RATE).Replace(",", "");
                            txtItemAmountTHB.Text = GetFormattedCurrency(vatSaleDetailsObj.TSD_NET_VALUE_BC).Replace(",", "");
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
        /// <summary>
        /// function used to bind Drop Downs Corresponding to the Drop Down passed
        /// </summary>
        private void BindDropDown(ControlsEnum controlType)
        {
            //switch (controlType)
            {
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
                    case ControlsEnum.VATSALELIST:
                        grdVatSaleList.DataSource = null;
                        vatSaleObj = VatSaleSession;
                        if (vatSaleObj != null && vatSaleObj.listVatSaleDetail.Count>0)
                        {
                            grdVatSaleList.DataSource = vatSaleObj.listVatSaleDetail;
                        }
                        grdVatSaleList.DataBind();
                        break;
                    case ControlsEnum.VATSALESEARCHLIST:
                        grdVatSaleSearchList.DataSource = null;
                        if (dtVatSaleSearch.Rows.Count > 0)
                        {
                            grdVatSaleSearchList.DataSource = dtVatSaleSearch;
                        }
                        grdVatSaleSearchList.DataBind();
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
        /// <summary>
        /// Method used to Reset Dorm Controls
        /// </summary>
        private void ResetForm(ControlsEnum controlType)
        {
            switch (controlType)
            {
                case ControlsEnum.ITEMDETAILS:
                    vatSaleItemPK = 0;
                    CurrSlNo = 0;
                    hdfDetailPK.Value = CommonConstants.SELECT_VALUE_ZERO;
                    txtItemAmountTHB.Text = string.Empty;
                    txtItemCustomerName.Text = string.Empty;
                    txtItemDeclarationNo.Text = string.Empty;
                    txtItemExchangeRate.Text = string.Empty;
                    txtItemInvoiceDate.Text = string.Empty;
                    txtItemInvoiceNo.Text = string.Empty;
                    txtItemProduct.Text = string.Empty;
                    txtItemValueFOB.Text = string.Empty;
                    break;
                case ControlsEnum.CLEAR:
                    //txtSearchFromDate.Text = string.Empty;
                    //txtSearchToDate.Text = string.Empty;
                    txtSearchFromDate.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("dd-MMM-yyyy");
                    txtSearchToDate.Text = Convert.ToDateTime(txtSearchFromDate.Text).AddMonths(1).AddDays(-1).ToString("dd-MMM-yyyy");
                    txtSearchCustomer.Text = "Select/Type";
                    txtSearchInvoiceNo.Text = "Select/Type";
                    grdVatSaleSearchList.DataSource = null;
                    grdVatSaleSearchList.DataBind();
                    break;
            }
        }
        private void setvisibility(ActionsEnum ActionsEnum)
        {
            //switch (ActionsEnum)
            {
            }
        }
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
        public string GetFormattedRate(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfRateFormat.Value);
        }
        public string GetFormattedRateExngRate(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfExngRateFormat.Value);
        }
        #endregion
        #region WorkFlow Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        private void FillddlBankCharge()
        {
            //GetFieldValues(ControlsEnum.BANKCURRENCYBASE);

            //ddlBankCharge.Items.Clear();
            //if (ddlCurrency.Items.Count > 0)
            //    ddlBankCharge.Items.Insert(0, (new ListItem(admCurrencyMstList[0].CUR_CODE, admCurrencyMstList[0].CUR_PK.ToString())));
            //if (currentUser.BaseCurrency.ToString().Trim() != ddlCurrency.SelectedValue)
            //    ddlBankCharge.Items.Insert(1, new ListItem(ddlCurrency.SelectedItem.Text, ddlCurrency.SelectedValue));
            //ddlHoldAccount.DataBind();

        }

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
            // if one page containes two process (pageurl?PID=1,pageurl?PID=2)
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
                    //ucrWrkf.PageUrl = path;
                    //ucrWrkf.ProcessID = int.Parse(dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString());
                    //hdfProcessID.Value = dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString();
                    //if (pid == 1)
                    //{
                    //    PageProcessID = ucrWrkf.ProcessID;
                    //}
                    //base.WkfPageUrl = path;
                }
            }
        }
        private string GetUrl()
        {
            string path = string.Empty;
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
                int? result;
                bool bIsChecked = false;

                string savePath = string.Empty;
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
                else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    if (((DropDownList)sender).ID == "ddlCurrency")
                    {
                        commonActions = ActionsEnum.BANKCURRENCY;
                    }
                    if (((DropDownList)sender).ID == "ddlHoldAccount")
                    {
                        commonActions = ActionsEnum.FCHOLDREVERTDTL;
                    }
                }
                else if (sender.GetType().IsEquivalentTo(typeof(TextBox)))
                {
                    if (((TextBox)sender).ID == "txtReverseNow")
                    {
                        commonActions = ActionsEnum.CHECKAMT;
                    }
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
                    #region Set
                    case ActionsEnum.SET:
                        GetFieldValues(ControlsEnum.VATSALELIST);
                        ResetForm(ControlsEnum.ITEMDETAILS);
                        SetFieldValues(ControlsEnum.VATSALELIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion
                    #region Edit Item
                    case ActionsEnum.EDITITEM:
                        ResetForm(ControlsEnum.ITEMDETAILS);
                        if (VatSaleSession.listVatSaleDetail != null && VatSaleSession.listVatSaleDetail.Count > 0)
                        {
                            CurrSlNo = Convert.ToInt32(grdVatSaleList.DataKeys[(((ImageButton)sender).Parent.Parent as GridViewRow).RowIndex][0]);
                            if (CurrSlNo > 0)
                            {
                                vatSaleDetailsObj = VatSaleSession.listVatSaleDetail.FirstOrDefault(row => CurrSlNo == row.TSD_SL_NO);
                                //vatSaleDetailsObj = VatSaleSession.listVatSaleDetail.SingleOrDefault(row => CurrSlNo == row.TSD_SL_NO);
                                GetUIValuesFromObject(ControlsEnum.SELECTEDITEM);
                                EntryStatus = EntryStatus.EDITMODE;
                            }
                        }
                        hdfIsItemDetailsVisible.Value = CommonConstants.SELECT_VALUE_ONE;
                        break;
                    #endregion
                    #region Add Item
                    case ActionsEnum.ADDITEM:
                        if (!IsValid)//validate Page
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Err_Add_Validation").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else//valid
                        {
                            vatSalesDetailsList = VatSaleSession.listVatSaleDetail;
                            vatSalesDetailsList = (List<VatSaleDetails>)SetUIValuesToObject(ControlsEnum.VATSALELIST);
                            if (vatSalesDetailsList != null && vatSalesDetailsList.Count > 0)
                            {
                                VatSaleSession.listVatSaleDetail = vatSalesDetailsList;
                                SetFieldValues(ControlsEnum.VATSALELIST);
                                ResetForm(ControlsEnum.ITEMDETAILS);
                                EntryStatus = EntryStatus.LISTMODE;
                            }
                        }
                        break;
                    #endregion
                    #region Clear Item
                    case ActionsEnum.CLEARITEM:
                        ResetForm(ControlsEnum.ITEMDETAILS);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion
                    #region Cancel
                    case ActionsEnum.CANCEL:
                        //ResetForm(ControlsEnum.ITEMDETAILS);
                        EntryStatus = EntryStatus.LISTMODE;
                        Response.Redirect(Resources.PageURL.FinanceInbox);
                        break;
                    #endregion
                    #region Save
                    case ActionsEnum.SAVE:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Err_Save_Validation").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else
                        {
                            if (VatSaleSession != null)
                            {
                                vatSaleObj = (VatSale)SetUIValuesToObject(ControlsEnum.ALERTSAVE);
                                if (vatSaleObj != null)
                                {
                                    string xmlDoc = CommonFunctions.XmlSerialize<VatSale>(vatSaleObj);
                                    dsVatSaleList = new DataSet();
                                    //result = BusinessLogic.Finance.VatSaleExportBL.SaveVatSale(xmlDoc);
                                    dsVatSaleList = BusinessLogic.Finance.VatSaleExportBL.SaveVatSale(xmlDoc);
                                    if (dsVatSaleList.Tables.Count > 0)
                                    {
                                        if (dsVatSaleList.Tables[0].Rows.Count > 0)
                                        {
                                            saveResult = Convert.ToInt32(dsVatSaleList.Tables[0].Rows[0][0].ToString());
                                            if (saveResult > 0)
                                            {
                                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.VatSaleExport);
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                                EntryStatus = EntryStatus.LISTMODE;
                                                GetFieldValues(ControlsEnum.VATSALELIST);
                                                SetFieldValues(ControlsEnum.VATSALELIST);
                                            }
                                            else
                                            {
                                                if (saveResult == (int)DbSaveStatus.SQLERROR)
                                                {
                                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                        + "','" + Resources.ErpRes.Information + "');", true);
                                                }
                                                else if (saveResult == (int)DbSaveStatus.CONCURRENCY)
                                                {
                                                    litErrorMsg.Text = Resources.PageNameRes.miscellaneous + " " + Resources.Messages.EditUsedByAnotherUser;
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                    + "','" + Resources.ErpRes.Information + "');", true);
                                                }
                                                else if (saveResult == (int)DbSaveStatus.CODEEXIST)
                                                {
                                                    litErrorMsg.Text = Resources.PageNameRes.miscellaneous + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                    + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                                }
                                                else if (saveResult == (int)DbSaveStatus.REFNOEXIST)
                                                {
                                                    litErrorMsg.Text = Resources.PageNameRes.miscellaneous + " " + GetLocalResourceObject("RefNoExist").ToString();
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                    + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                                }
                                                else if (saveResult == (int)DbSaveStatus.REFERRED)
                                                {
                                                    sb = new StringBuilder();
                                                    sb.Append(this.GetLocalResourceObject("Err_InvoiceDuplication").ToString());
                                                    for (int i = 0; i < dsVatSaleList.Tables[1].Rows.Count; i++)
                                                    {
                                                        sb.Append("<ul><li>" + dsVatSaleList.Tables[1].Rows[i][0].ToString() + "</li></ul>");
                                                    }
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(sb.ToString())
                                                                       + "','" + Resources.ErpRes.Information + "');", true);
                                                    return;
                                                }
                                                else
                                                {
                                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.miscellaneous);
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                        + "','" + Resources.ErpRes.Information + "');", true);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_SaveNoDataFound").ToString();
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.VatSaleExport);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                            }
                            hasValidRate = false;
                        }
                        break;
                    #endregion
                    #region Print
                    case ActionsEnum.PRINT:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else
                        {
                            if (VatSaleSession != null)
                            {
                                vatSaleObj = VatSaleSession;
                                bool printValue = false;
                                for (int i = 0; i < vatSaleObj.listVatSaleDetail.Count; i++)
                                {
                                    if (Convert.ToInt32(vatSaleObj.listVatSaleDetail[i].TSD_PK.ToString()) > 0)
                                    {
                                        printValue = true;
                                        break;
                                    }
                                }
                                if (printValue)
                                {
                                    if (vatSaleObj.listVatSaleDetail != null && vatSaleObj.listVatSaleDetail.Count > 0)
                                    {
                                        selectedDate = Convert.ToDateTime(txtCalender.Text);
                                        startOfMonth = new DateTime(selectedDate.Year, selectedDate.Month, 1);
                                        endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + null + "&APPTYPE=" + ApplicationType.VSE + "&FROMDATE=" + startOfMonth + "&TODATE=" + endOfMonth + "") + "');", true); ;
                                    }
                                }
                                else
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Msg_PrintNoDataFound").ToString();
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.VatSaleExport);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                }
                            }
                            else
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_PrintNoDataFound").ToString();
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.VatSaleExport);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region Search Popup
                    case ActionsEnum.SHOWPOPUP:
                        ResetForm(ControlsEnum.CLEAR);
                        GetFieldValues(ControlsEnum.VATSALESEARCHLIST);
                        SetFieldValues(ControlsEnum.VATSALESEARCHLIST);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1", "ClosePopup();", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divSearchPopup]','" + Resources.PageNameRes.VatSaleExport + "','1300','550');", true);
                        break;
                    #endregion
                    #region Search
                    case ActionsEnum.SEARCH:
                        GetFieldValues(ControlsEnum.VATSALESEARCHLIST);
                        SetFieldValues(ControlsEnum.VATSALESEARCHLIST);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divSearchPopup]','" + Resources.PageNameRes.VatSaleExport + "','1300','550');", true);
                        break;
                    #endregion
                    #region Search Clear
                    case ActionsEnum.CLEAR:
                        ResetForm(ControlsEnum.CLEAR);
                        GetFieldValues(ControlsEnum.VATSALESEARCHLIST);
                        SetFieldValues(ControlsEnum.VATSALESEARCHLIST);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divSearchPopup]','" + Resources.PageNameRes.VatSaleExport + "','1300','550');", true);
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

        #region --- For Grid Actions----
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (((GridView)sender).ID == "grdVatSaleList")
                {
                    if (e.Row.RowType == DataControlRowType.Footer)
                    {
                        Label lblTotalAmount = e.Row.FindControl("lblTotalAmount") as Label;
                        lblTotalAmount.Text = "";
                        decimal Amount = 0;
                        if (vatSaleObj!=null)
                        {
                            if (vatSaleObj.listVatSaleDetail != null && vatSaleObj.listVatSaleDetail.Count > 0)
                            {
                                Amount = decimal.Parse(vatSaleObj.listVatSaleDetail.Sum(so => so.TSD_NET_VALUE_BC).ToString());
                            }
                            lblTotalAmount.Text = Amount.ToString();
                            lblTotalAmount.Text = Convert.ToDecimal(lblTotalAmount.Text) < 0 ? "0" : lblTotalAmount.Text;
                            lblTotalAmount.Text = lblTotalAmount.ToolTip = String.Format("{0:c}", decimal.Parse(lblTotalAmount.Text.Replace(",", "")));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        /// <summary>
        /// Page Index Handler for grdVatSaleList
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewPageEventArgs e)
        {
            
        }
        /// <summary>
        /// Method used to Handle all actions in the page with GridViewSort
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewSortEventArgs e)
        {
            
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
            //btnSaveSubmit.PreRender += new EventHandler(btnAction_PreRender);
            //btnSubmit.PreRender += new EventHandler(btnAction_PreRender);
            //btnCancel.PreRender += new EventHandler(btnAction_PreRender);
            //btnSave.PreRender += new EventHandler(btnAction_PreRender);
            ////btnNew.PreRender += new EventHandler(btnAction_PreRender);
            ////btnDelete.PreRender += new EventHandler(btnAction_PreRender);
            ////btnJournalize.PreRender += new EventHandler(btnAction_PreRender);
            //btnEdit.PreRender += new EventHandler(btnAction_PreRender);
            //btnPrint.PreRender += new EventHandler(btnAction_PreRender);
            //btnView.PreRender += new EventHandler(btnAction_PreRender);
            ////lnkList.PreRender += new EventHandler(btnAction_PreRender);
            ////lnkDetail.PreRender += new EventHandler(btnAction_PreRender);

            ////btnSaveSubmit.Load += new EventHandler(btnAction_Load);
            //btnSubmit.Load += new EventHandler(btnAction_Load);
            //btnSave.Load += new EventHandler(btnAction_Load);
            //btnCancel.Load += new EventHandler(btnAction_Load);
            ////btnNew.Load += new EventHandler(btnAction_Load);
            ////btnDelete.Load += new EventHandler(btnAction_Load);
            ////btnJournalize.Load += new EventHandler(btnAction_Load);
            //btnPrint.Load += new EventHandler(btnAction_Load);
            //btnEdit.Load += new EventHandler(btnAction_Load);
            //btnView.Load += new EventHandler(btnAction_Load);
            ////lnkList.Load += new EventHandler(btnAction_Load);
            ////lnkDetail.Load += new EventHandler(btnAction_Load);
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
        /// Action Handlers For Pager Control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, DataNavigatorEventArgs e)
        {
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
            //InitializeComponent();

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
                if (EntryStatus == EntryStatus.EDITMODE)
                {
                    SetFocus(txtItemInvoiceDate);
                    //SetFocus(txtItemInvoiceNo);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowListing", "$(document).ready(function(){ShowHideItemDetails(1);});", true);
                }
                if (EntryStatus == EntryStatus.LISTMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowHideItemDetails();});", true);
                }
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideAdvancedSearch();});", true);
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
            VATSALELIST,
            ITEMDETAILS,
            SELECTEDITEM,
            VATSALEDETAIL,
            ALERTSAVE,
            ADDITEM,
            CLEAR,
            VATSALESEARCHLIST
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
	}
}