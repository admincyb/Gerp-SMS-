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
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.IO;
using BusinessLogic.CommonManagement;


namespace ERPSMS_v01.Sales
{
    public partial class DirectSalesInvoice : ERP.Store.UI.WorkFlowBasePage
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
        /// Is DO modified or not
        /// </summary>
        private bool IsDoModified
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsDoModified] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsDoModified]);
            }
            set
            {
                this.ViewState[ViewstateStrings.IsDoModified] = value;
            }
        }
        /// <summary>
        /// Is DO cancelled or not
        /// </summary>
        private int DOCancelStatus
        {
            get
            {
                return this.ViewState[ViewstateStrings.DOCancelStatus] == null ? 0 : Convert.ToInt32(this.ViewState[ViewstateStrings.DOCancelStatus]);
            }
            set
            {
                this.ViewState[ViewstateStrings.DOCancelStatus] = value;
            }
        }
        /// <summary>
        /// Invoice refresh button is visible or not
        /// </summary>
        private bool RefreshInvoice
        {
            get
            {
                return this.ViewState[ViewstateStrings.RefreshInvoice] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.RefreshInvoice]);
            }
            set
            {
                this.ViewState[ViewstateStrings.RefreshInvoice] = value;
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

        /// <summary>
        /// Is invoice is reloaded or not
        /// </summary>
        private bool ReloadInvoice
        {
            get
            {
                return this.ViewState[ViewstateStrings.ReloadInvoice] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.ReloadInvoice]);
            }
            set
            {
                this.ViewState[ViewstateStrings.ReloadInvoice] = value;
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
        /// To maintain the PageIndex in viewstate
        /// </summary>
        private string PageIndexSO
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
        /// To maintain the total pages in viewstate
        /// </summary>
        private int TotalPages
        {
            get
            {
                return this.ViewState[ViewstateStrings.TotalPages] != null ? Convert.ToInt32(this.ViewState[ViewstateStrings.TotalPages]) : 0;
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
        private DirectSOInvoiceHeader SOInvoiceHeaderSession
        {
            get
            {
                return (DirectSOInvoiceHeader)Session[ERP.Utilities.SessionStrings.DirectSOInvoiceHeaderSession];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.DirectSOInvoiceHeaderSession] = value;
            }
        }
        ///// <summary>
        ///// To maintain keep SO Invoice Header Tax Splitting
        ///// </summary>
        //private DirectSOInvoiceHeaderMul invoiceHeaderMulObj
        //{
        //    get
        //    {
        //        return (DirectSOInvoiceHeaderMul)Session[ERP.Utilities.SessionStrings.invoiceHeaderMulObj];
        //    }
        //    set
        //    {
        //        Session[ERP.Utilities.SessionStrings.invoiceHeaderMulObj] = value;
        //    }
        //}
        /// <summary>
        /// To maintain keep PO Invoice Header Tax Splitting
        /// </summary>
        private DirectSOInvoiceHeader TempInvoiceHeaderTemp
        {
            get
            {
                return (DirectSOInvoiceHeader)Session[ERP.Utilities.SessionStrings.DirectTempSOInvoiceDtlSession];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.DirectTempSOInvoiceDtlSession] = value;
            }
        }

        /// <summary>
        /// To maintain keep SO Invoice Header Tax Splitting
        /// </summary>
        private DirectSOInvoiceHeader TempSOInvoiceHeaderSession
        {
            get
            {
                return (DirectSOInvoiceHeader)this.ViewState[ERP.Utilities.SessionStrings.DirectTempSOInvoiceHeaderSession];
            }
            set
            {
                this.ViewState[ERP.Utilities.SessionStrings.DirectTempSOInvoiceHeaderSession] = value;
            }
        }

        /// <summary>
        /// To maintain keep CustomerAll Pending Allocation
        /// </summary>
        private DirectSOInvoiceHeader TempSOInvoiceHeaderSessionCustAll
        {
            get
            {
                return (DirectSOInvoiceHeader)this.ViewState[ERP.Utilities.SessionStrings.DirectTempSOInvoiceHeaderSessionCustAll];
            }
            set
            {
                this.ViewState[ERP.Utilities.SessionStrings.DirectTempSOInvoiceHeaderSessionCustAll] = value;
            }
        }

        ///// <summary>
        ///// To maintain keep CustomerAll Pending Allocation
        ///// </summary>
        //private DirectSOInvoiceHeaderMul TempSOInvoiceHeaderTSessionCustAll
        //{
        //    get
        //    {
        //        return (DirectSOInvoiceHeaderMul)this.ViewState[ERP.Utilities.SessionStrings.TempSOInvoiceHeaderTSessionCustAll];
        //    }
        //    set
        //    {
        //        this.ViewState[ERP.Utilities.SessionStrings.TempSOInvoiceHeaderTSessionCustAll] = value;
        //    }
        //}

        /// <summary>
        /// To maintain keep SO Invoice Dtl Tax deduction from line item
        /// </summary>       
        private List<DirectSOInvoiceDetails> TempORGsoInvoiceDetailsList
        {
            get
            {
                return (List<DirectSOInvoiceDetails>)this.ViewState[ViewstateStrings.DirectTempSOInvoiceDtlSession];
            }
            set
            {
                this.ViewState[ViewstateStrings.DirectTempSOInvoiceDtlSession] = value;
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
        /// <summary>
        /// Sale contract PK
        /// </summary>
        private int ScPK
        {
            get
            {
                return this.ViewState[ViewstateStrings.ScPK] == null ? 0 : Convert.ToInt32(this.ViewState[ViewstateStrings.ScPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.ScPK] = value;
            }
        }

        /// <summary>
        /// Sale contract details PK
        /// </summary>
        private int ScDetailsPK
        {
            get
            {
                return this.ViewState[ViewstateStrings.ScDetailsPK] == null ? 0 : Convert.ToInt32(this.ViewState[ViewstateStrings.ScDetailsPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.ScDetailsPK] = value;
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

        private string xmlDocSO
        {
            get
            {
                return (string)this.ViewState[ViewstateStrings.xmlDocSO];
            }
            set
            {
                this.ViewState[ViewstateStrings.xmlDocSO] = value;
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
        /// To maintain keep All Information(Customer,Currency etc) of InvoiceList (Multiple Paging)
        /// </summary>
        private List<DirectSelectionInfo> SelectedSalesInvoicesInfoLst
        {
            get
            {
                return (List<DirectSelectionInfo>)Session[ERP.Utilities.SessionStrings.SelectedSalesInvoicesInfoLst];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.SelectedSalesInvoicesInfoLst] = value;
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
        /// <summary>
        /// 
        /// </summary>
        private bool IsTaxForOtherCharge
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsTaxForOtherCharge] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsTaxForOtherCharge].ToString());
            }
            set
            {
                this.ViewState[ViewstateStrings.IsTaxForOtherCharge] = value;
            }
        }
        private bool IsExportExcel
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsExportExcel] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsExportExcel].ToString());
            }
            set
            {
                this.ViewState[ViewstateStrings.IsExportExcel] = value;
            }
        }
        private bool IsDeliveryTermsEnable
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsDeliveryTermsEnable] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsDeliveryTermsEnable].ToString());
            }
            set
            {
                this.ViewState[ViewstateStrings.IsDeliveryTermsEnable] = value;
            }
        }
        /// <summary>
        /// Tax calculation is enabled or disabled for advance invoice .
        /// </summary>
        private bool IsAdvInvHasTax
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsAdvInvHasTax] == null ? true : (bool)this.ViewState[ViewstateStrings.IsAdvInvHasTax];
            }
            set
            {
                this.ViewState[ViewstateStrings.IsAdvInvHasTax] = value;
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

        public double SCSubTotalAmount
        {
            get
            {
                return Convert.ToDouble(this.ViewState[ViewstateStrings.SCSubTotalAmount]);
            }
            set
            {
                this.ViewState[ViewstateStrings.SCSubTotalAmount] = value;
            }
        }

        public double SCTotalDiscountAmount
        {
            get
            {
                return Convert.ToDouble(this.ViewState[ViewstateStrings.SCTotalDiscountAmount]);
            }
            set
            {
                this.ViewState[ViewstateStrings.SCTotalDiscountAmount] = value;
            }
        }
        /// <summary>
        /// Current Sl No.
        /// </summary>
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
        private List<BusinessObject.SaleOrder.DirectFileDetailsSI> FileDetailsList
        {
            get
            {
                return Session[ERP.Utilities.SessionStrings.FileDirectSOInvoiceDetailsList] == null ? null : (List<BusinessObject.SaleOrder.DirectFileDetailsSI>)Session[ERP.Utilities.SessionStrings.FileDirectSOInvoiceDetailsList];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.FileDirectSOInvoiceDetailsList] = value;
            }
        }

        private List<BusinessObject.SaleOrder.DirectSOInvoiceUploads> SOInvoiceUploadList
        {
            get
            {
                return ViewState[ViewstateStrings.SOInvoiceUploadList] == null ? null : (List<BusinessObject.SaleOrder.DirectSOInvoiceUploads>)ViewState[ViewstateStrings.SOInvoiceUploadList];
            }
            set
            {
                ViewState[ViewstateStrings.SOInvoiceUploadList] = value;
            }
        }
        private int ContineInvoiceAmtGreaterThanSCAmt
        {
            get
            {
                return this.ViewState[ViewstateStrings.ContineInvoiceAmtGreaterThanSCAmt] == null ? 0 : Convert.ToInt16(this.ViewState[ViewstateStrings.ContineInvoiceAmtGreaterThanSCAmt].ToString());
            }
            set
            {
                this.ViewState[ViewstateStrings.ContineInvoiceAmtGreaterThanSCAmt] = value;
            }
        }

        private DataTable dtPendingSOList
        {
            get
            {
                return this.ViewState[ViewstateStrings.OtherDetailList] == null ? new DataTable() : (DataTable)this.ViewState[ViewstateStrings.OtherDetailList];
            }
            set
            {
                this.ViewState[ViewstateStrings.OtherDetailList] = value;
            }
        }

        /// <summary>
        /// To keep config value for Enable/Disable Header Discount in viewstate
        /// </summary>
        private bool IsHeaderDiscountForTradingSale
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsHeaderDiscountForTradingSale] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsHeaderDiscountForTradingSale].ToString());
            }
            set
            {
                this.ViewState[ViewstateStrings.IsHeaderDiscountForTradingSale] = value;
            }
        }

        /// <summary>
        /// To keep config value for Enable/Disable Header Tax in viewstate
        /// </summary>
        private bool IsHeaderTaxForTradingSale
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsHeaderTaxForTradingSale] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsHeaderTaxForTradingSale].ToString());
            }
            set
            {
                this.ViewState[ViewstateStrings.IsHeaderTaxForTradingSale] = value;
            }
        }

        /// <summary>
        /// To keep config value for Enable/Disable Item Discount in viewstate
        /// </summary>
        private bool IsItemwiseDiscountForTradingSale
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsItemwiseDiscountForTradingSale] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsItemwiseDiscountForTradingSale].ToString());
            }
            set
            {
                this.ViewState[ViewstateStrings.IsItemwiseDiscountForTradingSale] = value;
            }
        }

        /// <summary>
        /// To keep config value for Enable/Disable Item Tax in viewstate  
        /// </summary>
        private bool IsItemwiseTaxForTradingSale
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsItemwiseTaxForTradingSale] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsItemwiseTaxForTradingSale].ToString());
            }
            set
            {
                this.ViewState[ViewstateStrings.IsItemwiseTaxForTradingSale] = value;
            }
        }
        #endregion
        // Indicates the state as well as action
        private ActionsEnum commonActions;
        private ControlsEnum controlEnum;
        User currentUser;
        private ServiceUtility serviceUtilityObj;
        private List<decimal> SelectedINVTaxList;
        //page related Entity Object
        private DirectSOInvoiceHeader invoiceHeaderObj;
        private DirectSOInvoiceHeader invoiceHeaderTemp;


        //private DirectSOInvoiceHeaderMul invoiceHeaderMulTemp;
        //                 private SOInvoiceDetails invoiceHdrMulDummyObj;
        private List<DirectSOInvoiceDetails> invoiceHdrMulDummyLIST;

        private DirectSOHeaderBO SoHeaderObj;

        private SAL_ORDER_HDR objSalesOrderHeader;
        private DirectDueDateDetails objDueDateDtls;
        private List<SAL_ORDER_HDR> salesOrderHeaderList;
        private DirectSOInvoiceDetails soInvoiceDetailsObj;
        private DirectSOInvoiceTaxHdr soInvTaxHdrObj;
        //private RFQTaxDtl rfqTaxDtlObj;
        List<DirectSOInvoiceDetails> soInvoiceDetailsList;

        private DirectDueDetail objDueDetail;
        List<DirectDueDetail> lstDueDetail;

        //List<RFQTaxDtl> rfqTaxDtlList;
        //RFQTaxSplit rfqDtlSplitObj;
        List<DirectSOInvoiceTaxHdr> taxHdrList;
        List<DirectSOAdvDeductionDetails> deductionDtlList;

        List<DirectSOAdvDeductionDetails> deductionDtlListCustAll;

        private List<ADM_COMPANY_MST> admCompanyMstList;
        DirectSOInvoiceDetails soDtlObj;
        string selectedVendor;
        DataSet dsInvHeader;
        DataTable dtTaxDetails;
        private DataSet dsPageData;
        private DataTable dtTaxDetData;
        private DataTable dtInvoiceList;
        private DataTable dtCustomTaxSet;
        //private DataTable dtPendingSOList;
        DataTable dtSOData;
        DataTable dtInvoiceType;
        DataTable dtTaxSettings;
        DataTable dtPaymentTerms;
        DataTable dtInvoiceGstType;
        DataTable dtCompany = new DataTable();
        DataTable dtAmountDetails;

        DataSet dsCustomerTypes;
        DataSet dsCustomerDetailsByType;
        private int CustomerTypeSelectedPk;
        private string CustomerSavedBranchId;
        private string CustomerSavedTaxId;

        DataSet dsDueDate;
        private int paymentTermPK;
        private int InvoiceGstTypePK;
        private int custPK = 0;
        private int deliveryOrderPK = 0;
        private string scPK;
        private string doPK;
        bool hasValidRate;
        private FIN_INVOICE_CUS_HDR finInvoiceCusHdrObj;

        private FIN_TRX_HDR finTrxHdrObj;
        private List<FIN_TRX_HDR> finTrxHdrList;


        private ADM_CONFIG_MST admConfigMstObj;
        private List<ADM_CONFIG_MST> workflowStatusList;

        private List<ADM_CONFIG_MST> admConfigMstList;
        private ADM_CURRENCY_MST admCurrencyMstObj;
        private List<ADM_CURRENCY_MST> admCurrencyMstList;
        private List<ADM_CONST_MST> admConstMstList;
        private ADM_APP_CONFIG_MST admAppConfigMstObj;
        private List<ADM_APP_CONFIG_MST> admAppConstMstList;
        DataSet dsAlertList;
        private int invPK;
        private string appType;
        private string TypeRef;


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
        DirectSOInvoiceUploads SOInvoiceUploadObj;
        bool isCancelled = false;

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
            string prefID;
            //bool isDespatchPref = false;
            try
            {
                //ConfigData abc = GetConfigData();// CommonFunctions.GetMulipltPlantConfigData();// Newtonsoft.Json.JsonConvert.DeserializeObject<ConfigData>(Request.Cookies[Resources.ErpRes.ConfigDataCookie].Value);

                if (Session[ERP.Utilities.SessionStrings.TransactionType] == null)
                {
                    ucrWrkf.WrkfSubmit += new EventHandler(ActionHandler);
                    hdfJournalizeWorkFlow.Value = "0";
                }
                ucrJournalize.JournalizeSave += new EventHandler(ActionHandler);
                ucrJournalize.JournalizeSubmit += new EventHandler(ActionHandler);
                ucrJournalize.JournalizeDelete += new EventHandler(ActionHandler);
                ucrJournalize.JournalizeCancel += new EventHandler(ActionHandler);

                ucrWrkf.ViewType = 1;

                if (!IsPostBack)
                {
                    DOCancelStatus = 0;
                    ReloadInvoice = false;
                    SOInvoiceHeaderSession = null;
                    GetFieldValues(ControlsEnum.COMPANY);
                    SetFieldValues(ControlsEnum.COMPANY);

                    //TaxPayable dIV VISIBILITY sETTING
                    ConfigurationSettings();
                    if (IsDeliveryTermsEnable == true)
                    {
                        lblDeliveryTerms.Visible = true;
                        txtDeliveryTerms.Visible = true;
                    }
                    else
                    {
                        lblDeliveryTerms.Visible = false;
                        txtDeliveryTerms.Visible = false;
                    }
                    string[] datakeyarray;
                    datakeyarray = new string[1];
                    datakeyarray[0] = Resources.DataFieldRes.SalesInvoicePK;
                    grdInvoiceList.DataKeyNames = datakeyarray;

                    FileDetailsList = null;
                    SOInvoiceUploadList = null;
                    string[] itemkeyarray;
                    itemkeyarray = new string[1];
                    itemkeyarray[0] = "DOC_SEQ_NO";
                    grdUploads.DataKeyNames = itemkeyarray;

                    lblInvoiceNo.Focus();
                    hdfNumberDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits.ToString();
                    hdfCurrencyDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits.ToString();
                    string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
                    hdfDecimalFormatWithSeperator.Value = "#" + currencysep + "#0.";
                    hdfJournalizeWorkFlow.Value = "0";
                    hdfDecimalFormat.Value = "#0.";
                    for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits; i++)
                    {
                        hdfDecimalFormat.Value += "0";
                        hdfDecimalFormatWithSeperator.Value += "0";
                    }
                    hdfCurrencyFormatWithSeperator.Value = "#" + currencysep + "#0.";
                    hdfCurrencyFormat.Value = "#0.";
                    for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits; i++)
                    {
                        hdfCurrencyFormat.Value += "0";
                        hdfCurrencyFormatWithSeperator.Value += "0";
                    }
                    hdfRateFormat.Value = "#0.";
                    int rateDecimalDigits = (Session[ERP.Utilities.SessionStrings.RateDecimalDigit] == null
                        ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits
                        : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.RateDecimalDigit]));
                    for (int i = 0; i < rateDecimalDigits; i++)
                    {
                        hdfRateFormat.Value += "0";
                    }

                    hdfCurrencyGroup2.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes[0].ToString();
                    hdfCurrencyGroup1.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes[1].ToString();

                    PageIndexSO = CommonConstants.SELECT_VALUE_ONE;
                    uclSOPaging.CurrentPage = 1;
                    uclInvListPaging.CurrentPage = 1;

                    GetFieldValues(ControlsEnum.INVOICETYPE);
                    GetFieldValues(ControlsEnum.INVOICEGSTTYPE);
                    SetFieldValues(ControlsEnum.INVOICEGSTTYPE);
                    GetFieldValues(ControlsEnum.SOTYPE);
                    SetFieldValues(ControlsEnum.SOTYPE);

                    //Enable or disable custom tax 
                    GetFieldValues(ControlsEnum.CUSTOMTAXSETTINGS);

                    SelectedInvoicesCrDr = null;
                    SelectedSalesInvoices = null;
                    SelectedSalesInvoicesInfoLst = null;

                    //txtFromDate.Text = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString(Resources.Constants.DateFormatShort);
                    //hdfFromDate.Value = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString();
                    //txtToDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    //hdfToDate.Value = DateTime.Now.ToString();
                    txtFromDate.Text = string.Empty;
                    hdfFromDate.Value = string.Empty;
                    txtToDate.Text = string.Empty;
                    hdfToDate.Value = string.Empty;
                    btnApply.Visible = false;
                    imgPopupAdd.Visible = false;
                    grdTaxDetails.Columns[3].Visible = true;

                    AST_DOC_MODE.Value = "0";


                    //if (Session[ERP.Utilities.SessionStrings.SALEORDERDOPK] != null)
                    //{
                    //    string xml = Session[ERP.Utilities.SessionStrings.SALEORDERDOPK].ToString();
                    //    XDocument xdoc = XDocument.Parse(xml);
                    //    xdoc.Declaration = null;

                    //    xml = xdoc.ToString();

                    //    //CurrSOPK = Convert.ToInt32(Session[ERP.Utilities.SessionStrings.SALEORDERDOPK]);
                    //    SoHeaderObj = new DirectSOHeaderBO();

                    //    SoHeaderObj = (DirectSOHeaderBO)GTIService.CommonFunctions.DeserializeObject(xml, SoHeaderObj);

                    //    Session[ERP.Utilities.SessionStrings.SALEORDERDOPK] = null;
                    //    ////start
                    //    EntryStatus = EntryStatus.NEWMODE;
                    //    ////
                    //    //  ddlInvoiceType.Enabled = true;
                    //}
                    //else
                    //{
                    //    ddlInvoiceType.Enabled = false;
                    //}
                    ////start
                    string pid = Request.QueryString[QueryStrings.PID] != null ? Request.QueryString[QueryStrings.PID]
                        : Session[ERP.Utilities.SessionStrings.PID] != null ? Session[ERP.Utilities.SessionStrings.PID].ToString().Split('=')[1] : string.Empty;
                    ////
                    int processId = 1;
                    //int sohType = 0;
                    if (SoHeaderObj != null && SoHeaderObj.SOList != null && SoHeaderObj.SOList.Count > 0)
                    {
                        GetFieldValues(ControlsEnum.SOINVHEADERBYPK);
                        //if (salesOrderHeaderList != null)
                        //{
                        //    sohType = salesOrderHeaderList[0].SOH_TYPE;
                        //    if (sohType == 1)
                        //        processId = 3;
                        //}
                        processId = salesOrderHeaderList != null ? salesOrderHeaderList[0].SOH_TYPE == 1 ? 3 : processId : processId;
                        SaleOrderType = salesOrderHeaderList != null ? salesOrderHeaderList[0].SOH_TYPE : 0;
                        hdfSaleOrderType.Value = SaleOrderType.ToString();
                        SetSaleorderTypeVisibility();
                    }
                    else
                    {
                        int.TryParse(pid, out processId);
                    }
                    //if (processId == 3)
                    //    FillProcessID(3);
                    //else
                    //    FillProcessID(1);
                    FillProcessID(1);
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

                    //If Request From External(Report or Other page) other than Menu or Inbox
                    if (Request.QueryString[QueryStrings.FromExt] != null && Request.QueryString[QueryStrings.FromExt] == "T")
                    {
                        ucrWrkf.ViewType = 0;
                        EntryStatus = EntryStatus.VIEWMODE;
                        GetFieldValues(ControlsEnum.INVOICEGET);
                        SetFieldValues(ControlsEnum.INVOICEGET);
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
                                //btnSave.Visible = false;
                                //btnSubmit.Visible = false;
                            }
                            else
                            {
                                ucrWrkf.ViewType = 1;
                                EntryStatus = EntryStatus.ENTRYMODE;
                            }

                            ////start
                            if (string.IsNullOrEmpty(pid) || pid.Equals("1") || pid.Equals("3") || pid.Equals("11") || pid.Equals("13"))
                            {
                                ucrWrkf.RefID = int.Parse(refID);
                                base.WkfRefID = ucrWrkf.RefID;
                                CurrPK = GetApplicationID(ucrWrkf.RefID);
                                if (pid.Equals("11") || pid.Equals("13"))
                                {
                                    hdfIsInvCancelled.Value = "1";//For Showing Cancelled Stamp in Detail Page
                                    IsDeleted = true;
                                }
                            }
                            else if (pid.Equals("2") || pid.Equals("12"))
                            {

                                ucrWrkf.RefID = int.Parse(refID);
                                JournalPK = GetApplicationID(ucrWrkf.RefID);
                                GetFieldValues(ControlsEnum.GETINVOICEPKBYJOURNALPK);
                                if (finTrxHdrList != null && finTrxHdrList.Count == 1)
                                {
                                    CurrPK = (Int32)finTrxHdrList[0].FTH_REF_PK;
                                    GetFieldValues(ControlsEnum.SOINVHEADER);
                                    if (SOInvoiceHeaderSession != null)
                                    {
                                        DespatchID = Convert.ToInt16(SOInvoiceHeaderSession.ICH_DESPATCH_HDR);
                                    }
                                    GetFieldValues(ControlsEnum.SOINVHEADER);

                                    if (SOInvoiceHeaderSession != null)
                                    {
                                        //if (Convert.ToInt32(SOInvoiceHeaderSession.ICH_TYPE) == Convert.ToInt32(SalesInvoiceType.Domestic))
                                        //    FillProcessID(3);
                                        //else
                                        //    FillProcessID(1);
                                        FillProcessID(1);
                                    }

                                    WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                                    base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                                }
                            }
                        }
                        else if (!string.IsNullOrEmpty(prefID))
                        {
                            if (string.IsNullOrEmpty(pid) || pid.Equals("1") || pid.Equals("3"))
                            {
                                DataTable dt = BusinessLogic.CommonManagement.CommonBL.GetTransactionID(Convert.ToInt32(prefID));
                                if (dt.Rows.Count > 0)
                                {
                                    CurrPK = Convert.ToInt32(dt.Rows[0]["appPK"]);
                                    //FillTransactionData();
                                }
                                else
                                {
                                    ////isDespatchPref = true;
                                    DespatchID = GetApplicationID(int.Parse(prefID));
                                    EntryStatus = EntryStatus.NEWMODE;
                                }
                            }
                            else if (pid.Equals("2"))
                            {
                                ucrWrkf.RefID = int.Parse(prefID);
                                base.WkfRefID = ucrWrkf.RefID;
                                CurrPK = GetApplicationID(ucrWrkf.RefID);
                            }
                        }
                        if (CurrPK > 0 || DespatchID > 0)
                        {
                            SetCancelRef(CurrPK);
                            ucrWrkf.FillWorkFlowDetails();
                            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && (EntryStatus == EntryStatus.ENTRYMODE || EntryStatus == EntryStatus.NEWMODE) && ucrWrkf.HasPageTaskPermission)
                                ucrWrkf.ViewType = 1;
                            else
                            {
                                ucrWrkf.ViewType = 0;
                                //EntryStatus = EntryStatus.VIEWMODE;
                            }
                            btnPrint.Visible = true;
                            //EntryStatus = EntryStatus.ENTRYMODE;
                            GetFieldValues(ControlsEnum.INVOICETYPE);
                            SetFieldValues(ControlsEnum.INVOICETYPE);
                            //if (SoHeaderObj != null && SoHeaderObj.SOList != null && SoHeaderObj.SOList.Count > 0)
                            //{
                            //    if (Session[ERP.Utilities.SessionStrings.GONPK] != null || isDespatchPref)
                            //    {
                            //        if (!isDespatchPref)
                            //        {
                            //            DespatchID = Convert.ToInt32(Session[ERP.Utilities.SessionStrings.GONPK]);
                            //        }
                            //    }
                            //}
                            //GetFieldValues(ControlsEnum.SOINVHEADER);
                            //if (SOInvoiceHeaderSession != null)
                            //{
                            //    DespatchID = Convert.ToInt16(SOInvoiceHeaderSession.ICH_DESPATCH_HDR);
                            //}
                            //GetFieldValues(ControlsEnum.SOINVHEADER);
                            //if (SoHeaderObj != null && SoHeaderObj.SOList != null && SoHeaderObj.SOList.Count > 0)
                            //{
                            //    if (Session[ERP.Utilities.SessionStrings.GONPK] != null || isDespatchPref)
                            //    {
                            //        if (!isDespatchPref)
                            //        {
                            //            DespatchID = Convert.ToInt32(Session[ERP.Utilities.SessionStrings.GONPK]);
                            //            GetFieldValues(ControlsEnum.DELIVERYORDERDETAILS);
                            //            Session[ERP.Utilities.SessionStrings.GONPK] = null;
                            //        }
                            //        SetFieldValues(ControlsEnum.DELIVERYORDERDETAILS);
                            //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateInvNow", "$(document).ready(function () { CalculateInvNow(1);});", true);
                            //    }
                            //    else
                            //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateInvNow", "$(document).ready(function () { CalculateInvNow();});", true);
                            //}
                            GetFieldValues(ControlsEnum.SOINVHEADER);

                            //if (CurrPK == 0 && SOInvoiceHeaderSession != null && SOInvoiceHeaderSession.OrderDetail != null)
                            //{
                            //    SOInvoiceHeaderSession.ICH_AMOUNT_TC = 0;
                            //    SOInvoiceHeaderSession.ICH_TAX_TC = 0;
                            //    SOInvoiceHeaderSession.ICH_DISCOUNT_TC = 0;
                            //    SOInvoiceHeaderSession.ICH_AMOUNT_NET_TC = 0;
                            //    SOInvoiceHeaderSession.ICH_AMOUNT_NET_BC = 0;
                            //    //invoiceHeaderMulObj.SOMainList.ForEach(s =>
                            //    //       {
                            //    //           foreach (DirectSOInvoiceDetails dtl in s.OrderDetail)
                            //    //           {
                            //    //               dtl.CID_INV_QTY_NOW = 0;
                            //    //               dtl.CID_AMOUNT = 0;
                            //    //               dtl.CID_TAX = 0;
                            //    //               dtl.CID_DISCOUNT = 0;
                            //    //               dtl.CID_NET_AMOUNT = 0;                                              
                            //    //               dtl.CID_QTY_INVOICED = dtl.CID_ORDERED_QTY - dtl.CID_INV_QTY;                                               
                            //    //               dtl.TaxDtl.ForEach(tdl =>
                            //    //               {
                            //    //                   if (tdl.CIT_TYPE == 1)
                            //    //                       tdl.CIT_TAX_AMT = 0;
                            //    //                   else
                            //    //                   {
                            //    //                       tdl.CIT_TAX_AMT = tdl.CIT_TAX_AMT / dtl.CID_ORDERED_QTY * dtl.CID_QTY_INVOICED;
                            //    //                   }
                            //    //               });
                            //    //           }
                            //    //       });
                            //}
                            SetFieldValues(ControlsEnum.SOINVHEADER);

                            //if (CurrSOPK > 0)
                            //{
                            //    if (Session[ERP.Utilities.SessionStrings.GONPK] != null || isDespatchPref)
                            //    {
                            //        if (!isDespatchPref)
                            //        {
                            //            DespatchID = Convert.ToInt32(Session[ERP.Utilities.SessionStrings.GONPK]);
                            //            GetFieldValues(ControlsEnum.DELIVERYORDERDETAILS);
                            //            Session[ERP.Utilities.SessionStrings.GONPK] = null;
                            //        }
                            //        SetFieldValues(ControlsEnum.DELIVERYORDERDETAILS);
                            //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateInvNow", "$(document).ready(function () { CalculateInvNow(1);});", true);
                            //    }
                            //    else
                            //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateInvNow", "$(document).ready(function () { CalculateInvNow();});", true);
                            //}


                            GetFieldValues(ControlsEnum.TAXSETTINGS);
                            SetFieldValues(ControlsEnum.SOINVDETAIL);
                            SetFieldValues(ControlsEnum.UPLOADEDFILES);
                            GetFieldValues(ControlsEnum.GETDUEDATE);
                            SetFieldValues(ControlsEnum.GETDUEDATE);
                            TempSOInvoiceHeaderSession = SOInvoiceHeaderSession;
                            SetDetailTax(null);
                            SetHdrTax();
                            if (grdInvoice.Rows.Count > 0)
                            {
                                SetSubTotal();
                            }

                            //Settings of TaxPayableDiv
                            if (hdfIsTaxPayable.Value.ToString() == "1")
                            {
                                SetTaxPayableDiv();
                            }
                            GetFieldValues(ControlsEnum.PEDINGSOLIST);
                            SetFieldValues(ControlsEnum.PEDINGSOLIST);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "TaxPayable", "$(document).ready(function () { ShowHideTaxPayableOuter();});", true);
                            //end

                        }
                        else
                        {
                            btnPrint.Visible = false;
                            GetFieldValues(ControlsEnum.INVOICELIST);
                            SetFieldValues(ControlsEnum.INVOICELIST);
                            EntryStatus = EntryStatus.LISTMODE;

                            //btnEdit.Visible=btnJournalize.Visible=btnPickForCrDrNote.Visible=btnPickForReceipt.Visible=
                            //    BusinessLogic.CommonManagement.CommonManagement.GetInitialTaskPermission(currentUser.PKUser, FillProcessId());
                        }
                        AST_DOC_MODE.Value = GetDOCMODE();
                        AST_CODE.Value = ApplicationType.DSI;
                        lblInvoiceNo.Text = string.IsNullOrEmpty(hdfInvoiceNo.Value.Trim()) ? Resources.ErpRes.Draft : hdfInvoiceNo.Value;

                        hdfAppType.Value = ApplicationType.DSI;
                        hdfAppSubType.Value = string.Empty;

                        hdfCurrentPk.Value = CurrPK.ToString();

                        if (grdInvoice.Rows.Count <= 0)
                        {
                            ResetForm(ControlsEnum.RESETFORMODIFIEDDO);
                        }
                        #endregion
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

        /// <summary>
        /// 
        /// </summary>
        private void ConfigurationSettings()
        {
            DataTable dt = BusinessLogic.CommonManagement.CommonBL.GetCurrencyConfiguration("CURRENCY SETTINGS", "ShowAmountInBC", currentUser.SBUID);
            if (dt != null && dt.Rows.Count > 0)
            {
                hdfIsTaxPayable.Value = dt.Rows[0]["ACF_VALUE"].ToString();//If 1 Show Div taxpayable else hide                           
            }
            //Edit Option Configuration of Other Charges and tax in AdvanceInvoice Allocation Popup           
            DataTable dtEditTaxOtherCharge = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("SALES ADV DED", "EDIT TAX");
            if (dtEditTaxOtherCharge != null && dtEditTaxOtherCharge.Rows.Count > 0)
            {
                hdfIsTaxOCEditable.Value = dtEditTaxOtherCharge.Rows[0]["ACF_VALUE"].ToString();
            }

            #region Advance Invoice Tax Settings
            DataTable dtTax = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("ADVANCE INVOICE SETTINGS", "TAX");
            if (dtTax != null && dtTax.Rows.Count > 0)
            {
                IsAdvInvHasTax = dtTax.Rows[0]["ACF_VALUE"].ToString() == "1" ? true : false;
                hdfIsAdvInvHasTax.Value = dtTax.Rows[0]["ACF_VALUE"].ToString();
            }
            #endregion

            #region ItemWiseTax settings
            // for line item tax & discount
            dt = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("SALE ITEM WISE SETTINGS", string.Empty, currentUser.SBUID);
            if (dt != null && dt.Rows.Count > 0)
            {
                hdfDetailTax.Value = dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "TAX")["ACF_VALUE"].ToString();
                hdfDetalDiscount.Value = dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "DISCOUNT")["ACF_VALUE"].ToString();
            }
            #endregion

            IsTaxForOtherCharge = Convert.ToBoolean(Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "OtherChargeTaxSales")));
            IsDeliveryTermsEnable = Convert.ToBoolean(Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "IsDeliveryTermEnable")));
            IsExportExcel = Convert.ToBoolean(Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "IsExportExcel")));
            ContineInvoiceAmtGreaterThanSCAmt = Convert.ToInt16(Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "ContineInvoiceAmtGreaterThanSCAmt")));

            IsHeaderDiscountForTradingSale = Convert.ToBoolean(Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "IsHeaderDiscountForTradingSale")));
            IsHeaderTaxForTradingSale = Convert.ToBoolean(Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "IsHeaderTaxForTradingSale")));
            IsItemwiseDiscountForTradingSale = Convert.ToBoolean(Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "IsItemwiseDiscountForTradingSale")));
            IsItemwiseTaxForTradingSale = Convert.ToBoolean(Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "IsItemwiseTaxForTradingSale")));
        }

        #region Get Field Values
        /// <summary>
        /// Gets the data to bind/assign for the controls(inputs, grids, dropdowns etc.)
        /// pass string.empty to get all field values
        /// Assign it to the page level variables
        /// </summary>
        private void GetFieldValues(ControlsEnum type)
        {
            AdmCompanyMstService admCompanyMstServiceClient;

            DataSet dsTaxDetails;
            DeliveryOrderService deliveryOrderServiceClient;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            FinTrxService finTrxServiceClient;
            finTrxServiceClient = null;
            deliveryOrderServiceClient = null;
            CommonService CommonServiceClient;
            CommonServiceClient = null;
            SaleOrderService salesOrderServiceClient;
            salesOrderServiceClient = null;
            SalesInvoiceService salesInvoiceServiceClient;
            salesInvoiceServiceClient = null;
            int TotalRecords = 0;
            try
            {
                switch (type)
                {
                    #region PEDING SO LIST
                    case ControlsEnum.PEDINGSOLIST:
                        TotalPages = 0;
                        custPK = 0;
                        TotalRecords = 0;
                        deliveryOrderPK = 0;
                        int.TryParse(hdfCustomer.Value, out custPK);
                        int.TryParse(hdfDPHPK.Value, out deliveryOrderPK);
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = string.IsNullOrEmpty(PageIndexSO) ? 1 : Convert.ToInt32(PageIndexSO);
                        serviceUtilityObj.PageSize = Convert.ToInt32(GetLocalResourceObject("PageSize_SOList"));
                        dtPendingSOList = BusinessLogic.Sales.SalesInvoiceBL.GetPendingSOList(custPK, deliveryOrderPK, CurrPK, serviceUtilityObj.CurrentPage, serviceUtilityObj.PageSize);

                        //txtTaxID.Text = HttpUtility.HtmlDecode(dtPendingSOList.Rows[0]["CUS_TAX_NO"].ToString());
                        if (dtPendingSOList != null && dtPendingSOList.Rows.Count > 0)
                        {
                            //set Total Page Count
                            TotalRecords = dtPendingSOList.Rows.Count > 0 ? Convert.ToInt32(dtPendingSOList.Rows[0]["TOTAL_ROW_COUNT"].ToString()) : 0;
                            TotalPages = TotalRecords == 0 ? 1 : (TotalRecords <= serviceUtilityObj.PageSize) ? 1 :
                                        (TotalRecords % serviceUtilityObj.PageSize) == 0 ? (TotalRecords / serviceUtilityObj.PageSize) :
                                        (TotalRecords / serviceUtilityObj.PageSize) + 1;
                        }
                        break;
                    #endregion
                    #region SOINVHEADER
                    case ControlsEnum.SOINVHEADER:
                        xmlDocSO = string.Empty;
                        if (SoHeaderObj != null && SoHeaderObj.SOList != null && SoHeaderObj.SOList.Count > 0)
                        {
                            xmlDocSO = CommonFunctions.XmlSerialize<DirectSOHeaderBO>(SoHeaderObj);
                        }
                        //int CustAllAdv = 0; //0: Default, 1:List all advance from the customer
                        //CustAllAdv = SaleOrderType == 1 ? 0 : 1;//SaleOrderType = 1 : Domestic
                        invoiceHeaderObj = BusinessLogic.Sales.SalesInvoiceBL.GetDirectSalesInvoiceHeaderMUL(xmlDocSO, !string.IsNullOrEmpty(xmlDocSO) ? 0 : CurrPK, DespatchID);
                        if (SOInvoiceHeaderSession == null)
                            SOInvoiceHeaderSession = invoiceHeaderObj.DeepClone();
                        else if (invoiceHeaderObj != null)
                        {
                            SOInvoiceHeaderSession.ICH_REFERENCE = invoiceHeaderObj.ICH_REFERENCE;
                            List<string> objSoList = SOInvoiceHeaderSession.OrderDetail.Select(r => r.CID_SO).Distinct().ToList();
                            List<long> objAdvList = SOInvoiceHeaderSession.DeductionDetails.Select(r => r.IAD_PK).ToList();
                            List<int> objOtherChargeSoList = SOInvoiceHeaderSession.TaxHdr.Select(r => r.CIT_SO).ToList();
                            SOInvoiceHeaderSession.OrderDetail.AddRange(invoiceHeaderObj.OrderDetail.Where(r => !objSoList.Contains(r.CID_SO)).ToList());
                            SOInvoiceHeaderSession.DeductionDetails.AddRange(invoiceHeaderObj.DeductionDetails.Where(r => !objAdvList.Contains(r.IAD_PK)).ToList());
                            SOInvoiceHeaderSession.TaxHdr.AddRange(invoiceHeaderObj.TaxHdr.Where(r => !objOtherChargeSoList.Contains(r.CIT_SO) && r.CIT_TAX_CATEGORY == (int)TaxType.Shipping).ToList());
                        }
                        break;
                    #endregion
                    #region Invoice Hdr By PK
                    case ControlsEnum.SOINVHEADERBYPK:
                        salesOrderServiceClient = new SaleOrderService();
                        salesOrderServiceClient = CommonFunctions.InitiateClient(salesOrderServiceClient);
                        objSalesOrderHeader = CommonFunctions.Initilize<SAL_ORDER_HDR>();
                        objSalesOrderHeader.SOH_PK = SoHeaderObj.SOList.FirstOrDefault().SOH_PK;
                        objSalesOrderHeader.SOH_ACTIVE = 1;
                        salesOrderHeaderList = salesOrderServiceClient.GetSaleOrderHdrByPK(objSalesOrderHeader);
                        break;
                    #endregion
                    #region Invoice Hdr By PK
                    case ControlsEnum.LINETAX:
                        dtTaxDetData = BusinessLogic.Sales.SaleOrderBL.GetLineitemTaxList(ReceiptMpgPK).Tables[0];//ReceiptMpgPK);
                        break;
                    #endregion
                    #region TAXTYPES
                    case ControlsEnum.TAXTYPES:
                        int category = 1;
                        int.TryParse(hdfTaxCategory.Value, out category);
                        if (TaxPK > 0)
                        {
                            dsTaxDetails = BusinessLogic.PurchaseOrderManagement.RequestForQuote.GetRFQTaxDetails(TaxPK, 0, currentUser.SBUID, Convert.ToByte(DbActiveStatus.HASPK), 0);
                            if (dsTaxDetails != null && dsTaxDetails.Tables.Count > 0)
                            {
                                dtTaxDetails = dsTaxDetails.Tables[0];
                            }
                        }
                        else
                        {
                            if ((int)TaxType.Discount != category)
                            {
                                dtTaxDetails = BusinessLogic.Administration.Masters.TaxSettingsMaster.GetActiveTaxCategoryDateValue(category, currentUser.SBUID, Convert.ToByte(DbActiveStatus.ACTIVE), Convert.ToDateTime(txtInvoiceDate.Text), 0, TaxFilterType.SAL, 0, 0, 1);
                            }
                            else
                            {
                                dtTaxDetails = BusinessLogic.Administration.Masters.TaxSettingsMaster.GetActiveTaxCategoryDateValue(category, currentUser.SBUID, Convert.ToByte(DbActiveStatus.ACTIVE), Convert.ToDateTime(txtInvoiceDate.Text), 0);
                            }
                        }
                        break;
                    #endregion
                    #region EXCHANGERATE
                    case ControlsEnum.EXCHANGERATE:
                        DataSet dsExchangeRate = BusinessLogic.PurchaseOrderManagement.RequestForQuote.GetExchangeRate(Convert.ToInt32(hdfCurrency.Value), currentUser.BaseCurrency, Convert.ToDateTime(txtInvoiceDate.Text.Trim()));
                        if (dsExchangeRate != null && dsExchangeRate.Tables[0].Rows.Count > 0)
                        {
                            hdfExchangeRate.Value = dsExchangeRate.Tables[0].Rows[0][0].ToString();
                            txtExchangeRate.Text = Convert.ToDouble(dsExchangeRate.Tables[0].Rows[0][0]).ToString();
                        }
                        else
                        {
                            hdfExchangeRate.Value = "1";
                            txtExchangeRate.Text = "";
                        }
                        break;
                    #endregion
                    #region INVOICEGET
                    case ControlsEnum.INVOICEGET:
                        int GInvPk = Convert.ToInt32(Request.QueryString[QueryStrings.PK]);
                        int InvoiceType = Convert.ToInt32(ddlSaleOrderType.SelectedValue);
                        int statusfilter = 3;
                        dsPageData = BusinessLogic.Sales.SaleOrderBL.GetDirectSalesInvoiceList(
                            new BusinessObject.GridPrams()
                            {
                                SortBy = string.IsNullOrEmpty(SortBy) ? Resources.DataFieldRes.SalesInvoiceDate : SortBy,
                                SortDirection = string.IsNullOrEmpty(SortDirection) ? Resources.Report.SortDescending : SortDirection,
                                ThenBy = SortBy == ThenBy || SortBy == Resources.DataFieldRes.SalesInvoiceNo ? string.Empty : string.IsNullOrEmpty(ThenBy) ? Resources.DataFieldRes.SalesInvoiceNo : ThenBy,
                                ThenDirection = SortBy == ThenBy || SortBy == Resources.DataFieldRes.SalesInvoiceNo ? string.Empty : string.IsNullOrEmpty(ThenDirection) ? Resources.Report.SortDescending : ThenDirection,
                                FromDate = string.IsNullOrEmpty(txtFromDate.Text.Trim()) ? string.Empty : txtFromDate.Text.Trim(),
                                ToDate = string.IsNullOrEmpty(txtToDate.Text.Trim()) ? string.Empty : txtToDate.Text.Trim(),
                                SearchBy = "ICH_PK",
                                SearchValue = GInvPk.ToString(),
                                PageNumber = 1,
                                PageSize = Convert.ToInt32(GetLocalResourceObject("PageSize_InvList"))
                            }, currentUser, 0, GInvPk, CurrSOPK, string.Empty, InvoiceType, txtSCno.Text
                             , Resources.PageURL.Invoicing.Replace("~", ""), txtDrCrNo.Text, statusfilter, 0
                             , string.IsNullOrEmpty(txtDueAson.Text.Trim()) ? string.Empty : txtDueAson.Text.Trim()
                             , group: (byte)SalesInvoiceGroup.Goods
                             );
                        if (dsPageData != null)
                        {
                            DataView dvInvoice = dsPageData.Tables[1].DefaultView;
                            dtInvoiceList = dvInvoice.ToTable();
                        }
                        break;
                    #endregion
                    #region INVOICELIST
                    case ControlsEnum.INVOICELIST:
                        TotalRecords = 0;
                        int cusID = String.IsNullOrEmpty(hdfCustomerSearch.Value.Trim()) ? 0 : Convert.ToInt32(hdfCustomerSearch.Value);
                        if (hdfCustomerSearch.Value != null && hdfCustomerSearch.Value != "0" && hdfCustomerSearch.Value != "")
                        {
                            Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = hdfCustomerSearch.Value;
                            Session[ERP.Utilities.SessionStrings.CUSTOMERNAME] = txtCustomerSearch.Text;
                        }
                        int InvPk = String.IsNullOrEmpty(hdfIVHPK.Value.Trim()) ? 0 : Convert.ToInt32(hdfIVHPK.Value);
                        int Status = Convert.ToInt32(ddlStatus.SelectedValue);
                        int InvType = Convert.ToInt32(ddlSaleOrderType.SelectedValue);
                        string customer = string.IsNullOrEmpty(txtCustomerSearch.Text.Trim()) ? string.Empty : (txtCustomerSearch.Text.Trim() == Resources.ErpRes.AutoDefaultValue ? string.Empty : txtCustomerSearch.Text.Trim());
                        dsPageData = BusinessLogic.Sales.SaleOrderBL.GetDirectSalesInvoiceList(
                            new BusinessObject.GridPrams()
                            {
                                SortBy = string.IsNullOrEmpty(SortBy) ? Resources.DataFieldRes.SalesInvoiceDate : SortBy,
                                SortDirection = string.IsNullOrEmpty(SortDirection) ? Resources.Report.SortDescending : SortDirection,
                                ThenBy = SortBy == ThenBy || SortBy == Resources.DataFieldRes.SalesInvoiceNo ? string.Empty : string.IsNullOrEmpty(ThenBy) ? Resources.DataFieldRes.SalesInvoiceNo : ThenBy,
                                ThenDirection = SortBy == ThenBy || SortBy == Resources.DataFieldRes.SalesInvoiceNo ? string.Empty : string.IsNullOrEmpty(ThenDirection) ? Resources.Report.SortDescending : ThenDirection,
                                FromDate = string.IsNullOrEmpty(txtFromDate.Text.Trim()) ? string.Empty : txtFromDate.Text.Trim(),
                                ToDate = string.IsNullOrEmpty(txtToDate.Text.Trim()) ? string.Empty : txtToDate.Text.Trim(),
                                SearchBy = "ICH_NO",
                                SearchValue = string.IsNullOrEmpty(txtInvoiceNumber.Text.Trim()) ? string.Empty : (txtInvoiceNumber.Text.Trim() == Resources.ErpRes.AutoDefaultValue ? string.Empty : txtInvoiceNumber.Text.Trim()),
                                PageNumber = string.IsNullOrEmpty(PageIndex) ? 1 : Convert.ToInt32(PageIndex),
                                PageSize = Convert.ToInt32(GetLocalResourceObject("PageSize_InvList"))
                            }, currentUser, cusID, InvPk, CurrSOPK, HttpUtility.HtmlEncode(customer), InvType, txtSCno.Text
                             , Resources.PageURL.SalesInvoiceTrading.Replace("~", ""), txtDrCrNo.Text, Status, Convert.ToInt32(chkBalAmt.Checked)
                             , string.IsNullOrEmpty(txtDueAson.Text.Trim()) ? string.Empty : txtDueAson.Text.Trim()
                             , (byte)SalesInvoiceGroup.Goods
                             , (byte)SalesInvoiceCategory.Invoice
                             );
                        if (dsPageData != null)
                        {
                            //string customer = string.IsNullOrEmpty(txtCustomerSearch.Text.Trim()) ? string.Empty : (txtCustomerSearch.Text.Trim() == Resources.ErpRes.AutoDefaultValue ? string.Empty : txtCustomerSearch.Text.Trim());
                            DataView dvInvoice = dsPageData.Tables[1].DefaultView;
                            dtInvoiceList = dvInvoice.ToTable();
                            int pagsize = Convert.ToInt32(GetLocalResourceObject("PageSize_InvList"));
                            TotalRecords = dtInvoiceList.Rows.Count > 0 ? Convert.ToInt32(dtInvoiceList.Rows[0]["ROW_COUNT"].ToString()) : 0;
                            TotalPages = TotalRecords == 0 ? 1 : (TotalRecords <= pagsize) ? 1 :
                                        (TotalRecords % pagsize) == 0 ? (TotalRecords / pagsize) :
                                        (TotalRecords / pagsize) + 1;
                        }
                        break;
                    #endregion
                    #region SOTYPE
                    case ControlsEnum.SOTYPE:
                        dtSOData = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, "SO TYPE");
                        break;
                    #endregion
                    #region INVOICETYPE
                    case ControlsEnum.INVOICETYPE:
                        dtInvoiceType = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, "SALES INVOICE TYPE", "Regular");
                        break;
                    #endregion
                    #region TAXSETTINGS
                    case ControlsEnum.TAXSETTINGS:
                        dtTaxSettings = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("SALE ITEM WISE SETTINGS", string.Empty, currentUser.SBUID);
                        break;
                    #endregion
                    #region ADVANCEDTAXSETTINGS
                    case ControlsEnum.ADVANCEDTAXSETTINGS:
                        dtTaxSettings = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("ADVANCE INVOICE SETTINGS", string.Empty, currentUser.SBUID);
                        if (dtTaxSettings != null && dtTaxSettings.Rows.Count > 0)
                        {
                            DataRow drTaxSettings = dtTaxSettings.AsEnumerable().SingleOrDefault(aa => aa.Field<string>("ACF_DATA").Trim() == "TAX");
                            if (drTaxSettings != null)
                            {
                                hdfTaxSettings.Value = drTaxSettings["ACF_VALUE"].ToString();
                            }
                        }
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

                    #region GETINVOICEPKBYJOURNALPK
                    case ControlsEnum.GETINVOICEPKBYJOURNALPK:
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
                    #region FILLWORKFLOWSTATUS
                    case ControlsEnum.FILLWORKFLOWSTATUS:
                        CommonServiceClient = new CommonService();
                        CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);
                        admConfigMstObj = new ADM_CONFIG_MST();
                        admConfigMstObj.CFG_PK = 0;
                        admConfigMstObj.CFG_TYPE = GetLocalResourceObject("APPLICATION_STATUS").ToString();
                        admConfigMstObj.CFG_ACTIVE = Convert.ToByte(CommonConstants.ACTIVE);
                        workflowStatusList = CommonServiceClient.GetConfigValues(admConfigMstObj);
                        break;
                    #endregion
                    #region NOTIFICATIONTYPES

                    case ControlsEnum.NOTIFICATIONTYPES:
                        CommonServiceClient = new CommonService();
                        CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);
                        admConfigMstObj = new ADM_CONFIG_MST();
                        admConfigMstObj.CFG_PK = 0;
                        admConfigMstObj.CFG_TYPE = Resources.Constants.ALERT_NOTIFICATION_TYPES;
                        admConfigMstObj.CFG_ACTIVE = Convert.ToByte(CommonConstants.ACTIVE);
                        admConfigMstList = CommonServiceClient.GetConfigValues(admConfigMstObj);
                        break;
                    #endregion
                    #region ALERTBASIS
                    case ControlsEnum.ALERTBASIS:
                        CommonServiceClient = new CommonService();
                        CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);
                        admConfigMstObj = new ADM_CONFIG_MST();
                        admConfigMstObj.CFG_PK = 0;
                        admConfigMstObj.CFG_TYPE = Resources.Constants.ALERT_BASIS;
                        admConfigMstObj.CFG_ACTIVE = Convert.ToByte(CommonConstants.ACTIVE);
                        admConfigMstList = CommonServiceClient.GetConfigValues(admConfigMstObj);
                        break;
                    #endregion
                    #region ALERTTYPES
                    case ControlsEnum.ALERTTYPES:
                        CommonServiceClient = new CommonService();
                        CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);
                        admConstMstList = CommonServiceClient.GetConstMstValues(null, null, null, 16, 1, currentUser.SBUID);
                        break;
                    #endregion
                    #region NOTIFICATIONDAYS
                    case ControlsEnum.NOTIFICATIONDAYS:
                        CommonServiceClient = new CommonService();
                        CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);
                        admAppConfigMstObj = new ADM_APP_CONFIG_MST();
                        admAppConfigMstObj.ACF_PK = 0;
                        admAppConfigMstObj.ACF_SETTING = Resources.Constants.ALERT_NOTIFY_BEFORE;
                        admAppConfigMstObj.ACF_DATA = ApplicationType.DSI;
                        admAppConfigMstObj.ACF_ACTIVE = Convert.ToByte(CommonConstants.ACTIVE);
                        admAppConstMstList = CommonServiceClient.GetAlertNotify(admAppConfigMstObj);
                        break;
                    #endregion
                    #region ALERTCONFIG
                    case ControlsEnum.ALERTCONFIG:
                        CommonServiceClient = new CommonService();
                        CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);
                        admAppConfigMstObj = new ADM_APP_CONFIG_MST();
                        admAppConfigMstObj.ACF_PK = 0;
                        admAppConfigMstObj.ACF_SETTING = Resources.Constants.AUTO_ALERT_FROM_TRX;
                        admAppConfigMstObj.ACF_DATA = "1";
                        admAppConfigMstObj.ACF_ACTIVE = Convert.ToByte(CommonConstants.ACTIVE);
                        admAppConstMstList = CommonServiceClient.GetAlertNotify(admAppConfigMstObj);
                        break;
                    #endregion
                    #region ALERTDETAILS
                    case ControlsEnum.ALERTLIST:
                        dsAlertList = BusinessLogic.AlertManagement.Alerts.GetAlertDetails(0, Convert.ToByte(DbActiveStatus.ACTIVE), null, appType, invPK, currentUser.SBUID, currentUser.PKUser, (int)AlertType.System);
                        break;
                    #endregion
                    #region Company
                    case ControlsEnum.COMPANY:
                        //gets Company List
                        admCompanyMstServiceClient = new AdmCompanyMstService();
                        admCompanyMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_COMPANY_MST>();
                        admCompanyMstObj.CMP_ACTIVE = 1;
                        serviceUtilityObj = new ServiceUtility();
                        admCompanyMstList = admCompanyMstServiceClient.GetCompanyList(admCompanyMstObj, serviceUtilityObj);
                        //  dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(DbActiveStatus.ACTIVE), currentUser.SBUID, 0);
                        break;
                    #endregion
                    #region PaymentTerms
                    case ControlsEnum.PAYMENTTERMS:
                        dtPaymentTerms = BusinessLogic.Sales.CustomerProduct.GetCustomerTerms(0, custPK, (int)CustomerTermType.PaymentTerms, paymentTermPK > 0 ? 2 : 1);
                        //dtPaymentTerms = BusinessLogic.Sales.CustomerProduct.GetCustomerTerms(paymentTermPK, custPK, (int)CustomerTermType.PaymentTerms, paymentTermPK > 0 ? 2 : 1);
                        break;
                    #endregion

                    #region INVOICEGSTTYPE
                    case ControlsEnum.INVOICEGSTTYPE:
                         int InvoiceTypeValue = ddlInvoiceType.SelectedValue == "" ? Convert.ToInt16(CommonConstants.SELECTVAL) : Convert.ToInt16(ddlInvoiceType.SelectedValue);
                        dtInvoiceGstType = BusinessLogic.CommonManagement.CommonBL.GetInvoiceGstType(Convert.ToInt16(CommonConstants.SELECT_VALUE_ZERO), Convert.ToInt16(DbActiveStatus.ACTIVE), currentUser.SBUID, InvoiceTypeValue, Convert.ToInt16(GTIService.Constants.Common.InvoiceType.Sales));
                        break;
                    #endregion
                    #region DueDate
                    case ControlsEnum.GETDUEDATE:
                        objDueDateDtls = (DirectDueDateDetails)SetUIValuesToObject(ControlsEnum.GETDUEDATE);
                        string xmlDoc = CommonFunctions.XmlSerialize<DirectDueDateDetails>(objDueDateDtls);
                        dsDueDate = BusinessLogic.Sales.SalesInvoiceBL.GetDueDateByPaymentTerms(xmlDoc);
                        break;
                    #endregion
                    #region CustomerTypes
                    case ControlsEnum.CUSTOMERTYPES:
                        dsCustomerTypes = BusinessLogic.Sales.CustomerProduct.GetCustomerAddress(0, custPK, 1, null);

                        break;
                    #endregion
                    #region GetCustomerDetailsByCustomerType
                    case ControlsEnum.GETCUSTOMERDETAILSBYTYPE:
                        int CAD_PK = 0;
                        int.TryParse(ddlCustomerType.SelectedValue, out CAD_PK);
                        int CustomerPK = 0;
                        int.TryParse(hdfCustomer.Value, out CustomerPK);
                        dsCustomerDetailsByType = BusinessLogic.Sales.CustomerProduct.GetCustomerAddress(CAD_PK, CustomerPK, 2, 0);
                        break;
                    #endregion
                    #region SHOWPOPUP
                    case ControlsEnum.SHOWPOPUP:
                        if (SOInvoiceHeaderSession != null)
                        {
                            salesOrderServiceClient = new SaleOrderService();
                            salesOrderServiceClient = CommonFunctions.InitiateClient(salesOrderServiceClient);
                            doPK = DespatchID.ToString();// salesOrderServiceClient.GetPkFromScNo(SOInvoiceHeaderSession.ICH_DESPATCH_NO.ToString());
                        }
                        break;
                    #endregion
                    #region AMOUNTDETAILS
                    case ControlsEnum.AMOUNTDETAILS:
                        dtAmountDetails = new DataTable();
                        //objDueDateDtls = (DueDateDetails)SetUIValuesToObject(ControlsEnum.GETDUEDATE);
                        dtAmountDetails = BusinessLogic.Sales.SalesInvoiceBL.GetInvCusReceivedAmntDetails(Convert.ToInt16(InvoicePk)).Tables[0];

                        break;
                    #endregion
                    #region CUSTOM TAX SETTINGS
                    case ControlsEnum.CUSTOMTAXSETTINGS:
                        IsCustomTaxEnabled = true;
                        dtCustomTaxSet = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("CUSTOM TAX SETTINGS", "TAX REQUIRED");
                        if (dtCustomTaxSet != null && dtCustomTaxSet.Rows.Count > 0)
                        {
                            int cfgval = Convert.ToInt32(dtCustomTaxSet.Rows[0]["ACF_VALUE"]);
                            if (cfgval == 0)
                            {
                                IsCustomTaxEnabled = false;
                            }
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
                admCompanyMstServiceClient = null;
                deliveryOrderServiceClient = null;
                finTrxServiceClient = null;
                CommonServiceClient = null;
                salesOrderServiceClient = null;
                salesInvoiceServiceClient = null;
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
                    case ControlsEnum.PEDINGSOLIST:
                        BindGrid(controlType);
                        break;

                    case ControlsEnum.INVOICEGET:
                        GetUIValuesFromObject(controlType);
                        break;
                    case ControlsEnum.SOINVHEADER:
                        GetUIValuesFromObject(controlType);
                        break;
                    case ControlsEnum.SOINVDETAIL:
                        BindGrid(controlType);
                        break;
                    case ControlsEnum.TAXTYPES:
                        BindDropDown(controlType);
                        break;
                    case ControlsEnum.TAXPOPUPGRID:
                        ddlPopupTaxType.Focus();
                        BindGrid(controlType);
                        break;
                    case ControlsEnum.DEDUCTIONPOPUPGRID:
                        BindGrid(controlType);
                        break;
                    case ControlsEnum.INVOICELIST:
                        BindGrid(controlType);
                        break;
                    case ControlsEnum.INVOICEGSTTYPE:
                        BindDropDown(controlType);
                        break;
                    case ControlsEnum.AMOUNTDETAILS:
                        BindGrid(controlType);
                        break;
                    case ControlsEnum.SOTYPE:
                        BindDropDown(controlType);
                        break;
                    case ControlsEnum.INVOICETYPE:
                        BindDropDown(controlType);
                        break;
                    #region LINETAX
                    case ControlsEnum.LINETAX:
                        if (dtTaxDetData != null & dtTaxDetData.Rows.Count > 0)
                        {


                            //Tax
                            decimal Totaltax = 0;
                            decimal TotalLineitemtax = 0;

                            decimal TotalDiscount = 0;
                            decimal TotalLineitemDiscount = 0;

                            for (int i = 0; i < dtTaxDetData.Rows.Count; i++)
                            {
                                if (Convert.ToByte(dtTaxDetData.Rows[i]["RDT_TAX_CATEGORY"]) == (byte)TaxType.Tax)
                                {
                                    if (!string.IsNullOrEmpty(dtTaxDetData.Rows[i]["RDT_SOD"].ToString()))
                                    {
                                        TotalLineitemtax = TotalLineitemtax + Convert.ToDecimal(dtTaxDetData.Rows[i]["RDT_AMOUNT"]);
                                    }
                                    Totaltax = Totaltax + Convert.ToDecimal(dtTaxDetData.Rows[i]["RDT_AMOUNT"]);
                                }
                            }
                            //Discount

                            for (int i = 0; i < dtTaxDetData.Rows.Count; i++)
                            {

                                if (Convert.ToByte(dtTaxDetData.Rows[i]["RDT_TAX_CATEGORY"]) == (byte)TaxType.Discount)
                                {

                                    if (!string.IsNullOrEmpty(dtTaxDetData.Rows[i]["RDT_SOD"].ToString()))
                                    {

                                        TotalLineitemDiscount = TotalLineitemDiscount + Convert.ToDecimal(dtTaxDetData.Rows[i]["RDT_AMOUNT"]);

                                    }
                                    else
                                    {
                                        TotalHDRDiscount = TotalHDRDiscount + Convert.ToDecimal(dtTaxDetData.Rows[i]["RDT_AMOUNT"]);

                                    }

                                    TotalDiscount = TotalDiscount + Convert.ToDecimal(dtTaxDetData.Rows[i]["RDT_AMOUNT"]);

                                }

                            }
                            decimal TaxApplcableinLine = 0;
                            decimal DiscountApplcableinLine = 0;
                            if (Totaltax > 0)
                            {
                                TaxApplcableinLine = (TotalLineitemtax / Totaltax) * totalAllocatedTax;//Allocate now : In deduction popup
                            }
                            if (TotalDiscount > 0)
                            {
                                DiscountApplcableinLine = (TotalLineitemDiscount / TotalDiscount) * totalAllocatedDiscount;//Allocate now : In deduction popup
                            }



                            invoiceHeaderObj = SOInvoiceHeaderSession;

                            //if (TempInvoiceHeaderTemp != null)
                            //{

                            //    soInvoiceDetailsList = GetPrevOrderDetails();

                            //}



                            if (TaxApplcableinLine > 0 || DiscountApplcableinLine > 0)
                            {

                                //GetFieldValues(ControlsEnum.SOINVHEADER);

                                if (invoiceHeaderObj != null)
                                {
                                    soInvoiceDetailsList = new List<DirectSOInvoiceDetails>();
                                    if (EntryStatus == EntryStatus.NEWMODE)
                                    {
                                        soInvoiceDetailsList = invoiceHeaderObj.OrderDetail.Where(sod => sod.CID_QTY_DISPATCHED > 0).ToList();//To hide items without despatched and shipping qty
                                    }
                                    else
                                    {
                                        soInvoiceDetailsList = invoiceHeaderObj.OrderDetail;//.Where(sod => sod.CID_QTY_DISPATCHED > 0).ToList();//To hide items without despatched and shipping qty
                                    }
                                }

                                decimal LineitemGridSum = 0;
                                decimal LineitemGridSumDiscount = 0;
                                soInvoiceDetailsList.ForEach(dt => { LineitemGridSum = LineitemGridSum + Convert.ToDecimal(dt.CID_TAX); });
                                soInvoiceDetailsList.ForEach(dt => { LineitemGridSumDiscount = LineitemGridSumDiscount + Convert.ToDecimal(dt.CID_DISCOUNT); });



                                if (soInvoiceDetailsList.Count > 0 || soInvoiceDetailsList != null)
                                {
                                    soInvoiceDetailsList.ForEach(dtl =>
                                        {
                                            double applicableAmtDiscount = 0;
                                            double applicableAmt = 0;
                                            if (LineitemGridSum > 0)
                                            {
                                                applicableAmt = ERP.Utilities.CommonFunctions.DoubleFormatRound(Convert.ToDouble(((dtl.CID_TAX / Convert.ToDouble(LineitemGridSum)) * Convert.ToDouble(TaxApplcableinLine))), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);

                                            }
                                            if (LineitemGridSumDiscount > 0)
                                            {
                                                applicableAmtDiscount = ERP.Utilities.CommonFunctions.DoubleFormatRound(Convert.ToDouble(((dtl.CID_DISCOUNT / Convert.ToDouble(LineitemGridSumDiscount)) * Convert.ToDouble(DiscountApplcableinLine))), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                            }

                                            dtl.TaxDtl.ForEach(dtl2 =>
                                               {
                                                   //double a = dtl2.CIT_TAX_AMT / dtl.CID_TAX;
                                                   //double b = a * applicableAmt;
                                                   //double c = dtl2.CIT_TAX_AMT - b;
                                                   if (dtl2.CIT_TAX_CATEGORY == (byte)TaxType.Tax && (dtl.CID_TAX > 0))
                                                   {
                                                       dtl2.CIT_TAX_AMT = ERP.Utilities.CommonFunctions.DoubleFormatRound(Convert.ToDouble(dtl2.CIT_TAX_AMT - ((dtl2.CIT_TAX_AMT / dtl.CID_TAX) * applicableAmt)), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                                   }
                                                   else if (dtl2.CIT_TAX_CATEGORY == (byte)TaxType.Discount && (dtl.CID_DISCOUNT > 0))
                                                   {
                                                       dtl2.CIT_TAX_AMT = ERP.Utilities.CommonFunctions.DoubleFormatRound(Convert.ToDouble(dtl2.CIT_TAX_AMT - ((dtl2.CIT_TAX_AMT / dtl.CID_DISCOUNT) * applicableAmtDiscount)), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                                   }
                                               });
                                            dtl.CID_TAX = ERP.Utilities.CommonFunctions.DoubleFormatRound(Convert.ToDouble(dtl.CID_TAX - applicableAmt), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                            dtl.CID_DISCOUNT = ERP.Utilities.CommonFunctions.DoubleFormatRound(Convert.ToDouble(dtl.CID_DISCOUNT - applicableAmtDiscount), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);

                                            dtl.CID_NET_AMOUNT = (dtl.CID_AMOUNT - dtl.CID_DISCOUNT) + dtl.CID_TAX;
                                        });

                                }

                                //for (int i = 0; i < dsTaxDetData.Tables[0].Rows.Count; i++)
                                //{
                                //    decimal TotalTaxPerAmt = Convert.ToDecimal(dsTaxDetData.Tables[0].Rows[i]["RDT_AMOUNT"]) / Totaltax;



                                //}
                                foreach (GridViewRow grdrow in grdInvoice.Rows)
                                {
                                    HiddenField hdfInvoiceDtlPK;
                                    hdfInvoiceDtlPK = (HiddenField)grdrow.FindControl("hdfInvoiceDtlPK");
                                    TextBox txtInvNow;
                                    txtInvNow = (TextBox)grdrow.FindControl("txtInvNow");

                                    // soInvoiceDetailsObj = soInvoiceDetailsList.SingleOrDefault(dtl => dtl.VID_PK == Convert.ToInt16(hdfInvoiceDtlPK.Value));
                                    //if (soInvoiceDetailsObj != null)
                                    //{
                                    //soInvoiceDetailsObj.VID_QTY_INVOICED =Convert.ToDouble(txtInvNow.Text);
                                    soInvoiceDetailsList.SingleOrDefault(dtl => dtl.CID_PK == Convert.ToInt16(hdfInvoiceDtlPK.Value)).CID_QTY_INVOICED = Convert.ToDouble(txtInvNow.Text);
                                    //  }
                                }
                                invoiceHeaderObj.OrderDetail = soInvoiceDetailsList;
                                if (soInvoiceDetailsList != null)
                                {
                                    grdInvoice.DataSource = soInvoiceDetailsList;
                                    grdInvoice.DataBind();

                                    btnApply.Visible = false;
                                    imgPopupAdd.Visible = false;
                                    grdTaxDetails.Columns[3].Visible = false;
                                }
                                SetSubTotal();
                            }
                        }
                        break;
                    #endregion
                    case ControlsEnum.COMPANY:
                        //Bind Company List in DDL
                        BindDropDown(ControlsEnum.COMPANY);
                        break;
                    case ControlsEnum.PAYMENTTERMS:
                        BindDropDown(ControlsEnum.PAYMENTTERMS);
                        break;
                    case ControlsEnum.GETDUEDATE:
                        if (dsDueDate != null & dsDueDate.Tables[0].Rows.Count > 0)
                        {
                            txtInvoiceDueDate.Text = Convert.ToDateTime(dsDueDate.Tables[0].Rows[0]["DUE_DATE"].ToString()).ToString(Resources.Constants.DateFormatShort);
                            txtInvoiceDueDate.Enabled = false;
                        }
                        else
                        {
                            txtInvoiceDueDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                            txtInvoiceDueDate.Enabled = true;
                        }
                        break;

                    case ControlsEnum.CUSTOMERTYPES:
                        BindDropDown(ControlsEnum.CUSTOMERTYPES);
                        break;

                    case ControlsEnum.DUEDATEPOPUPGRID:
                        BindGrid(ControlsEnum.DUEDATEPOPUPGRID);
                        break;

                    case ControlsEnum.GETCUSTOMERDETAILSBYTYPE:
                        if (ddlCustomerType.SelectedValue == CommonConstants.SELECTVAL)
                        {

                            txtTypeID.Text = "";
                            txtTaxID.Text = "";
                        }
                        else
                        {
                            if (dsCustomerDetailsByType != null & dsCustomerDetailsByType.Tables[0].Rows.Count > 0)
                            {
                                txtTypeID.Text = dsCustomerDetailsByType.Tables[0].Rows[0]["CAD_TYPE_NAME"].ToString();
                                txtTaxID.Text = txtTaxID.ToolTip = HttpUtility.HtmlDecode(dsCustomerDetailsByType.Tables[0].Rows[0]["CAD_CUSTOMER_GST"].ToString());

                                hdfCustomerTypeId.Value = Convert.ToString(dsCustomerDetailsByType.Tables[0].Rows[0]["CAD_TYPE"]);
                                lblSpecialCat.Visible = true;
                                lblSpecialCat.Text = HttpUtility.HtmlDecode(dsCustomerDetailsByType.Tables[0].Rows[0]["CUS_SPECIAL_CAT_TEXT"].ToString());
                                if (Convert.ToInt32(dsCustomerDetailsByType.Tables[0].Rows[0]["CAD_TYPE"]) == (int)CustomerContactTypeEnum.Branch)
                                {
                                    txtTypeID.Enabled = true;
                                    vrfBranchCode.Enabled = true;
                                    //txtTypeID.CssClass = "medium";
                                }
                                else
                                {
                                    //txtTypeID.Enabled = false;
                                    if (string.IsNullOrEmpty(txtTypeID.Text))
                                        txtTypeID.Text = GetLocalResourceObject("DefaultCodeForHo").ToString();// "00000";

                                    vrfBranchCode.Enabled = false;
                                    //txtTypeID.CssClass = "medium";//txtTypeID.CssClass = "medium input-disabled";
                                }

                            }
                            else
                            {
                                txtTypeID.Text = "";
                                txtTaxID.Text = "";
                            }
                        }
                        break;

                    #region OTHERCHARGELIST
                    case ControlsEnum.OTHERCHARGELIST:
                        BindGrid(ControlsEnum.OTHERCHARGELIST);
                        break;
                    #endregion
                    #region UPLOADEDFILES
                    case ControlsEnum.UPLOADEDFILES:
                        if (invoiceHeaderObj != null)
                            GetUIValuesFromObject(ControlsEnum.UPLOADEDFILES);
                        BindGrid(ControlsEnum.UPLOADEDFILES);
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
        #region Helper Methods
        private List<DirectSOInvoiceDetails> GetPrevOrderDetails()
        {
            List<DirectSOInvoiceDetails> lstOrderDetails = new List<DirectSOInvoiceDetails>();
            if (TempInvoiceHeaderTemp != null)
            {
                DirectSOInvoiceDetails objPOInvoiceDetails;
                foreach (DirectSOInvoiceDetails OrDetal in TempInvoiceHeaderTemp.OrderDetail)
                {
                    objPOInvoiceDetails = new DirectSOInvoiceDetails();
                    List<DirectSOInvoiceTaxHdr> lstPoInvoiceTax = new List<DirectSOInvoiceTaxHdr>();
                    DirectSOInvoiceTaxHdr obj;
                    foreach (DirectSOInvoiceTaxHdr objInvTax in OrDetal.TaxDtl)
                    {
                        obj = new DirectSOInvoiceTaxHdr();
                        obj.CIT_NAME = objInvTax.CIT_NAME;
                        obj.CIT_PK = objInvTax.CIT_PK;
                        obj.CIT_INVOICE_DTL = objInvTax.CIT_INVOICE_DTL;
                        obj.CIT_SO_DTL = objInvTax.CIT_SO_DTL;
                        obj.CIT_SL_NO = objInvTax.CIT_SL_NO;
                        obj.CIT_TAX = objInvTax.CIT_TAX;
                        obj.CIT_TAX_AMT = objInvTax.CIT_TAX_AMT;
                        obj.CIT_TAX_CATEGORY = objInvTax.CIT_TAX_CATEGORY;
                        obj.CIT_TAX_CATEGORY_TEXT = objInvTax.CIT_TAX_CATEGORY_TEXT;
                        obj.CIT_TAX_CODE = objInvTax.CIT_TAX_CODE;
                        obj.CIT_TAX_FORMULA = objInvTax.CIT_TAX_FORMULA;
                        obj.CIT_TAX_RATE = objInvTax.CIT_TAX_RATE;
                        obj.CIT_TAX_TEXT = objInvTax.CIT_TAX_TEXT;
                        obj.CIT_TAX_CID_AMOUNT = objInvTax.CIT_TAX_CID_AMOUNT;
                        obj.CIT_TYPE = objInvTax.CIT_TYPE;
                        lstPoInvoiceTax.Add(obj);

                    }
                    objPOInvoiceDetails.TaxDtl = lstPoInvoiceTax;
                    objPOInvoiceDetails.CID_AMOUNT = OrDetal.CID_AMOUNT;

                    //objPOInvoiceDetails.CID_BRANCH = OrDetal.CID_BRANCH;
                    //objPOInvoiceDetails.CID_BRANCH_NAME = OrDetal.CID_BRANCH_NAME;
                    //objPOInvoiceDetails.CID_BRANCH_TEXT = OrDetal.CID_BRANCH_TEXT;
                    //objPOInvoiceDetails.CID_BRANCH_TYPE = OrDetal.CID_BRANCH_TYPE;
                    objPOInvoiceDetails.CID_CIM_PCS_PER_IP = OrDetal.CID_CIM_PCS_PER_IP;
                    objPOInvoiceDetails.CID_CIM_PCS_PER_OP = OrDetal.CID_CIM_PCS_PER_OP;
                    objPOInvoiceDetails.CID_CUST_ITEM = OrDetal.CID_CUST_ITEM;
                    objPOInvoiceDetails.CID_CUST_ITEM_TEXT = OrDetal.CID_CUST_ITEM_TEXT;
                    objPOInvoiceDetails.CID_DISCOUNT = OrDetal.CID_DISCOUNT;
                    objPOInvoiceDetails.CID_INSTRUCTIONS = OrDetal.CID_INSTRUCTIONS;
                    objPOInvoiceDetails.CID_INV_QTY = OrDetal.CID_INV_QTY;

                    objPOInvoiceDetails.CID_INVOICE_HDR = OrDetal.CID_INVOICE_HDR;
                    objPOInvoiceDetails.CID_ITEM = OrDetal.CID_ITEM;
                    objPOInvoiceDetails.CID_ITEM_TEXT = OrDetal.CID_ITEM_TEXT;
                    objPOInvoiceDetails.CID_NET_AMOUNT = OrDetal.CID_NET_AMOUNT;
                    objPOInvoiceDetails.CID_ORDERED_QTY = OrDetal.CID_SOD_QTY;
                    objPOInvoiceDetails.CID_SOD_QTY = OrDetal.CID_SOD_QTY;
                    objPOInvoiceDetails.CID_PACKING_SPEC = OrDetal.CID_PACKING_SPEC;
                    objPOInvoiceDetails.CID_PACKING_SPEC_TEXT = OrDetal.CID_PACKING_SPEC_TEXT;

                    objPOInvoiceDetails.CID_PK = OrDetal.CID_PK;
                    objPOInvoiceDetails.CID_SO = OrDetal.CID_SO;
                    objPOInvoiceDetails.CID_SO_DTL = OrDetal.CID_SO_DTL;
                    objPOInvoiceDetails.CID_QTY_CARTONS = OrDetal.CID_QTY_CARTONS;
                    objPOInvoiceDetails.CID_QTY_INVOICED = OrDetal.CID_QTY_INVOICED;
                    objPOInvoiceDetails.CID_RATE = OrDetal.CID_RATE;
                    //objPOInvoiceDetails.CID_REF_NO = OrDetal.CID_REF_NO;
                    objPOInvoiceDetails.CID_REMARKS = OrDetal.CID_REMARKS;
                    objPOInvoiceDetails.CID_SL_NO = OrDetal.CID_SL_NO;
                    objPOInvoiceDetails.CID_TAX = OrDetal.CID_TAX;
                    //objPOInvoiceDetails.CID_TAX_ID = OrDetal.CID_TAX_ID;
                    objPOInvoiceDetails.CID_UOM = OrDetal.CID_UOM;
                    objPOInvoiceDetails.CID_UOM_TEXT = OrDetal.CID_UOM_TEXT;
                    //objPOInvoiceDetails.CID_VENDOR = OrDetal.CID_VENDOR;

                    //objPOInvoiceDetails.CID_VENDOR_TEXT = OrDetal.CID_VENDOR_TEXT;
                    objPOInvoiceDetails.CID_VERSION = OrDetal.CID_VERSION;

                    lstOrderDetails.Add(objPOInvoiceDetails);
                }
            }
            return lstOrderDetails;

        }

        private void FillTransactionData()
        {

            SetUIEditView(ActionsEnum.EDIT);
            WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
            base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
            SetCancelRef(CurrPK);
            ucrWrkf.FillWorkFlowDetails();
            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                ucrWrkf.ViewType = 1;
            else
            {
                ucrWrkf.ViewType = 0;
                //EntryStatus = EntryStatus.VIEWMODE;

            }
            ucrWrkf.ViewAction();

            ModifiedDatePnl.Visible = true;
            GetFieldValues(ControlsEnum.INVOICETYPE);
            SetFieldValues(ControlsEnum.INVOICETYPE);
            GetFieldValues(ControlsEnum.SOINVHEADER);
            if (SOInvoiceHeaderSession != null)
            {
                DespatchID = Convert.ToInt16(SOInvoiceHeaderSession.ICH_DESPATCH_HDR);
            }
            GetFieldValues(ControlsEnum.SOINVHEADER);
            SetFieldValues(ControlsEnum.SOINVHEADER);
            GetFieldValues(ControlsEnum.TAXSETTINGS);
            SetFieldValues(ControlsEnum.SOINVDETAIL);
            SetFieldValues(ControlsEnum.UPLOADEDFILES);
            TempSOInvoiceHeaderSession = SOInvoiceHeaderSession;
            SetDetailTax(null);
            SetHdrTax();
            if (grdInvoice.Rows.Count > 0)
            {
                SetSubTotal();
            }

            //Settings of TaxPayableDiv
            if (hdfIsTaxPayable.Value.ToString() == "1")
            {
                SetTaxPayableDiv();
            }
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "TaxPayable", "$(document).ready(function () { ShowHideTaxPayableOuter();});", true);
            //end

        }


        #region Set BranchID Enable/Disable
        private void SetBranchIDEnableDisable()
        {
            if (ddlCustomerType.SelectedValue != "")
            {
                GetFieldValues(ControlsEnum.GETCUSTOMERDETAILSBYTYPE);
                SetFieldValues(ControlsEnum.GETCUSTOMERDETAILSBYTYPE);
            }

        }
        #endregion


        /// <summary>
        /// Method to Fill Process ID
        /// </summary>
        /// 
        private int FillProcessId()
        {
            int procId = 0;
            string path;
            path = "/Sales/Invoicing.aspx?PID=1";
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            DataTable dtProcess = wrkfService.GetProcessID(path, currentUser.CurrentDeptPK);
            if (dtProcess != null && dtProcess.Rows.Count > 0)
            {
                procId = int.Parse(dtProcess.Rows[0]["PROCESS_PK"].ToString());
            }
            return procId;
        }
        /// <summary>
        /// For Bind Cancelation comment on workflow user control
        /// </summary>
        /// <param name="curPK"></param>
        private void SetCancelRef(int curPK)
        {
            #region Cancel ref Setting
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            DataTable dtCancelProcess = wrkfService.GetProcessID(GetLocalResourceObject("CancelPageURL").ToString(), currentUser.CurrentDeptPK);
            if (dtCancelProcess != null && dtCancelProcess.Rows.Count > 0)
            {
                WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                ucrWrkf.CancelRefID = workflowCore.GetRefID(curPK, int.Parse(dtCancelProcess.Rows[0][CommonConstants.F_PROCESS].ToString()));
            }
            #endregion
        }
        /// <summary>
        /// Funtion used get doc mode
        /// </summary>
        private string GetDOCMODE()
        {
            cm = new CommonService();
            AppTypeDetailsList = cm.GetReportParameters(ApplicationType.DSI, 0, DateTime.Now);
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
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>
        private Object SetUIValuesToObject(ControlsEnum controlType)
        {
            Object retObject;
            retObject = null;
            int rowID;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            HiddenField hdfRRDPK;
            HiddenField hdfItemPK;
            HiddenField hdfUoM;
            //HiddenField hdfCurrency;
            HiddenField hdfRFQDtlPK;
            TextBox txtRate;
            TextBox txtAmount;
            TextBox txtDiscount;
            TextBox txtTax;
            TextBox txtTotal;
            TextBox txtSubTotal;
            TextBox txtSubTotalInvNow;
            List<DirectSOInvoiceTaxHdr> rfqTaxHeaderList;

            List<DirectSOHeaderListBO> objSCDOPKList = new List<DirectSOHeaderListBO>();
            DirectSOHeaderBO objSOitem = new DirectSOHeaderBO();
            objSOitem.SOList = new List<DirectSOHeaderListBO>();

            Label lblBalAmt;
            LinkButton lbnBalAmt;
            decimal balamt;
            decimal InvoiceValue = 0;
            balamt = 0;


            bool bIsChecked = false;

            int selectedInvoice;
            int selectedCurrency;
            int selectedInvoiceType;
            bool isCancelled;
            int selectedCustomer;
            int approvedStatus;
            bool isPosted;

            selectedInvoice = 0;
            approvedStatus = 0;
            selectedCustomer = 0;
            selectedCurrency = 0;
            selectedInvoiceType = 0;
            isCancelled = false;
            isPosted = false;
            AlertBO alertBoObj;

            bool InvalidReceiptItem = false;

            try
            {
                switch (controlType)
                {
                    #region SOINVHEADER
                    case ControlsEnum.SOINVHEADER:
                        if (SOInvoiceHeaderSession != null)
                        {
                            invoiceHeaderObj = SOInvoiceHeaderSession;
                            invoiceHeaderObj.ICH_PK = CurrPK;
                            if (DespatchID > 0)//&& CurrPK == 0
                            {
                                invoiceHeaderObj.ICH_DESPATCH_HDR = DespatchID.ToString();
                                //invoiceHeaderObj.ICH_FROM_PORT = hdfPortofLoading.Value;
                                //invoiceHeaderObj.ICH_TO_PORT = txtFinalDest.Text;
                            }
                            //string.IsNullOrEmpty(hdfResponsePK.Value) ? 0 : Convert.ToInt32(hdfResponsePK.Value);
                            invoiceHeaderObj.ICH_NO = invoiceHeaderObj.ICH_PK.ToString();
                            invoiceHeaderObj.ICH_VERSION = 1;
                            invoiceHeaderObj.ICH_STATUS = 0;

                            invoiceHeaderObj.ICH_DATE = string.IsNullOrEmpty(txtInvoiceDate.Text.Trim()) ? DateTime.Now.ToString(Resources.Constants.DateFormatShort) : txtInvoiceDate.Text.Trim();
                            //invoiceHeaderObj.ICH_ = CurrPK;
                            invoiceHeaderObj.ICH_DATE_PAY_BY = string.IsNullOrEmpty(txtInvoiceDueDate.Text.Trim()) ? DateTime.Now.ToString(Resources.Constants.DateFormatShort) : txtInvoiceDueDate.Text.Trim();
                            invoiceHeaderObj.ICH_REMARKS = HttpUtility.HtmlEncode(txtRemarks.Text);
                            invoiceHeaderObj.ICH_REFERENCE = HttpUtility.HtmlEncode(txtReference.Text);
                            invoiceHeaderObj.ICH_DEL_TERM_TEXT = HttpUtility.HtmlEncode(txtDeliveryTerms.Text.Trim());
                            //invoiceHeaderObj.ICH_FEEDER_VESSEL = HttpUtility.HtmlEncode(txtFeederVessel.Text);
                            //invoiceHeaderObj.ICH_MOTHER_VESSEL = HttpUtility.HtmlEncode(txtMotherVessel.Text);
                            //invoiceHeaderObj.ICH_CONTAINER_NO = HttpUtility.HtmlEncode(txtContainerNo.Text);
                            invoiceHeaderObj.ICH_TYPE = ddlInvoiceType.SelectedValue;
                            if (!string.IsNullOrEmpty(txtETD.Text))
                                invoiceHeaderObj.ICH_ETD = txtETD.Text;
                            //if (!string.IsNullOrEmpty(txtETA.Text))
                            //    invoiceHeaderObj.ICH_ETA = txtETA.Text;
                            //invoiceHeaderObj.ICH_SHIPPING_MARK = HttpUtility.HtmlEncode(txtShippingMark.Text);
                            //invoiceHeaderObj.ICH_SPECIAL_NOTES = HttpUtility.HtmlEncode(txtSpecialNotes.Text);
                            invoiceHeaderObj.ICH_SHIP_CHARGE_DED = txtdor.Text;

                            //Adding New fields Branch/HeadOffice Pk, Type,Branch id and taxid
                            invoiceHeaderObj.ICH_BRANCH = ddlCustomerType.SelectedValue;//selected Branch/HO Name PK
                            //Fetching Type with respect to selected Pk;
                            invoiceHeaderObj.ICH_BRANCH_TYPE = hdfCustomerTypeId.Value.ToString();//HO/Branch(4/5)
                            invoiceHeaderObj.ICH_BRANCH_TEXT = HttpUtility.HtmlEncode(txtTypeID.Text);
                            invoiceHeaderObj.ICH_TAX_ID = HttpUtility.HtmlEncode(txtTaxID.Text);


                            invoiceHeaderObj.ICH_DISCOUNT_TC = string.IsNullOrEmpty(txtHdrDiscount.Text.Trim()) ? 0 : Convert.ToDouble(txtHdrDiscount.Text.Trim());
                            if (SaleOrderType == 1 && IsAdvInvHasTax) { invoiceHeaderObj.ICH_AMOUNT_ADV_DED_TC = string.IsNullOrEmpty(txtHdrDeduction.Text.Trim()) ? 0 : Convert.ToDouble(txtHdrDeduction.Text.Trim()); }
                            else
                            {
                                invoiceHeaderObj.ICH_AMOUNT_ADV_DED_TC = string.IsNullOrEmpty(txtTotalDeductionExp.Text.Trim()) ? 0 : Convert.ToDouble(txtTotalDeductionExp.Text.Trim());
                            }
                            invoiceHeaderObj.ICH_TAX_TC = string.IsNullOrEmpty(txtHdrTax.Text.Trim()) ? 0 : Convert.ToDouble(txtHdrTax.Text.Trim());
                            invoiceHeaderObj.ICH_SHIP_CHARGE = string.IsNullOrEmpty(txtShipping.Text.Trim()) ? 0 : Convert.ToDouble(txtShipping.Text.Trim());
                            invoiceHeaderObj.ICH_AMOUNT_ADJUST = string.IsNullOrEmpty(txtPriceAdj.Text.Trim()) ? 0 : Convert.ToDouble(txtPriceAdj.Text.Trim());
                            invoiceHeaderObj.ICH_AMOUNT_NET_TC = string.IsNullOrEmpty(txtHdrNetTotal.Text.Trim()) ? 0 : Convert.ToDouble(txtHdrNetTotal.Text.Trim());
                            invoiceHeaderObj.ICH_NET_VALUE_TC = string.IsNullOrEmpty(txtHdrInvoiceTotal.Text.Trim()) ? 0 : Convert.ToDouble(txtHdrInvoiceTotal.Text.Trim());
                            invoiceHeaderObj.ICH_BASE_CURR = currentUser.BaseCurrency <= 0 ? 1 : currentUser.BaseCurrency;
                            // GetFieldValues(ControlsEnum.EXCHANGERATE);
                            invoiceHeaderObj.ICH_EXCHG_RATE = string.IsNullOrEmpty(txtExchangeRate.Text) ? 1 : Convert.ToDouble(txtExchangeRate.Text);
                            //invoiceHeaderObj.ICH_EXCHG_RATE = string.IsNullOrEmpty(hdfExchangeRate.Value) ? 1 : Convert.ToDouble(hdfExchangeRate.Value);
                            invoiceHeaderObj.ICH_AMOUNT_NET_BC = invoiceHeaderObj.ICH_AMOUNT_NET_TC * invoiceHeaderObj.ICH_EXCHG_RATE;
                            invoiceHeaderObj.ICH_COMPANY = Convert.ToInt16(ddlCompany.SelectedValue);
                            invoiceHeaderObj.ICH_IS_OPENING = 0;

                            invoiceHeaderObj.ICH_ADV_ADJ_DED = string.IsNullOrEmpty(txtAdvAdjustDeductAmount.Text.Trim()) ? 0 : Convert.ToDouble(txtAdvAdjustDeductAmount.Text.Trim());

                            invoiceHeaderObj.ICH_DEPT = Convert.ToInt16(currentUser.CurrentDeptPK);
                            invoiceHeaderObj.ICH_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                            invoiceHeaderObj.ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                            invoiceHeaderObj.USER_PK = Convert.ToInt16(currentUser.PKUser);
                            invoiceHeaderObj.LAST_MOD_DT = LastModifiedTime;

                            if (Convert.ToInt32(ddlInvoiceGstType.SelectedValue) > 0)
                                invoiceHeaderObj.ICH_GST_TYPE = ddlInvoiceGstType.SelectedValue;
                            else
                                invoiceHeaderObj.ICH_GST_TYPE = null;

                            if (ddlPaymentTerms.SelectedValue != CommonConstants.SELECTVAL)
                            {
                                invoiceHeaderObj.ICH_PAYMENT_TERM = ddlPaymentTerms.SelectedValue;
                                invoiceHeaderObj.ICH_PAYMENT_TERM_TEXT = ddlPaymentTerms.SelectedItem.Text;
                            }
                            else
                            {
                                invoiceHeaderObj.ICH_PAYMENT_TERM = string.Empty;
                                invoiceHeaderObj.ICH_PAYMENT_TERM_TEXT = string.Empty;
                            }
                            #region Invoice Amount exceed SC Amount(SC amount=SC amount+ Customer Return amount) Checking required or not
                            if (ContineInvoiceAmtGreaterThanSCAmt == 1)
                            {
                                if (hdfSaveWithGreaterInvAmount.Value == "1")//Yes Continue
                                    invoiceHeaderObj.ICH_INVOICE_AMT_FLAG = 0;//No need to check Invoice amount is greater or not
                                else
                                    invoiceHeaderObj.ICH_INVOICE_AMT_FLAG = 1;
                            }
                            else //Should not allow to Save invoice amount greater than SC Amount
                            {
                                invoiceHeaderObj.ICH_INVOICE_AMT_FLAG = 1;
                            }
                            #endregion
                            SetUIValuesToObject(ControlsEnum.SOINVDETAIL);
                            //Adding Invoice DueDate Details
                            SetUIValuesToObject(ControlsEnum.DUEDATEDETAIL);

                            invoiceHeaderObj.FileList = SOInvoiceUploadList; //File Uploads

                            invoiceHeaderObj.ICH_TOTAL_QTY = 0;//Dummy
                            txtSubTotal = (TextBox)grdInvoice.FooterRow.FindControl("txtSubTotalFooter");
                            txtSubTotalInvNow = (TextBox)grdInvoice.FooterRow.FindControl("txtSubTotalFooterInvNow");
                            invoiceHeaderObj.ICH_AMOUNT_TC = txtSubTotal == null ? 0 : string.IsNullOrEmpty(txtSubTotal.Text.Trim()) ? 0 : Convert.ToDouble(txtSubTotal.Text.Trim());
                            invoiceHeaderObj.AST_CODE = ApplicationType.DSI;
                            invoiceHeaderObj.AST_DOC_MODE = AST_DOC_MODE.Value == "1" ? 1 : 0;
                            invoiceHeaderObj.ICH_GROUP = (byte)SalesInvoiceGroup.Goods;
                            invoiceHeaderObj.ICH_CATEGORY = (byte)SalesInvoiceCategory.Invoice;
                            if (invoiceHeaderObj.ICH_PK == 0)
                            {
                                invoiceHeaderObj.OrderDetail.ForEach(dtl =>
                                {
                                    dtl.CID_PK = 0;
                                    dtl.TaxDtl.ForEach(tax => tax.CIT_PK = 0);
                                });
                                invoiceHeaderObj.TaxHdr.ForEach(tax => tax.CIT_PK = 0);
                            }
                            invoiceHeaderObj.ICH_TERM1 = HttpUtility.HtmlEncode(txtTerms.Text);
                            invoiceHeaderObj.ICH_TERM2 = HttpUtility.HtmlEncode(txtTotalTerms.Text);
                        }
                        //remove if deduction item,which have no amount,else it will block receipt voucher cancellation
                        invoiceHeaderObj.DeductionDetails.Where(dtl => dtl.IAD_AMOUNT <= 0)
                                                                           .ToList().ForEach(dtl => invoiceHeaderObj.DeductionDetails.Remove(dtl));


                        //if (EntryStatus == EntryStatus.NEWMODE)
                        //{
                        invoiceHeaderObj.OrderDetail = invoiceHeaderObj.OrderDetail.Where(dd => dd.CID_QTY_DO_DISPATCHED > 0 || DOCancelStatus == 1).ToList();
                        //}

                        #region Propotionate header tax and discount for SC's
                        double InvSubTotal = 0;
                        InvSubTotal = invoiceHeaderObj.OrderDetail.Sum(r => r.CID_AMOUNT);

                        List<DirectSOInvoiceDetails> tempSOMpg = new List<DirectSOInvoiceDetails>();
                        tempSOMpg = invoiceHeaderObj.OrderDetail.Where(dd => dd.CID_QTY_DO_DISPATCHED > 0 || DOCancelStatus == 1)
                            .GroupBy(gp => Convert.ToInt32(gp.CID_SO)).Select(tmp => new DirectSOInvoiceDetails
                            {
                                CID_AMOUNT = tmp.Sum(p => p.CID_AMOUNT),
                                CID_SO = tmp.Select(p => p.CID_SO).FirstOrDefault()
                            }).ToList();


                        List<DirectSOInvoiceMappingDetails> SOInvoiceMpgList = new List<DirectSOInvoiceMappingDetails>();
                        tempSOMpg.ForEach(odr =>
                            {

                                if (InvSubTotal > 0)
                                {
                                    DirectSOInvoiceMappingDetails MpgDetails = new DirectSOInvoiceMappingDetails();
                                    MpgDetails.ICM_ACTIVE = 1;
                                    MpgDetails.ICM_AMOUNT = odr.CID_AMOUNT;
                                    double TotalAdjAmount = 0;
                                    double TotalDiscountAmount = 0;
                                    double TotalTaxAmount = 0;
                                    DirectSOInvoiceDetails ObjOrderDet = new DirectSOInvoiceDetails();
                                    ObjOrderDet = invoiceHeaderObj.OrderDetail.Where(s => Convert.ToInt32(s.CID_SO) == Convert.ToInt32(odr.CID_SO)).FirstOrDefault();
                                    TotalAdjAmount = invoiceHeaderObj.OrderDetail.GroupBy(gp => Convert.ToInt32(gp.CID_SO)).Select(r => r.FirstOrDefault().SOH_TOTAL_ADJUST).Sum();
                                    if (TotalAdjAmount == 0)
                                        TotalAdjAmount = 1;
                                    TotalDiscountAmount = invoiceHeaderObj.OrderDetail.GroupBy(gp => Convert.ToInt32(gp.CID_SO)).Select(r => r.FirstOrDefault().SOH_TOTAL_DISCOUNT).Sum();
                                    if (TotalDiscountAmount == 0)
                                        TotalDiscountAmount = 1;
                                    TotalTaxAmount = invoiceHeaderObj.OrderDetail.GroupBy(gp => Convert.ToInt32(gp.CID_SO)).Select(r => r.FirstOrDefault().SOH_TOTAL_TAX).Sum();
                                    if (TotalTaxAmount == 0)
                                        TotalTaxAmount = 1;

                                    //MpgDetails.ICM_ADJUST_AMOUNT = (ObjOrderDet.SOH_TOTAL_ADJUST / TotalAdjAmount) * Convert.ToDouble(txtPriceAdj.Text);
                                    MpgDetails.ICM_ADJUST_AMOUNT = (ObjOrderDet.SOH_TOTAL_ADJUST / TotalAdjAmount) * (string.IsNullOrEmpty(txtPriceAdj.Text.Trim()) ? 0 : Convert.ToDouble(txtPriceAdj.Text.Trim()));// 4604
                                    MpgDetails.ICM_DISCOUNT_AMOUNT = (ObjOrderDet.SOH_TOTAL_DISCOUNT / TotalDiscountAmount) * Convert.ToDouble(txtHdrDiscount.Text);
                                    MpgDetails.ICM_TAX_AMOUNT = (ObjOrderDet.SOH_TOTAL_TAX / TotalTaxAmount) * Convert.ToDouble(txtHdrTax.Text);

                                    MpgDetails.ICM_OTHER_AMOUNT = (invoiceHeaderObj.TaxHdr != null) ? invoiceHeaderObj.TaxHdr.Where(tax => tax.CIT_SO == Convert.ToInt32(odr.CID_SO) && tax.CIT_TAX_CATEGORY == (int)TaxType.Shipping).Sum(amt => amt.CIT_TAX_AMT) : 0; //(invoiceHeaderObj.ICH_SHIP_CHARGE / InvSubTotal) * odr.CID_AMOUNT;
                                    MpgDetails.ICM_SO_HDR = Convert.ToInt32(odr.CID_SO);
                                    SOInvoiceMpgList.Add(MpgDetails);

                                    DirectSOHeaderListBO objTempSCDOPks = new DirectSOHeaderListBO();
                                    objTempSCDOPks.DPH_PK = Convert.ToInt32(invoiceHeaderObj.ICH_DESPATCH_HDR);
                                    objTempSCDOPks.SOH_PK = Convert.ToInt32(odr.CID_SO);
                                    objSCDOPKList.Add(objTempSCDOPks);
                                }

                            });
                        invoiceHeaderObj.SOMappingDetails.Clear();
                        invoiceHeaderObj.SOMappingDetails = SOInvoiceMpgList;
                        #endregion

                        retObject = invoiceHeaderObj;
                        break;
                    #endregion
                    #region SOINVDETAIL
                    case ControlsEnum.SOINVDETAIL:
                        rowID = 0;
                        invoiceHdrMulDummyLIST = new List<DirectSOInvoiceDetails>();
                        //List<SOInvoiceDetails> soInvoiceTLST;
                        foreach (GridViewRow grdrow in grdInvoice.Rows)
                        {

                            soInvoiceDetailsObj = new DirectSOInvoiceDetails();
                            hdfRRDPK = (HiddenField)grdInvoice.Rows[rowID].FindControl("hdfInvoiceDtlDummyPK");
                            int invDtlPK;

                            // double UomConv = 1;
                            if (hdfRRDPK != null && int.TryParse(hdfRRDPK.Value, out invDtlPK))
                            {
                                soInvoiceDetailsObj = SOInvoiceHeaderSession.OrderDetail.SingleOrDefault(dtl => dtl.CID_SL_UK == invDtlPK);

                                if (soInvoiceDetailsObj != null)
                                {
                                    soInvoiceDetailsObj.CID_SL_NO = rowID + 1;
                                    TextBox txtInvNow = (TextBox)grdInvoice.Rows[rowID].FindControl("txtInvNow");
                                    soInvoiceDetailsObj.CID_INV_QTY_NOW = txtInvNow == null ? 0 : string.IsNullOrEmpty(txtInvNow.Text) ? 0 : Convert.ToDouble(txtInvNow.Text);
                                    txtAmount = (TextBox)grdInvoice.Rows[rowID].FindControl("txtAmount");
                                    soInvoiceDetailsObj.CID_AMOUNT = txtAmount == null ? 0 : string.IsNullOrEmpty(txtAmount.Text.Trim()) ? 0 : Convert.ToDouble(txtAmount.Text.Trim());
                                    txtDiscount = (TextBox)grdInvoice.Rows[rowID].FindControl("txtDiscount");
                                    soInvoiceDetailsObj.CID_DISCOUNT = txtDiscount == null ? 0 : string.IsNullOrEmpty(txtDiscount.Text.Trim()) ? 0 : Convert.ToDouble(txtDiscount.Text.Trim());
                                    txtTax = (TextBox)grdInvoice.Rows[rowID].FindControl("txtTax");
                                    soInvoiceDetailsObj.CID_TAX = txtTax == null ? 0 : string.IsNullOrEmpty(txtTax.Text.Trim()) ? 0 : Convert.ToDouble(txtTax.Text.Trim());
                                    txtTotal = (TextBox)grdInvoice.Rows[rowID].FindControl("txtTotal");
                                    soInvoiceDetailsObj.CID_NET_AMOUNT = txtTotal == null ? 0 : string.IsNullOrEmpty(txtTotal.Text.Trim()) ? 0 : Convert.ToDouble(txtTotal.Text.Trim());
                                    txtRate = (TextBox)grdInvoice.Rows[rowID].FindControl("txtRate");
                                    soInvoiceDetailsObj.CID_RATE = txtRate == null ? 0 : string.IsNullOrEmpty(txtRate.Text.Trim()) ? 0 : Convert.ToDouble(txtRate.Text.Trim());
                                    if (soInvoiceDetailsObj.TaxDtl != null)
                                    {
                                        rfqTaxHeaderList = soInvoiceDetailsObj.TaxDtl.ToList();
                                        if (rfqTaxHeaderList != null && rfqTaxHeaderList.Count > 0)
                                        {
                                            rfqTaxHeaderList.ForEach(dtl => dtl.CIT_SL_NO = soInvoiceDetailsObj.CID_SL_NO);
                                        }
                                    }
                                    hasValidRate = hasValidRate || soInvoiceDetailsObj.CID_INV_QTY_NOW >= 0;

                                }
                            }

                            invoiceHdrMulDummyLIST.Add(soInvoiceDetailsObj);
                            rowID++;
                        }

                        //if (EntryStatus == EntryStatus.NEWMODE)
                        //{
                        //    if (invoiceHeaderMulObj != null)
                        //    {
                        //        invoiceHdrMulDummyLIST = new List<SOInvoiceDetails>();
                        //        invoiceHeaderMulObj.SOMainList.ForEach(f =>
                        //        {

                        //            invoiceHdrMulDummyLIST.AddRange(f.OrderDetail.ToList());
                        //        });
                        //    }
                        //}
                        SOInvoiceHeaderSession.OrderDetail.Clear();
                        SOInvoiceHeaderSession.OrderDetail = invoiceHdrMulDummyLIST;
                        break;
                    #endregion

                    #region Journalize
                    case ControlsEnum.JOURNALIZE:
                        ////Start
                        if (EntryStatus == EntryStatus.LISTMODE)
                        {
                            ////
                            foreach (GridViewRow grdrow in grdInvoiceList.Rows)
                            {
                                CheckBox chkInvselect;
                                HiddenField hdfDept;
                                int dept;
                                chkInvselect = (CheckBox)grdrow.FindControl("chkInvselect");
                                if (chkInvselect.Checked)
                                {
                                    bIsChecked = true;
                                    CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfInvoiceID")).Value);
                                    hdfCurrentPk.Value = CurrPK.ToString();
                                    Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                                    Posted = Convert.ToBoolean(((HiddenField)grdrow.FindControl("hdfPosted")).Value);
                                    Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = ((HiddenField)grdrow.FindControl("hdfCustomerPK")).Value;
                                    Session[ERP.Utilities.SessionStrings.CUSTOMERNAME] = ((Label)grdrow.FindControl("lblCustomer")).Text;

                                    hdfDept = grdrow.FindControl("hdfDept") as HiddenField;
                                    if (hdfDept != null && int.TryParse(hdfDept.Value, out dept))
                                    {
                                        Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                                        base.SetUserDept();
                                    }
                                    break;
                                }
                            }
                            ////Start
                        }
                        else if (EntryStatus == EntryStatus.ENTRYMODE || EntryStatus == EntryStatus.VIEWMODE)
                        {
                            bIsChecked = true;
                        }
                        ////
                        if (bIsChecked)
                        {
                            if (Approved == 2)
                            {
                                //GetFieldValues(ControlsEnum.SOINVHEADER);
                                //if (SOInvoiceHeaderSession != null)
                                //{
                                //    DespatchID = Convert.ToInt16(SOInvoiceHeaderSession.ICH_DESPATCH_HDR);
                                //}
                                GetFieldValues(ControlsEnum.SOINVHEADER);

                                //Adding Invoice DueDate Details
                                SetUIValuesToObject(ControlsEnum.DUEDATEDETAIL);

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

                                Invoice = ApplicationType.DSIJ;
                                ucrJournalize.TransactionType = Invoice;
                                Session[ERP.Utilities.SessionStrings.TransactionType] = Invoice;
                                ucrJournalize.TransactionPK = CurrPK;
                                Session[ERP.Utilities.SessionStrings.TransactionPK] = CurrPK;
                                ucrJournalize.JournalizePK = 0;
                                Session[ERP.Utilities.SessionStrings.JournalizePK] = null;
                                Session[ERP.Utilities.SessionStrings.TransactionNo] = invoiceHeaderObj == null ? TempSOInvoiceHeaderSessionCustAll.ICH_NO : invoiceHeaderObj.ICH_NO;
                                Session[ERP.Utilities.SessionStrings.TransactionDate] = invoiceHeaderObj == null ? TempSOInvoiceHeaderSessionCustAll.ICH_DATE : invoiceHeaderObj.ICH_DATE;
                                Session[ERP.Utilities.SessionStrings.TransactionCurrency] = invoiceHeaderObj == null ? TempSOInvoiceHeaderSessionCustAll.ICH_CURRENCY : invoiceHeaderObj.ICH_CURRENCY;
                                Session[ERP.Utilities.SessionStrings.AccountPayable] = ApplicationType.AP;
                                Session[ERP.Utilities.SessionStrings.AccountPayablePK] = invoiceHeaderObj == null ? TempSOInvoiceHeaderSessionCustAll.ICH_CUSTOMER : invoiceHeaderObj.ICH_CUSTOMER;
                                Session[ERP.Utilities.SessionStrings.JournalType] = ApplicationType.DSIJ;
                                ucrWrkf.WrkfSubmit -= ActionHandler;
                                ucrWrkf.Reset();
                                ucrWrkf.ViewType = 1;
                                FillProcessID(2);
                                GetFieldValues(ControlsEnum.FINHEADER);
                                //EntryStatus = EntryStatus.ENTRYMODE;
                                WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                                if (finTrxHdrList != null && finTrxHdrList.Count > 0)
                                {
                                    ucrWrkf.RefID = workflowCore.GetRefID((int)finTrxHdrList[0].FTH_PK, ucrWrkf.ProcessID);
                                    base.WkfRefID = ucrWrkf.RefID;
                                }
                                SetCancelRef(CurrPK);
                                ucrWrkf.FillWorkFlowDetails();
                                if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                                {
                                    ucrJournalize.JournalizeRefPK = ucrWrkf.RefID;
                                    ucrWrkf.ViewType = 1;
                                    // Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.ENTRYMODE;
                                }
                                else
                                {
                                    ucrWrkf.ViewType = 0;
                                    //EntryStatus = EntryStatus.VIEWMODE;
                                    //Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.VIEWMODE;
                                }
                                ucrJournalize.HasWkfPermission = ucrWrkf.HasPageTaskPermission;
                                Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus == EntryStatus.ENTRYMODE ? EntryStatus.ENTRYMODE : EntryStatus.VIEWMODE;
                                ucrWrkf.ViewAction();

                                TextBox WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                                WrkfComments.Text = "";
                                Session[ERP.Utilities.SessionStrings.RemoveRowIndex] = null;
                                hdfJournalizeWorkFlow.Value = "1";
                                ucrJournalize.CallUserControl();

                                Session[ERP.Utilities.SessionStrings.JournalHead] = GetLocalResourceObject("Sales_Invoice_Journal").ToString();

                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_PickPayment_Msg").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            }
                        }
                        else
                        {
                            if (EntryStatus == EntryStatus.NEWMODE)
                                litErrorMsg.Text = GetLocalResourceObject("Msg_PickPayment_Msg").ToString();
                            else
                                litErrorMsg.Text = Resources.Report.Msg_Not_Selected;

                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region ALERT
                    case ControlsEnum.ALERTSAVE:
                        string Typename = Resources.Constants.SystemAlertType;
                        int AlertPk = 0;
                        alertBoObj = new AlertBO();
                        appType = ApplicationType.DSI;
                        if (invPK > 0)
                        {
                            GetFieldValues(ControlsEnum.ALERTLIST);
                            if (dsAlertList != null && dsAlertList.Tables[0].Rows.Count == 1)
                            {
                                AlertPk = Convert.ToInt32(dsAlertList.Tables[0].Rows[0]["ATH_PK"].ToString());
                            }
                        }
                        alertBoObj.ATH_PK = AlertPk;
                        alertBoObj.ATH_NO = "";
                        alertBoObj.ATH_DATE = DateTime.Now;
                        alertBoObj.ATH_TRX_TYPE = appType;
                        alertBoObj.ATH_TRX_PK = invPK;
                        alertBoObj.ATH_TRX_DATE = txtInvoiceDate.Text.Trim() == string.Empty ? DateTime.Now : Convert.ToDateTime(txtInvoiceDate.Text.Trim());
                        string duedays = (Math.Floor((DateTime.Now - alertBoObj.ATH_TRX_DATE).TotalDays)).ToString();
                        alertBoObj.ATH_DUE_DAYS = Convert.ToInt16(duedays);
                        alertBoObj.ATH_DUE_DATE = Convert.ToDateTime((DateTime.Now).ToShortDateString());
                        alertBoObj.ATH_NAME = Typename;
                        alertBoObj.ATH_ALERT_TYPE = null;
                        GetFieldValues(ControlsEnum.ALERTBASIS);
                        if (admConfigMstList != null && admConfigMstList.Count > 0)
                        {
                            alertBoObj.ATH_BASIS = admConfigMstList[0].CFG_PK;
                            alertBoObj.ATH_NOTIFY_BFR_UOM = admConfigMstList[0].CFG_PK;
                            GetFieldValues(ControlsEnum.NOTIFICATIONDAYS);
                            if (admAppConstMstList != null && admAppConstMstList.Count > 0)
                            {
                                alertBoObj.ATH_NOTIFY_BFR = admAppConstMstList[0].ACF_VALUE;
                            }
                            else
                            {
                                alertBoObj.ATH_NOTIFY_BFR = 0;
                            }
                        }
                        else
                        {
                            alertBoObj.ATH_BASIS = null;
                            alertBoObj.ATH_NOTIFY_BFR_UOM = null;
                            alertBoObj.ATH_NOTIFY_BFR = 0;
                        }
                        alertBoObj.ATH_REMARKS = string.Empty;
                        //if (!string.IsNullOrEmpty(TypeRef) && !TypeRef.Equals("[New]".ToLower()))
                        //{
                        alertBoObj.ATH_NARRATION = lblInvoiceNo.Text.Trim() + " - " + txtCustomer.Text;
                        //if (!string.IsNullOrEmpty(TypePartyName))
                        //{
                        //    alertBoObj.ATH_NARRATION = !string.IsNullOrEmpty(alertBoObj.ATH_NARRATION) ? alertBoObj.ATH_NARRATION + " - " + TypePartyName :
                        //        TypePartyName;
                        //}
                        //}
                        alertBoObj.ATH_NOTIFY_MESSAGE = true;
                        alertBoObj.ATH_NOTIFY_USER = Convert.ToInt16(currentUser.PKUser);
                        alertBoObj.ATH_STATUS = (byte)0;
                        alertBoObj.ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        alertBoObj.USER_PK = Convert.ToInt16(currentUser.PKUser);
                        alertBoObj.BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                        alertBoObj.LAST_MOD_DT = LastModifiedTime;
                        retObject = alertBoObj;
                        break;
                    #endregion
                    #region Due Date
                    case ControlsEnum.GETDUEDATE:
                        objDueDateDtls = new DirectDueDateDetails();
                        //objDueDateDtls.TCH_PK = ddlPaymentTerms.SelectedValue != null ? Convert.ToInt32(ddlPaymentTerms.SelectedValue) : 0;
                        objDueDateDtls.TCH_PK = ddlPaymentTerms.SelectedIndex > 0 ? (ddlPaymentTerms.SelectedValue != null ? Convert.ToInt32(ddlPaymentTerms.SelectedItem.Value) : 0) : 0;
                        objDueDateDtls.TRX_TYPE = ApplicationType.DSI;
                        objDueDateDtls.TRX_PK = CurrPK;
                        //objDueDateDtls.AMOUNT = string.IsNullOrEmpty(txtHdrInvoiceTotal.Text.Trim()) ? 0 : Convert.ToDecimal(txtHdrInvoiceTotal.Text.Trim());
                        decimal InvoiceAmount = string.IsNullOrEmpty(txtHdrInvoiceTotal.Text.Trim()) ? 0 : Convert.ToDecimal(txtHdrInvoiceTotal.Text.Trim());
                        if (!string.IsNullOrEmpty(ddlInvoiceType.SelectedValue) && (Convert.ToInt32(ddlInvoiceType.SelectedValue) == Convert.ToInt32(SalesInvoiceType.Domestic)))
                        {
                            if (SOInvoiceHeaderSession != null)
                            {
                                SOInvoiceHeaderSession.DeductionDetails.ForEach(ded =>
                                {
                                    InvoiceAmount += ded.IAD_AMOUNT;
                                });
                            }
                        }
                        objDueDateDtls.AMOUNT = InvoiceAmount;
                        //if (txtETA.Text != string.Empty)
                        //{
                        //    objDueDateDtls.ETA = txtETA.Text;
                        //}
                        if (txtETD.Text != string.Empty)
                        {
                            objDueDateDtls.ETD = txtETD.Text;
                        }
                        if (txtInvoiceDate.Text != string.Empty)
                        {
                            objDueDateDtls.ICH_DATE = txtInvoiceDate.Text;
                        }
                        //if (lblSODateTxt.Text != string.Empty) // This label contains DO date not SC date
                        //{
                        //    objDueDateDtls.SOH_DATE = lblSODateTxt.Text;
                        //}
                        if (!string.IsNullOrEmpty(hdfSCDate.Value))
                        {
                            objDueDateDtls.SOH_DATE = hdfSCDate.Value;
                        }
                        retObject = objDueDateDtls;
                        break;
                    #endregion
                    #region Adding Invoice DueDate Details
                    case ControlsEnum.DUEDATEDETAIL:

                        GetFieldValues(ControlsEnum.GETDUEDATE);
                        SetFieldValues(ControlsEnum.DUEDATEPOPUPGRID);
                        int rowNo = 0;
                        lstDueDetail = new List<DirectDueDetail>();
                        foreach (GridViewRow grdrow in grdDueDateDetails.Rows)
                        {
                            objDueDetail = new DirectDueDetail();
                            HiddenField hdfTCDPK = (HiddenField)grdDueDateDetails.Rows[rowNo].FindControl("hdfTCDPK");
                            Label lblDate = (Label)grdDueDateDetails.Rows[rowNo].FindControl("lblDate");
                            Label lblDueAmount = (Label)grdDueDateDetails.Rows[rowNo].FindControl("lblDueAmount");
                            HiddenField hdfIDD_PK = (HiddenField)grdDueDateDetails.Rows[rowNo].FindControl("hdfIDD_PK");
                            objDueDetail.IDD_PK = "0";// hdfIDD_PK.Value;
                            // objDueDetail.IDD_INVOICE_HDR=
                            objDueDetail.IDD_TERM_DTL = hdfTCDPK.Value;//PK of CRM_CUST_TERM_DTL
                            objDueDetail.IDD_DUE_DATE = lblDate.Text;
                            objDueDetail.IDD_DUE_AMOUNT = Convert.ToDecimal(lblDueAmount.Text);
                            lstDueDetail.Add(objDueDetail);
                            rowNo++;
                        }
                        if (invoiceHeaderObj != null)
                            invoiceHeaderObj.DueDetail = lstDueDetail;

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
            int dept = 0;
            try
            {
                switch (controlType)
                {
                    #region INVOICEGET
                    case ControlsEnum.INVOICEGET:
                        int invType = 1;
                        int InvoiceType = 1;
                        IsDoModified = false;
                        btnRefreshInv.Visible = false;
                        if (dtInvoiceList != null && dtInvoiceList.Rows.Count > 0)
                        {
                            CurrPK = Convert.ToInt32(Request.QueryString["PK"].ToString());
                            hdfCurrentPk.Value = CurrPK.ToString();
                            if (dtInvoiceList.Rows[0]["ICH_IS_OPENING"].ToString() == "1")
                            {
                                Session[ERP.Utilities.SessionStrings.InvoicePK] = CurrPK.ToString();
                                Response.Redirect(Resources.PageURL.OpeningInvoice);
                                break;
                            }
                            Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = dtInvoiceList.Rows[0]["ICH_CUS_PK"].ToString();
                            Session[ERP.Utilities.SessionStrings.CUSTOMERNAME] = dtInvoiceList.Rows[0]["ICH_CUSTOMER_TEXT"].ToString();

                            Approved = Convert.ToInt32(dtInvoiceList.Rows[0]["ICH_STATUS"].ToString());
                            Posted = Convert.ToBoolean(dtInvoiceList.Rows[0]["ICH_HAS_JRNL_ENTRY"].ToString());
                            invType = Convert.ToInt32(dtInvoiceList.Rows[0]["SOH_TYPE"].ToString());
                            InvoiceType = Convert.ToInt32(dtInvoiceList.Rows[0]["ICH_TYPE"].ToString());

                            //if (IsDeleted)
                            //{
                            //    if (InvoiceType != (int)SalesInvoiceType.Proforma)
                            //    {
                            //        if (InvoiceType == Convert.ToInt32(SalesInvoiceType.Domestic))
                            //            FillProcessID(3);
                            //        else
                            //            FillProcessID(1);
                            //    }
                            //    else
                            //    {
                            //        if (invType == Convert.ToInt32(SalesInvoiceType.Domestic))
                            //            FillProcessID(3);
                            //        else
                            //            FillProcessID(1);
                            //    }
                            //}
                            //else
                            //{
                            //    if (invType == Convert.ToInt32(SalesInvoiceType.Domestic))
                            //        FillProcessID(3);
                            //    else
                            //        FillProcessID(1);
                            //}
                            FillProcessID(1);
                            if (!string.IsNullOrEmpty(dtInvoiceList.Rows[0]["ICH_DTL_COUNT"].ToString()) && Convert.ToInt32(dtInvoiceList.Rows[0]["ICH_DTL_COUNT"].ToString()) == 0)
                            {
                                IsDoModified = true;
                                btnRefreshInv.Visible = true;
                            }
                            if (Convert.ToInt16(dtInvoiceList.Rows[0]["ICH_DEL_STATUS"].ToString()) == 1)
                            {
                                btnEditforCancel.Visible = false;
                                btnSave.Visible = false;
                                btnEdit.Visible = false;
                                IsDeleted = true;
                            }
                            else
                            {
                                btnSave.Visible = true;
                                btnEdit.Visible = true;
                                IsDeleted = false;
                            }
                            if (!string.IsNullOrEmpty(dtInvoiceList.Rows[0]["ICH_DEPT"].ToString()) && int.TryParse(dtInvoiceList.Rows[0]["ICH_DEPT"].ToString(), out dept))
                            {
                                Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                                base.SetUserDept();
                            }
                            hdfCurrentPk.Value = CurrPK.ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SetPrintDocsVisibility", "$(document).ready(function(){SetPrintDocsVisibility();});", true);
                            SetUIEditView(ActionsEnum.VIEW);
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
                            GetFieldValues(ControlsEnum.INVOICETYPE);
                            SetFieldValues(ControlsEnum.INVOICETYPE);
                            GetFieldValues(ControlsEnum.SOINVHEADER);
                            if (SOInvoiceHeaderSession != null)
                            {
                                DespatchID = Convert.ToInt16(SOInvoiceHeaderSession.ICH_DESPATCH_HDR);
                            }
                            GetFieldValues(ControlsEnum.SOINVHEADER);
                            SetFieldValues(ControlsEnum.SOINVHEADER);
                            GetFieldValues(ControlsEnum.TAXSETTINGS);
                            SetFieldValues(ControlsEnum.SOINVDETAIL);
                            SetFieldValues(ControlsEnum.UPLOADEDFILES);
                            TempSOInvoiceHeaderSession = SOInvoiceHeaderSession;
                            SetDetailTax(null);
                            SetHdrTax();
                            if (grdInvoice.Rows.Count > 0)
                            {
                                SetSubTotal();
                            }
                            //Settings of TaxPayableDiv
                            if (hdfIsTaxPayable.Value.ToString() == "1")
                            {
                                SetTaxPayableDiv();
                            }
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "TaxPayable", "$(document).ready(function () { ShowHideTaxPayableOuter();});", true);

                        }
                        break;
                    #endregion
                    #region SO INV HEADER
                    case ControlsEnum.SOINVHEADER:
                        //int CustAllAdv = 0; //0: Default, 1:List all advance from the customer
                        //CustAllAdv = String.IsNullOrEmpty(hdfIsCusAllAdv.Value) ? 0 : Convert.ToInt32(hdfIsCusAllAdv.Value);
                        //if (CustAllAdv == 1)
                        //{
                        //    invoiceHeaderObj = TempSOInvoiceHeaderSessionCustAll;
                        //}
                        invoiceHeaderObj = SOInvoiceHeaderSession;
                        if (invoiceHeaderObj != null)
                        {
                            CurrPK = invoiceHeaderObj.ICH_PK;
                            hdfCurrentPk.Value = CurrPK.ToString();
                            Approved = invoiceHeaderObj.ICH_STATUS;
                            lblInvoiceNo.Focus();
                            lnkDoNumber.Text = ERP.Utilities.CommonFunctions.GetDecodedString(invoiceHeaderObj.ICH_DESPATCH_NO);
                            lnkDoNumber.CommandArgument = invoiceHeaderObj.ICH_DESPATCH_HDR.ToString();
                            txtCustomer.Text = ERP.Utilities.CommonFunctions.GetDecodedString(invoiceHeaderObj.ICH_CUSTOMER_TEXT);
                            //txtTaxID.Text = ERP.Utilities.CommonFunctions.GetDecodedString(invoiceHeaderObj.ICH_CUSTOMER_GSTNO);
                            hdfCustomer.Value = invoiceHeaderObj.ICH_CUSTOMER.ToString();
                            //lblCustomerTxt.ToolTip = HttpUtility.HtmlDecode(invoiceHeaderObj.ICH_CUSTOMER_TEXT);
                            custPK = Convert.ToInt32(invoiceHeaderObj.ICH_CUSTOMER);
                            hdfSCDate.Value = invoiceHeaderObj.ICH_SOH_DT;
                            GetFieldValues(ControlsEnum.PAYMENTTERMS);
                            SetFieldValues(ControlsEnum.PAYMENTTERMS);
                            //CustomerTypes
                            hdfCurrCustomerPK.Value = custPK.ToString();
                            CustomerTypeSelectedPk = string.IsNullOrEmpty(invoiceHeaderObj.ICH_BRANCH) ? 0 : Convert.ToInt32(invoiceHeaderObj.ICH_BRANCH);
                            CustomerSavedBranchId = string.IsNullOrEmpty(invoiceHeaderObj.ICH_BRANCH_TEXT) ? "" : invoiceHeaderObj.ICH_BRANCH_TEXT;
                           // CustomerSavedTaxId = string.IsNullOrEmpty(invoiceHeaderObj.ICH_TAX_ID) ? "" : invoiceHeaderObj.ICH_TAX_ID;
                            CustomerSavedTaxId = string.IsNullOrEmpty(invoiceHeaderObj.ICH_CUSTOMER_GSTNO) ? "" : invoiceHeaderObj.ICH_CUSTOMER_GSTNO;
                            /*if (!string.IsNullOrEmpty(invoiceHeaderObj.CUS_SPECIAL_CAT_TEXT))
                            {
                                lblSpecialCat.Visible = true;
                                lblSpecialCat.Text = invoiceHeaderObj.CUS_SPECIAL_CAT_TEXT.ToString();
                            }*/
                            GetFieldValues(ControlsEnum.CUSTOMERTYPES);
                            SetFieldValues(ControlsEnum.CUSTOMERTYPES);
                            SetBranchIDEnableDisable();
                            if (CurrPK > 0)
                            {
                                //In Edit Mode For Showing Saved Data
                                if (CustomerTypeSelectedPk > 0 && ddlCustomerType.Items.FindByValue(CustomerTypeSelectedPk.ToString()) != null)
                                {
                                    ddlCustomerType.SelectedValue = CustomerTypeSelectedPk.ToString();
                                    txtTypeID.Text = CustomerSavedBranchId.ToString();
                                    txtTaxID.Text = txtTaxID.ToolTip = HttpUtility.HtmlDecode(HttpUtility.HtmlDecode(CustomerSavedTaxId.ToString()));
                                }
                                else
                                {
                                    txtTypeID.Text = "";
                                    txtTaxID.Text = "";
                                }
                            }

                            //End

                            paymentTermPK = Convert.ToInt32(invoiceHeaderObj.ICH_PAYMENT_TERM);
                            ddlPaymentTerms.SelectedValue = invoiceHeaderObj.ICH_PAYMENT_TERM;

                            ////lnkDoNo.Text = lnkDoNo.ToolTip = HttpUtility.HtmlDecode(invoiceHeaderObj.ICH_DESPATCH_NO);// ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(invoiceHeaderObj.d) + " (" + HttpUtility.HtmlDecode(invoiceHeaderObj.ICH_TYPE_TEXT) + ")", 23);
                            ////if (invoiceHeaderObj.ICH_DESPATCH_STATUS == 1)
                            ////    lnkDoNo.Text = lnkDoNo.ToolTip = HttpUtility.HtmlDecode(invoiceHeaderObj.ICH_DESPATCH_NO + GetLocalResourceObject("DoCancelled"));

                            ////hdfDoPk.Value = invoiceHeaderObj.ICH_DESPATCH_HDR; //lnkSoNo.ToolTip = ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(invoiceHeaderObj.ICH_SO_NO) + " (" + HttpUtility.HtmlDecode(invoiceHeaderObj.ICH_TYPE_TEXT) + ")", 333);

                            SaleOrderType = HttpUtility.HtmlDecode(invoiceHeaderObj.ICH_TYPE_TEXT) == Convert.ToString(SalesInvoiceType.Domestic) ? 1 : 2;
                            hdfSaleOrderType.Value = SaleOrderType.ToString();
                            SetSaleorderTypeVisibility();

                            //lblSODateTxt.Text = DateTime.Parse(HttpUtility.HtmlDecode(invoiceHeaderObj.ICH_DESPATCH_DATE)).ToString(Resources.Constants.DateFormatShort);
                            //lblSODateTxt.ToolTip = DateTime.Parse(HttpUtility.HtmlDecode(invoiceHeaderObj.ICH_DESPATCH_DATE)).ToString(Resources.Constants.DateFormatShort);

                            //lblSOAmtTxt.ToolTip = lblSOAmtTxt.Text = string.IsNullOrEmpty(invoiceHeaderObj.ICH_SO_AMOUNT_NET_TC) ? String.Format("{0:c}", 0) : String.Format("{0:c}", Convert.ToDecimal(invoiceHeaderObj.ICH_SO_AMOUNT_NET_TC));

                            //if (invoiceHeaderObj.ICH_TYPE == "3")
                            //{
                            //    lblInvAmtTxt.Text = lblInvAmtTxt.ToolTip = string.IsNullOrEmpty(invoiceHeaderObj.ICH_INV_PAMT) ? String.Format("{0:c}", 0) : String.Format("{0:c}", Convert.ToDecimal(invoiceHeaderObj.ICH_INV_PAMT));
                            //}
                            //else
                            //{
                            //    lblInvAmtTxt.Text = lblInvAmtTxt.ToolTip = string.IsNullOrEmpty(invoiceHeaderObj.ICH_INV_AMT) ? String.Format("{0:c}", 0) : String.Format("{0:c}", Convert.ToDecimal(invoiceHeaderObj.ICH_INV_AMT));
                            //}


                            txtInvoiceDate.Text = invoiceHeaderObj.ICH_DATE;
                            hdfInvoiceDate.Value = invoiceHeaderObj.ICH_DATE;

                            if (!string.IsNullOrEmpty(invoiceHeaderObj.ICH_GST_TYPE))
                                ddlInvoiceGstType.SelectedValue = invoiceHeaderObj.ICH_GST_TYPE;

                            txtCurrency.Text = ERP.Utilities.CommonFunctions.GetDecodedString(invoiceHeaderObj.ICH_CURRENCY_TEXT);
                            hdfCurrency.Value = invoiceHeaderObj.ICH_CURRENCY.ToString();
                            GetFieldValues(ControlsEnum.EXCHANGERATE);
                            if (CurrPK > 0 && !ReloadInvoice)
                            {
                                txtExchangeRate.Text = hdfExchangeRate.Value = invoiceHeaderObj.ICH_EXCHG_RATE.ToString();
                            }

                            //lblCurrencyTxt.ToolTip = HttpUtility.HtmlDecode(invoiceHeaderObj.ICH_CURRENCY_TEXT);
                            if (!string.IsNullOrEmpty(invoiceHeaderObj.ICH_TYPE))
                                ddlInvoiceType.SelectedValue = invoiceHeaderObj.ICH_TYPE;

                            hdfInvoicePK.Value = invoiceHeaderObj.ICH_PK.ToString();
                            hdfInvoiceNo.Value = string.IsNullOrEmpty(invoiceHeaderObj.ICH_NO) ? string.Empty : invoiceHeaderObj.ICH_NO;
                            lblInvoiceNo.Text = string.IsNullOrEmpty(invoiceHeaderObj.ICH_NO) ? Resources.ErpRes.Draft : invoiceHeaderObj.ICH_NO;
                            hdfCreditDays.Value = invoiceHeaderObj.ICH_CUSTOMER_CREDIT_DAYS.ToString();
                            txtInvoiceDueDate.Text = ((CurrPK == 0 && !string.IsNullOrEmpty(invoiceHeaderObj.ICH_DATE)) ?
                                Convert.ToDateTime(invoiceHeaderObj.ICH_DATE).AddDays(invoiceHeaderObj.ICH_CUSTOMER_CREDIT_DAYS).ToString(Resources.Constants.DateFormatShort) :
                                invoiceHeaderObj.ICH_DATE_PAY_BY);
                            hdfInvoiceDueDate.Value = invoiceHeaderObj.ICH_DATE_PAY_BY;
                            txtRemarks.Text = HttpUtility.HtmlDecode(invoiceHeaderObj.ICH_REMARKS);
                            //txtInvoiceType.Text = HttpUtility.HtmlDecode(invoiceHeaderObj.ICH_TYPE_TEXT);
                            hdfType.Value = HttpUtility.HtmlDecode(invoiceHeaderObj.ICH_TYPE_TEXT);
                            if (hdfType.Value != string.Empty && CurrPK > 0)
                            {
                                btnPrint.Visible = true;
                            }
                            else
                            {
                                btnPrint.Visible = false;
                            }
                            string ReferenceNo = string.Empty;
                            string SpecialNote = string.Empty;
                            string DeliveryTerms = string.Empty;
                            double AdjAmount = 0;
                            //if (TempSOInvoiceHeaderTSessionCustAll != null && TempSOInvoiceHeaderTSessionCustAll.SOMainList.Count > 0 && TempSOInvoiceHeaderTSessionCustAll.SOMainList.Where(r => r.OrderDetail.Sum(odr => odr.CID_QTY_DO_DISPATCHED) > 0 || DOCancelStatus == 1).Count() > 0)
                            //{
                            //    TempSOInvoiceHeaderTSessionCustAll.SOMainList.ForEach(dtl =>
                            //            {
                            //                if (dtl.OrderDetail.Sum(odr => odr.CID_QTY_DO_DISPATCHED) > 0 || DOCancelStatus == 1)
                            //                {
                            //                    ReferenceNo += dtl.ICH_REF_NO + ",";
                            //                    SpecialNote += dtl.ICH_SPECIAL_NOTES + ",";
                            //                    if (string.IsNullOrEmpty(DeliveryTerms))
                            //                        DeliveryTerms = dtl.ICH_DEL_TERM_TEXT;
                            //                }
                            //            }
                            //        );
                            //    AdjAmount = TempSOInvoiceHeaderTSessionCustAll.SOMainList.Sum(lst => lst.ICH_AMOUNT_ADJUST);
                            //}
                            //else if (invoiceHeaderMulObj != null && invoiceHeaderMulObj.SOMainList.Count > 0 && invoiceHeaderMulObj.SOMainList.Where(r => r.OrderDetail.Sum(odr => odr.CID_QTY_DO_DISPATCHED) > 0 || DOCancelStatus == 1).Count() > 0)
                            //{
                            //    invoiceHeaderMulObj.SOMainList.ForEach(dtl =>
                            //            {
                            //                if (dtl.OrderDetail.Sum(odr => odr.CID_QTY_DO_DISPATCHED) > 0 || DOCancelStatus == 1)
                            //                {
                            //                    ReferenceNo += dtl.ICH_REF_NO + ",";
                            //                    SpecialNote += dtl.ICH_SPECIAL_NOTES + ",";
                            //                    if (string.IsNullOrEmpty(DeliveryTerms))
                            //                        DeliveryTerms = dtl.ICH_DEL_TERM_TEXT;
                            //                }
                            //            }
                            //        );
                            //    AdjAmount = invoiceHeaderMulObj.SOMainList.Sum(lst => lst.ICH_AMOUNT_ADJUST);
                            //}

                            AdjAmount = invoiceHeaderObj.ICH_AMOUNT_ADJUST;

                            ReferenceNo = HttpUtility.HtmlDecode(invoiceHeaderObj.ICH_REFERENCE);
                            SpecialNote = SpecialNote.TrimEnd(',');
                            if (string.IsNullOrEmpty(txtReference.Text.Trim()) && !string.IsNullOrEmpty(invoiceHeaderObj.ICH_REFERENCE) && (EntryStatus == EntryStatus.NEWMODE || ReloadInvoice || IsDoModified))//!string.IsNullOrEmpty(invoiceHeaderObj.ICH_REFERENCE) && (EntryStatus == EntryStatus.NEWMODE || ReloadInvoice || IsDoModified)
                            {
                                ReferenceNo = GetLocalResourceObject("ScNo") + ReferenceNo;
                            }
                            else
                                ReferenceNo = txtReference.Text + "," + ReferenceNo;
                            ReferenceNo = ReferenceNo.TrimStart(',');
                            txtReference.Text = HttpUtility.HtmlDecode(ReferenceNo);
                            txtReference.ToolTip = HttpUtility.HtmlDecode(ReferenceNo);

                            txtDeliveryTerms.Text = HttpUtility.HtmlDecode(DeliveryTerms);
                            //txtPriceAdj.Text = invoiceHeaderObj.ICH_AMOUNT_ADJUST.ToString(hdfCurrencyFormat.Value);
                            txtPriceAdj.Text = AdjAmount.ToString(hdfCurrencyFormat.Value);

                            //if (EntryStatus != EntryStatus.NEWMODE)
                            //{
                            //    txtPortofLoading.Text = HttpUtility.HtmlDecode(invoiceHeaderObj.ICH_FROM_PORT_TEXT);
                            //    txtFinalDest.Text = HttpUtility.HtmlDecode(invoiceHeaderObj.ICH_FINAL_DESTINATION);
                            //}
                            //if (EntryStatus != EntryStatus.NEWMODE)
                            //{
                            //    DespatchID = Convert.ToInt32(invoiceHeaderObj.ICH_DESPATCH_HDR);
                            //    GetFieldValues(ControlsEnum.DELIVERYORDERDETAILS);
                            //    if (salDespatchDtlList != null && salDespatchDtlList.Count > 0)
                            //    {
                            //        txtPortofLoading.Text = salDespatchDtlList[0].SAL_DESPATCH_HDR.ADM_CONST_MST1.CON_NAME;
                            //        hdfPortofLoading.Value = salDespatchDtlList[0].SAL_DESPATCH_HDR.ADM_CONST_MST1.CON_PK.ToString();
                            //        txtFinalDest.Text = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_FINAL_DESTINATION;
                            //    }
                            //}
                            txtHdrDiscount.ToolTip = txtHdrDiscount.Text = invoiceHeaderObj.ICH_DISCOUNT_TC.ToString(hdfCurrencyFormat.Value);
                            txtHdrDeduction.ToolTip = txtHdrDeduction.Text = invoiceHeaderObj.ICH_AMOUNT_ADV_DED_TC.ToString(hdfCurrencyFormat.Value);
                            txtTotalDeductionExp.ToolTip = txtTotalDeductionExp.Text = invoiceHeaderObj.ICH_AMOUNT_ADV_DED_TC.ToString(hdfCurrencyFormat.Value);
                            txtHdrTax.ToolTip = txtHdrTax.Text = invoiceHeaderObj.ICH_TAX_TC.ToString(hdfCurrencyFormat.Value);
                            txtShipping.Text = invoiceHeaderObj.ICH_SHIP_CHARGE.ToString(hdfCurrencyFormat.Value);

                            txtPriceAdj.ToolTip = invoiceHeaderObj.ICH_AMOUNT_ADJUST.ToString(hdfCurrencyFormat.Value);
                            hdfPriceAdj.Value = invoiceHeaderObj.ICH_AMOUNT_ADJUST.ToString(hdfCurrencyFormat.Value);
                            txtHdrNetTotal.ToolTip = txtHdrNetTotal.Text = invoiceHeaderObj.ICH_AMOUNT_NET_TC.ToString(hdfCurrencyFormat.Value);
                            txtTotalExp.ToolTip = txtTotalExp.Text = invoiceHeaderObj.ICH_AMOUNT_NET_TC.ToString(hdfCurrencyFormat.Value);
                            txtHdrInvoiceTotal.ToolTip = txtHdrInvoiceTotal.Text = invoiceHeaderObj.ICH_NET_VALUE_TC.ToString(hdfCurrencyFormat.Value);
                            if (invoiceHeaderObj.DeductionDetails != null && invoiceHeaderObj.DeductionDetails.Count > 0)
                            {
                                //txtDiscDeducted.Text = txtDiscDeducted.ToolTip = invoiceHeaderObj.DeductionDetails.Select(p => p.IAD_DISC_AMOUNT.ToString(hdfCurrencyFormat.Value)).FirstOrDefault();// totalAllocatedDiscount.ToString(hdfCurrencyFormat.Value);
                                // Fix Bug ID:- 4366
                                txtDiscDeducted.Text = txtDiscDeducted.ToolTip = invoiceHeaderObj.DeductionDetails.Sum(p => p.IAD_DISC_AMOUNT).ToString(hdfCurrencyFormat.Value);
                                totalAllocatedDiscount = Convert.ToDecimal(txtDiscDeducted.Text);
                            }

                            txtAdvAdjustDeductAmount.ToolTip = txtAdvAdjustDeductAmount.Text = invoiceHeaderObj.ICH_ADV_ADJ_DED.ToString(hdfCurrencyFormat.Value);
                            if (invoiceHeaderObj.ICH_ADV_ADJ_DED == 0)
                                trAdvAdjDed.Visible = false;
                            else
                                trAdvAdjDed.Visible = true;

                            //txtFeederVessel.Text = HttpUtility.HtmlDecode(invoiceHeaderObj.ICH_FEEDER_VESSEL);
                            //txtMotherVessel.Text = HttpUtility.HtmlDecode(invoiceHeaderObj.ICH_MOTHER_VESSEL);
                            //txtContainerNo.Text = HttpUtility.HtmlDecode(invoiceHeaderObj.ICH_CONTAINER_NO);
                            txtETD.Text = invoiceHeaderObj.ICH_ETD;
                            //txtETA.Text = invoiceHeaderObj.ICH_ETA;

                            ddlCompany.SelectedValue = invoiceHeaderObj.ICH_COMPANY.ToString();
                            //if (!string.IsNullOrEmpty(invoiceHeaderObj.ICH_SHIPPING_MARK))
                            //{
                            //    txtShippingMark.Text = HttpUtility.HtmlDecode((invoiceHeaderObj.ICH_SHIPPING_MARK)).Replace("\n", " , ");
                            //}
                            //else
                            //{
                            //    txtShippingMark.Text = string.Empty;
                            //}
                            ////txtSpecialNotes.Text = HttpUtility.HtmlDecode(invoiceHeaderObj.ICH_SPECIAL_NOTES);
                            //txtSpecialNotes.Text = HttpUtility.HtmlDecode(SpecialNote);
                            txtdor.Text = string.IsNullOrEmpty(invoiceHeaderObj.ICH_SHIP_CHARGE_DED) ? "0.00" : Convert.ToDouble(invoiceHeaderObj.ICH_SHIP_CHARGE_DED).ToString(hdfCurrencyFormat.Value);
                            txtTerms.Text = HttpUtility.HtmlDecode(invoiceHeaderObj.ICH_DEL_TERM_TEXT);
                            if (CurrPK > 0)
                            {
                                ModifiedDatePnl.Visible = true;
                                LastModifiedTime = invoiceHeaderObj.LAST_MOD_DT;
                                lblLastModifiedHDR.Text = Resources.ErpRes.LastModifiedOn + LastModifiedTime.ToString(Resources.ErpRes.LastModifiedDatetimeFormat);
                                txtTerms.Text = HttpUtility.HtmlDecode(invoiceHeaderObj.ICH_TERM1);
                                txtTotalTerms.Text = HttpUtility.HtmlDecode(invoiceHeaderObj.ICH_TERM2);
                            }


                            if (invoiceHeaderObj.ICH_HAS_DUE_DTL != null ? (invoiceHeaderObj.ICH_HAS_DUE_DTL.ToString() == "1" ? true : false) : false)
                            {
                                txtInvoiceDueDate.Enabled = false;
                            }
                            else
                            {
                                txtInvoiceDueDate.Enabled = true;
                            }

                        }
                        break;
                    #endregion
                    //case ControlsEnum.GETDUEDATE:
                    //    SetUIValuesToObject(ControlsEnum.GETDUEDATE);
                    //    break; 

                    #region SELECTED DOC
                    case ControlsEnum.SELECTEDDOC:
                        if (SOInvoiceUploadObj != null)
                        {
                            CurrSlNo = SOInvoiceUploadObj.DOC_SEQ_NO;
                            anchorFile.Visible = true;
                            vrfFileUpload.Enabled = false;
                            anchorFile.InnerHtml = SOInvoiceUploadObj.DOC_NAME;
                            anchorFile.HRef = SOInvoiceUploadObj.DOC_PATH;
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
                    #endregion
                    # region  FILE_UPLOAD
                    case ControlsEnum.UPLOADEDFILES:
                        CurrPK = invoiceHeaderObj.ICH_PK;
                        SOInvoiceUploadList = invoiceHeaderObj.FileList;
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
            switch (controlType)
            {
                #region TAXT YPES
                case ControlsEnum.TAXTYPES:
                    ddlPopupTaxType.Items.Clear();
                    if (dtTaxDetails != null && dtTaxDetails.Rows.Count > 0)
                    {
                        ddlPopupTaxType.DataSource = CommonFunctions.HtmlDecodeDataTable(dtTaxDetails, Resources.DataFieldRes.RFQResponseTaxHead);
                        ddlPopupTaxType.DataTextField = Resources.DataFieldRes.RFQResponseTaxHead;
                        ddlPopupTaxType.DataValueField = Resources.DataFieldRes.RFQResponseTaxPK;
                        ddlPopupTaxType.DataBind();
                    }
                    if (IsCustomTaxEnabled || Convert.ToInt16(hdfTaxCategory.Value) == ((int)TaxType.Discount) || Convert.ToInt16(hdfTaxCategory.Value) == ((int)TaxType.Shipping))
                        ddlPopupTaxType.Items.Add(new ListItem(Resources.Report.Custom, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region SO TYPE
                case ControlsEnum.SOTYPE:
                    ddlSaleOrderType.Items.Clear();
                    if (dtInvoiceType != null && dtInvoiceType.Rows.Count > 0)
                    {
                        ddlSaleOrderType.DataSource = dtInvoiceType;
                        ddlSaleOrderType.DataTextField = "CFG_DATA";
                        ddlSaleOrderType.DataValueField = "CFG_VALUE";
                        ddlSaleOrderType.DataBind();
                    }
                    ddlSaleOrderType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region INVOICE TYPE
                case ControlsEnum.INVOICETYPE:
                    ddlInvoiceType.Items.Clear();
                    if (dtInvoiceType != null && dtInvoiceType.Rows.Count > 0)
                    {
                        ddlInvoiceType.DataSource = dtInvoiceType;
                        ddlInvoiceType.DataTextField = "CFG_DATA";
                        ddlInvoiceType.DataValueField = "CFG_VALUE";
                        ddlInvoiceType.DataBind();
                    }
                    ddlInvoiceType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region PAYMENT TERMS
                case ControlsEnum.PAYMENTTERMS:
                    ddlPaymentTerms.Items.Clear();
                    if (dtPaymentTerms != null)
                    {
                        ddlPaymentTerms.DataSource = CommonFunctions.HtmlDecodeDataTable(dtPaymentTerms, "TCH_NAME");
                        ddlPaymentTerms.DataTextField = "TCH_NAME";
                        ddlPaymentTerms.DataValueField = "TCH_PK";
                        ddlPaymentTerms.DataBind();
                    }
                    ddlPaymentTerms.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    if (paymentTermPK > 0 && ddlPaymentTerms.Items.FindByValue(paymentTermPK.ToString()) != null)
                        ddlPaymentTerms.SelectedValue = paymentTermPK.ToString();
                    break;
                #endregion
                # region INVOICEGSTTYPE
                case ControlsEnum.INVOICEGSTTYPE:
                    ddlInvoiceGstType.Items.Clear();
                    if (dtInvoiceGstType != null && dtInvoiceGstType.Rows.Count > 0)
                    {
                        ddlInvoiceGstType.DataSource = dtInvoiceGstType;
                        ddlInvoiceGstType.DataTextField = "FTM_CODE";
                        ddlInvoiceGstType.DataValueField = "FTM_PK";
                        ddlInvoiceGstType.DataBind();
                    }
                    ddlInvoiceGstType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    if (InvoiceGstTypePK > 0 && ddlInvoiceGstType.Items.FindByValue(InvoiceGstTypePK.ToString()) != null)
                        ddlInvoiceGstType.SelectedValue = InvoiceGstTypePK.ToString();
                    break;
                #endregion

                #region COMPANY
                case ControlsEnum.COMPANY:
                    ddlCompany.Items.Clear();
                    if (admCompanyMstList != null && admCompanyMstList.Count > 0)
                    {
                        ddlCompany.DataSource = CommonFunctions.HtmlDecode(admCompanyMstList, Resources.DataFieldRes.CompanySpecs);
                        ddlCompany.DataTextField = Resources.DataFieldRes.CompanySpecs;
                        ddlCompany.DataValueField = Resources.DataFieldRes.CompanyMstPK;
                        ddlCompany.DataBind();
                    }
                    //  ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(dtCompany.Rows[0][Resources.DataFieldRes.CompanyMstPK].ToString()));
                    //if (dtCompany.Rows.Count > 0)
                    //{
                    //    ddlCompany.DataSource = dtCompany;

                    //    ddlCompany.DataSource = CommonFunctions.HtmlDecode(dtCompany);,Resources.DataFieldRes.CompanySpecs);
                    //    ddlCompany.DataTextField = Resources.DataFieldRes.CompanySpecs;
                    //    ddlCompany.DataValueField = Resources.DataFieldRes.CompanyMstPK;
                    //    ddlCompany.DataBind();

                    //}
                    break;
                #endregion
                #region CUSTOMER TYPES
                case ControlsEnum.CUSTOMERTYPES:
                    ddlCustomerType.Items.Clear();
                    if (dsCustomerTypes != null && dsCustomerTypes.Tables[0].Rows.Count > 0)
                    {
                        ddlCustomerType.DataSource = CommonFunctions.HtmlDecodeDataTable(dsCustomerTypes.Tables[0], "CAD_NAME");
                        ddlCustomerType.DataTextField = "CAD_NAME";
                        ddlCustomerType.DataValueField = "CAD_PK";
                        ddlCustomerType.DataBind();
                        //SetBranchIDEnableDisable();
                    }
                    ddlCustomerType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    if (CustomerTypeSelectedPk > 0 && ddlCustomerType.Items.FindByValue(CustomerTypeSelectedPk.ToString()) != null)
                    {
                        ddlCustomerType.SelectedValue = CustomerTypeSelectedPk.ToString();
                    }
                    break;
                #endregion
                default:
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
                    #region PEDING SO LIST
                    case ControlsEnum.PEDINGSOLIST:
                        uclSOPaging.TotalPages = TotalPages;
                        PageIndexSO = string.IsNullOrEmpty(PageIndexSO) ? CommonConstants.SELECT_VALUE_ONE : PageIndexSO;
                        uclSOPaging.CurrentPage = Convert.ToInt32(PageIndexSO);
                        if (dtPendingSOList != null && dtPendingSOList.Rows.Count > 0)
                            grdSoList.DataSource = dtPendingSOList;
                        else
                            grdSoList.DataSource = null;
                        grdSoList.DataBind();
                        uclSOPaging.Visible = true;
                        uclSOPaging.BindPager();
                        break;
                    #endregion
                    #region SO INV DETAIL
                    case ControlsEnum.SOINVDETAIL:
                        if (SOInvoiceHeaderSession != null)
                        {
                            List<DirectSOInvoiceDetails> soInvoiceTList;
                            soInvoiceDetailsList = new List<DirectSOInvoiceDetails>();

                            soInvoiceTList = new List<DirectSOInvoiceDetails>();
                            soInvoiceTList = SOInvoiceHeaderSession.OrderDetail.Where(sod => sod.CID_QTY_DO_DISPATCHED > 0 || DOCancelStatus == 1).ToList();
                            soInvoiceDetailsList.AddRange(soInvoiceTList);

                            if (soInvoiceDetailsList != null && !IsDoModified)
                            {
                                grdInvoice.DataSource = soInvoiceDetailsList;
                                grdInvoice.DataBind();
                            }
                            else
                            {
                                grdInvoice.DataSource = null;
                                grdInvoice.DataBind();
                            }
                        }
                        break;
                    #endregion
                    #region TAX POPUP GRID
                    case ControlsEnum.TAXPOPUPGRID:

                        if (IsHeaderTax)
                        {
                            taxHdrList = TempSOInvoiceHeaderSession.TaxHdr.Where(tax => tax.CIT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value)).ToList();
                        }
                        else
                        {
                            soDtlObj = TempSOInvoiceHeaderSession.OrderDetail.SingleOrDefault(rfq => rfq.CID_SO_DTL == ScDetailsPK.ToString());
                            if (soDtlObj != null)
                            {
                                taxHdrList = soDtlObj.TaxDtl.Where(tax => tax.CIT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value)).ToList();
                            }
                        }
                        grdTaxDetails.DataSource = taxHdrList;
                        grdTaxDetails.DataBind();
                        break;
                    #endregion
                    #region DEDUCTIONPOPUPGRID
                    case ControlsEnum.DEDUCTIONPOPUPGRID:

                        if (hdfIsCusAllAdv.Value == CommonConstants.SELECT_VALUE_ONE)//For Showing All Pending Allocation of Current Customer
                        {
                            if (SOInvoiceHeaderSession != null && SOInvoiceHeaderSession.DeductionDetails != null && SOInvoiceHeaderSession.DeductionDetails.Count > 0)
                                grdDeduction.DataSource = SOInvoiceHeaderSession.DeductionDetails.ToList();
                            else
                                grdDeduction.DataSource = null;
                            grdDeduction.DataBind();
                        }
                        else//in case of local
                        {
                            //Change for multiple SC
                            deductionDtlList = new List<DirectSOAdvDeductionDetails>();
                            if (SOInvoiceHeaderSession != null)
                            {
                                deductionDtlList.AddRange(SOInvoiceHeaderSession.DeductionDetails.ToList());
                            }
                            //TempSOInvoiceHeaderSession.DeductionDetails.ToList();                         
                            if (deductionDtlList != null && deductionDtlList.Count > 0)
                            {
                                grdDeduction.DataSource = deductionDtlList;
                                grdDeduction.DataBind();
                                Label lblDedTotalAllocateNowFooterSplit = grdDeduction.FooterRow.FindControl("lblDedTotalAllocateNowFooterSplit") as Label;
                                HiddenField hdfDedTotalAllocateNowFooterSplit = grdDeduction.FooterRow.FindControl("hdfDedTotalAllocateNowFooterSplit") as HiddenField;

                                Label lblTotalTaxFooter = grdDeduction.FooterRow.FindControl("lblTotalTaxFooter") as Label;
                                HiddenField hdfTaxTotalFooterSplit = grdDeduction.FooterRow.FindControl("hdfTaxTotalFooterSplit") as HiddenField;

                                if (lblDedTotalAllocateNowFooterSplit != null && hdfDedTotalAllocateNowFooterSplit != null)
                                {
                                    decimal totalTax = deductionDtlList.Sum(aa => aa.IAD_TAX_AMOUNT);
                                    decimal total = deductionDtlList.Sum(aa => aa.IAD_AMOUNT);
                                    if (total == 0)
                                    {
                                        total = deductionDtlList.Sum(aa => aa.RCM_RCVD_AMOUNT - aa.ICH_AMOUNT_ALLOCATED);
                                        totalTax = deductionDtlList.Sum(aa => aa.RCM_TAX_AMOUNT - aa.ICH_TAX_AMT_ALLOCATED);
                                    }
                                    hdfDedTotalAllocateNowFooterSplit.Value = total.ToString(hdfCurrencyFormat.Value);
                                    lblDedTotalAllocateNowFooterSplit.Text = string.Format("{0:c}", total);
                                    lblTotalTaxFooter.Text = GetFormattedCurrencyWithComa(totalTax);//.ToString(hdfCurrencyFormat.Value);
                                    hdfTaxTotalFooterSplit.Value = totalTax.ToString(hdfCurrencyFormat.Value);
                                }
                            }
                            else
                            {
                                grdDeduction.DataSource = deductionDtlList;
                                grdDeduction.DataBind();
                            }
                        }
                        break;
                    #endregion
                    #region INVOICELIST
                    case ControlsEnum.INVOICELIST:
                        uclInvListPaging.TotalPages = TotalPages;
                        PageIndex = string.IsNullOrEmpty(PageIndex) ? CommonConstants.SELECT_VALUE_ONE : PageIndex;
                        uclInvListPaging.CurrentPage = Convert.ToInt32(PageIndex);
                        if (dtInvoiceList != null)
                        {
                            GetFieldValues(ControlsEnum.FILLWORKFLOWSTATUS);
                            PageIndex = PageIndex == null ? "0" : PageIndex;
                            grdInvoiceList.PageIndex = Convert.ToInt32(PageIndex);
                            grdInvoiceList.DataSource = dtInvoiceList.DefaultView;
                            grdInvoiceList.DataBind();

                            uclInvListPaging.Visible = true;
                            uclInvListPaging.BindPager();
                            //For Setting/Resetting Colour of a selected InvoiceNo
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowSelectedRow", "$(document).ready(function(){SetSelectedRowColor();});", true);
                            //End
                        }
                        break;
                    #endregion
                    #region AMOUNTDETAILS
                    case ControlsEnum.AMOUNTDETAILS:
                        if (dtAmountDetails != null)
                        {
                            grdPaidAmntSplitup.DataSource = dtAmountDetails;
                            grdPaidAmntSplitup.DataBind();
                        }
                        break;
                    #endregion
                    #region DUEDATEPOPUPGRID
                    case ControlsEnum.DUEDATEPOPUPGRID:
                        if (dsDueDate != null & dsDueDate.Tables[0].Rows.Count > 0)
                            grdDueDateDetails.DataSource = dsDueDate.Tables[0];
                        else
                            grdDueDateDetails.DataSource = null;
                        grdDueDateDetails.DataBind();
                        break;
                    #endregion
                    #region OTHERCHARGELIST
                    case ControlsEnum.OTHERCHARGELIST:
                        taxHdrList = TempSOInvoiceHeaderSession.TaxHdr.Where(tax => tax.CIT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value)).ToList();
                        grdOtherchargeSplit.DataSource = taxHdrList;
                        grdOtherchargeSplit.DataBind();

                        //POTotalOtherAmount = 0;
                        //POTotalInvOtherAmount = 0;
                        //POTotalBalanceOtherAmount = 0;
                        //if (POInvoiceHeaderSession != null)
                        //{
                        //    poOtherChargeList = new List<POOtherChargeDetails>();
                        //    poOtherChargeList = POInvoiceHeaderSession.OtherChargeDetails;
                        //    POTotalOtherAmount = poOtherChargeList.Sum(toa => toa.PO_OTHER_AMOUNT);
                        //    POTotalInvOtherAmount = poOtherChargeList.Sum(tioa => tioa.PO_OTHER_AMOUNT_INVOICED);
                        //    POTotalBalanceOtherAmount = POTotalOtherAmount - POTotalInvOtherAmount;//poOtherChargeList.Sum(tba => tba.IVM_OTHER_AMOUNT);
                        //    grdOtherchargeSplit.DataSource = poOtherChargeList;
                        //    grdOtherchargeSplit.DataBind();
                        //}
                        //else
                        //{
                        //    grdOtherchargeSplit.DataSource = null;
                        //    grdOtherchargeSplit.DataBind();
                        //}
                        break;
                    #endregion
                    #region UPLOADED FILES
                    case ControlsEnum.UPLOADEDFILES:
                        grdUploads.DataSource = SOInvoiceUploadList;
                        grdUploads.DataBind();
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

        /// <summary>
        /// Method used to Reset Dorm Controls
        /// </summary>
        private void ResetForm(ControlsEnum controlType)
        {
            switch (controlType)
            {
                #region TAX POPUP GRID
                case ControlsEnum.TAXPOPUPGRID:
                    txtPopupAmount.Text = string.Empty;
                    txtPopupItemAmount.Text = string.Empty;
                    txtPopupOther.Text = string.Empty;
                    TaxPK = 0;
                    grdTaxDetails.DataSource = null;
                    grdTaxDetails.DataBind();
                    TempSOInvoiceHeaderSession = null;
                    hdfTaxFormula.Value = string.Empty;
                    hdfTaxCode.Value = string.Empty;
                    hdfTaxRate.Value = string.Empty;
                    SOInvoicePK = 0;
                    SelectedItemPK = 0;
                    hdfTaxCategory.Value = string.Empty;

                    break;
                #endregion
                #region SO INV HEADER
                case ControlsEnum.SOINVHEADER:
                    SOInvoiceHeaderSession = null;
                    //invoiceHeaderMulObj = null;
                    TempInvoiceHeaderTemp = null;
                    TempSOInvoiceHeaderSession = null;
                    TempSOInvoiceHeaderSessionCustAll = null;
                    //TempSOInvoiceHeaderTSessionCustAll = null;
                    CurrPK = 0;
                    IsDeleted = false;
                    AST_DOC_MODE.Value = GetDOCMODE();
                    AST_CODE.Value = ApplicationType.DSI;
                    hdfInvoiceNo.Value = string.Empty;
                    lblInvoiceNo.Text = Resources.ErpRes.Draft;
                    hdfAppType.Value = ApplicationType.DSI;
                    hdfAppSubType.Value = string.Empty;
                    hdfCurrentPk.Value = CurrPK.ToString();

                    txtInvoiceDate.Text = string.Empty;
                    txtCustomer.Text = string.Empty;
                    hdfCustomer.Value = string.Empty;
                    ddlInvoiceType.ClearSelection();
                    txtETD.Text = string.Empty;
                    txtReference.Text = string.Empty;
                    ddlPaymentTerms.ClearSelection();
                    txtInvoiceDueDate.Text = string.Empty;
                    txtCurrency.Text = string.Empty;
                    hdfCurrency.Value = string.Empty;
                    txtExchangeRate.Text = string.Empty;
                    ddlCustomerType.ClearSelection();
                    txtTypeID.Text = string.Empty;
                    txtTaxID.Text = string.Empty;
                    grdSoList.DataSource = null;
                    grdSoList.DataBind();
                    grdInvoice.DataSource = null;
                    grdInvoice.DataBind();
                    grdUploads.DataSource = null;
                    grdUploads.DataBind();

                    txtHdrDiscount.Text = string.Empty;
                    txtHdrTotal.Text = string.Empty;
                    txtHdrBalBeforeVat.Text = string.Empty;
                    txtShipping.Text = string.Empty;
                    txtHdrTax.Text = string.Empty;
                    txtPriceAdj.Text = string.Empty;
                    txtTotalExp.Text = string.Empty;
                    txtTotalDeductionExp.Text = string.Empty;
                    txtHdrNetTotal.Text = string.Empty;
                    txtHdrInvoiceTotal.Text = string.Empty;
                    txtTotalTerms.Text = string.Empty;
                    txtRemarks.Text = string.Empty;
                    txtTerms.Text = string.Empty;
                    txtHdrDeduction.Text = string.Empty;
                    txtDiscDeducted.Text = string.Empty;
                    txtdor.Text = string.Empty;
                    lnkDoNumber.Text = string.Empty;
                    base.WkfRefID = ucrWrkf.RefID = 0;
                    SetCancelRef(CurrPK);
                    ucrWrkf.FillWorkFlowDetails();
                    ucrWrkf.ViewType = 1;
                    ucrWrkf.ViewAction();
                    lblSpecialCat.Text = string.Empty;
                    break;
                #endregion
                #region INVOICE LIST
                case ControlsEnum.INVOICELIST:
                    SOInvoiceHeaderSession = null;
                    TempInvoiceHeaderTemp = null;
                    TempSOInvoiceHeaderSession = null;
                    TempSOInvoiceHeaderSessionCustAll = null;
                    PageIndex = PageIndexSO = CommonConstants.SELECT_VALUE_ONE;
                    uclSOPaging.CurrentPage = uclInvListPaging.CurrentPage = 1;
                    CurrPK = 0;
                    hdfCurrentPk.Value = CurrPK.ToString();
                    DespatchID = 0;
                    CurrSOPK = 0;
                    DOCancelStatus = 0;
                    txtInvoiceNumber.Text = string.Empty;
                    txtCustomerSearch.Text = string.Empty;
                    txtDueAson.Text = string.Empty;
                    txtDrCrNo.Text = string.Empty;
                    hdfIsCusAllAdv.Value = CommonConstants.SELECT_VALUE_ZERO;
                    ddlSaleOrderType.SelectedValue = CommonConstants.SELECTVAL;
                    //txtInType.Text = string.Empty;
                    hdfIVHPK.Value = "";
                    hdfCustomerSearch.Value = "";
                    //txtFromDate.Text = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString(Resources.Constants.DateFormatShort);
                    //hdfFromDate.Value = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString();
                    //txtToDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    //hdfToDate.Value = DateTime.Now.ToString();
                    txtFromDate.Text = string.Empty;
                    hdfFromDate.Value = string.Empty;
                    txtToDate.Text = string.Empty;
                    hdfToDate.Value = string.Empty;
                    ModifiedDatePnl.Visible = false;
                    base.WkfRefID = 0;
                    txtSCno.Text = string.Empty;
                    //ddlInvoiceType.Enabled = false;
                    ddlStatus.SelectedIndex = 0;
                    FileDetailsList = null;
                    SOInvoiceUploadList = null;
                    anchorFile.Visible = false;
                    hdfSaveWithGreaterInvAmount.Value = "0";
                    BindGrid(ControlsEnum.UPLOADEDFILES);
                    break;
                #endregion
                #region RESET FOR MODIFIED DO
                case ControlsEnum.RESETFORMODIFIEDDO:
                    decimal ResettedAmnt = 0;
                    txtHdrDiscount.Text = ResettedAmnt.ToString(hdfCurrencyFormat.Value);
                    txtHdrTotal.Text = ResettedAmnt.ToString(hdfCurrencyFormat.Value);
                    txtHdrTotal.ToolTip = ResettedAmnt.ToString(hdfCurrencyFormat.Value);
                    txtHdrBalBeforeVat.Text = ResettedAmnt.ToString(hdfCurrencyFormat.Value);
                    txtHdrBalBeforeVat.ToolTip = ResettedAmnt.ToString(hdfCurrencyFormat.Value);
                    txtHdrTax.Text = ResettedAmnt.ToString(hdfCurrencyFormat.Value);
                    txtShipping.Text = ResettedAmnt.ToString(hdfCurrencyFormat.Value);
                    txtPriceAdj.Text = ResettedAmnt.ToString(hdfCurrencyFormat.Value);
                    txtPriceAdj.ToolTip = ResettedAmnt.ToString(hdfCurrencyFormat.Value);
                    txtTotalExp.Text = ResettedAmnt.ToString(hdfCurrencyFormat.Value);
                    txtTotalDeductionExp.Text = ResettedAmnt.ToString(hdfCurrencyFormat.Value);
                    txtHdrNetTotal.Text = ResettedAmnt.ToString(hdfCurrencyFormat.Value);
                    txtHdrInvoiceTotal.Text = ResettedAmnt.ToString(hdfCurrencyFormat.Value);
                    txtHdrDeduction.Text = ResettedAmnt.ToString(hdfCurrencyFormat.Value);
                    txtDiscDeducted.Text = ResettedAmnt.ToString(hdfCurrencyFormat.Value);
                    txtdor.Text = ResettedAmnt.ToString(hdfCurrencyFormat.Value);
                    break;
                #endregion
                #region ADD ITEM
                case ControlsEnum.ADDITEM:
                    anchorFile.Visible = false;
                    vrfFileUpload.Enabled = true;
                    CurrSlNo = 0;
                    anchorFile.Attributes.Remove("onclick");
                    break;
                #endregion
                #region CHECKBOX TAX POPUP
                case ControlsEnum.TAXCHECKBOX:
                    chkSubTotal.Checked = false;
                    chkDiscount.Checked = false;
                    chkOtherCharges.Checked = true;
                    break;
                #endregion
            }
        }

        public double StringToFormula(string expression)
        {
            List<string> tokens = getTokens(expression);
            Stack<double> operandStack = new Stack<double>();
            Stack<string> operatorStack = new Stack<string>();
            int tokenIndex = 0;

            while (tokenIndex < tokens.Count)
            {
                string token = tokens[tokenIndex];
                if (token == "(")
                {
                    string subExpr = getSubExpression(tokens, ref tokenIndex);
                    operandStack.Push(StringToFormula(subExpr));
                    continue;
                }
                if (token == ")")
                {
                    throw new ArgumentException("Mis-matched parentheses in expression");
                }
                //If this is an operator  
                if (Array.IndexOf(_operators, token) >= 0)
                {
                    while (operatorStack.Count > 0 && Array.IndexOf(_operators, token) < Array.IndexOf(_operators, operatorStack.Peek()))
                    {
                        string op = operatorStack.Pop();
                        double arg2 = operandStack.Pop();
                        double arg1 = operandStack.Pop();
                        operandStack.Push(_operations[Array.IndexOf(_operators, op)](arg1, arg2));
                    }
                    operatorStack.Push(token);
                }
                else
                {
                    operandStack.Push(double.Parse(token));
                }
                tokenIndex += 1;
            }

            while (operatorStack.Count > 0)
            {
                string op = operatorStack.Pop();
                double arg2 = operandStack.Pop();
                double arg1 = operandStack.Pop();
                operandStack.Push(_operations[Array.IndexOf(_operators, op)](arg1, arg2));
            }
            return operandStack.Pop();
        }

        private string getSubExpression(List<string> tokens, ref int index)
        {
            StringBuilder subExpr = new StringBuilder();
            int parenlevels = 1;
            index += 1;
            while (index < tokens.Count && parenlevels > 0)
            {
                string token = tokens[index];
                if (tokens[index] == "(")
                {
                    parenlevels += 1;
                }

                if (tokens[index] == ")")
                {
                    parenlevels -= 1;
                }

                if (parenlevels > 0)
                {
                    subExpr.Append(token);
                }

                index += 1;
            }

            if ((parenlevels > 0))
            {
                throw new ArgumentException("Mis-matched parentheses in expression");
            }
            return subExpr.ToString();
        }

        private List<string> getTokens(string expression)
        {
            string operators = "()^*/+-";
            List<string> tokens = new List<string>();
            StringBuilder sb = new StringBuilder();

            foreach (char c in expression.Replace(" ", string.Empty))
            {
                if (operators.IndexOf(c) >= 0)
                {
                    if ((sb.Length > 0))
                    {
                        tokens.Add(sb.ToString());
                        sb.Length = 0;
                    }
                    tokens.Add(c.ToString());
                }
                else
                {
                    sb.Append(c);
                }
            }

            if ((sb.Length > 0))
            {
                tokens.Add(sb.ToString());
            }
            return tokens;
        }

        private void SetDetailTax(object sender)
        {
            TextBox txtAmount;
            TextBox txtDiscount;
            TextBox txtTax;
            TextBox txtTotal;

            TextBox txtRate;
            TextBox txtQuantity;

            Label lblOrderQuantity;
            Label lblInvQuantity;
            Label lblInvProformaQuantity;

            HiddenField hdfRRDPK;
            HiddenField hdfItemPK;
            HiddenField hdfSCPK;
            HiddenField hdfSODetailPK;

            double quantity;
            double rate;
            quantity = 0;
            rate = 0;

            double soQty = 0;
            double invdQty = 0;

            if (sender != null)
            {
                txtRate = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("txtRate") as TextBox);
                txtQuantity = sender as TextBox;
                lblOrderQuantity = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("lblOrderQuantity") as Label);
                lblInvQuantity = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("lblInvQuantity") as Label);
                lblInvProformaQuantity = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("lblInvProformaQuantity") as Label);

                txtAmount = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("txtAmount") as TextBox);
                if (txtRate != null && txtQuantity != null)
                {
                    if (double.TryParse(txtRate.Text, out rate) && double.TryParse(txtQuantity.Text, out quantity))
                    {
                        double.TryParse(lblOrderQuantity.Text, out soQty);
                        double.TryParse(lblInvQuantity.Text, out invdQty);
                        double.TryParse(lblInvProformaQuantity.Text, out invdQty);
                        //if (double.TryParse(lblOrderQuantity.Text, out soQty) && double.TryParse(lblInvQuantity.Text, out invdQty) && soQty - invdQty >= rate)
                        //{
                        if (txtAmount != null)
                        {
                            txtAmount.Text = txtAmount.ToolTip = (rate * quantity).ToString(hdfCurrencyFormat.Value);
                            txtDiscount = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("txtDiscount") as TextBox);
                            txtTax = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("txtTax") as TextBox);
                            txtTotal = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("txtTotal") as TextBox);

                            hdfRRDPK = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("hdfInvoiceDtlPK") as HiddenField);
                            hdfItemPK = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("hdfItemPK") as HiddenField);
                            hdfSCPK = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("hdfSOPK") as HiddenField);
                            hdfSODetailPK = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("hdfSODetailPK") as HiddenField);

                            if (hdfRRDPK != null && hdfItemPK != null)
                            {
                                SOInvoicePK = string.IsNullOrEmpty(hdfRRDPK.Value) ? 0 : Convert.ToInt32(hdfRRDPK.Value);
                                SelectedItemPK = string.IsNullOrEmpty(hdfItemPK.Value) ? 0 : Convert.ToInt32(hdfItemPK.Value);
                                ScPK = string.IsNullOrEmpty(hdfSCPK.Value) ? 0 : Convert.ToInt32(hdfSCPK.Value);
                                ScDetailsPK = string.IsNullOrEmpty(hdfSODetailPK.Value) ? 0 : Convert.ToInt32(hdfSODetailPK.Value);
                                SetDetailTax(txtAmount, txtDiscount, txtTax, txtTotal);
                            }
                        }
                        //}
                        //else
                        //{
                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Err_InvQty").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                        //}
                    }
                    else
                    {
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Err_Rate_Greater_Discount").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                    }
                }
            }
            else if (SOInvoicePK == 0 && SelectedItemPK == 0)
            {
                SOInvoiceHeaderSession = TempSOInvoiceHeaderSession;
                ////
                if (SOInvoiceHeaderSession != null)
                {
                    //invoiceHeaderMulObj.SOMainList.ForEach(s =>
                    //    {
                    //SOInvoiceHeaderSession = s;//commented for handling multiple SC                         
                    foreach (GridViewRow gvr in grdInvoice.Rows)
                    {
                        if (gvr.RowType == DataControlRowType.DataRow)
                        {
                            hdfRRDPK = (gvr.FindControl("hdfInvoiceDtlPK") as HiddenField);
                            hdfItemPK = (gvr.FindControl("hdfItemPK") as HiddenField);
                            hdfSCPK = (gvr.FindControl("hdfSOPK") as HiddenField);
                            hdfSODetailPK = (gvr.FindControl("hdfSODetailPK") as HiddenField);
                            SOInvoicePK = Convert.ToInt32(hdfRRDPK.Value);
                            SelectedItemPK = Convert.ToInt32(hdfItemPK.Value);
                            ScPK = Convert.ToInt32(hdfSCPK.Value);
                            ScDetailsPK = Convert.ToInt32(hdfSODetailPK.Value);

                            txtAmount = (gvr.FindControl("txtAmount") as TextBox);
                            txtDiscount = (gvr.FindControl("txtDiscount") as TextBox);
                            txtTax = (gvr.FindControl("txtTax") as TextBox);
                            txtTotal = (gvr.FindControl("txtTotal") as TextBox);
                            SetDetailTax(txtAmount, txtDiscount, txtTax, txtTotal);
                        }
                    }
                    //});

                }
                SOInvoicePK = 0;
                SelectedItemPK = 0;
            }
            else
            {
                SOInvoiceHeaderSession = TempSOInvoiceHeaderSession;
                foreach (GridViewRow gvr in grdInvoice.Rows)
                {
                    if (gvr.RowType == DataControlRowType.DataRow)
                    {
                        hdfRRDPK = (gvr.FindControl("hdfInvoiceDtlPK") as HiddenField);
                        hdfItemPK = (gvr.FindControl("hdfItemPK") as HiddenField);
                        if (SOInvoicePK == Convert.ToInt32(hdfRRDPK.Value) && SelectedItemPK == Convert.ToInt32(hdfItemPK.Value))
                        {
                            txtAmount = (gvr.FindControl("txtAmount") as TextBox);
                            txtDiscount = (gvr.FindControl("txtDiscount") as TextBox);
                            txtTax = (gvr.FindControl("txtTax") as TextBox);
                            txtTotal = (gvr.FindControl("txtTotal") as TextBox);
                            if (!SetDetailTax(txtAmount, txtDiscount, txtTax, txtTotal))
                                return;
                        }
                    }
                }
            }
            SetSubTotal();
        }
        private bool SetDetailTax(TextBox txtAmount, TextBox txtDiscount, TextBox txtTax, TextBox txtTotal)
        {
            double amount;
            double discount;
            double OrderQty=0;
            double Rate = 0;
            double InvoiceNow = 0;
            double CurrentDiscount = 0;


            double itmTax;
            double netAmount;
            amount = 0;
            discount = 0;
            netAmount = 0;
            itmTax = 0;
            if (txtAmount != null && txtDiscount != null && txtTax != null && txtTotal != null)
            {
                Double.TryParse(txtAmount.Text.Trim(), out amount);
                if (amount >= 0)
                {
                    if (SOInvoiceHeaderSession != null)
                    {
                        invoiceHeaderObj = SOInvoiceHeaderSession;
                        //soInvoiceDetailsObj = invoiceHeaderObj.OrderDetail.SingleOrDefault(rfq => rfq.CID_PK == SOInvoicePK && rfq.CID_ITEM == SelectedItemPK);
                        soInvoiceDetailsObj = invoiceHeaderObj.OrderDetail.SingleOrDefault(rfq => rfq.CID_PK == SOInvoicePK && rfq.CID_ITEM == SelectedItemPK && Convert.ToInt32(rfq.CID_SO) == ScPK && Convert.ToInt32(rfq.CID_SO_DTL) == ScDetailsPK);
                        if (soInvoiceDetailsObj != null)
                        {
                            if (CurrPK == 0)
                            {
                                var discDetail = soInvoiceDetailsObj.TaxDtl.Where(rfq => rfq.CIT_TAX_CATEGORY == ((int)TaxType.Discount));
                                foreach (DirectSOInvoiceTaxHdr rfqTaxHdrObj in discDetail)
                                {
                                    #region Discount Calculation

                                    OrderQty = soInvoiceDetailsObj.CID_SOD_QTY;
                                    Rate = soInvoiceDetailsObj.CID_RATE;
                                    InvoiceNow = soInvoiceDetailsObj.CID_INV_QTY_NOW;
                                    discount = rfqTaxHdrObj.CIT_TAX_AMT;
                                    discount = (discount / OrderQty) * InvoiceNow;
                                    rfqTaxHdrObj.CIT_TAX_AMT = discount;
                                    #endregion

                                    //string taxFormula = rfqTaxHdrObj.CIT_TAX_FORMULA;
                                    //if (!string.IsNullOrEmpty(taxFormula))
                                    //{


                                    //    //int isDeducted = 0;
                                    //    //if (invoiceHeaderObj.DeductionDetails != null || invoiceHeaderObj.DeductionDetails.Count > 0)
                                    //    //{
                                    //    //    //calculate tax if advance not deducted
                                    //    //    for (int i = 0; i < invoiceHeaderObj.DeductionDetails.Count; i++)
                                    //    //    {
                                    //    //        if (invoiceHeaderObj.DeductionDetails[i].IAD_DISC_AMOUNT > 0)
                                    //    //        {
                                    //    //            isDeducted = isDeducted + 1;
                                    //    //        }
                                    //    //    }
                                    //    //}
                                    //    //if (isDeducted <= 0)
                                    //    //{
                                    //    //    taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                                    //    //    rfqTaxHdrObj.CIT_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                    //    //}
                                    //}
                                }
                            }
                            discount = soInvoiceDetailsObj.TaxDtl.Where(rfq => rfq.CIT_TAX_CATEGORY == ((int)TaxType.Discount)).Sum(rfq => rfq.CIT_TAX_AMT);

                            netAmount = amount - discount;
                            txtDiscount.ToolTip = txtDiscount.Text = discount.ToString(hdfCurrencyFormat.Value);

                            var taxDetail = soInvoiceDetailsObj.TaxDtl.Where(rfq => rfq.CIT_TAX_CATEGORY == ((int)TaxType.Tax));
                            foreach (DirectSOInvoiceTaxHdr rfqTaxHdrObj in taxDetail)
                            {
                                string taxFormula = rfqTaxHdrObj.CIT_TAX_FORMULA;
                                if (!string.IsNullOrEmpty(taxFormula))
                                {
                                    int isDeducted = 0;
                                    if (invoiceHeaderObj.DeductionDetails != null || invoiceHeaderObj.DeductionDetails.Count > 0)
                                    {
                                        //calculate tax if advance not deducted
                                        for (int i = 0; i < invoiceHeaderObj.DeductionDetails.Count; i++)
                                        {
                                            if (invoiceHeaderObj.DeductionDetails[i].IAD_TAX_AMOUNT > 0)
                                            {
                                                isDeducted = isDeducted + 1;
                                            }
                                        }
                                    }
                                    if (isDeducted <= 0)
                                    {
                                        taxFormula = taxFormula.Replace("#SUBTOTAL#", netAmount.ToString());
                                        rfqTaxHdrObj.CIT_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                    }
                                }
                            }
                            itmTax = soInvoiceDetailsObj.TaxDtl.ToList().Where(rfq => rfq.CIT_TAX_CATEGORY == ((int)TaxType.Tax)).Sum(rfq => rfq.CIT_TAX_AMT);
                            txtTax.ToolTip = txtTax.Text = itmTax.ToString(hdfCurrencyFormat.Value);
                            soInvoiceDetailsObj.CID_AMOUNT = amount;
                            soInvoiceDetailsObj.CID_DISCOUNT = discount;
                            soInvoiceDetailsObj.CID_TAX = itmTax;
                            soInvoiceDetailsObj.CID_NET_AMOUNT = amount - discount + itmTax;
                            txtTotal.ToolTip = txtTotal.Text = soInvoiceDetailsObj.CID_NET_AMOUNT.ToString(hdfCurrencyFormat.Value);
                            SOInvoiceHeaderSession = invoiceHeaderObj;
                        }
                    }
                    return true;
                }
                else
                {
                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Err_Rate_Greater_Discount").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        private bool ReSetHdrDisc()
        {
            if (SOInvoiceHeaderSession != null)
            {
                double amount;
                double discount;
                discount = 0;
                //reset Header Disc

                //Change for handling multiple SC
                invoiceHeaderObj = SOInvoiceHeaderSession;

                amount = SOInvoiceHeaderSession.OrderDetail.Sum(dtl => dtl.CID_NET_AMOUNT);
                discount = 0;
                var discHeader = invoiceHeaderObj.TaxHdr.Where(rfq => rfq.CIT_TAX_CATEGORY == ((int)TaxType.Discount));

                foreach (DirectSOInvoiceTaxHdr rfqTaxHdrObj in discHeader)
                {
                    string taxFormula = rfqTaxHdrObj.CIT_TAX_FORMULA;
                    if (!string.IsNullOrEmpty(taxFormula))
                    {

                        taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                        rfqTaxHdrObj.CIT_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);

                    }
                }
                discount = invoiceHeaderObj.TaxHdr.Where(rfq => rfq.CIT_TAX_CATEGORY == ((int)TaxType.Discount)).Sum(rfq => rfq.CIT_TAX_AMT);
                invoiceHeaderObj.ICH_DISCOUNT_TC = discount;

                txtHdrDiscount.ToolTip = txtHdrDiscount.Text = discount.ToString(hdfCurrencyFormat.Value);
                SOInvoiceHeaderSession = invoiceHeaderObj;
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool ReSetDetailTax(double amount)
        {

            double discount;
            double itmTax;
            double netAmount;
            //amount = 0;
            discount = 0;
            netAmount = 0;
            itmTax = 0;
            if (amount > 0)
            {

                if (amount >= 0)
                {
                    if (SOInvoiceHeaderSession != null)
                    {

                        invoiceHeaderObj = SOInvoiceHeaderSession;


                        //reset Line Item Disc & Tax


                        //soInvoiceDetailsObj = invoiceHeaderObj.OrderDetail.SingleOrDefault(rfq => rfq.CID_PK == SOInvoicePK && rfq.CID_ITEM == SelectedItemPK);
                        //Change for handling multiple SC
                        soInvoiceDetailsObj = invoiceHeaderObj.OrderDetail.SingleOrDefault(rfq => rfq.CID_PK == SOInvoicePK && rfq.CID_ITEM == SelectedItemPK && Convert.ToInt32(rfq.CID_SO_DTL) == ScDetailsPK);
                        if (soInvoiceDetailsObj != null)
                        {
                            var discDetail = soInvoiceDetailsObj.TaxDtl.Where(rfq => rfq.CIT_TAX_CATEGORY == ((int)TaxType.Discount));
                            foreach (DirectSOInvoiceTaxHdr rfqTaxHdrObj in discDetail)
                            {
                                string taxFormula = rfqTaxHdrObj.CIT_TAX_FORMULA;
                                if (!string.IsNullOrEmpty(taxFormula))
                                {
                                    taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                                    rfqTaxHdrObj.CIT_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                }
                            }
                            discount = soInvoiceDetailsObj.TaxDtl.Where(rfq => rfq.CIT_TAX_CATEGORY == ((int)TaxType.Discount)).Sum(rfq => rfq.CIT_TAX_AMT);
                            netAmount = amount - discount;
                            //txtDiscount.ToolTip = txtDiscount.Text = discount.ToString(hdfCurrencyFormat.Value);

                            var taxDetail = soInvoiceDetailsObj.TaxDtl.Where(rfq => rfq.CIT_TAX_CATEGORY == ((int)TaxType.Tax));
                            foreach (DirectSOInvoiceTaxHdr rfqTaxHdrObj in taxDetail)
                            {
                                string taxFormula = rfqTaxHdrObj.CIT_TAX_FORMULA;
                                if (!string.IsNullOrEmpty(taxFormula))
                                {

                                    if (invoiceHeaderObj.DeductionDetails != null || invoiceHeaderObj.DeductionDetails.Count > 0)
                                    {

                                        taxFormula = taxFormula.Replace("#SUBTOTAL#", netAmount.ToString());
                                        rfqTaxHdrObj.CIT_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);

                                    }
                                }
                            }
                            itmTax = soInvoiceDetailsObj.TaxDtl.ToList().Where(rfq => rfq.CIT_TAX_CATEGORY == ((int)TaxType.Tax)).Sum(rfq => rfq.CIT_TAX_AMT);
                            soInvoiceDetailsObj.CID_AMOUNT = amount;
                            soInvoiceDetailsObj.CID_DISCOUNT = discount;
                            soInvoiceDetailsObj.CID_TAX = itmTax;
                            soInvoiceDetailsObj.CID_NET_AMOUNT = (amount - discount + itmTax);



                            SOInvoiceHeaderSession = invoiceHeaderObj;
                        }


                    }
                    return true;
                }
                else
                {
                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Err_Rate_Greater_Discount").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        private void SetSaleorderTypeVisibility()
        {
            int CurrencyPK = 0;
            int.TryParse(hdfCurrency.Value, out CurrencyPK);
            hdfSalOrderType.Value = SaleOrderType.ToString();
            if (SaleOrderType == 1)//1.Domestic,2.Export
            {
                trTotalDedExp.Visible = false;
                trTotalExp.Visible = false;
                trdor.Visible = true;
                trDiscDeducted.Visible = true;
                trDeduction.Visible = true;
                chkDedAll.Visible = false;
                if (!IsAdvInvHasTax)
                {
                    trTotalDedExp.Visible = true;
                    trTotalExp.Visible = true;
                    trdor.Visible = false;
                    trDiscDeducted.Visible = false;
                    trDeduction.Visible = false;
                }
                if (CurrencyPK == currentUser.BaseCurrency) // if customer currency equal to base currency
                {
                    txtExchangeRate.Enabled = false; //In case of Domestic exchange rate cannot be editable
                }
                else
                {
                    txtExchangeRate.Enabled = true;
                }
            }
            else
            {
                trTotalDedExp.Visible = true;
                trTotalExp.Visible = true;

                trdor.Visible = false;
                trDiscDeducted.Visible = false;
                trDeduction.Visible = false;
                chkDedAll.Visible = true;
                txtExchangeRate.Enabled = true;
            }
        }

        private void SetTaxPayableDiv()
        {

            List<DirectSOInvoiceDetails> soInvoiceDetailsTaxPayableLst = new List<DirectSOInvoiceDetails>();
            DirectSOInvoiceHeader invoiceHeaderObjTaxPayable;
            List<DirectSOInvoiceTaxHdr> LstTaxPayable = new List<DirectSOInvoiceTaxHdr>();

            if (SOInvoiceHeaderSession != null)
            {
                invoiceHeaderObjTaxPayable = SOInvoiceHeaderSession;
                soInvoiceDetailsTaxPayableLst = invoiceHeaderObjTaxPayable.OrderDetail;
                //Adding Line Item Tax
                if (soInvoiceDetailsTaxPayableLst != null)
                {
                    foreach (var item in soInvoiceDetailsTaxPayableLst)
                    {
                        double taxapplyAmount = 0;
                        taxapplyAmount = item.CID_AMOUNT - item.CID_DISCOUNT;
                        var taxDetail = item.TaxDtl.Where(rfq => rfq.CIT_TAX_CATEGORY == ((int)TaxType.Tax));
                        foreach (DirectSOInvoiceTaxHdr rfqTaxHdrObj in taxDetail)
                        {
                            //assign taxapplyamount to CIT_TAX_CID_AMOUNT for only showing in grid under subtotal heading
                            rfqTaxHdrObj.CIT_TAX_CID_AMOUNT = taxapplyAmount.ToString(hdfCurrencyFormat.Value);
                            LstTaxPayable.Add(rfqTaxHdrObj);
                        }
                    }

                }
                //Adding Header tax
                var HeaderTax = invoiceHeaderObjTaxPayable.TaxHdr.Where(rfq => rfq.CIT_TAX_CATEGORY == ((int)TaxType.Tax));
                if (HeaderTax != null)
                {
                    string taxapplyAmount = "";
                    double OtherCharge = string.IsNullOrEmpty(txtShipping.Text.Trim()) ? 0 : Convert.ToDouble(txtShipping.Text.Trim());
                    if (IsTaxForOtherCharge)  //Is Othercharge is need for Tax Calulation
                    {
                        double HdrBalBeforeVat;

                        double OtherChargeDeduct = string.IsNullOrEmpty(txtdor.Text.Trim()) ? 0 : Convert.ToDouble(txtdor.Text.Trim());
                        HdrBalBeforeVat = string.IsNullOrEmpty(txtHdrBalBeforeVat.Text.Trim()) ? 0 : Convert.ToDouble(txtHdrBalBeforeVat.Text) + (OtherCharge - OtherChargeDeduct);
                        taxapplyAmount = HdrBalBeforeVat.ToString(hdfCurrencyFormat.Value);
                    }
                    else
                    {
                        taxapplyAmount = string.IsNullOrEmpty(txtHdrBalBeforeVat.Text.Trim()) ? ((double)0).ToString(hdfCurrencyFormat.Value) : Convert.ToDouble(txtHdrBalBeforeVat.Text).ToString(hdfCurrencyFormat.Value);
                    }

                    if (!IsAdvInvHasTax)
                    {
                        double GrossAmnt = 0;
                        double.TryParse(txtHdrTotal.Text, out GrossAmnt);
                        taxapplyAmount = (GrossAmnt + OtherCharge).ToString();
                    }
                    foreach (DirectSOInvoiceTaxHdr rfqTaxHdrObj in HeaderTax)
                    {
                        rfqTaxHdrObj.CIT_TAX_CID_AMOUNT = taxapplyAmount;
                        LstTaxPayable.Add(rfqTaxHdrObj);
                    }
                }

            }

            if (LstTaxPayable.Count > 0)
            {
                var groupedTaxPayableList = LstTaxPayable.GroupBy(f => f.CIT_TAX)
                    .Select(grp => new DirectSOInvoiceTaxHdr
                    {
                        CIT_TAX_AMT = grp.Sum(p => p.CIT_TAX_AMT),
                        CIT_NAME = grp.Min(p => p.CIT_NAME),
                        CIT_TAX_TEXT = grp.Min(p => p.CIT_TAX_TEXT),
                        CIT_PK = grp.Min(p => p.CIT_PK),
                        CIT_TAX_CODE = grp.Min(p => p.CIT_TAX_CODE),
                        CIT_TAX_RATE = grp.Min(p => p.CIT_TAX_RATE),
                        CIT_TAX_CID_AMOUNT = grp.Sum(p => Convert.ToDecimal(p.CIT_TAX_CID_AMOUNT)).ToString()

                    })
                   .ToList();

                //Removing Items have Zero taxAmount
                groupedTaxPayableList = groupedTaxPayableList.Where(f => f.CIT_TAX_AMT > 0).ToList();

                grdTaxPayable.DataSource = groupedTaxPayableList;
                grdTaxPayable.DataBind();
            }
            else
            {
                grdTaxPayable.DataSource = null;
                grdTaxPayable.DataBind();
            }
        }


        private void SetSubTotal()
        {
            TextBox txtSubTotalFooter;
            TextBox txtSubTotalFooterInvNow;
            TextBox txtSubTotalFooterDiscount;
            TextBox txtSubTotalFooterTax;
            TextBox txtSubTotalFooterAmount;
            double discountTC = 0;
            double taxTC = 0;
            double amountRcvdTC = 0;
            double amountAdvDedTC = 0;

            if (grdInvoice.FooterRow != null)
            {
                txtSubTotalFooter = grdInvoice.FooterRow.FindControl("txtSubTotalFooter") as TextBox;
                txtSubTotalFooterInvNow = grdInvoice.FooterRow.FindControl("txtSubTotalFooterInvNow") as TextBox;
                txtSubTotalFooterAmount = grdInvoice.FooterRow.FindControl("txtSubTotalFooterAmount") as TextBox;
                txtSubTotalFooterDiscount = grdInvoice.FooterRow.FindControl("txtSubTotalFooterDiscount") as TextBox;
                txtSubTotalFooterTax = grdInvoice.FooterRow.FindControl("txtSubTotalFooterTax") as TextBox;
                if (txtSubTotalFooter != null && SOInvoiceHeaderSession != null)
                {
                    if (SOInvoiceHeaderSession != null && SOInvoiceHeaderSession.OrderDetail.Count > 0)
                    {
                        SOInvoiceHeaderSession.ICH_AMOUNT_TC = SOInvoiceHeaderSession.OrderDetail.Where(Dt => Dt.CID_QTY_DO_DISPATCHED > 0 || DOCancelStatus == 1).Sum(dtl => dtl.CID_NET_AMOUNT);

                        discountTC = SOInvoiceHeaderSession.OrderDetail.Where(Dt => Dt.CID_QTY_DO_DISPATCHED > 0 || DOCancelStatus == 1).Sum(dtl => dtl.CID_DISCOUNT);
                        taxTC = SOInvoiceHeaderSession.OrderDetail.Where(Dt => Dt.CID_QTY_DO_DISPATCHED > 0 || DOCancelStatus == 1).Sum(dtl => dtl.CID_TAX);
                        amountRcvdTC = SOInvoiceHeaderSession.OrderDetail.Where(Dt => Dt.CID_QTY_DO_DISPATCHED > 0 || DOCancelStatus == 1).Sum(dtl => dtl.CID_INV_QTY_NOW);
                        amountAdvDedTC = SOInvoiceHeaderSession.OrderDetail.Where(Dt => Dt.CID_QTY_DO_DISPATCHED > 0 || DOCancelStatus == 1).Sum(dtl => dtl.CID_AMOUNT);
                      
                        txtSubTotalFooter.ToolTip = txtSubTotalFooter.Text = SOInvoiceHeaderSession.ICH_AMOUNT_TC.ToString(hdfCurrencyFormat.Value);
                        txtSubTotalFooterInvNow.ToolTip = txtSubTotalFooterInvNow.Text = amountRcvdTC.ToString(hdfCurrencyFormat.Value);
                        txtSubTotalFooterAmount.ToolTip = txtSubTotalFooterAmount.Text = amountAdvDedTC.ToString(hdfCurrencyFormat.Value);
                        txtSubTotalFooterDiscount.ToolTip = txtSubTotalFooterDiscount.Text = discountTC.ToString(hdfCurrencyFormat.Value);
                        txtSubTotalFooterTax.ToolTip = txtSubTotalFooterTax.Text = taxTC.ToString(hdfCurrencyFormat.Value);
                    }
                    decimal subTotal = Convert.ToDecimal(txtSubTotalFooter.Text);
                    decimal subTotalInvNow = Convert.ToDecimal(txtSubTotalFooterInvNow.Text);
                    decimal subTotalDiscount = Convert.ToDecimal(txtSubTotalFooterDiscount.Text);
                    //decimal subTotalTax1 = Convert.ToDecimal(txtSubTotalFooterTax.Text);
                    decimal hdrDiscount = string.IsNullOrEmpty(txtHdrDiscount.Text) ? 0 : Convert.ToDecimal(txtHdrDiscount.Text);
                    double totalTax = 0;
                    double tax;
                    foreach (GridViewRow grdRow in grdInvoice.Rows)
                    {
                        TextBox txtTax = grdRow.FindControl("txtTax") as TextBox;
                        if (txtTax != null)
                        {
                            tax = 0;
                            Double.TryParse(txtTax.Text, out tax);
                            totalTax += tax;
                        }
                    }
                    //subTotal -= Convert.ToDecimal(totalTax);
                    txtHdrTotal.Text = (subTotal - hdrDiscount).ToString(hdfCurrencyFormat.Value);
                    txtHdrTotal.ToolTip = (subTotal - hdrDiscount).ToString(hdfCurrencyFormat.Value);
                    decimal hdrDeduction = 0;
                    if (SaleOrderType == 1 && IsAdvInvHasTax)
                    {
                        hdrDeduction = string.IsNullOrEmpty(txtHdrDeduction.Text) ? 0 : Convert.ToDecimal(txtHdrDeduction.Text);
                        txtHdrBalBeforeVat.Text = (((subTotal - hdrDiscount) - hdrDeduction) - totalAllocatedDiscount).ToString(hdfCurrencyFormat.Value);
                        txtHdrBalBeforeVat.ToolTip = (((subTotal - hdrDiscount) - hdrDeduction) - totalAllocatedDiscount).ToString(hdfCurrencyFormat.Value);
                    }
                    else
                    {
                        hdrDeduction = 0;// string.IsNullOrEmpty(txtTotalDeductionExp.Text) ? 0 : Convert.ToDecimal(txtTotalDeductionExp.Text);
                        txtHdrBalBeforeVat.Text = (((subTotal - hdrDiscount) - hdrDeduction)).ToString(hdfCurrencyFormat.Value);
                        txtHdrBalBeforeVat.ToolTip = (((subTotal - hdrDiscount) - hdrDeduction)).ToString(hdfCurrencyFormat.Value);
                    }
                }
            }
        }
        private void SetLineItemTax()
        {
            try
            {
                GetFieldValues(ControlsEnum.LINETAX);
                SetFieldValues(ControlsEnum.LINETAX);

            }
            catch (Exception)
            {

                throw;
            }

        }

        private bool SetHdrTax()
        {
            //TextBox txtSubTotal;
            double amount;
            double amountOcCalc;
            double discount;
            double taxAmt;
            double shipping;
            double adjust;
            double balBeforeVat;
            double otherCharges;

            amount = 0;
            amountOcCalc = 0;
            taxAmt = 0;
            shipping = 0;
            adjust = 0;
            balBeforeVat = 0;
            otherCharges = 0;

            if (SOInvoiceHeaderSession != null)
            {
                invoiceHeaderObj = SOInvoiceHeaderSession;
                if (invoiceHeaderObj != null)
                    //invoiceHeaderObj.SOMainList.ForEach(ot =>
                    // {
                    amount = amount + invoiceHeaderObj.ICH_AMOUNT_TC;
                //});

                discount = 0;
                var discHeader = invoiceHeaderObj.TaxHdr.Where(rfq => rfq.CIT_TAX_CATEGORY == ((int)TaxType.Discount));
                int isDeducted = 0;


                foreach (DirectSOInvoiceTaxHdr rfqTaxHdrObj in discHeader)
                {
                    //isHaveDiscount = 1;
                    string taxFormula = rfqTaxHdrObj.CIT_TAX_FORMULA;
                    if (!string.IsNullOrEmpty(taxFormula))
                    {
                        if (invoiceHeaderObj.DeductionDetails != null || invoiceHeaderObj.DeductionDetails.Count > 0)
                        {
                            //calculate tax if advance not deducted
                            for (int i = 0; i < invoiceHeaderObj.DeductionDetails.Count; i++)
                            {
                                if (invoiceHeaderObj.DeductionDetails[i].IAD_TAX_AMOUNT > 0 || invoiceHeaderObj.DeductionDetails[i].IAD_DISC_AMOUNT > 0)
                                {
                                    isDeducted = isDeducted + 1;
                                }
                            }
                        }
                        if (isDeducted <= 0)
                        {

                            taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                            rfqTaxHdrObj.CIT_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                        }
                    }
                    else//In case of Custom tax & Discount it doesnot have formula. So we create a formula . 
                    {
                        //Formula :InvoiceNowDiscount=(TotalPODiscount/SubTotalPOAmount)*InvoiceNowAmount
                        if (TempInvoiceHeaderTemp != null)
                        {
                            double invoicenowDiscount = 0, TotalPODiscount = 0, SubTotalPOAmount = 0;
                            SubTotalPOAmount = TempInvoiceHeaderTemp.OrderDetail.Sum(dtl => dtl.CID_NET_AMOUNT);
                            //Commented for Wrong discount in Invoice if SC with mutiple discount(ie Custom discount and discount with formula(percentage)) 
                            //TotalPODiscount = TempInvoiceHeaderTemp.TaxHdr.Where(rfq => rfq.CIT_TAX_CATEGORY == ((int)TaxType.Discount)).Sum(rfq => rfq.CIT_TAX_AMT);
                            TotalPODiscount = TempInvoiceHeaderTemp.TaxHdr.Where(rfq => rfq.CIT_TAX_CATEGORY == ((int)TaxType.Discount) && string.IsNullOrEmpty(rfq.CIT_TAX_FORMULA)).Sum(rfq => rfq.CIT_TAX_AMT);
                            if (TotalPODiscount > 0)
                            {
                                //invoicenowDiscount = (TotalPODiscount / SubTotalPOAmount) * amount; //commented for wrog discount : If despatch qty is less than SC qty and discount type is custom.                               
                                invoicenowDiscount = (SCTotalDiscountAmount / SCSubTotalAmount) * amount;
                                rfqTaxHdrObj.CIT_TAX_AMT = Math.Round(invoicenowDiscount, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1);
                            }
                        }
                    }

                }
                discount = invoiceHeaderObj.TaxHdr.Where(rfq => rfq.CIT_TAX_CATEGORY == ((int)TaxType.Discount)).Sum(rfq => rfq.CIT_TAX_AMT);
                invoiceHeaderObj.ICH_DISCOUNT_TC = discount;
                //if (isDeducted <= 0)
                //{
                if ((TotalHDRDiscount > 0) || (isDeducted <= 0))
                {
                    txtHdrDiscount.ToolTip = txtHdrDiscount.Text = discount.ToString(hdfCurrencyFormat.Value);
                    //txtHdrDiscount.Text = txtHdrDiscount.ToolTip = (Convert.ToDecimal(txtHdrDiscount.Text) - TotalHDRDiscount).ToString(hdfCurrencyFormat.Value);
                    txtHdrDiscount.Text = txtHdrDiscount.ToolTip = ((Convert.ToDecimal(txtHdrDiscount.Text) - TotalHDRDiscount) < 0 ? 0 : (Convert.ToDecimal(txtHdrDiscount.Text) - TotalHDRDiscount)).ToString(hdfCurrencyFormat.Value);
                    //discHeader.All(di => di.CIT_TAX_AMT == Convert.ToDouble(txtHdrDiscount.Text));
                    if (isHaveDiscount == 1)
                    {
                        List<DirectSOInvoiceTaxHdr> LstHeaderDiscount = new List<DirectSOInvoiceTaxHdr>();
                        LstHeaderDiscount = invoiceHeaderObj.TaxHdr.Where(rfq => rfq.CIT_TAX_CATEGORY == ((int)TaxType.Discount)).ToList();
                        if (LstHeaderDiscount != null && LstHeaderDiscount.Count > 0)
                            invoiceHeaderObj.TaxHdr.LastOrDefault(rfq => rfq.CIT_TAX_CATEGORY == ((int)TaxType.Discount)).CIT_TAX_AMT = Convert.ToDouble(txtHdrDiscount.Text);
                    }
                }
                amountOcCalc = amount;
                //}
                amount = amount - discount;

                #region Other Charge

                // To handle Other charges against multiple PO   
                // Total Other Charge = SUM(All SC other charge amount) - SUM(All SC invoiced other charge amount)

                if (CurrPK > 0) // Edit mode
                {
                    shipping = Convert.ToDouble(invoiceHeaderObj.TaxHdr.Where(quotation => quotation.CIT_TAX_CATEGORY == ((int)TaxType.Shipping)).Sum(chrg => chrg.CIT_TAX_AMT));
                }
                else // New mode
                {
                    if (hdfOtherCharge.Value == "1")
                    {
                        shipping = Convert.ToDouble(invoiceHeaderObj.TaxHdr.Where(quotation => quotation.CIT_TAX_CATEGORY == ((int)TaxType.Shipping)).Sum(chrg => chrg.CIT_TAX_AMT));
                    }
                    else
                    {
                        shipping = Convert.ToDouble(invoiceHeaderObj.TaxHdr.Where(quotation => quotation.CIT_TAX_CATEGORY == ((int)TaxType.Shipping)).Sum(chrg => chrg.CIT_TAX_AMT)) - Convert.ToDouble(invoiceHeaderObj.TaxHdr.Where(quotation => quotation.CIT_TAX_CATEGORY == ((int)TaxType.Shipping)).Sum(chrg => chrg.CIT_INV_TAX_AMT));
                    }
                }
                shipping = invoiceHeaderObj.ICH_SHIP_CHARGE = invoiceHeaderObj.TaxHdr.Where(quotation => quotation.CIT_TAX_CATEGORY == ((int)TaxType.Shipping)).Sum(quotation => quotation.CIT_TAX_AMT);

                #endregion

                txtShipping.Text = txtShipping.ToolTip = shipping.ToString(hdfCurrencyFormat.Value);
                decimal subTotal = 0;
                //if (invoiceHeaderMulObj != null)
                //    invoiceHeaderMulObj.SOMainList.ForEach(ot =>
                //{
                subTotal = subTotal + Convert.ToDecimal(invoiceHeaderObj.ICH_AMOUNT_TC);
                //});

                //decimal subTotal = Convert.ToDecimal(invoiceHeaderObj.ICH_AMOUNT_TC);
                double totalTax = 0;
                double tax;
                foreach (GridViewRow grdRow in grdInvoice.Rows)
                {
                    TextBox txtTax = grdRow.FindControl("txtTax") as TextBox;
                    if (txtTax != null)
                    {
                        tax = 0;
                        Double.TryParse(txtTax.Text, out tax);
                        totalTax += tax;
                    }
                }
                // subTotal -= Convert.ToDecimal(totalTax);
                decimal hdrDeduction;
                if (SaleOrderType == 1 && IsAdvInvHasTax)
                {
                    hdrDeduction = string.IsNullOrEmpty(txtHdrDeduction.Text) ? 0 : Convert.ToDecimal(txtHdrDeduction.Text);
                    txtHdrBalBeforeVat.Text = ((((subTotal - Convert.ToDecimal(txtHdrDiscount.Text)) - hdrDeduction) - totalAllocatedDiscount)).ToString(hdfCurrencyFormat.Value);
                    txtHdrBalBeforeVat.ToolTip = ((((subTotal - Convert.ToDecimal(txtHdrDiscount.Text)) - hdrDeduction) - totalAllocatedDiscount)).ToString(hdfCurrencyFormat.Value);
                }
                else
                {
                    decimal hdrDiscount = string.IsNullOrEmpty(txtHdrDiscount.Text) ? 0 : Convert.ToDecimal(txtHdrDiscount.Text);
                    hdrDeduction = 0;// string.IsNullOrEmpty(txtTotalDeductionExp.Text) ? 0 : Convert.ToDecimal(txtTotalDeductionExp.Text);
                    txtHdrBalBeforeVat.Text = (((subTotal - hdrDiscount) - hdrDeduction)).ToString(hdfCurrencyFormat.Value);
                    txtHdrBalBeforeVat.ToolTip = (((subTotal - hdrDiscount) - hdrDeduction)).ToString(hdfCurrencyFormat.Value);
                }
                txtHdrTotal.Text = (subTotal - Convert.ToDecimal(txtHdrDiscount.Text)).ToString(hdfCurrencyFormat.Value);
                txtHdrTotal.ToolTip = (subTotal - Convert.ToDecimal(txtHdrDiscount.Text)).ToString(hdfCurrencyFormat.Value);
                double.TryParse(txtHdrBalBeforeVat.Text, out balBeforeVat);

                amount = balBeforeVat;

                //double AdvDeduction = 0;
                //double.TryParse(txtHdrDeduction.Text, out AdvDeduction);
                //if (!IsAdvInvHasTax)
                //    amount = amount + AdvDeduction;

                double.TryParse(txtdor.Text, out otherCharges);
                if (IsTaxForOtherCharge)
                {
                    amount += string.IsNullOrEmpty(txtShipping.Text) ? 0.00 : (Convert.ToDouble(txtShipping.Text) - otherCharges);//Here Other Charge = (TotalOtherChrage- Allocated otherCharge);

                }
                ////

                var taxHeader = invoiceHeaderObj.TaxHdr.Where(rfq => rfq.CIT_TAX_CATEGORY == ((int)TaxType.Tax));
                if (Convert.ToInt16(hdfDetailTax.Value) == (int)TaxSettingEnum.BOTHHEADERITEM)
                {
                    #region BOTHHEADERITEM
                    foreach (DirectSOInvoiceTaxHdr rfqTaxHdrObj in taxHeader)
                    {
                        string taxFormula = rfqTaxHdrObj.CIT_TAX_FORMULA;
                        #region Tax Applicable amount setting

                        double hdrSubTotal = 0;
                        double hdrDisc = 0;
                        double hdrOtherCharge = 0;
                        amount = 0;

                        if (rfqTaxHdrObj.CIT_HAS_SUB_TOTAL == 1)
                        {
                            //TextBox txtSubTotalFooter=(TextBox)grdInvoice.FooterRow.FindControl("txtSubTotalFooter");
                            hdrSubTotal = invoiceHeaderObj.ICH_AMOUNT_TC;
                        }
                        if (rfqTaxHdrObj.CIT_HAS_DISCOUNT == 1)
                        {
                            hdrDisc = invoiceHeaderObj.ICH_DISCOUNT_TC;
                        }
                        if (rfqTaxHdrObj.CIT_HAS_OTHER_CHARGE == 1)
                        {
                            hdrOtherCharge = invoiceHeaderObj.ICH_SHIP_CHARGE;
                        }
                        if (hdrSubTotal == 0)
                            amount = hdrDisc + hdrOtherCharge;
                        else
                            amount = (hdrSubTotal - hdrDisc) + hdrOtherCharge;

                        #endregion
                        if (!string.IsNullOrEmpty(taxFormula))
                        {
                            // taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                            taxFormula = taxFormula.Replace("#SUBTOTAL#", amount > 0 ? amount.ToString() : "0");
                            rfqTaxHdrObj.CIT_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                        }
                        else//In case of Custom tax & Discount it doesnot have formula. So we create a formula. 
                        {
                            //Formula :InvoiceNowTax=(TotalPOTax/(SubTotalPOAmount-Discount))*InvoiceNowAmount
                            if (TempInvoiceHeaderTemp != null)
                            {
                                double invoicenowTax = 0, TotalPOTax = 0, SubTotalPOAmount = 0, balancePOAmount = 0;
                                SubTotalPOAmount = TempInvoiceHeaderTemp.OrderDetail.Sum(dtl => dtl.CID_NET_AMOUNT);
                                balancePOAmount = SubTotalPOAmount - TempInvoiceHeaderTemp.ICH_DISCOUNT_TC;
                                TotalPOTax = TempInvoiceHeaderTemp.TaxHdr.Where(rfq => rfq.CIT_TAX_CATEGORY == ((int)TaxType.Tax)).Sum(rfq => rfq.CIT_TAX_AMT);
                                if (TotalPOTax > 0)
                                {
                                    invoicenowTax = (TotalPOTax / balancePOAmount) * amount;
                                    //Commented for RBPL rounding issue
                                    //rfqTaxHdrObj.CIT_TAX_AMT = Math.Round(invoicenowTax, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1);
                                    rfqTaxHdrObj.CIT_TAX_AMT = Math.Round(invoicenowTax, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                }
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    foreach (DirectSOInvoiceTaxHdr rfqTaxHdrObj in taxHeader)
                    {
                        string taxFormula = rfqTaxHdrObj.CIT_TAX_FORMULA;
                        if (!string.IsNullOrEmpty(taxFormula))
                        {
                            // taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                            taxFormula = taxFormula.Replace("#SUBTOTAL#", amount > 0 ? amount.ToString() : "0");
                            rfqTaxHdrObj.CIT_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                        }
                        else//In case of Custom tax & Discount it doesnot have formula. So we create a formula. 
                        {
                            //Formula :InvoiceNowTax=(TotalPOTax/(SubTotalPOAmount-Discount))*InvoiceNowAmount
                            if (TempInvoiceHeaderTemp != null)
                            {
                                double invoicenowTax = 0, TotalPOTax = 0, SubTotalPOAmount = 0, balancePOAmount = 0;
                                SubTotalPOAmount = TempInvoiceHeaderTemp.OrderDetail.Sum(dtl => dtl.CID_NET_AMOUNT);
                                balancePOAmount = SubTotalPOAmount - TempInvoiceHeaderTemp.ICH_DISCOUNT_TC;
                                TotalPOTax = TempInvoiceHeaderTemp.TaxHdr.Where(rfq => rfq.CIT_TAX_CATEGORY == ((int)TaxType.Tax)).Sum(rfq => rfq.CIT_TAX_AMT);
                                if (TotalPOTax > 0)
                                {
                                    invoicenowTax = (TotalPOTax / balancePOAmount) * amount;
                                    rfqTaxHdrObj.CIT_TAX_AMT = Math.Round(invoicenowTax, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1);
                                }
                            }
                        }
                    }
                }
                taxAmt = invoiceHeaderObj.TaxHdr.Where(rfq => rfq.CIT_TAX_CATEGORY == ((int)TaxType.Tax)).Sum(rfq => rfq.CIT_TAX_AMT);
                //amountOcCalc = discount + amountOcCalc + taxAmt + invoiceHeaderObj.ICH_AMOUNT_ADJUST;
                //Other charges

                invoiceHeaderObj.ICH_TAX_TC = invoiceHeaderObj.TaxHdr.Where(rfq => rfq.CIT_TAX_CATEGORY == ((int)TaxType.Tax)).Sum(rfq => rfq.CIT_TAX_AMT);
                txtHdrTax.ToolTip = txtHdrTax.Text = invoiceHeaderObj.ICH_TAX_TC.ToString(hdfCurrencyFormat.Value);
                //double.TryParse(txtShipping.Text, out shipping);
                //invoiceHeaderObj.ICH_SHIP_CHARGE = shipping;

                double.TryParse(txtPriceAdj.Text, out adjust);
                invoiceHeaderObj.ICH_AMOUNT_ADJUST = adjust;
                invoiceHeaderObj.ICH_AMOUNT_NET_TC = balBeforeVat + invoiceHeaderObj.ICH_TAX_TC + invoiceHeaderObj.ICH_SHIP_CHARGE + invoiceHeaderObj.ICH_AMOUNT_ADJUST - otherCharges;
                invoiceHeaderObj.ICH_NET_VALUE_TC = Convert.ToInt16(ddlInvoiceType.SelectedValue) > 0 ? Convert.ToByte(ddlInvoiceType.SelectedValue) == Convert.ToByte(SalesInvoiceType.Domestic) ? invoiceHeaderObj.ICH_AMOUNT_NET_TC : (invoiceHeaderObj.ICH_AMOUNT_NET_TC +
                    (Convert.ToByte(ddlInvoiceType.SelectedValue) == Convert.ToByte(SalesInvoiceType.Export) ? (double)hdrDeduction : 0) + otherCharges) : invoiceHeaderObj.ICH_NET_VALUE_TC;
                //invoiceHeaderObj.ICH_AMOUNT_NET_TC = invoiceHeaderObj.ICH_AMOUNT_TC - invoiceHeaderObj.ICH_DISCOUNT_TC + invoiceHeaderObj.ICH_TAX_TC
                //    + invoiceHeaderObj.ICH_SHIP_CHARGE + invoiceHeaderObj.ICH_AMOUNT_ADJUST;
                txtHdrNetTotal.ToolTip = txtHdrNetTotal.Text = invoiceHeaderObj.ICH_AMOUNT_NET_TC.ToString(hdfCurrencyFormat.Value);
                if (SaleOrderType == 1 && IsAdvInvHasTax)
                {
                    txtTotalExp.ToolTip = txtTotalExp.Text = invoiceHeaderObj.ICH_AMOUNT_NET_TC.ToString(hdfCurrencyFormat.Value);
                }
                else
                {
                    txtTotalExp.ToolTip = txtTotalExp.Text = invoiceHeaderObj.ICH_AMOUNT_NET_TC.ToString(hdfCurrencyFormat.Value);
                    txtHdrNetTotal.ToolTip = txtHdrNetTotal.Text = (Convert.ToDouble(txtTotalExp.Text) - Convert.ToDouble(txtTotalDeductionExp.Text)).ToString(hdfCurrencyFormat.Value);
                }
                //Invoive Total
                decimal headerTotal = string.IsNullOrEmpty(txtHdrTotal.Text) ? 0 : Convert.ToDecimal(txtHdrTotal.Text);
                decimal headerTax = string.IsNullOrEmpty(txtHdrTax.Text) ? 0 : Convert.ToDecimal(txtHdrTax.Text);
                decimal Shipping = string.IsNullOrEmpty(txtShipping.Text) ? 0 : Convert.ToDecimal(txtShipping.Text);
                decimal PriceAdj = string.IsNullOrEmpty(txtPriceAdj.Text) ? 0 : Convert.ToDecimal(txtPriceAdj.Text);
                txtHdrInvoiceTotal.ToolTip = txtHdrInvoiceTotal.Text = (headerTotal + headerTax + Shipping + PriceAdj).ToString();

                //if domestic show nettotal in Invoice Total  (as per manoj sir :while testing 'LOCAL SALE +ADVANCE RECEIVE 100%')
                if (ddlInvoiceType.SelectedValue == "1" && IsAdvInvHasTax)
                {
                    txtHdrInvoiceTotal.Text = txtHdrNetTotal.Text;
                }

                if (TempSOInvoiceHeaderSessionCustAll != null)
                    TempSOInvoiceHeaderSessionCustAll.TaxHdr = invoiceHeaderObj.TaxHdr;

                SOInvoiceHeaderSession = invoiceHeaderObj;

            }
            return true;
        }


        /// <summary>
        /// Is Matcing Tax
        /// </summary>
        /// <param name="TaxList"></param>
        /// <param name="tax"></param>
        /// <returns></returns>
        private bool IsMatcingTax(List<decimal> TaxList, decimal tax)
        {

            bool flag = true;
            bool BaseType = true;
            bool CurType = true;
            if (TaxList != null)
            {
                BaseType = tax > 0 ? true : false;
                foreach (long var in TaxList)
                {
                    CurType = var > 0 ? true : false;
                    if (BaseType != CurType)
                    {
                        flag = false;
                        break;
                    }
                }
            }
            return flag;
        }
        /// <summary>
        /// I exist Po in list
        /// </summary>
        /// <param name="lst"></param>
        /// <param name="pk"></param>
        /// <returns></returns>
        private bool IsExixtPk(List<long> lst, long pk)
        {
            bool flag = false;
            if (lst != null)
                foreach (long item in lst)
                    if (item == pk)
                    {
                        flag = true;
                        break;
                    }
            return flag;
        }
        private double CalculateTaxFormula(string taxFormula, double amount)
        {
            double taxAmt;
            taxAmt = 0;
            if (!string.IsNullOrEmpty(taxFormula))
            {
                taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                taxAmt = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
            }
            return taxAmt;
        }
        public string GetFormattedNumber(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfDecimalFormat.Value);
        }
        public string GetFormattedNumberWithComma(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfDecimalFormatWithSeperator.Value);
        }
        public string GetFormattedCurrency(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfCurrencyFormat.Value);
        }
        public string GetFormattedCurrencyWithComa(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfCurrencyFormatWithSeperator.Value);
        }
        public string GetFormattedRate(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfRateFormat.Value);
        }
        public string GetCeiledInteger(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return Math.Ceiling(num).ToString();
        }
        public bool IsValidInvoiceDate()
        {
            try
            {
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                int category = 1;
                // dtTaxDetails = BusinessLogic.Administration.Masters.TaxSettingsMaster.GetActiveTaxCategoryDateValue(category, currentUser.SBUID, Convert.ToByte(DbActiveStatus.ACTIVE), Convert.ToDateTime(txtInvoiceDate.Text), (int)TaxSubCategory.VATSale, TaxFilterType.SAL);
                dtTaxDetails = BusinessLogic.Administration.Masters.TaxSettingsMaster.GetActiveTaxCategoryDateValue(category, currentUser.SBUID, Convert.ToByte(DbActiveStatus.ACTIVE), Convert.ToDateTime(txtInvoiceDate.Text), 0, null, (int)TaxStatus.Exclude, 0, 1);
                if (dtTaxDetails != null && dtTaxDetails.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return true;
            }
        }

        /// <summary>
        /// Redirect to a page if user has permission on that page.
        /// </summary>
        /// <param name="RedirectUrl">Page Url</param>
        private void CheckUserRightsAndRedirect(string RedirectUrl)
        {
            CommonBL userAuth = new CommonBL();
            if (currentUser == null)
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            UserRightsBO UsrRights = userAuth.GetUserPageRights(currentUser.PKUser, RedirectUrl.Replace("~", ""), currentUser.SBUID, currentUser.CurrentDeptPK);
            if (UsrRights != null && UsrRights.Rights.Count() > 0 && UsrRights.Rights[0].UserDeptRight)
                Response.Redirect(RedirectUrl, false);
            else
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(Resources.ErpRes.NoPressionMsg) + "','" + Resources.ErpRes.Information + "');", true);
        }

        #region Grd Status maintains
        //For sett allocation details
        private void SetAllocationDetails()
        {
            if (Session[ERP.Utilities.SessionStrings.SelectedSalesInvoicesInfoLst] != null)
                SelectedSalesInvoicesInfoLst = (List<DirectSelectionInfo>)Session[ERP.Utilities.SessionStrings.SelectedSalesInvoicesInfoLst];
            else
                SelectedSalesInvoicesInfoLst = new List<DirectSelectionInfo>();
            CheckBox chbSelect;
            foreach (GridViewRow item in grdInvoiceList.Rows)
            {
                chbSelect = (CheckBox)item.FindControl("chkInvselect");
                DirectSelectionInfo objSaleOrderInfo = new DirectSelectionInfo();
                objSaleOrderInfo.chkChecked = false;
                objSaleOrderInfo.InvoicePK = Convert.ToInt32(grdInvoiceList.DataKeys[item.RowIndex].Value.ToString());
                if (chbSelect.Checked)
                {
                    objSaleOrderInfo.chkChecked = true;
                    objSaleOrderInfo.CustomerPK = Convert.ToInt32(((HiddenField)item.FindControl("hdfCustomerPK")).Value);//E
                    objSaleOrderInfo.CurrencyPK = Convert.ToInt32(((HiddenField)item.FindControl("hdfSOCurrency")).Value);//E
                    objSaleOrderInfo.ApprovedStatus = Convert.ToInt32(((HiddenField)item.FindControl("hdfApproved")).Value);
                    objSaleOrderInfo.IsPosted = Convert.ToBoolean(((HiddenField)item.FindControl("hdfPosted")).Value);
                    objSaleOrderInfo.Tax = Convert.ToDecimal(string.IsNullOrEmpty(((HiddenField)item.FindControl("hdfTaxAmount")).Value) ? "0" : ((HiddenField)item.FindControl("hdfTaxAmount")).Value);
                    LinkButton lbnBalAmt = item.FindControl("lbnBalAmt") as LinkButton;
                    objSaleOrderInfo.BalanceAmt = Convert.ToDecimal(lbnBalAmt.Text);

                    objSaleOrderInfo.InvoiceType = Convert.ToInt32(((HiddenField)item.FindControl("hdfInvType")).Value);
                    objSaleOrderInfo.IsCancelled = Convert.ToInt16(((HiddenField)item.FindControl("hdfDelStatus")).Value) == 1 ? true : false;
                }

                bool alreadyExists = SelectedSalesInvoicesInfoLst.Exists(itemLst => itemLst.InvoicePK == objSaleOrderInfo.InvoicePK);
                if (alreadyExists)
                    ChangeItem(objSaleOrderInfo);
                else
                    SelectedSalesInvoicesInfoLst.Add(objSaleOrderInfo);
            }

            Session[ERP.Utilities.SessionStrings.SelectedSalesInvoicesInfoLst] = SelectedSalesInvoicesInfoLst;


        }

        //for change the status of checked items
        private void ChangeItem(DirectSelectionInfo item)
        {
            if (SelectedSalesInvoicesInfoLst.Count > 0)
                foreach (var Items in SelectedSalesInvoicesInfoLst)
                    if (Items.InvoicePK == item.InvoicePK)
                    {
                        Items.chkChecked = item.chkChecked;
                        Items.CustomerPK = item.CustomerPK;
                        Items.CurrencyPK = item.CurrencyPK;
                        Items.ApprovedStatus = item.ApprovedStatus;
                        Items.IsPosted = item.IsPosted;
                        Items.Tax = item.Tax;
                        Items.BalanceAmt = item.BalanceAmt;
                        Items.InvoiceType = item.InvoiceType;
                        Items.IsCancelled = item.IsCancelled;
                    }
        }

        //For Reset grid status
        private void SetGridStatus()
        {
            if (Session[ERP.Utilities.SessionStrings.SelectedSalesInvoicesInfoLst] != null)
            {
                SelectedSalesInvoicesInfoLst = (List<DirectSelectionInfo>)Session[ERP.Utilities.SessionStrings.SelectedSalesInvoicesInfoLst];
                int sodPK;
                foreach (GridViewRow item in grdInvoiceList.Rows)
                {
                    sodPK = Convert.ToInt32(grdInvoiceList.DataKeys[item.RowIndex].Value.ToString());
                    if (SelectedSalesInvoicesInfoLst.Exists(itemLst => itemLst.InvoicePK == sodPK && itemLst.chkChecked == true))
                        ((CheckBox)item.FindControl("chkInvselect")).Checked = true;

                }
            }

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
            CommonService CommonServiceClient;
            CommonServiceClient = null;

            SalesInvoiceService salesInvoiceServiceClient;
            salesInvoiceServiceClient = null;
            try
            {

                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

                bool bIsChecked = false;
                int? result;
                int alertresult;
                string itemAmount;
                List<DirectSOInvoiceTaxHdr> tempInvTaxHdrSplit;
                DirectSOInvoiceTaxHdr tempInvTaxSplitObj = null;
                HiddenField hdfInvoiceDtlPK;
                HiddenField hdfItemPK;
                HiddenField hdfInOpeningInv;
                HiddenField hdfSODetailPK;
                TextBox txtAmount;
                TextBox txtTax;
                TextBox txtDiscount;
                TextBox txtSubTotal;
                TextBox txtSubTotalInvNow;
                

                string action;
                DropDownList ddlWkfAction;
                TextBox WrkfComments;

                double amount;
                double discount;
                int count;

                double totalAmt;
                double currentTotal;
                double taxAmt;
                bool isValidDisc = true;
                bool isContinue;
                int invType = 1;
                int InvoiceType = 1;
                FileInfo tempFileInfoObj;
                int selectedItemPK;
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
                    if (((DropDownList)sender).ID == "ddlPopupTaxType")
                    {
                        commonActions = ActionsEnum.TAXTYPECHANGED;
                    }
                    if (((DropDownList)sender).ID == "ddlInvoiceType")
                    {
                        commonActions = ActionsEnum.SALESINVOICETYPECHANGED;
                    }
                    if (((DropDownList)sender).ID == "ddlPaymentTerms")
                    {
                        commonActions = ActionsEnum.GETDUEDATE;
                    }
                    //CustomerTypes
                    if (((DropDownList)sender).ID == "ddlCustomerType")
                    {
                        commonActions = ActionsEnum.CUSTOMERTYPECHANGING;
                    }

                }
                else if (sender.GetType().IsEquivalentTo(typeof(TextBox)))
                {
                    if (((TextBox)sender).ID == "txtInvNow")
                    {
                        commonActions = ActionsEnum.CALCULATEDTLTAX;
                    }

                    if ((((TextBox)sender).ID == "txtInvoiceDate") || (((TextBox)sender).ID == "txtETD"))
                    {
                        commonActions = ActionsEnum.GETDUEDATE;
                    }
                    //ExchangeRate text changing event
                    else if (((TextBox)sender).ID == "txtExchangeRate")
                    {
                        commonActions = ActionsEnum.CHANGEEXRATE;
                    }
                }
                else if (sender.GetType().IsEquivalentTo(typeof(CheckBox)))
                {
                    if (((CheckBox)sender).ID == "chkDedAll")
                    {
                        commonActions = ActionsEnum.CHECKEDCHANGED;
                    }
                    else if (((CheckBox)sender).ID == "chkSubTotal" || ((CheckBox)sender).ID == "chkDiscount" || ((CheckBox)sender).ID == "chkOtherCharges")
                    {
                        commonActions = ActionsEnum.SETTAXAPPLICABLEAMOUNT;
                    }
                }
                else if (sender.GetType().IsEquivalentTo(typeof(ActionsEnum)))
                {

                    commonActions = ActionsEnum.CHECKEDCHANGED;
                }

                switch (commonActions)
                {
                    #region DTL SEARCH/CUSTOMERCHANGE
                    case ActionsEnum.DTLSEARCH:
                    case ActionsEnum.CUSTOMERCHANGE:
                        uclSOPaging.CurrentPage = 1;
                        PageIndexSO = CommonConstants.SELECT_VALUE_ONE;

                        custPK = Convert.ToInt32(hdfCustomer.Value);
                        hdfCurrCustomerPK.Value = custPK.ToString();
                        GetFieldValues(ControlsEnum.CUSTOMERTYPES);
                        SetFieldValues(ControlsEnum.CUSTOMERTYPES);

                        GetFieldValues(ControlsEnum.PAYMENTTERMS);
                        SetFieldValues(ControlsEnum.PAYMENTTERMS);

                        GetFieldValues(ControlsEnum.PEDINGSOLIST);
                        SetFieldValues(ControlsEnum.PEDINGSOLIST);
                        GetFieldValues(ControlsEnum.GETCUSTOMERDETAILSBYTYPE);
                        SetFieldValues(ControlsEnum.GETCUSTOMERDETAILSBYTYPE);

                        hdfIsPendingDOVisible.Value = "1";
                        break;
                    #endregion
                    #region DTL CLEAR SEARCH
                    case ActionsEnum.DTLCLEARSEARCH:
                        txtDONumber.Text = string.Empty;
                        hdfDPHPK.Value = string.Empty;
                        uclSOPaging.CurrentPage = 1;
                        PageIndexSO = CommonConstants.SELECT_VALUE_ONE;
                        GetFieldValues(ControlsEnum.PEDINGSOLIST);
                        SetFieldValues(ControlsEnum.PEDINGSOLIST);
                        hdfIsPendingDOVisible.Value = "1";

                        break;
                    #endregion
                    #region ADD TO LIST
                    case ActionsEnum.ADDTOLIST:
                        SoHeaderObj = new DirectSOHeaderBO();
                        SoHeaderObj.SOList = new List<DirectSOHeaderListBO>();
                        List<DirectSOHeaderListBO> objItemList = new List<DirectSOHeaderListBO>();
                        DirectSOHeaderListBO objSoList;
                        HiddenField hdfDOPk;
                        HiddenField hdfSOPk;
                        HiddenField hdfSOType = new HiddenField();
                        List<int> lstItemType = new List<int>();
                        foreach (GridViewRow grdrow in grdSoList.Rows)
                        {
                            CheckBox chkSCselect = (CheckBox)grdrow.FindControl("chkSoSelect");
                            if (chkSCselect.Checked)
                            {
                                hdfDOPk = (HiddenField)grdrow.FindControl("hdfDOPk");
                                hdfSOPk = (HiddenField)grdrow.FindControl("hdfSOPk");
                                hdfSOType = (HiddenField)grdrow.FindControl("hdfSOType");
                                objSoList = new DirectSOHeaderListBO();
                                objSoList.SOH_PK = Convert.ToInt32(hdfSOPk.Value);
                                objSoList.DPH_PK = Convert.ToInt32(hdfDOPk.Value);
                                if (objItemList != null && objItemList.Where(r => r.DPH_PK != Convert.ToInt32(hdfDOPk.Value)).Count() > 0)
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_Err_SC").ToString()) + "');", true);
                                    return;
                                }
                                objItemList.Add(objSoList);
                                lstItemType.Add(Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfProductType")).Value));
                            }
                        }
                        if (objItemList.Count == 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_NoRecordsSelected").ToString()) + "');", true);
                            return;
                        }
                        if (CheckAddedType(lstItemType))
                        {
                            SoHeaderObj.SOList = objItemList;
                            #region Validations
                            if (objItemList != null && objItemList.Count > 0 && SOInvoiceHeaderSession != null && SOInvoiceHeaderSession.OrderDetail != null)
                            {
                                if (objItemList.Where(r => r.DPH_PK != Convert.ToInt32(SOInvoiceHeaderSession.ICH_DESPATCH_HDR)).Count() > 0)
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_Err_SC").ToString()) + "');", true);
                                    return;
                                }
                                List<string> objSoPkList = SOInvoiceHeaderSession.OrderDetail.Select(r => r.CID_SO).Distinct().ToList();
                                SoHeaderObj.SOList = objItemList.Where(r => !objSoPkList.Contains(r.SOH_PK.ToString())).ToList();
                                //foreach (DirectSOHeaderListBO objSelectedSC in objItemList)
                                //{
                                //    if (SOInvoiceHeaderSession.OrderDetail.Where(r => Convert.ToInt32(r.CID_SO) == objSelectedSC.SOH_PK).Count() > 0)
                                //    {
                                //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_Err_SCAlreadyAdded").ToString()) + "');", true);
                                //        return;
                                //    }
                                //}
                            }

                            #endregion
                            SaleOrderType = string.IsNullOrEmpty(hdfSOType.Value) ? 0 : Convert.ToInt32(hdfSOType.Value);
                            hdfSaleOrderType.Value = SaleOrderType.ToString();
                            SetSaleorderTypeVisibility();


                            //EntryStatus = EntryStatus.ENTRYMODE;
                            GetFieldValues(ControlsEnum.INVOICETYPE);
                            SetFieldValues(ControlsEnum.INVOICETYPE);
                            if (SoHeaderObj != null && SoHeaderObj.SOList != null && SoHeaderObj.SOList.Count > 0)
                            {
                                DespatchID = SoHeaderObj.SOList.FirstOrDefault().DPH_PK;
                                //    //GetFieldValues(ControlsEnum.SOINVHEADER);
                                //    GetFieldValues(ControlsEnum.DELIVERYORDERDETAILS);
                                //    SetFieldValues(ControlsEnum.DELIVERYORDERDETAILS);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateInvNow", "$(document).ready(function () { CalculateInvNow(1);});", true);
                            }

                            GetFieldValues(ControlsEnum.SOINVHEADER);
                            //if (CurrPK == 0 && SOInvoiceHeaderSession != null && SOInvoiceHeaderSession.OrderDetail != null)
                            //{
                            //SOInvoiceHeaderSession.ICH_AMOUNT_TC = 0;
                            //SOInvoiceHeaderSession.ICH_TAX_TC = 0;
                            //SOInvoiceHeaderSession.ICH_DISCOUNT_TC = 0;
                            //// SOInvoiceHeaderSession.ICH_SHIP_CHARGE = 0;
                            ////SOInvoiceHeaderSession.ICH_AMOUNT_ADJUST = 0;
                            //SOInvoiceHeaderSession.ICH_AMOUNT_NET_TC = 0;
                            //SOInvoiceHeaderSession.ICH_AMOUNT_NET_BC = 0;
                            //    SOInvoiceHeaderSession.TaxHdr.ForEach(thd => thd.CIT_TAX_AMT = 0);
                            //invoiceHeaderMulObj.SOMainList.ForEach(s =>
                            //       {
                            //           foreach (DirectSOInvoiceDetails dtl in s.OrderDetail)
                            //           {
                            //               dtl.CID_INV_QTY_NOW = 0;
                            //               dtl.CID_AMOUNT = 0;
                            //               dtl.CID_TAX = 0;
                            //               dtl.CID_DISCOUNT = 0;
                            //               dtl.CID_NET_AMOUNT = 0;
                            //               //dtl.TaxDtl.ForEach(tdl => tdl.CIT_TAX_AMT = 0);
                            //               dtl.CID_QTY_INVOICED = dtl.CID_ORDERED_QTY - dtl.CID_INV_QTY;
                            //               //dtl.TaxDtl.ForEach(tdl => tdl.VTL_TAX_AMT = 0);
                            //               dtl.TaxDtl.ForEach(tdl =>
                            //               {
                            //                   if (tdl.CIT_TYPE == 1)
                            //                       tdl.CIT_TAX_AMT = 0;
                            //                   else
                            //                   {
                            //                       tdl.CIT_TAX_AMT = tdl.CIT_TAX_AMT / dtl.CID_ORDERED_QTY * dtl.CID_QTY_INVOICED;
                            //                   }
                            //               });
                            //           }
                            //       });
                            //}
                            SetFieldValues(ControlsEnum.SOINVHEADER);
                            GetFieldValues(ControlsEnum.TAXSETTINGS);
                            SetFieldValues(ControlsEnum.SOINVDETAIL);
                            SetFieldValues(ControlsEnum.UPLOADEDFILES);
                            GetFieldValues(ControlsEnum.GETDUEDATE);
                            SetFieldValues(ControlsEnum.GETDUEDATE);
                            TempSOInvoiceHeaderSession = SOInvoiceHeaderSession;
                            SetDetailTax(null);
                            SetHdrTax();
                            if (grdInvoice.Rows.Count > 0)
                            {
                                SetSubTotal();
                            }

                            //Settings of TaxPayableDiv
                            if (hdfIsTaxPayable.Value.ToString() == "1")
                            {
                                SetTaxPayableDiv();
                            }
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "TaxPayable", "$(document).ready(function () { ShowHideTaxPayableOuter();});", true);
                            //end
                            hdfIsPendingDOVisible.Value = "0";
                            SetFieldValues(ControlsEnum.PEDINGSOLIST);
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Err_ItemType_differ").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region SAVE
                    case ActionsEnum.SAVE:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else
                        {
                            if (grdInvoice.Rows.Count > 0)
                            {
                                isContinue = true;

                                if (ddlCustomerType.Items.Count <= 0)
                                {
                                    isContinue = false;

                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_ReqType").ToString()) + "');", true);
                                    return;
                                }

                                if (hdfIsJournalize.Value == "True")
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(Resources.Messages.Msg_Journalize) + "','" + Resources.Messages.Information + "');", true);
                                    return;
                                }
                                if ((hdfSaveWithoutAllocation.Value == "0") || (hdfSaveWithoutAllocation.Value == ""))
                                {
                                    TempSOInvoiceHeaderSession = SOInvoiceHeaderSession;
                                    deductionDtlList = new List<DirectSOAdvDeductionDetails>();
                                    //For Avoiding Zero allocate now
                                    //deductionDtlList = TempSOInvoiceHeaderSession.DeductionDetails.ToList();
                                    List<DirectSOAdvDeductionDetails> deductionDtlListNonZero = new List<DirectSOAdvDeductionDetails>();
                                    // deductionDtlListNonZero = TempSOInvoiceHeaderSession.DeductionDetails.ToList();

                                    //if (SaleOrderType == 2)
                                    //{
                                    //    hdfIsCusAllAdv.Value = CommonConstants.SELECT_VALUE_ONE;
                                    //    GetFieldValues(ControlsEnum.SOINVHEADER);
                                    //    deductionDtlListNonZero = TempSOInvoiceHeaderSessionCustAll.DeductionDetails.ToList();
                                    //}
                                    //else
                                    //{
                                    deductionDtlListNonZero = TempSOInvoiceHeaderSession.DeductionDetails.ToList();
                                    // }
                                    foreach (DirectSOAdvDeductionDetails itm in deductionDtlListNonZero)
                                    {
                                        //if (itm.IAD_AMOUNT > 0)
                                        //{
                                        deductionDtlList.Add(itm);
                                        //}
                                    }
                                    //End  
                                    if (SaleOrderType == 1 && IsAdvInvHasTax)
                                    {
                                        if (Convert.ToDouble(txtHdrDeduction.Text) <= 0 && (deductionDtlList != null && deductionDtlList.Count > 0))
                                        {
                                            isContinue = false;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop3", "ShowSaveWithoutAllocationConfirm('" + (sender as Button).ID + "');", true);
                                        }
                                    }
                                    else
                                    {
                                        if (Convert.ToDouble(txtTotalDeductionExp.Text) <= 0 && (deductionDtlList != null && deductionDtlList.Count > 0))
                                        {
                                            isContinue = false;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop3", "ShowSaveWithoutAllocationConfirm('" + (sender as Button).ID + "');", true);
                                        }
                                    }
                                }

                                if (isContinue)
                                {
                                    hasValidRate = false;
                                    invoiceHeaderObj = new DirectSOInvoiceHeader();
                                    invoiceHeaderObj = (DirectSOInvoiceHeader)SetUIValuesToObject(ControlsEnum.SOINVHEADER);
                                    invoiceHeaderObj.WKF_FLAG = 0;
                                    if (hasValidRate)
                                    {
                                        if (invoiceHeaderObj != null && invoiceHeaderObj.OrderDetail != null)
                                        {
                                            //if (!ValidateInvoice())
                                            //    return;
                                            invoiceHeaderObj.WKF_TRX_FLAG = Convert.ToInt32(WorkflowTransactionFlag.SAVE);
                                            string xmlDoc = CommonFunctions.XmlSerialize<DirectSOInvoiceHeader>(invoiceHeaderObj);
                                            // save Process Control inspection details
                                            string invNumber = string.Empty;
                                            result = BusinessLogic.Sales.SalesInvoiceBL.SaveDirectSalesInvoiceHeader(xmlDoc, out invNumber);
                                            if (result > 0) // Success !  redirect to listing page
                                            {
                                                #region Update dummy entry while modify invoice after approval
                                                if (Approved == (int)DbStatus.APPROVED) // Checking invoice stataus wheteher invoice approved or not
                                                {
                                                    FinTrxService finTrxServiceClient;
                                                    finTrxServiceClient = new FinTrxService();
                                                    string refType = string.Empty;
                                                    refType = ApplicationType.DSIJ;
                                                    long DummyResult = invoiceHeaderObj.ICH_PK;
                                                    bool IsDummyEntry = finTrxServiceClient.IsDummyEntry(refType, (int)invoiceHeaderObj.ICH_PK, 0);
                                                    if (IsDummyEntry == true)
                                                    {
                                                        DummyResult = finTrxServiceClient.DeleteFinTrx(refType, (int)invoiceHeaderObj.ICH_PK, 0);
                                                    }
                                                    if (DummyResult > 0)
                                                    {
                                                        finTrxServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(finTrxServiceClient);
                                                        DummyResult = finTrxServiceClient.GenerateDummyEntry((int)CurrPK, refType);
                                                    }
                                                }
                                                #endregion

                                                #region ATTACHMENT SAVE
                                                if (SOInvoiceUploadList != null && SOInvoiceUploadList.Count > 0)
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

                                                    foreach (DirectSOInvoiceUploads obj in SOInvoiceUploadList)
                                                    {
                                                        string filePath = savePath + obj.AttachmentFileName;
                                                        FileInfo attachedFileInfo = new FileInfo(filePath);
                                                        if (FileDetailsList != null)
                                                        {
                                                            DirectFileDetailsSI fileDetailsObj = FileDetailsList.SingleOrDefault(aa => aa.SlNo == obj.DOC_SEQ_NO);
                                                            if (fileDetailsObj != null)
                                                            {
                                                                fileDetailsObj.PoFile.SaveAs(attachedFileInfo.FullName);

                                                            }
                                                        }
                                                    }
                                                }
                                                #endregion

                                                string invoiceNo = string.Empty;
                                                if (string.IsNullOrEmpty(lblInvoiceNo.Text.Trim())
                                                    || lblInvoiceNo.Text.Trim() == Resources.Messages.DocGenerationNew)
                                                {
                                                    litErrorMsg.Text = GetLocalResourceObject("Msg_Saved_Success").ToString();
                                                }
                                                else
                                                {
                                                    litErrorMsg.Text = GetLocalResourceObject("Msg_Save_Success").ToString();
                                                    invoiceNo = lblInvoiceNo.Text.Trim();
                                                    object[] args = new object[2];
                                                    args[0] = Resources.PageNameRes.SalesInvoice;
                                                    args[1] = invoiceNo;
                                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, args);
                                                }

                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                                EntryStatus = EntryStatus.LISTMODE;
                                                ResetForm(ControlsEnum.INVOICELIST);
                                                GetFieldValues(ControlsEnum.INVOICELIST);
                                                SetFieldValues(ControlsEnum.INVOICELIST);
                                                SOInvoiceHeaderSession = null;
                                            }
                                            else if (result == -51)
                                            {
                                                if (ContineInvoiceAmtGreaterThanSCAmt == 0)//Should not allow to Save invoice amount greater than SC Amount
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_InvAmtGreaterSCAmt").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                                                else
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowInvoiceAmtGreaterSCAmtConfirm('" + (sender as Button).ID + "');", true);
                                            }
                                            else
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Empty_Rate").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                    hasValidRate = false;
                                }
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save_emptygrid").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region TAX DETAILS
                    case ActionsEnum.TAXDETAILS:
                        divTaxApplicableAmount.Visible = false; //Hide Tax Applicable Amount Checkbox div
                        hdfTaxCategory.Value = ((int)TaxType.Tax).ToString();
                        hdfTaxFormula.Value = string.Empty;
                        hdfTaxCode.Value = string.Empty;
                        hdfTaxRate.Value = string.Empty;
                        txtAmount = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("txtAmount") as TextBox);
                        txtDiscount = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("txtDiscount") as TextBox);
                        hdfInvoiceDtlPK = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfInvoiceDtlPK") as HiddenField);
                        hdfItemPK = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfItemPK") as HiddenField);
                        hdfSODetailPK = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfSODetailPK") as HiddenField);
                        ScDetailsPK = Convert.ToInt32(hdfSODetailPK.Value);
                        if (txtAmount != null && hdfInvoiceDtlPK != null && txtDiscount != null)
                        {

                            txtPopupItemAmount.Text = string.IsNullOrEmpty(txtAmount.Text.Trim()) ? ((double)0).ToString(hdfCurrencyFormat.Value) : string.IsNullOrEmpty(txtDiscount.Text.Trim()) ? Convert.ToDouble(txtAmount.Text).ToString(hdfCurrencyFormat.Value) : (Convert.ToDouble(txtAmount.Text.Trim()) - Convert.ToDouble(txtDiscount.Text.Trim())).ToString(hdfCurrencyFormat.Value);
                            SOInvoicePK = string.IsNullOrEmpty(hdfInvoiceDtlPK.Value) ? 0 : Convert.ToInt32(hdfInvoiceDtlPK.Value);
                            SelectedItemPK = string.IsNullOrEmpty(hdfItemPK.Value) ? 0 : Convert.ToInt32(hdfItemPK.Value);
                            if (SOInvoiceHeaderSession != null)
                            {
                                if (Convert.ToInt16(hdfDetailTax.Value) != 2 && SOInvoiceHeaderSession.TaxHdr != null && SOInvoiceHeaderSession.TaxHdr.Where(r => r.CIT_TAX_CATEGORY == (int)TaxType.Tax).Count() > 0)
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_HdrTaxExist").ToString()) + "');", true);
                                    return;
                                }
                                TempSOInvoiceHeaderSession = SOInvoiceHeaderSession;
                                IsHeaderTax = false;
                                SetFieldValues(ControlsEnum.TAXPOPUPGRID);
                                GetFieldValues(ControlsEnum.TAXTYPES);
                                SetFieldValues(ControlsEnum.TAXTYPES);
                                if (ddlPopupTaxType.Items.Count > 0)
                                {
                                    if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                    {
                                        TaxPK = Convert.ToInt32(ddlPopupTaxType.SelectedValue);
                                        GetFieldValues(ControlsEnum.TAXTYPES);
                                        TaxPK = 0;
                                        if (dtTaxDetails != null && dtTaxDetails.Rows.Count == 1)
                                        {
                                            string taxFormula = dtTaxDetails.Rows[0]["TAX_FORMULA"].ToString();
                                            hdfTaxFormula.Value = taxFormula;
                                            hdfTaxCode.Value = Convert.ToString(dtTaxDetails.Rows[0]["TAX_CODE"]);
                                            hdfTaxRate.Value = Convert.ToString(dtTaxDetails.Rows[0]["TAX_RATE"]);
                                            taxFormula = taxFormula.Replace("#SUBTOTAL#", txtPopupItemAmount.Text.Trim());
                                            txtPopupAmount.Text = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString(hdfCurrencyFormat.Value);
                                            SelectedTaxText = HttpUtility.HtmlDecode(dtTaxDetails.Rows[0]["TAX_HEAD"].ToString());
                                            txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                        }
                                    }
                                    else
                                    {
                                        SelectedTaxText = Resources.Report.Custom;
                                        txtPopupAmount.Text = string.Empty;
                                    }
                                    txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                    if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) == -1)
                                    {
                                        txtPopupAmount.Enabled = true;
                                        txtPopupOther.Enabled = true;
                                    }
                                    else
                                    {
                                        if (soInvTaxHdrObj != null)
                                        {
                                            if (soInvTaxHdrObj.CIT_TAX_CATEGORY != (int)TaxType.Shipping)
                                            {
                                                txtPopupAmount.Enabled = false;
                                                txtPopupOther.Enabled = false;
                                            }
                                        }
                                        else
                                        {
                                            txtPopupAmount.Enabled = true;
                                            txtPopupOther.Enabled = true;
                                        }
                                    }
                                }
                                IsEditMode = true;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("TaxDetails").ToString() + "','600','300');", true);
                            }
                        }
                        #region manage tax
                        if (Convert.ToInt16(hdfDetailTax.Value) == (int)TaxSettingEnum.ITEMWISE || Convert.ToInt16(hdfDetailTax.Value) == (int)TaxSettingEnum.BOTHHEADERITEM)
                        {
                            btnApply.Visible = false;
                            imgPopupAdd.Visible = false;
                            grdTaxDetails.Columns[3].Visible = false;
                            grdTaxDetails.Columns[4].Visible = false;
                            ddlPopupTaxType.Enabled = false;
                        }
                        #endregion
                        break;
                    #endregion
                    #region DISC DETAILS
                    case ActionsEnum.DISCDETAILS:
                         divTaxApplicableAmount.Visible = false; //Hide Tax Applicable Amount Checkbox div
                        hdfTaxCategory.Value = ((int)TaxType.Discount).ToString();
                        hdfTaxFormula.Value = string.Empty;
                        hdfTaxCode.Value = string.Empty;
                        hdfTaxRate.Value = string.Empty;
                        txtAmount = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("txtAmount") as TextBox);
                        txtDiscount = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("txtDiscount") as TextBox);
                        hdfInvoiceDtlPK = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfInvoiceDtlPK") as HiddenField);
                        hdfItemPK = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfItemPK") as HiddenField);
                        hdfSODetailPK = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfSODetailPK") as HiddenField);
                        ScDetailsPK = Convert.ToInt32(hdfSODetailPK.Value);
                        if (txtAmount != null && hdfInvoiceDtlPK != null && txtDiscount != null)
                        {
                            txtPopupItemAmount.Text = string.IsNullOrEmpty(txtAmount.Text.Trim()) ? ((double)0).ToString(hdfCurrencyFormat.Value) : Convert.ToDouble(txtAmount.Text).ToString(hdfCurrencyFormat.Value);
                            SOInvoicePK = string.IsNullOrEmpty(hdfInvoiceDtlPK.Value) ? 0 : Convert.ToInt32(hdfInvoiceDtlPK.Value);
                            SelectedItemPK = string.IsNullOrEmpty(hdfItemPK.Value) ? 0 : Convert.ToInt32(hdfItemPK.Value);
                            if (SOInvoiceHeaderSession != null)
                            {
                                if (Convert.ToInt16(hdfDetalDiscount.Value) != 2 && SOInvoiceHeaderSession.TaxHdr != null && SOInvoiceHeaderSession.TaxHdr.Where(r => r.CIT_TAX_CATEGORY == (int)TaxType.Discount).Count() > 0)
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_HdrDiscountExist").ToString()) + "');", true);
                                    return;
                                }
                                TempSOInvoiceHeaderSession = SOInvoiceHeaderSession;
                                IsHeaderTax = false;
                                SetFieldValues(ControlsEnum.TAXPOPUPGRID);
                                GetFieldValues(ControlsEnum.TAXTYPES);
                                SetFieldValues(ControlsEnum.TAXTYPES);
                                if (ddlPopupTaxType.Items.Count > 0)
                                {
                                    if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                    {
                                        TaxPK = Convert.ToInt32(ddlPopupTaxType.SelectedValue);
                                        GetFieldValues(ControlsEnum.TAXTYPES);
                                        TaxPK = 0;
                                        if (dtTaxDetails != null && dtTaxDetails.Rows.Count == 1)
                                        {
                                            string taxFormula = dtTaxDetails.Rows[0]["TAX_FORMULA"].ToString();
                                            hdfTaxFormula.Value = taxFormula;
                                            hdfTaxCode.Value = Convert.ToString(dtTaxDetails.Rows[0]["TAX_CODE"]);
                                            hdfTaxRate.Value = Convert.ToString(dtTaxDetails.Rows[0]["TAX_RATE"]);
                                            taxFormula = taxFormula.Replace("#SUBTOTAL#", txtPopupItemAmount.Text.Trim());
                                            txtPopupAmount.Text = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString(hdfCurrencyFormat.Value);
                                            SelectedTaxText = HttpUtility.HtmlDecode(dtTaxDetails.Rows[0]["TAX_HEAD"].ToString());
                                            txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                        }
                                    }
                                    else
                                    {
                                        SelectedTaxText = Resources.Report.Custom;
                                        txtPopupAmount.Text = string.Empty;
                                    }
                                    txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                    if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) == -1)
                                    {
                                        txtPopupAmount.Enabled = true;
                                        txtPopupOther.Enabled = true;
                                    }
                                    else
                                    {
                                        if (soInvTaxHdrObj != null)
                                        {
                                            if (soInvTaxHdrObj.CIT_TAX_CATEGORY != (int)TaxType.Shipping)
                                            {
                                                txtPopupAmount.Enabled = false;
                                                txtPopupOther.Enabled = false;
                                            }
                                        }
                                        else
                                        {
                                            txtPopupAmount.Enabled = true;
                                            txtPopupOther.Enabled = true;
                                        }
                                    }
                                }
                                IsEditMode = true;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("DiscountDetails").ToString() + "','600','300');", true);
                            }
                        }
                        //Need subtotal for inv. amt , tax & discount. hide discount and tax. discount not getting saved
                        if (Convert.ToInt16(hdfDetalDiscount.Value) == (int)TaxSettingEnum.ITEMWISE || Convert.ToInt16(hdfDetalDiscount.Value) == (int)TaxSettingEnum.BOTHHEADERITEM)
                        {
                            btnApply.Visible = false;
                            imgPopupAdd.Visible = false;
                            grdTaxDetails.Columns[3].Visible = false;
                            grdTaxDetails.Columns[4].Visible = false;
                            ddlPopupTaxType.Enabled = false;
                        }
                        break;
                    #endregion
                    #region TAX HEADER
                    case ActionsEnum.TAXHEADER:
                        hdfTaxCategory.Value = ((int)TaxType.Tax).ToString();
                        hdfTaxFormula.Value = string.Empty;
                        hdfTaxCode.Value = string.Empty;
                        hdfTaxRate.Value = string.Empty;
                        divTax.Visible = true;
                        if (SOInvoiceHeaderSession != null && grdInvoice.Rows.Count > 0)
                        {
                            if (Convert.ToInt16(hdfDetailTax.Value) != 2 && SOInvoiceHeaderSession.OrderDetail.Where(r => r.TaxDtl.Where(t => t.CIT_TAX_CATEGORY == (int)TaxType.Tax).Count() > 0).Count() > 0)
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_DtlTaxExist").ToString()) + "');", true);
                                return;
                            }
                            TempSOInvoiceHeaderSession = SOInvoiceHeaderSession;
                            IsHeaderTax = true;
                            SetFieldValues(ControlsEnum.TAXPOPUPGRID);
                            GetFieldValues(ControlsEnum.TAXTYPES);
                            SetFieldValues(ControlsEnum.TAXTYPES);
                            //txtSubTotal = (TextBox)grdInvoice.FooterRow.FindControl("txtSubTotalFooter");
                            //if (txtSubTotal != null)
                            //{
                            //txtPopupItemAmount.Text = string.IsNullOrEmpty(txtSubTotal.Text.Trim()) ? ((double)0).ToString(hdfCurrencyFormat.Value) : string.IsNullOrEmpty(txtHdrDiscount.Text.Trim()) ? Convert.ToDouble(txtSubTotal.Text).ToString(hdfCurrencyFormat.Value) : (Convert.ToDouble(txtSubTotal.Text.Trim()) - Convert.ToDouble(txtHdrDiscount.Text.Trim())).ToString(hdfCurrencyFormat.Value);
                            GetFieldValues(ControlsEnum.ADVANCEDTAXSETTINGS);
                            if (Convert.ToInt16(hdfDetailTax.Value) == (int)TaxSettingEnum.BOTHHEADERITEM)
                            {
                                #region
                                ResetForm(ControlsEnum.TAXCHECKBOX);
                                double popupItemAmount = 0;
                                popupItemAmount = SetTaxApplicableAmount();
                                txtPopupItemAmount.Text = (popupItemAmount).ToString(hdfCurrencyFormat.Value);
                                divTaxApplicableAmount.Visible = true; //Hide Tax Applicable Amount Checkbox div
                                #endregion
                            }
                            else
                            {
                                if (hdfTaxSettings.Value.Equals("1"))
                                {
                                    txtPopupItemAmount.Text = string.IsNullOrEmpty(txtHdrBalBeforeVat.Text.Trim()) ? ((double)0).ToString(hdfCurrencyFormat.Value) : Convert.ToDouble(txtHdrBalBeforeVat.Text).ToString(hdfCurrencyFormat.Value);
                                }
                                else
                                {
                                    txtPopupItemAmount.Text = string.IsNullOrEmpty(txtHdrTotal.Text.Trim()) ? ((double)0).ToString(hdfCurrencyFormat.Value) : Convert.ToDouble(txtHdrTotal.Text).ToString(hdfCurrencyFormat.Value);
                                }
                            }                          
                            if (ddlPopupTaxType.Items.Count > 0)
                            {
                                if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                {
                                    TaxPK = Convert.ToInt32(ddlPopupTaxType.SelectedValue);
                                    GetFieldValues(ControlsEnum.TAXTYPES);
                                    TaxPK = 0;
                                    //Change
                                    if (dtTaxDetails != null && dtTaxDetails.Rows.Count == 1)
                                    {
                                        string AmtFortax = string.Empty;
                                        string taxFormula = dtTaxDetails.Rows[0]["TAX_FORMULA"].ToString();
                                        hdfTaxFormula.Value = taxFormula;
                                        hdfTaxCode.Value = Convert.ToString(dtTaxDetails.Rows[0]["TAX_CODE"]);
                                        hdfTaxRate.Value = Convert.ToString(dtTaxDetails.Rows[0]["TAX_RATE"]);
                                        if (IsTaxForOtherCharge)
                                        {
                                            double DeductOtherCharges;
                                            double.TryParse(txtdor.Text, out DeductOtherCharges);
                                            AmtFortax = Convert.ToDecimal(txtPopupItemAmount.Text) >= 0 ? (Convert.ToDouble(txtPopupItemAmount.Text) + (Convert.ToDouble(txtShipping.Text) - DeductOtherCharges)).ToString() : "0";
                                            txtPopupItemAmount.Text = AmtFortax;
                                        }
                                        else
                                            AmtFortax = Convert.ToDecimal(txtPopupItemAmount.Text) >= 0 ? txtPopupItemAmount.Text : "0";
                                        taxFormula = taxFormula.Replace("#SUBTOTAL#", AmtFortax);
                                        txtPopupAmount.Text = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString(hdfCurrencyFormat.Value);
                                        SelectedTaxText = HttpUtility.HtmlDecode(dtTaxDetails.Rows[0]["TAX_HEAD"].ToString());
                                        txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                    }
                                }
                                else
                                {
                                    SelectedTaxText = Resources.Report.Custom;
                                    txtPopupAmount.Text = string.Empty;
                                }
                                txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) == -1)
                                {
                                    txtPopupAmount.Enabled = true;
                                    txtPopupOther.Enabled = true;
                                }
                                else
                                {
                                    if (soInvTaxHdrObj != null)
                                    {
                                        if (soInvTaxHdrObj.CIT_TAX_CATEGORY != (int)TaxType.Shipping)
                                        {
                                            txtPopupAmount.Enabled = false;
                                            txtPopupOther.Enabled = false;
                                        }
                                    }
                                    else
                                    {
                                        txtPopupAmount.Enabled = true;
                                        txtPopupOther.Enabled = true;
                                    }
                                }
                            }
                            #region Manage HeaderTaxPopup Control Visibility

                            if (Convert.ToInt16(hdfDetailTax.Value) == (int)TaxSettingEnum.BOTHHEADERITEM)
                            {
                                btnApply.Visible = true;
                                imgPopupAdd.Visible = true;
                                grdTaxDetails.Columns[3].Visible = true;
                                ddlPopupTaxType.Enabled = true;
                            }

                            #endregion
                            IsEditMode = true;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("TaxDetails").ToString() + "','600','300');", true);
                            //}
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_NoItems").ToString()) + "');", true);
                        }
                        break;
                    #endregion
                    #region DISC HEADER
                    case ActionsEnum.DISCHEADER:
                         divTaxApplicableAmount.Visible = false; //Hide Tax Applicable Amount Checkbox div
                        hdfTaxCategory.Value = ((int)TaxType.Discount).ToString();
                        hdfTaxFormula.Value = string.Empty;
                        hdfTaxCode.Value = string.Empty;
                        hdfTaxRate.Value = string.Empty;
                        divTax.Visible = true;
                        if (SOInvoiceHeaderSession != null && grdInvoice.Rows.Count > 0)
                        {
                            if (Convert.ToInt16(hdfDetalDiscount.Value) != 2&& SOInvoiceHeaderSession.OrderDetail.Where(r => r.TaxDtl.Where(t => t.CIT_TAX_CATEGORY == (int)TaxType.Discount).Count() > 0).Count() > 0)
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_DtlDiscountExist").ToString()) + "');", true);
                                return;
                            }
                            TempSOInvoiceHeaderSession = SOInvoiceHeaderSession;
                            IsHeaderTax = true;
                            SetFieldValues(ControlsEnum.TAXPOPUPGRID);
                            GetFieldValues(ControlsEnum.TAXTYPES);
                            SetFieldValues(ControlsEnum.TAXTYPES);

                            txtSubTotal = (TextBox)grdInvoice.FooterRow.FindControl("txtSubTotalFooter");

                            txtSubTotalInvNow = (TextBox)grdInvoice.FooterRow.FindControl("txtSubTotalInvNowFooter");

                            if (txtSubTotal != null)
                            {
                                txtPopupItemAmount.Text = string.IsNullOrEmpty(txtSubTotal.Text.Trim()) ? ((double)0).ToString(hdfCurrencyFormat.Value) : Convert.ToDouble(txtSubTotal.Text).ToString(hdfCurrencyFormat.Value);
                                if (ddlPopupTaxType.Items.Count > 0)
                                {
                                    if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                    {
                                        TaxPK = Convert.ToInt32(ddlPopupTaxType.SelectedValue);
                                        GetFieldValues(ControlsEnum.TAXTYPES);
                                        TaxPK = 0;
                                        if (dtTaxDetails != null && dtTaxDetails.Rows.Count == 1)
                                        {
                                            string taxFormula = dtTaxDetails.Rows[0]["TAX_FORMULA"].ToString();
                                            hdfTaxFormula.Value = taxFormula;
                                            hdfTaxCode.Value = Convert.ToString(dtTaxDetails.Rows[0]["TAX_CODE"]);
                                            hdfTaxRate.Value = Convert.ToString(dtTaxDetails.Rows[0]["TAX_RATE"]);
                                            taxFormula = taxFormula.Replace("#SUBTOTAL#", txtPopupItemAmount.Text.Trim());
                                            txtPopupAmount.Text = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString(hdfCurrencyFormat.Value);
                                            SelectedTaxText = HttpUtility.HtmlDecode(dtTaxDetails.Rows[0]["TAX_HEAD"].ToString());
                                            txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                        }
                                    }
                                    else
                                    {
                                        SelectedTaxText = Resources.Report.Custom;
                                        txtPopupAmount.Text = string.Empty;
                                    }
                                    txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                    if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) == -1)
                                    {
                                        txtPopupAmount.Enabled = true;
                                        txtPopupOther.Enabled = true;
                                    }
                                    else
                                    {
                                        if (soInvTaxHdrObj != null)
                                        {
                                            if (soInvTaxHdrObj.CIT_TAX_CATEGORY != (int)TaxType.Shipping)
                                            {
                                                txtPopupAmount.Enabled = false;
                                                txtPopupOther.Enabled = false;
                                            }
                                        }
                                        else
                                        {
                                            txtPopupAmount.Enabled = true;
                                            txtPopupOther.Enabled = true;
                                        }
                                    }
                                }
                                IsEditMode = true;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("DiscountDetails").ToString() + "','600','300');", true);
                            }

                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_NoItems").ToString()) + "');", true);
                        }
                        break;
                    #endregion
                    #region SHIPPING HEADER
                    case ActionsEnum.SHIPPINGHEADER:
                        //To handle other charges pick for single/multiple SC 
                        if (grdInvoice.Rows.Count > 0)
                        {
                            if (SOInvoiceHeaderSession != null)
                            {
                                TempSOInvoiceHeaderSession = SOInvoiceHeaderSession;
                            }
                            hdfTaxCategory.Value = ((int)TaxType.Shipping).ToString();
                            SetFieldValues(ControlsEnum.OTHERCHARGELIST);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){CalculateTotalOtherCharge();});", true);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divOtherchargeSplitUp]','" + GetLocalResourceObject("OtherCharges").ToString() + "','800','300');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_NoItems").ToString()) + "');", true);
                        }

                        break;
                    #endregion
                    #region TAX APPLY
                    case ActionsEnum.TAXAPPLY:
                        SetDetailTax(null);
                        SetHdrTax();
                        ResetForm(ControlsEnum.TAXPOPUPGRID);
                        //Settings of TaxPayableDiv
                        if (hdfIsTaxPayable.Value.ToString() == "1")
                        {
                            SetTaxPayableDiv();
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "TaxPayable", "$(document).ready(function () { ShowHideTaxPayableOuter();});", true);
                        //end

                        IsHeaderTax = false;
                        IsEditMode = false;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                        break;
                    #endregion
                    #region Recalculate
                    case ActionsEnum.RECALCULATE:
                        TempSOInvoiceHeaderSession = SOInvoiceHeaderSession;
                        //SetDetailTax(null);
                        SetHdrTax();
                        //Settings of TaxPayableDiv
                        if (hdfIsTaxPayable.Value.ToString() == "1")
                        {
                            SetTaxPayableDiv();
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "TaxPayable", "$(document).ready(function () { ShowHideTaxPayableOuter();});", true);
                        //end

                        break;
                    #endregion
                    #region TAX ADD
                    case ActionsEnum.TAXADD:
                        bool errorTaxAdd = false;
                        bool errorTaxAmount = false;
                        if (TempSOInvoiceHeaderSession != null)
                        {
                            invoiceHeaderObj = TempSOInvoiceHeaderSession;
                            tempInvTaxSplitObj = null;
                            if (IsHeaderTax)
                            {
                                if (Convert.ToInt16(hdfTaxSlNo.Value) < 0)
                                {
                                    if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                    {

                                        tempInvTaxSplitObj = invoiceHeaderObj.TaxHdr.SingleOrDefault(rfq => rfq.CIT_TAX == Convert.ToInt32(ddlPopupTaxType.SelectedValue) && rfq.CIT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                    }
                                    else
                                    {
                                        tempInvTaxSplitObj = invoiceHeaderObj.TaxHdr.SingleOrDefault(rfq => rfq.CIT_NAME == txtPopupOther.Text.Trim() && rfq.CIT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                    }
                                }
                            }
                            else
                            {
                                soInvoiceDetailsObj = invoiceHeaderObj.OrderDetail.SingleOrDefault(rfq => rfq.CID_PK == SOInvoicePK && rfq.CID_ITEM == SelectedItemPK);
                                if (soInvoiceDetailsObj != null)
                                {
                                    if (Convert.ToInt16(hdfTaxSlNo.Value) < 0)
                                    {
                                        if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                        {
                                            tempInvTaxSplitObj = soInvoiceDetailsObj.TaxDtl.SingleOrDefault(rfq => rfq.CIT_TAX == Convert.ToInt32(ddlPopupTaxType.SelectedValue) && rfq.CIT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                        }
                                        else
                                        {
                                            tempInvTaxSplitObj = soInvoiceDetailsObj.TaxDtl.SingleOrDefault(rfq => rfq.CIT_NAME == txtPopupOther.Text.Trim() && rfq.CIT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                        }
                                    }
                                }
                            }
                            if (tempInvTaxSplitObj == null)
                            {
                                taxHdrList = new List<DirectSOInvoiceTaxHdr>();
                                if (Convert.ToInt16(hdfTaxSlNo.Value) >= 0)
                                {
                                    try
                                    {
                                        invoiceHeaderObj.TaxHdr.SingleOrDefault(rfq => rfq.CIT_SL_NO == Convert.ToInt16(hdfTaxSlNo.Value) && rfq.CIT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value)).CIT_TAX_AMT = string.IsNullOrEmpty(txtPopupAmount.Text.Trim()) ? 0 : Convert.ToDouble(txtPopupAmount.Text.Trim());
                                        invoiceHeaderObj.TaxHdr.SingleOrDefault(rfq => rfq.CIT_SL_NO == Convert.ToInt16(hdfTaxSlNo.Value) && rfq.CIT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value)).CIT_NAME = txtPopupOther.Text;
                                        IsOCEdit = true;
                                    }
                                    catch
                                    {
                                        errorTaxAmount = true;
                                    }
                                }
                                else
                                {
                                    soInvTaxHdrObj = new DirectSOInvoiceTaxHdr();
                                    try
                                    {
                                        soInvTaxHdrObj.CIT_TAX_AMT = string.IsNullOrEmpty(txtPopupAmount.Text.Trim()) ? 0 : Convert.ToDouble(txtPopupAmount.Text.Trim());
                                    }
                                    catch
                                    {
                                        errorTaxAmount = true;
                                    }
                                }
                                if (!errorTaxAmount && Convert.ToInt16(hdfTaxSlNo.Value) < 0)
                                {
                                    soInvTaxHdrObj.CIT_INVOICE_DTL = SOInvoicePK;
                                    soInvTaxHdrObj.CIT_SL_NO = ((invoiceHeaderObj.TaxHdr == null || invoiceHeaderObj.TaxHdr.Count == 0) ? 0 : invoiceHeaderObj.TaxHdr.LastOrDefault().CIT_SL_NO) + 1;
                                    if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                    {
                                        soInvTaxHdrObj.CIT_TAX = Convert.ToInt32(ddlPopupTaxType.SelectedValue);
                                        soInvTaxHdrObj.CIT_TYPE = 1;
                                    }
                                    else
                                        soInvTaxHdrObj.CIT_TYPE = 2;
                                    soInvTaxHdrObj.CIT_TAX_TEXT = HttpUtility.HtmlDecode(SelectedTaxText);
                                    soInvTaxHdrObj.CIT_NAME = HttpUtility.HtmlDecode(txtPopupOther.Text);
                                    soInvTaxHdrObj.CIT_PK = 0;
                                    //rfqTaxHdrObj.CIT_TAX_CATEGORY_TEXT = "Tax";
                                    soInvTaxHdrObj.CIT_TAX_CATEGORY = Convert.ToInt32(hdfTaxCategory.Value);
                                    soInvTaxHdrObj.CIT_TAX_FORMULA = string.IsNullOrEmpty(hdfTaxFormula.Value) ? string.Empty : hdfTaxFormula.Value;
                                    soInvTaxHdrObj.CIT_TAX_CODE = string.IsNullOrEmpty(hdfTaxCode.Value) ? string.Empty : hdfTaxCode.Value;
                                    double taxRate = 0;
                                    double.TryParse(hdfTaxRate.Value, out taxRate);
                                    soInvTaxHdrObj.CIT_TAX_RATE = taxRate;

                                    if (IsHeaderTax)
                                    {
                                        if (soInvTaxHdrObj.CIT_TAX_CATEGORY == (int)TaxType.Discount)
                                        {
                                            totalAmt = 0;
                                            currentTotal = 0;
                                            taxAmt = 0;
                                            //if (invoiceHeaderObj != null)
                                            //    invoiceHeaderMulObj.SOMainList.ForEach(ot =>
                                            //    {
                                            totalAmt = totalAmt + invoiceHeaderObj.ICH_AMOUNT_TC;
                                            //});
                                            //totalAmt = Convert.ToDouble(invoiceHeaderObj.ICH_AMOUNT_TC);
                                            currentTotal = invoiceHeaderObj.TaxHdr.Where(quotation => quotation.CIT_TAX_CATEGORY == ((int)TaxType.Discount)).Sum(disc => disc.CIT_TAX_AMT);
                                            if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                            {
                                                taxAmt = CalculateTaxFormula(soInvTaxHdrObj.CIT_TAX_FORMULA, totalAmt);
                                            }
                                            else
                                            {
                                                taxAmt = soInvTaxHdrObj.CIT_TAX_AMT;
                                            }
                                            if (totalAmt >= (currentTotal + taxAmt))
                                            {
                                                taxHdrList = invoiceHeaderObj.TaxHdr.ToList();
                                                taxHdrList.Add(soInvTaxHdrObj);
                                                invoiceHeaderObj.TaxHdr = taxHdrList;
                                            }
                                            else
                                            {
                                                isValidDisc = false;
                                            }
                                        }
                                        else
                                        {
                                            if (Convert.ToInt16(hdfDetailTax.Value) == (int)TaxSettingEnum.BOTHHEADERITEM)
                                            {
                                                soInvTaxHdrObj.CIT_HAS_SUB_TOTAL = chkSubTotal.Checked ? 1 : 0;
                                                soInvTaxHdrObj.CIT_HAS_DISCOUNT = chkDiscount.Checked ? 1 : 0;
                                                soInvTaxHdrObj.CIT_HAS_OTHER_CHARGE = chkOtherCharges.Checked ? 1 : 0;
                                            }
                                            taxHdrList = invoiceHeaderObj.TaxHdr.ToList();
                                            taxHdrList.Add(soInvTaxHdrObj);
                                            invoiceHeaderObj.TaxHdr = taxHdrList;
                                        }
                                    }
                                    else
                                    {
                                        soInvoiceDetailsObj = invoiceHeaderObj.OrderDetail.SingleOrDefault(rfq => rfq.CID_PK == SOInvoicePK && rfq.CID_ITEM == SelectedItemPK);
                                        if (soInvoiceDetailsObj != null)
                                        {
                                            if (soInvTaxHdrObj.CIT_TAX_CATEGORY == (int)TaxType.Discount)
                                            {
                                                totalAmt = 0;
                                                currentTotal = 0;
                                                taxAmt = 0;

                                                totalAmt = soInvoiceDetailsObj.CID_AMOUNT;
                                                currentTotal = soInvoiceDetailsObj.TaxDtl.Where(quotation => quotation.CIT_TAX_CATEGORY == ((int)TaxType.Discount)).Sum(disc => disc.CIT_TAX_AMT);
                                                if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                                {
                                                    taxAmt = CalculateTaxFormula(soInvTaxHdrObj.CIT_TAX_FORMULA, totalAmt);
                                                }
                                                else
                                                {
                                                    taxAmt = soInvTaxHdrObj.CIT_TAX_AMT;
                                                }
                                                if (totalAmt >= (currentTotal + taxAmt))
                                                {
                                                    taxHdrList = soInvoiceDetailsObj.TaxDtl.ToList();
                                                    taxHdrList.Add(soInvTaxHdrObj);
                                                    invoiceHeaderObj.OrderDetail.SingleOrDefault(rfq => rfq.CID_PK == SOInvoicePK && rfq.CID_ITEM == SelectedItemPK).TaxDtl = taxHdrList;
                                                }
                                                else
                                                {
                                                    isValidDisc = false;
                                                }
                                            }
                                            else
                                            {
                                                taxHdrList = soInvoiceDetailsObj.TaxDtl.ToList();
                                                taxHdrList.Add(soInvTaxHdrObj);
                                                invoiceHeaderObj.OrderDetail.SingleOrDefault(rfq => rfq.CID_PK == SOInvoicePK && rfq.CID_ITEM == SelectedItemPK).TaxDtl = taxHdrList;
                                            }
                                        }
                                    }

                                }
                                hdfTaxSlNo.Value = "-1";
                                TempSOInvoiceHeaderSession = invoiceHeaderObj;
                                //if (invoiceHeaderMulObj != null)
                                //    invoiceHeaderMulObj.SOMainList.ForEach(ot =>
                                //    {
                                //        ot.TaxHdr = invoiceHeaderObj.TaxHdr;
                                //    });
                                SetFieldValues(ControlsEnum.TAXPOPUPGRID);
                            }
                            else
                            {
                                errorTaxAdd = true;
                            }
                            if (ddlPopupTaxType.Items.Count > 0)
                            {
                                if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) == -1)
                                {
                                    txtPopupAmount.Enabled = true;
                                    txtPopupOther.Enabled = true;
                                }
                                else
                                {
                                    //txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                    if (soInvTaxHdrObj != null)
                                    {
                                        if (soInvTaxHdrObj.CIT_TAX_CATEGORY != (int)TaxType.Shipping)
                                        {
                                            txtPopupAmount.Enabled = false;
                                            txtPopupOther.Enabled = false;
                                        }
                                    }
                                    else
                                    {
                                        txtPopupAmount.Enabled = true;
                                        txtPopupOther.Enabled = true;
                                    }
                                }
                                if (!errorTaxAdd && !errorTaxAmount)
                                {
                                    txtPopupAmount.Text = string.Empty;
                                    txtPopupOther.Text = string.Empty;
                                }
                            }
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + (Convert.ToInt32(hdfTaxCategory.Value) == ((int)TaxType.Discount) ? GetLocalResourceObject("DiscountDetails").ToString() : GetLocalResourceObject("TaxDetails").ToString()) + "','600','300');", true);
                        if (errorTaxAdd)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Tax_Add").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else if (errorTaxAmount)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Tax_Amount").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else if (!isValidDisc)
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Discount_Amount").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        break;
                    #endregion
                    #region TAX EDIT
                    case ActionsEnum.TAXEDIT://OtherCharge
                        if (TempSOInvoiceHeaderSession != null)
                        {
                            invoiceHeaderObj = TempSOInvoiceHeaderSession;
                            HiddenField hdfTaxPK = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfTaxPK") as HiddenField);
                            HiddenField hdfTaxName = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfTaxName") as HiddenField);
                            if (hdfTaxPK != null)
                            {
                                int taxPK = string.IsNullOrEmpty(hdfTaxPK.Value) ? 0 : Convert.ToInt32(hdfTaxPK.Value);
                                taxHdrList = new List<DirectSOInvoiceTaxHdr>();
                                if (IsHeaderTax)
                                {
                                    txtPopupAmount.Enabled = true;
                                    txtPopupOther.Enabled = true;
                                    if (taxPK > 0)
                                    {

                                        tempInvTaxSplitObj = invoiceHeaderObj.TaxHdr.SingleOrDefault(rfq => rfq.CIT_TAX == taxPK && rfq.CIT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                        txtPopupAmount.Text = Math.Round(tempInvTaxSplitObj.CIT_TAX_AMT, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString(hdfCurrencyFormat.Value);
                                        txtPopupOther.Text = tempInvTaxSplitObj.CIT_NAME;
                                        hdfTaxSlNo.Value = tempInvTaxSplitObj.CIT_SL_NO.ToString();
                                    }
                                    else//Custom
                                    {
                                        if (hdfTaxName != null)
                                        {
                                            tempInvTaxSplitObj = invoiceHeaderObj.TaxHdr.SingleOrDefault(rfq => rfq.CIT_NAME == hdfTaxName.Value && rfq.CIT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                            txtPopupAmount.Text = Math.Round(tempInvTaxSplitObj.CIT_TAX_AMT, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString(hdfCurrencyFormat.Value);
                                            txtPopupOther.Text = tempInvTaxSplitObj.CIT_NAME;
                                            hdfTaxSlNo.Value = tempInvTaxSplitObj.CIT_SL_NO.ToString();
                                        }
                                    }
                                    //if (tempInvTaxSplitObj != null)
                                    //{
                                    //    taxHdrList = invoiceHeaderObj.TaxHdr.ToList();
                                    //    taxHdrList.Remove(tempInvTaxSplitObj);
                                    //    invoiceHeaderObj.TaxHdr = taxHdrList;
                                    //}
                                }
                                //else
                                //{
                                //    soInvoiceDetailsObj = invoiceHeaderObj.OrderDetail.SingleOrDefault(rfq => rfq.CID_PK == SOInvoicePK && rfq.CID_ITEM == SelectedItemPK);
                                //    if (soInvoiceDetailsObj != null)
                                //    {
                                //        if (taxPK > 0)
                                //        {
                                //            tempInvTaxSplitObj = soInvoiceDetailsObj.TaxDtl.SingleOrDefault(rfq => rfq.CIT_TAX == taxPK && rfq.CIT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                //        }
                                //        else
                                //        {
                                //            if (hdfTaxName != null)
                                //            {
                                //                tempInvTaxSplitObj = soInvoiceDetailsObj.TaxDtl.SingleOrDefault(rfq => rfq.CIT_NAME == hdfTaxName.Value && rfq.CIT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                //            }
                                //        }
                                //        soInvoiceDetailsObj = invoiceHeaderObj.OrderDetail.SingleOrDefault(rfq => rfq.CID_PK == SOInvoicePK && rfq.CID_ITEM == SelectedItemPK);
                                //        if (soInvoiceDetailsObj != null)
                                //        {
                                //            taxHdrList = soInvoiceDetailsObj.TaxDtl.ToList();
                                //            taxHdrList.Remove(tempInvTaxSplitObj);
                                //            invoiceHeaderObj.OrderDetail.SingleOrDefault(rfq => rfq.CID_PK == SOInvoicePK && rfq.CID_ITEM == SelectedItemPK).TaxDtl = taxHdrList;
                                //        }
                                //    }
                                //}

                                TempSOInvoiceHeaderSession = invoiceHeaderObj;
                                SetFieldValues(ControlsEnum.TAXPOPUPGRID);

                            }
                            //if (ddlPopupTaxType.Items.Count > 0)
                            //{
                            //    if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) == -1)
                            //    {
                            //        SelectedTaxText = Resources.Report.Custom;
                            //        txtPopupAmount.Enabled = true;
                            //        txtPopupOther.Enabled = true;
                            //    }
                            //    else
                            //    {
                            //        if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                            //        {
                            //            TaxPK = Convert.ToInt32(ddlPopupTaxType.SelectedValue);
                            //            GetFieldValues(ControlsEnum.TAXTYPES);
                            //            TaxPK = 0;
                            //            if (dtTaxDetails != null && dtTaxDetails.Rows.Count == 1)
                            //            {
                            //                string taxFormula = dtTaxDetails.Rows[0]["TAX_FORMULA"].ToString();
                            //                hdfTaxFormula.Value = taxFormula;
                            //                taxFormula = taxFormula.Replace("#SUBTOTAL#", txtPopupItemAmount.Text.Trim());
                            //                txtPopupAmount.Text = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString(hdfCurrencyFormat.Value);
                            //                SelectedTaxText = HttpUtility.HtmlEncode(dtTaxDetails.Rows[0]["TAX_HEAD"].ToString());
                            //                txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                            //            }
                            //        }
                            //        if (soInvTaxHdrObj != null)
                            //        {
                            //            if (soInvTaxHdrObj.CIT_TAX_CATEGORY != (int)TaxType.Shipping)
                            //            {
                            //                txtPopupAmount.Enabled = false;
                            //                txtPopupOther.Enabled = false;
                            //            }
                            //        }
                            //        else
                            //        {
                            //            txtPopupAmount.Enabled = true;
                            //            txtPopupOther.Enabled = true;
                            //        }
                            //    }
                            //}
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + (Convert.ToInt32(hdfTaxCategory.Value) == ((int)TaxType.Discount) ? GetLocalResourceObject("DiscountDetails").ToString() : Convert.ToInt32(hdfTaxCategory.Value) == ((int)TaxType.Shipping) ? GetLocalResourceObject("OtherCharges").ToString() : GetLocalResourceObject("TaxDetails").ToString()) + "','600','300');", true);

                        break;
                    #endregion
                    #region TAX DELETE
                    case ActionsEnum.TAXDELETE:
                        if (TempSOInvoiceHeaderSession != null)
                        {
                            invoiceHeaderObj = TempSOInvoiceHeaderSession;
                            HiddenField hdfTaxPK = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfTaxPK") as HiddenField);
                            HiddenField hdfTaxName = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfTaxName") as HiddenField);
                            if (hdfTaxPK != null)
                            {
                                int taxPK = string.IsNullOrEmpty(hdfTaxPK.Value) ? 0 : Convert.ToInt32(hdfTaxPK.Value);
                                taxHdrList = new List<DirectSOInvoiceTaxHdr>();
                                if (IsHeaderTax)
                                {
                                    if (taxPK > 0)
                                    {
                                        tempInvTaxSplitObj = invoiceHeaderObj.TaxHdr.SingleOrDefault(rfq => rfq.CIT_TAX == taxPK && rfq.CIT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                    }
                                    else
                                    {
                                        if (hdfTaxName != null)
                                        {
                                            tempInvTaxSplitObj = invoiceHeaderObj.TaxHdr.SingleOrDefault(rfq => rfq.CIT_NAME == hdfTaxName.Value && rfq.CIT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                        }
                                    }
                                    if (tempInvTaxSplitObj != null)
                                    {
                                        taxHdrList = invoiceHeaderObj.TaxHdr.ToList();
                                        taxHdrList.Remove(tempInvTaxSplitObj);
                                        invoiceHeaderObj.TaxHdr = taxHdrList;
                                    }
                                }
                                else
                                {
                                    soInvoiceDetailsObj = invoiceHeaderObj.OrderDetail.SingleOrDefault(rfq => rfq.CID_PK == SOInvoicePK && rfq.CID_ITEM == SelectedItemPK);
                                    if (soInvoiceDetailsObj != null)
                                    {
                                        if (taxPK > 0)
                                        {
                                            tempInvTaxSplitObj = soInvoiceDetailsObj.TaxDtl.SingleOrDefault(rfq => rfq.CIT_TAX == taxPK && rfq.CIT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                        }
                                        else
                                        {
                                            if (hdfTaxName != null)
                                            {
                                                tempInvTaxSplitObj = soInvoiceDetailsObj.TaxDtl.SingleOrDefault(rfq => rfq.CIT_NAME == hdfTaxName.Value && rfq.CIT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                            }
                                        }
                                        soInvoiceDetailsObj = invoiceHeaderObj.OrderDetail.SingleOrDefault(rfq => rfq.CID_PK == SOInvoicePK && rfq.CID_ITEM == SelectedItemPK);
                                        if (soInvoiceDetailsObj != null)
                                        {
                                            taxHdrList = soInvoiceDetailsObj.TaxDtl.ToList();
                                            taxHdrList.Remove(tempInvTaxSplitObj);
                                            invoiceHeaderObj.OrderDetail.SingleOrDefault(rfq => rfq.CID_PK == SOInvoicePK && rfq.CID_ITEM == SelectedItemPK).TaxDtl = taxHdrList;
                                        }
                                    }
                                }

                                TempSOInvoiceHeaderSession = invoiceHeaderObj;
                                SetFieldValues(ControlsEnum.TAXPOPUPGRID);

                            }
                            if (ddlPopupTaxType.Items.Count > 0)
                            {
                                if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) == -1)
                                {
                                    SelectedTaxText = Resources.Report.Custom;
                                    txtPopupAmount.Enabled = true;
                                    txtPopupOther.Enabled = true;
                                }
                                else
                                {
                                    if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                    {
                                        TaxPK = Convert.ToInt32(ddlPopupTaxType.SelectedValue);
                                        GetFieldValues(ControlsEnum.TAXTYPES);
                                        TaxPK = 0;
                                        if (dtTaxDetails != null && dtTaxDetails.Rows.Count == 1)
                                        {
                                            string taxFormula = dtTaxDetails.Rows[0]["TAX_FORMULA"].ToString();
                                            hdfTaxFormula.Value = taxFormula;
                                            hdfTaxCode.Value = Convert.ToString(dtTaxDetails.Rows[0]["TAX_CODE"]);
                                            hdfTaxRate.Value = Convert.ToString(dtTaxDetails.Rows[0]["TAX_RATE"]);
                                            taxFormula = taxFormula.Replace("#SUBTOTAL#", txtPopupItemAmount.Text.Trim());
                                            txtPopupAmount.Text = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString(hdfCurrencyFormat.Value);
                                            SelectedTaxText = HttpUtility.HtmlEncode(dtTaxDetails.Rows[0]["TAX_HEAD"].ToString());
                                            txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                        }
                                    }
                                    if (soInvTaxHdrObj != null)
                                    {
                                        if (soInvTaxHdrObj.CIT_TAX_CATEGORY != (int)TaxType.Shipping)
                                        {
                                            txtPopupAmount.Enabled = false;
                                            txtPopupOther.Enabled = false;
                                        }
                                    }
                                    else
                                    {
                                        txtPopupAmount.Enabled = true;
                                        txtPopupOther.Enabled = true;
                                    }
                                }
                            }
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + (Convert.ToInt32(hdfTaxCategory.Value) == ((int)TaxType.Discount) ? GetLocalResourceObject("DiscountDetails").ToString() : Convert.ToInt32(hdfTaxCategory.Value) == ((int)TaxType.Shipping) ? GetLocalResourceObject("OtherCharges").ToString() : GetLocalResourceObject("TaxDetails").ToString()) + "','600','300');", true);
                        break;
                    #endregion
                    #region TAX TYPE CHANGED
                    case ActionsEnum.TAXTYPECHANGED:
                        if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                        {
                            TaxPK = Convert.ToInt32(ddlPopupTaxType.SelectedValue);
                            GetFieldValues(ControlsEnum.TAXTYPES);
                            TaxPK = 0;
                            if (dtTaxDetails != null && dtTaxDetails.Rows.Count == 1)
                            {
                                string taxFormula = dtTaxDetails.Rows[0]["TAX_FORMULA"].ToString();
                                hdfTaxFormula.Value = taxFormula;
                                hdfTaxCode.Value = Convert.ToString(dtTaxDetails.Rows[0]["TAX_CODE"]);
                                hdfTaxRate.Value = Convert.ToString(dtTaxDetails.Rows[0]["TAX_RATE"]);
                                taxFormula = taxFormula.Replace("#SUBTOTAL#", txtPopupItemAmount.Text.Trim());
                                txtPopupAmount.Text = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString(hdfCurrencyFormat.Value);
                                SelectedTaxText = dtTaxDetails.Rows[0]["TAX_HEAD"].ToString();
                                txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                if (soInvTaxHdrObj != null)
                                {
                                    if (soInvTaxHdrObj.CIT_TAX_CATEGORY != (int)TaxType.Shipping)
                                    {
                                        txtPopupAmount.Enabled = false;
                                        txtPopupOther.Enabled = false;
                                    }
                                }
                                else
                                {
                                    txtPopupAmount.Enabled = true;
                                    txtPopupOther.Enabled = true;
                                }
                            }
                        }
                        else if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) == -1)
                        {
                            hdfTaxFormula.Value = string.Empty;
                            hdfTaxCode.Value = string.Empty;
                            hdfTaxRate.Value = string.Empty;
                            txtPopupAmount.Text = string.Empty;
                            SelectedTaxText = Resources.Report.Custom;
                            txtPopupOther.Text = string.Empty;
                            txtPopupAmount.Enabled = true;
                            txtPopupOther.Enabled = true;
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + (Convert.ToInt32(hdfTaxCategory.Value) == ((int)TaxType.Discount) ? GetLocalResourceObject("DiscountDetails").ToString() : GetLocalResourceObject("TaxDetails").ToString()) + "','600','300');", true);
                        break;
                    #endregion
                    #region CANCEL
                    case ActionsEnum.CANCEL:
                        FillProcessID(1);
                        ResetForm(ControlsEnum.INVOICELIST);
                        //Response.Redirect(Resources.PageURL.SoListing);
                        CurrPK = 0;
                        hdfCurrentPk.Value = CurrPK.ToString();
                        hdfIVHPK.Value = "";
                        GetFieldValues(ControlsEnum.INVOICELIST);
                        SetFieldValues(ControlsEnum.INVOICELIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion
                    #region CALCULATE DTL TAX
                    case ActionsEnum.CALCULATEDTLTAX:
                        bool isValidQty = true;
                        double maxQty = 0;
                        if (DespatchID > 0 || SOInvoiceHeaderSession != null && !string.IsNullOrEmpty(SOInvoiceHeaderSession.ICH_DESPATCH_HDR)
                                                      && Convert.ToInt32(SOInvoiceHeaderSession.ICH_DESPATCH_HDR) > 0)
                        {

                        }
                        else
                        {
                            TextBox txtQuantity;
                            Label lblOrderQuantity;
                            Label lblInvQuantity;
                            Label lblInvProformaQuantity;
                            Label lblInvNowMFS;
                            txtQuantity = sender as TextBox;
                            lblOrderQuantity = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("lblOrderQuantity") as Label);
                            lblInvQuantity = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("lblInvQuantity") as Label);
                            lblInvProformaQuantity = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("lblInvProformaQuantity") as Label);
                            lblInvNowMFS = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("lblInvNowMFS") as Label);
                            if (Convert.ToByte(ddlInvoiceType.SelectedValue) == Convert.ToByte(SalesInvoiceType.Proforma))
                            {
                                if (txtQuantity != null && lblOrderQuantity != null && lblInvQuantity != null && lblInvProformaQuantity != null)
                                {
                                    double qty = string.IsNullOrEmpty(txtQuantity.Text.Trim()) ? 0 : Convert.ToDouble(txtQuantity.Text.Trim());
                                    //double orderQty = string.IsNullOrEmpty(lblOrderQuantity.Text.Trim()) ? 0 : Convert.ToDouble(lblOrderQuantity.Text.Trim());
                                    double orderQty = 0;
                                    double.TryParse(lblOrderQuantity.Text.Trim(), out orderQty);
                                    //double invQty = string.IsNullOrEmpty(lblInvQuantity.Text.Trim()) ? 0 : Convert.ToDouble(lblInvQuantity.Text.Trim());
                                    double invQty = 0;
                                    double.TryParse(lblInvQuantity.Text.Trim(), out invQty);
                                    double proInvQty = string.IsNullOrEmpty(lblInvProformaQuantity.Text.Trim()) ? 0 : Convert.ToDouble(lblInvProformaQuantity.Text.Replace(",", "").Trim());
                                    double val = (invQty > proInvQty ? invQty : proInvQty);
                                    if (val < 0 && qty > 0)
                                    {
                                        maxQty = 0;
                                        isValidQty = false;
                                    }
                                    else if (qty > orderQty - val)
                                    {
                                        maxQty = orderQty - (invQty > proInvQty ? invQty : proInvQty);
                                        isValidQty = false;
                                    }
                                }
                            }
                            else
                            {
                                if (txtQuantity != null && lblOrderQuantity != null && lblInvQuantity != null)
                                {
                                    double qty = string.IsNullOrEmpty(txtQuantity.Text.Trim()) ? 0 : Convert.ToDouble(txtQuantity.Text.Trim());
                                    //double orderQty = string.IsNullOrEmpty(lblOrderQuantity.Text.Trim()) ? 0 : Convert.ToDouble(lblOrderQuantity.Text.Trim());
                                    //double invQty = string.IsNullOrEmpty(lblInvQuantity.Text.Trim()) ? 0 : Convert.ToDouble(lblInvQuantity.Text.Trim());
                                    double orderQty = 0;
                                    double.TryParse(lblOrderQuantity.Text.Trim(), out orderQty);
                                    double invQty = 0;
                                    double.TryParse(lblInvQuantity.Text.Trim(), out invQty);
                                    if (invQty < 0 && qty > 0)
                                    {
                                        maxQty = 0;
                                        isValidQty = false;
                                    }
                                    else if (qty > orderQty - invQty)
                                    {
                                        maxQty = orderQty - invQty;
                                        isValidQty = false;
                                    }
                                    if (Approved == 2)
                                    {

                                        qty -= Convert.ToDouble(lblInvNowMFS.Text);
                                        if (qty > invQty)
                                        {
                                            isValidQty = false;
                                        }
                                        else
                                        {
                                            isValidQty = true;
                                        }

                                    }
                                }
                            }

                        }

                        if (isValidQty)
                        {
                            SetDetailTax(sender);
                            SetHdrTax();
                            ResetForm(ControlsEnum.TAXPOPUPGRID);
                            //Settings of TaxPayableDiv
                            if (hdfIsTaxPayable.Value.ToString() == "1")
                            {
                                SetTaxPayableDiv();
                            }
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "TaxPayable", "$(document).ready(function () { ShowHideTaxPayableOuter();});", true);
                            //end

                        }
                        else
                        {
                            if (maxQty > 0)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_InvoiceQty").ToString();
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, maxQty);
                                //txtQuantity.Text = Math.Round((Convert.ToDecimal(0)), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString(hdfCurrencyFormat.Value);
                            }
                            else
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_InvoiceQty_Zero").ToString();
                            }
                            (sender as TextBox).Text = maxQty.ToString();
                            SetDetailTax(sender);
                            SetHdrTax();
                            ResetForm(ControlsEnum.TAXPOPUPGRID);
                            //Settings of TaxPayableDiv
                            if (hdfIsTaxPayable.Value.ToString() == "1")
                            {
                                SetTaxPayableDiv();
                            }
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "TaxPayable", "$(document).ready(function () { ShowHideTaxPayableOuter();});", true);
                            //end

                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region SALES INVOICE TYPE CHANGED
                    case ActionsEnum.SALESINVOICETYPECHANGED:
                        if (grdInvoice.Rows.Count > 0 && ddlInvoiceType.SelectedValue != CommonConstants.SELECTVAL)
                        {
                            if (Convert.ToByte(ddlInvoiceType.SelectedValue) == Convert.ToByte(SalesInvoiceType.Proforma))
                            {
                                grdInvoice.HeaderRow.Cells[5].Visible = true;
                                grdInvoice.FooterRow.Cells[5].Visible = true;
                                foreach (GridViewRow grdRow in grdInvoice.Rows)
                                {
                                    grdRow.Cells[5].Visible = true;
                                }
                            }
                            else
                            {
                                grdInvoice.HeaderRow.Cells[5].Visible = false;
                                grdInvoice.FooterRow.Cells[5].Visible = false;
                                foreach (GridViewRow grdRow in grdInvoice.Rows)
                                {
                                    grdRow.Cells[5].Visible = false;
                                }
                            }

                            #region Fill workflow details with invoice type
                            //if (Convert.ToInt32(ddlInvoiceType.SelectedValue) == Convert.ToInt32(SalesInvoiceType.Domestic))
                            //    FillProcessID(3);
                            //else
                            //    FillProcessID(1);
                            FillProcessID(1);

                            WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                            base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                            SetCancelRef(CurrPK);
                            ucrWrkf.FillWorkFlowDetails();
                            #endregion
                        }
                        break;
                    #endregion
                    #region CALCULATE HDR TAX
                    case ActionsEnum.CALCULATEHDRTAX:
                        SetHdrTax();
                        //Settings of TaxPayableDiv
                        if (hdfIsTaxPayable.Value.ToString() == "1")
                        {
                            SetTaxPayableDiv();
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "TaxPayable", "$(document).ready(function () { ShowHideTaxPayableOuter();});", true);
                        //end

                        break;
                    #endregion
                    #region NEW
                    case ActionsEnum.NEW:
                        ResetForm(ControlsEnum.SOINVHEADER);
                        IsDoModified = false;
                        btnRefreshInv.Visible = false;
                        ReloadInvoice = false;
                        FillProcessID(1);
                        EntryStatus = EntryStatus.NEWMODE;
                        TotalPages = 0;
                        uclSOPaging.CurrentPage = 1;
                        hdfIsPendingDOVisible.Value = "0";
                        PageIndexSO = CommonConstants.SELECT_VALUE_ONE;
                        SetFieldValues(ControlsEnum.PAYMENTTERMS);
                        SetFieldValues(ControlsEnum.CUSTOMERTYPES);
                        SetFieldValues(ControlsEnum.INVOICETYPE);
                        SetFieldValues(ControlsEnum.PEDINGSOLIST);
                        GetFieldValues(ControlsEnum.INVOICEGSTTYPE);
                        SetFieldValues(ControlsEnum.INVOICEGSTTYPE);
                        break;
                    #endregion
                    #region EDIT/VIEW/INVOICEDETAIL
                    case ActionsEnum.EDIT:
                    case ActionsEnum.VIEW:
                    case ActionsEnum.INVOICEDETAIL:
                        IsDoModified = false;
                        btnRefreshInv.Visible = false;
                        ReloadInvoice = false;
                        ResetForm(ControlsEnum.SOINVHEADER);
                        foreach (GridViewRow grdrow in grdInvoiceList.Rows)
                        {
                            CheckBox chkInvselect;
                            HiddenField hdfDept;
                            int dept;
                            chkInvselect = (CheckBox)grdrow.FindControl("chkInvselect");
                            if (chkInvselect.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfInvoiceID")).Value);
                                hdfCurrentPk.Value = CurrPK.ToString();
                                hdfInOpeningInv = (HiddenField)grdrow.FindControl("hdfInOpeningInv");
                                if (hdfInOpeningInv.Value == "1")
                                {
                                    Session[ERP.Utilities.SessionStrings.InvoicePK] = CurrPK.ToString();
                                    Response.Redirect(Resources.PageURL.OpeningInvoice);
                                    break;
                                }

                                Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = ((HiddenField)grdrow.FindControl("hdfCustomerPK")).Value;
                                Session[ERP.Utilities.SessionStrings.CUSTOMERNAME] = ((Label)grdrow.FindControl("lblCustomer")).Text;
                                ////start
                                Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                                Posted = Convert.ToBoolean(((HiddenField)grdrow.FindControl("hdfPosted")).Value);
                                invType = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfSalesContractType")).Value);
                                InvoiceType = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfInvType")).Value);
                                if (!string.IsNullOrEmpty(((HiddenField)grdrow.FindControl("hdfDespatchPk")).Value))
                                    DespatchID = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfDespatchPk")).Value);
                                SaleOrderType = invType;
                                ////

                                hdfDept = grdrow.FindControl("hdfDept") as HiddenField;
                                HiddenField hdfInvDtlCount = grdrow.FindControl("hdfInvDtlCount") as HiddenField;
                                if (!string.IsNullOrEmpty(hdfInvDtlCount.Value) && Convert.ToInt32(hdfInvDtlCount.Value) == 0)
                                {
                                    IsDoModified = true;
                                    btnRefreshInv.Visible = true;
                                }
                                if (Convert.ToInt16(((HiddenField)grdrow.FindControl("hdfDelStatus")).Value) == 1)
                                {
                                    btnEditforCancel.Visible = false;
                                    btnSave.Visible = false;
                                    btnEdit.Visible = false;
                                    IsDeleted = true;
                                }
                                else
                                {
                                    btnSave.Visible = true;
                                    btnEdit.Visible = true;
                                    IsDeleted = false;
                                }
                                if (hdfDept != null && int.TryParse(hdfDept.Value, out dept))
                                {
                                    Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                                    base.SetUserDept();
                                }
                                hdfCurrentPk.Value = CurrPK.ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SetPrintDocsVisibility", "$(document).ready(function(){SetPrintDocsVisibility();});", true);
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            //if (IsDeleted)
                            //{
                            //    if (InvoiceType != (int)SalesInvoiceType.Proforma)
                            //    {
                            //        if (InvoiceType == Convert.ToInt32(SalesInvoiceType.Domestic))
                            //            FillProcessID(3);
                            //        else
                            //            FillProcessID(1);
                            //    }
                            //    else
                            //    {
                            //        if (invType == Convert.ToInt32(SalesInvoiceType.Domestic))
                            //            FillProcessID(3);
                            //        else
                            //            FillProcessID(1);
                            //    }
                            //}
                            //else
                            //{
                            //    if (invType == Convert.ToInt32(SalesInvoiceType.Domestic))
                            //        FillProcessID(3);
                            //    else
                            //        FillProcessID(1);
                            //}
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
                                //EntryStatus = EntryStatus.VIEWMODE;

                            }
                            ucrWrkf.ViewAction();

                            ModifiedDatePnl.Visible = true;
                            GetFieldValues(ControlsEnum.INVOICETYPE);
                            SetFieldValues(ControlsEnum.INVOICETYPE);
                            //GetFieldValues(ControlsEnum.SOINVHEADER);
                            //if (SOInvoiceHeaderSession != null)
                            //{
                            //    DespatchID = Convert.ToInt16(SOInvoiceHeaderSession.ICH_DESPATCH_HDR);
                            //}
                            GetFieldValues(ControlsEnum.SOINVHEADER);
                            SetFieldValues(ControlsEnum.SOINVHEADER);
                            GetFieldValues(ControlsEnum.TAXSETTINGS);
                            SetFieldValues(ControlsEnum.SOINVDETAIL);
                            SetFieldValues(ControlsEnum.UPLOADEDFILES);
                            GetFieldValues(ControlsEnum.INVOICEGSTTYPE);
                            //GetFieldValues(ControlsEnum.PAYMENTTERMS);
                            //SetFieldValues(ControlsEnum.PAYMENTTERMS);
                            TempSOInvoiceHeaderSession = SOInvoiceHeaderSession;
                            SetDetailTax(null);
                            SetHdrTax();
                            if (grdInvoice.Rows.Count > 0)
                            {
                                SetSubTotal();
                            }
                            //Settings of TaxPayableDiv
                            if (hdfIsTaxPayable.Value.ToString() == "1")
                            {
                                SetTaxPayableDiv();
                            }
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "TaxPayable", "$(document).ready(function () { ShowHideTaxPayableOuter();});", true);
                            //end
                            uclSOPaging.CurrentPage = 1;
                            PageIndexSO = CommonConstants.SELECT_VALUE_ONE;
                            GetFieldValues(ControlsEnum.PEDINGSOLIST);
                            SetFieldValues(ControlsEnum.PEDINGSOLIST);
                            hdfIsPendingDOVisible.Value = "0";

                            if (IsDoModified)
                                ResetForm(ControlsEnum.RESETFORMODIFIEDDO);


                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Invoice").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region Print listing
                    case ActionsEnum.PRINTLISTING:
                        foreach (GridViewRow grdrow in grdInvoiceList.Rows)
                        {
                            CheckBox chkInvselect;
                            HiddenField hdfDept;
                            int dept;
                            chkInvselect = (CheckBox)grdrow.FindControl("chkInvselect");
                            if (chkInvselect.Checked)
                            {
                                hdfInOpeningInv = (HiddenField)grdrow.FindControl("hdfInOpeningInv");
                                if (hdfInOpeningInv.Value == "1")
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_Err_Opening_Print").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                    return;
                                }

                                if (Convert.ToInt16(((HiddenField)grdrow.FindControl("hdfDelStatus")).Value) == 1)
                                {
                                    btnEditforCancel.Visible = false;
                                    btnSave.Visible = false;
                                    btnEdit.Visible = false;
                                }
                                else
                                {
                                    btnSave.Visible = true;
                                    btnEdit.Visible = true;
                                }
                                hdfDept = grdrow.FindControl("hdfDept") as HiddenField;
                                if (hdfDept != null && int.TryParse(hdfDept.Value, out dept))
                                {
                                    Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                                    base.SetUserDept();
                                }
                                InvoiceType = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfInvType")).Value);
                                if (InvoiceType > 0)
                                {
                                    if (InvoiceType == (int)SalesInvoiceType.Domestic)
                                    {
                                        if (IsExportExcel != true)
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                                                ((HiddenField)grdrow.FindControl("hdfInvoiceID")).Value + "&APPTYPE=" + ApplicationType.DSI + "&APPSUBTYPE=" + (int)SalesInvoiceType.Domestic) + "');", true);
                                        }
                                        else
                                        {
                                            Response.Redirect(Resources.PageURL.ReportUrl + "?ID=" +
                                                ((HiddenField)grdrow.FindControl("hdfInvoiceID")).Value + "&APPTYPE=" + ApplicationType.DSI + "&APPSUBTYPE=" + (int)SalesInvoiceType.Domestic);
                                        }
                                    }
                                    else if (InvoiceType == (int)SalesInvoiceType.Export)
                                    {
                                        if (IsExportExcel != true)
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                                                ((HiddenField)grdrow.FindControl("hdfInvoiceID")).Value + "&APPTYPE=" + ApplicationType.DSI + "&APPSUBTYPE=" + (int)SalesInvoiceType.Export) + "');", true);
                                        }
                                        else
                                        {
                                            Response.Redirect(Resources.PageURL.ReportUrl + "?ID=" +
                                                ((HiddenField)grdrow.FindControl("hdfInvoiceID")).Value + "&APPTYPE=" + ApplicationType.DSI + "&APPSUBTYPE=" + (int)SalesInvoiceType.Export);
                                        }
                                    }
                                    else if (InvoiceType == (int)SalesInvoiceType.Proforma)
                                    {
                                        if (IsExportExcel != true)
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                                                ((HiddenField)grdrow.FindControl("hdfInvoiceID")).Value + "&APPTYPE=" + ApplicationType.DSI + "&APPSUBTYPE=" + (int)SalesInvoiceType.Proforma) + "');", true);
                                        }
                                        else
                                        {
                                            Response.Redirect(Resources.PageURL.ReportUrl + "?ID=" +
                                                ((HiddenField)grdrow.FindControl("hdfInvoiceID")).Value + "&APPTYPE=" + ApplicationType.DSI + "&APPSUBTYPE=" + (int)SalesInvoiceType.Proforma);
                                        }
                                    }
                                }
                                return;
                            }
                        }
                        litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Invoice").ToString();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        break;
                    #endregion
                    #region Delete
                    case ActionsEnum.DELETE:
                        if (CurrPK > 0)
                        {
                            result = 0;
                            result = BusinessLogic.Sales.SalesInvoiceBL.DeleteDirectSalesInvoiceDetails(currentUser.PKUser.ToString(), CurrPK, LastModifiedTime, null, ApplicationType.DSI);
                            if (result > 0) // Success ! re-initialize the page
                            {
                                //Show Save success message and reset Contract Entry
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SalesInvoice);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                EntryStatus = EntryStatus.LISTMODE;
                                ResetForm(ControlsEnum.INVOICELIST);
                                GetFieldValues(ControlsEnum.INVOICELIST);
                                SetFieldValues(ControlsEnum.INVOICELIST);
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
                                    litErrorMsg.Text = Resources.PageNameRes.SalesInvoice + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    EntryStatus = EntryStatus.LISTMODE;
                                    ResetForm(ControlsEnum.INVOICELIST);
                                    GetFieldValues(ControlsEnum.INVOICELIST);
                                    SetFieldValues(ControlsEnum.INVOICELIST);
                                }
                                else if (result == (int)DbDeleteStatus.REFERRED)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.SalesInvoice + " " + Resources.Messages.UsedInAnotherPlace;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.SalesInvoice + " " + Resources.Messages.AlreadyDeleted;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    EntryStatus = EntryStatus.LISTMODE;
                                    ResetForm(ControlsEnum.INVOICELIST);
                                    GetFieldValues(ControlsEnum.INVOICELIST);
                                    SetFieldValues(ControlsEnum.INVOICELIST);
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SalesInvoice);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                        }
                        break;
                    #endregion
                    #region Inactive
                    case ActionsEnum.INACTIVE:
                        if (CurrPK > 0)
                        {
                            GetFieldValues(ControlsEnum.SOINVHEADER);
                            if (SOInvoiceHeaderSession != null)
                            {
                                DespatchID = Convert.ToInt16(SOInvoiceHeaderSession.ICH_DESPATCH_HDR);
                            }
                            GetFieldValues(ControlsEnum.SOINVHEADER);
                            if (invoiceHeaderObj != null)
                            {
                                if (invoiceHeaderObj.ICH_STATUS > 0)
                                {
                                    if (invoiceHeaderObj.ICH_HAS_JRNL_ENTRY)
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("Msg_Already_Journalized").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    }
                                    else if (invoiceHeaderObj.ICH_AMOUNT_RCVD_TC > 0)
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("Msg_Already_Received").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    }
                                    else if (invoiceHeaderObj.ICH_AMOUNT_DN_TC > 0)
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("Msg_Already_Debit_Adjusted").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    }
                                    else if (invoiceHeaderObj.ICH_AMOUNT_CN_TC > 0)
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("Msg_Already_Credit_Adjusted").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    }
                                    else
                                    {
                                        result = 0;
                                        string reasonForDelete = HttpUtility.HtmlEncode(txtReason.Text.Trim());
                                        result = BusinessLogic.Sales.SalesInvoiceBL.DeleteSalesInvoiceDetails(currentUser.PKUser.ToString(), CurrPK, LastModifiedTime, reasonForDelete, ApplicationType.DSI);
                                        if (result > 0) // Success ! re-initialize the page
                                        {
                                            //Show Save success message and reset Contract Entry
                                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SalesInvoice);
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                            EntryStatus = EntryStatus.LISTMODE;
                                            ResetForm(ControlsEnum.INVOICELIST);
                                            GetFieldValues(ControlsEnum.INVOICELIST);
                                            SetFieldValues(ControlsEnum.INVOICELIST);
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
                                                litErrorMsg.Text = Resources.PageNameRes.SalesInvoice + " " + Resources.Messages.EditUsedByAnotherUser;
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                    + "','" + Resources.ErpRes.Information + "');", true);
                                                EntryStatus = EntryStatus.LISTMODE;
                                                ResetForm(ControlsEnum.INVOICELIST);
                                                GetFieldValues(ControlsEnum.INVOICELIST);
                                                SetFieldValues(ControlsEnum.INVOICELIST);
                                            }
                                            else if (result == (int)DbDeleteStatus.REFERRED)
                                            {
                                                litErrorMsg.Text = Resources.PageNameRes.SalesInvoice + " " + Resources.Messages.UsedInAnotherPlace;
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                            }
                                            else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                            {
                                                litErrorMsg.Text = Resources.PageNameRes.SalesInvoice + " " + Resources.Messages.AlreadyDeleted;
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                    + "','" + Resources.ErpRes.Information + "');", true);
                                                EntryStatus = EntryStatus.LISTMODE;
                                                ResetForm(ControlsEnum.INVOICELIST);
                                                GetFieldValues(ControlsEnum.INVOICELIST);
                                                SetFieldValues(ControlsEnum.INVOICELIST);
                                            }
                                            else
                                            {
                                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SalesInvoice);
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                    + "','" + Resources.ErpRes.Information + "');", true);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    result = 0;
                                    result = BusinessLogic.Sales.SalesInvoiceBL.DeleteSalesInvoiceDetails(currentUser.PKUser.ToString(), CurrPK, LastModifiedTime, null, ApplicationType.DSI);
                                    if (result > 0) // Success ! re-initialize the page
                                    {
                                        //Show Save success message and reset Contract Entry
                                        litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SalesInvoice);
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                        EntryStatus = EntryStatus.LISTMODE;
                                        ResetForm(ControlsEnum.INVOICELIST);
                                        GetFieldValues(ControlsEnum.INVOICELIST);
                                        SetFieldValues(ControlsEnum.INVOICELIST);
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
                                            litErrorMsg.Text = Resources.PageNameRes.SalesInvoice + " " + Resources.Messages.EditUsedByAnotherUser;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                            EntryStatus = EntryStatus.LISTMODE;
                                            ResetForm(ControlsEnum.INVOICELIST);
                                            GetFieldValues(ControlsEnum.INVOICELIST);
                                            SetFieldValues(ControlsEnum.INVOICELIST);
                                        }
                                        else if (result == (int)DbDeleteStatus.REFERRED)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.SalesInvoice + " " + Resources.Messages.UsedInAnotherPlace;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                        else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.SalesInvoice + " " + Resources.Messages.AlreadyDeleted;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                            EntryStatus = EntryStatus.LISTMODE;
                                            ResetForm(ControlsEnum.INVOICELIST);
                                            GetFieldValues(ControlsEnum.INVOICELIST);
                                            SetFieldValues(ControlsEnum.INVOICELIST);
                                        }
                                        else
                                        {
                                            litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SalesInvoice);
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                    }
                                }
                            }
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "closedeletepopup", "$(document).ready(function(){closeDeletePopup();});", true);
                        break;
                    #endregion
                    #region Clear
                    case ActionsEnum.CLEAR:
                        SelectedSalesInvoicesInfoLst = null;
                        ResetForm(ControlsEnum.INVOICELIST);
                        GetFieldValues(ControlsEnum.INVOICELIST);
                        SetFieldValues(ControlsEnum.INVOICELIST);
                        break;
                    #endregion
                    #region INVOICE LIST
                    case ActionsEnum.INVOICELIST:
                        FillProcessID(1);
                        CurrPK = 0;
                        hdfCurrentPk.Value = CurrPK.ToString();
                        hdfIVHPK.Value = "";
                        ResetForm(ControlsEnum.INVOICELIST);
                        GetFieldValues(ControlsEnum.INVOICELIST);
                        SetFieldValues(ControlsEnum.INVOICELIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion
                    #region SUBMIT
                    case ActionsEnum.SUBMIT:
                        isContinue = true;
                        if ((hdfSaveWithoutAllocation.Value == "0") || (hdfSaveWithoutAllocation.Value == ""))
                        {
                            TempSOInvoiceHeaderSession = SOInvoiceHeaderSession;
                            deductionDtlList = new List<DirectSOAdvDeductionDetails>();
                            List<DirectSOAdvDeductionDetails> deductionDtlListNonZero = new List<DirectSOAdvDeductionDetails>();
                            deductionDtlListNonZero = TempSOInvoiceHeaderSession.DeductionDetails.ToList();
                            foreach (DirectSOAdvDeductionDetails itm in deductionDtlListNonZero)
                            {
                                deductionDtlList.Add(itm);
                            }
                            if (SaleOrderType == 1 && IsAdvInvHasTax)
                            {
                                if (Convert.ToDouble(txtHdrDeduction.Text) <= 0 && (deductionDtlList != null && deductionDtlList.Count > 0))
                                {
                                    isContinue = false;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop3", "ShowSaveWithoutAllocationConfirm('" + (sender as Button).ID + "');", true);
                                }
                            }
                            else
                            {
                                if (Convert.ToDouble(txtTotalDeductionExp.Text) <= 0 && (deductionDtlList != null && deductionDtlList.Count > 0))
                                {
                                    isContinue = false;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop3", "ShowSaveWithoutAllocationConfirm('" + (sender as Button).ID + "');", true);
                                }
                            }
                        }
                        if (isContinue)
                        {
                            //Show WorkFlow Popup   
                            hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                            ucrWrkf.Visible = true;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        }
                        break;
                    #endregion
                    #region SAVE SUBMIT
                    case ActionsEnum.SAVESUBMIT:
                        //Show WorkFlow Popup
                        isContinue = true;

                        if (ddlCustomerType.Items.Count <= 0)
                        {

                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_ReqType").ToString()) + "');", true);
                            return;
                        }

                        if ((hdfSaveWithoutAllocation.Value == "0") || (hdfSaveWithoutAllocation.Value == ""))
                        {
                            TempSOInvoiceHeaderSession = SOInvoiceHeaderSession;
                            deductionDtlList = new List<DirectSOAdvDeductionDetails>();
                            //For Avoiding Zero in allocate now field
                            //deductionDtlList = TempSOInvoiceHeaderSession.DeductionDetails.ToList();
                            List<DirectSOAdvDeductionDetails> deductionDtlListNonZero = new List<DirectSOAdvDeductionDetails>();
                            //if (SaleOrderType == 2)
                            //{
                            //    hdfIsCusAllAdv.Value = CommonConstants.SELECT_VALUE_ONE;
                            //    GetFieldValues(ControlsEnum.SOINVHEADER);
                            //    deductionDtlListNonZero = TempSOInvoiceHeaderSessionCustAll.DeductionDetails.ToList();
                            //}
                            //else
                            //{
                            deductionDtlListNonZero = TempSOInvoiceHeaderSession.DeductionDetails.ToList();
                            // }
                            foreach (DirectSOAdvDeductionDetails itm in deductionDtlListNonZero)
                            {
                                //if (itm.IAD_AMOUNT > 0)
                                //{
                                deductionDtlList.Add(itm);
                                //}
                            }

                            if (SaleOrderType == 1 && IsAdvInvHasTax)
                            {
                                if (Convert.ToDouble(txtHdrDeduction.Text) <= 0 && (deductionDtlList != null && deductionDtlList.Count > 0))
                                {
                                    isContinue = false;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop3", "ShowSaveWithoutAllocationConfirm('" + (sender as Button).ID + "');", true);
                                }
                            }
                            else
                            {
                                if (Convert.ToDouble(txtTotalDeductionExp.Text) <= 0 && (deductionDtlList != null && deductionDtlList.Count > 0))
                                {
                                    isContinue = false;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop3", "ShowSaveWithoutAllocationConfirm('" + (sender as Button).ID + "');", true);
                                }
                            }


                        }
                        if (isContinue)
                        {
                            hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ONE;
                            ucrWrkf.Visible = true;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        }
                        break;
                    #endregion
                    #region WRKF SUBMIT
                    case ActionsEnum.WRKFSUBMIT:
                        //Submit Activity
                        //validate Page
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        else//valid
                        {
                            string invoiceNo = string.Empty;

                            if (grdInvoice.Rows.Count > 0)
                            {
                                ucrWrkf.ApplicationID = 0;
                                if (hdfIsSaveSubmit.Value == CommonConstants.SELECT_VALUE_ONE)
                                {
                                    hasValidRate = false;
                                    invoiceHeaderObj = new DirectSOInvoiceHeader();
                                    invoiceHeaderObj = (DirectSOInvoiceHeader)SetUIValuesToObject(ControlsEnum.SOINVHEADER);
                                    invoiceHeaderObj.WKF_FLAG = 1;
                                    if (hdfExchangeRate.Value != "-1")
                                    {
                                        // save Process Control inspection details
                                        if (hasValidRate)
                                        {
                                            if (invoiceHeaderObj != null && invoiceHeaderObj.OrderDetail != null)
                                            {
                                                invoiceHeaderObj.ATL_ACTION = (byte)LogAction.NEW;
                                                SaveTransaction(invoiceHeaderObj, Convert.ToInt32(WorkflowTransactionFlag.SAVEANDSUBMIT), sender);
                                            }
                                        }
                                        else
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Empty_Rate").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                                            return;
                                        }
                                        hasValidRate = false;
                                    }
                                    else
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                                                   "ClosePopup();", true);

                                        litErrorMsg.Text = Resources.Messages.Msg_ErrConversionFactor;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                        return;
                                    }
                                }
                                else if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)
                                {
                                    if (BusinessLogic.CommonManagement.CommonBL.ValidationForCancellation(CurrPK, ApplicationType.DSI))
                                    {
                                        isCancelled = true;
                                        SaveTransaction(null, Convert.ToInt32(WorkflowTransactionFlag.SUBMIT), sender);
                                    }
                                    else
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("Err_SI_Cancel").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                        WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                                        if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)
                                            FillProcessID(1);
                                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                                        WrkfComments.Text = "";
                                        EntryStatus = EntryStatus.LISTMODE;
                                        ResetForm(ControlsEnum.INVOICELIST);
                                        GetFieldValues(ControlsEnum.INVOICELIST);
                                        SetFieldValues(ControlsEnum.INVOICELIST);
                                        SOInvoiceHeaderSession = null;
                                    }
                                }
                                else
                                    SaveTransaction(null, Convert.ToInt32(WorkflowTransactionFlag.SUBMIT), sender);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save_emptygrid").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                            }

                        }
                        break;
                    #endregion
                    #region SEARCH
                    case ActionsEnum.SEARCH:
                        PageIndex = CommonConstants.SELECT_VALUE_ZERO;
                        SelectedSalesInvoicesInfoLst = null;
                        EntryStatus = EntryStatus.LISTMODE;
                        GetFieldValues(ControlsEnum.INVOICELIST);
                        SetFieldValues(ControlsEnum.INVOICELIST);
                        break;
                    #endregion
                    #region Reset
                    case ActionsEnum.RESET:
                        SelectedCurrency = 0;
                        SelectedCustomers = 0;
                        SelectedSalesInvoices = null;
                        SelectedSalesInvoicesInfoLst = null;
                        SelectedInvoiceType = 0;
                        SelectedINVTax = null;
                        SelectedINVTaxList = new List<decimal>();
                        SelectedInvoicesCrDr = null;
                        //Resetting Color
                        hdfSelectedItemPk.Value = "0";
                        break;
                    #endregion
                    #region Journalize
                    case ActionsEnum.JOURNALIZE:
                        hdfJournalizeWorkFlow.Value = "0";
                        finInvoiceCusHdrObj = CommonFunctions.Initilize<FIN_INVOICE_CUS_HDR>();
                        finInvoiceCusHdrObj.ICH_PK = CurrPK;
                        SetUIValuesToObject(ControlsEnum.JOURNALIZE);
                        break;
                    #endregion
                    #region Journalize Update
                    case ActionsEnum.JOURNALIZEUPDATE:
                        ucrJournalize.ResetForm();
                        hdfJournalizeWorkFlow.Value = "0";
                        EntryStatus = EntryStatus.LISTMODE;
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        ucrWrkf.Reset();
                        FillProcessID(1);
                        ResetForm(ControlsEnum.INVOICELIST);
                        GetFieldValues(ControlsEnum.INVOICELIST);
                        SetFieldValues(ControlsEnum.INVOICELIST);
                        break;
                    #endregion
                    #region Journalize Save
                    case ActionsEnum.JOURNALIZESAVE:
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        FillProcessID(1);
                        ResetForm(ControlsEnum.INVOICELIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        GetFieldValues(ControlsEnum.INVOICELIST);
                        SetFieldValues(ControlsEnum.INVOICELIST);
                        break;
                    #endregion
                    #region Journalize Submit
                    case ActionsEnum.JOURNALIZESUBMIT:
                        hdfJournalizeWorkFlow.Value = "0";
                        if (Session[ERP.Utilities.SessionStrings.Transaction] != null)
                        {
                            string Transaction = Session[ERP.Utilities.SessionStrings.Transaction].ToString();
                            if (Transaction == "SAVE")
                            {
                                salesInvoiceServiceClient = new SalesInvoiceService();
                                salesInvoiceServiceClient = CommonFunctions.InitiateClient(salesInvoiceServiceClient);
                                result = (int)salesInvoiceServiceClient.UpdateInvoiceHdrJounalizeFlag(int.Parse(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString()), true);
                            }
                        }

                        ucrWrkf.Reset();
                        FillProcessID(1);
                        ResetForm(ControlsEnum.INVOICELIST);
                        //if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && ReferanceID > 0)
                        //{
                        //    Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        //    Response.Redirect(Resources.PageURL.InboxURL);
                        //}
                        //else
                        //{
                        EntryStatus = EntryStatus.LISTMODE;
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        GetFieldValues(ControlsEnum.INVOICELIST);
                        SetFieldValues(ControlsEnum.INVOICELIST);
                        //}
                        break;
                    #endregion
                    #region Journalize Delete
                    case ActionsEnum.JOURNALIZEDELETE:
                        if (Session[ERP.Utilities.SessionStrings.Transaction] != null)
                        {
                            string Transaction = Session[ERP.Utilities.SessionStrings.Transaction].ToString();
                            if (Transaction == "DELETE")
                            {
                                salesInvoiceServiceClient = new SalesInvoiceService();
                                salesInvoiceServiceClient = CommonFunctions.InitiateClient(salesInvoiceServiceClient);
                                result = (int)salesInvoiceServiceClient.UpdateInvoiceHdrJounalizeFlag(int.Parse(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString()), false);
                            }
                        }
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        FillProcessID(1);
                        ResetForm(ControlsEnum.INVOICELIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        GetFieldValues(ControlsEnum.INVOICELIST);
                        SetFieldValues(ControlsEnum.INVOICELIST);
                        break;
                    #endregion
                    #region Journalize Cancel
                    case ActionsEnum.JOURNALIZECANCEL:
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        FillProcessID(1);
                        ResetForm(ControlsEnum.INVOICELIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        GetFieldValues(ControlsEnum.INVOICELIST);
                        SetFieldValues(ControlsEnum.INVOICELIST);
                        break;
                    #endregion
                    #region PRINT
                    case ActionsEnum.PRINT:
                        if (hdfType.Value != string.Empty)
                        {
                            if (hdfType.Value.ToString().ToLower() == "domestic")
                            {
                                if (IsExportExcel != true)
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.DSI + "&APPSUBTYPE=1") + "');", true);
                                }
                                else
                                {
                                    Response.Redirect(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.DSI + "&APPSUBTYPE=" + (int)SalesInvoiceType.Domestic);
                                }
                            }
                            else if (hdfType.Value.ToString().ToLower() == "export")
                            {
                                if (IsExportExcel != true)
                                {

                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.DSI + "&APPSUBTYPE=2") + "');", true);
                                }
                                else
                                {
                                    Response.Redirect(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.DSI + "&APPSUBTYPE=" + (int)SalesInvoiceType.Export);
                                }
                            }
                            else if (hdfType.Value.ToString().ToLower() == "proforma")
                            {
                                if (IsExportExcel != true)
                                {

                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.DSI + "&APPSUBTYPE=3") + "');", true);
                                }
                                else
                                {
                                    Response.Redirect(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.DSI + "&APPSUBTYPE=" + (int)SalesInvoiceType.Proforma);
                                }
                            }
                        }
                        break;
                    #endregion
                    #region EXCEL PRINT
                    case ActionsEnum.EXCELPRINT:
                        if (hdfType.Value != string.Empty)
                        {
                            if (hdfType.Value.ToString().ToLower() == "domestic")
                            {
                                if (IsExportExcel != true)
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.DSI + "&APPSUBTYPE=1" + "&ISEXCELPRINT=1") + "');", true);
                                }
                                else
                                {
                                    Response.Redirect(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.DSI + "&APPSUBTYPE=" + (int)SalesInvoiceType.Domestic + "&ISEXCELPRINT=1");
                                }
                            }
                            else if (hdfType.Value.ToString().ToLower() == "export")
                            {
                                if (IsExportExcel != true)
                                {

                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.DSI + "&APPSUBTYPE=2" + "&ISEXCELPRINT=1") + "');", true);
                                }
                                else
                                {
                                    Response.Redirect(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.DSI + "&APPSUBTYPE=" + (int)SalesInvoiceType.Export + "&ISEXCELPRINT=1");
                                }
                            }
                            else if (hdfType.Value.ToString().ToLower() == "proforma")
                            {
                                if (IsExportExcel != true)
                                {

                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.DSI + "&APPSUBTYPE=3" + "&ISEXCELPRINT=1") + "');", true);
                                }
                                else
                                {
                                    Response.Redirect(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.DSI + "&APPSUBTYPE=" + (int)SalesInvoiceType.Proforma + "&ISEXCELPRINT=1");
                                }
                            }
                        }
                        break;
                    #endregion
                    #region Alert
                    case ActionsEnum.ALERT:
                        ucrAlert.TypeCode = ApplicationType.DSI;
                        ucrAlert.TypePK = CurrPK;
                        ucrAlert.TypeRef = lblInvoiceNo.Text.Trim();
                        ucrAlert.TrxDate = string.IsNullOrEmpty(txtInvoiceDate.Text.Trim()) ? DateTime.Now : Convert.ToDateTime(txtInvoiceDate.Text.Trim());
                        ucrAlert.TypeText = GetLocalResourceObject("Alert_Type_Text").ToString();
                        ucrAlert.TypePartyName = txtCustomer.Text;
                        ucrAlert.GetAlertList();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2", "ShowContainerDiv('[id$=divAlert]','Alert','800','400');", true);
                        break;
                    #endregion
                    #region Deduction Popup
                    case ActionsEnum.DEDUCTIONHEADER:
                        if (grdInvoice.Rows.Count <= 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_NoItems").ToString()) + "');", true);
                            return;
                        }

                        if (SaleOrderType == 1)//Domestic
                        {
                            // chkDedAll.Checked = false;
                            hdfIsCusAllAdv.Value = CommonConstants.SELECT_VALUE_ZERO;
                        }
                        else //Export  //show all pending advance deduction related to this customer
                        {

                            hdfIsCusAllAdv.Value = CommonConstants.SELECT_VALUE_ONE;

                            if (TempSOInvoiceHeaderSessionCustAll == null)
                                GetFieldValues(ControlsEnum.SOINVHEADER);

                            //if (SOInvoiceHeaderSession != null)
                            //{
                            //    DespatchID = Convert.ToInt16(SOInvoiceHeaderSession.ICH_DESPATCH_HDR);
                            //}
                            //GetFieldValues(ControlsEnum.SOINVHEADER);
                            //deductionDtlListCustAll = new List<DirectSOAdvDeductionDetails>();
                            //if (TempSOInvoiceHeaderSessionCustAll != null)
                            //{
                            //    if (TempSOInvoiceHeaderSessionCustAll.DeductionDetails != null && TempSOInvoiceHeaderSessionCustAll.DeductionDetails.Count > 0)
                            //    {
                            //        deductionDtlListCustAll = TempSOInvoiceHeaderSessionCustAll.DeductionDetails;
                            //    }
                            //}
                        }
                        if (SOInvoiceHeaderSession != null && grdInvoice.Rows.Count > 0)
                        {
                            TempSOInvoiceHeaderSession = SOInvoiceHeaderSession;
                            //lblDedSaleOrderNo.Text = ERP.Utilities.CommonFunctions.GetShortString(lnkDoNo.ToolTip, 19);
                            //lblDedSaleOrderNo.ToolTip = lnkDoNo.ToolTip;
                            lblDedCustomer.Text = ERP.Utilities.CommonFunctions.GetShortString(txtCustomer.Text, 26);
                            lblDedCustomer.ToolTip = txtCustomer.Text;
                            //lblDedSaleOrderDate.Text = ERP.Utilities.CommonFunctions.GetShortString(lblSODateTxt.ToolTip, 15);
                            //lblDedSaleOrderDate.ToolTip = lblSODateTxt.ToolTip;
                            lblDedInvoiceNo.Text = ERP.Utilities.CommonFunctions.GetShortString(lblInvoiceNo.Text, 15);
                            lblDedInvoiceNo.ToolTip = lblInvoiceNo.Text;
                            lblDedSaleInvoiceDate.Text = ERP.Utilities.CommonFunctions.GetShortString(txtInvoiceDate.Text, 15);
                            lblDedSaleInvoiceDate.ToolTip = txtInvoiceDate.Text;
                            lblDedCurrency.Text = ERP.Utilities.CommonFunctions.GetShortString(txtCurrency.Text, 15);
                            //lblDedCurrency.ToolTip = lblCurrencyTxt.ToolTip;
                            SetFieldValues(ControlsEnum.DEDUCTIONPOPUPGRID);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2", "ShowContainerDiv('[id$=divDeduction]','" + GetLocalResourceObject("DeductionDetails").ToString() + "','900','400');", true);
                            //EditTaxOtherCharge Setting
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "EditTaxOtherCharge", "$(document).ready(function () { EnableDisableTaxOtherCharge();});", true);
                            //For Setting Colour for Current SC Advance Invoice (deduct)
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowCurrentAdvInvoice", "$(document).ready(function(){SetCurrentAdvInvoiceRowColor();});", true);
                            //End
                        }
                        else
                        {
                            if (grdInvoice.Rows.Count <= 0)
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_NoItems").ToString()) + "');", true);
                        }
                        //ActionHandler(ActionsEnum.CHECKEDCHANGED, new EventArgs());
                        break;
                    #endregion
                    #region DEDUCTION APPLY
                    case ActionsEnum.DEDUCTIONAPPLY:
                        //ResetForm(ControlsEnum.TAXPOPUPGRID);                      


                        bool close = true;
                        if (SOInvoiceHeaderSession != null)
                        {
                            hdfIsDedApplyClick.Value = "1";

                            //********************** Start IF CustomerAllPending Allocation Checkboc Checked******************** 
                            if (hdfIsCusAllAdv.Value == CommonConstants.SELECT_VALUE_ONE)//Set CustAllAdvDeduction Details 
                            {
                                if (TempSOInvoiceHeaderSessionCustAll != null)
                                {
                                    TempSOInvoiceHeaderSession = TempSOInvoiceHeaderSessionCustAll;
                                    if (SOInvoiceHeaderSession.TaxHdr != null)
                                        TempSOInvoiceHeaderSession.TaxHdr = SOInvoiceHeaderSession.TaxHdr;
                                }
                            }
                            //Resetting Chkbox And hiddenfiled
                            chkDedAll.Checked = false;
                            hdfIsCusAllAdv.Value = CommonConstants.SELECT_VALUE_ZERO;
                            //****************************End CustomerAllPending *************************

                            //Reset all line item tax and discount
                            SOInvoiceHeaderSession = TempSOInvoiceHeaderSession;
                            if (SaleOrderType == 1 && IsAdvInvHasTax)
                            {
                                if (SOInvoiceHeaderSession.OrderDetail != null && SOInvoiceHeaderSession.OrderDetail.Count > 0)
                                {
                                    foreach (DirectSOInvoiceDetails dtl in SOInvoiceHeaderSession.OrderDetail)
                                    {
                                        SOInvoicePK = string.IsNullOrEmpty(dtl.CID_PK.ToString()) ? 0 : Convert.ToInt32(dtl.CID_PK);
                                        SelectedItemPK = string.IsNullOrEmpty(dtl.CID_ITEM.ToString()) ? 0 : Convert.ToInt32(dtl.CID_ITEM);
                                        ScDetailsPK = string.IsNullOrEmpty(dtl.CID_SO_DTL.ToString()) ? 0 : Convert.ToInt32(dtl.CID_SO_DTL);
                                        ReSetDetailTax(dtl.CID_AMOUNT);
                                    }
                                }
                                ReSetHdrDisc();
                            }
                            if (grdDeduction.Rows.Count > 0)
                            {
                                HiddenField hdfDedTotalAllocateNowFooterSplit = grdDeduction.FooterRow.FindControl("hdfDedTotalAllocateNowFooterSplit") as HiddenField;
                                if (hdfDedTotalAllocateNowFooterSplit != null && !string.IsNullOrEmpty(hdfDedTotalAllocateNowFooterSplit.Value))
                                {
                                    List<DirectSOAdvDeductionDetails> TempsoAdvDeductionDetailsList = null;
                                    //int DedCnt = 0;
                                    double shipping = 0;
                                    double adjPrice = 0;
                                    double otherCharges = 0;
                                    double tax = 0;
                                    HiddenField hdfOtherTotalFooterSplit = grdDeduction.FooterRow.FindControl("hdfOtherTotalFooterSplit") as HiddenField;
                                    HiddenField hdfTaxTotalFooterSplit = grdDeduction.FooterRow.FindControl("hdfTaxTotalFooterSplit") as HiddenField;
                                    double grossAmt = 0;
                                    double deduction = 0;
                                    double deductionExp = 0;


                                    if (hdfOtherTotalFooterSplit != null && !string.IsNullOrEmpty(hdfOtherTotalFooterSplit.Value))

                                        double.TryParse(hdfOtherTotalFooterSplit.Value.Trim(), out otherCharges);

                                    if (hdfTaxTotalFooterSplit != null && !string.IsNullOrEmpty(hdfTaxTotalFooterSplit.Value))

                                        double.TryParse(hdfTaxTotalFooterSplit.Value.Trim(), out tax);


                                    if (SaleOrderType == 1 && IsAdvInvHasTax) // tax enabled for advance invoice
                                    {
                                        double.TryParse(txtHdrTotal.Text.Trim(), out grossAmt);
                                    }
                                    else
                                    {
                                        double.TryParse(txtTotalExp.Text.Trim(), out grossAmt);
                                    }

                                    double.TryParse(hdfDedTotalAllocateNowFooterSplit.Value.Replace(",", ""), out deduction);
                                    double.TryParse(hdfDedTotalAllocateNowFooterSplit.Value.Replace(",", ""), out deductionExp);

                                    double.TryParse(txtShipping.Text.Trim(), out shipping);
                                    double.TryParse(txtPriceAdj.Text.Trim(), out adjPrice);
                                    // grossAmt += shipping + adjPrice;

                                    deduction = deduction - (tax + otherCharges);

                                    if (otherCharges > 0) { IsOCded = false; } else { IsOCded = true; }

                                    deduction = Math.Round(deduction, 2);
                                    TempsoAdvDeductionDetailsList = new List<DirectSOAdvDeductionDetails>();
                                    decimal amtAdjAdvDedTotal = 0;
                                    if (deduction > grossAmt)
                                    {

                                        foreach (GridViewRow grdRow in grdDeduction.Rows)
                                        {
                                            //DedCnt = DedCnt + 1;
                                            // HiddenField hdfCurPaidOtherAmount = grdRow.FindControl("hdfCurPaidOtherAmount") as HiddenField;
                                            //HiddenField hdfCurPaidTax = grdRow.FindControl("hdfCurPaidTax") as HiddenField;
                                            //HiddenField hdfCurPaidDisc = grdRow.FindControl("hdfCurPaidDisc") as HiddenField;

                                            HiddenField hdfDeductionPK = grdRow.FindControl("hdfDeductionPK") as HiddenField;
                                            HiddenField hdfReceiptPK = grdRow.FindControl("hdfReceiptPK") as HiddenField;
                                            HiddenField hdfDedInvoicePK = grdRow.FindControl("hdfDedInvoicePK") as HiddenField;
                                            HiddenField hdfReceiptDTLPK = grdRow.FindControl("hdfReceiptDTLPK") as HiddenField;
                                            HiddenField hdfDedSoPk = grdRow.FindControl("hdfDedSoPk") as HiddenField;
                                            HiddenField hdfScPk = grdRow.FindControl("hdfScPk") as HiddenField;
                                            HiddenField hdfIcmSoHdr = grdRow.FindControl("hdfIcmSoHdr") as HiddenField;

                                            long deductionPk = hdfDeductionPK != null && !string.IsNullOrEmpty(hdfDeductionPK.Value.Trim())
                                                 ? Convert.ToInt64(hdfDeductionPK.Value) : 0;
                                            long receiptPK = hdfReceiptPK != null && !string.IsNullOrEmpty(hdfReceiptPK.Value.Trim())
                                                ? Convert.ToInt64(hdfReceiptPK.Value) : 0;
                                            long invPK = hdfDedInvoicePK != null && !string.IsNullOrEmpty(hdfDedInvoicePK.Value.Trim())
                                                ? Convert.ToInt64(hdfDedInvoicePK.Value) : 0;
                                            long SoPK = hdfDedSoPk != null && !string.IsNullOrEmpty(hdfDedSoPk.Value.Trim())
                                                ? Convert.ToInt64(hdfDedSoPk.Value) : 0;

                                            int ScPK = hdfScPk != null && !string.IsNullOrEmpty(hdfScPk.Value.Trim())
                                                ? Convert.ToInt32(hdfScPk.Value) : 0;
                                            int IcmSoHdr = hdfIcmSoHdr != null && !string.IsNullOrEmpty(hdfIcmSoHdr.Value.Trim())
                                               ? Convert.ToInt32(hdfIcmSoHdr.Value) : 0;
                                            ReceiptMpgPK = Convert.ToInt32(hdfReceiptDTLPK.Value);

                                            List<DirectSOAdvDeductionDetails> soAdvDeductList = null;

                                            TextBox txtDedAllocateNowSplit = grdRow.FindControl("txtDedAllocateNowSplit") as TextBox;
                                            Label lblDedTaxAmount = grdRow.FindControl("lblDedTaxAmount") as Label;
                                            //Label lblDedReceiptAmount = grdRow.FindControl("lblDedReceiptAmount") as Label;

                                            List<DirectSOAdvDeductionDetails> deductDtlList = new List<DirectSOAdvDeductionDetails>();
                                            if (SOInvoiceHeaderSession != null)
                                            {
                                                deductDtlList.AddRange(SOInvoiceHeaderSession.DeductionDetails.ToList());
                                            }
                                            if (deductionPk > 0)
                                            {
                                                soAdvDeductList = deductDtlList.Where(aa => aa.IAD_PK == deductionPk).ToList();
                                            }
                                            else
                                            {
                                                soAdvDeductList = deductDtlList.Where(aa => aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr).ToList();
                                            }
                                            decimal AdjAdvDeduction = 0;
                                            if (soAdvDeductList.Count > 0)
                                            {
                                                if (Convert.ToDecimal(lblDedTaxAmount.Text.Replace(",", "")) > 0)
                                                    AdjAdvDeduction = ((soAdvDeductList[0].ICM_ADJUST_AMOUNT) / Convert.ToDecimal(lblDedTaxAmount.Text.Replace(",", "")) * (string.IsNullOrEmpty(txtDedAllocateNowSplit.Text.Trim()) ? 0 : Convert.ToDecimal(txtDedAllocateNowSplit.Text)));

                                                amtAdjAdvDedTotal += AdjAdvDeduction;
                                            }
                                        }

                                    }


                                    if ((deduction - (double)amtAdjAdvDedTotal) <= grossAmt)
                                    {
                                        decimal amtAdjAdvDeductionTotal = 0;
                                        foreach (GridViewRow grdRow in grdDeduction.Rows)
                                        {
                                            //DedCnt = DedCnt + 1;
                                            HiddenField hdfCurPaidOtherAmount = grdRow.FindControl("hdfCurPaidOtherAmount") as HiddenField;
                                            HiddenField hdfCurPaidTax = grdRow.FindControl("hdfCurPaidTax") as HiddenField;
                                            HiddenField hdfCurPaidDisc = grdRow.FindControl("hdfCurPaidDisc") as HiddenField;

                                            HiddenField hdfDeductionPK = grdRow.FindControl("hdfDeductionPK") as HiddenField;
                                            HiddenField hdfReceiptPK = grdRow.FindControl("hdfReceiptPK") as HiddenField;
                                            HiddenField hdfDedInvoicePK = grdRow.FindControl("hdfDedInvoicePK") as HiddenField;
                                            HiddenField hdfReceiptDTLPK = grdRow.FindControl("hdfReceiptDTLPK") as HiddenField;
                                            HiddenField hdfDedSoPk = grdRow.FindControl("hdfDedSoPk") as HiddenField;
                                            HiddenField hdfScPk = grdRow.FindControl("hdfScPk") as HiddenField;
                                            HiddenField hdfIcmSoHdr = grdRow.FindControl("hdfIcmSoHdr") as HiddenField;

                                            long deductionPk = hdfDeductionPK != null && !string.IsNullOrEmpty(hdfDeductionPK.Value.Trim())
                                                 ? Convert.ToInt64(hdfDeductionPK.Value) : 0;
                                            long receiptPK = hdfReceiptPK != null && !string.IsNullOrEmpty(hdfReceiptPK.Value.Trim())
                                                ? Convert.ToInt64(hdfReceiptPK.Value) : 0;
                                            long invPK = hdfDedInvoicePK != null && !string.IsNullOrEmpty(hdfDedInvoicePK.Value.Trim())
                                                ? Convert.ToInt64(hdfDedInvoicePK.Value) : 0;
                                            long SoPK = hdfDedSoPk != null && !string.IsNullOrEmpty(hdfDedSoPk.Value.Trim())
                                                ? Convert.ToInt64(hdfDedSoPk.Value) : 0;

                                            int ScPK = hdfScPk != null && !string.IsNullOrEmpty(hdfScPk.Value.Trim())
                                                ? Convert.ToInt32(hdfScPk.Value) : 0;
                                            int IcmSoHdr = hdfIcmSoHdr != null && !string.IsNullOrEmpty(hdfIcmSoHdr.Value.Trim())
                                               ? Convert.ToInt32(hdfIcmSoHdr.Value) : 0;
                                            ReceiptMpgPK = Convert.ToInt32(hdfReceiptDTLPK.Value);
                                            //SOAdvDeductionDetails soAdvDeductionDetailsObj = null;
                                            List<DirectSOAdvDeductionDetails> soAdvDeductionDetailsList = null;

                                            TextBox txtDedAllocateNowSplit = grdRow.FindControl("txtDedAllocateNowSplit") as TextBox;
                                            Label lblDedTaxAmount = grdRow.FindControl("lblDedTaxAmount") as Label;
                                            Label lblDedReceiptAmount = grdRow.FindControl("lblDedReceiptAmount") as Label;

                                            //if (!string.IsNullOrEmpty(txtDedAllocateNowSplit.Text) && Convert.ToDecimal(txtDedAllocateNowSplit.Text.Trim()) != 0)
                                            //{
                                            deductionDtlList = new List<DirectSOAdvDeductionDetails>();
                                            if (SOInvoiceHeaderSession != null)
                                            {
                                                deductionDtlList.AddRange(SOInvoiceHeaderSession.DeductionDetails.ToList());
                                            }

                                            if (deductionPk > 0)
                                            {
                                                soAdvDeductionDetailsList = deductionDtlList.Where(aa => aa.IAD_PK == deductionPk).ToList();
                                            }
                                            else
                                            {
                                                //soAdvDeductionDetailsList = deductionDtlList.Where(aa => aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK).ToList();
                                                soAdvDeductionDetailsList = deductionDtlList.Where(aa => aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr).ToList();
                                            }

                                            //soAdvDeductionDetailsObj = POInvoiceHeaderSession.DeductionDetails.SingleOrDefault(aa => aa.VAD_INVOICE_ADV == advInvPk);
                                            if (soAdvDeductionDetailsList.Count > 0)
                                            {
                                                soAdvDeductionDetailsList[0].IAD_AMOUNT = txtDedAllocateNowSplit != null && !string.IsNullOrEmpty(txtDedAllocateNowSplit.Text.Trim()) ?
                                                    Convert.ToDecimal(txtDedAllocateNowSplit.Text.Trim()) : 0;
                                                soAdvDeductionDetailsList[0].IAD_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                                                soAdvDeductionDetailsList[0].IAD_TAX_AMOUNT = hdfCurPaidTax != null && !string.IsNullOrEmpty(hdfCurPaidTax.Value.Trim()) ?
                                                    (SaleOrderType == 1 ? Convert.ToDecimal(hdfCurPaidTax.Value.Trim()) : 0) : 0;
                                                soAdvDeductionDetailsList[0].IAD_DISC_AMOUNT = hdfCurPaidDisc != null && !string.IsNullOrEmpty(hdfCurPaidDisc.Value.Trim()) ?
                                                    (SaleOrderType == 1 ? Convert.ToDecimal(hdfCurPaidDisc.Value.Trim()) : 0) : 0;
                                                soAdvDeductionDetailsList[0].IAD_OTHER_AMOUNT = hdfCurPaidOtherAmount != null && !string.IsNullOrEmpty(hdfCurPaidOtherAmount.Value.Trim()) ?
                                                    (SaleOrderType == 1 ? Convert.ToDecimal(hdfCurPaidOtherAmount.Value.Trim()) : 0) : 0;
                                                if (Convert.ToDecimal(lblDedTaxAmount.Text.Replace(",", "")) > 0)
                                                    amtAdjAdvDeduction = ((soAdvDeductionDetailsList[0].ICM_ADJUST_AMOUNT) / Convert.ToDecimal(lblDedTaxAmount.Text.Replace(",", "")) * (string.IsNullOrEmpty(txtDedAllocateNowSplit.Text.Trim()) ? 0 : Convert.ToDecimal(txtDedAllocateNowSplit.Text)));
                                                soAdvDeductionDetailsList[0].IAD_ADJUST_AMOUNT = amtAdjAdvDeduction;
                                                amtAdjAdvDeductionTotal += amtAdjAdvDeduction;
                                            }

                                            if (SaleOrderType == 1 && IsAdvInvHasTax)
                                            {
                                                if (soAdvDeductionDetailsList.Count > 0)
                                                {
                                                    //txtPriceAdj.Text =( Convert.ToDecimal(txtPriceAdj.Text) - amtAdjAdvDeduction).ToString(hdfCurrencyFormat.Value);
                                                    totalAllocatedTax += soAdvDeductionDetailsList[0].IAD_TAX_AMOUNT;
                                                    totalAllocatedDiscount += soAdvDeductionDetailsList[0].IAD_DISC_AMOUNT;//soAdvDeductionDetailsList[0].IAD_DISC_AMOUNT == 0 ? soAdvDeductionDetailsList[0].ICM_DISCOUNT_AMOUNT : Convert.ToDecimal(hdfCurPaidDisc.Value) == 0 ? soAdvDeductionDetailsList[0].ICM_DISCOUNT_AMOUNT : Convert.ToDecimal(hdfCurPaidDisc.Value);// soAdvDeductionDetailsList[0].ICM_DISCOUNT_AMOUNT;

                                                    isHaveDiscount = 0;
                                                    SetLineItemTax();
                                                    if (Convert.ToDecimal(lblDedReceiptAmount.Text.Replace(",", "")) > 0)
                                                        TotalHDRDiscount = (TotalHDRDiscount / Convert.ToDecimal(lblDedReceiptAmount.Text.Replace(",", ""))) * (string.IsNullOrEmpty(txtDedAllocateNowSplit.Text.Trim()) ? 0 : Convert.ToDecimal(txtDedAllocateNowSplit.Text));

                                                }
                                            }
                                            TempsoAdvDeductionDetailsList.Add(soAdvDeductionDetailsList[0]);
                                            // }
                                        }

                                        SOInvoiceHeaderSession.DeductionDetails = TempsoAdvDeductionDetailsList;

                                        if (TempSOInvoiceHeaderSessionCustAll != null)
                                            TempSOInvoiceHeaderSessionCustAll.DeductionDetails = TempsoAdvDeductionDetailsList;

                                        if (SaleOrderType == 1 && IsAdvInvHasTax)
                                        {  //txtPriceAdj.Text =( Convert.ToDecimal(txtPriceAdj.Text) - amtAdjAdvDeduction).ToString(hdfCurrencyFormat.Value);
                                            txtDiscDeducted.Text = txtDiscDeducted.ToolTip = totalAllocatedDiscount.ToString(hdfCurrencyFormat.Value);

                                            //txtHdrDeduction.Text = txtHdrDeduction.ToolTip = (deduction - (double)amtAdjAdvDeduction).ToString(hdfCurrencyFormat.Value);
                                            txtHdrDeduction.Text = txtHdrDeduction.ToolTip = (deduction - (double)amtAdjAdvDeductionTotal).ToString(hdfCurrencyFormat.Value);

                                            txtAdvAdjustDeductAmount.Text = (amtAdjAdvDeductionTotal).ToString(hdfCurrencyFormat.Value);
                                            if (amtAdjAdvDeductionTotal == 0)
                                                trAdvAdjDed.Visible = false;
                                            else
                                                trAdvAdjDed.Visible = true;


                                            txtdor.Text = otherCharges.ToString(hdfCurrencyFormat.Value);
                                            //txtHdrDiscount.Text = (Convert.ToDecimal(txtHdrDiscount.Text) - TotalHDRDiscount).ToString(hdfCurrencyFormat.Value);
                                            isHaveDiscount = 1;
                                            SetHdrTax();


                                            //Settings of TaxPayableDiv
                                            if (hdfIsTaxPayable.Value.ToString() == "1")
                                            {
                                                SetTaxPayableDiv();
                                            }
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "TaxPayable", "$(document).ready(function () { ShowHideTaxPayableOuter();});", true);
                                            //end
                                        }
                                        else
                                        {

                                            txtTotalDeductionExp.Text = txtTotalDeductionExp.ToolTip = (deductionExp - (double)amtAdjAdvDeduction).ToString(hdfCurrencyFormat.Value);
                                            txtHdrNetTotal.Text = txtHdrNetTotal.ToolTip = (Convert.ToDecimal(txtTotalExp.Text) - Convert.ToDecimal(txtTotalDeductionExp.Text)).ToString();

                                            txtAdvAdjustDeductAmount.Text = (amtAdjAdvDeduction).ToString(hdfCurrencyFormat.Value);
                                            if (amtAdjAdvDeduction == 0)
                                                trAdvAdjDed.Visible = false;
                                            else
                                                trAdvAdjDed.Visible = true;
                                        }



                                    }
                                    else
                                    {
                                        close = false;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2", "ShowContainerDiv('[id$=divDeduction]','" + GetLocalResourceObject("DeductionDetails").ToString() + "','900','400');", true);
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateTotalSplit", "CalculateTotalSplit();", true);
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowCurrentAdvInvoice", "$(document).ready(function(){SetCurrentAdvInvoiceRowColor();});", true);

                                        litErrorMsg.Text = GetLocalResourceObject("Err_MsgExceeds_Allocation").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);

                                    }
                                }
                            }
                            //if domestic show nettotal in Invoice Total  (as per manoj sir :while testing 'LOCAL SALE +ADVANCE RECEIVE 100%')
                            if (ddlInvoiceType.SelectedValue == "1" && IsAdvInvHasTax)
                            {
                                txtHdrInvoiceTotal.Text = txtHdrNetTotal.Text;
                            }
                        }
                        if (close)
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                        break;
                    #endregion
                    #region PRINT DOCS
                    case ActionsEnum.PRINTINVOICE:
                        //if (CurrPK == 0)
                        //{
                        int DelStatus = 0;
                        foreach (GridViewRow grdrow in grdInvoiceList.Rows)
                        {
                            CheckBox chkInvselect;
                            chkInvselect = (CheckBox)grdrow.FindControl("chkInvselect");
                            if (chkInvselect.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfInvoiceID")).Value);
                                DelStatus = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfDelStatus")).Value);
                                break;
                            }
                        }
                        //}
                        if (CurrPK == 0)
                        {

                            litErrorMsg.Text = GetLocalResourceObject("Msg_Invoice_notcreated").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        else
                        {
                            PrinterControl1.InvoicePK = CurrPK;
                            PrinterControl1.DelStatus = DelStatus;
                            //Session[ERP.Utilities.SessionStrings.SHIPPINGPLANPK] = ShippingPlanID.ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPrint]','" + Resources.PageNameRes.SalesInvoicePrint + "','360','200');", true);
                        }
                        break;
                    #endregion
                    #region EDIT FOR CANCEL
                    case ActionsEnum.EDITFORCANCEL:
                        IsDoModified = false;
                        btnRefreshInv.Visible = false;
                        ReloadInvoice = false;
                        foreach (GridViewRow grdrow in grdInvoiceList.Rows)
                        {
                            CheckBox chkInvselect;
                            HiddenField hdfDept;
                            int dept;
                            chkInvselect = (CheckBox)grdrow.FindControl("chkInvselect");
                            if (chkInvselect.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfInvoiceID")).Value);
                                hdfCurrentPk.Value = CurrPK.ToString();

                                Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = ((HiddenField)grdrow.FindControl("hdfCustomerPK")).Value;
                                Session[ERP.Utilities.SessionStrings.CUSTOMERNAME] = ((Label)grdrow.FindControl("lblCustomer")).Text;
                                ////start
                                Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                                Posted = Convert.ToBoolean(((HiddenField)grdrow.FindControl("hdfPosted")).Value);
                                ////
                                invType = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfSalesContractType")).Value);
                                SaleOrderType = invType;
                                //if (invType == Convert.ToInt32(SalesInvoiceType.Domestic))
                                //    FillProcessID(3);
                                //else
                                //    FillProcessID(1);
                                FillProcessID(1);
                                hdfDept = grdrow.FindControl("hdfDept") as HiddenField;
                                HiddenField hdfInvStatus = grdrow.FindControl("hdfInvStatus") as HiddenField;
                                if (!string.IsNullOrEmpty(hdfInvStatus.Value) && Convert.ToInt32(hdfInvStatus.Value) == 0)
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_DraftedInv").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                    return;
                                }

                                HiddenField hdfInvDtlCount = grdrow.FindControl("hdfInvDtlCount") as HiddenField;
                                if (!string.IsNullOrEmpty(hdfInvDtlCount.Value) && Convert.ToInt32(hdfInvDtlCount.Value) == 0)
                                {
                                    IsDoModified = true;
                                    btnRefreshInv.Visible = true;
                                    litErrorMsg.Text = GetLocalResourceObject("Msg_DO_Modified").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    return;

                                }
                                if (Convert.ToInt16(((HiddenField)grdrow.FindControl("hdfDelStatus")).Value) == 1)
                                {
                                    btnSave.Visible = false;
                                    IsDeleted = true;
                                    //btnEditforCancel.Visible = false;
                                    //btnEdit.Visible = false;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_alreadycancelled").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                    return;

                                }
                                else
                                {
                                    btnSave.Visible = true;
                                    btnEdit.Visible = true;
                                    IsDeleted = false;
                                }
                                if (hdfDept != null && int.TryParse(hdfDept.Value, out dept))
                                {
                                    Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                                    base.SetUserDept();
                                }
                                hdfCurrentPk.Value = CurrPK.ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SetPrintDocsVisibility", "$(document).ready(function(){SetPrintDocsVisibility();});", true);

                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            //if (invType == Convert.ToInt32(SalesInvoiceType.Domestic))
                            //    FillProcessID(13);
                            //else
                            //    FillProcessID(11);
                            FillProcessID(11);
                            SetUIEditView(commonActions);
                            WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                            base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                            SetCancelRef(CurrPK);
                            ucrWrkf.FillWorkFlowDetails();
                            if (!ucrWrkf.HasPageTaskPermission)
                            {
                                //LinkButton lnkList = new LinkButton();
                                //lnkList.CommandName = ControlsEnum.INVOICELIST.ToString();
                                //ActionHandler(lnkList, e);
                                FillProcessID(1);
                                CurrPK = 0;
                                hdfCurrentPk.Value = CurrPK.ToString();
                                hdfIVHPK.Value = "";
                                ResetForm(ControlsEnum.INVOICELIST);
                                EntryStatus = EntryStatus.LISTMODE;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_nopermission").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                return;
                            }
                            ucrWrkf.ViewAction();
                            ModifiedDatePnl.Visible = true;
                            GetFieldValues(ControlsEnum.INVOICETYPE);
                            SetFieldValues(ControlsEnum.INVOICETYPE);
                            GetFieldValues(ControlsEnum.SOINVHEADER);
                            if (SOInvoiceHeaderSession != null)
                            {
                                DespatchID = Convert.ToInt16(SOInvoiceHeaderSession.ICH_DESPATCH_HDR);
                            }
                            GetFieldValues(ControlsEnum.SOINVHEADER);
                            SetFieldValues(ControlsEnum.SOINVHEADER);
                            GetFieldValues(ControlsEnum.TAXSETTINGS);
                            SetFieldValues(ControlsEnum.SOINVDETAIL);
                            SetFieldValues(ControlsEnum.UPLOADEDFILES);
                            TempSOInvoiceHeaderSession = SOInvoiceHeaderSession;
                            SetDetailTax(null);
                            SetHdrTax();
                            if (grdInvoice.Rows.Count > 0)
                            {
                                SetSubTotal();
                            }

                            //Settings of TaxPayableDiv
                            if (hdfIsTaxPayable.Value.ToString() == "1")
                            {
                                SetTaxPayableDiv();
                            }
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "TaxPayable", "$(document).ready(function () { ShowHideTaxPayableOuter();});", true);
                            //end

                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Invoice").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region DELETE SUBMIT popup
                    case ActionsEnum.DELETESUBMIT:
                        //Show WorkFlow Popup
                        hdfIsSaveSubmit.Value = CommonConstants.SELECTVAL;
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    #endregion
                    #region VALIDATE INVOICE DATE:
                    case ActionsEnum.VALIDATEINVOICEDATE:
                        if (!IsValidInvoiceDate())
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + Resources.Messages.TaxDateChanged + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region GET DUE DATE
                    case ActionsEnum.GETDUEDATE:
                        /*if (Convert.ToDateTime(txtInvoiceDate.Text) < Convert.ToDateTime(lblSODateTxt.Text))
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Invdate").ToString()) + "');", true);
                            txtInvoiceDate.Text = lblSODateTxt.Text;
                        }
                        else
                        {
                            GetFieldValues(ControlsEnum.GETDUEDATE);
                            SetFieldValues(ControlsEnum.GETDUEDATE);
                        }*/
                        GetFieldValues(ControlsEnum.GETDUEDATE);
                        SetFieldValues(ControlsEnum.GETDUEDATE);
                        break;
                    #endregion
                    #region CUSTOMER TYPE CHANGING
                    case ActionsEnum.CUSTOMERTYPECHANGING:
                        GetFieldValues(ControlsEnum.GETCUSTOMERDETAILSBYTYPE);
                        SetFieldValues(ControlsEnum.GETCUSTOMERDETAILSBYTYPE);
                        SetBranchIDEnableDisable();
                        break;
                    #endregion
                    #region CHANGE EXCHANGE RATE
                    case ActionsEnum.CHANGEEXRATE:
                        hdfExchangeRate.Value = string.IsNullOrEmpty(txtExchangeRate.Text) ? "1" : txtExchangeRate.Text;
                        //Settings of TaxPayableDiv
                        if (hdfIsTaxPayable.Value.ToString() == "1")
                        {
                            SetTaxPayableDiv();
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "TaxPayable", "$(document).ready(function () { ShowHideTaxPayableOuter();});", true);
                        //end
                        break;
                    #endregion
                    #region Show Popup
                    case ActionsEnum.SHOWPOPUP:
                        //if (hdfDoPk.Value != null || !string.IsNullOrEmpty(hdfDoPk.Value))
                        //{
                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + DespatchID.ToString() + "&APPTYPE=" + ApplicationType.DO + "&APPSUBTYPE=6") + "');", true);

                        //}                       
                        break;
                    #endregion
                    #region Show SO Popup
                    case ActionsEnum.SHOW:
                        scPK = ((LinkButton)sender).CommandArgument;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + scPK + "&APPTYPE=" + ApplicationType.SOD + "&APPSUBTYPE=") + "');", true);
                        break;
                    #endregion
                    #region AMOUNT DETAILS
                    case ActionsEnum.AMOUNTDETAILS:
                        HiddenField hdfInvoiceID = (HiddenField)((GridViewRow)((LinkButton)(sender)).Parent.Parent).FindControl("hdfInvoiceID");
                        long.TryParse(hdfInvoiceID.Value, out InvoicePk);
                        GetFieldValues(ControlsEnum.AMOUNTDETAILS);
                        SetFieldValues(ControlsEnum.AMOUNTDETAILS);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateTotalAmountSplit", "$(document).ready(function(){CalculateTotalAmountSplit();});", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPaidAmntSplitup]','" + GetLocalResourceObject("TrxDetails").ToString() + "','600','200');", true);

                        break;
                    #endregion
                    #region chkDedAll CHECKEDCHANGED
                    case ActionsEnum.CHECKEDCHANGED:
                        if (chkDedAll.Checked)
                        {
                            hdfIsCusAllAdv.Value = CommonConstants.SELECT_VALUE_ONE;
                        }
                        else
                        {
                            hdfIsCusAllAdv.Value = CommonConstants.SELECT_VALUE_ZERO;
                        }
                        GetFieldValues(ControlsEnum.SOINVHEADER);
                        if (SOInvoiceHeaderSession != null)
                        {
                            DespatchID = Convert.ToInt16(SOInvoiceHeaderSession.ICH_DESPATCH_HDR);
                        }
                        GetFieldValues(ControlsEnum.SOINVHEADER);
                        //SetFieldValues(ControlsEnum.SOINVHEADER);
                        //TempSOInvoiceHeaderSession = SOInvoiceHeaderSession;
                        //SetDetailTax(null);
                        //SetHdrTax();
                        //if (grdInvoice.Rows.Count > 0)
                        //{
                        //    SetSubTotal();
                        //}
                        //if (hdfIsTaxPayable.Value.ToString() == "1")
                        //{
                        //    SetTaxPayableDiv();
                        //}
                        //deductionDtlListCustAll = new List<DirectSOAdvDeductionDetails>();
                        //if (TempSOInvoiceHeaderSessionCustAll != null)
                        //{
                        //    if (TempSOInvoiceHeaderSessionCustAll.DeductionDetails != null && TempSOInvoiceHeaderSessionCustAll.DeductionDetails.Count > 0)
                        //    {
                        //        deductionDtlListCustAll = TempSOInvoiceHeaderSessionCustAll.DeductionDetails;
                        //    }
                        //}
                        SetFieldValues(ControlsEnum.DEDUCTIONPOPUPGRID);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2", "ShowContainerDiv('[id$=divDeduction]','" + GetLocalResourceObject("DeductionDetails").ToString() + "','900','400');", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "EditTaxOtherCharge", "$(document).ready(function () { EnableDisableTaxOtherCharge();});", true);  //EditTaxOtherCharge Setting
                        break;
                    #endregion
                    #region SHOW DUEDETAILS
                    case ActionsEnum.SHOWDUEDETAILS:
                        GetFieldValues(ControlsEnum.GETDUEDATE);
                        SetFieldValues(ControlsEnum.DUEDATEPOPUPGRID);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divDueDatePopup]','" + GetLocalResourceObject("ShowInvDueDetails").ToString() + "','400','200');", true);
                        break;
                    #endregion
                    #region RELOAD INV DETAILS
                    case ActionsEnum.RELOADINVDETAILS:
                        IsDoModified = false;
                        ReloadInvoice = true;
                        hdfIsDedApplyClick.Value = "0";
                        GetFieldValues(ControlsEnum.INVOICETYPE);
                        SetFieldValues(ControlsEnum.INVOICETYPE);
                        GetFieldValues(ControlsEnum.SOINVHEADER);
                        if (SOInvoiceHeaderSession != null)
                        {
                            DespatchID = Convert.ToInt16(SOInvoiceHeaderSession.ICH_DESPATCH_HDR);
                        }
                        GetFieldValues(ControlsEnum.SOINVHEADER);
                        SetFieldValues(ControlsEnum.SOINVHEADER);
                        GetFieldValues(ControlsEnum.TAXSETTINGS);
                        SetFieldValues(ControlsEnum.SOINVDETAIL);
                        SetFieldValues(ControlsEnum.UPLOADEDFILES);
                        //GetFieldValues(ControlsEnum.PAYMENTTERMS);
                        //SetFieldValues(ControlsEnum.PAYMENTTERMS);
                        TempSOInvoiceHeaderSession = SOInvoiceHeaderSession;
                        SetDetailTax(null);
                        SetHdrTax();
                        if (grdInvoice.Rows.Count > 0)
                        {
                            SetSubTotal();
                        }
                        //Settings of TaxPayableDiv
                        if (hdfIsTaxPayable.Value.ToString() == "1")
                        {
                            SetTaxPayableDiv();
                        }
                        if (grdInvoice.Rows.Count <= 0)
                        {
                            ResetForm(ControlsEnum.RESETFORMODIFIEDDO);
                        }

                        //ReloadInvoice = false;
                        break;
                    #endregion
                    #region Other Charge Popup
                    case ActionsEnum.OTHERCHARGEHEADER:
                        // SetFieldValues(ControlsEnum.OTHERCHARGELIST);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowotherchargePop", "ShowContainerDiv('[id$=divOtherchargeSplitUp]','" + GetLocalResourceObject("OtherchargeDetails").ToString() + "','800','300');", true);

                        break;
                    #endregion
                    #region OTHER CHARGE APPLY
                    case ActionsEnum.OTHERCHARGEAPPLY:
                        if (SOInvoiceHeaderSession != null)
                        {
                            if (SOInvoiceHeaderSession.TaxHdr != null && SOInvoiceHeaderSession.TaxHdr.Where(r => r.CIT_TAX_CATEGORY == (int)TaxType.Shipping).Count() > 0)
                            {
                                // To set total other charge details                                
                                decimal totalOtherCharge = 0;
                                //List<POOtherChargeDetails> PoOthrChrgLst = new List<POOtherChargeDetails>();
                                hdfOtherCharge.Value = "1";
                                foreach (GridViewRow gvr in grdOtherchargeSplit.Rows)
                                {
                                    if (gvr.RowType == DataControlRowType.DataRow)
                                    {
                                        TextBox txtAdjustNowAmount = gvr.FindControl("txtAdjustNowAmount") as TextBox;
                                        HiddenField hdfSCOtherchargePK = gvr.FindControl("hdfSCOtherchargePK") as HiddenField;
                                        HiddenField hdfSCPK = gvr.FindControl("hdfSCPK") as HiddenField;
                                        HiddenField hdfTaxPK = gvr.FindControl("hdfTaxPK") as HiddenField;
                                        CheckBox chkFob = gvr.FindControl("chkFob") as CheckBox;
                                        if (txtAdjustNowAmount.Text != string.Empty)
                                        {
                                            totalOtherCharge += Convert.ToDecimal(txtAdjustNowAmount.Text);
                                        }
                                        SOInvoiceHeaderSession.TaxHdr.Where(tax => tax.CIT_TAX_CATEGORY == (int)TaxType.Shipping && tax.CIT_PK == Convert.ToInt32(hdfSCOtherchargePK.Value) && tax.CIT_SO == Convert.ToInt32(hdfSCPK.Value) && tax.CIT_TAX == Convert.ToInt32(hdfTaxPK.Value)).ToList().ForEach(dtl =>
                                        {
                                            dtl.CIT_TAX_AMT = Convert.ToDouble(txtAdjustNowAmount.Text);
                                            dtl.CIT_IS_FOB = chkFob.Checked ? 1 : 0;
                                        });
                                    }
                                }
                                txtShipping.Text = totalOtherCharge.ToString(hdfCurrencyFormat.Value);
                                SetHdrTax();
                            }
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                        break;
                    #endregion
                    #region ADD ITEM UPLOAD
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
                                            SOInvoiceUploadObj = SOInvoiceUploadList.SingleOrDefault(itm => itm.DOC_SEQ_NO == CurrSlNo);
                                            if (SOInvoiceUploadObj != null)
                                            {
                                                if (FileDetailsList == null)
                                                {
                                                    FileDetailsList = new List<DirectFileDetailsSI>();
                                                }
                                                if (fupUpload.HasFile)
                                                {

                                                    tempFileInfoObj = new FileInfo(fupUpload.PostedFile.FileName);
                                                    string attachmentFileFormat = tempFileInfoObj.Extension;
                                                    string attachmentFileName = Guid.NewGuid().ToString() + attachmentFileFormat;
                                                    SOInvoiceUploadObj.AttachmentFileName = attachmentFileName;
                                                    SOInvoiceUploadObj.FileExtension = tempFileInfoObj.Extension;
                                                    SOInvoiceUploadObj.DOC_NAME = fupUpload.FileName;
                                                    SOInvoiceUploadObj.DOC_TYPE = tempFileInfoObj.Extension;
                                                    if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower()))
                                                    {
                                                        SOInvoiceUploadObj.DOC_PATH = "~/Upload/" + attachmentFileName;
                                                    }
                                                    else
                                                    {
                                                        SOInvoiceUploadObj.DOC_PATH = System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower() + attachmentFileName;
                                                    }
                                                    DirectFileDetailsSI fileDetailsObj = FileDetailsList.SingleOrDefault(aa => aa.SlNo == CurrSlNo);
                                                    if (fileDetailsObj == null)
                                                    {
                                                        FileDetailsList.Add(new DirectFileDetailsSI() { SlNo = CurrSlNo, PoFile = HttpContext.Current.Request.Files[0] });
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
                                            if (SOInvoiceUploadList == null || SOInvoiceUploadList.Count == 0)
                                            {
                                                SOInvoiceUploadList = new List<BusinessObject.SaleOrder.DirectSOInvoiceUploads>();
                                                slno = 1;
                                            }
                                            else
                                            {
                                                slno = SOInvoiceUploadList.Max(itm => itm.DOC_SEQ_NO);
                                                slno++;
                                            }
                                            if (FileDetailsList == null)
                                            {
                                                FileDetailsList = new List<DirectFileDetailsSI>();
                                            }

                                            SOInvoiceUploadObj = new DirectSOInvoiceUploads();
                                            SOInvoiceUploadObj.DOC_PK = 0;
                                            SOInvoiceUploadObj.DOC_SEQ_NO = slno;
                                            tempFileInfoObj = new FileInfo(fupUpload.PostedFile.FileName);
                                            string attachmentFileFormat = tempFileInfoObj.Extension;
                                            string attachmentFileName = Guid.NewGuid().ToString() + attachmentFileFormat;

                                            SOInvoiceUploadObj.AttachmentFileName = attachmentFileName;
                                            SOInvoiceUploadObj.FileExtension = tempFileInfoObj.Extension;
                                            SOInvoiceUploadObj.DOC_NAME = fupUpload.FileName;
                                            SOInvoiceUploadObj.DOC_TYPE = tempFileInfoObj.Extension;
                                            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower()))
                                            {
                                                SOInvoiceUploadObj.DOC_PATH = "~/Upload/" + attachmentFileName;
                                            }
                                            else
                                            {
                                                SOInvoiceUploadObj.DOC_PATH = System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower() + attachmentFileName;
                                            }
                                            SOInvoiceUploadObj.DOC_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                                            FileDetailsList.Add(new DirectFileDetailsSI() { SlNo = slno, PoFile = HttpContext.Current.Request.Files[0] });
                                            SOInvoiceUploadList.Add(SOInvoiceUploadObj);

                                        }
                                    }
                                }
                            }
                            BindGrid(ControlsEnum.UPLOADEDFILES); // SetFieldValues(ControlsEnum.UPLOADEDFILES);
                            ResetForm(ControlsEnum.ADDITEM);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowUploadDocDetails", "$(document).ready(function(){ShowHideUploadDocDetails(1);});", true);
                            grdUploads.Focus();
                        }
                        break;
                    #endregion
                    #region REMOVE ITEM UPLOAD
                    case ActionsEnum.REMOVEITEMUPLOAD:
                        if (SOInvoiceUploadList != null && SOInvoiceUploadList.Count > 0)
                        {
                            selectedItemPK = Convert.ToInt32(grdUploads.DataKeys[(((Button)sender).Parent.Parent as GridViewRow).RowIndex][0]);
                            if (selectedItemPK > 0)
                            {
                                SOInvoiceUploadList = SOInvoiceUploadList.Where(row => selectedItemPK != row.DOC_SEQ_NO).ToList();
                                //SetFieldValues(ControlsEnum.UPLOADEDFILES);
                                BindGrid(ControlsEnum.UPLOADEDFILES);
                                ResetForm(ControlsEnum.ADDITEM);
                            }
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowUploadDocDetails", "$(document).ready(function(){ShowHideUploadDocDetails(1);});", true);

                        break;
                    #endregion
                    #region EDIT ITEM UPLOAD
                    case ActionsEnum.EDITITEMUPLOAD:
                        if (SOInvoiceUploadList != null && SOInvoiceUploadList.Count > 0)
                        {
                            selectedItemPK = Convert.ToInt32(grdUploads.DataKeys[(((Button)sender).Parent.Parent as GridViewRow).RowIndex][0]);
                            if (selectedItemPK > 0)
                            {
                                SOInvoiceUploadObj = SOInvoiceUploadList.SingleOrDefault(row => selectedItemPK == row.DOC_SEQ_NO);
                                GetUIValuesFromObject(ControlsEnum.SELECTEDDOC);
                            }
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowUploadDocDetails", "$(document).ready(function(){ShowHideUploadDocDetails(1);});", true);

                        break;
                    #endregion
                    #region PRINT DELIVERY ORDER
                    case ActionsEnum.PRINTDELIVERYORDER:
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + ((LinkButton)sender).CommandArgument + "&APPTYPE=" + ApplicationType.DOD + "&APPSUBTYPE=6") + "');", true);
                        break;
                    #endregion
                    #region SETTAXAPPLICABLEAMOUNT
                    case ActionsEnum.SETTAXAPPLICABLEAMOUNT:
                        double applicableamount = 0;
                        applicableamount = SetTaxApplicableAmount();
                        txtPopupItemAmount.Text = (applicableamount).ToString(hdfCurrencyFormat.Value);
                        // GetFormula(parseInt($("[id$=ChooseTax]").val()));
                        ActionHandler(ddlPopupTaxType, EventArgs.Empty);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("TaxDetails").ToString() + "','600','300');", true);
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
                CommonServiceClient = null;
                salesInvoiceServiceClient = null;
            }
        }

        /// <summary>
        /// Method to save invoice and its workflow
        /// </summary>
        /// <param name="objInvoice"></param>
        /// <param name="workflowFlag"></param>
        /// <param name="sender"></param>
        private void SaveTransaction(DirectSOInvoiceHeader objInvoice, int workflowFlag, object sender)
        {
            int? result = 0;
            string savePath = string.Empty;
            WorkflowDetails wkfDetails = null;
            string TrxNo = string.Empty;
            string action = string.Empty;
            if (objInvoice == null)
                objInvoice = new DirectSOInvoiceHeader();

            #region New workflow Submition
            wkfDetails = ucrWrkf.GetWorkflowDetails();
            objInvoice.USER_PK = Convert.ToInt16(wkfDetails.UserPK);
            objInvoice.WKF_APPLICATION = CurrPK;
            objInvoice.WKF_COMMENTS = wkfDetails.Comments;
            objInvoice.WKF_TRX_FLAG = workflowFlag;
            objInvoice.WKF_PROCESS = wkfDetails.ProcessID;
            objInvoice.WKF_REFERENCE = wkfDetails.ReferenceID;
            objInvoice.WKF_TASK = wkfDetails.TaskID;
            objInvoice.WKF_TASK_ACTION = wkfDetails.ActionID;
            action = wkfDetails.ActionText;
            if (isCancelled)
                objInvoice.ATL_ACTION = (byte)LogAction.CANCEL;
            else
                objInvoice.ATL_ACTION = (byte)LogAction.SUBMIT;
            #endregion
            string xmlDoc = CommonFunctions.XmlSerialize<DirectSOInvoiceHeader>(objInvoice);
            result = BusinessLogic.Sales.SalesInvoiceBL.SaveDirectSalesInvoiceHeader(xmlDoc, out TrxNo);
            if (result.HasValue && result.Value > 0) // Success !  redirect to listing page
            {
                if (!string.IsNullOrEmpty(TrxNo))
                    lblInvoiceNo.Text = TrxNo;
                #region ATTACHMENT SAVE
                if (workflowFlag == (int)(WorkflowTransactionFlag.SAVEANDSUBMIT))
                {
                    if (SOInvoiceUploadList != null && SOInvoiceUploadList.Count > 0)
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

                        foreach (DirectSOInvoiceUploads obj in SOInvoiceUploadList)
                        {
                            string filePath = savePath + obj.AttachmentFileName;
                            FileInfo attachedFileInfo = new FileInfo(filePath);
                            if (FileDetailsList != null)
                            {
                                DirectFileDetailsSI fileDetailsObj = FileDetailsList.SingleOrDefault(aa => aa.SlNo == obj.DOC_SEQ_NO);
                                if (fileDetailsObj != null)
                                {
                                    fileDetailsObj.PoFile.SaveAs(attachedFileInfo.FullName);

                                }
                            }
                        }
                    }
                }
                #endregion

                #region ALERTSAVE
                if (workflowFlag == (int)(WorkflowTransactionFlag.SAVEANDSUBMIT))
                {
                    ucrAlert.TypeRef = lblInvoiceNo.Text;
                    GetFieldValues(ControlsEnum.ALERTCONFIG);
                    int isAlert = 0;
                    if (admAppConstMstList != null && admAppConstMstList.Count > 0)
                    {
                        isAlert = admAppConstMstList[0].ACF_VALUE;
                    }
                    if (isAlert == 1)
                    {
                        invPK = (int)result;
                        AlertBO alertBoObj = new AlertBO();
                        alertBoObj = (AlertBO)SetUIValuesToObject(ControlsEnum.ALERTSAVE);
                        if (alertBoObj != null)
                        {
                            int alertresult = BusinessLogic.AlertManagement.Alerts.SaveAlertDetails(alertBoObj);
                        }
                    }
                }
                #endregion

                ADM_APP_TRX_LOG AdmTrxLogDet = new ADM_APP_TRX_LOG();
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
                TextBox WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");

                object[] args = new object[2];
                args[0] = Resources.PageNameRes.SalesInvoice;
                args[1] = lblInvoiceNo.Text;
                litErrorMsg.Text = string.Format(litErrorMsg.Text, args);

                #region Inbox or Listing Page Redirection
                if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && base.WkfRefID > 0)
                {

                    SOInvoiceHeaderSession = null;
                    ResetForm(ControlsEnum.INVOICELIST);

                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                            + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.InboxURL) + "');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                    EntryStatus = EntryStatus.LISTMODE;
                    ResetForm(ControlsEnum.INVOICELIST);
                    GetFieldValues(ControlsEnum.INVOICELIST);
                    SetFieldValues(ControlsEnum.INVOICELIST);
                    SOInvoiceHeaderSession = null;
                }
                #endregion

                ucrWrkf.ApplicationID = result.Value;

            }
            else
            {
                if (result == -51)//invoice amount greater than SC amount
                {
                    if (ContineInvoiceAmtGreaterThanSCAmt == 0)//Should not allow to Save invoice amount greater than SC Amount
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_InvAmtGreaterSCAmt").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                    else
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowInvoiceAmtGreaterSCAmtConfirm('" + (sender as Button).ID + "');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                }
                return;
            }
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


        private bool CheckAddedType(List<int> lst)
        {
            bool result = true;
            if (lst.Distinct().Count() == 1)
            {
                foreach (GridViewRow grdRow in grdInvoice.Rows)
                {
                    HiddenField hdfProductType = grdRow.FindControl("hdfProductType") as HiddenField;
                    for (int i = 0; i < lst.Count; i++)
                    {
                        if (lst[i] != Convert.ToInt32(hdfProductType.Value))
                            result = false;
                    }
                }
            }
            else
                result = false;
            return result;
        }

        #region --- For Grid Actions----
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            try
            {
                #region Data Row/Data Item
                if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                {
                    #region grdDeduction
                    if (((GridView)sender).ID == "grdDeduction")
                    {
                        TextBox txtDedAllocateNowSplit = e.Row.FindControl("txtDedAllocateNowSplit") as TextBox;
                        TextBox txtOtherAmountSplit = e.Row.FindControl("txtOtherAmountSplit") as TextBox;
                        TextBox txtTaxSplit = e.Row.FindControl("txtTaxSplit") as TextBox;

                        Label lblDedInvoiceBal = e.Row.FindControl("lblDedInvoiceBal") as Label;

                        HiddenField hdfDedAllocateNowSplit = e.Row.FindControl("hdfDedAllocateNowSplit") as HiddenField;
                        HiddenField hdfCurPaidOtherAmount = e.Row.FindControl("hdfCurPaidOtherAmount") as HiddenField;
                        HiddenField hdfDedOtherChargeSplitBalance = e.Row.FindControl("hdfDedOtherChargeSplitBalance") as HiddenField;

                        HiddenField hdfCurPaidTax = e.Row.FindControl("hdfCurPaidTax") as HiddenField;
                        HiddenField hdfDedTaxSplitBalance = e.Row.FindControl("hdfDedTaxSplitBalance") as HiddenField;

                        HiddenField hdfDeductionPK = e.Row.FindControl("hdfDeductionPK") as HiddenField;
                        HiddenField hdfReceiptPK = e.Row.FindControl("hdfReceiptPK") as HiddenField;
                        HiddenField hdfDedInvoicePK = e.Row.FindControl("hdfDedInvoicePK") as HiddenField;
                        HiddenField hdfCusAdvFlag = e.Row.FindControl("hdfCusAdvFlag") as HiddenField;
                        HiddenField hdfScPk = e.Row.FindControl("hdfScPk") as HiddenField;
                        HiddenField hdfIcmSoHdr = e.Row.FindControl("hdfIcmSoHdr") as HiddenField;


                        List<DirectSOAdvDeductionDetails> advDeductionDtlList = new List<DirectSOAdvDeductionDetails>();
                        // advDeductionDtlList = (TempSOInvoiceHeaderSessionCustAll == null) ? TempSOInvoiceHeaderSession.DeductionDetails.ToList() : TempSOInvoiceHeaderSessionCustAll.DeductionDetails.ToList();
                        advDeductionDtlList = (TempSOInvoiceHeaderSessionCustAll == null) ? TempSOInvoiceHeaderSession.DeductionDetails.ToList() : TempSOInvoiceHeaderSessionCustAll.DeductionDetails.ToList();

                        long deductionPk = hdfDeductionPK != null && !string.IsNullOrEmpty(hdfDeductionPK.Value.Trim())
                                                 ? Convert.ToInt64(hdfDeductionPK.Value) : 0;
                        long receiptPK = hdfReceiptPK != null && !string.IsNullOrEmpty(hdfReceiptPK.Value.Trim())
                            ? Convert.ToInt64(hdfReceiptPK.Value) : 0;
                        long invPK = hdfDedInvoicePK != null && !string.IsNullOrEmpty(hdfDedInvoicePK.Value.Trim())
                            ? Convert.ToInt64(hdfDedInvoicePK.Value) : 0;

                        int ScPK = hdfScPk != null && !string.IsNullOrEmpty(hdfScPk.Value.Trim())
                           ? Convert.ToInt32(hdfScPk.Value) : 0;

                        int IcmSoHdr = hdfIcmSoHdr != null && !string.IsNullOrEmpty(hdfIcmSoHdr.Value.Trim())
                          ? Convert.ToInt32(hdfIcmSoHdr.Value) : 0;



                        decimal defaultAllocation = 0;
                        int CusAdvFlag = 0;
                        int.TryParse(hdfCusAdvFlag.Value, out CusAdvFlag);

                        if (advDeductionDtlList != null && advDeductionDtlList.Count > 0)
                        {
                            if (string.IsNullOrEmpty(hdfCurrentPk.Value.ToString()) || Convert.ToInt32(hdfCurrentPk.Value) == 0)
                            {

                                txtDedAllocateNowSplit.Text = advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).Count() <= 0 ? "0.00" : (GetFormattedCurrency(Convert.ToDecimal(advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).FirstOrDefault().IAD_AMOUNT) > 0 ? advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).FirstOrDefault().IAD_AMOUNT : hdfIsDedApplyClick.Value == "1" ? 0 : (Convert.ToDecimal(advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).FirstOrDefault().RCH_SO_RCVD_AMT) - Convert.ToDecimal(advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).FirstOrDefault().ICH_AMOUNT_ALLOCATED))));
                                decimal DedInvoiceBal = Convert.ToDecimal(lblDedInvoiceBal.Text.Replace(",", ""));
                                decimal DedInvoiceNow = Convert.ToDecimal(txtDedAllocateNowSplit.Text);
                                txtDedAllocateNowSplit.Text = DedInvoiceNow > DedInvoiceBal ? Convert.ToString(DedInvoiceBal) : txtDedAllocateNowSplit.Text;
                                hdfDedAllocateNowSplit.Value = advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).Count() <= 0 ? "0.00" : GetFormattedCurrency(Convert.ToDecimal(advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).FirstOrDefault().IAD_AMOUNT) > 0 ? advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).FirstOrDefault().IAD_AMOUNT : hdfIsDedApplyClick.Value == "1" ? 0 : (Convert.ToDecimal(advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).FirstOrDefault().RCH_SO_RCVD_AMT) - Convert.ToDecimal(advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).FirstOrDefault().ICH_AMOUNT_ALLOCATED)));
                                hdfAllocNowAmount.Value = advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).Count() <= 0 ? "0.00" : GetFormattedCurrency(Convert.ToDecimal(advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).FirstOrDefault().RCH_SO_RCVD_AMT) - Convert.ToDecimal(advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).FirstOrDefault().ICH_AMOUNT_ALLOCATED));
                                txtOtherAmountSplit.Text = advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).Count() <= 0 ? "0.00" : GetFormattedCurrency(Convert.ToDecimal(advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).FirstOrDefault().IAD_OTHER_AMOUNT) > 0 ? advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).FirstOrDefault().IAD_OTHER_AMOUNT : hdfIsDedApplyClick.Value == "1" ? 0 : (Convert.ToDecimal(advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).FirstOrDefault().RCH_SO_RCVD_OTHER_AMT) - Convert.ToDecimal(advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).FirstOrDefault().ICH_OTHER_AMT_ALLOCATED)));
                                hdfCurPaidOtherAmount.Value = advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).Count() <= 0 ? "0.00" : GetFormattedCurrency(Convert.ToDecimal(advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).FirstOrDefault().IAD_OTHER_AMOUNT) > 0 ? advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).FirstOrDefault().IAD_OTHER_AMOUNT : hdfIsDedApplyClick.Value == "1" ? 0 : (Convert.ToDecimal(advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).FirstOrDefault().RCH_SO_RCVD_OTHER_AMT) - Convert.ToDecimal(advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).FirstOrDefault().ICH_OTHER_AMT_ALLOCATED)));
                                txtTaxSplit.Text = advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).Count() <= 0 ? "0.00" : GetFormattedCurrency(Convert.ToDecimal(advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).FirstOrDefault().IAD_TAX_AMOUNT) > 0 ? advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).FirstOrDefault().IAD_TAX_AMOUNT : hdfIsDedApplyClick.Value == "1" ? 0 : (Convert.ToDecimal(advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).FirstOrDefault().RCH_SO_RCVD_TAX_AMT) - Convert.ToDecimal(advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).FirstOrDefault().ICH_TAX_AMT_ALLOCATED)));
                                hdfCurPaidTax.Value = advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).Count() <= 0 ? "0.00" : GetFormattedCurrency(Convert.ToDecimal(advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).FirstOrDefault().IAD_TAX_AMOUNT) > 0 ? advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).FirstOrDefault().IAD_TAX_AMOUNT : hdfIsDedApplyClick.Value == "1" ? 0 : (deductionDtlList == null ? 0 : Convert.ToDecimal(deductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).FirstOrDefault().RCH_SO_RCVD_TAX_AMT) - Convert.ToDecimal(advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).FirstOrDefault().ICH_TAX_AMT_ALLOCATED)));
                                if ((EntryStatus == EntryStatus.NEWMODE || ReloadInvoice) && hdfIsDedApplyClick.Value == "0" && CusAdvFlag != 1)
                                {
                                    txtDedAllocateNowSplit.Text = GetFormattedCurrency(defaultAllocation);
                                    hdfDedAllocateNowSplit.Value = GetFormattedCurrency(defaultAllocation);
                                    hdfAllocNowAmount.Value = GetFormattedCurrency(defaultAllocation);
                                    txtOtherAmountSplit.Text = GetFormattedCurrency(defaultAllocation);
                                    hdfCurPaidOtherAmount.Value = GetFormattedCurrency(defaultAllocation);
                                    txtTaxSplit.Text = GetFormattedCurrency(defaultAllocation);
                                    hdfCurPaidTax.Value = GetFormattedCurrency(defaultAllocation);
                                }
                            }
                            else
                            {
                                if (ReloadInvoice)
                                {
                                    txtDedAllocateNowSplit.Text = advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).Count() <= 0 ? "0.00" : (GetFormattedCurrency(Convert.ToDecimal(advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).FirstOrDefault().IAD_AMOUNT) > 0 ? advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).FirstOrDefault().IAD_AMOUNT : hdfIsDedApplyClick.Value == "1" ? 0 : (Convert.ToDecimal(advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).FirstOrDefault().RCH_SO_RCVD_AMT) - Convert.ToDecimal(advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).FirstOrDefault().ICH_AMOUNT_ALLOCATED))));
                                    decimal DedInvoiceBal = Convert.ToDecimal(lblDedInvoiceBal.Text.Replace(",", ""));
                                    decimal DedInvoiceNow = Convert.ToDecimal(txtDedAllocateNowSplit.Text);
                                    txtDedAllocateNowSplit.Text = DedInvoiceNow > DedInvoiceBal ? Convert.ToString(DedInvoiceBal) : txtDedAllocateNowSplit.Text;
                                    hdfDedAllocateNowSplit.Value = advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).Count() <= 0 ? "0.00" : GetFormattedCurrency(Convert.ToDecimal(advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).FirstOrDefault().IAD_AMOUNT) > 0 ? advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).FirstOrDefault().IAD_AMOUNT : hdfIsDedApplyClick.Value == "1" ? 0 : (Convert.ToDecimal(advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).FirstOrDefault().RCH_SO_RCVD_AMT) - Convert.ToDecimal(advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).FirstOrDefault().ICH_AMOUNT_ALLOCATED)));
                                    hdfAllocNowAmount.Value = advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).Count() <= 0 ? "0.00" : GetFormattedCurrency(Convert.ToDecimal(advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).FirstOrDefault().RCH_SO_RCVD_AMT) - Convert.ToDecimal(advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).FirstOrDefault().ICH_AMOUNT_ALLOCATED));
                                    txtOtherAmountSplit.Text = advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).Count() <= 0 ? "0.00" : GetFormattedCurrency(Convert.ToDecimal(advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).FirstOrDefault().IAD_OTHER_AMOUNT) > 0 ? advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).FirstOrDefault().IAD_OTHER_AMOUNT : hdfIsDedApplyClick.Value == "1" ? 0 : (Convert.ToDecimal(advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).FirstOrDefault().RCH_SO_RCVD_OTHER_AMT) - Convert.ToDecimal(advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).FirstOrDefault().ICH_OTHER_AMT_ALLOCATED)));
                                    hdfCurPaidOtherAmount.Value = advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).Count() <= 0 ? "0.00" : GetFormattedCurrency(Convert.ToDecimal(advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).FirstOrDefault().IAD_OTHER_AMOUNT) > 0 ? advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).FirstOrDefault().IAD_OTHER_AMOUNT : hdfIsDedApplyClick.Value == "1" ? 0 : (Convert.ToDecimal(advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).FirstOrDefault().RCH_SO_RCVD_OTHER_AMT) - Convert.ToDecimal(advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).FirstOrDefault().ICH_OTHER_AMT_ALLOCATED)));
                                    txtTaxSplit.Text = advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).Count() <= 0 ? "0.00" : GetFormattedCurrency(Convert.ToDecimal(advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).FirstOrDefault().IAD_TAX_AMOUNT) > 0 ? advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).FirstOrDefault().IAD_TAX_AMOUNT : hdfIsDedApplyClick.Value == "1" ? 0 : (Convert.ToDecimal(advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).FirstOrDefault().RCH_SO_RCVD_TAX_AMT) - Convert.ToDecimal(advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).FirstOrDefault().ICH_TAX_AMT_ALLOCATED)));
                                    hdfCurPaidTax.Value = advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).Count() <= 0 ? "0.00" : GetFormattedCurrency(Convert.ToDecimal(advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).FirstOrDefault().IAD_TAX_AMOUNT) > 0 ? advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).FirstOrDefault().IAD_TAX_AMOUNT : hdfIsDedApplyClick.Value == "1" ? 0 : (deductionDtlList == null ? 0 : Convert.ToDecimal(deductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).FirstOrDefault().RCH_SO_RCVD_TAX_AMT) - Convert.ToDecimal(advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).FirstOrDefault().ICH_TAX_AMT_ALLOCATED)));
                                    if ((EntryStatus == EntryStatus.NEWMODE || ReloadInvoice) && hdfIsDedApplyClick.Value == "0" && CusAdvFlag != 1)
                                    {
                                        txtDedAllocateNowSplit.Text = GetFormattedCurrency(defaultAllocation);
                                        hdfDedAllocateNowSplit.Value = GetFormattedCurrency(defaultAllocation);
                                        hdfAllocNowAmount.Value = GetFormattedCurrency(defaultAllocation);
                                        txtOtherAmountSplit.Text = GetFormattedCurrency(defaultAllocation);
                                        hdfCurPaidOtherAmount.Value = GetFormattedCurrency(defaultAllocation);
                                        txtTaxSplit.Text = GetFormattedCurrency(defaultAllocation);
                                        hdfCurPaidTax.Value = GetFormattedCurrency(defaultAllocation);
                                    }
                                }
                                else
                                {
                                    txtDedAllocateNowSplit.Text = advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).Count() <= 0 ? "0.00" : GetFormattedCurrency(advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).FirstOrDefault().IAD_AMOUNT.ToString());
                                    hdfDedAllocateNowSplit.Value = advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).Count() <= 0 ? "0.00" : GetFormattedCurrency(advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).FirstOrDefault().IAD_AMOUNT.ToString());
                                    hdfAllocNowAmount.Value = advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).Count() <= 0 ? "0.00" : GetFormattedCurrency(Convert.ToDecimal(advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).FirstOrDefault().RCH_SO_RCVD_AMT) - Convert.ToDecimal(advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).FirstOrDefault().ICH_AMOUNT_ALLOCATED));
                                    txtOtherAmountSplit.Text = advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).Count() <= 0 ? "0.00" : GetFormattedCurrency(advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).FirstOrDefault().IAD_OTHER_AMOUNT.ToString());
                                    hdfCurPaidOtherAmount.Value = advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).Count() <= 0 ? "0.00" : GetFormattedCurrency(advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).FirstOrDefault().IAD_OTHER_AMOUNT.ToString());
                                    txtTaxSplit.Text = advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).Count() <= 0 ? "0.00" : GetFormattedCurrency(advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).FirstOrDefault().IAD_TAX_AMOUNT.ToString());
                                    hdfCurPaidTax.Value = advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).Count() <= 0 ? "0.00" : GetFormattedCurrency(advDeductionDtlList.Where(aa => (deductionPk > 0 ? aa.IAD_PK == deductionPk : aa.IAD_RECEIPT_HDR == receiptPK && aa.IAD_INVOICE_ADV == invPK && aa.IAD_SO == ScPK && aa.ICM_SO_HDR == IcmSoHdr)).FirstOrDefault().IAD_TAX_AMOUNT.ToString());
                                    if ((EntryStatus == EntryStatus.NEWMODE || ReloadInvoice) && hdfIsDedApplyClick.Value == "0" && CusAdvFlag != 1)
                                    {
                                        txtDedAllocateNowSplit.Text = GetFormattedCurrency(defaultAllocation);
                                        hdfDedAllocateNowSplit.Value = GetFormattedCurrency(defaultAllocation);
                                        hdfAllocNowAmount.Value = GetFormattedCurrency(defaultAllocation);
                                        txtOtherAmountSplit.Text = GetFormattedCurrency(defaultAllocation);
                                        hdfCurPaidOtherAmount.Value = GetFormattedCurrency(defaultAllocation);
                                        txtTaxSplit.Text = GetFormattedCurrency(defaultAllocation);
                                        hdfCurPaidTax.Value = GetFormattedCurrency(defaultAllocation);
                                    }
                                }
                            }
                        }
                        if (SaleOrderType == 1)
                        {
                            e.Row.Cells[10].Visible = true;
                            e.Row.Cells[11].Visible = true;
                        }
                        else
                        {
                            e.Row.Cells[10].Visible = false;
                            e.Row.Cells[11].Visible = false;
                        }
                        if (!IsAdvInvHasTax)//If Advance Invoice have no tax then hide Tax and OtherCharge Columns
                        {
                            e.Row.Cells[10].Visible = false;
                            e.Row.Cells[11].Visible = false;
                        }
                    }
                    #endregion
                    #region grdInvoice
                    else if (((GridView)sender).ID == "grdInvoice")
                    {
                        TextBox txtInvNow;
                        LinkButton lblSoNo;
                        Label lblInvProformaQuantity;
                        txtInvNow = e.Row.FindControl("txtInvNow") as TextBox;
                        lblSoNo = e.Row.FindControl("lnkSoNo") as LinkButton;
                        lblInvProformaQuantity = e.Row.FindControl("lblInvProformaQuantity") as Label;
                        if (Convert.ToByte(ddlInvoiceType.SelectedValue) == Convert.ToByte(SalesInvoiceType.Proforma))
                        {
                            e.Row.Cells[6].Visible = true;
                        }
                        else
                        {
                            e.Row.Cells[6].Visible = false;
                        }
                        e.Row.Cells[9].Visible = IsItemwiseDiscountForTradingSale;
                        e.Row.Cells[10].Visible = IsItemwiseTaxForTradingSale;
                    }
                    #endregion
                    #region grdTaxPayable
                    else if (((GridView)sender).ID == "grdTaxPayable")
                    {
                        Label lblTaxAmount = e.Row.FindControl("lblTaxAmount") as Label;
                        Label lblAmountBeforeTax = e.Row.FindControl("lblAmountBeforeTax") as Label;
                        Label lblTaxCode = e.Row.FindControl("lblTaxCode") as Label;

                        double ExchangeRate = 0, TaxAmount = 0, AmountBeforTax = 0;
                        double TaxAmountLocal = 0, AmountBeforTaxLocal = 0;
                        ExchangeRate = string.IsNullOrEmpty(txtExchangeRate.Text) ? 1 : Convert.ToDouble(txtExchangeRate.Text);
                        TaxAmount = string.IsNullOrEmpty(lblTaxAmount.Text.Replace(",", "")) ? 1 : Convert.ToDouble(lblTaxAmount.Text.Replace(",", ""));
                        AmountBeforTax = string.IsNullOrEmpty(lblAmountBeforeTax.Text) ? 1 : Convert.ToDouble(lblAmountBeforeTax.Text.Replace(",", ""));

                        //Multiply with Exchangerate                       
                        AmountBeforTaxLocal = AmountBeforTax * ExchangeRate;
                        TaxAmountLocal = TaxAmount * ExchangeRate;

                        lblAmountBeforeTax.Text = GetFormattedCurrencyWithComa(AmountBeforTaxLocal);
                        lblTaxAmount.Text = GetFormattedCurrencyWithComa(TaxAmountLocal);
                        //For Avoiding Tax code shown as NULL.
                        if (lblTaxCode.Text.ToString() == "null" || lblTaxCode.Text.ToString() == "Null")
                        {
                            lblTaxCode.Text = "";
                        }
                    }
                    #endregion
                    #region grdUploads
                    else if (((GridView)sender).ID == "grdUploads")
                    {
                        int slno;
                        slno = Convert.ToInt32(grdUploads.DataKeys[e.Row.RowIndex][0]);
                        if (slno > 0)
                        {
                            e.Row.FindControl("fileView").Visible = FileDetailsList == null || FileDetailsList.Where(fle => fle.SlNo == slno).Count() == 0;
                        }
                        if (EntryStatus == EntryStatus.VIEWMODE)
                        {
                            //e.Row.Cells[4].Visible = false;
                            e.Row.Cells[3].Visible = false;
                            e.Row.Cells[4].Visible = false;
                        }
                    }
                    #endregion
                    #region grdSoList
                    else if (((GridView)sender).ID == "grdSoList")
                    {
                        HiddenField hdfDOPk = e.Row.FindControl("hdfDOPk") as HiddenField;
                        HiddenField hdfSOPk = e.Row.FindControl("hdfSOPk") as HiddenField;
                        CheckBox chkSoSelect = e.Row.FindControl("chkSoSelect") as CheckBox;
                        if (SOInvoiceHeaderSession != null && SOInvoiceHeaderSession.OrderDetail.Where(r => r.CID_SO == hdfSOPk.Value).Count() > 0
                            && SOInvoiceHeaderSession.ICH_DESPATCH_HDR == hdfDOPk.Value)
                        {
                            e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml(GetGlobalResourceObject("ErpRes", "selectedRowColor").ToString());
                            chkSoSelect.Checked = true;
                            chkSoSelect.Enabled = false;
                        }
                    }
                    #endregion
                }
                #endregion
                #region Header Row
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    #region grdDeduction
                    if (((GridView)sender).ID == "grdDeduction")
                    {
                        if (SaleOrderType == 1)
                        {
                            e.Row.Cells[10].Visible = true;
                            e.Row.Cells[11].Visible = true;
                        }
                        else
                        {
                            e.Row.Cells[10].Visible = false;
                            e.Row.Cells[11].Visible = false;
                        }
                        if (!IsAdvInvHasTax)//If Advance Invoice have no tax then hide Tax and OtherCharge Columns
                        {
                            e.Row.Cells[10].Visible = false;
                            e.Row.Cells[11].Visible = false;
                        }
                    }
                    #endregion
                    #region grdInvoice
                    else if (((GridView)sender).ID == "grdInvoice")
                    {
                        if (Convert.ToByte(ddlInvoiceType.SelectedValue) == Convert.ToByte(SalesInvoiceType.Proforma))
                        {
                            e.Row.Cells[6].Visible = true;
                        }
                        else
                        {
                            e.Row.Cells[6].Visible = false;
                        }
                        e.Row.Cells[9].Visible = IsItemwiseDiscountForTradingSale;
                        e.Row.Cells[10].Visible = IsItemwiseTaxForTradingSale;
                    }
                    #endregion
                    #region grdUploads
                    else if (((GridView)sender).ID == "grdUploads")
                    {
                        if (EntryStatus == EntryStatus.VIEWMODE)
                        {
                            e.Row.Cells[3].Visible = false;
                            e.Row.Cells[4].Visible = false;
                        }
                    }
                    #endregion
                }
                #endregion
                #region Footer Row
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    #region grdDeduction
                    if (((GridView)sender).ID == "grdDeduction")
                    {
                        decimal total = TempSOInvoiceHeaderSession.DeductionDetails.Sum(aa => aa.IAD_AMOUNT);
                        decimal totalTax = TempSOInvoiceHeaderSession.DeductionDetails.Sum(aa => aa.IAD_TAX_AMOUNT);

                        Label lblDedTotalAllocateNowFooterSplit = e.Row.FindControl("lblDedTotalAllocateNowFooterSplit") as Label;
                        Label lblTotalTaxFooter = e.Row.FindControl("lblTotalTaxFooter") as Label;

                        HiddenField hdfDedTotalAllocateNowFooterSplit = e.Row.FindControl("hdfDedTotalAllocateNowFooterSplit") as HiddenField;
                        HiddenField hdfTaxTotalFooterSplit = e.Row.FindControl("hdfTaxTotalFooterSplit") as HiddenField;

                        total = total < 0 ? 0 : total;
                        hdfDedTotalAllocateNowFooterSplit.Value = total.ToString(hdfCurrencyFormat.Value);
                        //hdfDedTotalAllocateNowFooterSplit.Value = lblDedTotalAllocateNowFooterSplit.Text = total.ToString(hdfCurrencyFormat.Value);
                        lblTotalTaxFooter.Text = GetFormattedCurrencyWithComa(totalTax);//.ToString(hdfCurrencyFormat.Value);
                        hdfTaxTotalFooterSplit.Value = totalTax.ToString(hdfCurrencyFormat.Value);
                        if (SaleOrderType == 1)
                        {
                            e.Row.Cells[10].Visible = true;
                            e.Row.Cells[11].Visible = true;
                        }
                        else
                        {
                            e.Row.Cells[10].Visible = false;
                            e.Row.Cells[11].Visible = false;
                        }
                        if (!IsAdvInvHasTax)//If Advance Invoice have no tax then hide Tax and OtherCharge Columns
                        {
                            e.Row.Cells[10].Visible = false;
                            e.Row.Cells[11].Visible = false;
                        }

                        //Calculate sum using script
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateTotalSplit", "CalculateTotalSplit();", true);
                    }
                    #endregion
                    #region grdInvoice
                    else if (((GridView)sender).ID == "grdInvoice")
                    {
                        if (Convert.ToByte(ddlInvoiceType.SelectedValue) == Convert.ToByte(SalesInvoiceType.Proforma))
                        {
                            e.Row.Cells[6].Visible = true;
                        }
                        else
                        {
                            e.Row.Cells[6].Visible = false;
                        }
                        e.Row.Cells[9].Visible = IsItemwiseDiscountForTradingSale;
                        e.Row.Cells[10].Visible = IsItemwiseTaxForTradingSale;
                    }
                    #endregion
                }
                #endregion
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
            SetAllocationDetails();
            PageIndex = e.NewPageIndex.ToString();
            GetFieldValues(ControlsEnum.INVOICELIST);
            SetFieldValues(ControlsEnum.INVOICELIST);
            EntryStatus = EntryStatus.LISTMODE;
            SetGridStatus();
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
                //if (SortBy == e.SortExpression)
                //{
                //    //Toggle the sort expression
                //    if (SortDirection == Resources.Report.SortAscending)
                //        SortDirection = Resources.Report.SortDescending;
                //    else
                //        SortDirection = Resources.Report.SortAscending;
                //}
                //else
                //{
                //    SortBy = e.SortExpression;
                //    SortDirection = Resources.Report.SortAscending;

                //}
                //this.PageIndex = "1";
                //GetFieldValues(ControlsEnum.INVOICELIST);
                //SetFieldValues(ControlsEnum.INVOICELIST);
                //EntryStatus = EntryStatus.LISTMODE;
                SortBy = e.SortExpression;
                if (SortDirection == Resources.Report.SortAscending)
                    SortDirection = Resources.Report.SortDescending;
                else
                    SortDirection = Resources.Report.SortAscending;
                GetFieldValues(ControlsEnum.INVOICELIST);
                SetFieldValues(ControlsEnum.INVOICELIST);
                EntryStatus = EntryStatus.LISTMODE;
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
            uclSOPaging.CurrentPage = 1;
            btnSaveSubmit.PreRender += new EventHandler(btnAction_PreRender);
            btnInvSubmit.PreRender += new EventHandler(btnAction_PreRender);
            btnSave.PreRender += new EventHandler(btnAction_PreRender);
            btnDeleteNew.PreRender += new EventHandler(btnAction_PreRender);
            btnPrint.PreRender += new EventHandler(btnAction_PreRender);
            btnCancel.PreRender += new EventHandler(btnAction_PreRender);
            btnJournalize.PreRender += new EventHandler(btnAction_PreRender);
            btnAlert.PreRender += new EventHandler(btnAction_PreRender);
            btnPrintSI.PreRender += new EventHandler(btnAction_PreRender);
            btnEdit.PreRender += new EventHandler(btnAction_PreRender);
            btnView.PreRender += new EventHandler(btnAction_PreRender);
            lnkList.PreRender += new EventHandler(btnAction_PreRender);
            lnkDetail.PreRender += new EventHandler(btnAction_PreRender);
            btnApply.PreRender += new EventHandler(btnAction_PreRender);
            imgPopupAdd.PreRender += new EventHandler(btnAction_PreRender);
            btnSaveDuduction.PreRender += new EventHandler(btnAction_PreRender);
            btnInActive.PreRender += new EventHandler(btnAction_PreRender);
            btnDeleteOK.PreRender += new EventHandler(btnAction_PreRender);
            btnEditforCancel.PreRender += new EventHandler(btnAction_PreRender);
            btnCancelSubmit.PreRender += new EventHandler(btnAction_PreRender);
            btnSaveSubmit.Load += new EventHandler(btnAction_Load);
            btnInvSubmit.Load += new EventHandler(btnAction_Load);
            btnSave.Load += new EventHandler(btnAction_Load);
            btnDeleteNew.Load += new EventHandler(btnAction_Load);
            btnPrint.Load += new EventHandler(btnAction_Load);
            btnCancel.Load += new EventHandler(btnAction_Load);
            btnJournalize.Load += new EventHandler(btnAction_Load);
            btnAlert.Load += new EventHandler(btnAction_Load);
            btnPrintSI.Load += new EventHandler(btnAction_Load);
            btnEdit.Load += new EventHandler(btnAction_Load);
            btnView.Load += new EventHandler(btnAction_Load);
            lnkList.Load += new EventHandler(btnAction_Load);
            lnkDetail.Load += new EventHandler(btnAction_Load);
            btnApply.Load += new EventHandler(btnAction_Load);
            imgPopupAdd.Load += new EventHandler(btnAction_Load);
            btnInActive.Load += new EventHandler(btnAction_Load);
            btnDeleteOK.Load += new EventHandler(btnAction_Load);
            btnSaveDuduction.Load += new EventHandler(btnAction_Load);
            btnEditforCancel.Load += new EventHandler(btnAction_Load);
            btnCancelSubmit.Load += new EventHandler(btnAction_Load);
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
            this.uclSOPaging.FirstPage += new ActionHandler(this.ActionHandler);
            this.uclSOPaging.PreviousPage += new ActionHandler(this.ActionHandler);
            this.uclSOPaging.NextPage += new ActionHandler(this.ActionHandler);
            this.uclSOPaging.LastPage += new ActionHandler(this.ActionHandler);
            this.uclSOPaging.PageChanged += new ActionHandler(this.ActionHandler);

            this.uclInvListPaging.FirstPage += new ActionHandler(this.ActionHandler);
            this.uclInvListPaging.PreviousPage += new ActionHandler(this.ActionHandler);
            this.uclInvListPaging.NextPage += new ActionHandler(this.ActionHandler);
            this.uclInvListPaging.LastPage += new ActionHandler(this.ActionHandler);
            this.uclInvListPaging.PageChanged += new ActionHandler(this.ActionHandler);
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
                PgerControlNew pagerControl = (PgerControlNew)sender;
                string senderId = pagerControl.ID;
                if (senderId == "uclSOPaging")
                {
                    switch (e.Action)
                    {
                        case NavigationEnum.PAGECHANGE:
                            uclSOPaging.CurrentPage = e.CurrentPage;
                            break;
                        case NavigationEnum.FIRST:
                            if (e.CurrentPage > 1)
                                uclSOPaging.CurrentPage = 1;
                            break;
                        case NavigationEnum.LAST:
                            if (e.CurrentPage <= e.TotalPages)
                                uclSOPaging.CurrentPage = e.TotalPages;
                            break;
                        case NavigationEnum.NEXT:
                            // increment the current page index.
                            if (e.CurrentPage <= e.TotalPages)
                                uclSOPaging.CurrentPage++;
                            break;
                        case NavigationEnum.PREVIOUS:
                            // Decrement the current page index.
                            if (e.CurrentPage > 1)
                                uclSOPaging.CurrentPage--;
                            break;
                    }
                    PageIndexSO = uclSOPaging.CurrentPage.ToString();
                    GetFieldValues(ControlsEnum.PEDINGSOLIST);
                    SetFieldValues(ControlsEnum.PEDINGSOLIST);
                    EnableDisableButtons(e.TotalPages, "uclSOPaging");
                }
                else if (senderId == "uclInvListPaging")
                {
                    switch (e.Action)
                    {
                        case NavigationEnum.PAGECHANGE:
                            uclInvListPaging.CurrentPage = e.CurrentPage;
                            break;
                        case NavigationEnum.FIRST:
                            if (e.CurrentPage > 1)
                                uclInvListPaging.CurrentPage = 1;
                            break;
                        case NavigationEnum.LAST:
                            if (e.CurrentPage <= e.TotalPages)
                                uclInvListPaging.CurrentPage = e.TotalPages;
                            break;
                        case NavigationEnum.NEXT:
                            // increment the current page index.
                            if (e.CurrentPage <= e.TotalPages)
                                uclInvListPaging.CurrentPage++;
                            break;
                        case NavigationEnum.PREVIOUS:
                            // Decrement the current page index.
                            if (e.CurrentPage > 1)
                                uclInvListPaging.CurrentPage--;
                            break;
                    }
                    PageIndex = uclInvListPaging.CurrentPage.ToString();
                    SetAllocationDetails();
                    GetFieldValues(ControlsEnum.INVOICELIST);
                    SetFieldValues(ControlsEnum.INVOICELIST);
                    EntryStatus = EntryStatus.LISTMODE;
                    SetGridStatus();
                    EnableDisableButtons(e.TotalPages, "uclInvListPaging");
                }
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
        private void EnableDisableButtons(int iTotalPages, string pagerId)
        {
            if (pagerId == "uclSOPaging")
            {
                uclSOPaging.FirstButtonEnabled = (uclSOPaging.CurrentPage == 1) ? false : true;
                uclSOPaging.PreviousButtonEnabled = (uclSOPaging.CurrentPage == 1) ? false : true;
                uclSOPaging.NextButtonEnabled = (uclSOPaging.CurrentPage < iTotalPages) ? true : false;
                uclSOPaging.LastButtonEnabled = (uclSOPaging.CurrentPage < iTotalPages) ? true : false;
            }
            else if (pagerId == "uclInvListPaging")
            {
                uclInvListPaging.FirstButtonEnabled = (uclInvListPaging.CurrentPage == 1) ? false : true;
                uclInvListPaging.PreviousButtonEnabled = (uclInvListPaging.CurrentPage == 1) ? false : true;
                uclInvListPaging.NextButtonEnabled = (uclInvListPaging.CurrentPage < iTotalPages) ? true : false;
                uclInvListPaging.LastButtonEnabled = (uclInvListPaging.CurrentPage < iTotalPages) ? true : false;
            }
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
                hdfgroup.Value = Convert.ToByte((byte)SalesInvoiceGroup.Goods).ToString();
                hdfSaveWithoutAllocation.Value = "0";
                hdfInvPk.Value = CurrPK.ToString();
                if (SOInvoiceHeaderSession != null)
                {
                    if (SOInvoiceHeaderSession.OrderDetail != null)
                    {
                        hdfHasTax.Value = ((SOInvoiceHeaderSession.TaxHdr == null || SOInvoiceHeaderSession.TaxHdr.Count == 0)
                            && SOInvoiceHeaderSession.OrderDetail.All(dtl => (dtl.TaxDtl == null || dtl.TaxDtl.Count == 0)))
                            ? CommonConstants.SELECT_VALUE_ZERO : CommonConstants.SELECT_VALUE_ONE;
                    }
                }
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
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SetPrintDocsVisibility", "$(document).ready(function(){SetPrintDocsVisibility();});", true);
                }
                else if (EntryStatus == EntryStatus.ENTRYMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "PageViewMode", "$(document).ready(function(){PageViewMode(2);});", true);
                    lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                }
                else if (EntryStatus == EntryStatus.LISTMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowListing", "$(document).ready(function(){ShowListing(1);});", true);
                    lnkList.CssClass = GetLocalResourceObject("TabActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabInActive").ToString();
                }
                hdfMode.Value = EntryStatus.ToString();
                if (grdInvoice.Rows.Count > 0)
                {
                    txtCustomer.Enabled = false;
                    tblCalc.Visible = true;
                }
                else
                {
                    txtCustomer.Enabled = true;
                    tblCalc.Visible = false;
                }
                custPK = 0;
                if (int.TryParse(hdfCustomer.Value, out custPK) && custPK > 0)
                    txtDONumber.Enabled = true;
                else
                    txtDONumber.Enabled = false;

                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_CalculateTotal", "$(document).ready(function(){CalculateTotal();});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideAdvancedSearch();});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){CalculateTotalSplit();});", true);
                //   ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SetPrintDocsVisibility", "$(document).ready(function(){SetPrintDocsVisibility();});", true);
                //Settings of TaxPayableDiv              
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "TaxPayable", "$(document).ready(function () { ShowHideTaxPayableOuter();});", true);
                //end
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "RemoveLink", "$(document).ready(function () { RemoveBalAmntHyperLink();});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowUploadDocDetails", "$(document).ready(function(){ShowHideUploadDocDetails();});", true);
                if (IsDeleted)
                {
                    btnSave.Visible = false;
                    btnDeleteNew.Visible = false;
                    hdfIsInvCancelled.Value = "1";
                    //btnSaveSubmit.Visible = false;
                    //btnSubmit.Visible = false;
                }
                else
                    hdfIsInvCancelled.Value = "0";

                if (RefreshInvoice)
                    btnRefreshInv.Visible = true;
                else
                    btnRefreshInv.Visible = false;
                if (hdfType.Value != string.Empty && CurrPK > 0)//Check whether new mode or not
                {
                    btnExcelPrint.Visible = GetGlobalResourceObject("ConfigurationsRes", "ShowSIExcelPrint").ToString() == "0" ? false : true;//Configuration Settings
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
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
            // if one page containes two process (pageurl?PID=1,pageurl?PID=2)
            //if (Request.QueryString[QueryStrings.PID] == null)
            //{
            if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
                path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "") + "?PID=" + pid.ToString();
            else
                path = Request.Url.AbsolutePath.ToLower() + "?PID=" + pid.ToString();
            //}
            //else
            //{
            //    if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
            //        path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "");
            //    else
            //        path = Request.Url.AbsolutePath.ToLower();
            //}

            if (Session[BusinessObject.Common.SessionStrings.CurDept] != null)
            {
                WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
                DataTable dtProcess = wrkfService.GetProcessID(path, Convert.ToInt32(Session[BusinessObject.Common.SessionStrings.CurDept].ToString()));
                if (dtProcess != null && dtProcess.Rows.Count > 0)
                {
                    ucrWrkf.PageUrl = path;
                    ucrWrkf.ProcessID = int.Parse(dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString());
                    hdfProcessID.Value = dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString();
                    if (pid == 1 || pid == 3)
                    {
                        PageProcessID = ucrWrkf.ProcessID;

                    }
                    base.WkfPageUrl = path;
                }
            }
        }

        #endregion
        #region Enum
        /// <summary>
        ///Page Controls Enum 
        /// </summary>
        public enum ControlsEnum
        {
            SOINVHEADER,
            SOINVDETAIL,
            TAXTYPES,
            TAXPOPUPGRID,
            TAXHEADER,
            EXCHANGERATE,
            INVOICELIST,
            INVOICEHDR,
            JOURNALIZE,
            FINHEADER,
            SOTYPE,
            GETINVOICEPKBYJOURNALPK,
            FILLWORKFLOWSTATUS,
            INVOICETYPE,
            DEDUCTIONPOPUPGRID,
            TAXSETTINGS,
            ADVANCEDTAXSETTINGS,
            NOTIFICATIONTYPES,
            ALERTBASIS,
            ALERTTYPES,
            NOTIFICATIONDAYS,
            ALERTCONFIG,
            ALERTLIST,
            ALERTSAVE,
            COMPANY,
            SOINVHEADERBYPK,
            PAYMENTTERMS,
            GETDUEDATE,
            CUSTOMERTYPES,
            GETCUSTOMERDETAILSBYTYPE,
            LINETAX,
            SHOWPOPUP,
            AMOUNTDETAILS,
            CUSTOMTAXSETTINGS,
            DUEDATEPOPUPGRID,
            DUEDATEDETAIL,
            RESETFORMODIFIEDDO,
            OTHERCHARGELIST,
            SELECTEDDOC,
            UPLOADEDFILES,
            ADDITEM,
            INVOICEGET,
            PEDINGSOLIST,
            TAXCHECKBOX,
            INVOICEGSTTYPE
        }
        /// <summary>
        /// Define Activity Status Enum
        /// </summary>
        public enum WkfStatusEnum
        {
            DRAFTED = 0,
            APPROVED = 2
        }

        private enum HdrTaxCalc
        {

        }
        #endregion

        #region CustomerContactTypes
        public enum CustomerContactTypeEnum
        {
            HeadOffice = 4,
            Branch = 5
        }
        #endregion

        /// <summary>
        /// Define Activity Status Enum
        /// </summary>
        public enum TaxSettingEnum
        {
            HEADERWISE = 0,
            ITEMWISE = 1,
            BOTHHEADERITEM = 2
        }

        private double SetTaxApplicableAmount()
        {
            double hdrSubTotal = 0;
            double hdrSubTotalInvNow = 0;
            double hdrDisc = 0;
            double hdrOtherCharge = 0;
            double amount = 0;

            if (chkSubTotal.Checked)
            {
                TextBox txtSubTotal = (TextBox)grdInvoice.FooterRow.FindControl("txtSubTotalFooter");
                hdrSubTotal = txtSubTotal == null ? 0 : string.IsNullOrEmpty(txtSubTotal.Text.Trim()) ? 0 : Convert.ToDouble(txtSubTotal.Text.Trim());

                TextBox txtSubTotalInvNow = (TextBox)grdInvoice.FooterRow.FindControl("txtSubTotalFooteInvNowr");
                hdrSubTotalInvNow = txtSubTotalInvNow == null ? 0 : string.IsNullOrEmpty(txtSubTotalInvNow.Text.Trim()) ? 0 : Convert.ToDouble(txtSubTotalInvNow.Text.Trim());
            }
            if (chkDiscount.Checked)
            {
                hdrDisc = string.IsNullOrEmpty(txtHdrDiscount.Text.Trim()) ? 0 : Convert.ToDouble(txtHdrDiscount.Text.Trim());
            }
            if (chkOtherCharges.Checked)
            {
                hdrOtherCharge = string.IsNullOrEmpty(txtShipping.Text.Trim()) ? 0 : Convert.ToDouble(txtShipping.Text.Trim());
            }
            if (hdrSubTotal == 0)
                amount = hdrDisc + hdrOtherCharge;
            else
                amount = (hdrSubTotal - hdrDisc) + hdrOtherCharge;
            return amount;
        }

    }
}