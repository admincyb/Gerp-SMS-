using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using BusinessObject.HRMS.Admin.Masters;

namespace BusinessObject.HRMS.Employee
{
    [Serializable]
    [XmlRoot("Root")]
    public class EmployeePayDetailsBO
    {
        [XmlElement("EPD_PK")]
        public int EPD_PK { get; set; }

        [XmlElement("EPD_EMPLOYEE")]
        public int EPD_EMPLOYEE { get; set; }

        [XmlElement("EPD_BASIC_PAY")]
        public double EPD_BASIC_PAY { get; set; }

        [XmlElement("EPD_PAN_NO")]
        public string EPD_PAN_NO { get; set; }

        [XmlElement("EPD_PAY_MODE")]
        public int EPD_PAY_MODE { get; set; }

        [XmlElement("EPD_SALARY_TEMP")]
        public int EPD_SALARY_TEMP { get; set; }

        [XmlElement("EPD_PAY_BANK")]
        public string EPD_PAY_BANK { get; set; }

        [XmlElement("EPD_PAY_BANK_BRANCH")]
        public string EPD_PAY_BANK_BRANCH { get; set; }

        [XmlElement("EPD_BANK_AC_NO")]
        public string EPD_BANK_AC_NO { get; set; }

        [XmlElement("EPD_BANK_AC_NAME")]
        public string EPD_BANK_AC_NAME { get; set; }

        [XmlElement("EPD_BANK_IFSC")]
        public string EPD_BANK_IFSC { get; set; }

        [XmlElement("EPD_PF_AC")]
        public string EPD_PF_AC { get; set; }

        [XmlElement("EPD_PF_DATE")]
        public string EPD_PF_DATE { get; set; }

        [XmlElement("EPD_SOCSO_AC")]
        public string EPD_SOCSO_AC { get; set; }

        [XmlElement("EPD_SOCSO_DATE")]
        public string EPD_SOCSO_DATE { get; set; }

        [XmlElement("EPD_EMP_TYPE")]
        public int EPD_EMP_TYPE { get; set; }

        [XmlElement("EPD_LEAVE_TEMP")]
        public string EPD_LEAVE_TEMP { get; set; }

        [XmlElement("EPD_OT_TEMP")]
        public string EPD_OT_TEMP { get; set; }

        [XmlElement("EPD_OT_AVAILABE")]
        public string EPD_OT_AVAILABE { get; set; }

        [XmlElement("EPD_HAS_OT_FROM_ATT")]
        public string EPD_HAS_OT_FROM_ATT { get; set; }

        [XmlElement("EPD_WORKING_DAY_TYPE")]
        public string EPD_WORKING_DAY_TYPE { get; set; }

        [XmlElement("EPD_CONSIDER_LATE_HRS")]
        public string EPD_CONSIDER_LATE_HRS { get; set; }

        [XmlElement("EDP_ESI_REQUIRED")]
        public string EDP_ESI_REQUIRED { get; set; }

        [XmlElement("EDP_PF_REQUIRED")]
        public string EDP_PF_REQUIRED { get; set; }

        [XmlElement("EDP_SSO_REQUIRED")]
        public string EDP_SSO_REQUIRED {get;set;}
        
        [XmlElement("EPD_WORKING_DAY")]
        public string EPD_WORKING_DAY { get; set; }

        [XmlElement("EPD_CURRENCY")]
        public int EPD_CURRENCY { get; set; }

        [XmlElement("EPD_CURRENCY_TEXT")]
        public string EPD_CURRENCY_TEXT { get; set; }

        [XmlElement("EPD_WRK_HRS")]
        public string EPD_WRK_HRS { get; set; }

        [XmlElement("EPD_OT_RATE")]
        public string EPD_OT_RATE { get; set; }

        [XmlElement("EPD_BREAK_TIME")]
        public string EPD_BREAK_TIME { get; set; }

        [XmlElement("BIZUNIT_PK")]
        public int BIZUNIT_PK { get; set; }

        [XmlElement("ACTIVE")]
        public int ACTIVE { get; set; }

        [XmlElement("USER_PK")]
        public int USER_PK { get; set; }

        [XmlElement("LAST_MOD_DT")]
        public DateTime LastModifiedDate { get; set; }

        [XmlElement("LEAVE_DEL")]
        public int LEAVE_DEL { get; set; }

        [XmlElement("EPD_RESIGNED_DATE")]
        public DateTime? EPD_RESIGNED_DATE { get; set; }

        [XmlElement("Details")]
        public List<WorkingHours> WorkingHours { get; set; }


    }
}
