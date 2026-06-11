using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.HRMS.Payroll
{
    public class OvertimeCalculatorBO
    {
        [Serializable]
        [XmlRoot("Root")]
        public class MonthlyOTMaster
        {
            [XmlElement("EOE_PK")]
            public int EOE_PK { get; set; }
            [XmlElement("EOE_NO")]
            public string EOE_NO { get; set; }
            [XmlElement("EOE_FROM_DATE")]
            public string EOE_FROM_DATE { get; set; }
            [XmlElement("EOE_TO_DATE")]
            public string EOE_TO_DATE { get; set; }
            [XmlElement("EOE_BRANCH")]
            public string EOE_BRANCH { get; set; }
            [XmlElement("EOE_BRANCH_TEXT")]
            public string EOE_BRANCH_TEXT { get; set; }
            [XmlElement("EOE_empDepartment")]
            public string EOE_empDepartment { get; set; }
            [XmlElement("EOE_empDepartment_TEXT")]
            public string EOE_empDepartment_TEXT { get; set; }
            [XmlElement("EOE_REMARKS")]
            public string EOE_REMARKS { get; set; }
            [XmlElement("EOE_STATUS")]
            public string EOE_STATUS { get; set; }
            [XmlElement("EOE_ACTIVE")]
            public int EOE_ACTIVE { get; set; }
            [XmlElement("EOE_COMPANY")]
            public int EOE_COMPANY { get; set; }
            [XmlElement("EOE_DEPT")]
            public int EOE_DEPT { get; set; }
            [XmlElement("LAST_MOD_DT")]
            public DateTime LAST_MOD_DT { get; set; }
            [XmlElement("USER_PK")]
            public int USER_PK { get; set; }
            [XmlElement("BIZUNIT_PK")]
            public int BIZUNIT_PK { get; set; }
            [XmlElement("WKF_FLAG")]
            public int WKF_FLAG { get; set; }
            [XmlElement("EOL_OTHER_LEAVE_TYPE")]
            public int EOL_OTHER_LEAVE_TYPE { get; set; }
            [XmlElement("EOL_SPECIAL_HOLYDAY_TYPE")]
            public int EOL_SPECIAL_HOLYDAY_TYPE { get; set; }
            [XmlElement("EOL_BRANCH")]
            public int EOL_BRANCH { get; set; }
            [XmlElement("EOL_BRANCH_Text")]
            public string EOL_BRANCH_Text { get; set; }
            [XmlElement("EOL_LV_MONTH")]
            public string EOL_LV_MONTH { get; set; }

            [XmlElement("EOD_LV_DAY")]//sun,mon
            public int EOD_LV_DAY { get; set; }

            [XmlElement("Detail")]
            public List<MonthlyOTData> MonthlyOTDatas { get; set; }
        }

        [Serializable]
        public class MonthlyOTData
        {
            [XmlElement("ROW_NO")]
            public int ROW_NO { get; set; }
            [XmlElement("EOT_PK")]
            public int EOT_PK { get; set; }
            [XmlElement("EOE_PK")]
            public int EOE_PK { get; set; }
            [XmlElement("EOT_EMPLOYEE")]
            public int EOT_EMPLOYEE { get; set; }
            [XmlElement("EOT_EMPLOYEE_TEXT")]
            public string EOT_EMPLOYEE_TEXT { get; set; }

            [XmlElement("EOT_EMPLOYEE_CODE")]
            public string EOT_EMPLOYEE_CODE { get; set; }

            [XmlElement("EOT_EMPLOYEE_PK")]
            public string EOT_EMPLOYEE_PK { get; set; }
            [XmlElement("EOT_EMPLOYEE_NAME")]
            public string EOT_EMPLOYEE_NAME { get; set; }
            [XmlElement("empBranch")]
            public string empBranch { get; set; }
            [XmlElement("empBranchText")]
            public string empBranchText { get; set; }
            [XmlElement("empBranchCode")]
            public string empBranchCode { get; set; }
            [XmlElement("empDesignationText")]
            public string empDesignationText { get; set; }
            [XmlElement("empdsgCode")]
            public string empdsgCode { get; set; }
            [XmlElement("empDepartmentText")]
            public string empDepartmentText { get; set; }
            [XmlElement("DPT_CODE")]
            public string DPT_CODE { get; set; }
            [XmlElement("IsChecked")]
            public int IsChecked { get; set; }
            [XmlElement("EOT_MOD_DT")]
            public string EOT_MOD_DT { get; set; }
            [XmlElement("EOT_WORK_HRS")]
            public double EOT_WORK_HRS { get; set; }
            [XmlElement("EOT_REMARKS")]
            public string EOT_REMARKS { get; set; }
            [XmlElement("LAST_MOD_DT")]
            public string LAST_MOD_DT { get; set; }
            [XmlElement("IS_DELETED")]
            public int IS_DELETED { get; set; }
            [XmlElement("EOT_DATE")]
            public string EOT_DATE { get; set; }
            [XmlElement("EOT_HOURS")]
            public double EOT_HOURS { get; set; }
            [XmlElement("EOT_HOURS1")]
            public double EOT_HOURS1 { get; set; }
            [XmlElement("EOT_HOURS2")]
            public double EOT_HOURS2 { get; set; }
            [XmlElement("EOT_HOURS3")]
            public double EOT_HOURS3 { get; set; }
            [XmlElement("EOT_HOURS4")]
            public double EOT_HOURS4 { get; set; }
            [XmlElement("EOT_HOURS5")]
            public double EOT_HOURS5 { get; set; }
            [XmlElement("EOT_PAYROLL_DTL")]
            public string EOT_PAYROLL_DTL { get; set; }

            [XmlElement("OTEntry")]
            public int OTEntry { get; set; }
            [XmlElement("empDepartment")]
            public int empDepartment { get; set; }

            [XmlElement("LeaveType")]//yearly-1,Monthly-2,SpecialHoliday
            public string LeaveType { get; set; }

            [XmlElement("SpecialHoliday")]//yes,No
            public string SpecialHoliday { get; set; }
        }
    }
}
