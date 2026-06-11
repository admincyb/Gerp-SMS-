using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.HRMS.Admin.Masters
{

    public class LeaveTemplateBO
    {
        [Serializable]
        [XmlRoot("Root")]
        public class LeaveTemplate
        {
            [XmlElement("LTE_PK")]
            public int LTE_PK { get; set; }
            [XmlElement("LTE_NAME")]
            public string LTE_NAME { get; set; }
            [XmlElement("LTE_CODE")]
            public string LTE_CODE { get; set; }
            [XmlElement("LTE_DESC")]
            public string LTE_DESC { get; set; }    
            //public string Formula { get; set; }
            //public string FormulaText { get; set; }
            [XmlElement("LTE_DEPT")]
            public int LTE_DEPT { get; set; }
            [XmlElement("LTE_COMPANY")]
            public int LTE_COMPANY { get; set; }
            [XmlElement("ACTIVE")]
            public int ACTIVE { get; set; }
            [XmlElement("USER_PK")]
            public int USER_PK { get; set; }
            [XmlElement("BIZUNIT_PK")]
            public int BIZUNIT_PK { get; set; }
            [XmlElement("LAST_MOD_DT")]
            public DateTime LAST_MOD_DT { get; set; }
            [XmlElement("Detail")]
            public List<LeaveDeductionDetail> LeaveDeductionDetails { get; set; }
        }

        [Serializable]
        public class LeaveDeductionDetail
        {
            public int? SlNo { get; set; }
            [XmlElement("LTD_PK")]
            public int LTD_PK { get; set; }
            [XmlElement("LTD_PAY_ELEMENT")]
            public int LTD_PAY_ELEMENT { get; set; }
            [XmlElement("LTD_ELEMENT_TEXT")]
            public string LTD_ELEMENT_TEXT { get; set; }
            [XmlElement("LTD_FORMULA")]
            public string LTD_FORMULA { get; set; }
            //[XmlElement("LTD_FORMULA_TEXT")]
            public string LTD_FORMULA_TEXT { get; set; }
        }
    }

    
}
