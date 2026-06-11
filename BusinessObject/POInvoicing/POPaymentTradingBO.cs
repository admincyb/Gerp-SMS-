using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.POInvoicing
{
    [Serializable]
    [XmlRoot("Root")]
    public class POPaymentTradingBO : WorkflowBO
    {
        
        #region Variables
        [XmlElement("PVH_PK")]
        public int PVH_PK { get; set; }
        [XmlElement("PVH_DATE")]
        public string PVH_DATE { get; set; }
        [XmlElement("PVH_NO")]
        public string PVH_NO { get; set; }
        [XmlElement("PVH_CATEGORY")]
        public byte PVH_CATEGORY { get; set; }
        [XmlElement("PVH_GROUP")]
        public byte PVH_GROUP { get; set; }
        [XmlElement("PVH_VENDOR")]
        public string PVH_VENDOR { get; set; }
        [XmlElement("PVH_VENDOR_TEXT")]
        public string PVH_VENDOR_TEXT { get; set; }
        [XmlElement("PVH_VENDOR_ACCOUNT")]
        public string PVH_VENDOR_ACCOUNT { get; set; }
        [XmlElement("PVH_MODE")]
        public string PVH_MODE { get; set; }
        [XmlElement("PVH_BANK")]
        public string PVH_BANK { get; set; }
        [XmlElement("PVH_BRANCH")]
        public string PVH_BRANCH { get; set; }
        [XmlElement("PVH_INSTR_NO")]
        public string PVH_INSTR_NO { get; set; }
        [XmlElement("PVH_INSTR_DATE")]
        public string PVH_INSTR_DATE { get; set; }
        [XmlElement("PVH_INSTR_FAVOUR")]
        public string PVH_INSTR_FAVOUR { get; set; }
        [XmlElement("PVH_BANK_CASH_ACCOUNT")]
        public string PVH_BANK_CASH_ACCOUNT { get; set; }
        [XmlElement("PVH_CURRENCY")]
        public string PVH_CURRENCY { get; set; }
        [XmlElement("PVH_CURRENCY_TEXT")]
        public string PVH_CURRENCY_TEXT { get; set; }
        [XmlElement("PVH_PAID_AMOUNT")]
        public decimal PVH_PAID_AMOUNT { get; set; }
        [XmlElement("PVH_DISCOUNT")]
        public string PVH_DISCOUNT { get; set; }
        [XmlElement("PVH_DISC_AMOUNT")]
        public decimal PVH_DISC_AMOUNT { get; set; }
        [XmlElement("PVH_TAX_AMOUNT")]
        public decimal PVH_TAX_AMOUNT { get; set; }
        [XmlElement("PVH_WHT_AMOUNT")]
        public decimal PVH_WHT_AMOUNT { get; set; }
        [XmlElement("PVH_WHT_TAX")]
        public string PVH_WHT_TAX { get; set; }
        [XmlElement("PVH_WHT_BOOK_NO")]
        public string PVH_WHT_BOOK_NO { get; set; }
        [XmlElement("PVH_WHT_NO")]
        public string PVH_WHT_NO { get; set; }
        [XmlElement("PVH_BANK_CHARGE")]
        public string PVH_BANK_CHARGE { get; set; }
        [XmlElement("PVH_BANK_CHARGE_CURR")]
        public string PVH_BANK_CHARGE_CURR { get; set; }
        [XmlElement("PVH_BASE_CURR")]
        public int PVH_BASE_CURR { get; set; }
        [XmlElement("PVH_EXCHG_RATE")]
        public double PVH_EXCHG_RATE { get; set; }
        [XmlElement("PVH_PAID_AMOUNT_BC")]
        public decimal PVH_PAID_AMOUNT_BC { get; set; }
        [XmlElement("PVH_REMARKS")]
        public string PVH_REMARKS { get; set; }
        [XmlElement("PVH_HAS_JRNL_ENTRY")]
        public bool PVH_HAS_JRNL_ENTRY { get; set; }
        [XmlElement("PVH_PDC")]
        public byte PVH_PDC { get; set; }
        [XmlElement("PVH_BOUNCED")]
        public int PVH_BOUNCED { get; set; }
        [XmlElement("PVH_PAY_FOR_VENDOR")]
        public bool PVH_PAY_FOR_VENDOR { get; set; }
        [XmlElement("PVH_STATUS")]
        public int PVH_STATUS { get; set; }
        [XmlElement("PVH_ACTIVE")]
        public byte PVH_ACTIVE { get; set; }
        [XmlElement("PVH_DEPT")]
        public int PVH_DEPT { get; set; }
        [XmlElement("PVH_BIZUNIT")]
        public int PVH_BIZUNIT { get; set; }
        [XmlElement("PVH_COMPANY")]
        public int PVH_COMPANY { get; set; }
        [XmlElement("PVH_DEL_STATUS")]
        public int PVH_DEL_STATUS { get; set; }
        [XmlElement("PVH_REASON_FOR_DELETE")]
        public string PVH_REASON_FOR_DELETE { get; set; }
        [XmlElement("PVH_BANK_CHARGE_TYPE")]
        public string PVH_BANK_CHARGE_TYPE { get; set; }
        [XmlElement("PVH_OTHER_AMOUNT")]
        public decimal PVH_OTHER_AMOUNT { get; set; }
        [XmlElement("PVH_VENDOR_BANK")]
        public string PVH_VENDOR_BANK { get; set; }
        [XmlElement("PVH_MOD_DT")]
        public DateTime PVH_MOD_DT { get; set; }

        [XmlElement("PVH_VEN_TIN")]
        public string PVH_VEN_TIN { get; set; }
        [XmlElement("PVH_VEN_ADDR3")]
        public string PVH_VEN_ADDR3 { get; set; }
        [XmlElement("PVH_VEN_NAME2")]
        public string PVH_VEN_NAME2 { get; set; }
        [XmlElement("PVH_VENDOR_WHT_TAX")]
        public string PVH_VENDOR_WHT_TAX { get; set; }
      

        [XmlElement("ATL_ACTION")]
        public byte ATL_ACTION { get; set; }
        [XmlElement("ATL_APP_TYPE")]
        public string ATL_APP_TYPE { get; set; }
        [XmlElement("USER_PK")]
        public short USER_PK { get; set; }

        [XmlElement("APT_CODE")]
        public string APT_CODE { get; set; }
        [XmlElement("AST_DOC_MODE")]
        public int AST_DOC_MODE { get; set; }
        [XmlElement("AST_VALUE")]
        public string AST_VALUE { get; set; }
        [XmlElement("WKF_FLAG")]
        public int WKF_FLAG { get; set; }

        #endregion

        [XmlElement("TrxMpg")]
        public List<PaymentInvoiceTrxMpgDetails> TrxMpg { get; set; }
        [XmlElement("ModeDetail")]
        public List<PaymentModeDetails> ModeDetail { get; set; }
        [XmlElement("TaxHdr")]
        public List<PaymentTaxHeader> TaxHdr { get; set; }
        [XmlElement("FileList")]
        public List<PaymentUploads> FileList { get; set; }

        
    }
    [Serializable]
    public class PaymentInvoiceTrxMpgDetails
    {

        #region FIN_PAYMENT_VND_TRX_MPG table fields
        [XmlElement]
        public long PVM_PK { get; set; }
        [XmlElement]
        public string PVM_PAYMENT_HDR { get; set; }
        [XmlElement]
        public string PVM_INVOICE_HDR { get; set; }
        [XmlElement]
        public string PVM_PO_HDR { get; set; }
        [XmlElement]
        public decimal PVM_PAID_AMOUNT { get; set; }
        [XmlElement]
        public decimal PVM_DISC_AMOUNT { get; set; }
        [XmlElement]
        public byte PVM_ACTIVE { get; set; }
        [XmlElement]
        public decimal PVM_TAX_AMOUNT { get; set; }
        [XmlElement]
        public decimal PVM_OTHER_AMOUNT { get; set; }
        [XmlElement]
        public int PVM_BOUNCED { get; set; }
        [XmlElement]
        public decimal PVM_ADJUST_AMOUNT { get; set; }
        [XmlElement]
        public decimal PVM_EXCESS_AMOUNT { get; set; }

        #endregion

        #region Invoice fields(FIN_INVOICE_VND_HDR)
        [XmlElement]
        public int IVH_PK { get; set; }
        [XmlElement]
        public string IVH_NO { get; set; }
        [XmlElement]
        public string IVH_DATE { get; set; }
        [XmlElement]
        public int IVH_VENDOR { get; set; }
        [XmlElement]
        public string IVH_VENDOR_TEXT { get; set; }
        [XmlElement]
        public byte IVH_IS_LINE_ITEM_TAX { get; set; }
        [XmlElement]
        public decimal IVH_AMOUNT { get; set; }
        [XmlElement]
        public decimal IVH_TAX_AMT { get; set; }
        [XmlElement]
        public decimal IVH_DISC_AMOUNT { get; set; }
        [XmlElement]
        public decimal IVH_SHIP_CHARGE { get; set; }
        [XmlElement]
        public decimal IVH_SHIP_CHARGE_DED { get; set; }
        [XmlElement]
        public decimal IVH_GROSS_AMT { get; set; }
        [XmlElement]
        public decimal IVH_TOTAL_AMT { get; set; }
        [XmlElement]
        public decimal IVH_AMOUNT_PAID_TC { get; set; }
        [XmlElement]
        public decimal IVH_ADJ_AMT { get; set; }
        [XmlElement]
        public decimal IVH_BAL_TO_PAY { get; set; }
        [XmlElement]
        public decimal IVH_TAX_PERC { get; set; }
        [XmlElement]
        public decimal VTL_TAX_AMOUNT { get; set; }
        [XmlElement]
        public decimal VTL_DISC_AMOUNT { get; set; }
        [XmlElement]
        public decimal IVH_DISCOUNT_TC { get; set; }
        [XmlElement]
        public decimal IVH_AMOUNT_NET_TC { get; set; }
        [XmlElement]
        public decimal IVH_PAID_AMT { get; set; }
        [XmlElement]
        public byte IVH_HAS_JRNL_ENTRY { get; set; }
        [XmlElement]
        public byte IVH_GROUP { get; set; }
        [XmlElement]
        public byte IVH_CATEGORY { get; set; }
        [XmlElement]
        public byte IVH_TYPE { get; set; }
        [XmlElement]
        public decimal IVH_INVOICE_AMT { get; set; }
        [XmlElement]
        public decimal IVH_AMOUNT_TC { get; set; }

        [XmlElement]
        public decimal IVH_HDR_TAX { get; set; }
        [XmlElement]
        public decimal IVH_LINE_ITM_TAX { get; set; }
        [XmlElement]
        public decimal IVH_TOTAL_TAX { get; set; }      
        [XmlElement]
        public decimal IVH_HDR_DISCOUNT { get; set; }
        [XmlElement]
        public decimal IVH_LINE_ITM_DISCOUNT { get; set; }
        [XmlElement]
        public decimal IVH_TOTAL_DISCOUNT { get; set; }

        [XmlElement]
        public decimal IVH_DISCOUNT_TOTAL { get; set; }//discTotAmount

        [XmlElement]
        public string IVH_VENDOR_INV_NO { get; set; }
        [XmlElement]
        public DateTime IVH_DATE_RECEIVED { get; set; }
        [XmlElement]
        public string IVH_CURRENCY_TEXT { get; set; }
        [XmlElement]
        public decimal PVM_CN_AMOUNT { get; set; }
        [XmlElement]
        public string CMP_DISPLAY_CODE { get; set; }
        [XmlElement]
        public string CMP_LINE_COLOUR { get; set; }
        #endregion

        [XmlElement("PVM_SL_NO")]
        public int PVM_SL_NO { get; set; }

        [XmlElement("AllocationMpg")]
        public List<PaymentCRDRAdjAllocationDtl> AllocationMpg { get; set; }
        [XmlElement("POMpg")]
        public List<PaymentPOMappingDetails> POMpg { get; set; }
        [XmlElement("CRDRMpg")]
        public List<PaymentCRDRMappingDetails> CRDRMpg { get; set; }
        [XmlElement("TaxDetails")]
        public List<PaymentTaxDetail> TaxDetails { get; set; }
    }
    [Serializable]
    public class PaymentCRDRAdjAllocationDtl
    {
        [XmlElement("PAD_PK")]
        public int PAD_PK { get; set; }
        [XmlElement("PAD_PAYMENT_TRX")]
        public long PAD_PAYMENT_TRX { get; set; }
        [XmlElement("PAD_ALCN_CDH")]
        public int PAD_ALCN_CDH { get; set; }
        [XmlElement("PAD_ALCN_PAYMENT_TRX")]
        public int PAD_ALCN_PAYMENT_TRX { get; set; }
        [XmlElement("PAD_AMOUNT")]
        public decimal PAD_AMOUNT { get; set; }
        [XmlElement("PAD_ACTIVE")]
        public int PAD_ACTIVE { get; set; }

        [XmlElement("PAD_PVM_INVOICE_HDR")]
        public long PAD_PVM_INVOICE_HDR { get; set; }  
        [XmlElement("PAD_PVM_SL_NO")]
        public int PAD_PVM_SL_NO { get; set; }       
    }
    [Serializable]
    public class PaymentPOMappingDetails
    {
        [XmlElement("PPO_PK")]
        public int PPO_PK { get; set; }
        [XmlElement("PPO_PAYMENT_HDR")]
        public int PPO_PAYMENT_HDR { get; set; }
        [XmlElement("PPO_PAYMENT_TRX_MPG")]
        public int PPO_PAYMENT_TRX_MPG { get; set; }
        [XmlElement("PPO_PO_HDR")]
        public string PPO_PO_HDR { get; set; }
        [XmlElement("PPO_PAID_AMOUNT")]
        public decimal PPO_PAID_AMOUNT { get; set; }
        [XmlElement("PPO_ACTIVE")]
        public int PPO_ACTIVE { get; set; }
        [XmlElement("PPO_BOUNCED")]
        public int PPO_BOUNCED { get; set; }
        [XmlElement("PPO_OTHER_AMOUNT")]
        public decimal PPO_OTHER_AMOUNT { get; set; }
        [XmlElement("PPO_TAX_AMOUNT")]
        public decimal PPO_TAX_AMOUNT { get; set; }
       
        [XmlElement("POH_NO")]
        public string POH_NO { get; set; }
        [XmlElement("POH_DATE")]
        public string POH_DATE { get; set; }
        [XmlElement("POH_CURRENCY_TEXT")]
        public string POH_CURRENCY_TEXT { get; set; }        
        [XmlElement("POH_IVH_PK")]
        public int POH_IVH_PK { get; set; }
        [XmlElement("POH_TOTAL_VALUE")]
        public decimal POH_TOTAL_VALUE { get; set; }
        [XmlElement("POD_TAX")]
        public decimal POD_TAX { get; set; }
        [XmlElement("POD_DISC_AMT")]
        public decimal POD_DISC_AMT { get; set; }
        [XmlElement("POH_AMT_PAID")]
        public decimal POH_AMT_PAID { get; set; }
        [XmlElement("POH_BAL_TO_PAY_AMT")]
        public decimal POH_BAL_TO_PAY_AMT { get; set; }
        [XmlElement("POH_INVOICE_OTH_AMT")]
        public decimal POH_INVOICE_OTH_AMT { get; set; }

     
        [XmlElement("POH_HDR_TAX")]
        public decimal POH_HDR_TAX { get; set; }
        [XmlElement("POH_LINE_ITM_TAX")]
        public decimal POH_LINE_ITM_TAX { get; set; }
        [XmlElement("POH_TOTAL_TAX")]
        public decimal POH_TOTAL_TAX { get; set; }      
        [XmlElement("POH_HDR_DISCOUNT")]
        public decimal POH_HDR_DISCOUNT { get; set; }
        [XmlElement("POH_LINE_ITM_DISCOUNT")]
        public decimal POH_LINE_ITM_DISCOUNT { get; set; }
        [XmlElement("POH_TOTAL_DISCOUNT")]
        public decimal POH_TOTAL_DISCOUNT { get; set; }
        

        [XmlElement("PPO_PVM_SL_NO")]
        public int PPO_PVM_SL_NO { get; set; }
        [XmlElement("PPO_SL_NO")]
        public int PPO_SL_NO { get; set; }
      
    }
    [Serializable]
    public class PaymentCRDRMappingDetails
    {
        [XmlElement("PNM_PK")]
        public int PNM_PK { get; set; }
        [XmlElement("PNM_INVOICE_HDR")]
        public int PNM_INVOICE_HDR { get; set; }
        [XmlElement("PNM_PAYMENT_HDR")]
        public int PNM_PAYMENT_HDR { get; set; }
        [XmlElement("PNM_PAYMENT_TRX_MPG")]
        public int PNM_PAYMENT_TRX_MPG { get; set; }
        [XmlElement("PNM_CRDR_HDR")]
        public int PNM_CRDR_HDR { get; set; }
        [XmlElement("PNM_PAID_AMOUNT")]
        public decimal PNM_PAID_AMOUNT { get; set; }
        [XmlElement("PNM_ADJ_AMOUNT")]
        public decimal PNM_ADJ_AMOUNT { get; set; }
        [XmlElement("PNM_ACTIVE")]
        public int PNM_ACTIVE { get; set; }
        [XmlElement("PNM_CRDR_MPG")]
        public string PNM_CRDR_MPG { get; set; }

        [XmlElement("PNM_BALANCE_AMOUNT")]
        public decimal PNM_BALANCE_AMOUNT { get; set; }
        [XmlElement("PNM_CRDR_NO")]
        public string PNM_CRDR_NO { get; set; }
        [XmlElement("PNM_CRDR_DATE")]
        public string PNM_CRDR_DATE { get; set; }        
        [XmlElement("PNM_CRDR_CURRENCY_TEXT")]
        public string PNM_CRDR_CURRENCY_TEXT { get; set; }
        [XmlElement("PNM_CRDR_AMOUNT")]
        public decimal PNM_CRDR_AMOUNT { get; set; }
        [XmlElement("PNM_ALLOCATED_AMOUNT")]
        public decimal PNM_ALLOCATED_AMOUNT { get; set; }
        

        [XmlElement("PNM_PVM_SL_NO")]
        public int PNM_PVM_SL_NO { get; set; }       
    }
    [Serializable]
    public class PaymentModeDetails
    {
        [XmlElement("PDM_PK")]
        public int PDM_PK { get; set; }

        [XmlElement("PDM_PAYMENT_HDR")]
        public int PDM_PAYMENT_HDR { get; set; }
        [XmlElement("PDM_MODE")]
        public byte PDM_MODE { get; set; }
        [XmlElement("PDM_ACCOUNT")]
        public int PDM_ACCOUNT { get; set; }
        [XmlElement("PDM_PAID_AMOUNT")]
        public decimal PDM_PAID_AMOUNT { get; set; }
        [XmlElement("PDM_BANK")]
        public string PDM_BANK { get; set; }
        [XmlElement("PDM_BRANCH")]
        public string PDM_BRANCH { get; set; }
        [XmlElement("PDM_BANK_CHARGE")]
        public decimal PDM_BANK_CHARGE { get; set; }
        [XmlElement("PDM_BANK_CHARGE_TYPE")]
        public bool PDM_BANK_CHARGE_TYPE { get; set; }
        [XmlElement("PDM_BANK_CHARGE_CURR")]
        public string PDM_BANK_CHARGE_CURR { get; set; }
        [XmlElement("PDM_EXCHG_RATE")]
        public double PDM_EXCHG_RATE { get; set; }
        [XmlElement("PDM_INSTR_NO")]
        public string PDM_INSTR_NO { get; set; }
        [XmlElement("PDM_INSTR_DATE")]
        public string PDM_INSTR_DATE { get; set; }
        [XmlElement("PDM_INSTR_FAVOUR")]
        public string PDM_INSTR_FAVOUR { get; set; }
        [XmlElement("PDM_PDC")]
        public byte PDM_PDC { get; set; }
        [XmlElement("PDM_BOUNCED")]
        public int PDM_BOUNCED { get; set; }
        [XmlElement("PDM_PAID_AMOUNT_BC")]
        public decimal PDM_PAID_AMOUNT_BC { get; set; }
    }
    [Serializable]
    public class PaymentTaxHeader
    {
        [XmlElement("WTH_PK")]
        public int WTH_PK { get; set; }
        [XmlElement("WTH_PAYMENT_HDR")]
        public int WTH_PAYMENT_HDR { get; set; }
        [XmlElement("WTH_TYPE")]
        public int WTH_TYPE { get; set; }
        [XmlElement("WTH_TAX")]
        public int WTH_TAX { get; set; }
        [XmlElement("WTH_TAX_CATEGORY")]
        public int WTH_TAX_CATEGORY { get; set; }
        [XmlElement("WTH_NAME")]
        public string WTH_NAME { get; set; }
        [XmlElement("WTH_AMOUNT")]
        public decimal WTH_AMOUNT { get; set; }
        [XmlElement("WTH_TAX_AMT")]
        public decimal WTH_TAX_AMT { get; set; }
        [XmlElement("WTH_DESC")]
        public string WTH_DESC { get; set; }
        [XmlElement("WTH_FORM_NO")]
        public string WTH_FORM_NO { get; set; }
        [XmlElement("WTH_PARTY_NAME")]
        public string WTH_PARTY_NAME { get; set; }
        [XmlElement("WTH_ADDRESS")]
        public string WTH_ADDRESS { get; set; }
        [XmlElement("WTH_TAX_ID")]
        public string WTH_TAX_ID { get; set; }
        [XmlElement("WTH_TRX_HDR")]
        public int WTH_TRX_HDR { get; set; }
        [XmlElement("WTH_CATEGORY")]
        public int WTH_CATEGORY { get; set; }
        [XmlElement("WTH_TAX_INV_NO")]
        public string WTH_TAX_INV_NO { get; set; }
        [XmlElement("WTH_TAX_DATE")]
        public DateTime WTH_TAX_DATE { get; set; }
        [XmlElement("WTH_BRANCH_TYPE")]
        public int WTH_BRANCH_TYPE { get; set; }
        [XmlElement("WTH_BRANCH_TEXT")]
        public string WTH_BRANCH_TEXT { get; set; }
        [XmlElement("WTH_ITEM_TEXT")]
        public string WTH_ITEM_TEXT { get; set; }
        [XmlElement("WTH_PUR_INVOICE")]
        public long WTH_PUR_INVOICE { get; set; }
        [XmlElement("WTH_BRANCH")]
        public string WTH_BRANCH { get; set; }
        [XmlElement("WTH_BRANCH_NAME")]
        public string WTH_BRANCH_NAME { get; set; }
        [XmlElement("WTH_VENDOR")]
        public string WTH_VENDOR { get; set; }
        [XmlElement("WTH_REFUND_DATE")]
        public DateTime WTH_REFUND_DATE { get; set; }
        [XmlElement("WTH_PAYMENT_TYPE")]
        public int WTH_PAYMENT_TYPE { get; set; }
        [XmlElement("WTH_INV_RECEIVED")]
        public int WTH_INV_RECEIVED { get; set; }
    }
    [Serializable]
    public class PaymentTaxDetail
    {
        [XmlElement("PDT_PK")]
        public int PDT_PK { get; set; }
        [XmlElement("PDT_PAYMENT_TRX")]
        public int PDT_PAYMENT_TRX { get; set; }
        [XmlElement("PDT_INVOICE_VND_DTL")]
        public int PDT_INVOICE_VND_DTL { get; set; }
        [XmlElement("PDT_VND_PO_MPG")]
        public int PDT_VND_PO_MPG { get; set; }
        [XmlElement("PDT_POD")]
        public int PDT_POD { get; set; }
        [XmlElement("PDT_TAX")]
        public decimal PDT_TAX { get; set; }
        [XmlElement("PDT_AMOUNT")]
        public decimal PDT_AMOUNT { get; set; }
        [XmlElement("PDT_TAX_CATEGORY")]
        public int PDT_TAX_CATEGORY { get; set; }

        [XmlElement("PDT_TAX_PERC")]
        public double PDT_TAX_PERC { get; set; }
        [XmlElement("PDT_INVOICE_HDR")] 
        public int PDT_INVOICE_HDR { get; set; }
        [XmlElement("PDT_PO_HDR")]  
        public int PDT_PO_HDR { get; set; }
        [XmlElement("PDT_PVM_SL_NO")]
        public int PDT_PVM_SL_NO { get; set; }
        //For Advance Invoice
        [XmlElement("PDT_PPO_SL_NO")]               
        public int PDT_PPO_SL_NO { get; set; }
    }
    [Serializable]
    public class PaymentUploads
    {
        [XmlElement("DOC_PK")]
        public int DOC_PK { get; set; }
        [XmlElement("DOC_SEQ_NO")]
        public int DOC_SEQ_NO { get; set; }
        [XmlElement("DOC_TITLE")]
        public string DOC_TITLE { get; set; }
        [XmlElement("DOC_NAME")]
        public string DOC_NAME { get; set; }
        [XmlElement("DOC_PATH")]
        public string DOC_PATH { get; set; }
        [XmlElement("DOC_TYPE")]
        public string DOC_TYPE { get; set; }
        [XmlElement("DOC_ACTIVE")]
        public int DOC_ACTIVE { get; set; }
        public string FileExtension { get; set; }
        public string AttachmentFileName { get; set; }
    }

    [Serializable]
    [XmlRoot("ROOT")]
    public class TradingInvHeaderBO
    {
        [XmlElement("INV")]
        public List<TradingInvHeaderListBO> INVList { get; set; }
    }
    [Serializable]
    public class TradingInvHeaderListBO
    {
        [XmlElement("IVH_PK")]
        public int IVH_PK { get; set; }
        [XmlElement("CDH_PK")]
        public int CDH_PK { get; set; }
        public int IVH_VENDOR { get; set; }
        public int IVH_TYPE { get; set; }
        public int IVH_CURRENCY { get; set; }
        public byte IVH_CATEGORY { get; set; }
        public byte IVH_GROUP { get; set; }
    }    
}
