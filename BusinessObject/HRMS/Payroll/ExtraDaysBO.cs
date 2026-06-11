using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.HRMS.Payroll
{
    public class ExtraDaysBO
    { }

    [Serializable]
    [XmlRoot("Root")]
    public class ExtraDaysHeader
    {
        [XmlElement("EXH_NO")]
        public string EXH_NO { get; set; }
        [XmlElement("EXH_PK")]
        public int EXH_PK { get; set; }
        [XmlElement("EXH_SAL_MONTH")]
        public DateTime EXH_SAL_MONTH { get; set; }
        [XmlElement("EXH_BRANCH")]
        public int EXH_BRANCH { get; set; }
        [XmlElement("EXH_DESC")]
        public string EXH_DESC { get; set; }
        [XmlElement("EXH_DEPT")]
        public int EXH_DEPT { get; set; }
        [XmlElement("EXH_ACTIVE")]
        public int EXH_ACTIVE { get; set; }
        [XmlElement("EXH_BRANCH_TEXT")]
        public string EXH_BRANCH_TEXT { get; set; }

        [XmlElement("EXH_COMPANY")]
        public int EXH_COMPANY { get; set; }
        [XmlElement("BIZUNIT")]
        public int BIZUNIT { get; set; }
        [XmlElement("USER_PK")]
        public int USER_PK { get; set; }
        [XmlElement("LAST_MOD_DT")]
        public DateTime LAST_MOD_DT { get; set; }
        [XmlElement("Details")]
        public List<ExtraDaysDetails> ExtraDaysDetails { get; set; }
    }

    [Serializable]
    [XmlRoot("Details")]
    public class ExtraDaysDetails
    {
        [XmlElement("EXD_PK")]
        public int EXD_PK { get; set; }
        [XmlElement("EXD_EXH_HDR")]
        public int EXD_EXH_HDR { get; set; }
        [XmlElement("EXD_DATE")]
        public DateTime EXD_DATE { get; set; }
        [XmlElement("EXD_DAYS")]
        public double EXD_DAYS { get; set; }
        [XmlElement("EXD_EMPLOYEE")]
        public int EXD_EMPLOYEE { get; set; }
        [XmlElement("EXD_EMPLOYEE_TEXT")]
        public string EXD_EMPLOYEE_TEXT { get; set; }
        [XmlElement("EXD_REMARK")]
        public string EXD_REMARK { get; set; }
        [XmlElement("EXD_ACTIVE")]
        public int EXD_ACTIVE { get; set; }
        [XmlElement("EXD_PAYROLL_DTL")]
        public int EXD_PAYROLL_DTL { get; set; }

        [XmlElement("EXD_BRANCH")]
        public string EXD_BRANCH { get; set; }    // Import
        [XmlElement("EXD_EMP_CODE")]
        public string EXD_EMP_CODE { get; set; }    // Import
    } 
}
