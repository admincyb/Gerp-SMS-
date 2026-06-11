using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.HRMS.Employee
{
    [Serializable]
    [XmlRoot("Root")]
    public class EmployeePerformanceBO
    {
        [XmlElement("EPD_PK")]
        public int EPD_PK { get; set; }
        [XmlElement("EPD_EMPLOYEE")]
        public int EPD_EMPLOYEE { get; set; }
        [XmlElement("EPD_DATE")]
        public string EPD_DATE { get; set; }
        [XmlElement("EPD_DONE_BY")]
        public int EPD_DONE_BY { get; set; }
        [XmlElement("EPD_DONE_BY_TEXT")]
        public string EPD_DONE_BY_TEXT { get; set; }
        [XmlElement("EPD_CATEGORY")]
        public string EPD_CATEGORY { get; set; }
        [XmlElement("EPD_ACTION")]
        public string EPD_ACTION { get; set; }
        [XmlElement("EPD_INCIDENT")]
        public string EPD_INCIDENT { get; set; }
        [XmlElement("EPD_FOLLOW_UP")]
        public string EPD_FOLLOW_UP { get; set; }
        [XmlElement("EPD_COMMENT")]
        public string EPD_COMMENT { get; set; }
        [XmlElement("EPD_ACTIVE")]
        public int EPD_ACTIVE { get; set; }
        [XmlElement("USER_PK")]
        public int USER_PK { get; set; }
        [XmlElement("EPD_BIZUNIT")]
        public int EPD_BIZUNIT { get; set; }
        [XmlElement("EPD_MOD_DT")]
        public DateTime EPD_MOD_DT { get; set; }

        //Documents
        [XmlElement("DOC_PK")]
        public int DOC_PK { get; set; }
        [XmlElement("DOC_SEQ_NO")]
        public int DOC_SEQ_NO { get; set; }
        [XmlElement("DOC_TITLE")]
        public string DOC_TITLE { get; set; }
        [XmlElement("DOC_NAME")]
        public string DOC_NAME { get; set; }
        [XmlElement("DOC_PATH")]
        public string DOC_PATH { get; set; }
        [XmlElement("DOC_TYPE")]
        public string DOC_TYPE { get; set; }

        [XmlElement("DocDetails")]
        public List<EmpDocUploadBinder> DocDetails { get; set; }

    }
}
