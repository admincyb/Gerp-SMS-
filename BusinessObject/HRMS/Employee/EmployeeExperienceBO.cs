using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.HRMS.Employee
{
    public class EmployeeExperienceBO
    {

    }
    [Serializable]
    [XmlRoot("Root")]
    public class EmployeeExprnc
    {
        [XmlElement("EED_PK")]
        public int EED_PK { get; set; }

        [XmlElement("EED_EMPLOYEE")]
        public int EED_EMPLOYEE { get; set; }

        [XmlElement("EED_EMPLOYER")]
        public string EED_EMPLOYER { get; set; }

        [XmlElement("EED_DESIGNATION")]
        public string EED_DESIGNATION { get; set; }

        [XmlElement("EED_EXIT_REASON")]
        public int EED_EXIT_REASON { get; set; }

        [XmlElement("EED_PERIOD_FROM")]
        public string EED_PERIOD_FROM { get; set; }

        [XmlElement("EED_PERIOD_TO")]
        public string EED_PERIOD_TO { get; set; }

        [XmlElement("EED_JOB_NATURE")]
        public string EED_JOB_NATURE { get; set; }

        [XmlElement("EED_SALARY")]
        public decimal EED_SALARY { get; set; }

        [XmlElement("EED_CURRENCY")]
        public int EED_CURRENCY { get; set; }

        [XmlElement("EED_ADDRESS1")]
        public string EED_ADDRESS1 { get; set; }

        [XmlElement("EED_ADDRESS2")]
        public string EED_ADDRESS2 { get; set; }

        [XmlElement("EED_CITY")]
        public string EED_CITY { get; set; }

        [XmlElement("EED_DISTRICT")]
        public string EED_DISTRICT { get; set; }

        [XmlElement("EED_STATE")]
        public string EED_STATE { get; set; }

        [XmlElement("EED_COUNTRY")]
        public int EED_COUNTRY { get; set; }

        [XmlElement("EED_ZIP_CODE")]
        public string EED_ZIP_CODE { get; set; }

        [XmlElement("EED_PHONE")]
        public string EED_PHONE { get; set; }

        [XmlElement("EED_MOBILE")]
        public string EED_MOBILE { get; set; }

        [XmlElement("EED_JOB_DESCRIPTION")]
        public string EED_JOB_DESCRIPTION { get; set; }

        [XmlElement("EED_REMARKS")]
        public string EED_REMARKS { get; set; }

        [XmlElement("EED_HR_NAME")]
        public string EED_HR_NAME { get; set; }

        [XmlElement("EED_HR_DESIGNATION")]
        public string EED_HR_DESIGNATION { get; set; }

        [XmlElement("EED_HR_EMAIL")]
        public string EED_HR_EMAIL { get; set; }

        [XmlElement("EED_HR_PHONE")]
        public string EED_HR_PHONE { get; set; }

        [XmlElement("EED_HR_MOBILE1")]
        public string EED_HR_MOBILE1 { get; set; }

        [XmlElement("EED_HR_MOBILE2")]
        public string EED_HR_MOBILE2 { get; set; }

        [XmlElement("EED_REF_NAME")]
        public string EED_REF_NAME { get; set; }

        [XmlElement("EED_REF_DESIGNATION")]
        public string EED_REF_DESIGNATION { get; set; }

        [XmlElement("EED_REF_EMAIL")]
        public string EED_REF_EMAIL { get; set; }

        [XmlElement("EED_REF_PHONE")]
        public string EED_REF_PHONE { get; set; }

        [XmlElement("EED_REF_MOBILE1")]
        public string EED_REF_MOBILE1 { get; set; }

        [XmlElement("EED_REF_MOBILE2")]
        public string EED_REF_MOBILE2 { get; set; }

        [XmlElement("EED_COUNTRY_TEXT")]
        public string EED_COUNTRY_TEXT { get; set; }

        [XmlElement("EED_EXIT_REASON_TEXT")]
        public string EED_EXIT_REASON_TEXT { get; set; }

        [XmlElement("EED_HR_PHONE_EXT")]
        public string EED_HR_PHONE_EXT { get; set; }

        [XmlElement("EED_REF_PHONE_EXT")]
        public string EED_REF_PHONE_EXT { get; set; }

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


        [XmlElement("ACTIVE")]
        public int ACTIVE { get; set; }

        [XmlElement("USER_PK")]
        public int USER_PK { get; set; }

        [XmlElement("BIZUNIT")]
        public int BIZUNIT { get; set; }

        [XmlElement("LAST_MOD_DT")]
        public string LAST_MOD_DT { get; set; }


    }
}

