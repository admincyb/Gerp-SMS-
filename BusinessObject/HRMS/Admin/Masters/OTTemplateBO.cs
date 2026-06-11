using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.HRMS.Admin.Masters
{
    public class OTTemplateBO
    {
        [Serializable]
        [XmlRoot("Root")]
        public class OTTemplate
        {
            [XmlElement("OTE_PK")]
            public int OTE_PK { get; set; }
            [XmlElement("OTE_CODE")]
            public string OTE_CODE { get; set; }
            [XmlElement("OTE_NAME")]
            public string OTE_NAME { get; set; }
            [XmlElement("OTE_DESC")]
            public string OTE_DESC { get; set; }    
            [XmlElement("OTE_DEPT")]
            public int OTE_DEPT { get; set; }
            [XmlElement("OTE_COMPANY")]
            public int OTE_COMPANY { get; set; }
            [XmlElement("BIZUNIT_PK")]
            public int BIZUNIT_PK { get; set; }
            [XmlElement("ACTIVE")]
            public int ACTIVE { get; set; }
            [XmlElement("USER_PK")]
            public int USER_PK { get; set; }
            [XmlElement("LAST_MOD_DT")]
            public DateTime LAST_MOD_DT { get; set; }
            [XmlElement("Detail")]
            public List<WeekDays> WeekDaysList { get; set; }
        }


        [Serializable]
        public class WeekDays
        {
            [XmlElement("OTD_WEEK_DAY")]
            public int OTD_WEEK_DAY { get; set; }
            public string OTD_WEEK_DAY_TEXT { get; set; }
            [XmlElement("OTD_PK")]
            public int OTD_PK { get; set; }
            [XmlElement("OTD_FORMULA")]
            public string OTD_FORMULA { get; set; }
            public string OTD_FORMULA_TEXT { get; set; }
        }
    }
}
