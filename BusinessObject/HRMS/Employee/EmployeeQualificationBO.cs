using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.HRMS.Employee
{
    public class EmployeeQualificationBO
    {
    }
    [Serializable]
    [XmlRoot("Root")]
    public class EmployeeQualifcn
    {
        [XmlElement("EQD_PK")]
        public int EQD_PK { get; set; }

        [XmlElement("EQD_EMPLOYEE")]
        public int EQD_EMPLOYEE { get; set; }

        [XmlElement("EQD_QUAL_STATUS")]
        public int EQD_QUAL_STATUS { get; set; }

        [XmlElement("EQD_HIGHEST_QUAL")]
        public int EQD_HIGHEST_QUAL { get; set; }

        [XmlElement("EQD_FROM_DATE")]
        public string EQD_FROM_DATE { get; set; }

        [XmlElement("EQD_TO_DATE")]
        public string EQD_TO_DATE { get; set; }

        [XmlElement("EQD_CERT_NO")]
        public string EQD_CERT_NO { get; set; }

        [XmlElement("EQD_COUNTRY")]
        public int EQD_COUNTRY { get; set; }

        [XmlElement("EQD_QUALIFICATION_TYPE")]
        public int EQD_QUALIFICATION_TYPE { get; set; }

        [XmlElement("EQD_CERT_NUM")]
        public string EQD_CERT_NUM { get; set; }

        [XmlElement("EQD_TITLE")]
        public string EQD_TITLE { get; set; }

        [XmlElement("EQD_ISSUED_BY")]
        public string EQD_ISSUED_BY { get; set; }

        [XmlElement("EQD_INSTITUTE")]
        public string EQD_INSTITUTE { get; set; }

        [XmlElement("EQD_QUALIFIED_ON")]
        public string EQD_QUALIFIED_ON { get; set; }

        [XmlElement("EQD_EXPIRY_DATE")]
        public string EQD_EXPIRY_DATE { get; set; }

        [XmlElement("EQD_DOC_ATTACHED")]
        public int EQD_DOC_ATTACHED { get; set; }

        [XmlElement("EQD_REMARKS")]
        public string EQD_REMARKS { get; set; }

        [XmlElement("EQD_COMPANY")]
        public int EQD_COMPANY { get; set; }

        [XmlElement("EQD_INTIMATE_BEFORE")]
        public int EQD_INTIMATE_BEFORE { get; set; }

        [XmlElement("EQD_ACTIVE")]
        public int EQD_ACTIVE { get; set; }

        
        [XmlElement("ACTIVE")]
        public int ACTIVE { get; set; }

        [XmlElement("USER_PK")]
        public int USER_PK { get; set; }

        [XmlElement("BIZUNIT")]
        public int BIZUNIT { get; set; }

        [XmlElement("LAST_MOD_DT")]
        public string LAST_MOD_DT { get; set; }

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


        [XmlElement("EQD_COUNTRY_TEXT")]
        public string EQD_COUNTRY_TEXT { get; set; }
        





    }
}
