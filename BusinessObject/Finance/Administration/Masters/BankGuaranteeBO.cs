using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
namespace BusinessObject.Finance.Administration.Masters
{
    public class BankGuaranteeBO
    {

    }
    [Serializable]
    [XmlRoot("Root")]
    public class BankGuaranteeHeader
    {
        [XmlElement("BGM_PK")]
        public int BGM_PK { get; set; }
        [XmlElement("BGM_NO")]
        public string BGM_NO { get; set; }
        [XmlElement("BGM_DATE")]
        public string BGM_DATE { get; set; }
        [XmlElement("BGM_REF_NO")]
        public string BGM_REF_NO { get; set; }
        [XmlElement("BGM_REF_DATE")]
        public string BGM_REF_DATE { get; set; }
        [XmlElement("BGM_PARTY")]
        public int BGM_PARTY { get; set; }
        [XmlElement("BGM_BANK")]
        public int BGM_BANK { get; set; }
        [XmlElement("BGM_AMOUNT")]
        public string BGM_AMOUNT { get; set; }
        [XmlElement("BGM_EXP_DATE")]
        public string BGM_EXP_DATE { get; set; }
        [XmlElement("BGM_FROM")]
        public string BGM_FROM { get; set; }
        [XmlElement("BGM_TO")]
        public string BGM_TO { get; set; }
        [XmlElement("BGM_CLAIM_DATE")]
        public string BGM_CLAIM_DATE { get; set; }
        [XmlElement("BGM_REMARKS")]
        public string BGM_REMARKS { get; set; }
        [XmlElement("BGM_DEPT")]
        public int BGM_DEPT { get; set; }
        [XmlElement("BGM_COMPANY")]
        public int BGM_COMPANY { get; set; }
        [XmlElement("BIZUNIT_PK")]
        public int BIZUNIT_PK { get; set; }
        [XmlElement("ACTIVE")]
        public int ACTIVE { get; set; }
        [XmlElement("USER_PK")]
        public int USER_PK { get; set; }
        [XmlElement("LAST_MOD_DT")]
        public string LAST_MOD_DT { get; set; }
        [XmlElement("WKF_FLAG")]
        public int WKF_FLAG { get; set; }
    }
}
