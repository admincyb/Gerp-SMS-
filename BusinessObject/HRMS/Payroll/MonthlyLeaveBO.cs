using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.HRMS.Payroll
{
    public class MonthlyLeaveBO
    {
        [Serializable]
        [XmlRoot("Root")]
        public class MonthlyLeaveMaster
        {
            [XmlElement("ELD_COMPANY")]
            public int ELD_COMPANY { get; set; }
            [XmlElement("ELD_DEPT")]
            public int ELD_DEPT { get; set; }
            [XmlElement("BIZUNIT_PK")]
            public int BIZUNIT_PK { get; set; }
            [XmlElement("USER_PK")]
            public int USER_PK { get; set; }
            //[XmlElement("ELD_MONTH")]
            //public DateTime ELD_MONTH { get; set; }
            [XmlElement("Detail")]
            public List<MonthlyLeaveData> MonthlyLeaveDatas { get; set; }
        }

        [Serializable]
        public class MonthlyLeaveData
        {

            [XmlElement("ELD_PK")]
            public int ELD_PK { get; set; }
            [XmlElement("ELD_EMPLOYEE")]
            public int ELD_EMPLOYEE { get; set; }
            [XmlElement("ELD_MONTH")]
            public DateTime ELD_MONTH { get; set; }
            [XmlElement("ELD_LEAVE_TYPE")]
            public int ELD_LEAVE_TYPE { get; set; }
            [XmlElement("ELD_LEAVE_COUNT")]
            public decimal ELD_LEAVE_COUNT { get; set; }
            [XmlElement("ELD_REMARKS")]
            public string ELD_REMARKS { get; set; }
            [XmlElement("ELD_PAYROLL_DTL")]
            public string ELD_PAYROLL_DTL { get; set; }

            [XmlElement("ROW_NO")]
            public int ROW_NO { get; set; }
            [XmlElement("ELD_EMPLOYEE_TEXT")]
            public string ELD_EMPLOYEE_TEXT { get; set; }
            [XmlElement("ELD_LEAVE_TYPE_TEXT")]
            public string ELD_LEAVE_TYPE_TEXT { get; set; }
            [XmlElement("ELD_LEAVE_TYPE_CODE_TEXT")]
            public string ELD_LEAVE_TYPE_CODE_TEXT { get; set; }
            [XmlElement("LAST_MOD_DT")]
            public string LAST_MOD_DT { get; set; }
            [XmlElement("IS_DELETED")]
            public int IS_DELETED { get; set; }

        }

        [Serializable]
        [XmlRoot("Root")]
        public class MonthlyLeaveDataRoot
        {
            [XmlElement("Detail")]
            public List<MonthlyLeaveData> MonthlyLeaveDatas { get; set; }
        }

        [Serializable]
        [XmlRoot("Root")]
        public class LeaveEntryMaster
        {

            [XmlElement("ELR_PK")]
            public int ELR_PK { get; set; }
            [XmlElement("ELR_COMPANY")]
            public int ELR_COMPANY { get; set; }
            [XmlElement("ELR_BRANCH")]
            public int ELR_BRANCH { get; set; }
            [XmlElement("ELR_DATE")]
            public DateTime ELR_DATE { get; set; }
            [XmlElement("ELR_REMARKS")]
            public string ELR_REMARKS { get; set; }
            [XmlElement("ELR_ACTIVE")]
            public int ELR_ACTIVE { get; set; }
            [XmlElement("ELR_BRANCH_TEXT")]
            public string ELR_BRANCH_TEXT { get; set; }

            [XmlElement("ELD_COMPANY")]
            public int ELD_COMPANY { get; set; }
            [XmlElement("ELD_DEPT")]
            public int ELD_DEPT { get; set; }
            [XmlElement("BIZUNIT_PK")]
            public int BIZUNIT_PK { get; set; }
            [XmlElement("USER_PK")]
            public int USER_PK { get; set; }
            [XmlElement("VALIDATE_LEAVE")]
            public int VALIDATE_LEAVE { get; set; }
            [XmlElement("LAST_MOD_DT")]
            public DateTime LAST_MOD_DT { get; set; }
            [XmlElement("LEAVE_SKIP")]
            public int LEAVE_SKIP { get; set; }
            [XmlElement("HOL_SKIP")]
            public int HOL_SKIP { get; set; }
            [XmlElement("OFFDAY_SKIP")]
            public int OFFDAY_SKIP { get; set; }

            [XmlElement("Details")]
            public List<LeaveDetails> LeaveDetails { get; set; }
        }

        [Serializable]
        public class LeaveDetails
        {

            [XmlElement("ELD_PK")]
            public int ELD_PK { get; set; }
            [XmlElement("ELD_EMPLOYEE")]
            public int ELD_EMPLOYEE { get; set; }
            //[XmlElement("ELD_MONTH")]
            // public DateTime ELD_MONTH { get; set; }
            [XmlElement("ELD_LEAVE_TYPE")]
            public int ELD_LEAVE_TYPE { get; set; }
            [XmlElement("ELD_LEAVE_COUNT")]
            public decimal ELD_LEAVE_COUNT { get; set; }
            [XmlElement("ELD_REMARKS")]
            public string ELD_REMARKS { get; set; }
            [XmlElement("ELD_PAYROLL_DTL")]
            public string ELD_PAYROLL_DTL { get; set; }

            [XmlElement("ELD_LV_FROM_DT")]
            public DateTime ELD_LV_FROM_DT { get; set; }
            [XmlElement("ELD_LV_TO_DT")]
            public DateTime ELD_LV_TO_DT { get; set; }
            [XmlElement("ELD_LV_FROM_HALF")]
            public int ELD_LV_FROM_HALF { get; set; }
            [XmlElement("ELD_LV_TO_HALF")]
            public int ELD_LV_TO_HALF { get; set; }

            [XmlElement("ROW_NO")]
            public int ROW_NO { get; set; }
            [XmlElement("ELD_EMPLOYEE_TEXT")]
            public string ELD_EMPLOYEE_TEXT { get; set; }
            [XmlElement("ELD_LEAVE_TYPE_TEXT")]
            public string ELD_LEAVE_TYPE_TEXT { get; set; }
            [XmlElement("ELD_LEAVE_TYPE_CODE_TEXT")]
            public string ELD_LEAVE_TYPE_CODE_TEXT { get; set; }
            [XmlElement("IS_DELETED")]
            public int IS_DELETED { get; set; }
            [XmlElement("ELD_MOD_DT")]
            public DateTime ELD_MOD_DT { get; set; }

            [XmlElement("ELD_CREDIT_LEAVE_COUNT")]
            public decimal ELD_CREDIT_LEAVE_COUNT { get; set; }



            [XmlElement("Item_details")]
            public List<LeaveDayDetails> LeaveDayDetails { get; set; }
        }

        [Serializable]
        public class LeaveDayDetails
        {

            [XmlElement("LDD_PK")]
            public int LDD_PK { get; set; }
            [XmlElement("LDD_ELD_PK")]
            public int LDD_ELD_PK { get; set; }
            [XmlElement("LDD_DATE")]
            public DateTime LDD_DATE { get; set; }
            [XmlElement("LDD_COUNT")]
            public double LDD_COUNT { get; set; }
            [XmlElement("LDD_LEAVE_IS_LOP")]
            public int LDD_LEAVE_IS_LOP { get; set; }
            [XmlElement("LDD_PAYROLL_DTL")]
            public int LDD_PAYROLL_DTL { get; set; }
            [XmlElement("LDD_LEAVE_IS_HALF")]
            public int  LDD_LEAVE_IS_HALF { get; set; }
        }

        [Serializable]
        [XmlRoot("Root")]
        public class LeaveEntryDeleteMaster
        {
            [XmlElement("ELR_PK")]
            public int ELR_PK { get; set; }
            [XmlElement("LV_EXIST")]
            public int LV_EXIST { get; set; }

            [XmlElement("Item_details")]
            public List<LeaveDayDetails> LeaveDayDetails { get; set; }
        }

    }
}
