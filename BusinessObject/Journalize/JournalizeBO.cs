using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.Journalize
{
    #region Direct Payment Voucher
    [Serializable]
    [XmlRoot("Root")]
    public class FinTrxHeaderBO : WorkflowBO
    {
        [XmlElement("APT_CODE")]
        public string APT_CODE { get; set; }
        [XmlElement("AST_VALUE")]
        public string AST_VALUE { get; set; }

        [XmlElement("FTH_PK")]
        public long FTH_PK { get; set; }
        [XmlElement("FTH_DATE")]
        public DateTime FTH_DATE { get; set; }
        [XmlElement("FTH_VOUCHER_NO")]
        public string FTH_VOUCHER_NO { get; set; }
        [XmlElement("FTH_REF_TYPE")]
        public string FTH_REF_TYPE { get; set; }
        [XmlElement("FTH_REF_PK")]
        public long FTH_REF_PK { get; set; }
        [XmlElement("FTH_REF_DATE")]
        public DateTime FTH_REF_DATE { get; set; }
        [XmlElement("FTH_REF_NO")]
        public string FTH_REF_NO { get; set; }
        [XmlElement("FTH_PARTY_NAME")]
        public string FTH_PARTY_NAME { get; set; }
        [XmlElement("FTH_NARRATION")]
        public string FTH_NARRATION { get; set; }
        [XmlElement("FTH_TRX_CURR")]
        public int FTH_TRX_CURR { get; set; }
        [XmlElement("FTH_EXCHG_RATE")]
        public double FTH_EXCHG_RATE { get; set; }
        [XmlElement("FTH_BASE_CURR")]
        public int FTH_BASE_CURR { get; set; }
        [XmlElement("FTH_FIN_YEAR")]
        public int FTH_FIN_YEAR { get; set; }
        [XmlElement("FTH_REMARKS")]
        public string FTH_REMARKS { get; set; }
        [XmlElement("FTH_IS_JRNLD")]
        public bool FTH_IS_JRNLD { get; set; }
        [XmlElement("FTH_STATUS")]
        public int FTH_STATUS { get; set; }
        [XmlElement("ACTIVE")]
        public int FTH_ACTIVE { get; set; }
        [XmlElement("WKF_FLAG")]
        public int WKF_FLAG { get; set; }
        [XmlElement("FTH_WHT_CERT_AUTO")]
        public int FTH_WHT_CERT_AUTO { get; set; }
        //[XmlElement("FTH_IS_DELETED")]
        //public int FTH_IS_DELETED { get; set; }
        //
        //FTH_DEL_REASON
        //FTH_TASK1_BY
        //FTH_TASK1_DT
        //FTH_TASK2_BY
        //FTH_TASK2_DT
        //FTH_TASK3_BY
        //FTH_TASK3_DT
        //FTH_TASK4_BY
        //FTH_TASK4_DT
        [XmlElement("FTH_CRTD_BY")]
        public int FTH_CRTD_BY { get; set; }
        [XmlElement("FTH_CRTD_DT")]
        public DateTime FTH_CRTD_DT { get; set; }
        [XmlElement("FTH_MOD_BY")]
        public int FTH_MOD_BY { get; set; }
        [XmlElement("LAST_MOD_DT")]
        public DateTime FTH_MOD_DT { get; set; }
        [XmlElement("FTH_DEPT")]
        public int FTH_DEPT { get; set; }
        [XmlElement("BIZUNIT")]
        public int FTH_BIZUNIT { get; set; }
        [XmlElement("FTH_COMPANY")]
        public int FTH_COMPANY { get; set; }
        [XmlElement("FTH_TRX_DATE")]
        public DateTime FTH_TRX_DATE { get; set; }
        [XmlElement("USER_PK")]
        public int USER_PK { get; set; }
        [XmlElement("FTH_TAX_DATE")]
        public string FTH_TAX_DATE { get; set; }
        [XmlElement("IS_CANCEL")]
        public string IS_CANCEL { get; set; }

        [XmlElement("RECONCILIATION")]
        public string RECONCILIATION { get; set; }
        [XmlElement("PDCFLAG")]
        public string PDCFLAG { get; set; }
        [XmlElement("BOUNCEFLAG")]
        public string BOUNCEFLAG { get; set; }
        [XmlElement("UPDATE_INV_STOCK")]
        public string UPDATE_INV_STOCK { get; set; }
        [XmlElement("PPC_RECONCILIATION")]
        public string PPC_RECONCILIATION { get; set; }
        [XmlElement("PDC_RECONCILIATION")]
        public string PDC_RECONCILIATION { get; set; }

        //FTH_VERSION
        //
        [XmlElement("FTH_PDC")]
        public byte FTH_PDC { get; set; }
        //FTH_BOUNCED
        [XmlElement("Detail")]
        public List<FinTrxDetailsBO> FinTrxDetails { get; set; }
        [XmlElement("TaxHDR")]
        public List<FinPaymentVndTaxHeaderBO> FinPaymentVndTaxHdr { get; set; }


    }
    [Serializable]
    public class FinCostCenterBO
    {
        [XmlElement("FTD_SL_NO")]
        public long FTD_SL_NO { get; set; }
        [XmlElement("FTD_PK")]
        public long FTD_PK { get; set; }
        [XmlElement("FTD_FTR_PK")]
        public long FTD_FTR_PK { get; set; }
        [XmlElement("FTD_CNM_PK")]
        public int FTD_CNM_PK { get; set; }
        [XmlElement("FTD_AMT_TC")]
        public decimal FTD_AMT_TC { get; set; }
        [XmlElement("FTD_AMT_BC")]
        public decimal FTD_AMT_BC { get; set; }
    }
    [Serializable]
    public class PaymentVndTadHdrBO
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
        public int WTH_FORM_NO { get; set; }
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
        public int WTH_PUR_INVOICE { get; set; }
        [XmlElement("WTH_BRANCH")]
        public int WTH_BRANCH { get; set; }
        [XmlElement("WTH_BRANCH_NAME")]
        public string WTH_BRANCH_NAME { get; set; }
        [XmlElement("WTH_VENDOR")]
        public int WTH_VENDOR { get; set; }
        [XmlElement("WTH_REFUND_DATE")]
        public DateTime WTH_REFUND_DATE { get; set; }
        [XmlElement("WTH_PAYMENT_TYPE")]
        public int WTH_PAYMENT_TYPE { get; set; }
      
        [XmlElement("WTH_CERT_NO")]
        public string WTH_CERT_NO { get; set; }
    }
    [Serializable]
    public class FinTrxDetailsBO
    {
        [XmlElement("FTR_SL_NO")]
        public long FTR_SL_NO { get; set; }
        [XmlElement("FTR_PK")]
        public long FTR_PK { get; set; }
        [XmlElement("FTR_TRX_HDR")]
        public long FTR_TRX_HDR { get; set; }
        [XmlElement("FTR_SEQUENCE")]
        public short FTR_SEQUENCE { get; set; }
        [XmlElement("FTR_ACCOUNT")]
        public int? FTR_ACCOUNT { get; set; }
        [XmlElement("FTR_ACC_SUB_TYPE")]
        public int FTR_ACC_SUB_TYPE { get; set; }
        [XmlElement("FTR_PAYMENT_MODE")]
        public string FTR_PAYMENT_MODE { get; set; }
        [XmlElement("FTR_INSTR_NO")]
        public string FTR_INSTR_NO { get; set; }
        [XmlElement("FTR_INSTR_DATE")]
        public string FTR_INSTR_DATE { get; set; }
        [XmlElement("FTR_INSTR_FAVOUR")]
        public string FTR_INSTR_FAVOUR { get; set; }
        [XmlElement("FTR_NARRATION")]
        public string FTR_NARRATION { get; set; }
        [XmlElement("FTR_DR_AMT_TC")]
        public decimal FTR_DR_AMT_TC { get; set; }
        [XmlElement("FTR_CR_AMT_TC")]
        public decimal FTR_CR_AMT_TC { get; set; }
        [XmlElement("FTR_DR_AMT_BC")]
        public decimal FTR_DR_AMT_BC { get; set; }
        [XmlElement("FTR_CR_AMT_BC")]
        public decimal FTR_CR_AMT_BC { get; set; }
        [XmlElement("FTR_TYPE")]
        public string FTR_TYPE { get; set; }
        [XmlElement("FTR_TYPE_PK")]
        public int FTR_TYPE_PK { get; set; }
        [XmlElement("FTR_CLEAR_DATE")]
        public String FTR_CLEAR_DATE { get; set; }
        [XmlElement("FTR_IS_RECONCILED")]
        public bool FTR_IS_RECONCILED { get; set; }
        [XmlElement("FTR_REMARKS")]
        public string FTR_REMARKS { get; set; }
        [XmlElement("FTR_ACTIVE")]
        public byte FTR_ACTIVE { get; set; }
        [XmlElement("FTR_CRTD_BY")]
        public int FTR_CRTD_BY { get; set; }
        [XmlElement("FTR_CRTD_DT")]
        public DateTime FTR_CRTD_DT { get; set; }
        [XmlElement("FTR_MOD_BY")]
        public int FTR_MOD_BY { get; set; }
        [XmlElement("FTR_MOD_DT")]
        public DateTime FTR_MOD_DT { get; set; }
        [XmlElement("FTR_DEPT")]
        public int FTR_DEPT { get; set; }
        [XmlElement("FTR_BIZUNIT")]
        public int FTR_BIZUNIT { get; set; }
        [XmlElement("FTR_EXCHG_RATE")]
        public double FTR_EXCHG_RATE { get; set; }
        [XmlElement("FTR_ENTRY_MODE")]
        public string FTR_ENTRY_MODE { get; set; }
        [XmlElement("FTR_EXCHG_RATE_YE")]
        public string FTR_EXCHG_RATE_YE { get; set; }
        [XmlElement("FTR_PDC")]
        public byte FTR_PDC { get; set; }
        [XmlElement("FTR_IS_BANK_CHARGE")]
        public byte FTR_IS_BANK_CHARGE { get; set; }

        [XmlElement("FTR_VENDOR_CODE")]
        public string FTR_VENDOR_CODE { get; set; }

        [XmlElement("FTR_INV_DATE")]
        public string FTR_INV_DATE { get; set; }

        [XmlElement("FTR_INV_NO")]
        public string FTR_INV_NO { get; set; }

        [XmlElement("FTR_REF_PO_NUMBER")]
        public string FTR_REF_PO_NUMBER { get; set; }

        [XmlElement("FTR_AMT_BFR_VAT")]
        public decimal FTR_AMT_BFR_VAT { get; set; }
       
        [XmlElement("CocDTL")]
        public List<FinCostCenterBO> CostCenterList { get; set; }

    }
    [Serializable]
    public class FinPaymentVndTaxHeaderBO
    {
        [XmlElement("WTH_PK")]
        public long WTH_PK { get; set; }
        //[XmlElement("WTH_PAYMENT_HDR")]
        //public long WTH_PAYMENT_HDR { get; set; }
        [XmlElement("WTH_TYPE")]
        public byte WTH_TYPE { get; set; }
        [XmlElement("WTH_TAX")]
        public int? WTH_TAX { get; set; }
        [XmlElement("WTH_TAX_CATEGORY")]
        public byte WTH_TAX_CATEGORY { get; set; }
        [XmlElement("WTH_NAME")]
        public string WTH_NAME { get; set; }
        [XmlElement("WTH_AMOUNT")]
        public decimal WTH_AMOUNT { get; set; }
        [XmlElement("WTH_TAX_AMT")]
        public decimal WTH_TAX_AMT { get; set; }
        [XmlElement("WTH_DESC")]
        public string WTH_DESC { get; set; }
        [XmlElement("WTH_FORM_NO")]
        public int? WTH_FORM_NO { get; set; }
        [XmlElement("WTH_PARTY_NAME")]
        public string WTH_PARTY_NAME { get; set; }
        [XmlElement("WTH_ADDRESS")]
        public string WTH_ADDRESS { get; set; }
        [XmlElement("WTH_TAX_ID")]
        public string WTH_TAX_ID { get; set; }
        [XmlElement("WTH_TRX_HDR")]
        public long? WTH_TRX_HDR { get; set; }
        [XmlElement("WTH_CATEGORY")]
        public byte WTH_CATEGORY { get; set; }
        [XmlElement("WTH_TAX_INV_NO")]
        public string WTH_TAX_INV_NO { get; set; }
        [XmlElement("WTH_TAX_DATE")]
        public DateTime? WTH_TAX_DATE { get; set; }
        [XmlElement("WTH_BRANCH_TYPE")]
        public byte WTH_BRANCH_TYPE { get; set; }
        [XmlElement("WTH_BRANCH_TEXT")]
        public string WTH_BRANCH_TEXT { get; set; }
        [XmlElement("WTH_ITEM_TEXT")]
        public string WTH_ITEM_TEXT { get; set; }
        //[XmlElement("WTH_PUR_INVOICE")]
        //public long WTH_PUR_INVOICE { get; set; }
        [XmlElement("WTH_BRANCH")]
        public int? WTH_BRANCH { get; set; }
        [XmlElement("WTH_BRANCH_NAME")]
        public string WTH_BRANCH_NAME { get; set; }
        [XmlElement("WTH_VENDOR")]
        public string WTH_VENDOR { get; set; }
        [XmlElement("WTH_REFUND_DATE")]
        public string WTH_REFUND_DATE { get; set; }
        [XmlElement("WTH_PAYMENT_TYPE")]
        public byte WTH_PAYMENT_TYPE { get; set; }
        [XmlElement("WTH_INV_RECEIVED")]
        public byte WTH_INV_RECEIVED { get; set; }
        [XmlElement("WTH_CERT_NO")]
        public string WTH_CERT_NO { get; set; }
    }

    #endregion
    [Serializable]
    [XmlRoot("ROOT")]
    public class VoucheData
    {
        [XmlElement("Voucher_Details")]
        public List<VoucherTransactionBO> VoucherTransactionList { get; set; }
    }


    [Serializable]
    public class VoucherTransactionBO
    {
        [XmlElement("SLNO")]
        public string SLNO { get; set; }

        [XmlElement("COA_CODE")]
        public string COA_CODE { get; set; }

        [XmlElement("ACCOUNT")]
        public int ACCOUNT { get; set; }
        [XmlElement("COA_PK")]
        public int COA_PK { get; set; }
        [XmlElement("ACCOUNT_TEXT")]
        public string ACCOUNT_TEXT { get; set; }

        [XmlElement("ACCOUNT_CODE")]
        public string ACCOUNT_CODE { get; set; }

        [XmlElement("SUB_TYPE_PK")]
        public int SUB_TYPE_PK { get; set; }
        [XmlElement("ACC_SUB_TYPE")]
        public string ACC_SUB_TYPE { get; set; }
        [XmlElement("PAYMENT_MODE_TEXT")]
        public string PAYMENT_MODE_TEXT { get; set; }
        [XmlElement("CFG_PK")]
        public int CFG_PK { get; set; }
        [XmlElement("PAYMENT_MODE")]
        public int PAYMENT_MODE { get; set; }
        [XmlElement("NARRATION")]
        public string NARRATION { get; set; }
        [XmlElement("DR_AMT_BC")]
        public decimal DR_AMT_BC { get; set; }
        [XmlElement("DR_AMT_TC")]
        public decimal DR_AMT_TC { get; set; }
        [XmlElement("CR_AMT_BC")]
        public decimal CR_AMT_BC { get; set; }
        [XmlElement("CR_AMT_TC")]
        public decimal CR_AMT_TC { get; set; }
        [XmlElement("INSTR_NO")]
        public string INSTR_NO { get; set; }
        [XmlElement("INSTR_DATE")]
        public string INSTR_DATE { get; set; }
        [XmlElement("INSTR_FAVOUR")]
        public string INSTR_FAVOUR { get; set; }
        [XmlElement("PDC")]
        public string PDC { get; set; }
        [XmlElement("IS_BANK_CHARGE")]
        public byte IS_BANK_CHARGE { get; set; }

        [XmlElement("VENDOR_CODE")]
        public string VENDOR_CODE { get; set; }

        [XmlElement("DATE_INVOICE")]
        public string DATE_INVOICE { get; set; }

        [XmlElement("INVOICE_NO")]
        public string INVOICE_NO { get; set; }

        [XmlElement("REFER_PUR")]
        public string REFER_PUR { get; set; }

        [XmlElement("FTR_AMT_BFR_VAT")]
        public decimal FTR_AMT_BFR_VAT { get; set; }


        [XmlElement("COC_Details")]
        public List<CostCenterMapBO> CostCenterMapList { get; set; }
    }

    [Serializable]
    public class CostCenterMapBO
    {
        [XmlElement("CNM_PK")]
        public int CNM_PK { get; set; }
        [XmlElement("FCM_PK")]
        public int FCM_PK { get; set; }
        [XmlElement("AMT_TC")]
        public decimal AMT_TC { get; set; }
        [XmlElement("AMT_BC")]
        public decimal AMT_BC { get; set; }
        [XmlElement("COST_CENTER_TEXT")]
        public string COST_CENTER_TEXT { get; set; }
    }



    class JournalizeBO
    {

    }
    [Serializable]
    [XmlRoot("ROOT")]
    public class VoucherGainLossHeader
    {
        [XmlElement("FTH_PK")]
        public long FTH_PK { get; set; }
        [XmlElement("FTH_EXCHG_RATE")]
        public double FTH_EXCHG_RATE { get; set; }
        [XmlElement("DETAILS")]
        public List<VoucherGainLossDetails> GainLossDetails { get; set; }
    }

    [Serializable]
    public class VoucherGainLossDetails
    {
        [XmlElement("FTR_PK")]
        public long FTR_PK { get; set; }
        [XmlElement("FTR_ACCOUNT")]
        public int FTR_ACCOUNT { get; set; }
        [XmlElement("FTR_DR_AMT_TC")]
        public decimal FTR_DR_AMT_TC { get; set; }
        [XmlElement("FTR_CR_AMT_TC")]
        public decimal FTR_CR_AMT_TC { get; set; }
        [XmlElement("FTR_DR_AMT_BC")]
        public decimal FTR_DR_AMT_BC { get; set; }
        [XmlElement("FTR_CR_AMT_BC")]
        public decimal FTR_CR_AMT_BC { get; set; }
        [XmlElement("FTR_ACTIVE")]
        public byte FTR_ACTIVE { get; set; }
        [XmlElement("FTR_EXCHG_RATE")]
        public double FTR_EXCHG_RATE { get; set; }
        [XmlElement("FTR_ENTRY_MODE")]
        public byte FTR_ENTRY_MODE { get; set; }
    }

    #region JV Template
    [Serializable]
    [XmlRoot("Root")]
    public class JVTemplateBO
    {
        [XmlElement("VLH_PK")]
        public int VLH_PK { get; set; }
        [XmlElement("VLH_NAME")]
        public string VLH_NAME { get; set; }
        [XmlElement("VLH_DESC")]
        public string VLH_DESC { get; set; }
        [XmlElement("VLH_TYPE")]
        public int VLH_TYPE { get; set; }
        [XmlElement("ACTIVE")]
        public int ACTIVE { get; set; }
        [XmlElement("USER_PK")]
        public int USER_PK { get; set; }
        [XmlElement("VLH_MOD_ON")]
        public string LAST_MOD_DT { get; set; }
        [XmlElement("Detail")]
        public List<JVTemplateDetails> Detail { get; set; }
    }
    [Serializable]
    public class JVTemplateDetails
    {
        [XmlElement("VLD_PK")]
        public int VLD_PK { get; set; }
        [XmlElement("VLD_MODE")]
        public int VLD_MODE { get; set; }
        [XmlElement("VLD_ACCOUNT")]
        public int VLD_ACCOUNT { get; set; }
        [XmlElement("VLD_SEQUENCE")]
        public short VLD_SEQUENCE { get; set; }
        [XmlElement("VLD_REF_TYPE")]
        public string VLD_REF_TYPE { get; set; }
        [XmlElement("VLD_REF_TYPE_PK")]
        public int VLD_REF_TYPE_PK { get; set; }
    }

    public class JVTemplateDetailsList
    {
        public int SLNO { get; set; }
        public int VLD_PK { get; set; }
        public int VLD_MODE { get; set; }
        public int VLD_ACCOUNT { get; set; }
        public string COA_CODE { get; set; }
        public string COA_NAME { get; set; }
        public short VLD_SEQUENCE { get; set; }
        public string VLD_REF_TYPE { get; set; }
        public int VLD_REF_TYPE_PK { get; set; }
    }

    #endregion

    #region BankReconciliation
    [Serializable]
    [XmlRoot("ROOT")]
    public class FinTrxDetails
    {
        [XmlElement("FIN_TRX")]
        public List<FinTransactions> FinTrx { get; set; }

    }

    [Serializable]
    public class FinTransactions
    {
        [XmlElement("FTH_PK")]
        public long FTH_PK { get; set; }
        [XmlElement("FTH_REF_PK")]
        public long FTH_REF_PK { get; set; }
        [XmlElement("FTH_DATE")]
        public DateTime FTH_DATE { get; set; }
        [XmlElement("FTH_REF_TYPE")]
        public string FTH_REF_TYPE { get; set; }
        [XmlElement("FTH_COMPANY")]
        public string FTH_COMPANY { get; set; }
        [XmlElement("FTH_VOUCHER_NO")]
        public string FTH_VOUCHER_NO { get; set; }
        [XmlElement("FTH_PARTY_NAME")]
        public string FTH_PARTY_NAME { get; set; }
        [XmlElement("FTR_INSTR_NO")]
        public string FTR_INSTR_NO { get; set; }
        [XmlElement("FTR_INSTR_DATE")]
        public DateTime? FTR_INSTR_DATE { get; set; }
        [XmlElement("FTR_DR_AMT_BC")]
        public decimal FTR_DR_AMT_BC { get; set; }
        [XmlElement("FTR_CR_AMT_BC")]
        public decimal FTR_CR_AMT_BC { get; set; }
        [XmlElement("FTR_CLEAR_DATE")]
        public DateTime? FTR_CLEAR_DATE { get; set; }

        [XmlElement("FTR_IS_RECONCILED")]
        public int FTR_IS_RECONCILED { get; set; }
        [XmlElement("FTR_TYPE")]
        public string FTR_TYPE { get; set; }
        [XmlElement("FTR_TYPE_PK")]
        public int FTR_TYPE_PK { get; set; }
        [XmlElement("FTR_PK")]
        public long FTR_PK { get; set; }

        [XmlElement("FTR_MOD_BY")]
        public long FTR_MOD_BY { get; set; }

        [XmlElement("FTR_MOD_DT")]
        public DateTime? FTR_MOD_DT { get; set; }
        


        public DateTime? FROM_DATE { get; set; }
        public DateTime? TO_DATE { get; set; }
        [XmlElement("BIZUNIT")]
        public int BIZUNIT { get; set; }
        public string SortBy { get; set; }
        public string SortDirection { get; set; }
        public int FTR_ACTIVE { get; set; }


    }
    #endregion

    public enum JournalTypeEnum
    {
        Voucher = 1,
        Reverse = 2,
        Return = 3
    }

    [Serializable]
    public class CostCenterDetails
    {
        public string CONTROL_ID { get; set; }
        public decimal FTD_AMT_TC { get; set; }
        public decimal FTD_AMT_BC { get; set; }
        public string FCM_COST_CENTER_TEXT { get; set; }
        public int FCM_CNM_PK { get; set; }
        public long FTR_PK { get; set; }
    }

    [Serializable]
    public class CostCenterMissmatch
    {
        public string AMT_TC_CONTROL_ID { get; set; }
    }

}
