using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Data;

namespace BusinessObject.HRMS.Payroll
{
    class AttendanceBO
    {
    }

    [Serializable]
    [XmlRoot("Root")]
    public class EmployeeAttendance
    {
        [XmlElement("EAR_PK")]
        public int EAR_PK { get; set; }
        [XmlElement("EAR_NO")]
        public string EAR_NO { get; set; }
        [XmlElement("EAR_FROM_DATE")]
        public DateTime EAR_FROM_DATE { get; set; }
        [XmlElement("EAR_TO_DATE")]
        public DateTime EAR_TO_DATE { get; set; }
        [XmlElement("EAR_BRANCH")]
        public string EAR_BRANCH { get; set; }
        [XmlElement("EAR_BRANCH_TEXT")]
        public string EAR_BRANCH_TEXT { get; set; }
        [XmlElement("EAR_REMARKS")]
        public string EAR_REMARKS { get; set; }
        [XmlElement("EAR_EMPDEPARTMENT")]
        public string EAR_EMPDEPARTMENT { get; set; }
        [XmlElement("EAR_EMPDEPARTMENT_TEXT")]
        public string EAR_EMPDEPARTMENT_TEXT { get; set; }
        [XmlElement("EAR_COMPANY")]
        public int EAR_COMPANY { get; set; }
        [XmlElement("EAR_STATUS")]
        public int EAR_STATUS { get; set; }
        [XmlElement("EAR_IN_FL")]
        public int EAR_IN_FL { get; set; }
        [XmlElement("IS_DEL_FL")]
        public int IS_DEL_FL { get; set; }
        [XmlElement("EAR_ATT_MODE")]
        public int EAR_ATT_MODE { get; set; }
        

        [XmlElement("EAR_DEPT")]
        public int EAR_DEPT { get; set; }
        [XmlElement("BIZUNIT_PK")]
        public int BIZUNIT_PK { get; set; }
        [XmlElement("ACTIVE")]
        public int ACTIVE { get; set; }
        [XmlElement("USER_PK")]
        public int USER_PK { get; set; }
        [XmlElement("LAST_MOD_DT")]
        public DateTime LAST_MOD_DT { get; set; }
        [XmlElement("WKF_FLAG")]
        public int WKF_FLAG { get; set; }   

        [XmlElement("Details")]
        public List<AttendancePopupDetails> AttnDetails { get; set; }
        
    }

    [Serializable]
    [XmlRoot("Details")]
    public class AttendancePopupDetails
    {
        [XmlElement("EAT_PK")]
        public int Pk { get; set; }  // EAT_PK
        [XmlElement("EAT_SLNO")]
        public int SlNo { get; set; }
        [XmlElement("EAT_EMPLOYEE")]
        public int EmpPk { get; set; } // EAT_EMPLOYEE_PK
        public string Date { get; set; }
        [XmlElement("EAT_EMPLOYEE_TEXT")]
        public string EmpCode { get; set; } // EAT_EMPLOYEE_TEXT
        [XmlElement("EAT_EMPLOYEE_NAME")]
        public string EmpName { get; set; } // EAT_EMPLOYEE_NAME
        [XmlElement("empBranch")]
        public string Branch { get; set; } // empBranch
        [XmlElement("empBranchText")]
        public string Location { get; set; } // empBranchText
        [XmlElement("empBranch_pk")]
        public string LocationPk { get; set; } 
        [XmlElement("empBranchCode")]
        public string LocationCode { get; set; } // empBranchCode
        [XmlElement("empDepartmentText")]
        public string EmpDepartment { get; set; } // empDepartmentText
        [XmlElement("DPT_CODE")]
        public string EmpDepartmentCode { get; set; } // DPT_CODE
        [XmlElement("DPT_PK")]
        public string EmpDeptPk { get; set; } 
        [XmlElement("empDesignationText")]
        public string EmpDesignation { get; set; } // empDesignationText       

        [XmlElement("EMP_BREAK_TIME")]
        public string BreakHrs { get; set; } // EMP_BREAK_TIME
        
        [XmlElement("EAT_WORK_HRS")]
        public string WorkingHrs { get; set; } // EAT_WORK_HRS
        [XmlElement("EAT_DATE")]
        public DateTime DateDt { get; set; } // EAT_DATE 

        [XmlElement("EAT_IN_TIME")]
        public string InDt { get; set; } // EAT_IN_TIME       
        [XmlElement("EAT_B_OUT1")]
        public string BOut1Dt { get; set; } // EAT_B_OUT1    
        [XmlElement("EAT_B_IN1")]
        public string BIn1Dt { get; set; } // EAT_B_IN1 
        [XmlElement("EAT_B_OUT2")]
        public string BOut2Dt { get; set; } // EAT_B_OUT2    
        [XmlElement("EAT_B_IN2")]
        public string BIn2Dt { get; set; } // EAT_B_IN2    
        [XmlElement("EAT_OUT_TIME")]
        public string OutDt { get; set; } // EAT_OUT_TIME   

        [XmlElement("EAT_NORMAL_HRS")]
        public double NormalHrs { get; set; } // EAT_NORMAL_HRS
        [XmlElement("EAT_WORKED_HRS")]
        public double TotalHrs { get; set; }  //EAT_WORKED_HRS
        [XmlElement("EAT_SHORT_HRS")]
        public double ShortHrs { get; set; } // EAT_SHORT_HRS
        [XmlElement("EAT_OT_HRS")]
        public double OTDtHrs { get; set; } // EAT_OT_HRS   

        [XmlElement("EMP_FL")]
        public int CheckedFlag { get; set; }
        //public int IsChecked { get; set; }
        [XmlElement("EMP_IS_SAL_PRCD")]
        public int IsSalaryPrcd { get; set; }
        [XmlElement("EAT_REMARKS")]
        public string Remarks { get; set; } // EAT_REMARKS   
        [XmlElement("EAT_STATUS")]
        public int EAT_STATUS { get; set; } // EAT_STATUS
        [XmlElement("EAT_IS_UPDATE")]
        public int UpdateFlag { get; set; }  //EAT_IS_UPDATE
        [XmlElement("IS_RECALCULATE")]
        public int RecalculateFlag { get; set; }  //IS_RECALCULATE
        [XmlElement("EAT_BREAK_1")]
        public double Break1 { get; set; }  //EAT_BREAK_1
        [XmlElement("EAT_BREAK_2")]
        public double Break2 { get; set; }  //EAT_BREAK_2
        [XmlElement("EPD_HAS_OT_FROM_ATT")]
        public int HasOTFromPunching { get; set; }  //EPD_HAS_OT_FROM_ATT

        [XmlElement("In")]
        public string In { get; set; }
        [XmlElement("Out")]
        public string Out { get; set; }
        [XmlElement("Total")]
        public string Total { get; set; }
        [XmlElement("Short")]
        public string Short { get; set; }
        [XmlElement("OT")]
        public string OT { get; set; }

        [XmlElement("EAT_BREAK1")]
        public string EAT_BREAK1 { get; set; }
        [XmlElement("EAT_BREAK2")]
        public string EAT_BREAK2 { get; set; }

        //[XmlElement("EAT_MOD_DT")]
        //public string ModifiedDate { get; set; }

    }

    [Serializable]
    public sealed class AttendanceDetails
    {
        [XmlElement("EAT_PK")]
        public int EAT_PK { get; set; }

        [XmlElement("EAT_DATE")]
        public DateTime EAT_DATE { get; set; }

        [XmlElement("EAT_EMPLOYEE")]
        public int EAT_EMPLOYEE { get; set; }

        [XmlElement("EAT_IN_TIME")]
        public DateTime? EAT_IN_TIME { get; set; }

        [XmlElement("EAT_OUT_TIME")]
        public DateTime? EAT_OUT_TIME { get; set; }

        [XmlElement("EAT_FIRST")]
        public short EAT_FIRST { get; set; }

        [XmlElement("EAT_SECOND")]
        public short EAT_SECOND { get; set; }

        [XmlElement("EAT_NORMAL_HRS")]
        public string EAT_NORMAL_HRS { get; set; }

        [XmlElement("EAT_SHORT_HRS")]
        public string EAT_SHORT_HRS { get; set; }

        [XmlElement("EAT_OT_HRS")]
        public string EAT_OT_HRS { get; set; }
    }

    [Serializable]
    [XmlRoot("Root")]
    public class AttendanceImport
    {
        [XmlElement("IS_UPDATE")]
        public int IS_UPDATE { get; set; }
        [XmlElement("BIZUNIT_PK")]
        public int BIZUNIT_PK { get; set; }
        [XmlElement("USER_PK")]
        public int USER_PK { get; set; }
        [XmlElement("TEMPLATE_TYPE")]
        public int TEMPLATE_TYPE { get; set; }
        [XmlElement("EAR_REMARKS")]
        public string EAR_REMARKS { get; set; }
        [XmlElement("EAR_COMPANY")]
        public int EAR_COMPANY { get; set; }
        [XmlElement("EAR_ACTIVE")]
        public int EAR_ACTIVE { get; set; }
        [XmlElement("EAR_IN_FL")]
        public int EAR_IN_FL { get; set; }
        [XmlElement("EAR_FROM_DATE")]
        public DateTime EAR_FROM_DATE { get; set; }
        [XmlElement("EAR_ATT_MODE")]
        public int EAR_ATT_MODE { get; set; }

        [XmlElement("Detail")]
        public List<AttendanceImportDetail> ImportData { get; set; }
    }
    [Serializable]
    [XmlRoot("Detail")]
    public class AttendanceImportDetail
    {
        private DateTime dt;
        [XmlElement("EAT_DATE")]
        public string EAT_DATE
        {
            get
            {
                return this.dt.ToString("dd-MMM-yyyy");
            }
            set
            {
                this.dt = Convert.ToDateTime(value);
            }
        }

        [XmlElement("EAT_SL_NO")]
        public int EAT_SL_NO { get; set; }
        [XmlElement("EAT_EMPLOYEE")]
        public string EAT_EMPLOYEE { get; set; }
        [XmlElement("EAT_IN_TIME")]
        public string EAT_IN_TIME { get; set; }
        [XmlElement("EAT_OUT_TIME")]
        public string EAT_OUT_TIME { get; set; }
        [XmlElement("EAT_NORMAL_HRS")]
        public decimal EAT_NORMAL_HRS { get; set; }
        [XmlElement("EAT_TYPE")]
        public string EAT_TYPE { get; set; }
        [XmlElement("EAT_EMPBIOMETRICID")]
        public string EAT_EMPBIOMETRICID { get; set; }

    }

    #region General Attendance
    [Serializable]
    [XmlRoot("Root")]
    public class AttendanceGen
    {
        [XmlElement("EAR_PK")]
        public int EAR_PK { get; set; }
        [XmlElement("EAR_NO")]
        public string EAR_NO { get; set; }
        [XmlElement("EAR_FROM_DATE")]
        public DateTime EAR_FROM_DATE { get; set; }
        [XmlElement("EAR_TO_DATE")]
        public DateTime EAR_TO_DATE { get; set; }
        [XmlElement("EAR_BRANCH")]
        public string EAR_BRANCH { get; set; }
        [XmlElement("EAR_BRANCH_TEXT")]
        public string EAR_BRANCH_TEXT { get; set; }
        [XmlElement("EAR_REMARKS")]
        public string EAR_REMARKS { get; set; }
        [XmlElement("EAR_EMPDEPARTMENT")]
        public string EAR_EMPDEPARTMENT { get; set; }
        [XmlElement("EAR_EMPDEPARTMENT_TEXT")]
        public string EAR_EMPDEPARTMENT_TEXT { get; set; }
        [XmlElement("EAR_COMPANY")]
        public int EAR_COMPANY { get; set; }
        [XmlElement("EAR_STATUS")]
        public int EAR_STATUS { get; set; }
        [XmlElement("EAR_IN_FL")]
        public int EAR_IN_FL { get; set; }
        [XmlElement("IS_DEL_FL")]
        public int IS_DEL_FL { get; set; }
        [XmlElement("EAR_ATT_MODE")]
        public int EAR_ATT_MODE { get; set; }

        [XmlElement("EAR_DEPT")]
        public int EAR_DEPT { get; set; }
        [XmlElement("BIZUNIT_PK")]
        public int BIZUNIT_PK { get; set; }
        [XmlElement("ACTIVE")]
        public int ACTIVE { get; set; }
        [XmlElement("USER_PK")]
        public int USER_PK { get; set; }
        [XmlElement("LAST_MOD_DT")]
        public DateTime LAST_MOD_DT { get; set; }
        [XmlElement("WKF_FLAG")]
        public int WKF_FLAG { get; set; }

        [XmlElement("Details")]
        public List<AttendanceGenDetails> AttnDetails { get; set; }

    }

    [Serializable]
    [XmlRoot("Details")]
    public class AttendanceGenDetails
    {
        [XmlElement("EAT_PK")]
        public int EAT_PK { get; set; }
        [XmlElement("EAT_SLNO")]
        public int EAT_SLNO { get; set; }
        [XmlElement("EAT_EMPLOYEE")]
        public int EAT_EMPLOYEE { get; set; } 
        [XmlElement("EAT_EMPLOYEE_TEXT")]
        public string EAT_EMPLOYEE_TEXT { get; set; } 
        [XmlElement("EAT_EMPLOYEE_NAME")]
        public string EAT_EMPLOYEE_NAME { get; set; } 
        [XmlElement("empBranch")]
        public string empBranch { get; set; } 
        [XmlElement("empBranchText")]
        public string empBranchText { get; set; } 
        [XmlElement("empBranch_pk")]
        public string LocationPk { get; set; }
        [XmlElement("empBranchCode")]
        public string empBranchCode { get; set; } 
        [XmlElement("empDepartmentText")]
        public string empDepartmentText { get; set; }
        [XmlElement("DPT_CODE")]
        public string DPT_CODE { get; set; }
        [XmlElement("DPT_PK")]
        public string DPT_PK { get; set; }
        [XmlElement("empDesignationText")]
        public string empDesignationText { get; set; } 
        [XmlElement("EAT_DATE")]
        public string EAT_DATE { get; set; } 
        [XmlElement("EAT_IN_TIME")]
        public string EAT_IN_TIME { get; set; }     
        [XmlElement("EAT_OUT_TIME")]
        public string EAT_OUT_TIME { get; set; } 
        [XmlElement("EAT_NORMAL_HRS")]
        public double EAT_NORMAL_HRS { get; set; } 
        [XmlElement("EAT_WORKED_HRS")]
        public double EAT_WORKED_HRS { get; set; } 
        [XmlElement("EAT_SHORT_HRS")]
        public double EAT_SHORT_HRS { get; set; } 
        [XmlElement("EAT_OT_HRS")]
        public double EAT_OT_HRS { get; set; } 

        [XmlElement("EMP_FL")]
        public int EMP_FL { get; set; }       
        [XmlElement("EMP_IS_SAL_PRCD")]
        public int EMP_IS_SAL_PRCD { get; set; }
        [XmlElement("EAT_REMARKS")]
        public string EAT_REMARKS { get; set; } 
        [XmlElement("EAT_STATUS")]
        public int EAT_STATUS { get; set; }
        [XmlElement("EAT_IS_UPDATE")]
        public int EAT_IS_UPDATE { get; set; } 
        [XmlElement("IS_RECALCULATE")]
        public int IS_RECALCULATE { get; set; }       
        [XmlElement("EPD_HAS_OT_FROM_ATT")]
        public int EPD_HAS_OT_FROM_ATT { get; set; } 

        //[XmlElement("In")]
        //public string In { get; set; }
        //[XmlElement("Out")]
        //public string Out { get; set; }
        //[XmlElement("Total")]
        //public string Total { get; set; }
        //[XmlElement("Short")]
        //public string Short { get; set; }
        //[XmlElement("OT")]
        //public string OT { get; set; }
    }

    [Serializable]
    [XmlRoot("Root")]
    public class AttendanceGenImport
    {
        [XmlElement("IS_UPDATE")]
        public int IS_UPDATE { get; set; }
        [XmlElement("BIZUNIT_PK")]
        public int BIZUNIT_PK { get; set; }
        [XmlElement("USER_PK")]
        public int USER_PK { get; set; }
        [XmlElement("TEMPLATE_TYPE")]
        public int TEMPLATE_TYPE { get; set; }
        [XmlElement("EAR_REMARKS")]
        public string EAR_REMARKS { get; set; }
        [XmlElement("EAR_COMPANY")]
        public int EAR_COMPANY { get; set; }
        [XmlElement("EAR_ACTIVE")]
        public int EAR_ACTIVE { get; set; }
        [XmlElement("EAR_IN_FL")]
        public int EAR_IN_FL { get; set; }
        [XmlElement("EAR_FROM_DATE")]
        public DateTime EAR_FROM_DATE { get; set; }
        [XmlElement("EAR_ATT_MODE")]
        public int EAR_ATT_MODE { get; set; }
        [XmlElement("EAR_BRANCH")]
        public int EAR_BRANCH { get; set; }

        [XmlElement("Detail")]
        public List<AttendanceGenImportDetail> ImportData { get; set; }
    }
    [Serializable]
    [XmlRoot("Detail")]
    public class AttendanceGenImportDetail
    {
        private DateTime dt;
        [XmlElement("EAT_DATE")]
        public string EAT_DATE
        {
            get
            {
                return this.dt.ToString("dd-MMM-yyyy");
            }
            set
            {
                this.dt = Convert.ToDateTime(value);
            }
        }

        [XmlElement("EAT_SL_NO")]
        public int EAT_SL_NO { get; set; }
        [XmlElement("EAT_EMPLOYEE")]
        public string EAT_EMPLOYEE { get; set; }
        [XmlElement("EAT_IN_TIME")]
        public string EAT_IN_TIME { get; set; }
        [XmlElement("EAT_OUT_TIME")]
        public string EAT_OUT_TIME { get; set; }
        [XmlElement("EAT_NORMAL_HRS")]
        public decimal EAT_NORMAL_HRS { get; set; }
        [XmlElement("EAT_WORKED_HRS")]
        public decimal EAT_WORKED_HRS { get; set; }
        [XmlElement("EAT_TYPE")]
        public string EAT_TYPE { get; set; }
        [XmlElement("EAT_EMPBIOMETRICID")]
        public string EAT_EMPBIOMETRICID { get; set; }

    }
    #endregion
}
