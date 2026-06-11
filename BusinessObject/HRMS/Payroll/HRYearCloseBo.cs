using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.HRMS.Payroll
{
    public class HRYearCloseBo
    {
    }
    [Serializable]
    [XmlRoot("Root")]
    public class HRYearCloseHeader
    {
        [XmlElement("HYR_PK")]
        public int HYR_PK { get; set; }
        [XmlElement("BIZUNIT_PK")]
        public int BIZUNIT_PK { get; set; }
        [XmlElement("USER_PK")]
        public int USER_PK { get; set; }
        [XmlElement("HYE_DATE")]
        public DateTime HYE_DATE { get; set; }
        [XmlElement("HYE_DESC")]
        public string HYE_DESC { get; set; }
        [XmlElement("LAST_MOD_DT")]
        public DateTime LAST_MOD_DT { get; set; }

        [XmlElement("LeaveType")]
        public List<HRYearCloseDetails> HRYearClauseDetails { get; set; }
    }

    public class HRYearCloseDetails
    {
        [XmlElement("LTM_PK")]
        public int LTM_PK { get; set; }

    }
}
