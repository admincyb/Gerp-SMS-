using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BusinessObject.Finance
{
    [Serializable]
    [XmlRoot("Root")]
    public class AssetDisposalBO : WorkflowBO
    {
        [XmlElement("ADH_PK")]
        public int DisposalPK { get; set; }

        [XmlElement("ADH_NO")]
        public string DisposalNo { get; set; }

        [XmlElement("ADH_DATE")]
        public DateTime DisposalDate { get; set; }

        [XmlElement("ADH_MONTH_YEAR")]
        public DateTime MonthYear { get; set; }

        [XmlElement("ADH_FROM")]
        public DateTime FromDate { get; set; }

        [XmlElement("ADH_TO")]
        public DateTime ToDate { get; set; }

        [XmlElement("ADH_DESC")]
        public string Description { get; set; }

        [XmlElement("ADH_DEPT")]
        public int DeptPK { get; set; }

        [XmlElement("ADH_TRX_CURR")]
        public int TrxCurrencyPK { get; set; }


        [XmlElement("ADH_TYPE")]
        public int TrxType { get; set; }

        [XmlElement("ADH_TRX_CURR_TEXT")]
        public string TrxCurrency { get; set; }

        [XmlElement("ADH_AMOUNT_TC")]
        public decimal AmountTC { get; set; }

        [XmlElement("ADH_AMOUNT_BC")]
        public decimal AmountBC { get; set; }

        [XmlElement("ADH_BASE_CURR")]
        public int BaseCurrencyPK { get; set; }

        [XmlElement("ADH_BASE_CURR_TEXT")]
        public string BaseCurrency { get; set; }

        [XmlElement("ADH_EXCHG_RATE")]
        public double ExchgRate { get; set; }

        [XmlElement("ADH_STATUS")]
        public string Status { get; set; }

        [XmlElement("ADH_ACTIVE")]
        public string DisposalActive { get; set; }

        [XmlElement("ADH_DEL_STATUS")]
        public string DeleteStatus { get; set; }

        [XmlElement("ADH_BIZUNIT")]
        public string BizunitPK { get; set; }

        [XmlElement("ADH_COMPANY")]
        public string CompanyPK { get; set; }

        [XmlElement("ADH_CRTD_BY")]
        public string CreatedBy { get; set; }

        [XmlElement("ADH_CRTD_DT")]
        public DateTime CreatedDate { get; set; }

        [XmlElement("ADH_MOD_BY")]
        public string ModifiedDate { get; set; }

        [XmlElement("LAST_MOD_DT")]
        public DateTime LastModDate { get; set; }

        [XmlElement("ADH_HAS_JRNL_ENTRY")]
        public short HasJournalEntry { get; set; }

        //Common Parameters
        [XmlElement("BIZUNIT_PK")]
        public int BizUnit { get; set; }

        [XmlElement("ACTIVE")]
        public int Active { get; set; }

        [XmlElement("USER_PK")]
        public int UserPk { get; set; }

        [XmlElement("WKF_FLAG")]
        public int WKF_FLAG { get; set; }

        [XmlElement("APT_CODE")]
        public string APT_CODE { get; set; }

        [XmlElement("Detail")]
        public List<AssetDisposalDetails> Details { get; set; }

        public AssetDisposalBO()
        {
            this.Details = new List<AssetDisposalDetails>();
        }
    }

    [Serializable]
    [XmlRoot("Detail")]
    public class AssetDisposalDetails
    {
        [XmlElement("ADD_PK")]
        public int DetailPK { get; set; }

        [XmlElement("ADD_HDR")]
        public int DisposalPK { get; set; }

        [XmlElement("ADD_ASSET")]
        public int AssetPK { get; set; }

        [XmlElement("ADD_AMOUNT")]
        public decimal Amount { get; set; }

        [XmlElement("ADD_ACTIVE")]
        public int DetailActive { get; set; }

        [XmlElement("asrCode")]
        public string asrCode { get; set; }

        [XmlElement("asrName")]
        public string asrName { get; set; }

        [XmlElement("asrType")]
        public string asrType { get; set; }

        [XmlElement("asrType_text")]
        public string asrType_text { get; set; }

        [XmlElement("asrCategory")]
        public string asrCategory { get; set; }

        [XmlElement("asrCategory_Text")]
        public string asrCategory_Text { get; set; }

        [XmlElement("amiDatePur")]
        public DateTime amiDatePur { get; set; }

        [XmlElement("amiCostPur")]
        public decimal amiCostPur { get; set; }

        [XmlElement("amiCostLand")]
        public decimal amiCostLand { get; set; }

        [XmlElement("amiCurrPur")]
        public string amiCurrPur { get; set; }

        [XmlElement("amiCurrPur_Text")]
        public string amiCurrPur_Text { get; set; }

        [XmlElement("amiDeprPerc")]
        public double amiDeprPerc { get; set; }

        [XmlElement("BalanceDeprAmt")]
        public decimal BalanceDeprAmt { get; set; }
    }
}
