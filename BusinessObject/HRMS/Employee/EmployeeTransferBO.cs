using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.HRMS.Employee
{
    public class EmployeeTransferBO
    {
    }
    [Serializable]
    [XmlRoot("Root")]
    public class EmployeeTransferHeader : WorkflowBO
    {
        [XmlElement("EFH_PK")]
        public int EFH_PK { get; set; }
        [XmlElement("EFH_NO")]
        public string EFH_NO { get; set; }
        [XmlElement("EFH_DATE")]
        public DateTime EFH_DATE { get; set; }
        [XmlElement("EFH_FROM")]
        public int EFH_FROM { get; set; }
        [XmlElement("EFH_FROM_TEXT")]
        public string EFH_FROM_TEXT { get; set; }
        [XmlElement("EFH_TO")]
        public int EFH_TO { get; set; }
        [XmlElement("EFH_TO_TEXT")]
        public string EFH_TO_TEXT { get; set; }
        [XmlElement("EFH_REASON")]
        public int EFH_REASON { get; set; }
        [XmlElement("EFH_REASON_TEXT")]
        public string EFH_REASON_TEXT { get; set; }
        [XmlElement("EFH_DESC")]
        public string EFH_DESC { get; set; }
        [XmlElement("EFH_EFFECT_DT")]
        public DateTime EFH_EFFECT_DT { get; set; }
        [XmlElement("EFH_STATUS")]
        public int EFH_STATUS { get; set; }
        [XmlElement("EFH_DEPT")]
        public int EFH_DEPT { get; set; }
        [XmlElement("EFH_COMPANY")]
        public int EFH_COMPANY { get; set; }
        [XmlElement("USER_PK")]
        public int USER_PK { get; set; }
        [XmlElement("BIZUNIT")]
        public int BIZUNIT { get; set; }
        [XmlElement("LAST_MOD_DT")]
        public DateTime LAST_MOD_DT { get; set; }
        [XmlElement("WKF_FLAG")]
        public int WKF_FLAG { get; set; }
        [XmlElement("Details")]
        public List<EmployeeTransferDetails> EmployeeTransferDtl { get; set; }
    }

    [Serializable]
    public class EmployeeTransferDetails
    {
        [XmlElement("EFD_PK")]
        public int EFD_PK { get; set; }
        [XmlElement("EFD_EFH_HDR")]
        public int EFD_EFH_HDR { get; set; }
        [XmlElement("EFD_EMPLOYEE")]
        public int EFD_EMPLOYEE { get; set; }
        [XmlElement("EFD_EMPLOYEE_TEXT")]
        public string EFD_EMPLOYEE_TEXT { get; set; }
        [XmlElement("EFD_EMPLOYEE_RPT")]
        public string EFD_EMPLOYEE_RPT { get; set; }
        [XmlElement("EFD_EMPLOYEE_RPT_TEXT")]
        public string EFD_EMPLOYEE_RPT_TEXT { get; set; }
        [XmlElement("EFD_REMARKS")]
        public string EFD_REMARKS { get; set; }
    }

}
