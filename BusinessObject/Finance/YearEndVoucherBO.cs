using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.Finance
{
    [Serializable]
    [XmlRoot("Root")]
    public sealed class YearEndVoucherBO
    {
        public YearEndVoucherBO()
        {
            this.Details = new List<YearEndVoucherDetails>();
        }

        [XmlElement("YED_DATE")]
        public DateTime YED_DATE { get; set; }

        [XmlElement("BIZUNIT_PK")]
        public int BIZUNIT_PK { get; set; }

        [XmlElement("ACTIVE")]
        public int ACTIVE { get; set; }

        [XmlElement("USER_PK")]
        public int USER_PK { get; set; }

        [XmlElement("YED_PROCESS")]
        public int YED_PROCESS { get; set; }

        [XmlElement("LAST_MOD_DT")]
        public DateTime LAST_MOD_DT { get; set; }

        [XmlElement("Detail")]
        public List<YearEndVoucherDetails> Details { get; set; }        
    }

    [Serializable]
    [XmlRoot("Detail")]
    public sealed class YearEndVoucherDetails
    {
        [XmlElement("YED_PK")]
        public long YED_PK { get; set; }

        [XmlElement("YED_TRX_TYPE")]
        public int YED_TRX_TYPE { get; set; }

        [XmlElement("YED_TRX_PK")]
        public long YED_TRX_PK { get; set; }

        [XmlElement("YED_DESC")]
        public string YED_DESC { get; set; }

        [XmlElement("YED_EXCHG_RATE")]
        public double YED_EXCHG_RATE { get; set; }

        [XmlElement("YED_DEPT")]
        public int YED_DEPT { get; set; }

        [XmlElement("YED_COMPANY")]
        public int YED_COMPANY { get; set; }

        [XmlElement("YED_OLD_EXCHG_RATE")]
        public double YED_OLD_EXCHG_RATE { get; set; }

        [XmlElement("YED_AMOUNT")]
        public Decimal YED_AMOUNT { get; set; }

        
    }
    
}
