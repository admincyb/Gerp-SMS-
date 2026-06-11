using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.POInvoicing
{
    [Serializable]
    [XmlRoot("ROOT")]
    public class POHeaderBO
    {

        [XmlElement("POH_GROUP")]
        public string POH_GROUP { get; set; }
        [XmlElement("PO")]
        public List<POHeaderListBO> POList { get; set; }
    }

    [Serializable]
    public class POHeaderListBO
    {
        [XmlElement("POH_PK")]
        public int POPK { get; set; }

        [XmlElement("POH_GROUP")]
        public int POHGROUP { get; set; }
    }



    [Serializable]
    [XmlRoot("Root")]
    public class InvoiceHearderBO
    {
        [XmlElement("InvHdr")]
        public List<InvoiceBO> InvoiceList { get; set; }
    }

    [Serializable]
    public class InvoiceBO
    {
        [XmlElement("IVH_PK")]
        public long IVH_PK { get; set; }
    }

    [Serializable]
    [XmlRoot("Root")]
    public class PurchaseOrder
    {
        [XmlElement("PO")]
        public List<PurchaseOrderHeader> PurchaseOrderList { get; set; }
    }

    [Serializable]
    public class PurchaseOrderHeader
    {
        [XmlElement("POH_PK")]
        public int PurchaseOrderPK { get; set; }

        [XmlElement("POH_NO")]
        public String PONo { get; set; }

        [XmlElement("AST_VALUE")]
        public int AST_VALUE { get; set; }

        [XmlElement("POH_VERSION")]
        public int Version { get; set; }

        [XmlElement("POH_DATE")]
        public DateTime PODate { get; set; }

        [XmlElement("POH_REFERENCE")]
        public string Reference { get; set; }

        [XmlElement("POH_REF_DATE")]
        public DateTime RefDate { get; set; }

        [XmlElement("POH_DELY_DATE")]
        public DateTime DelyDate { get; set; }

        [XmlElement("POH_STATUS")]
        public int Status { get; set; }

        [XmlElement("POH_CURRENCY")]
        public int CurrencyPK { get; set; }

        [XmlElement("POH_VENDOR")]
        public int VendorPK { get; set; }

        [XmlElement("POH_CONTRACT_REF_NO")]
        public string ContractRefNo { get; set; }

        [XmlElement("POH_SHIPPING")]
        public int ShippingPK { get; set; }

        [XmlElement("POH_BILLING")]
        public int BillingPK { get; set; }

        [XmlElement("POH_TOTAL_QTY")]
        public decimal TotalQuantity { get; set; }

        [XmlElement("POH_SUB_TOTAL")]
        public decimal SubTotal { get; set; }

        [XmlElement("POH_DISC_PERC")]
        public decimal DiscountPercentage { get; set; }

        [XmlElement("POH_DISC_AMT")]
        public decimal DiscountAmount { get; set; }

        [XmlElement("POH_NET_TOTAL")]
        public decimal NetTotal { get; set; }

        [XmlElement("POH_SHIP_CHARGE")]
        public decimal ShippingCharge { get; set; }

        [XmlElement("POH_SALES_TAX_PERC")]
        public decimal SalesTaxPercentage { get; set; }

        [XmlElement("POH_SALES_TAX_AMT")]
        public decimal SalesTaxAmount { get; set; }

        [XmlElement("POH_ADD_TAX_PERC")]
        public decimal AdditionalTaxPercentage { get; set; }

        [XmlElement("POH_ADD_TAX_AMT")]
        public decimal AdditionalTaxAmount { get; set; }

        [XmlElement("POH_PRICE_ADJUST")]
        public decimal PriceAdjust { get; set; }

        [XmlElement("POH_TOTAL_VALUE")]
        public decimal TotalValue { get; set; }

        [XmlElement("POH_CURRENCY_BC")]
        public int BaseCurrencyPK { get; set; }

        [XmlElement("POH_EXCHG_RATE")]
        public double ExchangeRate { get; set; }

        [XmlElement("POH_TOTAL_VALUE_BC")]
        public decimal TotalValueInBC { get; set; }

        [XmlElement("POH_AMT_INVOICED")]
        public decimal InvoicedAmount { get; set; }

        [XmlElement("POH_AMT_PAID")]
        public decimal AmountPaid { get; set; }

        [XmlElement("POH_REMARKS")]
        public string Remarks { get; set; }

        [XmlElement("POH_COMMENTS")]
        public string Comments { get; set; }

        [XmlElement("POH_VENDOR_TERMS")]
        public string VendorTerms { get; set; }

        [XmlElement("POH_TERMS")]
        public string Terms { get; set; }

        [XmlElement("POH_ACTIVE")]
        public int Active { get; set; }

        [XmlElement("POH_SUBMITTED_BY")]
        public int SubmittedUserPK { get; set; }

        [XmlElement("POH_SUBMITTED_DATE")]
        public DateTime SubmittedDate { get; set; }

        [XmlElement("POH_APPROVED_BY")]
        public int ApprovedUserPK { get; set; }

        [XmlElement("POH_APPROVED_DATE")]
        public DateTime ApprovedDate { get; set; }

        [XmlElement("POH_DEPT")]
        public int DepartmentPK { get; set; }

        [XmlElement("POH_BIZUNIT")]
        public int BizUnitPK { get; set; }

        [XmlElement("POH_CRTD_BY")]
        public int CreatedUserPK { get; set; }

        [XmlElement("POH_CRTD_DT")]
        public DateTime CreatedDate { get; set; }

        [XmlElement("POH_MOD_BY")]
        public int ModifiedUserPK { get; set; }

        [XmlElement("POH_MOD_DT")]
        public DateTime ModifiedDate { get; set; }

        [XmlElement("POH_AMEND_DATE")]
        public DateTime AmendDate { get; set; }

        [XmlElement("POH_TYPE")]
        public int Type { get; set; }

        [XmlElement("POH_ITEM_TYPE")]
        public int ItemType { get; set; }

        [XmlElement("POH_GROUP")]
        public int GroupPK { get; set; }

        [XmlElement("POH_COMPANY")]
        public int CompanyPK { get; set; }

        [XmlElement("POH_DEL_STATUS")]
        public int DeleteStatus { get; set; }

        [XmlElement("POH_VERIFIED")]
        public int VerifiedPK { get; set; }

        [XmlElement("POH_VENDOR_TERMS_TEXT")]
        public string VendorTermsText { get; set; }

        [XmlElement("POH_TERMS_TEXT")]
        public string TermsText { get; set; }

        [XmlElement("POH_FROM_PORT_TEXT")]
        public string FromPort { get; set; }

        [XmlElement("POH_TRX_TYPE")]
        public int TrxType { get; set; }

        [XmlElement("POH_IS_GLOVE")]
        public int IsGlove { get; set; }

        [XmlElement("POH_INVESTOR")]
        public string Investor { get; set; }

        [XmlElement("POH_MENU_TYPE")]
        public int MenuType { get; set; }

        [XmlElement("POH_CONVERTED")]
        public int ConvertedStatus { get; set; }

        [XmlElement("POH_GROSS_AMT")]
        public decimal GrossAmount { get; set; }

        [XmlElement("POH_TAX_AMT")]
        public decimal TaxAmount { get; set; }

        [XmlElement("POH_DISCOUNT_AMT")]
        public decimal POHDiscountAmount { get; set; }

        [XmlElement("POH_ADV_INV_AMT")]
        public decimal AdvanceInvoiceAmount { get; set; }

        [XmlElement("POH_INV_AMT")]
        public decimal InvoiceAmount { get; set; }

        [XmlElement("POH_ALLOCATED_ADV_AMT")]
        public decimal AllocatedAdvanceAmount { get; set; }

        [XmlElement("POH_BAL_AMT")]
        public decimal BalanceAmount { get; set; }

        [XmlElement("POH_OTHER_CHRG_AMT")]
        public decimal OtherChargesAmount { get; set; }

        [XmlElement("POH_OTHER_CHRG_PRVS_AMT")]
        public decimal OtherChargesPreviousAmount { get; set; }

        [XmlElement("PODetails")]
        public List<PurchaseOrdeDetails> PurchaseOrderDetailList { get; set; }

        [XmlElement("Vendor")]
        public VendorMaster Vendor { get; set; }

        [XmlElement("Currency")]
        public CurrencyMaster Currency { get; set; }

        [XmlElement("TaxHeader")]
        public List<PurchaseOrderTaxHeader> TaxHeaderList { get; set; }

        [XmlElement("POMpg")]
        public List<InvoiceVendorMapping> InvoiceVendorMappingList { get; set; }

        [XmlElement("VendorAdvanceDeduction")]
        public List<InvoiceVendorAdvanceDeductionDetail> VendorAdvanceDeductionList { get; set; }

        [XmlElement("POH_TAX_PREC")]
        public decimal TaxPercentage { get; set; }
    }

    [Serializable]
    public class PurchaseOrdeDetails
    {
        [XmlElement("POD_PK")]
        public int PurchaseOrdeDetailPK { get; set; }

        [XmlElement("POD_VERSION")]
        public int Version { get; set; }

        [XmlElement("POD_PO")]
        public int PurchaseOrderPk { get; set; }

        [XmlElement("POD_NO")]
        public string POD_NO { get; set; }

        [XmlElement("POD_DATE")]
        public DateTime Date { get; set; }

        [XmlElement("POD_REQD_DATE")]
        public DateTime RequiredDate { get; set; }

        [XmlElement("POD_SL_NO")]
        public int SlNo { get; set; }

        [XmlElement("POD_ITEM")]
        public int ItemPK { get; set; }

        [XmlElement("POD_QTY_REQUESTED")]
        public double RequestedQuantity { get; set; }

        [XmlElement("POD_QTY_APPROVED")]
        public double ApprovedQuantity { get; set; }

        [XmlElement("POD_QTY_RECEIVED")]
        public double ReceivedQuantity { get; set; }

        [XmlElement("POD_QTY_ADDITIONAL")]
        public double AdditionalQuantity { get; set; }

        [XmlElement("POD_QTY_ACCEPTED")]
        public double AcceptedQuantity { get; set; }

        [XmlElement("POD_QTY_ALLOCATED")]
        public double AllocatedQuantity { get; set; }

        [XmlElement("POD_QTY_ADDL_ALLOCATED")]
        public double AdditionalAllocatedQuantity { get; set; }
        
        [XmlElement("POD_QTY_INVOICED")]
        public double InvoicedQuantity { get; set; }

        [XmlElement("POD_UOM")]
        public int UOMPk { get; set; }

        [XmlElement("POD_CONV_FACT")]
        public double ConversionFactor { get; set; }

        [XmlElement("POD_RATE")]
        public double Rate { get; set; }

        [XmlElement("POD_AMOUNT")]
        public decimal Amount { get; set; }

        [XmlElement("POD_TAX_PERC")]
        public decimal TaxPercentage { get; set; }

        [XmlElement("POD_TAX")]
        public decimal Tax { get; set; }

        [XmlElement("POD_DISC_PERC")]
        public decimal DiscountPercentage { get; set; }

        [XmlElement("POD_DISC_AMT")]
        public decimal DiscountAmount { get; set; }

        [XmlElement("POD_AMT_VALUE")]
        public decimal AmountValue { get; set; }

        [XmlElement("POD_DEPT")]
        public int DepartmentPK { get; set; }

        [XmlElement("POD_BIZUNIT")]
        public int BizUnitPK { get; set; }

        [XmlElement("POD_RATE_UPDATE")]
        public int RateUpdate { get; set; }

        [XmlElement("POD_QTY_RETURNED")]
        public double ReturnedQuantity { get; set; }

        [XmlElement("POD_RATE_PREV")]
        public double PreviousRate { get; set; }

        [XmlElement("TaxDetails")]
        public List<PurchaseOrderTaxDetails> TaxDetailList { get; set; }
    }

    [Serializable]
    public class VendorMaster
    {
        [XmlElement("VEN_PK")]
        public int VendorPK { get; set; }

        [XmlElement("VEN_CODE")]
        public string VendorCode { get; set; }

        [XmlElement("VEN_NAME")]
        public string VEN_NAME { get; set; }

        [XmlElement("VEN_STATUS")]
        public int Status { get; set; }

        [XmlElement("VEN_ANNUAL_SALES")]
        public decimal AnnualSales { get; set; }

        [XmlElement("VEN_HAS_ISO")]
        public bool HasISO { get; set; }

        [XmlElement("VEN_ACTIVE")]
        public int Active { get; set; }

        [XmlElement("VEN_BIZUNIT")]
        public int BizUnitPK { get; set; }

        [XmlElement("VEN_ACCOUNT")]
        public int AccountPK { get; set; }

        [XmlElement("VEN_ADDR1")]
        public string Address1 { get; set; }

        [XmlElement("VEN_CNTRY")]
        public int CountryPK { get; set; }

        [XmlElement("VEN_EMAIL")]
        public string Email { get; set; }

        [XmlElement("VEN_FAX")]
        public string Fax { get; set; }

        [XmlElement("VEN_MOBIL")]
        public string Mobile { get; set; }

        [XmlElement("VEN_PHONE")]
        public string Phone { get; set; }

        [XmlElement("VEN_PIN")]
        public string PIN { get; set; }

        [XmlElement("VEN_CRTD_BY")]
        public int CreatedUserPK { get; set; }

        [XmlElement("VEN_CRTD_DT")]
        public DateTime CreatedDate { get; set; }

        [XmlElement("VEN_MOD_BY")]
        public int ModifiedUserPK { get; set; }

        [XmlElement("VEN_MOD_DT")]
        public DateTime ModifiedDate { get; set; }

        [XmlElement("VEN_PO_TYPE")]
        public int POType { get; set; }

        [XmlElement("VEN_PUR_TAX_TYPE")]
        public int TaxType { get; set; }

        [XmlElement("VEN_DATE")]
        public DateTime Date { get; set; }

        [XmlElement("VEN_GST_TYPE")]
        public int GSTType { get; set; }

        [XmlElement("VEN_TRX_TYPE")]
        public int TrxType { get; set; }
    }

    [Serializable]
    public class CurrencyMaster
    {
        [XmlElement("CUR_PK")]
        public int CurrencyPK { get; set; }

        [XmlElement("CUR_CODE")]
        public string CUR_CODE { get; set; }

        [XmlElement("CUR_NAME")]
        public string Name { get; set; }

        [XmlElement("CUR_ACTIVE")]
        public int Active { get; set; }

        [XmlElement("CUR_BIZUNIT")]
        public int BizUnitPK { get; set; }

        [XmlElement("CUR_CRTD_BY")]
        public int CreatedUserPK { get; set; }

        [XmlElement("CUR_CRTD_DT")]
        public DateTime CreatedDate { get; set; }

        [XmlElement("CUR_MOD_BY")]
        public int ModifiedUserPK { get; set; }

        [XmlElement("CUR_MOD_DT")]
        public DateTime ModifiedDate { get; set; }
    }

    [Serializable]
    public class PurchaseOrderTaxHeader
    {
        [XmlElement("PTH_PK")]
        public int PurchaseOrderTaxHeaderPK { get; set; }

        [XmlElement("PTH_PO")]
        public int PurchaseOrderPK { get; set; }

        [XmlElement("PTH_TYPE")]
        public int Type { get; set; }

        [XmlElement("PTH_TAX")]
        public int TaxPK { get; set; }

        [XmlElement("PTH_TAX_CATEGORY")]
        public int CategoryPK { get; set; }

        [XmlElement("PTH_NAME")]
        public string TaxName { get; set; }

        [XmlElement("PTH_TAX_AMT")]
        public decimal TaxAmount { get; set; }

        [XmlElement("PTH_HAS_SUB_TOTAL")]
        public int HasSubTotal { get; set; }

        [XmlElement("PTH_HAS_DISCOUNT")]
        public int HasDiscount { get; set; }

        [XmlElement("PTH_HAS_OTHER_CHARGE")]
        public int HasOtherCharge { get; set; }
        [XmlElement("PTH_TAX_PREC")]
        public decimal Taxpercentage { get; set; }
    }

    [Serializable]
    public class PurchaseOrderTaxDetails
    {
        [XmlElement("POT_PK")]
        public int PurchaseOrderTaxDetailPK { get; set; }

        [XmlElement("POT_PO_DTL")]
        public int PurchaseOrdeDetailPK { get; set; }

        [XmlElement("POT_TYPE")]
        public int Type { get; set; }

        [XmlElement("POT_TAX")]
        public int TaxPK { get; set; }

        [XmlElement("POT_TAX_CATEGORY")]
        public int CategoryPK { get; set; }

        [XmlElement("POT_NAME")]
        public string TaxName { get; set; }

        [XmlElement("POT_TAX_AMT")]
        public decimal TaxAmount { get; set; }
    }

    [Serializable]
    [XmlRoot("Root")]
    public class VendorInvMpg
    {
        [XmlElement("InvHeaderList")]
        public List<InvoiceVendorMapping> InvoiceVendorMappingList { get; set; }
    }

    [Serializable]
    public class InvoiceVendorMapping
    {
        [XmlElement("IVM_PK")]
        public int InvoiceVendorMappingPK { get; set; }

        [XmlElement("IVM_INVOICE_HDR")]
        public int InvoicePK { get; set; }

        [XmlElement("IVM_PO_HDR")]
        public int PurchaseOrderPk { get; set; }

        [XmlElement("IVM_AMOUNT")]
        public decimal Amount { get; set; }

        [XmlElement("IVM_ACTIVE")]
        public int Active { get; set; }

        [XmlElement("IVM_OTHER_AMOUNT")]
        public decimal OtherAmount { get; set; }

        [XmlElement("IVM_TAX_AMOUNT")]
        public decimal TaxAmount { get; set; }

        [XmlElement("IVM_DISCOUNT_AMOUNT")]
        public decimal Discount { get; set; }

        [XmlElement("IVM_ADJUST_AMOUNT")]
        public decimal AdjustAmount { get; set; }

        [XmlElement("IVM_WO_HDR")]
        public int WorkOrderPK { get; set; }

        [XmlElement("POH_GROUP")]
        public int GroupPK { get; set; }

        //[XmlElement("POH_GROSS_AMT")]
        //public decimal GrossAmount { get; set; }

        //[XmlElement("POH_TAX_AMT")]
        //public decimal TotalTaxAmount { get; set; }

        //[XmlElement("POH_DISCOUNT_AMT")]
        //public decimal POHDiscountAmount { get; set; }

        //[XmlElement("POH_ADV_INV_AMT")]
        //public decimal AdvanceInvoiceAmount { get; set; }

        //[XmlElement("POH_INV_AMT")]
        //public decimal InvoiceAmount { get; set; }

        //[XmlElement("POH_ALLOCATED_ADV_AMT")]
        //public decimal AllocatedAdvanceAmount { get; set; }

        //[XmlElement("POH_BAL_AMT")]
        //public decimal BalanceAmount { get; set; }

        //[XmlElement("POH_OTHER_CHRG_AMT")]
        //public decimal OtherChargesAmount { get; set; }

        //[XmlElement("POH_OTHER_CHRG_PRVS_AMT")]
        //public decimal OtherChargesPreviousAmount { get; set; }


        [XmlElement("InvHeader")]
        public InvoiceVendorHeader VendorHeader { get; set; }

        [XmlElement("POHeader")]
        public PurchaseOrderHeader PurchaseOrder { get; set; }
    }

    [Serializable]
    public class InvoiceVendorHeader
    {
        [XmlElement("IVH_PK")]
        public int InvoiceVendorHeaderPK { get; set; }

        [XmlElement("IVH_DEL_STATUS")]
        public int DeleteStatus { get; set; }

        [XmlElement("IVH_STATUS")]
        public int Status { get; set; }

        [XmlElement("IVH_CATEGORY")]
        public int CategoryPK { get; set; }
        
        [XmlElement("IVH_VENDOR_CONTACT")]
        public int VendorContact { get; set; }

        //[XmlElement("VendorAdvanceDeduction")]
        //public List<InvoiceVendorAdvanceDeductionDetail> VendorAdvanceDeductionList { get; set; }
    }

    [Serializable]
    public class InvoiceVendorAdvanceDeductionDetail //FIN_INVOICE_VND_ADV_DED_DTL
    {
        [XmlElement("VAD_PK")]
        public int VendorAdvanceDeductionDetailPK { get; set; }

        [XmlElement("VAD_INVOICE_HDR")]
        public int InvoicePK { get; set; }

        [XmlElement("VAD_AMOUNT")]
        public decimal Amount { get; set; }

        [XmlElement("VAD_TAX_AMOUNT")]
        public decimal TaxAmount { get; set; }

        [XmlElement("VAD_OTHER_AMOUNT")]
        public decimal OtherAmount { get; set; }

        [XmlElement("AdvDedInvHeader")]
        public InvoiceVendorHeader VendorHeader { get; set; }
    }
}
