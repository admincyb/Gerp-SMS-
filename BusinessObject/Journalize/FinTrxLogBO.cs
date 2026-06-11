using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.Journalize
{
    [Serializable]
    [XmlRoot("Root")]
    public class FinTrxLogBO
    {
        [XmlElement("TrxLog")]
        public List<FinTrxHeader> TrxLog { get; set; }
    }
    [Serializable]
    public class FinTrxHeader
    {
        [XmlElement("SL_NO")]
        public int SL_NO { get; set; }
        [XmlElement("FTH_PK")]
        public long FTH_PK { get; set; }
        [XmlElement("DTL")]
        public List<FinTrxDetail> DTL { get; set; }
        [XmlElement("FTH_DATE")]
        public string FTH_DATE { get; set; }
        [XmlElement("FTH_VERSION")]
        public int FTH_VERSION { get; set; }
        [XmlElement("FTH_VOUCHER_NO")]
        public string FTH_VOUCHER_NO { get; set; }
        [XmlElement("FTH_REF_TYPE")]
        public string FTH_REF_TYPE { get; set; }
        [XmlElement("FTH_REF_PK")]
        public string FTH_REF_PK { get; set; }
        [XmlElement("FTH_REF_DATE")]
        public string FTH_REF_DATE { get; set; }
        [XmlElement("FTH_REF_NO")]
        public string FTH_REF_NO { get; set; }
        [XmlElement("FTH_PARTY_NAME")]
        public string FTH_PARTY_NAME { get; set; }
        [XmlElement("FTH_NARRATION")]
        public string FTH_NARRATION { get; set; }
        [XmlElement("FTH_TRX_CURR")]
        public int FTH_TRX_CURR { get; set; }
        [XmlElement("FTH_TRX_CURR_TEXT")]
        public string FTH_TRX_CURR_TEXT { get; set; }
        [XmlElement("FTH_EXCHG_RATE")]
        public double FTH_EXCHG_RATE { get; set; }
        [XmlElement("FTH_BASE_CURR")]
        public int FTH_BASE_CURR { get; set; }
        [XmlElement("FTH_BASE_CURR_TEXT")]
        public string FTH_BASE_CURR_TEXT { get; set; }
        [XmlElement("FTH_FIN_YEAR")]
        public int FTH_FIN_YEAR { get; set; }
        [XmlElement("FTH_REMARKS")]
        public string FTH_REMARKS { get; set; }
        [XmlElement("FTH_IS_JRNLD")]
        public int FTH_IS_JRNLD { get; set; }
        [XmlElement("FTH_DEL_REASON")]
        public string FTH_DEL_REASON { get; set; }
        [XmlElement("FTH_TASK1_BY")]
        public string FTH_TASK1_BY { get; set; }
        [XmlElement("FTH_TASK1_DT")]
        public string FTH_TASK1_DT { get; set; }
        [XmlElement("FTH_TASK2_BY")]
        public string FTH_TASK2_BY { get; set; }
        [XmlElement("FTH_TASK2_DT")]
        public string FTH_TASK2_DT { get; set; }
        [XmlElement("FTH_TASK3_BY")]
        public string FTH_TASK3_BY { get; set; }
        [XmlElement("FTH_TASK3_DT")]
        public string FTH_TASK3_DT { get; set; }
        [XmlElement("FTH_TASK4_BY")]
        public string FTH_TASK4_BY { get; set; }
        [XmlElement("FTH_TASK4_DT")]
        public string FTH_TASK4_DT { get; set; }
        [XmlElement("FTH_DEPT")]
        public int FTH_DEPT { get; set; }
        [XmlElement("FTH_DEPT_TEXT")]
        public string FTH_DEPT_TEXT { get; set; }
        [XmlElement("FTH_COMPANY")]
        public int FTH_COMPANY { get; set; }
        [XmlElement("FTH_COMPANY_TEXT")]
        public string FTH_COMPANY_TEXT { get; set; }

        [XmlElement("FTH_TRX_DATE")]
        public string FTH_TRX_DATE { get; set; }
        [XmlElement("FTH_PDC")]
        public int FTH_PDC { get; set; }
        [XmlElement("FTH_BOUNCED")]
        public int FTH_BOUNCED { get; set; }
        [XmlElement("FTH_AUDIT_VERSION")]
        public int FTH_AUDIT_VERSION { get; set; }
        [XmlElement("FTH_ACTIVITY")]
        public string FTH_ACTIVITY { get; set; }
        [XmlElement("FTH_MOD_BY_TEXT")]
        public string FTH_MOD_BY_TEXT { get; set; }
        [XmlElement("FTH_DATE_TM")]
        public string FTH_DATE_TM { get; set; }

        [XmlElement("FTR_VERSION_ISMOD")]
        public int FTR_VERSION_ISMOD { get; set; }
        [XmlElement("FTH_VERSION_ISMOD")]
        public int FTH_VERSION_ISMOD { get; set; }
        [XmlElement("FTH_VOUCHER_NO_ISMOD")]
        public int FTH_VOUCHER_NO_ISMOD { get; set; }
        [XmlElement("FTH_REF_DATE_ISMOD")]
        public int FTH_REF_DATE_ISMOD { get; set; }
        [XmlElement("FTH_REF_TYPE_ISMOD")]
        public int FTH_REF_TYPE_ISMOD { get; set; }
        [XmlElement("FTH_REF_PK_ISMOD")]
        public int FTH_REF_PK_ISMOD { get; set; }
        [XmlElement("FTH_PARTY_NAME_ISMOD")]
        public int FTH_PARTY_NAME_ISMOD { get; set; }
        [XmlElement("FTH_NARRATION_ISMOD")]
        public int FTH_NARRATION_ISMOD { get; set; }
        [XmlElement("FTH_TRX_CURR_ISMOD")]
        public int FTH_TRX_CURR_ISMOD { get; set; }
        [XmlElement("FTH_EXCHG_RATE_ISMOD")]
        public int FTH_EXCHG_RATE_ISMOD { get; set; }
        [XmlElement("FTH_BASE_CURR_ISMOD")]
        public int FTH_BASE_CURR_ISMOD { get; set; }
        [XmlElement("FTH_FIN_YEAR_ISMOD")]
        public int FTH_FIN_YEAR_ISMOD { get; set; }
        [XmlElement("FTH_REMARKS_ISMOD")]
        public int FTH_REMARKS_ISMOD { get; set; }
        [XmlElement("FTH_DATE_ISMOD")]
        public int FTH_DATE_ISMOD { get; set; }
        [XmlElement("FTH_REF_NO_ISMOD")]
        public int FTH_REF_NO_ISMOD { get; set; }

    }
    [Serializable]
    public class FinTrxDetail
    {
        [XmlElement("FTR_PK")]
        public long FTR_PK { get; set; }
        [XmlElement("FTR_VERSION")]
        public int FTR_VERSION { get; set; }
        [XmlElement("FTR_TRX_HDR")]
        public int FTR_TRX_HDR { get; set; }
        [XmlElement("FTR_SEQUENCE")]
        public int FTR_SEQUENCE { get; set; }
        [XmlElement("FTR_ACCOUNT_CODE")]
        public string FTR_ACCOUNT_CODE { get; set; }
        [XmlElement("FTR_ACCOUNT_TEXT")]
        public string FTR_ACCOUNT_TEXT { get; set; }
        [XmlElement("FTR_ACC_SUB_TYPE")]
        public int FTR_ACC_SUB_TYPE { get; set; }
        [XmlElement("FTR_PAYMENT_MODE")]
        public int FTR_PAYMENT_MODE { get; set; }
        [XmlElement("FTR_PAYMENT_MODE_TEXT")]
        public string FTR_PAYMENT_MODE_TEXT { get; set; }
        [XmlElement("FTR_INSTR_NO")]
        public string FTR_INSTR_NO { get; set; }
        [XmlElement("FTR_INSTR_FAVOUR")]
        public string FTR_INSTR_FAVOUR { get; set; }
        [XmlElement("FTR_NARRATION")]
        public string FTR_NARRATION { get; set; }
        [XmlElement("FTR_DR_AMT_TC")]
        public double FTR_DR_AMT_TC { get; set; }
        [XmlElement("FTR_CR_AMT_TC")]
        public double FTR_CR_AMT_TC { get; set; }
        [XmlElement("FTR_DR_AMT_BC")]
        public double FTR_DR_AMT_BC { get; set; }
        [XmlElement("FTR_CR_AMT_BC")]
        public double FTR_CR_AMT_BC { get; set; }
        [XmlElement("FTR_TYPE")]
        public string FTR_TYPE { get; set; }
        [XmlElement("FTR_TYPE_PK")]
        public int FTR_TYPE_PK { get; set; }
        [XmlElement("FTR_IS_RECONCILED")]
        public int FTR_IS_RECONCILED { get; set; }
        [XmlElement("FTR_REMARKS")]
        public string FTR_REMARKS { get; set; }
        [XmlElement("FTR_ACTIVE")]
        public int FTR_ACTIVE { get; set; }
        [XmlElement("FTR_CRTD_BY")]
        public int FTR_CRTD_BY { get; set; }
        [XmlElement("FTR_CRTD_DT")]
        public string FTR_CRTD_DT { get; set; }
        [XmlElement("FTR_MOD_BY")]
        public int FTR_MOD_BY { get; set; }
        [XmlElement("FTR_MOD_DT")]
        public string FTR_MOD_DT { get; set; }
        [XmlElement("FTR_DEPT")]
        public int FTR_DEPT { get; set; }
        [XmlElement("FTR_DEPT_TEXT")]
        public string FTR_DEPT_TEXT { get; set; }
        [XmlElement("FTR_BIZUNIT")]
        public int FTR_BIZUNIT { get; set; }

        [XmlElement("FTR_ENTRY_MODE")]
        public int FTR_ENTRY_MODE { get; set; }
        [XmlElement("FTR_EXCHG_RATE")]
        public double FTR_EXCHG_RATE { get; set; }
        [XmlElement("FTR_PDC")]
        public int FTR_PDC { get; set; }
        [XmlElement("FTR_IS_BANK_CHARGE")]
        public int FTR_IS_BANK_CHARGE { get; set; }
        [XmlElement("FTR_EXCHG_RATE_YE")]
        public double FTR_EXCHG_RATE_YE { get; set; }
        [XmlElement("FTR_DATE")]
        public string FTR_DATE { get; set; }
        [XmlElement("FTR_ACCOUNT_ISMOD")]
        public int FTR_ACCOUNT_ISMOD { get; set; }
        [XmlElement("FTR_PAYMENT_MODE_ISMOD")]
        public int FTR_PAYMENT_MODE_ISMOD { get; set; }
        [XmlElement("FTR_NARRATION_ISMOD")]
        public int FTR_NARRATION_ISMOD { get; set; }
        [XmlElement("FTR_CR_AMT_TC_ISMOD")]
        public int FTR_CR_AMT_TC_ISMOD { get; set; }
        [XmlElement("FTR_DR_AMT_BC_ISMOD")]
        public int FTR_DR_AMT_BC_ISMOD { get; set; }
        [XmlElement("FTR_TYPE_TEXT")]
        public string FTR_TYPE_TEXT { get; set; }
        [XmlElement("FTR_TYPE_PK_ISMOD")]
        public int FTR_TYPE_PK_ISMOD { get; set; }

        [XmlElement("FTR_COST_BIT")]
        public int FTR_COST_BIT { get; set; }
        [XmlElement("FTR_AUDIT_VERSION")]
        public int FTR_AUDIT_VERSION { get; set; }
        [XmlElement("CostCenter")]
        public List<CostCenter> CostCenter { get; set; }

    }
    [Serializable]
    public class CostCenter
    {
        [XmlElement("FTR_FTD_AMT_BC")]
        public double FTR_FTD_AMT_BC { get; set; }
        [XmlElement("FTR_FTD_AMT_BC_ISMOD")]
        public int FTR_FTD_AMT_BC_ISMOD { get; set; }
        [XmlElement("FTR_FTD_AMT_TC")]
        public double FTR_FTD_AMT_TC { get; set; }
        [XmlElement("FTR_FTD_AMT_TC_ISMOD")]
        public int FTR_FTD_AMT_TC_ISMOD { get; set; }
        [XmlElement("FTR_CNM_CODE")]
        public string FTR_CNM_CODE { get; set; }
        [XmlElement("FTR_CNM_CODE_ISMOD")]
        public int FTR_CNM_CODE_ISMOD { get; set; }
    }
}

