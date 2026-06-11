using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.Sales
{
    public class SalesReceiptBO
    {
    }
    [Serializable]
    [XmlRoot("Root")]
    public class ReceiptHeader : WorkflowBO
    {       
        [XmlElement("RCH_PK")]
        public long RCH_PK { get; set; }
        [XmlElement("RCH_DATE")]
        public DateTime RCH_DATE { get; set; }
        [XmlElement("RCH_NO")]
        public string RCH_NO { get; set; }
        [XmlElement("RCH_GROUP")]
        public byte RCH_GROUP { get; set; }
        [XmlElement("RCH_CATEGORY")]
        public byte RCH_CATEGORY { get; set; }
        [XmlElement("RCH_TYPE")]
        public byte RCH_TYPE { get; set; }
        [XmlElement("RCH_CUSTOMER")]
        public int RCH_CUSTOMER { get; set; }
        [XmlElement("RCH_CUSTOMER_TEXT")]
        public string RCH_CUSTOMER_TEXT { get; set; }
        [XmlElement("RCH_CUSTOMER_ACCOUNT")]
        public string RCH_CUSTOMER_ACCOUNT { get; set; }
        [XmlElement("RCH_MODE")]
        public byte RCH_MODE { get; set; }
        [XmlElement("RCH_BANK")]
        public string RCH_BANK { get; set; }
        [XmlElement("RCH_BANK_TEXT")]
        public string RCH_BANK_TEXT { get; set; }
        [XmlElement("RCH_INSTR_NO")]
        public string RCH_INSTR_NO { get; set; }
        [XmlElement("RCH_INSTR_DATE")]
        public string RCH_INSTR_DATE { get; set; }
        [XmlElement("RCH_INSTR_FAVOUR")]
        public string RCH_INSTR_FAVOUR { get; set; }
        [XmlElement("RCH_BANK_OF_CHEQUE")]
        public string RCH_BANK_OF_CHEQUE { get; set; }
        [XmlElement("RCH_BANK_CASH_ACCOUNT")]
        public string RCH_BANK_CASH_ACCOUNT { get; set; }
        [XmlElement("RCH_CURRENCY")]
        public int RCH_CURRENCY { get; set; }
        [XmlElement("RCH_CURRENCY_TEXT")]
        public string RCH_CURRENCY_TEXT { get; set; }
        [XmlElement("RCH_RCVD_AMOUNT")]
        public decimal RCH_RCVD_AMOUNT { get; set; }
        [XmlElement("RCH_DISCOUNT")]
        public string RCH_DISCOUNT { get; set; }
        [XmlElement("RCH_DISC_AMOUNT")]
        public decimal RCH_DISC_AMOUNT { get; set; }
        [XmlElement("RCH_TAX_AMOUNT")]
        public decimal RCH_TAX_AMOUNT { get; set; }
        [XmlElement("RCH_BANK_CHARGE")]
        public decimal RCH_BANK_CHARGE { get; set; }
        [XmlElement("RCH_BANK_CHARGE_CURR")]
        public string RCH_BANK_CHARGE_CURR { get; set; }
        [XmlElement("RCH_BANK_CHARGE_CURR_TEXT")]
        public string RCH_BANK_CHARGE_CURR_TEXT { get; set; }
        [XmlElement("RCH_BASE_CURR")]
        public int RCH_BASE_CURR { get; set; }
        [XmlElement("RCH_EXCHG_RATE")]
        public double RCH_EXCHG_RATE { get; set; }
        [XmlElement("RCH_RCVD_AMOUNT_BC")]
        public decimal RCH_RCVD_AMOUNT_BC { get; set; }
        [XmlElement("RCH_REMARKS")]
        public string RCH_REMARKS { get; set; }
        [XmlElement("RCH_PDC")]
        public byte RCH_PDC { get; set; }
        [XmlElement("RCH_BOUNCED")]
        public byte RCH_BOUNCED { get; set; }
        [XmlElement("RCH_HAS_JRNL_ENTRY")]
        public bool RCH_HAS_JRNL_ENTRY { get; set; }
        [XmlElement("RCH_STATUS")]
        public byte RCH_STATUS { get; set; }
        [XmlElement("RCH_ACTIVE")]
        public byte RCH_ACTIVE { get; set; }
        [XmlElement("RCH_DEPT")]
        public int RCH_DEPT { get; set; }
        [XmlElement("LAST_MOD_DT")]
        public DateTime LAST_MOD_DT { get; set; }
        [XmlElement("USER_PK")]
        public int USER_PK { get; set; }
        [XmlElement("RCH_BIZUNIT")]
        public int RCH_BIZUNIT { get; set; }
        [XmlElement("RCH_COMPANY")]
        public int RCH_COMPANY { get; set; }
        [XmlElement("RCH_DEL_STATUS")]
        public byte RCH_DEL_STATUS { get; set; }
        [XmlElement("RCH_OTHER_AMOUNT")]
        public decimal RCH_OTHER_AMOUNT { get; set; }

        [XmlElement("CBM_ACC_NO")]
        public string CBM_ACC_NO { get; set; }
        [XmlElement("CBM_BRANCH")]
        public string CBM_BRANCH { get; set; }
        [XmlElement("CBM_ACCOUNT")]
        public string CBM_ACCOUNT { get; set; }
        [XmlElement("RCH_RETURN_STATUS")]
        public byte RCH_RETURN_STATUS { get; set; }
        [XmlElement("RCH_RETURN_DATE")]
        public string RCH_RETURN_DATE { get; set; }
        [XmlElement("RCH_RETURN_REMARKS")]
        public string RCH_RETURN_REMARKS { get; set; }

        [XmlElement("ATL_ACTION")]
        public byte ATL_ACTION { get; set; }
        [XmlElement("ATL_APP_TYPE")]
        public string ATL_APP_TYPE { get; set; }

        [XmlElement("APT_CODE")]
        public string APT_CODE { get; set; }
        [XmlElement("AST_DOC_MODE")]
        public int AST_DOC_MODE { get; set; }
        [XmlElement("AST_VALUE")]
        public string AST_VALUE { get; set; }
        [XmlElement("WKF_FLAG")]
        public int WKF_FLAG { get; set; }

        [XmlElement("ReceiptTrxMapping")]
        public List<ReceiptTrxMapping> ReceiptTrxMapping { get; set; }

        [XmlElement("SuspDtl")]
        public List<ReceiptSuspendDetails> receiptSuspendDetails { get; set; }

    }
    [Serializable]
    public class ReceiptSuspendDetails
    {
        [XmlElement("RSC_PK")]
        public long RSC_PK { get; set; }
        [XmlElement("RSC_RECEIPT_HDR")]
        public long RSC_RECEIPT_HDR { get; set; }
        [XmlElement("RSC_VOUCHER_HDR")]
        public long RSC_VOUCHER_HDR { get; set; }
        [XmlElement("RSC_AMOUNT")]
        public decimal RSC_AMOUNT { get; set; }
    }
    [Serializable]
    public class ReceiptTrxMapping
    {
        [XmlElement("RCM_PK")]
        public long RCM_PK { get; set; }
        [XmlElement("RCM_RECEIPT_HDR")]
        public long RCM_RECEIPT_HDR { get; set; }
        [XmlElement("RCM_INVOICE_HDR")]
        public long RCM_INVOICE_HDR { get; set; }
        [XmlElement("RCM_SO_HDR")]
        public int RCM_SO_HDR { get; set; }
        [XmlElement("RCM_RCVD_AMOUNT")]
        public decimal RCM_RCVD_AMOUNT { get; set; }
        [XmlElement("RCM_DISC_AMOUNT")]
        public decimal RCM_DISC_AMOUNT { get; set; }
        [XmlElement("RCM_TAX_AMOUNT")]
        public decimal RCM_TAX_AMOUNT { get; set; }
        [XmlElement("RCM_ACTIVE")]
        public byte RCM_ACTIVE { get; set; }
        [XmlElement("RCM_BOUNCED")]
        public byte RCM_BOUNCED { get; set; }
        [XmlElement("RCM_OTHER_AMOUNT")]
        public decimal RCM_OTHER_AMOUNT { get; set; }
        [XmlElement("RCM_PREV_OTHER_AMOUNT")]
        public decimal RCM_PREV_OTHER_AMOUNT { get; set; }
        [XmlElement("RCM_ADJUST_AMOUNT")]
        public decimal RCM_ADJUST_AMOUNT { get; set; }
        [XmlElement("RCM_EXCESS_AMOUNT")]
        public decimal RCM_EXCESS_AMOUNT { get; set; }

        [XmlElement("ICH_NO")]
        public string ICH_NO { get; set; }
        [XmlElement("ICH_TYPE")]
        public byte ICH_TYPE { get; set; }
        [XmlElement("ICH_CUSTOMER")]
        public int ICH_CUSTOMER { get; set; }
        [XmlElement("ICH_CATEGORY")]
        public byte ICH_CATEGORY { get; set; }
        [XmlElement("ICH_GROUP")]
        public byte ICH_GROUP { get; set; }
        [XmlElement("ICH_DATE")]
        public DateTime ICH_DATE { get; set; }

        [XmlElement("CMP_LINE_COLOUR")]
        public string CMP_LINE_COLOUR { get; set; }
        [XmlElement("CMP_DISPLAY_CODE")]
        public string CMP_DISPLAY_CODE { get; set; }
        [XmlElement("ICH_AMOUNT_NET_TC")]
        public decimal ICH_AMOUNT_NET_TC { get; set; }
        [XmlElement("ICH_TAX_TC")]
        public decimal ICH_TAX_TC { get; set; }
        [XmlElement("ICH_SHIP_CHARGE")]
        public decimal ICH_SHIP_CHARGE { get; set; }
        [XmlElement("ICH_AMOUNT_RCVD_TC")]
        public decimal ICH_AMOUNT_RCVD_TC { get; set; }
        [XmlElement("ICH_BAL_AMOUNT")]
        public decimal ICH_BAL_AMOUNT { get; set; }
        [XmlElement("RCM_DN_AMOUNT")]
        public decimal RCM_DN_AMOUNT { get; set; }
        [XmlElement("ICH_TOTAL_AMOUNT")]
        public decimal ICH_TOTAL_AMOUNT { get; set; }
        [XmlElement("ICH_DISCOUNT_TOTAL")]
        public decimal ICH_DISCOUNT_TOTAL { get; set; }

        [XmlElement("RCM_SL_NO")]
        public int RCM_SL_NO { get; set; }

        [XmlElement("ReceiptSOMapping")]
        public List<ReceiptSOMapping> ReceiptSOMapping { get; set; }
        [XmlElement("AdjAllocation")]
        public List<AdjAllocation> AdjAllocation { get; set; }
        [XmlElement("DebitNoteAllocation")]
        public List<DebitNoteAllocation> DebitNoteAllocation { get; set; }
        [XmlElement("ReceiptTaxDetails")]
        public List<ReceiptTaxDetails> ReceiptTaxDetails { get; set; }
    }
    [Serializable]
    public class ReceiptSOMapping
    {
        [XmlElement("RSO_PK")]
        public long RSO_PK { get; set; }
        [XmlElement("RSO_RECEIPT_HDR")]
        public long RSO_RECEIPT_HDR { get; set; }
        [XmlElement("RSO_RECEIPT_TRX_MPG")]
        public long RSO_RECEIPT_TRX_MPG { get; set; }
        [XmlElement("RSO_SO_HDR")]
        public int RSO_SO_HDR { get; set; }
        [XmlElement("RSO_RECEIVED_AMOUNT")]
        public decimal RSO_RECEIVED_AMOUNT { get; set; }
        [XmlElement("RSO_ACTIVE")]
        public byte RSO_ACTIVE { get; set; }
        [XmlElement("RSO_BOUNCED")]
        public byte RSO_BOUNCED { get; set; }
        [XmlElement("RSO_OTHER_AMOUNT")]
        public decimal RSO_OTHER_AMOUNT { get; set; }
        [XmlElement("RSO_TAX_AMOUNT")]
        public decimal RSO_TAX_AMOUNT { get; set; }

        [XmlElement("SOH_NO")]
        public string SOH_NO { get; set; }
        [XmlElement("SOH_DATE")]
        public DateTime SOH_DATE { get; set; }
        [XmlElement("SOH_CURRENCY_TEXT")]
        public string SOH_CURRENCY_TEXT { get; set; }
        [XmlElement("SOH_NET_AMOUNT")]
        public decimal SOH_NET_AMOUNT { get; set; }
        [XmlElement("SOH_TOTAL_TAX")]
        public decimal SOH_TOTAL_TAX { get; set; }
        [XmlElement("SOH_TOTAL_DISCOUNT")]
        public decimal SOH_TOTAL_DISCOUNT { get; set; }
        [XmlElement("SOH_AMT_RECEIVED")]
        public decimal SOH_AMT_RECEIVED { get; set; }
        [XmlElement("SOH_BALANCE")]
        public decimal SOH_BALANCE { get; set; }
        [XmlElement("SOH_TOTAL_SHIP_CHARGE")]
        public decimal SOH_TOTAL_SHIP_CHARGE { get; set; }       

        [XmlElement("RSO_RCM_SL_NO")]
        public int RSO_RCM_SL_NO { get; set; }
        [XmlElement("RSO_SL_NO")]
        public int RSO_SL_NO { get; set; }
    }
    [Serializable]
    public class AdjAllocation
    {
        [XmlElement("RAD_PK")]
        public long RAD_PK { get; set; }
        [XmlElement("RAD_RECEIPT_TRX")]
        public long RAD_RECEIPT_TRX { get; set; }
        [XmlElement("RAD_ALCN_CDH")]
        public long RAD_ALCN_CDH { get; set; }
        [XmlElement("RAD_ALCN_RECEIPT_TRX")]
        public long RAD_ALCN_RECEIPT_TRX { get; set; }
        [XmlElement("RAD_AMOUNT")]
        public decimal RAD_AMOUNT { get; set; }
        [XmlElement("RAD_ACTIVE")]
        public byte RAD_ACTIVE { get; set; }

        [XmlElement("RAD_RCM_INVOICE_HDR")]
        public long RAD_RCM_INVOICE_HDR { get; set; }
        [XmlElement("RAD_RCM_SL_NO")]
        public int RAD_RCM_SL_NO { get; set; }

        [XmlElement("RAD_NO")]
        public string RAD_NO { get; set; }
        [XmlElement("RAD_DATE")]
        public DateTime RAD_DATE { get; set; }
        [XmlElement("RAD_TYPE")]
        public string RAD_TYPE { get; set; }
        [XmlElement("RAD_AMOUNT_BAL")]
        public decimal RAD_AMOUNT_BAL { get; set; }
        [XmlElement("RAD_AMOUNT_RCVD")]
        public decimal RAD_AMOUNT_RCVD { get; set; }
        [XmlElement("RAD_AMOUNT_PREV_RCVD")]
        public decimal RAD_AMOUNT_PREV_RCVD { get; set; }
        [XmlElement("RAD_AMOUNT_TOTAL")]
        public decimal RAD_AMOUNT_TOTAL { get; set; }
    }
    [Serializable]
    public class DebitNoteAllocation
    {
        [XmlElement("RNM_PK")]
        public long RNM_PK { get; set; }
        [XmlElement("RNM_RECEIPT_HDR")]
        public long RNM_RECEIPT_HDR { get; set; }
        [XmlElement("RNM_RECEIPT_TRX_MPG")]
        public long RNM_RECEIPT_TRX_MPG { get; set; }
        [XmlElement("RNM_CRDR_HDR")]
        public long RNM_CRDR_HDR { get; set; }
        [XmlElement("RNM_CRDR_MPG")]
        public long RNM_CRDR_MPG { get; set; }
        [XmlElement("RNM_PAID_AMOUNT")]
        public decimal RNM_PAID_AMOUNT { get; set; }
        [XmlElement("RNM_ADJ_AMOUNT")]
        public decimal RNM_ADJ_AMOUNT { get; set; }
        [XmlElement("RNM_ACTIVE")]
        public byte RNM_ACTIVE { get; set; }

        [XmlElement("RNM_CRDR_NO")]
        public string RNM_CRDR_NO { get; set; }
        [XmlElement("RNM_CRDR_DATE")]
        public DateTime RNM_CRDR_DATE { get; set; }
        [XmlElement("RNM_CRDR_CURRENCY_TEXT")]
        public string RNM_CRDR_CURRENCY_TEXT { get; set; }
        [XmlElement("RNM_CRDR_AMOUNT")]
        public decimal RNM_CRDR_AMOUNT { get; set; }
        [XmlElement("RNM_ALLOCATED_AMOUNT")]
        public decimal RNM_ALLOCATED_AMOUNT { get; set; }
        [XmlElement("RNM_BALANCE_AMOUNT")]
        public decimal RNM_BALANCE_AMOUNT { get; set; }

        [XmlElement("RNM_RCM_SL_NO")]
        public int RNM_RCM_SL_NO { get; set; }
    }
    [Serializable]
    public class ReceiptTaxDetails
    {
        [XmlElement("RDT_PK")]
        public int RDT_PK { get; set; }
        [XmlElement("RDT_RECEIPT_TRX")]
        public long RDT_RECEIPT_TRX { get; set; }
        [XmlElement("RDT_INVOICE_CUS_DTL")]
        public long RDT_INVOICE_CUS_DTL { get; set; }
        [XmlElement("RDT_CUS_SO_MPG")]
        public long RDT_CUS_SO_MPG { get; set; }
        [XmlElement("RDT_SOD")]
        public int RDT_SOD { get; set; }
        [XmlElement("RDT_TAX")]
        public int RDT_TAX { get; set; }
        [XmlElement("RDT_AMOUNT")]
        public decimal RDT_AMOUNT { get; set; }
        [XmlElement("RDT_TAX_CATEGORY")]
        public byte RDT_TAX_CATEGORY { get; set; }

        [XmlElement("RDT_TAX_PERC")]
        public double RDT_TAX_PERC { get; set; }
        [XmlElement("RDT_INVOICE_HDR")]
        public long RDT_INVOICE_HDR { get; set; }
        [XmlElement("RDT_SO_HDR")]
        public int RDT_SO_HDR { get; set; }
        [XmlElement("RDT_RCM_SL_NO")]
        public int RDT_RCM_SL_NO { get; set; }
        //For Advance Invoice
        [XmlElement("RDT_RSO_SL_NO")]
        public int RDT_RSO_SL_NO { get; set; }
    }

    [Serializable]
    [XmlRoot("Root")]
    public class InvoiceBO
    {
        [XmlElement("Invoice")]
        public List<InvoiceDetails> InvList { get; set; }
    }

    [Serializable]
    public class InvoiceDetails
    {
        [XmlElement("ICH_PK")]
        public long ICH_PK { get; set; }
        public int ICH_CUSTOMER { get; set; }
        public int ICH_TYPE { get; set; }
        public int ICH_CURRENCY { get; set; }
        public byte ICH_CATEGORY { get; set; }
        public byte ICH_GROUP { get; set; }
    }

    [Serializable]
    [XmlRoot("ROOT")]
    public class BrandBO
    {
        [XmlElement("DETAIL")]
        public List<BrandDetails> BrandList { get; set; }
    }

    [Serializable]
    public class BrandDetails
    {
        [XmlElement("CIM_PK")]
        public int CIM_PK { get; set; }
    }
}
