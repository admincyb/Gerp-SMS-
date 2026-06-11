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


#region DB Summary
/*/////  TABLES  //////:
1. FIN_PAYMENT_VND_HDR
2. FIN_PAYMENT_VND_TRX_MPG
3. FIN_PAYMENT_VND_ALCN_DTL
4. FIN_PAYMENT_VND_PO_MPG
5. FIN_PAYMENT_VND_CRDR_MPG
6. FIN_PAYMENT_VND_MODE_DTL
7. FIN_PAYMENT_VND_TAX_HDR
8. FIN_PAYMENT_VND_TAX_DTL 

///// STORED PROCEDURES ////:
1.Pending Invoice List => SPFIN_PAYMENT_VND_PEND_INVOICE_GET
2.Add to list/Get SP   => SPFIN_PAYMENT_VND_HDR_GET
3.SAVE SP              => SPFIN_PAYMENT_VND_WKF_SAVE (SPFIN_PAYMENT_VND_HDR_SAVE)
4.LISTING SP           => SPFIN_PAYMENT_VND_HDR_GET_LIST
5.DELETE SP            => SPFIN_PAYMENT_VND_HDR_DELETE
6.Payment No Auto SP   => SPFIN_PAYMENT_TRADING_NO_AUTO
7.To get Adjustment Allocation Against Vendor.=> SPFIN_PAYMENT_VND_ALCN_GET


 * */

#endregion

namespace ERPSMS_v01.POInvoicing
{
    public partial class POPaymentTrading : ERP.Store.UI.WorkFlowBasePage
    {
        #region Variables and Properties
        #region Properties
        
        private List<PaymentPOMappingDetails> InvoicePOSplitList
        {

            get
            {
                return this.ViewState[ViewstateStrings.InvoicePOSplitList] != null ? (List<PaymentPOMappingDetails>)this.ViewState[ViewstateStrings.InvoicePOSplitList] : new List<PaymentPOMappingDetails>();

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
        private List<PaymentTaxHeader> WHTTaxDetails
        {
            get
            {
                return Session[ERP.Utilities.SessionStrings.WHTTaxDetails] == null ? new List<PaymentTaxHeader>()
                    : (List<PaymentTaxHeader>)Session[ERP.Utilities.SessionStrings.WHTTaxDetails];
            }
            set
            {
                if (value == null)
                    Session.Remove(ERP.Utilities.SessionStrings.WHTTaxDetails);
                else
                    Session[ERP.Utilities.SessionStrings.WHTTaxDetails] = value;
            }
        }
        private List<PaymentTaxHeader> TempWHTTaxDetails
        {
            get
            {
                return Session[ERP.Utilities.SessionStrings.TempWHTTaxDetails] == null ? new List<PaymentTaxHeader>()
                    : (List<PaymentTaxHeader>)Session[ERP.Utilities.SessionStrings.TempWHTTaxDetails];
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
                return this.ViewState[ViewstateStrings.TotalPages] != null ? Convert.ToInt32(this.ViewState[ViewstateStrings.TotalPages]) : 0;
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

        private List<PaymentTaxHeader> VATTaxDetails
        {
            get
            {
                return Session[ERP.Utilities.SessionStrings.VATTaxDetails] == null ? new List<PaymentTaxHeader>()
                    : (List<PaymentTaxHeader>)Session[ERP.Utilities.SessionStrings.VATTaxDetails];
            }
            set
            {
                if (value == null)
                    Session.Remove(ERP.Utilities.SessionStrings.VATTaxDetails);
                else
                    Session[ERP.Utilities.SessionStrings.VATTaxDetails] = value;
            }
        }

        private List<PaymentTaxHeader> TempVATTaxDetails
        {
            get
            {
                return Session[ERP.Utilities.SessionStrings.TempVATTaxDetails] == null ? new List<PaymentTaxHeader>()
                    : (List<PaymentTaxHeader>)Session[ERP.Utilities.SessionStrings.TempVATTaxDetails];
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
        private List<BusinessObject.POInvoicing.PaymentUploads> PaymentUploadList
        {
            get
            {
                return ViewState[ViewstateStrings.POUploadList] == null ? null : (List<BusinessObject.POInvoicing.PaymentUploads>)ViewState[ViewstateStrings.POUploadList];
            }
            set
            {
                ViewState[ViewstateStrings.POUploadList] = value;
            }
        }
        /// <summary>
        /// To maintain keep selected pos
        /// </summary>
        private List<PaymentCRDRAdjAllocationDtl> PaymentAdjnList
        {

            get
            {
                return this.ViewState[ViewstateStrings.PaymentAdjnList] != null ? (List<PaymentCRDRAdjAllocationDtl>)this.ViewState[ViewstateStrings.PaymentAdjnList] : new List<PaymentCRDRAdjAllocationDtl>();
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
        private List<PaymentModeDetails> PaymentModeDetailsList
        {
            get
            {
                return Session[ERP.Utilities.SessionStrings.PaymentModeDetailsList] == null ? new List<PaymentModeDetails>()
                    : (List<PaymentModeDetails>)Session[ERP.Utilities.SessionStrings.PaymentModeDetailsList];
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

        private List<PaymentCRDRMappingDetails> PaymentCrdrList
        {

            get
            {
                return this.ViewState[ViewstateStrings.PaymentCrdrList] != null ? (List<PaymentCRDRMappingDetails>)this.ViewState[ViewstateStrings.PaymentCrdrList] : new List<PaymentCRDRMappingDetails>();
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

        /// <summary>
        /// To maintain the PageIndex in viewstate
        /// </summary>
        private string PageIndexInv
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
        private DataTable dtPendingInvList
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
        /// To maintain keep PO Invoice Header Tax Splitting
        /// </summary>
        private POPaymentTradingBO InvPaymentHeaderSession
        {
            get
            {
                return (POPaymentTradingBO)Session["DirectInvPaymentHeaderSession"];
            }
            set
            {
                Session["DirectInvPaymentHeaderSession"] = value;
            }
        }

        /// <summary>
        /// To maintain keep PO Invoice Header Tax Splitting
        /// </summary>
        private POPaymentTradingBO TempInvPaymentHeaderSession
        {
            get
            {
                return (POPaymentTradingBO)this.ViewState["DirectTempInvPaymentHeaderSession"];
            }
            set
            {
                this.ViewState["DirectTempInvPaymentHeaderSession"] = value;
            }
        }

        /// <summary>
        /// To keep config value for Enable/Disable Header Discount in viewstate
        /// </summary>
        private bool IsHeaderDiscountForTradingPurchase
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsHeaderDiscountForTradingPurchase] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsHeaderDiscountForTradingPurchase].ToString());
            }
            set
            {
                this.ViewState[ViewstateStrings.IsHeaderDiscountForTradingPurchase] = value;
            }
        }
        /// <summary>
        /// To keep config value for Enable/Disable Header Tax in viewstate
        /// </summary>
        private bool IsHeaderTaxForTradingPurchase
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsHeaderTaxForTradingPurchase] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsHeaderTaxForTradingPurchase].ToString());
            }
            set
            {
                this.ViewState[ViewstateStrings.IsHeaderTaxForTradingPurchase] = value;
            }
        }
        /// <summary>
        /// To keep config value for Enable/Disable Item Discount in viewstate
        /// </summary>
        private bool IsItemwiseDiscountForTradingPurchase
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsItemwiseDiscountForTradingPurchase] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsItemwiseDiscountForTradingPurchase].ToString());
            }
            set
            {
                this.ViewState[ViewstateStrings.IsItemwiseDiscountForTradingPurchase] = value;
            }
        }
        /// <summary>
        /// To keep config value for Enable/Disable Item Tax in viewstate  
        /// </summary>
        private bool IsItemwiseTaxForTradingPurchase
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsItemwiseTaxForTradingPurchase] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsItemwiseTaxForTradingPurchase].ToString());
            }
            set
            {
                this.ViewState[ViewstateStrings.IsItemwiseTaxForTradingPurchase] = value;
            }
        }

        /// <summary>
        /// To maintain keep Mapping details
        /// </summary>
        private List<PaymentInvoiceTrxMpgDetails> PaymentInvMappingDetails
        {
            get
            {
                return (List<PaymentInvoiceTrxMpgDetails>)this.ViewState["PaymentInvMappingDetails"];
            }
            set
            {
                this.ViewState["PaymentInvMappingDetails"] = value;
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
        private PaymentInvoiceTrxMpgDetails finPaymentVndTrxMpgObj;
        private FIN_PAYMENT_VND_TAX_HDR finPaymentVndTaxHdrObj;
        private FIN_INVOICE_VND_HDR finInvoiceVndHdrObj;
        private FIN_COA_MST finCoaMstObj;
        private FIN_CASH_BANK_MST finCashBankMstObj;
        private PaymentPOMappingDetails finPaymentVndPoMpgObj;
        private PaymentCRDRMappingDetails finPaymentVndCrdrMpgObj;
        private FIN_INVOICE_VND_TRX_MPG FinInvoiceVndTrxMpgObj;
        private FIN_INVOICE_VND_HDR finInvoiceVndHdrObjForPaymentSplit;
       
        private ADM_COMPANY_MST admCompanyMstObj;
        private List<ADM_COMPANY_MST> admCompanyMstList;

        //List for binding details to controls  
        private List<POPaymentTradingBO> finPaymentVndHdrList;
        private List<PaymentInvoiceTrxMpgDetails> finPaymentVndTrxMpgList;
        private List<PaymentTaxHeader> finPaymentVndTaxHdrList;
        private List<FIN_PAYMENT_VND_TRX_MPG> FinPayVndTrxMpgList;
        private List<FIN_INVOICE_VND_HDR> finInvoiceVndHdrList;
        private List<FIN_COA_MST> finCoaMstList;
        private List<FIN_CASH_BANK_MST> finCashBankMstList;

        private List<PaymentPOMappingDetails> finPaymentVndPoMpgList;
        private List<FIN_INVOICE_VND_TRX_MPG> FinInvoiceVndTrxMpgList;
        private List<DirectPOInvoiceMappingDetails> FinInvoiceVndTrxMpgListForAutoAlcn;
        private List<FIN_INVOICE_VND_HDR> finInvoiceVndHdrListForPaymentSplit;
        private List<PaymentTaxHeader> finPayemtVndHdrList;
        private PaymentTaxHeader finVatPaymentDetails;

        private FIN_PAYMENT_VND_TAX_DTL finPaymentVndTaxDtlObj;       

        private List<PaymentTaxHeader> tempVATTaxDetails;
        private PaymentTaxHeader tempVATTax;
        private FIN_PAYMENT_VND_TAX_HDR finPymntObj;
        private List<FIN_PAYMENT_VND_TAX_HDR> finPymntList;

        private List<PaymentCRDRAdjAllocationDtl> FinPaymentVndAllocationList;
        private List<FIN_PAYMENT_VND_ALCN_DTL> FinPaymentVndAdjnDupCheckList;
        private PaymentCRDRAdjAllocationDtl FinPaymentVndAllocationObj;
        private List<FIN_INVOICE_VND_ADV_DED_DTL> finAdvDeductList;

        private List<PaymentCRDRMappingDetails> FinPaymentVndCrdrMpgList;
        private List<PaymentCRDRMappingDetails> FinPaymentVndCrdrAllocationList;
        private FIN_PAYMENT_VND_CRDR_MPG FinPaymentVndCrdrAllocationObj;
        private List<FIN_CRDR_NOTE_MPG> FinCrdrMpgList;
        private FIN_INVOICE_VND_HDR FinInvoiceVndObj;
        List<PaymentCrdrMpg> PaymentCrdrListForNewInv;
        private List<PaymentInvoiceTrxMpgDetails> tempPaymentInvoiceTrxMpgDetails;
        private List<PaymentPOMappingDetails> paymentPOMpgDetailsList;
        private List<PaymentAdjnAllocation> CrDrAdjnList = null;
      
        //private List<long> selectedInvoiceList;
        private List<long> NewInvoiceList = new List<long>();

        private bool isSplitChanged = false;
        private bool IsCreditExist = false;
        private long InPk;

        private bool updatePayment;
        int JournalPK;
        int invGroup;

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
        private DataTable dtPaymentList;      
        private DataTable dtPendingDrAdjn;
        private DataTable dtInvoiceType;
        private DataTable dtInvoiceCategory;
        private DataTable dtVendorDetails;
        DataSet dsPageData;

        private int whtTaxpk;
        private int VatBuyTaxpk;
        int vendorPk = 0;
        private int VendorBankPk = 0;
        private int TaxPk = 0;     

        private decimal VatBuyTax = 0;     
        private decimal AmountTotal = 0;
        private decimal TaxTotal = 0;
        private decimal TaxWhtTotal = 0;
        private decimal VatBuyTaxAmntTotal = 0;
        private decimal totAllocateAdjn = 0;
        private decimal totBalanceAdjn = 0;
        private decimal totalbalamtsplit = 0;
        private decimal PaymentTollerence = 1;

        private int vendPK = 0;
        private int purchaseInvoicePK = 0;  
        bool isCancelled = false;
        private DataTable dtAmountDetails;

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
        private string appType;
        List<PaymentPOMappingDetails> tempFinPaymentVndPoMpgList;
        private List<SPADM_APP_SUB_TYPE_DATA_GET_Result> AppTypeDetailsList;
        private CommonService cm;

        private TradingInvHeaderBO InvHeaderObj;
        private POPaymentTradingBO paymentHeaderObj;
        List<PaymentInvoiceTrxMpgDetails> paymentInvoiceMappingDetailList;
        private PaymentInvoiceTrxMpgDetails paymentInvMapDtlObj;

        private string[] _operators = { "-", "+", "/", "*", "^" };
        private Func<double, double, double>[] _operations = {
		    (a1, a2) => a1 - a2,
		    (a1, a2) => a1 + a2,
		    (a1, a2) => a1 / a2,
		    (a1, a2) => a1 * a2,
		    (a1, a2) => Math.Pow(a1, a2)
	    };

        PaymentUploads poUploadObj;

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

                    InvPaymentHeaderSession = null;
                    TempInvPaymentHeaderSession = null;
                    PaymentInvMappingDetails = null;
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
                    GetFieldValues(ControlsEnum.VENDORTYPES);
                    GetFieldValues(ControlsEnum.INVOICETYPE);
                    SetFieldValues(ControlsEnum.INVOICETYPE);
                    GetFieldValues(ControlsEnum.INVOICECATEGORY);
                    SetFieldValues(ControlsEnum.INVOICECATEGORY);

                    string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
                    hdfCurrencyFormatWithSeperation.Value = "#" + currencysep + "#0.";
                    hdfCurrencyFormat.Value = "#0.";
                    for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits; i++)
                    {
                        hdfCurrencyFormat.Value += "0";
                        hdfCurrencyFormatWithSeperation.Value += "0";
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


                  
                    string pid = Request.QueryString[QueryStrings.PID] != null ? Request.QueryString[QueryStrings.PID]
                        : Session[ERP.Utilities.SessionStrings.PID] != null ? Session[ERP.Utilities.SessionStrings.PID].ToString().Split('=')[1] : string.Empty;
                 
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
                            }
                            else
                            {
                                ucrWrkf.ViewType = 1;
                                EntryStatus = EntryStatus.ENTRYMODE;
                            }
                        
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
                            }
                            int mode;
                            GetFieldValues(ControlsEnum.INVPAYMENTHEADER);
                            SetFieldValues(ControlsEnum.INVPAYMENTHEADER);
                            SetFieldValues(ControlsEnum.INVPAYMENTDETAIL);
                            SetFieldValues(ControlsEnum.UPLOADEDFILES);
                            TempInvPaymentHeaderSession = InvPaymentHeaderSession;
                            GetFieldValues(ControlsEnum.PENDINGINVLIST);
                            SetFieldValues(ControlsEnum.PENDINGINVLIST);

                            ModifiedDatePnl.Visible = true;
                            mode = String.IsNullOrEmpty(ddlMode.SelectedValue) ? 0 : Convert.ToInt32(ddlMode.SelectedValue);
                            GetFieldValues(ControlsEnum.BASECURRENCY);
                            lblTotalAmountBC.Text = string.Format(lblTotalAmountBC.Text, hdfBaseCurrency.Value.Split('-')[0].Trim());
                            SetPaymentModeDetails(mode);
                        }
                        else
                        {

                            //Sets data key for the gird
                            string[] datakeyarray;
                            datakeyarray = new string[1];
                            datakeyarray[0] = Resources.DataFieldRes.POPaymentPK;
                            grdPOPaymentHdr.DataKeyNames = datakeyarray;

                            TempInvPaymentHeaderSession = null;
                            InvPaymentHeaderSession = null;
                            GetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                            SetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                            EntryStatus = EntryStatus.LISTMODE;
                            PageIndex = "1";
                            uclPaging.TotalPages = TotalPages;
                            uclPaging.CurrentPage = 1;
                            GetFieldValues(ControlsEnum.BASECURRENCY);
                            lblTotalAmountBC.Text = string.Format(lblTotalAmountBC.Text, hdfBaseCurrency.Value.Split('-')[0].Trim());
                        }              
                        EnableDisableExchangeRate();
                        GetFieldValues(ControlsEnum.VENDORBANKS);
                        SetFieldValues(ControlsEnum.VENDORBANKS);
                        Session[ERP.Utilities.SessionStrings.SelectedPos] = null;                       

                        if (!string.IsNullOrEmpty(ddlBankChargeCurrency.SelectedValue))//If Invoicetype is Domestic then exchangerate is noneditable                   
                        {
                            GetFieldValues(ControlsEnum.BANKCURRENCYEXCHANGERATE);
                        }
                        AST_CODE.Value = POGroup == POInvoiceGroup.Goods ? ApplicationType.VPT : POGroup == POInvoiceGroup.Services ? ApplicationType.SIPT : POGroup == POInvoiceGroup.AgtInvoice ? ApplicationType.AIPT : ApplicationType.EIPT;
                        AST_DOC_MODE.Value = GetDOCMODE();
                        lblPaymentNo.Text = hdfPaymentNo.Value == string.Empty ?  Resources.ErpRes.Draft : hdfPaymentNo.Value;
                        hdfAppType.Value = AST_CODE.Value;
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
            int Status = 0;
            int PDCStatus = 0;
            CommonService CommonServiceClient;
            CommonServiceClient = null;
            AdmCompanyMstService admCompanyMstServiceClient;
            FinTrxService FinTrxServiceClient = null;
            int TotalRecords = 0;
            string xmlDocInv = string.Empty;

            try
            {
                FinTrxServiceClient = new FinTrxService();
                FinTrxServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(FinTrxServiceClient);
                switch (type)
                {
                    #region PAYMENTHDRLIST
                    case ControlsEnum.PAYMENTHDRLIST:
                        TotalRecords = 0;
                        int vendorID = String.IsNullOrEmpty(hdfVendorID.Value.Trim()) ? 0 : Convert.ToInt32(hdfVendorID.Value);
                        if (hdfVendorID.Value != "" && hdfVendorID.Value != "0")
                        {
                            Session[ERP.Utilities.SessionStrings.VendorPK] = hdfVendorID.Value;
                            Session[ERP.Utilities.SessionStrings.Vendor] = txtVendor.Text;
                        }
                        int paymentPk = string.IsNullOrEmpty(hdfPaymentPK.Value) ? 0 : Convert.ToInt32(hdfPaymentPK.Value);
                        Status = Convert.ToInt32(ddlStatus.SelectedValue);
                        PDCStatus = Convert.ToInt32(ddlPDCStatus.SelectedValue);
                         string invNo = string.Empty;
                         invNo =string.IsNullOrEmpty(txtSINo.Text)?string.Empty : txtSINo.Text.Trim(); 
                        string vendor = string.IsNullOrEmpty(txtVendor.Text.Trim()) ? string.Empty : (txtVendor.Text.Trim() == "Select/Type" ? string.Empty : txtVendor.Text.Trim());
                        dsPageData = BusinessLogic.POInvoicing.POPaymentTradingBL.GetPaymentTradingList(
                            new BusinessObject.GridPrams()
                            {
                                SortBy = SortBy = SortBy == null ? Resources.DataFieldRes.POPaymentDate : SortBy,
                                SortDirection = string.IsNullOrEmpty(SortDirection) ? Resources.Report.SortDescending : SortDirection,  
                                FromDate = string.IsNullOrEmpty(txtSearchDateFrom.Text.Trim()) ? string.Empty : txtSearchDateFrom.Text.Trim(),
                                ToDate = string.IsNullOrEmpty(txtSearchDateTo.Text.Trim()) ? string.Empty : txtSearchDateTo.Text.Trim(),
                                SearchBy = "PVH_NO",
                                SearchValue = string.IsNullOrEmpty(txtPaymentNumber.Text.Trim()) ? string.Empty : (txtPaymentNumber.Text.Trim() == "Select/Type" ? string.Empty : txtPaymentNumber.Text.Trim()),
                                PageNumber = uclPaging.CurrentPage == 0 ? 1 : uclPaging.CurrentPage,
                                PageSize = Convert.ToInt32(GetLocalResourceObject("PageSize_PaymentList"))
                            }, currentUser, vendorID, paymentPk, invNo, Resources.PageURL.PoPaymentTrading.Replace("~", ""), Status,PDCStatus);

                        if (dsPageData != null && dsPageData.Tables.Count > 0)
                        {
                            DataView dvPayment = dsPageData.Tables[1].DefaultView;
                            dtPaymentList = dvPayment.ToTable();
                            int pagsize = Convert.ToInt32(GetLocalResourceObject("PageSize_PaymentList"));
                            TotalRecords = dsPageData.Tables[0].Rows.Count > 0 ? Convert.ToInt32(dsPageData.Tables[0].Rows[0][0].ToString()) : 0;
                            TotalPages = TotalRecords == 0 ? 1 : (TotalRecords <= pagsize) ? 1 :
                                        (TotalRecords % pagsize) == 0 ? (TotalRecords / pagsize) :
                                        (TotalRecords / pagsize) + 1;
                        }
                        break;
                    #endregion
                    #region PENDINGPOLIST
                    case ControlsEnum.PENDINGINVLIST:
                        TotalPages = 0;
                        vendPK = 0;
                        purchaseInvoicePK = 0;
                        int.TryParse(hdfVendorHd.Value, out vendPK);
                        int.TryParse(hdfPurchaseInvPK.Value, out purchaseInvoicePK);
                        int currentPage = string.IsNullOrEmpty(PageIndexInv) ? 1 : Convert.ToInt32(PageIndexInv);
                        int pageSize = Convert.ToInt32(GetLocalResourceObject("PageSize_InvList")); 
                        DateTime? fromDate = string.IsNullOrEmpty(txtPendingFromDate.Text.Trim()) ? (DateTime?)null : Convert.ToDateTime(txtPendingFromDate.Text.Trim());
                        DateTime? todate = string.IsNullOrEmpty(txtPendingToDate.Text.Trim()) ? (DateTime?)null : Convert.ToDateTime(txtPendingToDate.Text.Trim());
                        int invCategory = (ddlPendingInvCategory.SelectedValue != CommonConstants.SELECTVAL ? Convert.ToInt32(ddlPendingInvCategory.SelectedValue) : 0);
                        int invType = (ddlPendingInvType.SelectedValue != CommonConstants.SELECTVAL ? Convert.ToInt32(ddlPendingInvType.SelectedValue) : 0);                      
                            dtPendingInvList = BusinessLogic.POInvoicing.POPaymentTradingBL.GetPendingInvList(currentUser, vendPK, purchaseInvoicePK, currentPage, pageSize, fromDate, todate, invCategory, invType);  
                        break;
                    #endregion
                    #region INVPAYMENTHEADER (Getting Details of added MultipleINVPKs)
                    case ControlsEnum.INVPAYMENTHEADER:
                        xmlDocInv = string.Empty;
                        if (InvHeaderObj != null && InvHeaderObj.INVList != null && InvHeaderObj.INVList.Count > 0)
                        {
                            xmlDocInv = CommonFunctions.XmlSerialize<TradingInvHeaderBO>(InvHeaderObj);
                        }
                        paymentHeaderObj = BusinessLogic.POInvoicing.POPaymentTradingBL.GetTradingPurchaseInvoiceHeaderMUL(xmlDocInv, !string.IsNullOrEmpty(xmlDocInv) ? 0 : CurrPK);

                        if (InvPaymentHeaderSession == null)
                            InvPaymentHeaderSession = paymentHeaderObj.DeepClone();
                        else if (paymentHeaderObj != null)
                        {
                            List<string> objInvList = InvPaymentHeaderSession.TrxMpg.Select(r => r.PVM_INVOICE_HDR.ToString()).Distinct().ToList();
                            InvPaymentHeaderSession.TrxMpg.AddRange(paymentHeaderObj.TrxMpg.Where(r => !objInvList.Contains(r.PVM_INVOICE_HDR.ToString())).ToList());
                        }
                        break;
                    #endregion  
                    #region PAYMENTADJN
                    case ControlsEnum.PAYMENTADJN:                        
                        dtPendingDrAdjn = BusinessLogic.POInvoicing.POPaymentTradingBL.GetVendorDebitNoteAlcnForPaymentAdjn(Convert.ToInt32(hdfVendorHd.Value), CurrPK);//SPFIN_PAYMENT_VND_ALCN_GET                       
                        CrDrAdjnList = new List<PaymentAdjnAllocation>();
                        for (int i = 0; i < dtPendingDrAdjn.Rows.Count; i++)
                        {
                            PaymentAdjnAllocation paymentVndTrxAdjnObj = new PaymentAdjnAllocation();
                           paymentVndTrxAdjnObj.PAA_AMOUNT = Convert.ToDecimal(dtPendingDrAdjn.Rows[i]["PAA_AMOUNT"]);
                           paymentVndTrxAdjnObj.PAA_CRDRPK = Convert.ToInt64(dtPendingDrAdjn.Rows[i]["PAA_CRDRPK"]);
                           paymentVndTrxAdjnObj.PAA_NO = Convert.ToString(dtPendingDrAdjn.Rows[i]["PAA_NO"]);
                           paymentVndTrxAdjnObj.PAA_TRXPK = Convert.ToInt64(dtPendingDrAdjn.Rows[i]["PAA_TRXPK"]);
                           paymentVndTrxAdjnObj.PAA_DATE = Convert.ToDateTime(dtPendingDrAdjn.Rows[i]["PAA_DATE"]);
                           paymentVndTrxAdjnObj.PAA_TYPE = Convert.ToString(dtPendingDrAdjn.Rows[i]["PAA_TYPE"]);
                           CrDrAdjnList.Add(paymentVndTrxAdjnObj);
                        }
                      
                        break;
                    #endregion
                    #region PAYMENTADJNDUMMYLIST
                    case ControlsEnum.PAYMENTADJNDUMMYLIST:
                        //poPaymentServiceClient = new POPaymentService();
                        //poPaymentServiceClient = CommonFunctions.InitiateClient(poPaymentServiceClient);
                        //FinPaymentVndAllocationObj = CommonFunctions.Initilize<FIN_PAYMENT_VND_ALCN_DTL>();
                        //FinPaymentVndAllocationList = poPaymentServiceClient.GetFinPaymentAlcnListContext(PaymentAdjnList.Where(dtl => dtl.FIN_PAYMENT_VND_TRX_MPG.PVM_INVOICE_HDR == InvoicePK).ToList());
                        break;
                    #endregion
                    #region INVOICEVNDMPGLIST FOR AUTO ALLOCATION
                    case ControlsEnum.INVOICEVNDMPGLISTFORAUTOALCN:                        
                        DirectPOInvoiceHeader invoiceHeaderObj = BusinessLogic.POInvoicing.POInvoiceBL.GetDirectPurchaseInvoiceHeaderMUL(string.Empty, Convert.ToInt32(InvoicePK));
                        if (invoiceHeaderObj != null)
                        {
                            FinInvoiceVndTrxMpgListForAutoAlcn = invoiceHeaderObj.POMappingDetails.ToList();
                        }
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
                    #region BANK
                    case ControlsEnum.BANK:
                        bankMstServiceClient = new BankMstService();
                        bankMstServiceClient = CommonFunctions.InitiateClient(bankMstServiceClient);
                        short bankPk = string.IsNullOrEmpty(hdfPaymentBank.Value) ? Convert.ToInt16(0) : Convert.ToInt16(hdfPaymentBank.Value);
                        finCashBankMstList = bankMstServiceClient.GetFinBankMstByPK(bankPk);
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
                    #region Base Currency
                    case ControlsEnum.BASECURRENCY:
                        CurrencyMstService CurrencyMstServiceClient = new CurrencyMstService();
                        string BaseCurrency = CurrencyMstServiceClient.GetCurrencyCodeName(currentUser.BaseCurrency);
                        hdfBaseCurrency.Value = BaseCurrency;
                        break;
                    #endregion

                    #region PAYMODE
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

                    #region VENDORACCOUNT
                    case ControlsEnum.VENDORACCOUNT:
                        dtVendorAccount = CommonBL.GetTaxMstList(0, (int)TaxType.Tax, (int)TaxSubCategory.WHT, (byte)DbActiveStatus.ACTIVE, currentUser.SBUID);
                        break;
                    #endregion
                    #region VENDORACCOUNTTAX
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
                        purVendorMstList = CommonServiceClient.GetVendor(Convert.ToInt32(hdfVendorHd.Value));
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
                    #region FINHEADERSTATUS
                    case ControlsEnum.FINHEADERSTATUS:
                        finTrxServiceClient = new FinTrxService();
                        finTrxServiceClient = CommonFunctions.InitiateClient(finTrxServiceClient);
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.PageSize = 10;
                        finTrxHdrObj = ERP.Utilities.CommonFunctions.Initilize<FIN_TRX_HDR>();
                        finTrxHdrObj.FTH_REF_TYPE = invGroup == 1 ? finTrxHdrObj.FTH_REF_TYPE = ApplicationType.VPJ : invGroup == 2 ? finTrxHdrObj.FTH_REF_TYPE = ApplicationType.SIPJ :
                            invGroup == 4 ? finTrxHdrObj.FTH_REF_TYPE = ApplicationType.AIPJ : finTrxHdrObj.FTH_REF_TYPE = ApplicationType.EIPJ;
                        finTrxHdrObj.FTH_REF_PK = CurrPK;
                        finTrxHdrObj.FTH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        finTrxHdrList = finTrxServiceClient.GetSatusByAppPK(finTrxHdrObj);
                        break;
                    #endregion
                    #region  DISCOUNTTYPE(ADJTYPE )
                    case ControlsEnum.DISCOUNTTYPE:
                        dtDiscountTypes = BusinessLogic.CommonManagement.CommonBL.GetConstMstValues(0, 0, ConstGroupType.ReceiptAdjustments, (int)Adjustments.Payment, 1, currentUser.SBUID);
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

                        //bool blnIsVatbuyExist = false;
                        //try
                        //{
                        //   // GetFieldValues(ControlsEnum.PAYMENTHDRENTRYBYPK);                          
                        //    if (InvPaymentHeaderSession.TaxHdr.Where(whtpynt => whtpynt.WTH_CATEGORY == (byte)WHTCategoryEnum.VATBUY).Count() > 0)
                        //    {
                        //        blnIsVatbuyExist = true;
                        //    }
                        //}
                        //catch
                        //{
                        //    blnIsVatbuyExist = false;
                        //}                     
                        //tempVATTaxDetails = null;
                        //tempVATTaxDetails = TempVATTaxDetails;

                        //List<PaymentTaxHeader> objTempVATTaxDetails = new List<PaymentTaxHeader>();
                        //List<PaymentTaxHeader> objtempVATTaxDetails = new List<PaymentTaxHeader>();

                        //finPaymentVndTrxMpgList = new List<PaymentInvoiceTrxMpgDetails>();
                        //finPaymentVndTrxMpgList = (List<PaymentInvoiceTrxMpgDetails>)SetUIValuesToObject(ControlsEnum.PAYMENTMPGENTRY);
                      
                        //if (FinInvoiceVndHdrSelectedList != null)
                        //{

                        //    foreach (FIN_INVOICE_VND_HDR finInvVndHdr in FinInvoiceVndHdrSelectedList)
                        //    {
                        //        VatBuyTaxAmntTotal = 0;
                        //        decimal balToPay = 0;
                        //        decimal taxAmount = 0;
                        //        string itemName = "";
                        //        int? VendorContactPk = null;
                        //        tempVATTax = new PaymentTaxHeader();
                        //        tempVATTax = CommonFunctions.Initilize<PaymentTaxHeader>();
                        //        List<FIN_INVOICE_VND_TAX_HDR> finInvVndTaxList = finInvVndHdr.FIN_INVOICE_VND_TAX_HDR.ToList();
                        //        List<FIN_INVOICE_VND_DTL> finInvVndDtlList = finInvVndHdr.FIN_INVOICE_VND_DTL.ToList();

                        //        List<PaymentInvoiceTrxMpgDetails> finPymntTrxMpg = finPaymentVndTrxMpgList.Where(fpvtm => fpvtm.PVM_INVOICE_HDR == finInvVndHdr.IVH_PK).ToList();

                        //        if (finInvVndTaxList != null && finInvVndTaxList.Count > 0)
                        //        {                                  
                        //            try
                        //            {
                        //                tempVATTax.WTH_TAX = finInvVndTaxList.SingleOrDefault(objTax => (objTax.VTH_TYPE == (byte)WhtTypeEnum.DEFINEDTAX) && (objTax.VTH_TAX == (byte)TaxTypes.VATBuy || objTax.VTH_TAX == (byte)TaxTypes.VATBuyNotYetDue)).VTH_TAX;
                        //                tempVATTax.WTH_NAME = finInvVndTaxList.SingleOrDefault(objTax => (objTax.VTH_TYPE == (byte)WhtTypeEnum.DEFINEDTAX) && (objTax.VTH_TAX == (byte)TaxTypes.VATBuy || objTax.VTH_TAX == (byte)TaxTypes.VATBuyNotYetDue)).VTH_NAME;
                        //                tempVATTax.WTH_TAX_CATEGORY = finInvVndTaxList.SingleOrDefault(objTax => (objTax.VTH_TYPE == (byte)WhtTypeEnum.DEFINEDTAX) && (objTax.VTH_TAX == (byte)TaxTypes.VATBuy || objTax.VTH_TAX == (byte)TaxTypes.VATBuyNotYetDue)).VTH_TAX_CATEGORY;
                        //            }
                        //            catch
                        //            {
                        //                tempVATTax.WTH_TAX = finInvVndTaxList[0].VTH_TAX;
                        //                tempVATTax.WTH_NAME = finInvVndTaxList[0].VTH_NAME;
                        //                tempVATTax.WTH_TAX_CATEGORY = finInvVndTaxList[0].VTH_TAX_CATEGORY;
                        //            }
                        //        }


                        //        poPaymentServiceClient = new POPaymentService();
                        //        poPaymentServiceClient = CommonFunctions.InitiateClient(poPaymentServiceClient);
                        //        finInvoiceVndHdrObjForPaymentSplit = CommonFunctions.Initilize<FIN_INVOICE_VND_HDR>();
                        //        finInvoiceVndHdrObjForPaymentSplit.IVH_PK = finInvVndHdr.IVH_PK;
                        //        finInvoiceVndHdrObjForPaymentSplit.IVH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        //        finInvoiceVndHdrListForPaymentSplit = poPaymentServiceClient.GetInvoiceHdrByPK(finInvoiceVndHdrObjForPaymentSplit);
                        //        if (finInvoiceVndHdrListForPaymentSplit != null && finInvoiceVndHdrListForPaymentSplit.Count > 0)
                        //        {
                        //            if (finInvoiceVndHdrListForPaymentSplit[0].FIN_INVOICE_VND_DTL != null && finInvoiceVndHdrListForPaymentSplit[0].FIN_INVOICE_VND_DTL.Count > 0)
                        //            {
                        //                foreach (FIN_INVOICE_VND_DTL finInvDtl in finInvoiceVndHdrListForPaymentSplit[0].FIN_INVOICE_VND_DTL)
                        //                {
                        //                    if (finInvDtl.FIN_INVOICE_VND_TAX_DTL != null && finInvDtl.FIN_INVOICE_VND_TAX_DTL.Count > 0)
                        //                    {
                        //                        VatBuyTaxAmntTotal += finInvDtl.FIN_INVOICE_VND_TAX_DTL.Where(tx => tx.VTL_TAX == (int)(byte)TaxTypes.VATBuyNotYetDue).Sum(tax => tax.VTL_TAX_AMT);
                        //                    }
                        //                }
                        //            }
                        //            if (finInvoiceVndHdrListForPaymentSplit[0].FIN_INVOICE_VND_TAX_HDR != null && finInvoiceVndHdrListForPaymentSplit[0].FIN_INVOICE_VND_TAX_HDR.Count > 0)
                        //            {
                        //                VatBuyTaxAmntTotal += finInvoiceVndHdrListForPaymentSplit[0].FIN_INVOICE_VND_TAX_HDR.Where(tx => tx.VTH_TAX == (int)(byte)TaxTypes.VATBuyNotYetDue).Sum(tax => tax.VTH_TAX_AMT);
                        //            }
                        //            if (finInvVndHdr.IVH_CATEGORY == (int)POInvoiceCategory.Advanced)
                        //            {
                        //                VatBuyTaxAmntTotal += finInvoiceVndHdrListForPaymentSplit[0].FIN_INVOICE_VND_TRX_MPG.Sum(tx => tx.IVM_TAX_AMOUNT);
                        //            }                                    
                        //        }


                        //        if (finPymntTrxMpg != null && finPymntTrxMpg.Count > 0)
                        //        {
                        //            tempVATTax.WTH_AMOUNT = finPymntTrxMpg[0].PVM_PAID_AMOUNT - finPymntTrxMpg[0].PVM_OTHER_AMOUNT - finPymntTrxMpg[0].PVM_TAX_AMOUNT;
                                 
                        //            tempVATTax.WTH_TAX_AMT = VatBuyTaxAmntTotal;                                  
                        //        }
                               
                        //        tempVATTax.WTH_PK = 0;
                        //        tempVATTax.WTH_PAYMENT_HDR = CurrPK;
                        //        tempVATTax.WTH_TYPE = (byte)WhtTypeEnum.DEFINEDTAX;
                        //        tempVATTax.WTH_CATEGORY = (byte)WHTCategoryEnum.VATBUY;

                        //        tempVATTax.WTH_PUR_INVOICE = finInvVndHdr.IVH_PK;
                        //        tempVATTax.WTH_TAX_INV_NO = finInvVndHdr.IVH_VENDOR_INV_NO;
                        //        tempVATTax.WTH_INV_RECEIVED = finInvVndHdr.IVH_ORGINAL_RCVD;

                        //        tempVATTax.WTH_BRANCH_TEXT = txtBranchCode.Text;
                        //        vendorPk = finInvVndHdr.IVH_VENDOR;
                        //        GetFieldValues(ControlsEnum.VENDORINVDTL);
                        //        if (VendorDetails != null && VendorDetails.Rows.Count > 0)
                        //        {
                        //            tempVATTax.WTH_BRANCH_TYPE = Convert.ToByte(VendorDetails.Rows[0][Resources.DataFieldRes.vncType]);
                        //        }
                        //        tempVATTax.WTH_TAX_DATE = finInvVndHdr.IVH_DATE;
                        //        tempVATTax.WTH_REFUND_DATE = finInvVndHdr.IVH_DATE;
                        //        tempVATTax.WTH_PARTY_NAME = finInvVndHdr.PUR_VENDOR_MST.VEN_NAME;                              
                        //        tempVATTax.WTH_TAX_ID = finInvVndHdr.IVH_TAX_ID;
                        //        tempVATTax.WTH_VENDOR = finInvVndHdr.IVH_VENDOR;
                        //        hdfVendorPopup.Value = finInvVndHdr.IVH_VENDOR.ToString();
                        //        GetFieldValues(ControlsEnum.VENDORCONTACTYPE);
                        //        if (dtAdsType != null && dtAdsType.Rows.Count > 0)
                        //        {
                        //            if (!string.IsNullOrEmpty(dtAdsType.Rows[0][Resources.DataFieldRes.VncPk].ToString()))
                        //                VendorContactPk = Convert.ToInt32(dtAdsType.Rows[0][Resources.DataFieldRes.VncPk]);
                        //            tempVATTax.WTH_BRANCH = VendorContactPk;
                        //            tempVATTax.WTH_BRANCH_NAME = dtAdsType.Rows[0][Resources.DataFieldRes.VncName].ToString();
                        //            tempVATTax.WTH_BRANCH_TEXT = dtAdsType.Rows[0][Resources.DataFieldRes.VncTypeName].ToString();
                        //        }

                        //        try
                        //        {
                        //            if (finInvVndHdr.IVH_GROUP == (int)POInvoiceGroup.Expense)
                        //            {
                        //                if (finInvVndDtlList != null && finInvVndDtlList.Count > 0)
                        //                {
                        //                    foreach (FIN_INVOICE_VND_DTL finInvExpDtl in finInvVndDtlList)
                        //                    {
                        //                        PaymentTaxHeader tempVATTaxExp = new PaymentTaxHeader();
                        //                        tempVATTaxExp = CommonFunctions.Initilize<PaymentTaxHeader>();

                        //                        itemName = finInvExpDtl.VID_INSTRUCTIONS;
                        //                        tempVATTaxExp.WTH_TAX_ID = finInvExpDtl.VID_TAX_ID;
                        //                        tempVATTaxExp.WTH_BRANCH_TEXT = finInvExpDtl.VID_BRANCH_TEXT;
                        //                        tempVATTaxExp.WTH_BRANCH_TYPE = finInvExpDtl.VID_BRANCH_TYPE;
                        //                        tempVATTaxExp.WTH_BRANCH_NAME = finInvExpDtl.VID_BRANCH_NAME;
                        //                        tempVATTaxExp.WTH_BRANCH = finInvExpDtl.VID_BRANCH;
                        //                        tempVATTaxExp.WTH_TAX_INV_NO = finInvExpDtl.VID_REF_NO;
                        //                        tempVATTaxExp.WTH_INV_RECEIVED = finInvExpDtl.FIN_INVOICE_VND_HDR.IVH_ORGINAL_RCVD;                                              
                        //                        tempVATTaxExp.WTH_PARTY_NAME = finInvExpDtl.VID_VENDOR_TEXT;
                        //                        tempVATTaxExp.WTH_VENDOR = finInvExpDtl.VID_VENDOR;
                        //                        tempVATTaxExp.WTH_AMOUNT = finInvExpDtl.VID_AMOUNT;                                            
                        //                        tempVATTaxExp.WTH_TAX_AMT = 0;
                        //                        if (finInvoiceVndHdrListForPaymentSplit != null && finInvoiceVndHdrListForPaymentSplit.Count > 0)
                        //                        {
                        //                            FIN_INVOICE_VND_DTL finInvDtl = finInvoiceVndHdrListForPaymentSplit[0].FIN_INVOICE_VND_DTL.SingleOrDefault(inv => inv.VID_PK == finInvExpDtl.VID_PK);
                        //                            decimal totalHeaderTax = 0, SubTotalAmount = 0, LineItemAmount = 0, HeaderTaxAdding = 0;
                        //                            if (finInvDtl != null)
                        //                            {
                        //                                if (finInvDtl.FIN_INVOICE_VND_TAX_DTL != null && finInvDtl.FIN_INVOICE_VND_TAX_DTL.Count > 0)
                        //                                {
                        //                                    tempVATTaxExp.WTH_TAX_AMT = finInvDtl.FIN_INVOICE_VND_TAX_DTL.Where(tx => tx.VTL_TAX == (int)(byte)TaxTypes.VATBuyNotYetDue).Sum(tax => tax.VTL_TAX_AMT);
                        //                                }
                        //                                LineItemAmount = finInvDtl.VID_AMOUNT;
                        //                            }
                                                                                                                                                             
                        //                            if (finInvoiceVndHdrListForPaymentSplit[0].FIN_INVOICE_VND_TAX_HDR != null && finInvoiceVndHdrListForPaymentSplit[0].FIN_INVOICE_VND_TAX_HDR.Count > 0)
                        //                            {
                        //                                totalHeaderTax = finInvoiceVndHdrListForPaymentSplit[0].FIN_INVOICE_VND_TAX_HDR.Where(tx => tx.VTH_TAX == (int)(byte)TaxTypes.VATBuyNotYetDue).Sum(tax => tax.VTH_TAX_AMT);
                        //                                SubTotalAmount = finInvoiceVndHdrListForPaymentSplit[0].IVH_AMOUNT_TC;
                        //                                HeaderTaxAdding = (totalHeaderTax / SubTotalAmount) * LineItemAmount;
                        //                                tempVATTaxExp.WTH_TAX_AMT += HeaderTaxAdding;
                        //                            }

                        //                        }

                        //                        tempVATTaxExp.WTH_ITEM_TEXT = itemName;
                        //                        tempVATTaxExp.WTH_PK = 0;
                        //                        tempVATTaxExp.WTH_PAYMENT_HDR = CurrPK;
                        //                        tempVATTaxExp.WTH_TYPE = (byte)WhtTypeEnum.DEFINEDTAX;
                        //                        tempVATTaxExp.WTH_CATEGORY = (byte)WHTCategoryEnum.VATBUY;
                        //                        tempVATTaxExp.WTH_PUR_INVOICE = finInvVndHdr.IVH_PK;
                        //                        tempVATTaxExp.WTH_TAX_DATE = finInvExpDtl.VID_REF_DATE;
                        //                        tempVATTaxExp.WTH_REFUND_DATE = finInvExpDtl.VID_DATE;
                        //                        try
                        //                        {
                        //                            if (finInvExpDtl.FIN_INVOICE_VND_TAX_DTL != null && finInvExpDtl.FIN_INVOICE_VND_TAX_DTL.Count > 0)
                        //                            {
                        //                                tempVATTaxExp.WTH_TAX = finInvExpDtl.FIN_INVOICE_VND_TAX_DTL.SingleOrDefault(objTax => (objTax.VTL_TYPE == (byte)WhtTypeEnum.DEFINEDTAX) && (objTax.VTL_TAX == (byte)TaxTypes.VATBuy || objTax.VTL_TAX == (byte)TaxTypes.VATBuyNotYetDue)).VTL_TAX;
                        //                                tempVATTaxExp.WTH_NAME = finInvExpDtl.FIN_INVOICE_VND_TAX_DTL.SingleOrDefault(objTax => (objTax.VTL_TYPE == (byte)WhtTypeEnum.DEFINEDTAX) && (objTax.VTL_TAX == (byte)TaxTypes.VATBuy || objTax.VTL_TAX == (byte)TaxTypes.VATBuyNotYetDue)).VTL_NAME;
                        //                                tempVATTaxExp.WTH_TAX_CATEGORY = finInvExpDtl.FIN_INVOICE_VND_TAX_DTL.SingleOrDefault(objTax => (objTax.VTL_TYPE == (byte)WhtTypeEnum.DEFINEDTAX) && (objTax.VTL_TAX == (byte)TaxTypes.VATBuy || objTax.VTL_TAX == (byte)TaxTypes.VATBuyNotYetDue)).VTL_TAX_CATEGORY;
                        //                            }                                                
                        //                            if (!tempVATTaxExp.WTH_TAX.HasValue)
                        //                            {
                        //                                tempVATTaxExp.WTH_TAX = finInvExpDtl.FIN_INVOICE_VND_HDR.FIN_INVOICE_VND_TAX_HDR.SingleOrDefault(objTax => (objTax.VTH_TYPE == (byte)WhtTypeEnum.DEFINEDTAX) && (objTax.VTH_TAX == (byte)TaxTypes.VATBuy || objTax.VTH_TAX == (byte)TaxTypes.VATBuyNotYetDue)).VTH_TAX;
                        //                                tempVATTaxExp.WTH_NAME = finInvExpDtl.FIN_INVOICE_VND_HDR.FIN_INVOICE_VND_TAX_HDR.SingleOrDefault(objTax => (objTax.VTH_TYPE == (byte)WhtTypeEnum.DEFINEDTAX) && (objTax.VTH_TAX == (byte)TaxTypes.VATBuy || objTax.VTH_TAX == (byte)TaxTypes.VATBuyNotYetDue)).VTH_NAME;
                        //                                tempVATTaxExp.WTH_TAX_CATEGORY = finInvExpDtl.FIN_INVOICE_VND_HDR.FIN_INVOICE_VND_TAX_HDR.SingleOrDefault(objTax => (objTax.VTH_TYPE == (byte)WhtTypeEnum.DEFINEDTAX) && (objTax.VTH_TAX == (byte)TaxTypes.VATBuy || objTax.VTH_TAX == (byte)TaxTypes.VATBuyNotYetDue)).VTH_TAX_CATEGORY;

                        //                            }

                        //                            if (tempVATTaxExp.WTH_TAX.HasValue)
                        //                            {
                        //                                TaxPk = Convert.ToInt32(tempVATTaxExp.WTH_TAX);
                        //                                GetFieldValues(ControlsEnum.TAXDETAILS);
                        //                                if (dtTaxMst != null && dtTaxMst.Rows.Count > 0 && Convert.ToInt32(dtTaxMst.Rows[0][Resources.DataFieldRes.TaxNotDue]) == (int)TaxEnum.TaxNotYetDue)
                        //                                {
                        //                                    objtempVATTaxDetails.Add(tempVATTaxExp);
                        //                                    objTempVATTaxDetails = objtempVATTaxDetails;
                        //                                }
                        //                            }

                        //                        }
                        //                        catch
                        //                        {

                        //                        }
                        //                    }
                        //                }
                        //            }
                        //            else
                        //            {
                        //                if (finInvVndHdr.IVH_CATEGORY == (int)POInvoiceCategory.Invoice)
                        //                {
                        //                    if (finInvVndDtlList != null && finInvVndDtlList.Count > 0)
                        //                    {
                        //                        if (finInvVndDtlList[0].INV_ITEM_MST != null)
                        //                        {
                        //                            itemName = finInvVndDtlList[0].INV_ITEM_MST.ITM_NAME;
                        //                        }
                        //                    }
                        //                }
                        //                else if (finInvVndHdr.IVH_CATEGORY == (int)POInvoiceCategory.Advanced)
                        //                {
                        //                    itemName = finInvVndHdr.FIN_INVOICE_VND_TRX_MPG.ToList()[0].PUR_ORDER_HDR.PUR_ORDER_DTL.ToList()[0].INV_ITEM_MST.ITM_NAME;
                        //                    tempVATTax.WTH_TAX = finInvVndHdr.FIN_INVOICE_VND_TRX_MPG.ToList()[0].PUR_ORDER_HDR.PUR_ORDER_TAX_HDR.ToList()[0].PTH_TAX;
                        //                    tempVATTax.WTH_NAME = finInvVndHdr.FIN_INVOICE_VND_TRX_MPG.ToList()[0].PUR_ORDER_HDR.PUR_ORDER_TAX_HDR.ToList()[0].PTH_NAME;
                        //                    tempVATTax.WTH_TAX_CATEGORY = finInvVndHdr.FIN_INVOICE_VND_TRX_MPG.ToList()[0].PUR_ORDER_HDR.PUR_ORDER_TAX_HDR.ToList()[0].PTH_TAX_CATEGORY;
                        //                }
                        //                tempVATTax.WTH_ITEM_TEXT = itemName;                                     
                        //                if (tempVATTax.WTH_TAX.HasValue)
                        //                {
                        //                    TaxPk = Convert.ToInt32(tempVATTax.WTH_TAX);
                        //                    GetFieldValues(ControlsEnum.TAXDETAILS);
                        //                    if (dtTaxMst != null && dtTaxMst.Rows.Count > 0 && Convert.ToInt32(dtTaxMst.Rows[0][Resources.DataFieldRes.TaxNotDue]) == (int)TaxEnum.TaxNotYetDue)
                        //                    {
                        //                        objtempVATTaxDetails.Add(tempVATTax);
                        //                        objTempVATTaxDetails = objtempVATTaxDetails;
                        //                    }
                        //                }
                        //            }

                        //        }
                        //        catch { }

                        //    }
                        //    var finPymntTaxHdr = from finObj in objTempVATTaxDetails
                        //                         group finObj by new
                        //                         {
                        //                             finObj.WTH_PARTY_NAME,
                        //                             finObj.WTH_TAX_INV_NO,
                        //                             finObj.WTH_TAX_DATE
                        //                         } into finGrpdObj
                        //                         select new PaymentTaxHeader
                        //                         {
                        //                             WTH_PARTY_NAME = finGrpdObj.Key.WTH_PARTY_NAME,
                        //                             WTH_TAX_INV_NO = finGrpdObj.Key.WTH_TAX_INV_NO,
                        //                             WTH_INV_RECEIVED = finGrpdObj.FirstOrDefault().WTH_INV_RECEIVED,
                        //                             WTH_ADDRESS = finGrpdObj.FirstOrDefault().WTH_ADDRESS,
                        //                             WTH_BRANCH = finGrpdObj.FirstOrDefault().WTH_BRANCH,
                        //                             WTH_BRANCH_NAME = finGrpdObj.FirstOrDefault().WTH_BRANCH_NAME,
                        //                             WTH_BRANCH_TEXT = finGrpdObj.FirstOrDefault().WTH_BRANCH_TEXT,
                        //                             WTH_BRANCH_TYPE = finGrpdObj.FirstOrDefault().WTH_BRANCH_TYPE,
                        //                             WTH_CATEGORY = finGrpdObj.FirstOrDefault().WTH_CATEGORY,
                        //                             WTH_DESC = finGrpdObj.FirstOrDefault().WTH_DESC,
                        //                             WTH_FORM_NO = finGrpdObj.FirstOrDefault().WTH_FORM_NO,
                        //                             WTH_ITEM_TEXT = finGrpdObj.FirstOrDefault().WTH_ITEM_TEXT,
                        //                             WTH_NAME = finGrpdObj.FirstOrDefault().WTH_NAME,
                        //                             WTH_PAYMENT_HDR = finGrpdObj.FirstOrDefault().WTH_PAYMENT_HDR,
                        //                             WTH_PK = finGrpdObj.FirstOrDefault().WTH_PK,
                        //                             WTH_PUR_INVOICE = finGrpdObj.FirstOrDefault().WTH_PUR_INVOICE,
                        //                             WTH_TAX = finGrpdObj.FirstOrDefault().WTH_TAX,
                        //                             WTH_TAX_CATEGORY = finGrpdObj.FirstOrDefault().WTH_TAX_CATEGORY,
                        //                             WTH_TAX_DATE = finGrpdObj.FirstOrDefault().WTH_TAX_DATE,
                        //                             WTH_REFUND_DATE = finGrpdObj.FirstOrDefault().WTH_REFUND_DATE,
                        //                             WTH_TAX_ID = finGrpdObj.FirstOrDefault().WTH_TAX_ID,
                        //                             WTH_TRX_HDR = finGrpdObj.FirstOrDefault().WTH_TRX_HDR,
                        //                             WTH_TYPE = finGrpdObj.FirstOrDefault().WTH_TYPE,
                        //                             WTH_VENDOR = finGrpdObj.FirstOrDefault().WTH_VENDOR,
                        //                             WTH_AMOUNT = finGrpdObj.Sum(x => x.WTH_AMOUNT),
                        //                             WTH_TAX_AMT = finGrpdObj.Sum(x => x.WTH_TAX_AMT)
                        //                         };                        
                        //    objTempVATTaxDetails = finPymntTaxHdr.ToList();
                        //    if (TempVATTaxDetails != null && TempVATTaxDetails.Count > 0)
                        //    {
                        //        foreach (PaymentTaxHeader ObjfinPymnt in objTempVATTaxDetails)
                        //        {
                        //            try
                        //            {
                        //                int RowIndex = TempVATTaxDetails.FindIndex(objtemp => objtemp.WTH_TAX_DATE == ObjfinPymnt.WTH_TAX_DATE && objtemp.WTH_TAX_INV_NO == ObjfinPymnt.WTH_TAX_INV_NO && objtemp.WTH_PARTY_NAME == ObjfinPymnt.WTH_PARTY_NAME);
                        //                if (RowIndex >= 0 && !blnIsVatbuyExist)
                        //                {
                        //                    TempVATTaxDetails[RowIndex].WTH_AMOUNT = ObjfinPymnt.WTH_AMOUNT;
                        //                    TempVATTaxDetails[RowIndex].WTH_TAX_AMT = ObjfinPymnt.WTH_TAX_AMT;

                        //                }
                        //                if (TempVATTaxDetails.Where(objtemp => objtemp.WTH_PUR_INVOICE.Value == ObjfinPymnt.WTH_PUR_INVOICE).Count() == 0)
                        //                    TempVATTaxDetails.Add(ObjfinPymnt);
                        //            }
                        //            catch { }
                        //        }

                        //    }
                        //    else
                        //    {
                        //        TempVATTaxDetails = objTempVATTaxDetails;
                        //    }
                        //}

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
                        int.TryParse(hdfVendorHd.Value, out vendorPk);
                        //int.TryParse(ddlvendorBank.SelectedValue, out VendorbankPk);
                        dtVendorBanks = BusinessLogic.VendorManagement.VendorMaster.GetVendorBanks(currentUser, VendorBankPk, vendorPk, Convert.ToInt16(DbActiveStatus.ACTIVE));

                        break;
                    #endregion                   
                    #region TAXDETAILS
                    case ControlsEnum.TAXDETAILS:
                        dtTaxMst = BusinessLogic.Administration.Masters.TaxSettingsMaster.GetTaxDetails(TaxPk, 0, currentUser.SBUID, (int)DbActiveStatus.HASPK);
                        break;
                    #endregion
                    #region VATBUYPOPUPHEADER
                    case ControlsEnum.VATBUYPOPUPHEADER:
                        lblVatVendorHdr.Text = txtVendorHd.Text;
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
                    #region VENDORCONTACTFORWHT
                    case ControlsEnum.VENDORCONTACTFORWHT:
                        int.TryParse(hdfWthAddressType.Value, out VncPk);
                        int.TryParse(hdfVendorHd.Value, out VatVendorPopupPk);
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
                        break;

                    #endregion                   

                    #region AMOUNTDETAILS
                    case ControlsEnum.AMOUNTDETAILS:
                        dtAmountDetails = new DataTable();
                        dtAmountDetails = BusinessLogic.POInvoicing.POInvoiceBL.GetBalanceAmountDetails(purchaseInvoicePK);
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
                    #region INVOICETYPE
                    case ControlsEnum.INVOICETYPE:
                        dtInvoiceType = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, "PURCHASE INVOICE TYPE");
                        break; 
                    #endregion
                    #region INVOICECATEGORY (PURCHASE INVOICE LIST TYPE)
                    case ControlsEnum.INVOICECATEGORY:
                        dtInvoiceCategory = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, "PURCHASE INVOICE LIST TYPE");
                        break;
                    #endregion
                    #region VENDORDETAILSBYPK (For getting vendor default Type(Import/Local))
                    case ControlsEnum.VENDORDETAILSBYPK:
                        vendPK = 0;
                        int.TryParse(hdfVendorHd.Value, out vendPK);
                        dtVendorDetails = BusinessLogic.POInvoicing.POInvoiceBL.GetVendorDetails(vendPK);
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
                    #region PAYMENTHDRLIST
                    case ControlsEnum.PAYMENTHDRLIST:
                        BindGrid(ControlsEnum.PAYMENTHDRLIST);
                        break;
                    #endregion
                    #region PENDINGPOLIST
                    case ControlsEnum.PENDINGINVLIST:
                        BindGrid(controlType);
                        break;
                    #endregion
                    #region INVPAYMENTHEADER
                    case ControlsEnum.INVPAYMENTHEADER:
                        GetUIValuesFromObject(controlType);
                        break;
                    #endregion
                    #region INVPAYMENTDETAIL
                    case ControlsEnum.INVPAYMENTDETAIL:
                        BindGrid(controlType);
                        break; 
                    #endregion
                    #region PAYMENTADJN
                    case ControlsEnum.PAYMENTADJN:
                        BindGrid(ControlsEnum.PAYMENTADJN);
                        break;
                    #endregion
                    #region PAYMENTSPLITLIST
                    case ControlsEnum.PAYMENTSPLITLIST:
                        BindGrid(ControlsEnum.PAYMENTSPLITLIST);
                        break;
                    #endregion
                    #region CRDRALLOCATION
                    case ControlsEnum.CRDRALLOCATION:
                        BindGrid(ControlsEnum.CRDRALLOCATION);
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
                    #region PAYMENTTYPE
                    case ControlsEnum.PAYMENTTYPE:
                        BindDropDown(ControlsEnum.PAYMENTTYPE);
                        break;
                    #endregion
                    #region PAYMENTMODESGRID
                    case ControlsEnum.PAYMENTMODESGRID:
                        BindGrid(ControlsEnum.PAYMENTMODESGRID);
                        break;
                    #endregion
                    #region UPLOADEDFILES
                    case ControlsEnum.UPLOADEDFILES:
                        if (paymentHeaderObj != null)
                            GetUIValuesFromObject(ControlsEnum.UPLOADEDFILES);
                        BindGrid(ControlsEnum.UPLOADEDFILES);
                        break;
                    #endregion                
                    #region AMOUNTDETAILS
                    case ControlsEnum.AMOUNTDETAILS:
                        BindGrid(controlType);
                        break; 
                    #endregion
                    #region INVOICE TYPE
                    case ControlsEnum.INVOICETYPE:
                        BindDropDown(controlType);
                        break;
                    #endregion
                    #region INVOICECATEGORY
                    case ControlsEnum.INVOICECATEGORY:
                        BindDropDown(controlType);
                        break;
                    #endregion
                    #region VENDORDETAILSBYPK
                    case ControlsEnum.VENDORDETAILSBYPK:
                        GetUIValuesFromObject(controlType);                       
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
        /// Funtion used get doc mode
        /// </summary>
        private string GetDOCMODE()
        {
            cm = new CommonService();
            AppTypeDetailsList = cm.GetReportParameters(AST_CODE.Value, 0, DateTime.Now);
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
                string VendorBank = string.Empty;
                decimal PaidAmountBC = 0;
                bool PdcFlag = false;
                switch (controlType)
                {
                    #region INVPAYMENTHEADER
                    case ControlsEnum.INVPAYMENTHEADER:
                        if (InvPaymentHeaderSession != null)
                        {
                            paymentHeaderObj = InvPaymentHeaderSession;
                            paymentHeaderObj.PVH_PK = CurrPK;
                            paymentHeaderObj.PVH_NO = (string.IsNullOrEmpty(lblPaymentNo.Text.Trim()) || lblPaymentNo.Text.Trim().Equals("[NEW]")) ? string.Empty : lblPaymentNo.Text.Trim();
                            paymentHeaderObj.PVH_CATEGORY = (byte)PICategory;
                            paymentHeaderObj.PVH_OTHER_AMOUNT = Convert.ToDecimal(hdfTotalOtherCharges.Value.Trim());
                            paymentHeaderObj.PVH_GROUP = ((byte)POGroup) == (byte)0 ? (byte)1 : (byte)POGroup;
                            paymentHeaderObj.PVH_DATE = String.IsNullOrEmpty(txtPaymentDate.Text.Trim()) ? DateTime.Now.ToString(Resources.Constants.DateFormatShort) : txtPaymentDate.Text.Trim();                          
                            paymentHeaderObj.PVH_VENDOR_TEXT = HttpUtility.HtmlEncode(paymentHeaderObj.PVH_VENDOR_TEXT); 
                            paymentHeaderObj.PVH_CURRENCY = string.IsNullOrEmpty(hdfPaymentCurrency.Value) ? "1" : hdfPaymentCurrency.Value;
                            paymentHeaderObj.PVH_PAID_AMOUNT = Convert.ToDecimal(txtPaidAmount.Text.Trim());
                            paymentHeaderObj.PVH_REMARKS = HttpUtility.HtmlEncode(txtRemarks.Text.Trim());
                            paymentHeaderObj.PVH_BASE_CURR = currentUser.BaseCurrency <= 0 ? 1 : currentUser.BaseCurrency;

                            GetFieldValues(ControlsEnum.EXCHANGERATEINBASECURRENCY);

                            paymentHeaderObj.PVH_EXCHG_RATE = string.IsNullOrEmpty(txtHdrExchangeRate.Text) ? 0 : Convert.ToDouble(txtHdrExchangeRate.Text);
                            paymentHeaderObj.PVH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);

                            finPaymentVndTrxMpgList = new List<PaymentInvoiceTrxMpgDetails>();
                            finPaymentVndTrxMpgList = (List<PaymentInvoiceTrxMpgDetails>)SetUIValuesToObject(ControlsEnum.PAYMENTMPGENTRY);

                            //////////// For default split allocation apply ////////////////
                            foreach (PaymentInvoiceTrxMpgDetails paymentmpg in finPaymentVndTrxMpgList)
                            {
                                if (AppliedInvPkList == null || !AppliedInvPkList.Contains(Convert.ToInt64(paymentmpg.PVM_INVOICE_HDR)))
                                {
                                    InvoiceDetails(Convert.ToInt64(paymentmpg.PVM_INVOICE_HDR));
                                    PaymentSplitSave(false);
                                }
                            }                            

                            paymentHeaderObj.PVH_DEPT = Convert.ToInt16(currentUser.CurrentDeptPK);
                            paymentHeaderObj.PVH_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                            paymentHeaderObj.PVH_STATUS = 0;
                            paymentHeaderObj.USER_PK = Convert.ToInt16(currentUser.PKUser);
                            paymentHeaderObj.PVH_MOD_DT = LastModifiedTime;
                            paymentHeaderObj.PVH_COMPANY = Convert.ToInt16(ddlCompany.SelectedValue);

                            #region Application Code,AST_DOC_MODE
                            paymentHeaderObj.APT_CODE = POGroup == POInvoiceGroup.Goods ? ApplicationType.VPT : POGroup == POInvoiceGroup.Services ? ApplicationType.SIPT : POGroup == POInvoiceGroup.AgtInvoice ? ApplicationType.AIPT : ApplicationType.EIPT;
                            paymentHeaderObj.AST_DOC_MODE = AST_DOC_MODE.Value == "1" ? 1 : 0;                          
                            #endregion                         

                            if (Convert.ToInt32(ddlvendorBank.SelectedValue) != Convert.ToInt32(CommonConstants.SELECTVAL))
                                VendorBank = ddlvendorBank.SelectedValue;
                            paymentHeaderObj.PVH_VENDOR_BANK = VendorBank;

                            if (ddlAdjType.SelectedValue == CommonConstants.SELECTVAL)
                                paymentHeaderObj.PVH_DISCOUNT = string.Empty;
                            else
                                paymentHeaderObj.PVH_DISCOUNT = ddlAdjType.SelectedValue;

                            paymentHeaderObj.PVH_DISC_AMOUNT = txtAdjAmount.Text != string.Empty ? Convert.ToDecimal(txtAdjAmount.Text) : 0;
                            if (hdfSaveTax.Value == "1")
                            {
                                paymentHeaderObj.PVH_TAX_AMOUNT = txtTaxAmount.Text != string.Empty ? Convert.ToDecimal(txtTaxAmount.Text) : 0;
                            }
                            else
                            {
                                paymentHeaderObj.PVH_TAX_AMOUNT = 0;
                            }
                        
                            #region WHT 
                            decimal whtAmount = string.IsNullOrEmpty(txtWHTAmount.Text) ? Convert.ToDecimal(0) : Convert.ToDecimal(txtWHTAmount.Text);
                            if (whtAmount > 0)
                            {                                
                                if (WHTTaxDetails != null && WHTTaxDetails.Count > 0)
                                {
                                    if (hdfWHTNO.Value == string.Empty)
                                    {
                                        getWHTNO();
                                    }
                                    paymentHeaderObj.PVH_WHT_NO = hdfWHTNO.Value;                                
                                    paymentHeaderObj.TaxHdr = WHTTaxDetails;
                                }
                            }                           
                            #endregion
                          
                            #region VAT 
                            if (VATTaxDetails != null && VATTaxDetails.Count > 0)
                            {

                                PaymentTaxHeader objTemp;
                                List<PaymentTaxHeader> ItemList = new List<PaymentTaxHeader>();
                                foreach (PaymentTaxHeader objItem in VATTaxDetails)
                                {
                                    objTemp = new PaymentTaxHeader();
                                    objTemp.WTH_AMOUNT = objItem.WTH_AMOUNT;
                                    objTemp.WTH_TAX_AMT = objItem.WTH_TAX_AMT;

                                    objTemp.WTH_TAX = objItem.WTH_TAX;
                                    objTemp.WTH_PK = 0;
                                    objTemp.WTH_PAYMENT_HDR = objItem.WTH_PAYMENT_HDR;
                                    objTemp.WTH_TYPE = objItem.WTH_TYPE;
                                    objTemp.WTH_TAX_CATEGORY = objItem.WTH_TAX_CATEGORY;
                                    objTemp.WTH_NAME = objItem.WTH_NAME;
                                    objTemp.WTH_DESC = objItem.WTH_DESC;
                                    objTemp.WTH_PUR_INVOICE = objItem.WTH_PUR_INVOICE;
                                    objTemp.WTH_PARTY_NAME = HttpUtility.HtmlEncode(objItem.WTH_PARTY_NAME);
                                    objTemp.WTH_TAX_ID = objItem.WTH_TAX_ID;
                                    objTemp.WTH_CATEGORY = objItem.WTH_CATEGORY;
                                    objTemp.WTH_TAX_DATE = objItem.WTH_TAX_DATE;
                                    objTemp.WTH_REFUND_DATE = objItem.WTH_REFUND_DATE;
                                    objTemp.WTH_VENDOR = objItem.WTH_VENDOR;
                                    objTemp.WTH_BRANCH = objItem.WTH_BRANCH;
                                    objTemp.WTH_BRANCH_NAME = HttpUtility.HtmlEncode(objItem.WTH_BRANCH_NAME);
                                    objTemp.WTH_BRANCH_TEXT = HttpUtility.HtmlEncode(objItem.WTH_BRANCH_TEXT);
                                    objTemp.WTH_BRANCH_TYPE = objItem.WTH_BRANCH_TYPE;
                                    objTemp.WTH_ITEM_TEXT = HttpUtility.HtmlEncode(objItem.WTH_ITEM_TEXT);
                                    objTemp.WTH_TAX_INV_NO = HttpUtility.HtmlEncode(objItem.WTH_TAX_INV_NO);
                                    objTemp.WTH_INV_RECEIVED = objItem.WTH_INV_RECEIVED;
                                    ItemList.Add(objTemp);
                                }
                                ItemList.ForEach(dtl => paymentHeaderObj.TaxHdr.Add(dtl));

                            } 
                            #endregion

                            if (chkVendorforpayemnt.Checked)
                            {
                                paymentHeaderObj.PVH_WHT_AMOUNT = txtWHTAmount.Text != string.Empty ? (Convert.ToDecimal(txtWHTAmount.Text.Trim()) < 0 ? 0 : Convert.ToDecimal(txtWHTAmount.Text)) : 0;
                            }
                            else
                            {
                                paymentHeaderObj.PVH_WHT_AMOUNT = 0;
                            }
                            paymentHeaderObj.PVH_PAY_FOR_VENDOR = chkVendorforpayemnt.Checked;     

                            #region Payment mode details
                            PdcFlag = false;
                            if (PaymentModeDetailsList == null || PaymentModeDetailsList.Count == 0 || (PaymentModeDetailsList.Count == 1 && !IsPaymentModeAdded))
                            {
                                AddPaymentModes(false);
                                IsPaymentModeAdded = false;
                            }
                            if (PaymentModeDetailsList != null && PaymentModeDetailsList.Count > 0)
                            {                               
                                foreach (PaymentModeDetails objItem in PaymentModeDetailsList)
                                {  
                                    if (objItem.PDM_PDC > 0)
                                        PdcFlag = true;
                                }                              
                                PaidAmountBC = PaymentModeDetailsList.Sum(r => r.PDM_PAID_AMOUNT_BC);
                            }
                            if (PdcFlag)
                                paymentHeaderObj.PVH_PDC = (byte)1;
                            #endregion

                            paymentHeaderObj.PVH_PAID_AMOUNT_BC = PaidAmountBC;

                            #region Setting Tax Slno & HtmlEncodding
                            PaymentInvMappingDetails.ForEach(dtl =>
                            {
                                dtl.IVH_VENDOR_TEXT = HttpUtility.HtmlEncode(dtl.IVH_VENDOR_TEXT);//For avoiding XML parsing error : illegal name character (&) 
                                if (dtl.AllocationMpg != null && dtl.AllocationMpg.Count > 0)
                                {
                                    dtl.AllocationMpg = dtl.AllocationMpg.Where(f => f.PAD_AMOUNT > 0).ToList();//Should not save allocation details which have zero amount
                                    dtl.AllocationMpg.ForEach(f => f.PAD_PVM_SL_NO = dtl.PVM_SL_NO);
                                }
                                if (dtl.CRDRMpg != null && dtl.CRDRMpg.Count > 0)
                                {                                   
                                    dtl.CRDRMpg.ForEach(f => f.PNM_PVM_SL_NO = dtl.PVM_SL_NO);
                                }
                                #region CalculatePaymentTax,In the case of Advance Invoice we need to pass PDT_PPO_SL_NO (For saving PDT_VND_PO_MPG in FIN_PAYMENT_VND_TAX_DTL table)

                                if (dtl.POMpg != null && dtl.POMpg.Count > 0)
                                {
                                    foreach (PaymentPOMappingDetails item in dtl.POMpg)
                                    {
                                        if (PICategory == POInvoiceCategory.Advanced)
                                        {
                                            dtl.TaxDetails.ForEach(f => f.PDT_PPO_SL_NO = item.PPO_SL_NO);//For saving PDT_VND_PO_MPG in FIN_PAYMENT_VND_TAX_DTL table                                          

                                        }
                                        CalculatePaymentTax(dtl, Convert.ToInt32(item.PPO_PO_HDR), item.PPO_TAX_AMOUNT, (byte)PICategory, item.POH_IVH_PK, item.PPO_PAID_AMOUNT);
                                    }
                                }
                                else //In the case of Expense invoice there is no PO
                                {
                                    CalculatePaymentTax(dtl,0,dtl.IVH_TAX_AMT, (byte)PICategory,dtl.IVH_PK, dtl.IVH_DISCOUNT_TOTAL);
                                }
                              
                                #endregion
                            });
                            #endregion                           

                            paymentHeaderObj.TrxMpg = PaymentInvMappingDetails;
                            paymentHeaderObj.ModeDetail = PaymentModeDetailsList;
                            paymentHeaderObj.FileList = PaymentUploadList;  
                        }
                        retObject = paymentHeaderObj;
                        break;
                    #endregion
                    #region PAYMENTMPGENTRY
                    case ControlsEnum.PAYMENTMPGENTRY:
                        rowID = 0;
                        foreach (GridViewRow grdrow in grdInvoiceList.Rows)
                        {
                            #region Declaration
                            decimal taxpercentage = 0;
                            decimal basevalue = 0;
                            decimal ttaxamt = 0;
                            decimal hdftax = 0;
                            decimal hdfTaxHdrDtl = 0;
                            decimal hdftotalamt = 0;
                            int currRowInvoicePK = 0;
                            hdfPaymentMpgPK = (HiddenField)grdInvoiceList.Rows[rowID].FindControl(GetLocalResourceObject("hdfPaymentMpgPK").ToString());
                            hdfInvoicePK = (HiddenField)grdInvoiceList.Rows[rowID].FindControl(GetLocalResourceObject("hdfInvoicePK").ToString());
                            txtAmount = (TextBox)grdInvoiceList.Rows[rowID].FindControl(GetLocalResourceObject("txtPayNow").ToString());
                            txtOtherCharges = (TextBox)grdInvoiceList.Rows[rowID].FindControl("txtOtherCharges");
                            HiddenField hdfCategory = (HiddenField)grdInvoiceList.Rows[rowID].FindControl("hdfCategory");
                            HiddenField hdfGroup = (HiddenField)grdInvoiceList.Rows[rowID].FindControl("hdfGroup");
                            HiddenField hdfTaxAmt = (HiddenField)grdInvoiceList.Rows[rowID].FindControl("hdfTaxAmt");
                            HiddenField hdfTaxHdrDtlAmt = (HiddenField)grdInvoiceList.Rows[rowID].FindControl("hdfTaxHdrDtlAmt");
                            HiddenField hdfTotalAmt = (HiddenField)grdInvoiceList.Rows[rowID].FindControl("hdfTotalAmt");
                            HiddenField hdfTotalTax = (HiddenField)grdInvoiceList.Rows[rowID].FindControl("hdfTotalTax");
                            Label lblBaltopay = (Label)grdInvoiceList.Rows[rowID].FindControl("lblBaltopay");
                            lblAdjAmount = (Label)grdInvoiceList.Rows[rowID].FindControl("lblAdjAmount");
                            txtAdjustments = (TextBox)grdInvoiceList.Rows[rowID].FindControl(GetLocalResourceObject("txtAdjustments").ToString()); 
                            #endregion
                            currRowInvoicePK = hdfInvoicePK == null ? 0 : Convert.ToInt32(hdfInvoicePK.Value);
                            paymentInvMapDtlObj = PaymentInvMappingDetails.SingleOrDefault(dtl => dtl.IVH_PK == currRowInvoicePK);
                            if (paymentInvMapDtlObj != null)
                            {                               
                                paymentInvMapDtlObj.PVM_PK = hdfPaymentMpgPK == null ? 0 : Convert.ToInt64(hdfPaymentMpgPK.Value);
                                paymentInvMapDtlObj.PVM_PAYMENT_HDR = CurrPK.ToString();
                                paymentInvMapDtlObj.PVM_INVOICE_HDR = currRowInvoicePK.ToString();
                                paymentInvMapDtlObj.PVM_PAID_AMOUNT = txtAmount == null ? 0 : txtAmount.Text.Trim() == string.Empty ? 0 : Convert.ToDecimal(txtAmount.Text.Trim());
                                paymentInvMapDtlObj.PVM_OTHER_AMOUNT = txtOtherCharges == null ? 0 : txtOtherCharges.Text.Trim() == string.Empty ? 0 : Convert.ToDecimal(txtOtherCharges.Text.Trim());
                                paymentInvMapDtlObj.PVM_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);

                                hdftax = hdfTaxAmt.Value != string.Empty ? Convert.ToDecimal(hdfTaxAmt.Value) : 0;
                                hdfTaxHdrDtl = hdfTaxHdrDtlAmt.Value != string.Empty ? Convert.ToDecimal(hdfTaxHdrDtlAmt.Value) : 0;
                                hdftotalamt = hdfTotalAmt.Value != string.Empty ? Convert.ToDecimal(hdfTotalAmt.Value) : 0;
                                lblAdjAmount.Text = lblAdjAmount.Text.Replace(",", "");
                                lblBaltopay.Text = lblBaltopay.Text.Replace(",", "");
                                ttaxamt = Convert.ToDecimal(hdfTotalTax.Value);

                                paymentInvMapDtlObj.PVM_TAX_AMOUNT = Math.Round(ttaxamt, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                paymentInvMapDtlObj.PVM_DISC_AMOUNT = txtAdjustments == null ? 0 : txtAdjustments.Text.Trim() == string.Empty ? 0 : Convert.ToDecimal(txtAdjustments.Text.Trim());
                                paymentInvMapDtlObj.PVM_ADJUST_AMOUNT = string.IsNullOrEmpty(lblAdjAmount.Text) ? 0 : Convert.ToDecimal(lblAdjAmount.Text.Trim());
                                decimal excessAmt = (paymentInvMapDtlObj.PVM_PAID_AMOUNT - Convert.ToDecimal(lblBaltopay.Text.Trim()));
                                paymentInvMapDtlObj.PVM_EXCESS_AMOUNT = (excessAmt > 0) ? excessAmt : 0;
                            }                                                       
                            rowID++;
                        }
                        retObject = PaymentInvMappingDetails;
                        break;
                    #endregion                                
                    #region PAYMENTSPLITLIST
                    case ControlsEnum.PAYMENTSPLITLIST:
                        rowID = 0;
                        finPaymentVndPoMpgList = new List<PaymentPOMappingDetails>();
                        foreach (GridViewRow grdrow in grdPaymentSplit.Rows)//
                        {
                            hdfPOPK = (HiddenField)grdPaymentSplit.Rows[rowID].FindControl("hdfPOPK");
                            txtPayNowSplit = (TextBox)grdPaymentSplit.Rows[rowID].FindControl("txtPayNowSplit");
                            lblOtherChargesSplit = (Label)grdPaymentSplit.Rows[rowID].FindControl("lblOtherChargesSplit");
                            TextBox txtTaxSplit = (TextBox)grdPaymentSplit.Rows[rowID].FindControl("txtTaxSplit");
                            hdfPaymentSplitPK = (HiddenField)grdPaymentSplit.Rows[rowID].FindControl("hdfPaymentSplitPK");

                            finPaymentVndPoMpgObj = PaymentInvMappingDetails.SingleOrDefault(dtl => dtl.IVH_PK == InvoicePK).POMpg.SingleOrDefault(f => f.PPO_PO_HDR == hdfPOPK.Value);
                            if (finPaymentVndPoMpgObj != null)
                            {                                
                                finPaymentVndPoMpgObj.PPO_PK = hdfPaymentSplitPK == null ? 0 : Convert.ToInt32(hdfPaymentSplitPK.Value);
                                finPaymentVndPoMpgObj.PPO_PAYMENT_HDR = CurrPK;
                                finPaymentVndPoMpgObj.PPO_PAYMENT_TRX_MPG = Convert.ToInt32(PaymentMpgPK);
                                if (!string.IsNullOrEmpty(hdfPOPK.Value))
                                    finPaymentVndPoMpgObj.PPO_PO_HDR = hdfPOPK.Value;                              
                                finPaymentVndPoMpgObj.PPO_TAX_AMOUNT = string.IsNullOrEmpty(txtTaxSplit.Text) ? 0 : Convert.ToDecimal(txtTaxSplit.Text.Trim());
                                finPaymentVndPoMpgObj.PPO_PAID_AMOUNT = txtPayNowSplit == null ? 0 : txtPayNowSplit.Text.Trim() == string.Empty ? 0 : Convert.ToDecimal(txtPayNowSplit.Text.Trim());
                                finPaymentVndPoMpgObj.PPO_OTHER_AMOUNT = lblOtherChargesSplit == null ? 0 : lblOtherChargesSplit.Text.Replace(",", "").Trim() == string.Empty ? 0 : Convert.ToDecimal(lblOtherChargesSplit.Text.Replace(",", "").Trim());
                                finPaymentVndPoMpgObj.PPO_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                            }
                            finPaymentVndPoMpgList.Add(finPaymentVndPoMpgObj);
                            rowID++;
                        }
                        retObject = finPaymentVndPoMpgList;
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
                        FinPaymentVndAllocationList = new List<PaymentCRDRAdjAllocationDtl>();
                        foreach (GridViewRow grdrow in grdPaymentSplitAdjn.Rows)
                        {
                            hdfReceiptTRXAdjnPK = (HiddenField)grdPaymentSplitAdjn.Rows[rowID].FindControl("hdfReceiptTRXAdjnPK");
                            hdfCrDrPK = (HiddenField)grdPaymentSplitAdjn.Rows[rowID].FindControl("hdfCrDrPK");
                            hdfAdjnPK = (HiddenField)grdPaymentSplitAdjn.Rows[rowID].FindControl("hdfAdjnPK");
                            hdfReceiptAdjnPK = (HiddenField)grdPaymentSplitAdjn.Rows[rowID].FindControl("hdfReceiptAdjnPK");                         
                            txtAllocateAdjn = (TextBox)grdPaymentSplitAdjn.Rows[rowID].FindControl("txtAllocateAdjn");
                            FinPaymentVndAllocationObj = CommonFunctions.Initilize<PaymentCRDRAdjAllocationDtl>();                         
                            FinPaymentVndAllocationObj.PAD_PK = string.IsNullOrEmpty(hdfAdjnPK.Value) ? 0 : Convert.ToInt32(hdfAdjnPK.Value);
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
                        rowID = 0;
                        FinPaymentVndCrdrMpgList = new List<PaymentCRDRMappingDetails>();
                        foreach (GridViewRow grdrow in grdCrdrAllocation.Rows)
                        {
                            HiddenField hdfCRDRHdr = (HiddenField)grdCrdrAllocation.Rows[rowID].FindControl("hdfCRDRHdr");
                            TextBox txtCrdrPayNow = (TextBox)grdrow.FindControl("txtCrdrPayNow");
                            TextBox txtCrdrAdjAmount = (TextBox)grdrow.FindControl("txtCrdrAdjAmount");
                            HiddenField hdfCrdrMpgPk = (HiddenField)grdrow.FindControl("hdfCrdrMpgPk");
                            decimal CrdrPayNow = 0;
                            decimal CrdrAdjAmnt = 0;
                            decimal.TryParse(txtCrdrPayNow.Text, out CrdrPayNow);
                            decimal.TryParse(txtCrdrAdjAmount.Text, out CrdrAdjAmnt);

                            finPaymentVndCrdrMpgObj = PaymentInvMappingDetails.SingleOrDefault(dtl => dtl.IVH_PK == InvoicePK).CRDRMpg.SingleOrDefault(f => f.PNM_CRDR_HDR == Convert.ToInt32(hdfCRDRHdr.Value));
                            if (finPaymentVndCrdrMpgObj != null)
                            {
                                finPaymentVndCrdrMpgObj.PNM_PAID_AMOUNT = CrdrPayNow;
                                finPaymentVndCrdrMpgObj.PNM_ADJ_AMOUNT = CrdrAdjAmnt;
                                if (!string.IsNullOrEmpty(hdfCrdrMpgPk.Value))
                                    finPaymentVndCrdrMpgObj.PNM_CRDR_MPG = hdfCrdrMpgPk.Value;
                            }
                            FinPaymentVndCrdrMpgList.Add(finPaymentVndCrdrMpgObj);
                            rowID++;
                        }
                        retObject = FinPaymentVndCrdrMpgList;
                        break;
                    #endregion
                    #region JOURNALIZE (JOURNALIZE,REVERSE,CHEQUERETURN)
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
                                if (InvPaymentHeaderSession != null)
                                {
                                    if (InvPaymentHeaderSession.PVH_PAID_AMOUNT == 0)
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
                                    ucrJournalize.TransactionType = POGroup == POInvoiceGroup.Goods ? ApplicationType.VPTJ
                                        : POGroup == POInvoiceGroup.Services ? ApplicationType.SIPTJ : POGroup == POInvoiceGroup.AgtInvoice ? ApplicationType.AIPTJ : ApplicationType.EIPTJ;
                                    Session[ERP.Utilities.SessionStrings.TransactionType] = POGroup == POInvoiceGroup.Goods ? ApplicationType.VPTJ
                                        : POGroup == POInvoiceGroup.Services ? ApplicationType.SIPTJ : POGroup == POInvoiceGroup.AgtInvoice ? ApplicationType.AIPTJ : ApplicationType.EIPTJ;
                                    ucrJournalize.JournalType = (int)JournalTypeEnum.Voucher;
                                }
                                else if (controlType == ControlsEnum.REVERSE)
                                {
                                    Session[ERP.Utilities.SessionStrings.TransactionType] = ucrJournalize.TransactionType = ApplicationType.PPCCTJ;
                                    ucrJournalize.JournalType = (int)JournalTypeEnum.Reverse;
                                }
                                else if (controlType == ControlsEnum.CHEQUERETURN)
                                {
                                    Session[ERP.Utilities.SessionStrings.TransactionType] = ucrJournalize.TransactionType = ApplicationType.PCBTJ;
                                    ucrJournalize.JournalType = (int)JournalTypeEnum.Return;
                                }

                                ucrJournalize.TransactionPK = (int)CurrPK;
                                Session[ERP.Utilities.SessionStrings.TransactionPK] = CurrPK;
                                ucrJournalize.JournalizePK = 0;
                                Session[ERP.Utilities.SessionStrings.JournalizePK] = null;
                               // GetFieldValues(ControlsEnum.PAYMENTHDRENTRYBYPK);
                                Session[ERP.Utilities.SessionStrings.TransactionNo] = InvPaymentHeaderSession.PVH_NO;
                                Session[ERP.Utilities.SessionStrings.TransactionDate] = InvPaymentHeaderSession.PVH_DATE;
                                Session[ERP.Utilities.SessionStrings.TransactionCurrency] = InvPaymentHeaderSession.PVH_CURRENCY;
                                Session[ERP.Utilities.SessionStrings.AccountPayable] = ApplicationType.VND;
                                Session[ERP.Utilities.SessionStrings.AccountPayablePK] = InvPaymentHeaderSession.PVH_VENDOR;
                                Session[ERP.Utilities.SessionStrings.JournalType] = POGroup == POInvoiceGroup.Goods ? ApplicationType.VPTJ
                                    : POGroup == POInvoiceGroup.Services ? ApplicationType.SIPTJ : POGroup == POInvoiceGroup.AgtInvoice ? ApplicationType.AIPTJ : ApplicationType.EIPTJ;

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
                                }
                                else
                                {
                                    ucrWrkf.ViewType = 0;                                   
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
                    #region INVPAYMENTHEADER
                    case ControlsEnum.INVPAYMENTHEADER:
                        paymentHeaderObj = InvPaymentHeaderSession;
                        if (paymentHeaderObj != null)
                        {
                            lblPaymentNo.Text = hdfPaymentNo.Value = string.IsNullOrEmpty(paymentHeaderObj.PVH_NO) ?  Resources.ErpRes.Draft : paymentHeaderObj.PVH_NO;
                            txtVendorHd.Text = ERP.Utilities.CommonFunctions.GetDecodedString(paymentHeaderObj.PVH_VENDOR_TEXT);
                            hdfVendorHd.Value = paymentHeaderObj.PVH_VENDOR.ToString();
                            POGroup = (POInvoiceGroup)Enum.Parse(typeof(POInvoiceGroup), paymentHeaderObj.PVH_GROUP.ToString());
                            PICategory = (POInvoiceCategory)Enum.Parse(typeof(POInvoiceCategory), paymentHeaderObj.PVH_CATEGORY.ToString());
                            txtPaymentDate.Text = paymentHeaderObj.PVH_DATE;                     
                            txtPaymentCurrency.Text = paymentHeaderObj.PVH_CURRENCY_TEXT;
                            lblPaymentAmount.Text = GetLocalResourceObject("PaymentAmount") + "(" + paymentHeaderObj.PVH_CURRENCY_TEXT + ")";
                            hdfPaymentCurrency.Value = paymentHeaderObj.PVH_CURRENCY.ToString();
                            txtHdrExchangeRate.Text = paymentHeaderObj.PVH_EXCHG_RATE.ToString();

                            PaymentInvMappingDetails = paymentHeaderObj.TrxMpg.ToList();//Setting Invoice mapping details List                           
                            PaymentModeDetailsList = paymentHeaderObj.ModeDetail.ToList();
                            WHTTaxDetails = paymentHeaderObj.TaxHdr.Where(whtpynt => whtpynt.WTH_CATEGORY == (byte)WHTCategoryEnum.WHT).ToList();
                            VATTaxDetails = paymentHeaderObj.TaxHdr.Where(whtpynt => whtpynt.WTH_CATEGORY == (byte)WHTCategoryEnum.VATBUY).ToList();
                            finPayemtVndHdrList = paymentHeaderObj.TaxHdr.ToList();
                            TempWHTTaxDetails = finPayemtVndHdrList.Where(whtpynt => whtpynt.WTH_CATEGORY == (byte)WHTCategoryEnum.WHT).ToList();
                            TempVATTaxDetails = finPayemtVndHdrList.Where(whtpynt => whtpynt.WTH_CATEGORY == (byte)WHTCategoryEnum.VATBUY).ToList();

                            #region MyRegion
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
                            short pdc = 0;
                            List<PaymentModeDetails> paymentModeLst = paymentHeaderObj.ModeDetail.ToList();
                            List<byte> Mode = new List<byte>();
                            if (paymentModeLst != null && paymentModeLst.Count > 0)
                            {
                                Mode = paymentModeLst.Select(r => r.PDM_MODE).Distinct().ToList();
                                if (paymentModeLst.Where(r => r.PDM_PDC >= 1).Count() > 0)
                                {
                                    pdc = paymentModeLst.Where(r => r.PDM_PDC >= 1).ToList()[0].PDM_PDC;
                                }
                            }

                            string isposted = paymentHeaderObj.PVH_HAS_JRNL_ENTRY.ToString();
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
                            GetFieldValues(ControlsEnum.EXCHANGERATEBANK);
                            if (ddlBankChargeCurrency.Items[0].Value != paymentHeaderObj.PVH_CURRENCY.ToString())
                            {
                                ddlBankChargeCurrency.Items.Insert(1, (new ListItem(paymentHeaderObj.PVH_CURRENCY_TEXT + " - " + paymentHeaderObj.PVH_CURRENCY_TEXT, paymentHeaderObj.PVH_CURRENCY.ToString())));
                            }
                            GetFieldValues(ControlsEnum.BANKCURRENCYEXCHANGERATE); 
                            #endregion
                        
                            ddlCompany.SelectedValue = paymentHeaderObj.PVH_COMPANY.ToString();                           
                            txtRemarks.Text = HttpUtility.HtmlDecode(paymentHeaderObj.PVH_REMARKS);
                            LastModifiedTime = paymentHeaderObj.PVH_MOD_DT;
                            lblLastModifiedHDR.Text = Resources.Report.LastModifiedOn + LastModifiedTime.ToString(Resources.Report.LastModifiedDatetimeFormat);
                            Approved =  paymentHeaderObj.PVH_STATUS;
                            hdfDelStatus.Value = paymentHeaderObj.PVH_DEL_STATUS.ToString();
                            if (!string.IsNullOrEmpty(paymentHeaderObj.PVH_DISCOUNT))
                                ddlAdjType.SelectedIndex = Convert.ToInt32(ddlAdjType.Items.IndexOf(ddlAdjType.Items.FindByValue(Convert.ToInt32(paymentHeaderObj.PVH_DISCOUNT).ToString())));
                            txtAdjAmount.Text = Math.Round(paymentHeaderObj.PVH_DISC_AMOUNT, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            txtTaxAmount.Text = Math.Round(paymentHeaderObj.PVH_TAX_AMOUNT, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                           
                            hdfWHTNO.Value = paymentHeaderObj.PVH_WHT_NO != null ? paymentHeaderObj.PVH_WHT_NO.ToString() : string.Empty;

                            #region VENDOR_WHT_TAX,VENDORBANKS
                            if (paymentHeaderObj.PVH_VENDOR_WHT_TAX != null)
                            {
                                whtTaxpk = Convert.ToInt32(paymentHeaderObj.PVH_VENDOR_WHT_TAX);
                                txtWHTAmount.Text = Math.Round(paymentHeaderObj.PVH_WHT_AMOUNT == -1 ? 0 : paymentHeaderObj.PVH_WHT_AMOUNT, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                                trVendorAccount.Style.Add("display", "");
                                GetFieldValues(ControlsEnum.VENDORACCOUNTTAX);
                                SetFieldValues(ControlsEnum.VENDORACCOUNTTAX);
                            }
                            else
                            {
                                trVendorAccount.Style.Add("display", "none");
                            }
                            SetVendorPayment();
                            if (!string.IsNullOrEmpty(paymentHeaderObj.PVH_VENDOR_BANK))
                            {
                                VendorBankPk = Convert.ToInt32(paymentHeaderObj.PVH_VENDOR_BANK);
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
                            } 
                            #endregion

                            if (PaymentModeDetailsList != null && PaymentModeDetailsList.Count == 1)
                            {
                                PaymentModeRowIndex = 0;
                                SetUIEditViewPaymentModeDetails(0);
                            }
                            else
                            {
                                ResetForm(ControlsEnum.PAYMENTMODES);
                            }

                            #region Setting the slno
                            int newPVMslno = 1;
                            PaymentInvMappingDetails.ForEach(dtl =>
                            {
                                dtl.PVM_SL_NO = newPVMslno;
                                if (dtl.POMpg != null && dtl.POMpg.Count > 0)
                                {
                                    #region Linking with Slno Setting
                                    int newPPOslno = 1;
                                    foreach (PaymentPOMappingDetails itm in dtl.POMpg)// TrxMpg & POMpg linking 
                                    {
                                        itm.PPO_SL_NO = newPPOslno;
                                        itm.PPO_PVM_SL_NO = newPVMslno;
                                        newPPOslno++;
                                    }
                                    foreach (PaymentCRDRAdjAllocationDtl itm in dtl.AllocationMpg) //  TrxMpg & AllocationDtl linking 
                                    {
                                        itm.PAD_PVM_SL_NO = newPVMslno;
                                    }
                                    foreach (PaymentCRDRMappingDetails itm in dtl.CRDRMpg) //  TrxMpg & CRDRMpg linking
                                    {
                                        itm.PNM_PVM_SL_NO = newPVMslno;
                                    }
                                    foreach (PaymentTaxDetail itm in dtl.TaxDetails) // TrxMpg & TaxDetail linking
                                    {
                                        itm.PDT_PVM_SL_NO = newPVMslno;
                                    }
                                    #endregion
                                }
                                else //In the case of Expense invoice there is no PO
                                {
                                    #region Linking with Slno Setting
                                    foreach (PaymentTaxDetail itm in dtl.TaxDetails) // TrxMpg & TaxDetail linking
                                    {
                                        itm.PDT_PVM_SL_NO = newPVMslno;
                                    }
                                    #endregion
                                }
                                newPVMslno++;
                            });
                            #endregion
                        }
                        break;
                    #endregion
                    #region PAYMENTSPLITLIST
                    case ControlsEnum.PAYMENTSPLITLIST:
                        if (tempPaymentInvoiceTrxMpgDetails != null && tempPaymentInvoiceTrxMpgDetails.Count > 0)
                        {
                            lblInvSplitNo.Text = lblInvSplitNo_CrdrAlcn.Text = ERP.Utilities.CommonFunctions.GetShortString(tempPaymentInvoiceTrxMpgDetails[0].IVH_NO, 20);
                            lblInvSplitNo.ToolTip = lblInvSplitNo_CrdrAlcn.ToolTip = tempPaymentInvoiceTrxMpgDetails[0].IVH_NO;
                            lblInvSplitDate.Text = lblInvSplitDate_CrdrAlcn.Text = ERP.Utilities.CommonFunctions.GetShortString(tempPaymentInvoiceTrxMpgDetails[0].IVH_DATE, 13);
                            lblInvSplitDate.ToolTip = lblInvSplitDate_CrdrAlcn.ToolTip = tempPaymentInvoiceTrxMpgDetails[0].IVH_DATE;
                            lblInvSplitSupplier.Text = lblInvSplitSupplier_CrdrAlcn.Text = ERP.Utilities.CommonFunctions.GetShortString(tempPaymentInvoiceTrxMpgDetails[0].IVH_VENDOR_TEXT, 18);
                            lblInvSplitSupplier.ToolTip = lblInvSplitSupplier_CrdrAlcn.ToolTip = tempPaymentInvoiceTrxMpgDetails[0].IVH_VENDOR_TEXT;
                            lblInvSplitAmount.ToolTip = lblInvSplitAmount.Text = String.Format("{0:c}", tempPaymentInvoiceTrxMpgDetails[0].IVH_AMOUNT_NET_TC);
                            if (finPaymentVndPoMpgList != null && finPaymentVndPoMpgList.Count > 0)
                            {
                                lblInvSplitReceived.ToolTip = lblInvSplitReceived.Text = finPaymentVndPoMpgList[0].PPO_BOUNCED == 0 ? String.Format("{0:c}", tempPaymentInvoiceTrxMpgDetails[0].IVH_AMOUNT_PAID_TC - (CurrPK > 0 ? PayNowAmount : 0)) :
                                    String.Format("{0:c}", tempPaymentInvoiceTrxMpgDetails[0].IVH_AMOUNT_PAID_TC);
                            }
                            else
                            {
                                lblInvSplitReceived.ToolTip = lblInvSplitReceived.Text = String.Format("{0:c}", tempPaymentInvoiceTrxMpgDetails[0].IVH_AMOUNT_PAID_TC - (CurrPK > 0 ? PayNowAmount : 0));
                            }
                            lblInvSplitReceiveNow.ToolTip = lblInvSplitReceiveNow.Text = String.Format("{0:c}", PayNowAmount);

                            paymentPOMpgDetailsList = tempPaymentInvoiceTrxMpgDetails[0].POMpg;
                        }
                        break;
                    #endregion
                    #region SPLITPAYNOWFORMULIPO
                    case ControlsEnum.SPLITPAYNOWFORMULIPO:
                        if (grdPaymentSplit.Rows.Count > 0 && InvoicePK > 0)
                        {                           
                            GetFieldValues(ControlsEnum.INVOICEVNDMPGLISTFORAUTOALCN);                          
                            decimal InvoiceSubTotal = 0;                          
                            decimal PayNowSplit = 0;
                            decimal advDeductAmnt = 0;
                            decimal ExcessAmount = 0;
                            decimal InvTotalOtherCharge = 0;
                            decimal TotalOtherCharge = InvOtherCharge;
                            decimal InvAllocatedCNAmnt = 0;


                            if (FinInvoiceVndTrxMpgListForAutoAlcn != null && FinInvoiceVndTrxMpgListForAutoAlcn.Count > 0)
                            {
                                InvoiceSubTotal = Convert.ToDecimal(FinInvoiceVndTrxMpgListForAutoAlcn.Sum(r => r.IVM_AMOUNT));
                                if (tempPaymentInvoiceTrxMpgDetails != null && tempPaymentInvoiceTrxMpgDetails.Count > 0)
                                {
                                    PaymentCrdrList = tempPaymentInvoiceTrxMpgDetails[0].CRDRMpg;
                                }
                                if (PaymentCrdrList != null && PaymentCrdrList.Count > 0)
                                {
                                    long InvPk = FinInvoiceVndTrxMpgListForAutoAlcn[0].IVM_INVOICE_HDR;
                                    InvAllocatedCNAmnt = PaymentCrdrList.Where(r => r.PNM_INVOICE_HDR == InvPk).Sum(sm => sm.PNM_PAID_AMOUNT + sm.PNM_ADJ_AMOUNT);                                
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
                                        InvAmount =  Convert.ToDecimal(FinInvoiceVndTrxMpgListForAutoAlcn.Where(iv => iv.IVM_PO_HDR == PoPk).Sum(inv => inv.IVM_AMOUNT));
                                        InvOtherAmount =  Convert.ToDecimal(FinInvoiceVndTrxMpgListForAutoAlcn.Where(iv => iv.IVM_PO_HDR == PoPk).Sum(inv => inv.IVM_OTHER_AMOUNT));
                                        InvTotalOtherCharge =  Convert.ToDecimal(FinInvoiceVndTrxMpgListForAutoAlcn.Sum(inv => inv.IVM_OTHER_AMOUNT));
                                        if (FinInvoiceVndTrxMpgListForAutoAlcn[0].IVH_CATEGORY == (byte)POInvoiceCategory.Invoice)
                                        {
                                            InvAmount =  Convert.ToDecimal(FinInvoiceVndTrxMpgListForAutoAlcn.Where(iv => iv.IVM_PO_HDR == PoPk).Sum(inv => inv.IVM_AMOUNT));
                                        }

                                        TextBox txtPayNow = (TextBox)grdRow.FindControl("txtPayNowSplit");
                                        Label lblOtherChargesSplit = (Label)grdRow.FindControl("lblOtherChargesSplit");
                                        HiddenField hdfOtherChargesSplit = (HiddenField)grdRow.FindControl("hdfOtherChargesSplit");
                                        TextBox txtTaxSplit = (TextBox)grdRow.FindControl("txtTaxSplit");

                                        if (IsCreditNoteApplied || InvAllocatedCNAmnt > 0 || (string.IsNullOrEmpty(txtPayNow.Text) || Convert.ToDecimal(txtPayNow.Text) == 0))
                                        {                                          
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
                                        InvAmount =  Convert.ToDecimal(FinInvoiceVndTrxMpgListForAutoAlcn.Where(iv => iv.IVM_PO_HDR == PoPk).Sum(inv => inv.IVM_AMOUNT));
                                        InvOtherAmount =  Convert.ToDecimal(FinInvoiceVndTrxMpgListForAutoAlcn.Where(iv => iv.IVM_PO_HDR == PoPk).Sum(inv => inv.IVM_OTHER_AMOUNT));
                                        InvTotalOtherCharge =  Convert.ToDecimal(FinInvoiceVndTrxMpgListForAutoAlcn.Sum(inv => inv.IVM_OTHER_AMOUNT));
                                        if (FinInvoiceVndTrxMpgListForAutoAlcn[0].IVH_CATEGORY == (byte)POInvoiceCategory.Invoice)
                                        {
                                            InvAmount =  Convert.ToDecimal(FinInvoiceVndTrxMpgListForAutoAlcn.Where(iv => iv.IVM_PO_HDR == PoPk).Sum(inv => inv.IVM_AMOUNT));
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
                    #region CRDRALLOCATION
                    case ControlsEnum.CRDRALLOCATION:
                        if (tempPaymentInvoiceTrxMpgDetails != null && tempPaymentInvoiceTrxMpgDetails.Count > 0)
                        {
                            lblInvSplitNo_CrdrAlcn.Text = ERP.Utilities.CommonFunctions.GetShortString(tempPaymentInvoiceTrxMpgDetails[0].IVH_NO, 20);
                            lblInvSplitNo_CrdrAlcn.ToolTip = tempPaymentInvoiceTrxMpgDetails[0].IVH_NO;
                            lblInvSplitDate_CrdrAlcn.Text = ERP.Utilities.CommonFunctions.GetShortString(tempPaymentInvoiceTrxMpgDetails[0].IVH_DATE, 13);
                            lblInvSplitDate_CrdrAlcn.ToolTip = tempPaymentInvoiceTrxMpgDetails[0].IVH_DATE;
                            lblInvSplitSupplier_CrdrAlcn.Text = ERP.Utilities.CommonFunctions.GetShortString(tempPaymentInvoiceTrxMpgDetails[0].IVH_VENDOR_TEXT, 18);
                            lblInvSplitSupplier_CrdrAlcn.ToolTip = tempPaymentInvoiceTrxMpgDetails[0].IVH_VENDOR_TEXT;
                            lblInvSplitAmount.Text = String.Format("{0:c}", tempPaymentInvoiceTrxMpgDetails[0].IVH_AMOUNT_NET_TC);
                            if (finPaymentVndPoMpgList != null && finPaymentVndPoMpgList.Count > 0)
                            {
                                lblInvSplitReceived.ToolTip = lblInvSplitReceived.Text = finPaymentVndPoMpgList[0].PPO_BOUNCED == 0 ? String.Format("{0:c}", tempPaymentInvoiceTrxMpgDetails[0].IVH_AMOUNT_PAID_TC - (CurrPK > 0 ? PayNowAmount : 0)) :
                                    String.Format("{0:c}", tempPaymentInvoiceTrxMpgDetails[0].IVH_AMOUNT_PAID_TC);
                            }
                            else
                            {
                                lblInvSplitReceived.ToolTip = lblInvSplitReceived.Text = String.Format("{0:c}", tempPaymentInvoiceTrxMpgDetails[0].IVH_AMOUNT_PAID_TC - (CurrPK > 0 ? PayNowAmount : 0));
                            }
                            lblInvSplitReceiveNow.ToolTip = lblInvSplitReceiveNow.Text = String.Format("{0:c}", PayNowAmount);

                            PaymentCrdrList = tempPaymentInvoiceTrxMpgDetails[0].CRDRMpg;
                        }
                        break;
                    #endregion
                    #region SELECTEDDOC
                    case ControlsEnum.SELECTEDDOC:
                        if (poUploadObj != null)
                        {
                            CurrSlNo = poUploadObj.DOC_SEQ_NO;
                            anchorFile.Visible = true;
                            vrfFileUpload.Enabled = false;
                            anchorFile.InnerHtml = poUploadObj.DOC_NAME;
                            anchorFile.HRef = poUploadObj.DOC_PATH;
                            if (FileDetailsList != null && FileDetailsList.Where(fle => fle.SlNo == CurrSlNo).Count() > 0)
                            {
                                anchorFile.Attributes.Add("onclick", "return false;");
                            }
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
                                    
                                }
                            }
                            else if (Convert.ToInt32(ddlVATAccountPopup.SelectedValue) == -1)
                            {
                                hdfVATAccountPopup.Value = string.Empty;
                                hdfTaxformula.Value = string.Empty;
                                txtVATTaxAmountPopup.Text = string.Empty;
                              
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
                        }
                        break;
                    #endregion

                    #region VENDORACCOUNTTAX
                    case ControlsEnum.VENDORACCOUNTTAX:
                        if (dtVendorAccount != null && dtVendorAccount.Rows.Count > 0)
                        {                         
                            decimal amount = 0;
                            amount = txtPaidAmount.Text != string.Empty ? Convert.ToDecimal(txtPaidAmount.Text) : 0;                         
                            string taxFormula = dtVendorAccount.Rows[0]["TAX_FORMULA"].ToString();
                            hdfTaxformula.Value = taxFormula;                           
                        }
                        else
                        {
                            txtWHTAmount.Text = "0";
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

                    #region UPLOADEDFILES
                    case ControlsEnum.UPLOADEDFILES:
                        CurrPK = paymentHeaderObj.PVH_PK;
                        PaymentUploadList = paymentHeaderObj.FileList;
                        break;

                    #endregion
                    #region VENDORDETAILSBYPK
                    case ControlsEnum.VENDORDETAILSBYPK:
                        if (dtVendorDetails != null && dtVendorDetails.Rows.Count > 0)
                        {
                            ddlPendingInvType.SelectedIndex = ddlPendingInvType.Items.IndexOf(ddlPendingInvType.Items.FindByValue(dtVendorDetails.Rows[0]["VEN_PO_TYPE"].ToString()));
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

        private void SetVendorPayment()
        {           
            chkVendorforpayemnt.Visible = true;            
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

                #region VATBUYTAXTYPES
                case ControlsEnum.VATBUYTAXTYPES:             
                    ddlVATAccountPopup.Items.Clear();
                    if (dtTaxDetails != null && dtTaxDetails.Rows.Count > 0)
                    {
                        ddlVATAccountPopup.DataSource = CommonFunctions.HtmlDecodeDataTable(dtTaxDetails, Resources.DataFieldRes.RFQResponseTaxHead);
                        ddlVATAccountPopup.DataTextField = Resources.DataFieldRes.RFQResponseTaxHead;
                        ddlVATAccountPopup.DataValueField = Resources.DataFieldRes.RFQResponseTaxPK;
                        ddlVATAccountPopup.DataBind();
                    }                  
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
                #region INVOICE TYPE
                case ControlsEnum.INVOICETYPE:
                    ddlPendingInvType.Items.Clear();
                    if (dtInvoiceType != null && dtInvoiceType.Rows.Count > 0)
                    {
                        ddlPendingInvType.DataSource = dtInvoiceType;
                        ddlPendingInvType.DataTextField = Resources.DataFieldRes.cfgData;
                        ddlPendingInvType.DataValueField = Resources.DataFieldRes.cfgValue;
                        ddlPendingInvType.DataBind();
                    }
                    if (ddlPendingInvType.Items.Count > 1)
                        ddlPendingInvType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region INVOICECATEGORY
                case ControlsEnum.INVOICECATEGORY:
                    ddlPendingInvCategory.Items.Clear();
                    if (dtInvoiceCategory != null && dtInvoiceCategory.Rows.Count > 0)
                    {
                        ddlPendingInvCategory.DataSource = dtInvoiceCategory;
                        ddlPendingInvCategory.DataTextField = Resources.DataFieldRes.cfgData;
                        ddlPendingInvCategory.DataValueField = Resources.DataFieldRes.cfgValue;
                        ddlPendingInvCategory.DataBind();
                    }
                    if (ddlPendingInvCategory.Items.Count > 1)
                        ddlPendingInvCategory.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
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
                int rowCount = 0;
                int pageSize = 0;
                switch (controlType)
                {
                    #region PAYMENTHDRLIST
                    case ControlsEnum.PAYMENTHDRLIST:
                        //Paging Properties                       
                        uclPaging.TotalPages = TotalPages;
                        PageIndex = string.IsNullOrEmpty(PageIndex) ? CommonConstants.SELECT_VALUE_ONE : PageIndex;
                        uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                        if (dtPaymentList != null)
                        {
                            grdPOPaymentHdr.PageIndex = Convert.ToInt32(PageIndex);
                            grdPOPaymentHdr.DataSource = dtPaymentList.DefaultView;
                            grdPOPaymentHdr.DataBind();

                            uclPaging.Visible = true;
                            uclPaging.BindPager();
                        }
                        else
                        {
                            grdPOPaymentHdr.DataSource = null;
                            grdPOPaymentHdr.DataBind();
                            uclPaging.Visible = false;
                        }
                        break;
                    #endregion
                    #region PENDINGINVLIST
                    case ControlsEnum.PENDINGINVLIST:
                        #region Paging Properties
                        rowCount = 0;
                        pageSize = Convert.ToInt32(GetLocalResourceObject("PageSize_InvList"));
                        rowCount = dtPendingInvList.Rows.Count > 0 ? Convert.ToInt32(dtPendingInvList.Rows[0]["TOTAL_ROW_COUNT"].ToString()) : 0;
                        uclPendingInvPaging.TotalPages = rowCount == 0 ? 1 : (rowCount <= pageSize) ? 1 :
                                      (rowCount % pageSize) == 0 ? (rowCount / pageSize) :
                                      (rowCount / pageSize) + 1;
                        PageIndexInv = PageIndexInv == null ? CommonConstants.SELECT_VALUE_ONE : PageIndexInv;
                        uclPendingInvPaging.CurrentPage = Convert.ToInt32(PageIndexInv);
                        #endregion
                        if (dtPendingInvList != null && dtPendingInvList.Rows.Count > 0)
                        {
                            grdPendingInvList.DataSource = dtPendingInvList;
                            grdPendingInvList.DataBind();
                            uclPendingInvPaging.Visible = true;
                            uclPendingInvPaging.BindPager();
                            btnAddSelectedItems.Visible = true;
                        }
                        else
                        {
                            grdPendingInvList.DataSource = null;
                            grdPendingInvList.DataBind();
                            uclPendingInvPaging.Visible = false;
                            btnAddSelectedItems.Visible = false;
                        }
                        break;
                    #endregion
                    #region INVPAYMENTDETAIL
                    case ControlsEnum.INVPAYMENTDETAIL:
                        if (InvPaymentHeaderSession != null)
                        {
                            List<PaymentInvoiceTrxMpgDetails> poInvoiceTList;
                            paymentInvoiceMappingDetailList = new List<PaymentInvoiceTrxMpgDetails>();

                            poInvoiceTList = new List<PaymentInvoiceTrxMpgDetails>();
                            poInvoiceTList = InvPaymentHeaderSession.TrxMpg.ToList();
                            paymentInvoiceMappingDetailList.AddRange(poInvoiceTList);

                            if (paymentInvoiceMappingDetailList != null)
                            {
                                grdInvoiceList.DataSource = paymentInvoiceMappingDetailList;
                                grdInvoiceList.DataBind();
                            }
                            else
                            {
                                grdInvoiceList.DataSource = null;
                                grdInvoiceList.DataBind();
                            }
                        }
                        break;
                    #endregion
                    #region PAYMENTSPLITLIST
                    case ControlsEnum.PAYMENTSPLITLIST:
                        if (paymentPOMpgDetailsList != null)
                        {
                            grdPaymentSplit.DataSource = paymentPOMpgDetailsList;
                            grdPaymentSplit.DataBind();
                        }                        
                        break;
                    #endregion
                    #region CRDRALLOCATION
                    case ControlsEnum.CRDRALLOCATION:
                        grdCrdrAllocation.DataSource = FinPaymentVndCrdrMpgList;
                        grdCrdrAllocation.DataBind();
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
                    #region UPLOADEDFILES
                    case ControlsEnum.UPLOADEDFILES:
                        if (PaymentUploadList != null)
                        {
                            grdUploads.DataSource = PaymentUploadList;
                            grdUploads.DataBind();
                        }
                        break;
                    #endregion
                    #region PAYMENTADJN
                    case ControlsEnum.PAYMENTADJN:
                        if (dtPendingDrAdjn != null && dtPendingDrAdjn.Rows.Count > 0)
                        {
                            grdPaymentSplitAdjn.DataSource = dtPendingDrAdjn;
                            grdPaymentSplitAdjn.DataBind();
                        }
                        if (totBalanceAdjn <= 0)
                        {
                            grdPaymentSplitAdjn.DataSource = null;
                            grdPaymentSplitAdjn.DataBind();
                        }
                        break;
                    #endregion

                    #region WHTPOPUPGRID
                    case ControlsEnum.WHTPOPUPGRID:                       
                        grdWHTTaxDetails.DataSource = TempWHTTaxDetails;
                        grdWHTTaxDetails.DataBind();                       
                        break;
                    #endregion
                    #region VATPOPUPGRID
                    case ControlsEnum.VATPOPUPGRID:
                        grdVATTaxDetails.DataSource = TempVATTaxDetails;
                        grdVATTaxDetails.DataBind();
                        SetVatbuyNotYetDueRowColor();
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
        private void ResetForm()
        {
            CurrPK = 0;
            InvPaymentHeaderSession = null;    
            PaymentInvMappingDetails = null;
            InvoicePOSplitList = null;
            WHTTaxDetails = null;
            TempWHTTaxDetails = null;
            TempConfigMstDetails = null;
            VATTaxDetails = null;
            TempVATTaxDetails = null;
            PaymentModeDetailsList = null;
            PaymentModeConfigMstList = null;
            PaymentCrdrList = null;
            dtPendingDrAdjn = null;
            AppliedInvPkList = null;
        

            tempVATTaxDetails = null;
            FileDetailsList = null;
            DocAttachList = null;
         
            FinPaymentVndAllocationList = null;
            PaymentAdjnList = null;
            PaymentInvDetList = null;
            finInvoiceHdrList = null;
            FinInvoiceVndHdrSelectedList = null;

            txtVendor.Text = string.Empty;
            hdfVendorID.Value = string.Empty;
            txtVendorHd.Text = string.Empty;
            hdfVendorHd.Value = string.Empty;
            lblPaymentNo.Text = Resources.ErpRes.Draft;
            hdfPaymentNo.Value = Resources.ErpRes.Draft;
            txtPaymentDate.Text = string.Empty;
            txtHdrExchangeRate.Text = string.Empty;
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
            grdPendingInvList.DataSource = null;
            grdPendingInvList.DataBind();                  
            grdInvoiceList.DataSource = null;
            grdInvoiceList.DataBind();
            grdPaymentModes.DataSource =null;
            grdPaymentModes.DataBind();
            grdUploads.DataSource = null;
            grdUploads.DataBind();
            ddlStatus.SelectedValue = "3";
            txtSearchDateFrom.Text = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString(Resources.Constants.DateFormatShort);
            hdfSearchDateFrom.Value = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString();
            txtSearchDateTo.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
            hdfSearchDateTo.Value = DateTime.Now.ToString();
        
            ddlBankChargeCurrency.Items.Clear();
            GetFieldValues(ControlsEnum.BANKCURRENCY);
            SetFieldValues(ControlsEnum.BANKCURRENCY);
            txtBankCharge.Text = string.Empty;
            chkBankCharge.Checked = false;
            SetVendorPayment();          
            base.WkfRefID = ucrWrkf.RefID = 0;
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
            txtPendingFromDate.Text = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString(Resources.Constants.DateFormatShort);
            hdfPendingFromDate.Value = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString();
            txtPendingToDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
            hdfPendingToDate.Value = DateTime.Now.ToString();
            ddlPendingInvCategory.ClearSelection();
            ddlPendingInvType.ClearSelection();
           
            IsCreditExist = false;
            ResetForm(ControlsEnum.ADDITEM);
            ResetForm(ControlsEnum.PAYMENTMODES);
            hdfNotTalliedInvoicePk.Value = "0";
            ddlAdjType.SelectedValue = CommonConstants.SELECTVAL;
        }

        private void ResetForm(ControlsEnum controlType)
        {
            switch (controlType)
            {
                #region ADDITEM
                case ControlsEnum.ADDITEM:
                    anchorFile.Visible = false;
                    vrfFileUpload.Enabled = true;
                    CurrSlNo = 0;
                    anchorFile.Attributes.Remove("onclick");
                    break; 
                #endregion
                #region PAYMENTMODES
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
                    decimal.TryParse(txtHdrExchangeRate.Text, out ExchangeRate);
                    decimal.TryParse(txtPaidAmount.Text, out PayableAmnt);
                    decimal.TryParse(txtPaymentAmount.Text, out PaymentAmount);
                    if (PaymentModeDetailsList != null && PaymentModeDetailsList.Count > 0)
                    {
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
                    txtInstrumentNo.CssClass = "Uiinput-amount select-half";
                    txtInstrumentDate.CssClass = "input-small";
                    txtFavourof.CssClass = "multiline-2line";
                    Label5.Visible = true;
                    txtBankCharge.Visible = txtBankCharge.Enabled = true;
                    chkBankCharge.Visible = chkBankCharge.Enabled = true;
                    ddlBankChargeCurrency.Enabled = true;
                    break; 
                #endregion
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

        public string GetFormattedCurrency(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfCurrencyFormat.Value);
        }
        public string GetFormattedCurrencyWithSeperation(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfCurrencyFormatWithSeperation.Value);
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

                if (Convert.ToDecimal(lblOtherCharges.Text.Replace(",", "")) >= OtherCharge + Convert.ToDecimal(hdfOtherChargesPrev.Value))
                {
                    result = true;
                }
                else
                {
                    litErrorMsg.Text = GetLocalResourceObject("Err_msg_2").ToString();//Other Charges exceeds.
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
        #region Workflow Submit
        /// <summary>
        /// Save and submit With workflow 
        /// </summary>
        /// <param name="objSalPayment"></param>
        private void SaveTransaction(POPaymentTradingBO objPOPayment, int workflowFlag)
        {
            int? result = 0;
            string savePath = string.Empty;
            WorkflowDetails wkfDetails = null;
            string TrxNo = string.Empty;
            string action = string.Empty;
            if (objPOPayment == null)
                objPOPayment = new POPaymentTradingBO();
            #region Application Code
            objPOPayment.ATL_APP_TYPE = POGroup == POInvoiceGroup.Goods ? ApplicationType.VPT : POGroup == POInvoiceGroup.Services ? ApplicationType.SIPT : POGroup == POInvoiceGroup.AgtInvoice ? ApplicationType.AIPT : ApplicationType.EIPT;
            objPOPayment.APT_CODE = POGroup == POInvoiceGroup.Goods ? ApplicationType.VPT : POGroup == POInvoiceGroup.Services ? ApplicationType.SIPT : POGroup == POInvoiceGroup.AgtInvoice ? ApplicationType.AIPT : ApplicationType.EIPT;
            #endregion
            #region New workflow Submition
            wkfDetails = ucrWrkf.GetWorkflowDetails();
            objPOPayment.USER_PK = Convert.ToInt16(wkfDetails.UserPK);
            objPOPayment.WKF_APPLICATION = CurrPK;
            objPOPayment.WKF_COMMENTS = wkfDetails.Comments;
            objPOPayment.WKF_TRX_FLAG = workflowFlag;
            objPOPayment.WKF_PROCESS = wkfDetails.ProcessID;
            objPOPayment.WKF_REFERENCE = wkfDetails.ReferenceID;
            objPOPayment.WKF_TASK = wkfDetails.TaskID;
            objPOPayment.WKF_TASK_ACTION = wkfDetails.ActionID;
            action = wkfDetails.ActionText;
            if (isCancelled)
                objPOPayment.ATL_ACTION = (byte)LogAction.CANCEL;
            else
                objPOPayment.ATL_ACTION = (byte)LogAction.SUBMIT;
            #endregion
            string xmlDoc = CommonFunctions.XmlSerialize<POPaymentTradingBO>(objPOPayment);//CommonFunctions.ObjectTOXml(rfqResponseHeaderObj);
            string transactionNumber = string.Empty;
            result = BusinessLogic.POInvoicing.POPaymentTradingBL.SavePaymentTradingWkf(xmlDoc, out transactionNumber);
            if (result.HasValue && result.Value > 0) // Success !  redirect to listing page
            {
                #region File Upload
                if (workflowFlag == (int)(WorkflowTransactionFlag.SAVEANDSUBMIT))
                {
                    #region File Uploads
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

                    foreach (PaymentUploads obj in PaymentUploadList)
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
                    #endregion
                }
                #endregion
                if (result.HasValue && result.Value > 0)
                {                   
                    if (isCancelled)
                    {
                        FillProcessID(1);
                        litErrorMsg.Text = Resources.Messages.Msg_Cancelled_Success;                        
                    }
                    else
                    {
                        litErrorMsg.Text = GetLocalResourceObject("Msg_Submit_Success").ToString();                       
                    }
                    hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                    TextBox WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                    WrkfComments.Text = "";
                    if (string.IsNullOrEmpty(transactionNumber))
                        transactionNumber = lblPaymentNo.Text.Trim();
                    object[] args = new object[2];
                    args[0] = Resources.PageNameRes.PaymentTrading;
                    args[1] = transactionNumber;
                    litErrorMsg.Text = string.Format(litErrorMsg.Text, args);
                    // Show Save Message and redired to listing page   
                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.PaymentTrading);
                    #region Inbox or Listing Page Redirection
                    if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && base.WkfRefID > 0)
                    {
                        ResetForm(ControlsEnum.PAYMENTHDRLIST);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.InboxURL) + "');", true);
                    }
                    else
                    {
                        ResetForm(ControlsEnum.PAYMENTHDRLIST);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                        EntryStatus = EntryStatus.LISTMODE;
                        GetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                        SetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                    }
                    #endregion
                }
                ucrWrkf.ApplicationID = result.Value;
            }
            else
            {
                if (result == (int)DbSaveStatus.SQLERROR)
                {
                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                        + "','" + Resources.ErpRes.Information + "');", true);
                }
                else if (result == (int)DbSaveStatus.REFNOEXIST)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "VendorInvoiceDup", "$(document).ready(function(){ClosePopup();ShowDuplicateVendorInvNoContinue(2);});", true);
                }
                else if (result == (int)DbSaveStatus.CONCURRENCY)
                {
                    litErrorMsg.Text = Resources.PageNameRes.PaymentTrading + " " + Resources.Messages.EditUsedByAnotherUser;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                    + "','" + Resources.ErpRes.Information + "');", true);
                }
                else if (result == (int)DbSaveStatus.CODEEXIST)
                {
                    litErrorMsg.Text = Resources.PageNameRes.PaymentTrading + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                    + "','" + Resources.ErpRes.Information + "');", true);
                }
                else if (result == -25)//check duplicate based on bank + InstrNo + InstrDate
                {
                    //Payment is already created with same Instr No 
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save_PaymentDuplicate").ToString()) + "','" + Resources.Messages.Information + "');", true);
                }
                else if (result == (int)DbSaveStatus.AMOUNTEXCEEDS)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("AmountExceeds").ToString())
                   + "','" + Resources.ErpRes.Information + "');", true);
                }
                else
                {
                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.PaymentTrading);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup(); ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                        + "','" + Resources.ErpRes.Information + "');", true);
                }
                return;
            }

        }
        #endregion
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
        /// Set values to the control when edit details
        /// </summary>
        private void SetUIEditViewVatPopup(GridViewRow grw)
        {
            try
            {
                divVatErrorLabel.Visible = false;
                finVatPaymentDetails = TempVATTaxDetails[grw.RowIndex];             
                if (finVatPaymentDetails != null)
                {                   
                    if (finVatPaymentDetails.WTH_BRANCH !=null)
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
                    if (finVatPaymentDetails.WTH_VENDOR !=null)
                    {
                        hdfVendorPopup.Value = finVatPaymentDetails.WTH_VENDOR.ToString();
                    }
                    else
                    {
                        hdfVendorPopup.Value = string.Empty;
                        GetFieldValues(ControlsEnum.VENDORSELECTEDDTL);
                        SetFieldValues(ControlsEnum.VENDORSELECTEDDTL);
                    }
                    if (finVatPaymentDetails.WTH_TAX_DATE != null)
                    {
                        txtVatTaxInvDate.Text = Convert.ToDateTime(finVatPaymentDetails.WTH_TAX_DATE).ToString(Resources.Constants.DateFormatShort);
                    }
                    if (finVatPaymentDetails.WTH_REFUND_DATE != null)
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

            #region Declaration:ActionHandler
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
            List<PaymentPOMappingDetails> tempInvoicePOSplitList;
            List<PaymentTaxHeader> tempWHTTaxDetails;
            PaymentTaxHeader tempWHTTax;
            List<PaymentCRDRAdjAllocationDtl> tempPaymentAdjnList;
            List<PaymentCRDRMappingDetails> tempPaymentCrdrList;

            FileInfo tempFileInfoObj;
            int selectedItemPK;
            string savePath = string.Empty;
            long? docSaveResult;
            RadioButton rbtn;
            TextBox WrkfComments;
            #endregion
            try
            {
                #region Common Action Settings
                long? result;
                result = 0;
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
                    else if (((DropDownList)sender).ID == "ddlPendingInvCategory")
                    {
                        commonActions = ActionsEnum.DTLSEARCH;
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
                #endregion
                switch (commonActions)
                {
                    #region ItemSelected
                    case ActionsEnum.ITEMSELECTED:
                        GridViewRow gvr;
                        HiddenField hdtDepartment;
                        int department;
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


                        hdtDepartment = gvr.FindControl("hdfDept") as HiddenField;// grdShippingPlanList.FindControl("hdfDept") as HiddenField;
                        if (hdtDepartment != null && int.TryParse(hdtDepartment.Value, out department))
                        {
                            Session[BusinessObject.Common.SessionStrings.CurDept] = department;
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
                    #region NEW
                    case ActionsEnum.NEW:
                        ResetForm();
                        FillProcessID(1);
                        EntryStatus = EntryStatus.NEWMODE;
                        TotalPages = 0;
                        uclPendingInvPaging.CurrentPage = 1;
                        hdfIsPendingInvVisible.Value = "0";
                        SetFieldValues(ControlsEnum.PENDINGINVLIST);
                        break;
                    #endregion
                    #region ADDTOLIST
                    case ActionsEnum.ADDTOLIST:
                        InvHeaderObj = new TradingInvHeaderBO();
                        InvHeaderObj.INVList = new List<TradingInvHeaderListBO>();
                        List<TradingInvHeaderListBO> objItemList = new List<TradingInvHeaderListBO>();
                        TradingInvHeaderListBO objPoList;
                        HiddenField hdfInvID;
                        HiddenField hdfPOVendor;
                        HiddenField hdfInvCurrency;
                        HiddenField hdfPndInvCategory;
                        HiddenField hdfPndInvGroup;
                        if (InvPaymentHeaderSession != null && InvPaymentHeaderSession.TrxMpg != null)
                        {
                            InvPaymentHeaderSession.TrxMpg.ForEach(dtl =>
                            {
                                objPoList = new TradingInvHeaderListBO();
                                objPoList.IVH_PK = Convert.ToInt32(dtl.PVM_INVOICE_HDR);
                                objPoList.IVH_VENDOR = Convert.ToInt32(InvPaymentHeaderSession.PVH_VENDOR);
                                objPoList.IVH_TYPE = dtl.IVH_TYPE;
                                objPoList.IVH_CURRENCY =  Convert.ToInt32(InvPaymentHeaderSession.PVH_CURRENCY);
                                objPoList.IVH_CATEGORY = dtl.IVH_CATEGORY;
                                objPoList.IVH_GROUP = dtl.IVH_GROUP;
                                objItemList.Add(objPoList);
                            });
                        }
                        #region grdPendingInvList
                        foreach (GridViewRow grdrow in grdPendingInvList.Rows)
                        {
                            CheckBox chkInvPendSelect = (CheckBox)grdrow.FindControl("chkInvPendSelect");
                            if (chkInvPendSelect.Checked)
                            {
                                hdfInvID = (HiddenField)grdrow.FindControl("hdfInvoiceID");
                                hdfPOVendor = (HiddenField)grdrow.FindControl("hdfPOVendorPK");
                                hdfInvType = (HiddenField)grdrow.FindControl("hdfInvType");
                                hdfInvCurrency = (HiddenField)grdrow.FindControl("hdfInvCurrency");
                                hdfPndInvCategory = (HiddenField)grdrow.FindControl("hdfPndInvCategory");
                                hdfPndInvGroup = (HiddenField)grdrow.FindControl("hdfPndInvGroup");
                                objPoList = new TradingInvHeaderListBO();
                                objPoList.IVH_PK = Convert.ToInt32(hdfInvID.Value);
                                objPoList.IVH_VENDOR = Convert.ToInt32(hdfPOVendor.Value);
                                objPoList.IVH_TYPE = Convert.ToInt32(hdfInvType.Value); //PURCHASE INVOICE TYPE: Import=>1,	Local => 2
                                objPoList.IVH_CURRENCY = Convert.ToInt32(hdfInvCurrency.Value);
                                objPoList.IVH_CATEGORY = Convert.ToByte(hdfPndInvCategory.Value);
                                objPoList.IVH_GROUP = Convert.ToByte(hdfPndInvGroup.Value);
                                if (objItemList != null && (objItemList.Where(r => r.IVH_VENDOR != Convert.ToInt32(hdfPOVendor.Value)).Count() > 0
                                                            || objItemList.Where(r => r.IVH_TYPE != Convert.ToInt32(hdfInvType.Value)).Count() > 0
                                                            || objItemList.Where(r => r.IVH_CURRENCY != Convert.ToInt32(hdfInvCurrency.Value)).Count() > 0
                                                            || objItemList.Where(r => r.IVH_CATEGORY != Convert.ToByte(hdfPndInvCategory.Value)).Count() > 0
                                                            || objItemList.Where(r => r.IVH_GROUP != Convert.ToByte(hdfPndInvGroup.Value)).Count() > 0
                                                           )
                                    )
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_Err_Muliple_PO").ToString()) + "');", true);
                                    return;
                                }
                                if (objItemList != null && objItemList.Where(r => r.IVH_PK == Convert.ToInt32(hdfInvID.Value)).Count() <= 0)
                                    objItemList.Add(objPoList);
                            }
                        }
                        #endregion
                        if (objItemList.Count == 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_NoRecordsSelected").ToString()) + "');", true);
                            return;
                        }
                        InvHeaderObj.INVList = objItemList;
                        //Avoid already added PO.No need to get that PO details again
                        if (objItemList != null && objItemList.Count > 0 && InvPaymentHeaderSession != null && InvPaymentHeaderSession.TrxMpg != null)
                        {
                            List<string> objInvPkList = InvPaymentHeaderSession.TrxMpg.Select(r => r.PVM_INVOICE_HDR).Distinct().ToList();
                            InvHeaderObj.INVList = objItemList.Where(r => !objInvPkList.Contains(r.IVH_PK.ToString())).ToList();
                        }
                        GetFieldValues(ControlsEnum.INVPAYMENTHEADER);
                        SetFieldValues(ControlsEnum.INVPAYMENTHEADER);
                        SetFieldValues(ControlsEnum.INVPAYMENTDETAIL);
                        SetFieldValues(ControlsEnum.UPLOADEDFILES);
                        TempInvPaymentHeaderSession = InvPaymentHeaderSession;
                        hdfIsPendingInvVisible.Value = "0";  
                        SetFieldValues(ControlsEnum.PENDINGINVLIST);
                        break;
                    #endregion
                    #region REMOVE (Remove Invoice From List)
                    case ActionsEnum.REMOVE:
                        long invPk = long.Parse(((Button)sender).CommandArgument.ToString());
                        if (InvPaymentHeaderSession != null && InvPaymentHeaderSession.TrxMpg != null)
                        {
                            PaymentInvoiceTrxMpgDetails objMappingDtl = InvPaymentHeaderSession.TrxMpg.SingleOrDefault(r => r.IVH_PK == invPk);
                            InvPaymentHeaderSession.TrxMpg.Remove(objMappingDtl);

                            #region Remove VAT TAX
                            if (TempVATTaxDetails != null && TempVATTaxDetails.Count > 0)
                            {
                                List<PaymentTaxHeader> taxHrdList = TempVATTaxDetails.Where(r => r.WTH_PUR_INVOICE == invPk).ToList();
                                foreach (PaymentTaxHeader objtaxHrd in taxHrdList)
                                {
                                    TempVATTaxDetails.Remove(objtaxHrd);
                                }
                            }
                            #endregion

                            #region Remove Credit Notes
                            if (PaymentCrdrList != null && PaymentCrdrList.Count > 0)
                            {
                                List<PaymentCRDRMappingDetails> pmntMpgCrdr = PaymentCrdrList.Where(r => r.PNM_INVOICE_HDR == invPk).ToList();
                                if (pmntMpgCrdr != null && pmntMpgCrdr.Count > 0)
                                {
                                    PaymentCrdrList.Remove(pmntMpgCrdr[0]);
                                }
                            }
                            #endregion

                            #region Remove Adjustments
                            if (PaymentAdjnList != null && PaymentAdjnList.Count > 0)
                            {
                                List<PaymentCRDRAdjAllocationDtl> pmntAdjAlcn = PaymentAdjnList.Where(r => r.PAD_PVM_INVOICE_HDR == invPk && r.PAD_AMOUNT > 0).ToList();
                                if (pmntAdjAlcn != null && pmntAdjAlcn.Count > 0)
                                {
                                    PaymentAdjnList.Remove(pmntAdjAlcn[0]);
                                }
                            }
                            #endregion

                            SetFieldValues(ControlsEnum.INVPAYMENTHEADER);
                            SetFieldValues(ControlsEnum.INVPAYMENTDETAIL);
                            SetFieldValues(ControlsEnum.PENDINGINVLIST);
                        }
                        break;
                    #endregion
                    #region Save,Edit,View,Delete,SaveSubmit,Submit,WkfSubmit,EditForCancel,DeleteSubmit
                    #region SAVE
                    case ActionsEnum.SAVE:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        else//valid
                        {                           
                            int zeroCount = 0;
                            bool IsValidPayNow = true;
                            bool IsValidPayNowWithBalPay = true;                          
                            if (!IsValidPayment())
                            {
                                return;
                            }
                            #region IsValidPayNow,IsValidPayNowWithBalPay
                            foreach (GridViewRow gv in grdInvoiceList.Rows)
                            {
                                lblBaltopay = gv.FindControl("lblBaltopay") as Label;
                                TextBox txtPay = gv.FindControl("txtPayNow") as TextBox;
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
                            #endregion
                            if ((zeroCount <= 0) || (Iscont == true && (hdfIscontYes.Value == "1")))
                            {
                                Iscont = false; decimal adjAmt = txtAdjAmount.Text != string.Empty ? Convert.ToDecimal(txtAdjAmount.Text) : 0;
                                if (grdInvoiceList.Rows.Count >= 1)
                                {
                                    #region MyRegion
                                    if (adjAmt > 0 && ddlAdjType.SelectedValue == CommonConstants.SELECTVAL)
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_AdjType").ToString()) + "','" + Resources.Messages.Information + "');", true);

                                    }
                                    else
                                    {
                                        paymentHeaderObj = new POPaymentTradingBO();
                                        paymentHeaderObj = (POPaymentTradingBO)SetUIValuesToObject(ControlsEnum.INVPAYMENTHEADER);

                                        if (paymentHeaderObj != null)
                                        {
                                            #region Check whether the exchange rate exist for Transaction currency
                                            if (paymentHeaderObj.PVH_EXCHG_RATE <= 0)
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_ExngRate").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                            }
                                            #endregion
                                            #region Check whether the paid amount and paid amount in BC are same
                                            if (paymentHeaderObj.PVH_PAID_AMOUNT != paymentHeaderObj.ModeDetail.Sum(r => r.PDM_PAID_AMOUNT))
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("msg_not_tally_paidamountBc").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                                return;
                                            }
                                            #endregion
                                            #region Check whether the Bank charge greater than payable Amount
                                            if (paymentHeaderObj.PVH_PAID_AMOUNT < paymentHeaderObj.ModeDetail.Sum(r => (r.PDM_BANK_CHARGE_CURR == paymentHeaderObj.PVH_CURRENCY) ? r.PDM_BANK_CHARGE : (r.PDM_BANK_CHARGE / Convert.ToDecimal(r.PDM_EXCHG_RATE))))
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("msg_Bankcharge_Exceeds").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                                return;
                                            }
                                            #endregion


                                            paymentMpgCount = 0;
                                            paymentMpgCount = paymentHeaderObj.TrxMpg.ToList().Count;
                                            if (paymentMpgCount > 0)
                                            {
                                                #region Payment allocation amount is tallied or not Checking.
                                                if (POGroup != POInvoiceGroup.Expense)
                                                {
                                                    bool tally = true;
                                                    decimal CrdrAllocationAmnt = 0;
                                                    GetFieldValues(ControlsEnum.PAYMENTTOLERANCE);
                                                    foreach (PaymentInvoiceTrxMpgDetails trxObj in paymentHeaderObj.TrxMpg)
                                                    {
                                                        InvoicePK = Convert.ToInt64(trxObj.PVM_INVOICE_HDR);
                                                        GetFieldValues(ControlsEnum.INVOICEVNDHDR);
                                                        if (finInvoiceVndHdrListForPaymentSplit != null && finInvoiceVndHdrListForPaymentSplit.Count > 0 && finInvoiceVndHdrListForPaymentSplit[0].IVH_IS_OPENING == 1)
                                                        {
                                                            tally = true;
                                                        }
                                                        else
                                                        {
                                                            if (trxObj.IVH_GROUP == Convert.ToInt16(POInvoiceGroup.AgtInvoice))
                                                            {
                                                                tally = true;
                                                                break;
                                                            }
                                                            else
                                                            {
                                                                if (trxObj.PVM_PAID_AMOUNT > 0)
                                                                {
                                                                    if (PaymentCrdrList != null && PaymentCrdrList.Count > 0)
                                                                    {
                                                                        CrdrAllocationAmnt = PaymentCrdrList.Where(r => r.PNM_INVOICE_HDR == Convert.ToInt32(trxObj.PVM_INVOICE_HDR)).Sum(sm => sm.PNM_PAID_AMOUNT + sm.PNM_ADJ_AMOUNT);
                                                                        if (((trxObj.PVM_PAID_AMOUNT + trxObj.PVM_ADJUST_AMOUNT) - CrdrAllocationAmnt) <= 0)
                                                                        {
                                                                            tally = true;
                                                                            break;
                                                                        }

                                                                    }
                                                                    if (trxObj.POMpg != null && trxObj.POMpg.Count > 0)
                                                                    {
                                                                        decimal PaiWithVariation = 0;
                                                                        PaiWithVariation = trxObj.PVM_PAID_AMOUNT + trxObj.PVM_ADJUST_AMOUNT - CrdrAllocationAmnt + (trxObj.PVM_PAID_AMOUNT * PaymentTollerence);
                                                                        if (trxObj.POMpg.Sum(dtl => dtl.PPO_PAID_AMOUNT) > PaiWithVariation)
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
                                                                else
                                                                {

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
                                                #endregion
                                                if (paymentHeaderObj != null && paymentHeaderObj.TrxMpg != null)
                                                {
                                                    paymentHeaderObj.WKF_TRX_FLAG = Convert.ToInt32(WorkflowTransactionFlag.SAVE);
                                                    string xmlDoc = CommonFunctions.XmlSerialize<POPaymentTradingBO>(paymentHeaderObj);
                                                    bool isCont = true;
                                                    if (isCont)
                                                    {
                                                        string invNumber = string.Empty;
                                                        result = BusinessLogic.POInvoicing.POPaymentTradingBL.SavePaymentTradingWkf(xmlDoc, out invNumber);//SPFIN_PAYMENT_VND_WKF_SAVE
                                                    }
                                                    if (result > 0) // Success !  redirect to listing page
                                                    {
                                                        #region  ATTACHMENT SAVE
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

                                                        foreach (PaymentUploads obj in PaymentUploadList)
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
                                                        #endregion

                                                        #region Show Save Message and redired to listing page
                                                        string transNo = string.Empty;
                                                        if (string.IsNullOrEmpty(lblPaymentNo.Text.Trim()) || lblPaymentNo.Text.Trim() == Resources.Messages.DocGenerationNew)
                                                        {
                                                            litErrorMsg.Text = GetLocalResourceObject("Msg_Saved_Success").ToString();
                                                        }
                                                        else
                                                        {
                                                            litErrorMsg.Text = GetLocalResourceObject("Msg_Save_Success").ToString();
                                                            transNo = lblPaymentNo.Text.Trim();
                                                            object[] args = new object[2];
                                                            args[0] = Resources.PageNameRes.PaymentTrading;
                                                            args[1] = transNo;
                                                            litErrorMsg.Text = string.Format(litErrorMsg.Text, args);
                                                        }
                                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                                        EntryStatus = EntryStatus.LISTMODE;
                                                        ResetForm();
                                                        GetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                                                        SetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                                                        InvPaymentHeaderSession = null;
                                                        #endregion
                                                    }
                                                    else if (result == -25)//check duplicate based on bank + InstrNo + InstrDate
                                                    {
                                                        //Payment is already created with same Instr No 
                                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save_PaymentDuplicate").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                                    }
                                                    else if (result == -26)//There is a journalize entry against this payment
                                                    {                                                        
                                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Modify_VoucherEntry").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                                    }
                                                    else if (result == (int)DbSaveStatus.AMOUNTEXCEEDS)
                                                    {
                                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("AmountExceeds").ToString())
                                                       + "','" + Resources.ErpRes.Information + "');", true);
                                                    }
                                                    else
                                                    {
                                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save_InvoicePayNow").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                            }
                                        }
                                    }
                                    #endregion
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
                    #region  EDIT/VIEW/PAYMENTDETAIL
                    case ActionsEnum.EDIT:
                    case ActionsEnum.VIEW:
                    case ActionsEnum.PAYMENTDETAIL:
                        ResetForm();
                        foreach (GridViewRow grdrow in grdPOPaymentHdr.Rows)
                        {
                            #region grdInvoiceList
                            HiddenField hdfDept;
                            int dept;
                            rbtn = (RadioButton)grdrow.FindControl(Resources.Report.RadioButtonID);
                            if (rbtn.Checked)
                            {
                                hdfEdit.Value = "1";
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfPaymentID")).Value);
                                Session[ERP.Utilities.SessionStrings.VendorPK] = ((HiddenField)grdrow.FindControl("hdfVendorPK")).Value;
                                Session[ERP.Utilities.SessionStrings.Vendor] = ((Label)grdrow.FindControl("lblVendor")).Text;
                                Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);

                                Posted = Convert.ToBoolean(((HiddenField)grdrow.FindControl("hdfPosted")).Value);
                                hdfDept = grdrow.FindControl("hdfDept") as HiddenField;
                                if (Convert.ToInt16(((HiddenField)grdrow.FindControl("hdfDelStatus")).Value) == 1)
                                {
                                    btnEditforCancel.Visible = false;
                                    btnSavePmnt.Visible = false;
                                    btnEdit.Visible = false;
                                    IsDeleted = true;
                                    hdfIsCancelled.Value = "1";
                                }
                                else
                                {
                                    btnSavePmnt.Visible = true;
                                    btnEdit.Visible = true;
                                    IsDeleted = false;
                                    hdfIsCancelled.Value = "0";
                                }
                                if (hdfDept != null && int.TryParse(hdfDept.Value, out dept))
                                {
                                    Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                                    base.SetUserDept();
                                }
                                break;
                            }
                            #endregion
                        }
                        if (bIsChecked)
                        {
                            FillProcessID(1);
                            SetUIEditView(commonActions);
                            workflowCore = new WorkflowCore.CoreService();
                            base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                            SetCancelRef(CurrPK);
                            ucrWrkf.FillWorkFlowDetails();
                            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                                ucrWrkf.ViewType = 1;
                            else
                            {
                                ucrWrkf.ViewType = 0;
                            }
                            btnPrint.Visible = true;
                            ucrWrkf.ViewAction();
                            GetFieldValues(ControlsEnum.BASECURRENCY);
                            lblTotalAmountBC.Text = string.Format(lblTotalAmountBC.Text, hdfBaseCurrency.Value.Split('-')[0].Trim());

                            ModifiedDatePnl.Visible = true;
                            GetFieldValues(ControlsEnum.INVPAYMENTHEADER);

                            SetFieldValues(ControlsEnum.INVPAYMENTHEADER);
                            SetFieldValues(ControlsEnum.INVPAYMENTDETAIL);
                            mode = String.IsNullOrEmpty(ddlMode.SelectedValue) ? 0 : Convert.ToInt32(ddlMode.SelectedValue);
                            SetFieldValues(ControlsEnum.PAYMENTMODESGRID);
                            SetPaymentModeDetails(mode);
                            SetFieldValues(ControlsEnum.UPLOADEDFILES);
                            TempInvPaymentHeaderSession = InvPaymentHeaderSession;

                            uclPendingInvPaging.CurrentPage = 1;
                            PageIndexInv = CommonConstants.SELECT_VALUE_ONE;
                            GetFieldValues(ControlsEnum.PENDINGINVLIST);
                            SetFieldValues(ControlsEnum.PENDINGINVLIST);
                            hdfIsPendingInvVisible.Value = "0";
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Invoice").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region Delete
                    case ActionsEnum.DELETE:
                        if (CurrPK > 0)
                        {
                            result = 0;
                            result = BusinessLogic.POInvoicing.POPaymentTradingBL.DeletePaymentTradingDetails(CurrPK, LastModifiedTime, POGroup == POInvoiceGroup.AgtInvoice ? ApplicationType.AIP : ApplicationType.VP, currentUser.PKUser.ToString());
                            if (result > 0) // Success ! re-initialize the page
                            {
                                //Show Save success message and reset Invoice Entry
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.PaymentHdr);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                FillProcessID(1);
                                ResetForm();
                                GetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                                SetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                                this.btnNew.Focus();
                                EntryStatus = EntryStatus.LISTMODE;
                            }
                            else
                            {
                                #region Show Validation/Error Messages
                                if (result == (int)DbDeleteStatus.SQLERROR)
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.CONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.PaymentTrading + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    EntryStatus = EntryStatus.LISTMODE;
                                    ResetForm();
                                    GetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                                    SetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                                }
                                else if (result == (int)DbDeleteStatus.REFERRED)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.PaymentTrading + " " + Resources.Messages.UsedInAnotherPlace;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.PaymentTrading + " " + Resources.Messages.AlreadyDeleted;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    EntryStatus = EntryStatus.LISTMODE;
                                    ResetForm();
                                    GetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                                    SetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.PaymentTrading);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                #endregion
                            }
                        }
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
                                        paymentHeaderObj = new POPaymentTradingBO();
                                        paymentHeaderObj = (POPaymentTradingBO)SetUIValuesToObject(ControlsEnum.INVPAYMENTHEADER);
                                        if (paymentHeaderObj != null)
                                        {
                                            #region Check whether the exchange rate exist for Transaction currency
                                            if (paymentHeaderObj.PVH_EXCHG_RATE <= 0)
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_ExngRate").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                            }
                                            #endregion
                                            #region Check whether the paid amount and paid amount in BC are same
                                            if (paymentHeaderObj.PVH_PAID_AMOUNT != paymentHeaderObj.ModeDetail.Sum(r => r.PDM_PAID_AMOUNT))
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("msg_not_tally_paidamountBc").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                                return;
                                            }
                                            #endregion
                                            #region Check whether the Bank charge greater than payable Amount
                                            if (paymentHeaderObj.PVH_PAID_AMOUNT < paymentHeaderObj.ModeDetail.Sum(r => (r.PDM_BANK_CHARGE_CURR == paymentHeaderObj.PVH_CURRENCY) ? r.PDM_BANK_CHARGE : (r.PDM_BANK_CHARGE / Convert.ToDecimal(r.PDM_EXCHG_RATE))))
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("msg_Bankcharge_Exceeds").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                                return;
                                            }
                                            #endregion

                                            paymentMpgCount = 0;
                                            paymentMpgCount = paymentHeaderObj.TrxMpg.ToList().Count;
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
                                                    #region Payment allocation amount is tallied or not Checking.
                                                    GetFieldValues(ControlsEnum.PAYMENTTOLERANCE);
                                                    foreach (PaymentInvoiceTrxMpgDetails trxObj in paymentHeaderObj.TrxMpg)
                                                    {
                                                        InvoicePK = Convert.ToInt64(trxObj.PVM_INVOICE_HDR);
                                                        GetFieldValues(ControlsEnum.INVOICEVNDHDR);
                                                        if (finInvoiceVndHdrListForPaymentSplit != null && finInvoiceVndHdrListForPaymentSplit.Count > 0 && finInvoiceVndHdrListForPaymentSplit[0].IVH_IS_OPENING == 1)
                                                        {
                                                            tally = true;
                                                        }
                                                        else
                                                        {
                                                            if (trxObj.IVH_GROUP == Convert.ToInt16(POInvoiceGroup.AgtInvoice))
                                                            {
                                                                tally = true;
                                                                break;
                                                            }
                                                            else
                                                            {
                                                                if (trxObj.PVM_PAID_AMOUNT > 0)
                                                                {
                                                                    if (PaymentCrdrList != null && PaymentCrdrList.Count > 0)
                                                                    {
                                                                        CrdrAllocationAmnt = PaymentCrdrList.Where(r => r.PNM_INVOICE_HDR == Convert.ToInt32(trxObj.PVM_INVOICE_HDR)).Sum(sm => sm.PNM_PAID_AMOUNT + sm.PNM_ADJ_AMOUNT);
                                                                        if (((trxObj.PVM_PAID_AMOUNT + trxObj.PVM_ADJUST_AMOUNT) - CrdrAllocationAmnt) <= 0)
                                                                        {
                                                                            tally = true;
                                                                            break;
                                                                        }

                                                                    }
                                                                    if (trxObj.POMpg != null && trxObj.POMpg.Count > 0)
                                                                    {
                                                                        decimal PaiWithVariation = 0;
                                                                        PaiWithVariation = trxObj.PVM_PAID_AMOUNT + trxObj.PVM_ADJUST_AMOUNT - CrdrAllocationAmnt + (trxObj.PVM_PAID_AMOUNT * PaymentTollerence);
                                                                        if (trxObj.POMpg.Sum(dtl => dtl.PPO_PAID_AMOUNT) > PaiWithVariation)
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
                                                                else
                                                                {

                                                                }
                                                            }
                                                        }
                                                    }
                                                    if (!tally)
                                                    {
                                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("msg_not_tally").ToString() + "','" + Resources.Messages.Information + "');", true);
                                                        return;
                                                    }

                                                    #endregion
                                                    if (tally)
                                                    {
                                                        ucrWrkf.Visible = true;
                                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                                                    }
                                                    else
                                                    {
                                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("msg_allocation_tally").ToString() + "','" + Resources.Messages.Information + "');", true);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save_InvoicePayNow").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                            }
                                        }
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
                    #region Submit
                    case ActionsEnum.SUBMIT:
                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    #endregion
                    #region WRKFSubmit
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
                            isCancelled = false;
                            ucrWrkf.ApplicationID = 0;
                            if (hdfIsSaveSubmit.Value == CommonConstants.SELECT_VALUE_ONE)
                            {
                                paymentHeaderObj = new POPaymentTradingBO();
                                paymentHeaderObj = (POPaymentTradingBO)SetUIValuesToObject(ControlsEnum.INVPAYMENTHEADER);
                                paymentHeaderObj.WKF_FLAG = 1;
                                if (paymentHeaderObj != null && paymentHeaderObj.TrxMpg != null)
                                {
                                    paymentHeaderObj.ATL_ACTION = (byte)LogAction.NEW;
                                    SaveTransaction(paymentHeaderObj, Convert.ToInt32(WorkflowTransactionFlag.SAVEANDSUBMIT));
                                }
                            }
                            else if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)
                            {
                                if (BusinessLogic.CommonManagement.CommonBL.ValidationForCancellation(CurrPK, POGroup == POInvoiceGroup.AgtInvoice ? ApplicationType.AIPT : ApplicationType.VPT))
                                {
                                    isCancelled = true;
                                    SaveTransaction(null, Convert.ToInt32(WorkflowTransactionFlag.SUBMIT));
                                }
                                else
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Err_PP_Cancel").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                    WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                                    if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)
                                        FillProcessID(1);
                                    hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                                    WrkfComments.Text = "";
                                    EntryStatus = EntryStatus.LISTMODE;
                                    ResetForm(ControlsEnum.PAYMENTHDRLIST);
                                    GetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                                    SetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                                }
                            }
                            else
                                SaveTransaction(null, Convert.ToInt32(WorkflowTransactionFlag.SUBMIT));
                        }
                        break;
                    #endregion
                    #region EDITFORCANCEL
                    case ActionsEnum.EDITFORCANCEL:
                        foreach (GridViewRow grdrow in grdPOPaymentHdr.Rows)
                        {
                            #region grdInvoiceList
                            HiddenField hdfDept;
                            int dept;
                            rbtn = (RadioButton)grdrow.FindControl(Resources.Report.RadioButtonID);
                            if (rbtn.Checked)
                            {
                                hdfEdit.Value = "1";
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfPaymentID")).Value);
                                Session[ERP.Utilities.SessionStrings.VendorPK] = ((HiddenField)grdrow.FindControl("hdfVendorPK")).Value;
                                Session[ERP.Utilities.SessionStrings.Vendor] = ((Label)grdrow.FindControl("lblVendor")).Text;
                                Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);

                                Posted = Convert.ToBoolean(((HiddenField)grdrow.FindControl("hdfPosted")).Value);
                                hdfDept = grdrow.FindControl("hdfDept") as HiddenField;
                                if (Convert.ToInt16(((HiddenField)grdrow.FindControl("hdfDelStatus")).Value) == 1)
                                {
                                    btnEditforCancel.Visible = false;
                                    btnSavePmnt.Visible = false;
                                    btnEdit.Visible = false;
                                    IsDeleted = true;
                                    hdfIsCancelled.Value = "1";
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_alreadycancelled").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                    return;
                                }
                                else
                                {
                                    btnSavePmnt.Visible = true;
                                    btnEdit.Visible = true;
                                    IsDeleted = false;
                                    hdfIsCancelled.Value = "0";
                                }
                                if (hdfDept != null && int.TryParse(hdfDept.Value, out dept))
                                {
                                    Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                                    base.SetUserDept();
                                }
                                break;
                            }
                            #endregion
                        }
                        if (bIsChecked)
                        {
                            FillProcessID(11);
                            SetUIEditView(commonActions);
                            workflowCore = new WorkflowCore.CoreService();
                            base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                            SetCancelRef(CurrPK);
                            ucrWrkf.FillWorkFlowDetails();
                            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                                ucrWrkf.ViewType = 1;
                            else
                            {
                                ucrWrkf.ViewType = 0;
                            }
                            btnPrint.Visible = true;
                            ucrWrkf.ViewAction();
                            GetFieldValues(ControlsEnum.BASECURRENCY);
                            lblTotalAmountBC.Text = string.Format(lblTotalAmountBC.Text, hdfBaseCurrency.Value.Split('-')[0].Trim());

                            ModifiedDatePnl.Visible = true;
                            GetFieldValues(ControlsEnum.INVPAYMENTHEADER);

                            SetFieldValues(ControlsEnum.INVPAYMENTHEADER);
                            SetFieldValues(ControlsEnum.INVPAYMENTDETAIL);
                            mode = String.IsNullOrEmpty(ddlMode.SelectedValue) ? 0 : Convert.ToInt32(ddlMode.SelectedValue);
                            SetFieldValues(ControlsEnum.PAYMENTMODESGRID);
                            SetPaymentModeDetails(mode);
                            SetFieldValues(ControlsEnum.UPLOADEDFILES);
                            TempInvPaymentHeaderSession = InvPaymentHeaderSession;

                            uclPendingInvPaging.CurrentPage = 1;
                            PageIndexInv = CommonConstants.SELECT_VALUE_ONE;
                            GetFieldValues(ControlsEnum.PENDINGINVLIST);
                            SetFieldValues(ControlsEnum.PENDINGINVLIST);
                            hdfIsPendingInvVisible.Value = "0";
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Invoice").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }  
                        break;
                    #endregion
                    #region DELETESUBMIT popup
                    case ActionsEnum.DELETESUBMIT:
                         hdfIsSaveSubmit.Value = CommonConstants.SELECTVAL;
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    #endregion
                    #endregion

                    #region Adjustment Allocation (ADJNINVOICEDETAIL,ADJNSPLITSAVE)
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
                        lblVndname.Text = txtVendorHd.Text;
                        lblcurrencyname.Text = txtPaymentCurrency.Text;

                        InvoicePOSplitList = PaymentInvMappingDetails.Where(f => f.IVH_PK == InvoicePK).SingleOrDefault().POMpg.ToList();
                        PaymentCrdrList = PaymentInvMappingDetails.Where(f => f.IVH_PK == InvoicePK).SingleOrDefault().CRDRMpg.ToList();

                        if (hdfPaymentMpgPK != null && !string.IsNullOrEmpty(hdfPaymentMpgPK.Value) && !hdfPaymentMpgPK.Value.Equals("0"))
                        {
                            PaymentMpgPK = Convert.ToInt64(hdfPaymentMpgPK.Value);
                            PaymentAdjnList = PaymentInvMappingDetails.Where(f => f.IVH_PK == InvoicePK).SingleOrDefault().AllocationMpg.ToList();
                            PaymentAdjnList.ForEach(dtl =>
                                               {
                                                   dtl.PAD_PVM_INVOICE_HDR = InvoicePK;
                                               });

                            GetFieldValues(ControlsEnum.PAYMENTADJN);
                            SetFieldValues(ControlsEnum.PAYMENTADJN);
                        }
                        else
                        {
                            PaymentMpgPK = Convert.ToInt64(hdfPaymentMpgPK.Value);
                            GetFieldValues(ControlsEnum.PAYMENTADJN);
                            SetFieldValues(ControlsEnum.PAYMENTADJN);
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divSoSplitUpAdjn]','" + GetLocalResourceObject("AdjAllocation").ToString() + "','800','300');", true);
                        break;
                    #endregion
                    #region ADJNSPLITSAVE
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
                                            FinPaymentVndAllocationList = new List<PaymentCRDRAdjAllocationDtl>();
                                            FinPaymentVndAllocationList = (List<PaymentCRDRAdjAllocationDtl>)SetUIValuesToObject(ControlsEnum.ADJNSPLITLIST);

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
                                                    decimal AllocatedAmt = PaymentAdjnList.Where(sa => sa.PAD_ALCN_PAYMENT_TRX == Convert.ToInt32(hdfReceiptAdjnPK.Value) && sa.PAD_PVM_INVOICE_HDR != InvoicePK).Sum(a => a.PAD_AMOUNT);
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
                                                    decimal AllocatedAmt = PaymentAdjnList.Where(sa => sa.PAD_ALCN_CDH == Convert.ToInt32(hdfCrDrPK.Value) && sa.PAD_PVM_INVOICE_HDR != InvoicePK).Sum(a => a.PAD_AMOUNT);
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
                                                    tempPaymentAdjnList.Where(dtl => dtl.PAD_PVM_INVOICE_HDR == InvoicePK)
                                                        .ToList().ForEach(dtl => tempPaymentAdjnList.Remove(dtl));
                                                    FinPaymentVndAllocationList.ForEach(dtl =>
                                                    {
                                                        dtl.PAD_PAYMENT_TRX = PaymentMpgPK;
                                                        dtl.PAD_PVM_INVOICE_HDR = InvoicePK;
                                                        tempPaymentAdjnList.Add(dtl);
                                                    });
                                                    PaymentAdjnList = tempPaymentAdjnList;
                                                    //Update this changes to Orginal List
                                                    PaymentInvMappingDetails.Where(f => f.IVH_PK == InvoicePK).SingleOrDefault().AllocationMpg = PaymentAdjnList;

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

                                                        #region RESETTING payment split after adj apply
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
                                                            splitotaltamnt = tempInvoicePOSplitList.Where(dtl => dtl.POH_IVH_PK == InvoicePK).Sum(r => r.PPO_PAID_AMOUNT);
                                                            if (paynow != splitotaltamnt)
                                                            {
                                                                if (tempInvoicePOSplitList.Where(dtl => dtl.POH_IVH_PK == InvoicePK).Count() == 1)
                                                                {
                                                                    tempInvoicePOSplitList.Where(dtl => dtl.POH_IVH_PK == InvoicePK)
                                                                       .ToList().ForEach(dtl =>
                                                                       {
                                                                           dtl.PPO_PAID_AMOUNT = paynow;
                                                                           dtl.PPO_TAX_AMOUNT = ttaxamt;
                                                                           dtl.PPO_OTHER_AMOUNT = otherCharges;
                                                                       });
                                                                }
                                                                else
                                                                {
                                                                    tempInvoicePOSplitList.Where(dtl => dtl.POH_IVH_PK == InvoicePK)
                                                                       .ToList().ForEach(dtl =>
                                                                       {
                                                                           dtl.PPO_PAID_AMOUNT = 0;
                                                                           dtl.PPO_TAX_AMOUNT = 0;
                                                                           dtl.PPO_OTHER_AMOUNT = 0;
                                                                       });
                                                                }

                                                                InvoicePOSplitList = tempInvoicePOSplitList;
                                                            }
                                                            //Update this changes to Orginal List
                                                            PaymentInvMappingDetails.Where(f => f.IVH_PK == InvoicePK).SingleOrDefault().POMpg = InvoicePOSplitList;
                                                        }
                                                        #endregion

                                                        #region RESETTING CR/DR Allocation
                                                        tempPaymentCrdrList = PaymentCrdrList;
                                                        if (tempPaymentCrdrList != null && tempPaymentCrdrList.Where(r => r.PNM_INVOICE_HDR == InvoicePK).ToList().Count > 0)
                                                        {
                                                            tempPaymentCrdrList.Where(r => r.PNM_INVOICE_HDR == InvoicePK).ToList().ForEach(dtl =>
                                                            {
                                                                dtl.PNM_ADJ_AMOUNT = 0;
                                                                dtl.PNM_PAID_AMOUNT = 0;
                                                            });
                                                            PaymentCrdrList = tempPaymentCrdrList;

                                                            //Update this changes to Orginal List
                                                            PaymentInvMappingDetails.Where(f => f.IVH_PK == InvoicePK).SingleOrDefault().CRDRMpg = PaymentCrdrList;

                                                            decimal TotalCrdrAmnt = 0;
                                                            TotalCrdrAmnt = PaymentCrdrList.Where(r => r.PNM_INVOICE_HDR == InvoicePK).Sum(r => r.PNM_PAID_AMOUNT + r.PNM_ADJ_AMOUNT);
                                                            if (lblCrdrAlcnAmount != null)
                                                                lblCrdrAlcnAmount.Text = lblCrdrAlcnAmount.ToolTip = string.Format("{0:c}", TotalCrdrAmnt);
                                                        }
                                                        #endregion

                                                        #region RESETTING Payment PO Allocation
                                                        InvoiceDetails(InvoicePK);
                                                        PaymentSplitSave(false);
                                                        #endregion

                                                    }
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1", "ClosePopup();", true);
                                                }
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateAdjnFooter", "$(document).ready(function(){CalculateAdjnFooter();});", true);
                                            }
                                            else
                                            {
                                                divErrorAdj.Visible = true;
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divSoSplitUpAdjn]','" + GetLocalResourceObject("adj_Allocated").ToString() + "','800','300');", true);
                                            }
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
                    #endregion

                    #region INVOICEDETAIL
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
                        break;
                    #endregion
                    #region PAYMENTSPLITSAVE
                    case ActionsEnum.PAYMENTSPLITSAVE:
                        PaymentSplitSave(true);
                        break;
                    #endregion
                    #region CHANGEPAYNOW
                    case ActionsEnum.CHANGEPAYNOW:
                        hdfInvoicePK = (HiddenField)(((sender as Button).Parent.Parent as GridViewRow).FindControl("hdfInvoicePK"));
                        lblCrdrAlcnAmount = (Label)(((sender as Button).Parent.Parent as GridViewRow).FindControl("lblCrdrAlcnAmount"));
                        if (hdfInvoicePK != null && !string.IsNullOrEmpty(hdfInvoicePK.Value) || !hdfInvoicePK.Value.Equals("0"))
                        {
                            InvoicePK = Convert.ToInt64(hdfInvoicePK.Value);
                            tempInvoicePOSplitList = PaymentInvMappingDetails.Where(f => f.IVH_PK == InvoicePK).SingleOrDefault().POMpg.ToList(); //InvoicePOSplitList;

                            tempInvoicePOSplitList.Where(dtl => dtl.POH_IVH_PK == InvoicePK)
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
                            tempPaymentCrdrList = PaymentInvMappingDetails.Where(f => f.IVH_PK == InvoicePK).SingleOrDefault().CRDRMpg.ToList();//PaymentCrdrList;
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


                    #region CREDIT NOTE ALLOCATION(CRDRALLOCATION,CRDRALLOCATIONSAVE)
                    #region CRDRALLOCATION
                    case ActionsEnum.CRDRALLOCATION:
                        divCrdrErrorMsg.Visible = false;
                        FinPaymentVndCrdrMpgList = null;
                        hdfInvoicePK = (HiddenField)((((Button)sender).Parent).FindControl("hdfInvoicePK"));
                        Label lblTotalAmount = (Label)((((Button)sender).Parent).FindControl("lblTotalAmount"));
                        TextBox txtPayNowAmnt = (TextBox)((((Button)sender).Parent).FindControl("txtPayNow"));
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
                            lblInvSplitAmount_CrdrAlcn.Text = lblInvSplitAmount_CrdrAlcn.ToolTip = lblTotalAmount.Text;
                            lblInvSplitReceived_CrdrAlcn.Text = lblInvSplitReceived_CrdrAlcn.ToolTip = lblPaid.Text;
                            lblInvSplitReceiveNow_CrdrAlcn.Text = lblInvSplitReceiveNow_CrdrAlcn.ToolTip = String.Format("{0:c}", PayNowAmount);

                            List<PaymentInvoiceTrxMpgDetails> tempInvoiceTrxMpgDetails = PaymentInvMappingDetails;
                            tempPaymentInvoiceTrxMpgDetails = tempInvoiceTrxMpgDetails.Where(dtl => dtl.IVH_PK == InvoicePK).ToList();
                            GetUIValuesFromObject(ControlsEnum.CRDRALLOCATION);
                        }
                        if (PaymentCrdrList != null && PaymentCrdrList.Count > 0)
                            FinPaymentVndCrdrMpgList = PaymentCrdrList.Where(dtl => dtl.PNM_INVOICE_HDR == InvoicePK).ToList();
                        SetFieldValues(ControlsEnum.CRDRALLOCATION);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateTotalCreditSplitFooter", "$(document).ready(function(){CalculateTotalCreditSplit();});", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divCrdrAllocation]','" + GetLocalResourceObject("CreditAllocation").ToString() + "','850','300');", true);
                        break;
                    #endregion

                    #region CRDRALLOCATIONSAVE
                    case ActionsEnum.CRDRALLOCATIONSAVE:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        else
                        {
                            if (grdCrdrAllocation.Rows.Count >= 1)
                            {
                                IsCreditNoteApplied = false;
                                divErrorLabel.Visible = false;
                                FinPaymentVndCrdrMpgList = new List<PaymentCRDRMappingDetails>();
                                FinPaymentVndCrdrMpgList = (List<PaymentCRDRMappingDetails>)SetUIValuesToObject(ControlsEnum.CRDRSPLITLIST);
                                if (FinPaymentVndCrdrMpgList != null)
                                {
                                    decimal CrAlcnAmount = 0;
                                    decimal TotalCrPayNow = 0;
                                    decimal TotalCrAdj = 0;
                                    tempPaymentCrdrList = PaymentCrdrList.DeepClone();
                                    FinPaymentVndCrdrMpgList.ForEach(dtl =>
                                    {
                                        PaymentCRDRMappingDetails objPaymentCrdrMpg = tempPaymentCrdrList.SingleOrDefault(r => r.PNM_INVOICE_HDR == InvoicePK && r.PNM_CRDR_MPG == dtl.PNM_CRDR_MPG);
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
                    #endregion

                    #region WHTTAX(WHTTAXHEADER,WHT_ACCOUNT_INDEX_CHANGED_POPUP,WHTTAXADD,WHTTAXDELETE,WHTTAXAPPLY,POPUPGRIDEDITWHT,WHTTAXSAVE,WHTCHANGETYPE,PRINTWHT)
                    #region WHTTAXHEADER
                    case ActionsEnum.WHTTAXHEADER:
                        GetFieldValues(ControlsEnum.VENDORACCOUNT);
                        SetFieldValues(ControlsEnum.VENDORACCOUNT);
                        GetFieldValues(ControlsEnum.VENDOR);
                        SetFieldValues(ControlsEnum.WHTPOPUPGRID);
                        SetFieldValues(ControlsEnum.VENDOR);
                        GetFieldValues(ControlsEnum.PAYMENTTYPE);
                        SetFieldValues(ControlsEnum.PAYMENTTYPE);
                        ShowWhtPopup();
                        SetWhtButtons();
                        break;
                    #endregion
                    #region WHT_ACCOUNT_INDEX_CHANGED_POPUP
                    case ActionsEnum.WHT_ACCOUNT_INDEX_CHANGED_POPUP:

                        whtTaxpk = !string.IsNullOrEmpty(hdfWHTAccountPopup.Value) ? Convert.ToInt32(hdfWHTAccountPopup.Value) : 0;
                        GetFieldValues(ControlsEnum.VENDORACCOUNTTAX);
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
                        break;
                    #endregion
                    #region WHT TAX ADD
                    case ActionsEnum.WHTTAXADD:
                        bool errorWHTAdd = false;
                        bool errorWHTAmount = false;
                        int WhtDetRowIndex = -1;
                        if (ViewState["WhtDetRowIndex"] != null) WhtDetRowIndex = (int)(ViewState["WhtDetRowIndex"]);

                        string WHTVendorAddressType = string.Empty;
                        if (!string.IsNullOrEmpty(hdfWthAddressType.Value))
                        {
                            WHTVendorAddressType = hdfWthAddressType.Value;
                        }
                        tempWHTTaxDetails = null;
                        tempWHTTaxDetails = TempWHTTaxDetails.DeepClone();
                        if (tempWHTTaxDetails != null && WhtDetRowIndex >= 0)
                        {
                            tempWHTTaxDetails[WhtDetRowIndex].WTH_TAX = Convert.ToInt32(hdfWHTAccountPopup.Value);
                            tempWHTTaxDetails[WhtDetRowIndex].WTH_TRX_HDR = CurrPK;
                            tempWHTTaxDetails[WhtDetRowIndex].WTH_TYPE = 1;
                            tempWHTTaxDetails[WhtDetRowIndex].WTH_CATEGORY = (byte)WHTCategoryEnum.WHT;
                            tempWHTTaxDetails[WhtDetRowIndex].WTH_TAX_CATEGORY = string.IsNullOrEmpty(hdfWHTTaxCategory.Value) ? Convert.ToByte(1) : Convert.ToByte(hdfWHTTaxCategory.Value);
                            tempWHTTaxDetails[WhtDetRowIndex].WTH_NAME = hdfWHTTaxName.Value;
                            tempWHTTaxDetails[WhtDetRowIndex].WTH_DESC = HttpUtility.HtmlEncode(txtDescriptionPopup.Text);
                            tempWHTTaxDetails[WhtDetRowIndex].WTH_FORM_NO = ddlFormno.SelectedValue;
                            tempWHTTaxDetails[WhtDetRowIndex].WTH_PARTY_NAME = HttpUtility.HtmlEncode(txtCustomerTxtWHT.Text);
                            tempWHTTaxDetails[WhtDetRowIndex].WTH_ADDRESS = HttpUtility.HtmlEncode(txtpartyads.Text);
                            tempWHTTaxDetails[WhtDetRowIndex].WTH_TAX_ID = txtTaxid.Text;
                            tempWHTTaxDetails[WhtDetRowIndex].WTH_AMOUNT = Convert.ToDecimal(txtPopupWHTAmount.Text);
                            tempWHTTaxDetails[WhtDetRowIndex].WTH_TAX_AMT = Convert.ToDecimal(txtWHTTaxAmountPopup.Text);
                            tempWHTTaxDetails[WhtDetRowIndex].WTH_PAYMENT_TYPE = Convert.ToByte(ddlPayType.SelectedValue);
                            tempWHTTaxDetails[WhtDetRowIndex].WTH_BRANCH_TEXT = HttpUtility.HtmlEncode(txtWthBranchCode.Text);
                            tempWHTTaxDetails[WhtDetRowIndex].WTH_BRANCH = WHTVendorAddressType;
                            tempWHTTaxDetails[WhtDetRowIndex].WTH_BRANCH_NAME = HttpUtility.HtmlEncode(txtWthAddressType.Text);
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
                            tempWHTTax = tempWHTTaxDetails.SingleOrDefault(tax => tax.WTH_TAX == Convert.ToInt32(hdfWHTAccountPopup.Value) && tax.WTH_FORM_NO == ddlFormno.SelectedValue && tax.WTH_PARTY_NAME == txtCustomerTxtWHT.Text);
                            if (tempWHTTax == null)
                            {
                                tempWHTTax = new PaymentTaxHeader();
                                tempWHTTax = new PaymentTaxHeader();
                                tempWHTTax.WTH_AMOUNT = string.IsNullOrEmpty(txtPopupWHTAmount.Text) ? Convert.ToDecimal(0) : Convert.ToDecimal(txtPopupWHTAmount.Text);
                                tempWHTTax.WTH_TAX_AMT = string.IsNullOrEmpty(txtWHTTaxAmountPopup.Text) ? Convert.ToDecimal(0) : Convert.ToDecimal(txtWHTTaxAmountPopup.Text);
                                if (!errorWHTAmount)
                                {
                                    tempWHTTax.WTH_TAX = Convert.ToInt32(hdfWHTAccountPopup.Value);
                                    tempWHTTax.WTH_PK = 0;
                                    tempWHTTax.WTH_PAYMENT_HDR = CurrPK;
                                    tempWHTTax.WTH_TYPE = 1;
                                    tempWHTTax.WTH_TAX_CATEGORY = string.IsNullOrEmpty(hdfWHTTaxCategory.Value) ? Convert.ToByte(1) : Convert.ToByte(hdfWHTTaxCategory.Value);
                                    tempWHTTax.WTH_NAME = hdfWHTTaxName.Value;
                                    tempWHTTax.WTH_DESC = HttpUtility.HtmlEncode(txtDescriptionPopup.Text);
                                    tempWHTTax.WTH_FORM_NO = ddlFormno.SelectedValue;
                                    tempWHTTax.WTH_PARTY_NAME = HttpUtility.HtmlEncode(txtCustomerTxtWHT.Text);
                                    tempWHTTax.WTH_ADDRESS = HttpUtility.HtmlEncode(txtpartyads.Text);
                                    tempWHTTax.WTH_TAX_ID = txtTaxid.Text;
                                    tempWHTTax.WTH_CATEGORY = (byte)WHTCategoryEnum.WHT;
                                    tempWHTTax.WTH_TAX_DATE = string.IsNullOrEmpty(txtPaymentDate.Text.Trim()) ? DateTime.Now : DateTime.Parse(txtPaymentDate.Text.Trim());
                                    tempWHTTax.WTH_PAYMENT_TYPE = Convert.ToByte(ddlPayType.SelectedValue);
                                    tempWHTTax.WTH_BRANCH_TEXT = HttpUtility.HtmlEncode(txtWthBranchCode.Text);
                                    tempWHTTax.WTH_BRANCH = WHTVendorAddressType;
                                    tempWHTTax.WTH_BRANCH_NAME = HttpUtility.HtmlEncode(txtWthAddressType.Text);
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
                                    tempWHTTax = tempWHTTaxDetails.FirstOrDefault(rfq => rfq.WTH_PK == taxPK);
                                }
                                else
                                {
                                    if (hdfTaxName != null)
                                    {
                                        tempWHTTax = tempWHTTaxDetails.FirstOrDefault(rfq => rfq.WTH_NAME == hdfTaxName.Value);
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
                        break;
                    #endregion
                    #region WHTTAXAPPLY
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
                    #region POPUPGRIDEDITWHT
                    case ActionsEnum.POPUPGRIDEDITWHT:
                        GridViewRow grwWhtDetails = (GridViewRow)((ImageButton)(sender)).Parent.Parent;
                        SetUIEditViewWhtPopup(grwWhtDetails);
                        ShowWhtPopup();
                        break;
                    #endregion
                    #region WHTTAXSAVE
                    case ActionsEnum.WHTTAXSAVE:                        
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                        break;
                    #endregion
                    #region WHTCHANGETYPE
                    case ActionsEnum.WHTCHANGETYPE:
                        txtWthBranchCode.Text = string.Empty;
                        GetFieldValues(ControlsEnum.VENDORCONTACTFORWHT);
                        SetFieldValues(ControlsEnum.VENDORCONTACTFORWHT);
                        ShowWhtPopup();
                        break;
                    #endregion
                    #region PRINTWHT
                    case ActionsEnum.PRINTWHT:
                        if (CurrPK > 0)
                        {
                            if (TempWHTTaxDetails != null & TempWHTTaxDetails.Count > 0)
                            {                                
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx" + "?ID=" + CurrPK + "&APPTYPE=" + ApplicationType.VP +
                                "&APPSUBTYPE=" + Convert.ToInt32(AppSubTypeVP.WHTCERTIFICATE) + "');", true);  
                            }
                        }
                        break;
                    #endregion
                    #endregion

                    #region VATTAX (VATTAXHEADER,VAT_ACCOUNT_INDEX_CHANGED_POPUP,VATTAXADD,VATTAXDELETE,VATTAXAPPLY,TAXTYPECHANGED,CHANGETYPE,POPUPGRIDEDIT,VATTAXSAVE)
                    #region VATTAXHEADER
                    case ActionsEnum.VATTAXHEADER:
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
                       // GetFieldValues(ControlsEnum.VATPOPUPGRID);
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
                    #region VAT_ACCOUNT_INDEX_CHANGED_POPUP
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
                    #region VATTAXADD
                    case ActionsEnum.VATTAXADD:
                        bool errorVATAdd = false;
                        bool errorVATAmount = false;
                        decimal vatTaxAmnt = 0;
                        decimal vatTaxAmntSplit = 0;
                        int VatDetRowIndex = -1;
                        string VendorAddressType = string.Empty;
                        string VendorPopupPk = string.Empty;
                        if (ViewState["VatDetRowIndex"] != null) VatDetRowIndex = (int)(ViewState["VatDetRowIndex"]);
                        if (VatDetRowIndex < 0)
                            GetFieldValues(ControlsEnum.PAYMENTTAXHDR);

                        tempVATTaxDetails = null;
                        tempVATTaxDetails = TempVATTaxDetails;
                        divVatErrorLabel.Visible = false;
                        lblVatSplitErrorMessage.Text = string.Empty;

                        if (!string.IsNullOrEmpty(hdfAddressType.Value))
                        {
                            VendorAddressType = hdfAddressType.Value;
                        }
                        if (!string.IsNullOrEmpty(hdfVendorPopup.Value))
                        {
                            VendorPopupPk = hdfVendorPopup.Value;
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

                            tempVATTaxDetails[VatDetRowIndex].WTH_TAX = Convert.ToInt32(ddlVATAccountPopup.SelectedValue);
                            tempVATTaxDetails[VatDetRowIndex].WTH_PAYMENT_HDR = CurrPK;
                            tempVATTaxDetails[VatDetRowIndex].WTH_TYPE = 1;
                            tempVATTaxDetails[VatDetRowIndex].WTH_CATEGORY = (byte)WHTCategoryEnum.VATBUY;
                            tempVATTaxDetails[VatDetRowIndex].WTH_TAX_INV_NO = txtVatTaxInvNo.Text;
                            tempVATTaxDetails[VatDetRowIndex].WTH_INV_RECEIVED = chkOriginalinvoice.Checked ? (byte)1 : (byte)0;
                            tempVATTaxDetails[VatDetRowIndex].WTH_PUR_INVOICE = Convert.ToInt64(ddlPurInvNo.SelectedValue);
                            tempVATTaxDetails[VatDetRowIndex].WTH_VENDOR = VendorPopupPk;
                            tempVATTaxDetails[VatDetRowIndex].WTH_BRANCH = VendorAddressType;
                            tempVATTaxDetails[VatDetRowIndex].WTH_BRANCH_TEXT = txtBranchCode.Text;
                            tempVATTaxDetails[VatDetRowIndex].WTH_BRANCH = VendorAddressType;
                            tempVATTaxDetails[VatDetRowIndex].WTH_BRANCH_NAME = HttpUtility.HtmlEncode(txtAddressType.Text);
                            tempVATTaxDetails[VatDetRowIndex].WTH_BRANCH_TYPE = chkHeadOffice.Checked ? (byte)VendorContactTypeEnum.HeadOffice : (byte)VendorContactTypeEnum.Branch;
                            tempVATTaxDetails[VatDetRowIndex].WTH_TAX_DATE = string.IsNullOrEmpty(txtVatTaxInvDate.Text.Trim()) ? DateTime.Now : DateTime.Parse(txtVatTaxInvDate.Text.Trim());
                            tempVATTaxDetails[VatDetRowIndex].WTH_REFUND_DATE = string.IsNullOrEmpty(txtVatRefundDate.Text.Trim()) ? DateTime.Now : DateTime.Parse(txtVatRefundDate.Text.Trim());
                            tempVATTaxDetails[VatDetRowIndex].WTH_TAX_CATEGORY = Convert.ToByte(ddlVATAccountPopup.SelectedValue);// string.IsNullOrEmpty(hdfVATBUYTaxCategory.Value) ? Convert.ToByte(1) : Convert.ToByte(hdfVATBUYTaxCategory.Value);
                            tempVATTaxDetails[VatDetRowIndex].WTH_NAME = ddlVATAccountPopup.SelectedItem.Text;
                            //tempVATTaxDetails[VatDetRowIndex].WTH_DESC = txtDescriptionPopup.Text;
                            //tempVATTaxDetails[VatDetRowIndex].WTH_FORM_NO = Convert.ToInt32(ddlFormno.SelectedValue);
                            tempVATTaxDetails[VatDetRowIndex].WTH_PARTY_NAME = HttpUtility.HtmlEncode(txtVendorPopup.Text);
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
                                tempVATTax = new PaymentTaxHeader();
                                tempVATTax = CommonFunctions.Initilize<PaymentTaxHeader>();
                                tempVATTax.WTH_AMOUNT = string.IsNullOrEmpty(txtBeforeTaxAmount.Text) ? Convert.ToDecimal(0) : Convert.ToDecimal(txtBeforeTaxAmount.Text);
                                tempVATTax.WTH_TAX_AMT = string.IsNullOrEmpty(txtVATTaxAmountPopup.Text) ? Convert.ToDecimal(0) : Convert.ToDecimal(txtVATTaxAmountPopup.Text);
                                if (!errorVATAmount)
                                {
                                    tempVATTax.WTH_TAX = Convert.ToInt32(hdfVATAccountPopup.Value);
                                    tempVATTax.WTH_PK = 0;
                                    tempVATTax.WTH_PAYMENT_HDR = CurrPK;
                                    tempVATTax.WTH_TYPE = 1;
                                    tempVATTax.WTH_CATEGORY = (byte)WHTCategoryEnum.VATBUY;
                                    tempVATTax.WTH_TAX_INV_NO = txtVatTaxInvNo.Text;
                                    tempVATTax.WTH_INV_RECEIVED = chkOriginalinvoice.Checked ? (byte)1 : (byte)0;
                                    tempVATTax.WTH_PUR_INVOICE = Convert.ToInt64(ddlPurInvNo.SelectedValue);
                                    tempVATTax.WTH_VENDOR = VendorPopupPk;
                                    tempVATTax.WTH_BRANCH_TEXT = txtBranchCode.Text;
                                    tempVATTax.WTH_BRANCH = VendorAddressType;
                                    tempVATTax.WTH_BRANCH_NAME = HttpUtility.HtmlEncode(txtAddressType.Text);
                                    tempVATTax.WTH_BRANCH_TYPE = chkHeadOffice.Checked ? (byte)VendorContactTypeEnum.HeadOffice : (byte)VendorContactTypeEnum.Branch;
                                    tempVATTax.WTH_TAX_DATE = string.IsNullOrEmpty(txtVatTaxInvDate.Text.Trim()) ? DateTime.Now : DateTime.Parse(txtVatTaxInvDate.Text.Trim());
                                    tempVATTax.WTH_REFUND_DATE = string.IsNullOrEmpty(txtVatRefundDate.Text.Trim()) ? DateTime.Now : DateTime.Parse(txtVatRefundDate.Text.Trim());
                                    tempVATTax.WTH_TAX_CATEGORY = Convert.ToByte(ddlVATAccountPopup.SelectedValue);// string.IsNullOrEmpty(hdfVATBUYTaxCategory.Value) ? Convert.ToByte(1) : Convert.ToByte(hdfVATBUYTaxCategory.Value);
                                    tempVATTax.WTH_NAME = ddlVATAccountPopup.SelectedItem.Text;
                                    tempVATTax.WTH_FORM_NO = string.Empty;
                                    tempVATTax.WTH_PARTY_NAME = HttpUtility.HtmlEncode(txtVendorPopup.Text);
                                    tempVATTax.WTH_TAX_ID = txtVatTaxId.Text;
                                    tempVATTax.WTH_ITEM_TEXT = HttpUtility.HtmlEncode(txtMaterial.Text);
                                    tempVATTax.WTH_AMOUNT = Convert.ToDecimal(txtBeforeTaxAmount.Text);
                                    tempVATTax.WTH_TAX_AMT = Convert.ToDecimal(txtVATTaxAmountPopup.Text);
                                    tempVATTaxDetails.Add(tempVATTax);
                                    TempVATTaxDetails = tempVATTaxDetails;
                                    SetFieldValues(ControlsEnum.VATPOPUPGRID);
                                    finVatPaymentDetails = null;
                                    ViewState["VatDetRowIndex"] = null;
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
                    #region TAXTYPECHANGED
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
                    #region POPUPGRIDEDIT
                    case ActionsEnum.POPUPGRIDEDIT:
                        GridViewRow grwVatDetails = (GridViewRow)((ImageButton)(sender)).Parent.Parent;
                        SetUIEditViewVatPopup(grwVatDetails);
                        ShowVatbuyPopup();
                        break;
                    #endregion
                    #region VATTAXSAVE
                    case ActionsEnum.VATTAXSAVE:                       

                        break;
                    #endregion
                    #endregion

                    #region PAYMENT_MODE_INDEX_CHANGED
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
                            txtFavourof.Text = HttpUtility.HtmlDecode(txtVendorHd.Text);
                        break;

                    #endregion
                    #region SEARCHACCOUNTNO
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
                            PaymentModeDetails PaymentModeDtl = null;
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
                            SetFieldValues(ControlsEnum.PAYMENTMODESGRID);
                            ResetForm(ControlsEnum.PAYMENTMODES);
                            EnableDisableExchangeRate();
                        }
                        break;
                    #endregion

                    #region JOURNALIZE(JOURNALIZEUPDATE,JOURNALIZESAVE,REVERSESAVE,RETURNSAVE,JOURNALIZESUBMIT,JOURNALIZEDELETE,JOURNALIZECANCEL,REVERSECANCEL,RETURNCANCEL,REVERSE,CHEQUERETURN,REVERSESUBMIT,RETURNSUBMIT,REVERSEDELETE,RETURNDELETE)
                    #region JOURNALIZE
                    case ActionsEnum.JOURNALIZE:                       
                        GetFieldValues(ControlsEnum.INVPAYMENTHEADER);
                        SetUIValuesToObject(ControlsEnum.JOURNALIZE);
                        break;
                    #endregion
                    #region JOURNALIZEUPDATE
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
                    #region JOURNALIZESAVE
                    case ActionsEnum.REVERSESAVE:
                    case ActionsEnum.JOURNALIZESAVE:
                    case ActionsEnum.RETURNSAVE:
                        ResetAfterJournalize();
                        break;
                    #endregion
                    #region JOURNALIZESUBMIT
                    case ActionsEnum.JOURNALIZESUBMIT:
                        ResetAfterJournalize();
                        break;
                    #endregion
                    #region JOURNALIZEDELETE
                    case ActionsEnum.JOURNALIZEDELETE:
                        ResetAfterJournalize();
                        break;
                    #endregion
                    #region JOURNALIZECANCEL,REVERSECANCEL,RETURNCANCEL
                    case ActionsEnum.REVERSECANCEL:
                    case ActionsEnum.JOURNALIZECANCEL:
                    case ActionsEnum.RETURNCANCEL:
                        ResetAfterJournalize();
                        break;
                    #endregion
                    #region REVERSE
                    case ActionsEnum.REVERSE:
                        GetFieldValues(ControlsEnum.INVPAYMENTHEADER);
                        if (InvPaymentHeaderSession != null)
                        {
                            if (InvPaymentHeaderSession.ModeDetail.Where(r => r.PDM_PDC >= 1).Count() > 0) //commented for multiple cheque finPaymentVndHdrList[0].PVH_PDC >= 1
                            {
                                if (InvPaymentHeaderSession.PVH_HAS_JRNL_ENTRY)
                                {
                                    if (InvPaymentHeaderSession.ModeDetail.Where(r => r.PDM_PDC == 1).Count() > 0) //commented for multiple cheque finPaymentVndHdrList[0].PVH_PDC == 1
                                    {
                                        FinTrxService finTrxServiceClient;
                                        finTrxServiceClient = new FinTrxService();
                                        finTrxServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(finTrxServiceClient);
                                        result = finTrxServiceClient.GenerateDummyEntry((int)CurrPK, ApplicationType.PPCCTJ);
                                        if (result > 0 || result == -2)  // -2 already exist
                                        {
                                            //poPaymentServiceClient = new POPaymentService();
                                            //poPaymentServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(poPaymentServiceClient);
                                            //result = poPaymentServiceClient.UpdatePaymentHdrPDCFlag((int)CurrPK, 3);          //This was done in Job SP                                 
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
                    #region CHEQUE RETURN
                    case ActionsEnum.CHEQUERETURN:
                        GetFieldValues(ControlsEnum.INVPAYMENTHEADER);
                        if (InvPaymentHeaderSession != null)
                        {
                            if (InvPaymentHeaderSession.ModeDetail.Where(r => r.PDM_PDC != 1).Count() > 0) //commented for multiple cheque finPaymentVndHdrList[0].PVH_PDC != 1
                            {
                                if (InvPaymentHeaderSession.PVH_HAS_JRNL_ENTRY)
                                {
                                    #region Check whether the cheque return is possible or not
                                    bool IsReturnSuccess = true;
                                    if (InvPaymentHeaderSession.ModeDetail.Where(r => r.PDM_MODE != (byte)PaymentModeEnum.CHEQUE).Count() > 0)
                                        IsReturnSuccess = false;
                                    else if (InvPaymentHeaderSession.ModeDetail.Where(r => r.PDM_PDC > 0).Count() > 0 && InvPaymentHeaderSession.ModeDetail.Where(r => r.PDM_PDC == 0).Count() > 0)
                                        IsReturnSuccess = false;
                                    if (!IsReturnSuccess)
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("MsgErr_ReturnEntry_Multi_Mode").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                        return;
                                    }
                                    #endregion
                                    if (InvPaymentHeaderSession.ModeDetail.Where(r => r.PDM_BOUNCED == 0).Count() > 0) //commented for multiple cheque finPaymentVndHdrList[0].PVH_BOUNCED == 0
                                    {
                                        FinTrxService finTrxServiceClient;
                                        finTrxServiceClient = new FinTrxService();
                                        finTrxServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(finTrxServiceClient);
                                        result = finTrxServiceClient.GenerateDummyEntry((int)CurrPK, ApplicationType.PCBTJ);
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
                    #region REVERSESUBMIT,RETURNSUBMIT
                    case ActionsEnum.REVERSESUBMIT:
                    case ActionsEnum.RETURNSUBMIT:
                        ResetAfterJournalize();
                        break;
                    #endregion
                    #region REVERSEDELETE,RETURNDELETE
                    case ActionsEnum.REVERSEDELETE:
                    case ActionsEnum.RETURNDELETE:
                        ResetAfterJournalize();
                        break;
                    #endregion
                    #endregion
                                           
                    #region Upload Documents (ADDITEM,REMOVEITEM,EDITITEMUPLOAD)
                    #region ADDITEM
                    case ActionsEnum.ADDITEM:
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
                                            poUploadObj = PaymentUploadList.SingleOrDefault(itm => itm.DOC_SEQ_NO == CurrSlNo);
                                            if (poUploadObj != null)
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
                                                    poUploadObj.AttachmentFileName = attachmentFileName;
                                                    poUploadObj.FileExtension = tempFileInfoObj.Extension;
                                                    poUploadObj.DOC_NAME = fupUpload.FileName;
                                                    poUploadObj.DOC_TYPE = tempFileInfoObj.Extension;
                                                    if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower()))
                                                    {
                                                        poUploadObj.DOC_PATH = "~/Upload/" + attachmentFileName;
                                                    }
                                                    else
                                                    {
                                                        poUploadObj.DOC_PATH = System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower() + attachmentFileName;
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
                                            if (PaymentUploadList == null || PaymentUploadList.Count == 0)
                                            {
                                                PaymentUploadList = new List<BusinessObject.POInvoicing.PaymentUploads>();
                                                slno = 1;
                                            }
                                            else
                                            {
                                                slno = PaymentUploadList.Max(itm => itm.DOC_SEQ_NO);
                                                slno++;
                                            }
                                            if (FileDetailsList == null)
                                            {
                                                FileDetailsList = new List<FileDetails>();
                                            }

                                            poUploadObj = new PaymentUploads();
                                            poUploadObj.DOC_PK = 0;
                                            poUploadObj.DOC_SEQ_NO = slno;
                                            tempFileInfoObj = new FileInfo(fupUpload.PostedFile.FileName);
                                            string attachmentFileFormat = tempFileInfoObj.Extension;
                                            string attachmentFileName = Guid.NewGuid().ToString() + attachmentFileFormat;

                                            poUploadObj.AttachmentFileName = attachmentFileName;
                                            poUploadObj.FileExtension = tempFileInfoObj.Extension;
                                            poUploadObj.DOC_NAME = fupUpload.FileName;
                                            poUploadObj.DOC_TYPE = tempFileInfoObj.Extension;
                                            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower()))
                                            {
                                                poUploadObj.DOC_PATH = "~/Upload/" + attachmentFileName;
                                            }
                                            else
                                            {
                                                poUploadObj.DOC_PATH = System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower() + attachmentFileName;
                                            }
                                            poUploadObj.DOC_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                                            FileDetailsList.Add(new FileDetails() { SlNo = slno, PoFile = HttpContext.Current.Request.Files[0] });
                                            PaymentUploadList.Add(poUploadObj);

                                        }
                                    }
                                }
                            }
                            SetFieldValues(ControlsEnum.UPLOADEDFILES);
                            ResetForm(ControlsEnum.ADDITEM);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ScrollDown();", true);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowUploadDocDetails", "$(document).ready(function(){ShowHideUploadDocDetails(1);});", true);
                        }
                        break;
                    #endregion
                    #region REMOVEITEM
                    case ActionsEnum.REMOVEITEM:
                        if (PaymentUploadList != null && PaymentUploadList.Count > 0)
                        {
                            selectedItemPK = Convert.ToInt32(grdUploads.DataKeys[(((Button)sender).Parent.Parent as GridViewRow).RowIndex][0]);
                            if (selectedItemPK > 0)
                            {
                                PaymentUploadList = PaymentUploadList.Where(row => selectedItemPK != row.DOC_SEQ_NO).ToList();
                                BindGrid(ControlsEnum.UPLOADEDFILES);
                                ResetForm(ControlsEnum.ADDITEM);
                            }
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowUploadDocDetails", "$(document).ready(function(){ShowHideUploadDocDetails(1);});", true);
                        break;
                    #endregion
                    #region EDITITEMUPLOAD
                    case ActionsEnum.EDITITEMUPLOAD:
                        if (PaymentUploadList != null && PaymentUploadList.Count > 0)
                        {
                            selectedItemPK = Convert.ToInt32(grdUploads.DataKeys[(((Button)sender).Parent.Parent as GridViewRow).RowIndex][0]);
                            if (selectedItemPK > 0)
                            {
                                poUploadObj = PaymentUploadList.SingleOrDefault(row => selectedItemPK == row.DOC_SEQ_NO);
                                GetUIValuesFromObject(ControlsEnum.SELECTEDDOC);
                            }
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowUploadDocDetails", "$(document).ready(function(){ShowHideUploadDocDetails(1);});", true);
                        break;
                    #endregion
                    #endregion

                    #region DTL SEARCH/VENDOR CHANGE
                    case ActionsEnum.DTLSEARCH:
                    case ActionsEnum.VENDORSELECTED:
                        uclPendingInvPaging.CurrentPage = 1;
                        PageIndexInv = CommonConstants.SELECT_VALUE_ONE;
                        dtPendingInvList = null;
                        GetFieldValues(ControlsEnum.VENDORDETAILSBYPK);
                        SetFieldValues(ControlsEnum.VENDORDETAILSBYPK);//Set Type(Import/Local) of the Vendor by default in Pending invoice Search area
                        GetFieldValues(ControlsEnum.PENDINGINVLIST);
                        SetFieldValues(ControlsEnum.PENDINGINVLIST);
                        hdfIsPendingInvVisible.Value = "1";
                        break;
                    #endregion
                    #region DTL CLEAR SEARCH
                    case ActionsEnum.DTLCLEARSEARCH:
                        txtPurchaseInvNumber.Text = string.Empty;
                        hdfPurchaseInvPK.Value = string.Empty;
                        txtPendingFromDate.Text = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString(Resources.Constants.DateFormatShort);
                        hdfPendingFromDate.Value = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString();
                        txtPendingToDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                        hdfPendingToDate.Value = DateTime.Now.ToString();
                        ddlPendingInvCategory.ClearSelection();
                        ddlPendingInvType.ClearSelection();
                        uclPendingInvPaging.CurrentPage = 1;
                        PageIndexInv = CommonConstants.SELECT_VALUE_ONE;
                        dtPendingInvList = null;
                        GetFieldValues(ControlsEnum.PENDINGINVLIST);
                        SetFieldValues(ControlsEnum.PENDINGINVLIST);
                        hdfIsPendingInvVisible.Value = "1";
                        break;
                    #endregion                   
                    #region AMOUNTDETAILS
                    case ActionsEnum.AMOUNTDETAILS:
                        HiddenField hdfInvoiceID = (HiddenField)((GridViewRow)((LinkButton)(sender)).Parent.Parent).FindControl("hdfInvoiceID");
                        purchaseInvoicePK = 0;
                        Int32.TryParse(hdfInvoiceID.Value, out purchaseInvoicePK);
                        GetFieldValues(ControlsEnum.AMOUNTDETAILS);
                        SetFieldValues(ControlsEnum.AMOUNTDETAILS);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateTotalAmountSplit", "$(document).ready(function(){CalculateTotalAmountSplit();});", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPaidAmntSplitup]','" + GetLocalResourceObject("TrxDetails").ToString() + "','600','250');", true);
                        break;
                    #endregion
                    #region Cancel,PAYMENTLIST
                    case ActionsEnum.CANCEL:
                    case ActionsEnum.PAYMENTLIST:
                        FillProcessID(1);
                        ResetForm();
                        GetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                        SetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                        this.btnNew.Focus();
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion
                    #region SEARCH,CLEAR (Listing)
                    #region SEARCH
                    case ActionsEnum.SEARCH:
                        PageIndex = "1";
                        GetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                        SetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        uclPaging.TotalPages = TotalPages;
                        uclPaging.CurrentPage = 1;
                        break;
                    #endregion
                    #region CLEAR
                    case ActionsEnum.CLEAR:
                        ResetForm();
                        PageIndex = "1";
                        GetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                        SetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                        uclPaging.TotalPages = TotalPages;
                        uclPaging.CurrentPage = 1;
                        break;
                    #endregion
                    #endregion
                    #region PRINT, PRINTDT
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
                                        List<PaymentModeDetails> PaymentModeDetailsPrintList = PaymentModeDetailsList.Where(r => r.PDM_MODE == (byte)PaymentModeEnum.CHEQUE).ToList();
                                        string printURL = string.Empty;
                                        foreach (PaymentModeDetails objChequeList in PaymentModeDetailsPrintList)
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
                    #region PRINTINVOICE (Listing Invoice no Popup)
                    case ActionsEnum.PRINTINVOICE:
                        hdfInvType = (HiddenField)((((LinkButton)sender).Parent).FindControl("hdfInvType"));
                        hdfinvPK = (HiddenField)((((LinkButton)sender).Parent).FindControl("hdfinvPK"));
                        hdfinvCategory = (HiddenField)((((LinkButton)sender).Parent).FindControl("hdfinvCategory"));
                        hdfinvCategoryType = (HiddenField)((((LinkButton)sender).Parent).FindControl("hdfinvCategoryType"));

                        if (Convert.ToUInt32(hdfinvCategory.Value) == Convert.ToUInt32(POInvoiceCategory.Invoice))
                        {
                            if ((Convert.ToUInt32(hdfInvType.Value) == Convert.ToUInt32(POInvoiceGroup.Goods)) || (Convert.ToUInt32(hdfInvType.Value) == Convert.ToUInt32(POInvoiceGroup.Services)))  //|| (hdfInvType.Value == "2")
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + hdfinvPK.Value + "&APPTYPE=" + ApplicationType.TPI + "&APPSUBTYPE=") + "');", true);
                            if (Convert.ToUInt32(hdfInvType.Value) == Convert.ToUInt32(POInvoiceGroup.Expense))
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + hdfinvPK.Value + "&APPTYPE=" + ApplicationType.EIT + "&APPSUBTYPE=") + "');", true);
                        }
                        else if (Convert.ToUInt32(hdfinvCategory.Value) == Convert.ToUInt32(POInvoiceCategory.Advanced))
                        {
                            if (hdfinvCategoryType.Value == "2")
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + hdfinvPK.Value + "&APPTYPE=" + ApplicationType.TPI + "&APPSUBTYPE=13") + "');", true);
                            else
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + hdfinvPK.Value + "&APPTYPE=" + ApplicationType.TPI + "&APPSUBTYPE=12") + "');", true);
                        }
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
                            if (hdfInvGroup.Value == "3")
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + lnkInvoicePk.ToString() + "&APPTYPE=" + ApplicationType.EIT + "&APPSUBTYPE=") + "');", true);
                            }
                            else if (hdfInvCategory.Value == "2")
                            {
                                if (hdfInvoiceType.Value.ToString() == "2")
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + lnkInvoicePk.ToString() + "&APPTYPE=" + ApplicationType.TPI + "&APPSUBTYPE=13") + "');", true);
                                else
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + lnkInvoicePk.ToString() + "&APPTYPE=" + ApplicationType.TPI + "&APPSUBTYPE=12") + "');", true);
                            }
                            else if (hdfInvCategory.Value == "1")
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + lnkInvoicePk.ToString() + "&APPTYPE=" + ApplicationType.TPI + "&APPSUBTYPE=") + "');", true);
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
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "ClosePopup();OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + poPK + "&APPTYPE=" + ApplicationType.POT + "&APPSUBTYPE=" + RptSubType) + "');", true);
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
                    whtHdrAmnt = paymentHeaderObj.PVH_WHT_AMOUNT;
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
                    txtBankCharge.Enabled = true;
                    ddlBankChargeCurrency.Enabled = true;
                    chkBankCharge.Enabled = true;
                    trBankCharge.Visible = true;                                      
                    break;
            }
        }
        /// <summary>
        /// Function to add payment mode details to view state
        /// </summary>
        private bool AddPaymentModes(bool ShowMsg)
        {           
            List<PaymentModeDetails> tempPaymentModeDtlList;
            if (PaymentModeDetailsList == null)
                PaymentModeDetailsList = new List<PaymentModeDetails>();

            tempPaymentModeDtlList = null;
            tempPaymentModeDtlList = PaymentModeDetailsList;

            if (PaymentModeRowIndex >= 0 && tempPaymentModeDtlList.Count > 0)
            {
                #region MyRegion
                tempPaymentModeDtlList[PaymentModeRowIndex].PDM_MODE = Convert.ToByte(ddlMode.SelectedValue);
                if (!string.IsNullOrEmpty(hdfPaymentBank.Value))
                    tempPaymentModeDtlList[PaymentModeRowIndex].PDM_BANK = hdfPaymentBank.Value;
                if (Convert.ToInt32(ddlMode.SelectedValue) != (int)PaymentModeEnum.CASH && Convert.ToInt32(ddlMode.SelectedValue) != (int)PaymentModeEnum.OTHERS)
                {
                    tempPaymentModeDtlList[PaymentModeRowIndex].PDM_BANK_CHARGE = txtBankCharge.Text != string.Empty ? Convert.ToDecimal(txtBankCharge.Text) : 0;
                    tempPaymentModeDtlList[PaymentModeRowIndex].PDM_BANK_CHARGE_TYPE = chkBankCharge.Checked;
                    if (ddlBankChargeCurrency.Items.Count > 0)
                        tempPaymentModeDtlList[PaymentModeRowIndex].PDM_BANK_CHARGE_CURR = ddlBankChargeCurrency.SelectedValue;
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
                if (!String.IsNullOrEmpty(txtInstrumentDate.Text.Trim()))
                    tempPaymentModeDtlList[PaymentModeRowIndex].PDM_INSTR_DATE = txtInstrumentDate.Text.Trim();
                tempPaymentModeDtlList[PaymentModeRowIndex].PDM_INSTR_FAVOUR = HttpUtility.HtmlEncode(txtFavourof.Text.Trim());

                tempPaymentModeDtlList[PaymentModeRowIndex].PDM_EXCHG_RATE = Convert.ToDouble(txtHdrExchangeRate.Text.Trim());
                tempPaymentModeDtlList[PaymentModeRowIndex].PDM_PAID_AMOUNT = Convert.ToDecimal(txtPaymentAmount.Text.Replace(",", "").Trim());
                tempPaymentModeDtlList[PaymentModeRowIndex].PDM_PAID_AMOUNT_BC = Convert.ToDecimal(txtTotalAmountBC.Text.Replace(",", "").Trim());
                tempPaymentModeDtlList[PaymentModeRowIndex].PDM_ACCOUNT = string.IsNullOrEmpty(hdfBankAccount.Value) ? 1 : Convert.ToInt32(hdfBankAccount.Value);
                PaymentModeDetailsList = tempPaymentModeDtlList;
                IsPaymentModeAdded = true;
                SetPymntModeHdrValidation(false);
                return true; 
                #endregion
            }
            else
            {
                #region else Region
                short? BankId = string.IsNullOrEmpty(hdfPaymentBank.Value) ? (short?)null : Convert.ToInt16(hdfPaymentBank.Value);
                List<PaymentModeDetails> PymtList = tempPaymentModeDtlList.Where(r => r.PDM_MODE == Convert.ToByte(ddlMode.SelectedValue) && r.PDM_BANK == BankId.ToString() && r.PDM_INSTR_NO == HttpUtility.HtmlEncode(txtInstrumentNo.Text.Trim())).ToList();
                if (PymtList == null || PymtList.Count == 0)
                {
                    PaymentModeDetails PaymentModeDtl = new PaymentModeDetails();
                    PaymentModeDtl.PDM_MODE = Convert.ToByte(ddlMode.SelectedValue);
                    if (!string.IsNullOrEmpty(hdfPaymentBank.Value))
                        PaymentModeDtl.PDM_BANK = hdfPaymentBank.Value;
                    if (Convert.ToInt32(ddlMode.SelectedValue) != (int)PaymentModeEnum.CASH && Convert.ToInt32(ddlMode.SelectedValue) != (int)PaymentModeEnum.OTHERS)
                    {
                        PaymentModeDtl.PDM_BANK_CHARGE = txtBankCharge.Text != string.Empty ? Convert.ToDecimal(txtBankCharge.Text) : 0;
                        PaymentModeDtl.PDM_BANK_CHARGE_TYPE = chkBankCharge.Checked;
                        if (ddlBankChargeCurrency.Items.Count > 0)
                            PaymentModeDtl.PDM_BANK_CHARGE_CURR = ddlBankChargeCurrency.SelectedValue;                       
                    }
                    else
                    {
                        PaymentModeDtl.PDM_BANK_CHARGE = 0;
                        PaymentModeDtl.PDM_BANK_CHARGE_TYPE = false;
                       //PaymentModeDtl.PDM_BANK_CHARGE_CURR = 0;
                    }
                    if (Convert.ToInt32(ddlMode.SelectedValue) == (int)PaymentModeEnum.CHEQUE)
                        PaymentModeDtl.PDM_PDC = chkPDC.Checked == true ? (byte)1 : (byte)0;
                    else
                        PaymentModeDtl.PDM_PDC = 0;

                    PaymentModeDtl.PDM_BRANCH = HttpUtility.HtmlEncode(txtBranch.Text.Trim());
                    PaymentModeDtl.PDM_INSTR_NO = HttpUtility.HtmlEncode(txtInstrumentNo.Text.Trim());
                    if (!string.IsNullOrEmpty(txtInstrumentDate.Text.Trim()))
                        PaymentModeDtl.PDM_INSTR_DATE = txtInstrumentDate.Text.Trim();
                    PaymentModeDtl.PDM_INSTR_FAVOUR = HttpUtility.HtmlEncode(txtFavourof.Text.Trim());

                    PaymentModeDtl.PDM_EXCHG_RATE = Convert.ToDouble(txtHdrExchangeRate.Text.Trim());
                    PaymentModeDtl.PDM_PAID_AMOUNT = Convert.ToDecimal(txtPaymentAmount.Text.Replace(",", "").Trim());
                    PaymentModeDtl.PDM_PAID_AMOUNT_BC = Convert.ToDecimal(txtTotalAmountBC.Text.Replace(",", "").Trim());
                    PaymentModeDtl.PDM_ACCOUNT = string.IsNullOrEmpty(hdfBankAccount.Value) ? 1 : Convert.ToInt32(hdfBankAccount.Value);
                    tempPaymentModeDtlList.Add(PaymentModeDtl);
                    PaymentModeDetailsList = tempPaymentModeDtlList;

                    IsPaymentModeAdded = true;
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
                #endregion
            }           
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
                vamBankChargeHdr.Enabled = IsValid;
                csvBankChargeHdr.Enabled = IsValid;
                vrfTotalAmountBCHdr.Enabled = IsValid;
                vrfFavourofHdr.Enabled = IsValid;
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
                    if (PaymentModeDetailsList[RowIndex].PDM_BANK!=null)
                        hdfPaymentBank.Value = PaymentModeDetailsList[RowIndex].PDM_BANK;
                    else
                        hdfPaymentBank.Value = string.Empty;

                    if (PaymentModeDetailsList[RowIndex].PDM_MODE != (int)PaymentModeEnum.CASH)
                    {
                        txtBranch.Text = HttpUtility.HtmlDecode(PaymentModeDetailsList[RowIndex].PDM_BRANCH);
                        txtInstrumentNo.Text = HttpUtility.HtmlDecode(PaymentModeDetailsList[RowIndex].PDM_INSTR_NO);
                        if (PaymentModeDetailsList[RowIndex].PDM_INSTR_DATE != null)
                            txtInstrumentDate.Text = PaymentModeDetailsList[RowIndex].PDM_INSTR_DATE;
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

                    if (PaymentModeDetailsList[RowIndex].PDM_BANK_CHARGE_CURR!=null)
                        ddlBankChargeCurrency.SelectedValue = PaymentModeDetailsList[RowIndex].PDM_BANK_CHARGE_CURR.ToString();
                    GetFieldValues(ControlsEnum.BANKCURRENCYEXCHANGERATE);
                    txtBankCharge.Text = Math.Round(PaymentModeDetailsList[RowIndex].PDM_BANK_CHARGE, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                    chkBankCharge.Checked = PaymentModeDetailsList[RowIndex].PDM_BANK_CHARGE_TYPE;                  
                    txtPaymentAmount.Text = GetFormattedCurrency(PaymentModeDetailsList[RowIndex].PDM_PAID_AMOUNT);
                    txtTotalAmountBC.Text = GetFormattedCurrency(PaymentModeDetailsList[RowIndex].PDM_PAID_AMOUNT_BC);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void PaymentSplitSave(bool ShowMsg)
        {
         
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
                       
                        if (Convert.ToDecimal(TotalPayNowFooterSplit.Text) <= PayNowWithVariation)
                        {                            

                            divErrorLabel.Visible = false;
                            finPaymentVndPoMpgList = new List<PaymentPOMappingDetails>();                            
                            finPaymentVndPoMpgList = (List<PaymentPOMappingDetails>)SetUIValuesToObject(ControlsEnum.PAYMENTSPLITLIST);
                            if (finPaymentVndPoMpgList != null)
                            {
                                #region To keep applied invoice Pks
                                if (AppliedInvPkList == null)
                                    AppliedInvPkList = new List<long>();
                                if (!AppliedInvPkList.Contains(InvoicePK))
                                {
                                    AppliedInvPkList.Add(InvoicePK);
                                } 
                                #endregion
                                PaymentInvMappingDetails.SingleOrDefault(dtl => dtl.IVH_PK == InvoicePK).POMpg = finPaymentVndPoMpgList.ToList();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1", "ClosePopup();", true);
                            }
                        }
                        else
                        {
                            if (ShowMsg)
                            {
                                divErrorLabel.Visible = true;                               
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPoSplitUp]','" + GetLocalResourceObject("PaymentSplit").ToString() + "','850','300');", true);
                            }
                        }
                    }
                    else
                    {
                        if (ShowMsg)
                        {
                            divErrorLabel.Visible = true;                            
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPoSplitUp]','" + GetLocalResourceObject("PaymentSplit").ToString() + "','850','300');", true);
                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1","ClosePopup();", true);
                }

            }
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

        public bool GetBalanceLinkVisibility(string amt, string bal)
        {
            bool balVisible = false;
            if (!string.IsNullOrEmpty(amt) && !string.IsNullOrEmpty(bal))
            {
                double amountPayable = Convert.ToDouble(amt);
                double balanceToPay = Convert.ToDouble(bal);
                //if (amountPayable > balanceToPay)
                if (amountPayable != balanceToPay)
                {
                    balVisible = true;
                }
            }
            return balVisible;
        }
        public bool GetBalanceLableVisibility(string amt, string bal)
        {
            bool balVisible = true;
            if (!string.IsNullOrEmpty(amt) && !string.IsNullOrEmpty(bal))
            {
                double amountPayable = Convert.ToDouble(amt);
                double balanceToPay = Convert.ToDouble(bal);
                if (amountPayable != balanceToPay)
                {
                    balVisible = false;
                }
            }
            return balVisible;
        }

        private void InvoiceDetails(long InvPK)
        {
            InvoicePK = InvPK;
            List<PaymentPOMappingDetails> tempInvoicePOSplitList = new List<PaymentPOMappingDetails>();
            List<PaymentInvoiceTrxMpgDetails> tempInvoiceTrxMpgDetails;
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

            tempInvoiceTrxMpgDetails = PaymentInvMappingDetails;
            tempPaymentInvoiceTrxMpgDetails = tempInvoiceTrxMpgDetails.Where(dtl => dtl.IVH_PK == InvoicePK).ToList();

            if (hdfPaymentPK != null && !string.IsNullOrEmpty(hdfPaymentPK.Value) && !hdfPaymentPK.Value.Equals("0"))
            {
                PaymentMpgPK = Convert.ToInt64(hdfPaymentPK.Value);

                finPaymentVndPoMpgList = tempPaymentInvoiceTrxMpgDetails[0].POMpg;
               // GetFieldValues(ControlsEnum.PAYMENTSPLITLIST);
                //if (tempFinPaymentVndPoMpgList == null || tempFinPaymentVndPoMpgList.Count == 0)
                //{
                    //if (finPaymentVndPoMpgList != null && finPaymentVndPoMpgList.Count > 0)//Edit
                    //{
                    //    tempInvoicePOSplitList.Where(dtl => dtl.POH_IVH_PK == InvoicePK)
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
                    //    tempFinPaymentVndPoMpgList = tempInvoicePOSplitList.Where(dtl => dtl.FIN_PAYMENT_VND_TRX_MPG.PVM_INVOICE_HDR == InvoicePK).ToList();
                    //}
                    //else if (InvoicePK > 0)
                    //{
                    //    GetFieldValues(ControlsEnum.INVOICEVNDMPGLIST);
                    //}
                //}
                //else
                //{
                //    tempFinPaymentVndPoMpgList = tempInvoicePOSplitList;
                //}
                if (InvoicePK > 0)
                {                   
                    GetUIValuesFromObject(ControlsEnum.PAYMENTSPLITLIST);
                    isSplitChanged = false;
                    SetFieldValues(ControlsEnum.PAYMENTSPLITLIST);

                    GetUIValuesFromObject(ControlsEnum.SPLITPAYNOWFORMULIPO);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_CalculateTotal", "$(document).ready(function(){CalculateTotal();CalculateTotalSplit();});", true);
                }
            }
            else if (InvoicePK > 0)//New
            {              
                GetUIValuesFromObject(ControlsEnum.PAYMENTSPLITLIST);
                isSplitChanged = false;
                SetFieldValues(ControlsEnum.PAYMENTSPLITLIST);

                GetUIValuesFromObject(ControlsEnum.SPLITPAYNOWFORMULIPO);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_CalculateTotal", "$(document).ready(function(){CalculateTotal();CalculateTotalSplit();});", true);
            }
        }

        #region --- For Grid Actions----
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            decimal total;
            #region DataRow
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
            {
                #region grdPOPaymentHdr
                if (((GridView)sender).ID == "grdPOPaymentHdr")
                {
                    HiddenField hdfPDC = e.Row.FindControl("hdfPDC") as HiddenField;
                    HiddenField hdfMode = e.Row.FindControl("hdfMode") as HiddenField;
                    Button btnPDCFlag = e.Row.FindControl("btnPDCFlag") as Button;
                    Button btnPDCReturn = e.Row.FindControl("btnPDCReturn") as Button;

                    LinkButton lnkInvnos = e.Row.FindControl("lnkInvnos") as LinkButton;
                    HiddenField hdfInvType = e.Row.FindControl("hdfInvType") as HiddenField;
                    HiddenField hdfinvPK = e.Row.FindControl("hdfinvPK") as HiddenField;
                    HiddenField hdfinvCategory = e.Row.FindControl("hdfinvCategory") as HiddenField;
                    HiddenField hdfinvCategoryType = e.Row.FindControl("hdfinvCategoryType") as HiddenField;
                    if (dtPaymentList != null && dtPaymentList.Rows.Count > 0)
                    {
                        #region PDC Flag Settings
                        short PdcStatus = 0;
                        short BouncedStatus = 0;
                        PdcStatus = Convert.ToInt16(dtPaymentList.Rows[e.Row.RowIndex]["PDM_PDC_STATUS"]);
                        BouncedStatus = Convert.ToInt16(dtPaymentList.Rows[e.Row.RowIndex]["PDM_BOUNCED_STATUS"]);                              

                        hdfPDC.Value = PdcStatus.ToString();
                        btnPDCFlag.CssClass = (PdcStatus != 0 ? (((PdcStatus == 1) || (PdcStatus == 3)) ? "flaggrey-icon" : "flaggreen-icon") : "");
                        btnPDCFlag.ToolTip = (PdcStatus != 0 ? (((PdcStatus == 1) || (PdcStatus == 3)) ? GetLocalResourceObject("PDC_Cheque").ToString() : GetLocalResourceObject("Cheque_Reversed").ToString()) : "");
                        btnPDCFlag.Visible = (BouncedStatus != 0 ? false : (PdcStatus != 0 ? true : false));

                        btnPDCReturn.CssClass = (BouncedStatus != 0 ? "return-icon" : "");
                        btnPDCReturn.ToolTip = (BouncedStatus != 0 ? GetLocalResourceObject("PDC_Return").ToString() : "");
                        btnPDCReturn.Visible = (BouncedStatus != 0 ? true : false);
                        #endregion

                        #region Invoice Number (removelinkPopup)
                        string invoices = string.Empty;
                        int invCount = 0;
                        string invNumbers = dtPaymentList.Rows[e.Row.RowIndex]["PVH_INVOICE_NO"].ToString();                    
                        if (invNumbers != null && invNumbers.Count() > 0)
                        {
                            string[] invNumbersArray = invNumbers.Split(',');
                            foreach (string invNo in invNumbersArray)
                            {
                                invCount++;
                            }                            
                        }                   
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
                  
                }
                #endregion

                #region grdPendingInvList
                else if (((GridView)sender).ID == "grdPendingInvList")
                {
                    //selectedRowColor
                    if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                    {
                        HiddenField hdfInvoiceID = e.Row.FindControl("hdfInvoiceID") as HiddenField;
                        CheckBox chkInvPendSelect = e.Row.FindControl("chkInvPendSelect") as CheckBox;
                        if (InvPaymentHeaderSession != null && InvPaymentHeaderSession.TrxMpg.Where(r => r.IVH_PK == Convert.ToInt16(hdfInvoiceID.Value)).Count() > 0)
                        {
                            e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml(GetGlobalResourceObject("ErpRes", "selectedRowColor").ToString());
                            chkInvPendSelect.Checked = true;
                            chkInvPendSelect.Enabled = false;
                        }
                    }
                }
                #endregion

                #region grdInvoiceList
                if (((GridView)sender).ID == "grdInvoiceList")
                {
                    #region Declaration
                    Label lblInvoiceNo = e.Row.FindControl("lblInvoiceNo") as Label;
                    LinkButton lnkInvoiceNo = e.Row.FindControl("lnkInvoiceNo") as LinkButton;
                    HiddenField hdfGroup = e.Row.FindControl("hdfGroup") as HiddenField;
                    HiddenField hdfCategory = e.Row.FindControl("hdfCategory") as HiddenField;
                    Label lblOtherCharges = e.Row.FindControl("lblOtherCharges") as Label;
                    TextBox txtOtherCharges = e.Row.FindControl("txtOtherCharges") as TextBox;
                    HiddenField hdfOtherChargesPrev = e.Row.FindControl("hdfOtherChargesPrev") as HiddenField;
                    TextBox txtAdjustments = e.Row.FindControl("txtAdjustments") as TextBox;
                    HiddenField hdfPvmOtherAmount = e.Row.FindControl("hdfPvmOtherAmount") as HiddenField;
                    TextBox txtPayNow = e.Row.FindControl("txtPayNow") as TextBox;
                    Label lblCnAmount = e.Row.FindControl("lblCnAmount") as Label;
                    Label lblCrdrAlcnAmount = e.Row.FindControl("lblCrdrAlcnAmount") as Label; 
                    #endregion

                    hdfFavourof.Value = txtFavourof.Text = paymentInvoiceMappingDetailList[e.Row.RowIndex].IVH_VENDOR_TEXT;

                    lblInvoiceNo.Text = lnkInvoiceNo.Text = paymentInvoiceMappingDetailList[e.Row.RowIndex].IVH_NO;
                    lblInvoiceNo.ToolTip = lnkInvoiceNo.ToolTip = GetLocalResourceObject("DueDate").ToString() + " : " + paymentInvoiceMappingDetailList[e.Row.RowIndex].IVH_DATE;
                    lnkInvoiceNo.CommandArgument = paymentInvoiceMappingDetailList[e.Row.RowIndex].IVH_PK.ToString();

                    #region OtherCharges
                    decimal otherChargesPrev = 0;
                    long? IVH_PK = paymentInvoiceMappingDetailList[e.Row.RowIndex].IVH_PK;
                    if (CurrPK == 0)
                    {
                        otherChargesPrev = paymentInvoiceMappingDetailList[e.Row.RowIndex].PVM_OTHER_AMOUNT;
                        decimal otherCharges = paymentInvoiceMappingDetailList[e.Row.RowIndex].IVH_SHIP_CHARGE;
                        lblOtherCharges.Text = String.Format("{0:c}", otherCharges);
                        lblOtherCharges.ToolTip = String.Format("{0:c}", otherCharges);
                        otherCharges -= otherChargesPrev;
                        otherCharges = otherCharges < 0 ? 0 : otherCharges;

                        //For resolving excess payment of other charge.                 
                        decimal OtherChargeDeduct = paymentInvoiceMappingDetailList[e.Row.RowIndex].IVH_SHIP_CHARGE_DED;
                        txtOtherCharges.Text = Math.Round((otherCharges - (OtherChargeDeduct > 0 ? OtherChargeDeduct : 0)), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                        txtOtherCharges.ToolTip = String.Format("{0:c}", otherCharges);
                        hdfOtherChargesPrev.Value = (otherChargesPrev + paymentInvoiceMappingDetailList[e.Row.RowIndex].IVH_SHIP_CHARGE_DED).ToString();
                    }                   
                    else
                    {
                        decimal otherCharges = paymentInvoiceMappingDetailList[e.Row.RowIndex].IVH_SHIP_CHARGE;
                        decimal prevotherCharges = paymentInvoiceMappingDetailList[e.Row.RowIndex].PVM_OTHER_AMOUNT;
                        lblOtherCharges.Text = String.Format("{0:c}", otherCharges);
                        lblOtherCharges.ToolTip = String.Format("{0:c}", otherCharges);

                        decimal balOtherCharges = otherCharges - prevotherCharges;
                        balOtherCharges = balOtherCharges < 0 ? 0 : balOtherCharges;
                        txtOtherCharges.Text = Math.Round(prevotherCharges, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                        txtOtherCharges.ToolTip = String.Format("{0:c}", prevotherCharges);

                        hdfOtherChargesPrev.Value = (paymentInvoiceMappingDetailList[e.Row.RowIndex].PVM_OTHER_AMOUNT + paymentInvoiceMappingDetailList[e.Row.RowIndex].IVH_SHIP_CHARGE_DED - prevotherCharges).ToString();
                    } 

                    #endregion

                    txtAdjustments.Text = txtAdjustments.ToolTip = GetFormattedCurrency(0);


                    if (InvPaymentHeaderSession.PVH_VENDOR_WHT_TAX != null)
                    {
                        whtTaxpk = Convert.ToInt32(InvPaymentHeaderSession.PVH_VENDOR_WHT_TAX);
                        trVendorAccount.Style.Add("display", "");
                    }
                    else
                    {
                        trVendorAccount.Style.Add("display", "none");
                    }

                    decimal InvCNAmount = 0;
                    InvCNAmount = paymentInvoiceMappingDetailList[e.Row.RowIndex].PVM_CN_AMOUNT;
                    InvCNAmount = Math.Round(InvCNAmount, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                    if (InvCNAmount > 0 && !IsCreditExist)
                        IsCreditExist = true;

                    //fill WHT popup
                    txtCustomerTxtWHT.Text = ERP.Utilities.CommonFunctions.GetShortString(InvPaymentHeaderSession.PVH_VEN_NAME2, 300);
                    txtCustomerTxtWHT.ToolTip = InvPaymentHeaderSession.PVH_VEN_NAME2;
                    hdfvendorWHTPK.Value = InvPaymentHeaderSession.PVH_VENDOR.ToString();
                    txtpartyads.Text = InvPaymentHeaderSession.PVH_VEN_ADDR3;
                    txtTaxid.Text = InvPaymentHeaderSession.PVH_VEN_TIN;
                    // END fill WHT popup
                    SetVendorPayment();

                    #region Cedit note allocated amount
                    decimal CrdrAlcnAmnt = 0;
                    if (paymentInvoiceMappingDetailList[e.Row.RowIndex].CRDRMpg != null && paymentInvoiceMappingDetailList[e.Row.RowIndex].CRDRMpg.Count > 0)
                    {
                        CrdrAlcnAmnt = paymentInvoiceMappingDetailList[e.Row.RowIndex].CRDRMpg.Sum(r => r.PNM_PAID_AMOUNT + r.PNM_ADJ_AMOUNT);
                    }
                    if (lblCrdrAlcnAmount != null)
                        lblCrdrAlcnAmount.Text = lblCrdrAlcnAmount.ToolTip = String.Format("{0:c}", CrdrAlcnAmnt);      
                    #endregion             

                    #region To bind changed data back
                    if (paymentInvoiceMappingDetailList[e.Row.RowIndex].POMpg != null && paymentInvoiceMappingDetailList[e.Row.RowIndex].POMpg.Sum(dtl => dtl.PPO_PAID_AMOUNT) > 0)
                    {
                        txtPayNow.Text = txtPayNow.ToolTip = Math.Round(paymentInvoiceMappingDetailList[e.Row.RowIndex].PVM_PAID_AMOUNT, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                    }
                    #endregion

                    #region Column Visibility Settings
                    #region Visibility of adjn Coloumns
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
                    #endregion
                    #region Visibility of Allocation Column
                    if ((Convert.ToInt16(hdfGroup.Value) == Convert.ToInt16(POInvoiceGroup.AgtInvoice)) && !ShowAdjColumn)
                    {
                        grdInvoiceList.Columns[16].Visible = false;
                    }
                    else
                    {
                        grdInvoiceList.Columns[16].Visible = true;
                    }
                    #endregion
                    #region Visibility for credit amount columns
                    if ((Convert.ToInt16(hdfGroup.Value) == Convert.ToInt16(POInvoiceGroup.Goods) || Convert.ToInt16(hdfGroup.Value) == Convert.ToInt16(POInvoiceGroup.Services)
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
                    }
                    else
                    {
                        grdInvoiceList.Columns[10].Visible = false;
                        grdInvoiceList.Columns[17].Visible = false;
                        grdInvoiceList.Columns[18].Visible = false;
                    }
                    #endregion
                    #endregion
                }
                #endregion
                #region grdPaymentSplitAdjn
                else if (((GridView)sender).ID == "grdPaymentSplitAdjn")
                {                   
                    Label lblBalanceAdjn = e.Row.FindControl("lblBalanceAdjn") as Label;
                    TextBox txtAllocateAdjn = e.Row.FindControl("txtAllocateAdjn") as TextBox;
                    HiddenField hdfReceiptTRXAdjnPK = e.Row.FindControl("hdfReceiptTRXAdjnPK") as HiddenField;
               
                   

                    List<long?> lstInvNos = new List<long?>();
                    //PaymentAdjnList.ForEach(rr =>
                    //{
                    //    lstInvNos.Add(rr.FIN_PAYMENT_VND_TRX_MPG.PVM_INVOICE_HDR);
                    //});


                    if (PaymentAdjnList != null && PaymentAdjnList.Count > 0)//Edit before save
                    {
                        if (CrDrAdjnList[e.Row.RowIndex].PAA_CRDRPK > 0)
                        {
                            if (PaymentAdjnList.Where(fd => fd.PAD_ALCN_CDH == CrDrAdjnList[e.Row.RowIndex].PAA_CRDRPK && fd.PAD_PVM_INVOICE_HDR == InvoicePK).ToList() != null && PaymentAdjnList.Where(fd => fd.PAD_ALCN_CDH == CrDrAdjnList[e.Row.RowIndex].PAA_CRDRPK && fd.PAD_PVM_INVOICE_HDR == InvoicePK).ToList().Count > 0)
                            {
                                txtAllocateAdjn.Text = PaymentAdjnList.Where(fd => fd.PAD_ALCN_CDH == CrDrAdjnList[e.Row.RowIndex].PAA_CRDRPK && fd.PAD_PVM_INVOICE_HDR == InvoicePK).FirstOrDefault().PAD_AMOUNT.ToString(hdfCurrencyFormat.Value);
                            }
                            else
                            {
                                txtAllocateAdjn.Text = "0.00";
                            }
                        }
                        else
                        {

                            if (PaymentAdjnList.Where(fd => fd.PAD_ALCN_PAYMENT_TRX == CrDrAdjnList[e.Row.RowIndex].PAA_TRXPK && fd.PAD_PVM_INVOICE_HDR == InvoicePK).ToList() != null && PaymentAdjnList.Where(fd => fd.PAD_ALCN_PAYMENT_TRX == CrDrAdjnList[e.Row.RowIndex].PAA_TRXPK && fd.PAD_PVM_INVOICE_HDR == InvoicePK).ToList().Count > 0)
                            {
                                txtAllocateAdjn.Text = PaymentAdjnList.Where(fd => fd.PAD_ALCN_PAYMENT_TRX == CrDrAdjnList[e.Row.RowIndex].PAA_TRXPK && fd.PAD_PVM_INVOICE_HDR == InvoicePK).FirstOrDefault().PAD_AMOUNT.ToString(hdfCurrencyFormat.Value);
                            }
                            else
                            {
                                txtAllocateAdjn.Text = "0.00";
                            }
                        }
                    }
                    else
                    {                       
                        txtAllocateAdjn.Text = "0.00";
                    }

                        hdfReceiptTRXAdjnPK.Value = PaymentMpgPK.ToString();
                        totAllocateAdjn = totAllocateAdjn + Convert.ToDecimal(txtAllocateAdjn.Text);
                        totBalanceAdjn = totBalanceAdjn + Convert.ToDecimal(lblBalanceAdjn.Text.Replace(",", ""));
                    
                    if (Convert.ToDecimal(lblBalanceAdjn.Text.Replace(",", "")) <= 0)
                    {
                        e.Row.Visible = false;
                    }
                }
                #endregion
                #region grdPaymentSplit
                else if (((GridView)sender).ID == "grdPaymentSplit")
                {
                    Label lblOtherChargesSplit = e.Row.FindControl("lblOtherChargesSplit") as Label;
                    HiddenField hdfOtherChargesSplit = e.Row.FindControl("hdfOtherChargesSplit") as HiddenField;
                    Label lblBalanceSplit = e.Row.FindControl("lblBalanceSplit") as Label;

                    decimal OtherCharges = 0;
                    OtherCharges = string.IsNullOrEmpty(hdfTotalOtherCharges.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(hdfTotalOtherCharges.Value);
                    lblOtherChargesSplit.Text = lblOtherChargesSplit.ToolTip = String.Format("{0:c}", OtherCharges);
                    hdfOtherChargesSplit.Value = OtherCharges.ToString();

                    decimal balamnt = 0;
                    decimal.TryParse(lblBalanceSplit.Text.Replace(",", ""), out balamnt);
                    totalbalamtsplit += balamnt;
                }
                #endregion

                #region grdWHTTaxDetails
                else if (((GridView)sender).ID == "grdWHTTaxDetails")
                {
                    HiddenField hdfWHTFormNo = e.Row.FindControl("hdfWHTFormNo") as HiddenField;
                    HiddenField hdfWhtTaxAmount = e.Row.FindControl("hdfWhtTaxAmount") as HiddenField;
                    HiddenField hdfWhtBranchType = e.Row.FindControl("hdfWhtBranchType") as HiddenField;
                    Label lblWhtTye = e.Row.FindControl("lblWhtTye") as Label;
                    Label lblformnoGRD = e.Row.FindControl("lblformnoGRD") as Label;
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
                #endregion
                #region grdVATTaxDetails
                else if (((GridView)sender).ID == "grdVATTaxDetails")
                {
                    HiddenField hdfBranchType = e.Row.FindControl("hdfBranchType") as HiddenField;
                    HiddenField hdfIvnPk = e.Row.FindControl("hdfIvnPk") as HiddenField;

                    HiddenField hdfAmount = e.Row.FindControl("hdfAmount") as HiddenField;
                    HiddenField hdfTaxAmount = e.Row.FindControl("hdfTaxAmount") as HiddenField;

                    Label lblTye = e.Row.FindControl("lblTye") as Label;
                    Label lblPurInvNo = e.Row.FindControl("lblPurInvNo") as Label;

                    if (TempVATTaxDetails != null && TempVATTaxDetails.Count > 0)
                    {
                        AmountTotal += Convert.ToDecimal(hdfAmount.Value);
                        TaxTotal += Convert.ToDecimal(hdfTaxAmount.Value);

                        txtVendorPopup.Text = ERP.Utilities.CommonFunctions.GetShortString(TempVATTaxDetails[e.Row.RowIndex].WTH_PARTY_NAME, 300);
                        hdfVendorVatPK.Value = TempVATTaxDetails[e.Row.RowIndex].WTH_PK.ToString();
                        if (TempVATTaxDetails[e.Row.RowIndex].WTH_TAX_DATE!=null)
                        {
                            txtVatTaxInvDate.Text = Convert.ToDateTime(TempVATTaxDetails[e.Row.RowIndex].WTH_TAX_DATE).ToString(Resources.Constants.DateFormatShort);
                        }
                        chkOriginalinvoice.Checked = TempVATTaxDetails[e.Row.RowIndex].WTH_INV_RECEIVED == 1 ? true : false;
                        txtVatTaxId.Text = TempVATTaxDetails[e.Row.RowIndex].WTH_TAX_ID;
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
                    }
                }
                #endregion

                #region grdPaymentModes
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
                    CurrencyPk = 0;
                }
                #endregion
            } 
            #endregion

            #region Header
            else if (e.Row.RowType == DataControlRowType.Header)
            {
                #region grdPOPaymentHdr
                if (((GridView)sender).ID == "grdPOPaymentHdr")
                {
                    GetFieldValues(ControlsEnum.BASECURRENCY);
                    Label lblHdrAmountBaseCur = e.Row.FindControl("lblHdrAmountBaseCur") as Label;
                    lblHdrAmountBaseCur.Text = GetLocalResourceObject("Amount").ToString() + " (" + hdfBaseCurrency.Value.Split('-')[0].Trim() + ")";
                }
                #endregion
                #region grdPaymentModes
                else if (((GridView)sender).ID == "grdPaymentModes")
                {
                    GetFieldValues(ControlsEnum.BASECURRENCY);
                    Label lblHdrPymntTotalAmountBC = e.Row.FindControl("lblHdrPymntTotalAmountBC") as Label;
                    Label lblHdrPymntTotalAmount = e.Row.FindControl("lblHdrPymntTotalAmount") as Label;
                    lblHdrPymntTotalAmountBC.Text = string.Format(GetLocalResourceObject("TotalAmntBC").ToString(), hdfBaseCurrency.Value.Split('-')[0].Trim());
                    lblHdrPymntTotalAmount.Text = string.Format(GetLocalResourceObject("TotalAmnt").ToString(), txtPaymentCurrency.Text.Trim());
                }
                #endregion
            } 
            #endregion
            #region Footer
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                #region grdInvoiceList
                if (((GridView)sender).ID == "grdInvoiceList")
                {
                   // Label lblTotalFooter = e.Row.FindControl("lblTotalPayNowFooter") as Label;
                    if (PaymentInvMappingDetails != null && PaymentInvMappingDetails.Count > 0)
                    {
                        total = 0;
                        total = PaymentInvMappingDetails.Sum(dtl => dtl.PVM_PAID_AMOUNT);
                        total = total < 0 ? 0 : total;
                        //lblTotalFooter.Text = string.Format("{0:c}", total);
                        //txtPaidAmount.Text = Math.Round(total, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                    }
                }
                #endregion
                #region grdPaymentSplit
                else if (((GridView)sender).ID == "grdPaymentSplit")
                {
                    HiddenField hdfPOPK = e.Row.FindControl("hdfPOPK") as HiddenField;
                    Label lblTotalPayNowFooterSplit = e.Row.FindControl("lblTotalPayNowFooterSplit") as Label;
                    HiddenField hdfTotalPayNowFooterSplit = e.Row.FindControl("hdfTotalPayNowFooterSplit") as HiddenField;
                    Label lblTotalBalFooterSplit = e.Row.FindControl("lblTotalBalFooterSplit") as Label;
                    HiddenField hdfTotalBalFooterSplit = e.Row.FindControl("hdfTotalBalFooterSplit") as HiddenField;

                    lblTotalBalFooterSplit.Text = string.Format("{0:c}", totalbalamtsplit);
                    hdfTotalBalFooterSplit.Value = Math.Round(totalbalamtsplit, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();

                    if (PaymentInvMappingDetails != null && PaymentInvMappingDetails.Count > 0)
                    {
                        if (PaymentInvMappingDetails.SingleOrDefault(dtl => dtl.IVH_PK == InvoicePK).POMpg != null)
                        {
                            lblTotalPayNowFooterSplit.Text = string.Format("{0:c}", PaymentInvMappingDetails.SingleOrDefault(dtl => dtl.IVH_PK == InvoicePK).POMpg.Sum(mpg => mpg.PPO_PAID_AMOUNT));
                            hdfTotalPayNowFooterSplit.Value = PaymentInvMappingDetails.SingleOrDefault(dtl => dtl.IVH_PK == InvoicePK).POMpg.Sum(mpg => mpg.PPO_PAID_AMOUNT).ToString();
                        }
                    }


                }
                #endregion
                #region grdPaymentSplitAdjn
                if (((GridView)sender).ID == "grdPaymentSplitAdjn")
                {

                    Label lblTotalAllocateAdjn = e.Row.FindControl("lblTotalAllocateAdjn") as Label;
                    HiddenField hdfTotalAllocateAdjn = e.Row.FindControl("hdfTotalAllocateAdjn") as HiddenField;
                    HiddenField hdfBalanceAdjn = e.Row.FindControl("hdfBalanceAdjn") as HiddenField;

                    lblTotalAllocateAdjn.Text = String.Format("{0:c}", totAllocateAdjn);
                    hdfTotalAllocateAdjn.Value = totAllocateAdjn.ToString();
                    hdfBalanceAdjn.Value = totBalanceAdjn.ToString();

                } 
                #endregion
                #region grdVATTaxDetails
                else if (((GridView)sender).ID == "grdVATTaxDetails")
                {
                    Label lblAmountTotal = (Label)e.Row.FindControl("lblAmountTotal");
                    Label lblTaxTotal = (Label)e.Row.FindControl("lblTaxTotal");
                    lblAmountTotal.Text = Math.Round(AmountTotal, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                    lblTaxTotal.Text = Math.Round(TaxTotal, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                }
                #endregion
                #region grdWHTTaxDetails
                else if (((GridView)sender).ID == "grdWHTTaxDetails")
                {
                    Label lblWhtTaxTotal = (Label)e.Row.FindControl("lblWhtTaxTotal");
                    lblWhtTaxTotal.Text = Math.Round(TaxWhtTotal, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                }
                #endregion
            } 
            #endregion
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
                    if (finVatPaymentDetails.WTH_FORM_NO!=null)
                    {
                        ddlFormno.SelectedValue = finVatPaymentDetails.WTH_FORM_NO.ToString();
                    }
                    else
                    {
                        ddlFormno.SelectedValue = CommonConstants.SELECTVAL;
                    }
                    //ddlFormno.SelectedValue = finVatPaymentDetails.WTH_FORM_NO.ToString();
                    txtCustomerTxtWHT.Text = HttpUtility.HtmlDecode(finVatPaymentDetails.WTH_PARTY_NAME);
                    txtpartyads.Text = HttpUtility.HtmlDecode(finVatPaymentDetails.WTH_ADDRESS);
                    txtTaxid.Text = HttpUtility.HtmlDecode(finVatPaymentDetails.WTH_TAX_ID);
                    txtPopupWHTAmount.Text = GetFormattedCurrency(finVatPaymentDetails.WTH_AMOUNT);
                    txtWHTTaxAmountPopup.Text = GetFormattedCurrency(finVatPaymentDetails.WTH_TAX_AMT);
                    txtWHTAccountPopup.Text = HttpUtility.HtmlDecode(finVatPaymentDetails.WTH_NAME);
                    txtDescriptionPopup.Text = HttpUtility.HtmlDecode(finVatPaymentDetails.WTH_DESC);
                    txtWthAddressType.Text = HttpUtility.HtmlDecode(finVatPaymentDetails.WTH_BRANCH_NAME);
                    txtWthBranchCode.Text = HttpUtility.HtmlDecode(finVatPaymentDetails.WTH_BRANCH_TEXT);
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
            uclPendingInvPaging.CurrentPage = 1; 
            btnSaveSubmit.PreRender += new EventHandler(btnAction_PreRender);
            btnSubmit.PreRender += new EventHandler(btnAction_PreRender);
            btnSavePmnt.PreRender += new EventHandler(btnAction_PreRender);
            btnDeletePmnt.PreRender += new EventHandler(btnAction_PreRender);
            btnPrint.PreRender += new EventHandler(btnAction_PreRender);
            btnCancel.PreRender += new EventHandler(btnAction_PreRender);
            btnJournalize.PreRender += new EventHandler(btnAction_PreRender);
           
            btnEdit.PreRender += new EventHandler(btnAction_PreRender);
            btnView.PreRender += new EventHandler(btnAction_PreRender);          

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
            btnNew.PreRender += new EventHandler(btnAction_PreRender);
        
            btnSaveSubmit.Load += new EventHandler(btnAction_Load);
            btnSubmit.Load += new EventHandler(btnAction_Load);
            btnSavePmnt.Load += new EventHandler(btnAction_Load);
            btnDeletePmnt.Load += new EventHandler(btnAction_Load);
            btnPrint.Load += new EventHandler(btnAction_Load);
            btnCancel.Load += new EventHandler(btnAction_Load);
            btnJournalize.Load += new EventHandler(btnAction_Load);

            btnEdit.Load += new EventHandler(btnAction_Load);
            btnView.Load += new EventHandler(btnAction_Load);        

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
            btnNew.Load += new EventHandler(btnAction_Load);
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

            this.uclPendingInvPaging.FirstPage += new ActionHandler(this.ActionHandler);
            this.uclPendingInvPaging.PreviousPage += new ActionHandler(this.ActionHandler);
            this.uclPendingInvPaging.NextPage += new ActionHandler(this.ActionHandler);
            this.uclPendingInvPaging.LastPage += new ActionHandler(this.ActionHandler);
            this.uclPendingInvPaging.PageChanged += new ActionHandler(this.ActionHandler);

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
                        // increment the current page index.
                        if (e.CurrentPage <= e.TotalPages)
                            pagerControl.CurrentPage++;
                        break;
                    case NavigationEnum.PREVIOUS:
                        // Decrement the current page index.
                        if (e.CurrentPage > 1)
                            pagerControl.CurrentPage--;
                        break;


                }
                if (senderId == "uclPaging")
                {
                    PageIndex = uclPaging.CurrentPage.ToString();
                    GetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                    SetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                    EntryStatus = EntryStatus.LISTMODE;
                    EnableDisableButtons(e.TotalPages, "uclPaging");
                }
                else if (senderId == "uclPendingInvPaging")
                {
                    PageIndexInv = uclPendingInvPaging.CurrentPage.ToString();
                    GetFieldValues(ControlsEnum.PENDINGINVLIST);
                    SetFieldValues(ControlsEnum.PENDINGINVLIST);
                    EnableDisableButtons(e.TotalPages, "uclPendingInvPaging");
                }  
              
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
            else if (pagerId == "uclPendingInvPaging")
            {
                // Should we disable the first link
                uclPendingInvPaging.FirstButtonEnabled = (uclPendingInvPaging.CurrentPage == 1) ? false : true;
                // Should we disable the previous link
                uclPendingInvPaging.PreviousButtonEnabled = (uclPendingInvPaging.CurrentPage == 1) ? false : true;
                // Should we enable the next link
                uclPendingInvPaging.NextButtonEnabled = (uclPendingInvPaging.CurrentPage < iTotalPages) ? true : false;
                // Should we enable the last link
                uclPendingInvPaging.LastButtonEnabled = (uclPendingInvPaging.CurrentPage < iTotalPages) ? true : false;
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
                if (grdInvoiceList.Rows != null)
                {
                    foreach (GridViewRow gvr in grdInvoiceList.Rows)
                    {
                        HiddenField hdfInvoicePK = gvr.FindControl("hdfInvoicePK") as HiddenField;
                        if (hdfInvoicePK != null)
                        {
                            (gvr.FindControl("hdfHasSplit") as HiddenField).Value = InvoicePOSplitList.Any(dtl => dtl.POH_IVH_PK== Convert.ToInt64(hdfInvoicePK.Value))
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

                if (grdInvoiceList.Rows.Count > 0)
                {
                    txtVendorHd.Enabled = false;
                }
                else
                {
                    txtVendorHd.Enabled = true;
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
            PENDINGINVLIST,
            INVPAYMENTHEADER,
            INVPAYMENTDETAIL,
            AMOUNTDETAILS,
            INVOICETYPE,
            INVOICECATEGORY,
            VENDORDETAILSBYPK
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

        #region ConfigurationSettings
        /// <ConfigurationSettings>
        /// 
        /// </summary>
        private void ConfigurationSettings()
        {
            hdfIsTaxForOtherCharge.Value = GetGlobalResourceObject("ConfigurationsRes", "OtherChargeTaxPurchase").ToString();
            hdfIsMultipleCheque.Value = GetGlobalResourceObject("ConfigurationsRes", "IsMultipleChequePrint").ToString();
            ShowAdjColumn = Convert.ToBoolean(Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "ShowAdjColumn")));
            ShowExpenseCrDr = Convert.ToBoolean(Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "CNDNFromExpenseInv")));
        } 
        #endregion

        #region CalculatePaymentTax
        public void CalculatePaymentTax(PaymentInvoiceTrxMpgDetails finPaymentInvTrxMappingObj, int poHdr, decimal splitTaxAMOUNT, Byte Category, int invoiceHdr, decimal ppoPaidAmount)
        {

            List<PaymentPOMappingDetails> finPaymentPOMappingDtlList = new List<PaymentPOMappingDetails>();
            List<PaymentTaxDetail> finPaymentVndTaxDtlList = new List<PaymentTaxDetail>();
            finPaymentPOMappingDtlList = finPaymentInvTrxMappingObj.POMpg;

            if (poHdr > 0)
            {
                finPaymentVndTaxDtlList = finPaymentInvTrxMappingObj.TaxDetails.Where(f => f.PDT_PO_HDR == poHdr && f.PDT_INVOICE_HDR == invoiceHdr).ToList();
            }
            else
            {
                finPaymentVndTaxDtlList = finPaymentInvTrxMappingObj.TaxDetails.Where(f => f.PDT_INVOICE_HDR == invoiceHdr).ToList();
            }

            if (finPaymentVndTaxDtlList.Count > 0)
            {

                foreach (PaymentTaxDetail FIN_PAYMENT_VND_TAX_DTL_obj1 in finPaymentVndTaxDtlList)
                {
                    decimal splitDiscAMOUNT = ppoPaidAmount;
                    #region Advance Invoice
                    if (Category != (Byte)(POInvoiceCategory.Invoice))
                    {

                        if (FIN_PAYMENT_VND_TAX_DTL_obj1.PDT_TAX_CATEGORY != (byte)TaxType.Discount)
                        {
                            FIN_PAYMENT_VND_TAX_DTL_obj1.PDT_AMOUNT = Convert.ToDecimal(ERP.Utilities.CommonFunctions.DoubleFormatRound(Convert.ToDouble(Convert.ToDecimal(Convert.ToDouble(splitTaxAMOUNT) * FIN_PAYMENT_VND_TAX_DTL_obj1.PDT_TAX_PERC)), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits));
                        }
                        else if (FIN_PAYMENT_VND_TAX_DTL_obj1.PDT_TAX_CATEGORY == (byte)TaxType.Discount)
                        {
                            FIN_PAYMENT_VND_TAX_DTL_obj1.PDT_AMOUNT = Convert.ToDecimal(ERP.Utilities.CommonFunctions.DoubleFormatRound(Convert.ToDouble(Convert.ToDecimal(Convert.ToDouble(splitDiscAMOUNT) * FIN_PAYMENT_VND_TAX_DTL_obj1.PDT_TAX_PERC)), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits));
                        }
                    }
                    #endregion
                    #region Invoice
                    else if (Category == (Byte)(POInvoiceCategory.Invoice))
                    {
                        if (FIN_PAYMENT_VND_TAX_DTL_obj1.PDT_TAX_CATEGORY != (byte)TaxType.Discount)
                        {
                            FIN_PAYMENT_VND_TAX_DTL_obj1.PDT_AMOUNT = Convert.ToDecimal(ERP.Utilities.CommonFunctions.DoubleFormatRound(Convert.ToDouble(Convert.ToDecimal(Convert.ToDouble(splitTaxAMOUNT) * FIN_PAYMENT_VND_TAX_DTL_obj1.PDT_TAX_PERC)), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits));
                        }
                        else if (FIN_PAYMENT_VND_TAX_DTL_obj1.PDT_TAX_CATEGORY == (byte)TaxType.Discount)
                        {
                            FIN_PAYMENT_VND_TAX_DTL_obj1.PDT_AMOUNT = Convert.ToDecimal(ERP.Utilities.CommonFunctions.DoubleFormatRound(Convert.ToDouble(Convert.ToDecimal(Convert.ToDouble(splitDiscAMOUNT) * FIN_PAYMENT_VND_TAX_DTL_obj1.PDT_TAX_PERC)), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits));
                        }
                    }
                    #endregion

                }
            }
        } 
        #endregion

        #region ResetAfterJournalize
        private void ResetAfterJournalize()
        {
            EntryStatus = EntryStatus.LISTMODE;
            hdfJournalizeWorkFlow.Value = "0";
            ucrWrkf.Reset();
            FillProcessID(1);
            ResetForm();
            GetFieldValues(ControlsEnum.PAYMENTHDRLIST);
            SetFieldValues(ControlsEnum.PAYMENTHDRLIST);
        } 
        #endregion       
    }
}