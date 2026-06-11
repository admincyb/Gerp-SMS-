using BusinessObject.POInvoicing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject
{
    [Serializable]
    [XmlRoot("Root")]
    public class InvoiceHdrBO : WorkflowBO
    {
        [XmlElement("USER_PK")]
        public int USER_PK { get; set; }

        [XmlElement("APT_CODE")]
        public string APT_CODE { get; set; }

        [XmlElement("AST_VALUE")]
        public int AST_VALUE { get; set; }

        [XmlElement("AST_DOC_MODE")]
        public string AST_DOC_MODE { get; set; }

        [XmlElement("WKF_FLAG")]
        public int WKF_FLAG { get; set; }

        [XmlElement("InvHdrList")]
        public List<InvoiceHdr> InvoiceHdrList { get; set; }
    }

    [Serializable]
    public class InvoiceHdr
    {
        [XmlElement("IVH_PK")]
        public int IVH_PK { get; set; }

        [XmlElement("IVH_VERSION")]
        public int Version { get; set; }

        [XmlElement("IVH_DATE")]
        public DateTime IVH_DATE { get; set; }

        [XmlElement("IVH_NO")]
        public string IVH_NO { get; set; }

        [XmlElement("AST_VALUE")]
        public int AST_VALUE { get; set; }

        [XmlElement("IVH_TYPE")]
        public int InvoiceType { get; set; }

        [XmlElement("IVH_CATEGORY")]
        public int InvoiceCategory { get; set; }

        [XmlElement("IVH_GROUP")]
        public int InvoiceGroup { get; set; }

        [XmlElement("IVH_VENDOR")]
        public int IVH_VENDOR { get; set; }

        [XmlElement("IVH_VENDOR_INV_NO")]
        public string VendorInvoiceNo { get; set; }

        [XmlElement("IVH_VENDOR_ACCOUNT")]
        public int? VendorAccount { get; set; }

        [XmlElement("IVH_REFERENCE")]
        public string Reference { get; set; }

        [XmlElement("IVH_DATE_RECEIVED")]
        public DateTime ReceivedDate { get; set; }

        [XmlElement("IVH_DATE_PAY_BY")]
        public DateTime IVH_DATE_PAY_BY { get; set; }

        [XmlElement("IVH_CURRENCY")]
        public int IVH_CURRENCY { get; set; }

        [XmlElement("IVH_AMOUNT_TC")]
        public decimal AmountTC { get; set; }

        [XmlElement("IVH_DISCOUNT_TC")]
        public decimal DiscountTC { get; set; }

        [XmlElement("IVH_TAX_TC")]
        public decimal IVH_TAX_TC { get; set; }

        [XmlElement("IVH_AMOUNT_NET_TC")]
        public decimal IVH_AMOUNT_NET_TC { get; set; }

        [XmlElement("IVH_AMOUNT_NET_TC_ADJ")]
        public decimal NetAmountTCAdjust { get; set; }

        [XmlElement("IVH_NET_VALUE_TC")]
        public decimal NetValueTC { get; set; }

        [XmlElement("IVH_AMOUNT_PAID_TC")]
        public decimal PaidAmountTC { get; set; }

        [XmlElement("IVH_BAL_AMNT_TC")]
        public decimal BalanceAmountTC { get; set; }

        [XmlElement("IVH_AMOUNT_DN_TC")]
        public decimal DebitAmountTC { get; set; }

        [XmlElement("IVH_AMOUNT_CN_TC")]
        public decimal CreditAmountTC { get; set; }

        [XmlElement("IVH_BASE_CURR")]
        public int BaseCurrencyPK { get; set; }

        [XmlElement("IVH_EXCHG_RATE")]
        public double ExchangeRate { get; set; }

        [XmlElement("IVH_AMOUNT_NET_BC")]
        public decimal NetAmountBC { get; set; }

        [XmlElement("IVH_REMARKS")]
        public string Remarks { get; set; }

        [XmlElement("IVH_CREDIT_DAYS")]
        public int CreditDays { get; set; }

        [XmlElement("IVH_TRANSPORT")]
        public string Transport { get; set; }

        [XmlElement("IVH_VENDOR_NAME")]
        public string VendorName { get; set; }

        [XmlElement("IVH_VENDOR_ADDRESS")]
        public string VendorAddress { get; set; }

        [XmlElement("IVH_VENDOR_COUNTRY")]
        public int? VendorCountryPK { get; set; }

        [XmlElement("IVH_VENDOR_ZIP")]
        public string VendorZIP { get; set; }

        [XmlElement("IVH_VENDOR_PHONE")]
        public string VendorPhone { get; set; }

        [XmlElement("IVH_VENDOR_MOBILE")]
        public string VendorMobile { get; set; }

        [XmlElement("IVH_VENDOR_FAX")]
        public string VendorFAX { get; set; }

        [XmlElement("IVH_VENDOR_EMAIL")]
        public string VendorEmail { get; set; }

        [XmlElement("IVH_VENDOR_CONTACT")]
        public int? VendorContact { get; set; }

        [XmlElement("IVH_BRANCH_TYPE")]
        public int BranchType { get; set; }

        [XmlElement("IVH_BRANCH_TEXT")]
        public string BranchName { get; set; }

        [XmlElement("IVH_TAX_ID")]
        public string TaxID { get; set; }

        [XmlElement("IVH_SHIP_CHARGE")]
        public decimal ShippingCharge { get; set; }

        [XmlElement("IVH_SHIP_CHARGE_DED")]
        public decimal ShippingChargeDeduction { get; set; }

        [XmlElement("IVH_AMOUNT_ADJUST")]
        public decimal AmountAdjust { get; set; }

        [XmlElement("IVH_AMOUNT_ADV_DED_TC")]
        public decimal AmountAdvanceDeductionTC { get; set; }

        [XmlElement("IVH_ORGINAL_RCVD")]
        public int OriginalReceived { get; set; }

        [XmlElement("IVH_IMP_DECL_NO")]
        public string IVH_IMP_DECL_NO { get; set; }

        [XmlElement("IVH_HAS_JRNL_ENTRY")]
        public bool IVH_HAS_JRNL_ENTRY { get; set; }

        [XmlElement("IVH_TASK1_BY")]
        public int Task1By { get; set; }

        [XmlElement("IVH_TASK1_DT")]
        public DateTime Task1Date { get; set; }

        [XmlElement("IVH_TASK2_BY")]
        public int Task2By { get; set; }

        [XmlElement("IVH_TASK2_DT")]
        public DateTime Task2Date { get; set; }

        [XmlElement("IVH_TASK3_BY")]
        public int Task3By { get; set; }

        [XmlElement("IVH_TASK3_DT")]
        public DateTime Task3Date { get; set; }

        [XmlElement("IVH_TASK4_BY")]
        public int Task4By { get; set; }

        [XmlElement("IVH_TASK4_DT")]
        public DateTime Task4Date { get; set; }

        [XmlElement("IVH_STATUS")]
        public int IVH_STATUS { get; set; }

        [XmlElement("IVH_ACTIVE")]
        public int Active { get; set; }

        [XmlElement("IVH_CRTD_BY")]
        public int CreatedUserPK { get; set; }

        [XmlElement("IVH_CRTD_DT")]
        public DateTime CreatedDate { get; set; }

        [XmlElement("IVH_MOD_BY")]
        public int ModifiedUserPK { get; set; }

        [XmlElement("IVH_MOD_DT")]
        public DateTime ModifiedDate { get; set; }

        [XmlElement("IVH_DEPT")]
        public int IVH_DEPT { get; set; }

        [XmlElement("IVH_BIZUNIT")]
        public int BizUnitPK { get; set; }

        [XmlElement("IVH_COMPANY")]
        public int CompanyPK { get; set; }

        [XmlElement("IVH_DEL_STATUS")]
        public int IVH_DEL_STATUS { get; set; }

        [XmlElement("IVH_REASON_FOR_DELETE")]
        public string ReasonForDelete { get; set; }

        [XmlElement("IVH_IS_OPENING")]
        public int IsOpening { get; set; }

        [XmlElement("IVH_TRX_TYPE")]
        public int TransactionType { get; set; }

        [XmlElement("IVH_GST_TYPE")]
        public int GSTType { get; set; }

        [XmlElement("IVH_FROM_PORT")]
        public int FromPort { get; set; }

        [XmlElement("IVH_FROM_PORT_TEXT")]
        public string FromPortName { get; set; }

        [XmlElement("IVH_TO_PORT")]
        public int ToPort { get; set; }

        [XmlElement("IVH_BILL_ENTRY_NO")]
        public string BillEntryNo { get; set; }

        [XmlElement("IVH_BILL_ENTRY_DATE")]
        public DateTime BillEntryDate { get; set; }

        [XmlElement("IVH_BILL_ENTRY_VALUE")]
        public decimal BillEntryValue { get; set; }

        [XmlElement("IVH_AMOUNT_REFUND")]
        public decimal RefundAmount { get; set; }

        [XmlElement("IVH_AMOUNT_SET")]
        public decimal AmountSet { get; set; }

        [XmlElement("IVH_ISSUE_DEPT")]
        public int IssueDepartmentPK { get; set; }

        [XmlElement("IVH_IS_SETTLED")]
        public int IsSettled { get; set; }

        [XmlElement("IVH_INVESTOR")]
        public string Investor { get; set; }

        [XmlElement("IVH_CONVERT")]
        public int ConvertStatus { get; set; }

        [XmlElement("IVH_CONV_PO_PK")]
        public int ConvertedPOPK { get; set; }

        [XmlElement("IVH_CONV_SC_PK")]
        public int ConvertedSCPK { get; set; }

        [XmlElement("IVH_CONV_PO_NO")]
        public string ConvertedPONo { get; set; }

        [XmlElement("IVH_CONV_SC_NO")]
        public string ConvertedSCNo { get; set; }

        [XmlElement("IVH_IS_WORK_ORDER")]
        public int IsWorkOrder { get; set; }

        [XmlElement("Vendor")]
        public VendorMaster PUR_VENDOR_MST { get; set; }

        [XmlElement("Currency")]
        public CurrencyMaster ADM_CURRENCY_MST1 { get; set; }

        [XmlElement("Company")]
        public CompanyMaster ADM_COMPANY_MST { get; set; }

        private List<InvoiceVendorMapping> _InvoiceVendorMappingList;

        [XmlElement("VendorMapping")]
        public List<InvoiceVendorMapping> InvoiceVendorMappingList
        {
            get { return _InvoiceVendorMappingList == null ? new List<InvoiceVendorMapping>() : _InvoiceVendorMappingList; }
            set { _InvoiceVendorMappingList = value; }
        }
    }

    public class CompanyMaster
    {
        [XmlElement("CMP_PK")]
        public int CompanyPK { get; set; }

        [XmlElement("CMP_DISPLAY_CODE")]
        public string CMP_DISPLAY_CODE { get; set; }

        [XmlElement("CMP_LINE_COLOUR")]
        public string CMP_LINE_COLOUR { get; set; }

        [XmlElement("CMP_DISPLAY_NAME")]
        public string DisplayName { get; set; }
    }

    //[Serializable]
    //public class InvoiceVendorMapping
    //{
    //    [XmlElement("IVM_PK")]
    //    public int VendorMappingPK { get; set; }

    //    [XmlElement("IVM_INVOICE_HDR")]
    //    public int InvoicePK { get; set; }

    //    [XmlElement("IVM_PO_HDR")]
    //    public int PurchaseOrderPK { get; set; }

    //    [XmlElement("IVM_AMOUNT")]
    //    public decimal Amount { get; set; }

    //    [XmlElement("IVM_ACTIVE")]
    //    public int Active { get; set; }

    //    [XmlElement("IVM_OTHER_AMOUNT")]
    //    public decimal OterAmount { get; set; }

    //    [XmlElement("IVM_TAX_AMOUNT")]
    //    public decimal TaxAmount { get; set; }

    //    [XmlElement("IVM_DISCOUNT_AMOUNT")]
    //    public decimal DiscountAmount { get; set; }

    //    [XmlElement("IVM_ADJUST_AMOUNT")]
    //    public decimal AdjustAmount { get; set; }

    //    [XmlElement("IVM_WO_HDR")]
    //    public int WorkOrderPK { get; set; }
    //}
}
