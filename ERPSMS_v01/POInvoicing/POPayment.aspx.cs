using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.AccountManagement;
using ERPManager;
using ERPData;
using ERP.Utilities;
using ERPSMS_v01.UserControls;
using BusinessObject.Common;
using ERPService;
using BusinessObject.CommonManagement;
using BusinessObject;
using System.Data;
using System.Threading;
using System.Web.UI.HtmlControls;
using CustomControls;
using BusinessLogic.CommonManagement;
using System.Text;
using BusinessObject.AlertManagement;
using BusinessObject.Journalize;
using System.IO;
using BusinessObject.POInvoicing;
using ERPManager.POInvoicing;
using System.Drawing;


namespace ERPSMS_v01.POInvoicing
{
    public partial class POPayment : ERP.Store.UI.WorkFlowBasePage
    {
        #region Variables and Properties
        #region Properties
        /// <summary>
        /// Invoice PO Split List
        /// </summary>
        //private List<FIN_PAYMENT_VND_PO_MPG> InvoicePOSplitList
        //{
        //    get
        //    {
        //        return Session[ERP.Utilities.SessionStrings.InvoicePOSplitList] == null ? new List<FIN_PAYMENT_VND_PO_MPG>()
        //            : (List<FIN_PAYMENT_VND_PO_MPG>)Session[ERP.Utilities.SessionStrings.InvoicePOSplitList];
        //    }
        //    set
        //    {
        //        if (value == null)
        //            Session.Remove(ERP.Utilities.SessionStrings.InvoicePOSplitList);
        //        else
        //            Session[ERP.Utilities.SessionStrings.InvoicePOSplitList] = value;
        //    }
        //}
        private List<FIN_PAYMENT_VND_PO_MPG> InvoicePOSplitList
        {

            get
            {
                return this.ViewState[ViewstateStrings.InvoicePOSplitList] != null ? (List<FIN_PAYMENT_VND_PO_MPG>)this.ViewState[ViewstateStrings.InvoicePOSplitList] : new List<FIN_PAYMENT_VND_PO_MPG>();

            }
            set
            {
                if (value == null)
                    ViewState.Remove(ViewstateStrings.InvoicePOSplitList);
                else
                    this.ViewState[ViewstateStrings.InvoicePOSplitList] = value;
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
        /// Invoice PO Split List
        /// </summary>
        private List<FIN_PAYMENT_VND_TAX_HDR> WHTTaxDetails
        {
            get
            {
                return Session[ERP.Utilities.SessionStrings.WHTTaxDetails] == null ? new List<FIN_PAYMENT_VND_TAX_HDR>()
                    : (List<FIN_PAYMENT_VND_TAX_HDR>)Session[ERP.Utilities.SessionStrings.WHTTaxDetails];
            }
            set
            {
                if (value == null)
                    Session.Remove(ERP.Utilities.SessionStrings.WHTTaxDetails);
                else
                    Session[ERP.Utilities.SessionStrings.WHTTaxDetails] = value;
            }
        }
        private List<FIN_PAYMENT_VND_TAX_HDR> TempWHTTaxDetails
        {
            get
            {
                return Session[ERP.Utilities.SessionStrings.TempWHTTaxDetails] == null ? new List<FIN_PAYMENT_VND_TAX_HDR>()
                    : (List<FIN_PAYMENT_VND_TAX_HDR>)Session[ERP.Utilities.SessionStrings.TempWHTTaxDetails];
            }
            set
            {
                if (value == null)
                    Session.Remove(ERP.Utilities.SessionStrings.TempWHTTaxDetails);
                else
                    Session[ERP.Utilities.SessionStrings.TempWHTTaxDetails] = value;
            }
        }
        /// <summary>
        /// Payment category From PI
        /// </summary>
        private POInvoiceCategory PICategory
        {
            get
            {
                return (this.ViewState[ViewstateStrings.PICategory] == null ? (POInvoiceCategory)Enum.Parse(typeof(POInvoiceCategory),
                    CommonConstants.SELECT_VALUE_ONE) : (POInvoiceCategory)this.ViewState[ViewstateStrings.PICategory]);
            }
            set
            {
                this.ViewState[ViewstateStrings.PICategory] = value;
            }
        }
        /// <summary>
        /// Payment Type from PI
        /// </summary>
        private POInvoiceGroup POGroup
        {
            get
            {
                return (this.ViewState[ViewstateStrings.POGroup] == null ? (POInvoiceGroup)Enum.Parse(typeof(POInvoiceGroup),
                    CommonConstants.SELECT_VALUE_ONE) : (POInvoiceGroup)this.ViewState[ViewstateStrings.POGroup]);
            }
            set
            {
                this.ViewState[ViewstateStrings.POGroup] = value;
            }
        }
        /// <summary>
        /// Process ID of the Page
        /// </summary>
        private byte WkfStatus
        {
            get
            {
                return this.ViewState["WkfStatus"] == null ? Convert.ToByte(0) : Convert.ToByte(this.ViewState["WkfStatus"]);
            }
            set
            {
                this.ViewState["WkfStatus"] = value;
            }
        }
        private Boolean IsWorkOrder
        {
            get
            {
                return this.ViewState["IsWorkOrder"] == null ? false : Convert.ToBoolean(this.ViewState["IsWorkOrder"]);
            }
            set
            {
                this.ViewState["IsWorkOrder"] = value;
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
        private long CurrPK
        {
            get
            {
                return Convert.ToInt64(this.ViewState[ViewstateStrings.CurrPK]);
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
        /// VendorPK
        /// </summary>
        private int VendorPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.VendorPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.VendorPK] = value;
            }
        }

        /// <summary>
        /// VendorPK
        /// </summary>
        private long PaymentMpgPK
        {
            get
            {
                return Convert.ToInt64(this.ViewState[ViewstateStrings.paymentMpgPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.paymentMpgPK] = value;
            }
        }

        /// <summary>
        /// VendorPK
        /// </summary>
        private decimal PayNowAmount
        {
            get
            {
                return Convert.ToDecimal(this.ViewState[ViewstateStrings.PayNowAmount]);
            }
            set
            {
                this.ViewState[ViewstateStrings.PayNowAmount] = value;
            }
        }

        /// <summary>
        /// VendorPK
        /// </summary>
        private long InvoicePK
        {
            get
            {
                return Convert.ToInt64(this.ViewState[ViewstateStrings.invoicePK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.invoicePK] = value;
            }
        }
        /// <summary>
        /// VendorCode
        /// </summary>
        private string VendorCode
        {
            get
            {
                return this.ViewState[ViewstateStrings.VendorCode].ToString();
            }
            set
            {
                this.ViewState[ViewstateStrings.VendorCode] = value;
            }
        }
        /// <summary>
        /// VendorName
        /// </summary>
        private string VendorName
        {
            get
            {
                return this.ViewState[ViewstateStrings.VendorName].ToString();
            }
            set
            {
                this.ViewState[ViewstateStrings.VendorName] = value;
            }
        }
        /// <summary>
        /// IsVendorSelected
        /// </summary>
        private bool IsVendorSelected
        {
            get
            {
                return (bool)this.ViewState[ViewstateStrings.IsVendorSelected];
            }
            set
            {
                this.ViewState[ViewstateStrings.IsVendorSelected] = value;
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
        /// To maintain the tax in viewstate
        /// </summary>
        private decimal Tax
        {
            get
            {
                return this.ViewState[ViewstateStrings.Tax] == null ? 0 : Convert.ToDecimal(this.ViewState[ViewstateStrings.Tax]);
            }
            set
            {
                this.ViewState[ViewstateStrings.Tax] = value;
            }
        }

        /// <summary>
        /// To maintain keep selected pos
        /// </summary>
        private List<long> SelectedInvoices
        {
            get
            {
                return (List<long>)Session[ERP.Utilities.SessionStrings.SelectedInvoices];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.SelectedInvoices] = value;
            }

        }

        /// <summary>
        /// To maintain selected invoices in view state
        /// </summary>
        private List<long> selectedInvoiceList
        {
            get
            {
                return (List<long>)ViewState[ERP.Utilities.ViewstateStrings.SelectedInvoices];
            }
            set
            {
                ViewState[ERP.Utilities.ViewstateStrings.SelectedInvoices] = value;
            }
        }


        /// <summary>
        /// To maintain the other charge in viewstate
        /// </summary>
        private decimal InvOtherCharge
        {
            get
            {
                return this.ViewState[ViewstateStrings.POTotalOtherAmount] == null ? 0 : Convert.ToDecimal(this.ViewState[ViewstateStrings.POTotalOtherAmount]);
            }
            set
            {
                this.ViewState[ViewstateStrings.POTotalOtherAmount] = value;
            }
        }

        /// <summary>
        /// To maintain selected invoices
        /// </summary>


        private List<FIN_INVOICE_VND_HDR> PurInvoices
        {
            get
            {
                return Session[ERP.Utilities.SessionStrings.PurInvoices] == null ? new List<FIN_INVOICE_VND_HDR>()
                    : (List<FIN_INVOICE_VND_HDR>)Session[ERP.Utilities.SessionStrings.PurInvoices];
            }
            set
            {
                if (value == null)
                    Session.Remove(ERP.Utilities.SessionStrings.PurInvoices);
                else
                    Session[ERP.Utilities.SessionStrings.PurInvoices] = value;
            }
        }

        /// <summary>
        /// To maintain keep selected pos
        /// </summary>
        private List<ERPData.FIN_INVOICE_VND_HDR> EditedInvoices
        {
            get
            {
                return (List<ERPData.FIN_INVOICE_VND_HDR>)Session[ERP.Utilities.SessionStrings.EditedInvoices];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.EditedInvoices] = value;
            }

        }
        private List<ERPData.FIN_PAYMENT_VND_TRX_MPG> EditedPaymentDtls
        {
            get
            {
                return (List<ERPData.FIN_PAYMENT_VND_TRX_MPG>)Session[ERP.Utilities.SessionStrings.EditedPaymentDtls];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.EditedPaymentDtls] = value;
            }

        }

        private List<FIN_PAYMENT_VND_TAX_HDR> VATTaxDetails
        {
            get
            {
                return Session[ERP.Utilities.SessionStrings.VATTaxDetails] == null ? new List<FIN_PAYMENT_VND_TAX_HDR>()
                    : (List<FIN_PAYMENT_VND_TAX_HDR>)Session[ERP.Utilities.SessionStrings.VATTaxDetails];
            }
            set
            {
                if (value == null)
                    Session.Remove(ERP.Utilities.SessionStrings.VATTaxDetails);
                else
                    Session[ERP.Utilities.SessionStrings.VATTaxDetails] = value;
            }
        }

        private List<FIN_PAYMENT_VND_TAX_HDR> TempVATTaxDetails
        {
            get
            {
                return Session[ERP.Utilities.SessionStrings.TempVATTaxDetails] == null ? new List<FIN_PAYMENT_VND_TAX_HDR>()
                    : (List<FIN_PAYMENT_VND_TAX_HDR>)Session[ERP.Utilities.SessionStrings.TempVATTaxDetails];
            }
            set
            {
                if (value == null)
                    Session.Remove(ERP.Utilities.SessionStrings.TempVATTaxDetails);
                else
                    Session[ERP.Utilities.SessionStrings.TempVATTaxDetails] = value;
            }
        }

        private List<ADM_CONFIG_MST> TempConfigMstDetails
        {
            get
            {
                return Session[ERP.Utilities.SessionStrings.TempConfigMstDetails] == null ? new List<ADM_CONFIG_MST>()
                    : (List<ADM_CONFIG_MST>)Session[ERP.Utilities.SessionStrings.TempConfigMstDetails];
            }
            set
            {
                if (value == null)
                    Session.Remove(ERP.Utilities.SessionStrings.TempConfigMstDetails);
                else
                    Session[ERP.Utilities.SessionStrings.TempConfigMstDetails] = value;
            }
        }

        private List<FIN_INVOICE_VND_HDR> FinInvoiceVndHdrSelectedList
        {
            get
            {
                return Session[ERP.Utilities.SessionStrings.FinInvoiceVndHdrSelectedList] == null ? new List<FIN_INVOICE_VND_HDR>()
                    : (List<FIN_INVOICE_VND_HDR>)Session[ERP.Utilities.SessionStrings.FinInvoiceVndHdrSelectedList];
            }
            set
            {
                if (value == null)
                    Session.Remove(ERP.Utilities.SessionStrings.FinInvoiceVndHdrSelectedList);
                else
                    Session[ERP.Utilities.SessionStrings.FinInvoiceVndHdrSelectedList] = value;
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
        /// PaymentFlag
        /// </summary>
        private int PaymentFlag
        {
            get
            {
                return Convert.ToInt32(this.Session[ERP.Utilities.SessionStrings.PaymentFlag]);
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.PaymentFlag] = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private decimal AdjnNowAmount
        {
            get
            {
                return Convert.ToDecimal(this.ViewState[ViewstateStrings.AdjnNowAmount]);
            }
            set
            {
                this.ViewState[ViewstateStrings.AdjnNowAmount] = value;
            }
        }
        /// <summary>
        /// TrxIndex
        /// </summary>
        private int TrxIndex
        {
            get
            {
                return this.ViewState[ViewstateStrings.TrxIndex] == null ? -1 : Convert.ToInt32(this.ViewState[ViewstateStrings.TrxIndex].ToString());
            }
            set
            {
                this.ViewState[ViewstateStrings.TrxIndex] = value;
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
        /// To maintain the Paynow in viewstate
        /// </summary>
        private decimal PayNow
        {
            get
            {
                return ViewState[ERP.Utilities.ViewstateStrings.PayNow] == null ? 0 : (decimal)ViewState[ERP.Utilities.ViewstateStrings.PayNow];
            }
            set
            {
                this.ViewState[ViewstateStrings.PayNow] = value;
            }
        }
        /// <summary>
        /// To maintain the Adjustment amount in viewstate
        /// </summary>
        private decimal AdjustmentAmount
        {
            get
            {
                return ViewState[ERP.Utilities.ViewstateStrings.AdjustmentAmount] == null ? 0 : (decimal)ViewState[ERP.Utilities.ViewstateStrings.AdjustmentAmount];
            }
            set
            {
                this.ViewState[ViewstateStrings.AdjustmentAmount] = value;
            }
        }
        private List<ADM_DOC_ATTACH> DocAttachList
        {
            get
            {
                return ViewState[ViewstateStrings.DocAttachList] == null ? null : (List<ADM_DOC_ATTACH>)ViewState[ViewstateStrings.DocAttachList];
            }
            set
            {
                ViewState[ViewstateStrings.DocAttachList] = value;
            }
        }
        private List<BusinessObject.POInvoicing.FileDetails> FileDetailsList
        {
            get
            {
                return Session[ERP.Utilities.SessionStrings.FilePODetailsList] == null ? null : (List<BusinessObject.POInvoicing.FileDetails>)Session[ERP.Utilities.SessionStrings.FilePODetailsList];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.FilePODetailsList] = value;
            }
        }
        /// <summary>
        /// To maintain keep selected pos
        /// </summary>
        private List<FIN_PAYMENT_VND_ALCN_DTL> PaymentAdjnList
        {

            get
            {
                return this.ViewState[ViewstateStrings.PaymentAdjnList] != null ? (List<FIN_PAYMENT_VND_ALCN_DTL>)this.ViewState[ViewstateStrings.PaymentAdjnList] : new List<FIN_PAYMENT_VND_ALCN_DTL>();
            }
            set
            {
                if (value == null)
                    ViewState.Remove(ViewstateStrings.PaymentAdjnList);
                else
                    this.ViewState[ViewstateStrings.PaymentAdjnList] = value;
            }

        }
        /// <summary>
        /// To keep Invoice pk list for split applied invoices
        /// </summary>
        private List<long> AppliedInvPkList
        {
            get
            {
                return ViewState[ViewstateStrings.AppliedInvPkList] == null ? null : (List<long>)ViewState[ViewstateStrings.AppliedInvPkList];
            }
            set
            {
                this.ViewState[ViewstateStrings.AppliedInvPkList] = value;
            }
        }

        /// <summary>
        /// Payment Mode details list
        /// </summary>      
        private List<FIN_PAYMENT_VND_MODE_DTL> PaymentModeDetailsList
        {
            get
            {
                return Session[ERP.Utilities.SessionStrings.PaymentModeDetailsList] == null ? new List<FIN_PAYMENT_VND_MODE_DTL>()
                    : (List<FIN_PAYMENT_VND_MODE_DTL>)Session[ERP.Utilities.SessionStrings.PaymentModeDetailsList];
            }
            set
            {
                if (value == null)
                    Session.Remove(ERP.Utilities.SessionStrings.PaymentModeDetailsList);
                else
                    Session[ERP.Utilities.SessionStrings.PaymentModeDetailsList] = value;
            }
        }

        /// <summary>
        /// Payment Modes(from config table)
        /// </summary>      
        private List<ADM_CONFIG_MST> PaymentModeConfigMstList
        {
            get
            {
                return Session[ERP.Utilities.SessionStrings.PaymentModeConfigMstList] == null ? new List<ADM_CONFIG_MST>()
                    : (List<ADM_CONFIG_MST>)Session[ERP.Utilities.SessionStrings.PaymentModeConfigMstList];
            }
            set
            {
                if (value == null)
                    Session.Remove(ERP.Utilities.SessionStrings.PaymentModeConfigMstList);
                else
                    Session[ERP.Utilities.SessionStrings.PaymentModeConfigMstList] = value;
            }
        }


        /// <summary>
        /// To keep row index
        /// </summary>
        private int PaymentModeRowIndex
        {
            get
            {
                return this.ViewState[ViewstateStrings.PaymentModeRowIndex] != null ? Convert.ToInt32(this.ViewState[ViewstateStrings.PaymentModeRowIndex]) : -1;
            }
            set
            {
                this.ViewState[ViewstateStrings.PaymentModeRowIndex] = value;
            }
        }

        /// <summary>
        /// To keep row index
        /// </summary>
        private int CrdrRowIndex
        {
            get
            {
                return this.ViewState[ViewstateStrings.RowIndex] != null ? Convert.ToInt32(this.ViewState[ViewstateStrings.RowIndex]) : -1;
            }
            set
            {
                this.ViewState[ViewstateStrings.RowIndex] = value;
            }
        }
        /// <summary>
        /// Currency Pk
        /// </summary>
        private int CurrencyPk
        {
            get
            {
                return this.ViewState[ViewstateStrings.CurrencyPk] == null ? 0 : Convert.ToInt32(this.ViewState[ViewstateStrings.CurrencyPk].ToString());
            }
            set
            {
                this.ViewState[ViewstateStrings.CurrencyPk] = value;
            }
        }
        /// <summary>
        /// Is payment mode added
        /// </summary>
        private bool IsPaymentModeAdded
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsPaymentModeAdded] == null ? false : (bool)this.ViewState[ViewstateStrings.IsPaymentModeAdded];
            }
            set
            {
                this.ViewState[ViewstateStrings.IsPaymentModeAdded] = value;
            }
        }

        private List<PaymentCrdrMpg> PaymentCrdrList
        {

            get
            {
                return this.ViewState[ViewstateStrings.PaymentCrdrList] != null ? (List<PaymentCrdrMpg>)this.ViewState[ViewstateStrings.PaymentCrdrList] : new List<PaymentCrdrMpg>();
            }
            set
            {
                if (value == null)
                    ViewState.Remove(ViewstateStrings.PaymentCrdrList);
                else
                    this.ViewState[ViewstateStrings.PaymentCrdrList] = value;
            }

        }
        /// <summary>
        /// Is credit note added
        /// </summary>
        private bool IsCreditNoteApplied
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsCreditNoteApplied] == null ? false : (bool)this.ViewState[ViewstateStrings.IsCreditNoteApplied];
            }
            set
            {
                this.ViewState[ViewstateStrings.IsCreditNoteApplied] = value;
            }
        }

        /// <summary>
        /// To maintain payment trx list in view state
        /// </summary>
        private List<FIN_PAYMENT_VND_TRX_MPG> finPaymentTrxMpgList
        {

            get
            {
                return this.Session[ERP.Utilities.SessionStrings.finPaymentTrxMpgList] != null ? (List<FIN_PAYMENT_VND_TRX_MPG>)this.Session[ERP.Utilities.SessionStrings.finPaymentTrxMpgList] : new List<FIN_PAYMENT_VND_TRX_MPG>();
            }
            set
            {
                if (value == null)
                    Session.Remove(ERP.Utilities.SessionStrings.finPaymentTrxMpgList);
                else
                    this.Session[ERP.Utilities.SessionStrings.finPaymentTrxMpgList] = value;
            }

        }

        /// <summary>
        /// To maintain payment invoice list in view state
        /// </summary>
        private List<FIN_INVOICE_VND_HDR> finInvoiceHdrList
        {

            get
            {
                return this.Session[ERP.Utilities.SessionStrings.finInvoiceHdrList] != null ? (List<FIN_INVOICE_VND_HDR>)this.Session[ERP.Utilities.SessionStrings.finInvoiceHdrList] : new List<FIN_INVOICE_VND_HDR>();
            }
            set
            {
                if (value == null)
                    Session.Remove(ERP.Utilities.SessionStrings.finInvoiceHdrList);
                else
                    this.Session[ERP.Utilities.SessionStrings.finInvoiceHdrList] = value;
            }

        }

        /// <summary>
        /// To maintain invoice changes in view state
        /// </summary>
        private List<PaymentInvoiceDetails> PaymentInvDetList
        {

            get
            {
                return this.ViewState[ViewstateStrings.PaymentInvDetList] != null ? (List<PaymentInvoiceDetails>)this.ViewState[ViewstateStrings.PaymentInvDetList] : new List<PaymentInvoiceDetails>();
            }
            set
            {
                if (value == null)
                    ViewState.Remove(ViewstateStrings.PaymentInvDetList);
                else
                    this.ViewState[ViewstateStrings.PaymentInvDetList] = value;
            }

        }

        /// <summary>
        /// To show or hide adj column
        /// </summary>
        private bool ShowAdjColumn
        {
            get
            {
                return this.ViewState[ViewstateStrings.ShowAdjColumn] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.ShowAdjColumn]);
            }
            set
            {
                this.ViewState[ViewstateStrings.ShowAdjColumn] = value;
            }
        }
        /// <summary>
        /// To show or hide credit and debit allocation columns for expense invoice
        /// </summary>
        private bool ShowExpenseCrDr
        {
            get
            {
                return this.ViewState[ViewstateStrings.ShowExpenseCrDr] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.ShowExpenseCrDr]);
            }
            set
            {
                this.ViewState[ViewstateStrings.ShowExpenseCrDr] = value;
            }
        }
        #endregion
        // Indicates the state as well as action
        private ActionsEnum commonActions;
        private DataTable dtDiscountTypes;
        private DataTable VendorDetails;
        User currentUser;
        //page related Entity Object
        private ServiceUtility serviceUtilityObj;
        private FIN_PAYMENT_VND_HDR finPaymentVndHdrObj;
        private FIN_PAYMENT_VND_TRX_MPG finPaymentVndTrxMpgObj;
        private FIN_PAYMENT_VND_TAX_HDR finPaymentVndTaxHdrObj;
        private FIN_INVOICE_VND_HDR finInvoiceVndHdrObj;
        private FIN_COA_MST finCoaMstObj;
        private FIN_CASH_BANK_MST finCashBankMstObj;
        private FIN_PAYMENT_VND_PO_MPG finPaymentVndPoMpgObj;
        private FIN_INVOICE_VND_TRX_MPG FinInvoiceVndTrxMpgObj;
        private FIN_INVOICE_VND_HDR finInvoiceVndHdrObjForPaymentSplit;
        //private ERPData.POInvoice poInvoiceObj;

        private ADM_COMPANY_MST admCompanyMstObj;
        private List<ADM_COMPANY_MST> admCompanyMstList;

        //List for binding details to controls  
        private List<FIN_PAYMENT_VND_HDR> finPaymentVndHdrList;
        private List<FIN_PAYMENT_VND_TRX_MPG> finPaymentVndTrxMpgList;
        private List<FIN_PAYMENT_VND_TAX_HDR> finPaymentVndTaxHdrList;
        private List<FIN_PAYMENT_VND_TRX_MPG> FinPayVndTrxMpgList;
        private List<FIN_INVOICE_VND_HDR> finInvoiceVndHdrList;
        private List<FIN_COA_MST> finCoaMstList;
        private List<FIN_CASH_BANK_MST> finCashBankMstList;

        private List<FIN_PAYMENT_VND_PO_MPG> finPaymentVndPoMpgList;
        private List<FIN_INVOICE_VND_TRX_MPG> FinInvoiceVndTrxMpgList;
        private List<FIN_INVOICE_VND_TRX_MPG> FinInvoiceVndTrxMpgListForAutoAlcn;
        private List<FIN_INVOICE_VND_HDR> finInvoiceVndHdrListForPaymentSplit;
        private List<FIN_PAYMENT_VND_TAX_HDR> finPayemtVndHdrList;
        private FIN_PAYMENT_VND_TAX_HDR finVatPaymentDetails;

        private FIN_PAYMENT_VND_TAX_DTL finPaymentVndTaxDtlObj;
        private List<FIN_PAYMENT_VND_TAX_DTL> finPaymentVndTaxDtlList;

        private List<FIN_PAYMENT_VND_TAX_HDR> tempVATTaxDetails;
        private FIN_PAYMENT_VND_TAX_HDR tempVATTax;
        private FIN_PAYMENT_VND_TAX_HDR finPymntObj;
        private List<FIN_PAYMENT_VND_TAX_HDR> finPymntList;
        private List<PaymentAdjnAllocation> CrDrAdjnList = null;
        private List<FIN_PAYMENT_VND_ALCN_DTL> FinPaymentVndAllocationList;
        private List<FIN_PAYMENT_VND_ALCN_DTL> FinPaymentVndAdjnDupCheckList;
        private FIN_PAYMENT_VND_ALCN_DTL FinPaymentVndAllocationObj;
        private List<FIN_INVOICE_VND_ADV_DED_DTL> finAdvDeductList;

        private List<PaymentCrdrMpg> FinPaymentVndCrdrMpgList;
        private List<PaymentCrdrMpg> FinPaymentVndCrdrAllocationList;
        private FIN_PAYMENT_VND_CRDR_MPG FinPaymentVndCrdrAllocationObj;
        private List<FIN_CRDR_NOTE_MPG> FinCrdrMpgList;
        private FIN_INVOICE_VND_HDR FinInvoiceVndObj;
        List<PaymentCrdrMpg> PaymentCrdrListForNewInv;
        //private List<long> selectedInvoiceList;
        private List<long> NewInvoiceList = new List<long>();

        private bool isSplitChanged = false;
        private bool IsCreditExist = false;
        private long InPk;

        private bool updatePayment;
        int JournalPK;
        int invGroup;
        int ValidateId = 0;
        string PKXml = "";

        private string refID;
        private string inboxFlag;

        private FIN_TRX_HDR finTrxHdrObj;
        private List<FIN_TRX_HDR> finTrxHdrList;

        private ADM_CONFIG_MST admConfigMstObj;
        private List<ADM_CONFIG_MST> admConfigMstList;
        private List<SPADM_APP_STATUS_CFG_GET_KV_Result> workflowStatusList;

        private List<PUR_VENDOR_MST> purVendorMstList;

        private DataTable dtVendorAccount;
        private DataTable dtPageData;
        private DataTable dtTaxDetails;
        private DataTable dtVendorDtl;
        private DataTable dtAdsTypeDtl;
        private DataTable dtAdsType;
        private DataTable dtVendorBanks;
        private DataTable dtTaxMst;
        private DataTable dtCompany;
        private DataSet dsAdsType;
        private DataSet dsAdsTypeDtl;
        private DataTable dtPaymentTypes;
        private DataTable dtInvoiceList;
        private DataSet dsInvoiceList;

        private int whtTaxpk;
        private int VatBuyTaxpk;
        int vendorPk = 0;
        private int VendorBankPk = 0;
        private int TaxPk = 0;
        int InvoiceType = 1;
        private long InvPkSplit = 0;
        private int? PoPkSplit = null;
        private long NewInvPk;

        private decimal VatBuyTax = 0;
        private decimal withHoldTax = 0;
        private decimal AmountTotal = 0;
        private decimal TaxTotal = 0;
        private decimal TaxWhtTotal = 0;
        private decimal VatBuyTaxAmntTotal = 0;
        private decimal totAllocateAdjn = 0;
        private decimal totBalanceAdjn = 0;
        private decimal totalbalamtsplit = 0;
        private decimal PaymentTollerence = 1;


        private ADM_CURRENCY_MST admCurrencyMstObj;
        private List<ADM_CURRENCY_MST> admCurrencyMstList;
        private List<ADM_CURRENCY_MST> CurrencyMstList;
        private List<ADM_CONST_MST> admConstMstList;
        private ADM_APP_CONFIG_MST admAppConfigMstObj;
        private List<ADM_APP_CONFIG_MST> admAppConstMstList;
        DataSet dsAlertList;
        private int invPK;
        private int VatVendorPopupPk = 0;
        private int VncPk = 0;

        private byte selectedModeValue = 0;
        private string TypeRef;
        private string appType;
        List<FIN_PAYMENT_VND_PO_MPG> tempFinPaymentVndPoMpgList;

        private List<SPADM_APP_SUB_TYPE_DATA_GET_Result> AppTypeDetailsList;
        private CommonService cm;

        private string[] _operators = { "-", "+", "/", "*", "^" };
        private Func<double, double, double>[] _operations = {
            (a1, a2) => a1 - a2,
            (a1, a2) => a1 + a2,
            (a1, a2) => a1 / a2,
            (a1, a2) => a1 * a2,
            (a1, a2) => Math.Pow(a1, a2)
        };

        ADM_DOC_ATTACH admDocAttachObj;

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
            try
            {
                if (Session[ERP.Utilities.SessionStrings.TransactionType] == null)
                {
                    ucrWrkf.WrkfSubmit += new EventHandler(ActionHandler);
                    hdfJournalizeWorkFlow.Value = "0";
                }
                ucrWrkf.ViewType = 1;

                ucrJournalize.JournalizeSave += new EventHandler(ActionHandler);
                ucrJournalize.JournalizeSubmit += new EventHandler(ActionHandler);
                ucrJournalize.JournalizeDelete += new EventHandler(ActionHandler);
                ucrJournalize.JournalizeCancel += new EventHandler(ActionHandler);

                ucrJournalize.ReverseSave += new EventHandler(ActionHandler);
                ucrJournalize.ReverseSubmit += new EventHandler(ActionHandler);
                ucrJournalize.ReverseDelete += new EventHandler(ActionHandler);
                ucrJournalize.ReverseCancel += new EventHandler(ActionHandler);

                ucrJournalize.ReturnSave += new EventHandler(ActionHandler);
                ucrJournalize.ReturnSubmit += new EventHandler(ActionHandler);
                ucrJournalize.ReturnDelete += new EventHandler(ActionHandler);
                ucrJournalize.ReturnCancel += new EventHandler(ActionHandler);

                divErrorLabelAdjn.Visible = false;
                divErrorAdj.Visible = false;
                divBaltoAll.Visible = false;
                ConfigureWHTCertNoControl();

                if (!IsPostBack)
                {
                    ConfigurationSettings();
                    GetFieldValues(ControlsEnum.COMPANY);
                    SetFieldValues(ControlsEnum.COMPANY);

                    GetFieldValues(ControlsEnum.FORMNO);
                    SetFieldValues(ControlsEnum.FORMNO);

                    GetFieldValues(ControlsEnum.BANKCURRENCY);
                    SetFieldValues(ControlsEnum.BANKCURRENCY);

                    string[] itemkeyarray;
                    itemkeyarray = new string[1];
                    itemkeyarray[0] = "DOC_SEQ_NO";
                    grdUploads.DataKeyNames = itemkeyarray;

                    FileDetailsList = null;
                    DocAttachList = null;

                    InvoicePOSplitList = null;
                    WHTTaxDetails = null;
                    TempWHTTaxDetails = null;
                    TempConfigMstDetails = null;
                    VATTaxDetails = null;
                    TempVATTaxDetails = null;
                    PaymentModeDetailsList = null;
                    PaymentModeConfigMstList = null;
                    PaymentCrdrList = null;
                    GetFieldValues(ControlsEnum.DISCOUNTTYPE);
                    SetFieldValues(ControlsEnum.DISCOUNTTYPE);
                    //GetFieldValues(ControlsEnum.VENDORACCOUNT);
                    //SetFieldValues(ControlsEnum.VENDORACCOUNT);
                    //GetFieldValues(ControlsEnum.VENDORCONTACTYPE);
                    //SetFieldValues(ControlsEnum.VENDORCONTACTYPE);
                    GetFieldValues(ControlsEnum.VENDORTYPES);

                    hdfCurrencyFormat.Value = "#0.";
                    for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits; i++)
                    {
                        hdfCurrencyFormat.Value += "0";
                    }
                    hdfDecimalDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits.ToString();
                    hdfCurrencyGroup2.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes[0].ToString();
                    hdfCurrencyGroup1.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes[1].ToString();

                    hdfExchangeRateFormat.Value = "#0.";
                    int exchrateDecimalDigits = (Session[ERP.Utilities.SessionStrings.ExchRateDecimalDigit] == null
                        ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits
                        : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.ExchRateDecimalDigit]));
                    for (int i = 0; i < exchrateDecimalDigits; i++)
                    {
                        hdfExchangeRateFormat.Value += "0";
                    }

                    hdfCurrencyGroup2.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes[0].ToString();
                    hdfCurrencyGroup1.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes[1].ToString();

                    Session[ERP.Utilities.SessionStrings.TransactionType] = null;

                    txtSearchDateFrom.Text = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString(Resources.Constants.DateFormatShort);
                    hdfSearchDateFrom.Value = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString();
                    txtSearchDateTo.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    hdfSearchDateTo.Value = DateTime.Now.ToString();

                    GetFieldValues(ControlsEnum.PAYMODE);
                    SetFieldValues(ControlsEnum.PAYMODE);


                    ////start
                    string pid = Request.QueryString[QueryStrings.PID] != null ? Request.QueryString[QueryStrings.PID]
                        : Session[ERP.Utilities.SessionStrings.PID] != null ? Session[ERP.Utilities.SessionStrings.PID].ToString().Split('=')[1] : string.Empty;
                    ////
                    //Used for Integration purpose
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

                    //If Request From External(Report or Other page) otherthan Menu or Inbox
                    if (Request.QueryString[QueryStrings.FromExt] != null && Request.QueryString[QueryStrings.FromExt] == "T")
                    {
                        ucrWrkf.ViewType = 0;
                        EntryStatus = EntryStatus.VIEWMODE;
                        GetFieldValues(ControlsEnum.PAYMENTGET);
                        SetFieldValues(ControlsEnum.PAYMENTGET);
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
                                //divbtnSavePaymentSplit.Visible = false;
                            }
                            else
                            {
                                ucrWrkf.ViewType = 1;
                                EntryStatus = EntryStatus.ENTRYMODE;
                            }

                            ////start
                            if (string.IsNullOrEmpty(pid) || pid.Equals("1") || pid.Equals("11"))
                            {
                                base.WkfRefID = ucrWrkf.RefID = int.Parse(refID);
                                CurrPK = GetApplicationID(ucrWrkf.RefID);
                                if (pid.Equals("11"))
                                    hdfIsCancelled.Value = "1";//For Showing Cancelled Stamp in Detail Page
                            }
                            else if (pid.Equals("2") || pid.Equals("12") || pid.Equals("14") || pid.Equals("16"))
                            {
                                ucrWrkf.RefID = int.Parse(refID);
                                JournalPK = GetApplicationID(ucrWrkf.RefID);
                                GetFieldValues(ControlsEnum.GETPAYMENTPKBYJOURNALPK);
                                if (finTrxHdrList != null && finTrxHdrList.Count == 1)
                                {
                                    CurrPK = (Int32)finTrxHdrList[0].FTH_REF_PK;
                                    WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                                    base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(Convert.ToInt32(CurrPK), ucrWrkf.ProcessID);
                                }
                            }
                            else if (pid.Equals("4"))
                            {
                                ucrWrkf.RefID = int.Parse(refID);
                                JournalPK = GetApplicationID(ucrWrkf.RefID);
                                GetFieldValues(ControlsEnum.GETPAYMENTPKBYJOURNALPK);
                                if (finTrxHdrList != null && finTrxHdrList.Count == 1)
                                {
                                    CurrPK = (Int32)finTrxHdrList[0].FTH_REF_PK;
                                    WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                                    base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(Convert.ToInt32(CurrPK), ucrWrkf.ProcessID);
                                }
                            }
                            else if (pid.Equals("6"))
                            {
                                ucrWrkf.RefID = int.Parse(refID);
                                JournalPK = GetApplicationID(ucrWrkf.RefID);
                                GetFieldValues(ControlsEnum.GETPAYMENTPKBYJOURNALPK);
                                if (finTrxHdrList != null && finTrxHdrList.Count == 1)
                                {
                                    CurrPK = (Int32)finTrxHdrList[0].FTH_REF_PK;
                                    WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                                    base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(Convert.ToInt32(CurrPK), ucrWrkf.ProcessID);
                                }
                            }

                            ////

                        }
                        else if (!string.IsNullOrEmpty(prefID))
                        {
                            base.WkfRefID = ucrWrkf.RefID = int.Parse(prefID);
                            CurrPK = GetApplicationID(ucrWrkf.RefID);
                        }
                        if (CurrPK > 0)
                        {
                            SetCancelRef((int)CurrPK);
                            ucrWrkf.FillWorkFlowDetails();
                            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                                ucrWrkf.ViewType = 1;
                            else
                            {
                                ucrWrkf.ViewType = 0;
                                //EntryStatus = EntryStatus.VIEWMODE;
                                //btnSave.Visible = false;
                                //btnSubmit.Visible = false;
                                //divbtnSavePaymentSplit.Visible = false;
                            }
                            int mode;
                            // Get PaymentDetails
                            GetFieldValues(ControlsEnum.PAYMENTHDRENTRYBYPK);
                            GetUIValuesFromObject(ControlsEnum.PAYMENTHDRENTRY);
                            GetFieldValues(ControlsEnum.PAYMENTHDRINVLISTBYPK);
                            GetFieldValues(ControlsEnum.CRDRALLOCATION);
                            SetFieldValues(ControlsEnum.PAYMENTMPGLIST);

                            ModifiedDatePnl.Visible = true;
                            mode = String.IsNullOrEmpty(ddlMode.SelectedValue) ? 0 : Convert.ToInt32(ddlMode.SelectedValue);
                            GetFieldValues(ControlsEnum.BASECURRENCY);
                            lblTotalAmountBC.Text = string.Format(lblTotalAmountBC.Text, hdfBaseCurrency.Value.Split('-')[0].Trim());
                            SetPaymentModeDetails(mode);
                            //switch (mode)
                            //{
                            //    case (int)PaymentModeEnum.CASH:
                            //        vrfBranch.Enabled = vrfBranchHdr.Enabled = false;
                            //        vrfAccountNo.Enabled = vrfAccountNoHdr.Enabled = false;
                            //        vrfInstrumentNo.Enabled = vrfInstrumentNoHdr.Enabled = false;
                            //        vrfInstrumentDate.Enabled = vrfInstrumentDateHdr.Enabled = false;
                            //        vrfFavourof.Enabled = vrfFavourofHdr.Enabled = false;
                            //        txtInstrumentNo.Enabled = false;
                            //        txtInstrumentDate.Enabled = false;
                            //        txtFavourof.Enabled = false;
                            //        txtInstrumentNo.CssClass = "Uiinput-amount input-disabled medium";
                            //        txtInstrumentDate.CssClass = "Uidate-picker input-disabled";
                            //        txtFavourof.CssClass = "multiline-2line input-disabled";
                            //        Label5.Visible = false;
                            //        txtBankCharge.Visible = false;
                            //        chkBankCharge.Visible = false;
                            //        break;
                            //    case (int)PaymentModeEnum.OTHERS:
                            //        vrfBranch.Enabled = vrfBranchHdr.Enabled = false;
                            //        vrfAccountNo.Enabled = vrfAccountNoHdr.Enabled = false;
                            //        vrfInstrumentNo.Enabled = vrfInstrumentNoHdr.Enabled = false;
                            //        vrfInstrumentDate.Enabled = vrfInstrumentDateHdr.Enabled = false;
                            //        vrfFavourof.Enabled = vrfFavourof.Enabled = false;
                            //        txtInstrumentNo.Enabled = false;
                            //        txtInstrumentDate.Enabled = false;
                            //        txtFavourof.Enabled = false;
                            //        txtInstrumentNo.CssClass = "Uiinput-amount input-disabled medium";
                            //        txtInstrumentDate.CssClass = "Uidate-picker input-disabled";
                            //        txtFavourof.CssClass = "multiline-2line input-disabled";
                            //        Label5.Visible = false;
                            //        txtBankCharge.Visible = false;
                            //        chkBankCharge.Visible = false;
                            //        txtPaymentBank.Text = string.Empty;
                            //        txtPaymentBank.Enabled = false;
                            //        txtPaymentBank.CssClass = "input-disabled";
                            //        hdfPaymentBank.Value = string.Empty;
                            //        break;
                            //    default:
                            //        vrfBranch.Enabled = vrfBranchHdr.Enabled = true;
                            //        vrfAccountNo.Enabled = vrfAccountNoHdr.Enabled = true;
                            //        vrfInstrumentNo.Enabled = vrfInstrumentNoHdr.Enabled = true;
                            //        vrfInstrumentDate.Enabled = vrfInstrumentDateHdr.Enabled = true;
                            //        vrfFavourof.Enabled = vrfFavourofHdr.Enabled = true;
                            //        txtInstrumentNo.Enabled = true;
                            //        txtInstrumentDate.Enabled = true;
                            //        txtFavourof.Enabled = true;
                            //        txtInstrumentNo.CssClass = "Uiinput-amount medium";
                            //        txtInstrumentDate.CssClass = "Uidate-picker";
                            //        txtFavourof.CssClass = "multiline-2line";
                            //        Label5.Visible = true;
                            //        txtBankCharge.Visible = true;
                            //        chkBankCharge.Visible = true;
                            //        break;
                            //}
                        }
                        else
                        {

                            //Sets data key for the gird
                            string[] datakeyarray;
                            datakeyarray = new string[1];
                            datakeyarray[0] = Resources.DataFieldRes.POPaymentPK;
                            grdPOPaymentHdr.DataKeyNames = datakeyarray;
                            if (SelectedInvoices != null)
                            {
                                SetCancelRef((int)CurrPK);
                                ucrWrkf.FillWorkFlowDetails();
                                if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                                {
                                    ucrWrkf.ViewType = 1;
                                    EntryStatus = EntryStatus.NEWMODE;
                                }
                                else
                                {
                                    ucrWrkf.ViewType = 0;
                                    //EntryStatus = EntryStatus.VIEWMODE;
                                    //btnSave.Visible = false;
                                    //btnSubmit.Visible = false;
                                    // divbtnSavePaymentSplit.Visible = false;
                                }
                                btnPrint.Visible = false;
                                selectedInvoiceList = SelectedInvoices;
                                SelectedInvoices = null;
                                GetFieldValues(ControlsEnum.PAYMENTMPGLIST);
                                if (finInvoiceHdrList != null && finInvoiceHdrList.Count > 0)
                                {
                                    POGroup = (POInvoiceGroup)Enum.Parse(typeof(POInvoiceGroup), finInvoiceHdrList.First().IVH_GROUP.ToString());
                                    PICategory = (POInvoiceCategory)Enum.Parse(typeof(POInvoiceCategory), finInvoiceHdrList.First().IVH_CATEGORY.ToString());
                                }
                                GetFieldValues(ControlsEnum.CRDRALLOCATION);
                                SetFieldValues(ControlsEnum.PAYMENTMPGLIST);
                                txtHdrExchangeRate.Text = txtHdrExchangeRate.ToolTip = hdfHdrExchangeRate.Value;

                                GetFieldValues(ControlsEnum.EXCHANGERATEINBASECURRENCY);
                                decimal exchangeRate = !string.IsNullOrEmpty(hdfExchangeCurrBC.Value) ? Convert.ToDecimal(hdfExchangeCurrBC.Value) : 0;
                                //txtExchangeRate.Text = String.Format("{0:c}", exchangeRate);
                                if (exchangeRate > 0)
                                {
                                    //txtExchangeRate.Text = String.Format("{0:c4}", exchangeRate);
                                    ////txtExchangeRate.Text = exchangeRate.ToString();
                                }
                                //decimal paidAmount = !string.IsNullOrEmpty(txtPaidAmount.Text.Trim()) ? Convert.ToDecimal(txtPaidAmount.Text.Trim()) : 0;
                                decimal paidAmount = !string.IsNullOrEmpty(txtPaymentAmount.Text.Trim()) ? Convert.ToDecimal(txtPaymentAmount.Text.Trim()) : 0;
                                txtTotalAmountBC.Text = GetFormattedCurrency(exchangeRate * paidAmount);
                                GetFieldValues(ControlsEnum.BASECURRENCY);
                                lblTotalAmountBC.Text = string.Format(lblTotalAmountBC.Text, hdfBaseCurrency.Value.Split('-')[0].Trim());

                                //btnPrint.Enabled = false;
                                lblPaymentNo.Text = "[NEW]";
                                hdfPaymentNo.Value = "[NEW]";
                                txtPaymentDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                                int mode = String.IsNullOrEmpty(ddlMode.SelectedValue) ? 0 : Convert.ToInt32(ddlMode.SelectedValue);
                                SetPaymentModeDetails(mode);
                                //switch (mode)
                                //{
                                //    case (int)PaymentModeEnum.CASH:
                                //        vrfBranch.Enabled = vrfBranchHdr.Enabled = false;
                                //        vrfAccountNo.Enabled = vrfAccountNoHdr.Enabled = false;
                                //        vrfInstrumentNo.Enabled = vrfInstrumentNoHdr.Enabled = false;
                                //        vrfInstrumentDate.Enabled = vrfInstrumentDateHdr.Enabled = false;
                                //        vrfFavourof.Enabled = vrfFavourofHdr.Enabled = false;
                                //        txtInstrumentNo.Enabled = false;
                                //        txtInstrumentDate.Enabled = false;
                                //        txtFavourof.Enabled = false;
                                //        txtInstrumentNo.CssClass = "Uiinput-amount input-disabled medium";
                                //        txtInstrumentDate.CssClass = "Uidate-picker input-disabled";
                                //        txtFavourof.CssClass = "multiline-2line input-disabled";
                                //        Label5.Visible = false;
                                //        txtBankCharge.Visible = false;
                                //        chkBankCharge.Visible = false;
                                //        break;
                                //    case (int)PaymentModeEnum.OTHERS:
                                //        vrfBranch.Enabled = vrfBranchHdr.Enabled = false;
                                //        vrfAccountNo.Enabled = vrfAccountNoHdr.Enabled = false;
                                //        vrfInstrumentNo.Enabled = vrfInstrumentNoHdr.Enabled = false;
                                //        vrfInstrumentDate.Enabled = vrfInstrumentDateHdr.Enabled = false;
                                //        vrfFavourof.Enabled = vrfFavourofHdr.Enabled = false;
                                //        txtInstrumentNo.Enabled = false;
                                //        txtInstrumentDate.Enabled = false;
                                //        txtFavourof.Enabled = false;
                                //        txtInstrumentNo.CssClass = "Uiinput-amount input-disabled medium";
                                //        txtInstrumentDate.CssClass = "Uidate-picker input-disabled";
                                //        txtFavourof.CssClass = "multiline-2line input-disabled";
                                //        Label5.Visible = false;
                                //        txtBankCharge.Visible = false;
                                //        chkBankCharge.Visible = false;
                                //        txtPaymentBank.Text = string.Empty;
                                //        txtPaymentBank.Enabled = false;
                                //        txtPaymentBank.CssClass = "input-disabled";
                                //        hdfPaymentBank.Value = string.Empty;
                                //        break;
                                //    default:
                                //        vrfBranch.Enabled = vrfBranchHdr.Enabled = true;
                                //        vrfAccountNo.Enabled = vrfAccountNoHdr.Enabled = true;
                                //        vrfInstrumentNo.Enabled = vrfInstrumentNoHdr.Enabled = true;
                                //        vrfInstrumentDate.Enabled =vrfInstrumentDateHdr.Enabled= true;
                                //        vrfFavourof.Enabled = vrfFavourofHdr.Enabled = true;
                                //        txtInstrumentNo.Enabled = true;
                                //        txtInstrumentDate.Enabled = true;
                                //        txtFavourof.Enabled = true;
                                //        txtInstrumentNo.CssClass = "Uiinput-amount medium";
                                //        txtInstrumentDate.CssClass = "Uidate-picker";
                                //        txtFavourof.CssClass = "multiline-2line";
                                //        Label5.Visible = true;
                                //        txtBankCharge.Visible = true;
                                //        chkBankCharge.Visible = true;
                                //        break;
                                //}
                            }
                            else
                            {
                                selectedInvoiceList = new List<long>();
                                GetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                                SetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                                EntryStatus = EntryStatus.LISTMODE;
                                PageIndex = "1";
                                uclPaging.TotalPages = TotalPages;
                                uclPaging.CurrentPage = 1;
                            }
                        }
                        //txtHdrExchangeRate.Text = txtHdrExchangeRate.ToolTip = hdfHdrExchangeRate.Value;
                        EnableDisableExchangeRate();
                        GetFieldValues(ControlsEnum.VENDORBANKS);
                        SetFieldValues(ControlsEnum.VENDORBANKS);
                        Session[ERP.Utilities.SessionStrings.SelectedPos] = null;
                        ////if (InvoiceType == Convert.ToInt32(PurchaseType.Local))//If Invoicetype is Domestic then exchangerate is noneditable                   
                        ////{
                        ////    txtExchangeRate.Enabled = false;
                        ////    txtExchangeRate.CssClass = "medium numeric input-disabled";
                        ////}
                        ////else
                        ////{
                        ////    txtExchangeRate.Enabled = true;
                        ////    txtExchangeRate.CssClass = "numeric medium";

                        ////}

                        if (!string.IsNullOrEmpty(ddlBankChargeCurrency.SelectedValue))//If Invoicetype is Domestic then exchangerate is noneditable                   
                        {
                            GetFieldValues(ControlsEnum.BANKCURRENCYEXCHANGERATE);
                        }
                        //GetFieldValues(ControlsEnum.UPLOADEDFILES);
                        //SetFieldValues(ControlsEnum.UPLOADEDFILES);
                        //GetFieldValues(ControlsEnum.CRDRALLOCATION);
                        hdfAppSubType.Value = string.Empty;
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
        #region Get Field Values
        /// <summary>
        /// Gets the data to bind/assign for the controls(inputs, grids, dropdowns etc.)
        /// pass string.empty to get all field values
        /// Assign it to the page level variables
        /// </summary>
        private void GetFieldValues(ControlsEnum type)
        {
            POPaymentService poPaymentServiceClient;
            poPaymentServiceClient = null;
            BankMstService bankMstServiceClient;
            bankMstServiceClient = null;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            DateTime paymentDate;
            FinTrxService finTrxServiceClient;
            finTrxServiceClient = null;
            int? Status = null;
            int? PDCStatus = null;
            CommonService CommonServiceClient;
            CommonServiceClient = null;
            AdmCompanyMstService admCompanyMstServiceClient;
            FinTrxService FinTrxServiceClient = null;

            try
            {
                FinTrxServiceClient = new FinTrxService();
                FinTrxServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(FinTrxServiceClient);
                switch (type)
                {

                    #region PAYMENTGET
                    case ControlsEnum.PAYMENTGET:
                        int GInvPk = Convert.ToInt32(Request.QueryString[QueryStrings.PK]);
                        poPaymentServiceClient = new POPaymentService();
                        poPaymentServiceClient = CommonFunctions.InitiateClient(poPaymentServiceClient);
                        finPaymentVndHdrObj = CommonFunctions.Initilize<FIN_PAYMENT_VND_HDR>();
                        serviceUtilityObj = new ServiceUtility();
                        //serviceUtilityObj.CurrentPage = PageIndex == null ? 1 : Convert.ToInt32(PageIndex);
                        //serviceUtilityObj.PageSize = grdPOPaymentHdr.PageSize;
                        //serviceUtilityObj.SortBy = SortBy = SortBy == null ? Resources.DataFieldRes.POPaymentDate : SortBy;
                        //serviceUtilityObj.ThenBy = ThenBy = ThenBy == null ? Resources.DataFieldRes.POPaymentNo : ThenBy;
                        //serviceUtilityObj.SortDirection = SortDirection = SortDirection == null ? Resources.Report.SortDescending : SortDirection;
                        //serviceUtilityObj.FilterBy = string.Empty;
                        //serviceUtilityObj.FilterValue = string.Empty;
                        //finPaymentVndHdrObj.PVH_VENDOR = string.IsNullOrEmpty(hdfVendorID.Value) ? 0 : Convert.ToInt32(hdfVendorID.Value);
                        //if (hdfVendorID.Value != "" && hdfVendorID.Value != "0")
                        //{
                        //    Session[ERP.Utilities.SessionStrings.VendorPK] = hdfVendorID.Value;
                        //    Session[ERP.Utilities.SessionStrings.Vendor] = txtVendor.Text;
                        //}
                        finPaymentVndHdrObj.PVH_PK = GInvPk;
                        finPaymentVndHdrObj.PVH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        //serviceUtilityObj.FilterDate = string.IsNullOrEmpty(txtSearchDateFrom.Text.Trim()) ? DateTime.MinValue : Convert.ToDateTime(txtSearchDateFrom.Text.Trim());
                        //serviceUtilityObj.FilterToDate = string.IsNullOrEmpty(txtSearchDateTo.Text.Trim()) ? DateTime.MinValue : Convert.ToDateTime(txtSearchDateTo.Text.Trim());
                        finPaymentVndHdrObj.PVH_BIZUNIT = currentUser.SBUID;
                        finPaymentVndHdrObj.PVH_CRTD_BY = currentUser.PKUser;
                        //PDCStatus = Convert.ToInt32(ddlPDCStatus.SelectedValue);

                        finPaymentVndHdrList = poPaymentServiceClient.GetPaymentHdr(finPaymentVndHdrObj, serviceUtilityObj, 3, "", 0);

                        TotalPages = serviceUtilityObj.TotalRecords == 0 ? 1 : (serviceUtilityObj.TotalRecords <= serviceUtilityObj.PageSize) ? 1 :
              (serviceUtilityObj.TotalRecords % serviceUtilityObj.PageSize) == 0 ? (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) :
              (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) + 1;

                        break;
                    #endregion
                    #region Payment Hdr List
                    case ControlsEnum.PAYMENTHDRLIST:
                        poPaymentServiceClient = new POPaymentService();
                        poPaymentServiceClient = CommonFunctions.InitiateClient(poPaymentServiceClient);
                        finPaymentVndHdrObj = CommonFunctions.Initilize<FIN_PAYMENT_VND_HDR>();
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = PageIndex == null ? 1 : Convert.ToInt32(PageIndex);
                        serviceUtilityObj.PageSize = grdPOPaymentHdr.PageSize;
                        serviceUtilityObj.SortBy = SortBy = SortBy == null ? Resources.DataFieldRes.POPaymentDate : SortBy;
                        serviceUtilityObj.ThenBy = ThenBy = ThenBy == null ? Resources.DataFieldRes.POPaymentNo : ThenBy;
                        serviceUtilityObj.SortDirection = SortDirection = SortDirection == null ? Resources.Report.SortDescending : SortDirection;
                        serviceUtilityObj.FilterBy = string.Empty;
                        serviceUtilityObj.FilterValue = string.Empty;
                        finPaymentVndHdrObj.PVH_VENDOR = string.IsNullOrEmpty(hdfVendorID.Value) ? 0 : Convert.ToInt32(hdfVendorID.Value);
                        if (hdfVendorID.Value != "" && hdfVendorID.Value != "0")
                        {
                            Session[ERP.Utilities.SessionStrings.VendorPK] = hdfVendorID.Value;
                            Session[ERP.Utilities.SessionStrings.Vendor] = txtVendor.Text;
                        }
                        finPaymentVndHdrObj.PVH_PK = string.IsNullOrEmpty(hdfPaymentPK.Value) ? 0 : Convert.ToInt64(hdfPaymentPK.Value);
                        finPaymentVndHdrObj.PVH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        serviceUtilityObj.FilterDate = string.IsNullOrEmpty(txtSearchDateFrom.Text.Trim()) ? DateTime.MinValue : Convert.ToDateTime(txtSearchDateFrom.Text.Trim());
                        serviceUtilityObj.FilterToDate = string.IsNullOrEmpty(txtSearchDateTo.Text.Trim()) ? DateTime.MinValue : Convert.ToDateTime(txtSearchDateTo.Text.Trim());
                        finPaymentVndHdrObj.PVH_BIZUNIT = currentUser.SBUID;
                        finPaymentVndHdrObj.PVH_CRTD_BY = currentUser.PKUser;
                        Status = Convert.ToInt32(ddlStatus.SelectedValue);
                        PDCStatus = Convert.ToInt32(ddlPDCStatus.SelectedValue);
                        string invNo = null;
                        if (txtSINo.Text != null)
                        { invNo = txtSINo.Text.Trim(); }
                        else { invNo = ""; }
                        finPaymentVndHdrList = poPaymentServiceClient.GetPaymentHdr(finPaymentVndHdrObj, serviceUtilityObj, Status, invNo, PDCStatus);
                        //serviceUtilityObj.TotalRecords = finPaymentVndHdrList.Count > 0 ? Convert.ToInt32(finPaymentVndHdrList.Count) : 0;
                        //TotalPages = (serviceUtilityObj.TotalRecords == 0) ? 1 : (serviceUtilityObj.TotalRecords <= serviceUtilityObj.PageSize) ? 1 : (serviceUtilityObj.TotalRecords % serviceUtilityObj.PageSize) == 0 ? (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) : (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) + 1;

                        TotalPages = serviceUtilityObj.TotalRecords == 0 ? 1 : (serviceUtilityObj.TotalRecords <= serviceUtilityObj.PageSize) ? 1 :
              (serviceUtilityObj.TotalRecords % serviceUtilityObj.PageSize) == 0 ? (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) :
              (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) + 1;

                        break;
                    #endregion
                    #region Invoice Hdr List
                    case ControlsEnum.PAYMENTMPGLIST:
                        poPaymentServiceClient = new POPaymentService();
                        poPaymentServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(poPaymentServiceClient);
                        finInvoiceHdrList = poPaymentServiceClient.GetPaymentTrxMpg(selectedInvoiceList);
                        if (finInvoiceHdrList.Count > 0)
                        {
                            IsWorkOrder = finInvoiceHdrList[0].IVH_IS_WORK_ORDER == 1 ? true : false;
                            if ((finInvoiceHdrList.First().IVH_GROUP == null ? 1 : Convert.ToInt16(finInvoiceHdrList.First().IVH_GROUP)) == (int)POInvoiceGroup.AgtInvoice)
                            {
                                InvoiceType = finInvoiceHdrList.FirstOrDefault().IVH_TYPE == 1 ? 2 : 1;
                            }
                            List<FIN_INVOICE_VND_TRX_MPG> livoiceCusTrxMpgList = finInvoiceHdrList[0].FIN_INVOICE_VND_TRX_MPG.ToList();
                            if (livoiceCusTrxMpgList.Count > 0)
                                if (livoiceCusTrxMpgList[0].PUR_ORDER_HDR != null)
                                    InvoiceType = livoiceCusTrxMpgList[0].PUR_ORDER_HDR.POH_TYPE;
                        }
                        //EditedInvoices = finInvoiceHdrList;
                        FinInvoiceVndHdrSelectedList = finInvoiceHdrList;
                        // finInvoiceHdrList = finInvoiceHdrList;
                        break;
                    #endregion
                    #region Cash Bank List
                    case ControlsEnum.BANK:
                        bankMstServiceClient = new BankMstService();
                        bankMstServiceClient = CommonFunctions.InitiateClient(bankMstServiceClient);
                        short bankPk = string.IsNullOrEmpty(hdfPaymentBank.Value) ? Convert.ToInt16(0) : Convert.ToInt16(hdfPaymentBank.Value);
                        finCashBankMstList = bankMstServiceClient.GetFinBankMstByPK(bankPk);
                        break;
                    #endregion
                    #region Payment Details List By PK
                    case ControlsEnum.PAYMENTHDRINVLISTBYPK:
                        poPaymentServiceClient = new POPaymentService();
                        poPaymentServiceClient = CommonFunctions.InitiateClient(poPaymentServiceClient);
                        finPaymentVndTrxMpgList = poPaymentServiceClient.GetPaymentTrxMpg(CurrPK);
                        EditedPaymentDtls = finPaymentVndTrxMpgList;
                        FinInvoiceVndHdrSelectedList = new List<FIN_INVOICE_VND_HDR>();
                        finPaymentVndTrxMpgList.ForEach(dtl => FinInvoiceVndHdrSelectedList.Add(dtl.FIN_INVOICE_VND_HDR));
                        finPaymentTrxMpgList = finPaymentVndTrxMpgList;
                        break;
                    #endregion
                    #region Payment Hdr List By PK
                    case ControlsEnum.PAYMENTHDRENTRYBYPK:
                        poPaymentServiceClient = new POPaymentService();
                        poPaymentServiceClient = CommonFunctions.InitiateClient(poPaymentServiceClient);
                        finPaymentVndHdrList = poPaymentServiceClient.GetPaymentHdr(CurrPK);
                        break;
                    #endregion
                    #region Generate Payment No
                    case ControlsEnum.PAYMENTNO:
                        //Generate Payment No
                        poPaymentServiceClient = new POPaymentService();
                        poPaymentServiceClient = CommonFunctions.InitiateClient(poPaymentServiceClient);
                        hdfPaymentNo.Value = poPaymentServiceClient.GetPaymentNo(POGroup == POInvoiceGroup.Goods ? ApplicationType.VP
                            : POGroup == POInvoiceGroup.Services ? ApplicationType.SIP : POGroup == POInvoiceGroup.AgtInvoice ? ApplicationType.AIP : ApplicationType.EIP, 0, currentUser.CurrentDeptPK,
                            Convert.ToDateTime(txtPaymentDate.Text.Trim()), currentUser.PKUser, updatePayment, 0, Convert.ToInt32(ddlCompany.SelectedValue));
                        break;
                    #endregion
                    #region Generate Exchange Rate
                    case ControlsEnum.EXCHANGERATE:
                        //Generate Exchange Rate
                        poPaymentServiceClient = new POPaymentService();
                        poPaymentServiceClient = CommonFunctions.InitiateClient(poPaymentServiceClient);
                        int toCurrency = string.IsNullOrEmpty(hdfPaymentCurrency.Value) ? currentUser.BaseCurrency : Convert.ToInt32(hdfPaymentCurrency.Value);
                        paymentDate = string.IsNullOrEmpty(txtPaymentDate.Text.Trim()) ? DateTime.Now : Convert.ToDateTime(txtPaymentDate.Text.Trim());
                        hdfExchangeCurr.Value = poPaymentServiceClient.GetConversionFactor(
                                                             Convert.ToInt32(hdfInvoiceCurr.Value), toCurrency,
                                                             paymentDate, currentUser.SBUID).ToString();
                        hdfHdrExchangeRate.Value = poPaymentServiceClient.GetConversionFactor(toCurrency,
                                                             currentUser.BaseCurrency,
                                                             paymentDate, currentUser.SBUID).ToString();
                        //txtExchangeRate.Text =
                        //EnableDisableExchangeRate();
                        //if (toCurrency == currentUser.BaseCurrency)
                        //{
                        //    txtHdrExchangeRate.Enabled = false;
                        //    txtHdrExchangeRate.CssClass = "Uiinput-amount numeric input-disabled";
                        //    ////txtExchangeRate.Enabled = false;
                        //    ////txtExchangeRate.CssClass = "medium numeric input-disabled";
                        //}
                        //else
                        //{
                        //    txtHdrExchangeRate.Enabled = true;
                        //    txtHdrExchangeRate.CssClass = "Uiinput-amount numeric";
                        //    ////txtExchangeRate.Enabled = true;
                        //    ////txtExchangeRate.CssClass = "numeric medium";
                        //}
                        break;
                    #endregion
                    #region Generate Exchange Rate in base Currency
                    case ControlsEnum.EXCHANGERATEINBASECURRENCY:
                        //Generate Exchange Rate in base Currency
                        poPaymentServiceClient = new POPaymentService();
                        poPaymentServiceClient = CommonFunctions.InitiateClient(poPaymentServiceClient);
                        int fromCurrency = string.IsNullOrEmpty(hdfPaymentCurrency.Value) ? currentUser.BaseCurrency : Convert.ToInt32(hdfPaymentCurrency.Value);
                        paymentDate = string.IsNullOrEmpty(txtPaymentDate.Text.Trim()) ? DateTime.Now : Convert.ToDateTime(txtPaymentDate.Text.Trim());
                        hdfExchangeCurrBC.Value = poPaymentServiceClient.GetConversionFactor(
                                                             fromCurrency, currentUser.BaseCurrency,
                                                             paymentDate, currentUser.SBUID).ToString();
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
                    #region Base Currency
                    case ControlsEnum.BASECURRENCY:
                        CurrencyMstService CurrencyMstServiceClient = new CurrencyMstService();
                        string BaseCurrency = CurrencyMstServiceClient.GetCurrencyCodeName(currentUser.BaseCurrency);
                        hdfBaseCurrency.Value = BaseCurrency;
                        break;
                    #endregion
                    #region PAYMENTSPLITLIST
                    case ControlsEnum.PAYMENTSPLITLIST:
                        poPaymentServiceClient = new POPaymentService();
                        poPaymentServiceClient = CommonFunctions.InitiateClient(poPaymentServiceClient);
                        finPaymentVndPoMpgObj = CommonFunctions.Initilize<FIN_PAYMENT_VND_PO_MPG>();
                        finPaymentVndPoMpgObj.PPO_PAYMENT_TRX_MPG = PaymentMpgPK;
                        finPaymentVndPoMpgList = poPaymentServiceClient.GetFinPatmentVndPoMpgList(finPaymentVndPoMpgObj);
                        break;
                    #endregion
                    #region INVOICEVNDMPGLIST
                    case ControlsEnum.INVOICEVNDMPGLIST:
                        poPaymentServiceClient = new POPaymentService();
                        poPaymentServiceClient = CommonFunctions.InitiateClient(poPaymentServiceClient);
                        FinInvoiceVndTrxMpgObj = CommonFunctions.Initilize<FIN_INVOICE_VND_TRX_MPG>();
                        FinInvoiceVndTrxMpgObj.IVM_INVOICE_HDR = InvoicePK;
                        FinInvoiceVndTrxMpgList = poPaymentServiceClient.GetInvoiceTrxMpg(FinInvoiceVndTrxMpgObj);
                        break;
                    #endregion
                    #region INVOICEVNDMPGLIST FOR AUTO ALLOCATION
                    case ControlsEnum.INVOICEVNDMPGLISTFORAUTOALCN:
                        poPaymentServiceClient = new POPaymentService();
                        poPaymentServiceClient = CommonFunctions.InitiateClient(poPaymentServiceClient);
                        FinInvoiceVndTrxMpgObj = CommonFunctions.Initilize<FIN_INVOICE_VND_TRX_MPG>();
                        FinInvoiceVndTrxMpgObj.IVM_INVOICE_HDR = InvoicePK;
                        FinInvoiceVndTrxMpgListForAutoAlcn = poPaymentServiceClient.GetInvoiceTrxMpg(FinInvoiceVndTrxMpgObj);
                        break;
                    #endregion
                    #region INVOICEVNDHDR
                    case ControlsEnum.INVOICEVNDHDR:
                        poPaymentServiceClient = new POPaymentService();
                        poPaymentServiceClient = CommonFunctions.InitiateClient(poPaymentServiceClient);
                        finInvoiceVndHdrObjForPaymentSplit = CommonFunctions.Initilize<FIN_INVOICE_VND_HDR>();
                        finInvoiceVndHdrObjForPaymentSplit.IVH_PK = InvoicePK;
                        finInvoiceVndHdrObjForPaymentSplit.IVH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        finInvoiceVndHdrListForPaymentSplit = poPaymentServiceClient.GetInvoiceHdrByPK(finInvoiceVndHdrObjForPaymentSplit);
                        break;
                    #endregion
                    #region PAYMENTSPLITLISTBYPAYMENTPK
                    case ControlsEnum.PAYMENTSPLITLISTBYPAYMENTPK:
                        poPaymentServiceClient = new POPaymentService();
                        poPaymentServiceClient = CommonFunctions.InitiateClient(poPaymentServiceClient);
                        finPaymentVndPoMpgObj = CommonFunctions.Initilize<FIN_PAYMENT_VND_PO_MPG>();
                        finPaymentVndPoMpgObj.PPO_PAYMENT_HDR = CurrPK;
                        finPaymentVndPoMpgList = poPaymentServiceClient.GetFinPatmentVndPoMpgList(finPaymentVndPoMpgObj);
                        break;
                    #endregion
                    #region Payment Mode
                    case ControlsEnum.PAYMODE:
                        CommonServiceClient = new CommonService();
                        CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);
                        admConfigMstObj = new ADM_CONFIG_MST();
                        admConfigMstObj.CFG_PK = 0;
                        admConfigMstObj.CFG_TYPE = "PAYMENT MODE";
                        admConfigMstObj.CFG_ACTIVE = Convert.ToByte(CommonConstants.ACTIVE);
                        admConfigMstList = CommonServiceClient.GetConfigValues(admConfigMstObj);
                        PaymentModeConfigMstList = admConfigMstList;
                        break;
                    #endregion
                    #region GETPAYMENTPKBYJOURNALPK
                    case ControlsEnum.GETPAYMENTPKBYJOURNALPK:
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
                        workflowStatusList = CommonServiceClient.GetWorkFlowStatus(ApplicationType.VP, null, Convert.ToByte(CommonConstants.ACTIVE));
                        break;
                    #endregion

                    #region ADJTYPE
                    case ControlsEnum.DISCOUNTTYPE:
                        dtDiscountTypes = BusinessLogic.CommonManagement.CommonBL.GetConstMstValues(0, 0, ConstGroupType.ReceiptAdjustments, (int)Adjustments.Payment, 1, currentUser.SBUID);
                        break;
                    #endregion

                    #region VENDORACCOUNT
                    case ControlsEnum.VENDORACCOUNT:
                        dtVendorAccount = CommonBL.GetTaxMstList(0, (int)TaxType.Tax, (int)TaxSubCategory.WHT, (byte)DbActiveStatus.ACTIVE, currentUser.SBUID);
                        break;
                    #endregion

                    #region VENDORACCOUNTTAXFORMULA
                    case ControlsEnum.VENDORACCOUNTTAX:
                        if (whtTaxpk > 0)
                        {
                            dtVendorAccount = CommonBL.GetTaxMstList(whtTaxpk, (int)TaxType.Tax, (int)TaxSubCategory.WHT, (byte)DbActiveStatus.HASPK, currentUser.SBUID);
                        }
                        break;
                    #endregion

                    #region VENDOR
                    case ControlsEnum.VENDOR:
                        CommonServiceClient = new CommonService();
                        CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);
                        purVendorMstList = CommonServiceClient.GetVendor(Convert.ToInt32(hdfCusPK.Value));
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
                        admAppConfigMstObj.ACF_DATA = ApplicationType.PI;
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

                    #region BANKCURRENCY
                    case ControlsEnum.BANKCURRENCY:
                        CommonServiceClient = new CommonService();
                        CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);
                        admCurrencyMstObj = new ADM_CURRENCY_MST();
                        admCurrencyMstObj.CUR_PK = currentUser.BaseCurrency;
                        admCurrencyMstObj.CUR_ACTIVE = Convert.ToByte(CommonConstants.ACTIVE);
                        admCurrencyMstList = CommonServiceClient.GetCurrency(admCurrencyMstObj);
                        break;
                    #endregion

                    #region INVOICEVNDMPGLIST
                    case ControlsEnum.INVVNDMPGLIST:
                        poPaymentServiceClient = new POPaymentService();
                        poPaymentServiceClient = CommonFunctions.InitiateClient(poPaymentServiceClient);
                        FinPayVndTrxMpgList = poPaymentServiceClient.GetInvoicePaymentTrxMpg(InPk);
                        break;
                    #endregion

                    #region Generate Exchange Rate for Bank Charge
                    case ControlsEnum.EXCHANGERATEBANK:
                        //Generate Exchange Rate in base Currency
                        poPaymentServiceClient = new POPaymentService();
                        poPaymentServiceClient = CommonFunctions.InitiateClient(poPaymentServiceClient);
                        int fromCurrencyBank = string.IsNullOrEmpty(hdfPaymentCurrency.Value) ? currentUser.BaseCurrency : Convert.ToInt32(hdfPaymentCurrency.Value);
                        paymentDate = string.IsNullOrEmpty(txtPaymentDate.Text.Trim()) ? DateTime.Now : Convert.ToDateTime(txtPaymentDate.Text.Trim());
                        hdfExchRate.Value = poPaymentServiceClient.GetConversionFactor(
                                                             fromCurrencyBank, currentUser.BaseCurrency,
                                                             paymentDate, currentUser.SBUID).ToString();
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
                        break;
                    #endregion

                    #region FORMNO
                    case ControlsEnum.FORMNO:
                        CommonServiceClient = new CommonService();
                        CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);
                        admConstMstList = CommonServiceClient.GetConstMstValues(null, 1, null, Convert.ToInt16(ConstGroupType.WHTFormNo), Convert.ToInt16(ConstGroup.WHTFormnoval), currentUser.SBUID);
                        break;
                    #endregion

                    #region Get Status
                    case ControlsEnum.FINHEADERSTATUS:
                        finTrxServiceClient = new FinTrxService();
                        finTrxServiceClient = CommonFunctions.InitiateClient(finTrxServiceClient);
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.PageSize = 10;
                        finTrxHdrObj = ERP.Utilities.CommonFunctions.Initilize<FIN_TRX_HDR>();
                        finTrxHdrObj.FTH_REF_TYPE = (invGroup == 1 || invGroup == 6) ? finTrxHdrObj.FTH_REF_TYPE = ApplicationType.VPJ : invGroup == 2 ? finTrxHdrObj.FTH_REF_TYPE = ApplicationType.SIPJ :
                            invGroup == 4 ? finTrxHdrObj.FTH_REF_TYPE = ApplicationType.AIPJ : finTrxHdrObj.FTH_REF_TYPE = ApplicationType.EIPJ;
                        finTrxHdrObj.FTH_REF_PK = CurrPK;
                        finTrxHdrObj.FTH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        finTrxHdrList = finTrxServiceClient.GetSatusByAppPK(finTrxHdrObj);
                        break;
                    #endregion


                    #region VENDORTYPES
                    case ControlsEnum.VENDORTYPES:
                        byte[] Values = new byte[] { (int)VendorContactTypeEnum.Branch, (int)VendorContactTypeEnum.HeadOffice };
                        CommonServiceClient = new CommonService();
                        admConfigMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CONFIG_MST>();
                        admConfigMstObj.CFG_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        admConfigMstObj.CFG_TYPE = GetLocalResourceObject("VendorContactType").ToString();
                        if (selectedModeValue > 0)
                            admConfigMstObj.CFG_VALUE = selectedModeValue;
                        TempConfigMstDetails = CommonServiceClient.GetConfigValues(admConfigMstObj).Where(vcl => Values.Contains(vcl.CFG_VALUE)).ToList();
                        break;
                    #endregion
                    #region VENDORACCOUNTVATBUYTAX
                    case ControlsEnum.VENDORACCOUNTVATBUYTAX:
                        if (VatBuyTaxpk > 0)
                        {
                            dtVendorAccount = CommonBL.GetTaxMstList(VatBuyTaxpk, (int)TaxType.Tax, (int)TaxSubCategory.VATBuy, (byte)DbActiveStatus.HASPK, currentUser.SBUID);
                        }
                        break;
                    #endregion

                    #region VATBUYTAXTYPES
                    case ControlsEnum.VATBUYTAXTYPES:
                        if (GetGlobalResourceObject("ConfigurationsRes", "IsTaxNotDueForMaterialPO").ToString() == "1")//If 0 exclude Tax not due from tax poup ddl.If 1 include
                        {
                            dtTaxDetails = BusinessLogic.Administration.Masters.TaxSettingsMaster.GetActiveTaxCategoryDateValue((int)TaxType.Tax, currentUser.SBUID, Convert.ToByte(DbActiveStatus.ACTIVE), Convert.ToDateTime(txtPaymentDate.Text), 0, TaxFilterType.PUR, (int)TaxStatus.Include, (int)DbActiveStatus.ACTIVE);
                        }
                        else
                        {
                            dtTaxDetails = BusinessLogic.Administration.Masters.TaxSettingsMaster.GetActiveTaxCategoryDateValue((int)TaxType.Tax, currentUser.SBUID, Convert.ToByte(DbActiveStatus.ACTIVE), Convert.ToDateTime(txtPaymentDate.Text), 0, TaxFilterType.PUR, (int)TaxStatus.Exclude, (int)DbActiveStatus.ACTIVE);
                        }
                        break;
                    #endregion

                    #region PAYMENTTAXHDR
                    case ControlsEnum.PAYMENTTAXHDR:
                        finPymntObj = ERP.Utilities.CommonFunctions.Initilize<FIN_PAYMENT_VND_TAX_HDR>();
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = -1;
                        serviceUtilityObj.PageSize = -1;
                        if (!string.IsNullOrEmpty(txtVatTaxInvDate.Text.Trim()))
                            finPymntObj.WTH_TAX_DATE = DateTime.Parse(txtVatTaxInvDate.Text.Trim());
                        finPymntObj.WTH_TAX_INV_NO = txtVatTaxInvNo.Text;
                        finPymntList = FinTrxServiceClient.GetfinPymntVndTxtHdrList(finPymntObj, serviceUtilityObj);
                        break;
                    #endregion
                    #region VENDORSELECTEDDTL
                    case ControlsEnum.VENDORSELECTEDDTL:
                        //int vendorPopupPk = 0;
                        //int.TryParse(hdfVendorPopup.Value, out vendorPopupPk);
                        //dtPageData = BusinessLogic.VendorManagement.VendorRegistration.GetVendorData(vendorPopupPk);
                        if (string.IsNullOrEmpty(hdfVendorPopup.Value))
                        {
                            dtVendorDtl = BusinessLogic.VendorManagement.VendorMaster.GetVendor(currentUser, 0, Convert.ToInt16(DbActiveStatus.ACTIVE), txtVendorPopup.Text);
                        }
                        break;
                    #endregion
                    #region VENDORINVDTL
                    case ControlsEnum.VENDORINVDTL:
                        VendorDetails = new DataTable();
                        VendorDetails = CommonBL.GetVendorContactList(0, 1, vendorPk, currentUser.SBUID);
                        break;
                    #endregion
                    #region VATPOPUPGRID
                    case ControlsEnum.VATPOPUPGRID:

                        bool blnIsVatbuyExist = false;
                        try
                        {
                            GetFieldValues(ControlsEnum.PAYMENTHDRENTRYBYPK);
                            if (finPaymentVndHdrList.Where(whtpynt => whtpynt.FIN_PAYMENT_VND_TAX_HDR.Where(objTax => objTax.WTH_CATEGORY == (byte)WHTCategoryEnum.VATBUY).Count() > 0).Count() > 0)
                            {
                                blnIsVatbuyExist = true;
                            }
                        }
                        catch
                        {
                            blnIsVatbuyExist = false;
                        }
                        //if (TempVATTaxDetails == null || TempVATTaxDetails.Count == 0 || !blnIsVatbuyExist)
                        //{
                        //TempVATTaxDetails = null;
                        tempVATTaxDetails = null;
                        tempVATTaxDetails = TempVATTaxDetails;

                        List<FIN_PAYMENT_VND_TAX_HDR> objTempVATTaxDetails = new List<FIN_PAYMENT_VND_TAX_HDR>();
                        List<FIN_PAYMENT_VND_TAX_HDR> objtempVATTaxDetails = new List<FIN_PAYMENT_VND_TAX_HDR>();

                        finPaymentVndTrxMpgList = new List<FIN_PAYMENT_VND_TRX_MPG>();
                        finPaymentVndTrxMpgList = (List<FIN_PAYMENT_VND_TRX_MPG>)SetUIValuesToObject(ControlsEnum.PAYMENTMPGENTRY);

                        //finPaymentVndHdrObj = CommonFunctions.Initilize<FIN_PAYMENT_VND_HDR>();
                        //finPaymentVndHdrObj = (FIN_PAYMENT_VND_HDR)SetUIValuesToObject(ControlsEnum.PAYMENTHDRENTRY);  

                        if (FinInvoiceVndHdrSelectedList != null)
                        {

                            foreach (FIN_INVOICE_VND_HDR finInvVndHdr in FinInvoiceVndHdrSelectedList)
                            {
                                VatBuyTaxAmntTotal = 0;
                                decimal balToPay = 0;
                                decimal taxAmount = 0;
                                string itemName = "";
                                int? VendorContactPk = null;
                                tempVATTax = new FIN_PAYMENT_VND_TAX_HDR();
                                tempVATTax = CommonFunctions.Initilize<FIN_PAYMENT_VND_TAX_HDR>();
                                List<FIN_INVOICE_VND_TAX_HDR> finInvVndTaxList = finInvVndHdr.FIN_INVOICE_VND_TAX_HDR.ToList();
                                List<FIN_INVOICE_VND_DTL> finInvVndDtlList = finInvVndHdr.FIN_INVOICE_VND_DTL.ToList();

                                List<FIN_PAYMENT_VND_TRX_MPG> finPymntTrxMpg = finPaymentVndTrxMpgList.Where(fpvtm => fpvtm.PVM_INVOICE_HDR == finInvVndHdr.IVH_PK).ToList();

                                if (finInvVndTaxList != null && finInvVndTaxList.Count > 0)
                                {
                                    //tempVATTax.WTH_TAX = finInvVndTaxList[0].VTH_TAX;
                                    //tempVATTax.WTH_NAME = finInvVndTaxList[0].VTH_NAME;
                                    //tempVATTax.WTH_TAX_CATEGORY = finInvVndTaxList[0].VTH_TAX_CATEGORY;
                                    try
                                    {
                                        tempVATTax.WTH_TAX = finInvVndTaxList.SingleOrDefault(objTax => (objTax.VTH_TYPE == (byte)WhtTypeEnum.DEFINEDTAX) && (objTax.VTH_TAX == (byte)TaxTypes.VATBuy || objTax.VTH_TAX == (byte)TaxTypes.VATBuyNotYetDue)).VTH_TAX;
                                        tempVATTax.WTH_NAME = finInvVndTaxList.SingleOrDefault(objTax => (objTax.VTH_TYPE == (byte)WhtTypeEnum.DEFINEDTAX) && (objTax.VTH_TAX == (byte)TaxTypes.VATBuy || objTax.VTH_TAX == (byte)TaxTypes.VATBuyNotYetDue)).VTH_NAME;
                                        tempVATTax.WTH_TAX_CATEGORY = finInvVndTaxList.SingleOrDefault(objTax => (objTax.VTH_TYPE == (byte)WhtTypeEnum.DEFINEDTAX) && (objTax.VTH_TAX == (byte)TaxTypes.VATBuy || objTax.VTH_TAX == (byte)TaxTypes.VATBuyNotYetDue)).VTH_TAX_CATEGORY;
                                    }
                                    catch
                                    {
                                        tempVATTax.WTH_TAX = finInvVndTaxList[0].VTH_TAX;
                                        tempVATTax.WTH_NAME = finInvVndTaxList[0].VTH_NAME;
                                        tempVATTax.WTH_TAX_CATEGORY = finInvVndTaxList[0].VTH_TAX_CATEGORY;
                                    }
                                }


                                poPaymentServiceClient = new POPaymentService();
                                poPaymentServiceClient = CommonFunctions.InitiateClient(poPaymentServiceClient);
                                finInvoiceVndHdrObjForPaymentSplit = CommonFunctions.Initilize<FIN_INVOICE_VND_HDR>();
                                finInvoiceVndHdrObjForPaymentSplit.IVH_PK = finInvVndHdr.IVH_PK;
                                finInvoiceVndHdrObjForPaymentSplit.IVH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                                finInvoiceVndHdrListForPaymentSplit = poPaymentServiceClient.GetInvoiceHdrByPK(finInvoiceVndHdrObjForPaymentSplit);
                                if (finInvoiceVndHdrListForPaymentSplit != null && finInvoiceVndHdrListForPaymentSplit.Count > 0)
                                {
                                    if (finInvoiceVndHdrListForPaymentSplit[0].FIN_INVOICE_VND_DTL != null && finInvoiceVndHdrListForPaymentSplit[0].FIN_INVOICE_VND_DTL.Count > 0)
                                    {
                                        foreach (FIN_INVOICE_VND_DTL finInvDtl in finInvoiceVndHdrListForPaymentSplit[0].FIN_INVOICE_VND_DTL)
                                        {
                                            if (finInvDtl.FIN_INVOICE_VND_TAX_DTL != null && finInvDtl.FIN_INVOICE_VND_TAX_DTL.Count > 0)
                                            {
                                                VatBuyTaxAmntTotal += finInvDtl.FIN_INVOICE_VND_TAX_DTL.Where(tx => tx.VTL_TAX == (int)(byte)TaxTypes.VATBuyNotYetDue).Sum(tax => tax.VTL_TAX_AMT);
                                            }
                                        }
                                    }
                                    if (finInvoiceVndHdrListForPaymentSplit[0].FIN_INVOICE_VND_TAX_HDR != null && finInvoiceVndHdrListForPaymentSplit[0].FIN_INVOICE_VND_TAX_HDR.Count > 0)
                                    {
                                        VatBuyTaxAmntTotal += finInvoiceVndHdrListForPaymentSplit[0].FIN_INVOICE_VND_TAX_HDR.Where(tx => tx.VTH_TAX == (int)(byte)TaxTypes.VATBuyNotYetDue).Sum(tax => tax.VTH_TAX_AMT);
                                    }
                                    if (finInvVndHdr.IVH_CATEGORY == (int)POInvoiceCategory.Advanced)
                                    {
                                        VatBuyTaxAmntTotal += finInvoiceVndHdrListForPaymentSplit[0].FIN_INVOICE_VND_TRX_MPG.Sum(tx => tx.IVM_TAX_AMOUNT);
                                    }
                                    //try
                                    //{
                                    //    VatBuyTaxAmntTotal += finInvoiceVndHdrListForPaymentSplit[0].FIN_INVOICE_VND_TAX_HDR.Where(tx => tx.VTH_TAX == (int)(byte)TaxTypes.VATBuyNotYetDue).Sum(tax => tax.VTH_TAX_AMT);
                                    //}
                                    //catch { }
                                }


                                if (finPymntTrxMpg != null && finPymntTrxMpg.Count > 0)
                                {
                                    tempVATTax.WTH_AMOUNT = finPymntTrxMpg[0].PVM_PAID_AMOUNT - finPymntTrxMpg[0].PVM_OTHER_AMOUNT - finPymntTrxMpg[0].PVM_TAX_AMOUNT;
                                    //tempVATTax.WTH_TAX_AMT = finPymntTrxMpg[0].PVM_TAX_AMOUNT;
                                    tempVATTax.WTH_TAX_AMT = VatBuyTaxAmntTotal;
                                    //try
                                    //{
                                    //    tempVATTax.WTH_TAX_AMT = finPymntTrxMpg[0].FIN_INVOICE_VND_HDR.FIN_INVOICE_VND_TAX_HDR.Where(tx => tx.VTH_TAX == (int)(byte)TaxTypes.VATBuyNotYetDue).Sum(tax => tax.VTH_TAX_AMT);
                                    //}
                                    //catch 
                                    //{
                                    //    tempVATTax.WTH_TAX_AMT = 0;
                                    //}
                                }

                                //balToPay = finInvVndHdr.IVH_AMOUNT_NET_TC -
                                //            finInvVndHdr.IVH_AMOUNT_PAID_TC +
                                //            finInvVndHdr.IVH_AMOUNT_CN_TC -
                                //            finInvVndHdr.IVH_AMOUNT_DN_TC;

                                //decimal.TryParse(txtTaxAmount.Text, out taxAmount);

                                tempVATTax.WTH_PK = 0;
                                tempVATTax.WTH_PAYMENT_HDR = CurrPK;
                                tempVATTax.WTH_TYPE = (byte)WhtTypeEnum.DEFINEDTAX;
                                tempVATTax.WTH_CATEGORY = (byte)WHTCategoryEnum.VATBUY;

                                tempVATTax.WTH_PUR_INVOICE = finInvVndHdr.IVH_PK;
                                tempVATTax.WTH_TAX_INV_NO = finInvVndHdr.IVH_VENDOR_INV_NO;
                                tempVATTax.WTH_INV_RECEIVED = finInvVndHdr.IVH_ORGINAL_RCVD;

                                tempVATTax.WTH_BRANCH_TEXT = txtBranchCode.Text;
                                vendorPk = finInvVndHdr.IVH_VENDOR;
                                GetFieldValues(ControlsEnum.VENDORINVDTL);
                                if (VendorDetails != null && VendorDetails.Rows.Count > 0)
                                {
                                    tempVATTax.WTH_BRANCH_TYPE = Convert.ToByte(VendorDetails.Rows[0][Resources.DataFieldRes.vncType]);
                                }
                                tempVATTax.WTH_TAX_DATE = finInvVndHdr.IVH_DATE;
                                tempVATTax.WTH_REFUND_DATE = finInvVndHdr.IVH_DATE;
                                tempVATTax.WTH_PARTY_NAME = finInvVndHdr.PUR_VENDOR_MST.VEN_NAME;
                                //tempVATTax.WTH_TAX_ID = finInvVndHdr.PUR_VENDOR_MST.VEN_TIN;
                                tempVATTax.WTH_TAX_ID = finInvVndHdr.IVH_TAX_ID;
                                tempVATTax.WTH_VENDOR = finInvVndHdr.IVH_VENDOR;
                                hdfVendorPopup.Value = finInvVndHdr.IVH_VENDOR.ToString();
                                GetFieldValues(ControlsEnum.VENDORCONTACTYPE);
                                if (dtAdsType != null && dtAdsType.Rows.Count > 0)
                                {
                                    if (!string.IsNullOrEmpty(dtAdsType.Rows[0][Resources.DataFieldRes.VncPk].ToString()))
                                        VendorContactPk = Convert.ToInt32(dtAdsType.Rows[0][Resources.DataFieldRes.VncPk]);
                                    tempVATTax.WTH_BRANCH = VendorContactPk;
                                    tempVATTax.WTH_BRANCH_NAME = dtAdsType.Rows[0][Resources.DataFieldRes.VncName].ToString();
                                    tempVATTax.WTH_BRANCH_TEXT = dtAdsType.Rows[0][Resources.DataFieldRes.VncTypeName].ToString();
                                }

                                try
                                {
                                    if (finInvVndHdr.IVH_GROUP == (int)POInvoiceGroup.Expense)
                                    {
                                        if (finInvVndDtlList != null && finInvVndDtlList.Count > 0)
                                        {
                                            foreach (FIN_INVOICE_VND_DTL finInvExpDtl in finInvVndDtlList)
                                            {
                                                FIN_PAYMENT_VND_TAX_HDR tempVATTaxExp = new FIN_PAYMENT_VND_TAX_HDR();
                                                tempVATTaxExp = CommonFunctions.Initilize<FIN_PAYMENT_VND_TAX_HDR>();

                                                itemName = finInvExpDtl.VID_INSTRUCTIONS;
                                                tempVATTaxExp.WTH_TAX_ID = finInvExpDtl.VID_TAX_ID;
                                                tempVATTaxExp.WTH_BRANCH_TEXT = finInvExpDtl.VID_BRANCH_TEXT;
                                                tempVATTaxExp.WTH_BRANCH_TYPE = finInvExpDtl.VID_BRANCH_TYPE;
                                                tempVATTaxExp.WTH_BRANCH_NAME = finInvExpDtl.VID_BRANCH_NAME;
                                                tempVATTaxExp.WTH_BRANCH = finInvExpDtl.VID_BRANCH;
                                                tempVATTaxExp.WTH_TAX_INV_NO = finInvExpDtl.VID_REF_NO;
                                                tempVATTaxExp.WTH_INV_RECEIVED = finInvExpDtl.FIN_INVOICE_VND_HDR.IVH_ORGINAL_RCVD;
                                                //tempVATTaxExp.WTH_TAX_CATEGORY = finInvVndDtlList[0].VTH_TAX_CATEGORY;
                                                tempVATTaxExp.WTH_PARTY_NAME = finInvExpDtl.VID_VENDOR_TEXT;
                                                tempVATTaxExp.WTH_VENDOR = finInvExpDtl.VID_VENDOR;
                                                tempVATTaxExp.WTH_AMOUNT = finInvExpDtl.VID_AMOUNT;
                                                //tempVATTaxExp.WTH_TAX_AMT = finInvExpDtl.VID_TAX;
                                                tempVATTaxExp.WTH_TAX_AMT = 0;
                                                if (finInvoiceVndHdrListForPaymentSplit != null && finInvoiceVndHdrListForPaymentSplit.Count > 0)
                                                {
                                                    FIN_INVOICE_VND_DTL finInvDtl = finInvoiceVndHdrListForPaymentSplit[0].FIN_INVOICE_VND_DTL.SingleOrDefault(inv => inv.VID_PK == finInvExpDtl.VID_PK);
                                                    decimal totalHeaderTax = 0, SubTotalAmount = 0, LineItemAmount = 0, HeaderTaxAdding = 0;
                                                    if (finInvDtl != null)
                                                    {
                                                        if (finInvDtl.FIN_INVOICE_VND_TAX_DTL != null && finInvDtl.FIN_INVOICE_VND_TAX_DTL.Count > 0)
                                                        {
                                                            tempVATTaxExp.WTH_TAX_AMT = finInvDtl.FIN_INVOICE_VND_TAX_DTL.Where(tx => tx.VTL_TAX == (int)(byte)TaxTypes.VATBuyNotYetDue).Sum(tax => tax.VTL_TAX_AMT);
                                                        }
                                                        LineItemAmount = finInvDtl.VID_AMOUNT;
                                                    }

                                                    //if (finInvoiceVndHdrListForPaymentSplit[0].FIN_INVOICE_VND_TAX_HDR != null && finInvoiceVndHdrListForPaymentSplit[0].FIN_INVOICE_VND_TAX_HDR.Count > 0)
                                                    //{

                                                    //    tempVATTaxExp.WTH_TAX_AMT += finInvoiceVndHdrListForPaymentSplit[0].FIN_INVOICE_VND_TAX_HDR.Where(tx => tx.VTH_TAX == (int)(byte)TaxTypes.VATBuyNotYetDue).Sum(tax => tax.VTH_TAX_AMT);
                                                    //}                                                                                                           
                                                    if (finInvoiceVndHdrListForPaymentSplit[0].FIN_INVOICE_VND_TAX_HDR != null && finInvoiceVndHdrListForPaymentSplit[0].FIN_INVOICE_VND_TAX_HDR.Count > 0)
                                                    {
                                                        totalHeaderTax = finInvoiceVndHdrListForPaymentSplit[0].FIN_INVOICE_VND_TAX_HDR.Where(tx => tx.VTH_TAX == (int)(byte)TaxTypes.VATBuyNotYetDue).Sum(tax => tax.VTH_TAX_AMT);
                                                        SubTotalAmount = finInvoiceVndHdrListForPaymentSplit[0].IVH_AMOUNT_TC;
                                                        HeaderTaxAdding = (totalHeaderTax / SubTotalAmount) * LineItemAmount;
                                                        tempVATTaxExp.WTH_TAX_AMT += HeaderTaxAdding;
                                                    }

                                                }

                                                tempVATTaxExp.WTH_ITEM_TEXT = itemName;
                                                tempVATTaxExp.WTH_PK = 0;
                                                tempVATTaxExp.WTH_PAYMENT_HDR = CurrPK;
                                                tempVATTaxExp.WTH_TYPE = (byte)WhtTypeEnum.DEFINEDTAX;
                                                tempVATTaxExp.WTH_CATEGORY = (byte)WHTCategoryEnum.VATBUY;
                                                tempVATTaxExp.WTH_PUR_INVOICE = finInvVndHdr.IVH_PK;
                                                tempVATTaxExp.WTH_TAX_DATE = finInvExpDtl.VID_REF_DATE;
                                                tempVATTaxExp.WTH_REFUND_DATE = finInvExpDtl.VID_DATE;
                                                try
                                                {
                                                    if (finInvExpDtl.FIN_INVOICE_VND_TAX_DTL != null && finInvExpDtl.FIN_INVOICE_VND_TAX_DTL.Count > 0)
                                                    {
                                                        tempVATTaxExp.WTH_TAX = finInvExpDtl.FIN_INVOICE_VND_TAX_DTL.SingleOrDefault(objTax => (objTax.VTL_TYPE == (byte)WhtTypeEnum.DEFINEDTAX) && (objTax.VTL_TAX == (byte)TaxTypes.VATBuy || objTax.VTL_TAX == (byte)TaxTypes.VATBuyNotYetDue)).VTL_TAX;
                                                        tempVATTaxExp.WTH_NAME = finInvExpDtl.FIN_INVOICE_VND_TAX_DTL.SingleOrDefault(objTax => (objTax.VTL_TYPE == (byte)WhtTypeEnum.DEFINEDTAX) && (objTax.VTL_TAX == (byte)TaxTypes.VATBuy || objTax.VTL_TAX == (byte)TaxTypes.VATBuyNotYetDue)).VTL_NAME;
                                                        tempVATTaxExp.WTH_TAX_CATEGORY = finInvExpDtl.FIN_INVOICE_VND_TAX_DTL.SingleOrDefault(objTax => (objTax.VTL_TYPE == (byte)WhtTypeEnum.DEFINEDTAX) && (objTax.VTL_TAX == (byte)TaxTypes.VATBuy || objTax.VTL_TAX == (byte)TaxTypes.VATBuyNotYetDue)).VTL_TAX_CATEGORY;
                                                    }
                                                    //tempVATTaxDetails.Add(tempVATTaxExp);
                                                    //TempVATTaxDetails = tempVATTaxDetails;
                                                    if (!tempVATTaxExp.WTH_TAX.HasValue)
                                                    {
                                                        tempVATTaxExp.WTH_TAX = finInvExpDtl.FIN_INVOICE_VND_HDR.FIN_INVOICE_VND_TAX_HDR.SingleOrDefault(objTax => (objTax.VTH_TYPE == (byte)WhtTypeEnum.DEFINEDTAX) && (objTax.VTH_TAX == (byte)TaxTypes.VATBuy || objTax.VTH_TAX == (byte)TaxTypes.VATBuyNotYetDue)).VTH_TAX;
                                                        tempVATTaxExp.WTH_NAME = finInvExpDtl.FIN_INVOICE_VND_HDR.FIN_INVOICE_VND_TAX_HDR.SingleOrDefault(objTax => (objTax.VTH_TYPE == (byte)WhtTypeEnum.DEFINEDTAX) && (objTax.VTH_TAX == (byte)TaxTypes.VATBuy || objTax.VTH_TAX == (byte)TaxTypes.VATBuyNotYetDue)).VTH_NAME;
                                                        tempVATTaxExp.WTH_TAX_CATEGORY = finInvExpDtl.FIN_INVOICE_VND_HDR.FIN_INVOICE_VND_TAX_HDR.SingleOrDefault(objTax => (objTax.VTH_TYPE == (byte)WhtTypeEnum.DEFINEDTAX) && (objTax.VTH_TAX == (byte)TaxTypes.VATBuy || objTax.VTH_TAX == (byte)TaxTypes.VATBuyNotYetDue)).VTH_TAX_CATEGORY;

                                                    }

                                                    if (tempVATTaxExp.WTH_TAX.HasValue)
                                                    {
                                                        TaxPk = Convert.ToInt32(tempVATTaxExp.WTH_TAX);
                                                        GetFieldValues(ControlsEnum.TAXDETAILS);
                                                        if (dtTaxMst != null && dtTaxMst.Rows.Count > 0 && Convert.ToInt32(dtTaxMst.Rows[0][Resources.DataFieldRes.TaxNotDue]) == (int)TaxEnum.TaxNotYetDue)
                                                        {
                                                            objtempVATTaxDetails.Add(tempVATTaxExp);
                                                            objTempVATTaxDetails = objtempVATTaxDetails;
                                                        }
                                                    }

                                                }
                                                catch
                                                {

                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (finInvVndHdr.IVH_CATEGORY == (int)POInvoiceCategory.Invoice)
                                        {
                                            if (finInvVndDtlList != null && finInvVndDtlList.Count > 0)
                                            {
                                                if (finInvVndDtlList[0].INV_ITEM_MST != null)
                                                {
                                                    itemName = finInvVndDtlList[0].INV_ITEM_MST.ITM_NAME;
                                                }
                                            }
                                        }
                                        else if (finInvVndHdr.IVH_CATEGORY == (int)POInvoiceCategory.Advanced)
                                        {
                                            itemName = finInvVndHdr.FIN_INVOICE_VND_TRX_MPG.ToList()[0].PUR_ORDER_HDR.PUR_ORDER_DTL.ToList()[0].INV_ITEM_MST.ITM_NAME;
                                            tempVATTax.WTH_TAX = finInvVndHdr.FIN_INVOICE_VND_TRX_MPG.ToList()[0].PUR_ORDER_HDR.PUR_ORDER_TAX_HDR.ToList()[0].PTH_TAX;
                                            tempVATTax.WTH_NAME = finInvVndHdr.FIN_INVOICE_VND_TRX_MPG.ToList()[0].PUR_ORDER_HDR.PUR_ORDER_TAX_HDR.ToList()[0].PTH_NAME;
                                            tempVATTax.WTH_TAX_CATEGORY = finInvVndHdr.FIN_INVOICE_VND_TRX_MPG.ToList()[0].PUR_ORDER_HDR.PUR_ORDER_TAX_HDR.ToList()[0].PTH_TAX_CATEGORY;
                                        }
                                        tempVATTax.WTH_ITEM_TEXT = itemName;
                                        //tempVATTaxDetails.Add(tempVATTax);
                                        //TempVATTaxDetails = tempVATTaxDetails;

                                        if (tempVATTax.WTH_TAX.HasValue)
                                        {
                                            TaxPk = Convert.ToInt32(tempVATTax.WTH_TAX);
                                            GetFieldValues(ControlsEnum.TAXDETAILS);
                                            if (dtTaxMst != null && dtTaxMst.Rows.Count > 0 && Convert.ToInt32(dtTaxMst.Rows[0][Resources.DataFieldRes.TaxNotDue]) == (int)TaxEnum.TaxNotYetDue)
                                            {
                                                objtempVATTaxDetails.Add(tempVATTax);
                                                objTempVATTaxDetails = objtempVATTaxDetails;
                                            }
                                        }
                                    }

                                }
                                catch { }

                            }
                            var finPymntTaxHdr = from finObj in objTempVATTaxDetails
                                                 group finObj by new
                                                 {
                                                     finObj.WTH_PARTY_NAME,
                                                     finObj.WTH_TAX_INV_NO,
                                                     finObj.WTH_TAX_DATE
                                                 } into finGrpdObj
                                                 select new FIN_PAYMENT_VND_TAX_HDR
                                                 {
                                                     WTH_PARTY_NAME = finGrpdObj.Key.WTH_PARTY_NAME,
                                                     WTH_TAX_INV_NO = finGrpdObj.Key.WTH_TAX_INV_NO,
                                                     WTH_INV_RECEIVED = finGrpdObj.FirstOrDefault().WTH_INV_RECEIVED,
                                                     WTH_ADDRESS = finGrpdObj.FirstOrDefault().WTH_ADDRESS,
                                                     WTH_BRANCH = finGrpdObj.FirstOrDefault().WTH_BRANCH,
                                                     WTH_BRANCH_NAME = finGrpdObj.FirstOrDefault().WTH_BRANCH_NAME,
                                                     WTH_BRANCH_TEXT = finGrpdObj.FirstOrDefault().WTH_BRANCH_TEXT,
                                                     WTH_BRANCH_TYPE = finGrpdObj.FirstOrDefault().WTH_BRANCH_TYPE,
                                                     WTH_CATEGORY = finGrpdObj.FirstOrDefault().WTH_CATEGORY,
                                                     WTH_DESC = finGrpdObj.FirstOrDefault().WTH_DESC,
                                                     WTH_FORM_NO = finGrpdObj.FirstOrDefault().WTH_FORM_NO,
                                                     WTH_ITEM_TEXT = finGrpdObj.FirstOrDefault().WTH_ITEM_TEXT,
                                                     WTH_NAME = finGrpdObj.FirstOrDefault().WTH_NAME,
                                                     WTH_PAYMENT_HDR = finGrpdObj.FirstOrDefault().WTH_PAYMENT_HDR,
                                                     WTH_PK = finGrpdObj.FirstOrDefault().WTH_PK,
                                                     WTH_PUR_INVOICE = finGrpdObj.FirstOrDefault().WTH_PUR_INVOICE,
                                                     WTH_TAX = finGrpdObj.FirstOrDefault().WTH_TAX,
                                                     WTH_TAX_CATEGORY = finGrpdObj.FirstOrDefault().WTH_TAX_CATEGORY,
                                                     WTH_TAX_DATE = finGrpdObj.FirstOrDefault().WTH_TAX_DATE,
                                                     WTH_REFUND_DATE = finGrpdObj.FirstOrDefault().WTH_REFUND_DATE,
                                                     WTH_TAX_ID = finGrpdObj.FirstOrDefault().WTH_TAX_ID,
                                                     WTH_TRX_HDR = finGrpdObj.FirstOrDefault().WTH_TRX_HDR,
                                                     WTH_TYPE = finGrpdObj.FirstOrDefault().WTH_TYPE,
                                                     WTH_VENDOR = finGrpdObj.FirstOrDefault().WTH_VENDOR,
                                                     WTH_AMOUNT = finGrpdObj.Sum(x => x.WTH_AMOUNT),
                                                     WTH_TAX_AMT = finGrpdObj.Sum(x => x.WTH_TAX_AMT)
                                                 };
                            //TempVATTaxDetails = finPymntTaxHdr.ToList();
                            objTempVATTaxDetails = finPymntTaxHdr.ToList();
                            if (TempVATTaxDetails != null && TempVATTaxDetails.Count > 0)
                            {
                                foreach (FIN_PAYMENT_VND_TAX_HDR ObjfinPymnt in objTempVATTaxDetails)
                                {
                                    try
                                    {
                                        int RowIndex = TempVATTaxDetails.FindIndex(objtemp => objtemp.WTH_TAX_DATE == ObjfinPymnt.WTH_TAX_DATE && objtemp.WTH_TAX_INV_NO == ObjfinPymnt.WTH_TAX_INV_NO && objtemp.WTH_PARTY_NAME == ObjfinPymnt.WTH_PARTY_NAME);
                                        if (RowIndex >= 0 && !blnIsVatbuyExist)
                                        {
                                            TempVATTaxDetails[RowIndex].WTH_AMOUNT = ObjfinPymnt.WTH_AMOUNT;
                                            TempVATTaxDetails[RowIndex].WTH_TAX_AMT = ObjfinPymnt.WTH_TAX_AMT;

                                        }
                                        if (TempVATTaxDetails.Where(objtemp => objtemp.WTH_PUR_INVOICE.Value == ObjfinPymnt.WTH_PUR_INVOICE).Count() == 0)
                                            TempVATTaxDetails.Add(ObjfinPymnt);
                                    }
                                    catch { }
                                }

                            }
                            else
                            {
                                TempVATTaxDetails = objTempVATTaxDetails;
                            }
                        }

                        //}
                        break;
                    #endregion

                    #region PURINVNOS
                    case ControlsEnum.PURINVNOS:
                        SetUIValuesToObject(ControlsEnum.PURINVNOS);
                        break;
                    #endregion

                    #region VENDORCONTACTYPE
                    case ControlsEnum.VENDORCONTACTYPE:
                        int.TryParse(hdfVendorPopup.Value, out VatVendorPopupPk);
                        if (VatVendorPopupPk > 0)
                            dsAdsType = BusinessLogic.POInvoicing.POInvoiceBL.GetVendorAddressTypes(currentUser, Convert.ToInt16(DbActiveStatus.ACTIVE), 0, VatVendorPopupPk, 0);
                        if (dsAdsType != null && dsAdsType.Tables.Count > 0 && dsAdsType.Tables[0].Rows.Count > 0)
                            dtAdsType = dsAdsType.Tables[0];
                        break;
                    #endregion
                    #region VENDORCONTACTYPEDETAILS
                    case ControlsEnum.VENDORCONTACTYPEDETAILS:
                        // if (ddlAddressType.SelectedValue != CommonConstants.SELECTVAL)
                        int.TryParse(hdfAddressType.Value, out VncPk);
                        int.TryParse(hdfVendorPopup.Value, out VatVendorPopupPk);
                        if (VatVendorPopupPk > 0 && VncPk > 0)
                            dsAdsTypeDtl = BusinessLogic.POInvoicing.POInvoiceBL.GetVendorAddressTypes(currentUser, Convert.ToInt16(DbActiveStatus.HASPK), VncPk, VatVendorPopupPk, 0);
                        if (dsAdsTypeDtl != null && dsAdsTypeDtl.Tables.Count > 0 && dsAdsTypeDtl.Tables[0].Rows.Count > 0)
                            dtAdsTypeDtl = dsAdsTypeDtl.Tables[0];
                        break;
                    #endregion

                    #region VENDORBANKS
                    case ControlsEnum.VENDORBANKS:
                        int.TryParse(hdfCusPK.Value, out vendorPk);
                        //int.TryParse(ddlvendorBank.SelectedValue, out VendorbankPk);
                        dtVendorBanks = BusinessLogic.VendorManagement.VendorMaster.GetVendorBanks(currentUser, VendorBankPk, vendorPk, Convert.ToInt16(DbActiveStatus.ACTIVE));

                        break;
                    #endregion

                    #region UPLOADEDFILES
                    case ControlsEnum.UPLOADEDFILES:
                        if (finPaymentVndHdrList != null && finPaymentVndHdrList.Count > 0)
                        {
                            poPaymentServiceClient = new POPaymentService();
                            poPaymentServiceClient = CommonFunctions.InitiateClient(poPaymentServiceClient);
                            admDocAttachObj = CommonFunctions.Initilize<ADM_DOC_ATTACH>();
                            serviceUtilityObj = new ServiceUtility();
                            //serviceUtilityObj.CurrentPage = PageIndex == null ? 1 : Convert.ToInt32(PageIndex);
                            //serviceUtilityObj.PageSize = grdPOInvoiceList.PageSize;
                            admDocAttachObj.DOC_TASK_ID = (int)finPaymentVndHdrList[0].PVH_PK;
                            admDocAttachObj.DOC_TASK = (int)DocTaskEnum.FINANCETASK;
                            DocAttachList = poPaymentServiceClient.GetDocAttachments(admDocAttachObj, serviceUtilityObj);

                        }


                        break;
                    #endregion

                    #region TAXDETAILS
                    case ControlsEnum.TAXDETAILS:
                        dtTaxMst = BusinessLogic.Administration.Masters.TaxSettingsMaster.GetTaxDetails(TaxPk, 0, currentUser.SBUID, (int)DbActiveStatus.HASPK);
                        break;
                    #endregion

                    #region VATBUYPOPUPHEADER
                    case ControlsEnum.VATBUYPOPUPHEADER:
                        lblVatVendorHdr.Text = lblCustomerTxt.Text;
                        VatBuyTaxAmntTotal = 0;
                        foreach (GridViewRow grdrow in grdInvoiceList.Rows)
                        {
                            int InvoicePk = 0;
                            HiddenField hdfInvoicePK = (HiddenField)grdrow.FindControl("hdfInvoicePK");
                            int.TryParse(hdfInvoicePK.Value, out InvoicePk);
                            //FIN_INVOICE_VND_HDR finInvhdr=

                            poPaymentServiceClient = new POPaymentService();
                            poPaymentServiceClient = CommonFunctions.InitiateClient(poPaymentServiceClient);
                            finInvoiceVndHdrObjForPaymentSplit = CommonFunctions.Initilize<FIN_INVOICE_VND_HDR>();
                            finInvoiceVndHdrObjForPaymentSplit.IVH_PK = InvoicePk;
                            finInvoiceVndHdrObjForPaymentSplit.IVH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                            finInvoiceVndHdrListForPaymentSplit = poPaymentServiceClient.GetInvoiceHdrByPK(finInvoiceVndHdrObjForPaymentSplit);
                            foreach (FIN_INVOICE_VND_HDR fininvhderObj in finInvoiceVndHdrListForPaymentSplit)
                            {
                                if (fininvhderObj.FIN_INVOICE_VND_DTL != null && fininvhderObj.FIN_INVOICE_VND_DTL.Count > 0)
                                {
                                    foreach (FIN_INVOICE_VND_DTL finInvExpDtl in fininvhderObj.FIN_INVOICE_VND_DTL)
                                    {
                                        if (finInvExpDtl.FIN_INVOICE_VND_TAX_DTL != null && finInvExpDtl.FIN_INVOICE_VND_TAX_DTL.Count > 0)
                                        {
                                            VatBuyTaxAmntTotal += finInvExpDtl.FIN_INVOICE_VND_TAX_DTL.Where(tx => tx.VTL_TAX == (int)(byte)TaxTypes.VATBuyNotYetDue).Sum(tax => tax.VTL_TAX_AMT);
                                        }
                                    }
                                }
                                if (fininvhderObj.FIN_INVOICE_VND_TAX_HDR != null && fininvhderObj.FIN_INVOICE_VND_TAX_HDR.Count > 0)
                                {
                                    VatBuyTaxAmntTotal += fininvhderObj.FIN_INVOICE_VND_TAX_HDR.Where(tx => tx.VTH_TAX == (int)(byte)TaxTypes.VATBuyNotYetDue).Sum(tax => tax.VTH_TAX_AMT);
                                }

                                //if (fininvhderObj.IVH_GROUP == (int)POInvoiceGroup.Expense)
                                //{
                                //    if (fininvhderObj.FIN_INVOICE_VND_DTL != null)
                                //    {
                                //        foreach (FIN_INVOICE_VND_DTL finInvExpDtl in fininvhderObj.FIN_INVOICE_VND_DTL)
                                //        {
                                //            VatBuyTaxAmntTotal += finInvExpDtl.FIN_INVOICE_VND_TAX_DTL.Where(tx => tx.VTL_TAX == (int)(byte)TaxTypes.VATBuyNotYetDue).Sum(tax => tax.VTL_TAX_AMT);
                                //        }
                                //    }
                                //}
                                //else
                                //{
                                //    VatBuyTaxAmntTotal += fininvhderObj.FIN_INVOICE_VND_TAX_HDR.Where(tx => tx.VTH_TAX == (int)(byte)TaxTypes.VATBuyNotYetDue).Sum(tax => tax.VTH_TAX_AMT);
                                //}
                            }
                        }
                        lblVatTaxAmountHdr.Text = Math.Round(VatBuyTaxAmntTotal, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                        hdfVatTaxAmountHdr.Value = Math.Round(VatBuyTaxAmntTotal, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                        break;
                    #endregion

                    #region PAYMENTADJN
                    case ControlsEnum.PAYMENTADJN:
                        poPaymentServiceClient = new POPaymentService();
                        poPaymentServiceClient = CommonFunctions.InitiateClient(poPaymentServiceClient);
                        CrDrAdjnList = poPaymentServiceClient.GetCrDrAdjn(Convert.ToInt32(hdfCusPK.Value));
                        if (CrDrAdjnList != null)
                        {
                            FinPaymentVndAdjnDupCheckList = poPaymentServiceClient.GetPaymentVndAdjn(Convert.ToInt32(hdfCusPK.Value));
                            if (FinPaymentVndAdjnDupCheckList != null && FinPaymentVndAdjnDupCheckList.Count > 0)
                            {
                                List<long?> lstInvNos = new List<long?>();
                                PaymentAdjnList.ForEach(rr =>
                                {
                                    lstInvNos.Add(rr.FIN_PAYMENT_VND_TRX_MPG.PVM_INVOICE_HDR);
                                });

                                List<long?> lstTRXNos = new List<long?>();
                                PaymentAdjnList.ForEach(rr =>
                                {
                                    if (rr.PAD_PAYMENT_TRX > 0)
                                        lstTRXNos.Add(rr.PAD_PAYMENT_TRX);
                                });

                                CrDrAdjnList.ForEach(CrDtl =>
                                {
                                    if (FinPaymentVndAdjnDupCheckList.Where(recT => recT.PAD_ALCN_PAYMENT_TRX == CrDtl.PAA_TRXPK).Sum(a => a.PAD_AMOUNT) > 0)
                                    {
                                        CrDtl.PAA_AMOUNT_PAID = lstTRXNos.Count > 0 ? FinPaymentVndAdjnDupCheckList
                                            .Where(d => !lstInvNos.Contains(d.FIN_PAYMENT_VND_TRX_MPG.PVM_INVOICE_HDR)
                                                && !lstTRXNos.Contains(d.PAD_PAYMENT_TRX)).ToList()
                                            .Count > 0 ? (FinPaymentVndAdjnDupCheckList
                                            .Where(d => !lstInvNos.Contains(d.FIN_PAYMENT_VND_TRX_MPG.PVM_INVOICE_HDR) && !lstTRXNos.Contains(d.PAD_PAYMENT_TRX)).ToList()
                                            .Select(ss => ss.PAD_ALCN_PAYMENT_TRX == CrDtl.PAA_TRXPK).ToList().Count > 0 ? FinPaymentVndAdjnDupCheckList
                                            .Where(d => !lstInvNos.Contains(d.FIN_PAYMENT_VND_TRX_MPG.PVM_INVOICE_HDR) && !lstTRXNos.Contains(d.PAD_PAYMENT_TRX)).ToList()
                                            .Where(recT => recT.PAD_ALCN_PAYMENT_TRX == CrDtl.PAA_TRXPK).Sum(a => a.PAD_AMOUNT) : 0) : FinPaymentVndAdjnDupCheckList
                                            .Where(recT => recT.PAD_ALCN_PAYMENT_TRX == CrDtl.PAA_TRXPK)
                                            .Sum(a => a.PAD_AMOUNT) : FinPaymentVndAdjnDupCheckList.Where(recT => recT.PAD_ALCN_PAYMENT_TRX == CrDtl.PAA_TRXPK)
                                            .Sum(a => a.PAD_AMOUNT);

                                        CrDtl.PAA_AMOUNT_BAL = CrDtl.PAA_AMOUNT - CrDtl.PAA_AMOUNT_PAID;
                                    }
                                    else if (FinPaymentVndAdjnDupCheckList.Where(recT => recT.PAD_ALCN_CDH == CrDtl.PAA_CRDRPK).Sum(a => a.PAD_AMOUNT) > 0)
                                    {
                                        CrDtl.PAA_AMOUNT_PAID = lstTRXNos.Count > 0 ? FinPaymentVndAdjnDupCheckList
                                            .Where(d => !lstInvNos.Contains(d.FIN_PAYMENT_VND_TRX_MPG.PVM_INVOICE_HDR)
                                            && !lstTRXNos.Contains(d.PAD_PAYMENT_TRX)).ToList()
                                            .Count > 0 ? (FinPaymentVndAdjnDupCheckList
                                            .Where(d => !lstInvNos.Contains(d.FIN_PAYMENT_VND_TRX_MPG.PVM_INVOICE_HDR) && !lstTRXNos.Contains(d.PAD_PAYMENT_TRX)).ToList()
                                            .Select(ss => ss.PAD_ALCN_CDH == CrDtl.PAA_CRDRPK).ToList().Count > 0 ? FinPaymentVndAdjnDupCheckList
                                            .Where(d => !lstInvNos.Contains(d.FIN_PAYMENT_VND_TRX_MPG.PVM_INVOICE_HDR) && !lstTRXNos.Contains(d.PAD_PAYMENT_TRX)).ToList()
                                            .Where(recT => recT.PAD_ALCN_CDH == CrDtl.PAA_CRDRPK).Sum(a => a.PAD_AMOUNT) : 0) : FinPaymentVndAdjnDupCheckList
                                            .Where(recT => recT.PAD_ALCN_CDH == CrDtl.PAA_CRDRPK).Sum(a => a.PAD_AMOUNT) : FinPaymentVndAdjnDupCheckList
                                            .Where(recT => recT.PAD_ALCN_CDH == CrDtl.PAA_CRDRPK).Sum(a => a.PAD_AMOUNT);
                                        CrDtl.PAA_AMOUNT_BAL = CrDtl.PAA_AMOUNT - CrDtl.PAA_AMOUNT_PAID;
                                    }
                                    else
                                    {
                                        CrDtl.PAA_AMOUNT_PAID = 0;
                                        CrDtl.PAA_AMOUNT_BAL = CrDtl.PAA_AMOUNT;
                                    }
                                });
                            }
                            else
                            {
                                CrDrAdjnList.ForEach(CrDtl =>
                                {
                                    CrDtl.PAA_AMOUNT_PAID = 0;
                                    CrDtl.PAA_AMOUNT_BAL = CrDtl.PAA_AMOUNT;
                                });
                            }

                        }
                        break;
                    #endregion

                    #region PAYMENTADJNLIST
                    case ControlsEnum.PAYMENTADJNLIST:
                        poPaymentServiceClient = new POPaymentService();
                        poPaymentServiceClient = CommonFunctions.InitiateClient(poPaymentServiceClient);
                        FinPaymentVndAllocationObj = CommonFunctions.Initilize<FIN_PAYMENT_VND_ALCN_DTL>();
                        FinPaymentVndAllocationObj.PAD_PAYMENT_TRX = PaymentMpgPK;
                        FinPaymentVndAllocationList = poPaymentServiceClient.GetFinPaymentAlcnList(FinPaymentVndAllocationObj);
                        //PaymentAdjnList = FinPaymentVndAllocationList;
                        if (FinPaymentVndAllocationList != null && FinPaymentVndAllocationList.Count > 0)
                        {
                            List<FIN_PAYMENT_VND_ALCN_DTL> tempPaymentAdjnList = new List<FIN_PAYMENT_VND_ALCN_DTL>();// PaymentAdjnList;
                            tempPaymentAdjnList.Where(dtl => dtl.FIN_PAYMENT_VND_TRX_MPG.PVM_INVOICE_HDR == InvoicePK)
                                .ToList().ForEach(dtl => tempPaymentAdjnList.Remove(dtl));
                            FinPaymentVndAllocationList.ForEach(dtl =>
                            {
                                //dtl.FIN_PAYMENT_VND_TRX_MPG = new FIN_PAYMENT_VND_TRX_MPG()
                                //{
                                //    PVM_PK = PaymentMpgPK,
                                //    PVM_INVOICE_HDR = InvoicePK
                                //};
                                tempPaymentAdjnList.Add(dtl);
                            });
                            PaymentAdjnList = tempPaymentAdjnList;
                        }

                        break;
                    #endregion
                    #region VENDORCONTACTFORWHT
                    case ControlsEnum.VENDORCONTACTFORWHT:
                        int.TryParse(hdfWthAddressType.Value, out VncPk);
                        int.TryParse(hdfCusPK.Value, out VatVendorPopupPk);
                        if (VatVendorPopupPk > 0 && VncPk > 0)
                            dsAdsTypeDtl = BusinessLogic.POInvoicing.POInvoiceBL.GetVendorAddressTypes(currentUser, Convert.ToInt16(DbActiveStatus.HASPK), VncPk, VatVendorPopupPk, 0);
                        if (dsAdsTypeDtl != null && dsAdsTypeDtl.Tables.Count > 0 && dsAdsTypeDtl.Tables[0].Rows.Count > 0)
                            dtAdsTypeDtl = dsAdsTypeDtl.Tables[0];
                        break;
                    #endregion
                    #region Payment Types
                    case ControlsEnum.PAYMENTTYPE:
                        dtPaymentTypes = BusinessLogic.POInvoicing.POInvoiceBL.GetPaymentTypes(Convert.ToInt32(DbActiveStatus.ACTIVE), "WHT PAYMENT TYPE");
                        break;
                    #endregion
                    #region PAYADJNLIST
                    case ControlsEnum.PAYMENTADJNDUMMYLIST:
                        poPaymentServiceClient = new POPaymentService();
                        poPaymentServiceClient = CommonFunctions.InitiateClient(poPaymentServiceClient);
                        FinPaymentVndAllocationObj = CommonFunctions.Initilize<FIN_PAYMENT_VND_ALCN_DTL>();
                        FinPaymentVndAllocationList = poPaymentServiceClient.GetFinPaymentAlcnListContext(PaymentAdjnList.Where(dtl => dtl.FIN_PAYMENT_VND_TRX_MPG.PVM_INVOICE_HDR == InvoicePK).ToList());

                        break;
                    #endregion

                    #region PAYMENTTOLERANCE
                    case ControlsEnum.PAYMENTTOLERANCE:
                        CommonServiceClient = new CommonService();
                        CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);
                        admAppConfigMstObj = new ADM_APP_CONFIG_MST();
                        admAppConfigMstObj.ACF_PK = 0;
                        admAppConfigMstObj.ACF_SETTING = "PO PAYMENT SETTINGS";
                        admAppConfigMstObj.ACF_DATA = "PO PAYMENT TOLERANCE";
                        admAppConfigMstObj.ACF_ACTIVE = Convert.ToByte(CommonConstants.ACTIVE);
                        admAppConstMstList = CommonServiceClient.GetAlertNotify(admAppConfigMstObj);
                        if (admAppConstMstList != null && admAppConstMstList.Count > 0)
                        {
                            if (Convert.ToDecimal(admAppConstMstList[0].ACF_VALUE) > 0)
                            {
                                PaymentTollerence = Convert.ToDecimal(admAppConstMstList[0].ACF_VALUE);
                                PaymentTollerence = PaymentTollerence / 100;
                            }
                        }
                        break;
                    #endregion

                    #region ADVINVOICELIST
                    case ControlsEnum.ADVINVOICELIST:
                        poPaymentServiceClient = new POPaymentService();
                        poPaymentServiceClient = CommonFunctions.InitiateClient(poPaymentServiceClient);
                        finAdvDeductList = poPaymentServiceClient.GetAdvDeductList(InvPkSplit, PoPkSplit);
                        break;
                    #endregion
                    #region CURRENCYMST
                    case ControlsEnum.CURRENCYMST:
                        CommonServiceClient = new CommonService();
                        CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);
                        admCurrencyMstObj = new ADM_CURRENCY_MST();
                        admCurrencyMstObj.CUR_PK = CurrencyPk;
                        admCurrencyMstObj.CUR_ACTIVE = Convert.ToByte(CommonConstants.ACTIVE);
                        CurrencyMstList = CommonServiceClient.GetCurrency(admCurrencyMstObj);
                        break;
                    #endregion
                    #region BANK CURRENCY EXCHANGE RATE
                    case ControlsEnum.BANKCURRENCYEXCHANGERATE:
                        //Generate Exchange Rate
                        double ExngRate = 1;
                        poPaymentServiceClient = new POPaymentService();
                        poPaymentServiceClient = CommonFunctions.InitiateClient(poPaymentServiceClient);
                        int FromCurr = string.IsNullOrEmpty(ddlBankChargeCurrency.SelectedValue) ? currentUser.BaseCurrency : Convert.ToInt32(ddlBankChargeCurrency.SelectedValue);
                        paymentDate = string.IsNullOrEmpty(txtPaymentDate.Text.Trim()) ? DateTime.Now : Convert.ToDateTime(txtPaymentDate.Text.Trim());
                        ExngRate = poPaymentServiceClient.GetConversionFactor(FromCurr, currentUser.BaseCurrency, paymentDate, currentUser.SBUID);
                        //txtExchangeRate.Text = ExngRate.ToString();
                        //if (FromCurr == currentUser.BaseCurrency)
                        //{
                        //    txtExchangeRate.Enabled = false;
                        //    txtExchangeRate.CssClass = "medium numeric input-disabled";
                        //}
                        //else
                        //{
                        //    txtExchangeRate.Enabled = true;
                        //    txtExchangeRate.CssClass = "numeric medium";
                        //}
                        break;

                    #endregion
                    #region CR/DR ALLOCATION
                    case ControlsEnum.CRDRALLOCATION:
                        poPaymentServiceClient = new POPaymentService();
                        poPaymentServiceClient = CommonFunctions.InitiateClient(poPaymentServiceClient);
                        List<long> SelectedInvoicePks = new List<long>();
                        if (CurrPK <= 0)
                        {
                            if (finInvoiceHdrList != null)
                            {
                                SelectedInvoicePks = finInvoiceHdrList.Select(r => r.IVH_PK).ToList();
                                //foreach (FIN_INVOICE_VND_HDR objInvoiceDtl in finInvoiceVndHdrList)
                                //{
                                //    SelectedInvoicePks.Add(objInvoiceDtl.IVH_PK);
                                //}
                            }
                        }
                        PaymentCrdrList = poPaymentServiceClient.GetCrDrAllocations(SelectedInvoicePks, CurrPK);
                        break;

                    #endregion

                    #region CR/DR ALLOCATION FOR NEW INVOICE ADDED
                    case ControlsEnum.CRDRALCNFORNEWINV:
                        poPaymentServiceClient = new POPaymentService();
                        poPaymentServiceClient = CommonFunctions.InitiateClient(poPaymentServiceClient);
                        PaymentCrdrListForNewInv = new List<PaymentCrdrMpg>();
                        if (NewInvoiceList != null && NewInvoiceList.Count > 0)
                        {
                            PaymentCrdrListForNewInv = poPaymentServiceClient.GetCrDrAllocations(NewInvoiceList, 0);
                        }
                        break;

                    #endregion

                    #region CREDIT NOTE MPG LIST
                    case ControlsEnum.CRDRMPGLIST:
                        poPaymentServiceClient = new POPaymentService();
                        poPaymentServiceClient = CommonFunctions.InitiateClient(poPaymentServiceClient);
                        FinCrdrMpgList = poPaymentServiceClient.GetCrDrList(InvoicePK);
                        break;

                    #endregion

                    #region INVOICELIST
                    case ControlsEnum.INVOICELIST:
                        int VenorPk = String.IsNullOrEmpty(hdfCusPK.Value.Trim()) ? 0 : Convert.ToInt32(hdfCusPK.Value);
                        InvoiceHearderBO InvHdr = new InvoiceHearderBO();
                        List<InvoiceBO> InvList = new List<InvoiceBO>();
                        foreach (GridViewRow grvRow in grdInvoiceList.Rows)
                        {
                            HiddenField hdfInvoicePK = (HiddenField)grvRow.FindControl("hdfInvoicePK");
                            InvoiceBO Invoice = new InvoiceBO();
                            Invoice.IVH_PK = string.IsNullOrEmpty(hdfInvoicePK.Value) ? 0 : Convert.ToInt64(hdfInvoicePK.Value);
                            InvList.Add(Invoice);
                        }
                        InvHdr.InvoiceList = InvList;
                        string xmlDoc = CommonFunctions.XmlSerialize<InvoiceHearderBO>(InvHdr);
                        dsInvoiceList = BusinessLogic.POInvoicing.POInvoiceBL.GetPOInvoiceList(currentUser, VenorPk, xmlDoc);
                        if (dsInvoiceList != null && dsInvoiceList.Tables.Count > 0)
                        {
                            DataView dvInvoice = dsInvoiceList.Tables[0].DefaultView;
                            dtInvoiceList = dvInvoice.ToTable();
                        }

                        break;
                    #endregion
                    #region INVOICE DETAILS
                    case ControlsEnum.INVOICEDETAILS:
                        poPaymentServiceClient = new POPaymentService();
                        poPaymentServiceClient = CommonFunctions.InitiateClient(poPaymentServiceClient);
                        FinInvoiceVndObj = poPaymentServiceClient.GetInvoiceDetails(NewInvPk);
                        break;
                    #endregion

                    #region GetDebitCreditPost
                    case ControlsEnum.CHECKCREDITDEBITPOST:
                        ValidateId = BusinessLogic.POInvoicing.POInvoiceBL.GetDebitCreditPost(PKXml);
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
                poPaymentServiceClient = null;
                bankMstServiceClient = null;
                finTrxServiceClient = null;
                admCompanyMstServiceClient = null;
                CommonServiceClient = null;
                FinTrxServiceClient = null;

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
                    #region PAYMENTGET
                    case ControlsEnum.PAYMENTGET:
                        GetUIValuesFromObject(controlType);
                        break;
                    #endregion
                    #region COMPANY
                    case ControlsEnum.COMPANY:
                        BindDropDown(ControlsEnum.COMPANY);
                        break;
                    #endregion
                    #region FORMNO
                    case ControlsEnum.FORMNO:
                        BindDropDown(ControlsEnum.FORMNO);
                        break;
                    #endregion
                    #region PURINVNOS
                    case ControlsEnum.PURINVNOS:
                        BindDropDown(ControlsEnum.PURINVNOS);
                        break;
                    #endregion
                    #region PAYMENTHDRLIST
                    case ControlsEnum.PAYMENTHDRLIST:
                        BindGrid(ControlsEnum.PAYMENTHDRLIST);
                        break;
                    #endregion
                    #region PAYMENTMPGLIST
                    case ControlsEnum.PAYMENTMPGLIST:
                        BindGrid(ControlsEnum.PAYMENTMPGLIST);
                        break;
                    #endregion
                    #region PAYMENTSPLITLIST
                    case ControlsEnum.PAYMENTSPLITLIST:
                        BindGrid(ControlsEnum.PAYMENTSPLITLIST);
                        break;
                    #endregion
                    #region Payment Mode
                    case ControlsEnum.PAYMODE:
                        BindDropDown(ControlsEnum.PAYMODE);
                        break;
                    #endregion

                    #region DISCOUNTTYPE
                    case ControlsEnum.DISCOUNTTYPE:
                        BindDropDown(ControlsEnum.DISCOUNTTYPE);
                        break;
                    #endregion
                    #region VENDORACCOUNT
                    case ControlsEnum.VENDORACCOUNT:
                        BindDropDown(ControlsEnum.VENDORACCOUNT);
                        break;
                    #endregion
                    #region VENDORACCOUNTTAX
                    case ControlsEnum.VENDORACCOUNTTAX:
                        GetUIValuesFromObject(ControlsEnum.VENDORACCOUNTTAX);
                        break;
                    #endregion
                    #region BANKCURRENCY
                    case ControlsEnum.BANKCURRENCY:
                        if (admCurrencyMstList != null && admCurrencyMstList.Count > 0)
                        {
                            ddlBankChargeCurrency.Items.Insert(0, (new ListItem(admCurrencyMstList[0].CUR_CODE + " - " + admCurrencyMstList[0].CUR_NAME, admCurrencyMstList[0].CUR_PK.ToString())));
                            //ddlBankChargeCurrency.Items.Insert(0, (new ListItem(admCurrencyMstList[0].CUR_CODE , admCurrencyMstList[0].CUR_PK.ToString())));

                        }
                        break;
                    #endregion

                    #region WHTPOPUPGRID
                    case ControlsEnum.WHTPOPUPGRID:
                        BindGrid(ControlsEnum.WHTPOPUPGRID);
                        break;
                    #endregion

                    #region VATPOPUPGRID
                    case ControlsEnum.VATPOPUPGRID:
                        BindGrid(ControlsEnum.VATPOPUPGRID);
                        break;
                    #endregion

                    #region VENDOR
                    case ControlsEnum.VENDOR:
                        GetUIValuesFromObject(ControlsEnum.VENDOR);
                        break;
                    #endregion

                    #region VATBUYVENDOR
                    case ControlsEnum.VATBUYVENDOR:
                        GetUIValuesFromObject(ControlsEnum.VATBUYVENDOR);
                        break;
                    #endregion
                    #region VATBUYTAXTYPES
                    case ControlsEnum.VATBUYTAXTYPES:
                        BindDropDown(controlType);
                        break;
                    #endregion
                    #region VENDORCONTACTYPE
                    case ControlsEnum.VENDORCONTACTYPE:
                        GetUIValuesFromObject(ControlsEnum.VENDORCONTACTYPE);
                        break;
                    #endregion
                    #region VENDORSELECTEDDTL
                    case ControlsEnum.VENDORSELECTEDDTL:
                        GetUIValuesFromObject(ControlsEnum.VENDORSELECTEDDTL);
                        break;
                    #endregion
                    #region VENDORCONTACTYPEDETAILS
                    case ControlsEnum.VENDORCONTACTYPEDETAILS:
                        GetUIValuesFromObject(ControlsEnum.VENDORCONTACTYPEDETAILS);
                        break;
                    #endregion
                    #region VENDORBANKS
                    case ControlsEnum.VENDORBANKS:
                        BindDropDown(ControlsEnum.VENDORBANKS);
                        break;
                    #endregion
                    #region UPLOADEDFILES
                    case ControlsEnum.UPLOADEDFILES:
                        //if (finInvoiceVndHdrList != null)
                        //    CurrPK = int.Parse(finInvoiceVndHdrList[0].IVH_PK.ToString());
                        BindGrid(ControlsEnum.UPLOADEDFILES);
                        break;
                    #endregion

                    #region PAYMENTADJN
                    case ControlsEnum.PAYMENTADJN:
                        BindGrid(ControlsEnum.PAYMENTADJN);
                        break;
                    #endregion
                    #region PAYMENTTYPE
                    case ControlsEnum.PAYMENTTYPE:
                        BindDropDown(ControlsEnum.PAYMENTTYPE);
                        break;
                    #endregion
                    #region VENDORCONTACTFORWHT
                    case ControlsEnum.VENDORCONTACTFORWHT:
                        GetUIValuesFromObject(ControlsEnum.VENDORCONTACTFORWHT);
                        break;
                    #endregion
                    #region PAYMENTMODESGRID
                    case ControlsEnum.PAYMENTMODESGRID:
                        BindGrid(ControlsEnum.PAYMENTMODESGRID);
                        break;
                    #endregion
                    #region CRDRALLOCATION
                    case ControlsEnum.CRDRALLOCATION:
                        BindGrid(ControlsEnum.CRDRALLOCATION);
                        break;
                    #endregion
                    #region INVOICELIST
                    case ControlsEnum.INVOICELIST:
                        BindGrid(ControlsEnum.INVOICELIST);
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

        public double StringToFormula(string expression)
        {
            List<string> tokens = getTokens(expression);
            Stack<double> operandStack = new Stack<double>();
            Stack<string> operatorStack = new Stack<string>();
            int tokenIndex = 0;
            try
            {
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
            catch
            {
                return 0;
            }
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


        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>
        private Object SetUIValuesToObject(ControlsEnum controlType)
        {

            try
            {
                Object retObject;
                retObject = null;
                int rowID;
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                HiddenField hdfPaymentMpgPK;
                hdfPaymentMpgPK = null;
                HiddenField hdfPaymentSplitPK;
                hdfPaymentSplitPK = null;
                HiddenField hdfPOPK;
                hdfPOPK = null;
                TextBox txtPayNowSplit;
                AlertBO alertBoObj;
                HiddenField hdfWHTTaxPK;
                hdfWHTTaxPK = null;
                HiddenField hdfWHTTax;
                hdfWHTTax = null;
                HiddenField hdfInvoicePK;
                hdfInvoicePK = null;
                TextBox txtAmount;
                TextBox txtOtherCharges;
                Label lblTax;
                TextBox txtAdjustments;
                bool bIsChecked = false;

                Label lblOtherChargesSplit;

                HiddenField hdfTaxSplit;
                int? VendorBank = null;
                decimal PaidAmountBC = 0;
                bool PdcFlag = false;
                switch (controlType)
                {
                    #region Payment Hdr
                    case ControlsEnum.PAYMENTHDRENTRY:
                        finPaymentVndHdrObj.PVH_PK = CurrPK;
                        finPaymentVndHdrObj.PVH_NO = (string.IsNullOrEmpty(lblPaymentNo.Text.Trim()) || lblPaymentNo.Text.Trim().Equals("[NEW]")) ? string.Empty
                                                    : lblPaymentNo.Text.Trim();
                        finPaymentVndHdrObj.PVH_CATEGORY = (byte)PICategory;
                        //Other Charges
                        finPaymentVndHdrObj.PVH_OTHER_AMOUNT = Convert.ToDecimal(hdfTotalOtherCharges.Value.Trim());
                        finPaymentVndHdrObj.PVH_GROUP = ((byte)POGroup) == (byte)0 ? (byte)1 : (byte)POGroup;
                        finPaymentVndHdrObj.PVH_DATE = String.IsNullOrEmpty(txtPaymentDate.Text.Trim()) ? DateTime.Now : Convert.ToDateTime(txtPaymentDate.Text.Trim());
                        finPaymentVndHdrObj.PVH_VENDOR = string.IsNullOrEmpty(hdfVendorPK.Value) ? 0 : Convert.ToInt32(hdfVendorPK.Value);
                        finPaymentVndHdrObj.PVH_VENDOR_ACCOUNT = string.IsNullOrEmpty(hdfVendorAccountNo.Value) ? null : ERP.Utilities.CommonFunctions.NullableInt(hdfVendorAccountNo.Value);
                        //finPaymentVndHdrObj.PVH_MODE = Convert.ToByte(ddlMode.SelectedValue);
                        //finPaymentVndHdrObj.PVH_BANK = string.IsNullOrEmpty(hdfPaymentBank.Value) ? (short?)null : Convert.ToInt16(hdfPaymentBank.Value);
                        //if (Convert.ToInt32(ddlMode.SelectedValue) != (int)PaymentModeEnum.CASH)
                        //{
                        //    finPaymentVndHdrObj.PVH_BRANCH = HttpUtility.HtmlEncode(txtBranch.Text.Trim());
                        //    finPaymentVndHdrObj.PVH_INSTR_NO = HttpUtility.HtmlEncode(txtInstrumentNo.Text.Trim());
                        //    finPaymentVndHdrObj.PVH_INSTR_DATE = String.IsNullOrEmpty(txtInstrumentDate.Text.Trim()) ? DateTime.Now : Convert.ToDateTime(txtInstrumentDate.Text.Trim());
                        //    finPaymentVndHdrObj.PVH_INSTR_FAVOUR = HttpUtility.HtmlEncode(txtFavourof.Text.Trim());
                        //}
                        //finPaymentVndHdrObj.PVH_BANK_CASH_ACCOUNT = string.IsNullOrEmpty(hdfBankAccount.Value) ? 1 : Convert.ToInt32(hdfBankAccount.Value);//1;//Convert.ToInt32(ddlAccountNo.SelectedValue);
                        finPaymentVndHdrObj.PVH_IS_WORK_ORDER = IsWorkOrder == true ? (byte)1 : (byte)0;
                        finPaymentVndHdrObj.PVH_MODE = null;
                        finPaymentVndHdrObj.PVH_BANK = null;
                        finPaymentVndHdrObj.PVH_BRANCH = null;
                        finPaymentVndHdrObj.PVH_INSTR_NO = null;
                        finPaymentVndHdrObj.PVH_INSTR_DATE = null;
                        finPaymentVndHdrObj.PVH_INSTR_FAVOUR = null;
                        finPaymentVndHdrObj.PVH_BANK_CASH_ACCOUNT = null;

                        finPaymentVndHdrObj.PVH_CURRENCY = string.IsNullOrEmpty(hdfPaymentCurrency.Value) ? 1 : Convert.ToInt32(hdfPaymentCurrency.Value);
                        finPaymentVndHdrObj.PVH_PAID_AMOUNT = Convert.ToDecimal(txtPaidAmount.Text.Trim());
                        finPaymentVndHdrObj.PVH_REMARKS = HttpUtility.HtmlEncode(txtRemarks.Text.Trim());
                        finPaymentVndHdrObj.PVH_BASE_CURR = currentUser.BaseCurrency <= 0 ? 1 : currentUser.BaseCurrency;
                        //GetFieldValues(ControlsEnum.EXCHANGERATE);
                        GetFieldValues(ControlsEnum.EXCHANGERATEINBASECURRENCY);
                        //finPaymentVndHdrObj.PVH_EXCHG_RATE = string.IsNullOrEmpty(hdfHdrExchangeRate.Value) ? 0 : Convert.ToDouble(hdfHdrExchangeRate.Value);
                        finPaymentVndHdrObj.PVH_EXCHG_RATE = string.IsNullOrEmpty(txtHdrExchangeRate.Text) ? 0 : Convert.ToDouble(txtHdrExchangeRate.Text);
                        //finPaymentVndHdrObj.PVH_EXCHG_RATE = Convert.ToDouble(txtExchangeRate.Text.Trim());//string.IsNullOrEmpty(hdfExchangeCurrBC.Value) ? 1 : Convert.ToDouble(hdfExchangeCurrBC.Value);
                        //finPaymentVndHdrObj.PVH_PAID_AMOUNT_BC = Convert.ToDecimal(txtPaidAmount.Text.Trim()) * Convert.ToDecimal(finPaymentVndHdrObj.PVH_EXCHG_RATE);
                        finPaymentVndHdrObj.PVH_STATUS = WkfStatus;
                        finPaymentVndHdrObj.PVH_DEL_STATUS = Convert.ToByte(hdfDelStatus.Value);
                        finPaymentVndHdrObj.PVH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        finPaymentVndTrxMpgList = new List<FIN_PAYMENT_VND_TRX_MPG>();
                        finPaymentVndTrxMpgList = (List<FIN_PAYMENT_VND_TRX_MPG>)SetUIValuesToObject(ControlsEnum.PAYMENTMPGENTRY);

                        ////////////// For default split allocaation apply ////////////////
                        foreach (FIN_PAYMENT_VND_TRX_MPG paymentmpg in finPaymentVndTrxMpgList)
                        {

                            if (AppliedInvPkList == null || !AppliedInvPkList.Contains(Convert.ToInt64(paymentmpg.PVM_INVOICE_HDR)))
                            {
                                InvoiceDetails(Convert.ToInt64(paymentmpg.PVM_INVOICE_HDR));
                                PaymentSplitSave(false);
                            }

                        }
                        //////////////////////////////////////////////////////////////////
                        finPaymentVndHdrObj.PVH_DEPT = Convert.ToInt16(currentUser.CurrentDeptPK);
                        finPaymentVndHdrObj.PVH_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                        finPaymentVndHdrObj.PVH_CRTD_BY = Convert.ToInt16(currentUser.PKUser);
                        finPaymentVndHdrObj.PVH_MOD_BY = Convert.ToInt16(currentUser.PKUser);
                        finPaymentVndHdrObj.PVH_CRTD_DT = DateTime.Now;
                        finPaymentVndHdrObj.PVH_MOD_DT = LastModifiedTime;
                        finPaymentVndHdrObj.PVH_COMPANY = Convert.ToInt16(ddlCompany.SelectedValue);
                        finPaymentVndHdrObj.PVH_WHT_BOOK_NO = null;

                        if (Convert.ToInt32(ddlvendorBank.SelectedValue) != Convert.ToInt32(CommonConstants.SELECTVAL))
                            VendorBank = Convert.ToInt32(ddlvendorBank.SelectedValue);
                        finPaymentVndHdrObj.PVH_VENDOR_BANK = VendorBank;

                        //if (hdfWHTNO.Value == string.Empty)
                        //{
                        //    getWHTNO();
                        //}
                        //finPaymentVndHdrObj.PVH_WHT_NO = hdfWHTNO.Value;
                        if (ddlAdjType.SelectedValue == CommonConstants.SELECTVAL)
                            finPaymentVndHdrObj.PVH_DISCOUNT = null;
                        else
                            finPaymentVndHdrObj.PVH_DISCOUNT = Convert.ToInt32(ddlAdjType.SelectedValue);
                        //if (Convert.ToInt32(ddlMode.SelectedValue) == (int)PaymentModeEnum.CHEQUE)
                        //{
                        //    finPaymentVndHdrObj.PVH_PDC = chkPDC.Checked == true ? (byte)1 : (byte)0;
                        //}
                        //else
                        //{
                        //    finPaymentVndHdrObj.PVH_PDC = 0;
                        //}
                        finPaymentVndHdrObj.PVH_DISC_AMOUNT = txtAdjAmount.Text != string.Empty ? Convert.ToDecimal(txtAdjAmount.Text) : 0;
                        if (hdfSaveTax.Value == "1")
                        {
                            finPaymentVndHdrObj.PVH_TAX_AMOUNT = txtTaxAmount.Text != string.Empty ? Convert.ToDecimal(txtTaxAmount.Text) : 0;
                        }
                        else
                        {
                            finPaymentVndHdrObj.PVH_TAX_AMOUNT = 0;
                        }

                        //if (ddlBankChargeCurrency.Items.Count > 0)
                        //    finPaymentVndHdrObj.PVH_BANK_CHARGE_CURR = Convert.ToInt32(ddlBankChargeCurrency.SelectedValue);
                        //else
                        //    finPaymentVndHdrObj.PVH_BANK_CHARGE_CURR = null;
                        //finPaymentVndHdrObj.PVH_BANK_CHARGE = txtBankCharge.Text != string.Empty ? Convert.ToDecimal(txtBankCharge.Text) : 0;
                        //finPaymentVndHdrObj.PVH_BANK_CHARGE_TYPE = chkBankCharge.Checked;
                        finPaymentVndHdrObj.PVH_BANK_CHARGE_CURR = null;
                        finPaymentVndHdrObj.PVH_BANK_CHARGE = null;
                        finPaymentVndHdrObj.PVH_BANK_CHARGE_TYPE = null;

                        //if (ddlWHTAccount.SelectedValue == CommonConstants.SELECTVAL)
                        finPaymentVndHdrObj.PVH_WHT_TAX = null;
                        //else
                        //    finPaymentVndHdrObj.PVH_WHT_TAX = Convert.ToInt32(ddlWHTAccount.SelectedValue);
                        decimal whtAmount = string.IsNullOrEmpty(txtWHTAmount.Text) ? Convert.ToDecimal(0) : Convert.ToDecimal(txtWHTAmount.Text);
                        if (whtAmount > 0)
                        {
                            finPaymentVndTaxHdrList = new List<FIN_PAYMENT_VND_TAX_HDR>();
                            //finPaymentVndTaxHdrList = (List<FIN_PAYMENT_VND_TAX_HDR>)SetUIValuesToObject(ControlsEnum.WHTTAXDETAILS);
                            if (WHTTaxDetails != null && WHTTaxDetails.Count > 0)
                            {
                                SetWHTCertificateNo(finPaymentVndHdrObj);
                                //try
                                //{
                                //   // finPaymentVndHdrObj.FIN_PAYMENT_VND_TAX_HDR = new System.Data.Objects.DataClasses.EntityCollection<FIN_PAYMENT_VND_TAX_HDR>();
                                //}
                                //catch
                                //{

                                //}
                                //WHTTaxDetails.ForEach(dtl =>
                                //{
                                //    finPaymentVndHdrObj.FIN_PAYMENT_VND_TAX_HDR.Add(dtl);
                                //});
                                FIN_PAYMENT_VND_TAX_HDR objTemp;
                                List<FIN_PAYMENT_VND_TAX_HDR> ItemList = new List<FIN_PAYMENT_VND_TAX_HDR>();
                                foreach (FIN_PAYMENT_VND_TAX_HDR objItem in WHTTaxDetails)
                                {
                                    objTemp = CommonFunctions.Initilize<FIN_PAYMENT_VND_TAX_HDR>();
                                    objTemp.WTH_AMOUNT = objItem.WTH_AMOUNT;
                                    objTemp.WTH_TAX_AMT = objItem.WTH_TAX_AMT;

                                    objTemp.WTH_TAX = objItem.WTH_TAX;
                                    objTemp.WTH_PK = 0;
                                    objTemp.WTH_PAYMENT_HDR = objItem.WTH_PAYMENT_HDR;
                                    objTemp.WTH_TYPE = objItem.WTH_TYPE;
                                    objTemp.WTH_TAX_CATEGORY = objItem.WTH_TAX_CATEGORY;
                                    objTemp.WTH_NAME = objItem.WTH_NAME;
                                    objTemp.WTH_DESC = objItem.WTH_DESC;
                                    objTemp.WTH_FORM_NO = objItem.WTH_FORM_NO;
                                    objTemp.WTH_PARTY_NAME = objItem.WTH_PARTY_NAME;
                                    objTemp.WTH_ADDRESS = objItem.WTH_ADDRESS;
                                    objTemp.WTH_ADDRESS = objItem.WTH_ADDRESS;
                                    objTemp.WTH_TAX_ID = objItem.WTH_TAX_ID;
                                    objTemp.WTH_CATEGORY = objItem.WTH_CATEGORY;
                                    objTemp.WTH_TAX_DATE = objItem.WTH_TAX_DATE;
                                    objTemp.WTH_BRANCH = objItem.WTH_BRANCH;
                                    objTemp.WTH_BRANCH_NAME = HttpUtility.HtmlEncode(objItem.WTH_BRANCH_NAME);
                                    objTemp.WTH_BRANCH_TEXT = HttpUtility.HtmlEncode(objItem.WTH_BRANCH_TEXT);
                                    objTemp.WTH_BRANCH_TYPE = objItem.WTH_BRANCH_TYPE;
                                    objTemp.WTH_PAYMENT_TYPE = objItem.WTH_PAYMENT_TYPE;
                                    ItemList.Add(objTemp);
                                }
                                //finPaymentVndHdrObj.FIN_PAYMENT_VND_TAX_HDR=(ItemList;
                                ItemList.ForEach(dtl => finPaymentVndHdrObj.FIN_PAYMENT_VND_TAX_HDR.Add(dtl));
                            }
                        }
                        else
                        {
                            finPaymentVndTaxHdrList = new List<FIN_PAYMENT_VND_TAX_HDR>();
                            FIN_PAYMENT_VND_TAX_HDR objTemp;
                            List<FIN_PAYMENT_VND_TAX_HDR> ItemList = new List<FIN_PAYMENT_VND_TAX_HDR>();
                            objTemp = CommonFunctions.Initilize<FIN_PAYMENT_VND_TAX_HDR>();
                            objTemp.WTH_PK = -1;
                            objTemp.WTH_PAYMENT_HDR = CurrPK;
                            ItemList.Add(objTemp);
                            ItemList.ForEach(dtl => finPaymentVndHdrObj.FIN_PAYMENT_VND_TAX_HDR.Add(dtl));
                        }

                        finPaymentVndTaxHdrList = new List<FIN_PAYMENT_VND_TAX_HDR>();
                        if (VATTaxDetails != null && VATTaxDetails.Count > 0)
                        {

                            FIN_PAYMENT_VND_TAX_HDR objTemp;
                            List<FIN_PAYMENT_VND_TAX_HDR> ItemList = new List<FIN_PAYMENT_VND_TAX_HDR>();
                            foreach (FIN_PAYMENT_VND_TAX_HDR objItem in VATTaxDetails)
                            {
                                objTemp = CommonFunctions.Initilize<FIN_PAYMENT_VND_TAX_HDR>();
                                objTemp.WTH_AMOUNT = objItem.WTH_AMOUNT;
                                objTemp.WTH_TAX_AMT = objItem.WTH_TAX_AMT;

                                objTemp.WTH_TAX = objItem.WTH_TAX;
                                objTemp.WTH_PK = 0;
                                objTemp.WTH_PAYMENT_HDR = objItem.WTH_PAYMENT_HDR;
                                //objTemp.WTH_TRX_HDR = objItem.WTH_TRX_HDR;
                                objTemp.WTH_TYPE = objItem.WTH_TYPE;
                                objTemp.WTH_TAX_CATEGORY = objItem.WTH_TAX_CATEGORY;
                                objTemp.WTH_NAME = objItem.WTH_NAME;
                                objTemp.WTH_DESC = objItem.WTH_DESC;
                                objTemp.WTH_PUR_INVOICE = objItem.WTH_PUR_INVOICE;
                                //objTemp.WTH_FORM_NO = objItem.WTH_FORM_NO;
                                objTemp.WTH_PARTY_NAME = HttpUtility.HtmlEncode(objItem.WTH_PARTY_NAME);
                                //objTemp.WTH_ADDRESS = objItem.WTH_ADDRESS;                                    
                                objTemp.WTH_TAX_ID = objItem.WTH_TAX_ID;
                                objTemp.WTH_CATEGORY = objItem.WTH_CATEGORY;
                                objTemp.WTH_TAX_DATE = objItem.WTH_TAX_DATE;
                                objTemp.WTH_REFUND_DATE = objItem.WTH_REFUND_DATE;
                                objTemp.WTH_VENDOR = objItem.WTH_VENDOR;
                                objTemp.WTH_BRANCH = objItem.WTH_BRANCH;
                                objTemp.WTH_BRANCH_NAME = objItem.WTH_BRANCH_NAME;
                                objTemp.WTH_BRANCH_TEXT = HttpUtility.HtmlEncode(objItem.WTH_BRANCH_TEXT);
                                objTemp.WTH_BRANCH_TYPE = objItem.WTH_BRANCH_TYPE;
                                objTemp.WTH_ITEM_TEXT = HttpUtility.HtmlEncode(objItem.WTH_ITEM_TEXT);
                                objTemp.WTH_TAX_INV_NO = HttpUtility.HtmlEncode(objItem.WTH_TAX_INV_NO);
                                objTemp.WTH_INV_RECEIVED = objItem.WTH_INV_RECEIVED;
                                ItemList.Add(objTemp);
                            }
                            ItemList.ForEach(dtl => finPaymentVndHdrObj.FIN_PAYMENT_VND_TAX_HDR.Add(dtl));

                        }




                        if (chkVendorforpayemnt.Checked)// && ddlWHTAccount.SelectedValue != CommonConstants.SELECTVAL)
                        {
                            finPaymentVndHdrObj.PVH_WHT_AMOUNT = txtWHTAmount.Text != string.Empty ? (Convert.ToDecimal(txtWHTAmount.Text.Trim()) < 0 ? 0 : Convert.ToDecimal(txtWHTAmount.Text)) : 0;
                        }
                        else
                        {
                            finPaymentVndHdrObj.PVH_WHT_AMOUNT = 0;
                        }
                        finPaymentVndHdrObj.PVH_PAY_FOR_VENDOR = chkVendorforpayemnt.Checked;
                        //


                        //Adjn and CR/DR Allocation
                        if (finPaymentVndTrxMpgList != null && finPaymentVndTrxMpgList.Count > 0)
                        {
                            finPaymentVndTrxMpgList.ForEach(dtl =>
                            {
                                finPaymentVndHdrObj.FIN_PAYMENT_VND_TRX_MPG.Add(dtl);
                                #region Adjustment Allocation                               
                                FinPaymentVndAllocationList = PaymentAdjnList.Where(mpg => mpg.FIN_PAYMENT_VND_TRX_MPG.PVM_INVOICE_HDR == dtl.PVM_INVOICE_HDR).ToList();
                                if (FinPaymentVndAllocationList.Count() > 0)
                                {
                                    FinPaymentVndAllocationList.ForEach(mpg =>
                                    {
                                        FinPaymentVndAllocationObj = CommonFunctions.Initilize<FIN_PAYMENT_VND_ALCN_DTL>();
                                        FinPaymentVndAllocationObj.PAD_PK = mpg.PAD_PK;
                                        FinPaymentVndAllocationObj.PAD_PAYMENT_TRX = mpg.PAD_PAYMENT_TRX;
                                        FinPaymentVndAllocationObj.PAD_ALCN_CDH = mpg.PAD_ALCN_CDH;
                                        FinPaymentVndAllocationObj.PAD_ALCN_PAYMENT_TRX = mpg.PAD_ALCN_PAYMENT_TRX;
                                        FinPaymentVndAllocationObj.PAD_AMOUNT = mpg.PAD_AMOUNT;
                                        FinPaymentVndAllocationObj.PAD_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                                        dtl.FIN_PAYMENT_VND_ALCN_DTL.Add(FinPaymentVndAllocationObj);
                                    });
                                }
                                #endregion

                                #region CR/DR Allocation                               
                                FinPaymentVndCrdrAllocationList = PaymentCrdrList.Where(mpg => mpg.PNM_INVOICE_HDR == dtl.PVM_INVOICE_HDR && (mpg.PNM_PAID_AMOUNT > 0 || mpg.PNM_ADJ_AMOUNT > 0)).ToList();
                                if (FinPaymentVndCrdrAllocationList != null && FinPaymentVndCrdrAllocationList.Count() > 0)
                                {
                                    FinPaymentVndCrdrAllocationList.ForEach(mpg =>
                                    {
                                        FinPaymentVndCrdrAllocationObj = CommonFunctions.Initilize<FIN_PAYMENT_VND_CRDR_MPG>();
                                        FinPaymentVndCrdrAllocationObj.PNM_ACTIVE = mpg.PNM_ACTIVE;
                                        FinPaymentVndCrdrAllocationObj.PNM_ADJ_AMOUNT = mpg.PNM_ADJ_AMOUNT;
                                        FinPaymentVndCrdrAllocationObj.PNM_CRDR_HDR = mpg.PNM_CRDR_HDR;
                                        FinPaymentVndCrdrAllocationObj.PNM_PAID_AMOUNT = mpg.PNM_PAID_AMOUNT;
                                        FinPaymentVndCrdrAllocationObj.PNM_PAYMENT_HDR = mpg.PNM_PAYMENT_HDR;
                                        FinPaymentVndCrdrAllocationObj.PNM_PAYMENT_TRX_MPG = mpg.PNM_PAYMENT_TRX_MPG;
                                        FinPaymentVndCrdrAllocationObj.PNM_PK = mpg.PNM_PK;
                                        FinPaymentVndCrdrAllocationObj.PNM_CRDR_MPG = mpg.PNM_CRDR_MPG;
                                        dtl.FIN_PAYMENT_VND_CRDR_MPG.Add(FinPaymentVndCrdrAllocationObj);
                                    });
                                }
                                #endregion

                            });
                        }


                        if (finPaymentVndTrxMpgList != null && finPaymentVndTrxMpgList.Count > 0)
                        {
                            //try
                            //{
                            //    finPaymentVndHdrObj.FIN_PAYMENT_VND_TRX_MPG = new System.Data.Objects.DataClasses.EntityCollection<FIN_PAYMENT_VND_TRX_MPG>();
                            //}
                            //catch
                            //{
                            //}
                            finPaymentVndHdrObj.FIN_PAYMENT_VND_PO_MPG = new System.Data.Objects.DataClasses.EntityCollection<FIN_PAYMENT_VND_PO_MPG>();
                            foreach (FIN_PAYMENT_VND_TRX_MPG dtl in finPaymentVndTrxMpgList)
                            {
                                finPaymentVndPoMpgList = InvoicePOSplitList.Where(mpg => mpg.FIN_PAYMENT_VND_TRX_MPG != null
                                       && mpg.FIN_PAYMENT_VND_TRX_MPG.PVM_INVOICE_HDR == dtl.PVM_INVOICE_HDR).ToList();
                                if (finPaymentVndPoMpgList.Count() > 0)
                                {
                                    FIN_PAYMENT_VND_PO_MPG obTemp = new FIN_PAYMENT_VND_PO_MPG();
                                    //if (finPaymentVndPoMpgList.Count == 1 && finPaymentVndPoMpgList[0].PPO_PAID_AMOUNT == 0)
                                    //{
                                    //    foreach (FIN_PAYMENT_VND_PO_MPG mpg in finPaymentVndPoMpgList)
                                    //    {
                                    //        //mpg.FIN_PAYMENT_VND_TRX_MPG = null;
                                    //        obTemp = CommonFunctions.Initilize<FIN_PAYMENT_VND_PO_MPG>();
                                    //        obTemp.PPO_ACTIVE = mpg.PPO_ACTIVE;
                                    //        obTemp.PPO_PAID_AMOUNT = dtl.PVM_PAID_AMOUNT;
                                    //        obTemp.PPO_PAYMENT_HDR = mpg.PPO_PAYMENT_HDR;
                                    //        obTemp.PPO_PAYMENT_TRX_MPG = mpg.PPO_PAYMENT_TRX_MPG;
                                    //        obTemp.PPO_OTHER_AMOUNT = mpg.PPO_OTHER_AMOUNT;
                                    //        obTemp.PPO_TAX_AMOUNT = dtl.PVM_TAX_AMOUNT;
                                    //        obTemp.PPO_PK = mpg.PPO_PK;
                                    //        obTemp.PPO_PO_HDR = mpg.PPO_PO_HDR;

                                    //        finPaymentVndTaxDtlObj = CommonFunctions.Initilize<FIN_PAYMENT_VND_TAX_DTL>();
                                    //        finPaymentVndTaxDtlObj.PDT_PK = 0;
                                    //        obTemp.FIN_PAYMENT_VND_TAX_DTL.Add(finPaymentVndTaxDtlObj);

                                    //        dtl.FIN_PAYMENT_VND_PO_MPG.Add(obTemp);
                                    //        finPaymentVndHdrObj.FIN_PAYMENT_VND_PO_MPG.Add(obTemp);
                                    //    }
                                    //}
                                    //else
                                    //{
                                    foreach (FIN_PAYMENT_VND_PO_MPG mpg in finPaymentVndPoMpgList)
                                    {
                                        //mpg.FIN_PAYMENT_VND_TRX_MPG = null;
                                        obTemp = CommonFunctions.Initilize<FIN_PAYMENT_VND_PO_MPG>();
                                        obTemp.PPO_ACTIVE = mpg.PPO_ACTIVE;
                                        obTemp.PPO_PAID_AMOUNT = mpg.PPO_PAID_AMOUNT;
                                        obTemp.PPO_PAYMENT_HDR = mpg.PPO_PAYMENT_HDR;
                                        obTemp.PPO_PAYMENT_TRX_MPG = mpg.PPO_PAYMENT_TRX_MPG;
                                        obTemp.PPO_OTHER_AMOUNT = mpg.PPO_OTHER_AMOUNT;
                                        obTemp.PPO_TAX_AMOUNT = mpg.PPO_TAX_AMOUNT;
                                        obTemp.PPO_PK = mpg.PPO_PK;
                                        obTemp.PPO_PO_HDR = mpg.PPO_PO_HDR;
                                        obTemp.PPO_WO_HDR = mpg.PPO_WO_HDR;

                                        finPaymentVndTaxDtlObj = CommonFunctions.Initilize<FIN_PAYMENT_VND_TAX_DTL>();
                                        finPaymentVndTaxDtlObj.PDT_PK = 0;
                                        obTemp.FIN_PAYMENT_VND_TAX_DTL.Add(finPaymentVndTaxDtlObj);

                                        dtl.FIN_PAYMENT_VND_PO_MPG.Add(obTemp);
                                        finPaymentVndHdrObj.FIN_PAYMENT_VND_PO_MPG.Add(obTemp);
                                    }
                                    //}

                                }
                                else if (CurrPK == 0 && dtl.PVM_INVOICE_HDR.HasValue)
                                {
                                    InvoicePK = dtl.PVM_INVOICE_HDR.Value;
                                    GetFieldValues(ControlsEnum.INVOICEVNDMPGLIST);
                                    if (FinInvoiceVndTrxMpgList != null && FinInvoiceVndTrxMpgList.Count == 1)
                                    {
                                        //dtl.FIN_PAYMENT_VND_PO_MPG = new System.Data.Objects.DataClasses.EntityCollection<FIN_PAYMENT_VND_PO_MPG>();
                                        finPaymentVndPoMpgObj = CommonFunctions.Initilize<FIN_PAYMENT_VND_PO_MPG>();
                                        finPaymentVndPoMpgObj.PPO_PK = 0;
                                        finPaymentVndPoMpgObj.PPO_PO_HDR = FinInvoiceVndTrxMpgList.First().IVM_PO_HDR;
                                        finPaymentVndPoMpgObj.PPO_WO_HDR = FinInvoiceVndTrxMpgList.First().IVM_WO_HDR;
                                        finPaymentVndPoMpgObj.PPO_PAID_AMOUNT = dtl.PVM_PAID_AMOUNT;
                                        finPaymentVndPoMpgObj.PPO_OTHER_AMOUNT = dtl.PVM_OTHER_AMOUNT;
                                        finPaymentVndPoMpgObj.PPO_TAX_AMOUNT = dtl.PVM_TAX_AMOUNT;
                                        finPaymentVndPoMpgObj.PPO_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);

                                        finPaymentVndTaxDtlObj = CommonFunctions.Initilize<FIN_PAYMENT_VND_TAX_DTL>();
                                        finPaymentVndTaxDtlObj.PDT_PK = 0;
                                        finPaymentVndPoMpgObj.FIN_PAYMENT_VND_TAX_DTL.Add(finPaymentVndTaxDtlObj);

                                        dtl.FIN_PAYMENT_VND_PO_MPG.Add(finPaymentVndPoMpgObj);
                                        finPaymentVndHdrObj.FIN_PAYMENT_VND_PO_MPG.Add(finPaymentVndPoMpgObj);
                                        finPaymentVndPoMpgList = dtl.FIN_PAYMENT_VND_PO_MPG.ToList();
                                    }
                                }
                                finPaymentVndHdrObj.FIN_PAYMENT_VND_TRX_MPG.Add(dtl);
                                // finPaymentVndHdrObj.FIN_PAYMENT_VND_PO_MPG.Add(
                            }


                        }


                        #region Payment mode details
                        PdcFlag = false;
                        if (PaymentModeDetailsList == null || PaymentModeDetailsList.Count == 0 || (PaymentModeDetailsList.Count == 1 && !IsPaymentModeAdded))
                        {
                            AddPaymentModes(false);
                            IsPaymentModeAdded = false;
                        }
                        if (PaymentModeDetailsList != null && PaymentModeDetailsList.Count > 0)
                        {

                            FIN_PAYMENT_VND_MODE_DTL PaymentModeDtl;
                            List<FIN_PAYMENT_VND_MODE_DTL> PaymentModeItemList = new List<FIN_PAYMENT_VND_MODE_DTL>();
                            foreach (FIN_PAYMENT_VND_MODE_DTL objItem in PaymentModeDetailsList)
                            {
                                PaymentModeDtl = CommonFunctions.Initilize<FIN_PAYMENT_VND_MODE_DTL>();
                                PaymentModeDtl.PDM_MODE = objItem.PDM_MODE;
                                PaymentModeDtl.PDM_BANK = objItem.PDM_BANK;
                                PaymentModeDtl.PDM_PK = objItem.PDM_PK;
                                PaymentModeDtl.PDM_PAYMENT_HDR = objItem.PDM_PAYMENT_HDR;
                                PaymentModeDtl.PDM_BRANCH = objItem.PDM_BRANCH;
                                PaymentModeDtl.PDM_INSTR_NO = objItem.PDM_INSTR_NO;
                                PaymentModeDtl.PDM_INSTR_DATE = objItem.PDM_INSTR_DATE;
                                PaymentModeDtl.PDM_INSTR_FAVOUR = objItem.PDM_INSTR_FAVOUR;
                                PaymentModeDtl.PDM_PDC = objItem.PDM_PDC;
                                PaymentModeDtl.PDM_BANK_CHARGE_CURR = objItem.PDM_BANK_CHARGE_CURR;
                                PaymentModeDtl.PDM_BANK_CHARGE = objItem.PDM_BANK_CHARGE;
                                PaymentModeDtl.PDM_BANK_CHARGE_TYPE = objItem.PDM_BANK_CHARGE_TYPE;
                                PaymentModeDtl.PDM_EXCHG_RATE = objItem.PDM_EXCHG_RATE;
                                PaymentModeDtl.PDM_PAID_AMOUNT = objItem.PDM_PAID_AMOUNT;
                                PaymentModeDtl.PDM_PAID_AMOUNT_BC = objItem.PDM_PAID_AMOUNT_BC;
                                PaymentModeDtl.PDM_ACCOUNT = objItem.PDM_ACCOUNT;
                                PaymentModeItemList.Add(PaymentModeDtl);

                                if (objItem.PDM_PDC > 0)
                                    PdcFlag = true;
                            }
                            PaymentModeItemList.ForEach(PymntModDtl => finPaymentVndHdrObj.FIN_PAYMENT_VND_MODE_DTL.Add(PymntModDtl));
                            PaidAmountBC = PaymentModeItemList.Sum(r => r.PDM_PAID_AMOUNT_BC);
                        }
                        if (PdcFlag)
                            finPaymentVndHdrObj.PVH_PDC = (byte)1;
                        #endregion

                        finPaymentVndHdrObj.PVH_PAID_AMOUNT_BC = PaidAmountBC;

                        ////if (finPaymentVndPoMpgList != null && finPaymentVndPoMpgList.Count > 0)
                        ////{
                        ////    //finReceiptCusHdrObj.FIN_RECEIPT_CUS_SO_MPG = new System.Data.Objects.DataClasses.EntityCollection<FIN_RECEIPT_CUS_SO_MPG>();
                        ////    //finReceiptCusHdrObj.FIN_RECEIPT_CUS_TRX_MPG = new System.Data.Objects.DataClasses.EntityCollection<FIN_RECEIPT_CUS_TRX_MPG>(); 
                        ////    finPaymentVndPoMpgList.ForEach(dtl =>
                        ////    {                                
                        ////        //finReceiptCusHdrObj.FIN_RECEIPT_CUS_SO_MPG.Add(dtl);
                        ////        // dtl.FIN_RECEIPT_CUS_TAX_DTL = new System.Data.Objects.DataClasses.EntityCollection<FIN_RECEIPT_CUS_TAX_DTL>();
                        ////        finPaymentVndTaxDtlObj = CommonFunctions.Initilize<FIN_PAYMENT_VND_TAX_DTL>();
                        ////        finPaymentVndTaxDtlObj.PDT_PK = 0;
                        ////        //if (dtl.FIN_PAYMENT_VND_TAX_DTL.Count < 1)
                        ////            dtl.FIN_PAYMENT_VND_TAX_DTL.Add(finPaymentVndTaxDtlObj);// finReceiptCusTaxDtlList.Add(finReceiptCusTaxDtlObj);
                        ////    });
                        ////}
                        retObject = finPaymentVndHdrObj;
                        break;
                    #endregion
                    #region Payment Maping
                    case ControlsEnum.PAYMENTMPGENTRY:
                        rowID = 0;
                        foreach (GridViewRow grdrow in grdInvoiceList.Rows)
                        {
                            decimal taxpercentage = 0;
                            decimal basevalue = 0;
                            decimal ttaxamt = 0;
                            decimal hdftax = 0;
                            decimal hdfTaxHdrDtl = 0;
                            decimal hdftotalamt = 0;

                            finPaymentVndTrxMpgObj = CommonFunctions.Initilize<FIN_PAYMENT_VND_TRX_MPG>();
                            hdfPaymentMpgPK = (HiddenField)grdInvoiceList.Rows[rowID].FindControl(GetLocalResourceObject("hdfPaymentMpgPK").ToString());
                            finPaymentVndTrxMpgObj.PVM_PK = hdfPaymentMpgPK == null ? 0 : Convert.ToInt64(hdfPaymentMpgPK.Value);
                            finPaymentVndTrxMpgObj.PVM_PAYMENT_HDR = CurrPK;
                            hdfInvoicePK = (HiddenField)grdInvoiceList.Rows[rowID].FindControl(GetLocalResourceObject("hdfInvoicePK").ToString());
                            finPaymentVndTrxMpgObj.PVM_INVOICE_HDR = hdfInvoicePK == null ? 0 : Convert.ToInt32(hdfInvoicePK.Value);
                            txtAmount = (TextBox)grdInvoiceList.Rows[rowID].FindControl(GetLocalResourceObject("txtPayNow").ToString());
                            txtOtherCharges = (TextBox)grdInvoiceList.Rows[rowID].FindControl("txtOtherCharges");
                            finPaymentVndTrxMpgObj.PVM_PAID_AMOUNT = txtAmount == null ? 0 : txtAmount.Text.Trim() == string.Empty ? 0 : Convert.ToDecimal(txtAmount.Text.Trim());
                            finPaymentVndTrxMpgObj.PVM_OTHER_AMOUNT = txtOtherCharges == null ? 0 : txtOtherCharges.Text.Trim() == string.Empty ? 0 : Convert.ToDecimal(txtOtherCharges.Text.Trim());
                            finPaymentVndTrxMpgObj.PVM_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);

                            HiddenField hdfCategory = (HiddenField)grdInvoiceList.Rows[rowID].FindControl("hdfCategory");
                            HiddenField hdfGroup = (HiddenField)grdInvoiceList.Rows[rowID].FindControl("hdfGroup");
                            HiddenField hdfTaxAmt = (HiddenField)grdInvoiceList.Rows[rowID].FindControl("hdfTaxAmt");
                            HiddenField hdfTaxHdrDtlAmt = (HiddenField)grdInvoiceList.Rows[rowID].FindControl("hdfTaxHdrDtlAmt");
                            hdftax = hdfTaxAmt.Value != string.Empty ? Convert.ToDecimal(hdfTaxAmt.Value) : 0;
                            hdfTaxHdrDtl = hdfTaxHdrDtlAmt.Value != string.Empty ? Convert.ToDecimal(hdfTaxHdrDtlAmt.Value) : 0;
                            HiddenField hdfTotalAmt = (HiddenField)grdInvoiceList.Rows[rowID].FindControl("hdfTotalAmt");
                            hdftotalamt = hdfTotalAmt.Value != string.Empty ? Convert.ToDecimal(hdfTotalAmt.Value) : 0;
                            //taxpercentage = hdftax / (hdftotalamt == 0 ? 1 : hdftotalamt);
                            //basevalue = (Convert.ToDecimal(txtAmount.Text)) / (1 + taxpercentage);
                            //ttaxamt = (Convert.ToDecimal(txtAmount.Text) - basevalue);
                            HiddenField hdfTotalTax = (HiddenField)grdInvoiceList.Rows[rowID].FindControl("hdfTotalTax");
                            lblAdjAmount = (Label)grdInvoiceList.Rows[rowID].FindControl("lblAdjAmount");
                            lblAdjAmount.Text = lblAdjAmount.Text.Replace(",", "");
                            Label lblBaltopay = (Label)grdInvoiceList.Rows[rowID].FindControl("lblBaltopay");
                            lblBaltopay.Text = lblBaltopay.Text.Replace(",", "");
                            ttaxamt = Convert.ToDecimal(hdfTotalTax.Value);
                            finPaymentVndTrxMpgObj.PVM_TAX_AMOUNT = Math.Round(ttaxamt, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);

                            //if (Convert.ToInt32(hdfCategory.Value) == (int)POInvoiceCategory.Advanced || Convert.ToInt32(hdfGroup.Value) == (int)POInvoiceGroup.Expense || Convert.ToInt32(hdfGroup.Value) == (int)POInvoiceGroup.Services)
                            //{
                            //    finPaymentVndTrxMpgObj.PVM_TAX_AMOUNT = Math.Round(ttaxamt, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                            //}
                            //else
                            //{
                            //    finPaymentVndTrxMpgObj.PVM_TAX_AMOUNT = 0;
                            //}

                            txtAdjustments = (TextBox)grdInvoiceList.Rows[rowID].FindControl(GetLocalResourceObject("txtAdjustments").ToString());
                            finPaymentVndTrxMpgObj.PVM_DISC_AMOUNT = txtAdjustments == null ? 0 : txtAdjustments.Text.Trim() == string.Empty ? 0 : Convert.ToDecimal(txtAdjustments.Text.Trim());

                            finPaymentVndTrxMpgObj.PVM_ADJUST_AMOUNT = string.IsNullOrEmpty(lblAdjAmount.Text) ? 0 : Convert.ToDecimal(lblAdjAmount.Text.Trim());
                            decimal excessAmt = (finPaymentVndTrxMpgObj.PVM_PAID_AMOUNT - Convert.ToDecimal(lblBaltopay.Text.Trim()));
                            finPaymentVndTrxMpgObj.PVM_EXCESS_AMOUNT = (excessAmt > 0) ? excessAmt : 0;

                            ////////////// For default split allocaation apply ////////////////
                            //if (AppliedInvPkList == null || !AppliedInvPkList.Contains(Convert.ToInt64(finPaymentVndTrxMpgObj.PVM_INVOICE_HDR)))
                            //{
                            //    InvoiceDetails(Convert.ToInt64(finPaymentVndTrxMpgObj.PVM_INVOICE_HDR));
                            //    PaymentSplitSave();
                            //}
                            //////////////////////////////////////////////////////////////////

                            //if (finPaymentVndTrxMpgObj.PVM_PAID_AMOUNT > 0)
                            //{
                            finPaymentVndTrxMpgList.Add(finPaymentVndTrxMpgObj);
                            //}
                            rowID++;
                        }
                        retObject = finPaymentVndTrxMpgList;
                        break;
                    #endregion

                    #region Journalize
                    case ControlsEnum.REVERSE:
                    case ControlsEnum.JOURNALIZE:
                    case ControlsEnum.CHEQUERETURN:
                        if (EntryStatus == EntryStatus.LISTMODE)
                        {
                            foreach (GridViewRow grdrow in grdPOPaymentHdr.Rows)
                            {
                                RadioButton rbtn;
                                rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                                if (rbtn.Checked)
                                {
                                    bIsChecked = true;
                                    CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfPaymentID")).Value);
                                    Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                                    Session[ERP.Utilities.SessionStrings.VendorPK] = ((HiddenField)grdrow.FindControl("hdfVendorPK")).Value;
                                    Session[ERP.Utilities.SessionStrings.Vendor] = ((Label)grdrow.FindControl("lblVendor")).Text;
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
                                if (finPaymentVndHdrList != null && finPaymentVndHdrList.Count() > 0)
                                {
                                    if (finPaymentVndHdrList[0].PVH_PAID_AMOUNT == 0)
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_Journalize_Zero_Amnt").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                        return null;
                                    }
                                }

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
                                if (controlType == ControlsEnum.JOURNALIZE)
                                {
                                    ucrJournalize.TransactionType = (POGroup == POInvoiceGroup.Goods || POGroup == POInvoiceGroup.WorkOrder) ? ApplicationType.VPJ
                                        : POGroup == POInvoiceGroup.Services ? ApplicationType.SIPJ : POGroup == POInvoiceGroup.AgtInvoice ? ApplicationType.AIPJ : ApplicationType.EIPJ;
                                    Session[ERP.Utilities.SessionStrings.TransactionType] = (POGroup == POInvoiceGroup.Goods || POGroup == POInvoiceGroup.WorkOrder) ? ApplicationType.VPJ
                                        : POGroup == POInvoiceGroup.Services ? ApplicationType.SIPJ : POGroup == POInvoiceGroup.AgtInvoice ? ApplicationType.AIPJ : ApplicationType.EIPJ;
                                    ucrJournalize.JournalType = (int)JournalTypeEnum.Voucher;
                                }
                                else if (controlType == ControlsEnum.REVERSE)
                                {
                                    Session[ERP.Utilities.SessionStrings.TransactionType] = ucrJournalize.TransactionType = ApplicationType.PPCCJ;
                                    ucrJournalize.JournalType = (int)JournalTypeEnum.Reverse;
                                }
                                else if (controlType == ControlsEnum.CHEQUERETURN)
                                {
                                    Session[ERP.Utilities.SessionStrings.TransactionType] = ucrJournalize.TransactionType = ApplicationType.PCBJ;
                                    ucrJournalize.JournalType = (int)JournalTypeEnum.Return;
                                }

                                ucrJournalize.TransactionPK = (int)CurrPK;
                                Session[ERP.Utilities.SessionStrings.TransactionPK] = CurrPK;
                                ucrJournalize.JournalizePK = 0;
                                Session[ERP.Utilities.SessionStrings.JournalizePK] = null;
                                GetFieldValues(ControlsEnum.PAYMENTHDRENTRYBYPK);
                                Session[ERP.Utilities.SessionStrings.TransactionNo] = finPaymentVndHdrList[0].PVH_NO;
                                Session[ERP.Utilities.SessionStrings.TransactionDate] = finPaymentVndHdrList[0].PVH_DATE;
                                Session[ERP.Utilities.SessionStrings.TransactionCurrency] = finPaymentVndHdrList[0].PVH_CURRENCY;
                                Session[ERP.Utilities.SessionStrings.AccountPayable] = ApplicationType.VND;
                                Session[ERP.Utilities.SessionStrings.AccountPayablePK] = finPaymentVndHdrList[0].PVH_VENDOR;
                                Session[ERP.Utilities.SessionStrings.JournalType] = POGroup == POInvoiceGroup.Goods ? ApplicationType.VPJ
                                    : POGroup == POInvoiceGroup.Services ? ApplicationType.SIPJ : POGroup == POInvoiceGroup.AgtInvoice ? ApplicationType.AIPJ : ApplicationType.EIPJ;

                                ucrWrkf.WrkfSubmit -= ActionHandler;
                                ucrWrkf.Reset();
                                ucrWrkf.ViewType = 1;
                                if (controlType == ControlsEnum.JOURNALIZE)
                                {
                                    FillProcessID(2);
                                }
                                else if (controlType == ControlsEnum.REVERSE)
                                {
                                    FillProcessID(4);
                                }
                                else if (controlType == ControlsEnum.CHEQUERETURN)
                                {
                                    FillProcessID(6);
                                }
                                GetFieldValues(ControlsEnum.FINHEADER);
                                //EntryStatus = EntryStatus.ENTRYMODE;
                                WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                                if (finTrxHdrList != null && finTrxHdrList.Count > 0)
                                {
                                    ucrWrkf.RefID = workflowCore.GetRefID((int)finTrxHdrList[0].FTH_PK, ucrWrkf.ProcessID);
                                    base.WkfRefID = ucrWrkf.RefID;
                                }
                                SetCancelRef((int)CurrPK);
                                ucrWrkf.FillWorkFlowDetails();
                                if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && (EntryStatus == EntryStatus.ENTRYMODE || EntryStatus == EntryStatus.LISTMODE) && ucrWrkf.HasPageTaskPermission)
                                {
                                    ucrJournalize.JournalizeRefPK = ucrWrkf.RefID;
                                    ucrWrkf.ViewType = 1;
                                    //Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.ENTRYMODE;
                                }
                                else
                                {
                                    ucrWrkf.ViewType = 0;
                                    //EntryStatus = EntryStatus.VIEWMODE;
                                    //Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.VIEWMODE;
                                }
                                ucrJournalize.HasWkfPermission = ucrWrkf.HasPageTaskPermission;
                                Session[ERP.Utilities.SessionStrings.JournalMode] = (EntryStatus == EntryStatus.ENTRYMODE || EntryStatus == EntryStatus.LISTMODE) ? EntryStatus.ENTRYMODE : EntryStatus.VIEWMODE;
                                ucrWrkf.ViewAction();

                                HiddenField hdfExchangeRateJV = (HiddenField)ucrJournalize.FindControl("hdfExchangeRateJV");
                                hdfExchangeRateJV.Value = "";

                                TextBox txtJournalExchangeRate = (TextBox)ucrJournalize.FindControl("txtJournalExchangeRate");
                                txtJournalExchangeRate.Text = "";

                                TextBox txtNarration = (TextBox)ucrJournalize.FindControl("txtNarration");
                                txtNarration.Text = "";

                                TextBox WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                                WrkfComments.Text = "";
                                Session[ERP.Utilities.SessionStrings.RemoveRowIndex] = null;
                                hdfJournalizeWorkFlow.Value = "1";
                                int mode = String.IsNullOrEmpty(ddlMode.SelectedValue) ? 0 : Convert.ToInt32(ddlMode.SelectedValue);
                                ucrJournalize.TypeForNumberGenaration = mode == (int)PaymentModeEnum.CASH ? "1" : "0";
                                ucrJournalize.CallUserControl();
                                if (controlType == ControlsEnum.JOURNALIZE)
                                {
                                    Session[ERP.Utilities.SessionStrings.JournalHead] = hdfJournalName.Value = GetLocalResourceObject("Vendor_Payment_Journal").ToString();
                                }
                                else if (controlType == ControlsEnum.REVERSE)
                                {
                                    Session[ERP.Utilities.SessionStrings.JournalHead] = hdfJournalName.Value = GetLocalResourceObject("Payment_PDC_Voucher").ToString();
                                }
                                else if (controlType == ControlsEnum.CHEQUERETURN)
                                {
                                    Session[ERP.Utilities.SessionStrings.JournalHead] = hdfJournalName.Value = GetLocalResourceObject("Payment_Return_Voucher").ToString();
                                }
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_Journalize_Msg").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            }
                        }
                        else
                        {
                            if (EntryStatus == EntryStatus.NEWMODE)
                            {
                                litErrorMsg.Text = Resources.Report.PmtNtApp;
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            }
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region Payment Hdr WorkFlow
                    case ControlsEnum.WRKFSUBMIT:

                        finPaymentVndHdrObj.PVH_PK = CurrPK;
                        updatePayment = true;

                        //Other Charges

                        finPaymentVndHdrObj.PVH_OTHER_AMOUNT = Convert.ToDecimal(hdfTotalOtherCharges.Value.Trim());
                        finPaymentVndHdrObj.PVH_DATE = String.IsNullOrEmpty(txtPaymentDate.Text.Trim()) ? DateTime.Now : Convert.ToDateTime(txtPaymentDate.Text.Trim());
                        finPaymentVndHdrObj.PVH_CATEGORY = (byte)PICategory;
                        finPaymentVndHdrObj.PVH_GROUP = ((byte)POGroup) == (byte)0 ? (byte)1 : (byte)POGroup;
                        finPaymentVndHdrObj.PVH_VENDOR = string.IsNullOrEmpty(hdfVendorPK.Value) ? 0 : Convert.ToInt32(hdfVendorPK.Value);
                        finPaymentVndHdrObj.PVH_VENDOR_ACCOUNT = null;

                        //Is workOrder
                        finPaymentVndHdrObj.PVH_IS_WORK_ORDER = IsWorkOrder == true ? (byte)1 : (byte)0;
                        //finPaymentVndHdrObj.PVH_MODE = Convert.ToByte(ddlMode.SelectedValue);
                        //finPaymentVndHdrObj.PVH_BANK = string.IsNullOrEmpty(hdfPaymentBank.Value) ? (short?)null : Convert.ToInt16(hdfPaymentBank.Value);
                        //if (Convert.ToInt32(ddlMode.SelectedValue) != (int)PaymentModeEnum.CASH)
                        //{
                        //    finPaymentVndHdrObj.PVH_BRANCH = HttpUtility.HtmlEncode(txtBranch.Text.Trim());
                        //    finPaymentVndHdrObj.PVH_INSTR_NO = HttpUtility.HtmlEncode(txtInstrumentNo.Text.Trim());
                        //    finPaymentVndHdrObj.PVH_INSTR_DATE = String.IsNullOrEmpty(txtInstrumentDate.Text.Trim()) ? DateTime.Now : Convert.ToDateTime(txtInstrumentDate.Text.Trim());
                        //    finPaymentVndHdrObj.PVH_INSTR_FAVOUR = HttpUtility.HtmlEncode(txtFavourof.Text.Trim());
                        //}
                        //finPaymentVndHdrObj.PVH_BANK_CASH_ACCOUNT = string.IsNullOrEmpty(hdfBankAccount.Value) ? 1 : Convert.ToInt32(hdfBankAccount.Value);
                        finPaymentVndHdrObj.PVH_MODE = null;
                        finPaymentVndHdrObj.PVH_BANK = null;
                        finPaymentVndHdrObj.PVH_BRANCH = null;
                        finPaymentVndHdrObj.PVH_INSTR_NO = null;
                        finPaymentVndHdrObj.PVH_INSTR_DATE = null;
                        finPaymentVndHdrObj.PVH_INSTR_FAVOUR = null;
                        finPaymentVndHdrObj.PVH_BANK_CASH_ACCOUNT = null;

                        finPaymentVndHdrObj.PVH_CURRENCY = string.IsNullOrEmpty(hdfPaymentCurrency.Value) ? 1 : Convert.ToInt32(hdfPaymentCurrency.Value);
                        finPaymentVndHdrObj.PVH_PAID_AMOUNT = Convert.ToDecimal(txtPaidAmount.Text.Trim());
                        finPaymentVndHdrObj.PVH_REMARKS = HttpUtility.HtmlEncode(txtRemarks.Text.Trim());
                        finPaymentVndHdrObj.PVH_BASE_CURR = currentUser.BaseCurrency <= 0 ? 1 : currentUser.BaseCurrency;
                        //GetFieldValues(ControlsEnum.EXCHANGERATE);
                        GetFieldValues(ControlsEnum.EXCHANGERATEINBASECURRENCY);
                        //finPaymentVndHdrObj.PVH_EXCHG_RATE = string.IsNullOrEmpty(hdfHdrExchangeRate.Value) ? 0 : Convert.ToDouble(hdfHdrExchangeRate.Value);
                        finPaymentVndHdrObj.PVH_EXCHG_RATE = string.IsNullOrEmpty(txtHdrExchangeRate.Text) ? 0 : Convert.ToDouble(txtHdrExchangeRate.Text);
                        //finPaymentVndHdrObj.PVH_EXCHG_RATE = Convert.ToDouble(txtExchangeRate.Text.Trim());//string.IsNullOrEmpty(hdfExchangeCurrBC.Value) ? 1 : Convert.ToDouble(hdfExchangeCurrBC.Value);
                        //finPaymentVndHdrObj.PVH_PAID_AMOUNT_BC = Convert.ToDecimal(txtPaidAmount.Text.Trim()) * Convert.ToDecimal(finPaymentVndHdrObj.PVH_EXCHG_RATE);
                        finPaymentVndHdrObj.PVH_STATUS = WkfStatus;
                        finPaymentVndHdrObj.PVH_DEL_STATUS = Convert.ToByte(hdfDelStatus.Value);
                        finPaymentVndHdrObj.PVH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        finPaymentVndTrxMpgList = new List<FIN_PAYMENT_VND_TRX_MPG>();
                        finPaymentVndTrxMpgList = (List<FIN_PAYMENT_VND_TRX_MPG>)SetUIValuesToObject(ControlsEnum.PAYMENTMPGENTRY);

                        ////////////// For default split allocaation apply ////////////////

                        foreach (FIN_PAYMENT_VND_TRX_MPG paymentmpg in finPaymentVndTrxMpgList)
                        {

                            if (AppliedInvPkList == null || !AppliedInvPkList.Contains(Convert.ToInt64(paymentmpg.PVM_INVOICE_HDR)))
                            {
                                InvoiceDetails(Convert.ToInt64(paymentmpg.PVM_INVOICE_HDR));
                                PaymentSplitSave(false);
                            }

                        }

                        //////////////////////////////////////////////////////////////////

                        finPaymentVndHdrObj.PVH_DEPT = Convert.ToInt16(currentUser.CurrentDeptPK);
                        finPaymentVndHdrObj.PVH_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                        finPaymentVndHdrObj.PVH_CRTD_BY = Convert.ToInt16(currentUser.PKUser);
                        finPaymentVndHdrObj.PVH_MOD_BY = Convert.ToInt16(currentUser.PKUser);
                        finPaymentVndHdrObj.PVH_CRTD_DT = DateTime.Now;
                        finPaymentVndHdrObj.PVH_MOD_DT = LastModifiedTime;
                        finPaymentVndHdrObj.PVH_COMPANY = Convert.ToInt16(ddlCompany.SelectedValue);
                        //
                        finPaymentVndHdrObj.PVH_WHT_BOOK_NO = null;

                        if (Convert.ToInt32(ddlvendorBank.SelectedValue) != Convert.ToInt32(CommonConstants.SELECTVAL))
                            VendorBank = Convert.ToInt32(ddlvendorBank.SelectedValue);
                        finPaymentVndHdrObj.PVH_VENDOR_BANK = VendorBank;

                        SetWHTCertificateNo(finPaymentVndHdrObj);
                        if (ddlAdjType.SelectedValue == CommonConstants.SELECTVAL)
                            finPaymentVndHdrObj.PVH_DISCOUNT = null;
                        else
                            finPaymentVndHdrObj.PVH_DISCOUNT = Convert.ToInt32(ddlAdjType.SelectedValue);
                        //if (Convert.ToInt32(ddlMode.SelectedValue) == (int)PaymentModeEnum.CHEQUE)
                        //{
                        //    finPaymentVndHdrObj.PVH_PDC = chkPDC.Checked == true ? (byte)1 : (byte)0;
                        //}
                        //else
                        //{
                        //    finPaymentVndHdrObj.PVH_PDC = 0;
                        //}
                        finPaymentVndHdrObj.PVH_PDC = 0;
                        finPaymentVndHdrObj.PVH_DISC_AMOUNT = txtAdjAmount.Text != string.Empty ? Convert.ToDecimal(txtAdjAmount.Text) : 0;

                        if (hdfSaveTax.Value == "1")
                        {
                            finPaymentVndHdrObj.PVH_TAX_AMOUNT = txtTaxAmount.Text != string.Empty ? Convert.ToDecimal(txtTaxAmount.Text) : 0;
                        }
                        else
                        {
                            finPaymentVndHdrObj.PVH_TAX_AMOUNT = 0;
                        }

                        //if (ddlBankChargeCurrency.Items.Count > 0)
                        //    finPaymentVndHdrObj.PVH_BANK_CHARGE_CURR = Convert.ToInt32(ddlBankChargeCurrency.SelectedValue);
                        //else
                        //    finPaymentVndHdrObj.PVH_BANK_CHARGE_CURR = null;                        
                        //finPaymentVndHdrObj.PVH_BANK_CHARGE = txtBankCharge.Text != string.Empty ? Convert.ToDecimal(txtBankCharge.Text) : 0;
                        //finPaymentVndHdrObj.PVH_BANK_CHARGE_TYPE = chkBankCharge.Checked;
                        finPaymentVndHdrObj.PVH_BANK_CHARGE_CURR = null;
                        finPaymentVndHdrObj.PVH_BANK_CHARGE = null;
                        finPaymentVndHdrObj.PVH_BANK_CHARGE_TYPE = null;
                        //if (ddlWHTAccount.SelectedValue == CommonConstants.SELECTVAL)
                        finPaymentVndHdrObj.PVH_WHT_TAX = null;
                        //else
                        //    finPaymentVndHdrObj.PVH_WHT_TAX = Convert.ToInt32(ddlWHTAccount.SelectedValue);
                        decimal whtAmount1 = string.IsNullOrEmpty(txtWHTAmount.Text) ? Convert.ToDecimal(0) : Convert.ToDecimal(txtWHTAmount.Text);
                        if (whtAmount1 > 0)
                        {
                            finPaymentVndTaxHdrList = new List<FIN_PAYMENT_VND_TAX_HDR>();
                            //finPaymentVndTaxHdrList = (List<FIN_PAYMENT_VND_TAX_HDR>)SetUIValuesToObject(ControlsEnum.WHTTAXDETAILS);
                            if (WHTTaxDetails != null && WHTTaxDetails.Count > 0)
                            {
                                //    try
                                //    {
                                //        finPaymentVndHdrObj.FIN_PAYMENT_VND_TAX_HDR = new System.Data.Objects.DataClasses.EntityCollection<FIN_PAYMENT_VND_TAX_HDR>();
                                //    }
                                //    catch
                                //    {
                                //    }
                                //    WHTTaxDetails.ForEach(dtl =>
                                //    {
                                //        finPaymentVndHdrObj.FIN_PAYMENT_VND_TAX_HDR.Add(dtl);
                                //    });
                                FIN_PAYMENT_VND_TAX_HDR objTemp;
                                List<FIN_PAYMENT_VND_TAX_HDR> ItemList = new List<FIN_PAYMENT_VND_TAX_HDR>();
                                foreach (FIN_PAYMENT_VND_TAX_HDR objItem in WHTTaxDetails)
                                {
                                    objTemp = CommonFunctions.Initilize<FIN_PAYMENT_VND_TAX_HDR>();
                                    objTemp.WTH_AMOUNT = objItem.WTH_AMOUNT;
                                    objTemp.WTH_TAX_AMT = objItem.WTH_TAX_AMT;

                                    objTemp.WTH_TAX = objItem.WTH_TAX;
                                    objTemp.WTH_PK = 0;
                                    objTemp.WTH_PAYMENT_HDR = objItem.WTH_PAYMENT_HDR;
                                    objTemp.WTH_TYPE = objItem.WTH_TYPE;
                                    objTemp.WTH_TAX_CATEGORY = objItem.WTH_TAX_CATEGORY;
                                    objTemp.WTH_NAME = objItem.WTH_NAME;
                                    objTemp.WTH_DESC = objItem.WTH_DESC;
                                    objTemp.WTH_FORM_NO = objItem.WTH_FORM_NO;
                                    objTemp.WTH_PARTY_NAME = objItem.WTH_PARTY_NAME;
                                    objTemp.WTH_ADDRESS = objItem.WTH_ADDRESS;
                                    objTemp.WTH_ADDRESS = objItem.WTH_ADDRESS;
                                    objTemp.WTH_TAX_ID = objItem.WTH_TAX_ID;
                                    objTemp.WTH_CATEGORY = objItem.WTH_CATEGORY;
                                    objTemp.WTH_TAX_DATE = objItem.WTH_TAX_DATE;
                                    objTemp.WTH_BRANCH = objItem.WTH_BRANCH;
                                    objTemp.WTH_BRANCH_NAME = HttpUtility.HtmlEncode(objItem.WTH_BRANCH_NAME);
                                    objTemp.WTH_BRANCH_TEXT = HttpUtility.HtmlEncode(objItem.WTH_BRANCH_TEXT);
                                    objTemp.WTH_BRANCH_TYPE = objItem.WTH_BRANCH_TYPE;
                                    objTemp.WTH_PAYMENT_TYPE = objItem.WTH_PAYMENT_TYPE;
                                    ItemList.Add(objTemp);
                                }
                                //finPaymentVndHdrObj.FIN_PAYMENT_VND_TAX_HDR=(ItemList;
                                ItemList.ForEach(dtl => finPaymentVndHdrObj.FIN_PAYMENT_VND_TAX_HDR.Add(dtl));
                            }
                        }

                        finPaymentVndTaxHdrList = new List<FIN_PAYMENT_VND_TAX_HDR>();
                        if (VATTaxDetails != null && VATTaxDetails.Count > 0)
                        {

                            FIN_PAYMENT_VND_TAX_HDR objTemp;
                            List<FIN_PAYMENT_VND_TAX_HDR> ItemList = new List<FIN_PAYMENT_VND_TAX_HDR>();
                            foreach (FIN_PAYMENT_VND_TAX_HDR objItem in VATTaxDetails)
                            {
                                objTemp = CommonFunctions.Initilize<FIN_PAYMENT_VND_TAX_HDR>();
                                objTemp.WTH_AMOUNT = objItem.WTH_AMOUNT;
                                objTemp.WTH_TAX_AMT = objItem.WTH_TAX_AMT;

                                objTemp.WTH_TAX = objItem.WTH_TAX;
                                objTemp.WTH_PK = 0;
                                objTemp.WTH_PAYMENT_HDR = objItem.WTH_PAYMENT_HDR;
                                //objTemp.WTH_TRX_HDR = objItem.WTH_TRX_HDR;
                                objTemp.WTH_TYPE = objItem.WTH_TYPE;
                                objTemp.WTH_TAX_CATEGORY = objItem.WTH_TAX_CATEGORY;
                                objTemp.WTH_NAME = objItem.WTH_NAME;
                                objTemp.WTH_DESC = objItem.WTH_DESC;
                                objTemp.WTH_PUR_INVOICE = objItem.WTH_PUR_INVOICE;
                                //objTemp.WTH_FORM_NO = objItem.WTH_FORM_NO;
                                objTemp.WTH_PARTY_NAME = HttpUtility.HtmlEncode(objItem.WTH_PARTY_NAME);
                                //objTemp.WTH_ADDRESS = objItem.WTH_ADDRESS;                                    
                                objTemp.WTH_TAX_ID = objItem.WTH_TAX_ID;
                                objTemp.WTH_CATEGORY = objItem.WTH_CATEGORY;
                                objTemp.WTH_TAX_DATE = objItem.WTH_TAX_DATE;
                                objTemp.WTH_REFUND_DATE = objItem.WTH_REFUND_DATE;
                                objTemp.WTH_VENDOR = objItem.WTH_VENDOR;
                                objTemp.WTH_BRANCH = objItem.WTH_BRANCH;
                                objTemp.WTH_BRANCH_NAME = objItem.WTH_BRANCH_NAME;
                                objTemp.WTH_BRANCH_TEXT = HttpUtility.HtmlEncode(objItem.WTH_BRANCH_TEXT);
                                objTemp.WTH_BRANCH_TYPE = objItem.WTH_BRANCH_TYPE;
                                objTemp.WTH_ITEM_TEXT = HttpUtility.HtmlEncode(objItem.WTH_ITEM_TEXT);
                                objTemp.WTH_TAX_INV_NO = HttpUtility.HtmlEncode(objItem.WTH_TAX_INV_NO);
                                objTemp.WTH_INV_RECEIVED = objItem.WTH_INV_RECEIVED;
                                ItemList.Add(objTemp);
                            }
                            ItemList.ForEach(dtl => finPaymentVndHdrObj.FIN_PAYMENT_VND_TAX_HDR.Add(dtl));

                        }

                        if (chkVendorforpayemnt.Checked)// && ddlWHTAccount.SelectedValue != CommonConstants.SELECTVAL)
                        {
                            finPaymentVndHdrObj.PVH_WHT_AMOUNT = txtWHTAmount.Text != string.Empty ? (Convert.ToDecimal(txtWHTAmount.Text.Trim()) < 0 ? 0 : Convert.ToDecimal(txtWHTAmount.Text)) : 0;
                        }
                        else
                        {
                            finPaymentVndHdrObj.PVH_WHT_AMOUNT = 0;
                        }
                        finPaymentVndHdrObj.PVH_PAY_FOR_VENDOR = chkVendorforpayemnt.Checked;

                        ////


                        //Adjn Allocation
                        if (finPaymentVndTrxMpgList != null && finPaymentVndTrxMpgList.Count > 0)
                        {
                            finPaymentVndTrxMpgList.ForEach(dtl =>
                            {
                                finPaymentVndHdrObj.FIN_PAYMENT_VND_TRX_MPG.Add(dtl);

                                #region Adjustment Allocation 
                                FinPaymentVndAllocationList = PaymentAdjnList.Where(mpg => mpg.FIN_PAYMENT_VND_TRX_MPG.PVM_INVOICE_HDR == dtl.PVM_INVOICE_HDR).ToList();
                                if (FinPaymentVndAllocationList.Count() > 0)
                                {
                                    FinPaymentVndAllocationList.ForEach(mpg =>
                                    {
                                        FinPaymentVndAllocationObj = CommonFunctions.Initilize<FIN_PAYMENT_VND_ALCN_DTL>();
                                        FinPaymentVndAllocationObj.PAD_PK = mpg.PAD_PK;
                                        FinPaymentVndAllocationObj.PAD_PAYMENT_TRX = mpg.PAD_PAYMENT_TRX;
                                        FinPaymentVndAllocationObj.PAD_ALCN_CDH = mpg.PAD_ALCN_CDH;
                                        FinPaymentVndAllocationObj.PAD_ALCN_PAYMENT_TRX = mpg.PAD_ALCN_PAYMENT_TRX;
                                        FinPaymentVndAllocationObj.PAD_AMOUNT = mpg.PAD_AMOUNT;
                                        FinPaymentVndAllocationObj.PAD_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                                        dtl.FIN_PAYMENT_VND_ALCN_DTL.Add(FinPaymentVndAllocationObj);
                                    });
                                }
                                #endregion

                                #region CR/DR Allocation
                                FinPaymentVndCrdrAllocationList = PaymentCrdrList.Where(mpg => mpg.PNM_INVOICE_HDR == dtl.PVM_INVOICE_HDR && (mpg.PNM_PAID_AMOUNT > 0 || mpg.PNM_ADJ_AMOUNT > 0)).ToList();
                                if (FinPaymentVndCrdrAllocationList != null && FinPaymentVndCrdrAllocationList.Count() > 0)
                                {
                                    FinPaymentVndCrdrAllocationList.ForEach(mpg =>
                                    {
                                        FinPaymentVndCrdrAllocationObj = CommonFunctions.Initilize<FIN_PAYMENT_VND_CRDR_MPG>();
                                        FinPaymentVndCrdrAllocationObj.PNM_ACTIVE = mpg.PNM_ACTIVE;
                                        FinPaymentVndCrdrAllocationObj.PNM_ADJ_AMOUNT = mpg.PNM_ADJ_AMOUNT;
                                        FinPaymentVndCrdrAllocationObj.PNM_CRDR_HDR = mpg.PNM_CRDR_HDR;
                                        FinPaymentVndCrdrAllocationObj.PNM_PAID_AMOUNT = mpg.PNM_PAID_AMOUNT;
                                        FinPaymentVndCrdrAllocationObj.PNM_PAYMENT_HDR = mpg.PNM_PAYMENT_HDR;
                                        FinPaymentVndCrdrAllocationObj.PNM_PAYMENT_TRX_MPG = mpg.PNM_PAYMENT_TRX_MPG;
                                        FinPaymentVndCrdrAllocationObj.PNM_PK = mpg.PNM_PK;
                                        FinPaymentVndCrdrAllocationObj.PNM_CRDR_MPG = mpg.PNM_CRDR_MPG;
                                        dtl.FIN_PAYMENT_VND_CRDR_MPG.Add(FinPaymentVndCrdrAllocationObj);
                                    });
                                }
                                #endregion
                            });
                        }

                        if (finPaymentVndTrxMpgList != null && finPaymentVndTrxMpgList.Count > 0)
                        {
                            //try
                            //{
                            // finPaymentVndHdrObj.FIN_PAYMENT_VND_TRX_MPG = new System.Data.Objects.DataClasses.EntityCollection<FIN_PAYMENT_VND_TRX_MPG>();
                            //}
                            //catch
                            //{
                            //}
                            //finPaymentVndTrxMpgList.ForEach(dtl =>
                            //{
                            //    finPaymentVndHdrObj.FIN_PAYMENT_VND_TRX_MPG.Add(dtl);

                            //    finPaymentVndPoMpgList = InvoicePOSplitList.Where(mpg => mpg.FIN_PAYMENT_VND_TRX_MPG != null
                            //        && mpg.FIN_PAYMENT_VND_TRX_MPG.PVM_INVOICE_HDR == dtl.PVM_INVOICE_HDR).ToList();
                            //    if (finPaymentVndPoMpgList.Count() > 0)
                            //    {
                            //        finPaymentVndPoMpgList.ForEach(mpg =>
                            //        {
                            //            mpg.FIN_PAYMENT_VND_TRX_MPG = null;
                            //            dtl.FIN_PAYMENT_VND_PO_MPG.Add(new FIN_PAYMENT_VND_PO_MPG()
                            //            {
                            //                PPO_ACTIVE = mpg.PPO_ACTIVE,
                            //                PPO_PAID_AMOUNT = mpg.PPO_PAID_AMOUNT,
                            //                PPO_PAYMENT_HDR = mpg.PPO_PAYMENT_HDR,
                            //                PPO_PAYMENT_TRX_MPG = mpg.PPO_PAYMENT_TRX_MPG,
                            //                PPO_PK = mpg.PPO_PK,
                            //                PPO_PO_HDR = mpg.PPO_PO_HDR
                            //            });
                            //        });
                            //    }
                            //    else if (CurrPK == 0 && dtl.PVM_INVOICE_HDR.HasValue)
                            //    {
                            //        InvoicePK = dtl.PVM_INVOICE_HDR.Value;
                            //        GetFieldValues(ControlsEnum.INVOICEVNDMPGLIST);
                            //        if (FinInvoiceVndTrxMpgList != null && FinInvoiceVndTrxMpgList.Count == 1)
                            //        {
                            //            dtl.FIN_PAYMENT_VND_PO_MPG = new System.Data.Objects.DataClasses.EntityCollection<FIN_PAYMENT_VND_PO_MPG>();
                            //            finPaymentVndPoMpgObj = CommonFunctions.Initilize<FIN_PAYMENT_VND_PO_MPG>();
                            //            finPaymentVndPoMpgObj.PPO_PK = 0;
                            //            finPaymentVndPoMpgObj.PPO_PO_HDR = FinInvoiceVndTrxMpgList.First().IVM_PO_HDR;
                            //            finPaymentVndPoMpgObj.PPO_PAID_AMOUNT = dtl.PVM_PAID_AMOUNT;
                            //            finPaymentVndPoMpgObj.PPO_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                            //            dtl.FIN_PAYMENT_VND_PO_MPG.Add(finPaymentVndPoMpgObj);
                            //        }
                            //    }
                            //});
                            finPaymentVndHdrObj.FIN_PAYMENT_VND_PO_MPG = new System.Data.Objects.DataClasses.EntityCollection<FIN_PAYMENT_VND_PO_MPG>();
                            foreach (FIN_PAYMENT_VND_TRX_MPG dtl in finPaymentVndTrxMpgList)
                            {
                                finPaymentVndPoMpgList = InvoicePOSplitList.Where(mpg => mpg.FIN_PAYMENT_VND_TRX_MPG != null
                                       && mpg.FIN_PAYMENT_VND_TRX_MPG.PVM_INVOICE_HDR == dtl.PVM_INVOICE_HDR).ToList();
                                if (finPaymentVndPoMpgList.Count() > 0)
                                {

                                    FIN_PAYMENT_VND_PO_MPG obTemp = new FIN_PAYMENT_VND_PO_MPG();
                                    //if (finPaymentVndPoMpgList.Count == 1 && finPaymentVndPoMpgList[0].PPO_PAID_AMOUNT == 0)
                                    //{
                                    //    foreach (FIN_PAYMENT_VND_PO_MPG mpg in finPaymentVndPoMpgList)
                                    //    {
                                    //        //mpg.FIN_PAYMENT_VND_TRX_MPG = null;
                                    //        obTemp = CommonFunctions.Initilize<FIN_PAYMENT_VND_PO_MPG>();
                                    //        obTemp.PPO_ACTIVE = mpg.PPO_ACTIVE;
                                    //        obTemp.PPO_PAID_AMOUNT = dtl.PVM_PAID_AMOUNT;
                                    //        obTemp.PPO_PAYMENT_HDR = mpg.PPO_PAYMENT_HDR;
                                    //        obTemp.PPO_PAYMENT_TRX_MPG = mpg.PPO_PAYMENT_TRX_MPG;
                                    //        obTemp.PPO_OTHER_AMOUNT = mpg.PPO_OTHER_AMOUNT;
                                    //        obTemp.PPO_TAX_AMOUNT = dtl.PVM_TAX_AMOUNT;
                                    //        obTemp.PPO_PK = mpg.PPO_PK;
                                    //        obTemp.PPO_PO_HDR = mpg.PPO_PO_HDR;

                                    //        finPaymentVndTaxDtlObj = CommonFunctions.Initilize<FIN_PAYMENT_VND_TAX_DTL>();
                                    //        finPaymentVndTaxDtlObj.PDT_PK = 0;
                                    //        obTemp.FIN_PAYMENT_VND_TAX_DTL.Add(finPaymentVndTaxDtlObj);

                                    //        dtl.FIN_PAYMENT_VND_PO_MPG.Add(obTemp);
                                    //        finPaymentVndHdrObj.FIN_PAYMENT_VND_PO_MPG.Add(obTemp);
                                    //    }
                                    //}
                                    //else
                                    //{
                                    foreach (FIN_PAYMENT_VND_PO_MPG mpg in finPaymentVndPoMpgList)
                                    {
                                        //mpg.FIN_PAYMENT_VND_TRX_MPG = null;
                                        obTemp = CommonFunctions.Initilize<FIN_PAYMENT_VND_PO_MPG>();
                                        obTemp.PPO_ACTIVE = mpg.PPO_ACTIVE;
                                        obTemp.PPO_PAID_AMOUNT = mpg.PPO_PAID_AMOUNT;
                                        obTemp.PPO_PAYMENT_HDR = mpg.PPO_PAYMENT_HDR;
                                        obTemp.PPO_PAYMENT_TRX_MPG = mpg.PPO_PAYMENT_TRX_MPG;
                                        obTemp.PPO_OTHER_AMOUNT = mpg.PPO_OTHER_AMOUNT;
                                        obTemp.PPO_TAX_AMOUNT = mpg.PPO_TAX_AMOUNT;
                                        obTemp.PPO_PK = mpg.PPO_PK;
                                        obTemp.PPO_PO_HDR = mpg.PPO_PO_HDR;
                                        obTemp.PPO_WO_HDR = mpg.PPO_WO_HDR;

                                        finPaymentVndTaxDtlObj = CommonFunctions.Initilize<FIN_PAYMENT_VND_TAX_DTL>();
                                        finPaymentVndTaxDtlObj.PDT_PK = 0;
                                        obTemp.FIN_PAYMENT_VND_TAX_DTL.Add(finPaymentVndTaxDtlObj);

                                        dtl.FIN_PAYMENT_VND_PO_MPG.Add(obTemp);
                                        finPaymentVndHdrObj.FIN_PAYMENT_VND_PO_MPG.Add(obTemp);
                                    }
                                    //}
                                }
                                else if (CurrPK == 0 && dtl.PVM_INVOICE_HDR.HasValue)
                                {
                                    InvoicePK = dtl.PVM_INVOICE_HDR.Value;
                                    GetFieldValues(ControlsEnum.INVOICEVNDMPGLIST);
                                    if (FinInvoiceVndTrxMpgList != null && FinInvoiceVndTrxMpgList.Count == 1)
                                    {
                                        //dtl.FIN_PAYMENT_VND_PO_MPG = new System.Data.Objects.DataClasses.EntityCollection<FIN_PAYMENT_VND_PO_MPG>();
                                        finPaymentVndPoMpgObj = CommonFunctions.Initilize<FIN_PAYMENT_VND_PO_MPG>();
                                        finPaymentVndPoMpgObj.PPO_PK = 0;
                                        finPaymentVndPoMpgObj.PPO_PO_HDR = FinInvoiceVndTrxMpgList.First().IVM_PO_HDR;
                                        finPaymentVndPoMpgObj.PPO_PAID_AMOUNT = dtl.PVM_PAID_AMOUNT;
                                        finPaymentVndPoMpgObj.PPO_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                                        finPaymentVndPoMpgObj.PPO_OTHER_AMOUNT = dtl.PVM_OTHER_AMOUNT;
                                        finPaymentVndPoMpgObj.PPO_TAX_AMOUNT = dtl.PVM_TAX_AMOUNT;

                                        finPaymentVndTaxDtlObj = CommonFunctions.Initilize<FIN_PAYMENT_VND_TAX_DTL>();
                                        finPaymentVndTaxDtlObj.PDT_PK = 0;
                                        finPaymentVndPoMpgObj.FIN_PAYMENT_VND_TAX_DTL.Add(finPaymentVndTaxDtlObj);

                                        dtl.FIN_PAYMENT_VND_PO_MPG.Add(finPaymentVndPoMpgObj);
                                        finPaymentVndHdrObj.FIN_PAYMENT_VND_PO_MPG.Add(finPaymentVndPoMpgObj);
                                        finPaymentVndPoMpgList = dtl.FIN_PAYMENT_VND_PO_MPG.ToList();

                                    }
                                }
                                finPaymentVndHdrObj.FIN_PAYMENT_VND_TRX_MPG.Add(dtl);
                                // finPaymentVndHdrObj.FIN_PAYMENT_VND_PO_MPG.Add(
                            }
                        }

                        #region Payment mode details
                        PdcFlag = false;
                        if (PaymentModeDetailsList == null || PaymentModeDetailsList.Count == 0 || (PaymentModeDetailsList.Count == 1 && !IsPaymentModeAdded))
                        {
                            AddPaymentModes(false);
                            //IsPaymentModeAdded = false;    
                        }
                        if (PaymentModeDetailsList != null && PaymentModeDetailsList.Count > 0)
                        {

                            FIN_PAYMENT_VND_MODE_DTL PaymentModeDtl;
                            List<FIN_PAYMENT_VND_MODE_DTL> PaymentModeItemList = new List<FIN_PAYMENT_VND_MODE_DTL>();
                            foreach (FIN_PAYMENT_VND_MODE_DTL objItem in PaymentModeDetailsList)
                            {
                                PaymentModeDtl = CommonFunctions.Initilize<FIN_PAYMENT_VND_MODE_DTL>();
                                PaymentModeDtl.PDM_MODE = objItem.PDM_MODE;
                                PaymentModeDtl.PDM_BANK = objItem.PDM_BANK;
                                PaymentModeDtl.PDM_PK = objItem.PDM_PK;
                                PaymentModeDtl.PDM_PAYMENT_HDR = objItem.PDM_PAYMENT_HDR;
                                PaymentModeDtl.PDM_BRANCH = objItem.PDM_BRANCH;
                                PaymentModeDtl.PDM_INSTR_NO = objItem.PDM_INSTR_NO;
                                PaymentModeDtl.PDM_INSTR_DATE = objItem.PDM_INSTR_DATE;
                                PaymentModeDtl.PDM_INSTR_FAVOUR = objItem.PDM_INSTR_FAVOUR;
                                PaymentModeDtl.PDM_PDC = objItem.PDM_PDC;
                                PaymentModeDtl.PDM_BANK_CHARGE_CURR = objItem.PDM_BANK_CHARGE_CURR;
                                PaymentModeDtl.PDM_BANK_CHARGE = objItem.PDM_BANK_CHARGE;
                                PaymentModeDtl.PDM_BANK_CHARGE_TYPE = objItem.PDM_BANK_CHARGE_TYPE;
                                PaymentModeDtl.PDM_EXCHG_RATE = objItem.PDM_EXCHG_RATE;
                                PaymentModeDtl.PDM_PAID_AMOUNT = objItem.PDM_PAID_AMOUNT;
                                PaymentModeDtl.PDM_PAID_AMOUNT_BC = objItem.PDM_PAID_AMOUNT_BC;
                                PaymentModeDtl.PDM_ACCOUNT = objItem.PDM_ACCOUNT;
                                PaymentModeItemList.Add(PaymentModeDtl);

                                if (objItem.PDM_PDC > 0)
                                    PdcFlag = true;
                            }
                            PaymentModeItemList.ForEach(PymntModDtl => finPaymentVndHdrObj.FIN_PAYMENT_VND_MODE_DTL.Add(PymntModDtl));
                            PaidAmountBC = PaymentModeItemList.Sum(r => r.PDM_PAID_AMOUNT_BC);


                        }
                        if (PdcFlag)
                            finPaymentVndHdrObj.PVH_PDC = (byte)1;

                        #endregion

                        finPaymentVndHdrObj.PVH_PAID_AMOUNT_BC = PaidAmountBC;
                        ////if (finPaymentVndPoMpgList != null && finPaymentVndPoMpgList.Count > 0)
                        ////{
                        ////    //finReceiptCusHdrObj.FIN_RECEIPT_CUS_SO_MPG = new System.Data.Objects.DataClasses.EntityCollection<FIN_RECEIPT_CUS_SO_MPG>();
                        ////    //finReceiptCusHdrObj.FIN_RECEIPT_CUS_TRX_MPG = new System.Data.Objects.DataClasses.EntityCollection<FIN_RECEIPT_CUS_TRX_MPG>(); 
                        ////    finPaymentVndPoMpgList.ForEach(dtl =>
                        ////    {
                        ////        //finReceiptCusHdrObj.FIN_RECEIPT_CUS_SO_MPG.Add(dtl);
                        ////        // dtl.FIN_RECEIPT_CUS_TAX_DTL = new System.Data.Objects.DataClasses.EntityCollection<FIN_RECEIPT_CUS_TAX_DTL>();
                        ////        finPaymentVndTaxDtlObj = CommonFunctions.Initilize<FIN_PAYMENT_VND_TAX_DTL>();
                        ////        finPaymentVndTaxDtlObj.PDT_PK = 0;
                        ////        if (dtl.FIN_PAYMENT_VND_TAX_DTL.Count < 1)
                        ////            dtl.FIN_PAYMENT_VND_TAX_DTL.Add(finPaymentVndTaxDtlObj);// finReceiptCusTaxDtlList.Add(finReceiptCusTaxDtlObj);
                        ////    });
                        ////}
                        retObject = finPaymentVndHdrObj;
                        break;
                    #endregion
                    #region Payment Split
                    case ControlsEnum.PAYMENTSPLITLIST:
                        rowID = 0;
                        finPaymentVndPoMpgList = new List<FIN_PAYMENT_VND_PO_MPG>();
                        foreach (GridViewRow grdrow in grdPaymentSplit.Rows)//
                        {
                            finPaymentVndPoMpgObj = CommonFunctions.Initilize<FIN_PAYMENT_VND_PO_MPG>();
                            hdfPaymentSplitPK = (HiddenField)grdPaymentSplit.Rows[rowID].FindControl("hdfPaymentSplitPK");

                            finPaymentVndPoMpgObj.PPO_PK = hdfPaymentSplitPK == null ? 0 : hdfPaymentSplitPK.Value == "" ? 0 : Convert.ToInt64(hdfPaymentSplitPK.Value);
                            finPaymentVndPoMpgObj.PPO_PAYMENT_HDR = CurrPK;
                            finPaymentVndPoMpgObj.PPO_PAYMENT_TRX_MPG = PaymentMpgPK;
                            hdfPOPK = (HiddenField)grdPaymentSplit.Rows[rowID].FindControl("hdfPOPK");
                            if (!IsWorkOrder)
                            {
                                finPaymentVndPoMpgObj.PPO_PO_HDR = hdfPOPK == null ? 1 : hdfPOPK.Value == "" ? 1 : Convert.ToInt32(hdfPOPK.Value);
                            }
                            else if (IsWorkOrder)
                            {
                                finPaymentVndPoMpgObj.PPO_WO_HDR = hdfPOPK == null ? 1 : hdfPOPK.Value == "" ? 1 : Convert.ToInt32(hdfPOPK.Value);
                            }

                            txtPayNowSplit = (TextBox)grdPaymentSplit.Rows[rowID].FindControl("txtPayNowSplit");

                            lblOtherChargesSplit = (Label)grdPaymentSplit.Rows[rowID].FindControl("lblOtherChargesSplit");

                            // juno
                            //hdfTaxSplit = (HiddenField)grdPaymentSplit.Rows[rowID].FindControl("hdfTaxSplit");
                            //finPaymentVndPoMpgObj.PPO_TAX_AMOUNT = hdfTaxSplit == null ? 0 : hdfTaxSplit.Value.Trim() == string.Empty ? 0 : Convert.ToDecimal(hdfTaxSplit.Value.Trim());
                            TextBox txtTaxSplit = (TextBox)grdPaymentSplit.Rows[rowID].FindControl("txtTaxSplit");
                            finPaymentVndPoMpgObj.PPO_TAX_AMOUNT = string.IsNullOrEmpty(txtTaxSplit.Text) ? 0 : Convert.ToDecimal(txtTaxSplit.Text.Trim());

                            finPaymentVndPoMpgObj.PPO_PAID_AMOUNT = txtPayNowSplit == null ? 0 : txtPayNowSplit.Text.Trim() == string.Empty ? 0 : Convert.ToDecimal(txtPayNowSplit.Text.Trim());

                            finPaymentVndPoMpgObj.PPO_OTHER_AMOUNT = lblOtherChargesSplit == null ? 0 : lblOtherChargesSplit.Text.Replace(",", "").Trim() == string.Empty ? 0 : Convert.ToDecimal(lblOtherChargesSplit.Text.Replace(",", "").Trim());


                            finPaymentVndPoMpgObj.PPO_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                            finPaymentVndPoMpgList.Add(finPaymentVndPoMpgObj);
                            rowID++;
                        }
                        retObject = finPaymentVndPoMpgList;
                        break;
                    #endregion

                    #region ALERT
                    case ControlsEnum.ALERTSAVE:
                        string Typename = Resources.Constants.SystemAlertType;
                        int AlertPk = 0;
                        alertBoObj = new AlertBO();
                        appType = POGroup == POInvoiceGroup.Goods ? ApplicationType.PI
                            : POGroup == POInvoiceGroup.Services ? ApplicationType.PSI : ApplicationType.EI;
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
                        alertBoObj.ATH_TRX_DATE = txtPaymentDate.Text.Trim() == string.Empty ? DateTime.Now : Convert.ToDateTime(txtPaymentDate.Text.Trim());
                        string duedays = (Math.Floor((DateTime.Now - alertBoObj.ATH_TRX_DATE).TotalDays)).ToString();
                        alertBoObj.ATH_DUE_DAYS = Convert.ToInt16(duedays);
                        alertBoObj.ATH_DUE_DATE = DateTime.Now;
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
                        if (!string.IsNullOrEmpty(TypeRef) && !TypeRef.Equals("[New]".ToLower()))
                        {
                            alertBoObj.ATH_NARRATION = TypeRef.Trim();
                            //if (!string.IsNullOrEmpty(TypePartyName))
                            //{
                            //    alertBoObj.ATH_NARRATION = !string.IsNullOrEmpty(alertBoObj.ATH_NARRATION) ? alertBoObj.ATH_NARRATION + " - " + TypePartyName :
                            //        TypePartyName;
                            //}
                        }
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
                    #region PURINVNOS
                    case ControlsEnum.PURINVNOS:
                        PurInvoices = new List<FIN_INVOICE_VND_HDR>();
                        foreach (GridViewRow grdrow in grdInvoiceList.Rows)
                        {
                            FIN_INVOICE_VND_HDR finInvVndHdrObj = new FIN_INVOICE_VND_HDR();
                            Label lblInvoiceNo = (Label)grdrow.FindControl("lblInvoiceNo");
                            HiddenField hdfInvPK = (HiddenField)grdrow.FindControl("hdfInvoicePK");
                            finInvVndHdrObj.IVH_NO = lblInvoiceNo.Text;
                            finInvVndHdrObj.IVH_PK = Convert.ToInt64(hdfInvPK.Value);
                            PurInvoices.Add(finInvVndHdrObj);
                        }
                        break;
                    #endregion

                    #region ADJN SPLIT LIST
                    case ControlsEnum.ADJNSPLITLIST:
                        rowID = 0;
                        HiddenField hdfReceiptTRXAdjnPK;
                        HiddenField hdfCrDrPK;
                        HiddenField hdfReceiptAdjnPK;
                        HiddenField hdfAdjnPK;

                        TextBox txtAllocateAdjn;
                        FinPaymentVndAllocationList = new List<FIN_PAYMENT_VND_ALCN_DTL>();
                        foreach (GridViewRow grdrow in grdPaymentSplitAdjn.Rows)//
                        {
                            hdfReceiptTRXAdjnPK = (HiddenField)grdPaymentSplitAdjn.Rows[rowID].FindControl("hdfReceiptTRXAdjnPK");
                            hdfCrDrPK = (HiddenField)grdPaymentSplitAdjn.Rows[rowID].FindControl("hdfCrDrPK");
                            hdfAdjnPK = (HiddenField)grdPaymentSplitAdjn.Rows[rowID].FindControl("hdfAdjnPK");
                            hdfReceiptAdjnPK = (HiddenField)grdPaymentSplitAdjn.Rows[rowID].FindControl("hdfReceiptAdjnPK");
                            //hdfOtherChargesSplit = (HiddenField)grdReceiptSplit.Rows[rowID].FindControl("hdfOtherChargesSplit");
                            txtAllocateAdjn = (TextBox)grdPaymentSplitAdjn.Rows[rowID].FindControl("txtAllocateAdjn");
                            FinPaymentVndAllocationObj = CommonFunctions.Initilize<FIN_PAYMENT_VND_ALCN_DTL>();
                            //hdfReceiptSplitPK = (HiddenField)grdReceiptSplit.Rows[rowID].FindControl("hdfReceiptSplitPK");
                            FinPaymentVndAllocationObj.PAD_PK = string.IsNullOrEmpty(hdfAdjnPK.Value) ? 0 : Convert.ToInt64(hdfAdjnPK.Value);
                            FinPaymentVndAllocationObj.PAD_PAYMENT_TRX = Convert.ToInt32(hdfReceiptTRXAdjnPK.Value);
                            FinPaymentVndAllocationObj.PAD_ALCN_CDH = string.IsNullOrEmpty(hdfCrDrPK.Value) ? 0 : Convert.ToInt32(hdfCrDrPK.Value);
                            FinPaymentVndAllocationObj.PAD_ALCN_PAYMENT_TRX = string.IsNullOrEmpty(hdfReceiptAdjnPK.Value) ? 0 : Convert.ToInt32(hdfReceiptAdjnPK.Value);
                            FinPaymentVndAllocationObj.PAD_AMOUNT = txtAllocateAdjn == null ? 0 : txtAllocateAdjn.Text.Trim() == string.Empty ? 0 : Convert.ToDecimal(txtAllocateAdjn.Text.Trim());
                            FinPaymentVndAllocationObj.PAD_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                            FinPaymentVndAllocationList.Add(FinPaymentVndAllocationObj);
                            rowID++;
                        }
                        retObject = FinPaymentVndAllocationList;
                        break;
                    #endregion

                    #region CR/DR Split
                    case ControlsEnum.CRDRSPLITLIST:
                        FinPaymentVndCrdrMpgList = new List<PaymentCrdrMpg>();
                        foreach (GridViewRow grdrow in grdCrdrAllocation.Rows)
                        {
                            PaymentCrdrMpg finPaymentVndCrdrMpgObj = new PaymentCrdrMpg();
                            TextBox txtCrdrPayNow = (TextBox)grdrow.FindControl("txtCrdrPayNow");
                            TextBox txtCrdrAdjAmount = (TextBox)grdrow.FindControl("txtCrdrAdjAmount");
                            HiddenField hdfCrdrMpgPk = (HiddenField)grdrow.FindControl("hdfCrdrMpgPk");
                            decimal CrdrPayNow = 0;
                            decimal CrdrAdjAmnt = 0;
                            decimal.TryParse(txtCrdrPayNow.Text, out CrdrPayNow);
                            decimal.TryParse(txtCrdrAdjAmount.Text, out CrdrAdjAmnt);
                            finPaymentVndCrdrMpgObj.PNM_PAID_AMOUNT = CrdrPayNow;
                            finPaymentVndCrdrMpgObj.PNM_ADJ_AMOUNT = CrdrAdjAmnt;
                            finPaymentVndCrdrMpgObj.PNM_CRDR_MPG = string.IsNullOrEmpty(hdfCrdrMpgPk.Value) ? (long?)null : Convert.ToInt64(hdfCrdrMpgPk.Value);
                            FinPaymentVndCrdrMpgList.Add(finPaymentVndCrdrMpgObj);
                        }
                        retObject = FinPaymentVndCrdrMpgList;
                        break;
                    #endregion

                    #region INVOICE LIST
                    case ControlsEnum.INVOICELIST:
                        List<PaymentInvoiceDetails> PymntInvDetLst = new List<PaymentInvoiceDetails>();
                        foreach (GridViewRow grdrow in grdInvoiceList.Rows)
                        {
                            decimal adjAmnt = 0;
                            decimal balToPay = 0;
                            decimal crdrAmount = 0;
                            decimal taxAmount = 0;
                            decimal reductionAmount = 0;
                            decimal payNow = 0;
                            decimal otherCharge = 0;
                            long InvPk = 0;
                            Label lblAdjAmount = (Label)grdrow.FindControl("lblAdjAmount");
                            Label lblBaltopay = (Label)grdrow.FindControl("lblBaltopay");
                            Label lblCrdrAlcnAmount = (Label)grdrow.FindControl("lblCrdrAlcnAmount");
                            TextBox txtReduction = (TextBox)grdrow.FindControl("txtAdjustments");
                            TextBox txtPayNow = (TextBox)grdrow.FindControl("txtPayNow");
                            TextBox txtOtherCharge = (TextBox)grdrow.FindControl("txtOtherCharges");
                            HiddenField hdfTotalTax = (HiddenField)grdrow.FindControl("hdfTotalTax");
                            HiddenField hdfInvPK = (HiddenField)grdrow.FindControl("hdfInvoicePK");
                            decimal.TryParse(lblAdjAmount.Text, out adjAmnt);
                            decimal.TryParse(lblBaltopay.Text, out balToPay);
                            decimal.TryParse(lblCrdrAlcnAmount.Text, out crdrAmount);
                            decimal.TryParse(txtReduction.Text, out reductionAmount);
                            decimal.TryParse(txtPayNow.Text, out payNow);
                            decimal.TryParse(txtOtherCharge.Text, out otherCharge);
                            decimal.TryParse(hdfTotalTax.Value, out taxAmount);
                            long.TryParse(hdfInvPK.Value, out InvPk);
                            PaymentInvoiceDetails PymntInvDetObj = new PaymentInvoiceDetails();
                            PymntInvDetObj.PVM_ADJ_AMOUNT = adjAmnt;
                            PymntInvDetObj.PVM_BALANCE_TO_PAY = balToPay;
                            PymntInvDetObj.PVM_CRDR_AMOUNT = crdrAmount;
                            PymntInvDetObj.PVM_INVOICE_HDR = InvPk;
                            PymntInvDetObj.PVM_OTHER_CHARGE = otherCharge;
                            PymntInvDetObj.PVM_PAYNOW_AMOUNT = payNow;
                            PymntInvDetObj.PVM_REDUCTION_AMOUNT = reductionAmount;
                            PymntInvDetObj.PVM_TAX_AMOUNT = taxAmount;
                            PymntInvDetLst.Add(PymntInvDetObj);

                        }
                        PaymentInvDetList = PymntInvDetLst;
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
                    case ControlsEnum.PAYMENTGET:
                        if (finPaymentVndHdrList != null && finPaymentVndHdrList.Count > 0)
                        {
                            CurrPK = Convert.ToInt32(Request.QueryString["PK"].ToString());
                            bool posted;
                            FileDetailsList = null;
                            DocAttachList = null;
                            IsPaymentModeAdded = false;

                            if (Convert.ToInt16(finPaymentVndHdrList[0].PVH_DEL_STATUS.ToString()) == 1)
                            {
                                btnSavePmnt.Visible = false;
                                hdfIsCancelled.Value = "1";
                            }
                            else
                            {
                                btnSavePmnt.Visible = true;
                                hdfIsCancelled.Value = "0";
                            }

                            if (!string.IsNullOrEmpty(finPaymentVndHdrList[0].PVH_DEPT.ToString()) && int.TryParse(finPaymentVndHdrList[0].PVH_DEPT.ToString(), out dept))
                            {
                                Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                                base.SetUserDept();
                            }

                            hdfShowPDC.Value = "0";
                            posted = Convert.ToBoolean(finPaymentVndHdrList[0].PVH_HAS_JRNL_ENTRY.ToString());
                            if (posted)
                            {
                                int pdc = 0;
                                int payMode = 0;
                                if (finPaymentVndHdrList[0].PVH_PDC.ToString() != null && int.TryParse(finPaymentVndHdrList[0].PVH_PDC.ToString(), out pdc))
                                    hdfShowPDC.Value = pdc.ToString();
                                else
                                    hdfShowPDC.Value = "0";

                                if (finPaymentVndHdrList[0].PVH_MODE.ToString() != null && int.TryParse(finPaymentVndHdrList[0].PVH_MODE.ToString(), out payMode))
                                {
                                    if (payMode == (int)BusinessObject.CommonManagement.PaymentModeEnum.Cheque)
                                    {
                                        if (finPaymentVndHdrList[0].PVH_PDC.ToString() != null && int.TryParse(finPaymentVndHdrList[0].PVH_PDC.ToString(), out pdc))
                                            hdfShowChequeReturn.Value = pdc.ToString();
                                        else
                                            hdfShowChequeReturn.Value = "1";
                                    }
                                    else
                                    {
                                        hdfShowChequeReturn.Value = "1";
                                    }
                                }
                                else
                                {
                                    hdfShowChequeReturn.Value = "1";
                                }
                            }

                            hdfEdit.Value = "1";

                            Session[ERP.Utilities.SessionStrings.VendorPK] = finPaymentVndHdrList[0].PVH_VENDOR;
                            Session[ERP.Utilities.SessionStrings.Vendor] = finPaymentVndHdrList[0].PUR_VENDOR_MST.VEN_NAME;
                            Approved = Convert.ToInt32(finPaymentVndHdrList[0].PVH_STATUS);
                            // Get PaymentDetails
                            GetFieldValues(ControlsEnum.PAYMENTHDRENTRYBYPK);
                            GetUIValuesFromObject(ControlsEnum.PAYMENTHDRENTRY);
                            GetFieldValues(ControlsEnum.PAYMENTHDRINVLISTBYPK);
                            SetFieldValues(ControlsEnum.PAYMENTMPGLIST);

                            GetFieldValues(ControlsEnum.UPLOADEDFILES);
                            SetFieldValues(ControlsEnum.UPLOADEDFILES);
                            EntryStatus = EntryStatus.VIEWMODE;
                            WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                            base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID((int)CurrPK, ucrWrkf.ProcessID);
                            SetCancelRef((int)CurrPK);
                            ucrWrkf.FillWorkFlowDetails();
                            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                            {
                                ucrWrkf.ViewType = 1;
                            }
                            else
                            {
                                ucrWrkf.ViewType = 0;
                            }
                            btnPrint.Visible = true;
                            ucrWrkf.ViewAction();
                            GetFieldValues(ControlsEnum.BASECURRENCY);
                            lblTotalAmountBC.Text = string.Format(lblTotalAmountBC.Text, hdfBaseCurrency.Value.Split('-')[0].Trim());
                            //for adj allocation;need all saved val's in ReceiptAdjnList
                            //foreach (GridViewRow gvrw in grdInvoiceList.Rows)
                            //{
                            //    HiddenField hdfReceiptMpgPK = gvrw.FindControl("hdfPaymentMpgPK") as HiddenField;
                            //    long mpgpk = 0;
                            //    long.TryParse(hdfReceiptMpgPK.Value, out mpgpk);
                            //    PaymentMpgPK = mpgpk;// Convert.ToInt64(hdfReceiptMpgPK.Value);
                            //    HiddenField hdfInvoicePK = gvrw.FindControl("hdfInvoicePK") as HiddenField;
                            //    InvoicePK = Convert.ToInt32(hdfInvoicePK.Value);
                            //    GetFieldValues(ControlsEnum.PAYMENTADJNLIST);
                            //}
                            GetFieldValues(ControlsEnum.CRDRALLOCATION);
                            SetFieldValues(ControlsEnum.PAYMENTMODESGRID);
                            btnCancel.Focus();
                        }
                        break;
                    #region Payment Header
                    case ControlsEnum.PAYMENTHDRENTRY:
                        if (finPaymentVndHdrList != null && finPaymentVndHdrList.Count > 0)
                        {
                            InvoicePOSplitList = finPaymentVndHdrList[0].FIN_PAYMENT_VND_PO_MPG.ToList();
                            PaymentModeDetailsList = finPaymentVndHdrList[0].FIN_PAYMENT_VND_MODE_DTL.ToList();

                            //WHTTaxDetails = finPaymentVndHdrList[0].FIN_PAYMENT_VND_TAX_HDR.ToList();
                            //finPayemtVndHdrList = finPaymentVndHdrList[0].FIN_PAYMENT_VND_TAX_HDR.ToList();
                            //TempWHTTaxDetails = finPayemtVndHdrList;
                            WHTTaxDetails = finPaymentVndHdrList[0].FIN_PAYMENT_VND_TAX_HDR.Where(whtpynt => whtpynt.WTH_CATEGORY == (byte)WHTCategoryEnum.WHT).ToList();
                            VATTaxDetails = finPaymentVndHdrList[0].FIN_PAYMENT_VND_TAX_HDR.Where(whtpynt => whtpynt.WTH_CATEGORY == (byte)WHTCategoryEnum.VATBUY).ToList();
                            finPayemtVndHdrList = finPaymentVndHdrList[0].FIN_PAYMENT_VND_TAX_HDR.ToList();
                            TempWHTTaxDetails = finPayemtVndHdrList.Where(whtpynt => whtpynt.WTH_CATEGORY == (byte)WHTCategoryEnum.WHT).ToList();
                            TempVATTaxDetails = finPayemtVndHdrList.Where(whtpynt => whtpynt.WTH_CATEGORY == (byte)WHTCategoryEnum.VATBUY).ToList();

                            lblPaymentNo.Text = finPaymentVndHdrList[0].PVH_NO == "" ? "[NEW]" : finPaymentVndHdrList[0].PVH_NO;
                            POGroup = (POInvoiceGroup)Enum.Parse(typeof(POInvoiceGroup), finPaymentVndHdrList[0].PVH_GROUP.ToString());
                            PICategory = (POInvoiceCategory)Enum.Parse(typeof(POInvoiceCategory), finPaymentVndHdrList[0].PVH_CATEGORY.ToString());
                            txtPaymentDate.Text = finPaymentVndHdrList[0].PVH_DATE.ToString(Resources.Constants.DateFormatShort);
                            //ddlMode.SelectedValue = finPaymentVndHdrList[0].PVH_MODE.ToString();
                            //txtPaymentBank.Text = (finPaymentVndHdrList[0].FIN_CASH_BANK_MST == null) ? string.Empty : finPaymentVndHdrList[0].FIN_CASH_BANK_MST.CBM_CODE + " - " + finPaymentVndHdrList[0].FIN_CASH_BANK_MST.CBM_NAME;// poPaymentList[0].PVH_BANK_TEXT;
                            //hdfPaymentBank.Value = (finPaymentVndHdrList[0].PVH_BANK.HasValue) ? finPaymentVndHdrList[0].PVH_BANK.ToString() : string.Empty;

                            //txtPaymentCurrency.Text = finPaymentVndHdrList[0].ADM_CURRENCY_MST.CUR_CODE;
                            //hdfPaymentCurrency.Value = finPaymentVndHdrList[0].PVH_CURRENCY.ToString();
                            txtPaymentCurrency.Text = finPaymentVndHdrList[0].ADM_CURRENCY_MST2.CUR_CODE;
                            lblPaymentAmount.Text = GetLocalResourceObject("PaymentAmount") + "(" + finPaymentVndHdrList[0].ADM_CURRENCY_MST2.CUR_CODE + ")";
                            hdfPaymentCurrency.Value = finPaymentVndHdrList[0].PVH_CURRENCY.ToString();
                            txtHdrExchangeRate.Text = finPaymentVndHdrList[0].PVH_EXCHG_RATE.ToString();

                            ResetForm(ControlsEnum.PAYMENTMODES);
                            if (PaymentModeDetailsList != null && PaymentModeDetailsList.Count > 1)
                            {
                                IsPaymentModeAdded = true;
                                SetPymntModeHdrValidation(false);
                                SetFieldValues(ControlsEnum.PAYMENTMODESGRID);
                                EnableDisableExchangeRate(false);
                            }
                            else
                            {
                                EnableDisableExchangeRate(true);
                            }

                            //GetFieldValues(ControlsEnum.EXCHANGERATE);
                            //string Mode = finPaymentVndHdrList[0].PVH_MODE.ToString();
                            //string pdc = finPaymentVndHdrList[0].PVH_PDC.ToString();
                            short pdc = 0;
                            List<FIN_PAYMENT_VND_MODE_DTL> paymentModeLst = finPaymentVndHdrList[0].FIN_PAYMENT_VND_MODE_DTL.ToList();
                            List<byte> Mode = new List<byte>();
                            if (paymentModeLst != null && paymentModeLst.Count > 0)
                            {
                                Mode = paymentModeLst.Select(r => r.PDM_MODE).Distinct().ToList();
                                if (paymentModeLst.Where(r => r.PDM_PDC >= 1).Count() > 0)
                                {
                                    pdc = paymentModeLst.Where(r => r.PDM_PDC >= 1).ToList()[0].PDM_PDC;
                                }
                            }

                            string isposted = finPaymentVndHdrList[0].PVH_HAS_JRNL_ENTRY.ToString();
                            //When coming from inbox managing return button,Otherwise its in editmode,and itemselect
                            hdfShowPDC.Value = "0";

                            if (isposted != null)
                            {
                                if (Convert.ToBoolean(isposted) == true)
                                {

                                    if (pdc != null)
                                        hdfShowPDC.Value = pdc.ToString();
                                    else
                                        hdfShowPDC.Value = "0";

                                    if (Mode != null)
                                    {
                                        if (Mode.Contains((int)BusinessObject.CommonManagement.PaymentModeEnum.Cheque))// if (Convert.ToInt16(Mode) == (int)BusinessObject.CommonManagement.PaymentModeEnum.Cheque)
                                        {
                                            if (pdc != null)
                                                hdfShowChequeReturn.Value = pdc.ToString();
                                            else
                                                hdfShowChequeReturn.Value = "1";
                                        }
                                        else
                                        {
                                            hdfShowChequeReturn.Value = "1";
                                        }
                                    }
                                    else
                                    {
                                        hdfShowChequeReturn.Value = "1";
                                    }
                                }
                            }


                            //int pdc = 0;
                            //pdc = finPaymentVndHdrList[0].PVH_MODE;
                            //GetFieldValues(ControlsEnum.FINHEADERSTATUS);
                            //if (finTrxHdrList != null && finTrxHdrList.Count == 1)
                            //{
                            //    if (pdc != null)
                            //        if (finTrxHdrList[0].FTH_STATUS == 2)
                            //        {
                            //            hdfShowChequeReturn.Value = pdc.ToString();
                            //        }
                            //        else
                            //            hdfShowChequeReturn.Value = "1";
                            //    else
                            //        hdfShowChequeReturn.Value = "1";
                            //}

                            GetFieldValues(ControlsEnum.EXCHANGERATEBANK);
                            if (ddlBankChargeCurrency.Items[0].Value != finPaymentVndHdrList[0].PVH_CURRENCY.ToString())
                            {
                                ddlBankChargeCurrency.Items.Insert(1, (new ListItem(finPaymentVndHdrList[0].ADM_CURRENCY_MST2.CUR_CODE + " - " + finPaymentVndHdrList[0].ADM_CURRENCY_MST2.CUR_NAME, finPaymentVndHdrList[0].PVH_CURRENCY.ToString())));
                                //ddlBankChargeCurrency.Items.Insert(1, (new ListItem(finPaymentVndHdrList[0].ADM_CURRENCY_MST2.CUR_CODE , finPaymentVndHdrList[0].PVH_CURRENCY.ToString())));

                                ////try
                                ////{
                                ////    ddlBankChargeCurrency.SelectedValue = finPaymentVndHdrList[0].PVH_BANK_CHARGE_CURR.ToString();
                                ////}
                                ////catch { }
                            }
                            GetFieldValues(ControlsEnum.BANKCURRENCYEXCHANGERATE);

                            //txtBankCharge.Text = Math.Round(finPaymentVndHdrList[0].PVH_BANK_CHARGE, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            //chkBankCharge.Checked = finPaymentVndHdrList[0].PVH_BANK_CHARGE_TYPE;
                            ddlCompany.SelectedValue = finPaymentVndHdrList[0].PVH_COMPANY.ToString();
                            //if (!string.IsNullOrEmpty(hdfPaymentBank.Value) && Convert.ToInt32(hdfPaymentBank.Value.ToString()) > 0)
                            //{
                            //    GetFieldValues(ControlsEnum.BANK);
                            //    if (finCashBankMstList != null && finCashBankMstList.Count > 0)
                            //    {
                            //        if (Convert.ToInt32(ddlMode.SelectedValue) != (int)PaymentModeEnum.CASH)
                            //        {
                            //            txtAccountNo.Text = finCashBankMstList[0].CBM_ACC_NO;
                            //            txtBranch.Text = finCashBankMstList[0].CBM_BRANCH;
                            //        }
                            //        hdfBankAccount.Value = finCashBankMstList[0].CBM_ACCOUNT.ToString();
                            //    }
                            //    else
                            //    {
                            //        txtAccountNo.Text = string.Empty;
                            //        txtBranch.Text = string.Empty;
                            //        hdfBankAccount.Value = string.Empty;
                            //    }
                            //}
                            //txtInstrumentNo.Text = HttpUtility.HtmlDecode(finPaymentVndHdrList[0].PVH_INSTR_NO);
                            //txtInstrumentDate.Text = finPaymentVndHdrList[0].PVH_INSTR_DATE.HasValue == false ? "" :
                            //    finPaymentVndHdrList[0].PVH_INSTR_DATE.Value.ToString(Resources.Constants.DateFormatShort);
                            //txtFavourof.Text = HttpUtility.HtmlDecode(finPaymentVndHdrList[0].PVH_INSTR_FAVOUR);
                            txtRemarks.Text = HttpUtility.HtmlDecode(finPaymentVndHdrList[0].PVH_REMARKS);
                            LastModifiedTime = finPaymentVndHdrList[0].PVH_MOD_DT;
                            lblLastModifiedHDR.Text = Resources.Report.LastModifiedOn + LastModifiedTime.ToString(Resources.Report.LastModifiedDatetimeFormat);
                            Approved = WkfStatus = finPaymentVndHdrList[0].PVH_STATUS;
                            hdfDelStatus.Value = finPaymentVndHdrList[0].PVH_DEL_STATUS.ToString();
                            ddlAdjType.SelectedIndex = Convert.ToInt32(ddlAdjType.Items.IndexOf(ddlAdjType.Items.FindByValue(finPaymentVndHdrList[0].PVH_DISCOUNT.ToString())));
                            txtAdjAmount.Text = Math.Round(finPaymentVndHdrList[0].PVH_DISC_AMOUNT, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            txtTaxAmount.Text = Math.Round(finPaymentVndHdrList[0].PVH_TAX_AMOUNT, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            //chkPDC.Checked = finPaymentVndHdrList[0].PVH_PDC >= 1 ? true : false;
                            hdfWHTNO.Value = finPaymentVndHdrList[0].PVH_WHT_NO != null ? finPaymentVndHdrList[0].PVH_WHT_NO.ToString() : string.Empty;
                            txtCRTNo.Text = hdfWHTNO.Value;
                            //txtExchangeRate.Text = String.Format("{0:c}", finPaymentVndHdrList[0].PVH_EXCHG_RATE);
                            //txtExchangeRate.Text = String.Format("{0:c4}", finPaymentVndHdrList[0].PVH_EXCHG_RATE);
                            ////txtExchangeRate.Text = finPaymentVndHdrList[0].PVH_EXCHG_RATE.ToString();
                            //txtTotalAmountBC.Text = String.Format("{0:c}", finPaymentVndHdrList[0].PVH_PAID_AMOUNT_BC);

                            if (finPaymentVndHdrList[0].PUR_VENDOR_MST.VEN_WHT_TAX != null)
                            {
                                whtTaxpk = Convert.ToInt32(finPaymentVndHdrList[0].PUR_VENDOR_MST.VEN_WHT_TAX);
                                //ddlWHTAccount.SelectedIndex = Convert.ToInt32(ddlWHTAccount.Items.IndexOf(ddlWHTAccount.Items.FindByValue(finPaymentVndHdrList[0].PVH_WHT_TAX.ToString())));
                                //chkVendorforpayemnt.Checked = finPaymentVndHdrList[0].PVH_PAY_FOR_VENDOR;
                                txtWHTAmount.Text = Math.Round(finPaymentVndHdrList[0].PVH_WHT_AMOUNT == -1 ? 0 : finPaymentVndHdrList[0].PVH_WHT_AMOUNT, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                                //if (Convert.ToDecimal(txtWHTAmount.Text) > 0)
                                //{
                                //    imgbtnPrint.Visible = true;
                                //}
                                //else
                                //{
                                //    imgbtnPrint.Visible = false;
                                //}
                                trVendorAccount.Style.Add("display", "");
                                GetFieldValues(ControlsEnum.VENDORACCOUNTTAX);
                                SetFieldValues(ControlsEnum.VENDORACCOUNTTAX);
                            }
                            else
                            {
                                trVendorAccount.Style.Add("display", "none");
                            }
                            SetVendorPayment();
                            if (finPaymentVndHdrList[0].PVH_VENDOR_BANK.HasValue)
                            {
                                VendorBankPk = Convert.ToInt32(finPaymentVndHdrList[0].PVH_VENDOR_BANK);
                            }
                            GetFieldValues(ControlsEnum.VENDORBANKS);
                            SetFieldValues(ControlsEnum.VENDORBANKS);
                            if (VendorBankPk > 0)
                            {
                                try
                                {
                                    ddlvendorBank.SelectedValue = VendorBankPk.ToString();
                                }
                                catch
                                {
                                    ddlvendorBank.SelectedValue = CommonConstants.SELECTVAL.ToString();
                                }
                            }
                            else
                            {
                                //ddlvendorBank.SelectedValue = CommonConstants.SELECTVAL.ToString();
                            }

                            if (PaymentModeDetailsList != null && PaymentModeDetailsList.Count == 1)
                            {
                                PaymentModeRowIndex = 0;
                                SetUIEditViewPaymentModeDetails(0);
                            }
                            else
                            {
                                ResetForm(ControlsEnum.PAYMENTMODES);
                            }

                        }
                        break;
                    #endregion
                    #region PAYMENTSPLITLIST
                    case ControlsEnum.PAYMENTSPLITLIST:
                        if (finInvoiceVndHdrListForPaymentSplit != null && finInvoiceVndHdrListForPaymentSplit.Count > 0)
                        {
                            lblInvSplitNo.Text = lblInvSplitNo_CrdrAlcn.Text = ERP.Utilities.CommonFunctions.GetShortString(finInvoiceVndHdrListForPaymentSplit[0].IVH_NO, 13);
                            lblInvSplitNo.ToolTip = lblInvSplitNo_CrdrAlcn.ToolTip = finInvoiceVndHdrListForPaymentSplit[0].IVH_NO;
                            lblInvSplitDate.Text = lblInvSplitDate_CrdrAlcn.Text = ERP.Utilities.CommonFunctions.GetShortString(finInvoiceVndHdrListForPaymentSplit[0].IVH_DATE.ToString(Resources.Constants.DateFormatShort), 13);
                            lblInvSplitDate.ToolTip = lblInvSplitDate_CrdrAlcn.ToolTip = finInvoiceVndHdrListForPaymentSplit[0].IVH_DATE.ToString(Resources.Constants.DateFormatShort);
                            lblInvSplitSupplier.Text = lblInvSplitSupplier_CrdrAlcn.Text = ERP.Utilities.CommonFunctions.GetShortString(finInvoiceVndHdrListForPaymentSplit[0].PUR_VENDOR_MST.VEN_NAME, 10);
                            lblInvSplitSupplier.ToolTip = lblInvSplitSupplier_CrdrAlcn.ToolTip = finInvoiceVndHdrListForPaymentSplit[0].PUR_VENDOR_MST.VEN_NAME;
                            lblInvSplitAmount.ToolTip = lblInvSplitAmount.Text = String.Format("{0:c}", finInvoiceVndHdrListForPaymentSplit[0].IVH_AMOUNT_NET_TC);
                            if (finPaymentVndPoMpgList != null && finPaymentVndPoMpgList.Count > 0)
                            {
                                lblInvSplitReceived.ToolTip = lblInvSplitReceived.Text = finPaymentVndPoMpgList[0].PPO_BOUNCED == 0 ? String.Format("{0:c}", finInvoiceVndHdrListForPaymentSplit[0].IVH_AMOUNT_PAID_TC - (CurrPK > 0 ? PayNowAmount : 0)) :
                                    String.Format("{0:c}", finInvoiceVndHdrListForPaymentSplit[0].IVH_AMOUNT_PAID_TC);
                            }
                            else
                            {
                                lblInvSplitReceived.ToolTip = lblInvSplitReceived.Text = String.Format("{0:c}", finInvoiceVndHdrListForPaymentSplit[0].IVH_AMOUNT_PAID_TC - (CurrPK > 0 ? PayNowAmount : 0));
                            }
                            lblInvSplitReceiveNow.ToolTip = lblInvSplitReceiveNow.Text = String.Format("{0:c}", PayNowAmount);
                        }
                        break;
                    #endregion
                    #region TAXTYPECHANGED
                    case ControlsEnum.TAXTYPECHANGED:
                        if (!string.IsNullOrEmpty(ddlVATAccountPopup.SelectedValue))
                        {
                            if (Convert.ToInt32(ddlVATAccountPopup.SelectedValue) > 0)
                            {
                                TaxPK = Convert.ToInt32(ddlVATAccountPopup.SelectedValue);
                                hdfVATAccountPopup.Value = ddlVATAccountPopup.SelectedValue;
                                GetFieldValues(ControlsEnum.VATBUYTAXTYPES);
                                TaxPK = 0;
                                if (dtTaxDetails != null && dtTaxDetails.Rows.Count > 0)
                                {
                                    string taxFormula = dtTaxDetails.Rows[0][Resources.DataFieldRes.TaxFormula].ToString();
                                    hdfTaxformula.Value = taxFormula;
                                    taxFormula = taxFormula.Replace("#SUBTOTAL#", txtBeforeTaxAmount.Text.Trim());
                                    txtVATTaxAmountPopup.Text = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString(hdfCurrencyFormat.Value);
                                    //SelectedTaxText = HttpUtility.HtmlEncode(dtTaxDetails.Rows[0]["TAX_HEAD"].ToString());
                                    //txtPopupOther.Text = ddlVATAccountPopup.SelectedItem.Text;
                                    //txtVATTaxAmountPopup.Enabled = false;
                                    //txtPopupOther.Enabled = false;
                                }
                            }
                            else if (Convert.ToInt32(ddlVATAccountPopup.SelectedValue) == -1)
                            {
                                hdfVATAccountPopup.Value = string.Empty;
                                hdfTaxformula.Value = string.Empty;
                                txtVATTaxAmountPopup.Text = string.Empty;
                                //SelectedTaxText = Resources.Report.Custom;
                                // txtPopupOther.Text = string.Empty;
                                //txtPopupAmount.Enabled = true;
                                //txtPopupOther.Enabled = true;
                            }
                        }
                        break;
                    #endregion
                    #region VENDORCONTACTYPE
                    case ControlsEnum.VENDORCONTACTYPE:

                        if (dtAdsType != null && dtAdsType.Rows.Count > 0)
                        {
                            chkHeadOffice.Checked = false;
                            txtBranchCode.Text = dtAdsType.Rows[0][Resources.DataFieldRes.VncTypeName].ToString();
                            txtVatTaxId.Text = dtAdsType.Rows[0][Resources.DataFieldRes.VncTaxNo].ToString();
                            if (string.IsNullOrEmpty(txtVatTaxId.Text))
                                txtVatTaxId.Text = dtAdsType.Rows[0][Resources.DataFieldRes.VendorTaxId].ToString();
                            hdfAddressType.Value = dtAdsType.Rows[0][Resources.DataFieldRes.VncPk].ToString();
                            txtAddressType.Text = dtAdsType.Rows[0][Resources.DataFieldRes.VncName].ToString();
                            if (Convert.ToInt32(dtAdsType.Rows[0][Resources.DataFieldRes.vncType]) == (int)VendorContactTypeEnum.HeadOffice)
                            {
                                chkHeadOffice.Checked = true;
                                HeadofficeCheckedChanged();
                                if (string.IsNullOrEmpty(txtBranchCode.Text))
                                    txtBranchCode.Text = GetLocalResourceObject("DefaultCodeForHo").ToString();
                            }

                        }
                        break;
                    #endregion
                    #region VENDORCONTACTYPEDETAILS
                    case ControlsEnum.VENDORCONTACTYPEDETAILS:
                        if (dtAdsTypeDtl != null && dtAdsTypeDtl.Rows.Count > 0)
                        {
                            chkHeadOffice.Checked = false;
                            txtBranchCode.Text = dtAdsTypeDtl.Rows[0][Resources.DataFieldRes.VncTypeName].ToString();
                            txtVatTaxId.Text = dtAdsTypeDtl.Rows[0][Resources.DataFieldRes.VncTaxNo].ToString();
                            if (string.IsNullOrEmpty(txtVatTaxId.Text))
                                txtVatTaxId.Text = dtAdsTypeDtl.Rows[0][Resources.DataFieldRes.VendorTaxId].ToString();
                            if (Convert.ToInt32(dtAdsTypeDtl.Rows[0][Resources.DataFieldRes.vncType]) == (int)VendorContactTypeEnum.HeadOffice)
                            {
                                chkHeadOffice.Checked = true;
                                HeadofficeCheckedChanged();
                                if (string.IsNullOrEmpty(txtBranchCode.Text))
                                    txtBranchCode.Text = GetLocalResourceObject("DefaultCodeForHo").ToString();
                            }
                        }
                        break;
                    #endregion
                    #region VENDORSELECTEDDTL
                    case ControlsEnum.VENDORSELECTEDDTL:
                        if (dtVendorDtl != null && dtVendorDtl.Rows.Count > 0)
                        {
                            hdfVendorPopup.Value = dtVendorDtl.Rows[0][Resources.DataFieldRes.VendorPK].ToString();
                        }
                        break;
                    #endregion
                    #region VENDOR
                    case ControlsEnum.VENDOR:
                        if (purVendorMstList != null && purVendorMstList.Count > 0)
                        {
                            txtCustomerTxtWHT.Text = !string.IsNullOrEmpty(purVendorMstList[0].VEN_NAME2) ? purVendorMstList[0].VEN_NAME2 : ERP.Utilities.CommonFunctions.GetShortString(purVendorMstList[0].VEN_NAME, 300);
                            //txtCustomerTxtWHT.ToolTip = purVendorMstList[0].VEN_NAME2;
                            hdfvendorWHTPK.Value = purVendorMstList[0].VEN_PK.ToString();
                            txtpartyads.Text = !string.IsNullOrEmpty(purVendorMstList[0].VEN_ADDR3) ? purVendorMstList[0].VEN_ADDR3 : purVendorMstList[0].VEN_ADDR1 + " " + purVendorMstList[0].VEN_ADDR2;
                            txtTaxid.Text = purVendorMstList[0].VEN_TIN;
                        }
                        break;
                    #endregion
                    #region VATBUYVENDOR
                    case ControlsEnum.VATBUYVENDOR:
                        if (purVendorMstList != null && purVendorMstList.Count > 0)
                        {
                            txtVatTaxId.Text = purVendorMstList[0].VEN_TIN;
                            txtVendorPopup.Text = purVendorMstList[0].VEN_NAME;
                            hdfVendorPopup.Value = purVendorMstList[0].VEN_PK.ToString();
                            //txtAddressType.Text = purVendorMstList[0].VEN_WAREHOUSE_DTL.ToString();  
                            //ddlAddressType.SelectedItem.Text = purVendorMstList[0].VEN_WAREHOUSE_DTL.ToString(); 
                        }

                        break;
                    #endregion

                    #region VENDORACCOUNTTAX
                    case ControlsEnum.VENDORACCOUNTTAX:
                        if (dtVendorAccount != null && dtVendorAccount.Rows.Count > 0)
                        {
                            //if (chkVendorforpayemnt.Checked)
                            //{
                            decimal amount = 0;

                            amount = txtPaidAmount.Text != string.Empty ? Convert.ToDecimal(txtPaidAmount.Text) : 0;
                            //amount = amount + (txtWHTAmount.Text != string.Empty ? Convert.ToDecimal(txtWHTAmount.Text) : 0);
                            string taxFormula = dtVendorAccount.Rows[0]["TAX_FORMULA"].ToString();
                            hdfTaxformula.Value = taxFormula;
                            //taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());

                            //decimal amt = Convert.ToDecimal(StringToFormula(taxFormula));
                            //txtWHTAmount.Text = Math.Round(amt, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            //}
                            //else
                            //{
                            //    txtWHTAmount.Text = "0";
                            //}
                        }
                        else
                        {
                            txtWHTAmount.Text = "0";
                        }
                        break;
                    #endregion

                    #region SELECTED DOC
                    case ControlsEnum.SELECTEDDOC:
                        if (admDocAttachObj != null)
                        {
                            CurrSlNo = admDocAttachObj.DOC_SEQ_NO;
                            anchorFile.Visible = true;
                            vrfFileUpload.Enabled = false;
                            anchorFile.InnerHtml = admDocAttachObj.DOC_NAME;
                            anchorFile.HRef = admDocAttachObj.DOC_PATH;
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
                    #region VENDORCONTACTFORWHT
                    case ControlsEnum.VENDORCONTACTFORWHT:
                        if (dtAdsTypeDtl != null && dtAdsTypeDtl.Rows.Count > 0)
                        {
                            chkWthHeadOffice.Checked = false;
                            txtWthBranchCode.Text = dtAdsTypeDtl.Rows[0][Resources.DataFieldRes.VncTypeName].ToString();
                            txtTaxid.Text = dtAdsTypeDtl.Rows[0][Resources.DataFieldRes.VncTaxNo].ToString();
                            if (string.IsNullOrEmpty(txtTaxid.Text))
                                txtTaxid.Text = dtAdsTypeDtl.Rows[0][Resources.DataFieldRes.VendorTaxId].ToString();
                            if (Convert.ToInt32(dtAdsTypeDtl.Rows[0][Resources.DataFieldRes.vncType]) == (int)VendorContactTypeEnum.HeadOffice)
                            {
                                chkWthHeadOffice.Checked = true;
                                if (string.IsNullOrEmpty(txtWthBranchCode.Text))
                                    txtWthBranchCode.Text = GetLocalResourceObject("DefaultCodeForHo").ToString();
                            }
                        }
                        else
                        {
                            chkWthHeadOffice.Checked = false;
                            txtWthBranchCode.Text = string.Empty;
                        }
                        break;
                    #endregion

                    #region SPLITPAYNOWFORMULIPO
                    case ControlsEnum.SPLITPAYNOWFORMULIPO:
                        if (grdPaymentSplit.Rows.Count > 0 && InvoicePK > 0)
                        {
                            //GetFieldValues(ControlsEnum.INVOICEVNDMPGLIST);
                            GetFieldValues(ControlsEnum.INVOICEVNDMPGLISTFORAUTOALCN);
                            //HiddenField hdfFooterBalSplit = (HiddenField)grdPaymentSplit.FooterRow.FindControl("hdfTotalBalFooterSplit");
                            decimal InvoiceSubTotal = 0;
                            //decimal.TryParse(hdfFooterBalSplit.Value, out InvoiceSubTotal);
                            decimal PayNowSplit = 0;
                            decimal advDeductAmnt = 0;
                            decimal ExcessAmount = 0;
                            decimal InvTotalOtherCharge = 0;
                            decimal TotalOtherCharge = InvOtherCharge;
                            decimal InvAllocatedCNAmnt = 0;


                            if (FinInvoiceVndTrxMpgListForAutoAlcn != null && FinInvoiceVndTrxMpgListForAutoAlcn.Count > 0)
                            {
                                InvoiceSubTotal = FinInvoiceVndTrxMpgListForAutoAlcn.Sum(r => r.IVM_AMOUNT);
                                //InvoiceSubTotal = FinInvoiceVndTrxMpgListForAutoAlcn[0].FIN_INVOICE_VND_HDR.IVH_AMOUNT_NET_TC;
                                //if (FinInvoiceVndTrxMpgListForAutoAlcn[0].FIN_INVOICE_VND_HDR.IVH_CATEGORY == (byte)POInvoiceCategory.Invoice)
                                //{
                                //    //InvoiceSubTotal = FinInvoiceVndTrxMpgListForAutoAlcn[0].FIN_INVOICE_VND_HDR.IVH_AMOUNT_NET_TC;
                                //    InvoiceSubTotal = FinInvoiceVndTrxMpgListForAutoAlcn.Sum(r => r.IVM_AMOUNT);
                                //    //InvPkSplit = InvoicePK;
                                //    //PoPkSplit = null;
                                //    //GetFieldValues(ControlsEnum.ADVINVOICELIST);
                                //    //if (finAdvDeductList != null && finAdvDeductList.Count > 0)
                                //    //{
                                //    //    advDeductAmnt = finAdvDeductList.Sum(adv => adv.VAD_AMOUNT);
                                //    //    if ((InvoiceSubTotal - advDeductAmnt)>0)
                                //    //    InvoiceSubTotal = InvoiceSubTotal - advDeductAmnt;
                                //    //}
                                //}
                                if (PaymentCrdrList != null && PaymentCrdrList.Count > 0)
                                {
                                    long InvPk = FinInvoiceVndTrxMpgListForAutoAlcn[0].IVM_INVOICE_HDR;
                                    InvAllocatedCNAmnt = PaymentCrdrList.Where(r => r.PNM_INVOICE_HDR == InvPk).Sum(sm => sm.PNM_PAID_AMOUNT + sm.PNM_ADJ_AMOUNT);
                                    //InvoiceSubTotal = InvoiceSubTotal - InvAllocatedCNAmnt;
                                    PayNow = (PayNow - InvAllocatedCNAmnt) < 0 ? 0 : (PayNow - InvAllocatedCNAmnt);
                                }

                                if (InvoiceSubTotal > 0)
                                {
                                    foreach (GridViewRow grdRow in grdPaymentSplit.Rows)
                                    {
                                        HiddenField hdfPOPK = (HiddenField)grdRow.FindControl("hdfPOPK");
                                        Label lblBalanceSplit = (Label)grdRow.FindControl("lblBalanceSplit");
                                        decimal InvAmount = 0;
                                        decimal AdvInvAmount = 0;
                                        decimal PoBalanceAmount = 0;
                                        decimal InvOtherAmount = 0;
                                        decimal OtherAmount = 0;
                                        int PoPk = 0;
                                        int.TryParse(hdfPOPK.Value, out PoPk);
                                        decimal.TryParse(lblBalanceSplit.Text.Replace(",", "").Trim(), out PoBalanceAmount);
                                        if (!IsWorkOrder)
                                        {
                                            InvAmount = FinInvoiceVndTrxMpgListForAutoAlcn.Where(iv => iv.IVM_PO_HDR == PoPk).Sum(inv => inv.IVM_AMOUNT);
                                            InvOtherAmount = FinInvoiceVndTrxMpgListForAutoAlcn.Where(iv => iv.IVM_PO_HDR == PoPk).Sum(inv => inv.IVM_OTHER_AMOUNT);
                                            InvTotalOtherCharge = FinInvoiceVndTrxMpgListForAutoAlcn.Sum(inv => inv.IVM_OTHER_AMOUNT);
                                            if (FinInvoiceVndTrxMpgListForAutoAlcn[0].FIN_INVOICE_VND_HDR.IVH_CATEGORY == (byte)POInvoiceCategory.Invoice)
                                            {
                                                InvAmount = FinInvoiceVndTrxMpgListForAutoAlcn.Where(iv => iv.IVM_PO_HDR == PoPk).Sum(inv => inv.IVM_AMOUNT);
                                            }
                                        }
                                        else if (IsWorkOrder)
                                        {
                                            InvAmount = FinInvoiceVndTrxMpgListForAutoAlcn.Where(iv => iv.IVM_WO_HDR == PoPk).Sum(inv => inv.IVM_AMOUNT);
                                            InvOtherAmount = FinInvoiceVndTrxMpgListForAutoAlcn.Where(iv => iv.IVM_WO_HDR == PoPk).Sum(inv => inv.IVM_OTHER_AMOUNT);
                                            InvTotalOtherCharge = FinInvoiceVndTrxMpgListForAutoAlcn.Sum(inv => inv.IVM_OTHER_AMOUNT);
                                            if (FinInvoiceVndTrxMpgListForAutoAlcn[0].FIN_INVOICE_VND_HDR.IVH_CATEGORY == (byte)POInvoiceCategory.Invoice)
                                            {
                                                InvAmount = FinInvoiceVndTrxMpgListForAutoAlcn.Where(iv => iv.IVM_WO_HDR == PoPk).Sum(inv => inv.IVM_AMOUNT);
                                            }
                                        }

                                        //InvPkSplit = InvoicePK;
                                        //PoPkSplit = PoPk;
                                        //GetFieldValues(ControlsEnum.ADVINVOICELIST);
                                        //if (finAdvDeductList != null && finAdvDeductList.Count > 0)
                                        //{
                                        //    AdvInvAmount = finAdvDeductList.Sum(adv => adv.VAD_AMOUNT);
                                        //}
                                        //if ((InvAmount - AdvInvAmount) > 0)
                                        //{
                                        //    InvAmount = InvAmount - AdvInvAmount;
                                        //}
                                        TextBox txtPayNow = (TextBox)grdRow.FindControl("txtPayNowSplit");
                                        Label lblOtherChargesSplit = (Label)grdRow.FindControl("lblOtherChargesSplit");
                                        HiddenField hdfOtherChargesSplit = (HiddenField)grdRow.FindControl("hdfOtherChargesSplit");
                                        TextBox txtTaxSplit = (TextBox)grdRow.FindControl("txtTaxSplit");

                                        if (IsCreditNoteApplied || InvAllocatedCNAmnt > 0 || (string.IsNullOrEmpty(txtPayNow.Text) || Convert.ToDecimal(txtPayNow.Text) == 0))
                                        {
                                            //Label lblBalanceSplit = (Label)grdRow.FindControl("lblBalanceSplit");
                                            //PayNowSplit = Convert.ToDecimal(lblBalanceSplit.Text.Replace(",", "")) * (PayNow / InvoiceSubTotal);
                                            PayNowSplit = (InvAmount / InvoiceSubTotal) * PayNow;
                                            PayNowSplit = Math.Round(PayNowSplit, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                            if (PayNowSplit > PoBalanceAmount)
                                            {
                                                ExcessAmount += PayNowSplit - PoBalanceAmount;
                                                PayNowSplit = PoBalanceAmount;
                                            }
                                            txtPayNow.Text = Math.Round(PayNowSplit, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                                            if (InvTotalOtherCharge > 0)
                                            {
                                                OtherAmount = (InvOtherAmount / InvTotalOtherCharge) * TotalOtherCharge;
                                            }
                                            else
                                            {
                                                OtherAmount = 0;
                                            }
                                            lblOtherChargesSplit.Text = string.Format("{0:c}", Math.Round(OtherAmount, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits));
                                            hdfOtherChargesSplit.Value = OtherAmount.ToString();

                                            decimal TaxAmnt = 0;
                                            decimal TaxPer = 1;
                                            decimal.TryParse(hdfTaxPer.Value, out TaxPer);
                                            TaxAmnt = PayNowSplit * TaxPer;
                                            txtTaxSplit.Text = Math.Round(TaxAmnt, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                                        }
                                    }

                                    // Distribute excess amount for payment split
                                    decimal TotalSplitAmnt = 0;
                                    decimal SplitAmnt = 0;
                                    decimal TotalSplitTaxAmnt = 0;
                                    decimal SplitTaxAmnt = 0;
                                    foreach (GridViewRow grdRow in grdPaymentSplit.Rows)
                                    {
                                        HiddenField hdfPOPK = (HiddenField)grdRow.FindControl("hdfPOPK");
                                        Label lblBalanceSplit = (Label)grdRow.FindControl("lblBalanceSplit");
                                        decimal InvAmount = 0;
                                        decimal AdvInvAmount = 0;
                                        decimal PoBalanceAmount = 0;
                                        decimal InvOtherAmount = 0;
                                        decimal OtherAmount = 0;
                                        int PoPk = 0;
                                        int.TryParse(hdfPOPK.Value, out PoPk);
                                        decimal.TryParse(lblBalanceSplit.Text.Replace(",", "").Trim(), out PoBalanceAmount);
                                        if (!IsWorkOrder)
                                        {
                                            InvAmount = FinInvoiceVndTrxMpgListForAutoAlcn.Where(iv => iv.IVM_PO_HDR == PoPk).Sum(inv => inv.IVM_AMOUNT);
                                            InvOtherAmount = FinInvoiceVndTrxMpgListForAutoAlcn.Where(iv => iv.IVM_PO_HDR == PoPk).Sum(inv => inv.IVM_OTHER_AMOUNT);
                                            InvTotalOtherCharge = FinInvoiceVndTrxMpgListForAutoAlcn.Sum(inv => inv.IVM_OTHER_AMOUNT);
                                            if (FinInvoiceVndTrxMpgListForAutoAlcn[0].FIN_INVOICE_VND_HDR.IVH_CATEGORY == (byte)POInvoiceCategory.Invoice)
                                            {
                                                InvAmount = FinInvoiceVndTrxMpgListForAutoAlcn.Where(iv => iv.IVM_PO_HDR == PoPk).Sum(inv => inv.IVM_AMOUNT);
                                            }
                                        }
                                        else if (IsWorkOrder)
                                        {
                                            InvAmount = FinInvoiceVndTrxMpgListForAutoAlcn.Where(iv => iv.IVM_WO_HDR == PoPk).Sum(inv => inv.IVM_AMOUNT);
                                            InvOtherAmount = FinInvoiceVndTrxMpgListForAutoAlcn.Where(iv => iv.IVM_WO_HDR == PoPk).Sum(inv => inv.IVM_OTHER_AMOUNT);
                                            InvTotalOtherCharge = FinInvoiceVndTrxMpgListForAutoAlcn.Sum(inv => inv.IVM_OTHER_AMOUNT);
                                            if (FinInvoiceVndTrxMpgListForAutoAlcn[0].FIN_INVOICE_VND_HDR.IVH_CATEGORY == (byte)POInvoiceCategory.Invoice)
                                            {
                                                InvAmount = FinInvoiceVndTrxMpgListForAutoAlcn.Where(iv => iv.IVM_WO_HDR == PoPk).Sum(inv => inv.IVM_AMOUNT);
                                            }
                                        }
                                        TextBox txtPayNow = (TextBox)grdRow.FindControl("txtPayNowSplit");
                                        Label lblOtherChargesSplit = (Label)grdRow.FindControl("lblOtherChargesSplit");
                                        HiddenField hdfOtherChargesSplit = (HiddenField)grdRow.FindControl("hdfOtherChargesSplit");
                                        TextBox txtTaxSplit = (TextBox)grdRow.FindControl("txtTaxSplit");
                                        if (string.IsNullOrEmpty(txtPayNow.Text) || Convert.ToDecimal(txtPayNow.Text) != PoBalanceAmount)
                                        {
                                            PayNowSplit = (InvAmount / InvoiceSubTotal) * PayNow;
                                            PayNowSplit = Math.Round(PayNowSplit, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                            if (PayNowSplit < PoBalanceAmount && ExcessAmount > 0)
                                            {
                                                decimal Excess = ExcessAmount;
                                                ExcessAmount -= PoBalanceAmount - PayNowSplit;
                                                PayNowSplit += (ExcessAmount < 0) ? Excess : (PoBalanceAmount - PayNowSplit);
                                                txtPayNow.Text = Math.Round(PayNowSplit, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                                                if (InvTotalOtherCharge > 0)
                                                {
                                                    OtherAmount = (InvOtherAmount / InvTotalOtherCharge) * TotalOtherCharge;
                                                }
                                                else
                                                {
                                                    OtherAmount = 0;
                                                }
                                                lblOtherChargesSplit.Text = string.Format("{0:c}", Math.Round(OtherAmount, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits));
                                                hdfOtherChargesSplit.Value = OtherAmount.ToString();

                                                decimal TaxAmnt = 0;
                                                decimal TaxPer = 1;
                                                decimal.TryParse(hdfTaxPer.Value, out TaxPer);
                                                TaxAmnt = PayNowSplit * TaxPer;
                                                txtTaxSplit.Text = Math.Round(TaxAmnt, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                                            }
                                        }
                                        decimal.TryParse(txtPayNow.Text, out SplitAmnt);
                                        TotalSplitAmnt += SplitAmnt;
                                        decimal.TryParse(txtTaxSplit.Text, out SplitTaxAmnt);
                                        TotalSplitTaxAmnt += SplitTaxAmnt;

                                    }

                                    int RowIndex = grdPaymentSplit.Rows.Count - 1;
                                    if (PayNow != TotalSplitAmnt)
                                    {
                                        for (int rowcount = grdPaymentSplit.Rows.Count - 1; rowcount > 0; rowcount--)
                                        {
                                            GridViewRow grdSplitRow = grdPaymentSplit.Rows[rowcount];
                                            TextBox txtPayNowSplit = (TextBox)grdSplitRow.FindControl("txtPayNowSplit");
                                            Label lblBalanceSplit = (Label)grdSplitRow.FindControl("lblBalanceSplit");
                                            decimal paynowsplitlast = 0;
                                            decimal.TryParse(txtPayNowSplit.Text, out paynowsplitlast);

                                            decimal balancesplitlast = 0;
                                            decimal.TryParse(lblBalanceSplit.Text, out balancesplitlast);
                                            if (balancesplitlast >= ((PayNow - TotalSplitAmnt) + paynowsplitlast))
                                            {
                                                paynowsplitlast = paynowsplitlast + (PayNow - TotalSplitAmnt);
                                                txtPayNowSplit.Text = Math.Round(paynowsplitlast, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                                                RowIndex = rowcount;
                                                break;
                                            }
                                        }

                                    }
                                    if (Tax != TotalSplitTaxAmnt)
                                    {
                                        GridViewRow grdSplitRow = grdPaymentSplit.Rows[RowIndex];
                                        TextBox txtTaxSplit = (TextBox)grdSplitRow.FindControl("txtTaxSplit");
                                        TextBox txtPayNowSplit = (TextBox)grdSplitRow.FindControl("txtPayNowSplit");
                                        decimal TaxSplitlast = 0;
                                        decimal.TryParse(txtTaxSplit.Text, out TaxSplitlast);
                                        decimal PayNowsplt = 0;
                                        decimal.TryParse(txtPayNowSplit.Text, out PayNowsplt);
                                        if (PayNowsplt > 0)
                                        {
                                            if ((Tax - TotalSplitTaxAmnt) < 1)
                                            {
                                                TaxSplitlast = TaxSplitlast + (Tax - TotalSplitTaxAmnt);
                                                txtTaxSplit.Text = Math.Round(TaxSplitlast, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                                            }
                                        }
                                    }
                                }
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
        }

        /// <summary>
        ///check duplicate based on bank + InstrNo + InstrDate
        /// </summary>
        private bool CheckPayment(FIN_PAYMENT_VND_HDR finPaymentHdrObj)
        {
            bool success = true;
            POPaymentService poPaymentServiceClient;
            poPaymentServiceClient = null;
            try
            {

                if (Convert.ToInt32(ddlMode.SelectedValue) != (int)PaymentModeEnum.CASH)
                {
                    poPaymentServiceClient = new POPaymentService();
                    poPaymentServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(poPaymentServiceClient);
                    success = poPaymentServiceClient.CheckPaymentHdr(finPaymentHdrObj);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                poPaymentServiceClient = null;
            }
            return success;
        }

        private void SetVendorPayment()
        {
            //if (ddlWHTAccount.SelectedValue == CommonConstants.SELECTVAL)
            //{
            //    //chkVendorforpayemnt.Checked = false;
            //    chkVendorforpayemnt.Checked = true;
            //    chkVendorforpayemnt.Visible = false;
            //}
            //else
            //{
            chkVendorforpayemnt.Visible = true;
            //}
        }

        /// <summary>
        /// function used to bind Drop Downs Corresponding to the Drop Down passed
        /// </summary>
        private void BindDropDown(ControlsEnum controlType)
        {
            switch (controlType)
            {
                #region FormNo
                case ControlsEnum.FORMNO:
                    ddlFormno.Items.Clear();
                    if (admConstMstList != null && admConstMstList.Count > 0)
                    {
                        ddlFormno.DataSource = admConstMstList;
                        ddlFormno.DataTextField = Resources.DataFieldRes.ConstName;
                        ddlFormno.DataValueField = Resources.DataFieldRes.ConstPK;
                        ddlFormno.DataBind();
                    }
                    ddlFormno.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));

                    break;
                #endregion

                #region Company
                case ControlsEnum.COMPANY:
                    ddlCompany.Items.Clear();
                    if (admCompanyMstList != null && admCompanyMstList.Count > 0)
                    {
                        ddlCompany.DataSource = CommonFunctions.HtmlDecode(admCompanyMstList, Resources.DataFieldRes.CompanySpecs);
                        ddlCompany.DataTextField = Resources.DataFieldRes.CompanySpecs;
                        ddlCompany.DataValueField = Resources.DataFieldRes.CompanyMstPK;
                        ddlCompany.DataBind();
                    }
                    break;
                #endregion

                #region Payment Mode
                case ControlsEnum.PAYMODE:
                    ddlMode.Items.Clear();
                    if (admConfigMstList != null && admConfigMstList.Count > 0)
                    {
                        ddlMode.DataSource = admConfigMstList;
                        ddlMode.DataTextField = Resources.DataFieldRes.cfgData;
                        ddlMode.DataValueField = Resources.DataFieldRes.cfgValue;
                        ddlMode.DataBind();
                    }
                    ddlMode.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion

                #region Adjustment Types
                case ControlsEnum.DISCOUNTTYPE:
                    ddlAdjType.Items.Clear();
                    if (dtDiscountTypes != null && dtDiscountTypes.Rows.Count > 0)
                    {
                        ddlAdjType.DataSource = dtDiscountTypes;
                        ddlAdjType.DataTextField = Resources.DataFieldRes.ConstName;
                        ddlAdjType.DataValueField = Resources.DataFieldRes.ConstValue;
                        ddlAdjType.DataBind();
                    }
                    ddlAdjType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion

                #region Vendor Account
                case ControlsEnum.VENDORACCOUNT:
                    //ddlWHTAccountPopup.Items.Clear();
                    //if (dtVendorAccount != null && dtVendorAccount.Rows.Count > 0)
                    //{
                    //    ddlWHTAccountPopup.DataSource = dtVendorAccount;
                    //    ddlWHTAccountPopup.DataTextField = Resources.DataFieldRes.RFQResponseTaxHead;
                    //    ddlWHTAccountPopup.DataValueField = Resources.DataFieldRes.RFQResponseTaxPK;
                    //    ddlWHTAccountPopup.DataBind();
                    //}
                    //ddlWHTAccountPopup.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion

                //#region VENDORCONTACTYPE
                //case ControlsEnum.VENDORCONTACTYPE:
                //    ddlAddressType.Items.Clear();
                //    if (TempConfigMstDetails != null && TempConfigMstDetails.Count > 0)
                //    {
                //        ddlAddressType.DataSource = TempConfigMstDetails;
                //        ddlAddressType.DataTextField = Resources.DataFieldRes.cfgData;
                //        ddlAddressType.DataValueField = Resources.DataFieldRes.cfgValue;
                //        ddlAddressType.DataBind();
                //    }
                //    ddlAddressType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));

                //    break;
                //#endregion
                #region VATBUYTAXTYPES
                case ControlsEnum.VATBUYTAXTYPES:
                    //Bind Tax dropdown
                    ddlVATAccountPopup.Items.Clear();
                    if (dtTaxDetails != null && dtTaxDetails.Rows.Count > 0)
                    {
                        ddlVATAccountPopup.DataSource = CommonFunctions.HtmlDecodeDataTable(dtTaxDetails, Resources.DataFieldRes.RFQResponseTaxHead);
                        ddlVATAccountPopup.DataTextField = Resources.DataFieldRes.RFQResponseTaxHead;
                        ddlVATAccountPopup.DataValueField = Resources.DataFieldRes.RFQResponseTaxPK;
                        ddlVATAccountPopup.DataBind();
                    }
                    //ddlVATAccountPopup.Items.Add(new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion

                #region PURINVNOS
                case ControlsEnum.PURINVNOS:
                    ddlPurInvNo.Items.Clear();
                    if (PurInvoices != null && PurInvoices.Count > 0)
                    {
                        ddlPurInvNo.DataSource = PurInvoices;
                        ddlPurInvNo.DataTextField = Resources.DataFieldRes.INVNo;
                        ddlPurInvNo.DataValueField = Resources.DataFieldRes.POInvoicePK;
                        ddlPurInvNo.DataBind();
                    }
                    //ddlPurInvNo.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion

                #region VENDOR BANKS
                case ControlsEnum.VENDORBANKS:
                    ddlvendorBank.Items.Clear();
                    if (dtVendorBanks != null && dtVendorBanks.Rows.Count > 0)
                    {
                        ddlvendorBank.DataSource = dtVendorBanks;
                        ddlvendorBank.DataTextField = Resources.DataFieldRes.VendorBankName;
                        ddlvendorBank.DataValueField = Resources.DataFieldRes.VendorBankPk;
                        ddlvendorBank.DataBind();
                    }
                    ddlvendorBank.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region Payment Types
                case ControlsEnum.PAYMENTTYPE:
                    ddlPayType.DataSource = dtPaymentTypes;
                    ddlPayType.DataTextField = Resources.DataFieldRes.cfgData;
                    ddlPayType.DataValueField = Resources.DataFieldRes.cfgValue;
                    ddlPayType.DataBind();
                    ddlPayType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                    #endregion

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
                    #region PAYMENTHDRLIST
                    case ControlsEnum.PAYMENTHDRLIST:
                        if (finPaymentVndHdrList != null)
                        {
                            GetFieldValues(ControlsEnum.FILLWORKFLOWSTATUS);
                            uclPaging.TotalPages = TotalPages;
                            PageIndex = PageIndex == null ? CommonConstants.SELECT_VALUE_ONE : PageIndex;
                            uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                            grdPOPaymentHdr.DataSource = finPaymentVndHdrList;
                            grdPOPaymentHdr.DataBind();

                            uclPaging.Visible = true;
                            uclPaging.BindPager();
                        }
                        break;
                    #endregion
                    #region PAYMENTMPGLIST
                    case ControlsEnum.PAYMENTMPGLIST:
                        if (finInvoiceHdrList != null && CurrPK == 0)
                        {
                            grdInvoiceList.DataSource = finInvoiceHdrList;
                            grdInvoiceList.DataBind();
                            if (finInvoiceHdrList.Count > 0)
                            {
                                ddlCompany.SelectedValue = finInvoiceHdrList[0].IVH_COMPANY.ToString();
                                hdfVendorPK.Value = finInvoiceHdrList[0].IVH_VENDOR.ToString();
                                hdfVendorAccountNo.Value = finInvoiceHdrList[0].IVH_VENDOR_ACCOUNT.ToString();
                                hdfInvoiceCurr.Value = finInvoiceHdrList[0].IVH_CURRENCY.ToString();
                                hdfPaymentCurrency.Value = finInvoiceHdrList[0].IVH_CURRENCY.ToString();
                                txtPaymentCurrency.Text = finInvoiceHdrList[0].ADM_CURRENCY_MST1.CUR_CODE.ToString();
                                lblPaymentAmount.Text = GetLocalResourceObject("PaymentAmount") + "(" + finInvoiceHdrList[0].ADM_CURRENCY_MST1.CUR_CODE + ")";
                                GetFieldValues(ControlsEnum.EXCHANGERATE);
                                GetFieldValues(ControlsEnum.EXCHANGERATEBANK);
                                if (ddlBankChargeCurrency.Items[0].Value != finInvoiceHdrList[0].IVH_CURRENCY.ToString())
                                {
                                    ddlBankChargeCurrency.Items.Insert(1, (new ListItem(finInvoiceHdrList[0].ADM_CURRENCY_MST1.CUR_CODE + " - " + finInvoiceHdrList[0].ADM_CURRENCY_MST1.CUR_NAME, finInvoiceHdrList[0].IVH_CURRENCY.ToString())));
                                    //ddlBankChargeCurrency.Items.Insert(1, (new ListItem(finInvoiceHdrList[0].ADM_CURRENCY_MST1.CUR_CODE , finInvoiceHdrList[0].IVH_CURRENCY.ToString())));

                                }

                                if (finInvoiceHdrList[0].FIN_INVOICE_VND_TRX_MPG != null)
                                {
                                    if ((finInvoiceHdrList.First().IVH_GROUP == null ? 1 : Convert.ToInt16(finInvoiceHdrList.First().IVH_GROUP)) == (int)POInvoiceGroup.AgtInvoice)
                                    {
                                        InvoiceType = finInvoiceHdrList.FirstOrDefault().IVH_TYPE == 1 ? 2 : 1;
                                    }
                                    List<FIN_INVOICE_VND_TRX_MPG> livoiceCusTrxMpgList = finInvoiceHdrList[0].FIN_INVOICE_VND_TRX_MPG.ToList();
                                    if (livoiceCusTrxMpgList.Count > 0)
                                        if (livoiceCusTrxMpgList[0].PUR_ORDER_HDR != null)
                                            InvoiceType = livoiceCusTrxMpgList[0].PUR_ORDER_HDR.POH_TYPE;
                                }
                            }
                            else
                            {
                                txtPaidAmount.Text = "0.00";
                            }
                        }
                        else if (finPaymentTrxMpgList != null)
                        {
                            grdInvoiceList.DataSource = finPaymentTrxMpgList;
                            grdInvoiceList.DataBind();
                            if (finPaymentTrxMpgList.Count > 0)
                            {
                                if (finPaymentTrxMpgList[0].FIN_INVOICE_VND_HDR != null)
                                {
                                    hdfVendorPK.Value = finPaymentTrxMpgList[0].FIN_INVOICE_VND_HDR.IVH_VENDOR.ToString();
                                    hdfVendorAccountNo.Value = finPaymentTrxMpgList[0].FIN_INVOICE_VND_HDR.IVH_VENDOR_ACCOUNT.ToString();
                                    hdfInvoiceCurr.Value = finPaymentTrxMpgList[0].FIN_INVOICE_VND_HDR.IVH_CURRENCY.ToString();
                                    hdfPaymentCurrency.Value = finPaymentTrxMpgList[0].FIN_INVOICE_VND_HDR.IVH_CURRENCY.ToString();
                                    txtPaymentCurrency.Text = finPaymentTrxMpgList[0].FIN_INVOICE_VND_HDR.ADM_CURRENCY_MST1.CUR_CODE.ToString();
                                    lblPaymentAmount.Text = GetLocalResourceObject("PaymentAmount") + "(" + finPaymentTrxMpgList[0].FIN_INVOICE_VND_HDR.ADM_CURRENCY_MST1.CUR_CODE + ")";
                                    GetFieldValues(ControlsEnum.EXCHANGERATE);
                                    GetFieldValues(ControlsEnum.EXCHANGERATEBANK);
                                    //if (ddlBankChargeCurrency.Items[0].Value != finPaymentTrxMpgList[0].FIN_INVOICE_VND_HDR.IVH_CURRENCY.ToString())
                                    //{
                                    //    ddlBankChargeCurrency.Items.Insert(1, (new ListItem(finPaymentTrxMpgList[0].FIN_INVOICE_VND_HDR.ADM_CURRENCY_MST1.CUR_CODE + " - " + finPaymentTrxMpgList[0].FIN_INVOICE_VND_HDR.ADM_CURRENCY_MST1.CUR_NAME, finPaymentTrxMpgList[0].FIN_INVOICE_VND_HDR.IVH_CURRENCY.ToString())));
                                    //}
                                    if ((finPaymentTrxMpgList[0].FIN_INVOICE_VND_HDR.IVH_GROUP == null ? 1 : Convert.ToInt16(finPaymentTrxMpgList[0].FIN_INVOICE_VND_HDR.IVH_GROUP)) == (int)POInvoiceGroup.AgtInvoice)
                                    {
                                        InvoiceType = finPaymentTrxMpgList[0].FIN_INVOICE_VND_HDR.IVH_TYPE == 1 ? 2 : 1;
                                    }
                                    List<FIN_INVOICE_VND_TRX_MPG> livoiceCusTrxMpgList = finPaymentTrxMpgList[0].FIN_INVOICE_VND_HDR.FIN_INVOICE_VND_TRX_MPG.ToList();
                                    if (livoiceCusTrxMpgList.Count > 0)
                                        if (livoiceCusTrxMpgList[0].PUR_ORDER_HDR != null)
                                            InvoiceType = livoiceCusTrxMpgList[0].PUR_ORDER_HDR.POH_TYPE;

                                }
                            }
                            else
                            {
                                txtPaidAmount.Text = "0.00";
                            }
                        }
                        break;
                    #endregion

                    #region PAYMENTSPLITLIST
                    case ControlsEnum.PAYMENTSPLITLIST:
                        if (FinInvoiceVndTrxMpgList != null)
                        {
                            grdPaymentSplit.DataSource = FinInvoiceVndTrxMpgList;
                            grdPaymentSplit.DataBind();
                        }
                        else if (finPaymentVndPoMpgList != null)
                        {
                            grdPaymentSplit.DataSource = finPaymentVndPoMpgList;
                            grdPaymentSplit.DataBind();
                        }
                        break;
                    #endregion

                    #region WHTPOPUPGRID
                    case ControlsEnum.WHTPOPUPGRID:
                        //if (WHTTaxDetails != null && WHTTaxDetails.Count > 0)
                        //{
                        grdWHTTaxDetails.DataSource = TempWHTTaxDetails;
                        grdWHTTaxDetails.DataBind();
                        // }
                        break;
                    #endregion

                    #region VATPOPUPGRID
                    case ControlsEnum.VATPOPUPGRID:
                        grdVATTaxDetails.DataSource = TempVATTaxDetails;
                        grdVATTaxDetails.DataBind();
                        SetVatbuyNotYetDueRowColor();
                        break;
                    #endregion

                    #region UPLOADED FILES
                    case ControlsEnum.UPLOADEDFILES:
                        //if (DocAttachList != null)
                        //{                    
                        grdUploads.DataSource = DocAttachList;
                        grdUploads.DataBind();
                        //}
                        break;
                    #endregion

                    #region PAYMENTADJN
                    case ControlsEnum.PAYMENTADJN:
                        if (CrDrAdjnList != null)
                        {
                            grdPaymentSplitAdjn.DataSource = CrDrAdjnList;
                            grdPaymentSplitAdjn.DataBind();
                        }
                        else if (FinPaymentVndAllocationList != null)
                        {
                            grdPaymentSplitAdjn.DataSource = FinPaymentVndAllocationList;
                            grdPaymentSplitAdjn.DataBind();
                        }
                        if (totBalanceAdjn <= 0)
                        {
                            grdPaymentSplitAdjn.DataSource = null;
                            grdPaymentSplitAdjn.DataBind();
                        }
                        break;
                    #endregion

                    #region PAYMENTMODESGRID
                    case ControlsEnum.PAYMENTMODESGRID:
                        pnlPaymentModesList.Visible = false;
                        grdPaymentModes.DataSource = PaymentModeDetailsList;
                        grdPaymentModes.DataBind();
                        if (PaymentModeDetailsList != null)
                        {
                            if ((PaymentModeDetailsList.Count == 1 && IsPaymentModeAdded) || PaymentModeDetailsList.Count > 1)
                                pnlPaymentModesList.Visible = true;
                        }
                        break;
                    #endregion
                    #region CRDRALLOCATION
                    case ControlsEnum.CRDRALLOCATION:
                        grdCrdrAllocation.DataSource = FinPaymentVndCrdrMpgList;
                        grdCrdrAllocation.DataBind();
                        break;
                    #endregion

                    #region INVOICELIST
                    case ControlsEnum.INVOICELIST:
                        grdNewInvList.DataSource = dtInvoiceList;
                        grdNewInvList.DataBind();
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
                foreach (GridViewRow grdrow in grdPOPaymentHdr.Rows)
                {
                    RadioButton rbtn = (RadioButton)grdrow.FindControl(Resources.Report.RadioButtonID);
                    // check row selected or not
                    if (rbtn.Checked)
                    {
                        hdfEdit.Value = "1";
                        // get pk from the grid and assign to CurrPk
                        CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfPaymentID")).Value);
                        Session[ERP.Utilities.SessionStrings.VendorPK] = ((HiddenField)grdrow.FindControl("hdfVendorPK")).Value;
                        Session[ERP.Utilities.SessionStrings.Vendor] = ((Label)grdrow.FindControl("lblVendor")).Text;
                        Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                        // Get PaymentDetails
                        GetFieldValues(ControlsEnum.PAYMENTHDRENTRYBYPK);
                        GetUIValuesFromObject(ControlsEnum.PAYMENTHDRENTRY);
                        GetFieldValues(ControlsEnum.PAYMENTHDRINVLISTBYPK);
                        SetFieldValues(ControlsEnum.PAYMENTMPGLIST);

                        //finPaymentVndHdrObj = CommonFunctions.Initilize<FIN_PAYMENT_VND_HDR>();
                        //finPaymentVndHdrObj.PVH_PK = CurrPK;

                        GetFieldValues(ControlsEnum.UPLOADEDFILES);
                        SetFieldValues(ControlsEnum.UPLOADEDFILES);
                        if (Mode == ActionsEnum.VIEW)
                        {
                            EntryStatus = EntryStatus.VIEWMODE;
                        }
                        else
                        {
                            EntryStatus = EntryStatus.ENTRYMODE;
                        }
                        WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                        base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID((int)CurrPK, ucrWrkf.ProcessID);
                        SetCancelRef((int)CurrPK);
                        ucrWrkf.FillWorkFlowDetails();
                        if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                        {
                            ucrWrkf.ViewType = 1;
                            //btnSave.Visible = true;
                            //divbtnSavePaymentSplit.Visible = true;
                        }
                        else
                        {
                            ucrWrkf.ViewType = 0;
                            //EntryStatus = EntryStatus.VIEWMODE;                           
                            //btnSave.Visible = false;
                            //divbtnSavePaymentSplit.Visible = false;
                        }
                        btnPrint.Visible = true;
                        ucrWrkf.ViewAction();
                        GetFieldValues(ControlsEnum.BASECURRENCY);
                        lblTotalAmountBC.Text = string.Format(lblTotalAmountBC.Text, hdfBaseCurrency.Value.Split('-')[0].Trim());

                        //if (InvoiceType == Convert.ToInt32(PurchaseType.Local))//If Invoicetype is Domestic then exchangerate is noneditable                   
                        //{
                        //    txtExchangeRate.Enabled = false;
                        //    txtExchangeRate.CssClass = "medium numeric input-disabled";
                        //}
                        //else
                        //{
                        //    txtExchangeRate.Enabled = true;
                        //    txtExchangeRate.CssClass = "numeric medium";

                        //}
                        //for adj allocation;need all saved val's in ReceiptAdjnList
                        //foreach (GridViewRow gvrw in grdInvoiceList.Rows)
                        //{
                        //    HiddenField hdfReceiptMpgPK = gvrw.FindControl("hdfPaymentMpgPK") as HiddenField;
                        //    long mpgpk = 0;
                        //    long.TryParse(hdfReceiptMpgPK.Value, out mpgpk);
                        //    PaymentMpgPK = mpgpk;// Convert.ToInt64(hdfReceiptMpgPK.Value);
                        //    HiddenField hdfInvoicePK = gvrw.FindControl("hdfInvoicePK") as HiddenField;

                        //    InvoicePK = Convert.ToInt32(hdfInvoicePK.Value);

                        //    GetFieldValues(ControlsEnum.PAYMENTADJNLIST);
                        //}
                        GetFieldValues(ControlsEnum.CRDRALLOCATION);
                        return;
                    }
                }
                // if no items selected, Show Error Message
                litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                EntryStatus = EntryStatus.LISTMODE;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method used to Reset Dorm Controls
        /// </summary>
        private void ResetForm()
        {
            CurrPK = 0;
            InvoicePOSplitList = null;
            txtVendor.Text = string.Empty;
            hdfVendorID.Value = string.Empty;
            txtPaymentNumber.Text = string.Empty;
            hdfPaymentPK.Value = string.Empty;
            hdfBankAccount.Value = string.Empty;
            txtBranch.Text = string.Empty;
            txtAccountNo.Text = string.Empty;
            txtPaymentBank.Text = string.Empty;
            hdfPaymentBank.Value = string.Empty;
            txtRemarks.Text = string.Empty;
            txtPaidAmount.Text = string.Empty;
            EditedInvoices = null;
            EditedPaymentDtls = null;
            hdfExchangeCurr.Value = string.Empty;
            hdfExchangeCurrBC.Value = string.Empty;
            hdfInvoiceCurr.Value = string.Empty;
            txtSINo.Text = string.Empty;
            hdfVendorAccountNo.Value = string.Empty;
            hdfVendorID.Value = string.Empty;
            hdfVendorPK.Value = string.Empty;
            hdfVendorID.Value = string.Empty;
            ModifiedDatePnl.Visible = false;
            lblLastModifiedHDR.Text = string.Empty;
            grdInvoiceList.DataSource = null;
            grdInvoiceList.DataBind();
            ddlStatus.SelectedValue = "3";
            txtSearchDateFrom.Text = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString(Resources.Constants.DateFormatShort);
            hdfSearchDateFrom.Value = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString();
            txtSearchDateTo.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
            hdfSearchDateTo.Value = DateTime.Now.ToString();
            //ddlWHTAccount.SelectedValue = CommonConstants.SELECTVAL;
            ddlBankChargeCurrency.Items.Clear();
            GetFieldValues(ControlsEnum.BANKCURRENCY);
            SetFieldValues(ControlsEnum.BANKCURRENCY);
            txtBankCharge.Text = string.Empty;
            chkBankCharge.Checked = false;
            SetVendorPayment();
            base.WkfRefID = 0;
            ddlStatus.SelectedIndex = 0;
            ddlPDCStatus.SelectedIndex = 0;
            txtWHTAccountPopup.Text = "Select/Type";
            hdfWHTAccountPopup.Value = "0";
            txtPopupWHTAmount.Text = 0.ToString(hdfCurrencyFormat.Value);
            txtWHTTaxAmountPopup.Text = 0.ToString(hdfCurrencyFormat.Value);
            txtDescriptionPopup.Text = string.Empty;
            txtWHTAmount.Text = string.Empty;
            TempWHTTaxDetails = null;


            SetBranchCodeVisibility();
            txtBeforeTaxAmount.Text = 0.ToString(hdfCurrencyFormat.Value);
            txtVATTaxAmountPopup.Text = 0.ToString(hdfCurrencyFormat.Value);
            txtVatTaxInvNo.Text = string.Empty;
            txtVatTaxInvDate.Text = string.Empty;
            chkOriginalinvoice.Checked = false;
            txtMaterial.Text = string.Empty;
            tempVATTaxDetails = null;
            FileDetailsList = null;
            DocAttachList = null;

            CrDrAdjnList = null;
            FinPaymentVndAllocationList = null;
            PaymentAdjnList = null;
            PaymentInvDetList = null;
            finInvoiceHdrList = null;
            FinInvoiceVndHdrSelectedList = null;
            IsCreditExist = false;
            ResetForm(ControlsEnum.ADDITEM);
            hdfNotTalliedInvoicePk.Value = "0";
        }

        private void ResetForm(ControlsEnum controlType)
        {
            switch (controlType)
            {
                case ControlsEnum.ADDITEM:
                    //ddlType.ClearSelection();
                    anchorFile.Visible = false;
                    vrfFileUpload.Enabled = true;
                    CurrSlNo = 0;
                    anchorFile.Attributes.Remove("onclick");
                    break;
                case ControlsEnum.PAYMENTMODES:
                    double zeroAmount = 0;
                    decimal ExchangeRate = 0;
                    decimal PayableAmnt = 0;
                    decimal PaymentAmount = 0;
                    ddlMode.SelectedValue = CommonConstants.SELECTVAL;
                    chkBankCharge.Checked = false;
                    PaymentModeRowIndex = -1;
                    txtPaymentBank.Text = string.Empty;
                    hdfPaymentBank.Value = string.Empty;
                    txtBranch.Text = string.Empty;
                    txtAccountNo.Text = string.Empty;
                    txtInstrumentNo.Text = string.Empty;
                    txtInstrumentDate.Text = string.Empty;
                    txtFavourof.Text = string.Empty;
                    txtBankCharge.Text = GetFormattedCurrency(zeroAmount);
                    //txtTotalAmountBC.Text = GetFormattedCurrency(zeroAmount);
                    //decimal.TryParse(txtExchangeRate.Text, out ExchangeRate);
                    decimal.TryParse(txtHdrExchangeRate.Text, out ExchangeRate);
                    decimal.TryParse(txtPaidAmount.Text, out PayableAmnt);
                    decimal.TryParse(txtPaymentAmount.Text, out PaymentAmount);
                    if (PaymentModeDetailsList != null && PaymentModeDetailsList.Count > 0)
                    {
                        //decimal TotPaidAmntBc = 0;                        
                        //decimal BalanceAmntBC = 0;                     
                        //TotPaidAmntBc = PaymentModeDetailsList.Sum(r => r.PDM_PAID_AMOUNT / (r.PDM_EXCHG_RATE <= 0 ? 1 : Convert.ToDecimal(r.PDM_EXCHG_RATE)));
                        //BalanceAmntBC = (PayableAmnt - TotPaidAmntBc) < 0 ? 0 : (PayableAmnt - TotPaidAmntBc);
                        //BalanceAmntBC = BalanceAmntBC * (ExchangeRate <= 0 ? 1 : ExchangeRate);
                        //txtTotalAmountBC.Text = GetFormattedCurrency(BalanceAmntBC);
                        decimal TotPaidAmnt = 0;
                        decimal BalanceAmnt = 0;
                        TotPaidAmnt = PaymentModeDetailsList.Sum(r => r.PDM_PAID_AMOUNT);
                        BalanceAmnt = (PayableAmnt - TotPaidAmnt) < 0 ? 0 : (PayableAmnt - TotPaidAmnt);
                        txtPaymentAmount.Text = GetFormattedCurrency(BalanceAmnt);
                        txtTotalAmountBC.Text = GetFormattedCurrency(BalanceAmnt * ExchangeRate);
                    }
                    else
                    {
                        txtTotalAmountBC.Text = GetFormattedCurrency(PaymentAmount * ExchangeRate);
                    }

                    vrfBranch.Enabled = true;
                    vrfAccountNo.Enabled = true;
                    vrfInstrumentNo.Enabled = true;
                    vrfInstrumentDate.Enabled = true;
                    vrfFavourof.Enabled = true;
                    txtInstrumentNo.Enabled = true;
                    txtInstrumentDate.Enabled = true;
                    txtFavourof.Enabled = true;
                    hdfFavourof.Value = lblCustomerTxt.ToolTip;//txtFavourof.Text =                    
                    txtInstrumentNo.CssClass = "Uiinput-amount select-half";
                    txtInstrumentDate.CssClass = "input-small";
                    txtFavourof.CssClass = "multiline-2line";
                    Label5.Visible = true;
                    txtBankCharge.Visible = txtBankCharge.Enabled = true;
                    chkBankCharge.Visible = chkBankCharge.Enabled = true;
                    ddlBankChargeCurrency.Enabled = true;
                    break;
            }
        }

        /// <summary>
        /// Funtion used get WHT NO
        /// </summary>
        private void getWHTNO()
        {
            cm = new CommonService();
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            string WhtNo = cm.GetTrxDocNo(ApplicationType.VP, Convert.ToInt32(AppSubTypeVP.WHTCERTIFICATE), 1,
                            DateTime.Now, currentUser.PKUser, true, 0, Convert.ToInt32(ddlCompany.SelectedValue));
            hdfWHTNO.Value = WhtNo;
        }

        private bool ShowWHTCertNoInPayment()
        {
            string configValue = Convert.ToString(GetGlobalResourceObject("ConfigurationsRes", "ShowWHTCertNOInPayment"));
            return configValue == "1" || string.Equals(configValue, "true", StringComparison.OrdinalIgnoreCase);
        }

        private void ConfigureWHTCertNoControl()
        {
            bool showWHTCertNo = ShowWHTCertNoInPayment();
            lblCRTNo.Visible = showWHTCertNo;
            txtCRTNo.Visible = showWHTCertNo;
            rfvCRTNO.Visible = showWHTCertNo;
            rfvCRTNO.Enabled = showWHTCertNo;
        }

        private void SetWHTCertificateNo(FIN_PAYMENT_VND_HDR paymentHeader)
        {
            if (ShowWHTCertNoInPayment())
            {
                hdfWHTNO.Value = txtCRTNo.Text.Trim();
                paymentHeader.PVH_WHT_NO = hdfWHTNO.Value;
                return;
            }

            if (hdfWHTNO.Value == string.Empty)
            {
                getWHTNO();
            }
            paymentHeader.PVH_WHT_NO = hdfWHTNO.Value;
        }

        public string GetFormattedCurrency(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfCurrencyFormat.Value);
        }
        /// <summary>
        /// Check Valid Payment
        /// </summary>
        /// <returns></returns>
        private bool IsValidPayment()
        {
            bool result = true;
            foreach (GridViewRow grdPOrow in grdInvoiceList.Rows)
            {
                grdPOrow.BackColor = Color.White;
                Label lblTotalAmount = (Label)grdPOrow.FindControl("lblTotalAmount");
                Label lblPaid = (Label)grdPOrow.FindControl("lblPaid");
                TextBox txtPayNow = (TextBox)grdPOrow.FindControl("txtPayNow");

                TextBox txtOtherCharges = (TextBox)grdPOrow.FindControl("txtOtherCharges");
                Label lblOtherCharges = (Label)grdPOrow.FindControl("lblOtherCharges");
                HiddenField hdfOtherChargesPrev = (HiddenField)grdPOrow.FindControl("hdfOtherChargesPrev");
                Label lblBaltopay = (Label)grdPOrow.FindControl("lblBaltopay");
                HiddenField hdfInvoicePK = (HiddenField)grdPOrow.FindControl("hdfInvoicePK");
                Label lblCrdrAlcnAmount = (Label)grdPOrow.FindControl("lblCrdrAlcnAmount");
                Label lblAdjAmount = (Label)grdPOrow.FindControl("lblAdjAmount");
                HiddenField hdfInitialBaltoPay = (HiddenField)grdPOrow.FindControl("hdfInitialBaltoPay");

                Label lblCnAmount = (Label)grdPOrow.FindControl("lblCnAmount");
                decimal TotCnAmnt = 0;
                if (lblCnAmount != null)
                    decimal.TryParse(lblCnAmount.Text, out TotCnAmnt);

                decimal OtherCharge = 0;
                decimal.TryParse(txtOtherCharges.Text, out OtherCharge);

                //if (Convert.ToDecimal(lblTotalAmount.Text) >= Convert.ToDecimal(lblPaid.Text) + Convert.ToDecimal(txtPayNow.Text))
                //{
                //    result = true;
                //}
                //else
                //{
                //    litErrorMsg.Text = GetLocalResourceObject("Err_msg_1").ToString();
                //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                //    result = false;
                //    break;
                //}
                if (Convert.ToDecimal(lblOtherCharges.Text.Replace(",", "")) >= OtherCharge + Convert.ToDecimal(hdfOtherChargesPrev.Value))
                {
                    result = true;
                }
                else
                {
                    litErrorMsg.Text = GetLocalResourceObject("Err_msg_2").ToString();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                    result = false;
                    grdPOrow.BackColor = Color.FromName(Resources.ErpRes.RowColourPink.ToString());
                    break;
                }



                #region Validation for Credit Note Amount
                decimal Payable = 0;
                decimal Paid = 0;
                //decimal PaidCNAmount = 0;
                decimal AllocatedCN = 0;
                decimal AdjAmount = 0;
                decimal InitialPayable = 0;
                decimal BalanceDN = 0;
                decimal BalanceToPay = 0;
                decimal DraftedCNAmount = 0;
                decimal.TryParse(lblTotalAmount.Text, out Payable);
                decimal.TryParse(lblPaid.Text, out Paid);
                decimal.TryParse(lblCrdrAlcnAmount.Text, out AllocatedCN);
                decimal.TryParse(lblAdjAmount.Text, out AdjAmount);
                decimal.TryParse(lblBaltopay.Text, out BalanceToPay);
                InvoicePK = Convert.ToInt64(hdfInvoicePK.Value);
                if (PaymentCrdrList != null)
                {
                    //PaidCNAmount = PaymentCrdrList.Where(r => r.PNM_INVOICE_HDR == Convert.ToInt64(hdfInvoicePK.Value)).Sum(sm => sm.PNM_BALANCE_AMOUNT);
                    BalanceDN = PaymentCrdrList.Where(r => r.PNM_INVOICE_HDR == Convert.ToInt64(hdfInvoicePK.Value)).Sum(sm => sm.PNM_BALANCE_AMOUNT - (sm.PNM_ADJ_AMOUNT + sm.PNM_PAID_AMOUNT));
                }
                //decimal InvoicePayable = Payable - (Paid - PaidCNAmount);
                //if (Convert.ToDecimal(txtPayNow.Text) > (InvoicePayable + AllocatedCN))
                //{
                //    litErrorMsg.Text = GetLocalResourceObject("Err_msg_CNAmount").ToString();
                //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                //    result = false;
                //    break;
                //}
                GetFieldValues(ControlsEnum.CRDRMPGLIST);
                if (FinCrdrMpgList != null && FinCrdrMpgList.Count > 0)
                {
                    DraftedCNAmount = FinCrdrMpgList.Sum(r => r.CDM_AMOUNT);
                }
                InitialPayable = BalanceToPay + AdjAmount;
                decimal PayNowWithAdj = Convert.ToDecimal(txtPayNow.Text) + AdjAmount;
                decimal PayableWithoutBlnDN = InitialPayable - BalanceDN - DraftedCNAmount;
                if (PayNowWithAdj > PayableWithoutBlnDN)
                {
                    if (DraftedCNAmount > 0 || BalanceDN > 0)
                        litErrorMsg.Text = GetLocalResourceObject("Err_msg_CNAmount").ToString();
                    else
                        litErrorMsg.Text = GetLocalResourceObject("Err_PayNowExcedsBalPay").ToString();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                    result = false;
                    grdPOrow.BackColor = Color.FromName(Resources.ErpRes.RowColourPink.ToString());
                    break;
                }
                #endregion


                //Uncommented for bug : 3014,3076
                if ((Convert.ToDecimal(txtPayNow.Text) + (Convert.ToDecimal(lblPaid.Text.Replace(",", "")))) - (OtherCharge + (Convert.ToDecimal(hdfOtherChargesPrev.Value))) <= (Convert.ToDecimal(lblTotalAmount.Text.Replace(",", "")) - Convert.ToDecimal(lblOtherCharges.Text.Replace(",", "")) + TotCnAmnt))
                {
                    result = true;
                }
                else
                {
                    litErrorMsg.Text = GetLocalResourceObject("Err_msg_3").ToString();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                    result = false;
                    grdPOrow.BackColor = Color.FromName(Resources.ErpRes.RowColourPink.ToString());
                    break;
                }
            }
            return result;
        }
        //private bool IsValidPayment()
        //{
        //    bool result = false;
        //    foreach (GridViewRow grdPOrow in grdInvoiceList.Rows)
        //    {
        //        Label lblTotalAmount = (Label)grdPOrow.FindControl("lblTotalAmount");
        //        Label lblPaid = (Label)grdPOrow.FindControl("lblPaid");
        //        TextBox txtPayNow = (TextBox)grdPOrow.FindControl("txtPayNow");

        //        TextBox txtOtherCharges = (TextBox)grdPOrow.FindControl("txtOtherCharges");
        //        Label lblOtherCharges = (Label)grdPOrow.FindControl("lblOtherCharges");
        //        HiddenField hdfOtherChargesPrev = (HiddenField)grdPOrow.FindControl("hdfOtherChargesPrev");

        //        if (Convert.ToDecimal(lblTotalAmount.Text) >= Convert.ToDecimal(lblPaid.Text) + Convert.ToDecimal(txtPayNow.Text))
        //        {
        //            result = true;
        //        }
        //        else
        //        {
        //            litErrorMsg.Text = GetLocalResourceObject("Err_msg_1").ToString();
        //            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
        //            result = false;
        //            break;
        //        }

        //        if (Convert.ToDecimal(lblOtherCharges.Text) >= Convert.ToDecimal(txtOtherCharges.Text) + Convert.ToDecimal(hdfOtherChargesPrev.Value))
        //        {
        //            result = true;
        //        }
        //        else
        //        {
        //            litErrorMsg.Text = GetLocalResourceObject("Err_msg_2").ToString();
        //            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
        //            result = false;
        //            break;
        //        }
        //        if ((Convert.ToDecimal(txtPayNow.Text) + (Convert.ToDecimal(lblPaid.Text))) - (Convert.ToDecimal(txtOtherCharges.Text) + (Convert.ToDecimal(hdfOtherChargesPrev.Value))) <= (Convert.ToDecimal(lblTotalAmount.Text) - Convert.ToDecimal(lblOtherCharges.Text)))
        //        {
        //            result = true;
        //        }
        //        else
        //        {
        //            litErrorMsg.Text = GetLocalResourceObject("Err_msg_3").ToString();
        //            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);

        //            break;
        //        }

        //    }
        //    return result;
        //}
        private bool IsValidOtherCharges()
        {
            Label lblOtherCharhes;
            HiddenField hdfPrevOtherCharges;
            decimal GrdOtherchargesTotal = 0;
            decimal GrdPrevOtherchargesTotal = 0;
            decimal totoalOtherCharges = 0;
            bool result = false;
            foreach (GridViewRow gv in grdInvoiceList.Rows)
            {
                lblOtherCharhes = gv.FindControl("lblOtherCharges") as Label;
                //  hdfPrevOtherCharges = gv.FindControl("hdfOtherChargesPrev") as HiddenField;
                GrdOtherchargesTotal += Convert.ToDecimal(lblOtherCharhes.Text);
                // GrdPrevOtherchargesTotal += Convert.ToDecimal(hdfPrevOtherCharges.Value);
            }
            totoalOtherCharges = Convert.ToDecimal(hdfTotalOtherCharges.Value);
            if (totoalOtherCharges > GrdOtherchargesTotal)
                result = false;
            else
                result = true;
            return result;
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
        #endregion

        #region Set BranchCode Visibility
        private void SetBranchCodeVisibility()
        {
            txtBranchCode.Text = string.Empty;
            txtVatTaxId.Text = string.Empty;

            GetFieldValues(ControlsEnum.VENDORCONTACTYPEDETAILS);

            if (dtAdsTypeDtl != null && dtAdsTypeDtl.Rows.Count > 0)
            {
                if (Convert.ToInt32(dtAdsTypeDtl.Rows[0][Resources.DataFieldRes.vncType]) == (int)VendorContactTypeEnum.Branch)
                {
                    txtBranchCode.Enabled = true;
                    vrfBranchCode.Enabled = true;
                    txtBranchCode.CssClass = "";
                }
                else
                {
                    //txtBranchCode.Enabled = false;
                    //txtBranchCode.Text = "";
                    vrfBranchCode.Enabled = false;
                    //txtBranchCode.CssClass = "input-disabled";
                }

                SetFieldValues(ControlsEnum.VENDORCONTACTYPEDETAILS);
            }

        }
        #endregion

        #region Show Vat Buy popup
        /// <summary>
        /// Function to show VAT BUY popup.
        /// </summary>
        private void ShowVatbuyPopup()
        {
            SetVatbuyNotYetDueRowColor();
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divVatBuy]','" + GetLocalResourceObject("TaxDetails").ToString() + "','916','400');", true);
        }
        #endregion

        #region Show WHT popup
        /// <summary>
        /// Function to show WHT popup.
        /// </summary>
        private void ShowWhtPopup()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("WHTTaxDetails").ToString() + "','916','400');", true);
        }
        #endregion
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

            POPaymentService poPaymentServiceClient;
            poPaymentServiceClient = null;
            int bankPK;
            int mode;
            decimal vatTax = 0;
            decimal InvTotalOtherAmnt = 0;
            decimal whtHdrAmnt = 0;
            decimal whtSplitAmnt = 0;
            string poPK;
            int RptSubType;

            bool bIsChecked = false;
            DropDownList ddlWkfAction;
            string action;

            Label lblBaltopay;
            Label lblCrdrAlcnAmount;

            HiddenField hdfIsApply;
            HiddenField hdfBaltopay;
            HiddenField hdfInvType;
            HiddenField hdfinvPK;
            HiddenField hdfinvCategory;
            HiddenField hdfinvCategoryType;
            POInvoiceService poInvoiceServiceClient;
            poInvoiceServiceClient = null;

            poInvoiceServiceClient = new POInvoiceService();
            poInvoiceServiceClient = CommonFunctions.InitiateClient(poInvoiceServiceClient);
            WorkflowCore.CoreService workflowCore;
            int paymentMpgCount;

            HiddenField hdfInvoicePK;
            HiddenField hdfPaymentMpgPK;
            long selectedInvoicePK;
            List<FIN_PAYMENT_VND_PO_MPG> tempInvoicePOSplitList;
            List<FIN_PAYMENT_VND_TAX_HDR> tempWHTTaxDetails;
            FIN_PAYMENT_VND_TAX_HDR tempWHTTax;
            List<FIN_PAYMENT_VND_ALCN_DTL> tempPaymentAdjnList;
            List<PaymentCrdrMpg> tempPaymentCrdrList;

            FileInfo tempFileInfoObj;
            int selectedItemPK;
            string savePath = string.Empty;
            long? docSaveResult;
            try
            {
                long? result;
                int? alertresult;
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
                    if (((DropDownList)sender).ID == "ddlMode")
                    {
                        commonActions = ActionsEnum.PAYMENT_MODE_INDEX_CHANGED;
                    }
                    else if (((DropDownList)sender).ID == "ddlWHTAccountPopup")
                    {
                        commonActions = ActionsEnum.WHT_ACCOUNT_INDEX_CHANGED_POPUP;
                    }
                    else if (((DropDownList)sender).ID == "ddlVATAccountPopup")
                    {
                        commonActions = ActionsEnum.TAXTYPECHANGED;
                    }
                    else if (((DropDownList)sender).ID == "ddlAddressType")
                    {
                        commonActions = ActionsEnum.CHANGETYPE;
                    }
                    else if (((DropDownList)sender).ID == "ddlBankChargeCurrency")
                    {
                        commonActions = ActionsEnum.CHANGEBANKCURRENCY;
                    }

                }
                else if (sender.GetType().IsEquivalentTo(typeof(CheckBox)))
                {
                    if (((CheckBox)sender).ID == "chkVendorforpayemnt")
                    {
                        commonActions = ActionsEnum.WHT_ACCOUNT_INDEX_CHANGED;
                    }
                    else if (((CheckBox)sender).ID == "chkHeadOffice")
                    {
                        commonActions = ActionsEnum.CHECKEDCHANGED;
                    }
                }
                else if (sender.GetType().IsEquivalentTo(typeof(RadioButton)))
                {
                    if (((RadioButton)sender).ID == "rbtSelect")
                    {
                        commonActions = ActionsEnum.ITEMSELECTED;
                    }
                }
                else if (sender.GetType().IsEquivalentTo(typeof(TextBox)))
                {
                    if (((TextBox)sender).ID == "txtVendorPopup")
                    {
                        commonActions = ActionsEnum.VENDORTEXTCHANGED;
                    }
                    if (((TextBox)sender).ID == "txtAddressType")
                    {
                        commonActions = ActionsEnum.VENDORCONTACTTEXTCHANGED;
                    }

                }


                switch (commonActions)
                {
                    #region ItemSelected
                    case ActionsEnum.ITEMSELECTED:
                        GridViewRow gvr;
                        HiddenField hdfDept;
                        int dept;
                        HiddenField hdfPaymentID;
                        int pk;
                        HiddenField hdfPosted;
                        bool posted;

                        FileDetailsList = null;
                        DocAttachList = null;
                        IsPaymentModeAdded = false;

                        gvr = ((RadioButton)sender).Parent.Parent as GridViewRow;
                        hdfPaymentID = gvr.FindControl("hdfPaymentID") as HiddenField;
                        if (hdfPaymentID != null && int.TryParse(hdfPaymentID.Value, out pk))
                        {
                            CurrPK = pk;
                        }

                        if (Convert.ToInt16(((HiddenField)gvr.FindControl("hdfDelStatus")).Value) == 1)
                        {
                            btnSavePmnt.Visible = false;
                            hdfIsCancelled.Value = "1";
                        }
                        else
                        {
                            btnSavePmnt.Visible = true;
                            hdfIsCancelled.Value = "0";
                        }


                        hdfDept = gvr.FindControl("hdfDept") as HiddenField;// grdShippingPlanList.FindControl("hdfDept") as HiddenField;
                        if (hdfDept != null && int.TryParse(hdfDept.Value, out dept))
                        {
                            Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                            base.SetUserDept();
                        }

                        hdfShowPDC.Value = "0";
                        hdfPosted = gvr.FindControl("hdfPosted") as HiddenField;
                        if (hdfPosted != null && bool.TryParse(hdfPosted.Value, out posted))
                        {
                            if (posted)
                            {
                                HiddenField hdfPDC;
                                HiddenField hdfMode;
                                int pdc = 0;
                                int payMode = 0;
                                hdfPDC = gvr.FindControl("hdfPDC") as HiddenField;
                                hdfMode = gvr.FindControl("hdfMode") as HiddenField;
                                if (hdfPDC != null && int.TryParse(hdfPDC.Value, out pdc))
                                    hdfShowPDC.Value = pdc.ToString();
                                else
                                    hdfShowPDC.Value = "0";

                                if (hdfMode != null && int.TryParse(hdfMode.Value, out payMode))
                                {
                                    if (payMode == (int)BusinessObject.CommonManagement.PaymentModeEnum.Cheque)
                                    {
                                        if (hdfPDC != null && int.TryParse(hdfPDC.Value, out pdc))
                                            hdfShowChequeReturn.Value = pdc.ToString();
                                        else
                                            hdfShowChequeReturn.Value = "1";
                                    }
                                    else
                                    {
                                        hdfShowChequeReturn.Value = "1";
                                    }
                                }
                                else
                                {
                                    hdfShowChequeReturn.Value = "1";
                                }
                            }
                        }

                        workflowCore = new WorkflowCore.CoreService();
                        base.WkfRefID = workflowCore.GetRefID((int)CurrPK, PageProcessID);
                        break;
                    #endregion
                    #region SAVE
                    // Do Action for , when click save button
                    case ActionsEnum.SAVE:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        else//valid
                        {
                            //if ((PaymentModeDetailsList == null || PaymentModeDetailsList.Count == 0) && !IsPaymentModeAdded)
                            //{                                
                            //    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Validate", "$(document).ready(function(){ValidateForPaymentMode();});", true);                               
                            //    return;
                            //}

                            if (CurrPK > 0)
                            {
                                GetFieldValues(ControlsEnum.PAYMENTHDRENTRYBYPK);
                                if (finPaymentVndHdrList != null && finPaymentVndHdrList.Count > 0)
                                {
                                    if (finPaymentVndHdrList[0].PVH_HAS_JRNL_ENTRY)
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Posted").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                        return;
                                    }
                                }
                            }
                            if (VATTaxDetails == null || VATTaxDetails.Count == 0)
                            {
                                GetFieldValues(ControlsEnum.PURINVNOS);
                                GetFieldValues(ControlsEnum.VATPOPUPGRID);
                            }
                            tempVATTaxDetails = null;
                            if (TempVATTaxDetails != null && TempVATTaxDetails.Count > 0)
                            {
                                tempVATTaxDetails = VATTaxDetails = TempVATTaxDetails;
                            }

                            int zeroCount = 0;
                            bool IsValidPayNow = true;
                            bool IsValidPayNowWithBalPay = true;
                            //chek bal pmt
                            //Check Other Charges

                            if (!IsValidPayment())
                            {
                                return;
                            }
                            foreach (GridViewRow gv in grdInvoiceList.Rows)
                            {

                                lblBaltopay = gv.FindControl("lblBaltopay") as Label;
                                TextBox txtPay = gv.FindControl("txtPayNow") as TextBox;
                                //if (Convert.ToDecimal(lblBaltopay.Text) == Convert.ToDecimal(0))
                                //{
                                //    zeroCount = zeroCount + 1;
                                //}
                                if (Convert.ToDecimal(lblBaltopay.Text.Replace(",", "")) > 0 && Convert.ToDecimal(txtPay.Text.Replace(",", "")) == 0)
                                {
                                    IsValidPayNow = false;
                                    break;
                                }
                                if (Convert.ToDecimal(txtPay.Text.Replace(",", "")) > Convert.ToDecimal(lblBaltopay.Text.Replace(",", "")))
                                {
                                    IsValidPayNowWithBalPay = false;
                                    break;
                                }
                                if (Convert.ToDecimal(lblBaltopay.Text.Replace(",", "")) < Convert.ToDecimal(txtPay.Text.Replace(",", "")))
                                {
                                    zeroCount = zeroCount + 1;
                                }
                            }
                            if (!IsValidPayNow)
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save_InvoicePayNow").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                return;
                            }
                            else if (!IsValidPayNowWithBalPay)
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_PayNowExcedsBalPay").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                return;
                            }
                            if ((zeroCount <= 0) || (Iscont == true && (hdfIscontYes.Value == "1")))
                            {
                                Iscont = false; decimal adjAmt = txtAdjAmount.Text != string.Empty ? Convert.ToDecimal(txtAdjAmount.Text) : 0;
                                if (grdInvoiceList.Rows.Count >= 1)
                                {
                                    if (adjAmt > 0 && ddlAdjType.SelectedValue == CommonConstants.SELECTVAL)
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_AdjType").ToString()) + "','" + Resources.Messages.Information + "');", true);

                                    }
                                    else
                                    {
                                        finPaymentVndHdrList = new List<FIN_PAYMENT_VND_HDR>();
                                        poPaymentServiceClient = new POPaymentService();
                                        poPaymentServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(poPaymentServiceClient);
                                        finPaymentVndHdrObj = CommonFunctions.Initilize<FIN_PAYMENT_VND_HDR>();
                                        finPaymentVndHdrObj = (FIN_PAYMENT_VND_HDR)SetUIValuesToObject(ControlsEnum.PAYMENTHDRENTRY);

                                        if (finPaymentVndHdrObj != null)
                                        {
                                            #region Check whether the exchange rate exist for Transaction currency
                                            if (finPaymentVndHdrObj.PVH_EXCHG_RATE <= 0)
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_ExngRate").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                            }
                                            #endregion
                                            #region Check whether the paid amount and paid amount in BC are same
                                            if (finPaymentVndHdrObj.PVH_PAID_AMOUNT != finPaymentVndHdrObj.FIN_PAYMENT_VND_MODE_DTL.Sum(r => r.PDM_PAID_AMOUNT)) // (r.PDM_EXCHG_RATE <= 0 ? 1 : Convert.ToDecimal(r.PDM_EXCHG_RATE))
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("msg_not_tally_paidamountBc").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                                return;
                                            }
                                            #endregion
                                            #region Check whether the Bank charge greater than payable Amount
                                            if (finPaymentVndHdrObj.PVH_PAID_AMOUNT < finPaymentVndHdrObj.FIN_PAYMENT_VND_MODE_DTL.Sum(r => (r.PDM_BANK_CHARGE_CURR == finPaymentVndHdrObj.PVH_CURRENCY) ? r.PDM_BANK_CHARGE : (r.PDM_BANK_CHARGE / Convert.ToDecimal(r.PDM_EXCHG_RATE))))
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("msg_Bankcharge_Exceeds").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                                return;
                                            }
                                            #endregion

                                            TypeRef = finPaymentVndHdrObj.PVH_NO;
                                            paymentMpgCount = 0;
                                            paymentMpgCount = finPaymentVndHdrObj.FIN_PAYMENT_VND_TRX_MPG.ToList().Count;
                                            if (paymentMpgCount > 0)
                                            {
                                                if (CheckPayment(finPaymentVndHdrObj))
                                                {
                                                    if (POGroup != POInvoiceGroup.Expense)
                                                    {
                                                        bool tally = true;
                                                        decimal CrdrAllocationAmnt = 0;
                                                        GetFieldValues(ControlsEnum.PAYMENTTOLERANCE);
                                                        foreach (FIN_PAYMENT_VND_TRX_MPG trxObj in finPaymentVndHdrObj.FIN_PAYMENT_VND_TRX_MPG)
                                                        {
                                                            InvoicePK = trxObj.PVM_INVOICE_HDR.HasValue ? trxObj.PVM_INVOICE_HDR.Value : 0;
                                                            GetFieldValues(ControlsEnum.INVOICEVNDHDR);
                                                            if (finInvoiceVndHdrListForPaymentSplit != null && finInvoiceVndHdrListForPaymentSplit.Count > 0 && finInvoiceVndHdrListForPaymentSplit[0].IVH_IS_OPENING == 1)
                                                            {
                                                                tally = true;
                                                            }
                                                            else
                                                            {
                                                                if (trxObj.FIN_PAYMENT_VND_HDR.PVH_GROUP == Convert.ToInt16(POInvoiceGroup.AgtInvoice))
                                                                {
                                                                    tally = true;
                                                                    break;
                                                                }
                                                                else
                                                                {
                                                                    if (trxObj.PVM_PAID_AMOUNT > 0)// && tempFinPaymentVndPoMpg != null && tempFinPaymentVndPoMpg.Count > 0)
                                                                    {
                                                                        if (PaymentCrdrList != null && PaymentCrdrList.Count > 0)
                                                                        {
                                                                            CrdrAllocationAmnt = PaymentCrdrList.Where(r => r.PNM_INVOICE_HDR == trxObj.PVM_INVOICE_HDR).Sum(sm => sm.PNM_PAID_AMOUNT + sm.PNM_ADJ_AMOUNT);
                                                                            if (((trxObj.PVM_PAID_AMOUNT + trxObj.PVM_ADJUST_AMOUNT) - CrdrAllocationAmnt) <= 0)
                                                                            {
                                                                                tally = true;
                                                                                break;
                                                                            }

                                                                        }
                                                                        if (trxObj.FIN_PAYMENT_VND_PO_MPG != null && trxObj.FIN_PAYMENT_VND_PO_MPG.Count > 0)
                                                                        {
                                                                            decimal PaiWithVariation = 0;
                                                                            //PaiWithVariation = trxObj.PVM_PAID_AMOUNT + (trxObj.PVM_PAID_AMOUNT * PaymentTollerence);
                                                                            PaiWithVariation = trxObj.PVM_PAID_AMOUNT + trxObj.PVM_ADJUST_AMOUNT - CrdrAllocationAmnt + (trxObj.PVM_PAID_AMOUNT * PaymentTollerence);
                                                                            //if (trxObj.PVM_PAID_AMOUNT != trxObj.FIN_PAYMENT_VND_PO_MPG.Sum(dtl => dtl.PPO_PAID_AMOUNT))
                                                                            //{
                                                                            //    tally = false;
                                                                            //    break;
                                                                            //}

                                                                            //Full amount paid againist PO.Invoice created with Excess Quantity
                                                                            //if (trxObj.FIN_PAYMENT_VND_PO_MPG.Sum(dtl => dtl.PPO_PAID_AMOUNT) == 0)
                                                                            //{
                                                                            //    tally = false;                                                                                
                                                                            //    break;
                                                                            //}
                                                                            if (trxObj.FIN_PAYMENT_VND_PO_MPG.Sum(dtl => dtl.PPO_PAID_AMOUNT) > PaiWithVariation)
                                                                            {
                                                                                tally = false;
                                                                                //For Setting/Resetting Colour of a not tallied InvoiceNo
                                                                                hdfNotTalliedInvoicePk.Value = hdfNotTalliedInvoicePk.Value + "," + InvoicePK.ToString();
                                                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowNotTalliedRow", "$(document).ready(function(){SetNotTalliedRowColor();});", true);
                                                                                break;
                                                                            }

                                                                        }
                                                                        else
                                                                        {
                                                                            tally = false;
                                                                            //For Setting/Resetting Colour of a not tallied InvoiceNo
                                                                            hdfNotTalliedInvoicePk.Value = hdfNotTalliedInvoicePk.Value + "," + InvoicePK.ToString();
                                                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowNotTalliedRow", "$(document).ready(function(){SetNotTalliedRowColor();});", true);
                                                                            break;
                                                                        }
                                                                    }
                                                                    else// if (paymentTrxObj.PVM_PAID_AMOUNT <= 0)
                                                                    {

                                                                        //tally = false;
                                                                        //break;

                                                                    }
                                                                }
                                                            }
                                                        }

                                                        if (!tally)
                                                        {
                                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("msg_not_tally").ToString() + "','" + Resources.Messages.Information + "');", true);
                                                            return;
                                                        }
                                                    }


                                                    finPaymentVndHdrList.Add(finPaymentVndHdrObj);
                                                    result = poPaymentServiceClient.SavePaymentHdr(finPaymentVndHdrList);
                                                    //   result = -1;
                                                    if (result >= 0)
                                                    {

                                                        #region ATTACHMENT SAVE
                                                        if (DocAttachList != null && DocAttachList.Count > 0)
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

                                                            foreach (ADM_DOC_ATTACH obj in DocAttachList)
                                                            {
                                                                string[] docName = obj.DOC_PATH.Split('/');
                                                                string filePath = savePath + obj.DOC_NAME;
                                                                if (docName.Length > 0)
                                                                    filePath = savePath + docName[docName.Length - 1];
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
                                                            docSaveResult = poInvoiceServiceClient.SaveDocAttachemts(DocAttachList, (int)result);
                                                        }
                                                        #endregion

                                                        #region Generate dummy entry
                                                        if (finPaymentVndHdrList[0].PVH_PK > 0 && finPaymentVndHdrList[0].PVH_STATUS == (byte)DbStatus.APPROVED)
                                                        {
                                                            FinTrxService finTrxServiceClient;
                                                            finTrxServiceClient = new FinTrxService();
                                                            string refType = string.Empty;
                                                            if (finPaymentVndHdrList[0].PVH_GROUP == 1) // goods
                                                            {
                                                                refType = ApplicationType.VPJ;
                                                            }
                                                            else if (finPaymentVndHdrList[0].PVH_GROUP == 2) // service
                                                            {
                                                                refType = ApplicationType.SIPJ;
                                                            }
                                                            else if (finPaymentVndHdrList[0].PVH_GROUP == 3) // expense
                                                            {
                                                                refType = ApplicationType.EIPJ;
                                                            }
                                                            else if (finPaymentVndHdrList[0].PVH_GROUP == 4)  // agent
                                                            {
                                                                refType = ApplicationType.AIPJ;
                                                            }

                                                            long DummyResult = finPaymentVndHdrList[0].PVH_PK;
                                                            bool IsDummyEntry = finTrxServiceClient.IsDummyEntry(refType, (int)finPaymentVndHdrList[0].PVH_PK, 0);
                                                            if (IsDummyEntry == true)
                                                            {
                                                                DummyResult = finTrxServiceClient.DeleteFinTrx(refType, (int)finPaymentVndHdrList[0].PVH_PK, 0);
                                                            }

                                                            if (DummyResult > 0)
                                                            {
                                                                finTrxServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(finTrxServiceClient);
                                                                DummyResult = finTrxServiceClient.GenerateDummyEntry((int)finPaymentVndHdrList[0].PVH_PK, refType);
                                                            }
                                                        }
                                                        #endregion

                                                        CurrPK = (long)result;
                                                        GetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                                                        FIN_PAYMENT_VND_HDR tempFinPaymentVndHdrObj = null;
                                                        if (finPaymentVndHdrList != null && finPaymentVndHdrList.Count > 0)
                                                        {
                                                            LastModifiedTime = finPaymentVndHdrList.FirstOrDefault(aa => aa.PVH_PK == CurrPK) == null ? DateTime.Now
                                                                : finPaymentVndHdrList.FirstOrDefault(aa => aa.PVH_PK == CurrPK).PVH_MOD_DT;
                                                            tempFinPaymentVndHdrObj = finPaymentVndHdrList.FirstOrDefault(aa => aa.PVH_PK == CurrPK) == null ? finPaymentVndHdrObj
                                                                : finPaymentVndHdrList.FirstOrDefault(aa => aa.PVH_PK == CurrPK);
                                                        }

                                                        //#region ALERTSAVE
                                                        //GetFieldValues(ControlsEnum.ALERTCONFIG);
                                                        //int isAlert = 0;
                                                        //if (admAppConstMstList != null && admAppConstMstList.Count > 0)
                                                        //{
                                                        //    isAlert = admAppConstMstList[0].ACF_VALUE;
                                                        //}
                                                        //if (chkPDC.Checked && isAlert == 1)
                                                        //{
                                                        //    foreach (FIN_PAYMENT_VND_TRX_MPG paymentTrxObj in tempFinPaymentVndHdrObj.FIN_PAYMENT_VND_TRX_MPG)
                                                        //    {
                                                        //        invPK = Convert.ToInt32(paymentTrxObj.FIN_INVOICE_VND_HDR.IVH_PK);
                                                        //        AlertBO alertBoObj = new AlertBO();
                                                        //        alertBoObj = (AlertBO)SetUIValuesToObject(ControlsEnum.ALERTSAVE);
                                                        //        if (alertBoObj != null)
                                                        //        {
                                                        //            alertresult = BusinessLogic.AlertManagement.Alerts.SaveAlertDetails(alertBoObj);
                                                        //        }
                                                        //    }
                                                        //}
                                                        //#endregion

                                                        SelectedInvoices = null;
                                                        litErrorMsg.Text = Resources.Messages.Msg_Save_Success;
                                                        litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.PaymentHdr);
                                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                                        EntryStatus = EntryStatus.LISTMODE;
                                                        ResetForm();
                                                        GetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                                                        SetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                                                        btnNew.Focus();

                                                        //GetFieldValues(ControlsEnum.UPLOADEDFILES);
                                                        //SetFieldValues(ControlsEnum.UPLOADEDFILES);

                                                        //CurrPK = (long)result;
                                                        //GetFieldValues(ControlsEnum.PAYMENTSPLITLISTBYPAYMENTPK);
                                                        //if (finPaymentVndPoMpgList != null && finPaymentVndPoMpgList.Count > 0
                                                        //    && finPaymentVndPoMpgList.Count == paymentMpgCount)
                                                        //{
                                                        //    litErrorMsg.Text = Resources.Messages.Msg_Save_Success;
                                                        //    litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.PaymentHdr);
                                                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                                        //    EntryStatus = EntryStatus.LISTMODE;
                                                        //    ResetForm();
                                                        //    GetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                                                        //    SetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                                                        //    btnNew.Focus();
                                                        //}
                                                        //else
                                                        //{
                                                        //    GetFieldValues(ControlsEnum.PAYMENTHDRENTRYBYPK);
                                                        //    GetUIValuesFromObject(ControlsEnum.PAYMENTHDRENTRY);
                                                        //    GetFieldValues(ControlsEnum.PAYMENTHDRINVLISTBYPK);
                                                        //    SetFieldValues(ControlsEnum.PAYMENTMPGLIST);
                                                        //    EntryStatus = EntryStatus.ENTRYMODE;
                                                        //    workflowCore = new WorkflowCore.CoreService();
                                                        //    base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID((int)CurrPK, ucrWrkf.ProcessID);
                                                        //    ucrWrkf.FillWorkFlowDetails();
                                                        //    if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                                                        //    {
                                                        //        ucrWrkf.ViewType = 1;
                                                        //        //btnSubmit.Visible = true;
                                                        //        //btnSave.Visible = true;
                                                        //        //divbtnSavePaymentSplit.Visible = true;
                                                        //    }
                                                        //    else
                                                        //    {
                                                        //        ucrWrkf.ViewType = 0;
                                                        //        //EntryStatus = EntryStatus.VIEWMODE;
                                                        //        //btnSubmit.Visible = false;
                                                        //        //btnSave.Visible = false;
                                                        //        // divbtnSavePaymentSplit.Visible = false;
                                                        //    }
                                                        //    ModifiedDatePnl.Visible = true;
                                                        //    mode = String.IsNullOrEmpty(ddlMode.SelectedValue) ? 0 : Convert.ToInt32(ddlMode.SelectedValue);

                                                        //    switch (mode)
                                                        //    {
                                                        //        case (int)PaymentModeEnum.CASH:
                                                        //            vrfBranch.Enabled = false;
                                                        //            vrfAccountNo.Enabled = false;
                                                        //            vrfInstrumentNo.Enabled = false;
                                                        //            vrfInstrumentDate.Enabled = false;
                                                        //            vrfFavourof.Enabled = false;
                                                        //            txtInstrumentNo.Enabled = false;
                                                        //            txtInstrumentDate.Enabled = false;
                                                        //            txtFavourof.Enabled = false;
                                                        //            txtInstrumentNo.CssClass = "Uiinput-amount input-disabled medium";
                                                        //            txtInstrumentDate.CssClass = "Uidate-picker input-disabled";
                                                        //            txtFavourof.CssClass = "multiline-2line input-disabled";
                                                        //            break;
                                                        //        default:
                                                        //            vrfBranch.Enabled = true;
                                                        //            vrfAccountNo.Enabled = true;
                                                        //            vrfInstrumentNo.Enabled = true;
                                                        //            vrfInstrumentDate.Enabled = true;
                                                        //            vrfFavourof.Enabled = true;
                                                        //            txtInstrumentNo.Enabled = true;
                                                        //            txtInstrumentDate.Enabled = true;
                                                        //            txtFavourof.Enabled = true;
                                                        //            txtInstrumentNo.CssClass = "Uiinput-amount medium";
                                                        //            txtInstrumentDate.CssClass = "Uidate-picker";
                                                        //            txtFavourof.CssClass = "multiline-2line";
                                                        //            break;
                                                        //    }
                                                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("msg_allocation").ToString() + "','" + Resources.Messages.Information + "');", true);
                                                        //}
                                                    }
                                                    else if (result.Value == (int)DbSaveStatus.AMOUNTEXCEEDS)
                                                    {
                                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("AmountExceeds").ToString())
                                                        + "','" + Resources.ErpRes.Information + "');", true);
                                                    }
                                                }
                                                else
                                                {
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save_PaymentDuplicate").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                                }

                                            }
                                            else
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save_InvoicePayNow").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save_InvoiceCount").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                }
                            }
                            else
                            {
                                Iscont = true;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowAlreadyPaid", "$(document).ready(function(){ShowAlreadyPaid();});", true);

                            }
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideOverlay", "HideOverlay();", true);
                        }
                        break;
                    #endregion
                    #region New
                    case ActionsEnum.NEW:
                        if (SelectedInvoices != null)
                        {
                            selectedInvoiceList = SelectedInvoices;
                            GetFieldValues(ControlsEnum.PAYMENTMPGLIST);
                            SetFieldValues(ControlsEnum.PAYMENTMPGLIST);
                            ModifiedDatePnl.Visible = false;
                            EntryStatus = EntryStatus.NEWMODE;
                            updatePayment = false;
                            GetFieldValues(ControlsEnum.PAYMENTNO);
                            lblPaymentNo.Text = hdfPaymentNo.Value;
                            txtPaymentDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                            mode = String.IsNullOrEmpty(ddlMode.SelectedValue) ? 0 : Convert.ToInt32(ddlMode.SelectedValue);
                            SetPaymentModeDetails(mode);
                            //switch (mode)
                            //{
                            //    case (int)PaymentModeEnum.CASH:
                            //        vrfBranch.Enabled = vrfBranchHdr.Enabled = false;
                            //        vrfAccountNo.Enabled = vrfAccountNoHdr.Enabled = false;
                            //        vrfInstrumentNo.Enabled = vrfInstrumentNoHdr.Enabled = false;
                            //        vrfInstrumentDate.Enabled = vrfInstrumentDateHdr.Enabled = false;
                            //        vrfFavourof.Enabled = vrfFavourofHdr.Enabled = false;
                            //        txtInstrumentNo.Enabled = false;
                            //        txtInstrumentDate.Enabled = false;
                            //        txtFavourof.Enabled = false;
                            //        txtInstrumentNo.CssClass = "Uiinput-amount input-disabled medium";
                            //        txtInstrumentDate.CssClass = "Uidate-picker input-disabled";
                            //        txtFavourof.CssClass = "multiline-2line input-disabled";
                            //        Label5.Visible = false;
                            //        txtBankCharge.Visible = false;
                            //        chkBankCharge.Visible = false;
                            //        break;
                            //    case (int)PaymentModeEnum.OTHERS:
                            //        vrfBranch.Enabled = vrfBranchHdr.Enabled = false;
                            //        vrfAccountNo.Enabled = vrfAccountNoHdr.Enabled = false;
                            //        vrfInstrumentNo.Enabled = vrfInstrumentNoHdr.Enabled = false;
                            //        vrfInstrumentDate.Enabled = vrfInstrumentDateHdr.Enabled = false;
                            //        vrfFavourof.Enabled = vrfFavourofHdr.Enabled = false;
                            //        txtInstrumentNo.Enabled = false;
                            //        txtInstrumentDate.Enabled = false;
                            //        txtFavourof.Enabled = false;
                            //        txtInstrumentNo.CssClass = "Uiinput-amount input-disabled medium";
                            //        txtInstrumentDate.CssClass = "Uidate-picker input-disabled";
                            //        txtFavourof.CssClass = "multiline-2line input-disabled";
                            //        Label5.Visible = false;
                            //        txtBankCharge.Visible = false;
                            //        chkBankCharge.Visible = false;
                            //        txtPaymentBank.Text = string.Empty;
                            //        txtPaymentBank.Enabled = false;
                            //        txtPaymentBank.CssClass = "input-disabled";
                            //        hdfPaymentBank.Value = string.Empty;
                            //        break;
                            //    default:
                            //        vrfBranch.Enabled = vrfBranchHdr.Enabled = true;
                            //        vrfAccountNo.Enabled = vrfAccountNoHdr.Enabled = true;
                            //        vrfInstrumentNo.Enabled = vrfInstrumentNoHdr.Enabled = true;
                            //        vrfInstrumentDate.Enabled = vrfInstrumentDateHdr.Enabled = true;
                            //        vrfFavourof.Enabled = vrfFavourofHdr.Enabled = true;
                            //        txtInstrumentNo.Enabled = true;
                            //        txtInstrumentDate.Enabled = true;
                            //        txtFavourof.Enabled = true;
                            //        txtInstrumentNo.CssClass = "Uiinput-amount medium";
                            //        txtInstrumentDate.CssClass = "Uidate-picker";
                            //        txtFavourof.CssClass = "multiline-2line";
                            //        Label5.Visible = true;
                            //        txtBankCharge.Visible = true;
                            //        chkBankCharge.Visible = true;
                            //        break;
                            //}
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_New_Payment").ToString()) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region SEARCH
                    case ActionsEnum.SEARCH:
                        if (string.IsNullOrEmpty(txtVendor.Text) || txtVendor.Text.Equals(Resources.ErpRes.AutoDefaultValue))
                        {
                            hdfVendorID.Value = "0";
                        }
                        PageIndex = "1";
                        GetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                        SetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                        break;
                    #endregion
                    #region Cancel
                    case ActionsEnum.CANCEL:
                        FillProcessID(1);
                        ResetForm();
                        GetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                        SetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                        this.btnNew.Focus();
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion
                    #region Edit
                    case ActionsEnum.EDIT:
                        FillProcessID(1);
                        SetUIEditView(commonActions);
                        ModifiedDatePnl.Visible = true;
                        mode = String.IsNullOrEmpty(ddlMode.SelectedValue) ? 0 : Convert.ToInt32(ddlMode.SelectedValue);
                        SetFieldValues(ControlsEnum.PAYMENTMODESGRID);
                        SetPaymentModeDetails(mode);
                        //switch (mode)
                        //{
                        //    case (int)PaymentModeEnum.CASH:
                        //        vrfBranch.Enabled = vrfBranchHdr.Enabled = false;
                        //        vrfAccountNo.Enabled = vrfAccountNoHdr.Enabled = false;
                        //        vrfInstrumentNo.Enabled = vrfInstrumentNoHdr.Enabled = false;
                        //        vrfInstrumentDate.Enabled = vrfInstrumentDateHdr.Enabled = false;
                        //        vrfFavourof.Enabled = vrfFavourofHdr.Enabled = false;
                        //        txtInstrumentNo.Enabled = false;
                        //        txtInstrumentDate.Enabled = false;
                        //        txtFavourof.Enabled = false;
                        //        txtFavourof.Text = string.Empty;
                        //        txtInstrumentNo.CssClass = "Uiinput-amount input-disabled medium";
                        //        txtInstrumentDate.CssClass = "Uidate-picker input-disabled";
                        //        txtFavourof.CssClass = "multiline-2line input-disabled";
                        //        Label5.Visible = false;
                        //        txtBankCharge.Visible = false;
                        //        chkBankCharge.Visible = false;
                        //        break;
                        //    case (int)PaymentModeEnum.OTHERS:
                        //        vrfBranch.Enabled = vrfBranchHdr.Enabled = false;
                        //        vrfAccountNo.Enabled = vrfAccountNoHdr.Enabled = false;
                        //        vrfInstrumentNo.Enabled = vrfInstrumentNoHdr.Enabled = false;
                        //        vrfInstrumentDate.Enabled = vrfInstrumentDateHdr.Enabled = false;
                        //        vrfFavourof.Enabled = vrfFavourofHdr.Enabled = false;
                        //        txtInstrumentNo.Enabled = false;
                        //        txtInstrumentDate.Enabled = false;
                        //        txtFavourof.Enabled = false;
                        //        txtFavourof.Text = string.Empty;
                        //        txtInstrumentNo.CssClass = "Uiinput-amount input-disabled medium";
                        //        txtInstrumentDate.CssClass = "Uidate-picker input-disabled";
                        //        txtFavourof.CssClass = "multiline-2line input-disabled";
                        //        Label5.Visible = false;
                        //        txtBankCharge.Visible = false;
                        //        chkBankCharge.Visible = false;
                        //        txtPaymentBank.Text = string.Empty;
                        //        txtPaymentBank.Enabled = false;
                        //        txtPaymentBank.CssClass = "input-disabled";
                        //        hdfPaymentBank.Value = string.Empty;
                        //        break;
                        //    default:
                        //        vrfBranch.Enabled = vrfBranchHdr.Enabled = true;
                        //        vrfAccountNo.Enabled = vrfAccountNoHdr.Enabled = true;
                        //        vrfInstrumentNo.Enabled = vrfInstrumentNoHdr.Enabled = true;
                        //        vrfInstrumentDate.Enabled = vrfInstrumentDateHdr.Enabled = true;
                        //        vrfFavourof.Enabled = vrfFavourofHdr.Enabled = true;
                        //        txtInstrumentNo.Enabled = true;
                        //        txtInstrumentDate.Enabled = true;
                        //        txtFavourof.Enabled = true;
                        //        txtFavourof.Text = lblCustomerTxt.ToolTip;
                        //        txtInstrumentNo.CssClass = "Uiinput-amount medium";
                        //        txtInstrumentDate.CssClass = "Uidate-picker";
                        //        txtFavourof.CssClass = "multiline-2line";
                        //        Label5.Visible = true;
                        //        txtBankCharge.Visible = true;
                        //        chkBankCharge.Visible = true;
                        //        break;
                        //}
                        break;
                    #endregion
                    #region View
                    case ActionsEnum.VIEW:
                        SetUIEditView(commonActions);
                        SetFieldValues(ControlsEnum.PAYMENTMODESGRID);
                        btnCancel.Focus();
                        break;
                    #endregion
                    #region Clear
                    case ActionsEnum.CLEAR:
                        ResetForm();
                        PageIndex = "1";
                        GetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                        SetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                        break;
                    #endregion
                    #region Remove
                    case ActionsEnum.REMOVE:
                        int INVPk = int.Parse(((Button)sender).CommandArgument.ToString());
                        if (CurrPK == 0)
                        {
                            //SelectedInvoices.Remove(INVPk);
                            //selectedInvoiceList = SelectedInvoices;
                            selectedInvoiceList.Remove(INVPk);
                        }
                        SetUIValuesToObject(ControlsEnum.INVOICELIST);
                        if (finInvoiceHdrList != null && finInvoiceHdrList.Count > 0 && CurrPK == 0)//EditedInvoices != null
                        {
                            finInvoiceVndHdrList = finInvoiceHdrList;
                            finInvoiceVndHdrObj = CommonFunctions.Initilize<ERPData.FIN_INVOICE_VND_HDR>();
                            finInvoiceVndHdrObj = finInvoiceHdrList.SingleOrDefault(ivh => ivh.IVH_PK == INVPk);
                            if (finInvoiceVndHdrObj != null)
                            {
                                if (finInvoiceHdrList.Count > 1)
                                {
                                    finInvoiceVndHdrList.Remove(finInvoiceVndHdrObj);
                                    //EditedInvoices = finInvoiceVndHdrList;
                                    finInvoiceHdrList = finInvoiceVndHdrList;
                                    FinInvoiceVndHdrSelectedList = finInvoiceVndHdrList;
                                    SetFieldValues(ControlsEnum.PAYMENTMPGLIST);
                                    #region Remove VAT TAX
                                    if (TempVATTaxDetails != null && TempVATTaxDetails.Count > 0)
                                    {
                                        List<FIN_PAYMENT_VND_TAX_HDR> taxHrdList = TempVATTaxDetails.Where(r => r.WTH_PUR_INVOICE.Value == INVPk).ToList();
                                        foreach (FIN_PAYMENT_VND_TAX_HDR objtaxHrd in taxHrdList)
                                        {
                                            TempVATTaxDetails.Remove(objtaxHrd);
                                        }
                                    }
                                    #endregion
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("MsgErr_Atleast_One_inv").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                }
                            }
                        }
                        else if (EditedPaymentDtls != null)
                        {
                            finPaymentVndTrxMpgList = EditedPaymentDtls;
                            finPaymentVndTrxMpgObj = CommonFunctions.Initilize<ERPData.FIN_PAYMENT_VND_TRX_MPG>();
                            finPaymentVndTrxMpgObj = finPaymentVndTrxMpgList.SingleOrDefault(pvm => pvm.PVM_INVOICE_HDR == INVPk);
                            if (finPaymentVndTrxMpgObj != null)
                            {
                                if (finPaymentVndTrxMpgList.Count > 1)
                                {
                                    finPaymentVndTrxMpgList.Remove(finPaymentVndTrxMpgObj);
                                    EditedPaymentDtls = finPaymentVndTrxMpgList;
                                    finPaymentTrxMpgList = finPaymentVndTrxMpgList;
                                    FinInvoiceVndHdrSelectedList = new List<FIN_INVOICE_VND_HDR>();
                                    finPaymentVndTrxMpgList.ForEach(dtl => FinInvoiceVndHdrSelectedList.Add(dtl.FIN_INVOICE_VND_HDR));
                                    SetFieldValues(ControlsEnum.PAYMENTMPGLIST);
                                    #region Remove VAT TAX
                                    if (TempVATTaxDetails != null && TempVATTaxDetails.Count > 0)
                                    {
                                        List<FIN_PAYMENT_VND_TAX_HDR> taxHrdList = TempVATTaxDetails.Where(r => r.WTH_PUR_INVOICE.Value == INVPk).ToList();
                                        foreach (FIN_PAYMENT_VND_TAX_HDR objtaxHrd in taxHrdList)
                                        {
                                            TempVATTaxDetails.Remove(objtaxHrd);
                                        }
                                    }
                                    #endregion
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("MsgErr_Atleast_One_inv").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                }
                            }
                        }

                        #region Remove Credit Notes
                        if (PaymentCrdrList != null && PaymentCrdrList.Count > 0)
                        {
                            List<PaymentCrdrMpg> pmntMpgCrdr = PaymentCrdrList.Where(r => r.PNM_INVOICE_HDR == INVPk).ToList();
                            if (pmntMpgCrdr != null && pmntMpgCrdr.Count > 0)
                            {
                                PaymentCrdrList.Remove(pmntMpgCrdr[0]);
                            }
                        }
                        #endregion

                        #region Remove Adjustments
                        if (PaymentAdjnList != null && PaymentAdjnList.Count > 0)
                        {
                            List<FIN_PAYMENT_VND_ALCN_DTL> pmntAdjAlcn = PaymentAdjnList.Where(r => r.FIN_PAYMENT_VND_TRX_MPG.PVM_INVOICE_HDR == INVPk && r.PAD_AMOUNT > 0).ToList();
                            if (pmntAdjAlcn != null && pmntAdjAlcn.Count > 0)
                            {
                                PaymentAdjnList.Remove(pmntAdjAlcn[0]);
                            }
                        }
                        #endregion

                        break;
                    #endregion
                    #region Account Number
                    case ActionsEnum.SEARCHACCOUNTNO:
                        if (!string.IsNullOrEmpty(hdfPaymentBank.Value) && Convert.ToInt32(hdfPaymentBank.Value.ToString()) > 0)
                        {
                            GetFieldValues(ControlsEnum.BANK);
                            if (finCashBankMstList != null && finCashBankMstList.Count > 0)
                            {
                                if (Convert.ToInt32(ddlMode.SelectedValue) != (int)PaymentModeEnum.CASH)
                                {
                                    txtAccountNo.Text = finCashBankMstList[0].CBM_ACC_NO;
                                    txtBranch.Text = finCashBankMstList[0].CBM_BRANCH;
                                    if (Convert.ToInt32(ddlMode.SelectedValue) == (int)PaymentModeEnum.DD)
                                    {
                                        hdfFavourof.Value = txtFavourof.Text = finCashBankMstList[0].CBM_NAME;
                                    }
                                }
                                hdfBankAccount.Value = finCashBankMstList[0].CBM_ACCOUNT.ToString();
                            }
                            else
                            {
                                txtAccountNo.Text = string.Empty;
                                txtBranch.Text = string.Empty;
                                hdfBankAccount.Value = string.Empty;
                            }
                        }
                        else
                        {
                            txtAccountNo.Text = string.Empty;
                            txtBranch.Text = string.Empty;
                            hdfBankAccount.Value = string.Empty;
                        }
                        break;
                    #endregion
                    #region Payment mode changed
                    case ActionsEnum.PAYMENT_MODE_INDEX_CHANGED:
                        mode = String.IsNullOrEmpty(ddlMode.SelectedValue) ? 0 : Convert.ToInt32(ddlMode.SelectedValue);
                        txtPaymentBank.Text = string.Empty;
                        hdfPaymentBank.Value = string.Empty;
                        txtBranch.Text = string.Empty;
                        txtAccountNo.Text = string.Empty;
                        txtInstrumentNo.Text = string.Empty;
                        txtInstrumentDate.Text = string.Empty;
                        txtFavourof.Text = string.Empty;
                        txtBankCharge.Text = GetFormattedCurrency(0);
                        chkBankCharge.Checked = false;
                        chkPDC.Checked = false;
                        if (GetGlobalResourceObject("ConfigurationsRes", "PPCReconciliation").ToString() == "1")
                        {
                            chkPDC.Checked = true;
                            chkPDC.Disabled = true;
                        }

                        SetPaymentModeDetails(mode);
                        if (mode != (int)PaymentModeEnum.CASH && mode != (int)PaymentModeEnum.OTHERS)
                            txtFavourof.Text = HttpUtility.HtmlDecode(lblCustomerTxt.ToolTip);
                        //switch (mode)
                        //{
                        //    case (int)PaymentModeEnum.CASH:
                        //        vrfBranch.Enabled = vrfBranchHdr.Enabled = false;
                        //        vrfAccountNo.Enabled = vrfAccountNoHdr.Enabled = false;
                        //        vrfInstrumentNo.Enabled = vrfInstrumentNoHdr.Enabled = false;
                        //        vrfInstrumentDate.Enabled = vrfInstrumentDateHdr.Enabled = false;
                        //        vrfFavourof.Enabled = vrfFavourofHdr.Enabled = false;
                        //        txtInstrumentNo.Enabled = false;
                        //        txtInstrumentDate.Enabled = false;
                        //        txtFavourof.Enabled = false;
                        //        txtFavourof.Text = string.Empty;
                        //        txtInstrumentNo.CssClass = "Uiinput-amount input-disabled medium";
                        //        txtInstrumentDate.CssClass = "Uidate-picker input-disabled";
                        //        txtFavourof.CssClass = "multiline-2line input-disabled";
                        //        Label5.Visible = false;
                        //        txtBankCharge.Visible = false;
                        //        chkBankCharge.Visible = false;
                        //        break;
                        //    case (int)PaymentModeEnum.OTHERS:
                        //        vrfBranch.Enabled = vrfBranchHdr.Enabled = false;
                        //        vrfAccountNo.Enabled = vrfAccountNoHdr.Enabled = false;
                        //        vrfInstrumentNo.Enabled = vrfInstrumentNoHdr.Enabled = false;
                        //        vrfInstrumentDate.Enabled = vrfInstrumentDateHdr.Enabled = false;
                        //        vrfFavourof.Enabled = vrfFavourofHdr.Enabled = false;
                        //        txtInstrumentNo.Enabled = false;
                        //        txtInstrumentDate.Enabled = false;
                        //        txtFavourof.Enabled = false;
                        //        txtFavourof.Text = string.Empty;
                        //        txtInstrumentNo.CssClass = "Uiinput-amount input-disabled medium";
                        //        txtInstrumentDate.CssClass = "Uidate-picker input-disabled";
                        //        txtFavourof.CssClass = "multiline-2line input-disabled";
                        //        Label5.Visible = false;
                        //        txtBankCharge.Visible = false;
                        //        chkBankCharge.Visible = false;
                        //        txtPaymentBank.Text = string.Empty;
                        //        txtPaymentBank.Enabled = false;
                        //        txtPaymentBank.CssClass = "input-disabled";
                        //        hdfPaymentBank.Value = string.Empty;
                        //        break;
                        //    default:
                        //        vrfBranch.Enabled = vrfBranchHdr.Enabled = true;
                        //        vrfAccountNo.Enabled = vrfAccountNoHdr.Enabled = true;
                        //        vrfInstrumentNo.Enabled = vrfInstrumentNoHdr.Enabled = true;
                        //        vrfInstrumentDate.Enabled = vrfInstrumentDateHdr.Enabled = true;
                        //        vrfFavourof.Enabled = vrfFavourofHdr.Enabled = true;
                        //        txtInstrumentNo.Enabled = true;
                        //        txtInstrumentDate.Enabled = true;
                        //        txtFavourof.Enabled = true;
                        //        txtFavourof.Text = lblCustomerTxt.ToolTip;
                        //        txtInstrumentNo.CssClass = "Uiinput-amount medium";
                        //        txtInstrumentDate.CssClass = "Uidate-picker";
                        //        txtFavourof.CssClass = "multiline-2line";
                        //        Label5.Visible = true;
                        //        txtBankCharge.Visible = true;
                        //        chkBankCharge.Visible = true;
                        //        break;
                        //}
                        break;

                    #endregion
                    #region Delete
                    case ActionsEnum.DELETE:
                        if (grdInvoiceList.Rows.Count > 0)
                        {
                            poPaymentServiceClient = new POPaymentService();
                            poPaymentServiceClient = CommonFunctions.InitiateClient(poPaymentServiceClient);
                            result = poPaymentServiceClient.DeletePaymentHdr(CurrPK);
                            if (result > 0)
                            {
                                EntryStatus = EntryStatus.LISTMODE;
                                ResetForm();
                                GetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                                SetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                                btnNew.Focus();
                                litErrorMsg.Text = Resources.Messages.Msg_Delete_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.PaymentHdr);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);

                            }
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_ErrSave_Invoice").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideOverlay", "HideOverlay();", true);
                        break;
                    #endregion
                    #region ExchangeRate
                    case ActionsEnum.EXCHANGERATE:
                        if (!string.IsNullOrEmpty(hdfPaymentCurrency.Value))
                        {
                            GetFieldValues(ControlsEnum.EXCHANGERATE);
                            double exchangeRate = string.IsNullOrEmpty(hdfExchangeCurr.Value) ? 1 : Convert.ToDouble(hdfExchangeCurr.Value);
                            double paidAmount = string.IsNullOrEmpty(txtPaidAmount.Text.Trim()) ? 0 * exchangeRate : Convert.ToDouble(txtPaidAmount.Text.Trim()) * exchangeRate;
                            txtPaidAmount.Text = paidAmount.ToString();
                        }
                        break;
                    #endregion
                    #region ExchangeRate
                    case ActionsEnum.CALCURRENCY:
                        if (!string.IsNullOrEmpty(hdfPaymentCurrency.Value))
                        {
                            GetFieldValues(ControlsEnum.EXCHANGERATE);
                            double exchangeRate = string.IsNullOrEmpty(hdfExchangeCurrBC.Value) ? 1 : Convert.ToDouble(hdfExchangeCurrBC.Value);
                            double paidAmount = string.IsNullOrEmpty(txtPaidAmount.Text.Trim()) ? 0 * exchangeRate : Convert.ToDouble(txtPaidAmount.Text.Trim()) * exchangeRate;
                            txtPaidAmount.Text = paidAmount.ToString();
                        }
                        break;
                    #endregion
                    #region Tabs
                    case ActionsEnum.POINVOICE:
                        PaymentFlag = 1;
                        CheckUserRightsAndRedirect(Resources.PageURL.PurchaseOrderInvoicing);
                        //Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.PurchaseOrderInvoicing), false);
                        break;
                    case ActionsEnum.INVOICE:
                        CheckUserRightsAndRedirect(Resources.PageURL.PoInvoicing);
                        //Response.Redirect(Resources.PageURL.PoInvoicing);
                        break;
                    case ActionsEnum.PAYMENT:
                        CheckUserRightsAndRedirect(Resources.PageURL.PoPayment);
                        //Response.Redirect(Resources.PageURL.PoPayment);
                        break;
                    case ActionsEnum.DEFAULT:
                        CheckUserRightsAndRedirect(Resources.PageURL.PoListing);
                        //Response.Redirect(Resources.PageURL.PoListing);
                        break;
                    case ActionsEnum.CRDRNOTE:
                        Session[ERP.Utilities.SessionStrings.CrDrType] = ApplicationType.PI;
                        CheckUserRightsAndRedirect(Resources.PageURL.DrCrNote);
                        //Response.Redirect(Resources.PageURL.DrCrNote);
                        break;
                    case ActionsEnum.ACPAYABLES:
                        foreach (GridViewRow grdrow in grdPOPaymentHdr.Rows)
                        {
                            RadioButton rbtn = (RadioButton)grdrow.FindControl(Resources.Report.RadioButtonID);
                            if (rbtn.Checked)
                            {
                                Session[ERP.Utilities.SessionStrings.VendorPK] = ((HiddenField)grdrow.FindControl("hdfVendorPK")).Value;
                                Session[ERP.Utilities.SessionStrings.Vendor] = ((Label)grdrow.FindControl("lblVendor")).ToolTip;//For Showing name in vendorddl of AccountPayable Page Completely 
                                break;
                            }
                        }
                        CheckUserRightsAndRedirect(Resources.PageURL.AccountsPayable);
                        //Response.Redirect(Resources.PageURL.AccountsPayable);
                        break;
                    case ActionsEnum.EXPENSES:
                        CheckUserRightsAndRedirect(Resources.PageURL.ExpenseInvoice);
                        //Response.Redirect(Resources.PageURL.ExpenseInvoice);
                        break;
                    #endregion
                    #region Payment
                    case ActionsEnum.PAYMENTLIST:
                        FillProcessID(1);
                        ResetForm();
                        GetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                        SetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                        this.btnNew.Focus();
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    case ActionsEnum.PAYMENTDETAIL:
                        SetUIEditView(commonActions);
                        ModifiedDatePnl.Visible = true;
                        mode = String.IsNullOrEmpty(ddlMode.SelectedValue) ? 0 : Convert.ToInt32(ddlMode.SelectedValue);
                        SetFieldValues(ControlsEnum.PAYMENTMODESGRID);
                        SetPaymentModeDetails(mode);
                        //switch (mode)
                        //{
                        //    case (int)PaymentModeEnum.CASH:
                        //        vrfBranch.Enabled = vrfBranchHdr.Enabled = false;
                        //        vrfAccountNo.Enabled = vrfAccountNoHdr.Enabled = false;
                        //        vrfInstrumentNo.Enabled = vrfInstrumentNoHdr.Enabled = false;
                        //        vrfInstrumentDate.Enabled = vrfInstrumentDateHdr.Enabled = false;
                        //        vrfFavourof.Enabled = vrfFavourofHdr.Enabled = false;
                        //        txtInstrumentNo.Enabled = false;
                        //        txtInstrumentDate.Enabled = false;
                        //        txtFavourof.Enabled = false;
                        //        txtInstrumentNo.CssClass = "Uiinput-amount input-disabled medium";
                        //        txtInstrumentDate.CssClass = "Uidate-picker input-disabled";
                        //        txtFavourof.CssClass = "multiline-2line input-disabled";
                        //        Label5.Visible = false;
                        //        txtBankCharge.Visible = false;
                        //        chkBankCharge.Visible = false;
                        //        break;
                        //    case (int)PaymentModeEnum.OTHERS:
                        //        vrfBranch.Enabled = vrfBranchHdr.Enabled = false;
                        //        vrfAccountNo.Enabled = vrfAccountNoHdr.Enabled = false;
                        //        vrfInstrumentNo.Enabled = vrfInstrumentNoHdr.Enabled = false;
                        //        vrfInstrumentDate.Enabled = vrfInstrumentDateHdr.Enabled = false;
                        //        vrfFavourof.Enabled = vrfFavourofHdr.Enabled = false;
                        //        txtInstrumentNo.Enabled = false;
                        //        txtInstrumentDate.Enabled = false;
                        //        txtFavourof.Enabled = false;
                        //        txtInstrumentNo.CssClass = "Uiinput-amount input-disabled medium";
                        //        txtInstrumentDate.CssClass = "Uidate-picker input-disabled";
                        //        txtFavourof.CssClass = "multiline-2line input-disabled";
                        //        Label5.Visible = false;
                        //        txtBankCharge.Visible = false;
                        //        chkBankCharge.Visible = false;
                        //        txtPaymentBank.Text = string.Empty;
                        //        txtPaymentBank.Enabled = false;
                        //        txtPaymentBank.CssClass = "input-disabled";
                        //        hdfPaymentBank.Value = string.Empty;
                        //        break;
                        //    default:
                        //        vrfBranch.Enabled = vrfBranchHdr.Enabled = true;
                        //        vrfAccountNo.Enabled = vrfAccountNoHdr.Enabled = true;
                        //        vrfInstrumentNo.Enabled = vrfInstrumentNoHdr.Enabled = true;
                        //        vrfInstrumentDate.Enabled = vrfInstrumentDateHdr.Enabled = true;
                        //        vrfFavourof.Enabled = vrfFavourofHdr.Enabled = true;
                        //        txtInstrumentNo.Enabled = true;
                        //        txtInstrumentDate.Enabled = true;
                        //        txtFavourof.Enabled = true;
                        //        txtInstrumentNo.CssClass = "Uiinput-amount medium";
                        //        txtInstrumentDate.CssClass = "Uidate-picker";
                        //        txtFavourof.CssClass = "multiline-2line";
                        //        Label5.Visible = true;
                        //        txtBankCharge.Visible = true;
                        //        chkBankCharge.Visible = true;
                        //        break;
                        //}
                        break;
                    #endregion
                    #region SAVESUBMIT
                    case ActionsEnum.SAVESUBMIT:
                        if (VATTaxDetails == null || VATTaxDetails.Count == 0)
                        {
                            GetFieldValues(ControlsEnum.PURINVNOS);
                            GetFieldValues(ControlsEnum.VATPOPUPGRID);
                        }
                        tempVATTaxDetails = null;
                        if (TempVATTaxDetails != null && TempVATTaxDetails.Count > 0)
                        {
                            tempVATTaxDetails = VATTaxDetails = TempVATTaxDetails;
                        }
                        //Show WorkFlow Popup
                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ONE;
                        //Show WorkFlow Popup                     

                        if (EntryStatus == EntryStatus.VIEWMODE)
                        {
                            ucrWrkf.Visible = true;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        }
                        else
                        {
                            //IF Value=1 Validate invoice type same or not IF Value=0 Allows invoices of any type.
                            if (GetGlobalResourceObject("ConfigurationsRes", "InvoiceTypeValidation").ToString() == "1")
                            {
                                if (finInvoiceHdrList.Count > 0)
                                {
                                    if ((finInvoiceHdrList.Select(x => x.IVH_GROUP).Distinct().ToList()).Count > 1)
                                    {
                                        litErrorMsg.Text = Resources.Report.Msg_Save_PaymentValidation;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                        return;
                                    }
                                }
                            }
                            if (!IsValid)
                            {
                                litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            }
                            else//valid
                            {
                                if (!IsValidPayment())
                                {
                                    return;
                                }
                                int zeroCount = 0;
                                bool IsValidPayNow = true;
                                bool IsValidPayNowWithBalPay = true;
                                //chek bal pmt
                                foreach (GridViewRow gv in grdInvoiceList.Rows)
                                {

                                    lblBaltopay = gv.FindControl("lblBaltopay") as Label;
                                    TextBox txtPay = gv.FindControl("txtPayNow") as TextBox;
                                    //if (Convert.ToDecimal(lblBaltopay.Text) <= Convert.ToDecimal(0))
                                    //{
                                    //    zeroCount = zeroCount + 1;
                                    //}
                                    if (Convert.ToDecimal(lblBaltopay.Text.Replace(",", "")) > 0 && Convert.ToDecimal(txtPay.Text.Replace(",", "")) == 0)
                                    {
                                        IsValidPayNow = false;
                                        break;
                                    }
                                    if (Convert.ToDecimal(txtPay.Text.Replace(",", "")) > Convert.ToDecimal(lblBaltopay.Text.Replace(",", "")))
                                    {
                                        IsValidPayNowWithBalPay = false;
                                        break;
                                    }
                                    if (Convert.ToDecimal(lblBaltopay.Text.Replace(",", "")) < Convert.ToDecimal(txtPay.Text.Replace(",", "")))
                                    {
                                        zeroCount = zeroCount + 1;
                                    }
                                }
                                if (!IsValidPayNow)
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save_InvoicePayNow").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                    return;
                                }
                                else if (!IsValidPayNowWithBalPay)
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_PayNowExcedsBalPay").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                    return;
                                }
                                if ((zeroCount <= 0) || (Iscont == true && (hdfIscontYes.Value == "1")))
                                {
                                    Iscont = false;
                                    decimal adjAmt = txtAdjAmount.Text != string.Empty ? Convert.ToDecimal(txtAdjAmount.Text) : 0;
                                    if (grdInvoiceList.Rows.Count >= 1)
                                    {
                                        if (adjAmt > 0 && ddlAdjType.SelectedValue == CommonConstants.SELECTVAL)
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_AdjType").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                            return;
                                        }
                                        bool tally = true;
                                        decimal CrdrAllocationAmnt = 0;
                                        finPaymentVndHdrList = new List<FIN_PAYMENT_VND_HDR>();
                                        poPaymentServiceClient = new POPaymentService();
                                        poPaymentServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(poPaymentServiceClient);
                                        finPaymentVndHdrObj = CommonFunctions.Initilize<FIN_PAYMENT_VND_HDR>();
                                        finPaymentVndHdrObj = (FIN_PAYMENT_VND_HDR)SetUIValuesToObject(ControlsEnum.WRKFSUBMIT);
                                        //if (hdfExchangeCurrBC.Value != "-1")
                                        //{
                                        if (finPaymentVndHdrObj != null)
                                        {
                                            #region Check whether the exchange rate exist for Transaction currency
                                            if (finPaymentVndHdrObj.PVH_EXCHG_RATE <= 0)
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_ExngRate").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                            }
                                            #endregion
                                            #region Check whether the paid amount and paid amount in BC are same
                                            if (finPaymentVndHdrObj.PVH_PAID_AMOUNT != finPaymentVndHdrObj.FIN_PAYMENT_VND_MODE_DTL.Sum(r => r.PDM_PAID_AMOUNT)) // / (r.PDM_EXCHG_RATE <= 0 ? 1 : Convert.ToDecimal(r.PDM_EXCHG_RATE))
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("msg_not_tally_paidamountBc").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                                return;
                                            }
                                            #endregion
                                            #region Check whether the Bank charge greater than payable Amount
                                            if (finPaymentVndHdrObj.PVH_PAID_AMOUNT < finPaymentVndHdrObj.FIN_PAYMENT_VND_MODE_DTL.Sum(r => (r.PDM_BANK_CHARGE_CURR == finPaymentVndHdrObj.PVH_CURRENCY) ? r.PDM_BANK_CHARGE : (r.PDM_BANK_CHARGE / Convert.ToDecimal(r.PDM_EXCHG_RATE))))
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("msg_Bankcharge_Exceeds").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                                return;
                                            }
                                            #endregion

                                            if (CheckPayment(finPaymentVndHdrObj))
                                            {
                                                TypeRef = finPaymentVndHdrObj.PVH_NO;
                                                paymentMpgCount = 0;
                                                paymentMpgCount = finPaymentVndHdrObj.FIN_PAYMENT_VND_TRX_MPG.ToList().Count;
                                                if (paymentMpgCount > 0)
                                                {
                                                    if (POGroup == POInvoiceGroup.Expense)
                                                    {
                                                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ONE;
                                                        ucrWrkf.Visible = true;
                                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                                                    }
                                                    else
                                                    {
                                                        //decimal paidamount = (Convert.ToDecimal(txtPaidAmount.Text.Trim().Replace(",", "")) + Convert.ToDecimal(txtAdjAmount.Text.Trim().Replace(",", "")));
                                                        //if (chkVendorforpayemnt.Checked == true && ddlWHTAccount.SelectedValue != CommonConstants.SELECTVAL)
                                                        //    paidamount = paidamount + (Convert.ToDecimal(txtWHTAmount.Text.Trim().Replace(",", "")));
                                                        GetFieldValues(ControlsEnum.PAYMENTTOLERANCE);
                                                        foreach (FIN_PAYMENT_VND_TRX_MPG trxObj in finPaymentVndHdrObj.FIN_PAYMENT_VND_TRX_MPG)
                                                        {
                                                            InvoicePK = trxObj.PVM_INVOICE_HDR.HasValue ? trxObj.PVM_INVOICE_HDR.Value : 0;
                                                            GetFieldValues(ControlsEnum.INVOICEVNDHDR);
                                                            if (finInvoiceVndHdrListForPaymentSplit != null && finInvoiceVndHdrListForPaymentSplit.Count > 0 && finInvoiceVndHdrListForPaymentSplit[0].IVH_IS_OPENING == 1)
                                                            {
                                                                tally = true;
                                                            }
                                                            else
                                                            {
                                                                if (trxObj.FIN_PAYMENT_VND_HDR.PVH_GROUP == Convert.ToInt16(POInvoiceGroup.AgtInvoice))
                                                                {
                                                                    tally = true;
                                                                    break;
                                                                }
                                                                else if (trxObj.PVM_PAID_AMOUNT > 0)// && tempFinPaymentVndPoMpg != null && tempFinPaymentVndPoMpg.Count > 0)
                                                                {
                                                                    CrdrAllocationAmnt = PaymentCrdrList.Where(r => r.PNM_INVOICE_HDR == trxObj.PVM_INVOICE_HDR).Sum(sm => sm.PNM_PAID_AMOUNT + sm.PNM_ADJ_AMOUNT);
                                                                    if (PaymentCrdrList != null && PaymentCrdrList.Count > 0)
                                                                    {
                                                                        if ((trxObj.PVM_PAID_AMOUNT - CrdrAllocationAmnt) <= 0)
                                                                        {
                                                                            tally = true;
                                                                            break;
                                                                        }

                                                                    }
                                                                    if (trxObj.FIN_PAYMENT_VND_PO_MPG != null && trxObj.FIN_PAYMENT_VND_PO_MPG.Count > 0)
                                                                    {
                                                                        decimal PaiWithVariation = 0;
                                                                        //PaiWithVariation = trxObj.PVM_PAID_AMOUNT + (trxObj.PVM_PAID_AMOUNT * PaymentTollerence);
                                                                        PaiWithVariation = trxObj.PVM_PAID_AMOUNT + trxObj.PVM_ADJUST_AMOUNT - PaiWithVariation + (trxObj.PVM_PAID_AMOUNT * PaymentTollerence);
                                                                        //if (trxObj.PVM_PAID_AMOUNT != trxObj.FIN_PAYMENT_VND_PO_MPG.Sum(dtl => dtl.PPO_PAID_AMOUNT))
                                                                        //{
                                                                        //    tally = false;
                                                                        //    break;
                                                                        //}
                                                                        //if (trxObj.FIN_PAYMENT_VND_PO_MPG.Sum(dtl => dtl.PPO_PAID_AMOUNT) == 0)
                                                                        //{
                                                                        //    tally = false;
                                                                        //    break;
                                                                        //}
                                                                        if (trxObj.FIN_PAYMENT_VND_PO_MPG.Sum(dtl => dtl.PPO_PAID_AMOUNT) > PaiWithVariation)
                                                                        {
                                                                            tally = false;
                                                                            //For Setting/Resetting Colour of a not tallied InvoiceNo
                                                                            hdfNotTalliedInvoicePk.Value = hdfNotTalliedInvoicePk.Value + "," + InvoicePK.ToString();
                                                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowNotTalliedRow", "$(document).ready(function(){SetNotTalliedRowColor();});", true);
                                                                            break;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        tally = false;
                                                                        //For Setting/Resetting Colour of a not tallied InvoiceNo
                                                                        hdfNotTalliedInvoicePk.Value = hdfNotTalliedInvoicePk.Value + "," + InvoicePK.ToString();
                                                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowNotTalliedRow", "$(document).ready(function(){SetNotTalliedRowColor();});", true);
                                                                        break;
                                                                    }
                                                                }
                                                                else// if (paymentTrxObj.PVM_PAID_AMOUNT <= 0)
                                                                {

                                                                    //tally = false;
                                                                    //break;

                                                                }
                                                            }
                                                        }

                                                        if (tally)
                                                        {
                                                            ucrWrkf.Visible = true;
                                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                                                        }
                                                        else
                                                        {
                                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("msg_allocation_tally").ToString() + "','" + Resources.Messages.Information + "');", true);
                                                        }


                                                        //if (finPaymentVndHdrObj.FIN_PAYMENT_VND_PO_MPG != null && finPaymentVndHdrObj.FIN_PAYMENT_VND_PO_MPG.Count > 0)
                                                        //{
                                                        //   // decimal paidamount = (Convert.ToDecimal(txtPaidAmount.Text.Trim().Replace(",", "")) + Convert.ToDecimal(txtAdjAmount.Text.Trim().Replace(",", "")));
                                                        //    //if (chkVendorforpayemnt.Checked == true && ddlWHTAccount.SelectedValue != CommonConstants.SELECTVAL)
                                                        //    //    paidamount = paidamount + (Convert.ToDecimal(txtWHTAmount.Text.Trim().Replace(",", "")));
                                                        //    if (paidamount == finPaymentVndHdrObj.FIN_PAYMENT_VND_PO_MPG.Sum(dtl => dtl.PPO_PAID_AMOUNT))
                                                        //    {
                                                        //        List<FIN_PAYMENT_VND_PO_MPG> tempFinPaymentVndPoMpg;
                                                        //        tempFinPaymentVndPoMpg = null;
                                                        //        foreach (FIN_PAYMENT_VND_TRX_MPG paymentTrxObj in finPaymentVndHdrObj.FIN_PAYMENT_VND_TRX_MPG)
                                                        //        {
                                                        //            tempFinPaymentVndPoMpg = finPaymentVndHdrObj.FIN_PAYMENT_VND_PO_MPG.Where(aa => aa.PPO_PAYMENT_TRX_MPG == paymentTrxObj.PVM_PK) == null ? null
                                                        //                : finPaymentVndHdrObj.FIN_PAYMENT_VND_PO_MPG.Where(aa => aa.PPO_PAYMENT_TRX_MPG == paymentTrxObj.PVM_PK).ToList();
                                                        //            if (paymentTrxObj.PVM_PAID_AMOUNT > 0)// && tempFinPaymentVndPoMpg != null && tempFinPaymentVndPoMpg.Count > 0)
                                                        //            {
                                                        //                if (tempFinPaymentVndPoMpg != null && tempFinPaymentVndPoMpg.Count > 0)
                                                        //                {
                                                        //                    if (paymentTrxObj.PVM_PAID_AMOUNT != tempFinPaymentVndPoMpg.Sum(dtl => dtl.PPO_PAID_AMOUNT))
                                                        //                    {
                                                        //                        tally = false;
                                                        //                        break;
                                                        //                    }
                                                        //                }
                                                        //                else
                                                        //                {
                                                        //                    tally = false;
                                                        //                    break;
                                                        //                }

                                                        //            }
                                                        //            else// if (paymentTrxObj.PVM_PAID_AMOUNT <= 0)
                                                        //            {
                                                        //                if (tempFinPaymentVndPoMpg != null && tempFinPaymentVndPoMpg.Count > 0)
                                                        //                {
                                                        //                    if (paymentTrxObj.PVM_PAID_AMOUNT != tempFinPaymentVndPoMpg.Sum(dtl => dtl.PPO_PAID_AMOUNT))
                                                        //                    {
                                                        //                        tally = false;
                                                        //                        break;
                                                        //                    }
                                                        //                }
                                                        //            }
                                                        //        }
                                                        //        if (tally)
                                                        //        {
                                                        //            ucrWrkf.Visible = true;
                                                        //            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                                                        //        }
                                                        //        else
                                                        //        {
                                                        //            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("msg_allocation_tally").ToString() + "','" + Resources.Messages.Information + "');", true);
                                                        //        }
                                                        //    }
                                                        //    else
                                                        //    {
                                                        //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("msg_allocation_tally").ToString() + "','" + Resources.Messages.Information + "');", true);
                                                        //    }
                                                        //}
                                                        //else
                                                        //{
                                                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("msg_allocation_tally").ToString() + "','" + Resources.Messages.Information + "');", true);
                                                        //}

                                                    }
                                                }
                                                else
                                                {
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save_InvoicePayNow").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                                }
                                            }
                                            else
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save_PaymentDuplicate").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                            }
                                        }
                                        //}
                                        //else
                                        //{
                                        //    litErrorMsg.Text = Resources.Messages.Msg_ErrConversionFactor;
                                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                        //}
                                    }

                                    else
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save_InvoiceCount").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                    }
                                }
                                else
                                {
                                    Iscont = true;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowAlreadyPaidWKF", "$(document).ready(function(){ShowAlreadyPaidWKF();});", true);

                                }
                            }
                        }
                        break;
                    #endregion
                    #region SUBMIT
                    case ActionsEnum.SUBMIT:
                        //Show WorkFlow Popup
                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    case ActionsEnum.WRKFSUBMIT:
                        //Submit Activity
                        //validate Page
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        else if (!IsFinancialYearExist())//Check whether the financial year exist  or not
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Financial_Year_Notentered").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        else//valid
                        {
                            ucrWrkf.ApplicationID = 0;
                            if (hdfIsSaveSubmit.Value == CommonConstants.SELECT_VALUE_ONE)
                            {
                                finPaymentVndHdrList = new List<FIN_PAYMENT_VND_HDR>();
                                poPaymentServiceClient = new POPaymentService();
                                poPaymentServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(poPaymentServiceClient);
                                finPaymentVndHdrObj = CommonFunctions.Initilize<FIN_PAYMENT_VND_HDR>();
                                finPaymentVndHdrObj = (FIN_PAYMENT_VND_HDR)SetUIValuesToObject(ControlsEnum.WRKFSUBMIT);

                                #region Check Credit\Debit is posted or not
                                if (selectedInvoiceList != null)
                                    PKXml = String.Join(",", selectedInvoiceList.Select(x => x.ToString()).ToArray());
                                
                                GetFieldValues(ControlsEnum.CHECKCREDITDEBITPOST);
                                if (ValidateId == -100)
                                {
                                    EntryStatus = EntryStatus.ENTRYMODE;
                                    litErrorMsg.Text = GetLocalResourceObject("ReceiptCD_Msg").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');ClosePopup();", true);
                                    break;
                                }
                                #endregion

                                if (string.IsNullOrEmpty(lblPaymentNo.Text.Trim()) || lblPaymentNo.Text.Trim().Equals("[NEW]"))
                                {
                                    GetFieldValues(ControlsEnum.PAYMENTNO);
                                    lblPaymentNo.Text = hdfPaymentNo.Value;
                                }
                                finPaymentVndHdrObj.PVH_NO = lblPaymentNo.Text;
                                IsPaymentModeAdded = false;
                                //if (hdfExchangeCurrBC.Value != "-1")
                                //{
                                if (finPaymentVndHdrObj != null)
                                {
                                    paymentMpgCount = 0;
                                    paymentMpgCount = finPaymentVndHdrObj.FIN_PAYMENT_VND_TRX_MPG.ToList().Count;
                                    if (paymentMpgCount > 0)
                                    {
                                        if (CheckPayment(finPaymentVndHdrObj))
                                        {
                                            finPaymentVndHdrList.Add(finPaymentVndHdrObj);
                                            result = poPaymentServiceClient.SavePaymentHdr(finPaymentVndHdrList);
                                            if (result > 0)// Save Success ! do WorkFlow
                                            {
                                                CurrPK = (long)result;

                                                ucrWrkf.ApplicationID = (int)CurrPK;
                                                #region ALERTSAVE
                                                GetFieldValues(ControlsEnum.ALERTCONFIG);
                                                int isAlert = 0;
                                                if (admAppConstMstList != null && admAppConstMstList.Count > 0)
                                                {
                                                    isAlert = admAppConstMstList[0].ACF_VALUE;
                                                }
                                                if (chkPDC.Checked && isAlert == 1)
                                                {
                                                    foreach (FIN_PAYMENT_VND_TRX_MPG paymentTrxObj in finPaymentVndHdrObj.FIN_PAYMENT_VND_TRX_MPG)
                                                    {
                                                        invPK = Convert.ToInt32(paymentTrxObj.FIN_INVOICE_VND_HDR.IVH_PK);
                                                        AlertBO alertBoObj = new AlertBO();
                                                        alertBoObj = (AlertBO)SetUIValuesToObject(ControlsEnum.ALERTSAVE);
                                                        if (alertBoObj != null)
                                                        {
                                                            alertresult = BusinessLogic.AlertManagement.Alerts.SaveAlertDetails(alertBoObj);
                                                        }
                                                    }
                                                }
                                                #endregion

                                                #region ATTACHMENT SAVE
                                                if (DocAttachList != null && DocAttachList.Count > 0)
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

                                                    foreach (ADM_DOC_ATTACH obj in DocAttachList)
                                                    {
                                                        string[] docName = obj.DOC_PATH.Split('/');
                                                        string filePath = savePath + obj.DOC_NAME;
                                                        if (docName.Length > 0)
                                                            filePath = savePath + docName[docName.Length - 1];
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
                                                    docSaveResult = poInvoiceServiceClient.SaveDocAttachemts(DocAttachList, (int)result);
                                                }
                                                #endregion
                                                #region WkfSummarySave
                                                int resultSummary = BusinessLogic.CommonManagement.CommonBL.SaveSummary(result.Value, Convert.ToInt32(hdfProcessID.Value));
                                                if (resultSummary <= 0)
                                                {

                                                    litErrorMsg.Text = Resources.Messages.SummaryActionFailed;
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                                }
                                                #endregion
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
                                                    litErrorMsg.Text = Resources.PageNameRes.RFQ + " " + Resources.Messages.EditUsedByAnotherUser;
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                    + "','" + Resources.ErpRes.Information + "');", true);
                                                }
                                                else if (result == (int)DbSaveStatus.CODEEXIST)
                                                {
                                                    litErrorMsg.Text = Resources.PageNameRes.RFQ + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                    + "','" + Resources.ErpRes.Information + "');", true);
                                                }
                                                else if (result.Value == (int)DbSaveStatus.AMOUNTEXCEEDS)
                                                {
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("AmountExceeds").ToString())
                                                    + "','" + Resources.ErpRes.Information + "');", true);
                                                }
                                                else
                                                {
                                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.RFQ);
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                        + "','" + Resources.ErpRes.Information + "');", true);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save_PaymentDuplicate").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                        }
                                    }
                                }
                                //}
                            }
                            else if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)
                            {
                                if (BusinessLogic.CommonManagement.CommonBL.ValidationForCancellation((int)CurrPK, POGroup == POInvoiceGroup.AgtInvoice ? ApplicationType.AIP : ApplicationType.VP))
                                {
                                    ucrWrkf.ApplicationID = (int)CurrPK;
                                }
                                else
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Err_PP_Cancel").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                    TextBox WrkfComments;
                                    WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                                    FillProcessID(1);
                                    hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                                    WrkfComments.Text = "";
                                    SelectedInvoices = null;

                                    EntryStatus = EntryStatus.LISTMODE;
                                    ResetForm();
                                    GetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                                    SetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                                }
                            }
                            else
                                ucrWrkf.ApplicationID = (int)CurrPK;

                            if (ucrWrkf.ApplicationID > 0)
                            {
                                ddlWkfAction = (DropDownList)ucrWrkf.FindControl("WRKFACT_ID");
                                //Do WorkFlow if WorkFlow has Actions
                                if (ddlWkfAction.Items.Count > 0)
                                {
                                    action = ddlWkfAction.SelectedItem.ToString();
                                    result = ucrWrkf.DoWorkFlow();
                                    if (result > 0)
                                    {
                                        if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)
                                        {
                                            FillProcessID(1);
                                            litErrorMsg.Text = Resources.Messages.Msg_Cancelled_Success;
                                        }
                                        else
                                            litErrorMsg.Text = GetLocalResourceObject("Msg_Submit_Success").ToString();
                                        CurrPK = (long)result;
                                        GetFieldValues(ControlsEnum.PAYMENTHDRENTRYBYPK);
                                        if (finPaymentVndHdrList != null && finPaymentVndHdrList.Count > 0)
                                        {
                                            InvoicePOSplitList = finPaymentVndHdrList.First().FIN_PAYMENT_VND_PO_MPG.ToList();
                                        }
                                        //Show Save success message and reset Contract Entry

                                        object[] args = new object[2];
                                        args[0] = Resources.PageNameRes.Payment;
                                        args[1] = lblPaymentNo.Text;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, args);


                                        #region Inbox or Listing Page Redirection
                                        if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && base.WkfRefID > 0)
                                        {
                                            ResetForm();
                                            SelectedInvoices = null;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.InboxURL) + "');", true);
                                        }
                                        else
                                        {
                                            ResetForm();
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.ErpRes.Information + "');", true);
                                            SelectedInvoices = null;
                                            EntryStatus = EntryStatus.LISTMODE;
                                            GetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                                            SetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                                            btnNew.Focus();
                                        }
                                        #endregion
                                    }
                                }
                            }
                        }
                        break;
                    #endregion
                    #region Journalize
                    case ActionsEnum.JOURNALIZE:
                        finPaymentVndHdrObj = CommonFunctions.Initilize<FIN_PAYMENT_VND_HDR>();
                        finPaymentVndHdrObj.PVH_PK = CurrPK;
                        GetFieldValues(ControlsEnum.PAYMENTHDRENTRYBYPK);
                        SetUIValuesToObject(ControlsEnum.JOURNALIZE);
                        break;
                    #endregion
                    #region Journalize Update
                    case ActionsEnum.JOURNALIZEUPDATE:

                        ucrJournalize.ResetForm();
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        FillProcessID(1);
                        ResetForm();
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        EntryStatus = EntryStatus.LISTMODE;
                        GetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                        SetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                        break;
                    #endregion
                    #region Journalize Save
                    case ActionsEnum.REVERSESAVE:
                    case ActionsEnum.JOURNALIZESAVE:
                    case ActionsEnum.RETURNSAVE:
                        //if (Session[ERP.Utilities.SessionStrings.Transaction] != null)
                        //{
                        //    string Transaction = Session[ERP.Utilities.SessionStrings.Transaction].ToString();
                        //    if (Transaction == "SAVE")
                        //    {
                        //        poPaymentServiceClient = new POPaymentService();
                        //        poPaymentServiceClient = CommonFunctions.InitiateClient(poPaymentServiceClient);
                        //        result = poPaymentServiceClient.UpdatePaymentHdrJounalizeFlag(int.Parse(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString()), true);

                        //    }
                        //    else if (Transaction == "DELETE")
                        //    {
                        //        poPaymentServiceClient = new POPaymentService();
                        //        poPaymentServiceClient = CommonFunctions.InitiateClient(poPaymentServiceClient);
                        //        result = poPaymentServiceClient.UpdatePaymentHdrJounalizeFlag(int.Parse(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString()), false);
                        //    }
                        //}
                        EntryStatus = EntryStatus.LISTMODE;
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        FillProcessID(1);
                        ResetForm();
                        GetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                        SetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                        break;
                    #endregion
                    #region Journalize Submit
                    case ActionsEnum.JOURNALIZESUBMIT:
                        if (Session[ERP.Utilities.SessionStrings.Transaction] != null)
                        {
                            string Transaction = Session[ERP.Utilities.SessionStrings.Transaction].ToString();
                            if (Transaction == "SAVE")
                            {
                                poPaymentServiceClient = new POPaymentService();
                                poPaymentServiceClient = CommonFunctions.InitiateClient(poPaymentServiceClient);
                                result = poPaymentServiceClient.UpdatePaymentHdrJounalizeFlag(int.Parse(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString()), true);

                            }
                            else if (Transaction == "DELETE")
                            {
                                poPaymentServiceClient = new POPaymentService();
                                poPaymentServiceClient = CommonFunctions.InitiateClient(poPaymentServiceClient);
                                result = poPaymentServiceClient.UpdatePaymentHdrJounalizeFlag(int.Parse(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString()), false);
                            }
                        }
                        EntryStatus = EntryStatus.LISTMODE;
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        FillProcessID(1);
                        ResetForm();

                        #region Inbox or Listing Page Redirection
                        //if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && ReferanceID > 0)
                        //{
                        //    Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.InboxURL));
                        //}
                        //else
                        //{
                        GetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                        SetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                        //}
                        #endregion
                        break;
                    #endregion
                    #region Journalize Delete
                    case ActionsEnum.JOURNALIZEDELETE:
                        if (Session[ERP.Utilities.SessionStrings.Transaction] != null)
                        {
                            string Transaction = Session[ERP.Utilities.SessionStrings.Transaction].ToString();
                            if (Transaction == "SAVE")
                            {
                                poPaymentServiceClient = new POPaymentService();
                                poPaymentServiceClient = CommonFunctions.InitiateClient(poPaymentServiceClient);
                                result = poPaymentServiceClient.UpdatePaymentHdrJounalizeFlag(int.Parse(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString()), true);

                            }
                            else if (Transaction == "DELETE")
                            {
                                poPaymentServiceClient = new POPaymentService();
                                poPaymentServiceClient = CommonFunctions.InitiateClient(poPaymentServiceClient);
                                result = poPaymentServiceClient.UpdatePaymentHdrJounalizeFlag(int.Parse(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString()), false);
                            }
                        }
                        EntryStatus = EntryStatus.LISTMODE;
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        FillProcessID(1);
                        ResetForm();
                        GetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                        SetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                        break;
                    #endregion
                    #region Journalize Cancel
                    case ActionsEnum.REVERSECANCEL:
                    case ActionsEnum.JOURNALIZECANCEL:
                    case ActionsEnum.RETURNCANCEL:
                        EntryStatus = EntryStatus.LISTMODE;
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        FillProcessID(1);
                        ResetForm();
                        GetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                        SetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                        break;
                    #endregion
                    #region REVERSE Submit
                    case ActionsEnum.REVERSESUBMIT:
                    case ActionsEnum.RETURNSUBMIT:
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        FillProcessID(1);
                        ResetForm();
                        EntryStatus = EntryStatus.LISTMODE;
                        GetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                        SetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                        break;
                    #endregion
                    #region REVERSE Delete
                    case ActionsEnum.REVERSEDELETE:
                    case ActionsEnum.RETURNDELETE:
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        FillProcessID(1);
                        ResetForm();
                        EntryStatus = EntryStatus.LISTMODE;
                        GetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                        SetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                        break;
                    #endregion
                    #region Print
                    case ActionsEnum.PRINT:
                        if (CurrPK > 0)
                        {
                            if (PaymentModeDetailsList != null && PaymentModeDetailsList.Count > 0)
                            {
                                if (PaymentModeDetailsList.Where(r => r.PDM_MODE == (byte)PaymentModeEnum.CHEQUE).Count() > 0)
                                {
                                    if (hdfIsMultipleCheque.Value == "0")
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx" + "?ID=" + CurrPK + "&APPTYPE=" + GetLocalResourceObject("PrintAPPType").ToString() +
                                         "&APPSUBTYPE=" + GetLocalResourceObject("PrintSubType").ToString() + "');", true);
                                    }
                                    else
                                    {
                                        List<FIN_PAYMENT_VND_MODE_DTL> PaymentModeDetailsPrintList = PaymentModeDetailsList.Where(r => r.PDM_MODE == (byte)PaymentModeEnum.CHEQUE).ToList();
                                        string printURL = string.Empty;
                                        foreach (FIN_PAYMENT_VND_MODE_DTL objChequeList in PaymentModeDetailsPrintList)
                                        {
                                            printURL += "../Reports/GenerateReport.aspx" + "?ID=" + CurrPK + "&APPTYPE=" + GetLocalResourceObject("PrintAPPType").ToString() +
                                               "&APPSUBTYPE=" + GetLocalResourceObject("PrintSubType").ToString() + "&ChequeID=" + objChequeList.PDM_PK.ToString() + ",";
                                        }
                                        hdfPrintCheque.Value = printURL.Remove(printURL.Length - 1);
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "PrintCheque();", true);
                                    }
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("chque_PrintError").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                }
                            }
                        }
                        break;
                    case ActionsEnum.PRINTDT:
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.VP + "&APPSUBTYPE=1") + "');", true);
                        }
                        break;
                    #endregion
                    #region Po Invoices
                    case ActionsEnum.INVOICEDETAIL:
                        hdfInvoicePK = (HiddenField)((((Button)sender).Parent).FindControl("hdfInvoicePK"));
                        if (hdfInvoicePK != null && !string.IsNullOrEmpty(hdfInvoicePK.Value) && !hdfInvoicePK.Value.Equals("0"))
                        {
                            InvoicePK = Convert.ToInt64(hdfInvoicePK.Value);
                        }
                        else
                            InvoicePK = 0;
                        InvoiceDetails(InvoicePK);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPoSplitUp]','" + GetLocalResourceObject("PaymentSplit").ToString() + "','850','300');", true);

                        //Tax = 0;
                        //PayNow = 0;
                        //InvOtherCharge = 0;
                        //divErrorLabel.Visible = false;
                        //HiddenField hdfPaymentPK = (HiddenField)((((Button)sender).Parent).FindControl("hdfPaymentMpgPK"));
                        //TextBox txtPayNow = (TextBox)((((Button)sender).Parent).FindControl("txtPayNow"));
                        //HiddenField hdfTotalTax = (HiddenField)((((Button)sender).Parent).FindControl("hdfTotalTax"));
                        //TextBox txtOtherCharges = (TextBox)((((Button)sender).Parent).FindControl("txtOtherCharges"));
                        //Label lblOtherCharges = (Label)((((Button)sender).Parent).FindControl("lblOtherCharges"));
                        //hdfTaxPer.Value = "0";
                        //hdfOtherPer.Value = "0";

                        //if (lblOtherCharges != null && !string.IsNullOrEmpty(lblOtherCharges.Text))
                        //{
                        //    decimal.TryParse(lblOtherCharges.Text.Replace(",", ""), out InvTotalOtherAmnt);
                        //    hdfInvTotalOtherCharge.Value = InvTotalOtherAmnt.ToString();
                        //}

                        //if (hdfTotalTax != null && !string.IsNullOrEmpty(hdfTotalTax.Value.Trim()) && txtPayNow != null && !string.IsNullOrEmpty(txtPayNow.Text.Trim()))
                        //{
                        //    if (Convert.ToDecimal(txtPayNow.Text.Trim()) > 0)
                        //    {
                        //        hdfTaxPer.Value = (Convert.ToDecimal(hdfTotalTax.Value.Trim()) / Convert.ToDecimal(txtPayNow.Text.Trim())).ToString();
                        //        Tax = Convert.ToDecimal(Convert.ToDecimal(hdfTotalTax.Value.Trim()));
                        //        hdfTax.Value = Tax.ToString();
                        //    }
                        //}
                        //if (txtOtherCharges != null && !string.IsNullOrEmpty(txtOtherCharges.Text.Trim()) && txtPayNow != null && !string.IsNullOrEmpty(txtPayNow.Text.Trim()))
                        //{
                        //    InvOtherCharge = Convert.ToDecimal(txtOtherCharges.Text.Replace(",", "").Trim());

                        //    if (Convert.ToDecimal(txtPayNow.Text.Trim()) > 0)
                        //    {
                        //        hdfOtherPer.Value = (Convert.ToDecimal(txtOtherCharges.Text.Trim()) / Convert.ToDecimal(txtPayNow.Text.Trim())).ToString();
                        //    }
                        //}
                        //if (txtPayNow != null && !string.IsNullOrEmpty(txtPayNow.Text.Trim()))
                        //{
                        //    PayNowAmount = Convert.ToDecimal(txtPayNow.Text.Trim());
                        //    PayNow = PayNowAmount;
                        //}
                        //hdfInvoicePK = (HiddenField)((((Button)sender).Parent).FindControl("hdfInvoicePK"));
                        //if (hdfInvoicePK != null && !string.IsNullOrEmpty(hdfInvoicePK.Value) && !hdfInvoicePK.Value.Equals("0"))
                        //{
                        //    InvoicePK = Convert.ToInt64(hdfInvoicePK.Value);
                        //}
                        //else
                        //    InvoicePK = 0;
                        //tempInvoicePOSplitList = InvoicePOSplitList;
                        //tempFinPaymentVndPoMpgList = tempInvoicePOSplitList.Where(dtl => dtl.FIN_PAYMENT_VND_TRX_MPG.PVM_INVOICE_HDR == InvoicePK).ToList();

                        //if (hdfPaymentPK != null && !string.IsNullOrEmpty(hdfPaymentPK.Value) && !hdfPaymentPK.Value.Equals("0"))
                        //{
                        //    PaymentMpgPK = Convert.ToInt64(hdfPaymentPK.Value);
                        //    GetFieldValues(ControlsEnum.PAYMENTSPLITLIST);
                        //    if (tempFinPaymentVndPoMpgList == null || tempFinPaymentVndPoMpgList.Count == 0)
                        //    {
                        //        if (finPaymentVndPoMpgList != null && finPaymentVndPoMpgList.Count > 0)//Edit
                        //        {
                        //            tempInvoicePOSplitList.Where(dtl => dtl.FIN_PAYMENT_VND_TRX_MPG.PVM_INVOICE_HDR == InvoicePK)
                        //                            .ToList().ForEach(dtl => tempInvoicePOSplitList.Remove(dtl));
                        //            finPaymentVndPoMpgList.ForEach(dtl =>
                        //            {
                        //                dtl.FIN_PAYMENT_VND_TRX_MPG = new FIN_PAYMENT_VND_TRX_MPG()
                        //                {
                        //                    PVM_INVOICE_HDR = InvoicePK
                        //                };
                        //                tempInvoicePOSplitList.Add(dtl);
                        //            });
                        //            InvoicePOSplitList = tempInvoicePOSplitList;
                        //            tempFinPaymentVndPoMpgList = tempInvoicePOSplitList.Where(dtl => dtl.FIN_PAYMENT_VND_TRX_MPG.PVM_INVOICE_HDR == InvoicePK).ToList();
                        //        }
                        //        else if (InvoicePK > 0)
                        //        {
                        //            GetFieldValues(ControlsEnum.INVOICEVNDMPGLIST);
                        //        }
                        //    }
                        //    else
                        //    {
                        //        tempFinPaymentVndPoMpgList = tempInvoicePOSplitList;
                        //    }
                        //    if (InvoicePK > 0)
                        //    {
                        //        GetFieldValues(ControlsEnum.INVOICEVNDHDR);
                        //        GetUIValuesFromObject(ControlsEnum.PAYMENTSPLITLIST);
                        //        isSplitChanged = false;
                        //        SetFieldValues(ControlsEnum.PAYMENTSPLITLIST);
                        //        //GetUIValuesFromObject(ControlsEnum.SPLITPAYNOWFORMULIPO);
                        //    }

                        //    //if (finPaymentVndPoMpgList != null && finPaymentVndPoMpgList.Count > 0)//Edit
                        //    //{
                        //    //    tempInvoicePOSplitList.Where(dtl => dtl.FIN_PAYMENT_VND_TRX_MPG.PVM_INVOICE_HDR == InvoicePK)
                        //    //                    .ToList().ForEach(dtl => tempInvoicePOSplitList.Remove(dtl));
                        //    //    finPaymentVndPoMpgList.ForEach(dtl =>
                        //    //    {
                        //    //        dtl.FIN_PAYMENT_VND_TRX_MPG = new FIN_PAYMENT_VND_TRX_MPG()
                        //    //        {
                        //    //            PVM_INVOICE_HDR = InvoicePK
                        //    //        };
                        //    //        tempInvoicePOSplitList.Add(dtl);
                        //    //    });
                        //    //    InvoicePOSplitList = tempInvoicePOSplitList;
                        //    //    if (InvoicePK > 0)
                        //    //    {
                        //    //        GetFieldValues(ControlsEnum.INVOICEVNDHDR);
                        //    //        GetUIValuesFromObject(ControlsEnum.PAYMENTSPLITLIST);
                        //    //        isSplitChanged = false;
                        //    //        SetFieldValues(ControlsEnum.PAYMENTSPLITLIST);
                        //    //    }
                        //    //}
                        //    //else//New
                        //    //{
                        //    //    if (InvoicePK > 0)
                        //    //    {
                        //    //        GetFieldValues(ControlsEnum.INVOICEVNDMPGLIST);
                        //    //        if (FinInvoiceVndTrxMpgList != null && FinInvoiceVndTrxMpgList.Count > 0)
                        //    //        {
                        //    //            GetFieldValues(ControlsEnum.INVOICEVNDHDR);
                        //    //            GetUIValuesFromObject(ControlsEnum.PAYMENTSPLITLIST);
                        //    //            isSplitChanged = false;
                        //    //            SetFieldValues(ControlsEnum.PAYMENTSPLITLIST);
                        //    //        }
                        //    //    }
                        //    //}
                        //}
                        //else if (InvoicePK > 0)//New
                        //{
                        //    GetFieldValues(ControlsEnum.INVOICEVNDMPGLIST);
                        //    GetFieldValues(ControlsEnum.INVOICEVNDHDR);
                        //    GetUIValuesFromObject(ControlsEnum.PAYMENTSPLITLIST);
                        //    isSplitChanged = false;
                        //    SetFieldValues(ControlsEnum.PAYMENTSPLITLIST);
                        //    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_CalculateTotal", "$(document).ready(function(){CalculatePymntSplitBalFooter();});", true);

                        //    GetUIValuesFromObject(ControlsEnum.SPLITPAYNOWFORMULIPO);
                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_CalculateTotal", "$(document).ready(function(){CalculateTotal();CalculateTotalSplit();});", true);

                        //    //if (InvoicePK > 0)
                        //    //{
                        //    //    GetFieldValues(ControlsEnum.INVOICEVNDMPGLIST);
                        //    //    if (FinInvoiceVndTrxMpgList != null && FinInvoiceVndTrxMpgList.Count > 0)
                        //    //    {
                        //    //        GetFieldValues(ControlsEnum.INVOICEVNDHDR);
                        //    //        GetUIValuesFromObject(ControlsEnum.PAYMENTSPLITLIST);
                        //    //        isSplitChanged = false;
                        //    //        SetFieldValues(ControlsEnum.PAYMENTSPLITLIST);
                        //    //    }
                        //    //}
                        //}

                        ////ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_CalculateTotal", "$(document).ready(function(){CalculatePymntSplitBalFooter();});", true);

                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPoSplitUp]','" + GetLocalResourceObject("PaymentSplit").ToString() + "','850','300');", true);

                        break;
                    #endregion
                    #region PAYMENT SPLIT SAVE
                    case ActionsEnum.PAYMENTSPLITSAVE:

                        PaymentSplitSave(true);
                        //if (!IsValid)
                        //{
                        //    litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        //}
                        //else//valid
                        //{
                        //    lblSplitErrorMessage.Text = GetLocalResourceObject("error_allocation").ToString();
                        //    if (grdPaymentSplit.Rows.Count >= 1)
                        //    {
                        //        HiddenField lblTotalPayNowFooterSplit = (HiddenField)(grdPaymentSplit.FooterRow.FindControl("hdfTotalPayNowFooterSplit"));
                        //        HiddenField hdfTotalTaxFooterSplit1 = (HiddenField)(grdPaymentSplit.FooterRow.FindControl("hdfTotalTaxFooterSplit1"));

                        //        if (lblTotalPayNowFooterSplit != null && !string.IsNullOrEmpty(lblTotalPayNowFooterSplit.Value) && !string.IsNullOrEmpty(lblInvSplitReceiveNow.Text))
                        //        {
                        //            GetFieldValues(ControlsEnum.PAYMENTTOLERANCE);
                        //            Label TotalPayNowFooterSplit = (Label)(grdPaymentSplit.FooterRow.FindControl("lblTotalPayNowFooterSplit"));
                        //            decimal FooterSplitAmt = Convert.ToDecimal(lblTotalPayNowFooterSplit.Value);
                        //            TotalPayNowFooterSplit.Text = Math.Round(FooterSplitAmt, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                        //            decimal PayNowAmount = Convert.ToDecimal(lblInvSplitReceiveNow.Text.Trim().Replace(",", ""));
                        //            decimal PayNowWithVariation = 0;
                        //            decimal Variation = (PayNowAmount * PaymentTollerence);
                        //            PayNowWithVariation = PayNowAmount + Variation;
                        //            //if (Convert.ToDecimal(TotalPayNowFooterSplit.Text) == Convert.ToDecimal(lblInvSplitReceiveNow.Text.Trim().Replace(",", "")))
                        //            //{
                        //            if (Convert.ToDecimal(TotalPayNowFooterSplit.Text) <= PayNowWithVariation)
                        //            {
                        //                if (!string.IsNullOrEmpty(hdfTotalTaxFooterSplit1.Value))
                        //                {
                        //                    if (Tax != Convert.ToDecimal(hdfTotalTaxFooterSplit1.Value))
                        //                    {
                        //                        divErrorLabel.Visible = true;
                        //                        lblSplitErrorMessage.Text = GetLocalResourceObject("Err_TaxSplit").ToString();// +" " + Tax.ToString();
                        //                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('Total split amount should be less than or equal to pay now amount','" + Resources.Messages.Information + "');", true);
                        //                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPoSplitUp]','" + GetLocalResourceObject("PaymentSplit").ToString() + "','850','300');", true);
                        //                        return;
                        //                    }
                        //                }

                        //                divErrorLabel.Visible = false;
                        //                finPaymentVndPoMpgList = new List<FIN_PAYMENT_VND_PO_MPG>();
                        //                poPaymentServiceClient = new POPaymentService();
                        //                poPaymentServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(poPaymentServiceClient);
                        //                finPaymentVndPoMpgList = (List<FIN_PAYMENT_VND_PO_MPG>)SetUIValuesToObject(ControlsEnum.PAYMENTSPLITLIST);
                        //                if (finPaymentVndPoMpgList != null)
                        //                {
                        //                    tempInvoicePOSplitList = InvoicePOSplitList;
                        //                    tempInvoicePOSplitList.Where(dtl => dtl.FIN_PAYMENT_VND_TRX_MPG.PVM_INVOICE_HDR == InvoicePK)
                        //                        .ToList().ForEach(dtl => tempInvoicePOSplitList.Remove(dtl));
                        //                    finPaymentVndPoMpgList.ForEach(dtl =>
                        //                    {
                        //                        dtl.FIN_PAYMENT_VND_TRX_MPG = new FIN_PAYMENT_VND_TRX_MPG()
                        //                        {
                        //                            PVM_INVOICE_HDR = InvoicePK
                        //                        };
                        //                        tempInvoicePOSplitList.Add(dtl);
                        //                    });
                        //                    InvoicePOSplitList = tempInvoicePOSplitList;
                        //                    //result = poPaymentServiceClient.SavePaymentSplit(finPaymentVndPoMpgList);
                        //                    //if (result >= 0)
                        //                    //{
                        //                    //    litErrorMsg.Text = Resources.Messages.Msg_Save_Success;
                        //                    //    litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), GetLocalResourceObject("Payment_allocation").ToString());
                        //                    //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                        //                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                        //                        "ClosePopup();", true);
                        //                    //}
                        //                }
                        //            }
                        //            else
                        //            {
                        //                divErrorLabel.Visible = true;
                        //                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('Total split amount should be less than or equal to pay now amount','" + Resources.Messages.Information + "');", true);
                        //                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPoSplitUp]','" + GetLocalResourceObject("PaymentSplit").ToString() + "','850','300');", true);
                        //            }

                        //        }
                        //        else
                        //        {
                        //            divErrorLabel.Visible = true;
                        //            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('Total split amount should be less than or equal to pay now amount','" + Resources.Messages.Information + "');", true);
                        //            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPoSplitUp]','" + GetLocalResourceObject("PaymentSplit").ToString() + "','850','300');", true);
                        //        }
                        //    }
                        //    else
                        //    {
                        //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                        //                            "ClosePopup();", true);
                        //    }

                        //}
                        break;
                    #endregion
                    #region WHT ACCOUNT Changed
                    case ActionsEnum.WHT_ACCOUNT_INDEX_CHANGED:

                        if (hdfIsBtnUpload.Value == "1")
                        {
                            if (hdftest.Value == "1")
                            {
                                chkVendorforpayemnt.Checked = true;
                            }
                            else
                            {
                                chkVendorforpayemnt.Checked = false;
                            }
                            break;
                        }
                        SetVendorPayment();
                        whtTaxpk = ddlWHTAccount.SelectedValue != CommonConstants.SELECTVAL ? Convert.ToInt32(ddlWHTAccount.SelectedValue) : 0;
                        //if (chkVendorforpayemnt.Checked)
                        //{
                        GetFieldValues(ControlsEnum.VENDORACCOUNTTAX);
                        SetFieldValues(ControlsEnum.VENDORACCOUNTTAX);
                        //}
                        //else
                        //{
                        //    txtWHTAmount.Text = "0.00";
                        //}

                        break;
                    #endregion
                    #region PayNow Change
                    case ActionsEnum.CHANGEPAYNOW:
                        hdfInvoicePK = (HiddenField)(((sender as Button).Parent.Parent as GridViewRow).FindControl("hdfInvoicePK"));
                        lblCrdrAlcnAmount = (Label)(((sender as Button).Parent.Parent as GridViewRow).FindControl("lblCrdrAlcnAmount"));
                        if (hdfInvoicePK != null && !string.IsNullOrEmpty(hdfInvoicePK.Value) || !hdfInvoicePK.Value.Equals("0"))
                        {
                            InvoicePK = Convert.ToInt64(hdfInvoicePK.Value);
                            tempInvoicePOSplitList = InvoicePOSplitList;

                            //tempInvoicePOSplitList.Where(dtl => dtl.FIN_PAYMENT_VND_TRX_MPG.PVM_INVOICE_HDR == InvoicePK)
                            //    .ToList().ForEach(dtl => tempInvoicePOSplitList.Remove(dtl));
                            tempInvoicePOSplitList.Where(dtl => dtl.FIN_PAYMENT_VND_TRX_MPG.PVM_INVOICE_HDR == InvoicePK)
                               .ToList().ForEach(dtl =>
                               {
                                   dtl.PPO_PAID_AMOUNT = 0;
                                   dtl.PPO_TAX_AMOUNT = 0;
                                   dtl.PPO_OTHER_AMOUNT = 0;
                               });

                            InvoicePOSplitList = tempInvoicePOSplitList;

                            if (AppliedInvPkList != null && AppliedInvPkList.Count > 0)
                                AppliedInvPkList.Remove(InvoicePK);

                            #region Reset CR/DR Allocation
                            tempPaymentCrdrList = PaymentCrdrList;
                            if (tempPaymentCrdrList != null && tempPaymentCrdrList.Where(r => r.PNM_INVOICE_HDR == InvoicePK).ToList().Count > 0)
                            {
                                tempPaymentCrdrList.Where(r => r.PNM_INVOICE_HDR == InvoicePK).ToList().ForEach(dtl =>
                                {
                                    dtl.PNM_ADJ_AMOUNT = 0;
                                    dtl.PNM_PAID_AMOUNT = 0;
                                });
                                PaymentCrdrList = tempPaymentCrdrList;
                                decimal TotalCrdrAmnt = 0;
                                TotalCrdrAmnt = PaymentCrdrList.Where(r => r.PNM_INVOICE_HDR == InvoicePK).Sum(r => r.PNM_PAID_AMOUNT + r.PNM_ADJ_AMOUNT);
                                if (lblCrdrAlcnAmount != null)
                                    lblCrdrAlcnAmount.Text = lblCrdrAlcnAmount.ToolTip = string.Format("{0:c}", TotalCrdrAmnt);
                            }
                            #endregion
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CloseMsgPopup1",
                                                "CloseMsgPopup();", true);
                        break;
                    #endregion
                    #region REVERSE
                    case ActionsEnum.REVERSE:
                        finPaymentVndHdrObj = CommonFunctions.Initilize<FIN_PAYMENT_VND_HDR>();
                        finPaymentVndHdrObj.PVH_PK = CurrPK;
                        GetFieldValues(ControlsEnum.PAYMENTHDRENTRYBYPK);
                        if (finPaymentVndHdrList != null && finPaymentVndHdrList.Count == 1)
                        {
                            if (finPaymentVndHdrList[0].FIN_PAYMENT_VND_MODE_DTL.Where(r => r.PDM_PDC >= 1).Count() > 0) //commented for multiple cheque finPaymentVndHdrList[0].PVH_PDC >= 1
                            {
                                if (finPaymentVndHdrList[0].PVH_HAS_JRNL_ENTRY)
                                {
                                    if (finPaymentVndHdrList[0].FIN_PAYMENT_VND_MODE_DTL.Where(r => r.PDM_PDC == 1).Count() > 0) //commented for multiple cheque finPaymentVndHdrList[0].PVH_PDC == 1
                                    {
                                        FinTrxService finTrxServiceClient;
                                        finTrxServiceClient = new FinTrxService();
                                        finTrxServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(finTrxServiceClient);
                                        result = finTrxServiceClient.GenerateDummyEntry((int)CurrPK, ApplicationType.PPCCJ);
                                        if (result > 0 || result == -2)  // -2 already exist
                                        {
                                            poPaymentServiceClient = new POPaymentService();
                                            poPaymentServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(poPaymentServiceClient);
                                            result = poPaymentServiceClient.UpdatePaymentHdrPDCFlag((int)CurrPK, 3);
                                            //(Eval("PVH_PDC").ToString() != "0" ? (((Eval("PVH_PDC").ToString() == "1") || (Eval("PVH_PDC").ToString() == "3")) ? "flaggrey-icon" : "flaggreen-icon"): "")
                                            //(Eval("PVH_PDC").ToString() != "0" ? (((Eval("PVH_PDC").ToString() == "1") || (Eval("PVH_PDC").ToString() == "1")) ? GetLocalResourceObject("PDC_Cheque").ToString() : GetLocalResourceObject("Cheque_Reversed").ToString()) : "")
                                        }
                                        else
                                        {
                                            litErrorMsg.Text = GetLocalResourceObject("Msg_Err_ReverseEntry").ToString();
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                            break;
                                        }
                                    }
                                    SetUIValuesToObject(ControlsEnum.REVERSE);
                                }
                                else
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("MsgErr_ReverseEntry_Not_Journalized").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                            }
                            else
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_Err_ReverseEntry_Not_PDC").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                break;
                            }
                        }
                        break;
                    #endregion
                    #region ChequeReturn
                    case ActionsEnum.CHEQUERETURN:
                        finPaymentVndHdrObj = CommonFunctions.Initilize<FIN_PAYMENT_VND_HDR>();
                        finPaymentVndHdrObj.PVH_PK = CurrPK;
                        GetFieldValues(ControlsEnum.PAYMENTHDRENTRYBYPK);
                        if (finPaymentVndHdrList != null && finPaymentVndHdrList.Count == 1)
                        {
                            if (finPaymentVndHdrList[0].FIN_PAYMENT_VND_MODE_DTL.Where(r => r.PDM_PDC != 1).Count() > 0) //commented for multiple cheque finPaymentVndHdrList[0].PVH_PDC != 1
                            {
                                if (finPaymentVndHdrList[0].PVH_HAS_JRNL_ENTRY)
                                {

                                    #region Check whether the cheque return is possible or not
                                    bool IsReturnSuccess = true;
                                    if (finPaymentVndHdrList[0].FIN_PAYMENT_VND_MODE_DTL.Where(r => r.PDM_MODE != (byte)PaymentModeEnum.CHEQUE).Count() > 0)
                                        IsReturnSuccess = false;
                                    else if (finPaymentVndHdrList[0].FIN_PAYMENT_VND_MODE_DTL.Where(r => r.PDM_PDC > 0).Count() > 0 && finPaymentVndHdrList[0].FIN_PAYMENT_VND_MODE_DTL.Where(r => r.PDM_PDC == 0).Count() > 0)
                                        IsReturnSuccess = false;
                                    if (!IsReturnSuccess)
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("MsgErr_ReturnEntry_Multi_Mode").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                        return;
                                    }
                                    #endregion

                                    if (finPaymentVndHdrList[0].FIN_PAYMENT_VND_MODE_DTL.Where(r => r.PDM_BOUNCED == 0).Count() > 0) //commented for multiple cheque finPaymentVndHdrList[0].PVH_BOUNCED == 0
                                    {
                                        FinTrxService finTrxServiceClient;
                                        finTrxServiceClient = new FinTrxService();
                                        finTrxServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(finTrxServiceClient);
                                        result = finTrxServiceClient.GenerateDummyEntry((int)CurrPK, ApplicationType.PCBJ);
                                        //if (result > 0 || result == -2)  // -2 already exist
                                        //{
                                        //    poPaymentServiceClient = new POPaymentService();
                                        //    poPaymentServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(poPaymentServiceClient);
                                        //    result = poPaymentServiceClient.UpdatePaymentHdrBounceFlag((int)CurrPK, 1);
                                        //}
                                        //else
                                        //{
                                        //    litErrorMsg.Text = GetLocalResourceObject("Msg_Err_ReturnEntry").ToString();
                                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                        //    break;
                                        //}
                                    }
                                    SetUIValuesToObject(ControlsEnum.CHEQUERETURN);
                                }
                                else
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("MsgErr_ReturnEntry_Not_Journalized").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                            }
                            else
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_Err_ReturnEntry_Is_PDC").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                break;
                            }
                        }
                        break;
                    #endregion
                    #region PrintWHT
                    case ActionsEnum.PRINTWHT:
                        if (CurrPK > 0)
                        {
                            if (TempWHTTaxDetails != null & TempWHTTaxDetails.Count > 0)
                            {
                                //if (TempWHTTaxDetails[0].WTH_FORM_NO == Convert.ToInt32(WHTFormNo.PND54))
                                //{
                                //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx" + "?ID=" + CurrPK + "&APPTYPE=" + ApplicationType.VP +
                                //    "&APPSUBTYPE=" + Convert.ToInt32(AppSubTypeVP.PND54) + "');", true);
                                //}
                                //else
                                //{
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx" + "?ID=" + CurrPK + "&APPTYPE=" + ApplicationType.VP +
                                "&APPSUBTYPE=" + Convert.ToInt32(AppSubTypeVP.WHTCERTIFICATE) + "');", true);
                                //}

                            }
                        }
                        break;
                    #endregion
                    #region EDITFORCANCEL
                    case ActionsEnum.EDITFORCANCEL:
                        FillProcessID(11);
                        SetUIEditView(commonActions);
                        ModifiedDatePnl.Visible = true;
                        mode = String.IsNullOrEmpty(ddlMode.SelectedValue) ? 0 : Convert.ToInt32(ddlMode.SelectedValue);
                        SetPaymentModeDetails(mode);
                        //switch (mode)
                        //{
                        //    case (int)PaymentModeEnum.CASH:
                        //        vrfBranch.Enabled = vrfBranchHdr.Enabled = false;
                        //        vrfAccountNo.Enabled = vrfAccountNoHdr.Enabled = false;
                        //        vrfInstrumentNo.Enabled = vrfInstrumentNoHdr.Enabled = false;
                        //        vrfInstrumentDate.Enabled = vrfInstrumentDateHdr.Enabled = false;
                        //        vrfFavourof.Enabled = vrfFavourofHdr.Enabled = false;
                        //        txtInstrumentNo.Enabled = false;
                        //        txtInstrumentDate.Enabled = false;
                        //        txtFavourof.Enabled = false;
                        //        txtInstrumentNo.CssClass = "Uiinput-amount input-disabled medium";
                        //        txtInstrumentDate.CssClass = "Uidate-picker input-disabled";
                        //        txtFavourof.CssClass = "multiline-2line input-disabled";
                        //        Label5.Visible = false;
                        //        txtBankCharge.Visible = false;
                        //        chkBankCharge.Visible = false;
                        //        break;
                        //    case (int)PaymentModeEnum.OTHERS:
                        //        vrfBranch.Enabled = vrfBranchHdr.Enabled = false;
                        //        vrfAccountNo.Enabled = vrfAccountNoHdr.Enabled = false;
                        //        vrfInstrumentNo.Enabled = vrfInstrumentNoHdr.Enabled = false;
                        //        vrfInstrumentDate.Enabled = vrfInstrumentDateHdr.Enabled = false;
                        //        vrfFavourof.Enabled = vrfFavourofHdr.Enabled = false;
                        //        txtInstrumentNo.Enabled = false;
                        //        txtInstrumentDate.Enabled = false;
                        //        txtFavourof.Enabled = false;
                        //        txtInstrumentNo.CssClass = "Uiinput-amount input-disabled medium";
                        //        txtInstrumentDate.CssClass = "Uidate-picker input-disabled";
                        //        txtFavourof.CssClass = "multiline-2line input-disabled";
                        //        Label5.Visible = false;
                        //        txtBankCharge.Visible = false;
                        //        chkBankCharge.Visible = false;
                        //        txtPaymentBank.Text = string.Empty;
                        //        txtPaymentBank.Enabled = false;
                        //        txtPaymentBank.CssClass = "input-disabled";
                        //        hdfPaymentBank.Value = string.Empty;
                        //        break;
                        //    default:
                        //        vrfBranch.Enabled = vrfBranchHdr.Enabled = true;
                        //        vrfAccountNo.Enabled = vrfAccountNoHdr.Enabled = true;
                        //        vrfInstrumentNo.Enabled = vrfInstrumentNoHdr.Enabled = true;
                        //        vrfInstrumentDate.Enabled = vrfInstrumentDateHdr.Enabled = true;
                        //        vrfFavourof.Enabled = vrfFavourofHdr.Enabled = true;
                        //        txtInstrumentNo.Enabled = true;
                        //        txtInstrumentDate.Enabled = true;
                        //        txtFavourof.Enabled = true;
                        //        txtInstrumentNo.CssClass = "Uiinput-amount medium";
                        //        txtInstrumentDate.CssClass = "Uidate-picker";
                        //        txtFavourof.CssClass = "multiline-2line";
                        //        Label5.Visible = true;
                        //        txtBankCharge.Visible = true;
                        //        chkBankCharge.Visible = true;
                        //        break;
                        //}
                        break;
                    #endregion
                    #region DELETESUBMIT popup
                    case ActionsEnum.DELETESUBMIT:
                        //Show WorkFlow Popup
                        hdfIsSaveSubmit.Value = CommonConstants.SELECTVAL;
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    #endregion

                    #region WHTTAXHEADER
                    case ActionsEnum.WHTTAXHEADER:
                        //TempWHTTaxDetails = WHTTaxDetails;                       
                        GetFieldValues(ControlsEnum.VENDORACCOUNT);
                        SetFieldValues(ControlsEnum.VENDORACCOUNT);
                        GetFieldValues(ControlsEnum.VENDOR);
                        SetFieldValues(ControlsEnum.WHTPOPUPGRID);
                        SetFieldValues(ControlsEnum.VENDOR);
                        GetFieldValues(ControlsEnum.PAYMENTTYPE);
                        SetFieldValues(ControlsEnum.PAYMENTTYPE);
                        ShowWhtPopup();
                        SetWhtButtons();

                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("WHTTaxDetails").ToString() + "','800','400');", true);
                        break;
                    #endregion
                    #region WHT ACCOUNT Changed
                    case ActionsEnum.WHT_ACCOUNT_INDEX_CHANGED_POPUP:
                        //SetVendorPayment();
                        //whtTaxpk = ddlWHTAccountPopup.SelectedValue != CommonConstants.SELECTVAL ? Convert.ToInt32(ddlWHTAccountPopup.SelectedValue) : 0;
                        whtTaxpk = !string.IsNullOrEmpty(hdfWHTAccountPopup.Value) ? Convert.ToInt32(hdfWHTAccountPopup.Value) : 0;
                        //if (chkVendorforpayemnt.Checked)
                        //{
                        GetFieldValues(ControlsEnum.VENDORACCOUNTTAX);
                        //SetFieldValues(ControlsEnum.VENDORACCOUNTTAX);
                        if (dtVendorAccount != null && dtVendorAccount.Rows.Count > 0)
                        {
                            decimal amount = 0;
                            amount = txtPopupWHTAmount.Text != string.Empty ? Convert.ToDecimal(txtPopupWHTAmount.Text) : 0;
                            string taxFormula = dtVendorAccount.Rows[0]["TAX_FORMULA"].ToString();
                            hdfTaxformula.Value = taxFormula;
                            taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                            decimal amt = Convert.ToDecimal(StringToFormula(taxFormula));
                            txtWHTTaxAmountPopup.Text = ERP.Utilities.CommonFunctions.DoubleFormatRound(Convert.ToDouble(amt), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString(); //Math.Round(amt, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            txtDescriptionPopup.Text = dtVendorAccount.Rows[0]["TAX_DESC"].ToString();
                            hdfWHTTaxCategory.Value = dtVendorAccount.Rows[0]["TAX_CATEGORY"].ToString();
                            hdfWHTTaxName.Value = dtVendorAccount.Rows[0]["TAX_HEAD"].ToString();
                        }
                        else
                        {
                            txtWHTTaxAmountPopup.Text = Math.Round(Convert.ToDouble(0), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            txtDescriptionPopup.Text = string.Empty;
                        }
                        ShowWhtPopup();
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("WHTTaxDetails").ToString() + "','800','400');", true);
                        //}
                        //else
                        //{
                        //    txtWHTAmount.Text = "0.00";
                        //}

                        break;
                    #endregion
                    #region WHT TAX ADD
                    case ActionsEnum.WHTTAXADD:
                        bool errorWHTAdd = false;
                        bool errorWHTAmount = false;
                        int WhtDetRowIndex = -1;
                        if (ViewState["WhtDetRowIndex"] != null) WhtDetRowIndex = (int)(ViewState["WhtDetRowIndex"]);

                        int? WHTVendorAddressType = null;
                        if (!string.IsNullOrEmpty(hdfWthAddressType.Value))
                        {
                            WHTVendorAddressType = Convert.ToInt32(hdfWthAddressType.Value);
                        }
                        tempWHTTaxDetails = null;
                        tempWHTTaxDetails = TempWHTTaxDetails.DeepClone();
                        if (tempWHTTaxDetails != null && WhtDetRowIndex >= 0)
                        {
                            tempWHTTaxDetails[WhtDetRowIndex].WTH_TAX = Convert.ToInt32(hdfWHTAccountPopup.Value);
                            //tempWHTTaxDetails.WTH_PK = 0;
                            //tempWHTTaxDetails.WTH_PAYMENT_HDR = CurrPK;
                            tempWHTTaxDetails[WhtDetRowIndex].WTH_TRX_HDR = CurrPK;
                            tempWHTTaxDetails[WhtDetRowIndex].WTH_TYPE = 1;
                            tempWHTTaxDetails[WhtDetRowIndex].WTH_CATEGORY = (byte)WHTCategoryEnum.WHT;
                            tempWHTTaxDetails[WhtDetRowIndex].WTH_TAX_CATEGORY = string.IsNullOrEmpty(hdfWHTTaxCategory.Value) ? Convert.ToByte(1) : Convert.ToByte(hdfWHTTaxCategory.Value);
                            tempWHTTaxDetails[WhtDetRowIndex].WTH_NAME = hdfWHTTaxName.Value;
                            tempWHTTaxDetails[WhtDetRowIndex].WTH_DESC = txtDescriptionPopup.Text;
                            tempWHTTaxDetails[WhtDetRowIndex].WTH_FORM_NO = Convert.ToInt32(ddlFormno.SelectedValue);
                            tempWHTTaxDetails[WhtDetRowIndex].WTH_PARTY_NAME = txtCustomerTxtWHT.Text;
                            tempWHTTaxDetails[WhtDetRowIndex].WTH_ADDRESS = txtpartyads.Text;
                            tempWHTTaxDetails[WhtDetRowIndex].WTH_TAX_ID = txtTaxid.Text;
                            tempWHTTaxDetails[WhtDetRowIndex].WTH_AMOUNT = Convert.ToDecimal(txtPopupWHTAmount.Text);
                            tempWHTTaxDetails[WhtDetRowIndex].WTH_TAX_AMT = Convert.ToDecimal(txtWHTTaxAmountPopup.Text);
                            tempWHTTaxDetails[WhtDetRowIndex].WTH_PAYMENT_TYPE = Convert.ToByte(ddlPayType.SelectedValue);
                            tempWHTTaxDetails[WhtDetRowIndex].WTH_BRANCH_TEXT = txtWthBranchCode.Text;
                            tempWHTTaxDetails[WhtDetRowIndex].WTH_BRANCH = WHTVendorAddressType;
                            tempWHTTaxDetails[WhtDetRowIndex].WTH_BRANCH_NAME = txtWthAddressType.Text;
                            tempWHTTaxDetails[WhtDetRowIndex].WTH_BRANCH_TYPE = chkWthHeadOffice.Checked ? (byte)VendorContactTypeEnum.HeadOffice : (byte)VendorContactTypeEnum.Branch;

                            TempWHTTaxDetails = tempWHTTaxDetails;
                            SetFieldValues(ControlsEnum.WHTPOPUPGRID);
                            finVatPaymentDetails = null;
                            ViewState["WhtDetRowIndex"] = null;
                            txtPopupWHTAmount.Text = string.Empty;
                            txtWHTTaxAmountPopup.Text = string.Empty;
                            txtDescriptionPopup.Text = string.Empty;
                            ddlPayType.SelectedIndex = 0;
                        }
                        else
                        {
                            tempWHTTax = null;
                            tempWHTTax = tempWHTTaxDetails.SingleOrDefault(tax => tax.WTH_TAX == Convert.ToInt32(hdfWHTAccountPopup.Value) && tax.WTH_FORM_NO == Convert.ToInt32(ddlFormno.SelectedValue) && tax.WTH_PARTY_NAME == txtCustomerTxtWHT.Text);
                            if (tempWHTTax == null)
                            {
                                tempWHTTax = new FIN_PAYMENT_VND_TAX_HDR();
                                tempWHTTax = CommonFunctions.Initilize<FIN_PAYMENT_VND_TAX_HDR>();
                                //try
                                //{
                                tempWHTTax.WTH_AMOUNT = string.IsNullOrEmpty(txtPopupWHTAmount.Text) ? Convert.ToDecimal(0) : Convert.ToDecimal(txtPopupWHTAmount.Text);
                                tempWHTTax.WTH_TAX_AMT = string.IsNullOrEmpty(txtWHTTaxAmountPopup.Text) ? Convert.ToDecimal(0) : Convert.ToDecimal(txtWHTTaxAmountPopup.Text);
                                //}
                                //catch
                                //{
                                //    errorWHTAmount = true;
                                //}
                                if (!errorWHTAmount)
                                {
                                    tempWHTTax.WTH_TAX = Convert.ToInt32(hdfWHTAccountPopup.Value);
                                    tempWHTTax.WTH_PK = 0;
                                    tempWHTTax.WTH_PAYMENT_HDR = CurrPK;
                                    tempWHTTax.WTH_TYPE = 1;
                                    tempWHTTax.WTH_TAX_CATEGORY = string.IsNullOrEmpty(hdfWHTTaxCategory.Value) ? Convert.ToByte(1) : Convert.ToByte(hdfWHTTaxCategory.Value);
                                    tempWHTTax.WTH_NAME = hdfWHTTaxName.Value;
                                    tempWHTTax.WTH_DESC = txtDescriptionPopup.Text;
                                    tempWHTTax.WTH_FORM_NO = Convert.ToInt32(ddlFormno.SelectedValue);
                                    tempWHTTax.WTH_PARTY_NAME = txtCustomerTxtWHT.Text;
                                    tempWHTTax.WTH_ADDRESS = txtpartyads.Text;
                                    tempWHTTax.WTH_TAX_ID = txtTaxid.Text;
                                    tempWHTTax.WTH_CATEGORY = (byte)WHTCategoryEnum.WHT;
                                    tempWHTTax.WTH_TAX_DATE = string.IsNullOrEmpty(txtPaymentDate.Text.Trim()) ? DateTime.Now : DateTime.Parse(txtPaymentDate.Text.Trim());
                                    tempWHTTax.WTH_PAYMENT_TYPE = Convert.ToByte(ddlPayType.SelectedValue);
                                    tempWHTTax.WTH_BRANCH_TEXT = txtWthBranchCode.Text;
                                    tempWHTTax.WTH_BRANCH = WHTVendorAddressType;
                                    tempWHTTax.WTH_BRANCH_NAME = txtWthAddressType.Text;
                                    tempWHTTax.WTH_BRANCH_TYPE = chkWthHeadOffice.Checked ? (byte)VendorContactTypeEnum.HeadOffice : (byte)VendorContactTypeEnum.Branch;

                                    tempWHTTaxDetails.Add(tempWHTTax);
                                    TempWHTTaxDetails = tempWHTTaxDetails;
                                    SetFieldValues(ControlsEnum.WHTPOPUPGRID);
                                    finVatPaymentDetails = null;
                                    ViewState["WhtDetRowIndex"] = null;
                                }
                            }
                            else
                            {
                                errorWHTAdd = true;
                            }
                            if (!errorWHTAdd && !errorWHTAmount)
                            {
                                txtPopupWHTAmount.Text = string.Empty;
                                txtWHTTaxAmountPopup.Text = string.Empty;
                                txtDescriptionPopup.Text = string.Empty;
                                ddlPayType.SelectedIndex = 0;
                            }
                        }

                        ShowWhtPopup();
                        SetWhtButtons();
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("WHTTaxDetails").ToString() + "','800','400');", true);
                        if (errorWHTAdd)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Tax_Add").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else if (errorWHTAmount)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Tax_Amount").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region WHTTAXDELETE
                    case ActionsEnum.WHTTAXDELETE:
                        tempWHTTaxDetails = null;
                        if (TempWHTTaxDetails != null)
                        {
                            tempWHTTax = null;
                            tempWHTTaxDetails = TempWHTTaxDetails;
                            HiddenField hdfTaxPK = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfWHTTaxPK") as HiddenField);
                            HiddenField hdfTaxName = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfWHTTaxName") as HiddenField);
                            if (hdfTaxPK != null)
                            {
                                int taxPK = string.IsNullOrEmpty(hdfTaxPK.Value) ? 0 : Convert.ToInt32(hdfTaxPK.Value);
                                if (taxPK > 0)
                                {
                                    //commented temporarily
                                    //tempWHTTax = tempWHTTaxDetails.SingleOrDefault(rfq => rfq.WTH_PK == taxPK);// && rfq.WTH_TAX_CATEGORY == Convert.ToInt32(hdfWHTTaxCategory.Value));
                                    tempWHTTax = tempWHTTaxDetails.FirstOrDefault(rfq => rfq.WTH_PK == taxPK);// && rfq.WTH_TAX_CATEGORY == Convert.ToInt32(hdfWHTTaxCategory.Value));
                                }
                                else
                                {
                                    if (hdfTaxName != null)
                                    {
                                        //commented temporarily
                                        //tempWHTTax = tempWHTTaxDetails.SingleOrDefault(rfq => rfq.WTH_NAME == hdfTaxName.Value);// && rfq.VTL_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                        tempWHTTax = tempWHTTaxDetails.FirstOrDefault(rfq => rfq.WTH_NAME == hdfTaxName.Value);// && rfq.VTL_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                    }
                                }
                                if (tempWHTTax != null)
                                {
                                    tempWHTTaxDetails.Remove(tempWHTTax);
                                    TempWHTTaxDetails = tempWHTTaxDetails;
                                }
                            }
                            SetFieldValues(ControlsEnum.WHTPOPUPGRID);


                            whtHdrAmnt = 0;
                            whtSplitAmnt = 0;
                            decimal.TryParse(txtWHTAmount.Text, out whtHdrAmnt);
                            whtSplitAmnt = Convert.ToDecimal(CommonFunctions.DoubleFormatRound(Convert.ToDouble(TempWHTTaxDetails.Sum(aa => aa.WTH_TAX_AMT)), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits));
                            if (whtHdrAmnt != whtSplitAmnt)
                            {
                                btnApply.Visible = true;
                                btnWhtSave.Visible = false;
                            }
                            else
                            {
                                btnApply.Visible = false;
                                btnWhtSave.Visible = true;
                            }
                        }
                        ShowWhtPopup();
                        SetWhtButtons();
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("WHTTaxDetails").ToString() + "','800','400');", true);
                        break;
                    #endregion
                    #region WHT TAX APPLY
                    case ActionsEnum.WHTTAXAPPLY:
                        tempWHTTaxDetails = null;
                        if (TempWHTTaxDetails != null && TempWHTTaxDetails.Count > 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                            tempWHTTaxDetails = WHTTaxDetails = TempWHTTaxDetails;
                            decimal totalWHTAmount = tempWHTTaxDetails.Sum(aa => aa.WTH_TAX_AMT);
                            txtWHTAmount.Text = totalWHTAmount.ToString(hdfCurrencyFormat.Value);
                        }
                        else
                        {
                            txtWHTAmount.Text = 0.ToString(hdfCurrencyFormat.Value);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + GetLocalResourceObject("Msg_Save_trx_wht") + "','" + Resources.Messages.Information + "');", true);

                        break;

                    #endregion
                    #region Edit WHT Item From Grid
                    case ActionsEnum.POPUPGRIDEDITWHT:
                        GridViewRow grwWhtDetails = (GridViewRow)((ImageButton)(sender)).Parent.Parent;
                        SetUIEditViewWhtPopup(grwWhtDetails);
                        ShowWhtPopup();
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("WHTTaxDetails").ToString() + "','800','400');", true);
                        break;
                    #endregion
                    #region WHTTAXSAVE
                    case ActionsEnum.WHTTAXSAVE:

                        poPaymentServiceClient = new POPaymentService();
                        poPaymentServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(poPaymentServiceClient);
                        tempWHTTaxDetails = null;
                        divWhtErrorLabel.Visible = false;
                        lblWhtErrorMessage.Text = string.Empty;
                        if (CurrPK > 0)
                        {
                            GetFieldValues(ControlsEnum.PAYMENTHDRENTRYBYPK);
                            if (finPaymentVndHdrList != null && finPaymentVndHdrList.Count > 0)
                            {
                                if (grdWHTTaxDetails.Rows.Count > 0)
                                {
                                    if (TempWHTTaxDetails != null && TempWHTTaxDetails.Count > 0)
                                    {

                                        if (TempWHTTaxDetails != null && TempWHTTaxDetails.Count > 0)
                                        {
                                            tempWHTTaxDetails = WHTTaxDetails = TempWHTTaxDetails.DeepClone();
                                            VatBuyTax = Convert.ToDecimal(CommonFunctions.DoubleFormatRound(Convert.ToDouble(tempWHTTaxDetails.Sum(aa => aa.WTH_TAX_AMT)), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits));

                                        }
                                        decimal.TryParse(txtWHTAmount.Text, out vatTax);
                                        if (finPaymentVndHdrList[0].PVH_HAS_JRNL_ENTRY)
                                        {
                                            if (vatTax != VatBuyTax && vatTax > 0)
                                            {
                                                divWhtErrorLabel.Visible = true;
                                                lblWhtErrorMessage.Text = GetLocalResourceObject("Err_WhtAmnt").ToString() + " " + txtWHTAmount.Text;
                                                ShowWhtPopup();
                                                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("WHTTaxDetails").ToString() + "','800','400');", true);
                                                return;
                                            }
                                            else if (vatTax == 0)
                                            {
                                                divWhtErrorLabel.Visible = true;
                                                lblWhtErrorMessage.Text = GetLocalResourceObject("Err_Posted").ToString();
                                                ShowWhtPopup();
                                                return;
                                            }
                                        }
                                        else
                                        {
                                            txtWHTAmount.Text = String.Format("{0:c}", VatBuyTax);
                                        }
                                        result = poPaymentServiceClient.SaveDirectVatTaxForPOPayment(tempWHTTaxDetails);
                                        if (result >= 0)
                                        {
                                            litErrorMsg.Text = Resources.Messages.Msg_Save_Success;
                                            litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), GetLocalResourceObject("Tax").ToString());
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                        }

                                    }
                                }
                                else
                                {
                                    //divWhtErrorLabel.Visible = true;
                                    //lblWhtErrorMessage.Text = GetLocalResourceObject("Err_NoRecord").ToString();
                                    //ShowWhtPopup();
                                    if (finPaymentVndHdrList[0].PVH_HAS_JRNL_ENTRY)
                                    {
                                        divWhtErrorLabel.Visible = true;
                                        lblWhtErrorMessage.Text = GetLocalResourceObject("Err_Posted").ToString();
                                        ShowWhtPopup();
                                        return;
                                    }

                                    try
                                    {
                                        tempWHTTaxDetails = WHTTaxDetails = TempWHTTaxDetails.DeepClone();
                                    }
                                    catch { }
                                    result = poPaymentServiceClient.DeleteDirectVatTaxForPOPayment(CurrPK, (byte)WHTCategoryEnum.WHT);
                                    if (result >= 0)
                                    {
                                        litErrorMsg.Text = Resources.Messages.Msg_Save_Success;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), GetLocalResourceObject("Tax").ToString());
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                    }

                                    txtWHTAmount.Text = String.Format("{0:c}", Convert.ToDecimal(0));

                                }
                            }
                        }

                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);

                        break;
                    #endregion
                    #region CHNAGETYPE WHTPOPUP
                    case ActionsEnum.WHTCHANGETYPE:
                        txtWthBranchCode.Text = string.Empty;
                        GetFieldValues(ControlsEnum.VENDORCONTACTFORWHT);
                        SetFieldValues(ControlsEnum.VENDORCONTACTFORWHT);
                        ShowWhtPopup();
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("WHTTaxDetails").ToString() + "','800','400');", true);
                        break;
                    #endregion

                    #region VATTAXHEADER
                    case ActionsEnum.VATTAXHEADER:
                        //if (EntryStatus == EntryStatus.NEWMODE)
                        //    btnVatTaxSave.Visible = false;
                        txtBeforeTaxAmount.Text = 0.ToString(hdfCurrencyFormat.Value);
                        txtVATTaxAmountPopup.Text = 0.ToString(hdfCurrencyFormat.Value);
                        txtVatTaxInvNo.Text = string.Empty;
                        txtVatTaxInvDate.Text = string.Empty;
                        chkOriginalinvoice.Checked = false;
                        txtMaterial.Text = string.Empty;
                        txtVatRefundDate.Text = string.IsNullOrEmpty(txtPaymentDate.Text) ? string.Empty : Convert.ToDateTime(txtPaymentDate.Text).ToString(Resources.Constants.DateFormatMonthYear);

                        divVatErrorLabel.Visible = false;
                        lblVatSplitErrorMessage.Text = string.Empty;
                        ViewState["VatDetRowIndex"] = null;
                        GetFieldValues(ControlsEnum.VENDOR);
                        SetFieldValues(ControlsEnum.VATBUYVENDOR);
                        GetFieldValues(ControlsEnum.PURINVNOS);
                        SetFieldValues(ControlsEnum.PURINVNOS);
                        GetFieldValues(ControlsEnum.VATPOPUPGRID);
                        SetFieldValues(ControlsEnum.VATPOPUPGRID);
                        GetFieldValues(ControlsEnum.VATBUYTAXTYPES);
                        SetFieldValues(ControlsEnum.VATBUYTAXTYPES);
                        GetUIValuesFromObject(ControlsEnum.TAXTYPECHANGED);

                        GetFieldValues(ControlsEnum.VENDORSELECTEDDTL);
                        SetFieldValues(ControlsEnum.VENDORSELECTEDDTL);
                        GetFieldValues(ControlsEnum.VENDORCONTACTYPE);
                        SetFieldValues(ControlsEnum.VENDORCONTACTYPE);
                        ShowVatbuyPopup();
                        SetBranchCodeVisibility();
                        GetFieldValues(ControlsEnum.VATBUYPOPUPHEADER);
                        break;
                    #endregion
                    #region VAT ACCOUNT Changed
                    case ActionsEnum.VAT_ACCOUNT_INDEX_CHANGED_POPUP:
                        VatBuyTaxpk = !string.IsNullOrEmpty(hdfVATAccountPopup.Value) ? Convert.ToInt32(hdfVATAccountPopup.Value) : 0;
                        //if (chkVendorforpayemnt.Checked)
                        //{
                        GetFieldValues(ControlsEnum.VENDORACCOUNTVATBUYTAX);
                        //SetFieldValues(ControlsEnum.VENDORACCOUNTTAX);
                        if (dtVendorAccount != null && dtVendorAccount.Rows.Count > 0)
                        {
                            decimal amount = 0;
                            amount = txtBeforeTaxAmount.Text != string.Empty ? Convert.ToDecimal(txtBeforeTaxAmount.Text) : 0;
                            string taxFormula = dtVendorAccount.Rows[0]["TAX_FORMULA"].ToString();
                            hdfTaxformula.Value = taxFormula;
                            taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                            decimal amt = Convert.ToDecimal(StringToFormula(taxFormula));
                            txtVATTaxAmountPopup.Text = ERP.Utilities.CommonFunctions.DoubleFormatRound(Convert.ToDouble(amt), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString(); //Math.Round(amt, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            //txtDescriptionPopup.Text = dtVendorAccount.Rows[0]["TAX_DESC"].ToString();
                            hdfVATBUYTaxCategory.Value = dtVendorAccount.Rows[0]["TAX_CATEGORY"].ToString();
                            hdfVATBUYTaxName.Value = dtVendorAccount.Rows[0]["TAX_HEAD"].ToString();
                        }
                        else
                        {
                            txtVATTaxAmountPopup.Text = Math.Round(Convert.ToDouble(0), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            //txtDescriptionPopup.Text = string.Empty;
                        }
                        ShowVatbuyPopup();


                        break;
                    #endregion
                    #region VAT TAX ADD
                    case ActionsEnum.VATTAXADD:
                        bool errorVATAdd = false;
                        bool errorVATAmount = false;
                        decimal vatTaxAmnt = 0;
                        decimal vatTaxAmntSplit = 0;
                        int VatDetRowIndex = -1;
                        int? VendorAddressType = null;
                        int? VendorPopupPk = null;
                        if (ViewState["VatDetRowIndex"] != null) VatDetRowIndex = (int)(ViewState["VatDetRowIndex"]);
                        if (VatDetRowIndex < 0)
                            GetFieldValues(ControlsEnum.PAYMENTTAXHDR);

                        tempVATTaxDetails = null;
                        tempVATTaxDetails = TempVATTaxDetails;
                        divVatErrorLabel.Visible = false;
                        lblVatSplitErrorMessage.Text = string.Empty;

                        if (!string.IsNullOrEmpty(hdfAddressType.Value))
                        {
                            VendorAddressType = Convert.ToInt32(hdfAddressType.Value);
                        }
                        if (!string.IsNullOrEmpty(hdfVendorPopup.Value))
                        {
                            VendorPopupPk = Convert.ToInt32(hdfVendorPopup.Value);
                        }
                        if (finPymntList != null && finPymntList.Count > 0)
                        {
                            divVatErrorLabel.Visible = true;
                            lblVatSplitErrorMessage.Text = GetLocalResourceObject("TaxAlteadyExist").ToString();
                            ShowVatbuyPopup();
                            return;
                        }
                        vatTaxAmnt = GetTaxAmount(Convert.ToInt64(ddlPurInvNo.SelectedValue));
                        if (tempVATTaxDetails != null && VatDetRowIndex >= 0)
                        {
                            //vatTaxAmntSplit = tempVATTaxDetails.Where(inv => inv.WTH_PUR_INVOICE == Convert.ToInt64(ddlPurInvNo.SelectedValue)).Sum(tx => tx.WTH_TAX_AMT);
                            //vatTaxAmntSplit = vatTaxAmntSplit - tempVATTaxDetails[VatDetRowIndex].WTH_TAX_AMT + Convert.ToDecimal(txtVATTaxAmountPopup.Text);
                            //if (vatTaxAmntSplit > vatTaxAmnt)
                            //{
                            //    divVatErrorLabel.Visible = true;
                            //    lblVatSplitErrorMessage.Text = GetLocalResourceObject("Err_VatTaxSplit").ToString() + " " + vatTaxAmnt.ToString();
                            //    ShowVatbuyPopup();
                            //    return;
                            //}
                            //else
                            //{
                            tempVATTaxDetails[VatDetRowIndex].WTH_TAX = Convert.ToInt32(ddlVATAccountPopup.SelectedValue);
                            //tempVATTaxDetails[VatDetRowIndex].WTH_PK = 0;
                            tempVATTaxDetails[VatDetRowIndex].WTH_PAYMENT_HDR = CurrPK;
                            //tempVATTaxDetails[VatDetRowIndex].WTH_TRX_HDR = CurrPK;
                            tempVATTaxDetails[VatDetRowIndex].WTH_TYPE = 1;
                            tempVATTaxDetails[VatDetRowIndex].WTH_CATEGORY = (byte)WHTCategoryEnum.VATBUY;
                            tempVATTaxDetails[VatDetRowIndex].WTH_TAX_INV_NO = txtVatTaxInvNo.Text;
                            tempVATTaxDetails[VatDetRowIndex].WTH_INV_RECEIVED = chkOriginalinvoice.Checked ? (byte)1 : (byte)0;
                            tempVATTaxDetails[VatDetRowIndex].WTH_PUR_INVOICE = Convert.ToInt64(ddlPurInvNo.SelectedValue);
                            tempVATTaxDetails[VatDetRowIndex].WTH_VENDOR = VendorPopupPk;
                            tempVATTaxDetails[VatDetRowIndex].WTH_BRANCH = VendorAddressType;
                            tempVATTaxDetails[VatDetRowIndex].WTH_BRANCH_TEXT = txtBranchCode.Text;
                            tempVATTaxDetails[VatDetRowIndex].WTH_BRANCH = VendorAddressType;
                            tempVATTaxDetails[VatDetRowIndex].WTH_BRANCH_NAME = txtAddressType.Text;
                            tempVATTaxDetails[VatDetRowIndex].WTH_BRANCH_TYPE = chkHeadOffice.Checked ? (byte)VendorContactTypeEnum.HeadOffice : (byte)VendorContactTypeEnum.Branch;
                            tempVATTaxDetails[VatDetRowIndex].WTH_TAX_DATE = string.IsNullOrEmpty(txtVatTaxInvDate.Text.Trim()) ? DateTime.Now : DateTime.Parse(txtVatTaxInvDate.Text.Trim());
                            tempVATTaxDetails[VatDetRowIndex].WTH_REFUND_DATE = string.IsNullOrEmpty(txtVatRefundDate.Text.Trim()) ? DateTime.Now : DateTime.Parse(txtVatRefundDate.Text.Trim());
                            tempVATTaxDetails[VatDetRowIndex].WTH_TAX_CATEGORY = Convert.ToByte(ddlVATAccountPopup.SelectedValue);// string.IsNullOrEmpty(hdfVATBUYTaxCategory.Value) ? Convert.ToByte(1) : Convert.ToByte(hdfVATBUYTaxCategory.Value);
                            tempVATTaxDetails[VatDetRowIndex].WTH_NAME = ddlVATAccountPopup.SelectedItem.Text;
                            //tempVATTaxDetails[VatDetRowIndex].WTH_DESC = txtDescriptionPopup.Text;
                            //tempVATTaxDetails[VatDetRowIndex].WTH_FORM_NO = Convert.ToInt32(ddlFormno.SelectedValue);
                            tempVATTaxDetails[VatDetRowIndex].WTH_PARTY_NAME = txtVendorPopup.Text;
                            //tempVATTaxDetails[VatDetRowIndex].WTH_ADDRESS = txtpartyads.Text;
                            tempVATTaxDetails[VatDetRowIndex].WTH_TAX_ID = txtVatTaxId.Text;
                            tempVATTaxDetails[VatDetRowIndex].WTH_ITEM_TEXT = txtMaterial.Text;
                            tempVATTaxDetails[VatDetRowIndex].WTH_AMOUNT = Convert.ToDecimal(txtBeforeTaxAmount.Text);
                            tempVATTaxDetails[VatDetRowIndex].WTH_TAX_AMT = Convert.ToDecimal(txtVATTaxAmountPopup.Text);
                            TempVATTaxDetails = tempVATTaxDetails;
                            SetFieldValues(ControlsEnum.VATPOPUPGRID);
                            finVatPaymentDetails = null;
                            ViewState["VatDetRowIndex"] = null;

                            txtBeforeTaxAmount.Text = 0.ToString(hdfCurrencyFormat.Value);
                            txtVATTaxAmountPopup.Text = 0.ToString(hdfCurrencyFormat.Value);
                            txtVatTaxInvNo.Text = string.Empty;
                            txtVatTaxInvDate.Text = string.Empty;
                            chkOriginalinvoice.Checked = false;
                            txtMaterial.Text = string.Empty;
                            // }
                        }
                        else
                        {
                            tempVATTax = null;
                            tempVATTax = tempVATTaxDetails.SingleOrDefault(tax => tax.WTH_TAX_DATE == DateTime.Parse(txtVatTaxInvDate.Text.Trim()) && tax.WTH_TAX_INV_NO == txtVatTaxInvNo.Text);
                            if (tempVATTax == null)
                            {
                                tempVATTax = new FIN_PAYMENT_VND_TAX_HDR();
                                tempVATTax = CommonFunctions.Initilize<FIN_PAYMENT_VND_TAX_HDR>();
                                tempVATTax.WTH_AMOUNT = string.IsNullOrEmpty(txtBeforeTaxAmount.Text) ? Convert.ToDecimal(0) : Convert.ToDecimal(txtBeforeTaxAmount.Text);
                                tempVATTax.WTH_TAX_AMT = string.IsNullOrEmpty(txtVATTaxAmountPopup.Text) ? Convert.ToDecimal(0) : Convert.ToDecimal(txtVATTaxAmountPopup.Text);
                                if (!errorVATAmount)
                                {
                                    //vatTaxAmntSplit = tempVATTaxDetails.Where(inv => inv.WTH_PUR_INVOICE == Convert.ToInt64(ddlPurInvNo.SelectedValue)).Sum(tx => tx.WTH_TAX_AMT);
                                    //vatTaxAmntSplit = vatTaxAmntSplit + Convert.ToDecimal(txtVATTaxAmountPopup.Text);
                                    //if (vatTaxAmntSplit > vatTaxAmnt)
                                    //{
                                    //    divVatErrorLabel.Visible = true;
                                    //    lblVatSplitErrorMessage.Text = GetLocalResourceObject("Err_VatTaxSplit").ToString() + " " + vatTaxAmnt.ToString();
                                    //    ShowVatbuyPopup();
                                    //    return;
                                    //}
                                    //else
                                    //{
                                    tempVATTax.WTH_TAX = Convert.ToInt32(hdfVATAccountPopup.Value);
                                    tempVATTax.WTH_PK = 0;
                                    tempVATTax.WTH_PAYMENT_HDR = CurrPK;
                                    //tempVATTax.WTH_TRX_HDR = CurrPK;
                                    tempVATTax.WTH_TYPE = 1;
                                    tempVATTax.WTH_CATEGORY = (byte)WHTCategoryEnum.VATBUY;
                                    tempVATTax.WTH_TAX_INV_NO = txtVatTaxInvNo.Text;
                                    tempVATTax.WTH_INV_RECEIVED = chkOriginalinvoice.Checked ? (byte)1 : (byte)0;
                                    tempVATTax.WTH_PUR_INVOICE = Convert.ToInt64(ddlPurInvNo.SelectedValue);
                                    tempVATTax.WTH_VENDOR = VendorPopupPk;
                                    tempVATTax.WTH_BRANCH_TEXT = txtBranchCode.Text;
                                    tempVATTax.WTH_BRANCH = VendorAddressType;
                                    tempVATTax.WTH_BRANCH_NAME = txtAddressType.Text;
                                    tempVATTax.WTH_BRANCH_TYPE = chkHeadOffice.Checked ? (byte)VendorContactTypeEnum.HeadOffice : (byte)VendorContactTypeEnum.Branch;
                                    tempVATTax.WTH_TAX_DATE = string.IsNullOrEmpty(txtVatTaxInvDate.Text.Trim()) ? DateTime.Now : DateTime.Parse(txtVatTaxInvDate.Text.Trim());
                                    tempVATTax.WTH_REFUND_DATE = string.IsNullOrEmpty(txtVatRefundDate.Text.Trim()) ? DateTime.Now : DateTime.Parse(txtVatRefundDate.Text.Trim());
                                    tempVATTax.WTH_TAX_CATEGORY = Convert.ToByte(ddlVATAccountPopup.SelectedValue);// string.IsNullOrEmpty(hdfVATBUYTaxCategory.Value) ? Convert.ToByte(1) : Convert.ToByte(hdfVATBUYTaxCategory.Value);
                                    tempVATTax.WTH_NAME = ddlVATAccountPopup.SelectedItem.Text;
                                    //tempVATTax.WTH_DESC = txtDescriptionPopup.Text;
                                    tempVATTax.WTH_FORM_NO = null;
                                    tempVATTax.WTH_PARTY_NAME = txtVendorPopup.Text;
                                    //tempVATTax.WTH_ADDRESS = txtpartyads.Text;
                                    tempVATTax.WTH_TAX_ID = txtVatTaxId.Text;
                                    tempVATTax.WTH_ITEM_TEXT = txtMaterial.Text;
                                    tempVATTax.WTH_AMOUNT = Convert.ToDecimal(txtBeforeTaxAmount.Text);
                                    tempVATTax.WTH_TAX_AMT = Convert.ToDecimal(txtVATTaxAmountPopup.Text);
                                    tempVATTaxDetails.Add(tempVATTax);
                                    TempVATTaxDetails = tempVATTaxDetails;
                                    SetFieldValues(ControlsEnum.VATPOPUPGRID);
                                    finVatPaymentDetails = null;
                                    ViewState["VatDetRowIndex"] = null;
                                    //}
                                }
                            }
                            else
                            {
                                errorVATAdd = true;
                            }
                            if (!errorVATAdd && !errorVATAmount)
                            {
                                txtBeforeTaxAmount.Text = 0.ToString(hdfCurrencyFormat.Value);
                                txtVATTaxAmountPopup.Text = 0.ToString(hdfCurrencyFormat.Value);
                                txtVatTaxInvNo.Text = string.Empty;
                                txtVatTaxInvDate.Text = string.Empty;
                                chkOriginalinvoice.Checked = false;
                                txtMaterial.Text = string.Empty;
                            }
                        }

                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CloseMsgPopup1", "CloseMsgPopup();", true);
                        ShowVatbuyPopup();

                        if (errorVATAdd)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Tax_Add").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else if (errorVATAmount)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Tax_Amount").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region VATTAXDELETE
                    case ActionsEnum.VATTAXDELETE:
                        tempVATTaxDetails = null;
                        if (TempVATTaxDetails != null)
                        {
                            tempVATTax = null;
                            tempVATTaxDetails = TempVATTaxDetails;
                            HiddenField hdfTaxPK = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfVATTaxPK") as HiddenField);
                            HiddenField hdfTaxName = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfVATTaxName") as HiddenField);
                            HiddenField hdfTaxDate = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfTaxDate") as HiddenField);
                            Label lblVatTaxInvNo = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("lblVatTaxInvNo") as Label);
                            if (hdfTaxPK != null)
                            {
                                int taxPK = string.IsNullOrEmpty(hdfTaxPK.Value) ? 0 : Convert.ToInt32(hdfTaxPK.Value);
                                if (taxPK > 0)
                                {
                                    tempVATTax = tempVATTaxDetails.SingleOrDefault(rfq => rfq.WTH_PK == taxPK);// && rfq.WTH_TAX_CATEGORY == Convert.ToInt32(hdfVATTaxCategory.Value));
                                }
                                else
                                {
                                    if (hdfTaxName != null && !string.IsNullOrEmpty(hdfTaxDate.Value))
                                    {
                                        tempVATTax = tempVATTaxDetails.SingleOrDefault(rfq => rfq.WTH_TAX_DATE == Convert.ToDateTime(hdfTaxDate.Value) && rfq.WTH_TAX_INV_NO == lblVatTaxInvNo.Text);// && rfq.VTL_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                    }
                                    else if (hdfTaxName != null)
                                    {
                                        tempVATTax = tempVATTaxDetails.SingleOrDefault(rfq => rfq.WTH_TAX_INV_NO == lblVatTaxInvNo.Text);// && rfq.VTL_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                    }
                                }
                                if (tempVATTax != null)
                                {
                                    tempVATTaxDetails.Remove(tempVATTax);
                                    TempVATTaxDetails = tempVATTaxDetails;
                                }
                            }
                            SetFieldValues(ControlsEnum.VATPOPUPGRID);
                        }
                        ShowVatbuyPopup();
                        break;
                    #endregion
                    #region VATTAXAPPLY
                    case ActionsEnum.VATTAXAPPLY:
                        tempVATTaxDetails = null;
                        divVatErrorLabel.Visible = false;
                        if (TempVATTaxDetails != null && TempVATTaxDetails.Count > 0)
                        {

                            if (TempVATTaxDetails != null && TempVATTaxDetails.Count > 0)
                            {
                                tempVATTaxDetails = VATTaxDetails = TempVATTaxDetails;
                                VatBuyTax = Convert.ToDecimal(tempVATTaxDetails.Sum(aa => aa.WTH_TAX_AMT));
                            }
                            //decimal.TryParse(txtTaxAmount.Text, out vatTax);
                            decimal.TryParse(hdfVatTaxAmountHdr.Value, out vatTax);
                            if (vatTax != VatBuyTax && (hdfIscontYesVat.Value != "1") && vatTax > 0)
                            {
                                hdfAmntMissmatch.Value = GetLocalResourceObject("Err_VatAmntNotTallied").ToString();
                                ShowVatbuyPopup();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowAlreadyPaid", "$(document).ready(function(){VatAmtMismatchApply();});", true);

                                return;
                            }
                            hdfIscontYesVat.Value = "0";
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                        }
                        else
                        {
                            ShowVatbuyPopup();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_VatTaxSave").ToString()) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;

                    #endregion
                    #region VATTAXTYPECHANGED
                    case ActionsEnum.TAXTYPECHANGED:
                        GetUIValuesFromObject(ControlsEnum.TAXTYPECHANGED);
                        ShowVatbuyPopup();
                        break;
                    #endregion
                    #region CHANGETYPE
                    case ActionsEnum.CHANGETYPE:
                        txtBranchCode.Text = string.Empty;
                        SetBranchCodeVisibility();
                        ShowVatbuyPopup();

                        break;
                    #endregion
                    #region Edit VAT Item From Grid
                    case ActionsEnum.POPUPGRIDEDIT:
                        GridViewRow grwVatDetails = (GridViewRow)((ImageButton)(sender)).Parent.Parent;
                        SetUIEditViewVatPopup(grwVatDetails);
                        ShowVatbuyPopup();
                        break;
                    #endregion
                    #region VATTAXSAVE
                    case ActionsEnum.VATTAXSAVE:

                        poPaymentServiceClient = new POPaymentService();
                        poPaymentServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(poPaymentServiceClient);
                        //tempVATTaxDetails = null;
                        divVatErrorLabel.Visible = false;
                        if (grdVATTaxDetails.Rows.Count > 0)
                        {
                            if (TempVATTaxDetails != null && TempVATTaxDetails.Count > 0)
                            {

                                if (TempVATTaxDetails != null && TempVATTaxDetails.Count > 0)
                                {
                                    tempVATTaxDetails = VATTaxDetails = TempVATTaxDetails;
                                    VatBuyTax = Convert.ToDecimal(CommonFunctions.DoubleFormatRound(Convert.ToDouble(tempVATTaxDetails.Sum(aa => aa.WTH_TAX_AMT)), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits));

                                }
                                decimal.TryParse(txtTaxAmount.Text.Replace(",", ""), out vatTax);
                                if (vatTax != VatBuyTax && vatTax > 0)
                                {
                                    if ((hdfIscontYesVat.Value != "1"))
                                    {
                                        hdfAmntMissmatch.Value = GetLocalResourceObject("Err_VatAmntNotTallied").ToString();
                                        ShowVatbuyPopup();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowAlreadyPaid", "$(document).ready(function(){VatAmtMismatch();});", true);
                                        return;
                                    }
                                }
                                hdfIscontYesVat.Value = "0";
                                result = poPaymentServiceClient.SaveDirectVatTaxForPOPayment(tempVATTaxDetails);
                                if (result >= 0)
                                {
                                    litErrorMsg.Text = Resources.Messages.Msg_Save_Success;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), GetLocalResourceObject("Tax").ToString());
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                }

                            }
                        }
                        else
                        {
                            try
                            {
                                tempVATTaxDetails = VATTaxDetails = TempVATTaxDetails;
                            }
                            catch { }
                            hdfIscontYesVat.Value = "0";
                            result = poPaymentServiceClient.DeleteDirectVatTaxForPOPayment(CurrPK, (byte)WHTCategoryEnum.VATBUY);
                            if (result >= 0)
                            {
                                litErrorMsg.Text = Resources.Messages.Msg_Save_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), GetLocalResourceObject("Tax").ToString());
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                            }
                            //ShowVatbuyPopup();
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_VatTaxSave").ToString()) + "','" + Resources.Messages.Information + "');", true);
                        }

                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);

                        break;
                    #endregion

                    #region VENDORSELECTEDDTL
                    case ActionsEnum.VENDORSELECTEDDTL:
                        GetFieldValues(ControlsEnum.VENDORSELECTEDDTL);
                        SetFieldValues(ControlsEnum.VENDORSELECTEDDTL);
                        GetFieldValues(ControlsEnum.VENDORCONTACTYPE);
                        SetFieldValues(ControlsEnum.VENDORCONTACTYPE);
                        SetBranchCodeVisibility();
                        ShowVatbuyPopup();
                        break;
                    #endregion
                    #region VENDORTEXTCHANGED
                    case ActionsEnum.VENDORTEXTCHANGED:
                        hdfAddressType.Value = string.Empty;
                        hdfVendorPopup.Value = string.Empty;
                        txtBranchCode.Enabled = true;
                        vrfBranchCode.Enabled = true;
                        chkHeadOffice.Checked = false;
                        txtBranchCode.CssClass = "";
                        SetBranchCodeVisibility();
                        ShowVatbuyPopup();
                        break;
                    #endregion
                    #region VENDORCONTACTTEXTCHANGED
                    case ActionsEnum.VENDORCONTACTTEXTCHANGED:
                        //hdfAddressType.Value = string.Empty;                        
                        txtBranchCode.Enabled = true;
                        vrfBranchCode.Enabled = true;
                        chkHeadOffice.Checked = false;
                        txtBranchCode.CssClass = "";
                        SetBranchCodeVisibility();
                        ShowVatbuyPopup();
                        break;
                    #endregion
                    #region CHECKEDCHANGED
                    case ActionsEnum.CHECKEDCHANGED:
                        HeadofficeCheckedChanged();
                        ShowVatbuyPopup();
                        break;
                    #endregion

                    #region ADDITEM
                    case ActionsEnum.ADDITEM:
                        //validate Page
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_SavError;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else//valid
                        {
                            //tempFileInfoObj = new FileInfo(fupUpload.PostedFile.FileName);
                            //if (!IsValidExtension(tempFileInfoObj.Extension))
                            //{
                            //    // litErrorMsg.Text = Resources.ErrorMessages.Msg_Valid_File;
                            //    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                            //    //    + "','" + Resources.ErpRes.Information + "');", true);
                            //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(Resources.ErrorMessages.Msg_Valid_File) + "','" + Resources.ErpRes.Information + "');", true);

                            //}
                            //else
                            //{
                            if (CurrSlNo != 0)
                            {
                                if (fupUpload.HasFile || !string.IsNullOrEmpty(anchorFile.HRef))
                                {
                                    admDocAttachObj = DocAttachList.SingleOrDefault(itm => itm.DOC_SEQ_NO == CurrSlNo);
                                    if (admDocAttachObj != null)
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
                                            //admDocAttachObj.AttachmentFileName = attachmentFileName;
                                            //admDocAttachObj.FileExtension = tempFileInfoObj.Extension;
                                            admDocAttachObj.DOC_NAME = fupUpload.FileName;
                                            admDocAttachObj.DOC_TYPE = tempFileInfoObj.Extension;
                                            admDocAttachObj.DOC_CRTD_DT = DateTime.Now;
                                            admDocAttachObj.DOC_CRTD_BY = currentUser.PKUser;
                                            admDocAttachObj.DOC_MOD_DT = DateTime.Now;
                                            admDocAttachObj.DOC_MOD_BY = currentUser.PKUser;
                                            admDocAttachObj.DOC_BIZUNIT = currentUser.SBUID;
                                            admDocAttachObj.DOC_MODULE = (int)DocModuleEnum.FINANCE;
                                            admDocAttachObj.DOC_TASK = (int)DocTaskEnum.FINANCETASK;
                                            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower()))
                                            {
                                                admDocAttachObj.DOC_PATH = "~/Upload/" + attachmentFileName;
                                            }
                                            else
                                            {
                                                admDocAttachObj.DOC_PATH = System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower() + attachmentFileName;
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
                                    if (DocAttachList == null || DocAttachList.Count == 0)
                                    {
                                        DocAttachList = new List<ADM_DOC_ATTACH>();
                                        slno = 1;
                                    }
                                    else
                                    {
                                        slno = DocAttachList.Max(itm => itm.DOC_SEQ_NO);
                                        slno++;
                                    }
                                    if (FileDetailsList == null)
                                    {
                                        FileDetailsList = new List<FileDetails>();
                                    }

                                    admDocAttachObj = new ADM_DOC_ATTACH();
                                    admDocAttachObj.DOC_PK = 0;
                                    admDocAttachObj.DOC_SEQ_NO = (short)slno;
                                    tempFileInfoObj = new FileInfo(fupUpload.PostedFile.FileName);
                                    string attachmentFileFormat = tempFileInfoObj.Extension;
                                    string attachmentFileName = Guid.NewGuid().ToString() + attachmentFileFormat;

                                    //admDocAttachObj.AttachmentFileName = attachmentFileName;
                                    //admDocAttachObj.FileExtension = tempFileInfoObj.Extension;
                                    admDocAttachObj.DOC_NAME = fupUpload.FileName;
                                    admDocAttachObj.DOC_TYPE = tempFileInfoObj.Extension;
                                    admDocAttachObj.DOC_CRTD_DT = DateTime.Now;
                                    admDocAttachObj.DOC_CRTD_BY = currentUser.PKUser;
                                    admDocAttachObj.DOC_MOD_DT = DateTime.Now;
                                    admDocAttachObj.DOC_MOD_BY = currentUser.PKUser;
                                    admDocAttachObj.DOC_BIZUNIT = currentUser.SBUID;
                                    admDocAttachObj.DOC_MODULE = (int)DocModuleEnum.FINANCE;
                                    admDocAttachObj.DOC_TASK = (int)DocTaskEnum.FINANCETASK;
                                    if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower()))
                                    {
                                        admDocAttachObj.DOC_PATH = "~/Upload/" + attachmentFileName;
                                    }
                                    else
                                    {
                                        admDocAttachObj.DOC_PATH = System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower() + attachmentFileName;
                                    }

                                    // admDocAttachObj.SCD_FILE_PATH = "~/Upload/" + attachmentFileName;
                                    admDocAttachObj.DOC_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                                    FileDetailsList.Add(new FileDetails() { SlNo = slno, PoFile = HttpContext.Current.Request.Files[0] });
                                    DocAttachList.Add(admDocAttachObj);

                                }
                            }

                            SetFieldValues(ControlsEnum.UPLOADEDFILES);
                            ResetForm(ControlsEnum.ADDITEM);
                            //SetFieldValues(ControlsEnum.POINVOICELIST);
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ScrollDown();", true);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowUploadDocDetails", "$(document).ready(function(){ShowHideUploadDocDetails(1);});", true);

                            //}
                        }
                        break;
                    #endregion
                    #region REMOVEITEM
                    case ActionsEnum.REMOVEITEM:
                        if (DocAttachList != null && DocAttachList.Count > 0)
                        {
                            selectedItemPK = Convert.ToInt32(grdUploads.DataKeys[(((Button)sender).Parent.Parent as GridViewRow).RowIndex][0]);
                            if (selectedItemPK > 0)
                            {
                                DocAttachList = DocAttachList.Where(row => selectedItemPK != row.DOC_SEQ_NO).ToList();
                                //SetFieldValues(ControlsEnum.UPLOADEDFILES);
                                BindGrid(ControlsEnum.UPLOADEDFILES);
                                ResetForm(ControlsEnum.ADDITEM);
                            }
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowUploadDocDetails", "$(document).ready(function(){ShowHideUploadDocDetails(1);});", true);

                        break;
                    #endregion
                    #region EDITITEM
                    case ActionsEnum.EDITITEM:
                        if (DocAttachList != null && DocAttachList.Count > 0)
                        {
                            selectedItemPK = Convert.ToInt32(grdUploads.DataKeys[(((Button)sender).Parent.Parent as GridViewRow).RowIndex][0]);
                            if (selectedItemPK > 0)
                            {
                                admDocAttachObj = DocAttachList.SingleOrDefault(row => selectedItemPK == row.DOC_SEQ_NO);
                                GetUIValuesFromObject(ControlsEnum.SELECTEDDOC);
                            }
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowUploadDocDetails", "$(document).ready(function(){ShowHideUploadDocDetails(1);});", true);

                        break;
                    #endregion
                    #region Show Popup
                    case ActionsEnum.SHOWPOPUP:
                        string lnkInvoicePk = string.Empty;
                        LinkButton lnkInvoiceNoTemp = (LinkButton)sender; //Gets the linkbutton
                        HiddenField hdfInvCategory;
                        HiddenField hdfInvoiceType;
                        HiddenField hdfInvGroup;
                        GridViewRow grdrowTemp = (GridViewRow)lnkInvoiceNoTemp.NamingContainer; //Gets the gridview 
                        int index;
                        if (grdrowTemp != null)
                        {
                            index = grdrowTemp.RowIndex; //Gets the row index of selected linkbutton
                            GridViewRow grdrow = grdInvoiceList.Rows[index]; //Gets the row of selected index
                            LinkButton lnkInvoiceNo;
                            lnkInvoiceNo = (LinkButton)grdrow.FindControl("lnkInvoiceNo"); //Gets the exact linkbutton
                            lnkInvoicePk = lnkInvoiceNo.CommandArgument.ToString(); //Gets the command agrument of exact linkbutton
                            hdfInvCategory = (HiddenField)grdrow.FindControl("hdfCategory");
                            hdfInvoiceType = (HiddenField)grdrow.FindControl("hdfInvoiceType");
                            hdfInvGroup = (HiddenField)grdrow.FindControl("hdfGroup");
                            if (hdfInvGroup.Value == "4")
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + lnkInvoicePk.ToString() + "&APPTYPE=" + ApplicationType.ACI + "&APPSUBTYPE=0") + "');", true);
                            }
                            else if (hdfInvGroup.Value == "3")
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + lnkInvoicePk.ToString() + "&APPTYPE=" + ApplicationType.EI + "&APPSUBTYPE=") + "');", true);
                            }
                            else if (hdfInvCategory.Value == "2")
                            {
                                if (hdfInvoiceType.Value.ToString() == "2")
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + lnkInvoicePk.ToString() + "&APPTYPE=" + ApplicationType.PI + "&APPSUBTYPE=13") + "');", true);
                                else
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + lnkInvoicePk.ToString() + "&APPTYPE=" + ApplicationType.PI + "&APPSUBTYPE=12") + "');", true);
                            }
                            else if (hdfInvCategory.Value == "1")
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + lnkInvoicePk.ToString() + "&APPTYPE=" + ApplicationType.PI + "&APPSUBTYPE=") + "');", true);
                            }

                        }
                        break;
                    #endregion

                    #region ADJNINVOICEDETAIL
                    case ActionsEnum.ADJNINVOICEDETAIL:
                        divErrorLabelAdjn.Visible = false;
                        divErrorAdj.Visible = false;
                        divBaltoAll.Visible = false;

                        hdfPaymentMpgPK = (HiddenField)((((Button)sender).Parent).FindControl("hdfPaymentMpgPK"));
                        hdfInvoicePK = (HiddenField)((((Button)sender).Parent).FindControl("hdfInvoicePK"));
                        hdfIsApply = (HiddenField)((((Button)sender).Parent).FindControl("hdfIsApply"));
                        hdfBaltopay = (HiddenField)((((Button)sender).Parent).FindControl("hdfBaltopay"));
                        hdfBaltoAlloc.Value = string.IsNullOrEmpty(hdfBaltopay.Value) ? "0" : hdfBaltopay.Value;
                        InvoicePK = Convert.ToInt32(hdfInvoicePK.Value);
                        TrxIndex = ((GridViewRow)((sender as Control)).NamingContainer).RowIndex;
                        lblVndname.Text = lblCustomerTxt.ToolTip;
                        lblcurrencyname.Text = txtPaymentCurrency.Text;
                        if (hdfPaymentMpgPK != null && !string.IsNullOrEmpty(hdfPaymentMpgPK.Value) && !hdfPaymentMpgPK.Value.Equals("0"))
                        {
                            PaymentMpgPK = Convert.ToInt64(hdfPaymentMpgPK.Value);
                            //if (hdfIsApply.Value != "1")
                            //    GetFieldValues(ControlsEnum.PAYMENTADJNLIST);
                            //else
                            GetFieldValues(ControlsEnum.PAYMENTADJNDUMMYLIST);
                            if (FinPaymentVndAllocationList != null && FinPaymentVndAllocationList.Count > 0)
                            {
                                SetFieldValues(ControlsEnum.PAYMENTADJN);
                            }
                            else
                            {
                                GetFieldValues(ControlsEnum.PAYMENTADJNLIST);
                                GetFieldValues(ControlsEnum.PAYMENTADJN);
                                SetFieldValues(ControlsEnum.PAYMENTADJN);
                            }
                        }
                        else//new
                        {
                            PaymentMpgPK = Convert.ToInt64(hdfPaymentMpgPK.Value);
                            GetFieldValues(ControlsEnum.PAYMENTADJN);
                            SetFieldValues(ControlsEnum.PAYMENTADJN);
                            //Header
                            //GetFieldValues(ControlsEnum.INVOICEVNDHDR);
                            //GetUIValuesFromObject(ControlsEnum.PAYMENTSPLITLIST);
                            //isSplitChanged = false;
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divSoSplitUpAdjn]','" + GetLocalResourceObject("AdjAllocation").ToString() + "','800','300');", true);
                        break;
                    #endregion
                    #region ADJN SPLIT SAVE
                    case ActionsEnum.ADJNSPLITSAVE:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        else//valid
                        {
                            if (grdPaymentSplitAdjn.Rows.Count >= 1)
                            {


                                Label lblTotalAllocateAdjn = (Label)(grdPaymentSplitAdjn.FooterRow.FindControl("lblTotalAllocateAdjn"));
                                Label lblBalanceAdjn = (Label)(grdPaymentSplitAdjn.FooterRow.FindControl("lblBalanceAdjn"));
                                HiddenField hdfTotalAllocateAdjn = (HiddenField)(grdPaymentSplitAdjn.FooterRow.FindControl("hdfTotalAllocateAdjn"));
                                HiddenField hdfBalanceAdjn = (HiddenField)(grdPaymentSplitAdjn.FooterRow.FindControl("hdfBalanceAdjn"));
                                if (Convert.ToDecimal(hdfBalanceAdjn.Value) >= Convert.ToDecimal(hdfTotalAllocateAdjn.Value))
                                {
                                    if (Convert.ToDecimal(hdfBaltoAlloc.Value) >= Convert.ToDecimal(hdfTotalAllocateAdjn.Value))
                                    {
                                        if (TrxIndex >= 0)
                                        {
                                            //HiddenField hdfInitialBaltoPay = (HiddenField)grdInvoiceList.Rows[TrxIndex].FindControl("hdfInitialBaltoPay");
                                            HiddenField hdfInitialBaltoPay = (HiddenField)grdInvoiceList.Rows[TrxIndex].FindControl("hdfBaltopay");
                                            decimal baltopay = 0;
                                            decimal.TryParse(hdfInitialBaltoPay.Value, out baltopay);
                                            if (Convert.ToDecimal(hdfTotalAllocateAdjn.Value) > baltopay)
                                            {
                                                divErrorLabelAdjn.Visible = true;
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateAdjnFooter", "$(document).ready(function(){CalculateAdjnFooter();});", true);
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divSoSplitUpAdjn]','" + GetLocalResourceObject("AdjAllocation").ToString() + "','800','300');", true);

                                                return;
                                            }

                                        }
                                        if (lblTotalAllocateAdjn != null && !string.IsNullOrEmpty(lblTotalAllocateAdjn.Text))
                                        {
                                            divErrorLabel.Visible = false;
                                            FinPaymentVndAllocationList = new List<FIN_PAYMENT_VND_ALCN_DTL>();
                                            poPaymentServiceClient = new POPaymentService();
                                            poPaymentServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(poPaymentServiceClient);
                                            FinPaymentVndAllocationList = (List<FIN_PAYMENT_VND_ALCN_DTL>)SetUIValuesToObject(ControlsEnum.ADJNSPLITLIST);

                                            int rowID = 0;
                                            #region check any other trx have already taken the amount or not
                                            HiddenField hdfCrDrPK;
                                            HiddenField hdfReceiptAdjnPK;
                                            Label lblBaltoRec;

                                            TextBox txtAllocateAdjn;
                                            bool isCon = true;

                                            foreach (GridViewRow grdrow in grdPaymentSplitAdjn.Rows)//
                                            {
                                                hdfCrDrPK = (HiddenField)grdPaymentSplitAdjn.Rows[rowID].FindControl("hdfCrDrPK");
                                                hdfReceiptAdjnPK = (HiddenField)grdPaymentSplitAdjn.Rows[rowID].FindControl("hdfReceiptAdjnPK");
                                                lblBaltoRec = (Label)grdPaymentSplitAdjn.Rows[rowID].FindControl("lblBalanceAdjn");
                                                txtAllocateAdjn = (TextBox)grdPaymentSplitAdjn.Rows[rowID].FindControl("txtAllocateAdjn");
                                                if (hdfCrDrPK.Value == null)
                                                {
                                                    decimal AllocatedAmt = PaymentAdjnList.Where(sa => sa.PAD_ALCN_PAYMENT_TRX == Convert.ToInt32(hdfReceiptAdjnPK.Value) && sa.FIN_PAYMENT_VND_TRX_MPG.PVM_INVOICE_HDR != InvoicePK).Sum(a => a.PAD_AMOUNT);
                                                    if (!string.IsNullOrEmpty(txtAllocateAdjn.Text))
                                                    {
                                                        if (Convert.ToDecimal(txtAllocateAdjn.Text) > 0)
                                                            if ((Convert.ToDecimal(lblBaltoRec.Text.Replace(",", "")) - AllocatedAmt) < (string.IsNullOrEmpty(txtAllocateAdjn.Text) ? 0 : Convert.ToDecimal(txtAllocateAdjn.Text)))
                                                            {
                                                                isCon = false;
                                                            }
                                                    }
                                                }
                                                else
                                                {
                                                    decimal AllocatedAmt = PaymentAdjnList.Where(sa => sa.PAD_ALCN_CDH == Convert.ToInt32(hdfCrDrPK.Value) && sa.FIN_PAYMENT_VND_TRX_MPG.PVM_INVOICE_HDR != InvoicePK).Sum(a => a.PAD_AMOUNT);
                                                    if (!string.IsNullOrEmpty(txtAllocateAdjn.Text))
                                                    {
                                                        if (Convert.ToDecimal(txtAllocateAdjn.Text) > 0) if ((Convert.ToDecimal(lblBaltoRec.Text.Replace(",", "")) - AllocatedAmt) < (string.IsNullOrEmpty(txtAllocateAdjn.Text) ? 0 : Convert.ToDecimal(txtAllocateAdjn.Text)))
                                                            {
                                                                isCon = false;
                                                            }
                                                    }
                                                }
                                                rowID++;
                                            }
                                            #endregion
                                            if (isCon == true)
                                            {
                                                if (FinPaymentVndAllocationList != null && FinPaymentVndAllocationList.Count > 0)
                                                {
                                                    tempPaymentAdjnList = PaymentAdjnList;
                                                    tempPaymentAdjnList.Where(dtl => dtl.FIN_PAYMENT_VND_TRX_MPG.PVM_INVOICE_HDR == InvoicePK)
                                                        .ToList().ForEach(dtl => tempPaymentAdjnList.Remove(dtl));
                                                    FinPaymentVndAllocationList.ForEach(dtl =>
                                                    {
                                                        dtl.PAD_PAYMENT_TRX = PaymentMpgPK;

                                                        dtl.FIN_PAYMENT_VND_TRX_MPG = new FIN_PAYMENT_VND_TRX_MPG()
                                                        {
                                                            PVM_PK = PaymentMpgPK,
                                                            PVM_INVOICE_HDR = InvoicePK

                                                        };
                                                        tempPaymentAdjnList.Add(dtl);
                                                    });
                                                    PaymentAdjnList = tempPaymentAdjnList;

                                                    if (TrxIndex >= 0)
                                                    {
                                                        Label lblAdjAmount = (Label)grdInvoiceList.Rows[TrxIndex].FindControl("lblAdjAmount");
                                                        Label BalancetoPay = (Label)grdInvoiceList.Rows[TrxIndex].FindControl("lblBaltopay");
                                                        TextBox txtPayNow = (TextBox)grdInvoiceList.Rows[TrxIndex].FindControl("txtPayNow");
                                                        hdfBaltopay = (HiddenField)grdInvoiceList.Rows[TrxIndex].FindControl("hdfBaltopay");
                                                        hdfIsApply = (HiddenField)grdInvoiceList.Rows[TrxIndex].FindControl("hdfIsApply");
                                                        lblCrdrAlcnAmount = (Label)grdInvoiceList.Rows[TrxIndex].FindControl("lblCrdrAlcnAmount");



                                                        lblAdjAmount.Text = String.Format("{0:c}", Convert.ToDecimal(hdfTotalAllocateAdjn.Value));
                                                        hdfIsApply.Value = "1";
                                                        if (lblAdjAmount != null && !string.IsNullOrEmpty(lblAdjAmount.Text.Trim()))
                                                        {
                                                            AdjnNowAmount = Convert.ToDecimal(lblAdjAmount.Text.Trim());
                                                            decimal BalPay = Convert.ToDecimal(hdfBaltopay.Value.Replace(",", "")) - AdjnNowAmount;
                                                            BalancetoPay.Text = BalancetoPay.ToolTip = String.Format("{0:c}", (BalPay < 0 ? 0 : BalPay));
                                                            txtPayNow.Text = txtPayNow.ToolTip = Math.Round(BalPay < 0 ? 0 : BalPay, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();// AdjnNowAmount > 0 ?: txtPayNow.Text;
                                                        }


                                                        // Label lblAdjAmount = (Label)grdInvoiceList.Rows[TrxIndex].FindControl("lblAdjAmount");
                                                        // lblAdjAmount.Text = Math.Round(Convert.ToDecimal(lblTotalAllocateAdjn.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();


                                                        #region Resetting payment split after adj apply
                                                        if (InvoicePOSplitList != null && InvoicePOSplitList.Count > 0)
                                                        {

                                                            HiddenField hdfGrosAmount = (HiddenField)grdInvoiceList.Rows[TrxIndex].FindControl("hdfPayable");
                                                            Label lblOAmtSO = (Label)grdInvoiceList.Rows[TrxIndex].FindControl("lblOtherCharges");
                                                            Label lbladjAmt = (Label)grdInvoiceList.Rows[TrxIndex].FindControl("lblAdjAmount");
                                                            HiddenField hdftax = (HiddenField)grdInvoiceList.Rows[TrxIndex].FindControl("hdfTaxHdrDtlAmt");
                                                            TextBox txtotherCharges = (TextBox)grdInvoiceList.Rows[TrxIndex].FindControl("txtOtherCharges");

                                                            decimal GrosAmount = 0;
                                                            decimal OAmtSO = 0;
                                                            decimal tax = 0;
                                                            decimal taxpercentage = 1;
                                                            decimal tamt = 0;
                                                            decimal adjAmt = 0;
                                                            decimal otherCharges = 0;
                                                            decimal paynow = 0;
                                                            decimal.TryParse(txtPayNow.Text, out paynow);
                                                            decimal.TryParse(hdfGrosAmount.Value, out GrosAmount);
                                                            decimal.TryParse(lblOAmtSO.Text, out OAmtSO);
                                                            decimal.TryParse(lbladjAmt.Text, out adjAmt);
                                                            decimal.TryParse(hdftax.Value, out tax);
                                                            decimal.TryParse(txtotherCharges.Text, out otherCharges);

                                                            taxpercentage = (((GrosAmount - OAmtSO)) / ((((GrosAmount - OAmtSO) - tax) == 0) ? 1 : ((GrosAmount - OAmtSO) - tax))) - 1;
                                                            tamt = (paynow + adjAmt) - otherCharges;
                                                            tamt = tamt <= 0 ? 0 : tamt;
                                                            decimal basevalue = (tamt) / (((1 + taxpercentage) == 0) ? 1 : (1 + taxpercentage));
                                                            decimal ttaxamt = (tamt - basevalue);
                                                            ttaxamt = Math.Round(ttaxamt, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                                            decimal splitotaltamnt = 0;

                                                            tempInvoicePOSplitList = InvoicePOSplitList;
                                                            splitotaltamnt = tempInvoicePOSplitList.Where(dtl => dtl.FIN_PAYMENT_VND_TRX_MPG.PVM_INVOICE_HDR == InvoicePK).Sum(r => r.PPO_PAID_AMOUNT);
                                                            if (paynow != splitotaltamnt)
                                                            {
                                                                if (tempInvoicePOSplitList.Where(dtl => dtl.FIN_PAYMENT_VND_TRX_MPG.PVM_INVOICE_HDR == InvoicePK).Count() == 1)
                                                                {
                                                                    tempInvoicePOSplitList.Where(dtl => dtl.FIN_PAYMENT_VND_TRX_MPG.PVM_INVOICE_HDR == InvoicePK)
                                                                       .ToList().ForEach(dtl =>
                                                                       {
                                                                           dtl.PPO_PAID_AMOUNT = paynow;
                                                                           dtl.PPO_TAX_AMOUNT = ttaxamt;
                                                                           dtl.PPO_OTHER_AMOUNT = otherCharges;
                                                                       });
                                                                }
                                                                else
                                                                {
                                                                    tempInvoicePOSplitList.Where(dtl => dtl.FIN_PAYMENT_VND_TRX_MPG.PVM_INVOICE_HDR == InvoicePK)
                                                                       .ToList().ForEach(dtl =>
                                                                       {
                                                                           dtl.PPO_PAID_AMOUNT = 0;
                                                                           dtl.PPO_TAX_AMOUNT = 0;
                                                                           dtl.PPO_OTHER_AMOUNT = 0;
                                                                       });
                                                                }

                                                                InvoicePOSplitList = tempInvoicePOSplitList;
                                                            }
                                                        }
                                                        #endregion

                                                        #region Reset CR/DR Allocation
                                                        tempPaymentCrdrList = PaymentCrdrList;
                                                        if (tempPaymentCrdrList != null && tempPaymentCrdrList.Where(r => r.PNM_INVOICE_HDR == InvoicePK).ToList().Count > 0)
                                                        {
                                                            tempPaymentCrdrList.Where(r => r.PNM_INVOICE_HDR == InvoicePK).ToList().ForEach(dtl =>
                                                            {
                                                                dtl.PNM_ADJ_AMOUNT = 0;
                                                                dtl.PNM_PAID_AMOUNT = 0;
                                                            });
                                                            PaymentCrdrList = tempPaymentCrdrList;
                                                            decimal TotalCrdrAmnt = 0;
                                                            TotalCrdrAmnt = PaymentCrdrList.Where(r => r.PNM_INVOICE_HDR == InvoicePK).Sum(r => r.PNM_PAID_AMOUNT + r.PNM_ADJ_AMOUNT);
                                                            if (lblCrdrAlcnAmount != null)
                                                                lblCrdrAlcnAmount.Text = lblCrdrAlcnAmount.ToolTip = string.Format("{0:c}", TotalCrdrAmnt);
                                                        }
                                                        #endregion

                                                        #region Reset Payment PO Allocation
                                                        InvoiceDetails(InvoicePK);
                                                        PaymentSplitSave(false);
                                                        #endregion

                                                    }

                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                                                       "ClosePopup();", true);

                                                }

                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateAdjnFooter", "$(document).ready(function(){CalculateAdjnFooter();});", true);
                                                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculatelblAdjAmount", "$(document).ready(function(){CalculatelblAdjAmount();});", true);
                                            }
                                            else
                                            {
                                                divErrorAdj.Visible = true;
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divSoSplitUpAdjn]','" + GetLocalResourceObject("adj_Allocated").ToString() + "','800','300');", true);

                                            }
                                        }
                                        else
                                        {
                                            //divErrorLabel.Visible = true;
                                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divSoSplitUp]','" + GetLocalResourceObject("PaymentSplit").ToString() + "','800','300');", true);
                                        }
                                    }
                                    else
                                    {
                                        divBaltoAll.Visible = true;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateAdjnFooter", "$(document).ready(function(){CalculateAdjnFooter();});", true);
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divSoSplitUpAdjn]','" + GetLocalResourceObject("AdjAllocation").ToString() + "','800','300');", true);
                                    }
                                }
                                else
                                {
                                    divErrorLabelAdjn.Visible = true;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateAdjnFooter", "$(document).ready(function(){CalculateAdjnFooter();});", true);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divSoSplitUpAdjn]','" + GetLocalResourceObject("AdjAllocation").ToString() + "','800','300');", true);

                                }
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1", "ClosePopup();", true);
                            }

                        }
                        break;
                    #endregion
                    #region Show
                    case ActionsEnum.SHOW:
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPoSplitUp]','" + GetLocalResourceObject("PaymentSplit").ToString() + "','850','300');", true);

                        poPK = ((LinkButton)sender).CommandArgument;
                        //if (Convert.ToInt32(ddlType.SelectedValue) == 2)
                        //{
                        //RptSubType = 11;
                        //}
                        //else { RptSubType = 0; }
                        RptSubType = 0;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "ClosePopup();OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + poPK + "&APPTYPE=" + ApplicationType.PO + "&APPSUBTYPE=" + RptSubType) + "');", true);
                        break;
                    #endregion
                    #region ADD PAYMENT MODE
                    case ActionsEnum.ADDPAYMENTMODE:
                        bool IsSuccess = AddPaymentModes(true);
                        if (IsSuccess)
                        {
                            SetFieldValues(ControlsEnum.PAYMENTMODESGRID);
                            ResetForm(ControlsEnum.PAYMENTMODES);
                            PaymentModeRowIndex = -1;
                            hdfIsEdited.Value = "0";
                            EnableDisableExchangeRate();

                        }
                        break;
                    #endregion
                    #region Edit Payment mode Items From Grid
                    case ActionsEnum.EDITGRID:
                        GridViewRow grdPmntModes = (GridViewRow)((ImageButton)(sender)).Parent.Parent;
                        PaymentModeRowIndex = grdPmntModes.RowIndex;
                        SetUIEditViewPaymentModeDetails(PaymentModeRowIndex);
                        hdfIsEdited.Value = "1";
                        break;
                    #endregion

                    #region DELETEGRID
                    case ActionsEnum.DELETEGRID:
                        GridViewRow grdPymntModes = (GridViewRow)((ImageButton)(sender)).Parent.Parent;
                        if (PaymentModeDetailsList != null && PaymentModeDetailsList.Count > 0)
                        {
                            FIN_PAYMENT_VND_MODE_DTL PaymentModeDtl = null;
                            tempVATTax = null;
                            tempVATTaxDetails = TempVATTaxDetails;
                            PaymentModeDtl = PaymentModeDetailsList[grdPymntModes.RowIndex];
                            if (PaymentModeDtl != null)
                            {
                                PaymentModeDetailsList.Remove(PaymentModeDtl);
                                if (PaymentModeDetailsList == null || PaymentModeDetailsList.Count == 0)
                                {
                                    IsPaymentModeAdded = false;
                                    SetPymntModeHdrValidation(true);
                                }
                                else
                                {
                                    IsPaymentModeAdded = true;
                                }
                            }
                            //HiddenField hdfPymntMode = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfPymntMode") as HiddenField);
                            //HiddenField hdfPymntBankPk = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfPymntBankPk") as HiddenField);
                            //Label lblPymntInstrumentNo = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("lblPymntInstrumentNo") as Label);
                            //if (hdfPymntMode != null && hdfPymntBankPk != null && lblPymntInstrumentNo != null)
                            //{
                            //    byte PaymentMode = string.IsNullOrEmpty(hdfPymntMode.Value) ? (byte)0 : Convert.ToByte(hdfPymntMode.Value);
                            //    short? PaymentBankPk = string.IsNullOrEmpty(hdfPymntBankPk.Value) ? (short?)null : Convert.ToInt16(hdfPymntBankPk.Value);
                            //    string InstrNo = null;
                            //    if (!string.IsNullOrEmpty(lblPymntInstrumentNo.ToolTip))
                            //        InstrNo = HttpUtility.HtmlEncode(lblPymntInstrumentNo.ToolTip.Trim());
                            //    PaymentModeDtl = PaymentModeDetailsList.SingleOrDefault(r => r.PDM_MODE == PaymentMode && r.PDM_BANK == PaymentBankPk && r.PDM_INSTR_NO == InstrNo);
                            //    if (PaymentModeDtl != null)
                            //    {
                            //        PaymentModeDetailsList.Remove(PaymentModeDtl);
                            //        if (PaymentModeDetailsList == null || PaymentModeDetailsList.Count == 0)
                            //        {
                            //            IsPaymentModeAdded = false;
                            //            SetPymntModeHdrValidation(true);
                            //        }
                            //    }
                            //}
                            SetFieldValues(ControlsEnum.PAYMENTMODESGRID);
                            ResetForm(ControlsEnum.PAYMENTMODES);
                            EnableDisableExchangeRate();

                        }

                        break;
                    #endregion

                    #region CHANGE BANK CURRENCY
                    case ActionsEnum.CHANGEBANKCURRENCY:
                        GetFieldValues(ControlsEnum.BANKCURRENCYEXCHANGERATE);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateTotalBC", "$(document).ready(function(){CalculateTotalBC();});", true);
                        break;
                    #endregion

                    #region CR/DR ALLOCATION
                    case ActionsEnum.CRDRALLOCATION:
                        divCrdrErrorMsg.Visible = false;
                        FinPaymentVndCrdrMpgList = null;
                        hdfInvoicePK = (HiddenField)((((Button)sender).Parent).FindControl("hdfInvoicePK"));
                        Label lblTotalAmount = (Label)((((Button)sender).Parent).FindControl("lblTotalAmount"));
                        TextBox txtPayNowAmnt = (TextBox)((((Button)sender).Parent).FindControl("txtPayNow"));
                        //LinkButton lnkInvNo = (LinkButton)((((Button)sender).Parent).FindControl("lnkInvoiceNo"));
                        Label lblPaid = (Label)((((Button)sender).Parent).FindControl("lblPaid"));
                        CrdrRowIndex = ((GridViewRow)((sender as Control)).NamingContainer).RowIndex;
                        if (txtPayNowAmnt != null && !string.IsNullOrEmpty(txtPayNowAmnt.Text.Trim()))
                        {
                            PayNowAmount = Convert.ToDecimal(txtPayNowAmnt.Text.Trim());
                        }
                        if (hdfInvoicePK != null && !string.IsNullOrEmpty(hdfInvoicePK.Value) && !hdfInvoicePK.Value.Equals("0"))
                            InvoicePK = Convert.ToInt64(hdfInvoicePK.Value);
                        else
                            InvoicePK = 0;

                        if (InvoicePK > 0)
                        {
                            //lblInvSplitNo_CrdrAlcn.Text = lblInvSplitNo_CrdrAlcn.ToolTip = lnkInvNo.Text;
                            lblInvSplitAmount_CrdrAlcn.Text = lblInvSplitAmount_CrdrAlcn.ToolTip = lblTotalAmount.Text;
                            lblInvSplitReceived_CrdrAlcn.Text = lblInvSplitReceived_CrdrAlcn.ToolTip = lblPaid.Text;
                            lblInvSplitReceiveNow_CrdrAlcn.Text = lblInvSplitReceiveNow_CrdrAlcn.ToolTip = String.Format("{0:c}", PayNowAmount);
                            GetFieldValues(ControlsEnum.INVOICEVNDHDR);
                            GetUIValuesFromObject(ControlsEnum.PAYMENTSPLITLIST);
                        }
                        if (PaymentCrdrList != null && PaymentCrdrList.Count > 0)
                            FinPaymentVndCrdrMpgList = PaymentCrdrList.Where(dtl => dtl.PNM_INVOICE_HDR == InvoicePK).ToList();
                        SetFieldValues(ControlsEnum.CRDRALLOCATION);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateTotalCreditSplitFooter", "$(document).ready(function(){CalculateTotalCreditSplit();});", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divCrdrAllocation]','" + GetLocalResourceObject("CreditAllocation").ToString() + "','850','300');", true);
                        break;
                    #endregion

                    #region CR/DR ALLOCATION SAVE
                    case ActionsEnum.CRDRALLOCATIONSAVE:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        else//valid
                        {
                            if (grdCrdrAllocation.Rows.Count >= 1)
                            {
                                IsCreditNoteApplied = false;
                                divErrorLabel.Visible = false;
                                FinPaymentVndCrdrMpgList = new List<PaymentCrdrMpg>();
                                FinPaymentVndCrdrMpgList = (List<PaymentCrdrMpg>)SetUIValuesToObject(ControlsEnum.CRDRSPLITLIST);
                                if (FinPaymentVndCrdrMpgList != null)
                                {
                                    decimal CrAlcnAmount = 0;
                                    decimal TotalCrPayNow = 0;
                                    decimal TotalCrAdj = 0;
                                    tempPaymentCrdrList = PaymentCrdrList.DeepClone();
                                    FinPaymentVndCrdrMpgList.ForEach(dtl =>
                                    {
                                        PaymentCrdrMpg objPaymentCrdrMpg = tempPaymentCrdrList.SingleOrDefault(r => r.PNM_INVOICE_HDR == InvoicePK && r.PNM_CRDR_MPG == dtl.PNM_CRDR_MPG);
                                        objPaymentCrdrMpg.PNM_ADJ_AMOUNT = dtl.PNM_ADJ_AMOUNT;
                                        objPaymentCrdrMpg.PNM_PAID_AMOUNT = dtl.PNM_PAID_AMOUNT;
                                        CrAlcnAmount += (dtl.PNM_ADJ_AMOUNT + dtl.PNM_PAID_AMOUNT);
                                        TotalCrPayNow += dtl.PNM_PAID_AMOUNT;
                                        TotalCrAdj += dtl.PNM_ADJ_AMOUNT;
                                    });

                                    if (CrdrRowIndex >= 0)
                                    {
                                        decimal InvAdjAmnt = 0;
                                        decimal InvPayNow = 0;
                                        lblCrdrAlcnAmount = (Label)grdInvoiceList.Rows[CrdrRowIndex].FindControl("lblCrdrAlcnAmount");
                                        Label lblAdjAmount = (Label)grdInvoiceList.Rows[CrdrRowIndex].FindControl("lblAdjAmount");
                                        TextBox txtPayNow = (TextBox)grdInvoiceList.Rows[CrdrRowIndex].FindControl("txtPayNow");
                                        decimal.TryParse(txtPayNow.Text, out InvPayNow);
                                        if (lblAdjAmount != null)
                                            decimal.TryParse(lblAdjAmount.Text, out InvAdjAmnt);
                                        if (TotalCrPayNow > InvPayNow)
                                        {
                                            divCrdrErrorMsg.Visible = true;
                                            lblSplitErrorMessage_crdrAllocation.Text = GetLocalResourceObject("Err_ExcessCrPaynow").ToString();
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateTotalCreditSplitFooter", "$(document).ready(function(){CalculateTotalCreditSplit();});", true);
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divCrdrAllocation]','" + GetLocalResourceObject("CreditAllocation").ToString() + "','850','300');", true);

                                        }
                                        else if (TotalCrAdj > InvAdjAmnt)
                                        {
                                            divCrdrErrorMsg.Visible = true;
                                            lblSplitErrorMessage_crdrAllocation.Text = GetLocalResourceObject("Err_ExcessCrAdj").ToString();
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateTotalCreditSplitFooter", "$(document).ready(function(){CalculateTotalCreditSplit();});", true);
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divCrdrAllocation]','" + GetLocalResourceObject("CreditAllocation").ToString() + "','850','300');", true);

                                        }
                                        else
                                        {
                                            lblCrdrAlcnAmount.Text = lblCrdrAlcnAmount.ToolTip = string.Format("{0:c}", CrAlcnAmount);
                                            PaymentCrdrList = tempPaymentCrdrList;
                                            IsCreditNoteApplied = true;
                                            //GetUIValuesFromObject(ControlsEnum.SPLITPAYNOWFORMULIPO);
                                            InvoiceDetails(InvoicePK);
                                            PaymentSplitSave(false);
                                            IsCreditNoteApplied = false;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_CalculateTotal", "$(document).ready(function(){CalculateTotal();CalculateTotalSplit();});", true);

                                        }
                                    }
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1", "ClosePopup();", true);
                                }
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1", "ClosePopup();", true);
                            }
                        }
                        break;
                    #endregion

                    #region INVOICELIST
                    case ActionsEnum.INVOICELIST:
                        GetFieldValues(ControlsEnum.INVOICELIST);
                        SetFieldValues(ControlsEnum.INVOICELIST);
                        SetUIValuesToObject(ControlsEnum.INVOICELIST);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divNewInvList]','" + GetLocalResourceObject("InvList").ToString() + "','900','300');", true);
                        break;
                    #endregion

                    #region NEW INVOICE APPLY
                    case ActionsEnum.NEWINVOICEAPPLY:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        else//valid
                        {
                            if (grdNewInvList.Rows.Count > 0)
                            {
                                foreach (GridViewRow grvRow in grdNewInvList.Rows)
                                {
                                    CheckBox chkPIselect = (CheckBox)grvRow.FindControl("chkPIselect");
                                    if (chkPIselect.Checked)
                                    {
                                        HiddenField hdfInvoiceID = (HiddenField)grvRow.FindControl("hdfInvoiceID");
                                        long InvoiceId = string.IsNullOrEmpty(hdfInvoiceID.Value) ? 0 : Convert.ToInt64(hdfInvoiceID.Value);
                                        if (InvoiceId > 0)
                                        {
                                            if (selectedInvoiceList == null)
                                                selectedInvoiceList = new List<long>();
                                            selectedInvoiceList.Add(InvoiceId);
                                            NewInvoiceList.Add(InvoiceId);
                                        }
                                    }
                                }
                                if (CurrPK == 0)
                                {
                                    if (selectedInvoiceList != null)
                                    {
                                        //GetFieldValues(ControlsEnum.PAYMENTMPGLIST);
                                        //if (finInvoiceVndHdrList != null && finInvoiceVndHdrList.Count > 0)
                                        //{
                                        //    POGroup = (POInvoiceGroup)Enum.Parse(typeof(POInvoiceGroup), finInvoiceVndHdrList.First().IVH_GROUP.ToString());
                                        //    PICategory = (POInvoiceCategory)Enum.Parse(typeof(POInvoiceCategory), finInvoiceVndHdrList.First().IVH_CATEGORY.ToString());
                                        //}
                                        if (finInvoiceHdrList == null)
                                            finInvoiceHdrList = new List<FIN_INVOICE_VND_HDR>();
                                        foreach (long InvoiceVndPk in NewInvoiceList)
                                        {
                                            NewInvPk = InvoiceVndPk;
                                            GetFieldValues(ControlsEnum.INVOICEDETAILS);
                                            if (FinInvoiceVndObj != null)
                                            {
                                                finInvoiceHdrList.Add(FinInvoiceVndObj);
                                            }
                                        }
                                        //EditedInvoices = finInvoiceHdrList;
                                        FinInvoiceVndHdrSelectedList = finInvoiceHdrList;
                                        GetFieldValues(ControlsEnum.CRDRALCNFORNEWINV);
                                        if (PaymentCrdrListForNewInv != null && PaymentCrdrListForNewInv.Count > 0)
                                        {
                                            if (PaymentCrdrList == null)
                                                PaymentCrdrList = new List<PaymentCrdrMpg>();
                                            foreach (PaymentCrdrMpg pmntCrdrMpg in PaymentCrdrListForNewInv)
                                            {
                                                if (PaymentCrdrList.Where(r => r.PNM_INVOICE_HDR == pmntCrdrMpg.PNM_INVOICE_HDR).Count() == 0)
                                                    PaymentCrdrList.Add(pmntCrdrMpg);
                                            }
                                        }
                                        //GetFieldValues(ControlsEnum.CRDRALLOCATION);
                                        SetFieldValues(ControlsEnum.PAYMENTMPGLIST);

                                        GetFieldValues(ControlsEnum.VATPOPUPGRID);
                                        if (TempVATTaxDetails != null && TempVATTaxDetails.Count > 0)
                                        {
                                            tempVATTaxDetails = VATTaxDetails = TempVATTaxDetails;
                                        }

                                        GetFieldValues(ControlsEnum.EXCHANGERATEINBASECURRENCY);
                                        decimal exchangeRate = !string.IsNullOrEmpty(hdfExchangeCurrBC.Value) ? Convert.ToDecimal(hdfExchangeCurrBC.Value) : 0;

                                        decimal paidAmount = !string.IsNullOrEmpty(txtPaymentAmount.Text.Trim()) ? Convert.ToDecimal(txtPaymentAmount.Text.Trim()) : 0;
                                        txtTotalAmountBC.Text = GetFormattedCurrency(exchangeRate * paidAmount);
                                        GetFieldValues(ControlsEnum.BASECURRENCY);
                                        lblTotalAmountBC.Text = string.Format(lblTotalAmountBC.Text, hdfBaseCurrency.Value.Split('-')[0].Trim());
                                    }
                                }
                                else
                                {
                                    if (finPaymentTrxMpgList == null)
                                        finPaymentTrxMpgList = new List<FIN_PAYMENT_VND_TRX_MPG>();
                                    foreach (long InvoiceVndPk in NewInvoiceList)
                                    {
                                        NewInvPk = InvoiceVndPk;
                                        GetFieldValues(ControlsEnum.INVOICEDETAILS);
                                        if (FinInvoiceVndObj != null)
                                        {
                                            FIN_PAYMENT_VND_TRX_MPG objPaymntTrxMpg = new FIN_PAYMENT_VND_TRX_MPG();
                                            objPaymntTrxMpg.FIN_INVOICE_VND_HDR = FinInvoiceVndObj;
                                            objPaymntTrxMpg.PVM_INVOICE_HDR = FinInvoiceVndObj.IVH_PK;
                                            //objPaymntTrxMpg.PVM_PAYMENT_HDR = CurrPK;
                                            finPaymentTrxMpgList.Add(objPaymntTrxMpg);
                                        }
                                    }
                                    EditedPaymentDtls = finPaymentTrxMpgList;
                                    FinInvoiceVndHdrSelectedList = new List<FIN_INVOICE_VND_HDR>();
                                    finPaymentTrxMpgList.ForEach(dtl => FinInvoiceVndHdrSelectedList.Add(dtl.FIN_INVOICE_VND_HDR));
                                    GetFieldValues(ControlsEnum.CRDRALCNFORNEWINV);
                                    if (PaymentCrdrListForNewInv != null && PaymentCrdrListForNewInv.Count > 0)
                                    {
                                        if (PaymentCrdrList == null)
                                            PaymentCrdrList = new List<PaymentCrdrMpg>();
                                        foreach (PaymentCrdrMpg pmntCrdrMpg in PaymentCrdrListForNewInv)
                                        {
                                            if (PaymentCrdrList.Where(r => r.PNM_INVOICE_HDR == pmntCrdrMpg.PNM_INVOICE_HDR).Count() == 0)
                                                PaymentCrdrList.Add(pmntCrdrMpg);
                                        }
                                    }
                                    SetFieldValues(ControlsEnum.PAYMENTMPGLIST);
                                    GetUIValuesFromObject(ControlsEnum.SPLITPAYNOWFORMULIPO);

                                    GetFieldValues(ControlsEnum.VATPOPUPGRID);
                                    if (TempVATTaxDetails != null && TempVATTaxDetails.Count > 0)
                                    {
                                        tempVATTaxDetails = VATTaxDetails = TempVATTaxDetails;
                                    }
                                }
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1", "ClosePopup();", true);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1", "ClosePopup();", true);
                            }

                        }
                        break;
                    #endregion

                    #region Show Popup
                    case ActionsEnum.PRINTINVOICE:
                        hdfInvType = (HiddenField)((((LinkButton)sender).Parent).FindControl("hdfInvType"));
                        hdfinvPK = (HiddenField)((((LinkButton)sender).Parent).FindControl("hdfinvPK"));
                        hdfinvCategory = (HiddenField)((((LinkButton)sender).Parent).FindControl("hdfinvCategory"));
                        hdfinvCategoryType = (HiddenField)((((LinkButton)sender).Parent).FindControl("hdfinvCategoryType"));

                        if (Convert.ToUInt32(hdfinvCategory.Value) == Convert.ToUInt32(POInvoiceCategory.Invoice))
                        {
                            if ((Convert.ToUInt32(hdfInvType.Value) == Convert.ToUInt32(POInvoiceGroup.Goods)) || (Convert.ToUInt32(hdfInvType.Value) == Convert.ToUInt32(POInvoiceGroup.Services)))  //|| (hdfInvType.Value == "2")
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + hdfinvPK.Value + "&APPTYPE=" + ApplicationType.PI + "&APPSUBTYPE=") + "');", true);
                            if (Convert.ToUInt32(hdfInvType.Value) == Convert.ToUInt32(POInvoiceGroup.Expense))
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + hdfinvPK.Value + "&APPTYPE=" + ApplicationType.EI + "&APPSUBTYPE=") + "');", true);
                        }
                        else if (Convert.ToUInt32(hdfinvCategory.Value) == Convert.ToUInt32(POInvoiceCategory.Advanced))
                        {
                            if (hdfinvCategoryType.Value == "2")
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + hdfinvPK.Value + "&APPTYPE=" + ApplicationType.PI + "&APPSUBTYPE=13") + "');", true);
                            else
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + hdfinvPK.Value + "&APPTYPE=" + ApplicationType.PI + "&APPSUBTYPE=12") + "');", true);
                        }
                        break;
                        #endregion
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }

            finally
            {
                poPaymentServiceClient = null;
                poInvoiceServiceClient = null;

            }
        }

        /// <summary>
        /// To set SAVE and APPLY buttons visibility
        /// </summary>
        private void SetWhtButtons()
        {
            decimal whtHdrAmnt = 0;
            decimal whtSplitAmnt = 0;
            decimal.TryParse(txtWHTAmount.Text, out whtHdrAmnt);
            if (CurrPK > 0)
            {
                GetFieldValues(ControlsEnum.PAYMENTHDRENTRYBYPK);
                if (finPaymentVndHdrList != null)
                {
                    whtHdrAmnt = finPaymentVndHdrList[0].PVH_WHT_AMOUNT;
                }
            }
            if (TempWHTTaxDetails != null)
            {
                whtSplitAmnt = Convert.ToDecimal(CommonFunctions.DoubleFormatRound(Convert.ToDouble(TempWHTTaxDetails.Sum(aa => aa.WTH_TAX_AMT)), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits));
            }
            if (whtHdrAmnt != whtSplitAmnt)
            {
                btnApply.Visible = true;
                btnWhtSave.Visible = false;
            }
            else if ((whtHdrAmnt == whtSplitAmnt & CurrPK == 0) || (whtHdrAmnt == 0))
            {
                btnApply.Visible = true;
                btnWhtSave.Visible = false;
            }
            else
            {
                btnApply.Visible = false;
                btnWhtSave.Visible = true;
            }
        }

        private void EnableDisableExchangeRate(bool IsEnable = false)
        {
            int PymntCurr = string.IsNullOrEmpty(hdfPaymentCurrency.Value) ? currentUser.BaseCurrency : Convert.ToInt32(hdfPaymentCurrency.Value);
            if ((grdPaymentModes.Rows.Count == 0 || IsEnable) && PymntCurr != currentUser.BaseCurrency)
            {
                txtHdrExchangeRate.Enabled = true;
                txtHdrExchangeRate.CssClass = "Uiinput-amount numeric";
            }
            else
            {
                txtHdrExchangeRate.Enabled = false;
                txtHdrExchangeRate.CssClass = "Uiinput-amount numeric input-disabled";
            }
        }

        /// <summary>
        /// Function to set payment details by mode.
        /// </summary>
        /// <param name="mode"></param>
        private void SetPaymentModeDetails(int mode)
        {
            switch (mode)
            {
                case (int)PaymentModeEnum.CASH:
                    vrfBranch.Enabled = vrfBranchHdr.Enabled = false;
                    vrfAccountNo.Enabled = vrfAccountNoHdr.Enabled = false;
                    vrfInstrumentNo.Enabled = vrfInstrumentNoHdr.Enabled = false;
                    vrfInstrumentDate.Enabled = vrfInstrumentDateHdr.Enabled = false;
                    vrfFavourof.Enabled = vrfFavourofHdr.Enabled = false;
                    txtInstrumentNo.Enabled = false;
                    txtInstrumentDate.Enabled = false;
                    txtFavourof.Enabled = false;
                    txtInstrumentNo.CssClass = "Uiinput-amount input-disabled select-half";
                    txtInstrumentDate.CssClass = "input-small input-disabled";
                    txtFavourof.CssClass = "multiline-2line input-disabled";
                    txtPaymentBank.Enabled = true;
                    txtPaymentBank.CssClass = "input-half";
                    //Label5.Visible = false;
                    //txtBankCharge.Visible = false;
                    //chkBankCharge.Visible = false;
                    txtBankCharge.Enabled = false;
                    ddlBankChargeCurrency.Enabled = false;
                    chkBankCharge.Enabled = false;
                    trBankCharge.Visible = false;

                    break;
                case (int)PaymentModeEnum.OTHERS:
                    vrfBranch.Enabled = vrfBranchHdr.Enabled = false;
                    vrfAccountNo.Enabled = vrfAccountNoHdr.Enabled = false;
                    vrfInstrumentNo.Enabled = vrfInstrumentNoHdr.Enabled = false;
                    vrfInstrumentDate.Enabled = vrfInstrumentDateHdr.Enabled = false;
                    vrfFavourof.Enabled = vrfFavourofHdr.Enabled = false;
                    txtInstrumentNo.Enabled = false;
                    txtInstrumentDate.Enabled = false;
                    txtFavourof.Enabled = false;
                    txtInstrumentNo.CssClass = "Uiinput-amount input-disabled select-half";
                    txtInstrumentDate.CssClass = "input-small input-disabled";
                    txtFavourof.CssClass = "multiline-2line input-disabled";
                    //Label5.Visible = false;
                    //txtBankCharge.Visible = false;
                    //chkBankCharge.Visible = false;
                    txtBankCharge.Enabled = false;
                    ddlBankChargeCurrency.Enabled = false;
                    chkBankCharge.Enabled = false;
                    txtPaymentBank.Text = string.Empty;
                    txtPaymentBank.Enabled = false;
                    txtPaymentBank.CssClass = "input-half input-disabled";
                    hdfPaymentBank.Value = string.Empty;
                    trBankCharge.Visible = false;
                    break;
                default:
                    vrfBranch.Enabled = vrfBranchHdr.Enabled = true;
                    vrfAccountNo.Enabled = vrfAccountNoHdr.Enabled = true;
                    vrfInstrumentNo.Enabled = vrfInstrumentNoHdr.Enabled = true;
                    vrfInstrumentDate.Enabled = vrfInstrumentDateHdr.Enabled = true;
                    vrfFavourof.Enabled = vrfFavourofHdr.Enabled = true;
                    txtInstrumentNo.Enabled = true;
                    txtInstrumentDate.Enabled = true;
                    txtFavourof.Enabled = true;
                    txtInstrumentNo.CssClass = "Uiinput-amount select-half";
                    txtInstrumentDate.CssClass = "input-small";
                    txtFavourof.CssClass = "multiline-2line";
                    txtPaymentBank.Enabled = true;
                    txtPaymentBank.CssClass = "input-half";
                    //Label5.Visible = true;
                    //txtBankCharge.Visible = true;
                    //chkBankCharge.Visible = true;
                    txtBankCharge.Enabled = true;
                    ddlBankChargeCurrency.Enabled = true;
                    chkBankCharge.Enabled = true;
                    trBankCharge.Visible = true;
                    //txtFavourof.Text = HttpUtility.HtmlDecode(hdfFavourof.Value);                       
                    break;
            }
        }

        /// <summary>
        /// Function to add payment mode details to view state
        /// </summary>
        private bool AddPaymentModes(bool ShowMsg)
        {
            //decimal AmountBc = 0;
            //decimal.TryParse(txtTotalAmountBC.Text, out AmountBc);
            //if (AmountBc > 0)
            //{
            List<FIN_PAYMENT_VND_MODE_DTL> tempPaymentModeDtlList;
            if (PaymentModeDetailsList == null)
                PaymentModeDetailsList = new List<FIN_PAYMENT_VND_MODE_DTL>();

            tempPaymentModeDtlList = null;
            tempPaymentModeDtlList = PaymentModeDetailsList;

            if (PaymentModeRowIndex >= 0 && tempPaymentModeDtlList.Count > 0)
            {

                tempPaymentModeDtlList[PaymentModeRowIndex].PDM_MODE = Convert.ToByte(ddlMode.SelectedValue);
                tempPaymentModeDtlList[PaymentModeRowIndex].PDM_BANK = string.IsNullOrEmpty(hdfPaymentBank.Value) ? (short?)null : Convert.ToInt16(hdfPaymentBank.Value);
                if (Convert.ToInt32(ddlMode.SelectedValue) != (int)PaymentModeEnum.CASH && Convert.ToInt32(ddlMode.SelectedValue) != (int)PaymentModeEnum.OTHERS)
                {
                    tempPaymentModeDtlList[PaymentModeRowIndex].PDM_BANK_CHARGE = txtBankCharge.Text != string.Empty ? Convert.ToDecimal(txtBankCharge.Text) : 0;
                    tempPaymentModeDtlList[PaymentModeRowIndex].PDM_BANK_CHARGE_TYPE = chkBankCharge.Checked;
                    if (ddlBankChargeCurrency.Items.Count > 0)
                        tempPaymentModeDtlList[PaymentModeRowIndex].PDM_BANK_CHARGE_CURR = Convert.ToInt32(ddlBankChargeCurrency.SelectedValue);
                    else
                        tempPaymentModeDtlList[PaymentModeRowIndex].PDM_BANK_CHARGE_CURR = null;
                }
                else
                {
                    tempPaymentModeDtlList[PaymentModeRowIndex].PDM_BANK_CHARGE = 0;
                    tempPaymentModeDtlList[PaymentModeRowIndex].PDM_BANK_CHARGE_TYPE = false;
                    tempPaymentModeDtlList[PaymentModeRowIndex].PDM_BANK_CHARGE_CURR = null;
                }
                if (Convert.ToInt32(ddlMode.SelectedValue) == (int)PaymentModeEnum.CHEQUE)
                    tempPaymentModeDtlList[PaymentModeRowIndex].PDM_PDC = chkPDC.Checked == true ? (byte)1 : (byte)0;
                else
                    tempPaymentModeDtlList[PaymentModeRowIndex].PDM_PDC = 0;

                tempPaymentModeDtlList[PaymentModeRowIndex].PDM_BRANCH = HttpUtility.HtmlEncode(txtBranch.Text.Trim());
                tempPaymentModeDtlList[PaymentModeRowIndex].PDM_INSTR_NO = HttpUtility.HtmlEncode(txtInstrumentNo.Text.Trim());
                tempPaymentModeDtlList[PaymentModeRowIndex].PDM_INSTR_DATE = String.IsNullOrEmpty(txtInstrumentDate.Text.Trim()) ? (DateTime?)null : Convert.ToDateTime(txtInstrumentDate.Text.Trim());
                tempPaymentModeDtlList[PaymentModeRowIndex].PDM_INSTR_FAVOUR = HttpUtility.HtmlEncode(txtFavourof.Text.Trim());

                tempPaymentModeDtlList[PaymentModeRowIndex].PDM_EXCHG_RATE = Convert.ToDouble(txtHdrExchangeRate.Text.Trim());
                tempPaymentModeDtlList[PaymentModeRowIndex].PDM_PAID_AMOUNT = Convert.ToDecimal(txtPaymentAmount.Text.Replace(",", "").Trim());
                tempPaymentModeDtlList[PaymentModeRowIndex].PDM_PAID_AMOUNT_BC = Convert.ToDecimal(txtTotalAmountBC.Text.Replace(",", "").Trim());
                tempPaymentModeDtlList[PaymentModeRowIndex].PDM_ACCOUNT = string.IsNullOrEmpty(hdfBankAccount.Value) ? 1 : Convert.ToInt32(hdfBankAccount.Value);
                PaymentModeDetailsList = tempPaymentModeDtlList;
                //PaymentModeRowIndex = -1;
                IsPaymentModeAdded = true;
                //SetFieldValues(ControlsEnum.PAYMENTMODESGRID);
                //ResetForm(ControlsEnum.PAYMENTMODES);
                SetPymntModeHdrValidation(false);
                return true;
            }
            else
            {
                short? BankId = string.IsNullOrEmpty(hdfPaymentBank.Value) ? (short?)null : Convert.ToInt16(hdfPaymentBank.Value);
                List<FIN_PAYMENT_VND_MODE_DTL> PymtList = tempPaymentModeDtlList.Where(r => r.PDM_MODE == Convert.ToByte(ddlMode.SelectedValue) && r.PDM_BANK == BankId && r.PDM_INSTR_NO == HttpUtility.HtmlEncode(txtInstrumentNo.Text.Trim())).ToList();
                if (PymtList == null || PymtList.Count == 0)
                {
                    FIN_PAYMENT_VND_MODE_DTL PaymentModeDtl = new FIN_PAYMENT_VND_MODE_DTL();
                    PaymentModeDtl.PDM_MODE = Convert.ToByte(ddlMode.SelectedValue);
                    PaymentModeDtl.PDM_BANK = string.IsNullOrEmpty(hdfPaymentBank.Value) ? (short?)null : Convert.ToInt16(hdfPaymentBank.Value);
                    if (Convert.ToInt32(ddlMode.SelectedValue) != (int)PaymentModeEnum.CASH && Convert.ToInt32(ddlMode.SelectedValue) != (int)PaymentModeEnum.OTHERS)
                    {
                        PaymentModeDtl.PDM_BANK_CHARGE = txtBankCharge.Text != string.Empty ? Convert.ToDecimal(txtBankCharge.Text) : 0;
                        PaymentModeDtl.PDM_BANK_CHARGE_TYPE = chkBankCharge.Checked;
                        if (ddlBankChargeCurrency.Items.Count > 0)
                            PaymentModeDtl.PDM_BANK_CHARGE_CURR = Convert.ToInt32(ddlBankChargeCurrency.SelectedValue);
                        else
                            PaymentModeDtl.PDM_BANK_CHARGE_CURR = null;
                    }
                    else
                    {
                        PaymentModeDtl.PDM_BANK_CHARGE = 0;
                        PaymentModeDtl.PDM_BANK_CHARGE_TYPE = false;
                        PaymentModeDtl.PDM_BANK_CHARGE_CURR = null;
                    }
                    if (Convert.ToInt32(ddlMode.SelectedValue) == (int)PaymentModeEnum.CHEQUE)
                        PaymentModeDtl.PDM_PDC = chkPDC.Checked == true ? (byte)1 : (byte)0;
                    else
                        PaymentModeDtl.PDM_PDC = 0;

                    PaymentModeDtl.PDM_BRANCH = HttpUtility.HtmlEncode(txtBranch.Text.Trim());
                    PaymentModeDtl.PDM_INSTR_NO = HttpUtility.HtmlEncode(txtInstrumentNo.Text.Trim());
                    PaymentModeDtl.PDM_INSTR_DATE = String.IsNullOrEmpty(txtInstrumentDate.Text.Trim()) ? (DateTime?)null : Convert.ToDateTime(txtInstrumentDate.Text.Trim());
                    PaymentModeDtl.PDM_INSTR_FAVOUR = HttpUtility.HtmlEncode(txtFavourof.Text.Trim());

                    PaymentModeDtl.PDM_EXCHG_RATE = Convert.ToDouble(txtHdrExchangeRate.Text.Trim());
                    PaymentModeDtl.PDM_PAID_AMOUNT = Convert.ToDecimal(txtPaymentAmount.Text.Replace(",", "").Trim());
                    PaymentModeDtl.PDM_PAID_AMOUNT_BC = Convert.ToDecimal(txtTotalAmountBC.Text.Replace(",", "").Trim());
                    PaymentModeDtl.PDM_ACCOUNT = string.IsNullOrEmpty(hdfBankAccount.Value) ? 1 : Convert.ToInt32(hdfBankAccount.Value);
                    tempPaymentModeDtlList.Add(PaymentModeDtl);
                    PaymentModeDetailsList = tempPaymentModeDtlList;
                    //PaymentModeRowIndex = -1;
                    IsPaymentModeAdded = true;
                    //SetFieldValues(ControlsEnum.PAYMENTMODESGRID);
                    //ResetForm(ControlsEnum.PAYMENTMODES);
                    SetPymntModeHdrValidation(false);
                    return true;
                }
                else
                {
                    if (ShowMsg)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_PaymentMode_Already_Add").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }


            }
            //}
            //else
            //{
            //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_AmountBc").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
            //    return false;
            //}
        }

        private void SetPymntModeHdrValidation(bool IsValid)
        {

            if (IsValid)
            {
                int mode = Convert.ToInt32(ddlMode.SelectedValue);
                switch (mode)
                {
                    case (int)PaymentModeEnum.CASH:
                        vrfBranchHdr.Enabled = false;
                        vrfAccountNoHdr.Enabled = false;
                        vrfInstrumentNoHdr.Enabled = false;
                        vrfInstrumentDateHdr.Enabled = false;
                        vrfFavourofHdr.Enabled = false;
                        break;
                    case (int)PaymentModeEnum.OTHERS:
                        vrfBranchHdr.Enabled = false;
                        vrfAccountNoHdr.Enabled = false;
                        vrfInstrumentNoHdr.Enabled = false;
                        vrfInstrumentDateHdr.Enabled = false;
                        vrfFavourofHdr.Enabled = false;
                        break;
                    default:
                        vrfBranchHdr.Enabled = true;
                        vrfAccountNoHdr.Enabled = true;
                        vrfInstrumentNoHdr.Enabled = true;
                        vrfInstrumentDateHdr.Enabled = true;
                        vrfFavourofHdr.Enabled = true;
                        break;
                }
            }
            else
            {
                vrfModeHdr.Enabled = IsValid;
                vrfBranchHdr.Enabled = IsValid;
                vrfInstrumentNoHdr.Enabled = IsValid;
                vrfBankNameHdr.Enabled = IsValid;
                vrfAccountNoHdr.Enabled = IsValid;
                vrfInstrumentDateHdr.Enabled = IsValid;
                //vreExchangeRateHdr.Enabled = IsValid;
                //vrfExchangeRateHdr.Enabled = IsValid;
                vamBankChargeHdr.Enabled = IsValid;
                csvBankChargeHdr.Enabled = IsValid;
                vrfTotalAmountBCHdr.Enabled = IsValid;
                vrfFavourofHdr.Enabled = IsValid;
            }
        }

        private void PaymentSplitSave(bool ShowMsg)
        {
            POPaymentService poPaymentServiceClient;
            poPaymentServiceClient = null;
            List<FIN_PAYMENT_VND_PO_MPG> tempInvoicePOSplitList;

            if (!IsValid)
            {
                litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
            }
            else//valid
            {
                lblSplitErrorMessage.Text = GetLocalResourceObject("error_allocation").ToString();
                if (grdPaymentSplit.Rows.Count >= 1)
                {
                    HiddenField lblTotalPayNowFooterSplit = (HiddenField)(grdPaymentSplit.FooterRow.FindControl("hdfTotalPayNowFooterSplit"));
                    HiddenField hdfTotalTaxFooterSplit1 = (HiddenField)(grdPaymentSplit.FooterRow.FindControl("hdfTotalTaxFooterSplit1"));
                    decimal TotalPayNowSplit = 0;
                    foreach (GridViewRow grvRow in grdPaymentSplit.Rows)
                    {
                        TextBox txtPayNowSplit = (TextBox)grvRow.FindControl("txtPayNowSplit");
                        decimal PaynowSplit = 0;
                        decimal.TryParse(txtPayNowSplit.Text, out PaynowSplit);
                        TotalPayNowSplit += PaynowSplit;
                    }
                    if (lblTotalPayNowFooterSplit != null && !string.IsNullOrEmpty(lblTotalPayNowFooterSplit.Value) && !string.IsNullOrEmpty(lblInvSplitReceiveNow.Text))
                    {
                        GetFieldValues(ControlsEnum.PAYMENTTOLERANCE);
                        Label TotalPayNowFooterSplit = (Label)(grdPaymentSplit.FooterRow.FindControl("lblTotalPayNowFooterSplit"));
                        //decimal FooterSplitAmt = Convert.ToDecimal(lblTotalPayNowFooterSplit.Value);
                        //TotalPayNowFooterSplit.Text = Math.Round(FooterSplitAmt, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                        TotalPayNowFooterSplit.Text = Math.Round(TotalPayNowSplit, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                        decimal PayNowAmount = Convert.ToDecimal(lblInvSplitReceiveNow.Text.Trim().Replace(",", ""));
                        decimal PayNowWithVariation = 0;
                        decimal Variation = (PayNowAmount * PaymentTollerence);
                        decimal CrAllocatedAmnt = 0;
                        PayNowWithVariation = PayNowAmount + Variation + AdjustmentAmount;
                        if (PaymentCrdrList != null && PaymentCrdrList.Count > 0)
                        {
                            CrAllocatedAmnt = PaymentCrdrList.Where(r => r.PNM_INVOICE_HDR == InvoicePK).Sum(sm => sm.PNM_PAID_AMOUNT + sm.PNM_ADJ_AMOUNT);
                            PayNowWithVariation = (PayNowWithVariation - CrAllocatedAmnt) < 0 ? 0 : (PayNowWithVariation - CrAllocatedAmnt);
                        }

                        //if (Convert.ToDecimal(TotalPayNowFooterSplit.Text) == Convert.ToDecimal(lblInvSplitReceiveNow.Text.Trim().Replace(",", "")))
                        //{
                        if (Convert.ToDecimal(TotalPayNowFooterSplit.Text) <= PayNowWithVariation)
                        {
                            //if (!string.IsNullOrEmpty(hdfTotalTaxFooterSplit1.Value))
                            //{
                            //    if (Tax != Convert.ToDecimal(hdfTotalTaxFooterSplit1.Value))
                            //    {
                            //        divErrorLabel.Visible = true;
                            //        lblSplitErrorMessage.Text = GetLocalResourceObject("Err_TaxSplit").ToString();// +" " + Tax.ToString();
                            //        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('Total split amount should be less than or equal to pay now amount','" + Resources.Messages.Information + "');", true);
                            //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPoSplitUp]','" + GetLocalResourceObject("PaymentSplit").ToString() + "','850','300');", true);
                            //        return;
                            //    }
                            //}

                            divErrorLabel.Visible = false;
                            finPaymentVndPoMpgList = new List<FIN_PAYMENT_VND_PO_MPG>();
                            poPaymentServiceClient = new POPaymentService();
                            poPaymentServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(poPaymentServiceClient);
                            finPaymentVndPoMpgList = (List<FIN_PAYMENT_VND_PO_MPG>)SetUIValuesToObject(ControlsEnum.PAYMENTSPLITLIST);
                            if (finPaymentVndPoMpgList != null)
                            {
                                tempInvoicePOSplitList = InvoicePOSplitList;
                                tempInvoicePOSplitList.Where(dtl => dtl.FIN_PAYMENT_VND_TRX_MPG.PVM_INVOICE_HDR == InvoicePK)
                                    .ToList().ForEach(dtl => tempInvoicePOSplitList.Remove(dtl));
                                finPaymentVndPoMpgList.ForEach(dtl =>
                                {
                                    dtl.FIN_PAYMENT_VND_TRX_MPG = new FIN_PAYMENT_VND_TRX_MPG()
                                    {
                                        PVM_INVOICE_HDR = InvoicePK
                                    };
                                    tempInvoicePOSplitList.Add(dtl);
                                });
                                InvoicePOSplitList = tempInvoicePOSplitList;

                                if (AppliedInvPkList == null)
                                    AppliedInvPkList = new List<long>();
                                if (!AppliedInvPkList.Contains(InvoicePK))
                                {
                                    AppliedInvPkList.Add(InvoicePK);  // To keep applied invoice Pks
                                }

                                //result = poPaymentServiceClient.SavePaymentSplit(finPaymentVndPoMpgList);
                                //if (result >= 0)
                                //{
                                //    litErrorMsg.Text = Resources.Messages.Msg_Save_Success;
                                //    litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), GetLocalResourceObject("Payment_allocation").ToString());
                                //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                poPaymentServiceClient = null;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                                    "ClosePopup();", true);
                                //}
                            }
                        }
                        else
                        {
                            if (ShowMsg)
                            {
                                divErrorLabel.Visible = true;
                                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('Total split amount should be less than or equal to pay now amount','" + Resources.Messages.Information + "');", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPoSplitUp]','" + GetLocalResourceObject("PaymentSplit").ToString() + "','850','300');", true);
                            }
                        }

                    }
                    else
                    {
                        if (ShowMsg)
                        {
                            divErrorLabel.Visible = true;
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('Total split amount should be less than or equal to pay now amount','" + Resources.Messages.Information + "');", true);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPoSplitUp]','" + GetLocalResourceObject("PaymentSplit").ToString() + "','850','300');", true);
                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                                        "ClosePopup();", true);
                }

            }
        }

        private void InvoiceDetails(long InvPK)
        {
            InvoicePK = InvPK;
            List<FIN_PAYMENT_VND_PO_MPG> tempInvoicePOSplitList = new List<FIN_PAYMENT_VND_PO_MPG>();
            decimal InvTotalOtherAmnt = 0;
            decimal AdjAmnt = 0;
            AdjustmentAmount = 0;
            HiddenField hdfInvoicePK;
            Tax = 0;
            PayNow = 0;
            InvOtherCharge = 0;

            divErrorLabel.Visible = false;
            GridViewRow GrdInvRow = null;

            foreach (GridViewRow grdRow in grdInvoiceList.Rows)
            {
                hdfInvoicePK = (HiddenField)grdRow.FindControl("hdfInvoicePK");
                if (Convert.ToInt64(hdfInvoicePK.Value) == InvoicePK)
                {
                    GrdInvRow = grdRow;
                    break;
                }
            }

            HiddenField hdfPaymentPK = (HiddenField)GrdInvRow.FindControl("hdfPaymentMpgPK");
            TextBox txtPayNow = (TextBox)GrdInvRow.FindControl("txtPayNow");
            HiddenField hdfTotalTax = (HiddenField)GrdInvRow.FindControl("hdfTotalTax");
            TextBox txtOtherCharges = (TextBox)GrdInvRow.FindControl("txtOtherCharges");
            Label lblOtherCharges = (Label)GrdInvRow.FindControl("lblOtherCharges");
            Label lblAdjAmount = (Label)GrdInvRow.FindControl("lblAdjAmount");
            hdfTaxPer.Value = "0";
            hdfOtherPer.Value = "0";

            if (lblOtherCharges != null && !string.IsNullOrEmpty(lblOtherCharges.Text))
            {
                decimal.TryParse(lblOtherCharges.Text.Replace(",", ""), out InvTotalOtherAmnt);
                hdfInvTotalOtherCharge.Value = InvTotalOtherAmnt.ToString();
            }

            if (hdfTotalTax != null && !string.IsNullOrEmpty(hdfTotalTax.Value.Trim()) && txtPayNow != null && !string.IsNullOrEmpty(txtPayNow.Text.Trim()))
            {
                if (Convert.ToDecimal(txtPayNow.Text.Trim()) > 0)
                {
                    hdfTaxPer.Value = (Convert.ToDecimal(hdfTotalTax.Value.Trim()) / Convert.ToDecimal(txtPayNow.Text.Trim())).ToString();
                    Tax = Convert.ToDecimal(Convert.ToDecimal(hdfTotalTax.Value.Trim()));
                    hdfTax.Value = Tax.ToString();
                }
            }
            if (txtOtherCharges != null && !string.IsNullOrEmpty(txtOtherCharges.Text.Trim()) && txtPayNow != null && !string.IsNullOrEmpty(txtPayNow.Text.Trim()))
            {
                InvOtherCharge = Convert.ToDecimal(txtOtherCharges.Text.Replace(",", "").Trim());
                if (Convert.ToDecimal(txtPayNow.Text.Trim()) > 0)
                {
                    hdfOtherPer.Value = (Convert.ToDecimal(txtOtherCharges.Text.Trim()) / Convert.ToDecimal(txtPayNow.Text.Trim())).ToString();
                }
            }
            hdfInvOtherCharge.Value = InvOtherCharge.ToString();
            if (txtPayNow != null && !string.IsNullOrEmpty(txtPayNow.Text.Trim()))
            {
                PayNowAmount = Convert.ToDecimal(txtPayNow.Text.Trim());
                PayNow = PayNowAmount;
            }
            if (lblAdjAmount != null && !string.IsNullOrEmpty(lblAdjAmount.Text.Trim()))
            {
                decimal.TryParse(lblAdjAmount.Text.Replace(",", ""), out AdjAmnt);
                AdjustmentAmount = AdjAmnt;
                PayNow += AdjAmnt;
            }
            //hdfInvoicePK = (HiddenField)((((Button)sender).Parent).FindControl("hdfInvoicePK"));
            //if (hdfInvoicePK != null && !string.IsNullOrEmpty(hdfInvoicePK.Value) && !hdfInvoicePK.Value.Equals("0"))
            //{
            //    InvoicePK = Convert.ToInt64(hdfInvoicePK.Value);
            //}
            //else
            //    InvoicePK = 0;
            tempInvoicePOSplitList = InvoicePOSplitList;
            tempFinPaymentVndPoMpgList = tempInvoicePOSplitList.Where(dtl => dtl.FIN_PAYMENT_VND_TRX_MPG.PVM_INVOICE_HDR == InvoicePK).ToList();

            if (hdfPaymentPK != null && !string.IsNullOrEmpty(hdfPaymentPK.Value) && !hdfPaymentPK.Value.Equals("0"))
            {
                PaymentMpgPK = Convert.ToInt64(hdfPaymentPK.Value);
                GetFieldValues(ControlsEnum.PAYMENTSPLITLIST);
                if (tempFinPaymentVndPoMpgList == null || tempFinPaymentVndPoMpgList.Count == 0)
                {
                    if (finPaymentVndPoMpgList != null && finPaymentVndPoMpgList.Count > 0)//Edit
                    {
                        tempInvoicePOSplitList.Where(dtl => dtl.FIN_PAYMENT_VND_TRX_MPG.PVM_INVOICE_HDR == InvoicePK)
                                        .ToList().ForEach(dtl => tempInvoicePOSplitList.Remove(dtl));
                        finPaymentVndPoMpgList.ForEach(dtl =>
                        {
                            dtl.FIN_PAYMENT_VND_TRX_MPG = new FIN_PAYMENT_VND_TRX_MPG()
                            {
                                PVM_INVOICE_HDR = InvoicePK
                            };
                            tempInvoicePOSplitList.Add(dtl);
                        });
                        InvoicePOSplitList = tempInvoicePOSplitList;
                        tempFinPaymentVndPoMpgList = tempInvoicePOSplitList.Where(dtl => dtl.FIN_PAYMENT_VND_TRX_MPG.PVM_INVOICE_HDR == InvoicePK).ToList();
                    }
                    else if (InvoicePK > 0)
                    {
                        GetFieldValues(ControlsEnum.INVOICEVNDMPGLIST);
                    }
                }
                else
                {
                    tempFinPaymentVndPoMpgList = tempInvoicePOSplitList;
                }
                if (InvoicePK > 0)
                {
                    GetFieldValues(ControlsEnum.INVOICEVNDHDR);
                    GetUIValuesFromObject(ControlsEnum.PAYMENTSPLITLIST);
                    isSplitChanged = false;
                    SetFieldValues(ControlsEnum.PAYMENTSPLITLIST);

                    GetUIValuesFromObject(ControlsEnum.SPLITPAYNOWFORMULIPO);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_CalculateTotal", "$(document).ready(function(){CalculateTotal();CalculateTotalSplit();});", true);

                }

                //if (finPaymentVndPoMpgList != null && finPaymentVndPoMpgList.Count > 0)//Edit
                //{
                //    tempInvoicePOSplitList.Where(dtl => dtl.FIN_PAYMENT_VND_TRX_MPG.PVM_INVOICE_HDR == InvoicePK)
                //                    .ToList().ForEach(dtl => tempInvoicePOSplitList.Remove(dtl));
                //    finPaymentVndPoMpgList.ForEach(dtl =>
                //    {
                //        dtl.FIN_PAYMENT_VND_TRX_MPG = new FIN_PAYMENT_VND_TRX_MPG()
                //        {
                //            PVM_INVOICE_HDR = InvoicePK
                //        };
                //        tempInvoicePOSplitList.Add(dtl);
                //    });
                //    InvoicePOSplitList = tempInvoicePOSplitList;
                //    if (InvoicePK > 0)
                //    {
                //        GetFieldValues(ControlsEnum.INVOICEVNDHDR);
                //        GetUIValuesFromObject(ControlsEnum.PAYMENTSPLITLIST);
                //        isSplitChanged = false;
                //        SetFieldValues(ControlsEnum.PAYMENTSPLITLIST);
                //    }
                //}
                //else//New
                //{
                //    if (InvoicePK > 0)
                //    {
                //        GetFieldValues(ControlsEnum.INVOICEVNDMPGLIST);
                //        if (FinInvoiceVndTrxMpgList != null && FinInvoiceVndTrxMpgList.Count > 0)
                //        {
                //            GetFieldValues(ControlsEnum.INVOICEVNDHDR);
                //            GetUIValuesFromObject(ControlsEnum.PAYMENTSPLITLIST);
                //            isSplitChanged = false;
                //            SetFieldValues(ControlsEnum.PAYMENTSPLITLIST);
                //        }
                //    }
                //}
            }
            else if (InvoicePK > 0)//New
            {
                GetFieldValues(ControlsEnum.INVOICEVNDMPGLIST);
                GetFieldValues(ControlsEnum.INVOICEVNDHDR);
                GetUIValuesFromObject(ControlsEnum.PAYMENTSPLITLIST);
                isSplitChanged = false;
                SetFieldValues(ControlsEnum.PAYMENTSPLITLIST);
                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_CalculateTotal", "$(document).ready(function(){CalculatePymntSplitBalFooter();});", true);

                GetUIValuesFromObject(ControlsEnum.SPLITPAYNOWFORMULIPO);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_CalculateTotal", "$(document).ready(function(){CalculateTotal();CalculateTotalSplit();});", true);

                //if (InvoicePK > 0)
                //{
                //    GetFieldValues(ControlsEnum.INVOICEVNDMPGLIST);
                //    if (FinInvoiceVndTrxMpgList != null && FinInvoiceVndTrxMpgList.Count > 0)
                //    {
                //        GetFieldValues(ControlsEnum.INVOICEVNDHDR);
                //        GetUIValuesFromObject(ControlsEnum.PAYMENTSPLITLIST);
                //        isSplitChanged = false;
                //        SetFieldValues(ControlsEnum.PAYMENTSPLITLIST);
                //    }
                //}
            }

            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_CalculateTotal", "$(document).ready(function(){CalculatePymntSplitBalFooter();});", true);

            // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPoSplitUp]','" + GetLocalResourceObject("PaymentSplit").ToString() + "','850','300');", true);

        }
        /// <summary>
        /// Head office checkbox checked changed event
        /// </summary>
        private void HeadofficeCheckedChanged()
        {
            if (chkHeadOffice.Checked)
            {
                //txtBranchCode.Enabled = false;
                //txtBranchCode.Text = string.Empty;
                vrfBranchCode.Enabled = false;
                //txtBranchCode.CssClass = "input-disabled";
            }
            else
            {
                txtBranchCode.Enabled = true;
                vrfBranchCode.Enabled = true;
                txtBranchCode.CssClass = "";
            }
        }

        /// <summary>
        /// Set values to the control when edit details
        /// </summary>
        private void SetUIEditViewWhtPopup(GridViewRow grwWhtDetails)
        {
            try
            {

                divErrorLabel.Visible = false;
                finVatPaymentDetails = TempWHTTaxDetails[grwWhtDetails.RowIndex];
                if (finVatPaymentDetails != null)
                {

                    hdfWHTAccountPopup.Value = finVatPaymentDetails.WTH_TAX.ToString();
                    hdfWHTTaxCategory.Value = finVatPaymentDetails.WTH_TAX_CATEGORY.ToString();
                    hdfWHTTaxName.Value = finVatPaymentDetails.WTH_NAME;
                    txtDescriptionPopup.Text = finVatPaymentDetails.WTH_DESC;
                    if (finVatPaymentDetails.WTH_FORM_NO.HasValue)
                    {
                        ddlFormno.SelectedValue = finVatPaymentDetails.WTH_FORM_NO.ToString();
                    }
                    else
                    {
                        ddlFormno.SelectedValue = CommonConstants.SELECTVAL;
                    }
                    //ddlFormno.SelectedValue = finVatPaymentDetails.WTH_FORM_NO.ToString();
                    txtCustomerTxtWHT.Text = finVatPaymentDetails.WTH_PARTY_NAME;
                    txtpartyads.Text = finVatPaymentDetails.WTH_ADDRESS;
                    txtTaxid.Text = finVatPaymentDetails.WTH_TAX_ID;
                    txtPopupWHTAmount.Text = GetFormattedCurrency(finVatPaymentDetails.WTH_AMOUNT);
                    txtWHTTaxAmountPopup.Text = GetFormattedCurrency(finVatPaymentDetails.WTH_TAX_AMT);
                    txtWHTAccountPopup.Text = finVatPaymentDetails.WTH_NAME;
                    txtDescriptionPopup.Text = finVatPaymentDetails.WTH_DESC;
                    txtWthAddressType.Text = finVatPaymentDetails.WTH_BRANCH_NAME;
                    txtWthBranchCode.Text = finVatPaymentDetails.WTH_BRANCH_TEXT;
                    if (finVatPaymentDetails.WTH_BRANCH_TYPE == (byte)VendorContactTypeEnum.HeadOffice)
                    {
                        chkWthHeadOffice.Checked = true;
                    }
                    else
                    {
                        chkWthHeadOffice.Checked = false;
                    }
                    if (finVatPaymentDetails.WTH_PAYMENT_TYPE != 0)
                    {
                        ddlPayType.SelectedValue = Convert.ToString(finVatPaymentDetails.WTH_PAYMENT_TYPE);
                    }
                    else
                    {
                        ddlPayType.SelectedValue = CommonConstants.SELECTVAL;
                    }
                    whtTaxpk = !string.IsNullOrEmpty(hdfWHTAccountPopup.Value) ? Convert.ToInt32(hdfWHTAccountPopup.Value) : 0;
                    GetFieldValues(ControlsEnum.VENDORACCOUNTTAX);
                    if (dtVendorAccount != null && dtVendorAccount.Rows.Count > 0)
                    {
                        decimal amount = 0;
                        amount = txtPopupWHTAmount.Text != string.Empty ? Convert.ToDecimal(txtPopupWHTAmount.Text) : 0;
                        string taxFormula = dtVendorAccount.Rows[0]["TAX_FORMULA"].ToString();
                        hdfTaxformula.Value = taxFormula;
                    }
                }
                ViewState["WhtDetRowIndex"] = grwWhtDetails.RowIndex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private decimal GetTaxAmount(long InvoicePk)
        {
            decimal taxamnt = 0;
            foreach (GridViewRow grdrow in grdInvoiceList.Rows)
            {
                HiddenField hdfInvPK = (HiddenField)grdrow.FindControl("hdfInvoicePK");
                HiddenField hdfTotalTax = (HiddenField)grdrow.FindControl("hdfTotalTax");
                if (InvoicePk == Convert.ToInt64(hdfInvPK.Value))
                {
                    taxamnt = Convert.ToDecimal(hdfTotalTax.Value.Replace(",", ""));
                    break;
                }
            }
            return taxamnt;
        }

        /// <summary>
        /// Set values to the control when edit details
        /// </summary>
        private void SetUIEditViewVatPopup(GridViewRow grw)
        {
            try
            {

                divVatErrorLabel.Visible = false;
                finVatPaymentDetails = TempVATTaxDetails[grw.RowIndex];
                //ResetForm(2);
                //ResetForm(3);
                if (finVatPaymentDetails != null)
                {
                    //if (finVatPaymentDetails.WTH_TAX.HasValue)
                    //{
                    //    hdfVATAccountPopup.Value = finVatPaymentDetails.WTH_TAX.ToString();
                    //    hdfVATBUYTaxCategory.Value = finVatPaymentDetails.WTH_TAX_CATEGORY.ToString();
                    //    hdfVATBUYTaxName.Value = finVatPaymentDetails.WTH_NAME;
                    //}

                    if (finVatPaymentDetails.WTH_BRANCH.HasValue)
                    {
                        hdfAddressType.Value = finVatPaymentDetails.WTH_BRANCH.ToString();
                    }
                    else
                    {
                        hdfAddressType.Value = string.Empty;
                    }

                    txtAddressType.Text = finVatPaymentDetails.WTH_BRANCH_NAME;

                    if (finVatPaymentDetails.WTH_BRANCH_TYPE == (byte)VendorContactTypeEnum.HeadOffice)
                        chkHeadOffice.Checked = true;
                    HeadofficeCheckedChanged();
                    SetBranchCodeVisibility();

                    txtVatTaxId.Text = finVatPaymentDetails.WTH_TAX_ID;
                    txtBranchCode.Text = HttpUtility.HtmlDecode(finVatPaymentDetails.WTH_BRANCH_TEXT);
                    txtVendorPopup.Text = finVatPaymentDetails.WTH_PARTY_NAME;
                    if (finVatPaymentDetails.WTH_VENDOR.HasValue)
                    {
                        hdfVendorPopup.Value = finVatPaymentDetails.WTH_VENDOR.ToString();
                    }
                    else
                    {
                        hdfVendorPopup.Value = string.Empty;
                        GetFieldValues(ControlsEnum.VENDORSELECTEDDTL);
                        SetFieldValues(ControlsEnum.VENDORSELECTEDDTL);
                    }


                    if (finVatPaymentDetails.WTH_TAX_DATE.HasValue)
                    {
                        txtVatTaxInvDate.Text = Convert.ToDateTime(finVatPaymentDetails.WTH_TAX_DATE).ToString(Resources.Constants.DateFormatShort);
                    }
                    if (finVatPaymentDetails.WTH_REFUND_DATE.HasValue)
                    {
                        txtVatRefundDate.Text = Convert.ToDateTime(finVatPaymentDetails.WTH_REFUND_DATE).ToString(Resources.Constants.DateFormatMonthYear);
                    }

                    chkOriginalinvoice.Checked = finVatPaymentDetails.WTH_INV_RECEIVED == 1 ? true : false;

                    try
                    {
                        ddlVATAccountPopup.SelectedValue = finVatPaymentDetails.WTH_TAX.ToString();
                        GetUIValuesFromObject(ControlsEnum.TAXTYPECHANGED);
                    }
                    catch { }
                    txtBeforeTaxAmount.Text = GetFormattedCurrency(finVatPaymentDetails.WTH_AMOUNT);
                    txtVATTaxAmountPopup.Text = GetFormattedCurrency(finVatPaymentDetails.WTH_TAX_AMT);
                    //tempVATTax.WTH_DESC = txtDescriptionPopup.Text;
                    //tempVATTax.WTH_FORM_NO = Convert.ToInt32(ddlFormno.SelectedValue);

                    //tempVATTax.WTH_ADDRESS = txtpartyads.Text;
                    txtVatTaxInvNo.Text = HttpUtility.HtmlDecode(finVatPaymentDetails.WTH_TAX_INV_NO);
                    try
                    {
                        ddlPurInvNo.SelectedValue = finVatPaymentDetails.WTH_PUR_INVOICE.ToString();
                    }
                    catch { }
                    txtMaterial.Text = HttpUtility.HtmlDecode(finVatPaymentDetails.WTH_ITEM_TEXT);

                }
                ViewState["VatDetRowIndex"] = grw.RowIndex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Set values to the control when edit details
        /// </summary>
        private void SetUIEditViewPaymentModeDetails(int RowIndex)
        {
            try
            {

                if (PaymentModeDetailsList[RowIndex] != null)
                {
                    ddlMode.SelectedValue = PaymentModeDetailsList[RowIndex].PDM_MODE.ToString();
                    ActionHandler(ddlMode, new EventArgs());
                    if (PaymentModeDetailsList[RowIndex].PDM_BANK.HasValue)
                        hdfPaymentBank.Value = PaymentModeDetailsList[RowIndex].PDM_BANK.Value.ToString();
                    else
                        hdfPaymentBank.Value = string.Empty;

                    if (PaymentModeDetailsList[RowIndex].PDM_MODE != (int)PaymentModeEnum.CASH)
                    {
                        txtBranch.Text = HttpUtility.HtmlDecode(PaymentModeDetailsList[RowIndex].PDM_BRANCH);
                        txtInstrumentNo.Text = HttpUtility.HtmlDecode(PaymentModeDetailsList[RowIndex].PDM_INSTR_NO);
                        if (PaymentModeDetailsList[RowIndex].PDM_INSTR_DATE.HasValue)
                            txtInstrumentDate.Text = PaymentModeDetailsList[RowIndex].PDM_INSTR_DATE.Value.ToString(Resources.Constants.DateFormatShort);
                        hdfFavourof.Value = txtFavourof.Text = HttpUtility.HtmlDecode(PaymentModeDetailsList[RowIndex].PDM_INSTR_FAVOUR);
                    }
                    if (PaymentModeDetailsList[RowIndex].PDM_MODE == (int)PaymentModeEnum.CHEQUE)
                    {
                        chkPDC.Checked = Convert.ToBoolean(PaymentModeDetailsList[RowIndex].PDM_PDC);
                    }
                    else
                    {
                        chkPDC.Checked = false;
                    }
                    if (!string.IsNullOrEmpty(hdfPaymentBank.Value) && Convert.ToInt32(hdfPaymentBank.Value.ToString()) > 0)
                    {
                        GetFieldValues(ControlsEnum.BANK);
                        if (finCashBankMstList != null && finCashBankMstList.Count > 0)
                        {
                            if (Convert.ToInt32(ddlMode.SelectedValue) != (int)PaymentModeEnum.CASH && Convert.ToInt32(ddlMode.SelectedValue) != (int)PaymentModeEnum.OTHERS)
                            {
                                txtAccountNo.Text = finCashBankMstList[0].CBM_ACC_NO;
                                txtBranch.Text = finCashBankMstList[0].CBM_BRANCH;
                                hdfPaymentBankName.Value = txtPaymentBank.Text = finCashBankMstList[0].CBM_CODE + " - " + finCashBankMstList[0].CBM_NAME;
                            }
                            hdfBankAccount.Value = finCashBankMstList[0].CBM_ACCOUNT.ToString();
                        }
                        else
                        {
                            txtAccountNo.Text = string.Empty;
                            txtBranch.Text = string.Empty;
                            hdfBankAccount.Value = string.Empty;
                        }
                    }

                    if (PaymentModeDetailsList[RowIndex].PDM_BANK_CHARGE_CURR.HasValue)
                        ddlBankChargeCurrency.SelectedValue = PaymentModeDetailsList[RowIndex].PDM_BANK_CHARGE_CURR.ToString();
                    GetFieldValues(ControlsEnum.BANKCURRENCYEXCHANGERATE);
                    txtBankCharge.Text = Math.Round(PaymentModeDetailsList[RowIndex].PDM_BANK_CHARGE, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                    chkBankCharge.Checked = PaymentModeDetailsList[RowIndex].PDM_BANK_CHARGE_TYPE;
                    //txtExchangeRate.Text = PaymentModeDetailsList[RowIndex].PDM_EXCHG_RATE.ToString();
                    txtPaymentAmount.Text = GetFormattedCurrency(PaymentModeDetailsList[RowIndex].PDM_PAID_AMOUNT);
                    txtTotalAmountBC.Text = GetFormattedCurrency(PaymentModeDetailsList[RowIndex].PDM_PAID_AMOUNT_BC);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region --- For Grid Actions----
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            Label lblTotalFooter;
            Label lblTotalAllocateAdjn;
            decimal total;
            decimal taxpercentage;
            decimal basevalue;
            decimal taxamt = 0;
            HiddenField hdfWHTFormNo;
            HiddenField hdfCategory;
            HiddenField hdfInvoiceType;
            HiddenField hdfGroup;
            HiddenField hdfTotalAmt;
            HiddenField hdfTaxAmt;
            HiddenField hdfTaxHdrDtlAmt;
            HiddenField hdfInvoicePK;
            HiddenField hdfPayable;
            HiddenField hdfPaymentMpgPK;
            HiddenField hdfBaltopay;
            HiddenField hdfInitialBaltoPay;
            HiddenField hdfTotalAllocateAdjn;
            HiddenField hdfBalanceAdjn;
            Label lblInvoiceNo;

            LinkButton lnkInvoiceNo;

            Label lblInvoiceDate;
            Label lblVendorInv;
            Label lblVendInvNo;
            Label lblInvCurrency;
            Label lblCmpDisplayCode;
            Label lblGrossAmount;
            Label lblTax;
            Label lblDiscount;
            Label lblTotalAmount;
            Label lblTotalTaxAmount;
            Label lblPaid;
            Label lblBaltopay;
            TextBox txtPayNow;
            Button lnkRemove;
            HiddenField hdfPayNow;
            Label lblOtherCharges;
            //Splitup
            HiddenField hdfPOPK;
            LinkButton lnkPONOSplit;
            //Label lblPONOSplit;
            Label lblformnoGRD;
            Label lblPODateSplit;
            Label lblCurrSplit;
            Label lblAmountSplit;
            Label lblInvdAmtSplit;
            Label lblPaidSplit;
            Label lblBalanceSplit;
            TextBox txtPayNowSplit;

            Label lblOtherChargesSplit;

            HiddenField hdfPayNowSplit;
            HiddenField hdfPaymentSplitPK;
            Label lblTotalPayNowFooterSplit;
            HiddenField hdfTotalPayNowFooterSplit;
            HiddenField hdfOtherChargesPrev;
            Button lnkAllocation;
            Label lblAdjAmount;
            Label lblTotalTax;
            TextBox txtAdjustments;
            TextBox txtOtherCharges;
            CustomValidator vcmPayNow;
            Label lblTaxSplit;
            Label lblDiscountSplit;
            HiddenField hdfGrossAmt;
            SPADM_APP_STATUS_CFG_GET_KV_Result wkfStatus;
            FIN_PAYMENT_VND_PO_MPG tempFinPaymentVndPoMpgObj = null;

            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                {
                    if (((GridView)sender).ID == "grdWHTTaxDetails")
                    {
                        hdfWHTFormNo = e.Row.FindControl("hdfWHTFormNo") as HiddenField;
                        HiddenField hdfWhtTaxAmount = e.Row.FindControl("hdfWhtTaxAmount") as HiddenField;
                        HiddenField hdfWhtBranchType = e.Row.FindControl("hdfWhtBranchType") as HiddenField;
                        Label lblWhtTye = e.Row.FindControl("lblWhtTye") as Label;
                        lblformnoGRD = e.Row.FindControl("lblformnoGRD") as Label;
                        if (TempWHTTaxDetails != null && TempWHTTaxDetails.Count > 0)
                        {
                            string tempFormno = ddlFormno.Items.FindByValue(hdfWHTFormNo.Value).Text;
                            lblformnoGRD.Text = tempFormno;
                            //fill WHT popup
                            ddlFormno.SelectedValue = TempWHTTaxDetails[e.Row.RowIndex].WTH_FORM_NO.ToString();
                            txtCustomerTxtWHT.Text = ERP.Utilities.CommonFunctions.GetShortString(TempWHTTaxDetails[e.Row.RowIndex].WTH_PARTY_NAME, 300);
                            txtCustomerTxtWHT.ToolTip = TempWHTTaxDetails[e.Row.RowIndex].WTH_PARTY_NAME;
                            hdfvendorWHTPK.Value = TempWHTTaxDetails[e.Row.RowIndex].WTH_PK.ToString();
                            txtpartyads.Text = TempWHTTaxDetails[e.Row.RowIndex].WTH_ADDRESS;
                            txtTaxid.Text = TempWHTTaxDetails[e.Row.RowIndex].WTH_TAX_ID;
                            TaxWhtTotal += Convert.ToDecimal(hdfWhtTaxAmount.Value);

                            if (TempConfigMstDetails.Count > 0)
                            {
                                if (!string.IsNullOrEmpty(hdfWhtBranchType.Value) && (Convert.ToInt32(hdfWhtBranchType.Value) != Convert.ToInt32(CommonConstants.SELECT_VALUE_ZERO)))
                                {
                                    lblWhtTye.Text = TempConfigMstDetails.SingleOrDefault(cnfg => cnfg.CFG_VALUE == Convert.ToByte(hdfWhtBranchType.Value)).CFG_DATA;
                                    if (Convert.ToByte(hdfWhtBranchType.Value) == (byte)VendorContactTypeEnum.Branch)
                                    {
                                        lblWhtTye.ToolTip = lblWhtTye.Text + " (" + TempWHTTaxDetails[e.Row.RowIndex].WTH_BRANCH_TEXT + ")";
                                    }
                                }
                            }
                            else
                            {
                                GetFieldValues(ControlsEnum.VENDORTYPES);
                                if (TempConfigMstDetails.Count > 0)
                                {
                                    if (!string.IsNullOrEmpty(hdfWhtBranchType.Value) && (Convert.ToInt32(hdfWhtBranchType.Value) != Convert.ToInt32(CommonConstants.SELECT_VALUE_ZERO)))
                                    {
                                        lblWhtTye.Text = TempConfigMstDetails.SingleOrDefault(cnfg => cnfg.CFG_VALUE == Convert.ToByte(hdfWhtBranchType.Value)).CFG_DATA;
                                        if (Convert.ToByte(hdfWhtBranchType.Value) == (byte)VendorContactTypeEnum.Branch)
                                        {
                                            lblWhtTye.ToolTip = lblWhtTye.Text + " (" + TempWHTTaxDetails[e.Row.RowIndex].WTH_BRANCH_TEXT + ")";
                                        }
                                    }
                                }
                            }
                            // END fill WHT popup
                        }
                    }

                    if (((GridView)sender).ID == "grdVATTaxDetails")
                    {
                        HiddenField hdfBranchType = e.Row.FindControl("hdfBranchType") as HiddenField;
                        HiddenField hdfIvnPk = e.Row.FindControl("hdfIvnPk") as HiddenField;

                        HiddenField hdfAmount = e.Row.FindControl("hdfAmount") as HiddenField;
                        HiddenField hdfTaxAmount = e.Row.FindControl("hdfTaxAmount") as HiddenField;

                        //HiddenField hdfIsVatbuyNotDue = e.Row.FindControl("hdfIsVatbuyNotDue") as HiddenField; 

                        Label lblTye = e.Row.FindControl("lblTye") as Label;
                        Label lblPurInvNo = e.Row.FindControl("lblPurInvNo") as Label;

                        if (TempVATTaxDetails != null && TempVATTaxDetails.Count > 0)
                        {


                            AmountTotal += Convert.ToDecimal(hdfAmount.Value);
                            TaxTotal += Convert.ToDecimal(hdfTaxAmount.Value);

                            //txtVendorPopup.Text = purVendorMstList[0].VEN_NAME;
                            //hdfVendorPopup.Value = purVendorMstList[0].VEN_PK.ToString();


                            //string tempFormno = ddlFormno.Items.FindByValue(hdfWHTFormNo.Value).Text;
                            //lblformnoGRD.Text = tempFormno;
                            //fill WHT popup
                            //ddlFormno.SelectedValue = TempVATTaxDetails[e.Row.RowIndex].WTH_FORM_NO.ToString();
                            txtVendorPopup.Text = ERP.Utilities.CommonFunctions.GetShortString(TempVATTaxDetails[e.Row.RowIndex].WTH_PARTY_NAME, 300);
                            //txtCustomerTxtWHT.ToolTip = TempWHTTaxDetails[e.Row.RowIndex].WTH_PARTY_NAME;
                            hdfVendorVatPK.Value = TempVATTaxDetails[e.Row.RowIndex].WTH_PK.ToString();
                            //txtpartyads.Text = TempVATTaxDetails[e.Row.RowIndex].WTH_ADDRESS;
                            if (TempVATTaxDetails[e.Row.RowIndex].WTH_TAX_DATE.HasValue)
                            {
                                txtVatTaxInvDate.Text = Convert.ToDateTime(TempVATTaxDetails[e.Row.RowIndex].WTH_TAX_DATE).ToString(Resources.Constants.DateFormatShort);
                            }
                            chkOriginalinvoice.Checked = TempVATTaxDetails[e.Row.RowIndex].WTH_INV_RECEIVED == 1 ? true : false;
                            txtVatTaxId.Text = TempVATTaxDetails[e.Row.RowIndex].WTH_TAX_ID;
                            //ddlAddressType.SelectedValue = TempVATTaxDetails[e.Row.RowIndex].WTH_BRANCH_TYPE.ToString();
                            if (PurInvoices != null && PurInvoices.Count > 0)
                            {
                                lblPurInvNo.Text = PurInvoices.SingleOrDefault(pinv => pinv.IVH_PK == Convert.ToInt64(hdfIvnPk.Value)).IVH_NO;
                            }
                            if (TempConfigMstDetails.Count > 0)
                            {
                                if (!string.IsNullOrEmpty(hdfBranchType.Value) && (Convert.ToInt32(hdfBranchType.Value) != Convert.ToInt32(CommonConstants.SELECT_VALUE_ZERO)))
                                {
                                    lblTye.Text = TempConfigMstDetails.SingleOrDefault(cnfg => cnfg.CFG_VALUE == Convert.ToByte(hdfBranchType.Value)).CFG_DATA;
                                    if (Convert.ToByte(hdfBranchType.Value) == (byte)VendorContactTypeEnum.Branch)
                                    {
                                        lblTye.ToolTip = lblTye.Text + " (" + TempVATTaxDetails[e.Row.RowIndex].WTH_BRANCH_TEXT + ")";
                                    }
                                }
                            }
                            else
                            {
                                GetFieldValues(ControlsEnum.VENDORTYPES);
                                if (TempConfigMstDetails.Count > 0)
                                {
                                    if (!string.IsNullOrEmpty(hdfBranchType.Value) && (Convert.ToInt32(hdfBranchType.Value) != Convert.ToInt32(CommonConstants.SELECT_VALUE_ZERO)))
                                    {
                                        lblTye.Text = TempConfigMstDetails.SingleOrDefault(cnfg => cnfg.CFG_VALUE == Convert.ToByte(hdfBranchType.Value)).CFG_DATA;
                                        if (Convert.ToByte(hdfBranchType.Value) == (byte)VendorContactTypeEnum.Branch)
                                        {
                                            lblTye.ToolTip = lblTye.Text + " (" + TempVATTaxDetails[e.Row.RowIndex].WTH_BRANCH_TEXT + ")";
                                        }
                                    }
                                }
                            }

                            //hdfIsVatbuyNotDue.Value = "0"; 
                            //if (TempVATTaxDetails[e.Row.RowIndex].WTH_TAX.HasValue)
                            //{

                            //    DataTable dtTaxMst = BusinessLogic.Administration.Masters.TaxSettingsMaster.GetTaxDetails(Convert.ToInt32(TempVATTaxDetails[e.Row.RowIndex].WTH_TAX), 0, currentUser.SBUID, 2);
                            //    if (dtTaxMst != null && dtTaxMst.Rows.Count > 0 && Convert.ToInt32(dtTaxMst.Rows[0]["TAX_NOT_DUE"]) == 1)
                            //    {
                            //        hdfIsVatbuyNotDue.Value = "1";                                   
                            //    }
                            //}
                        }
                    }
                }
                else if (e.Row.RowType == DataControlRowType.Footer)
                {
                    if (((GridView)sender).ID == "grdVATTaxDetails")
                    {
                        Label lblAmountTotal = (Label)e.Row.FindControl("lblAmountTotal");
                        Label lblTaxTotal = (Label)e.Row.FindControl("lblTaxTotal");
                        lblAmountTotal.Text = Math.Round(AmountTotal, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                        lblTaxTotal.Text = Math.Round(TaxTotal, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                    }
                    else if (((GridView)sender).ID == "grdWHTTaxDetails")
                    {
                        Label lblWhtTaxTotal = (Label)e.Row.FindControl("lblWhtTaxTotal");
                        lblWhtTaxTotal.Text = Math.Round(TaxWhtTotal, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                    }

                }
                if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                {
                    if (((GridView)sender).ID == "grdInvoiceList")
                    {
                        hdfCategory = e.Row.FindControl("hdfCategory") as HiddenField;
                        hdfInvoiceType = e.Row.FindControl("hdfInvoiceType") as HiddenField;
                        hdfGroup = e.Row.FindControl("hdfGroup") as HiddenField;
                        hdfTotalAmt = e.Row.FindControl("hdfTotalAmt") as HiddenField;
                        hdfTaxAmt = e.Row.FindControl("hdfTaxAmt") as HiddenField;
                        hdfTaxHdrDtlAmt = e.Row.FindControl("hdfTaxHdrDtlAmt") as HiddenField;
                        hdfGrossAmt = e.Row.FindControl("hdfGrossAmt") as HiddenField;

                        hdfInvoicePK = e.Row.FindControl("hdfInvoicePK") as HiddenField;
                        hdfPaymentMpgPK = e.Row.FindControl("hdfPaymentMpgPK") as HiddenField;
                        hdfBaltopay = e.Row.FindControl("hdfBaltopay") as HiddenField;
                        hdfInitialBaltoPay = e.Row.FindControl("hdfInitialBaltoPay") as HiddenField;
                        lblInvoiceNo = e.Row.FindControl("lblInvoiceNo") as Label;
                        lnkInvoiceNo = e.Row.FindControl("lnkInvoiceNo") as LinkButton;

                        lblOtherCharges = e.Row.FindControl("lblOtherCharges") as Label;

                        lblInvoiceDate = e.Row.FindControl("lblInvoiceDate") as Label;
                        lblVendorInv = e.Row.FindControl("lblVendorInv") as Label;
                        lblVendInvNo = e.Row.FindControl("lblVendInvNo") as Label;
                        lblInvCurrency = e.Row.FindControl("lblInvCurrency") as Label;
                        lblCmpDisplayCode = e.Row.FindControl("lblCmpDisplayCode") as Label;
                        lblGrossAmount = e.Row.FindControl("lblGrossAmount") as Label;
                        lblTax = e.Row.FindControl("lblTax") as Label;
                        lblDiscount = e.Row.FindControl("lblDiscount") as Label;
                        lblTotalAmount = e.Row.FindControl("lblTotalAmount") as Label;
                        hdfPayable = e.Row.FindControl("hdfPayable") as HiddenField;
                        lblPaid = e.Row.FindControl("lblPaid") as Label;
                        lblBaltopay = e.Row.FindControl("lblBaltopay") as Label;
                        txtPayNow = e.Row.FindControl("txtPayNow") as TextBox;
                        txtOtherCharges = e.Row.FindControl("txtOtherCharges") as TextBox;
                        lnkRemove = e.Row.FindControl("lnkRemove") as Button;
                        lnkAllocation = e.Row.FindControl("lnkAllocation") as Button;
                        //vcmPayNow = e.Row.FindControl("vcmPayNow") as CustomValidator;
                        hdfPayNow = e.Row.FindControl("hdfPayNow") as HiddenField;
                        hdfOtherChargesPrev = e.Row.FindControl("hdfOtherChargesPrev") as HiddenField;
                        lblAdjAmount = e.Row.FindControl("lblAdjAmount") as Label;

                        //lblTotalTaxAmount = e.Row.FindControl("lblTotalTaxAmount") as Label;
                        lblAdjAmount = e.Row.FindControl("lblAdjAmount") as Label;
                        lblTotalTax = e.Row.FindControl("lblTotalTax") as Label;
                        HiddenField hdfTotalTax = e.Row.FindControl("hdfTotalTax") as HiddenField;
                        txtAdjustments = e.Row.FindControl("txtAdjustments") as TextBox;
                        Label lblCrdrAlcnAmount = e.Row.FindControl("lblCrdrAlcnAmount") as Label;
                        Label lblCnAmount = e.Row.FindControl("lblCnAmount") as Label;

                        if (finInvoiceHdrList != null && finInvoiceHdrList.Count > 0)
                        {

                            hdfFavourof.Value = txtFavourof.Text = finInvoiceHdrList[0].PUR_VENDOR_MST.VEN_NAME;
                            lblCustomerTxt.Text = ERP.Utilities.CommonFunctions.GetShortString(finInvoiceHdrList[0].PUR_VENDOR_MST.VEN_NAME, 50);
                            lblCustomerTxt.ToolTip = finInvoiceHdrList[0].PUR_VENDOR_MST.VEN_NAME;
                            hdfCusPK.Value = finInvoiceHdrList[0].IVH_VENDOR.ToString();

                            decimal lineItemTax = finInvoiceHdrList[e.Row.RowIndex].FIN_INVOICE_VND_DTL.Sum(ss => ss.VID_TAX);
                            decimal lineItemDiscount = finInvoiceHdrList[e.Row.RowIndex].FIN_INVOICE_VND_DTL.Sum(ss => ss.VID_DISCOUNT);

                            hdfCategory.Value = finInvoiceHdrList[e.Row.RowIndex].IVH_CATEGORY.ToString();
                            hdfInvoiceType.Value = finInvoiceHdrList[e.Row.RowIndex].IVH_TYPE.ToString();
                            hdfGroup.Value = finInvoiceHdrList[e.Row.RowIndex].IVH_GROUP.ToString();
                            hdfTotalAmt.Value = (finInvoiceHdrList[e.Row.RowIndex].IVH_AMOUNT_TC - finInvoiceHdrList[e.Row.RowIndex].IVH_DISCOUNT_TC + lineItemDiscount).ToString();
                            hdfTaxAmt.Value = finInvoiceHdrList[e.Row.RowIndex].IVH_TAX_TC.ToString() == string.Empty ? (0 + lineItemTax).ToString() : (finInvoiceHdrList[e.Row.RowIndex].IVH_TAX_TC + lineItemTax).ToString();
                            hdfInvoicePK.Value = finInvoiceHdrList[e.Row.RowIndex].IVH_PK.ToString();
                            hdfPaymentMpgPK.Value = "0";
                            lblInvoiceNo.Text = finInvoiceHdrList[e.Row.RowIndex].IVH_NO;
                            lnkInvoiceNo.Text = finInvoiceHdrList[e.Row.RowIndex].IVH_NO;
                            lblInvoiceNo.ToolTip = finInvoiceHdrList[e.Row.RowIndex].IVH_NO;
                            lblInvoiceNo.ToolTip = GetLocalResourceObject("DueDate").ToString() + " : " + finInvoiceHdrList[e.Row.RowIndex].IVH_DATE.ToString(Resources.Constants.DateFormatShort);

                            lnkInvoiceNo.ToolTip = GetLocalResourceObject("DueDate").ToString() + " : " + finInvoiceHdrList[e.Row.RowIndex].IVH_DATE.ToString(Resources.Constants.DateFormatShort);
                            lnkInvoiceNo.CommandArgument = finInvoiceHdrList[e.Row.RowIndex].IVH_PK.ToString();
                            //lblInvoiceDate.Text = finInvoiceHdrList[e.Row.RowIndex].IVH_DATE.ToString(Resources.Constants.DateFormatShort);
                            //lblInvoiceDate.ToolTip = finInvoiceHdrList[e.Row.RowIndex].IVH_DATE.ToString(Resources.Constants.DateFormatShort);
                            //lblInvoiceDate.Text = finInvoiceHdrList[e.Row.RowIndex].IVH_DATE_PAY_BY.ToString(Resources.Constants.DateFormatShort);
                            //lblInvoiceDate.ToolTip = finInvoiceHdrList[e.Row.RowIndex].IVH_DATE_PAY_BY.ToString(Resources.Constants.DateFormatShort);

                            lblVendorInv.Text = ERP.Utilities.CommonFunctions.GetShortString(finInvoiceHdrList[e.Row.RowIndex].PUR_VENDOR_MST.VEN_NAME, 10);
                            lblVendorInv.ToolTip = finInvoiceHdrList[e.Row.RowIndex].PUR_VENDOR_MST.VEN_NAME;
                            lblVendInvNo.Text = finInvoiceHdrList[e.Row.RowIndex].IVH_VENDOR_INV_NO;
                            lblVendInvNo.ToolTip = finInvoiceHdrList[e.Row.RowIndex].IVH_VENDOR_INV_NO;
                            lblInvCurrency.Text = finInvoiceHdrList[e.Row.RowIndex].ADM_CURRENCY_MST1.CUR_CODE;
                            lblInvCurrency.ToolTip = finInvoiceHdrList[e.Row.RowIndex].ADM_CURRENCY_MST1.CUR_CODE;

                            lblCmpDisplayCode.Text = lblCmpDisplayCode.ToolTip = finInvoiceHdrList[e.Row.RowIndex].ADM_COMPANY_MST.CMP_DISPLAY_CODE;
                            lblCmpDisplayCode.CssClass = finInvoiceHdrList[e.Row.RowIndex].ADM_COMPANY_MST.CMP_LINE_COLOUR;


                            //   GrossAmount  Setting

                            if (finInvoiceHdrList[e.Row.RowIndex].IVH_CATEGORY == (int)POInvoiceCategory.Advanced)
                            {
                                lblGrossAmount.Text = String.Format("{0:c}", finInvoiceHdrList[e.Row.RowIndex].IVH_AMOUNT_TC);
                                hdfGrossAmt.Value = finInvoiceHdrList[e.Row.RowIndex].IVH_AMOUNT_TC.ToString();
                                lblGrossAmount.ToolTip = String.Format("{0:c}", finInvoiceHdrList[e.Row.RowIndex].IVH_AMOUNT_TC);
                            }
                            else
                            {
                                decimal GrossAmount = 0;
                                if (finInvoiceHdrList[e.Row.RowIndex].FIN_INVOICE_VND_DTL != null)
                                {
                                    GrossAmount += (decimal)finInvoiceHdrList[e.Row.RowIndex].FIN_INVOICE_VND_DTL.Sum(a => a.VID_AMOUNT);
                                }
                                lblGrossAmount.Text = String.Format("{0:c}", GrossAmount);
                                hdfGrossAmt.Value = GrossAmount.ToString();
                                lblGrossAmount.ToolTip = String.Format("{0:c}", GrossAmount);
                            }
                            //End


                            decimal tax = finInvoiceHdrList[e.Row.RowIndex].IVH_TAX_TC;
                            decimal disc = finInvoiceHdrList[e.Row.RowIndex].IVH_DISCOUNT_TC;
                            if (finInvoiceHdrList[e.Row.RowIndex].FIN_INVOICE_VND_DTL != null)
                            {
                                foreach (FIN_INVOICE_VND_DTL dtlObj in finInvoiceHdrList[e.Row.RowIndex].FIN_INVOICE_VND_DTL)
                                {
                                    if (dtlObj.FIN_INVOICE_VND_TAX_DTL != null)
                                        //tax += (decimal)dtlObj.FIN_INVOICE_VND_TAX_DTL.Sum(aa => aa.VTL_TAX_AMT);
                                        tax += (decimal)dtlObj.FIN_INVOICE_VND_TAX_DTL.Where(aa => aa.VTL_TAX_CATEGORY == 1).Sum(aa => aa.VTL_TAX_AMT);
                                    disc += (decimal)dtlObj.FIN_INVOICE_VND_TAX_DTL.Where(aa => aa.VTL_TAX_CATEGORY == 3).Sum(aa => aa.VTL_TAX_AMT);
                                }
                            }
                            hdfTaxHdrDtlAmt.Value = tax.ToString();
                            //lblTax.ToolTip = lblTax.Text = String.Format("{0:c}", tax);
                            lblTax.ToolTip = lblTax.Text = String.Format("{0:c}", Convert.ToDecimal(finInvoiceHdrList[e.Row.RowIndex].IVH_TAX_TC.ToString() == string.Empty ? (0 + lineItemTax).ToString() : (finInvoiceHdrList[e.Row.RowIndex].IVH_TAX_TC + lineItemTax).ToString()));
                            lblDiscount.Text = String.Format("{0:c}", disc);
                            lblDiscount.ToolTip = String.Format("{0:c}", disc);
                            lblTotalAmount.Text = String.Format("{0:c}", finInvoiceHdrList[e.Row.RowIndex].IVH_AMOUNT_NET_TC);
                            hdfPayable.Value = (finInvoiceHdrList[e.Row.RowIndex].IVH_AMOUNT_NET_TC).ToString();
                            lblTotalAmount.ToolTip = String.Format("{0:c}", finInvoiceHdrList[e.Row.RowIndex].IVH_AMOUNT_NET_TC);
                            //Adjustment amount not included in paid amount
                            //lblPaid.Text = String.Format("{0:c}", finInvoiceHdrList[e.Row.RowIndex].IVH_AMOUNT_PAID_TC);
                            //lblPaid.ToolTip = String.Format("{0:c}", finInvoiceHdrList[e.Row.RowIndex].IVH_AMOUNT_PAID_TC);
                            lblPaid.Text = lblPaid.ToolTip = String.Format("{0:c}", finInvoiceHdrList[e.Row.RowIndex].IVH_AMOUNT_PAID_TC + finInvoiceHdrList[e.Row.RowIndex].IVH_AMOUNT_DN_TC);

                            FinPayVndTrxMpgList = null;
                            FinPayVndTrxMpgList = finInvoiceHdrList[e.Row.RowIndex].FIN_PAYMENT_VND_TRX_MPG != null ? finInvoiceHdrList[e.Row.RowIndex].FIN_PAYMENT_VND_TRX_MPG.ToList() : null;
                            decimal balOtherCharges = 0;
                            decimal balToPay = 0;
                            decimal prevOtherCharges = 0;
                            if ((FinPayVndTrxMpgList != null && FinPayVndTrxMpgList.Count > 1) || FinPayVndTrxMpgList == null || FinPayVndTrxMpgList.Count == 0)
                            {
                                balToPay = (finInvoiceHdrList[e.Row.RowIndex].IVH_AMOUNT_NET_TC) -
                                                 finInvoiceHdrList[e.Row.RowIndex].IVH_AMOUNT_PAID_TC +
                                                 finInvoiceHdrList[e.Row.RowIndex].IVH_AMOUNT_CN_TC -
                                                 finInvoiceHdrList[e.Row.RowIndex].IVH_AMOUNT_DN_TC;
                                //prevOtherCharges = FinPayVndTrxMpgList[0].FIN_INVOICE_VND_HDR.IVH_DEL_STATUS == 0 ? FinPayVndTrxMpgList[0].PVM_OTHER_AMOUNT : 0;
                            }
                            else if (FinPayVndTrxMpgList != null && FinPayVndTrxMpgList.Count == 1)
                            {
                                balToPay = (finInvoiceHdrList[e.Row.RowIndex].IVH_AMOUNT_NET_TC) -
                                                 //(FinPayVndTrxMpgList[0].FIN_INVOICE_VND_HDR.IVH_DEL_STATUS ==0 ? FinPayVndTrxMpgList[0].PVM_PAID_AMOUNT:0) +
                                                 finInvoiceHdrList[e.Row.RowIndex].IVH_AMOUNT_PAID_TC +
                                                 finInvoiceHdrList[e.Row.RowIndex].IVH_AMOUNT_CN_TC -
                                                 finInvoiceHdrList[e.Row.RowIndex].IVH_AMOUNT_DN_TC;
                                prevOtherCharges = FinPayVndTrxMpgList[0].FIN_INVOICE_VND_HDR.IVH_DEL_STATUS == 0 ? FinPayVndTrxMpgList[0].PVM_OTHER_AMOUNT : 0;
                            }

                            decimal otherChargesPrev = 0;
                            //OtherCharges
                            long? IVH_PK = finInvoiceHdrList[e.Row.RowIndex].IVH_PK;
                            if (finInvoiceHdrList[e.Row.RowIndex].FIN_PAYMENT_VND_TRX_MPG != null)
                            {
                                otherChargesPrev = (finInvoiceHdrList[e.Row.RowIndex].FIN_PAYMENT_VND_TRX_MPG.Where(pp => pp.PVM_INVOICE_HDR == IVH_PK && pp.FIN_PAYMENT_VND_HDR.PVH_DEL_STATUS == 0).ToList()).Sum(pv => pv.PVM_OTHER_AMOUNT);
                            }
                            decimal otherCharges = finInvoiceHdrList[e.Row.RowIndex].IVH_SHIP_CHARGE;
                            lblOtherCharges.Text = String.Format("{0:c}", otherCharges);
                            lblOtherCharges.ToolTip = String.Format("{0:c}", otherCharges);
                            otherCharges -= otherChargesPrev;
                            otherCharges = otherCharges < 0 ? 0 : otherCharges;
                            //txtOtherCharges.Text = String.Format("{0:c}", otherCharges);
                            //txtOtherCharges.ToolTip = String.Format("{0:c}", otherCharges);
                            //For resolving excess payment of other charge.
                            //  txtOtherCharges.Text = Math.Round(otherCharges, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            decimal OtherChargeDeduct = finInvoiceHdrList[e.Row.RowIndex].IVH_SHIP_CHARGE_DED;
                            txtOtherCharges.Text = Math.Round((otherCharges - (OtherChargeDeduct > 0 ? OtherChargeDeduct : 0)), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            txtOtherCharges.ToolTip = String.Format("{0:c}", otherCharges);

                            //Bug : 3014,3076
                            //hdfOtherChargesPrev.Value = otherChargesPrev.ToString();
                            hdfOtherChargesPrev.Value = (otherChargesPrev + finInvoiceHdrList[e.Row.RowIndex].IVH_SHIP_CHARGE_DED).ToString();

                            //decimal balToPay = finInvoiceHdrList[e.Row.RowIndex].IVH_AMOUNT_NET_TC -
                            //                  finInvoiceHdrList[e.Row.RowIndex].IVH_AMOUNT_PAID_TC +
                            //                  finInvoiceHdrList[e.Row.RowIndex].IVH_AMOUNT_CN_TC -
                            //                  finInvoiceHdrList[e.Row.RowIndex].IVH_AMOUNT_DN_TC;

                            // decimal balToPay = finInvoiceHdrList[e.Row.RowIndex].IVH_AMOUNT_NET_TC - finInvoiceHdrList[e.Row.RowIndex].IVH_AMOUNT_PAID_TC;
                            lblBaltopay.Text = String.Format("{0:c}", balToPay);
                            hdfBaltopay.Value = balToPay.ToString();
                            lblBaltopay.ToolTip = String.Format("{0:c}", balToPay);
                            balToPay = balToPay < 0 ? 0 : balToPay;
                            txtPayNow.Text = Math.Round(balToPay, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            txtPayNow.ToolTip = Math.Round(balToPay, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            hdfPayNow.Value = Math.Round(balToPay, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            lnkRemove.CommandArgument = finInvoiceHdrList[e.Row.RowIndex].IVH_PK.ToString();
                            lnkAllocation.Visible = (POGroup != POInvoiceGroup.Expense);//vcmPayNow.Enabled =
                            //lblAdjAmount.Text = String.Format("{0:c}", finInvoiceHdrList[e.Row.RowIndex].IVH_AMOUNT_CN_TC - finInvoiceHdrList[e.Row.RowIndex].IVH_AMOUNT_DN_TC);
                            lblAdjAmount.Text = lblAdjAmount.ToolTip = GetFormattedCurrency(0);
                            //lblTotalTaxAmount.Text = String.Format("{0:c}", finInvoiceHdrList[e.Row.RowIndex].IVH_TAX_TC.ToString() == string.Empty ? "0" : finInvoiceHdrList[e.Row.RowIndex].IVH_TAX_TC.ToString());
                            //lblTotalTaxAmount.ToolTip = lblTotalTaxAmount.Text;
                            hdfInitialBaltoPay.Value = Math.Round(balToPay, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();

                            txtAdjustments.Text = GetFormattedCurrency(0);
                            txtAdjustments.ToolTip = txtAdjustments.Text;

                            if (Convert.ToInt32(hdfCategory.Value) == (int)POInvoiceCategory.Advanced)
                            {
                                //taxpercentage = Convert.ToDecimal(hdfTaxAmt.Value) / (Convert.ToDecimal(finInvoiceHdrList[e.Row.RowIndex].IVH_AMOUNT_TC - finInvoiceHdrList[e.Row.RowIndex].IVH_DISCOUNT_TC) == 0 ? 1 : Convert.ToDecimal(finInvoiceHdrList[e.Row.RowIndex].IVH_AMOUNT_TC - finInvoiceHdrList[e.Row.RowIndex].IVH_DISCOUNT_TC));
                                taxpercentage = Convert.ToDecimal(hdfTaxHdrDtlAmt.Value) / (Convert.ToDecimal(finInvoiceHdrList[e.Row.RowIndex].IVH_AMOUNT_TC - finInvoiceHdrList[e.Row.RowIndex].IVH_DISCOUNT_TC) == 0 ? 1 : Convert.ToDecimal(finInvoiceHdrList[e.Row.RowIndex].IVH_AMOUNT_TC - finInvoiceHdrList[e.Row.RowIndex].IVH_DISCOUNT_TC));
                                basevalue = (txtPayNow.Text != string.Empty ? Convert.ToDecimal(txtPayNow.Text) : 0) / (1 + taxpercentage);
                                taxamt = (basevalue - (txtPayNow.Text != string.Empty ? Convert.ToDecimal(txtPayNow.Text) : 0));
                            }
                            lblTotalTax.Text = Math.Round(taxamt, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            lblTotalTax.ToolTip = lblTotalTax.Text;
                            hdfTotalTax.Value = Math.Round(taxamt, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();


                            //GetFieldValues(ControlsEnum.VENDOR);
                            if (finInvoiceHdrList[0].PUR_VENDOR_MST != null)
                            {
                                if (finInvoiceHdrList[0].PUR_VENDOR_MST.VEN_WHT_TAX != null)
                                {
                                    whtTaxpk = Convert.ToInt32(finInvoiceHdrList[0].PUR_VENDOR_MST.VEN_WHT_TAX);
                                    //ddlWHTAccount.SelectedIndex = Convert.ToInt32(ddlWHTAccount.Items.IndexOf(ddlWHTAccount.Items.FindByValue(finInvoiceHdrList[0].PUR_VENDOR_MST.VEN_WHT_TAX.ToString())));
                                    //chkVendorforpayemnt.Checked = finInvoiceHdrList[0].PUR_VENDOR_MST.VEN_PAY_FOR_VENDOR != null ? Convert.ToBoolean(finInvoiceHdrList[0].PUR_VENDOR_MST.VEN_PAY_FOR_VENDOR) : false;
                                    //GetFieldValues(ControlsEnum.VENDORACCOUNTTAX);
                                    //SetFieldValues(ControlsEnum.VENDORACCOUNTTAX);
                                    trVendorAccount.Style.Add("display", "");
                                }
                                else
                                {
                                    trVendorAccount.Style.Add("display", "none");
                                }
                            }
                            else
                            {
                                trVendorAccount.Style.Add("display", "none");
                            }

                            decimal InvCNAmount = 0;
                            InvCNAmount = finInvoiceHdrList[e.Row.RowIndex].FIN_CRDR_NOTE_MPG.Where(r => !r.FIN_CRDR_NOTE_HDR.CDH_IS_DELETED && r.FIN_CRDR_NOTE_HDR.CDH_STATUS != 0 && r.FIN_CRDR_NOTE_HDR.CDH_TYPE == (byte)DebitCreditModeEnum.CREDIT).Sum(sm => sm.CDM_AMOUNT);
                            InvCNAmount = Math.Round(InvCNAmount, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                            if (InvCNAmount > 0 && !IsCreditExist)
                                IsCreditExist = true;
                            if (lblCnAmount != null)
                                lblCnAmount.Text = lblCnAmount.ToolTip = String.Format("{0:c}", InvCNAmount);

                            //fill WHT popup
                            txtCustomerTxtWHT.Text = ERP.Utilities.CommonFunctions.GetShortString(finInvoiceHdrList[0].PUR_VENDOR_MST.VEN_NAME2, 300);
                            txtCustomerTxtWHT.ToolTip = finInvoiceHdrList[0].PUR_VENDOR_MST.VEN_NAME2;
                            hdfvendorWHTPK.Value = finInvoiceHdrList[0].PUR_VENDOR_MST.VEN_PK.ToString();
                            txtpartyads.Text = finInvoiceHdrList[0].PUR_VENDOR_MST.VEN_ADDR3;
                            txtTaxid.Text = finInvoiceHdrList[0].PUR_VENDOR_MST.VEN_TIN;
                            // END fill WHT popup
                            SetVendorPayment();

                            //Cedit note allocated amount                           
                            //lblCrdrAlcnAmount.Text=lblCrdrAlcnAmount.ToolTip=   String.Format("{0:c}", 0);

                            //Cedit note allocated amount
                            decimal CrdrAlcnAmnt = 0;
                            if (PaymentCrdrList != null && PaymentCrdrList.Count > 0)
                            {
                                CrdrAlcnAmnt = PaymentCrdrList.Where(r => r.PNM_INVOICE_HDR == finInvoiceHdrList[e.Row.RowIndex].IVH_PK).Sum(sm => sm.PNM_PAID_AMOUNT + sm.PNM_ADJ_AMOUNT);
                            }
                            if (lblCrdrAlcnAmount != null)
                                lblCrdrAlcnAmount.Text = lblCrdrAlcnAmount.ToolTip = String.Format("{0:c}", CrdrAlcnAmnt);



                            #region To bind changed data back
                            if (PaymentInvDetList != null && PaymentInvDetList.Count > 0)
                            {
                                List<PaymentInvoiceDetails> PymntInvdetLst = PaymentInvDetList.Where(r => r.PVM_INVOICE_HDR == finInvoiceHdrList[e.Row.RowIndex].IVH_PK).ToList();
                                if (PymntInvdetLst != null && PymntInvdetLst.Count > 0)
                                {
                                    lblAdjAmount.Text = lblAdjAmount.ToolTip = String.Format("{0:c}", PymntInvdetLst[0].PVM_ADJ_AMOUNT);
                                    lblBaltopay.Text = lblBaltopay.ToolTip = String.Format("{0:c}", PymntInvdetLst[0].PVM_BALANCE_TO_PAY);
                                    txtPayNow.Text = txtPayNow.ToolTip = PymntInvdetLst[0].PVM_PAYNOW_AMOUNT.ToString();
                                    lblCrdrAlcnAmount.Text = lblCrdrAlcnAmount.ToolTip = String.Format("{0:c}", PymntInvdetLst[0].PVM_CRDR_AMOUNT);
                                    lblTotalTax.Text = lblTotalTax.ToolTip = String.Format("{0:c}", PymntInvdetLst[0].PVM_TAX_AMOUNT);
                                    hdfTotalTax.Value = PymntInvdetLst[0].PVM_TAX_AMOUNT.ToString();
                                    txtAdjustments.Text = txtAdjustments.ToolTip = PymntInvdetLst[0].PVM_REDUCTION_AMOUNT.ToString();
                                }
                            }
                            #endregion
                        }
                        else if (finPaymentTrxMpgList != null && finPaymentTrxMpgList.Count > 0)
                        {
                            if (finPaymentTrxMpgList[e.Row.RowIndex].FIN_PAYMENT_VND_HDR != null) // invoice saved in payment
                            {
                                lblCustomerTxt.Text = ERP.Utilities.CommonFunctions.GetShortString(finPaymentTrxMpgList[0].FIN_PAYMENT_VND_HDR.PUR_VENDOR_MST.VEN_NAME, 50);
                                lblCustomerTxt.ToolTip = finPaymentTrxMpgList[0].FIN_PAYMENT_VND_HDR.PUR_VENDOR_MST.VEN_NAME;
                                hdfCusPK.Value = finPaymentTrxMpgList[0].FIN_PAYMENT_VND_HDR.PVH_VENDOR.ToString();

                                decimal lineItemTax = finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.FIN_INVOICE_VND_DTL.Sum(o => o.VID_TAX);
                                decimal lineItemDiscount = finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.FIN_INVOICE_VND_DTL.Sum(ss => ss.VID_DISCOUNT);

                                hdfCategory.Value = finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_CATEGORY.ToString();
                                hdfInvoiceType.Value = finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_TYPE.ToString();
                                hdfGroup.Value = finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_GROUP.ToString();
                                hdfTotalAmt.Value = (finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_TC - (finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_DISCOUNT_TC + lineItemDiscount)).ToString();
                                if (finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR != null)
                                {
                                    hdfTaxAmt.Value = finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_TAX_TC.ToString() == string.Empty ? (0 + lineItemTax).ToString() : (finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_TAX_TC + lineItemTax).ToString();
                                }
                                else
                                {
                                    hdfTaxAmt.Value = "0";
                                }

                                hdfInvoicePK.Value = finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_PK.ToString();
                                hdfPaymentMpgPK.Value = finPaymentTrxMpgList[e.Row.RowIndex].PVM_PK.ToString();
                                lblInvoiceNo.Text = finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_NO;
                                lnkInvoiceNo.Text = finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_NO;
                                //lblInvoiceNo.ToolTip =  finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_NO;
                                lblInvoiceNo.ToolTip = GetLocalResourceObject("DueDate").ToString() + " : " + finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_DATE_PAY_BY.ToString(Resources.Constants.DateFormatShort);
                                lnkInvoiceNo.ToolTip = GetLocalResourceObject("DueDate").ToString() + " : " + finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_DATE_PAY_BY.ToString(Resources.Constants.DateFormatShort);
                                lnkInvoiceNo.CommandArgument = finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_PK.ToString();

                                //lblOtherCharges.Text = finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_SHIP_CHARGE.ToString();
                                //lblOtherCharges.ToolTip = finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_SHIP_CHARGE.ToString();

                                //lblInvoiceDate.Text = finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_DATE.ToString(Resources.Constants.DateFormatShort);
                                //lblInvoiceDate.ToolTip = finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_DATE.ToString(Resources.Constants.DateFormatShort);
                                //lblInvoiceDate.Text = finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_DATE_PAY_BY.ToString(Resources.Constants.DateFormatShort);
                                //lblInvoiceDate.ToolTip = finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_DATE_PAY_BY.ToString(Resources.Constants.DateFormatShort);
                                lblVendorInv.Text = ERP.Utilities.CommonFunctions.GetShortString(finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.PUR_VENDOR_MST.VEN_NAME, 10);
                                lblVendorInv.ToolTip = finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.PUR_VENDOR_MST.VEN_NAME;
                                lblVendInvNo.Text = finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_VENDOR_INV_NO;
                                lblVendInvNo.ToolTip = finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_VENDOR_INV_NO;
                                lblInvCurrency.Text = finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.ADM_CURRENCY_MST1.CUR_CODE;
                                lblInvCurrency.ToolTip = finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.ADM_CURRENCY_MST1.CUR_CODE;

                                lblCmpDisplayCode.Text = lblCmpDisplayCode.ToolTip = finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.ADM_COMPANY_MST.CMP_DISPLAY_CODE;
                                lblCmpDisplayCode.CssClass = finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.ADM_COMPANY_MST.CMP_LINE_COLOUR;
                                //   GrossAmount  Setting
                                if (finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_CATEGORY == (int)POInvoiceCategory.Advanced)
                                {
                                    hdfGrossAmt.Value = finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_TC.ToString();
                                    lblGrossAmount.Text = String.Format("{0:c}", finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_TC);
                                    lblGrossAmount.ToolTip = String.Format("{0:c}", finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_TC);
                                }
                                else
                                {
                                    decimal GrossAmount = 0;
                                    if (finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.FIN_INVOICE_VND_DTL != null)
                                    {
                                        GrossAmount += (decimal)finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.FIN_INVOICE_VND_DTL.Sum(a => a.VID_AMOUNT);
                                    }
                                    lblGrossAmount.Text = String.Format("{0:c}", GrossAmount);
                                    hdfGrossAmt.Value = GrossAmount.ToString();
                                    lblGrossAmount.ToolTip = String.Format("{0:c}", GrossAmount);
                                }
                                //End



                                decimal tax = finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_TAX_TC;
                                decimal disc = finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_DISCOUNT_TC;
                                if (finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.FIN_INVOICE_VND_DTL != null)
                                {
                                    foreach (FIN_INVOICE_VND_DTL dtlObj in finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.FIN_INVOICE_VND_DTL)
                                    {
                                        if (dtlObj.FIN_INVOICE_VND_TAX_DTL != null)
                                            tax += (decimal)dtlObj.FIN_INVOICE_VND_TAX_DTL.Where(aa => aa.VTL_TAX_CATEGORY == 1).Sum(aa => aa.VTL_TAX_AMT);
                                        disc += (decimal)dtlObj.FIN_INVOICE_VND_TAX_DTL.Where(aa => aa.VTL_TAX_CATEGORY == 3).Sum(aa => aa.VTL_TAX_AMT);
                                    }
                                }
                                hdfTaxHdrDtlAmt.Value = tax.ToString();
                                lblTax.ToolTip = lblTax.Text = String.Format("{0:c}", Convert.ToDecimal(finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_TAX_TC.ToString() == string.Empty ? 0 + lineItemTax : finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_TAX_TC + lineItemTax));
                                lblDiscount.Text = String.Format("{0:c}", disc);
                                lblDiscount.ToolTip = String.Format("{0:c}", disc);
                                //lblTotalAmount.Text = String.Format("{0:c}", finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_NET_TC + lineItemTax);

                                lblTotalAmount.Text = String.Format("{0:c}", finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_NET_TC);
                                ////hdfPayable.Value = (finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_NET_TC + lineItemTax).ToString();
                                hdfPayable.Value = (finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_NET_TC).ToString();
                                //lblTotalAmount.ToolTip = String.Format("{0:c}", finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_NET_TC + lineItemTax);
                                lblTotalAmount.ToolTip = String.Format("{0:c}", finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_NET_TC);

                                decimal adjAlcn = finPaymentTrxMpgList[e.Row.RowIndex].FIN_PAYMENT_VND_ALCN_DTL.Sum(s => s.PAD_AMOUNT);

                                decimal paid = (finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_DEL_STATUS == 0 && finPaymentTrxMpgList[e.Row.RowIndex].PVM_BOUNCED == 0)
                                    ? finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_PAID_TC - finPaymentTrxMpgList[e.Row.RowIndex].PVM_PAID_AMOUNT
                                    : finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_PAID_TC;
                                //Adj allocation not included
                                //lblPaid.Text = String.Format("{0:c}", paid);
                                //lblPaid.ToolTip = String.Format("{0:c}", paid);
                                lblPaid.Text = lblPaid.ToolTip = String.Format("{0:c}", paid + finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_DN_TC - adjAlcn);

                                InPk = finPaymentTrxMpgList[e.Row.RowIndex].PVM_INVOICE_HDR != null ? Convert.ToInt64(finPaymentTrxMpgList[e.Row.RowIndex].PVM_INVOICE_HDR) : 0;
                                FinPayVndTrxMpgList = null;
                                GetFieldValues(ControlsEnum.INVVNDMPGLIST);
                                decimal balToPay = 0;
                                decimal balToPayWithoutAdjn = 0;
                                //if ((FinPayVndTrxMpgList != null && FinPayVndTrxMpgList.Count > 1) || FinPayVndTrxMpgList == null || FinPayVndTrxMpgList.Count == 0)
                                //{
                                decimal adj = (finPaymentTrxMpgList[e.Row.RowIndex].FIN_PAYMENT_VND_ALCN_DTL.Where(r => (r.FIN_CRDR_NOTE_HDR != null ? r.FIN_CRDR_NOTE_HDR.CDH_IS_DELETED == false : true) && (r.FIN_PAYMENT_VND_TRX_MPG1 != null ? r.FIN_PAYMENT_VND_TRX_MPG1.FIN_PAYMENT_VND_HDR.PVH_BOUNCED == 0 : true) && (r.FIN_PAYMENT_VND_TRX_MPG1 != null ? r.FIN_PAYMENT_VND_TRX_MPG1.FIN_PAYMENT_VND_HDR.PVH_DEL_STATUS == 0 : true)).Sum(c => c.PAD_AMOUNT));

                                //Adj amount not included
                                //balToPay = (finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_NET_TC) -
                                //               finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_PAID_TC +
                                //               (finPaymentTrxMpgList[e.Row.RowIndex].PVM_BOUNCED == 1 ? 0 : finPaymentTrxMpgList[e.Row.RowIndex].PVM_PAID_AMOUNT) +
                                //               finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_CN_TC - adj;

                                balToPay = (finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_NET_TC) -
                                             (finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_PAID_TC + finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_DN_TC) +
                                             (finPaymentTrxMpgList[e.Row.RowIndex].PVM_BOUNCED == 1 ? 0 : finPaymentTrxMpgList[e.Row.RowIndex].PVM_PAID_AMOUNT) +
                                             finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_CN_TC; // -adj;


                                //finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_DN_TC +


                                //////(finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_NET_TC + lineItemTax) -
                                ////balToPay = (finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_NET_TC) -
                                ////               finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_PAID_TC +
                                ////               finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_CN_TC -
                                ////               finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_DN_TC +
                                ////(finPaymentTrxMpgList[e.Row.RowIndex].PVM_BOUNCED == 1 ? 0 : finPaymentTrxMpgList[e.Row.RowIndex].PVM_PAID_AMOUNT);
                                //////balToPayWithoutAdjn = finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_NET_TC;
                                balToPayWithoutAdjn = balToPay + adj;
                                //}
                                //else if (FinPayVndTrxMpgList != null && FinPayVndTrxMpgList.Count == 1)
                                //{
                                //    //finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_NET_TC + lineItemTax) -   
                                //    ////balToPay = (finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_NET_TC) -                                     
                                //    ////               finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_CN_TC -
                                //    ////               finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_DN_TC;
                                //    balToPay = (finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_NET_TC) +
                                //                  finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_CN_TC -
                                //                  finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_DN_TC;
                                //    //balToPayWithoutAdjn = finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_NET_TC;
                                //    balToPayWithoutAdjn = balToPay + (finPaymentTrxMpgList[e.Row.RowIndex].FIN_PAYMENT_VND_ALCN_DTL.Where(r => (r.FIN_CRDR_NOTE_HDR != null ? r.FIN_CRDR_NOTE_HDR.CDH_IS_DELETED == false : true) && (r.FIN_PAYMENT_VND_TRX_MPG1 != null ? r.FIN_PAYMENT_VND_TRX_MPG1.FIN_PAYMENT_VND_HDR.PVH_BOUNCED == 0 : true) && (r.FIN_PAYMENT_VND_TRX_MPG1 != null ? r.FIN_PAYMENT_VND_TRX_MPG1.FIN_PAYMENT_VND_HDR.PVH_DEL_STATUS == 0 : true)).Sum(c => c.PAD_AMOUNT));

                                //}

                                decimal otherCharges = finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_SHIP_CHARGE;
                                decimal prevotherCharges = finPaymentTrxMpgList[e.Row.RowIndex].PVM_OTHER_AMOUNT;
                                lblOtherCharges.Text = String.Format("{0:c}", otherCharges);
                                lblOtherCharges.ToolTip = String.Format("{0:c}", otherCharges);

                                decimal balOtherCharges = otherCharges - prevotherCharges;
                                balOtherCharges = balOtherCharges < 0 ? 0 : balOtherCharges;
                                txtOtherCharges.Text = Math.Round(prevotherCharges, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();// String.Format("{0:c}", prevotherCharges);
                                txtOtherCharges.ToolTip = String.Format("{0:c}", prevotherCharges);

                                //Bug : 3014,3076
                                hdfOtherChargesPrev.Value = (finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.FIN_PAYMENT_VND_TRX_MPG.Where(r => r.FIN_PAYMENT_VND_HDR.PVH_DEL_STATUS == 0 && r.FIN_PAYMENT_VND_HDR.PVH_BOUNCED == 0).Sum(sm => sm.PVM_OTHER_AMOUNT) + finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_SHIP_CHARGE_DED - prevotherCharges).ToString();

                                //decimal balpay = balToPay;
                                //if (finPaymentTrxMpgList[e.Row.RowIndex].FIN_PAYMENT_VND_ALCN_DTL != null && finPaymentTrxMpgList[e.Row.RowIndex].FIN_PAYMENT_VND_ALCN_DTL.Count > 0)
                                //{
                                //    balpay = balToPay - finPaymentTrxMpgList[e.Row.RowIndex].FIN_PAYMENT_VND_ALCN_DTL.Sum(s => s.PAD_AMOUNT);
                                //}
                                //balToPay = balpay <= 0 ? 0 : balpay;

                                lblBaltopay.Text = String.Format("{0:c}", balToPay);
                                // hdfBaltopay.Value = balToPay.ToString();
                                hdfBaltopay.Value = balToPayWithoutAdjn.ToString();
                                lblBaltopay.ToolTip = String.Format("{0:c}", balToPay);

                                txtPayNow.Text = Math.Round(finPaymentTrxMpgList[e.Row.RowIndex].PVM_PAID_AMOUNT,
                                    Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                                txtPayNow.ToolTip = Math.Round(finPaymentTrxMpgList[e.Row.RowIndex].PVM_PAID_AMOUNT,
                                    Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                                hdfPayNow.Value = Math.Round(finPaymentTrxMpgList[e.Row.RowIndex].PVM_PAID_AMOUNT,
                                    Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                                lnkRemove.CommandArgument = finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_PK.ToString();
                                //lnkAllocation.Visible = true;
                                //lblAdjAmount.Text = String.Format("{0:c}", finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_CN_TC - finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_DN_TC);
                                lblAdjAmount.Text = lblAdjAmount.ToolTip = String.Format("{0:c}", finPaymentTrxMpgList[e.Row.RowIndex].FIN_PAYMENT_VND_ALCN_DTL.Sum(s => s.PAD_AMOUNT));
                                hdfInitialBaltoPay.Value = Math.Round(balToPay, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                                //lblTotalTaxAmount.Text = Math.Round(finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_TAX_TC.ToString() == string.Empty ? 0 : finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_TAX_TC,
                                //    Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                                //lblTotalTaxAmount.ToolTip = lblTotalTaxAmount.Text;

                                txtAdjustments.Text = Math.Round(finPaymentTrxMpgList[e.Row.RowIndex].PVM_DISC_AMOUNT,
                                   Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                                txtAdjustments.ToolTip = txtAdjustments.Text;

                                if (Convert.ToInt32(hdfCategory.Value) == (int)POInvoiceCategory.Advanced)
                                {
                                    hdfTotalTax.Value = lblTotalTax.Text = Math.Round(finPaymentTrxMpgList[e.Row.RowIndex].PVM_TAX_AMOUNT,
                                         Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                                }
                                else
                                {
                                    hdfTotalTax.Value = lblTotalTax.Text = Math.Round(taxamt, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                                }
                                lblTotalTax.ToolTip = lblTotalTax.Text;

                                decimal InvCNAmount = 0;
                                InvCNAmount = finPaymentTrxMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.FIN_CRDR_NOTE_MPG.Where(r => !r.FIN_CRDR_NOTE_HDR.CDH_IS_DELETED && r.FIN_CRDR_NOTE_HDR.CDH_STATUS != 0 && r.FIN_CRDR_NOTE_HDR.CDH_TYPE == (byte)DebitCreditModeEnum.CREDIT).Sum(sm => sm.CDM_AMOUNT);
                                InvCNAmount = Math.Round(InvCNAmount, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                if (InvCNAmount > 0 && !IsCreditExist)
                                    IsCreditExist = true;
                                if (lblCnAmount != null)
                                    lblCnAmount.Text = lblCnAmount.ToolTip = String.Format("{0:c}", InvCNAmount);


                                //fill WHT popup
                                txtCustomerTxtWHT.Text = ERP.Utilities.CommonFunctions.GetShortString(finPaymentTrxMpgList[0].FIN_INVOICE_VND_HDR.PUR_VENDOR_MST.VEN_NAME2, 300);
                                txtCustomerTxtWHT.ToolTip = finPaymentTrxMpgList[0].FIN_INVOICE_VND_HDR.PUR_VENDOR_MST.VEN_NAME2;
                                hdfvendorWHTPK.Value = finPaymentTrxMpgList[0].FIN_INVOICE_VND_HDR.PUR_VENDOR_MST.VEN_PK.ToString();
                                txtpartyads.Text = finPaymentTrxMpgList[0].FIN_INVOICE_VND_HDR.PUR_VENDOR_MST.VEN_ADDR3;
                                txtTaxid.Text = finPaymentTrxMpgList[0].FIN_INVOICE_VND_HDR.PUR_VENDOR_MST.VEN_TIN;

                                //Cedit note allocated amount
                                decimal CrdrAlcnAmnt = 0;
                                if (finPaymentTrxMpgList[e.Row.RowIndex].FIN_PAYMENT_VND_CRDR_MPG != null && finPaymentTrxMpgList[e.Row.RowIndex].FIN_PAYMENT_VND_CRDR_MPG.Count > 0)
                                {
                                    CrdrAlcnAmnt = finPaymentTrxMpgList[e.Row.RowIndex].FIN_PAYMENT_VND_CRDR_MPG.Sum(r => r.PNM_PAID_AMOUNT + r.PNM_ADJ_AMOUNT);
                                }
                                if (lblCrdrAlcnAmount != null)
                                    lblCrdrAlcnAmount.Text = lblCrdrAlcnAmount.ToolTip = String.Format("{0:c}", CrdrAlcnAmnt);

                                #region To bind changed data back
                                if (PaymentInvDetList != null && PaymentInvDetList.Count > 0)
                                {
                                    List<PaymentInvoiceDetails> PymntInvdetLst = PaymentInvDetList.Where(r => r.PVM_INVOICE_HDR == finPaymentTrxMpgList[e.Row.RowIndex].PVM_INVOICE_HDR).ToList();
                                    if (PymntInvdetLst != null && PymntInvdetLst.Count > 0)
                                    {
                                        lblAdjAmount.Text = lblAdjAmount.ToolTip = String.Format("{0:c}", PymntInvdetLst[0].PVM_ADJ_AMOUNT);
                                        lblBaltopay.Text = lblBaltopay.ToolTip = String.Format("{0:c}", PymntInvdetLst[0].PVM_BALANCE_TO_PAY);
                                        txtPayNow.Text = txtPayNow.ToolTip = PymntInvdetLst[0].PVM_PAYNOW_AMOUNT.ToString();
                                        lblCrdrAlcnAmount.Text = lblCrdrAlcnAmount.ToolTip = String.Format("{0:c}", PymntInvdetLst[0].PVM_CRDR_AMOUNT);
                                        lblTotalTax.Text = lblTotalTax.ToolTip = String.Format("{0:c}", PymntInvdetLst[0].PVM_TAX_AMOUNT);
                                        hdfTotalTax.Value = PymntInvdetLst[0].PVM_TAX_AMOUNT.ToString();
                                        txtAdjustments.Text = txtAdjustments.ToolTip = PymntInvdetLst[0].PVM_REDUCTION_AMOUNT.ToString();
                                    }
                                }
                                #endregion
                            }
                            else // new invoice added
                            {
                                #region New added invoice. Bind data from Invoice table
                                NewInvPk = finPaymentTrxMpgList[e.Row.RowIndex].PVM_INVOICE_HDR.Value;
                                GetFieldValues(ControlsEnum.INVOICEDETAILS);
                                if (FinInvoiceVndObj != null)
                                {

                                    hdfFavourof.Value = txtFavourof.Text = FinInvoiceVndObj.PUR_VENDOR_MST.VEN_NAME;
                                    lblCustomerTxt.Text = ERP.Utilities.CommonFunctions.GetShortString(FinInvoiceVndObj.PUR_VENDOR_MST.VEN_NAME, 50);
                                    lblCustomerTxt.ToolTip = FinInvoiceVndObj.PUR_VENDOR_MST.VEN_NAME;
                                    hdfCusPK.Value = FinInvoiceVndObj.IVH_VENDOR.ToString();

                                    decimal lineItemTax = FinInvoiceVndObj.FIN_INVOICE_VND_DTL.Sum(ss => ss.VID_TAX);
                                    decimal lineItemDiscount = FinInvoiceVndObj.FIN_INVOICE_VND_DTL.Sum(ss => ss.VID_DISCOUNT);

                                    hdfCategory.Value = FinInvoiceVndObj.IVH_CATEGORY.ToString();
                                    hdfInvoiceType.Value = FinInvoiceVndObj.IVH_TYPE.ToString();
                                    hdfGroup.Value = FinInvoiceVndObj.IVH_GROUP.ToString();
                                    hdfTotalAmt.Value = (FinInvoiceVndObj.IVH_AMOUNT_TC - FinInvoiceVndObj.IVH_DISCOUNT_TC + lineItemDiscount).ToString();
                                    hdfTaxAmt.Value = FinInvoiceVndObj.IVH_TAX_TC.ToString() == string.Empty ? (0 + lineItemTax).ToString() : (FinInvoiceVndObj.IVH_TAX_TC + lineItemTax).ToString();
                                    hdfInvoicePK.Value = FinInvoiceVndObj.IVH_PK.ToString();
                                    hdfPaymentMpgPK.Value = "0";
                                    lblInvoiceNo.Text = FinInvoiceVndObj.IVH_NO;
                                    lnkInvoiceNo.Text = FinInvoiceVndObj.IVH_NO;
                                    lblInvoiceNo.ToolTip = FinInvoiceVndObj.IVH_NO;
                                    lblInvoiceNo.ToolTip = GetLocalResourceObject("DueDate").ToString() + " : " + FinInvoiceVndObj.IVH_DATE.ToString(Resources.Constants.DateFormatShort);

                                    lnkInvoiceNo.ToolTip = GetLocalResourceObject("DueDate").ToString() + " : " + FinInvoiceVndObj.IVH_DATE.ToString(Resources.Constants.DateFormatShort);
                                    lnkInvoiceNo.CommandArgument = FinInvoiceVndObj.IVH_PK.ToString();
                                    //lblInvoiceDate.Text = FinInvoiceVndObj.IVH_DATE.ToString(Resources.Constants.DateFormatShort);
                                    //lblInvoiceDate.ToolTip = FinInvoiceVndObj.IVH_DATE.ToString(Resources.Constants.DateFormatShort);
                                    //lblInvoiceDate.Text = FinInvoiceVndObj.IVH_DATE_PAY_BY.ToString(Resources.Constants.DateFormatShort);
                                    //lblInvoiceDate.ToolTip = FinInvoiceVndObj.IVH_DATE_PAY_BY.ToString(Resources.Constants.DateFormatShort);

                                    lblVendorInv.Text = ERP.Utilities.CommonFunctions.GetShortString(FinInvoiceVndObj.PUR_VENDOR_MST.VEN_NAME, 10);
                                    lblVendorInv.ToolTip = FinInvoiceVndObj.PUR_VENDOR_MST.VEN_NAME;
                                    lblVendInvNo.Text = FinInvoiceVndObj.IVH_VENDOR_INV_NO;
                                    lblVendInvNo.ToolTip = FinInvoiceVndObj.IVH_VENDOR_INV_NO;
                                    lblInvCurrency.Text = FinInvoiceVndObj.ADM_CURRENCY_MST1.CUR_CODE;
                                    lblInvCurrency.ToolTip = FinInvoiceVndObj.ADM_CURRENCY_MST1.CUR_CODE;

                                    //   GrossAmount  Setting

                                    if (FinInvoiceVndObj.IVH_CATEGORY == (int)POInvoiceCategory.Advanced)
                                    {
                                        lblGrossAmount.Text = String.Format("{0:c}", FinInvoiceVndObj.IVH_AMOUNT_TC);
                                        hdfGrossAmt.Value = FinInvoiceVndObj.IVH_AMOUNT_TC.ToString();
                                        lblGrossAmount.ToolTip = String.Format("{0:c}", FinInvoiceVndObj.IVH_AMOUNT_TC);
                                    }
                                    else
                                    {
                                        decimal GrossAmount = 0;
                                        if (FinInvoiceVndObj.FIN_INVOICE_VND_DTL != null)
                                        {
                                            GrossAmount += (decimal)FinInvoiceVndObj.FIN_INVOICE_VND_DTL.Sum(a => a.VID_AMOUNT);
                                        }
                                        lblGrossAmount.Text = String.Format("{0:c}", GrossAmount);
                                        hdfGrossAmt.Value = GrossAmount.ToString();
                                        lblGrossAmount.ToolTip = String.Format("{0:c}", GrossAmount);
                                    }
                                    //End


                                    decimal tax = FinInvoiceVndObj.IVH_TAX_TC;
                                    decimal disc = FinInvoiceVndObj.IVH_DISCOUNT_TC;
                                    if (FinInvoiceVndObj.FIN_INVOICE_VND_DTL != null)
                                    {
                                        foreach (FIN_INVOICE_VND_DTL dtlObj in FinInvoiceVndObj.FIN_INVOICE_VND_DTL)
                                        {
                                            if (dtlObj.FIN_INVOICE_VND_TAX_DTL != null)
                                                //tax += (decimal)dtlObj.FIN_INVOICE_VND_TAX_DTL.Sum(aa => aa.VTL_TAX_AMT);
                                                tax += (decimal)dtlObj.FIN_INVOICE_VND_TAX_DTL.Where(aa => aa.VTL_TAX_CATEGORY == 1).Sum(aa => aa.VTL_TAX_AMT);
                                            disc += (decimal)dtlObj.FIN_INVOICE_VND_TAX_DTL.Where(aa => aa.VTL_TAX_CATEGORY == 3).Sum(aa => aa.VTL_TAX_AMT);
                                        }
                                    }
                                    hdfTaxHdrDtlAmt.Value = tax.ToString();
                                    //lblTax.ToolTip = lblTax.Text = String.Format("{0:c}", tax);
                                    lblTax.ToolTip = lblTax.Text = String.Format("{0:c}", Convert.ToDecimal(FinInvoiceVndObj.IVH_TAX_TC.ToString() == string.Empty ? (0 + lineItemTax).ToString() : (FinInvoiceVndObj.IVH_TAX_TC + lineItemTax).ToString()));
                                    lblDiscount.Text = String.Format("{0:c}", disc);
                                    lblDiscount.ToolTip = String.Format("{0:c}", disc);
                                    lblTotalAmount.Text = String.Format("{0:c}", FinInvoiceVndObj.IVH_AMOUNT_NET_TC);
                                    hdfPayable.Value = (FinInvoiceVndObj.IVH_AMOUNT_NET_TC).ToString();
                                    lblTotalAmount.ToolTip = String.Format("{0:c}", FinInvoiceVndObj.IVH_AMOUNT_NET_TC);
                                    //Adjustment amount not included in paid amount
                                    //lblPaid.Text = String.Format("{0:c}", FinInvoiceVndObj.IVH_AMOUNT_PAID_TC);
                                    //lblPaid.ToolTip = String.Format("{0:c}", FinInvoiceVndObj.IVH_AMOUNT_PAID_TC);
                                    lblPaid.Text = lblPaid.ToolTip = String.Format("{0:c}", FinInvoiceVndObj.IVH_AMOUNT_PAID_TC + FinInvoiceVndObj.IVH_AMOUNT_DN_TC);

                                    FinPayVndTrxMpgList = null;
                                    FinPayVndTrxMpgList = FinInvoiceVndObj.FIN_PAYMENT_VND_TRX_MPG != null ? FinInvoiceVndObj.FIN_PAYMENT_VND_TRX_MPG.ToList() : null;
                                    decimal balOtherCharges = 0;
                                    decimal balToPay = 0;
                                    decimal prevOtherCharges = 0;
                                    if ((FinPayVndTrxMpgList != null && FinPayVndTrxMpgList.Count > 1) || FinPayVndTrxMpgList == null || FinPayVndTrxMpgList.Count == 0)
                                    {
                                        balToPay = (FinInvoiceVndObj.IVH_AMOUNT_NET_TC) -
                                                         FinInvoiceVndObj.IVH_AMOUNT_PAID_TC +
                                                         FinInvoiceVndObj.IVH_AMOUNT_CN_TC -
                                                         FinInvoiceVndObj.IVH_AMOUNT_DN_TC;
                                        //prevOtherCharges = FinPayVndTrxMpgList[0].FIN_INVOICE_VND_HDR.IVH_DEL_STATUS == 0 ? FinPayVndTrxMpgList[0].PVM_OTHER_AMOUNT : 0;
                                    }
                                    else if (FinPayVndTrxMpgList != null && FinPayVndTrxMpgList.Count == 1)
                                    {
                                        balToPay = (FinInvoiceVndObj.IVH_AMOUNT_NET_TC) -
                                                         //(FinPayVndTrxMpgList[0].FIN_INVOICE_VND_HDR.IVH_DEL_STATUS ==0 ? FinPayVndTrxMpgList[0].PVM_PAID_AMOUNT:0) +
                                                         FinInvoiceVndObj.IVH_AMOUNT_PAID_TC +
                                                         FinInvoiceVndObj.IVH_AMOUNT_CN_TC -
                                                         FinInvoiceVndObj.IVH_AMOUNT_DN_TC;
                                        prevOtherCharges = FinPayVndTrxMpgList[0].FIN_INVOICE_VND_HDR.IVH_DEL_STATUS == 0 ? FinPayVndTrxMpgList[0].PVM_OTHER_AMOUNT : 0;
                                    }

                                    decimal otherChargesPrev = 0;
                                    //OtherCharges
                                    long? IVH_PK = FinInvoiceVndObj.IVH_PK;
                                    if (FinInvoiceVndObj.FIN_PAYMENT_VND_TRX_MPG != null)
                                    {
                                        otherChargesPrev = (FinInvoiceVndObj.FIN_PAYMENT_VND_TRX_MPG.Where(pp => pp.PVM_INVOICE_HDR == IVH_PK && pp.FIN_PAYMENT_VND_HDR.PVH_DEL_STATUS == 0).ToList()).Sum(pv => pv.PVM_OTHER_AMOUNT);
                                    }
                                    decimal otherCharges = FinInvoiceVndObj.IVH_SHIP_CHARGE;
                                    lblOtherCharges.Text = String.Format("{0:c}", otherCharges);
                                    lblOtherCharges.ToolTip = String.Format("{0:c}", otherCharges);
                                    otherCharges -= otherChargesPrev;
                                    otherCharges = otherCharges < 0 ? 0 : otherCharges;
                                    //txtOtherCharges.Text = String.Format("{0:c}", otherCharges);
                                    //txtOtherCharges.ToolTip = String.Format("{0:c}", otherCharges);
                                    //For resolving excess payment of other charge.
                                    //  txtOtherCharges.Text = Math.Round(otherCharges, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                                    decimal OtherChargeDeduct = FinInvoiceVndObj.IVH_SHIP_CHARGE_DED;
                                    txtOtherCharges.Text = Math.Round((otherCharges - (OtherChargeDeduct > 0 ? OtherChargeDeduct : 0)), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                                    txtOtherCharges.ToolTip = String.Format("{0:c}", otherCharges);

                                    //Bug : 3014,3076
                                    //hdfOtherChargesPrev.Value = otherChargesPrev.ToString();
                                    hdfOtherChargesPrev.Value = (otherChargesPrev + FinInvoiceVndObj.IVH_SHIP_CHARGE_DED).ToString();

                                    //decimal balToPay = FinInvoiceVndObj.IVH_AMOUNT_NET_TC -
                                    //                  FinInvoiceVndObj.IVH_AMOUNT_PAID_TC +
                                    //                  FinInvoiceVndObj.IVH_AMOUNT_CN_TC -
                                    //                  FinInvoiceVndObj.IVH_AMOUNT_DN_TC;

                                    // decimal balToPay = FinInvoiceVndObj.IVH_AMOUNT_NET_TC - FinInvoiceVndObj.IVH_AMOUNT_PAID_TC;
                                    lblBaltopay.Text = String.Format("{0:c}", balToPay);
                                    hdfBaltopay.Value = balToPay.ToString();
                                    lblBaltopay.ToolTip = String.Format("{0:c}", balToPay);
                                    balToPay = balToPay < 0 ? 0 : balToPay;
                                    txtPayNow.Text = Math.Round(balToPay, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                                    txtPayNow.ToolTip = Math.Round(balToPay, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                                    hdfPayNow.Value = Math.Round(balToPay, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                                    lnkRemove.CommandArgument = FinInvoiceVndObj.IVH_PK.ToString();
                                    lnkAllocation.Visible = (POGroup != POInvoiceGroup.Expense);//vcmPayNow.Enabled =
                                    //lblAdjAmount.Text = String.Format("{0:c}", FinInvoiceVndObj.IVH_AMOUNT_CN_TC - FinInvoiceVndObj.IVH_AMOUNT_DN_TC);
                                    lblAdjAmount.Text = lblAdjAmount.ToolTip = GetFormattedCurrency(0);
                                    //lblTotalTaxAmount.Text = String.Format("{0:c}", FinInvoiceVndObj.IVH_TAX_TC.ToString() == string.Empty ? "0" : FinInvoiceVndObj.IVH_TAX_TC.ToString());
                                    //lblTotalTaxAmount.ToolTip = lblTotalTaxAmount.Text;
                                    hdfInitialBaltoPay.Value = Math.Round(balToPay, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();

                                    txtAdjustments.Text = GetFormattedCurrency(0);
                                    txtAdjustments.ToolTip = txtAdjustments.Text;

                                    if (Convert.ToInt32(hdfCategory.Value) == (int)POInvoiceCategory.Advanced)
                                    {
                                        //taxpercentage = Convert.ToDecimal(hdfTaxAmt.Value) / (Convert.ToDecimal(FinInvoiceVndObj.IVH_AMOUNT_TC - FinInvoiceVndObj.IVH_DISCOUNT_TC) == 0 ? 1 : Convert.ToDecimal(FinInvoiceVndObj.IVH_AMOUNT_TC - FinInvoiceVndObj.IVH_DISCOUNT_TC));
                                        taxpercentage = Convert.ToDecimal(hdfTaxHdrDtlAmt.Value) / (Convert.ToDecimal(FinInvoiceVndObj.IVH_AMOUNT_TC - FinInvoiceVndObj.IVH_DISCOUNT_TC) == 0 ? 1 : Convert.ToDecimal(FinInvoiceVndObj.IVH_AMOUNT_TC - FinInvoiceVndObj.IVH_DISCOUNT_TC));
                                        basevalue = (txtPayNow.Text != string.Empty ? Convert.ToDecimal(txtPayNow.Text) : 0) / (1 + taxpercentage);
                                        taxamt = (basevalue - (txtPayNow.Text != string.Empty ? Convert.ToDecimal(txtPayNow.Text) : 0));
                                    }
                                    lblTotalTax.Text = Math.Round(taxamt, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                                    lblTotalTax.ToolTip = lblTotalTax.Text;
                                    hdfTotalTax.Value = Math.Round(taxamt, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();


                                    //GetFieldValues(ControlsEnum.VENDOR);
                                    if (FinInvoiceVndObj.PUR_VENDOR_MST != null)
                                    {
                                        if (FinInvoiceVndObj.PUR_VENDOR_MST.VEN_WHT_TAX != null)
                                        {
                                            whtTaxpk = Convert.ToInt32(FinInvoiceVndObj.PUR_VENDOR_MST.VEN_WHT_TAX);
                                            //ddlWHTAccount.SelectedIndex = Convert.ToInt32(ddlWHTAccount.Items.IndexOf(ddlWHTAccount.Items.FindByValue(FinInvoiceVndObj.PUR_VENDOR_MST.VEN_WHT_TAX.ToString())));
                                            //chkVendorforpayemnt.Checked = FinInvoiceVndObj.PUR_VENDOR_MST.VEN_PAY_FOR_VENDOR != null ? Convert.ToBoolean(FinInvoiceVndObj.PUR_VENDOR_MST.VEN_PAY_FOR_VENDOR) : false;
                                            //GetFieldValues(ControlsEnum.VENDORACCOUNTTAX);
                                            //SetFieldValues(ControlsEnum.VENDORACCOUNTTAX);
                                            trVendorAccount.Style.Add("display", "");
                                        }
                                        else
                                        {
                                            trVendorAccount.Style.Add("display", "none");
                                        }
                                    }
                                    else
                                    {
                                        trVendorAccount.Style.Add("display", "none");
                                    }

                                    decimal InvCNAmount = 0;
                                    InvCNAmount = FinInvoiceVndObj.FIN_CRDR_NOTE_MPG.Where(r => !r.FIN_CRDR_NOTE_HDR.CDH_IS_DELETED && r.FIN_CRDR_NOTE_HDR.CDH_STATUS != 0 && r.FIN_CRDR_NOTE_HDR.CDH_TYPE == (byte)DebitCreditModeEnum.CREDIT).Sum(sm => sm.CDM_AMOUNT);
                                    InvCNAmount = Math.Round(InvCNAmount, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                    if (InvCNAmount > 0 && !IsCreditExist)
                                        IsCreditExist = true;
                                    if (lblCnAmount != null)
                                        lblCnAmount.Text = lblCnAmount.ToolTip = String.Format("{0:c}", InvCNAmount);


                                    //fill WHT popup
                                    txtCustomerTxtWHT.Text = ERP.Utilities.CommonFunctions.GetShortString(FinInvoiceVndObj.PUR_VENDOR_MST.VEN_NAME2, 300);
                                    txtCustomerTxtWHT.ToolTip = FinInvoiceVndObj.PUR_VENDOR_MST.VEN_NAME2;
                                    hdfvendorWHTPK.Value = FinInvoiceVndObj.PUR_VENDOR_MST.VEN_PK.ToString();
                                    txtpartyads.Text = FinInvoiceVndObj.PUR_VENDOR_MST.VEN_ADDR3;
                                    txtTaxid.Text = FinInvoiceVndObj.PUR_VENDOR_MST.VEN_TIN;
                                    // END fill WHT popup
                                    SetVendorPayment();

                                    //Cedit note allocated amount                           
                                    //lblCrdrAlcnAmount.Text=lblCrdrAlcnAmount.ToolTip=   String.Format("{0:c}", 0);

                                    //Cedit note allocated amount
                                    decimal CrdrAlcnAmnt = 0;
                                    if (PaymentCrdrList != null && PaymentCrdrList.Count > 0)
                                    {
                                        CrdrAlcnAmnt = PaymentCrdrList.Where(r => r.PNM_INVOICE_HDR == FinInvoiceVndObj.IVH_PK).Sum(sm => sm.PNM_PAID_AMOUNT + sm.PNM_ADJ_AMOUNT);
                                    }
                                    if (lblCrdrAlcnAmount != null)
                                        lblCrdrAlcnAmount.Text = lblCrdrAlcnAmount.ToolTip = String.Format("{0:c}", CrdrAlcnAmnt);

                                    #region To bind changed data back 
                                    if (PaymentInvDetList != null && PaymentInvDetList.Count > 0)
                                    {
                                        List<PaymentInvoiceDetails> PymntInvdetLst = PaymentInvDetList.Where(r => r.PVM_INVOICE_HDR == FinInvoiceVndObj.IVH_PK).ToList();
                                        if (PymntInvdetLst != null && PymntInvdetLst.Count > 0)
                                        {
                                            lblAdjAmount.Text = lblAdjAmount.ToolTip = String.Format("{0:c}", PymntInvdetLst[0].PVM_ADJ_AMOUNT);
                                            lblBaltopay.Text = lblBaltopay.ToolTip = String.Format("{0:c}", PymntInvdetLst[0].PVM_BALANCE_TO_PAY);
                                            txtPayNow.Text = txtPayNow.ToolTip = PymntInvdetLst[0].PVM_PAYNOW_AMOUNT.ToString();
                                            lblCrdrAlcnAmount.Text = lblCrdrAlcnAmount.ToolTip = String.Format("{0:c}", PymntInvdetLst[0].PVM_CRDR_AMOUNT);
                                            lblTotalTax.Text = lblTotalTax.ToolTip = String.Format("{0:c}", PymntInvdetLst[0].PVM_TAX_AMOUNT);
                                            hdfTotalTax.Value = PymntInvdetLst[0].PVM_TAX_AMOUNT.ToString();
                                            txtAdjustments.Text = txtAdjustments.ToolTip = PymntInvdetLst[0].PVM_REDUCTION_AMOUNT.ToString();
                                        }
                                    }
                                    #endregion
                                }
                                #endregion
                            }
                        }

                        txtPayNow.Focus();

                        //Visibility of adjn Coloumns
                        if ((Convert.ToInt16(hdfGroup.Value) == Convert.ToInt16(POInvoiceGroup.Expense)) && ShowExpenseCrDr)
                        {
                            grdInvoiceList.Columns[12].Visible = true;
                            grdInvoiceList.Columns[13].Visible = true;
                        }
                        else if ((Convert.ToInt32(hdfCategory.Value) == (int)SalesInvoiceCategory.Advanced
                            || Convert.ToInt16(hdfGroup.Value) == Convert.ToInt16(POInvoiceGroup.AgtInvoice)
                            || Convert.ToInt16(hdfGroup.Value) == Convert.ToInt16(POInvoiceGroup.Expense)) && !ShowAdjColumn)
                        {
                            grdInvoiceList.Columns[12].Visible = false;
                            grdInvoiceList.Columns[13].Visible = false;
                        }
                        else
                        {
                            grdInvoiceList.Columns[12].Visible = true;
                            grdInvoiceList.Columns[13].Visible = true;
                        }

                        if ((Convert.ToInt16(hdfGroup.Value) == Convert.ToInt16(POInvoiceGroup.AgtInvoice)) && !ShowAdjColumn)
                        {
                            //grdInvoiceList.Columns[11].Visible = false;
                            //grdInvoiceList.Columns[12].Visible = false;
                            grdInvoiceList.Columns[16].Visible = false;
                        }
                        else
                        {
                            //grdInvoiceList.Columns[11].Visible = true;
                            //grdInvoiceList.Columns[12].Visible = true;
                            grdInvoiceList.Columns[16].Visible = true;
                        }

                        //Visibility for credit amount columns
                        if ((Convert.ToInt16(hdfGroup.Value) == Convert.ToInt16(POInvoiceGroup.Goods)
                            || Convert.ToInt16(hdfGroup.Value) == Convert.ToInt16(POInvoiceGroup.Services)
                            || (Convert.ToInt16(hdfGroup.Value) == Convert.ToInt16(POInvoiceGroup.Expense) && ShowExpenseCrDr))
                            && Convert.ToInt32(hdfCategory.Value) != (int)POInvoiceCategory.Advanced)
                        {
                            if (IsCreditExist)
                            {
                                grdInvoiceList.Columns[10].Visible = true;
                                grdInvoiceList.Columns[17].Visible = true;
                                grdInvoiceList.Columns[18].Visible = true;
                            }
                            else
                            {
                                grdInvoiceList.Columns[10].Visible = false;
                                grdInvoiceList.Columns[17].Visible = false;
                                grdInvoiceList.Columns[18].Visible = false;
                            }
                            //grdInvoiceList.Columns[16].Visible = true;
                            //grdInvoiceList.Columns[17].Visible = true;                           
                        }
                        else
                        {
                            grdInvoiceList.Columns[10].Visible = false;
                            grdInvoiceList.Columns[17].Visible = false;
                            grdInvoiceList.Columns[18].Visible = false;
                        }
                    }
                    else if (((GridView)sender).ID == "grdPOPaymentHdr")
                    {
                        if (finPaymentVndHdrList != null && finPaymentVndHdrList.Count > 0)
                        {
                            Label lblModeofPayment = e.Row.FindControl("lblModeofPayment") as Label;
                            Label lblBankName = e.Row.FindControl("lblBankName") as Label;
                            HiddenField hdfPaymentID = e.Row.FindControl("hdfPaymentID") as HiddenField;
                            HiddenField hdfPDC = e.Row.FindControl("hdfPDC") as HiddenField;
                            HiddenField hdfMode = e.Row.FindControl("hdfMode") as HiddenField;
                            Button btnPDCFlag = e.Row.FindControl("btnPDCFlag") as Button;
                            Button btnPDCReturn = e.Row.FindControl("btnPDCReturn") as Button;

                            LinkButton lnkInvnos = e.Row.FindControl("lnkInvnos") as LinkButton;
                            HiddenField hdfInvType = e.Row.FindControl("hdfInvType") as HiddenField;
                            HiddenField hdfinvPK = e.Row.FindControl("hdfinvPK") as HiddenField;
                            HiddenField hdfinvCategory = e.Row.FindControl("hdfinvCategory") as HiddenField;
                            HiddenField hdfinvCategoryType = e.Row.FindControl("hdfinvCategoryType") as HiddenField;
                            FIN_PAYMENT_VND_HDR PaymentHdr = finPaymentVndHdrList.SingleOrDefault(pmnt => pmnt.PVH_PK == Convert.ToInt64(hdfPaymentID.Value));
                            if (PaymentHdr != null)
                            {
                                #region Payment Modes
                                string PaymentModes = string.Empty;
                                var paymentModeList = (from c in PaymentHdr.FIN_PAYMENT_VND_MODE_DTL
                                                       join d in PaymentModeConfigMstList
                                                       on c.PDM_MODE equals d.CFG_VALUE
                                                       select new
                                                       {
                                                           CFG_DATA = d.CFG_DATA
                                                       }).Distinct().ToList();
                                if (paymentModeList != null && paymentModeList.Count() > 0)
                                {
                                    paymentModeList.ForEach(dtl =>
                                    {
                                        PaymentModes += dtl.CFG_DATA + ",";
                                    });
                                    PaymentModes = PaymentModes.TrimEnd(',');
                                    lblModeofPayment.Text = CommonFunctions.GetShortString(PaymentModes, 7);
                                    lblModeofPayment.ToolTip = PaymentModes;
                                }

                                if (PaymentHdr.FIN_PAYMENT_VND_MODE_DTL.Where(r => r.PDM_MODE == (byte)PaymentModeEnum.CHEQUE).Count() > 0)
                                    hdfMode.Value = Convert.ToByte(PaymentModeEnum.CHEQUE).ToString();
                                else
                                    hdfMode.Value = "0";

                                #endregion

                                #region Bank Names
                                string BankNames = string.Empty;
                                var BankNameLst = (from c in PaymentHdr.FIN_PAYMENT_VND_MODE_DTL
                                                   where c.PDM_BANK.HasValue
                                                   select new
                                                   {
                                                       CBM_NAME = c.FIN_CASH_BANK_MST.CBM_NAME
                                                   }).Distinct().ToList();
                                if (BankNameLst != null && BankNameLst.Count() > 0)
                                {
                                    BankNameLst.ForEach(dtl =>
                                    {
                                        BankNames += dtl.CBM_NAME + ",";
                                    });
                                    BankNames = BankNames.TrimEnd(',');
                                    lblBankName.Text = CommonFunctions.GetShortString(BankNames, 27);
                                    lblBankName.ToolTip = BankNames;
                                }
                                #endregion

                                #region PDC Flag Settings
                                short PdcStatus = 0;
                                short BouncedStatus = 0;
                                List<FIN_PAYMENT_VND_MODE_DTL> ModesList = PaymentHdr.FIN_PAYMENT_VND_MODE_DTL.Where(r => r.PDM_PDC >= 1).ToList();
                                List<FIN_PAYMENT_VND_MODE_DTL> ModesListBounced = PaymentHdr.FIN_PAYMENT_VND_MODE_DTL.Where(r => r.PDM_BOUNCED >= 1).ToList();
                                if (ModesList != null && ModesList.Count() > 0)
                                {
                                    PdcStatus = ModesList[0].PDM_PDC;
                                    //BouncedStatus = ModesList[0].PDM_BOUNCED;
                                }
                                if (ModesListBounced != null && ModesListBounced.Count() > 0)
                                {
                                    BouncedStatus = 1;
                                }

                                hdfPDC.Value = PdcStatus.ToString();
                                btnPDCFlag.CssClass = (PdcStatus != 0 ? (((PdcStatus == 1) || (PdcStatus == 3)) ? "flaggrey-icon" : "flaggreen-icon") : "");
                                btnPDCFlag.ToolTip = (PdcStatus != 0 ? (((PdcStatus == 1) || (PdcStatus == 3)) ? GetLocalResourceObject("PDC_Cheque").ToString() : GetLocalResourceObject("Cheque_Reversed").ToString()) : "");
                                btnPDCFlag.Visible = (BouncedStatus != 0 ? false : (PdcStatus != 0 ? true : false));

                                btnPDCReturn.CssClass = (BouncedStatus != 0 ? "return-icon" : "");
                                btnPDCReturn.ToolTip = (BouncedStatus != 0 ? GetLocalResourceObject("PDC_Return").ToString() : "");
                                btnPDCReturn.Visible = (BouncedStatus != 0 ? true : false);
                                #endregion

                                #region Invoice Number
                                string invoices = string.Empty;
                                int invCount = 0;
                                var invnos = from c in PaymentHdr.FIN_PAYMENT_VND_TRX_MPG select c.FIN_INVOICE_VND_HDR.IVH_NO;
                                int invType = PaymentHdr.FIN_PAYMENT_VND_TRX_MPG.ToList()[0].FIN_INVOICE_VND_HDR.IVH_GROUP;
                                int invPK = Convert.ToInt32(PaymentHdr.FIN_PAYMENT_VND_TRX_MPG.ToList()[0].FIN_INVOICE_VND_HDR.IVH_PK);
                                int invCategory = PaymentHdr.FIN_PAYMENT_VND_TRX_MPG.ToList()[0].FIN_INVOICE_VND_HDR.IVH_CATEGORY;
                                int invCategoryType = PaymentHdr.FIN_PAYMENT_VND_TRX_MPG.ToList()[0].FIN_INVOICE_VND_HDR.IVH_TYPE;
                                if (invnos != null && invnos.Count() > 0)
                                {
                                    foreach (string invNo in invnos)
                                    {
                                        invoices += invNo + ",";
                                        invCount++;
                                    }
                                    invoices = invoices.TrimEnd(',');
                                }
                                hdfInvType.Value = Convert.ToString(invType);
                                lnkInvnos.Text = CommonFunctions.GetShortString(invoices, 17);
                                lnkInvnos.ToolTip = invoices;
                                hdfinvPK.Value = Convert.ToString(invPK);
                                hdfinvCategory.Value = Convert.ToString(invCategory);
                                hdfinvCategoryType.Value = Convert.ToString(invCategoryType);
                                if (invCount > 1)
                                {
                                    lnkInvnos.Attributes.Add("onclick", "return false;");
                                    lnkInvnos.CssClass = "removelinkPopup";
                                }
                                else
                                {
                                    lnkInvnos.Attributes.Add("onclick", "return true;");
                                }
                                #endregion

                            }
                            //hdfPaymentMode.Value = lblModeofPayment.Text;
                            //int cfgpk = Convert.ToInt32(hdfPaymentMode.Value);
                            //GetFieldValues(ControlsEnum.PAYMODE);
                            //if (admConfigMstList != null && admConfigMstList.Count > 0)
                            //{
                            //    lblModeofPayment.Text = admConfigMstList.SingleOrDefault(con => con.CFG_VALUE == cfgpk).CFG_DATA;
                            //    lblModeofPayment.ToolTip = admConfigMstList.SingleOrDefault(con => con.CFG_VALUE == cfgpk).CFG_DATA;
                            //}

                        }

                        Button imgApproved = e.Row.FindControl("imgApproved") as Button;
                        Button imgPosted = e.Row.FindControl("imgPosted") as Button;

                        HiddenField hdfApproved = e.Row.FindControl("hdfApproved") as HiddenField;
                        HiddenField hdfPosted = e.Row.FindControl("hdfPosted") as HiddenField;

                        short appstatus = Convert.ToInt16(hdfApproved.Value);
                        if (workflowStatusList != null && workflowStatusList.Count > 0)
                        {
                            wkfStatus = workflowStatusList.SingleOrDefault(aa => aa.ASC_VALUE == appstatus);
                            if (wkfStatus != null)
                            {
                                imgApproved.ToolTip = wkfStatus.ASC_NAME;
                                imgApproved.CssClass = wkfStatus.ASC_CSS_CLASS;
                            }
                        }
                        HiddenField hdfInvGroup = e.Row.FindControl("hdfInvGroup") as HiddenField;
                        invGroup = Convert.ToInt32(hdfInvGroup.Value);
                        CurrPK = Convert.ToInt32(((HiddenField)e.Row.FindControl("hdfPaymentID")).Value);
                        GetFieldValues(ControlsEnum.FINHEADERSTATUS);
                        short status = 0;
                        if (finTrxHdrList.Count > 0)
                        {
                            status = finTrxHdrList[0].FTH_STATUS;
                        }
                        if (workflowStatusList != null && workflowStatusList.Count > 0)
                        {
                            wkfStatus = workflowStatusList.SingleOrDefault(aa => aa.ASC_VALUE == status);
                            if (wkfStatus != null)
                            {
                                if (status != 0)
                                {
                                    imgPosted.ToolTip = wkfStatus.ASC_NAME;
                                    imgPosted.CssClass = wkfStatus.ASC_CSS_CLASS;
                                }
                                else
                                {
                                    imgPosted.CssClass = GetLocalResourceObject("unposted").ToString();
                                    imgPosted.ToolTip = Resources.Captions.NotPosted;
                                }
                            }
                        }

                        //if (Convert.ToBoolean(hdfPosted.Value) == true)
                        //{
                        //    imgPosted.CssClass = GetLocalResourceObject("posted").ToString();
                        //    imgPosted.ToolTip = Resources.Captions.Posted;
                        //}
                        //else
                        //{
                        //    imgPosted.CssClass = GetLocalResourceObject("unposted").ToString();
                        //    imgPosted.ToolTip = Resources.Captions.NotPosted;
                        //}
                    }
                    else if (((GridView)sender).ID == "grdPaymentSplitAdjn")
                    {
                        Label lblCrDrNOSplit;
                        Label lblTotAmountAdjn;
                        Label lblAllocatedAdjn;
                        Label lblBalanceAdjn;
                        Label lblPageType;
                        Label lblDate;

                        TextBox txtAllocateAdjn;

                        HiddenField hdfReceiptTRXAdjnPK;
                        HiddenField hdfCrDrPK;
                        HiddenField hdfAdjnPK;
                        HiddenField hdfReceiptAdjnPK;
                        //grdPaymentSplitAdjn
                        lblCrDrNOSplit = e.Row.FindControl("lblCrDrNOAdjn") as Label;
                        lblTotAmountAdjn = e.Row.FindControl("lblTotAmountAdjn") as Label;
                        lblAllocatedAdjn = e.Row.FindControl("lblAllocatedAdjn") as Label;
                        lblBalanceAdjn = e.Row.FindControl("lblBalanceAdjn") as Label;
                        lblPageType = e.Row.FindControl("lblPageType") as Label;
                        lblDate = e.Row.FindControl("lblDate") as Label;

                        txtAllocateAdjn = e.Row.FindControl("txtAllocateAdjn") as TextBox;

                        hdfReceiptTRXAdjnPK = e.Row.FindControl("hdfReceiptTRXAdjnPK") as HiddenField;
                        hdfCrDrPK = e.Row.FindControl("hdfCrDrPK") as HiddenField;
                        hdfAdjnPK = e.Row.FindControl("hdfAdjnPK") as HiddenField;
                        hdfReceiptAdjnPK = e.Row.FindControl("hdfReceiptAdjnPK") as HiddenField;
                        hdfReceiptTRXAdjnPK.Value = PaymentMpgPK.ToString();

                        List<long?> lstInvNos = new List<long?>();
                        PaymentAdjnList.ForEach(rr =>
                        {
                            lstInvNos.Add(rr.FIN_PAYMENT_VND_TRX_MPG.PVM_INVOICE_HDR);
                        });


                        if (CrDrAdjnList != null && CrDrAdjnList.Count > 0)
                        {
                            lblCrDrNOSplit.Text = CrDrAdjnList[e.Row.RowIndex].PAA_NO;
                            lblPageType.Text = CrDrAdjnList[e.Row.RowIndex].PAA_TYPE;
                            lblDate.Text = CrDrAdjnList[e.Row.RowIndex].PAA_DATE.ToString(Resources.Constants.DateFormatShort);
                            lblTotAmountAdjn.Text = String.Format("{0:c}", Convert.ToDecimal(CrDrAdjnList[e.Row.RowIndex].PAA_AMOUNT));
                            lblAllocatedAdjn.Text = String.Format("{0:c}", Convert.ToDecimal(CrDrAdjnList[e.Row.RowIndex].PAA_AMOUNT_PAID));


                            hdfReceiptTRXAdjnPK.Value = PaymentMpgPK.ToString();
                            hdfCrDrPK.Value = CrDrAdjnList[e.Row.RowIndex].PAA_CRDRPK.ToString();
                            hdfReceiptAdjnPK.Value = CrDrAdjnList[e.Row.RowIndex].PAA_TRXPK.ToString();

                            if (PaymentAdjnList != null && PaymentAdjnList.Count > 0)//Edit before save
                            {
                                if (CrDrAdjnList[e.Row.RowIndex].PAA_CRDRPK > 0)
                                {
                                    lblBalanceAdjn.Text = lblBalanceAdjn.ToolTip = String.Format("{0:c}", Convert.ToDecimal(CrDrAdjnList[e.Row.RowIndex].PAA_AMOUNT) > 0 ? (PaymentAdjnList.Where(fd => fd.PAD_ALCN_CDH == CrDrAdjnList[e.Row.RowIndex].PAA_CRDRPK && !lstInvNos.Contains(fd.FIN_PAYMENT_VND_TRX_MPG.PVM_INVOICE_HDR)).ToList().Count > 0 ? Convert.ToDecimal(lblTotAmountAdjn.Text.Replace(",", "")) : Convert.ToDecimal(CrDrAdjnList[e.Row.RowIndex].PAA_AMOUNT_BAL)) : Convert.ToDecimal(CrDrAdjnList[e.Row.RowIndex].PAA_AMOUNT_BAL));
                                    if (PaymentAdjnList.Where(fd => fd.PAD_ALCN_CDH == CrDrAdjnList[e.Row.RowIndex].PAA_CRDRPK && fd.FIN_PAYMENT_VND_TRX_MPG.PVM_INVOICE_HDR == InvoicePK).ToList() != null && PaymentAdjnList.Where(fd => fd.PAD_ALCN_CDH == CrDrAdjnList[e.Row.RowIndex].PAA_CRDRPK && fd.FIN_PAYMENT_VND_TRX_MPG.PVM_INVOICE_HDR == InvoicePK).ToList().Count > 0)
                                    {
                                        txtAllocateAdjn.Text = PaymentAdjnList.Where(fd => fd.PAD_ALCN_CDH == CrDrAdjnList[e.Row.RowIndex].PAA_CRDRPK && fd.FIN_PAYMENT_VND_TRX_MPG.PVM_INVOICE_HDR == InvoicePK).FirstOrDefault().PAD_AMOUNT.ToString(hdfCurrencyFormat.Value);
                                    }
                                    else
                                    {
                                        txtAllocateAdjn.Text = "0.00";
                                    }
                                }
                                else
                                {
                                    lblBalanceAdjn.Text = lblBalanceAdjn.ToolTip = String.Format("{0:c}", Convert.ToDecimal(CrDrAdjnList[e.Row.RowIndex].PAA_AMOUNT_BAL) > 0 ? (PaymentAdjnList.Where(fd => fd.PAD_ALCN_PAYMENT_TRX == CrDrAdjnList[e.Row.RowIndex].PAA_TRXPK && !lstInvNos.Contains(fd.FIN_PAYMENT_VND_TRX_MPG.PVM_INVOICE_HDR)).ToList().Count > 0 ? Convert.ToDecimal(lblTotAmountAdjn.Text.Replace(",", "")) : Convert.ToDecimal(CrDrAdjnList[e.Row.RowIndex].PAA_AMOUNT_BAL)) : Convert.ToDecimal(CrDrAdjnList[e.Row.RowIndex].PAA_AMOUNT_BAL));
                                    if (PaymentAdjnList.Where(fd => fd.PAD_ALCN_PAYMENT_TRX == CrDrAdjnList[e.Row.RowIndex].PAA_TRXPK && fd.FIN_PAYMENT_VND_TRX_MPG.PVM_INVOICE_HDR == InvoicePK).ToList() != null && PaymentAdjnList.Where(fd => fd.PAD_ALCN_PAYMENT_TRX == CrDrAdjnList[e.Row.RowIndex].PAA_TRXPK && fd.FIN_PAYMENT_VND_TRX_MPG.PVM_INVOICE_HDR == InvoicePK).ToList().Count > 0)
                                    {
                                        txtAllocateAdjn.Text = PaymentAdjnList.Where(fd => fd.PAD_ALCN_PAYMENT_TRX == CrDrAdjnList[e.Row.RowIndex].PAA_TRXPK && fd.FIN_PAYMENT_VND_TRX_MPG.PVM_INVOICE_HDR == InvoicePK).FirstOrDefault().PAD_AMOUNT.ToString(hdfCurrencyFormat.Value);
                                    }
                                    else
                                    {
                                        txtAllocateAdjn.Text = "0.00";
                                    }
                                }
                            }
                            else
                            {
                                lblBalanceAdjn.Text = lblBalanceAdjn.ToolTip = String.Format("{0:c}", Convert.ToDecimal(CrDrAdjnList[e.Row.RowIndex].PAA_AMOUNT_BAL));
                                txtAllocateAdjn.Text = "0.00";//CrDrAdjnList[e.Row.RowIndex].PAA_AMOUNT_BAL.ToString()
                            }

                            totAllocateAdjn = totAllocateAdjn + Convert.ToDecimal(txtAllocateAdjn.Text);
                            totBalanceAdjn = totBalanceAdjn + Convert.ToDecimal(lblBalanceAdjn.Text.Replace(",", ""));
                        }
                        else if (FinPaymentVndAllocationList != null && FinPaymentVndAllocationList.Count > 0)
                        {
                            bool isCr = true;
                            decimal allocated = 0;
                            long PymntPK = FinPaymentVndAllocationList[e.Row.RowIndex].FIN_PAYMENT_VND_TRX_MPG.FIN_PAYMENT_VND_HDR.PVH_PK;
                            long InvPk = Convert.ToInt64(FinPaymentVndAllocationList[e.Row.RowIndex].FIN_PAYMENT_VND_TRX_MPG.PVM_INVOICE_HDR);
                            if (FinPaymentVndAllocationList[e.Row.RowIndex].PAD_ALCN_CDH == null)
                            {
                                isCr = false;
                                lblCrDrNOSplit.Text = FinPaymentVndAllocationList[e.Row.RowIndex].FIN_PAYMENT_VND_TRX_MPG1.FIN_PAYMENT_VND_HDR.PVH_NO;
                                lblPageType.Text = "PAY";
                                lblDate.Text = FinPaymentVndAllocationList[e.Row.RowIndex].FIN_PAYMENT_VND_TRX_MPG1.FIN_PAYMENT_VND_HDR.PVH_DATE.ToString(Resources.Constants.DateFormatShort);

                                //lblTotAmountAdjn.Text = String.Format("{0:c}", Convert.ToDecimal(FinPaymentVndAllocationList[e.Row.RowIndex].FIN_PAYMENT_VND_TRX_MPG1.PVM_EXCESS_AMOUNT));
                                ////lblAllocatedAdjn.Text = String.Format("{0:c}", FinPaymentVndAllocationList[e.Row.RowIndex].FIN_PAYMENT_VND_TRX_MPG1.FIN_PAYMENT_VND_ALCN_DTL1.Where(k => k.PAD_PAYMENT_TRX != PaymentMpgPK).Sum(ra => ra.PAD_AMOUNT));// String.Format("{0:c}", Convert.ToDecimal(FinPaymentVndAllocationList[e.Row.RowIndex].FIN_RECEIPT_CUS_TRX_MPG.RCM_EXCESS_AMOUNT).ToString());
                                //lblAllocatedAdjn.Text = String.Format("{0:c}", FinPaymentVndAllocationList[e.Row.RowIndex].FIN_PAYMENT_VND_TRX_MPG1.FIN_PAYMENT_VND_ALCN_DTL1.Where(rad => rad.PAD_PK == FinPaymentVndAllocationList[e.Row.RowIndex].PAD_PK && rad.FIN_PAYMENT_VND_TRX_MPG.FIN_PAYMENT_VND_HDR.PVH_DEL_STATUS == 0 && rad.FIN_PAYMENT_VND_TRX_MPG.FIN_PAYMENT_VND_HDR.PVH_BOUNCED == 0).Sum(ra => ra.PAD_AMOUNT));// String.Format("{0:c}", Convert.ToDecimal(FinReceiptCusAllocationList[e.Row.RowIndex].FIN_RECEIPT_CUS_TRX_MPG.RCM_EXCESS_AMOUNT).ToString());

                                lblTotAmountAdjn.Text = String.Format("{0:c}", Convert.ToDecimal(FinPaymentVndAllocationList[e.Row.RowIndex].FIN_PAYMENT_VND_TRX_MPG1.PVM_EXCESS_AMOUNT));
                                //lblAllocatedAdjn.Text = String.Format("{0:c}", FinPaymentVndAllocationList[e.Row.RowIndex].FIN_PAYMENT_VND_TRX_MPG1.FIN_PAYMENT_VND_ALCN_DTL1
                                //    .Where(rad => rad.FIN_PAYMENT_VND_TRX_MPG.FIN_PAYMENT_VND_HDR.PVH_DEL_STATUS == 0 && rad.FIN_PAYMENT_VND_TRX_MPG.FIN_PAYMENT_VND_HDR.PVH_BOUNCED == 0
                                //        && rad.PAD_PK != FinPaymentVndAllocationList[e.Row.RowIndex].PAD_PK).Where(d => !lstInvNos.Contains(d.FIN_PAYMENT_VND_TRX_MPG.PVM_INVOICE_HDR)).Sum(ra => ra.PAD_AMOUNT));
                                decimal TotalAllocated = FinPaymentVndAllocationList[e.Row.RowIndex].FIN_PAYMENT_VND_TRX_MPG1.PVM_EXCESS_AMOUNT;
                                decimal CurrentInvAlcn = FinPaymentVndAllocationList[e.Row.RowIndex].FIN_PAYMENT_VND_TRX_MPG1.FIN_PAYMENT_VND_ALCN_DTL1
                                        .Where(rad => rad.FIN_PAYMENT_VND_TRX_MPG.FIN_PAYMENT_VND_HDR.PVH_DEL_STATUS == 0
                                        && rad.FIN_PAYMENT_VND_TRX_MPG.FIN_PAYMENT_VND_HDR.PVH_BOUNCED == 0
                                        && rad.PAD_PK != FinPaymentVndAllocationList[e.Row.RowIndex].PAD_PK)
                                        .Where(d => d.FIN_PAYMENT_VND_TRX_MPG.PVM_INVOICE_HDR == InvPk && d.FIN_PAYMENT_VND_TRX_MPG.PVM_PAYMENT_HDR == PymntPK).Sum(ra => ra.PAD_AMOUNT);
                                lblAllocatedAdjn.Text = String.Format("{0:c}", (TotalAllocated - CurrentInvAlcn < 0 ? 0 : TotalAllocated - CurrentInvAlcn));

                                hdfReceiptAdjnPK.Value = FinPaymentVndAllocationList[e.Row.RowIndex].FIN_PAYMENT_VND_TRX_MPG1.PVM_PK.ToString();
                                //allocated = Convert.ToDecimal(lblTotAmountAdjn.Text) - (FinPaymentVndAllocationList[e.Row.RowIndex].FIN_PAYMENT_VND_TRX_MPG1.FIN_PAYMENT_VND_ALCN_DTL1.Where(k => k.PAD_PAYMENT_TRX != PaymentMpgPK).Sum(ra => ra.PAD_AMOUNT));
                                allocated = Convert.ToDecimal(lblTotAmountAdjn.Text.Replace(",", "")) - Convert.ToDecimal(lblAllocatedAdjn.Text.Replace(",", ""));
                            }
                            else
                            {
                                isCr = true;
                                lblCrDrNOSplit.Text = FinPaymentVndAllocationList[e.Row.RowIndex].FIN_CRDR_NOTE_HDR.CDH_NO;
                                lblPageType.Text = "DN";
                                lblDate.Text = FinPaymentVndAllocationList[e.Row.RowIndex].FIN_CRDR_NOTE_HDR.CDH_DATE.ToString(Resources.Constants.DateFormatShort);
                                lblTotAmountAdjn.Text = String.Format("{0:c}", Convert.ToDecimal(FinPaymentVndAllocationList[e.Row.RowIndex].FIN_CRDR_NOTE_HDR.CDH_AMOUNT_TC));
                                //lblAllocatedAdjn.Text = String.Format("{0:c}", FinPaymentVndAllocationList[e.Row.RowIndex].FIN_CRDR_NOTE_HDR.FIN_PAYMENT_VND_ALCN_DTL.Where(k => k.PAD_PAYMENT_TRX != PaymentMpgPK).Sum(ra => ra.PAD_AMOUNT));// String.Format("{0:c}", Convert.ToDecimal(FinPaymentVndAllocationList[e.Row.RowIndex].FIN_RECEIPT_CUS_TRX_MPG.RCM_EXCESS_AMOUNT).ToString());

                                //lblAllocatedAdjn.Text = String.Format("{0:c}", FinPaymentVndAllocationList[e.Row.RowIndex].FIN_CRDR_NOTE_HDR.FIN_PAYMENT_VND_ALCN_DTL.Where(r => r.FIN_PAYMENT_VND_TRX_MPG.FIN_PAYMENT_VND_HDR.PVH_DEL_STATUS == 0 && r.FIN_PAYMENT_VND_TRX_MPG.FIN_PAYMENT_VND_HDR.PVH_BOUNCED == 0 && r.PAD_PK != FinPaymentVndAllocationList[e.Row.RowIndex].PAD_PK).Sum(ra => ra.PAD_AMOUNT));// String.Format("{0:c}", Convert.ToDecimal(FinReceiptCusAllocationList[e.Row.RowIndex].FIN_RECEIPT_CUS_TRX_MPG.RCM_EXCESS_AMOUNT).ToString());

                                decimal TotalAllocated = FinPaymentVndAllocationList[e.Row.RowIndex].FIN_CRDR_NOTE_HDR.FIN_PAYMENT_VND_ALCN_DTL.Where(r => r.FIN_PAYMENT_VND_TRX_MPG.FIN_PAYMENT_VND_HDR.PVH_DEL_STATUS == 0 && r.FIN_PAYMENT_VND_TRX_MPG.FIN_PAYMENT_VND_HDR.PVH_BOUNCED == 0 && r.PAD_PK != FinPaymentVndAllocationList[e.Row.RowIndex].PAD_PK).Sum(ra => ra.PAD_AMOUNT);
                                decimal CurrentInvAlcn = FinPaymentVndAllocationList[e.Row.RowIndex].FIN_CRDR_NOTE_HDR.FIN_PAYMENT_VND_ALCN_DTL
                                      .Where(v => v.FIN_PAYMENT_VND_TRX_MPG.FIN_PAYMENT_VND_HDR.PVH_DEL_STATUS == 0
                                      && v.FIN_PAYMENT_VND_TRX_MPG.FIN_PAYMENT_VND_HDR.PVH_BOUNCED == 0
                                      && v.PAD_PK != FinPaymentVndAllocationList[e.Row.RowIndex].PAD_PK).ToList()
                                      .Where(d => d.FIN_PAYMENT_VND_TRX_MPG.PVM_INVOICE_HDR == InvPk && d.FIN_PAYMENT_VND_TRX_MPG.PVM_PAYMENT_HDR == PymntPK).Sum(ra => ra.PAD_AMOUNT);


                                //lblAllocatedAdjn.Text = String.Format("{0:c}", FinPaymentVndAllocationList[e.Row.RowIndex].FIN_CRDR_NOTE_HDR.FIN_PAYMENT_VND_ALCN_DTL.Where(r => r.FIN_PAYMENT_VND_TRX_MPG.FIN_PAYMENT_VND_HDR.PVH_DEL_STATUS == 0 && r.FIN_PAYMENT_VND_TRX_MPG.FIN_PAYMENT_VND_HDR.PVH_BOUNCED == 0 && r.PAD_PK != FinPaymentVndAllocationList[e.Row.RowIndex].PAD_PK).Sum(ra => ra.PAD_AMOUNT));// String.Format("{0:c}", Convert.ToDecimal(FinReceiptCusAllocationList[e.Row.RowIndex].FIN_RECEIPT_CUS_TRX_MPG.RCM_EXCESS_AMOUNT).ToString());
                                lblAllocatedAdjn.Text = String.Format("{0:c}", (TotalAllocated - CurrentInvAlcn < 0 ? 0 : TotalAllocated - CurrentInvAlcn));
                                //commented for solving wrong allocated amount in edit mode(fully allocated DN shown in )
                                //lblAllocatedAdjn.Text = String.Format("{0:c}", FinPaymentVndAllocationList[e.Row.RowIndex].FIN_CRDR_NOTE_HDR.FIN_PAYMENT_VND_ALCN_DTL
                                //  .Where(v => v.FIN_PAYMENT_VND_TRX_MPG.FIN_PAYMENT_VND_HDR.PVH_DEL_STATUS == 0 && v.FIN_PAYMENT_VND_TRX_MPG.FIN_PAYMENT_VND_HDR.PVH_BOUNCED == 0
                                //      && v.PAD_PK != FinPaymentVndAllocationList[e.Row.RowIndex].PAD_PK).ToList().Where(d => !lstInvNos.Contains(d.FIN_PAYMENT_VND_TRX_MPG.PVM_INVOICE_HDR)).Sum(ra => ra.PAD_AMOUNT));

                                hdfCrDrPK.Value = FinPaymentVndAllocationList[e.Row.RowIndex].FIN_CRDR_NOTE_HDR.CDH_PK.ToString();
                                //allocated = Convert.ToDecimal(lblTotAmountAdjn.Text) - (FinPaymentVndAllocationList[e.Row.RowIndex].FIN_CRDR_NOTE_HDR.FIN_PAYMENT_VND_ALCN_DTL.Where(k => k.PAD_PAYMENT_TRX != PaymentMpgPK).Sum(ra => ra.PAD_AMOUNT));
                                allocated = Convert.ToDecimal(lblTotAmountAdjn.Text.Replace(",", "")) - Convert.ToDecimal(lblAllocatedAdjn.Text.Replace(",", ""));
                            }


                            //if (AdjnNowAmount > 0) { txtAllocateAdjn.Text = String.Format("{0:c}", AdjnNowAmount); } else { txtAllocateAdjn.Text = String.Format("{0:c}", Convert.ToDecimal(FinPaymentVndAllocationList[e.Row.RowIndex].RAD_AMOUNT) ); }
                            if (PaymentAdjnList != null && PaymentAdjnList.Count > 0)//Edit before save
                            {
                                if (isCr)
                                {
                                    txtAllocateAdjn.Text = PaymentAdjnList.Where(fd => fd.PAD_ALCN_CDH == FinPaymentVndAllocationList[e.Row.RowIndex].FIN_CRDR_NOTE_HDR.CDH_PK && fd.FIN_PAYMENT_VND_TRX_MPG.PVM_INVOICE_HDR == InvoicePK).FirstOrDefault().PAD_AMOUNT.ToString(hdfCurrencyFormat.Value); ;
                                }
                                else
                                {
                                    txtAllocateAdjn.Text = PaymentAdjnList.Where(fd => fd.PAD_ALCN_PAYMENT_TRX == FinPaymentVndAllocationList[e.Row.RowIndex].FIN_PAYMENT_VND_TRX_MPG1.PVM_PK && fd.FIN_PAYMENT_VND_TRX_MPG.PVM_INVOICE_HDR == InvoicePK).FirstOrDefault().PAD_AMOUNT.ToString(hdfCurrencyFormat.Value); ;
                                }
                            }
                            else
                            {
                                txtAllocateAdjn.Text = Convert.ToDecimal(FinPaymentVndAllocationList[e.Row.RowIndex].PAD_AMOUNT).ToString(hdfCurrencyFormat.Value);
                            }
                            ////hdfAdjnPK.Value = FinPaymentVndAllocationList[e.Row.RowIndex].PAD_PK.ToString();
                            //////lblAllocatedAdjn.Text = String.Format("{0:c}", (Convert.ToDecimal(lblAllocatedAdjn.Text) - Convert.ToDecimal(txtAllocateAdjn.Text)));
                            ////lblBalanceAdjn.Text = String.Format("{0:c}", allocated);
                            ////totAllocateAdjn = totAllocateAdjn + Convert.ToDecimal(txtAllocateAdjn.Text);
                            ////totBalanceAdjn = totBalanceAdjn + Convert.ToDecimal(lblBalanceAdjn.Text);


                            lblAllocatedAdjn.Text = String.Format("{0:c}", (Convert.ToDecimal(lblAllocatedAdjn.Text.Replace(",", ""))));//- Convert.ToDecimal(txtAllocateAdjn.Text)
                            lblBalanceAdjn.Text = String.Format("{0:c}", allocated);//(allocated).ToString(hdfCurrencyFormat.Value);// + Convert.ToDecimal(txtAllocateAdjn.Text)

                            totAllocateAdjn = totAllocateAdjn + Convert.ToDecimal(txtAllocateAdjn.Text);
                            totBalanceAdjn = totBalanceAdjn + Convert.ToDecimal(lblBalanceAdjn.Text.Replace(",", ""));
                            hdfAdjnPK.Value = FinPaymentVndAllocationList[e.Row.RowIndex].PAD_PK.ToString();


                        }
                        if (Convert.ToDecimal(lblBalanceAdjn.Text.Replace(",", "")) <= 0)
                        {
                            e.Row.Visible = false;
                        }
                    }
                    else if (((GridView)sender).ID == "grdPaymentSplit")
                    {
                        hdfPOPK = e.Row.FindControl("hdfPOPK") as HiddenField;
                        hdfPaymentSplitPK = e.Row.FindControl("hdfPaymentSplitPK") as HiddenField;
                        lnkPONOSplit = e.Row.FindControl("lnkPONOSplit") as LinkButton;
                        //lblPONOSplit = e.Row.FindControl("lblPONOSplit") as Label;
                        lblPODateSplit = e.Row.FindControl("lblPODateSplit") as Label;
                        lblCurrSplit = e.Row.FindControl("lblCurrSplit") as Label;
                        lblAmountSplit = e.Row.FindControl("lblAmountSplit") as Label;
                        lblInvdAmtSplit = e.Row.FindControl("lblInvdAmtSplit") as Label;
                        lblTaxSplit = e.Row.FindControl("lblTaxSplit") as Label;
                        lblDiscountSplit = e.Row.FindControl("lblDiscountSplit") as Label;
                        lblPaidSplit = e.Row.FindControl("lblPaidSplit") as Label;
                        lblBalanceSplit = e.Row.FindControl("lblBalanceSplit") as Label;
                        txtPayNowSplit = e.Row.FindControl("txtPayNowSplit") as TextBox;
                        TextBox txtTaxSplit = e.Row.FindControl("txtTaxSplit") as TextBox;
                        Label lblTotalTaxSplit = e.Row.FindControl("lblTotalTaxSplit") as Label;

                        lblOtherChargesSplit = e.Row.FindControl("lblOtherChargesSplit") as Label;
                        HiddenField hdfOtherChargesSplit = e.Row.FindControl("hdfOtherChargesSplit") as HiddenField;
                        HiddenField hdfInvPoOtherAmnt = e.Row.FindControl("hdfInvPoOtherAmnt") as HiddenField;



                        hdfPayNowSplit = e.Row.FindControl("hdfPayNowSplit") as HiddenField;
                        decimal invothercharge = 0;
                        int popk = 0;

                        if (FinInvoiceVndTrxMpgList != null && FinInvoiceVndTrxMpgList.Count > 0)
                        {
                            if (!IsWorkOrder)
                            {
                                hdfPOPK.Value = FinInvoiceVndTrxMpgList[e.Row.RowIndex].PUR_ORDER_HDR.POH_PK.ToString();
                                hdfPaymentSplitPK.Value = "0";
                                lnkPONOSplit.Text = FinInvoiceVndTrxMpgList[e.Row.RowIndex].PUR_ORDER_HDR.POH_NO;
                                lnkPONOSplit.ToolTip = FinInvoiceVndTrxMpgList[e.Row.RowIndex].PUR_ORDER_HDR.POH_NO;
                                lnkPONOSplit.CommandArgument = FinInvoiceVndTrxMpgList[e.Row.RowIndex].PUR_ORDER_HDR.POH_PK.ToString();
                                //lblPONOSplit.Text = FinInvoiceVndTrxMpgList[e.Row.RowIndex].PUR_ORDER_HDR.POH_NO;
                                //lblPONOSplit.ToolTip = FinInvoiceVndTrxMpgList[e.Row.RowIndex].PUR_ORDER_HDR.POH_NO;
                                lblPODateSplit.Text = FinInvoiceVndTrxMpgList[e.Row.RowIndex].PUR_ORDER_HDR.POH_DATE.ToString(Resources.Constants.DateFormatShort);
                                lblPODateSplit.ToolTip = FinInvoiceVndTrxMpgList[e.Row.RowIndex].PUR_ORDER_HDR.POH_DATE.ToString(Resources.Constants.DateFormatShort);
                                lblCurrSplit.Text = FinInvoiceVndTrxMpgList[e.Row.RowIndex].PUR_ORDER_HDR.ADM_CURRENCY_MST1.CUR_CODE;
                                lblCurrSplit.ToolTip = FinInvoiceVndTrxMpgList[e.Row.RowIndex].PUR_ORDER_HDR.ADM_CURRENCY_MST1.CUR_CODE;
                                lblAmountSplit.Text = String.Format("{0:c}", FinInvoiceVndTrxMpgList[e.Row.RowIndex].PUR_ORDER_HDR.POH_TOTAL_VALUE);
                                lblAmountSplit.ToolTip = String.Format("{0:c}", FinInvoiceVndTrxMpgList[e.Row.RowIndex].PUR_ORDER_HDR.POH_TOTAL_VALUE);

                                //lblInvdAmtSplit.Text = String.Format("{0:c}", FinInvoiceVndTrxMpgList[e.Row.RowIndex].PUR_ORDER_HDR.POH_AMT_INVOICED);
                                //lblInvdAmtSplit.ToolTip = String.Format("{0:c}", FinInvoiceVndTrxMpgList[e.Row.RowIndex].PUR_ORDER_HDR.POH_AMT_INVOICED);
                                decimal poinvamnt = 0;
                                decimal poadvinvamnt = 0;
                                try
                                {
                                    poinvamnt = FinInvoiceVndTrxMpgList[e.Row.RowIndex].PUR_ORDER_HDR.FIN_INVOICE_VND_TRX_MPG.Where(r => r.FIN_INVOICE_VND_HDR.IVH_DEL_STATUS == 0).Sum(c => c.IVM_AMOUNT + c.IVM_OTHER_AMOUNT + c.IVM_TAX_AMOUNT + c.IVM_ADJUST_AMOUNT);
                                    if (FinInvoiceVndTrxMpgList[e.Row.RowIndex].PUR_ORDER_HDR.POH_TYPE == (byte)PurchaseType.Import)
                                    {
                                        //poadvinvamnt = FinInvoiceVndTrxMpgList[e.Row.RowIndex].PUR_ORDER_HDR.FIN_INVOICE_VND_ADV_DED_DTL.Where(r => r.VAD_INVOICE_HDR == FinInvoiceVndTrxMpgList[e.Row.RowIndex].IVM_INVOICE_HDR && r.FIN_INVOICE_VND_HDR.IVH_DEL_STATUS == 0).Sum(c => c.VAD_AMOUNT);
                                        poadvinvamnt = FinInvoiceVndTrxMpgList[e.Row.RowIndex].PUR_ORDER_HDR.FIN_INVOICE_VND_TRX_MPG.Where(r => r.FIN_INVOICE_VND_HDR.IVH_DEL_STATUS == 0 & r.FIN_INVOICE_VND_HDR.IVH_CATEGORY == (byte)POInvoiceCategory.Advanced).Sum(c => c.IVM_AMOUNT);

                                        poinvamnt = poinvamnt - poadvinvamnt;
                                        if (poinvamnt < 0)
                                            poinvamnt = 0;
                                    }
                                }
                                catch { }
                                lblInvdAmtSplit.Text = String.Format("{0:c}", poinvamnt);
                                lblInvdAmtSplit.ToolTip = String.Format("{0:c}", poinvamnt);

                                //lblPaidSplit.Text = String.Format("{0:c}", FinInvoiceVndTrxMpgList[e.Row.RowIndex].PUR_ORDER_HDR.POH_AMT_PAID);
                                //lblPaidSplit.ToolTip = String.Format("{0:c}", FinInvoiceVndTrxMpgList[e.Row.RowIndex].PUR_ORDER_HDR.POH_AMT_PAID);
                                decimal paid = Convert.ToDecimal(FinInvoiceVndTrxMpgList[e.Row.RowIndex].PUR_ORDER_HDR.POH_AMT_PAID == null ? 0 : FinInvoiceVndTrxMpgList[e.Row.RowIndex].PUR_ORDER_HDR.POH_AMT_PAID);
                                decimal? tax = FinInvoiceVndTrxMpgList[e.Row.RowIndex].PUR_ORDER_HDR.POH_ADD_TAX_AMT + FinInvoiceVndTrxMpgList[e.Row.RowIndex].PUR_ORDER_HDR.PUR_ORDER_DTL.Sum(o => o.POD_TAX);
                                decimal? disc = FinInvoiceVndTrxMpgList[e.Row.RowIndex].PUR_ORDER_HDR.POH_DISC_AMT + FinInvoiceVndTrxMpgList[e.Row.RowIndex].PUR_ORDER_HDR.PUR_ORDER_DTL.Sum(o => o.POD_DISC_AMT);
                                //- FinInvoiceVndTrxMpgList[e.Row.RowIndex].PUR_ORDER_HDR.POH_AMT_PAID;
                                lblPaidSplit.Text = String.Format("{0:c}", paid);
                                lblPaidSplit.ToolTip = String.Format("{0:c}", paid);
                                lblTaxSplit.Text = String.Format("{0:c}", tax);
                                lblTaxSplit.ToolTip = String.Format("{0:c}", tax);
                                lblDiscountSplit.Text = String.Format("{0:c}", disc);
                                lblDiscountSplit.ToolTip = String.Format("{0:c}", disc);

                                //decimal balToPay = Convert.ToDecimal(FinInvoiceVndTrxMpgList[e.Row.RowIndex].PUR_ORDER_HDR.POH_TOTAL_VALUE == null ? 0 : FinInvoiceVndTrxMpgList[e.Row.RowIndex].PUR_ORDER_HDR.POH_TOTAL_VALUE)
                                //    - paid;      

                                ////decimal balToPay = Convert.ToDecimal(FinInvoiceVndTrxMpgList[e.Row.RowIndex].PUR_ORDER_HDR.POH_AMT_INVOICED == null ? 0 : FinInvoiceVndTrxMpgList[e.Row.RowIndex].PUR_ORDER_HDR.POH_AMT_INVOICED)
                                ////   - paid;
                                decimal balToPay = Convert.ToDecimal(FinInvoiceVndTrxMpgList[e.Row.RowIndex].PUR_ORDER_HDR.POH_TOTAL_VALUE == null ? 0 : FinInvoiceVndTrxMpgList[e.Row.RowIndex].PUR_ORDER_HDR.POH_TOTAL_VALUE) - paid;

                                lblBalanceSplit.Text = String.Format("{0:c}", balToPay < 0 ? 0 : balToPay);
                                lblBalanceSplit.ToolTip = String.Format("{0:c}", balToPay < 0 ? 0 : balToPay);
                                decimal OtherCharges = 0;
                                if (FinInvoiceVndTrxMpgList.Count == 1)
                                {
                                    balToPay = string.IsNullOrEmpty(lblInvSplitReceiveNow.ToolTip) ? Convert.ToDecimal(0) : Convert.ToDecimal(lblInvSplitReceiveNow.ToolTip.Trim().Replace(",", ""));
                                    OtherCharges = string.IsNullOrEmpty(hdfTotalOtherCharges.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(hdfTotalOtherCharges.Value);
                                }
                                else
                                {
                                    balToPay = 0;
                                }
                                if (tempFinPaymentVndPoMpgList != null)
                                {
                                    tempFinPaymentVndPoMpgObj = tempFinPaymentVndPoMpgList.SingleOrDefault(mpg => mpg.PPO_PO_HDR == Convert.ToInt64(hdfPOPK.Value)
                                        && mpg.FIN_PAYMENT_VND_TRX_MPG.PVM_INVOICE_HDR == InvoicePK);
                                }


                                lblOtherChargesSplit.Text = String.Format("{0:c}", OtherCharges);
                                lblOtherChargesSplit.ToolTip = String.Format("{0:c}", OtherCharges);
                                hdfOtherChargesSplit.Value = OtherCharges.ToString();

                                //commented for auto allocation of amount and tax
                                //txtPayNowSplit.Text = txtPayNowSplit.ToolTip = hdfPayNowSplit.Value =
                                //    tempFinPaymentVndPoMpgObj == null ?
                                //    Math.Round(balToPay < 0 ? 0 : balToPay, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString()
                                //    : Math.Round(tempFinPaymentVndPoMpgObj.PPO_PAID_AMOUNT, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();

                                // new changes for multiple PO
                                decimal poamnt = 0;
                                decimal paidamnt = 0;
                                decimal balamnt = 0;

                                decimal.TryParse(lblAmountSplit.Text.Replace(",", ""), out poamnt);
                                decimal.TryParse(lblPaidSplit.Text.Replace(",", ""), out paidamnt);
                                balamnt = poamnt - paidamnt;
                                totalbalamtsplit += balamnt;
                                lblBalanceSplit.Text = String.Format("{0:c}", Math.Round(balamnt < 0 ? 0 : balamnt, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits));

                                int.TryParse(hdfPOPK.Value, out popk);
                                invothercharge = FinInvoiceVndTrxMpgList.Where(iv => iv.IVM_PO_HDR == popk).Sum(inv => inv.IVM_OTHER_AMOUNT);
                                hdfInvPoOtherAmnt.Value = Math.Round(invothercharge, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            }
                            else if (IsWorkOrder)
                            {
                                hdfPOPK.Value = FinInvoiceVndTrxMpgList[e.Row.RowIndex].INV_WORK_ORDER_ITEM_HDR.WIH_PK.ToString(); //.PUR_ORDER_HDR.POH_PK.ToString();
                                hdfPaymentSplitPK.Value = "0";
                                lnkPONOSplit.Text = FinInvoiceVndTrxMpgList[e.Row.RowIndex].INV_WORK_ORDER_ITEM_HDR.WIH_NO; //.PUR_ORDER_HDR.POH_NO;
                                lnkPONOSplit.ToolTip = FinInvoiceVndTrxMpgList[e.Row.RowIndex].INV_WORK_ORDER_ITEM_HDR.WIH_NO; //.PUR_ORDER_HDR.POH_NO;
                                lnkPONOSplit.CommandArgument = FinInvoiceVndTrxMpgList[e.Row.RowIndex].INV_WORK_ORDER_ITEM_HDR.WIH_PK.ToString(); //.PUR_ORDER_HDR.POH_PK.ToString();
                                //lblPONOSplit.Text = FinInvoiceVndTrxMpgList[e.Row.RowIndex].PUR_ORDER_HDR.POH_NO;
                                //lblPONOSplit.ToolTip = FinInvoiceVndTrxMpgList[e.Row.RowIndex].PUR_ORDER_HDR.POH_NO;
                                lblPODateSplit.Text = FinInvoiceVndTrxMpgList[e.Row.RowIndex].INV_WORK_ORDER_ITEM_HDR.WIH_DATE.ToString(Resources.Constants.DateFormatShort); //.PUR_ORDER_HDR.POH_DATE.ToString(Resources.Constants.DateFormatShort);
                                lblPODateSplit.ToolTip = FinInvoiceVndTrxMpgList[e.Row.RowIndex].INV_WORK_ORDER_ITEM_HDR.WIH_DATE.ToString(Resources.Constants.DateFormatShort); //.PUR_ORDER_HDR.POH_DATE.ToString(Resources.Constants.DateFormatShort);
                                lblCurrSplit.Text = FinInvoiceVndTrxMpgList[e.Row.RowIndex].INV_WORK_ORDER_ITEM_HDR.ADM_CURRENCY_MST1.CUR_CODE; //.PUR_ORDER_HDR.ADM_CURRENCY_MST1.CUR_CODE;
                                lblCurrSplit.ToolTip = FinInvoiceVndTrxMpgList[e.Row.RowIndex].INV_WORK_ORDER_ITEM_HDR.ADM_CURRENCY_MST1.CUR_CODE; //.PUR_ORDER_HDR.ADM_CURRENCY_MST1.CUR_CODE;
                                lblAmountSplit.Text = String.Format("{0:c}", FinInvoiceVndTrxMpgList[e.Row.RowIndex].INV_WORK_ORDER_ITEM_HDR.WIH_NET_TOTAL); //.PUR_ORDER_HDR.POH_TOTAL_VALUE);
                                lblAmountSplit.ToolTip = String.Format("{0:c}", FinInvoiceVndTrxMpgList[e.Row.RowIndex].INV_WORK_ORDER_ITEM_HDR.WIH_NET_TOTAL); //.PUR_ORDER_HDR.POH_TOTAL_VALUE);

                                //lblInvdAmtSplit.Text = String.Format("{0:c}", FinInvoiceVndTrxMpgList[e.Row.RowIndex].PUR_ORDER_HDR.POH_AMT_INVOICED);
                                //lblInvdAmtSplit.ToolTip = String.Format("{0:c}", FinInvoiceVndTrxMpgList[e.Row.RowIndex].PUR_ORDER_HDR.POH_AMT_INVOICED);
                                decimal poinvamnt = 0;
                                decimal poadvinvamnt = 0;
                                try
                                {
                                    //poinvamnt = FinInvoiceVndTrxMpgList[e.Row.RowIndex].PUR_ORDER_HDR.FIN_INVOICE_VND_TRX_MPG.Where(r => r.FIN_INVOICE_VND_HDR.IVH_DEL_STATUS == 0).Sum(c => c.IVM_AMOUNT + c.IVM_OTHER_AMOUNT + c.IVM_TAX_AMOUNT + c.IVM_ADJUST_AMOUNT);
                                    poinvamnt = FinInvoiceVndTrxMpgList[e.Row.RowIndex].INV_WORK_ORDER_ITEM_HDR.FIN_INVOICE_VND_TRX_MPG.Where(r => r.FIN_INVOICE_VND_HDR.IVH_DEL_STATUS == 0).Sum(c => c.IVM_AMOUNT + c.IVM_OTHER_AMOUNT + c.IVM_TAX_AMOUNT + c.IVM_ADJUST_AMOUNT);
                                    //if (FinInvoiceVndTrxMpgList[e.Row.RowIndex].PUR_ORDER_HDR.POH_TYPE == (byte)PurchaseType.Import)
                                    //{
                                    //    //poadvinvamnt = FinInvoiceVndTrxMpgList[e.Row.RowIndex].PUR_ORDER_HDR.FIN_INVOICE_VND_ADV_DED_DTL.Where(r => r.VAD_INVOICE_HDR == FinInvoiceVndTrxMpgList[e.Row.RowIndex].IVM_INVOICE_HDR && r.FIN_INVOICE_VND_HDR.IVH_DEL_STATUS == 0).Sum(c => c.VAD_AMOUNT);
                                    //    poadvinvamnt = FinInvoiceVndTrxMpgList[e.Row.RowIndex].PUR_ORDER_HDR.FIN_INVOICE_VND_TRX_MPG.Where(r => r.FIN_INVOICE_VND_HDR.IVH_DEL_STATUS == 0 & r.FIN_INVOICE_VND_HDR.IVH_CATEGORY == (byte)POInvoiceCategory.Advanced).Sum(c => c.IVM_AMOUNT);

                                    //    poinvamnt = poinvamnt - poadvinvamnt;
                                    //    if (poinvamnt < 0)
                                    //        poinvamnt = 0;
                                    //}
                                }
                                catch { }
                                lblInvdAmtSplit.Text = String.Format("{0:c}", poinvamnt);
                                lblInvdAmtSplit.ToolTip = String.Format("{0:c}", poinvamnt);

                                //lblPaidSplit.Text = String.Format("{0:c}", FinInvoiceVndTrxMpgList[e.Row.RowIndex].PUR_ORDER_HDR.POH_AMT_PAID);
                                //lblPaidSplit.ToolTip = String.Format("{0:c}", FinInvoiceVndTrxMpgList[e.Row.RowIndex].PUR_ORDER_HDR.POH_AMT_PAID);
                                decimal paid = Convert.ToDecimal(FinInvoiceVndTrxMpgList[e.Row.RowIndex].INV_WORK_ORDER_ITEM_HDR.WIH_AMT_PAID == null ? 0 : FinInvoiceVndTrxMpgList[e.Row.RowIndex].INV_WORK_ORDER_ITEM_HDR.WIH_AMT_PAID);//.PUR_ORDER_HDR.POH_AMT_PAID == null ? 0 : FinInvoiceVndTrxMpgList[e.Row.RowIndex].PUR_ORDER_HDR.POH_AMT_PAID);
                                decimal? tax = FinInvoiceVndTrxMpgList[e.Row.RowIndex].INV_WORK_ORDER_ITEM_HDR.WIH_TAX_AMT + FinInvoiceVndTrxMpgList[e.Row.RowIndex].INV_WORK_ORDER_ITEM_HDR.INV_WORK_ORDER_ITEM_DTL.Sum(s => s.WID_TAX); //PUR_ORDER_HDR.POH_ADD_TAX_AMT + FinInvoiceVndTrxMpgList[e.Row.RowIndex].PUR_ORDER_HDR.PUR_ORDER_DTL.Sum(o => o.POD_TAX);
                                decimal? disc = FinInvoiceVndTrxMpgList[e.Row.RowIndex].INV_WORK_ORDER_ITEM_HDR.WIH_DISC_AMT + FinInvoiceVndTrxMpgList[e.Row.RowIndex].INV_WORK_ORDER_ITEM_HDR.INV_WORK_ORDER_ITEM_DTL.Sum(o => o.WID_DISC_AMT);//.PUR_ORDER_HDR.POH_DISC_AMT + FinInvoiceVndTrxMpgList[e.Row.RowIndex].PUR_ORDER_HDR.PUR_ORDER_DTL.Sum(o => o.POD_DISC_AMT);
                                //- FinInvoiceVndTrxMpgList[e.Row.RowIndex].PUR_ORDER_HDR.POH_AMT_PAID;
                                lblPaidSplit.Text = String.Format("{0:c}", paid);
                                lblPaidSplit.ToolTip = String.Format("{0:c}", paid);
                                lblTaxSplit.Text = String.Format("{0:c}", tax);
                                lblTaxSplit.ToolTip = String.Format("{0:c}", tax);
                                lblDiscountSplit.Text = String.Format("{0:c}", disc);
                                lblDiscountSplit.ToolTip = String.Format("{0:c}", disc);

                                //decimal balToPay = Convert.ToDecimal(FinInvoiceVndTrxMpgList[e.Row.RowIndex].PUR_ORDER_HDR.POH_TOTAL_VALUE == null ? 0 : FinInvoiceVndTrxMpgList[e.Row.RowIndex].PUR_ORDER_HDR.POH_TOTAL_VALUE)
                                //    - paid;      

                                ////decimal balToPay = Convert.ToDecimal(FinInvoiceVndTrxMpgList[e.Row.RowIndex].PUR_ORDER_HDR.POH_AMT_INVOICED == null ? 0 : FinInvoiceVndTrxMpgList[e.Row.RowIndex].PUR_ORDER_HDR.POH_AMT_INVOICED)
                                ////   - paid;

                                //decimal balToPay = Convert.ToDecimal(FinInvoiceVndTrxMpgList[e.Row.RowIndex].PUR_ORDER_HDR.POH_TOTAL_VALUE == null ? 0 : FinInvoiceVndTrxMpgList[e.Row.RowIndex].PUR_ORDER_HDR.POH_TOTAL_VALUE) - paid;
                                decimal balToPay = Convert.ToDecimal(FinInvoiceVndTrxMpgList[e.Row.RowIndex].INV_WORK_ORDER_ITEM_HDR.WIH_NET_TOTAL == null ? 0 : FinInvoiceVndTrxMpgList[e.Row.RowIndex].INV_WORK_ORDER_ITEM_HDR.WIH_NET_TOTAL) - paid;

                                lblBalanceSplit.Text = String.Format("{0:c}", balToPay < 0 ? 0 : balToPay);
                                lblBalanceSplit.ToolTip = String.Format("{0:c}", balToPay < 0 ? 0 : balToPay);
                                decimal OtherCharges = 0;
                                if (FinInvoiceVndTrxMpgList.Count == 1)
                                {
                                    balToPay = string.IsNullOrEmpty(lblInvSplitReceiveNow.ToolTip) ? Convert.ToDecimal(0) : Convert.ToDecimal(lblInvSplitReceiveNow.ToolTip.Trim().Replace(",", ""));
                                    OtherCharges = string.IsNullOrEmpty(hdfTotalOtherCharges.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(hdfTotalOtherCharges.Value);
                                }
                                else
                                {
                                    balToPay = 0;
                                }
                                if (tempFinPaymentVndPoMpgList != null)
                                {
                                    //tempFinPaymentVndPoMpgObj = tempFinPaymentVndPoMpgList.SingleOrDefault(mpg => mpg.PPO_PO_HDR == Convert.ToInt64(hdfPOPK.Value)
                                    //    && mpg.FIN_PAYMENT_VND_TRX_MPG.PVM_INVOICE_HDR == InvoicePK);
                                    tempFinPaymentVndPoMpgObj = tempFinPaymentVndPoMpgList.SingleOrDefault(mpg => mpg.PPO_WO_HDR == Convert.ToInt64(hdfPOPK.Value)
                                        && mpg.FIN_PAYMENT_VND_TRX_MPG.PVM_INVOICE_HDR == InvoicePK);
                                }


                                lblOtherChargesSplit.Text = String.Format("{0:c}", OtherCharges);
                                lblOtherChargesSplit.ToolTip = String.Format("{0:c}", OtherCharges);
                                hdfOtherChargesSplit.Value = OtherCharges.ToString();

                                //commented for auto allocation of amount and tax
                                //txtPayNowSplit.Text = txtPayNowSplit.ToolTip = hdfPayNowSplit.Value =
                                //    tempFinPaymentVndPoMpgObj == null ?
                                //    Math.Round(balToPay < 0 ? 0 : balToPay, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString()
                                //    : Math.Round(tempFinPaymentVndPoMpgObj.PPO_PAID_AMOUNT, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();

                                // new changes for multiple PO
                                decimal poamnt = 0;
                                decimal paidamnt = 0;
                                decimal balamnt = 0;

                                decimal.TryParse(lblAmountSplit.Text.Replace(",", ""), out poamnt);
                                decimal.TryParse(lblPaidSplit.Text.Replace(",", ""), out paidamnt);
                                balamnt = poamnt - paidamnt;
                                totalbalamtsplit += balamnt;
                                lblBalanceSplit.Text = String.Format("{0:c}", Math.Round(balamnt < 0 ? 0 : balamnt, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits));

                                int.TryParse(hdfPOPK.Value, out popk);
                                invothercharge = FinInvoiceVndTrxMpgList.Where(iv => iv.IVM_WO_HDR == popk).Sum(inv => inv.IVM_OTHER_AMOUNT); //.Where(iv => iv.IVM_PO_HDR == popk).Sum(inv => inv.IVM_OTHER_AMOUNT);
                                hdfInvPoOtherAmnt.Value = Math.Round(invothercharge, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();

                            }
                        }//
                        else if (finPaymentVndPoMpgList != null && finPaymentVndPoMpgList.Count > 0)
                        {
                            hdfPOPK.Value = finPaymentVndPoMpgList[e.Row.RowIndex].PUR_ORDER_HDR.POH_PK.ToString();
                            hdfPaymentSplitPK.Value = finPaymentVndPoMpgList[e.Row.RowIndex].PPO_PK.ToString();
                            lnkPONOSplit.Text = finPaymentVndPoMpgList[e.Row.RowIndex].PUR_ORDER_HDR.POH_NO;
                            lnkPONOSplit.ToolTip = finPaymentVndPoMpgList[e.Row.RowIndex].PUR_ORDER_HDR.POH_NO;
                            lnkPONOSplit.CommandArgument = finPaymentVndPoMpgList[e.Row.RowIndex].PUR_ORDER_HDR.POH_PK.ToString();
                            //lblPONOSplit.Text = finPaymentVndPoMpgList[e.Row.RowIndex].PUR_ORDER_HDR.POH_NO;
                            //lblPONOSplit.ToolTip = finPaymentVndPoMpgList[e.Row.RowIndex].PUR_ORDER_HDR.POH_NO;
                            lblPODateSplit.Text = finPaymentVndPoMpgList[e.Row.RowIndex].PUR_ORDER_HDR.POH_DATE.ToString(Resources.Constants.DateFormatShort);
                            lblPODateSplit.ToolTip = finPaymentVndPoMpgList[e.Row.RowIndex].PUR_ORDER_HDR.POH_DATE.ToString(Resources.Constants.DateFormatShort);
                            lblCurrSplit.Text = finPaymentVndPoMpgList[e.Row.RowIndex].PUR_ORDER_HDR.ADM_CURRENCY_MST1.CUR_CODE;
                            lblCurrSplit.ToolTip = finPaymentVndPoMpgList[e.Row.RowIndex].PUR_ORDER_HDR.ADM_CURRENCY_MST1.CUR_CODE;
                            lblAmountSplit.Text = String.Format("{0:c}", finPaymentVndPoMpgList[e.Row.RowIndex].PUR_ORDER_HDR.POH_TOTAL_VALUE);
                            lblAmountSplit.ToolTip = String.Format("{0:c}", finPaymentVndPoMpgList[e.Row.RowIndex].PUR_ORDER_HDR.POH_TOTAL_VALUE);

                            //lblInvdAmtSplit.Text = String.Format("{0:c}", finPaymentVndPoMpgList[e.Row.RowIndex].PUR_ORDER_HDR.POH_AMT_INVOICED);
                            //lblInvdAmtSplit.ToolTip = String.Format("{0:c}", finPaymentVndPoMpgList[e.Row.RowIndex].PUR_ORDER_HDR.POH_AMT_INVOICED);
                            decimal poinvamnt = 0;
                            decimal poadvinvamnt = 0;
                            try
                            {
                                poinvamnt = finPaymentVndPoMpgList[e.Row.RowIndex].PUR_ORDER_HDR.FIN_INVOICE_VND_TRX_MPG.Where(r => r.FIN_INVOICE_VND_HDR.IVH_DEL_STATUS == 0).Sum(c => c.IVM_AMOUNT + c.IVM_OTHER_AMOUNT + c.IVM_TAX_AMOUNT + c.IVM_ADJUST_AMOUNT);
                                if (finPaymentVndPoMpgList[e.Row.RowIndex].PUR_ORDER_HDR.POH_TYPE == (byte)PurchaseType.Import)
                                {
                                    //poadvinvamnt = finPaymentVndPoMpgList[e.Row.RowIndex].PUR_ORDER_HDR.FIN_INVOICE_VND_ADV_DED_DTL.Where(r => r.VAD_INVOICE_HDR == finPaymentVndPoMpgList[e.Row.RowIndex].FIN_PAYMENT_VND_TRX_MPG.PVM_INVOICE_HDR & r.FIN_INVOICE_VND_HDR.IVH_DEL_STATUS == 0).Sum(c => c.VAD_AMOUNT);
                                    poadvinvamnt = finPaymentVndPoMpgList[e.Row.RowIndex].PUR_ORDER_HDR.FIN_INVOICE_VND_TRX_MPG.Where(r => r.FIN_INVOICE_VND_HDR.IVH_DEL_STATUS == 0 & r.FIN_INVOICE_VND_HDR.IVH_CATEGORY == (byte)POInvoiceCategory.Advanced).Sum(c => c.IVM_AMOUNT);
                                    poinvamnt = poinvamnt - poadvinvamnt;
                                    if (poinvamnt < 0)
                                        poinvamnt = 0;
                                }
                            }
                            catch { }
                            lblInvdAmtSplit.Text = String.Format("{0:c}", poinvamnt);
                            lblInvdAmtSplit.ToolTip = String.Format("{0:c}", poinvamnt);

                            //decimal paid = finPaymentVndPoMpgList[e.Row.RowIndex].PPO_BOUNCED == 0 ? finPaymentVndPoMpgList[e.Row.RowIndex].PUR_ORDER_HDR.POH_AMT_PAID - finPaymentVndPoMpgList[e.Row.RowIndex].PPO_PAID_AMOUNT : finPaymentVndPoMpgList[e.Row.RowIndex].PUR_ORDER_HDR.POH_AMT_PAID;
                            decimal paid = finPaymentVndPoMpgList[e.Row.RowIndex].PPO_BOUNCED == 0 ? finPaymentVndPoMpgList[e.Row.RowIndex].PUR_ORDER_HDR.POH_AMT_PAID - finPaymentVndPoMpgList[e.Row.RowIndex].PUR_ORDER_HDR.FIN_PAYMENT_VND_PO_MPG.Where(mpg => mpg.PPO_PAYMENT_HDR == finPaymentVndPoMpgList[e.Row.RowIndex].PPO_PAYMENT_HDR).Sum(pamt => pamt.PPO_PAID_AMOUNT) : finPaymentVndPoMpgList[e.Row.RowIndex].PUR_ORDER_HDR.POH_AMT_PAID;

                            decimal? disc = finPaymentVndPoMpgList[e.Row.RowIndex].PUR_ORDER_HDR.POH_DISC_AMT + finPaymentVndPoMpgList[e.Row.RowIndex].PUR_ORDER_HDR.PUR_ORDER_DTL.Sum(o => o.POD_DISC_AMT);
                            decimal? tax = finPaymentVndPoMpgList[e.Row.RowIndex].PUR_ORDER_HDR.POH_ADD_TAX_AMT + finPaymentVndPoMpgList[e.Row.RowIndex].PUR_ORDER_HDR.PUR_ORDER_DTL.Sum(o => o.POD_TAX);//.FIN_RECEIPT_CUS_TRX_MPG.FIN_INVOICE_CUS_HDR.ICH_TAX_TC;


                            lblPaidSplit.Text = String.Format("{0:c}", paid);
                            lblPaidSplit.ToolTip = String.Format("{0:c}", paid);
                            lblTaxSplit.Text = String.Format("{0:c}", tax);
                            lblTaxSplit.ToolTip = String.Format("{0:c}", tax);
                            lblDiscountSplit.Text = String.Format("{0:c}", disc);
                            lblDiscountSplit.ToolTip = String.Format("{0:c}", disc);

                            //decimal balToPay = (Convert.ToDecimal(finPaymentVndPoMpgList[e.Row.RowIndex].PUR_ORDER_HDR.POH_TOTAL_VALUE == null ? 0 : finPaymentVndPoMpgList[e.Row.RowIndex].PUR_ORDER_HDR.POH_TOTAL_VALUE)
                            //    - paid);

                            ////decimal balToPay = (Convert.ToDecimal(finPaymentVndPoMpgList[e.Row.RowIndex].PUR_ORDER_HDR.POH_AMT_INVOICED == null ? 0 : finPaymentVndPoMpgList[e.Row.RowIndex].PUR_ORDER_HDR.POH_AMT_INVOICED)
                            ////  - paid);

                            decimal balToPay = (Convert.ToDecimal(finPaymentVndPoMpgList[e.Row.RowIndex].PUR_ORDER_HDR.POH_TOTAL_VALUE == null ? 0 : finPaymentVndPoMpgList[e.Row.RowIndex].PUR_ORDER_HDR.POH_TOTAL_VALUE)
                              - paid);

                            lblBalanceSplit.Text = String.Format("{0:c}", balToPay < 0 ? 0 : balToPay);
                            lblBalanceSplit.ToolTip = String.Format("{0:c}", balToPay < 0 ? 0 : balToPay);
                            balToPay = finPaymentVndPoMpgList[e.Row.RowIndex].PPO_PAID_AMOUNT;
                            decimal tempValue = string.IsNullOrEmpty(lblInvSplitReceiveNow.ToolTip) ? Convert.ToDecimal(0) : Convert.ToDecimal(lblInvSplitReceiveNow.ToolTip.Trim().Replace(",", ""));
                            if (balToPay != tempValue)
                            {
                                isSplitChanged = true;
                            }
                            //if (finPaymentVndPoMpgList.Count == 1)
                            //{
                            //    balToPay = string.IsNullOrEmpty(lblInvSplitReceiveNow.ToolTip) ? Convert.ToDecimal(0) : Convert.ToDecimal(lblInvSplitReceiveNow.ToolTip.Trim().Replace(",", ""));
                            //}
                            //else
                            //{
                            //    balToPay = 0;
                            //}

                            if (tempFinPaymentVndPoMpgList != null)
                            {
                                tempFinPaymentVndPoMpgObj = tempFinPaymentVndPoMpgList.SingleOrDefault(mpg => mpg.PPO_PO_HDR == Convert.ToInt64(hdfPOPK.Value)
                                    && mpg.FIN_PAYMENT_VND_TRX_MPG.PVM_INVOICE_HDR == InvoicePK);
                            }
                            txtPayNowSplit.Text = txtPayNowSplit.ToolTip = hdfPayNowSplit.Value =
                                tempFinPaymentVndPoMpgObj == null ?
                                Math.Round(balToPay < 0 ? 0 : balToPay, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString()
                                : Math.Round(tempFinPaymentVndPoMpgObj.PPO_PAID_AMOUNT, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString(); ;

                            //txtPayNowSplit.Text = Math.Round(finPaymentVndPoMpgList[e.Row.RowIndex].PPO_PAID_AMOUNT < 0 ? 0 : finPaymentVndPoMpgList[e.Row.RowIndex].PPO_PAID_AMOUNT,
                            //    Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            //txtPayNowSplit.ToolTip = Math.Round(finPaymentVndPoMpgList[e.Row.RowIndex].PPO_PAID_AMOUNT < 0 ? 0 : finPaymentVndPoMpgList[e.Row.RowIndex].PPO_PAID_AMOUNT,
                            //    Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            //hdfPayNowSplit.Value = Math.Round(finPaymentVndPoMpgList[e.Row.RowIndex].PPO_PAID_AMOUNT < 0 ? 0 : finPaymentVndPoMpgList[e.Row.RowIndex].PPO_PAID_AMOUNT,
                            //    Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();

                            // new changes for multiple PO
                            decimal poamnt = 0;
                            decimal paidamnt = 0;
                            decimal balamnt = 0;
                            decimal OtherCharge = 0;
                            decimal.TryParse(lblAmountSplit.Text.Replace(",", ""), out poamnt);
                            decimal.TryParse(lblPaidSplit.Text.Replace(",", ""), out paidamnt);
                            balamnt = poamnt - paidamnt;
                            totalbalamtsplit += balamnt;
                            lblBalanceSplit.Text = String.Format("{0:c}", Math.Round(balamnt < 0 ? 0 : balamnt, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits));
                            lblTotalTaxSplit.Text = lblTotalTaxSplit.ToolTip = txtTaxSplit.Text = txtTaxSplit.ToolTip =
                                 tempFinPaymentVndPoMpgObj == null ?
                                String.Format("{0:c}", 0)
                                 : Math.Round(tempFinPaymentVndPoMpgObj.PPO_TAX_AMOUNT, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();

                            OtherCharge = tempFinPaymentVndPoMpgObj == null ?
                                0
                                : Math.Round(tempFinPaymentVndPoMpgObj.PPO_OTHER_AMOUNT, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);

                            lblOtherChargesSplit.Text = lblOtherChargesSplit.ToolTip = String.Format("{0:c}", OtherCharge);
                            hdfOtherChargesSplit.Value = OtherCharge.ToString();
                            int.TryParse(hdfPOPK.Value, out popk);
                            try
                            {
                                invothercharge = finPaymentVndPoMpgList[e.Row.RowIndex].FIN_PAYMENT_VND_TRX_MPG.FIN_INVOICE_VND_HDR.FIN_INVOICE_VND_TRX_MPG.Where(iv => iv.IVM_PO_HDR == popk).Sum(inv => inv.IVM_OTHER_AMOUNT);
                            }
                            catch { }

                            hdfInvPoOtherAmnt.Value = Math.Round(invothercharge, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();

                        }
                    }
                    #region grdPaymentSplit
                    else if (((GridView)sender).ID == "grdPaymentModes")
                    {
                        HiddenField hdfPymntMode = e.Row.FindControl("hdfPymntMode") as HiddenField;
                        HiddenField hdfPymntBankPk = e.Row.FindControl("hdfPymntBankPk") as HiddenField;
                        HiddenField hdfPymntAccount = e.Row.FindControl("hdfPymntAccount") as HiddenField;
                        HiddenField hdfPymntBankCurrency = e.Row.FindControl("hdfPymntBankCurrency") as HiddenField;

                        Label lblPymntMode = e.Row.FindControl("lblPymntMode") as Label;
                        Label lblPymntBankName = e.Row.FindControl("lblPymntBankName") as Label;
                        Label lblPymntAccountNo = e.Row.FindControl("lblPymntAccountNo") as Label;
                        Label lblPymntBankCurrency = e.Row.FindControl("lblPymntBankCurrency") as Label;
                        int bnkCurrency = 0;
                        int.TryParse(hdfPymntBankCurrency.Value, out bnkCurrency);
                        CurrencyPk = bnkCurrency;

                        GetFieldValues(ControlsEnum.PAYMODE);
                        if (admConfigMstList != null && admConfigMstList.Count > 0)
                        {
                            ADM_CONFIG_MST objConfig = admConfigMstList.SingleOrDefault(cfg => cfg.CFG_VALUE == Convert.ToByte(hdfPymntMode.Value));
                            if (objConfig != null)
                                lblPymntMode.Text = lblPymntMode.ToolTip = objConfig.CFG_DATA;
                        }
                        hdfPaymentBank.Value = hdfPymntBankPk.Value;
                        if (!string.IsNullOrEmpty(hdfPaymentBank.Value) && Convert.ToInt32(hdfPaymentBank.Value.ToString()) > 0)
                        {
                            GetFieldValues(ControlsEnum.BANK);
                            if (finCashBankMstList != null && finCashBankMstList.Count > 0)
                            {
                                lblPymntBankName.Text = CommonFunctions.GetShortString(HttpUtility.HtmlDecode(finCashBankMstList[0].CBM_NAME), 16);
                                lblPymntBankName.ToolTip = HttpUtility.HtmlDecode(finCashBankMstList[0].CBM_NAME);
                                lblPymntAccountNo.Text = lblPymntAccountNo.ToolTip = finCashBankMstList[0].CBM_ACC_NO;
                                txtBranch.Text = finCashBankMstList[0].CBM_BRANCH;
                                if (Convert.ToInt32(ddlMode.SelectedValue) == (int)PaymentModeEnum.DD)
                                {
                                    hdfFavourof.Value = txtFavourof.Text = CommonFunctions.GetShortString(HttpUtility.HtmlDecode(finCashBankMstList[0].CBM_NAME), 10);
                                    txtFavourof.ToolTip = HttpUtility.HtmlDecode(finCashBankMstList[0].CBM_NAME);
                                }
                            }
                        }
                        GetFieldValues(ControlsEnum.CURRENCYMST);
                        if (CurrencyMstList != null && CurrencyMstList.Count > 0 && Convert.ToInt32(hdfPymntMode.Value) != (int)PaymentModeEnum.CASH && Convert.ToInt32(hdfPymntMode.Value) != (int)PaymentModeEnum.OTHERS)
                        {
                            lblPymntBankCurrency.Text = lblPymntBankCurrency.ToolTip = CurrencyMstList[0].CUR_CODE;// +" - " + CurrencyMstList[0].CUR_NAME;                            
                        }
                        //hdfPaymentBank.Value = string.Empty;
                        CurrencyPk = 0;
                    }
                    #endregion

                }
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    if (((GridView)sender).ID == "grdPOPaymentHdr")
                    {
                        GetFieldValues(ControlsEnum.BASECURRENCY);
                        Label lblHdrAmountBaseCur = e.Row.FindControl("lblHdrAmountBaseCur") as Label;
                        lblHdrAmountBaseCur.Text = GetLocalResourceObject("Amount").ToString() + " (" + hdfBaseCurrency.Value.Split('-')[0].Trim() + ")";
                    }
                    else if (((GridView)sender).ID == "grdPaymentModes")
                    {
                        GetFieldValues(ControlsEnum.BASECURRENCY);
                        Label lblHdrPymntTotalAmountBC = e.Row.FindControl("lblHdrPymntTotalAmountBC") as Label;
                        Label lblHdrPymntTotalAmount = e.Row.FindControl("lblHdrPymntTotalAmount") as Label;
                        lblHdrPymntTotalAmountBC.Text = string.Format(GetLocalResourceObject("TotalAmntBC").ToString(), hdfBaseCurrency.Value.Split('-')[0].Trim());
                        lblHdrPymntTotalAmount.Text = string.Format(GetLocalResourceObject("TotalAmnt").ToString(), txtPaymentCurrency.Text.Trim());
                        if (hdfBaseCurrency.Value.Split('-')[0].Trim() == txtPaymentCurrency.Text.Trim())
                            grdPaymentModes.Columns[11].Visible = false;
                    }
                }
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    if (((GridView)sender).ID == "grdPaymentSplitAdjn")
                    {

                        lblTotalAllocateAdjn = e.Row.FindControl("lblTotalAllocateAdjn") as Label;
                        hdfTotalAllocateAdjn = e.Row.FindControl("hdfTotalAllocateAdjn") as HiddenField;
                        hdfBalanceAdjn = e.Row.FindControl("hdfBalanceAdjn") as HiddenField;
                        //if (CrDrAdjnList != null && CrDrAdjnList.Count > 0)
                        //{
                        lblTotalAllocateAdjn.Text = String.Format("{0:c}", totAllocateAdjn);
                        hdfTotalAllocateAdjn.Value = totAllocateAdjn.ToString();
                        hdfBalanceAdjn.Value = totBalanceAdjn.ToString();
                        //}

                    }
                    if (((GridView)sender).ID == "grdInvoiceList")
                    {
                        lblTotalFooter = e.Row.FindControl("lblTotalPayNowFooter") as Label;
                        if (finInvoiceHdrList != null && finInvoiceHdrList.Count > 0)
                        {
                            total = 0;
                            total = finInvoiceHdrList.Sum(dtl => dtl.IVH_AMOUNT_NET_TC - dtl.IVH_AMOUNT_PAID_TC + dtl.IVH_AMOUNT_CN_TC - dtl.IVH_AMOUNT_DN_TC);
                            total = total < 0 ? 0 : total;
                            lblTotalFooter.Text = string.Format("{0:c}", total);
                            txtPaidAmount.Text = Math.Round(total, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            //WHT Tax
                            if (ddlWHTAccount.SelectedValue != CommonConstants.SELECTVAL)
                            {
                                whtTaxpk = Convert.ToInt32(ddlWHTAccount.SelectedValue);
                                //if (chkVendorforpayemnt.Checked)
                                //{
                                GetFieldValues(ControlsEnum.VENDORACCOUNTTAX);
                                SetFieldValues(ControlsEnum.VENDORACCOUNTTAX);
                                //}
                            }
                        }
                        else if (finPaymentTrxMpgList != null && finPaymentTrxMpgList.Count > 0)
                        {
                            total = 0;
                            total = finPaymentTrxMpgList.Sum(dtl => dtl.PVM_PAID_AMOUNT);
                            total = total < 0 ? 0 : total;
                            lblTotalFooter.Text = string.Format("{0:c}", total);
                            txtPaidAmount.Text = Math.Round(total, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                        }
                    }
                    else if (((GridView)sender).ID == "grdPaymentSplit")
                    {
                        lblTotalPayNowFooterSplit = e.Row.FindControl("lblTotalPayNowFooterSplit") as Label;
                        hdfTotalPayNowFooterSplit = e.Row.FindControl("hdfTotalPayNowFooterSplit") as HiddenField;
                        HiddenField hdfTotalBalFooterSplit = e.Row.FindControl("hdfTotalBalFooterSplit") as HiddenField;
                        Label lblTotalBalFooterSplit = e.Row.FindControl("lblTotalBalFooterSplit") as Label;


                        if (FinInvoiceVndTrxMpgList != null && FinInvoiceVndTrxMpgList.Count > 0)
                        {
                            if (FinInvoiceVndTrxMpgList.Count == 1)
                            {
                                total = string.IsNullOrEmpty(lblInvSplitReceiveNow.Text) ? Convert.ToDecimal(0) : Convert.ToDecimal(lblInvSplitReceiveNow.Text.Trim().Replace(",", ""));
                            }
                            else
                            {
                                total = 0;
                            }
                            if (tempFinPaymentVndPoMpgList == null || tempFinPaymentVndPoMpgList.Count == 0)
                            {
                                lblTotalPayNowFooterSplit.Text = string.Format("{0:c}", total < 0 ? 0 : total);
                                hdfTotalPayNowFooterSplit.Value = total.ToString();
                            }
                            else
                            {
                                lblTotalPayNowFooterSplit.Text = string.Format("{0:c}", tempFinPaymentVndPoMpgList
                                    .Where(mpg => mpg.FIN_PAYMENT_VND_TRX_MPG.PVM_INVOICE_HDR == InvoicePK)
                                    .Sum(mpg => mpg.PPO_PAID_AMOUNT));
                                hdfTotalPayNowFooterSplit.Value = tempFinPaymentVndPoMpgList
                                    .Where(mpg => mpg.FIN_PAYMENT_VND_TRX_MPG.PVM_INVOICE_HDR == InvoicePK)
                                    .Sum(mpg => mpg.PPO_PAID_AMOUNT).ToString();


                            }
                        }
                        else if (finPaymentVndPoMpgList != null && finPaymentVndPoMpgList.Count > 0)
                        {
                            if (!isSplitChanged)
                            {
                                if (finPaymentVndPoMpgList.Count == 1)
                                {
                                    total = string.IsNullOrEmpty(lblInvSplitReceiveNow.Text) ? Convert.ToDecimal(0) : Convert.ToDecimal(lblInvSplitReceiveNow.Text.Trim().Replace(",", ""));
                                }
                                else
                                {
                                    total = 0;
                                }
                            }
                            else
                            {
                                total = 0;
                            }
                            if (tempFinPaymentVndPoMpgList == null || tempFinPaymentVndPoMpgList.Count == 0)
                            {
                                lblTotalPayNowFooterSplit.Text = string.Format("{0:c}", total < 0 ? 0 : total);
                                hdfTotalPayNowFooterSplit.Value = total.ToString();
                            }
                            else
                            {
                                lblTotalPayNowFooterSplit.Text = string.Format("{0:c}", tempFinPaymentVndPoMpgList
                                    .Where(mpg => mpg.FIN_PAYMENT_VND_TRX_MPG.PVM_INVOICE_HDR == InvoicePK)
                                    .Sum(mpg => mpg.PPO_PAID_AMOUNT));
                                hdfTotalPayNowFooterSplit.Value = tempFinPaymentVndPoMpgList
                                    .Where(mpg => mpg.FIN_PAYMENT_VND_TRX_MPG.PVM_INVOICE_HDR == InvoicePK)
                                    .Sum(mpg => mpg.PPO_PAID_AMOUNT).ToString();

                            }
                        }
                        lblTotalBalFooterSplit.Text = string.Format("{0:c}", totalbalamtsplit);
                        hdfTotalBalFooterSplit.Value = Math.Round(totalbalamtsplit, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();

                    }

                }

                int slno;
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
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        private void SetVatbuyNotYetDueRowColor()
        {


            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            foreach (GridViewRow grvRowVatTax in grdVATTaxDetails.Rows)
            {
                HiddenField hdfVATTax = grvRowVatTax.FindControl("hdfVATTax") as HiddenField;
                HiddenField hdfIsVatbuyNotDue = grvRowVatTax.FindControl("hdfIsVatbuyNotDue") as HiddenField;
                //ImageButton imbVatTaxEdit = grvRowVatTax.FindControl("imbVatTaxEdit") as ImageButton;
                //ImageButton imbVatTaxRemove = grvRowVatTax.FindControl("imbVatTaxRemove") as ImageButton;
                hdfIsVatbuyNotDue.Value = "0";
                if (!string.IsNullOrEmpty(hdfVATTax.Value))
                {
                    //imbVatTaxEdit.Enabled = false;
                    //imbVatTaxRemove.Enabled = false;
                    int.TryParse(hdfVATTax.Value, out TaxPk);
                    GetFieldValues(ControlsEnum.TAXDETAILS);
                    //DataTable dtTaxMst = BusinessLogic.Administration.Masters.TaxSettingsMaster.GetTaxDetails(Convert.ToInt32(hdfVATTax.Value), 0, currentUser.SBUID, 2);
                    if (dtTaxMst != null && dtTaxMst.Rows.Count > 0 && Convert.ToInt32(dtTaxMst.Rows[0][Resources.DataFieldRes.TaxNotDue]) == (int)TaxEnum.TaxNotYetDue)
                    {
                        hdfIsVatbuyNotDue.Value = dtTaxMst.Rows[0][Resources.DataFieldRes.TaxNotDue].ToString();
                        //grvRowVatTax.BackColor = System.Drawing.Color.AliceBlue;
                        //imbVatTaxEdit.Enabled = true;
                        //imbVatTaxRemove.Enabled = true;
                    }
                }
            }

            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowSelectedRow", "$(document).ready(function(){SetVatbuyNotYetDueRowColor();});", true);

        }

        public bool IsFinancialYearExist()
        {
            bool isExist = true;
            DateTime paymentDate = String.IsNullOrEmpty(txtPaymentDate.Text.Trim()) ? DateTime.Now : Convert.ToDateTime(txtPaymentDate.Text.Trim());
            if (BusinessLogic.CommonManagement.CommonBL.IsFinancialYearExist(paymentDate, currentUser.SBUID))
            {
                isExist = false;
            }
            return isExist;
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
                    if (SortDirection == Resources.Report.SortAscending)
                        SortDirection = Resources.Report.SortDescending;
                    else
                        SortDirection = Resources.Report.SortAscending;
                }
                else
                {
                    SortBy = e.SortExpression;
                    SortDirection = Resources.Report.SortAscending;

                }
                this.PageIndex = "1";
                GetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                SetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                EntryStatus = EntryStatus.LISTMODE;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.ErpRes.Information + "');", true);
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
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            uclPaging.CurrentPage = 1;
            btnSaveSubmit.PreRender += new EventHandler(btnAction_PreRender);
            btnSubmit.PreRender += new EventHandler(btnAction_PreRender);
            btnSavePmnt.PreRender += new EventHandler(btnAction_PreRender);
            btnDeletePmnt.PreRender += new EventHandler(btnAction_PreRender);
            btnPrint.PreRender += new EventHandler(btnAction_PreRender);
            btnCancel.PreRender += new EventHandler(btnAction_PreRender);
            btnJournalize.PreRender += new EventHandler(btnAction_PreRender);

            btnNew.PreRender += new EventHandler(btnAction_PreRender);
            btnEdit.PreRender += new EventHandler(btnAction_PreRender);
            btnView.PreRender += new EventHandler(btnAction_PreRender);

            //lbnPOListing.PreRender += new EventHandler(btnAction_PreRender);
            //lnkInvoicing.PreRender += new EventHandler(btnAction_PreRender);
            //lbnPOInvoice.PreRender += new EventHandler(btnAction_PreRender);
            //lbnExpenses.PreRender += new EventHandler(btnAction_PreRender);
            //lnkPayment.PreRender += new EventHandler(btnAction_PreRender);
            //lnbCrDrNote.PreRender += new EventHandler(btnAction_PreRender);
            //lnbAcPayables.PreRender += new EventHandler(btnAction_PreRender);

            lnkList.PreRender += new EventHandler(btnAction_PreRender);
            lnkDetail.PreRender += new EventHandler(btnAction_PreRender);

            btnReverse.PreRender += new EventHandler(btnAction_PreRender);
            btnReverseDetail.PreRender += new EventHandler(btnAction_PreRender);
            btnReturnDetail.PreRender += new EventHandler(btnAction_PreRender);
            btnReturn.PreRender += new EventHandler(btnAction_PreRender);

            btnSavePaymentSplit.PreRender += new EventHandler(btnAction_PreRender);
            btnEditforCancel.PreRender += new EventHandler(btnAction_PreRender);
            btnCancelSubmit.PreRender += new EventHandler(btnAction_PreRender);
            btnVatTaxApply.PreRender += new EventHandler(btnAction_PreRender);
            btnVatTaxSave.PreRender += new EventHandler(btnAction_PreRender);
            btnWhtSave.PreRender += new EventHandler(btnAction_PreRender);
            //btnApply.PreRender += new EventHandler(btnAction_PreRender);



            btnSaveSubmit.Load += new EventHandler(btnAction_Load);
            btnSubmit.Load += new EventHandler(btnAction_Load);
            btnSavePmnt.Load += new EventHandler(btnAction_Load);
            btnDeletePmnt.Load += new EventHandler(btnAction_Load);
            btnPrint.Load += new EventHandler(btnAction_Load);
            btnCancel.Load += new EventHandler(btnAction_Load);
            btnJournalize.Load += new EventHandler(btnAction_Load);

            btnEdit.Load += new EventHandler(btnAction_Load);
            btnView.Load += new EventHandler(btnAction_Load);
            btnNew.Load += new EventHandler(btnAction_Load);

            //lbnPOListing.Load += new EventHandler(btnAction_Load);
            //lnkInvoicing.Load += new EventHandler(btnAction_Load);
            //lbnPOInvoice.Load += new EventHandler(btnAction_Load);
            //lbnExpenses.Load += new EventHandler(btnAction_Load);
            //lnkPayment.Load += new EventHandler(btnAction_Load);
            //lnbCrDrNote.Load += new EventHandler(btnAction_Load);
            //lnbAcPayables.Load += new EventHandler(btnAction_Load);

            lnkList.Load += new EventHandler(btnAction_Load);
            lnkDetail.Load += new EventHandler(btnAction_Load);

            btnSavePaymentSplit.Load += new EventHandler(btnAction_Load);
            btnReverse.Load += new EventHandler(btnAction_Load);
            btnReverseDetail.Load += new EventHandler(btnAction_Load);
            btnReturnDetail.Load += new EventHandler(btnAction_Load);
            btnReturn.Load += new EventHandler(btnAction_Load);
            btnEditforCancel.Load += new EventHandler(btnAction_Load);
            btnCancelSubmit.Load += new EventHandler(btnAction_Load);
            btnVatTaxApply.Load += new EventHandler(btnAction_Load);
            btnVatTaxSave.Load += new EventHandler(btnAction_Load);
            btnWhtSave.Load += new EventHandler(btnAction_Load);
            //btnApply.Load += new EventHandler(btnAction_Load);

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
            this.uclPaging.FirstPage += new ActionHandler(this.ActionHandler);
            this.uclPaging.PreviousPage += new ActionHandler(this.ActionHandler);
            this.uclPaging.NextPage += new ActionHandler(this.ActionHandler);
            this.uclPaging.LastPage += new ActionHandler(this.ActionHandler);
            this.uclPaging.PageChanged += new ActionHandler(this.ActionHandler);
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
                switch (e.Action)
                {
                    case NavigationEnum.PAGECHANGE:
                        uclPaging.CurrentPage = e.CurrentPage;
                        break;
                    case NavigationEnum.FIRST:
                        if (e.CurrentPage > 1)
                            uclPaging.CurrentPage = 1;
                        break;
                    case NavigationEnum.LAST:
                        if (e.CurrentPage <= e.TotalPages)
                            uclPaging.CurrentPage = e.TotalPages;
                        break;
                    case NavigationEnum.NEXT:
                        // increment the current page index.
                        if (e.CurrentPage <= e.TotalPages)
                            uclPaging.CurrentPage++;
                        break;
                    case NavigationEnum.PREVIOUS:
                        // Decrement the current page index.
                        if (e.CurrentPage > 1)
                            uclPaging.CurrentPage--;
                        break;


                }

                PageIndex = uclPaging.CurrentPage.ToString();
                // Change Code As per the page
                GetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                SetFieldValues(ControlsEnum.PAYMENTHDRLIST);

                //============================
                EnableDisableButtons(e.TotalPages);
                EntryStatus = EntryStatus.LISTMODE;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.ErpRes.Information + "');", true);
            }

        }

        /// <summary>
        /// Methord used to enable and Disable Page Navigation Controls
        /// </summary>
        /// <param name="iTotalPages"></param>
        private void EnableDisableButtons(int iTotalPages)
        {
            uclPaging.FirstButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;

            uclPaging.PreviousButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;

            uclPaging.NextButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;

            uclPaging.LastButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
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
                //hdfHasSplit,grdInvoiceList,hdfInvoicePK
                if (grdInvoiceList.Rows != null)
                {
                    foreach (GridViewRow gvr in grdInvoiceList.Rows)
                    {
                        HiddenField hdfInvoicePK = gvr.FindControl("hdfInvoicePK") as HiddenField;
                        if (hdfInvoicePK != null)
                        {
                            (gvr.FindControl("hdfHasSplit") as HiddenField).Value = InvoicePOSplitList.Any(dtl => dtl.FIN_PAYMENT_VND_TRX_MPG != null
                                && dtl.FIN_PAYMENT_VND_TRX_MPG.PVM_INVOICE_HDR == Convert.ToInt64(hdfInvoicePK.Value))
                                ? CommonConstants.SELECT_VALUE_ONE : CommonConstants.SELECT_VALUE_ZERO;
                        }
                        Button lnkAllocation = gvr.FindControl("lnkAllocation") as Button;
                        if (lnkAllocation != null)
                        {
                            lnkAllocation.Visible = (POGroup != POInvoiceGroup.Expense);
                        }
                    }
                }

                if (IsPaymentModeAdded)
                {
                    hdfIsPaymentModeAdded.Value = "1";
                    SetPymntModeHdrValidation(false);
                }
                else
                {
                    hdfIsPaymentModeAdded.Value = "0";
                    SetPymntModeHdrValidation(true);
                }
                hdfPaymentModeRowIndex.Value = PaymentModeRowIndex.ToString();
                if (EntryStatus == EntryStatus.VIEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(1);});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ModeAutoComplete", "ModeAutoComplete();", true);
                    lnkList.CssClass = GetLocalResourceObject("TabInactive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                }
                else if (EntryStatus == EntryStatus.NEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(2);});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ModeAutoComplete", "ModeAutoComplete();", true);
                    lnkList.CssClass = GetLocalResourceObject("TabInactive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                }
                else if (EntryStatus == EntryStatus.ENTRYMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ModeAutoComplete", "ModeAutoComplete();", true);
                    lnkList.CssClass = GetLocalResourceObject("TabInactive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                }
                else if (EntryStatus == EntryStatus.LISTMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowListing", "$(document).ready(function(){ShowListing(1);});", true);
                    lnkList.CssClass = GetLocalResourceObject("TabActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabInactive").ToString();
                }
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideAdvancedSearch();});", true);
                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_CalculateTotal", "$(document).ready(function(){CalculateTotal();CalculateTotalSplit();});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_CalculateTotal", "$(document).ready(function(){CalculateTotal();});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_CalculateTotalFooter", "$(document).ready(function(){CalculateTotalFooterTaxSplit();});", true);

                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowUploadDocDetails", "$(document).ready(function(){ShowHideUploadDocDetails();});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_CalculatePymntModeTotalFooter", "$(document).ready(function(){CalculatePymntModeTotalFooter();});", true);

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
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            string TYPE = Request.QueryString[QueryStrings.PageType] != null ? Request.QueryString[QueryStrings.PageType] : string.Empty;
            if (TYPE != "3")//Type 3 for cancelation
            {
                DataTable dtCancelProcess = wrkfService.GetProcessID(GetLocalResourceObject("CancelPageURL").ToString(), Convert.ToInt32(Session[BusinessObject.Common.SessionStrings.CurDept].ToString()));
                if (dtCancelProcess != null && dtCancelProcess.Rows.Count > 0)
                {
                    WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                    ucrWrkf.CancelRefID = workflowCore.GetRefID(curPK, int.Parse(dtCancelProcess.Rows[0][CommonConstants.F_PROCESS].ToString()));
                }
            }
            #endregion
        }
        #endregion
        #region Enum
        /// <summary>
        ///Page Controls Enum 
        /// </summary>
        public enum ControlsEnum
        {
            PAYMENTHDRLIST,
            PAYMENTHDRENTRY,
            PAYMENTMPGENTRY,
            PAYMENTMPGLIST,
            BANK,
            PAYMENTHDRINVLISTBYPK,
            PAYMENTHDRENTRYBYPK,
            PAYMENTNO,
            EXCHANGERATE,
            EXCHANGERATEINBASECURRENCY,
            JOURNALIZE,
            FINHEADER,
            WRKFSUBMIT,
            BASECURRENCY,
            PAYMENTSPLITLIST,
            INVOICEVNDMPGLIST,
            INVOICEVNDHDR,
            PAYMENTSPLITLISTBYPAYMENTPK,
            PAYMODE,
            GETPAYMENTPKBYJOURNALPK,
            FILLWORKFLOWSTATUS,
            DISCOUNTTYPE,
            VENDORACCOUNT,
            VENDORACCOUNTTAX,
            VENDOR,
            ALERTSAVE,
            NOTIFICATIONTYPES,
            ALERTBASIS,
            ALERTTYPES,
            NOTIFICATIONDAYS,
            ALERTLIST,
            ALERTCONFIG,
            BANKCURRENCY,
            INVVNDMPGLIST,
            REVERSE,
            EXCHANGERATEBANK,
            CHEQUERETURN,
            COMPANY,
            WHTPOPUPGRID,
            WHTTAXDETAILS,
            FORMNO,
            FINHEADERSTATUS,
            VATBUYVENDOR,
            VENDORACCOUNTVATBUYTAX,
            VATPOPUPGRID,
            VENDORCONTACTYPE,
            VATBUYTAXTYPES,
            TAXTYPECHANGED,
            PAYMENTTAXHDR,
            VENDORSELECTEDDTL,
            VENDORINVDTL,
            VENDORCONTACTYPEDETAILS,
            PURINVNOS,
            WHTVENDOR,
            VENDORTYPES,
            VENDORBANKS,
            UPLOADEDFILES,
            ADDITEM,
            SELECTEDDOC,
            TAXDETAILS,
            VATBUYPOPUPHEADER,
            PAYMENTADJN,
            ADJNSPLITLIST,
            PAYMENTADJNLIST,
            VENDORCONTACTFORWHT,
            PAYMENTTYPE,
            SPLITPAYNOWFORMULIPO,
            PAYMENTTOLERANCE,
            ADVINVOICELIST,
            PAYMENTADJNDUMMYLIST,
            INVOICEVNDMPGLISTFORAUTOALCN,
            PAYMENTMODES,
            PAYMENTMODESGRID,
            CURRENCYMST,
            BANKCURRENCYEXCHANGERATE,
            CRDRALLOCATION,
            CRDRSPLITLIST,
            CRDRMPGLIST,
            INVOICELIST,
            INVOICEDETAILS,
            CRDRALCNFORNEWINV,
            PAYMENTGET,
            CHECKCREDITDEBITPOST
        }
        /// <summary>
        ///Payment Mode Enum 
        /// </summary>
        public enum PaymentModeEnum
        {
            CASH = 1,
            CHEQUE,
            DD,
            BANK,
            GENERAL,
            OTHERS
        }

        /// <summary>
        /// Define Activity Status Enum
        /// </summary>
        public enum WkfStatusEnum
        {
            DRAFTED = 0,
            APPROVED = 2
        }

        public enum WHTFormNo
        {
            PND54 = 390
        }
        public enum TaxEnum
        {
            TaxNotYetDue = 1
        }

        /// <summary>
        /// Attachment Module Enum
        /// </summary>
        public enum DocModuleEnum
        {
            FINANCE = 8

        }
        /// <summary>
        /// Attachment Task Enum
        /// </summary>
        public enum DocTaskEnum
        {
            FINANCETASK = 12

        }
        #endregion

        /// <ConfigurationSettings>
        /// 
        /// </summary>
        private void ConfigurationSettings()
        {
            hdfIsSBUsPaymnetBank.Value = GetGlobalResourceObject("ConfigurationsRes", "IsSBUsPaymnetBank").ToString();
            hdfIsTaxForOtherCharge.Value = GetGlobalResourceObject("ConfigurationsRes", "OtherChargeTaxPurchase").ToString();
            hdfIsMultipleCheque.Value = GetGlobalResourceObject("ConfigurationsRes", "IsMultipleChequePrint").ToString();
            ShowAdjColumn = Convert.ToBoolean(Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "ShowAdjColumn")));
            ShowExpenseCrDr = Convert.ToBoolean(Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "CNDNFromExpenseInv")));
            DataTable dtAppConfigs = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("BIZUNIT SETTINGS", "VENDOR");
            if (dtAppConfigs != null && dtAppConfigs.Rows.Count > 0)
            {
                hdfIsSBUVendor.Value = dtAppConfigs.Rows[0]["ACF_VALUE"].ToString() == "0" ? "true" : "false";
            }
        }
    }
}
