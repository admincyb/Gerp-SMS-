using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BusinessObject.AssetService
{
    [Serializable]
    [XmlRoot("Root")]
    public class CWIPAsset : WorkflowBO
    {
        [XmlElement("CWH_PK")]
        public int CWIPAsset_PK { get; set; }

        [XmlElement("CWH_NO")]
        public string CWIPNo { get; set; }

        [XmlElement("CWH_DATE")]
        public DateTime CWIPDate { get; set; }

        [XmlElement("CWH_GROUP")]
        public int AccGroup { get; set; }

        [XmlElement("CWH_ACCOUNT")]
        public int CWIPAccount { get; set; }

        [XmlElement("CWH_ACCOUNT_TEXT")]
        public string CWIPAccountText { get; set; }

        [XmlElement("AssetCost")]
        public decimal AssetCost { get; set; }
        public int CreatedUserPK { get; set; }

        [XmlElement("WKF_FLAG")]
        public int WKF_FLAG { get; set; }

        [XmlElement("USER_PK")]
        public int USER_PK { get; set; }

        [XmlElement("APT_CODE")]
        public string APT_CODE { get; set; }

        [XmlElement("CWH_STATUS")]
        public int UserStatus { get; set; }

        [XmlElement("CWH_STATUS_TEXT")]
        public string StatusText { get; set; }

        [XmlElement("CWH_INV_AMD_FLAG")]
        public int InvoiceAmendFlag { get; set; }

        [XmlElement("CWH_Depreciation_Flag")]
        public int HasDepreciation { get; set; }

        [XmlElement("Asset_Cost_Flag")]
        public int AssetCostChanged { get; set; }

        [XmlElement("Account_Exists")]
        public int AccountExist { get; set; }

        [XmlElement("CWH_DEPT")]
        public int DeptPK { get; set; }

        [XmlElement("BIZUNIT")]
        public int BizunitPK { get; set; }

        [XmlElement("CWH_COMPANY")]
        public int CompanyPK { get; set; }

        [XmlElement("Voucher_Exists")]
        public int VoucherExist { get; set; }

        [XmlElement("CWH_REFRESH")]
        public int Refresh { get; set; }

        [XmlElement("Details")]
        public List<TransactionList> TranDetails { get; set; }
    }

    [Serializable]
    public class TransactionList
    {
        [XmlElement("FTH_PK")]
        public int FTH_PK { get; set; }

        [XmlElement("FTR_PK")]
        public int FTR_PK { get; set; }

        [XmlElement("FTH_PO_NUMBER")]
        public string FTH_PO_NUMBER { get; set; }

        [XmlElement("FTR_NARRATION")]
        public string FTR_NARRATION { get; set; }

        [XmlElement("FTR_SELECTED")]
        public int FTR_SELECTED { get; set; }

        [XmlElement("FTH_REF_PK")]
        public int InvoicePK { get; set; }

        [XmlElement("FTH_REF_NO")]
        public string InvoiceNo { get; set; }

        [XmlElement("FTH_REF_DATE")]
        public DateTime InvoiceDate { get; set; }

        [XmlElement("FTH_REF_TYPE")]
        public string InvoiceType { get; set; }

        [XmlElement("FTH_VOUCHER_NO")]
        public string VoucherNo { get; set; }

        [XmlElement("FTH_DATE")]
        public DateTime FTH_DATE { get; set; }

        [XmlElement("FTH_TRX_CURR")]
        public int TransactionCurrency { get; set; }

        [XmlElement("FTH_TRX_CURR_TEXT")]
        public string TranCurrencyText { get; set; }

        [XmlElement("FTH_BASE_CURR")]
        public int BaseCurrency { get; set; }

        [XmlElement("FTH_BASE_CURR_TEXT")]
        public string BaseCurrencyText { get; set; }

        [XmlElement("COA_PK")]
        public int COA_PK { get; set; }

        [XmlElement("COA_CODE")]
        public string COACode { get; set; }

        [XmlElement("COA_NAME")]
        public string COAName { get; set; }

        [XmlElement("FTR_DR_AMT_BC")]
        public decimal DebitAmtBC { get; set; }

        [XmlElement("FTR_CR_AMT_BC")]
        public decimal CreditAmtBC { get; set; }

        [XmlElement("FTR_EXCHG_RATE")]
        public decimal ExchangeRate { get; set; }

        //[XmlElement("CWD_INV_AMD_VERSION")]
        //public int AmendVersion { get; set; }

        [XmlElement("CWD_INV_AMD_FLAG")]
        public int AmendFlag{ get; set; }

        [XmlElement("CWD_AMOUNT")]
        public decimal TransAmount { get; set; }

        [XmlElement("CWD_TRNS_AMOUNT")]
        public decimal TransNow { get; set; }

        [XmlElement("CWD_IS_DR")]
        public int IsDebit { get; set; }
    }
}
