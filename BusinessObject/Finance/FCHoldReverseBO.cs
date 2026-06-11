using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;


namespace BusinessObject.Finance
{

    [Serializable]
    [XmlRoot("Root")]
    public class FCHoldReverseBO
    {
        [XmlElement("ACTIVE")]
        public int Active { get; set; }
        [XmlElement("HRH_PK")]
        public int HrhPK { get; set; }
        [XmlElement("HRH_NO")]
        public string HrhNo { get; set; }
        [XmlElement("HRH_DATE")]
        public string HrhDate { get; set; }
        [XmlElement("HRH_REF_NO")]
        public string RefNo { get; set; }
        [XmlElement("HRH_REF_DATE")]
        public string RefDate { get; set; }
        [XmlElement("HRH_BANK")]
        public int Bank { get; set; }
        [XmlElement("HRH_REMARKS")]
        public string Remarks { get; set; }
        [XmlElement("HRH_CURRENCY")]
        public int Currency { get; set; }
        [XmlElement("HRH_AMOUNT_TC")]
        public double AmountTC { get; set; }
        [XmlElement("HRH_BANK_CHARGE")]
        public double BankCharge { get; set; }
        [XmlElement("HRH_BANK_CHARGE_CURR")]
        public int BankChargeCurrency { get; set; }
        [XmlElement("HRH_AMOUNT_BC")]
        public double AmountBC { get; set; }
        [XmlElement("HRH_EXCHG_RATE")]
        public double ExchangeRate { get; set; }
        [XmlElement("HRH_HAS_JRNL_ENTRY")]
        public int HasJournalEntry { get; set; }
        [XmlElement("HRH_DEL_STATUS")]
        public int DelStatus { get; set; }
        [XmlElement("HRH_STATUS")]
        public int Status { get; set; }
        [XmlElement("HRH_COMPANY")]
        public int Company { get; set; }
        [XmlElement("HRH_DEPT")]
        public int Department { get; set; }
        [XmlElement("BIZUNIT_PK")]
        public int BizUnit { get; set; }
        [XmlElement("USER_PK")]
        public int UserPK { get; set; }
        [XmlElement("WKF_FLAG")]
        public int WkfFlag { get; set; }
        [XmlElement("LAST_MOD_DT")]
        public string LastModDate { get; set; }
        [XmlElement("AST_DOC_MODE")]
        public int DocMode { get; set; }
        [XmlElement("APT_CODE")]
        public string AptCode { get; set; }
        [XmlElement("Detail")]
        public List<Detail> DetailsList { get; set; }
    }

    [Serializable]
    public class Detail
    {
        [XmlElement("HRD_PK")]
        public int HrdPK { get; set; }
        [XmlElement("HRD_AMOUNT")]
        public double HrdAmt { get; set; }
        [XmlElement("HRD_ACTIVE")]
        public int HrdActive { get; set; }

        [XmlElement("HRD_FIN_TRX_DTL")]
        public int TrxDtlPK { get; set; }
    }

}
