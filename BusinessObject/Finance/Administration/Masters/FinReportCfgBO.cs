using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.Finance.Administration.Masters
{
    [Serializable]
    [XmlRoot("Root")]
    public class FinReportCfgBO
    {
        
        [XmlElement("USER_PK")]
        public int USER_PK { get; set; }
        //[XmlElement("RTC_CRTD_DT")]
        //public DateTime RTC_CRTD_DT { get; set; }
        //[XmlElement("RTC_MOD_BY")]
        //public int RTC_MOD_BY { get; set; }
        [XmlElement("LAST_MOD_DT")]
        public DateTime LAST_MOD_DT { get; set; }
        [XmlElement("BIZUNIT_PK")]
        public int BIZUNIT_PK { get; set; }
        [XmlElement("Detail")]
        public List<FinReportCfgDtls> FinReportCfgDtls { get; set; }
    }
    public class FinReportCfgDtls
    {
        [XmlElement("RTC_PK")]
        public int RTC_PK { get; set; }
        [XmlElement("RTC_NAME")]
        public string RTC_NAME { get; set; }
        [XmlElement("RTC_DESC")]
        public string RTC_DESC { get; set; }
        [XmlElement("RTC_TEMPLATE")]
        public byte RTC_TEMPLATE { get; set; }
        [XmlElement("RTC_TYPE")]
        public int RTC_TYPE { get; set; }
        [XmlElement("RTC_LEVEL")]
        public byte RTC_LEVEL { get; set; }
        [XmlElement("RTC_SEQUENCE")]
        public byte RTC_SEQUENCE { get; set; }
        [XmlElement("RTC_PARENT")]
        public string RTC_PARENT { get; set; }
        [XmlElement("RTC_IS_GROUP")]
        public byte RTC_IS_GROUP { get; set; }
        [XmlElement("RTC_IS_BOLD")]
        public byte RTC_IS_BOLD { get; set; }
        [XmlElement("RTC_IS_TOTAL1")]
        public byte RTC_IS_TOTAL1 { get; set; }
        [XmlElement("RTC_IS_TOTAL2")]
        public byte RTC_IS_TOTAL2 { get; set; }
        [XmlElement("RTC_SPL_COND")]
        public string RTC_SPL_COND { get; set; }
        [XmlElement("RTC_ACTIVE")]
        public byte RTC_ACTIVE { get; set; }
        [XmlElement("RTC_DISPLAY_NAME")]
        public string RTC_DISPLAY_NAME { get; set; }
      
    }
}
